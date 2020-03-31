using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CompliXpertApp.ViewModels;

namespace CompliXpertApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContactListScreen : ContentPage
	{
        private ContactListScreenViewModel contactListScreenViewModel;

		public ContactListScreen ()
		{
            contactListScreenViewModel = new ContactListScreenViewModel();
            BindingContext = contactListScreenViewModel;
            NavigationPage.SetTitleIconImageSource(this, "compli_logo_xsmall.png");
            InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            contactListScreenViewModel.InitializeData();
            base.OnAppearing();

        }
    }
}