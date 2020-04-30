using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class PersonsListScreenViewModel : AbstractNotifyPropertyChanged
    {
        private List<Person> persons;
        private bool createdOnMobile;
        private Person selectedPerson;
        private bool personsCreated;
        private int crId;
        private bool callReportCreatedAlready = false;

        public PersonsListScreenViewModel()
        {
            AddPersonCommand = new Command(async () => await AddPersonAsync());

            MessagingCenter.Subscribe<CallReportDetailsViewModel, int>(this, Message.CallReportId, (sender, callReportId) =>
            {
                crId = callReportId;
                callReportCreatedAlready = true;
            });

            //call any DB actions here
            MessagingCenter.Subscribe<CallReportDetailsViewModel, int>(this, Message.CallReportId, async (sender, callReportId) =>
            {
                using (CompliXperAppContext context = new CompliXperAppContext())
                {
                    Persons = await (from persons in context.Persons
                             where persons.CallReportId == callReportId
                             select persons).ToListAsync();
                    CreatedOnMobile = await (from c in context.CallReport
                                            where c.CallReportId == callReportId
                                            select c.CreatedOnMobile).FirstOrDefaultAsync();
                    if (Persons.Count == 0)
                        PersonsCreated = false;
                    else
                        PersonsCreated = true;
                }

            });

            MessagingCenter.Subscribe<PersonDetailsScreenViewModel, List<Person>>(this, Message.PersonsLoaded, (sender, personsList) =>
            {
                if (personsList.Count == 0)
                    PersonsCreated = false;
                else
                {
                    PersonsCreated = true;
                    CreatedOnMobile = personsList[0].CreatedonMobile;
                }
                Persons = personsList;

            });

            MessagingCenter.Subscribe<AddPersonScreenViewModel, Person>(this, Message.PersonCreated, (sender, person) =>
            {
                List<Person> dummyList = new List<Person>();

                foreach (Person p in Persons)
                {
                    dummyList.Add(p);
                }

                dummyList.Add(person);
                Persons = dummyList;
                PersonsCreated = true;
            });
        }
        //properties
        public ICommand AddPersonCommand { get; set; }
        public List<Person> Persons
        {
            get
            {
                return persons;
            }
            set
            {
                persons = value;
                OnPropertyChanged();
            }
        }
        public bool PersonsCreated
        {
            get
            {
                return personsCreated;
            }
            set
            {
                personsCreated = value;
                OnPropertyChanged();
            }
        }
        public bool CreatedOnMobile
        {
            get
            {
                return createdOnMobile;
            }
            set
            {
                createdOnMobile = value;
                OnPropertyChanged();
            }
        }
        public Person SelectedPerson
        {
            get
            {
                return selectedPerson;
            }
            set
            {
                selectedPerson = value;
                if (selectedPerson == null)
                    return;
                GetPersonDetailsScreen(selectedPerson);
            }
        }

        //methods
        async private void GetPersonDetailsScreen(Person person)
        {
            SelectedPerson = null;
            await App.Current.MainPage.Navigation.PushModalAsync(new PersonDetailsScreen());
            MessagingCenter.Send<PersonsListScreenViewModel, Person>(this, Message.PersonLoaded, person);
        }
        async Task AddPersonAsync()
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new AddPersonScreen(crId, callReportCreatedAlready));
        }
    }
}
