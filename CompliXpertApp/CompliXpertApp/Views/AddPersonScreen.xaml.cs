using CompliXpertApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddPersonScreen : ContentPage
	{
        private AddPersonScreenViewModel addPersonScreenViewModel;
        public AddPersonScreen(int callReportId, bool callReportCreated)
        {
            addPersonScreenViewModel = new AddPersonScreenViewModel();
            BindingContext = addPersonScreenViewModel;
            InitializeComponent();
            addPersonScreenViewModel.callreportId = callReportId;
            addPersonScreenViewModel.callReportCreatedAlready = callReportCreated;
        }
	}
}