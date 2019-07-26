using CompliXpertApp.Models;
using Xamarin.Forms;
using CompliXpertApp.Helpers;
using System.Threading.Tasks;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CompliXpertApp.ViewModels
{
    class CreateCallReportViewModel : AbstractNotifyPropertyChanged
    {
        //attributes
        private bool isBusy = false;
        private bool customerVisitSelected = false;
        private Account _account;
        private string _customerName;
        private List<CallReportType> _types;
        private List<CallReportQuestions> _callReportQuestions;
        private CallReportType _type;
        private int _height = 0;

        //constructor
        public CreateCallReportViewModel()
        {
            MessagingCenter.Subscribe<AccountMasterViewModel, Account>(this, Message.CustomerLoaded, async (sender, account)=> 
            {
                Account = account;
                using (var context = new CompliXperAppContext())
                {
                    CustomerName = (
                        from c in context.Customer
                        where c.CustomerNumber == Account.CustomerNumber
                        select c.CustomerName
                    ).FirstOrDefault();

                    //get all the types
                    Types = await context.CallReportType.ToListAsync();
                }
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
        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        }
        public List<CallReportType> Types
        {
            get
            {
                return _types;
            }
            set
            {
                _types = value;
                OnPropertyChanged();
            }
        }
        public string CustomerName
        {
            get
            {
                return _customerName;
            }
            set
            {
                _customerName = value;
                OnPropertyChanged();
            }
        }
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

        public CallReportType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                using (var context = new CompliXperAppContext())
                {
                    Questions = (
                        from q in context.CallReportQuestions
                        where q.Type == Type.Type
                        select new CallReportQuestions
                        {
                            QuestionId = q.QuestionId,
                            QuestionHeader = q.QuestionHeader,
                            Status = q.Status,
                            Type = q.Type
                        }
                    ).ToList();
                }
            }
        }
        //binding for questions
        public List<CallReportQuestions> Questions
        {
            get
            {
                return _callReportQuestions;
            }
            set
            {
                _callReportQuestions = value;
                Height = 90 * _callReportQuestions.Count();
                ReasonSelected = true;
                OnPropertyChanged();
            }
        }
        public bool ReasonSelected
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
