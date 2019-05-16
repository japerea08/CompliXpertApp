using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class AccountListScreenViewModel : INotifyPropertyChanged
    {
        private string _accounts;
        private bool isBusy = false;
        private List<Account> _accountList = new List<Account>();
        public event PropertyChangedEventHandler PropertyChanged;

        public AccountListScreenViewModel()
        {
            MessagingCenter.Subscribe<LoginViewModel, List<Account>>(this, Message.AccountListLoaded, (sender, args) => 
            {
                Accounts = args;
            });
            Customer = new Account();
            AddCustomerCommand = new Command(AddCustomer);
        }

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
        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
