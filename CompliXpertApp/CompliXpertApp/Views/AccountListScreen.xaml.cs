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
		public AccountListScreen (List<Account> accounts)
		{
			InitializeComponent ();
            //binding context for data binding
            BindingContext = new AccountListScreenViewModel(accounts);
        }
	}
}