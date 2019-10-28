using CompliXpertApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectAccountforCallReport : ContentPage
    {
        private SelectAccountforCallReportViewModel selectAccountforCallReportViewModel;
        public SelectAccountforCallReport()
        {
            NavigationPage.SetTitleIconImageSource(this, "compli_logo_xsmall.png");
            selectAccountforCallReportViewModel = new SelectAccountforCallReportViewModel();
            BindingContext = selectAccountforCallReportViewModel;
            InitializeComponent();
        }
    }
}