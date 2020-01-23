using CompliXpertApp.Helpers;
using CompliXpertApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CallReportsList : ContentPage
	{
        private CallReportListViewModel callReportListViewModel;
		public CallReportsList ()
		{
            callReportListViewModel = new CallReportListViewModel();
            NavigationPage.SetTitleIconImageSource(this, "compli_logo_xsmall.png");
            InitializeComponent ();
            BindingContext = callReportListViewModel;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            BindingContext = null;
            MessagingCenter.Unsubscribe<CustomerMasterViewModel, int>(callReportListViewModel, Message.AccountNumber);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext == null)
            {
                callReportListViewModel = new CallReportListViewModel();
                BindingContext = new CallReportListViewModel();
            }            
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await App.Current.MainPage.DisplayAlert("Sign Off", "Are you sure you want to sign off?", "Yes", "No"))
                {
                    await App.Current.MainPage.Navigation.PopToRootAsync();
                }

            });
            return true;

        }
    }
}