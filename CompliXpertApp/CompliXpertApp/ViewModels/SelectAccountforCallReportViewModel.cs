using System.Linq;
using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using System.Collections.Generic;
using Xamarin.Forms;
using CompliXpertApp.Views;
using System.Threading.Tasks;

namespace CompliXpertApp.ViewModels
{
    class SelectAccountforCallReportViewModel : AbstractNotifyPropertyChanged
    {
        private List<Account> _accounts;
        private Account _account;
        private bool _canCreateCallReport;
        private List<CallReportType> callReportTypes;
        private CallReportType callReportType;
        public SelectAccountforCallReportViewModel()
        {
            _canCreateCallReport = false;

            MessagingCenter.Subscribe<CompliXpertAppMasterDetailPageMasterViewModel, int>(this, Message.CustomerIdAttached, (sender, customerNumber) =>
            {
                using (CompliXperAppContext context = new CompliXperAppContext())
                {
                    _accounts  = (from _account in context.Account
                                where _account.CustomerNumber == customerNumber
                                select _account).ToList();

                    //get all callreport types
                    CallReportTypes = context.CallReportType.ToList();
                }
                Accounts = _accounts;
            });

            CreateCallReportCommand = new Command(async () => await CreateCallReportAsync(), () => _canCreateCallReport);
        }

        //properties
        public List<CallReportType> CallReportTypes
        {
            get
            {
                return callReportTypes;
            }
            set
            {
                callReportTypes = value;
                OnPropertyChanged();
            }
        }
        public CallReportType CallReportTypeSelected
        {
            get
            {
                return callReportType;
            }
            set
            {
                callReportType = value;
                if (callReportType == null)
                    return;
                if (_account != null)
                    CanCreateCallReport(true);
                OnPropertyChanged();
            }
        }
        public Command CreateCallReportCommand { get; }
        public List<Account> Accounts
        {
            get
            {
                return _accounts;
            }
            set
            {
                _accounts = value;
                OnPropertyChanged();
            }
        }

        public Account SelectedAccount
        {
            get
            {
                return _account;
            }
            set
            {
                _account = value;
                if (_account == null)
                    return;
                if(callReportType != null)
                    CanCreateCallReport(true);
                OnPropertyChanged();
            }
        }
        //methods
        void CanCreateCallReport(bool value)
        {
            _canCreateCallReport = value;
            ((Command) CreateCallReportCommand).ChangeCanExecute();
        }
        //will check to see if an account is selected and a call report type
        virtual public async Task CreateCallReportAsync()
        {
            if(SelectedAccount != null && CallReportTypeSelected != null)
            {
                await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage() { Detail = new NavigationPage(new CreateCallReportScreen()) });

                MessagingCenter.Send<SelectAccountforCallReportViewModel, CallReportType>(this, Message.CallReportTypeLoaded, CallReportTypeSelected);

                MessagingCenter.Send<SelectAccountforCallReportViewModel, Account>(this, Message.AccountLoaded, SelectedAccount);
                SelectedAccount = null;                
            }
        }
    }
}
