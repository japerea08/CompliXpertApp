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
		public CallReportsList ()
		{
			InitializeComponent ();
            BindingContext = new CallReportListViewModel();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            BindingContext = null;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext == null)
                BindingContext = new CallReportListViewModel();
        }
    }
}