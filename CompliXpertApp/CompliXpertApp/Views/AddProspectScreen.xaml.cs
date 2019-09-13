using CompliXpertApp.Helpers;
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
        //let native code know to unlock orientation
        protected override void OnAppearing()
        {
            MessagingCenter.Send(this, Message.AllowLandscapePortrait);
            base.OnAppearing();
        }
        //let native code know to set back to portrait
        protected override void OnDisappearing()
        {
            MessagingCenter.Send(this, Message.PreventLandscape);
            base.OnDisappearing();
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