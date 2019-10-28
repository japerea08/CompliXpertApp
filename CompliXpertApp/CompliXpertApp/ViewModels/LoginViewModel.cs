using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class LoginViewModel : AbstractNotifyPropertyChanged
    {
        private bool isBusy = false;
        private bool canLogin = true;
        private Color usernamePlaceholderColor = Color.Default;
        private Color passwordPlaceholderColor = Color.Default;

        public LoginViewModel()
        {
            //object for sigining in
            User = new User();
            CheckLoginCredentialsCommand = new Command(async () => await CheckLoginCredentialsAsync(), () => canLogin);
        }

        //properties
        public User User { get; set; }
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
        public string UsernamePlaceholder { get; } = "Enter Username";
        public string PasswordPlaceholder { get; } = "Enter Password";
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        public string Username
        {
            get
            {
                return User.UserName;
                
            }
            set
            {
                User.UserName = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get
            {
                return User.Password;
            }
            set
            {
                User.Password = value;
                OnPropertyChanged();
            }
        }

        //methods
        //databind to this command property
        public ICommand CheckLoginCredentialsCommand { get; private set; }

        void CanAttemptLogin(bool value)
        {
            canLogin = value;
            ((Command) CheckLoginCredentialsCommand).ChangeCanExecute();
        }
        async Task CheckLoginCredentialsAsync()
        {
            
            if (String.IsNullOrEmpty(Username) == false && String.IsNullOrEmpty(Password) == false)
            {
                CanAttemptLogin(false);
                //IsBusy = true;
                await App.Current.MainPage.Navigation.PushAsync(new LoadingScreen());
                //IsBusy = false;
                Username = null;
                Password = null;
                CanAttemptLogin(true);             
            }
            else
            {
                //change entry color to red
                if (String.IsNullOrEmpty(User.UserName) == true)
                    UsernamePlaceholderColor = Color.Red;
                if (String.IsNullOrEmpty(User.Password) == true)
                    PasswordPlaceholderColor = Color.Red;
            }
        }
    }
}
