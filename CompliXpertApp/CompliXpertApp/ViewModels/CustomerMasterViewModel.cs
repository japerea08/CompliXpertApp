using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class CustomerMasterViewModel : AbstractNotifyPropertyChanged
    {
        //attributes
        private bool canTap = true;
        private string _customerMaster = "Customer Master- ";
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
            MessagingCenter.Subscribe<CreateCallReportViewModel, Customer>(this, Message.CallReportCreated, (sender, account) =>
            {
                Customer = account;
            });
        }

        //properties
        public Account Account { get; set; }
        public string CustomerMaster
        {
            get
            {
                return _customerMaster;
            }
            set
            {
                if (Customer != null)
                    _customerMaster += value;
                OnPropertyChanged();
            }
        }
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
                CustomerMaster = _customer.CustomerName;
                OnPropertyChanged();
            }
        }
        //methods
        async void GetAccountMaster(Account account)
        {
            AccountSelected = null;
            IsBusy = true;
            await App.Current.MainPage.Navigation.PushAsync(new AccountMasterScreen());
            IsBusy = false;
            MessagingCenter.Send<CustomerMasterViewModel, Account>(this, Message.AccountLoaded, account);
        }        
    }
}
