using CompliXpertApp.Helpers;
using CompliXpertApp.ViewModels;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using System;

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
    }
}