using CompliXpertApp.Models;
using System;
using System.Collections.Generic;
using CompliXpertApp.Helpers;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class CallReportListViewModel: AbstractNotifyPropertyChanged
    {
        //attributes
        private List<CallReport> _callReportsList;
        private CallReport _callReport;
        
        //constructor
        public CallReportListViewModel()
        {
            MessagingCenter.Subscribe<AccountMasterViewModel, List<CallReport>>(this, Message.CallReportListLoaded, (sender, args) => { CallReports = args; });
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
