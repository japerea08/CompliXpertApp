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
                }
                Accounts = _accounts;
            });

            CreateCallReportCommand = new Command(async () => await CreateCallReportAsync(), () => _canCreateCallReport);
        }

        //properties
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
        private async Task CreateCallReportAsync()
        {
            if(SelectedAccount != null)
            {
                //just go to Callreport screen
                await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage() { Detail = new NavigationPage(new CreateCallReportScreen()) });
                MessagingCenter.Send<SelectAccountforCallReportViewModel, Account>(this, Message.AccountLoaded, SelectedAccount);
                SelectedAccount = null;
                
            }
            return;
        }
    }
}
