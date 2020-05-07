using CompliXpertApp.Models;
using Xamarin.Forms;
using CompliXpertApp.Helpers;
using System.Windows.Input;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompliXpertApp.ViewModels
{
    class PersonDetailsScreenViewModel : AbstractEmailValidator
    {
        private Person person;
        private bool canSavePerson;
        private bool textEntered;
        private string personEmail;
        private ICommand emailValidateMessageCommand;

        public PersonDetailsScreenViewModel()
        {
            //true for now need to build in some capacity of checking info entered by the user
            canSavePerson = true;

            MessagingCenter.Subscribe<PersonsListScreenViewModel, Person>(this, Message.PersonLoaded, (sender, person) =>
            {
                Person = person;
                PersonEmail = person.Email;
                TextEntered = false;
            });

            SavePersonCommand = new Command(async () => await SavePersonAsync(), () => canSavePerson);
            DeletePersonCommand = new Command(async () => await DeletePersonAsync());
        }

        public ICommand SavePersonCommand { get; set; }
        public ICommand EmailValidateMessageCommand => emailValidateMessageCommand ?? (emailValidateMessageCommand = new Command<bool>(CheckEmailFormat));
        public ICommand DeletePersonCommand { get; set; }
        public Person Person
        {
            get
            {
                return person;
            }
            set
            {
                person = value;
                OnPropertyChanged();
            }
        }
        public string PersonEmail
        {
            get
            {
                return personEmail;
            }
            set
            {
                personEmail = value;
                if (String.IsNullOrEmpty(personEmail) == true || String.IsNullOrWhiteSpace(personEmail) == true)
                    TextEntered = false;
                else
                    TextEntered = true;
                OnPropertyChanged();
            }
        }

        //methods
        private void CanSavePerson(bool value)
        {
            canSavePerson = value;
            if(canSavePerson == true)
                ((Command) SavePersonCommand).ChangeCanExecute();
        }
        void CheckEmailFormat(bool input)
        {
            if (input == false)
                EmailValidated = false;
            else
                EmailValidated = true;
        }
        public async Task SavePersonAsync()
        {
            using (var context = new CompliXperAppContext())
            {
                //retrieving entity by person id
                Person dbPerson = await context.Persons.SingleOrDefaultAsync(p => p.PersonId == person.PersonId);

                if (dbPerson != null)
                {
                    dbPerson.FirstName = person.FirstName;
                    dbPerson.LastName = person.LastName;
                    dbPerson.Email = personEmail;
                    dbPerson.PhoneNumber = person.PhoneNumber;
                    dbPerson.Position = person.Position;
                    dbPerson.CreatedDate = DateTime.Now;
                    context.Persons.Update(dbPerson);
                }

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    Console.WriteLine(e.InnerException);
                }
                await App.Current.MainPage.Navigation.PopModalAsync();
                //send person back to the list screen
                List<Person> personsList = await (from p in context.Persons
                                                  where p.CallReportId == person.CallReportId
                                                  select p).ToListAsync();
                MessagingCenter.Send<PersonDetailsScreenViewModel, List<Person>>(this, Message.PersonsLoaded, personsList);
            }
        }

        public async Task DeletePersonAsync()
        {
            if (await App.Current.MainPage.DisplayAlert("Are you sure you want to delete this Person?", "This action cannot be undone.", "Yes", "Cancel"))
            {
                using (var context = new CompliXperAppContext())
                {
                    var entity = await context.Persons.FirstOrDefaultAsync(x => x.PersonId == person.PersonId);
                    context.Persons.Remove(entity);
                    await context.SaveChangesAsync();

                    //get current list
                    List<Person> p = (from persons in context.Persons
                               where persons.CallReportId == person.CallReportId
                               select persons).ToList();
                    await App.Current.MainPage.Navigation.PopModalAsync();
                    MessagingCenter.Send<PersonDetailsScreenViewModel, List<Person>>(this, Message.PersonsLoaded, p);
                }
            }
        }
    }
}
