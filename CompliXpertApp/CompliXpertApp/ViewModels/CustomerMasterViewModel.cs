using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using System.Windows.Input;
using Xamarin.Forms;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CompliXpertApp.ViewModels
{
    class CustomerMasterViewModel : AbstractNotifyPropertyChanged
    {
        //attributes
        private bool canTap = true;
        private Customer _customer;
        private bool _isBusy = false;

        //constructor
        public CustomerMasterViewModel()
        {
            //this message is for the incoming
            MessagingCenter.Subscribe<CustomerListScreenViewModel, Customer>(this, Message.CustomerLoaded, (sender, args) =>
            {
                Customer = args;
            });
            //this message is for the return
            MessagingCenter.Subscribe<CreateCallReportViewModel, Customer>(this, Message.CallReportCreated, (sender, _account) =>
            {
                Customer = _account;
                foreach (Account account in Customer.Account)
                {
                    if (account.AccountClass == null)
                    {
                        using (CompliXperAppContext context = new CompliXperAppContext())
                        {
                            account.AccountClass = (from _accountClass in context.AccountClasses
                                                    where _accountClass.AccountClassCode == account.AccountClassCode
                                                    select new AccountClass
                                                    {
                                                        AccountClassCode = _accountClass.AccountClassCode,
                                                        Description = _accountClass.Description
                                                    }).FirstOrDefault();
                        }
                    }
                }
            });
        }

        //properties
        public Account Account { get; set; }
        public Account AccountSelected
        {
            get
            {
                return Account;
            }
            set
            {
                Account = value;
                if (Account == null)
                    return;
                GetAccountMaster(Account);
                OnPropertyChanged();
            }
        }
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }
        public ICommand ViewCallReportsCommand { get; private set; }
        public ICommand GoToCreateCallReportCommand { get; private set; }
        public Customer Customer
        {
            get
            {
                return _customer;
            }
            set
            {
                _customer = value;
                OnPropertyChanged();
            }
        }
        //methods
        async void GetAccountMaster(Account account)
        {
            AccountSelected = null;
            IsBusy = true;
            await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage() { Detail = new NavigationPage(new AccountMasterScreen()) });
            IsBusy = false;
            MessagingCenter.Send<CustomerMasterViewModel, Account>(this, Message.AccountLoaded, account);
        }        
    }
}
