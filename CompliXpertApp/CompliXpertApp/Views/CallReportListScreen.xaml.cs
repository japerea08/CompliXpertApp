using CompliXpertApp.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CallReportListScreen : ContentPage
	{
        private CallReportListScreenViewModel callReportListScreenViewModel = new CallReportListScreenViewModel();
        public CallReportListScreen ()
		{
            BindingContext = callReportListScreenViewModel;
            NavigationPage.SetTitleIconImageSource(this, "compli_logo_xsmall.png");
            InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            callReportListScreenViewModel.InitializeData();
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await App.Current.MainPage.DisplayAlert("Are you sure you want to return to Customer List?", "", "Yes", "No"))
                {
                    await App.Current.MainPage.Navigation.PopToRootAsync();
                }
            });
            return true;
        }
    }
}