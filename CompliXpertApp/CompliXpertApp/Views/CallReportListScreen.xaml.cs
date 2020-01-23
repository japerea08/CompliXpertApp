using CompliXpertApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CompliXpertApp.Models;

namespace CompliXpertApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CallReportListScreen : ContentPage
	{
        private CallReportListScreenViewModel callReportListScreenViewModel = new CallReportListScreenViewModel();
        public CallReportListScreen ()
		{
            BindingContext = callReportListScreenViewModel;
            NavigationPage.SetTitleIconImageSource(this, "compli_logo_xsmall.png");
            InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            callReportListScreenViewModel.InitializeData();
        }
        //method only works for Android Hard Key
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await App.Current.MainPage.DisplayAlert("Sign Off", "Are you sure you want to sign off?", "Yes", "No"))
                {
                    await App.Current.MainPage.Navigation.PopToRootAsync();
                }
            });
            return true;

        }
    }
}