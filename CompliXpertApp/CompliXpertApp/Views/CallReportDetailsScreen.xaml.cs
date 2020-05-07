using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CallReportDetailsScreen : ContentPage
	{
        private CallReportDetailsViewModel viewModel;
        public CallReportDetailsScreen()
        {
            NavigationPage.SetTitleIconImageSource(this, "compli_logo_xsmall.png");
            InitializeComponent();
            viewModel = new CallReportDetailsViewModel();
            BindingContext = viewModel;
        }
        protected override void OnAppearing()
        {
            MessagingCenter.Send(this, Message.AllowLandscapePortrait);
            //DB needs to be checked everytime
            viewModel.Subscribe();
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            MessagingCenter.Send(this, Message.PreventLandscape);
            base.OnDisappearing();
            viewModel.Unsubscribe();
        }
        protected override bool OnBackButtonPressed()
        {
            if(viewModel.CreatedOnMobile == true)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (await App.Current.MainPage.DisplayAlert("Are you sure you want to return to Customer List?", "Any new information entered will be lost.", "Yes", "No"))
                    {
                        await App.Current.MainPage.Navigation.PopToRootAsync();
                    }
                });
                return true;
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                        await App.Current.MainPage.Navigation.PopToRootAsync();
                    
                });
                return true;
            }
            

        }
    }
}