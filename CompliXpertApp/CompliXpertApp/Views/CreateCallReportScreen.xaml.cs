using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateCallReportScreen : ContentPage
	{
        public Action CustomNavBackButton { get; set; }
        private CreateCallReportViewModel createCallReportViewModel;
		public CreateCallReportScreen ()
		{
            NavigationPage.SetTitleIconImageSource(this, "compli_logo_xsmall.png");
            CustomNavBackButton = () =>
            {
                if (createCallReportViewModel.ReasonSelected == true)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (await App.Current.MainPage.DisplayAlert("Are you sure you want to go back?", "All unsaved information will be lost.", "Yes", "Cancel"))
                        {
                            await App.Current.MainPage.Navigation.PopAsync();
                        }
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.Current.MainPage.Navigation.PopAsync();
                    });
                }
            };
            createCallReportViewModel = new CreateCallReportViewModel();
			InitializeComponent();
            BindingContext = createCallReportViewModel;
		}
        protected override void OnAppearing()
        {
            MessagingCenter.Send(this, Message.AllowLandscapePortrait);
            base.OnAppearing();            
        }
        protected override void OnDisappearing()
        {
            MessagingCenter.Send(this, Message.PreventLandscape);
            base.OnDisappearing();
        }
        //only for Android hard button back; returns false to implement the back action and returns true to cancel the back action
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await App.Current.MainPage.DisplayAlert("Are you sure you want to return to Customer List?", "All unsaved information will be lost.", "Yes", "No"))
                {
                    await App.Current.MainPage.Navigation.PopToRootAsync();
                }
            });
            return true;
        }
    }
}