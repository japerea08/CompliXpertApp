using CompliXpertApp.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoadingScreen : ContentPage
	{
        private LoadingScreenViewModel loadingScreenViewModel = new LoadingScreenViewModel();
		public LoadingScreen ()
		{
			InitializeComponent ();
            //hides the navigation bar
            NavigationPage.SetHasNavigationBar(this, false);

            //binding context for data binding
            BindingContext = loadingScreenViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await loadingScreenViewModel.DBContainsRecords();
        }
    }
}