using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class CallReportListScreenViewModel : AbstractNotifyPropertyChanged
    {
        private List<CallReportList> callReportGroup = new List<CallReportList>();
        private CallReport callReportSelected;
        private List<CallReportType> callReportTypes;
        //properties
        public List<CallReport> CallReports { get; set; }
        public List<CallReportList> CallReportGroup
        {
            get
            {
                return callReportGroup;
            }
            set
            {
                callReportGroup = value;
                OnPropertyChanged();
            }
        }
        public CallReport CallReportSelected
        {
            get
            {
                return callReportSelected;
            }
            set
            {
                callReportSelected = value;
                if (callReportSelected == null)
                    return;
                GetCallReportDetailScreenAsync();
                OnPropertyChanged();
            }
        }

        //methods
        public void InitializeData()
        {
            using (CompliXperAppContext context = new CompliXperAppContext())
            {
                //initialize callreporttype list
                callReportTypes = context.CallReportType.ToList();

                var newList = new List<CallReportList>();
                //get each customer                
                foreach (Customer customer in context.Customer)
                {
                    CallReportList dummyCallReportList = new CallReportList();
                    dummyCallReportList.Heading = customer.CustomerName;

                    //get all accounts
                    List<Account> accounts = (from account in context.Account
                                              where account.CustomerNumber == customer.CustomerNumber
                                              select account).ToList();
                    foreach(Account account in accounts)
                    {
                        //get the list of call reports
                        IEnumerable<CallReport> callReports = (from callreport in context.CallReport
                                                   where callreport.AccountNumber == account.AccountNumber
                                                   orderby callreport.CallDate descending
                                                   select callreport).ToList();

                        foreach (CallReport callreport in callReports)
                        {
                            callreport.Reason = "Type: " + callreport.Reason;
                            callreport.Responses = (from response in context.CallReportResponse
                                                    where response.CallReportId == callreport.CallReportId
                                                    select response).ToList();                 
                        }
                        dummyCallReportList.AddRange(callReports);

                    }

                    newList.Add(dummyCallReportList);
                }

                CallReportGroup = newList;
            }
        }
        private async void GetCallReportDetailScreenAsync()
        { 
            await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage() { Detail = new NavigationPage(new CallReportDetailsScreen()) });
            MessagingCenter.Send<CallReportListScreenViewModel, CallReport>(this, Message.CallReportLoaded, callReportSelected);
            CallReportSelected = null;
        }
    }
}
