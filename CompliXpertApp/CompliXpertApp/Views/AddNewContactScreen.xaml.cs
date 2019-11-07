using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CompliXpertApp.ViewModels;

namespace CompliXpertApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddNewContactScreen : ContentPage
	{
        private AddNewContactScreenViewModel addNewContactScreenViewModel;
		public AddNewContactScreen ()
		{
            addNewContactScreenViewModel = new AddNewContactScreenViewModel();
            NavigationPage.SetTitleIconImageSource(this, "compli_logo_xsmall.png");
            InitializeComponent ();
            BindingContext = addNewContactScreenViewModel;
		}
	}
}