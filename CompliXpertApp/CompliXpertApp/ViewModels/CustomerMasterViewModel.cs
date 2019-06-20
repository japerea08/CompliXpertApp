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
        //private List<Account> _accounts;
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
            ViewCallReportsCommand = new Command(async () => await ViewCallReportsAsync(), () => canTap);
            GoToCreateCallReportCommand = new Command(async () => await GoToCreateCallReportAsync());
        }

        //properties
        public Account Account { get; set; }
        //public List<Account> Accounts
        //{
        //    get
        //    {
        //        return _accounts;
        //    }
        //    set
        //    {
        //        _accounts = value;
        //        OnPropertyChanged();
        //    }
        //}
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
                //Accounts = _customer.Account.ToList();
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
        async Task ViewCallReportsAsync()
        {
            IsBusy = true;
            //get the call report list
            using (var context = new CompliXperAppContext())
            {
                //var reports = await context.CallReport.Where(report => report.AccountNumber == Customer.AccountNumber).ToListAsync();
                await App.Current.MainPage.Navigation.PushAsync(new CallReportsList());
                IsBusy = false;
                //MessagingCenter.Send<AccountMasterViewModel, int>(this, Message.AccountNumber, Customer.AccountNumber);
            }
        }
        async Task GoToCreateCallReportAsync()
        {
            await App.Current.MainPage.Navigation.PushAsync(new CreateCallReportScreen());
            MessagingCenter.Send<CustomerMasterViewModel, Customer>(this, Message.CustomerLoaded, Customer);
        }
        
    }
}
