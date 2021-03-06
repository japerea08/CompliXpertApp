﻿using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomerMaster : ContentPage
	{
        CustomerMasterViewModel customerMasterViewModel;
		public CustomerMaster()
		{
            customerMasterViewModel = new CustomerMasterViewModel();
            NavigationPage.SetTitleIconImageSource(this, "compli_logo_xsmall.png");
            InitializeComponent ();
            BindingContext = customerMasterViewModel;
		}
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<CustomerListScreenViewModel, Customer>(customerMasterViewModel, Message.CustomerLoaded);
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            ListView listView = (ListView)this.FindByName("ListView");
            var args = (TappedEventArgs) e;
            int index = (int) args.Parameter;
        }

    }
}