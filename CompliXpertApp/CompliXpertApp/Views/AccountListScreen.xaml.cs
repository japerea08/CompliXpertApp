using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountListScreen : ContentPage
	{
        AccountListScreenViewModel listScreenViewModel;
		public AccountListScreen ()
		{
            listScreenViewModel = new AccountListScreenViewModel();
            InitializeComponent ();
            BindingContext = listScreenViewModel;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<LoginViewModel, List<Account>>(listScreenViewModel, Message.AccountListLoaded);
        }
    }
}