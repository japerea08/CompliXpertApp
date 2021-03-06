﻿using CompliXpertApp.Models;
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
        public List<Note> _notes;
        public List<Person> persons;

        //constructor
        public CreateCallReportViewModel()
        {
            Notes = new List<Note>();
            _notes = new List<Note>();
            Persons = new List<Person>();
            persons = new List<Person>();

            
            SaveCallReportCommand = new Command(async () => await SaveNewCallReportAsync(), () => canSave);

            AddNoteCommand = new Command(async () => await AddNoteAsync());

            AddPersonCommand = new Command(async () => await AddPersonAsync());
            //must instantiate new call report to take in the new data
            NewCallReport = new CallReport
            {
                CallDate = DateTime.Now,
                CreatedDate = DateTime.Now
            };

            Subscribe();
            
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
                CustomerName = await (
                    from c in context.Customer
                    where c.CustomerNumber == Account.CustomerNumber
                    select c.CustomerName
                ).FirstOrDefaultAsync();
            }
        }
        #region Properties
        //properties
        public ICommand AddNoteCommand { get; set; }
        public ICommand AddPersonCommand { get; set; }
        public ICommand SaveCallReportCommand { get; set; }
        public double StandardHeight { get; set; }
        public List<Note> Notes { get; set; }
        public List<Person> Persons { get; set; }
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

            if(canSave == true)
                ((Command) SaveCallReportCommand).ChangeCanExecute();
        }

        async Task AddNoteAsync()
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new AddNoteScreen());

        }

        async Task AddPersonAsync()
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new AddPersonScreen(-1, false));
        }
        //save the new call report to local db for persistance
        async Task SaveNewCallReportAsync()
        {
            Unsubscribe();
            if (Notes.Count > 0)
            {
                foreach (Note note in Notes)
                {
                    note.CallReportId = NewCallReport.CallReportId;
                }
                NewCallReport.Notes = Notes;
            }
            if(Persons.Count > 0)
            {
                foreach(Person person in Persons)
                {
                    person.CallReportId = NewCallReport.CallReportId;
                }
                NewCallReport.Persons = Persons;
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
        //method to unsubscribe
        private void Unsubscribe()
        {
            MessagingCenter.Unsubscribe<AddNoteScreenViewModel, Note>(this, Message.NoteCreated);

            MessagingCenter.Unsubscribe<AddPersonScreenViewModel, Person>(this, Message.PersonCreated);

            MessagingCenter.Unsubscribe<SelectAccountforCallReportViewModel, Account>(this, Message.AccountLoaded);

            MessagingCenter.Unsubscribe<SelectAccountforCallReportViewModel, CallReportType>(this, Message.CallReportTypeLoaded);

            MessagingCenter.Unsubscribe<SelectTypeOfCallReportViewModel, Account>(this, Message.AccountLoaded);

            MessagingCenter.Unsubscribe<SelectTypeOfCallReportViewModel, CallReportType>(this, Message.CallReportTypeLoaded);

            MessagingCenter.Unsubscribe<CustomerMasterViewModel, Account>(this, Message.AccountLoaded);
        }
        private void Subscribe()
        {
            MessagingCenter.Subscribe<AddNoteScreenViewModel, Note>(this, Message.NoteCreated, (sender, note) =>
            {
                Notes.Add(note);
                //toast the user
                int amount = Notes.Count();
                if (amount > 1)
                    DependencyService.Get<IToast>().WriteToast($"{amount} notes were created");
                else if (amount == 1)
                    DependencyService.Get<IToast>().WriteToast("A note was created");
            });

            MessagingCenter.Subscribe<AddPersonScreenViewModel, Person>(this, Message.PersonCreated, (sender, person) =>
            {
                Persons.Add(person);
                //toast the user
                int amount = Persons.Count();
                if (amount > 1)
                    DependencyService.Get<IToast>().WriteToast($"{amount} persons were created");
                else if (amount == 1)
                    DependencyService.Get<IToast>().WriteToast("A person was created");
            });

            MessagingCenter.Subscribe<SelectAccountforCallReportViewModel, Account>(this, Message.AccountLoaded, (sender, account) =>
            {
                Account = account;
                InitializeCreateCallReportScreenAsync();

            });

            MessagingCenter.Subscribe<SelectAccountforCallReportViewModel, CallReportType>(this, Message.CallReportTypeLoaded, (sender, callreporttype) =>
            {
                Type = callreporttype;
                InitializeCallReportQuestions();
            });
            MessagingCenter.Subscribe<SelectTypeOfCallReportViewModel, Account>(this, Message.AccountLoaded, (sender, account) =>
            {
                Account = account;
                InitializeCreateCallReportScreenAsync();

            });

            MessagingCenter.Subscribe<SelectTypeOfCallReportViewModel, CallReportType>(this, Message.CallReportTypeLoaded, (sender, callreporttype) =>
            {
                Type = callreporttype;
                InitializeCallReportQuestions();
            });
            MessagingCenter.Subscribe<CustomerMasterViewModel, Account>(this, Message.AccountLoaded, (sender, account) =>
            {
                Account = account;
                InitializeCreateCallReportScreenAsync();
            });
        }
        #endregion
    }
}
