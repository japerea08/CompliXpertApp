using CompliXpertApp.Models;
using Xamarin.Forms;
using CompliXpertApp.Helpers;
using System.Threading.Tasks;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompliXpertApp.ViewModels
{
    class CreateCallReportViewModel : AbstractNotifyPropertyChanged
    {
        //attributes
        private bool isBusy = false;
        private bool customerVisitSelected = false;
        private bool fatcaQuestionnaireSelected = false;
        private int index = -1;
        private Account _account;
        
        //constructor
        public CreateCallReportViewModel()
        {
            MessagingCenter.Subscribe<AccountMasterViewModel, Account>(this, Message.CustomerLoaded, (sender, account)=> 
            {
                Account = account;
            });
            SaveCallReportCommand = new Command(async () => await SaveNewCallReportAsync());
            //must instantiate new call report to take in the new data
            NewCallReport = new CallReport
            {
                CallDate = DateTime.Today
            };
            DeleteCallReportCommand = new Command(async ()=> await DeleteCallReportAsync());
        }
        #region Properties
        //properties
        public ICommand SaveCallReportCommand { get; set; }
        public ICommand DeleteCallReportCommand { get; set; }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        public CallReport NewCallReport { get; set; }
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
        public int SelectedIndex
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
                if(index == 1)
                {
                    CustomerVisitSelected = false;
                    FatcaSelected = true;
                }
                else
                {
                    FatcaSelected = false;
                    CustomerVisitSelected = true;
                }
            }
        }
        public bool CustomerVisitSelected
        {
            get
            {
                return customerVisitSelected;
            }
            set
            {
                customerVisitSelected = value;
                OnPropertyChanged();
            }
        }

        public bool FatcaSelected
        {
            get
            {
                return fatcaQuestionnaireSelected;
            }
            set
            {
                fatcaQuestionnaireSelected = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Methods
        //save the new call report to local db for persistance
        async Task SaveNewCallReportAsync()
        {
            NewCallReport.AccountNumber = Account.AccountNumber;
            NewCallReport.Officer = "Tester";
            NewCallReport.CreatedOnMobile = true;
            List<CallReport> cr = Account.CallReport.ToList();
            cr.Add(NewCallReport);
            Account.CallReport = cr;
            IsBusy = true;
            //save to the DB
            await SaveCallReportAsync();
            IsBusy = false;
            //go back to the previous page
            await App.Current.MainPage.Navigation.PopAsync();
            MessagingCenter.Send<CreateCallReportViewModel, Account>(this, Message.CallReportCreated, Account);
        }
        //adding the callreport to the table
        public async Task SaveCallReportAsync()
        {
            using (var context = new CompliXperAppContext())
            {
                context.Add<CallReport>(NewCallReport);
                await context.SaveChangesAsync();
            }
        }
        public async Task DeleteCallReportAsync()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
        #endregion
    }
}
