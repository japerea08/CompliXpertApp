using CompliXpertApp.Models;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class LoginViewModel:INotifyPropertyChanged
    {
        
        public LoginViewModel()
        {
            User = new User();

            OnValidationCommand = new Command((obj) => 
            {
                User.UserName.NotValidMessageError = "Username is required";
                User.UserName.IsNotValid = string.IsNullOrEmpty(User.UserName.Name);
                User.Password.NotValidMessageError = "Password is required";
                User.Password.IsNotValid = string.IsNullOrEmpty(User.Password.Name);
            });
        }

        string username = string.Empty;
        string password = string.Empty;
        bool isUsernameEntered = false;
        bool isPasswordEntered = false;
        bool isBusy = false;

        public event PropertyChangedEventHandler PropertyChanged;

        //properties
        public ICommand OnValidationCommand { get; set; }
        public User User { get; set; }
        public bool ErrorMessageVisibility { get; set; }
        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; }
        }
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public bool IsUsernameEntered
        {
            get { return isUsernameEntered; }
            set { isUsernameEntered = value; }
        }

        public bool IsPasswordEntered
        {
            get { return isPasswordEntered; }
            set { isPasswordEntered = value; }
        }

        //methods
        public Command CheckLoginCredentialsCommand { get; }

        void CheckLoginCredentials()
        {
            if(String.IsNullOrEmpty(username) == false && String.IsNullOrEmpty(password) == false)
            {
                //run the login logic
            }
            else
            {
                //change entry color to red

            }
        }
    }
}
