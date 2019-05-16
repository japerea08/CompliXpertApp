using CompliXpertApp.Models;
using CompliXpertApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountMaster : ContentPage
	{
		public AccountMaster ()
		{
			InitializeComponent ();
            BindingContext = new AccountMasterViewModel();
		}
	}
}