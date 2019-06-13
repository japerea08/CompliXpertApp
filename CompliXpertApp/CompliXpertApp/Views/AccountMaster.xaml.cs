using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountMaster : ContentPage
	{
        AccountMasterViewModel accountMasterViewModel;
		public AccountMaster ()
		{
            accountMasterViewModel = new AccountMasterViewModel();
			InitializeComponent ();
            BindingContext = accountMasterViewModel;
		}
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<CustomerListScreenViewModel, Account>(accountMasterViewModel, Message.CustomerLoaded);
        }
    }
}