using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountMasterScreen : ContentPage
	{
        private AccountMasterViewModel accountMasterViewModel;
		public AccountMasterScreen ()
		{
            accountMasterViewModel = new AccountMasterViewModel();
            NavigationPage.SetTitleIconImageSource(this, "compli_logo_xsmall.png");
            InitializeComponent ();
            BindingContext = accountMasterViewModel;
		}
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<CustomerMasterViewModel, Account>(accountMasterViewModel, Message.AccountLoaded);
        }
    }
}