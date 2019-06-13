using CompliXpertApp.Models;
using System.Collections.Generic;
using CompliXpertApp.Helpers;
using Xamarin.Forms;
using CompliXpertApp.Views;
using System.Linq;

namespace CompliXpertApp.ViewModels
{
    class CallReportListViewModel: AbstractNotifyPropertyChanged
    {
        //attributes
        private List<CallReport> _callReportsList;
        
        //constructor
        public CallReportListViewModel()
        {
            MessagingCenter.Subscribe<CustomerMasterViewModel, int>(this, Message.AccountNumber, (sender, acctNumber) =>
            {
                using (var context = new CompliXperAppContext())
                {
                    CallReports = context.CallReport.Where(report => report.AccountNumber == acctNumber).ToList();
                }
            });
            MessagingCenter.Subscribe<CallReportDetailsViewModel, int?>(this, Message.AccountNumber, (sender, acctNumber) =>
            {
                using (var context = new CompliXperAppContext())
                {
                    CallReports = context.CallReport.Where(report => report.AccountNumber == acctNumber).ToList();
                }
            });
        }
        //properties
        //listview is binded to this
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
        public CallReport CallReport { get; set; }
        public CallReport CallReportSelected
        {
            get
            {
                return CallReport;
            }
            set
            {
                CallReport = value;
                if (CallReport == null)
                    return;
                GetCallReportDetails(CallReport);
                OnPropertyChanged();
            }
        }

        #region Methods
        async void GetCallReportDetails(CallReport report)
        {
            CallReport = null;
            await App.Current.MainPage.Navigation.PushAsync(new CallReportDetailsScreen());
            MessagingCenter.Send<CallReportListViewModel, CallReport>(this, Message.CallReportLoaded, report);
        }
        #endregion
    }
}
