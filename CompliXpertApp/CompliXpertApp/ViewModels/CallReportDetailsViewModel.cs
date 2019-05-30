using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class CallReportDetailsViewModel : AbstractNotifyPropertyChanged
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
            SaveCallReportCommand = new Command(async () => await SaveCallReportAsync());
            DeleteCallReportCommand = new Command(async () => await DeleteCallReportAsync());
            CloseCallReportCommand = new Command(async () => await CloseCallReportAsync());
            IsBusy = false;
        }
        #region Properties
        public ICommand SaveCallReportCommand { get; set; }
        public ICommand DeleteCallReportCommand { get; set; }
        public ICommand CloseCallReportCommand { get; set; }
        public bool IsBusy { get; set;}
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
        //really an update call report
        public async Task SaveCallReportAsync()
        {
            using (var context = new CompliXperAppContext())
            {
                IsBusy = true;
                //retrieving entity by id
                var entity = await context.CallReport.FirstOrDefaultAsync(x => x.CallReportId == Report.CallReportId);
                context.Entry(entity).State = EntityState.Modified;
                entity.CallDate = Report.CallDate;
                entity.CustomerComments = Report.CustomerComments;
                entity.CustomerResponse = Report.CustomerResponse;
                entity.CallDate = Report.CallDate;
                entity.Nationality = Report.Nationality;
                entity.OfficerComments = Report.OfficerComments;
                entity.OtherComments = Report.OtherComments;
                entity.Position = Report.Position;
                entity.Purpose = Report.Purpose;
                entity.Reason = Report.Reason;
                entity.ReasonforAlert = Report.ReasonforAlert;
                entity.Reference = Report.Reference;
                entity.Status = Report.Status;
                await context.SaveChangesAsync();
                IsBusy = false;
                await App.Current.MainPage.Navigation.PopAsync();
            }
        }
        //remove Call Report from local DB
        public async Task DeleteCallReportAsync()
        {

        }
        public async Task CloseCallReportAsync()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
        #endregion
    }
}
