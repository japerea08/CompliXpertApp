using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CallReportDetailsScreen : ContentPage
	{
        private CallReportDetailsViewModel viewModel;
        public CallReportDetailsScreen()
        {
            NavigationPage.SetTitleIconImageSource(this, "compli_logo_xsmall.png");
            InitializeComponent();
            viewModel = new CallReportDetailsViewModel();
            BindingContext = viewModel;
        }
        protected override void OnAppearing()
        {
            MessagingCenter.Send(this, Message.AllowLandscapePortrait);
            //DB needs to be checked everytime
            MessagingCenter.Subscribe<CallReportListScreenViewModel, CallReport>(this, Message.CallReportLoaded, async (sender, _report) =>
            {
                viewModel.Report = _report;
                viewModel.CreatedOnMobile = viewModel.Report.CreatedOnMobile;

                //get the questions
                using (var context = new CompliXperAppContext())
                {
                    viewModel.ReportType = (from r in context.CallReportType
                                  where r.Type == viewModel.Report.CallReportType
                                  select r.Description).SingleOrDefault();

                    viewModel.Report.Notes = (from notes in context.Notes
                                    where notes.CallReportId == viewModel.Report.CallReportId
                                    select notes).ToList();

                    List<CallReportQuestions> Questions = await (
                        from _q in context.CallReportQuestions
                        where _q.Type == viewModel.Report.CallReportType
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
                        foreach (var response in viewModel.Report.Responses)
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
                    viewModel.QuestionandResponses = _qr;
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
                MessagingCenter.Send<CallReportDetailsViewModel, int?>(viewModel, Message.AccountNumber, viewModel.Report.AccountNumber);
            });

            MessagingCenter.Subscribe<CallReportListViewModel, CallReport>(this, Message.CallReportLoaded, async (sender, _report) =>
            {
                viewModel.Report = _report;
                viewModel.CreatedOnMobile = viewModel.Report.CreatedOnMobile;

                //get the questions
                using (var context = new CompliXperAppContext())
                {
                    viewModel.ReportType = (from r in context.CallReportType
                                  where r.Type == viewModel.Report.CallReportType
                                  select r.Description).SingleOrDefault();

                    viewModel.Report.Notes = (from notes in context.Notes
                                    where notes.CallReportId == viewModel.Report.CallReportId
                                    select notes).ToList();

                    List<CallReportQuestions> Questions = await (
                        from _q in context.CallReportQuestions
                        where _q.Type == viewModel.Report.CallReportType
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
                        foreach (var response in viewModel.Report.Responses)
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
                    viewModel.QuestionandResponses = _qr;
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
                MessagingCenter.Send<CallReportDetailsViewModel, int?>(viewModel, Message.AccountNumber, viewModel.Report.AccountNumber);
            });
            MessagingCenter.Subscribe<CompliXpertAppMasterDetailPageMasterViewModel, CallReport>(this, Message.CallReportLoaded, async (sender, _report) =>
            {
                viewModel.Report = _report;
                viewModel.CreatedOnMobile = viewModel.Report.CreatedOnMobile;

                //get the questions
                using (var context = new CompliXperAppContext())
                {
                    viewModel.ReportType = (from r in context.CallReportType
                                  where r.Type == viewModel.Report.CallReportType
                                  select r.Description).SingleOrDefault();

                    viewModel.Report.Notes = (from notes in context.Notes
                                    where notes.CallReportId == viewModel.Report.CallReportId
                                    select notes).ToList();

                    List<CallReportQuestions> Questions = await (
                        from _q in context.CallReportQuestions
                        where _q.Type == viewModel.Report.CallReportType
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
                        foreach (var response in viewModel.Report.Responses)
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
                    viewModel.QuestionandResponses = _qr;
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
                MessagingCenter.Send<CallReportDetailsViewModel, int?>(viewModel, Message.AccountNumber, viewModel.Report.AccountNumber);
            });
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            MessagingCenter.Send(this, Message.PreventLandscape);
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<CallReportListViewModel>(viewModel, Message.CallReportLoaded);
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await App.Current.MainPage.DisplayAlert("Are you sure you want to return to Customer List?", "Any new information entered will be lost.", "Yes", "No"))
                {
                    await App.Current.MainPage.Navigation.PopToRootAsync();
                }
            });
            return true;

        }
    }
}