using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged, IRWExternalStorage
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
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
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

        async void CheckLoginCredentials()
        {
            if(String.IsNullOrEmpty(Username) == false && String.IsNullOrEmpty(Password) == false)
            { 
                //check for the file that has our data async
                string json = await ReadFileAsync();
                IsBusy = true;
                await Task.Delay(4000);
                IsBusy = false;
                //launch the next activity
                await App.Current.MainPage.Navigation.PushAsync(new AccountListScreen());
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

        public Task<string> WriteFileAsync(string filePath, string jsonString)
        {
            throw new NotImplementedException();
        }

        public async Task<string> ReadFileAsync()
        {
            var json = await DependencyService.Get<IRWExternalStorage>().ReadFileAsync();
            return json;
        }
    }
}
