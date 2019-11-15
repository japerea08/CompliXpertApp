using CompliXpertApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewContactMaster : ContentPage
	{
        private NewContactMasterViewModel newContactMasterViewModel; 
		public NewContactMaster ()
		{
            newContactMasterViewModel = new NewContactMasterViewModel();
            NavigationPage.SetTitleIconImageSource(this, "compli_logo_xsmall.png");
            InitializeComponent ();
            BindingContext = newContactMasterViewModel;
		}
    }
}