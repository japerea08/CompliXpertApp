using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomerMaster : ContentPage
	{
        CustomerMasterViewModel customerMasterViewModel;
		public CustomerMaster ()
		{
            customerMasterViewModel = new CustomerMasterViewModel();
			InitializeComponent ();
            BindingContext = customerMasterViewModel;
		}
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<CustomerListScreenViewModel, Customer>(customerMasterViewModel, Message.CustomerLoaded);
        }
    }
}