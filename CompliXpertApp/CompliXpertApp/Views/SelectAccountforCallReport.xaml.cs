﻿using CompliXpertApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectAccountforCallReport : ContentPage
    {
        private SelectAccountforCallReportViewModel selectAccountforCallReportViewModel;
        public SelectAccountforCallReport()
        {
            NavigationPage.SetTitleIconImageSource(this, "compli_logo_xsmall.png");
            selectAccountforCallReportViewModel = new SelectAccountforCallReportViewModel();
            BindingContext = selectAccountforCallReportViewModel;
            InitializeComponent();
        }
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