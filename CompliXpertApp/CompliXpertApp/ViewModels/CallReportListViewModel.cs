using CompliXpertApp.Models;
using System.Collections.Generic;
using CompliXpertApp.Helpers;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class CallReportListViewModel: AbstractNotifyPropertyChanged
    {
        //attributes
        private List<CallReport> _callReportsList;
        
        //constructor
        public CallReportListViewModel()
        {
            MessagingCenter.Subscribe<AccountMasterViewModel, List<CallReport>>(this, Message.CallReportListLoaded, (sender, callreportList) => 
            {
                CallReports = callreportList;   
            });
        }
        //properties
        public List<CallReport> CallReports
        {
            get
            {
                return _callReportsList;
            }
            set
            {
                _callReportsList = value;
                OnPropertyChanged();
            }
        }
    }
}
