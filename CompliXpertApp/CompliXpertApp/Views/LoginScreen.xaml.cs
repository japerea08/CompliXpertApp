using CompliXpertApp.Helpers;
using CompliXpertApp.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginScreen : ContentPage
    {
        public LoginScreen()
        {
            InitializeComponent();
            //hides the navigation bar
            NavigationPage.SetHasNavigationBar(this, false);

            //binding context for data binding
            BindingContext = new LoginViewModel();
        }
        //handler for the create account
        void CreateAccountClicked(object sender, EventArgs args)
        {
            DependencyService.Get<IToast>().WriteToast("Create account tapped");
            //call create account screen
        }

        //handler for forgot password
        void ForgotPasswordClicked(object sender, EventArgs args)
        {
            DependencyService.Get<IToast>().WriteToast("Forgot password tapped");
            //call forgot password account
        }
        //handler for the login button
        async void LoginClicked(object sender, EventArgs agrs)
        {
            //call a new screen if login credentials are correct
            if (username.Text != null)
            {
                //this is where the password check would occur
                //call new screen
                await Navigation.PushAsync(new AccountListScreen());
                DependencyService.Get<IToast>().WriteToast("Username and password entered");
            }
            else
            {
                //toast to the user an incorrect password or username was entered (interface necessary)
                DependencyService.Get<IToast>().WriteToast("Incorrect Username or Password");
            }
        }
    }
}