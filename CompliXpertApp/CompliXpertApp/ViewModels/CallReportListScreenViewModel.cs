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

                    //get the list of call reports
                    IEnumerable<CallReport> callReports = (from callReport in context.CallReport
                                                    where callReport.AccountNumber == (from account in context.Account
                                                                                      where account.CustomerNumber == customer.CustomerNumber
                                                                                      select account.AccountNumber).FirstOrDefault()
                                                                                      orderby callReport.CallDate descending
                                                    select callReport).ToList();
                    foreach (CallReport callreport in callReports)
                    {
                        callreport.Responses = (from response in context.CallReportResponse
                                                where response.CallReportId == callreport.CallReportId
                                                select response).ToList(); 
                    }
                    //convert the callreport type to the name
                    dummyCallReportList.AddRange(ConvertCallReportTypeToName(context, callReports));

                    newList.Add(dummyCallReportList);
                }

                CallReportGroup = newList;
            }
        }
        private async void GetCallReportDetailScreenAsync()
        {
            //switching back the description to type in the selectedcallreport 
            callReportSelected.CallReportType = (from callReportType in callReportTypes
                                                 where callReportSelected.CallReportType == callReportType.Description
                                                 select callReportType.Type).FirstOrDefault();

            await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage() { Detail = new NavigationPage(new CallReportDetailsScreen()) });
            MessagingCenter.Send<CallReportListScreenViewModel, CallReport>(this, Message.CallReportLoaded, callReportSelected);
            CallReportSelected = null;
        }
        private IEnumerable<CallReport> ConvertCallReportTypeToName(CompliXperAppContext context, IEnumerable<CallReport> callReports)
        {
            foreach (CallReport callReport in callReports)
            {
                callReport.CallReportType = (from type in context.CallReportType
                                             where callReport.CallReportType == type.Type
                                             select type.Description).FirstOrDefault();

            }
            return callReports;
        }
    }
}
