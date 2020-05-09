using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CompliXpertApp.ViewModels
{
    class NewContactMasterViewModel : AbstractEmailValidator
    {
        private bool emailValidated;
        private ICommand emailValidateMessageCommand;
        private Color emailValidationColor;
        private string emailValidationMessage;
        private bool textEntered;
        private string newContactEmail;
        private NewContact newContact;

        public NewContactMasterViewModel()
        {
            

            SaveNewContactCommand = new Command(async () => await SaveNewContactCommandAsync());
        }

 

        public ICommand SaveNewContactCommand { get; set; }

        //properties
        public NewContact NewContact
        {
            get
            {
                return newContact;
            }
            set
            {
                newContact = value;
                OnPropertyChanged();
            }
        }

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

        public ICommand EmailValidateMessageCommand => emailValidateMessageCommand ?? (emailValidateMessageCommand = new Command<bool>(CheckEmailFormat));


        //methods
        public void Subscribe()
        {
            MessagingCenter.Subscribe<CustomerListScreenViewModel, NewContact>(this, Message.NewContactLoaded, (sender, args) =>
            {
                NewContact = args;
                NewContactEmail = newContact.Email;
                TextEntered = false;
            });
            MessagingCenter.Subscribe<ContactListScreenViewModel, NewContact>(this, Message.NewContactLoaded, (sender, contact) =>
            {
                NewContact = contact;
                NewContactEmail = newContact.Email;
                TextEntered = false;
            });
        }
        public void Unsubscribe()
        {
            MessagingCenter.Unsubscribe<CustomerListScreenViewModel, NewContact>(this, Message.NewContactLoaded);
            MessagingCenter.Unsubscribe<ContactListScreenViewModel, NewContact>(this, Message.NewContactLoaded);
        }
        void CheckEmailFormat(bool input)
        {
            if (input == false)
                EmailValidated = false;
            else
                EmailValidated = true;
        }

        private async Task SaveNewContactCommandAsync()
        {
            using (CompliXperAppContext context = new CompliXperAppContext())
            {
                NewContact contact = await (from c in context.NewContacts
                                           where c.ContactId == newContact.ContactId
                                           select c).FirstOrDefaultAsync();
                if(contact != null)
                {
                    contact.Comments = newContact.Comments;
                    contact.Company = newContact.Company;
                    contact.CreatedDate = DateTime.Now;
                    contact.Email = newContactEmail;
                    contact.FirstName = newContact.FirstName;
                    contact.LastName = newContact.LastName;
                    contact.Phonenumber = newContact.Phonenumber;
                    contact.Title = newContact.Title;
                    context.NewContacts.Update(contact);

                    try
                    {
                        await context.SaveChangesAsync();
                        
                    }
                    catch (DbUpdateException e)
                    {
                        Console.WriteLine(e.InnerException);
                    }
                    await App.Current.MainPage.Navigation.PopToRootAsync();
                }
            }
        }
    }
}
