using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompliXpertApp.ViewModels
{
    class CallReportListScreenViewModel : AbstractNotifyPropertyChanged
    {
        private List<CallReportList> callReportGroup = new List<CallReportList>();
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

        //methods
        public void InitializeData()
        {
            using (CompliXperAppContext context = new CompliXperAppContext())
            {
                
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
                    //convert the callreport type to the name
                    dummyCallReportList.AddRange(ConvertCallReportTypeToName(context, callReports));

                    newList.Add(dummyCallReportList);
                }             

                CallReportGroup = newList;
            }
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
