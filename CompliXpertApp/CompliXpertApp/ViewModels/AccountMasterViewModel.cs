using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class AccountMasterViewModel : INotifyPropertyChanged
    {
        //attributes
        private bool canTap = true;
        private Account _customer = new Account();
        public event PropertyChangedEventHandler PropertyChanged;

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
            MessagingCenter.Subscribe<AccountListScreenViewModel, Account>(this, Message.CustomerLoaded, (sender, args) =>
            {
                Customer = args;
            });
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

            await App.Current.MainPage.Navigation.PushAsync(new CallReportsList(callReportList));
        }
        async Task GoToCreateCallReportAsync()
        {
            await App.Current.MainPage.Navigation.PushAsync(new CreateCallReportScreen(Customer));
        }
        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
