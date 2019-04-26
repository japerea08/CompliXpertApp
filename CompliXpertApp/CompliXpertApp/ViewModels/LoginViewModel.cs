using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged, IRWExternalStorage
    {
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
                List<Account> accounts = new List<Account>();
                //check for the file that has our data async
                IsBusy = true;
                string json = await ReadFileAsync();
                if(json != null)
                {
                    accounts = JsonConvert.DeserializeObject<List<Account>>(json);
                }
                //database access here...
                IsBusy = false;
                //launch the next activity
                await App.Current.MainPage.Navigation.PushAsync(new AccountListScreen(accounts));
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
