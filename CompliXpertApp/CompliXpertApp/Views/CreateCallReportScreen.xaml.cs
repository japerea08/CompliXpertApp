using CompliXpertApp.Models;
using CompliXpertApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateCallReportScreen : ContentPage
	{
		public CreateCallReportScreen (Account account)
		{
			InitializeComponent ();
            BindingContext = new CreateCallReportViewModel(account);
		}
	}
}