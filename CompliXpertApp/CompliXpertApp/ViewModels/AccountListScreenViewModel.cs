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
        private bool isBusy = false;

        public event PropertyChangedEventHandler PropertyChanged;

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
        private Account customer { get; set; }
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
                GetAccountMaster(CustomerSelected);
            }
        }
        //methods
        async void GetAccountMaster(Account account)
        {
            IsBusy = true;
            await App.Current.MainPage.Navigation.PushAsync(new AccountMaster(account));
            IsBusy = false;
        }
        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
