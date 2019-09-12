using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomerListScreen : ContentPage
	{
        CustomerListScreenViewModel listScreenViewModel;
		public CustomerListScreen ()
		{
            listScreenViewModel = new CustomerListScreenViewModel();
            NavigationPage.SetTitleIconImageSource(this, "compli_logo_xsmall.png");
            InitializeComponent ();
            BindingContext = listScreenViewModel;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<LoginViewModel, List<Account>>(listScreenViewModel, Message.AccountListLoaded);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            listScreenViewModel.CheckForNewData();
        }
    }
}