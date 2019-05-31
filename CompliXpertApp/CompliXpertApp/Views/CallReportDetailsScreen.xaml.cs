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
            InitializeComponent();
            viewModel = new CallReportDetailsViewModel();
            BindingContext = viewModel;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<CallReportListViewModel>(viewModel, Message.CallReportLoaded);
        }
    }
}