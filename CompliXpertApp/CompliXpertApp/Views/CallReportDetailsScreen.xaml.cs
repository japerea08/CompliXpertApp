using CompliXpertApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CallReportDetailsScreen : ContentPage
	{
		public CallReportDetailsScreen ()
		{
			InitializeComponent ();
            BindingContext = new CallReportDetailsViewModel();
		}
	}
}