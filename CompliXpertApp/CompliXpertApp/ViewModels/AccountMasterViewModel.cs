using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class AccountMasterViewModel : AbstractNotifyPropertyChanged
    {
        //attributes
        private bool canTap = true;
        private Account _customer;

        //properties
        public ICommand ViewCallReportsCommand { get; private set; }
        public ICommand GoToCreateCallReportCommand { get; private set; }
        public Account Customer
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
        //constructor
        public AccountMasterViewModel()
        {
            //this message is for the incoming
            MessagingCenter.Subscribe<AccountListScreenViewModel, Account>(this, Message.CustomerLoaded, (sender, args) =>
            {
                Customer = args;
            });
            //this message is for the return
            MessagingCenter.Subscribe<CreateCallReportViewModel, Account>(this, Message.CallReportCreated, (sender, account)=> { Customer = account; });
            ViewCallReportsCommand = new Command(async () => await ViewCallReportsAsync(), () => canTap);
            GoToCreateCallReportCommand = new Command(async () => await GoToCreateCallReportAsync());
        }

        //methods
        async Task ViewCallReportsAsync()
        {
            List<CallReport> callReportList = new List<CallReport>();
            //get all the Callreport
            foreach (var callReport in Customer.CallReport)
                callReportList.Add(callReport);

            await App.Current.MainPage.Navigation.PushAsync(new CallReportsList());
            MessagingCenter.Send<AccountMasterViewModel, List<CallReport>>(this, Message.CallReportListLoaded, callReportList);
        }
        async Task GoToCreateCallReportAsync()
        {
            await App.Current.MainPage.Navigation.PushAsync(new CreateCallReportScreen());
            MessagingCenter.Send<AccountMasterViewModel, Account>(this, Message.CustomerLoaded, Customer);
        }
        
    }
}
