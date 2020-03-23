using CompliXpertApp.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using CompliXpertApp.Helpers;

namespace CompliXpertApp.ViewModels
{
    class AddPersonScreenViewModel
    {
        public int callreportId;
        public bool callReportCreatedAlready;
        private string firstName;
        private string lastName;
        private bool canSave;

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
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        //methods
        void CanSave(bool value)
        {
            canSave = value;
            ((Command) SavePersonCommand).ChangeCanExecute();
        }
        async Task SavePersonAsync()
        {
            Person person = new Person()
            {
                CallReportId = callreportId,
                CreatedDate = DateTime.Now,
                CreatedonMobile = true,
                Email = Email,
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
