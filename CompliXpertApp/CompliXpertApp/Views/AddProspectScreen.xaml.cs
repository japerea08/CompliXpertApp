using CompliXpertApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddProspectScreen : ContentPage
	{
        private AddProspectScreenViewModel addProspectScreenViewModel;
		public AddProspectScreen ()
		{
            addProspectScreenViewModel = new AddProspectScreenViewModel();
            NavigationPage.SetTitleIconImageSource(this, "compli_logo_xsmall.png");
            InitializeComponent ();
            BindingContext = addProspectScreenViewModel;
		}
        //method only works for Android Hard Key
        protected override bool OnBackButtonPressed()
        {
            //check to see if data has been entered
            if (addProspectScreenViewModel.CustomerNameEntered == false)
            {
                return base.OnBackButtonPressed();
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (await App.Current.MainPage.DisplayAlert("Are you sure you want to go back?", "All unsaved information will be lost.", "Yes", "Cancel"))
                    {
                        await this.Navigation.PopAsync();
                    }
                });
                return true;
            }
        }
    }
}