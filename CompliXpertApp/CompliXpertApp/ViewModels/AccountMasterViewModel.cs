using CompliXpertApp.Models;
using CompliXpertApp.Views;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class AccountMasterViewModel
    {
        //attributes
        private bool canTap = true;
        //properties
        public ICommand ViewCallReportsCommand { get; private set; }
        public ICommand GoToCreateCallReportCommand { get; private set; }
        public Account Customer { get; set; }
        public int AccountNumber
        {
            get { return Customer.AccountNumber; }
        }
        public int CustomerNumber
        {
            get { return Customer.CustomerNumber; }
        }
        public string Status
        {
            get { return Customer.AccountStatus; }
        }
        public string AccountType
        {
            get { return Customer.AccountType; }
        }
        public string AccountClass
        {
            get { return Customer.AccountClass; }
        }
        public string Country
        {
            get { return Customer.Country; }
        }
        //constructor
        public AccountMasterViewModel(Account account)
        {
            Customer = new Account();
            Customer = account;
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
            await App.Current.MainPage.Navigation.PushAsync(new CreateCallReportScreen());
        }
    }
}
