using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CompliXpertApp.ViewModels
{
    class AccountMasterViewModel : AbstractNotifyPropertyChanged
    {
        //attributes
        private bool canTap = true;
        private Account _customer;
        private bool _isBusy = false;

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
            MessagingCenter.Subscribe<CustomerListScreenViewModel, Account>(this, Message.CustomerLoaded, (sender, args) =>
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
            IsBusy = true;
            //get the call report list
            using (var context = new CompliXperAppContext())
            {
                //var reports = await context.CallReport.Where(report => report.AccountNumber == Customer.AccountNumber).ToListAsync();
                await App.Current.MainPage.Navigation.PushAsync(new CallReportsList());
                IsBusy = false;
                MessagingCenter.Send<AccountMasterViewModel, int>(this, Message.AccountNumber, Customer.AccountNumber);
            }
        }
        async Task GoToCreateCallReportAsync()
        {
            await App.Current.MainPage.Navigation.PushAsync(new CreateCallReportScreen());
            MessagingCenter.Send<AccountMasterViewModel, Account>(this, Message.CustomerLoaded, Customer);
        }
        
    }
}
