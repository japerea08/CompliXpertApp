using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class AccountListScreenViewModel
    {
        private Account customer { get; set; }
        public AccountListScreenViewModel(List<Account> accounts)
        {
            customer = new Account();
            Accounts = accounts;
            AddCustomerCommand = new Command(AddCustomer);
        }

        public async void AddCustomer()
        {
            await App.Current.MainPage.Navigation.PushAsync(new AddCustomerScreen());
        }

        //properties
        public Command AddCustomerCommand { get; }

        public List<Account> Accounts { get; set; }
        public Account CustomerSelected
        {
            get
            {
                return customer;
            }
            set
            {
                customer = value;
                if (customer == null)
                    return;
                callNextScreen();
            }
        }
        //
        public void callNextScreen()
        {
            DependencyService.Get<IToast>().WriteToast(CustomerSelected.CustomerName);
        }
    }
}
