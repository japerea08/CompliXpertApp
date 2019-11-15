using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using System;
using System.Windows.Input;
using Xamarin.Forms;

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
            MessagingCenter.Subscribe<CustomerListScreenViewModel, NewContact>(this, Message.NewContactLoaded, (sender, args) =>
                {
                    NewContact = args;
                    NewContactEmail = newContact.Email;
                    TextEntered = false;
                });
        }

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
        //public bool EmailValidated
        //{
        //    get
        //    {
        //        return emailValidated;
        //    }
        //    set
        //    {
        //        emailValidated = value;
        //        //if email is not validated
        //        if (value == false)
        //        {
        //            EmailValidationMessage = "Email format is not correct";
        //            EmailValidationColor = Color.Red;
        //        }
        //        else
        //        {
        //            EmailValidationMessage = "Email format looks correct";
        //            EmailValidationColor = Color.Green;
        //        }
        //        OnPropertyChanged();
        //    }
        //}
        //public Color EmailValidationColor
        //{
        //    get
        //    {
        //        return emailValidationColor;
        //    }
        //    set
        //    {
        //        emailValidationColor = value;
        //        OnPropertyChanged();
        //    }
        //}
        //public string EmailValidationMessage
        //{
        //    get
        //    {
        //        //if nothing has been entered into the email entry
        //        return emailValidationMessage;
        //    }
        //    set
        //    {
        //        emailValidationMessage = value;
        //        OnPropertyChanged();
        //    }
        //}
        //public bool TextEntered
        //{
        //    get
        //    {
        //        return textEntered;
        //    }
        //    set
        //    {
        //        textEntered = value;
        //        OnPropertyChanged();
        //    }
        //}

        public ICommand EmailValidateMessageCommand => emailValidateMessageCommand ?? (emailValidateMessageCommand = new Command<bool>(CheckEmailFormat));


        //methods
        void CheckEmailFormat(bool input)
        {
            if (input == false)
                EmailValidated = false;
            else
                EmailValidated = true;
        }
    }
}
