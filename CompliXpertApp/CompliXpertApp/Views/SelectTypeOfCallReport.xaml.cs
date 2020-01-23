using CompliXpertApp.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectTypeOfCallReport : ContentPage
	{
        private SelectTypeOfCallReportViewModel selectTypeOfCallReportViewModel;
        public SelectTypeOfCallReport ()
		{
            NavigationPage.SetTitleIconImageSource(this, "compli_logo_xsmall.png");
            selectTypeOfCallReportViewModel = new SelectTypeOfCallReportViewModel();
            BindingContext = selectTypeOfCallReportViewModel;
			InitializeComponent ();
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
            selectTypeOfCallReportViewModel.InitializeData();
        }
    }
}