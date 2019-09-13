using CompliXpertApp.Helpers;
using CompliXpertApp.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            MessagingCenter.Send(this, Message.PreventLandscape);
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<CallReportListViewModel>(viewModel, Message.CallReportLoaded);
        }
    }
}