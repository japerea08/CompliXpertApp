using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class AddNewContactScreenViewModel : AbstractEmailValidator
    {
        private bool canAdd = false;
        private string firstName;
        private string lastName;
        private bool emailValidated;
        private ICommand emailValidateMessageCommand;
        private Color emailValidationColor;
        private string emailValidationMessage;
        private string newContactEmail;
        private bool textEntered;

        public AddNewContactScreenViewModel()
        {
            NewContact = new NewContact();
            AddNewContactCommand = new Command(async () => await AddNewContactAsync(), () => canAdd);
        }

        //properties
        public NewContact NewContact { get; set; }
        public string NewContactEmail
        {
            get
            {
                return newContactEmail;
            }
            set
            {
                newContactEmail = value;
                if (String.IsNullOrEmpty(newContactEmail) == true || String.IsNullOrWhiteSpace(newContactEmail) == true)
                    TextEntered = false;
                else
                    TextEntered = true;
                OnPropertyChanged();
            }
        }
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value;
                if (String.IsNullOrEmpty(firstName) == false && String.IsNullOrWhiteSpace(firstName) == false)
                    CanAdd(true);
                else
                    CanAdd(false);
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
                if (String.IsNullOrEmpty(lastName) == false && String.IsNullOrWhiteSpace(lastName) == false)
                    CanAdd(true);
                else
                    CanAdd(false);
            }
        }

        public Command AddNewContactCommand { get; private set; }
        public ICommand EmailValidateMessageCommand => emailValidateMessageCommand ?? (emailValidateMessageCommand = new Command<bool>(CheckEmailFormat));


        //methods
        void CheckEmailFormat(bool input)
        {
            if (input == false)
                EmailValidated = false;
            else
                EmailValidated = true;
        }
        void CanAdd(bool value)
        {
            canAdd = value;
            ((Command) AddNewContactCommand).ChangeCanExecute();
        }
        private async Task AddNewContactAsync()
        {
            using (CompliXperAppContext context = new CompliXperAppContext())
            {
                if(NewContact != null || firstName != null)
                {
                    NewContact.FirstName = firstName;
                    NewContact.LastName = lastName;
                    NewContact.Email = newContactEmail;
                    NewContact.CreatedDate = DateTime.Now;
                    context.Add(NewContact);
                    try
                    {
                        await context.SaveChangesAsync();
                        App.Current.MainPage = new NavigationPage(new CompliXpertAppMasterDetailPage());
                        await App.Current.MainPage.Navigation.PopToRootAsync();
                    }
                    catch (DbUpdateException e)
                    {
                        SqliteException ex = (SqliteException) e.InnerException;
                        Console.WriteLine(ex.SqliteErrorCode);
                        await App.Current.MainPage.Navigation.PopToRootAsync();
                        DependencyService.Get<IToast>().WriteToast(e.InnerException.Message);
                    }
                }
            }
        }

    }
}
