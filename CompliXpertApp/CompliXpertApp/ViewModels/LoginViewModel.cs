using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {
        private string username = string.Empty;
        private string password = string.Empty;
        private readonly string userNamePlaceholder = "Enter Username";
        private readonly string passwordPlaceholder = "Enter Password";
        private bool isBusy = false;
        private Color usernamePlaceholderColor = Color.Default;
        private Color passwordPlaceholderColor = Color.Default;

        public LoginViewModel()
        {
            User = new User();
            CheckLoginCredentialsCommand = new Command(CheckLoginCredentials);
        }

        //properties
        public ICommand OnValidationCommand { get; set; }
        public User User { get; set; }
        public bool ErrorMessageVisibility { get; set; }
        public Color UsernamePlaceholderColor
        {
            get { return usernamePlaceholderColor; }
            set
            {
                usernamePlaceholderColor = value;
                OnPropertyChanged();
            }
        }
        public Color PasswordPlaceholderColor
        {
            get { return passwordPlaceholderColor; }
            set
            {
                passwordPlaceholderColor = value;
                OnPropertyChanged();
            }
        }
        public string UsernamePlaceholder
        {
            get { return userNamePlaceholder; }
        }
        public string PasswordPlaceholder
        {
            get { return passwordPlaceholder; }
        }
        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; }
        }
        public string Username
        {
            get { return User.UserName; }
            set { User.UserName = value; }
        }
        public string Password
        {
            get { return User.Password; }
            set { User.Password = value; }
        }

        //methods
        //databind to this command property
        public Command CheckLoginCredentialsCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        void CheckLoginCredentials()
        {
            if(String.IsNullOrEmpty(Username) == false && String.IsNullOrEmpty(Password) == false)
            {
                //await Navigation.PushAsync(new AccountListScreen());
                DependencyService.Get<IToast>().WriteToast("Username and password entered");
            }
            else
            {
                //change entry color to red
                if (String.IsNullOrEmpty(username) == true)
                    UsernamePlaceholderColor = Color.Red;
                if (String.IsNullOrEmpty(password) == true)
                    PasswordPlaceholderColor = Color.Red;
            }
        }
    }
}
