using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateCallReportScreen : ContentPage
	{
        private CreateCallReportViewModel createCallReportViewModel;
		public CreateCallReportScreen ()
		{
            createCallReportViewModel = new CreateCallReportViewModel();
			InitializeComponent();
            BindingContext = createCallReportViewModel;
		}
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<AccountMasterViewModel>(createCallReportViewModel, Message.CustomerLoaded);
        }

    }
}