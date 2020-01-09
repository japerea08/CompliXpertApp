using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class CompliXpertAppMasterDetailPageMasterViewModel: AbstractNotifyPropertyChanged
    {
        private CompliXpertAppMasterDetailPageMenuItem _menuItem;
        private List<CompliXpertAppMasterDetailPageMenuItem> customers;
        private List<CompliXpertAppMasterDetailPageMenuItem> menuItems;
        private List<CompliXpertAppMasterDetailPageMenuItem> callReports;

        public CompliXpertAppMasterDetailPageMasterViewModel()
        {
            CreateCallReportTapped = false;
            //first list that everyone will see
            MenuItems = new List<CompliXpertAppMasterDetailPageMenuItem>()
            {
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 0, Title = "Customer List", ImageSource = null, TargetType = typeof(CustomerListScreen) },
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 1, Title = "Prospect List", ImageSource = null, TargetType = typeof(ProspectListScreen) },
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 2, Title = "Create Call Report", ImageSource = null, TargetType = typeof(CreateCallReportScreen)},
                    new CompliXpertAppMasterDetailPageMenuItem{Id = 3, Title = "Add Prospect", ImageSource = null, TargetType = typeof(AddProspectScreen)},
                    new CompliXpertAppMasterDetailPageMenuItem{Id = 4, Title = "Add New Contact", ImageSource = null, TargetType = typeof(AddNewContactScreen) },
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 5, Title = "Call Report List", ImageSource = null, TargetType = typeof(CallReportListScreen)},
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 6, Title = "Add Note/Add Attendee", ImageSource = null, TargetType = typeof(LoadingScreen)}
            };
        }

        //properties
        public List<CompliXpertAppMasterDetailPageMenuItem> MenuItems
        {
            get
            {
                return menuItems;
            }
            set
            {
                menuItems = value;
                OnPropertyChanged();
            }
        }
        public bool CreateCallReportTapped { get; set; }
        public bool AddNoteAttendeeTapped { get; set; }
        public CompliXpertAppMasterDetailPageMenuItem MenuItem
        {
            get
            {
                return _menuItem;
            }
            set
            {
                _menuItem = value;
                if (_menuItem == null || _menuItem.TargetType == null)
                    return;
                if (_menuItem.TargetType.FullName.Equals(typeof(CreateCallReportScreen).FullName ) == true)
                {
                    //to redo the list
                    RebuildMenuForCreateCallReport(_menuItem.Id);
                }
                else if(_menuItem.TargetType.FullName.Equals(typeof(LoadingScreen).FullName) == true)
                {
                    RebuildMenuForAddNoteAttendee(_menuItem.Id);
                }
                CallScreen(MenuItem);
                OnPropertyChanged();
            }
        }

        public void InitializeData()
        {
            using (CompliXperAppContext context = new CompliXperAppContext())
            {
                //gets all the customers in the DB
                List<Customer> customerList = context.Customer.ToList();

                //get only call reports that are created on mobile and initialize type correctly
                List<CallReport> callReportList = (from callreport in context.CallReport
                                                   where callreport.CreatedOnMobile == true
                                                   select new CallReport
                                                   {
                                                       AccountNumber = callreport.AccountNumber,
                                                       CallReportId = callreport.CallReportId,
                                                       CallReportType = (from callreporttype in context.CallReportType
                                                                         where callreporttype.Type == callreport.CallReportType
                                                                         select callreporttype.Description).FirstOrDefault(),
                                                       CallDate = callreport.CallDate
                                                    }).ToList();

                customers = new List<CompliXpertAppMasterDetailPageMenuItem>();
                callReports = new List<CompliXpertAppMasterDetailPageMenuItem>();

                foreach (Customer customer in customerList)
                {
                    customers.Add(new CompliXpertAppMasterDetailPageMenuItem { Id = customer.CustomerNumber, Title = customer.CustomerName, TargetType = typeof(SelectAccountforCallReport), Color = "#a1a1a1" });
                }

                foreach (CallReport callReport in callReportList)
                {
                    string customername = (from customer in context.Customer
                                          where customer.CustomerNumber == (from account in context.Account
                                                                            where account.AccountNumber == callReport.AccountNumber
                                                                            select account.CustomerNumber).FirstOrDefault()
                                          select customer.CustomerName).FirstOrDefault();
                    callReports.Add(new CompliXpertAppMasterDetailPageMenuItem { Id = callReport.CallReportId, Title = customername + ": " + callReport.CallReportType + " created " + callReport.Date, TargetType = typeof(CallReportDetailsScreen), Color = "#a1a1a1"});
                }
            }
        }
        private void RebuildMenuForCreateCallReport(int index)
        {
            List<CompliXpertAppMasterDetailPageMenuItem> items = new List<CompliXpertAppMasterDetailPageMenuItem>();
            //if create call report was tapped to see all customers
            if (CreateCallReportTapped == false)
            {
                //add the list of customers to the menu items          
                items.AddRange(MenuItems);
                if (AddNoteAttendeeTapped == true)
                {
                    int i;
                    foreach (CompliXpertAppMasterDetailPageMenuItem item in menuItems)
                    {
                        if (item.TargetType.FullName.Equals(typeof(LoadingScreen).FullName))
                        {
                            i = item.Id;
                            items.RemoveRange(i + 1, customers.Count);
                            break;
                        }
                    }
                    AddNoteAttendeeTapped = false;
                }
                items.InsertRange((index + 1), customers);
                MenuItems = items;
                CreateCallReportTapped = true;
            }
            //action to be taken if create call report has already been tapped and you want to close the list
            else
            {
                items.AddRange(MenuItems);
                items.RemoveRange((index + 1), customers.Count);
                MenuItems = items;
                CreateCallReportTapped = false;
            }
        }
        private void RebuildMenuForAddNoteAttendee(int index)
        {
            //logic if there are no Call Reports in the system
            if(callReports.Count == 0)
            {
                List<CompliXpertAppMasterDetailPageMenuItem> items = new List<CompliXpertAppMasterDetailPageMenuItem>();

                if (AddNoteAttendeeTapped == false)
                {
                    items.AddRange(MenuItems);
                    //check to see if createcallreport was tapped
                    if (CreateCallReportTapped == true)
                    {
                        int i;
                        foreach (CompliXpertAppMasterDetailPageMenuItem item in menuItems)
                        {
                            if (item.TargetType.FullName.Equals(typeof(CreateCallReportScreen).FullName))
                            {
                                i = item.Id;
                                items.RemoveRange(i + 1, customers.Count);
                                break;
                            }
                        }

                        CreateCallReportTapped = false;
                    }
                    callReports.Add(new CompliXpertAppMasterDetailPageMenuItem
                    {
                        Title = "No Call Reports Created",
                        Color = "#a1a1a1"
                    });
                    items.InsertRange((index + 1), callReports);
                    MenuItems = items;
                    AddNoteAttendeeTapped = true;
                }
                else
                {
                    items.AddRange(MenuItems);
                    items.RemoveRange(index + 1, callReports.Count);
                    MenuItems = items;
                    AddNoteAttendeeTapped = false;
                }
            }
            else
            {
                List<CompliXpertAppMasterDetailPageMenuItem> items = new List<CompliXpertAppMasterDetailPageMenuItem>();

                if (AddNoteAttendeeTapped == false)
                {
                    items.AddRange(MenuItems);
                    //check to see if createcallreport was tapped
                    if (CreateCallReportTapped == true)
                    {
                        int i;
                        foreach (CompliXpertAppMasterDetailPageMenuItem item in menuItems)
                        {
                            if (item.TargetType.FullName.Equals(typeof(CreateCallReportScreen).FullName))
                            {
                                i = item.Id;
                                items.RemoveRange(i + 1, customers.Count);
                                break;
                            }
                        }

                        CreateCallReportTapped = false;
                    }
                    items.InsertRange((index + 1), callReports);
                    MenuItems = items;
                    AddNoteAttendeeTapped = true;
                }
                else
                {
                    items.AddRange(MenuItems);
                    items.RemoveRange(index + 1, callReports.Count);
                    MenuItems = items;
                    AddNoteAttendeeTapped = false;
                }
            }
            
        }
        public void RebuildOriginalMenu()
        {
            MenuItems = new List<CompliXpertAppMasterDetailPageMenuItem>()
            {
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 0, Title = "Customer List", ImageSource = null, TargetType = typeof(CustomerListScreen) },
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 1, Title = "Prospect List", ImageSource = null, TargetType = typeof(ProspectListScreen) },
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 2, Title = "Create Call Report", ImageSource = null, TargetType = typeof(CreateCallReportScreen)},
                    new CompliXpertAppMasterDetailPageMenuItem{Id = 3, Title = "Add Prospect", ImageSource = null, TargetType = typeof(AddProspectScreen)},
                    new CompliXpertAppMasterDetailPageMenuItem{Id = 4, Title = "Add New Contact", ImageSource = null, TargetType = typeof(AddNewContactScreen) },
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 5, Title = "Call Report List", ImageSource = null, TargetType = typeof(CallReportListScreen)},
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 6, Title = "Add Note/Add Attendee", ImageSource = null, TargetType = typeof(LoadingScreen)},
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 7, Title = "History", ImageSource = null }
            };
            CreateCallReportTapped = false;
            AddNoteAttendeeTapped = false;
        }

        private async void CallScreen(CompliXpertAppMasterDetailPageMenuItem menuItem)
        {
            MenuItem = null;
            var page = (Page) Activator.CreateInstance(menuItem.TargetType);

            if (menuItem.TargetType.FullName.Equals(typeof(CustomerListScreen).FullName))
            {
                App.Current.MainPage = new NavigationPage(new CompliXpertAppMasterDetailPage());
                await App.Current.MainPage.Navigation.PopToRootAsync();
            }
            else if (menuItem.TargetType.FullName.Equals(typeof(CreateCallReportScreen).FullName) == true || menuItem.TargetType.FullName.Equals(typeof(LoadingScreen).FullName) == true)
            {
                return;
            }
            //check target type for selectaccountforcallreport
            else if (menuItem.TargetType.FullName.Equals(typeof(SelectAccountforCallReport).FullName))
            {
                await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage() { Detail = new NavigationPage(page) });
                //use messaging center here to send the customer for CreateCallReport
                MessagingCenter.Send<CompliXpertAppMasterDetailPageMasterViewModel, int>(this, Message.CustomerIdAttached, menuItem.Id);
            }
            //check to see if a note or attendee wants to be added
            else if (menuItem.TargetType.FullName.Equals(typeof(CallReportDetailsScreen).FullName))
            {
                using (CompliXperAppContext context = new CompliXperAppContext())
                {
                    CallReport callReport =  (from callreport in context.CallReport
                                             where callreport.CallReportId == menuItem.Id
                                             select new CallReport
                                             {
                                                 AccountNumber = callreport.AccountNumber,
                                                 AccountNumberNavigation = callreport.AccountNumberNavigation,
                                                 ApprovedBy = callreport.ApprovedBy,
                                                 ApprovedDate = callreport.ApprovedDate,
                                                 CallDate = callreport.CallDate,
                                                 CallReportId = callreport.CallReportId,
                                                 CallReportType = callreport.CallReportType,
                                                 CreatedOnMobile = callreport.CreatedOnMobile,
                                                 LastUpdated = callreport.LastUpdated,
                                                 LastUpdatedDate = callreport.LastUpdatedDate,
                                                 Notes = (from notes in context.Notes
                                                          where notes.NoteId == callreport.CallReportId
                                                          select notes).ToList(),
                                                 Officer = callreport.Officer,
                                                 Position = callreport.Position,
                                                 Reason = callreport.Reason,
                                                 Reference = callreport.Reference,
                                                 Responses = (from responses in context.CallReportResponse
                                                              where responses.CallReportId == callreport.CallReportId
                                                              select responses).ToList()
                                             }).FirstOrDefault();

                    await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage() { Detail = new NavigationPage(page) });
                    //use messaging center here to send the customer for CreateCallReport
                    MessagingCenter.Send<CompliXpertAppMasterDetailPageMasterViewModel, CallReport>(this, Message.CallReportLoaded, callReport);
                }
            }
            //this is the condition for any other menu choice
            else
            {
                await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage() { Detail = new NavigationPage(page) });

            }
             
        }
    }
}
