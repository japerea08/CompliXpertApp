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
    class AddNewContactScreenViewModel : AbstractNotifyPropertyChanged
    {
        private bool canAdd = false;
        private string contactName;
        private bool emailValidated;
        private ICommand emailValidateMessageCommand;
        private Color emailValidationColor;
        private string emailValidationMessage;

        public AddNewContactScreenViewModel()
        {
            NewContact = new NewContact();
            AddNewContactCommand = new Command(async () => await AddNewContactAsync(), () => canAdd);
        }

        //properties
        public NewContact NewContact { get; set; }
        public string ContactName
        {
            get
            {
                return contactName;
            }
            set
            {
                contactName = value;
                if (String.IsNullOrEmpty(ContactName) == false && String.IsNullOrWhiteSpace(ContactName) == false)
                    CanAdd(true);
                else
                    CanAdd(false);
            }
        }
        public bool EmailValidated
        {
            get
            {
                return emailValidated;
            }
            set
            {
                emailValidated = value;
                //if email is not validated
                if (value == false)
                {
                    EmailValidationMessage = "Email format is not correct";
                    EmailValidationColor = Color.Red;
                }
                else
                {
                    EmailValidationMessage = "Email format looks correct";
                    EmailValidationColor = Color.Green;
                }
                OnPropertyChanged();
            }
        }
        public Color EmailValidationColor
        {
            get
            {
                return emailValidationColor;
            }
            set
            {
                emailValidationColor = value;
                OnPropertyChanged();
            }
        }
        public string EmailValidationMessage
        {
            get
            {
                //if nothing has been entered into the email entry
                return emailValidationMessage;
            }
            set
            {
                emailValidationMessage = value;
                OnPropertyChanged();
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
                if(NewContact != null || ContactName != null)
                {
                    NewContact.Name = contactName;
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
