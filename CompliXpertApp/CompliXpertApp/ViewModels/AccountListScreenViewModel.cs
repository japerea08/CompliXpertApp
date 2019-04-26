using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using System.Collections.Generic;
using System.ComponentModel;
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
        }

        //properties
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
