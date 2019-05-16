﻿using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class AccountListScreenViewModel : AbstractNotifyPropertyChanged
    {
        private bool isBusy = false;
        private List<Account> _accountList;

        public AccountListScreenViewModel()
        {
            MessagingCenter.Subscribe<LoginViewModel, List<Account>>(this, Message.AccountListLoaded, (sender, args) => 
            {
                Accounts = args;
            });
            AddCustomerCommand = new Command(AddCustomer);
        }
        //to be used for future development
        public async void AddCustomer()
        {
            await App.Current.MainPage.Navigation.PushAsync(new AddCustomerScreen());
        }

        //properties
        private Account Customer { get; set; }
        public Command AddCustomerCommand { get; }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        public List<Account> Accounts
        {
            get
            {
                return _accountList;
            }
            set
            {
                _accountList = value;
                OnPropertyChanged();
            }
        }
        public Account CustomerSelected
        {
            get
            {
                return Customer;
            }
            set
            {               
                Customer = value;
                if (Customer == null)
                    return;
                GetAccountMaster(CustomerSelected);
            }
        }
        //methods
        async void GetAccountMaster(Account account)
        {
            IsBusy = true;
            await App.Current.MainPage.Navigation.PushAsync(new AccountMaster());
            MessagingCenter.Send<AccountListScreenViewModel, Account>(this, Message.CustomerLoaded, account);
            IsBusy = false;
        }
    }
}
