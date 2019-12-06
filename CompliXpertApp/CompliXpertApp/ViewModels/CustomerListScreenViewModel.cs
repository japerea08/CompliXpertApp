using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class CustomerListScreenViewModel : AbstractNotifyPropertyChanged
    {
        private bool isBusy = false;
        private List<Customer> _accountList;
        private List<Customer> _prospectList;
        private List<NewContact> _newContactList;
        private List<ApplicationEntityGroup> listOfEntities;
        private bool canDownload = false;
        private bool createdOnMobile = false;
        private bool prospectCreated = false;
        private bool newContactAdded;

        public CustomerListScreenViewModel()
        {
            SendNewDataCommand = new Command(async () => await DownloadCallReportsAsync(), () => canDownload);
        }
        //properties
        public bool CreatedOnMobile
        {
            get
            {
                return createdOnMobile;
            }
            set
            {
                createdOnMobile = value;
                OnPropertyChanged();
            }
        }
        public bool NewContactAdded
        {
            get
            {
                return newContactAdded;
            }
            set
            {
                newContactAdded = value;
                OnPropertyChanged();
            }
        }
        public bool ProspectCreated
        {
            get
            {
                return prospectCreated;
            }
            set
            {
                prospectCreated = value;
                OnPropertyChanged();
            }
        }
        private Object Entity { get; set; }
        void CanDownload(bool value)
        {
            canDownload = value;
            ((Command) SendNewDataCommand).ChangeCanExecute();
        }
        public Command SendNewDataCommand { get; }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        public List<NewContact> NewContactList
        {
            get
            {
                return _newContactList;
            }
            set
            {
                _newContactList = value;
                OnPropertyChanged();
            }
        }
        public List<Customer> Prospects
        {
            get
            {
                return _prospectList;
            }
            set
            {
                _prospectList = value;
                OnPropertyChanged();
            }
        }
        public List<Customer> Customers
        {
            get
            {
                return _accountList;
            }
            set
            {
                _accountList = value;
                OnPropertyChanged();
            }
        }
        public Object EntitySelected
        {
            get
            {
                return Entity;
            }
            set
            {
                Entity = value;
                if (Entity == null)
                    return;
                GetEntityMaster(Entity);
                OnPropertyChanged();
            }
        }

        async void GetEntityMaster(object entity)
        {
            if(entity.GetType() == typeof(Customer))
            {
                EntitySelected = null;
                await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage() { Detail = new NavigationPage(new CustomerMaster()) });
                MessagingCenter.Send<CustomerListScreenViewModel, Customer>(this, Message.CustomerLoaded, (Customer)entity);
            }
            else if (entity.GetType() == typeof(NewContact))
            {
                EntitySelected = null;
                await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage() { Detail = new NavigationPage(new NewContactMaster()) });
                MessagingCenter.Send<CustomerListScreenViewModel, NewContact>(this, Message.NewContactLoaded, (NewContact) entity);
            }
        }
        public List<ApplicationEntityGroup> Group
        {
            get
            {
                return listOfEntities;
            }
            set
            {
                listOfEntities = value;
                OnPropertyChanged();
            }
        }
        //methods
        public void CreateGroups()
        {
            var customerList = new ApplicationEntityGroup();
            var groupList = new List<ApplicationEntityGroup>();

            if (_accountList.Any() == true)
            {
                
                customerList.AddRange(_accountList);
                customerList.Heading = "Customer List";
                groupList.Add(customerList);
            }

            var prospectList = new ApplicationEntityGroup();
            if (_prospectList.Any() == true)
            {
                
                prospectList.AddRange(_prospectList);
                prospectList.Heading = "Prospect List";
                groupList.Add(prospectList);
            }

            var newContactList = new ApplicationEntityGroup();
            if (_newContactList.Any() == true)
            {
                newContactList.AddRange(_newContactList);
                newContactList.Heading = "New Contacts";
                groupList.Add(newContactList);
            }
                   

            Group = groupList;

        }
        //will check to see if new call reports were created or a prospect
        public void CheckForNewData()
        {
            using (var context = new CompliXperAppContext())
            {
                //get all the Call Reports in the system that were created on the mobile device
                var callReports = context.CallReport
                    .Where(report => report.CreatedOnMobile == true)
                    .ToList();
                //get all Customers that were created on the mobile devices
                var prospectList = context.Customer
                    .Where(customer => customer.CreatedOnMobile == true)
                    .ToList();
                if (prospectList.Count > 0)
                {
                    ProspectCreated = true;
                    CanDownload(true);
                }

                if(_newContactList.Count > 0)
                {
                    NewContactAdded = true;
                    CanDownload(true);
                }
                    
                if (callReports.Count > 0 )
                {
                    CanDownload(true);
                    CreatedOnMobile = true;
                }
                else
                {
                    CanDownload(false);
                    CreatedOnMobile = false;
                }
            }
        }
        //download all the new data that has been added...phase 1(just new Call Reports)
        //will become a Http Push request
        public async Task DownloadCallReportsAsync()
        {
            //ask the user if he wants to download
            if (await App.Current.MainPage.DisplayAlert("Downloading all New Data will remove the data from the CompliXpert App.", "This action cannot be undone.", "Yes", "Cancel"))
            {
                IsBusy = true;
                using (var context = new CompliXperAppContext())
                {
                    //get all the Call Reports in the system that were created on the mobile device
                    var callReports = await context.CallReport
                        .Where(report => report.CreatedOnMobile == true)
                        .ToListAsync();
                    
                    //then get all the accounts formed associated with the newly created Call Reports
                    List<Account> accounts = new List<Account>();
                    foreach(CallReport report in callReports)
                    {
                        Account account = await (from _account in context.Account
                                            where _account.AccountNumber == report.AccountNumber
                                            select new Account
                                            {
                                                AccountNumber = _account.AccountNumber,
                                                AccountType = _account.AccountType,
                                                AccountClassCode = _account.AccountClassCode,
                                                CustomerNumber = _account.CustomerNumber,
                                            }).FirstOrDefaultAsync();

                        account.CallReport.Add(report);
                        //accounts.Add(await context.Account
                        //    .Where(account => account.AccountNumber == report.AccountNumber)
                        //    .FirstOrDefaultAsync());
                    }

                    //get the customers associated with those accounts


                    //make into a Json variable
                    string callReportsJson = JsonConvert.SerializeObject(callReports);

                    //save the json to text file for testing purposes
                    JsonConvert.DeserializeObject<List<Customer>>(await DependencyService.Get<IRWExternalStorage>().ReadFileAsync());
                    await DependencyService.Get<IRWExternalStorage>().WriteFileAsync(callReportsJson);
                    IsBusy = false;
                    //drop all data from the sqlite database
                    //context.Account.RemoveRange(context.Account.ToArray());

                }
            }
        }
    }
}
