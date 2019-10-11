using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class AccountMasterViewModel : AbstractNotifyPropertyChanged
    {
        //attributes
        private bool canTap = true;
        private Account _account;
        private bool _isBusy = false;
        //constructor
        public AccountMasterViewModel()
        {
            //this message is for the incoming
            MessagingCenter.Subscribe<CustomerMasterViewModel, Account>(this, Message.AccountLoaded, (sender, args) =>
            {
                Account = args;
            });
            //this message is for the return
            MessagingCenter.Subscribe<CreateCallReportViewModel, Account>(this, Message.CallReportCreated, (sender, account) => 
            {
                Account = account;
            });
            ViewCallReportsCommand = new Command(async () => await ViewCallReportsAsync(), () => canTap);
            GoToCreateCallReportCommand = new Command(async () => await GoToCreateCallReportAsync());
        }
        //properties
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
        public Account Account
        {
            get
            {
                return _account;
            }
            set
            {
                _account = value;
                OnPropertyChanged();
            }
        }
        //methods
        async Task ViewCallReportsAsync()
        {
            IsBusy = true;
            await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage() { Detail = new NavigationPage(new CallReportsList()) });
            IsBusy = false;
            MessagingCenter.Send<AccountMasterViewModel, int>(this, Message.AccountNumber, Account.AccountNumber);
           
        }
        async Task GoToCreateCallReportAsync()
        {
            await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage() { Detail = new NavigationPage(new CreateCallReportScreen()) });
            MessagingCenter.Send<AccountMasterViewModel, Account>(this, Message.CustomerLoaded, Account);
        }

    }
}
