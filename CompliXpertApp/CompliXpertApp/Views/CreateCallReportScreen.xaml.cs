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
            MessagingCenter.Subscribe<AddNoteScreenViewModel, Note> (this, Message.NoteCreated, (sender, note) =>
            {
                createCallReportViewModel.Note = note;
            }); 
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            MessagingCenter.Send(this, Message.PreventLandscape);
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<CustomerMasterViewModel>(createCallReportViewModel, Message.CustomerLoaded);
        }
        //only for Android hard button back; returns false to implement the back action and returns true to cancel the back action
        protected override bool OnBackButtonPressed()
        {
            //check to see if data is saved
            if (createCallReportViewModel.ReasonSelected == false)
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