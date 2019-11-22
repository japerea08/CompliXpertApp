using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using System;

namespace CompliXpertApp.ViewModels
{
    class CallReportDetailsViewModel : AbstractNotifyPropertyChanged
    {
        //attributes
        private CallReport _callReport;
        private bool _createdOnMobile = false;
        private bool _isBusy = false;
        private List<QuestionandResponse> _questionandResponses = new List<QuestionandResponse>();
        private string _callReportType;
        private int _height = 0;


        //constructor
        public CallReportDetailsViewModel()
        {
            MessagingCenter.Subscribe<CallReportListScreenViewModel, CallReport>(this, Message.CallReportLoaded, async (sender, _report) =>
            {
                Report = _report;
                CreatedOnMobile = Report.CreatedOnMobile;

                //get the questions
                using (var context = new CompliXperAppContext())
                {
                    ReportType = _report.CallReportType;

                    List<CallReportQuestions> Questions = await (
                        from _q in context.CallReportQuestions
                        where _q.Type == Report.CallReportType
                        select new CallReportQuestions
                        {
                            QuestionId = _q.QuestionId,
                            QuestionHeader = _q.QuestionHeader,
                            Status = _q.Status,
                            Type = _q.Type
                        }
                    ).ToListAsync();
                    List<QuestionandResponse> _qr = new List<QuestionandResponse>();
                    foreach (var question in Questions)
                    {
                        foreach (var response in Report.Responses)
                        {
                            if (question.QuestionId == response.QuestionId)
                            {
                                //instantiate new object
                                QuestionandResponse questionandResponse = new QuestionandResponse();
                                questionandResponse.QuestionHeader = question.QuestionHeader;
                                questionandResponse.Response = response.Response;
                                questionandResponse.QuestionId = response.QuestionId;
                                questionandResponse.ResponseId = response.ResponseId;
                                _qr.Add(questionandResponse);
                                break;
                            }
                        }
                    }
                    QuestionandResponses = _qr;
                }
                //manipulate the stack
                List<Page> stackPages = new List<Page>();
                foreach (Page page in App.Current.MainPage.Navigation.NavigationStack)
                {
                    stackPages.Add(page);
                }
                App.Current.MainPage.Navigation.RemovePage(App.Current.MainPage.Navigation.NavigationStack[App.Current.MainPage.Navigation.NavigationStack.Count - 2]);
                //place the new page into stack
                App.Current.MainPage.Navigation.InsertPageBefore(new CompliXpertAppMasterDetailPage() { Detail = new NavigationPage(new CallReportsList()) }, App.Current.MainPage.Navigation.NavigationStack[App.Current.MainPage.Navigation.NavigationStack.Count - 1]);
                MessagingCenter.Send<CallReportDetailsViewModel, int?>(this, Message.AccountNumber, Report.AccountNumber);
            });

            MessagingCenter.Subscribe<CallReportListViewModel, CallReport>(this, Message.CallReportLoaded, async (sender, _report) =>
            {
                Report = _report;
                CreatedOnMobile = Report.CreatedOnMobile;

                //get the questions
                using (var context = new CompliXperAppContext())
                {
                    ReportType = (from r in context.CallReportType
                                 where r.Type == Report.CallReportType
                                 select r.Description).SingleOrDefault();

                    List<CallReportQuestions> Questions = await (
                        from _q in context.CallReportQuestions
                        where _q.Type == Report.CallReportType
                        select new CallReportQuestions
                        {
                            QuestionId = _q.QuestionId,
                            QuestionHeader = _q.QuestionHeader,
                            Status = _q.Status,
                            Type = _q.Type
                        }
                    ).ToListAsync();
                    List<QuestionandResponse> _qr = new List<QuestionandResponse>();
                    foreach (var question in Questions)
                    {
                       foreach(var response in Report.Responses)
                        {
                            if (question.QuestionId == response.QuestionId)
                            {
                                //instantiate new object
                                QuestionandResponse questionandResponse = new QuestionandResponse();
                                questionandResponse.QuestionHeader = question.QuestionHeader;
                                questionandResponse.Response = response.Response;
                                questionandResponse.QuestionId = response.QuestionId;
                                questionandResponse.ResponseId = response.ResponseId;
                                _qr.Add(questionandResponse);
                                break;
                            }
                        }
                    }
                    QuestionandResponses = _qr;
                }
                //manipulate the stack
                List<Page> stackPages = new List<Page>();
                foreach (Page page in App.Current.MainPage.Navigation.NavigationStack)
                {
                    stackPages.Add(page);
                }
                App.Current.MainPage.Navigation.RemovePage(App.Current.MainPage.Navigation.NavigationStack[App.Current.MainPage.Navigation.NavigationStack.Count - 2]);
                //place the new page into stack
                App.Current.MainPage.Navigation.InsertPageBefore(new CompliXpertAppMasterDetailPage() { Detail = new NavigationPage(new CallReportsList()) }, App.Current.MainPage.Navigation.NavigationStack[App.Current.MainPage.Navigation.NavigationStack.Count - 1]);
                MessagingCenter.Send<CallReportDetailsViewModel, int?>(this, Message.AccountNumber, Report.AccountNumber);
            });
            SaveCallReportCommand = new Command(async () => await SaveCallReportAsync());
            DeleteCallReportCommand = new Command(async () => await DeleteCallReportAsync());
            CloseCallReportCommand = new Command(async () => await CloseCallReportAsync());
        }
        #region Properties
        public ICommand SaveCallReportCommand { get; set; }
        public ICommand DeleteCallReportCommand { get; set; }
        public ICommand CloseCallReportCommand { get; set; }
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
        public string ReportType
        {
            get
            {
                return _callReportType;
            }
            set
            {
                _callReportType = value;
                OnPropertyChanged();
            }
        }
        public List<QuestionandResponse> QuestionandResponses
        {
            get
            {
                return _questionandResponses;
            }
            set
            {
                _questionandResponses = value;
                Height = 80 * _questionandResponses.Count();
                OnPropertyChanged();
            }
        }
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }
        public CallReport Report
        {
            get
            {
                return _callReport;
            }
            set
            {
                _callReport = value;
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
        #endregion
        #region Methods
        //really an update call report
        public async Task SaveCallReportAsync()
        {
            using (var context = new CompliXperAppContext())
            {
                IsBusy = true;
                //retrieving entity by id
                CallReport report = await context.CallReport
                                    .SingleAsync(r => r.CallReportId == Report.CallReportId);
                report.CallDate = Report.CallDate;
                report.Position = Report.Position;
                report.Reference = Report.Reference;
                List<CallReportResponse> responses = new List<CallReportResponse>();
                foreach (var qr in QuestionandResponses)
                {
                    responses.Add(
                        new CallReportResponse
                            {
                                
                                Response = qr.Response,
                                QuestionId = qr.QuestionId,
                                CallReportId = report.CallReportId,
                                ResponseId = qr.ResponseId
                            }
                        );
                }
                context.CallReportResponse.UpdateRange(responses);

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    Console.WriteLine(e.InnerException);
                }
                
                IsBusy = false;
                await App.Current.MainPage.Navigation.PopAsync();
                MessagingCenter.Send<CallReportDetailsViewModel, int?>(this, Message.AccountNumber, Report.AccountNumber);
            }
        }
        //remove Call Report from local DB
        public async Task DeleteCallReportAsync()
        {
            if (await App.Current.MainPage.DisplayAlert("Are you sure you want to delete this Call Report?", "This action cannot be undone.", "Yes", "Cancel"))
            {
                using (var context = new CompliXperAppContext())
                {
                    IsBusy = true;
                    var entity = await context.CallReport.FirstOrDefaultAsync(x => x.CallReportId == Report.CallReportId);
                    context.CallReport.Remove(entity);
                    await context.SaveChangesAsync();
                    IsBusy = false;
                    await App.Current.MainPage.Navigation.PopAsync();
                    MessagingCenter.Send<CallReportDetailsViewModel, int?>(this, Message.AccountNumber, Report.AccountNumber);
                }
            }
        }
        public async Task CloseCallReportAsync()
        {
            await App.Current.MainPage.Navigation.PopAsync();
            //send back the current call Reports
            MessagingCenter.Send<CallReportDetailsViewModel, int?>(this, Message.AccountNumber, Report.AccountNumber);
        }
        #endregion
    }
}
