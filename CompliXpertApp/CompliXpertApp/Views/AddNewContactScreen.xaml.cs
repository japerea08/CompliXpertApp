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

        //method only works for Android Hard Key
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await App.Current.MainPage.DisplayAlert("Are you sure you want to Sign Off?", "All unsaved information will be lost.", "Yes", "Cancel"))
                {
                    await App.Current.MainPage.Navigation.PopToRootAsync();
                }
            });
            return true;

        }
    }
}