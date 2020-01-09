using CompliXpertApp.Models;
using Xamarin.Forms;
using CompliXpertApp.Helpers;
using System.Threading.Tasks;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CompliXpertApp.Views;

namespace CompliXpertApp.ViewModels
{
    class CreateCallReportViewModel : AbstractNotifyPropertyChanged
    {
        //attributes
        private bool isBusy = false;
        private bool customerVisitSelected = false;
        private Account _account;
        private string _customerName;
        private List<CallReportQuestions> _callReportQuestions;
        private List<QuestionandResponse> _questionsandResponse = new List<QuestionandResponse>();
        private CallReportType _type;
        private double _height = 0;
        private bool canSave = false;

        //constructor
        public CreateCallReportViewModel()
        {
            MessagingCenter.Subscribe<SelectAccountforCallReportViewModel, Account>(this, Message.AccountLoaded,  (sender, account) => 
            {
                Account = account;
                InitializeCreateCallReportScreenAsync();

            });

            MessagingCenter.Subscribe<SelectAccountforCallReportViewModel, CallReportType>(this, Message.CallReportTypeLoaded, (sender, callreporttype) => 
            {
                Type = callreporttype;
                InitializeCallReportQuestions();
            });

            MessagingCenter.Subscribe<AccountMasterViewModel, Account>(this, Message.CustomerLoaded,  (sender, account)=> 
            {
                Account = account;
                InitializeCreateCallReportScreenAsync();
            });
            SaveCallReportCommand = new Command(async () => await SaveNewCallReportAsync(), () => canSave);

            AddNoteCommand = new Command(async () => await AddNoteAsync());

            AddPersonCommand = new Command(async () => await AddPersonAsync());
            //must instantiate new call report to take in the new data
            NewCallReport = new CallReport
            {
                CallDate = DateTime.Now,
                CreatedDate = DateTime.Now
            };

            
        }

        private void InitializeCallReportQuestions()
        {
            using (CompliXperAppContext context = new CompliXperAppContext())
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

                //add to qr

                List<QuestionandResponse> ques = new List<QuestionandResponse>();
                foreach (CallReportQuestions question in Questions)
                {
                    QuestionandResponse qr = new QuestionandResponse();
                    qr.QuestionHeader = question.QuestionHeader;
                    qr.QuestionId = question.QuestionId;
                    qr.Response = "";
                    ques.Add(qr);
                }
                QR = ques;
            }
        }
        private async void InitializeCreateCallReportScreenAsync()
        {
            using (CompliXperAppContext context = new CompliXperAppContext())
            {
                CustomerName = (
                    from c in context.Customer
                    where c.CustomerNumber == Account.CustomerNumber
                    select c.CustomerName
                ).FirstOrDefault();
            }
        }
        #region Properties
        //properties
        public ICommand AddNoteCommand { get; set; }
        public ICommand AddPersonCommand { get; set; }
        public ICommand SaveCallReportCommand { get; set; }
        public double StandardHeight { get; set; }
        public Note Note { get; set; }
        public double Height
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
                if(_type != null)
                {
                    CanSave(true);
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

                        //add to qr

                        List<QuestionandResponse> ques = new List<QuestionandResponse>();
                        foreach (CallReportQuestions question in Questions)
                        {
                            QuestionandResponse qr = new QuestionandResponse();
                            qr.QuestionHeader = question.QuestionHeader;
                            qr.QuestionId = question.QuestionId;
                            qr.Response = "";
                            ques.Add(qr);
                        }
                        QR = ques;
                    }
                }
                OnPropertyChanged();
            }
        }
        public List<QuestionandResponse> QR
        {
            get
            {
                return _questionsandResponse;
            }
            set
            {
                _questionsandResponse = value;
                Height = (StandardHeight * 2) * _questionsandResponse.Count();
                ReasonSelected = true;
                OnPropertyChanged();
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
                Height = (StandardHeight*2) * _callReportQuestions.Count();
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
        void CanSave(bool value)
        {
            canSave = value;
            ((Command) SaveCallReportCommand).ChangeCanExecute();
        }

        async Task AddNoteAsync()
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new AddNoteScreen());
            //use messaging center to send call report info
            MessagingCenter.Send<CreateCallReportViewModel, CallReport>(this, Message.CallReportLoaded, NewCallReport);
        }

        private Task AddPersonAsync()
        {
            throw new NotImplementedException();
        }
        //save the new call report to local db for persistance
        async Task SaveNewCallReportAsync()
        {
            if (Note != null)
            {
                Note.CallReportId = NewCallReport.CallReportId;
                NewCallReport.Notes = new List<Note>();
                NewCallReport.Notes.Add(Note);
            }

            NewCallReport.AccountNumber = Account.AccountNumber;
            NewCallReport.Officer = "Tester";
            NewCallReport.CreatedOnMobile = true;
            NewCallReport.CallReportType = Type.Type;
            NewCallReport.Reason = Type.Description;
            List<CallReport> cr = Account.CallReport.ToList();
            cr.Add(NewCallReport);
            Account.CallReport = cr;
            IsBusy = true;
            //save to the DB
            await SaveCallReportAsync();
            IsBusy = false;
            //go back to the previous page
            //deselect the type of callreport
            Type = null;
            ReasonSelected = false;
            await App.Current.MainPage.Navigation.PopToRootAsync();
            MessagingCenter.Send<CreateCallReportViewModel, Account>(this, Message.CallReportCreated, Account);
        }
        //adding the callreport to the table
        public async Task SaveCallReportAsync()
        {
            using (var context = new CompliXperAppContext())
            {
                context.Add<CallReport>(NewCallReport);
                await context.SaveChangesAsync();

                ////get the lastes call report
                CallReport lastReport = await context.CallReport
                                        .OrderByDescending(r => r.CallReportId)
                                        .FirstAsync();
                ////get the lastes call report

                List<CallReportResponse> responses = new List<CallReportResponse>();
                foreach (QuestionandResponse qr in QR)
                {
                    responses.Add(
                        new CallReportResponse
                            {
                                Response = qr.Response,
                                QuestionId = qr.QuestionId,
                                CallReportId = lastReport.CallReportId
                            }
                        );
                }
                lastReport.Responses = responses;
                //add the responses to the DB
                await context.AddRangeAsync(responses);
                await context.SaveChangesAsync();
            }
        }
        #endregion
    }
}
