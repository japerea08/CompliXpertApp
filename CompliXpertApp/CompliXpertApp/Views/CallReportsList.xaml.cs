using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CallReportsList : ContentPage
	{
        private CallReportListViewModel callReportListViewModel;
		public CallReportsList ()
		{
            callReportListViewModel = new CallReportListViewModel();
            InitializeComponent ();
            BindingContext = callReportListViewModel;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            BindingContext = null;
            MessagingCenter.Unsubscribe<AccountMasterViewModel, int>(callReportListViewModel, Message.AccountNumber);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext == null)
            {
                callReportListViewModel = new CallReportListViewModel();
                BindingContext = new CallReportListViewModel();
            }            
        }
    }
}