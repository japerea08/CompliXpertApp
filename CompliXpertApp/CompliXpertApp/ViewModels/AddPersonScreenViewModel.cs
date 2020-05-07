using CompliXpertApp.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using CompliXpertApp.Helpers;

namespace CompliXpertApp.ViewModels
{
    class AddPersonScreenViewModel : AbstractEmailValidator
    {
        public int callreportId;
        public bool callReportCreatedAlready;
        private string firstName;
        private string lastName;
        private bool canSave;
        private bool textEntered;
        private ICommand emailValidateMessageCommand;
        private string personEmail;

        public AddPersonScreenViewModel()
        {
            SavePersonCommand = new Command(async () => await SavePersonAsync(), () => canSave);
        }
        //properties
        public ICommand SavePersonCommand { get; set; }
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value;
                if (String.IsNullOrEmpty(firstName) == false || String.IsNullOrWhiteSpace(firstName) == false)
                    CanSave(true);
                else
                    CanSave(false);
            }
        }
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
                if (String.IsNullOrEmpty(lastName) == false || String.IsNullOrWhiteSpace(lastName) == false)
                    CanSave(true);
                else
                    CanSave(false);
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
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public ICommand EmailValidateMessageCommand => emailValidateMessageCommand ?? (emailValidateMessageCommand = new Command<bool>(CheckEmailFormat));
        //methods
        void CanSave(bool value)
        {
            canSave = value;
            ((Command) SavePersonCommand).ChangeCanExecute();
        }
        void CheckEmailFormat(bool input)
        {
            if (input == false)
                EmailValidated = false;
            else
                EmailValidated = true;
        }
        async Task SavePersonAsync()
        {
            Person person = new Person()
            {
                CallReportId = callreportId,
                CreatedDate = DateTime.Now,
                CreatedonMobile = true,
                Email = personEmail,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = PhoneNumber,
                Position = Position
            };

            if (callReportCreatedAlready == true)
            {
               
                using (CompliXperAppContext context = new CompliXperAppContext())
                {
                   
                    context.Persons.Add(person);

                    try
                    {
                        await context.SaveChangesAsync();
                        await App.Current.MainPage.Navigation.PopModalAsync();
                        MessagingCenter.Send<AddPersonScreenViewModel, Person>(this, Message.PersonCreated, person);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else
            {
                await App.Current.MainPage.Navigation.PopModalAsync();
                ////use messaging center to send person back
                MessagingCenter.Send<AddPersonScreenViewModel, Person>(this, Message.PersonCreated, person);
            }


        }
    }
}
