using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class CallReportDetailsViewModel: AbstractNotifyPropertyChanged
    {
        //attributes
        private CallReport _callReport;
        private bool _customerVisitSelected = false;
        private bool _fatcaSelected = false;
        private bool _createdOnMobile = false;
        //constructor
        public CallReportDetailsViewModel()
        {
            MessagingCenter.Subscribe<CallReportListViewModel, CallReport>(this, Message.CallReportLoaded, (sender, _report) =>
            {
                Report = _report;
                CreatedOnMobile = Report.CreatedOnMobile;
            });
        }
        #region Properties
        public ICommand SaveCallReportCommand { get; set; }
        public ICommand DeleteCallReportCommand { get; set; }
        public CallReport Report {
            get
            {
                return _callReport;
            }
            set
            {
                _callReport = value;
                if (_callReport.Reason == "FATCA Questionnaire")
                {
                    FatcaSelected = true;
                    CustomerVisitSelected = false;
                }
                else
                {
                    FatcaSelected = false;
                    CustomerVisitSelected = true;
                }
                OnPropertyChanged();
            }
        }
        public bool CreatedOnMobile
        {
            get
            {
                return _createdOnMobile;
            }
            set
            {
                _createdOnMobile = value;
                OnPropertyChanged();
            }
        }
        public bool CustomerVisitSelected
        {
            get
            {
                return _customerVisitSelected;
            }
            set
            {
                _customerVisitSelected = value;
                OnPropertyChanged();
            }
        }
        public bool FatcaSelected
        {
            get
            {
                return _fatcaSelected;
            }
            set
            {
                _fatcaSelected = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Methods
       
        #endregion
    }
}
