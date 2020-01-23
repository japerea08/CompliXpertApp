using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using System.Windows.Input;
using Xamarin.Forms;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Collections.Generic;

namespace CompliXpertApp.ViewModels
{
    class CustomerMasterViewModel : AbstractNotifyPropertyChanged
    {
        //attributes
        private readonly bool canTap = true;
        private Customer _customer;
        private bool _isBusy = false;
        private bool _isExpanded;
        private List<ObjectIndexer> objectIndexers;
        private ObjectIndexer oiAccountSelected;

        //constructor
        public CustomerMasterViewModel()
        {
            ExpandMenuCommand = new Command(ExpandMenu);
            CallCreateCallReportCommand = new Command(CallCreateCallReportScreenAsync);
            CallViewCallReportsCommand = new Command(CallViewCallReportsScreenAsync);
            //message from the prospect list screen
            MessagingCenter.Subscribe<ProspectListScreenViewModel, Customer>(this, Message.CustomerLoaded, (sender, args) =>
            {
                Customer = args;

                IndexedAccount = new List<ObjectIndexer>();
                int i = 0;
                //initialize the indexed list
                foreach (Account account in Customer.Account)
                {
                    IndexedAccount.Add(new ObjectIndexer()
                    {
                        Object = account,
                        Index = i,
                        IsVisible = true
                    });
                    i++;
                }
            });
            //this message is for the incoming
            MessagingCenter.Subscribe<CustomerListScreenViewModel, Customer>(this, Message.CustomerLoaded, (sender, args) =>
            {
                Customer = args;

                //dummy list
                List<ObjectIndexer> dummyindexer = new List<ObjectIndexer>();
                int i = 0;
                //initialize the indexed list
                foreach (Account account in Customer.Account)
                {
                    dummyindexer.Add(new ObjectIndexer()
                    {
                        Object = account,
                        Index = i,
                        IsVisible = true
                    });
                    i++;
                }
                IndexedAccount = dummyindexer;
            });
            //this message is for the return
            MessagingCenter.Subscribe<CreateCallReportViewModel, Customer>(this, Message.CallReportCreated, (sender, _account) =>
            {
                Customer = _account;

                IndexedAccount = new List<ObjectIndexer>();
                int i = 0;
                foreach (Account account in Customer.Account)
                {
                    IndexedAccount.Add(new ObjectIndexer()
                    {
                        Object = account,
                        Index = i,
                        IsVisible = true
                    });
                    i++;

                    if (account.AccountClass == null)
                    {
                        using (CompliXperAppContext context = new CompliXperAppContext())
                        {
                            account.AccountClass = (from _accountClass in context.AccountClasses
                                                    where _accountClass.AccountClassCode == account.AccountClassCode
                                                    select new AccountClass
                                                    {
                                                        AccountClassCode = _accountClass.AccountClassCode,
                                                        Description = _accountClass.Description
                                                    }).FirstOrDefault();
                        }
                    }
                }
            });
        }
        //goes to the create call report screen
        private async void CallCreateCallReportScreenAsync(object indexer)
        {
            await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage() { Detail = new NavigationPage(new SelectTypeOfCallReport())});
            MessagingCenter.Send<CustomerMasterViewModel, Account>(this, Message.AccountLoaded, (Account) ((ObjectIndexer) indexer).Object);
        }
        //goes to the account's call report list, which is expecting an account number 
        private async void CallViewCallReportsScreenAsync(object indexer)
        {
            Account account = (Account) ((ObjectIndexer) indexer).Object;

            await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage() { Detail = new NavigationPage(new CallReportsList())});
            IsBusy = false;
            MessagingCenter.Send<CustomerMasterViewModel, int>(this, Message.AccountNumber, account.AccountNumber);
        }
        //will take the number of index as a parameter
        private void ExpandMenu(object index)
        {
            ObjectIndexer o = (ObjectIndexer) index;
            //dummy list
            List<ObjectIndexer> dummy = new List<ObjectIndexer>();
            foreach (ObjectIndexer oz in objectIndexers)
            {
                dummy.Add(oz);
            }
            //check to see if menu is exanded
            if (objectIndexers[(int)o.Index].IsVisible == true)
                dummy[(int) o.Index].IsVisible = false;
            else
                dummy[(int) o.Index].IsVisible = true;


            IndexedAccount = dummy;
        }

        //properties
        public Account Account { get; set; }
        public List<ObjectIndexer> IndexedAccount
        {
            get
            {
                return objectIndexers;
            }
            set
            {
                objectIndexers = value;
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
        public ICommand ExpandMenuCommand { get; set; }
        public ICommand CallCreateCallReportCommand { get; set; }
        public ICommand CallViewCallReportsCommand { get; set; }
        public Customer Customer
        {
            get
            {
                return _customer;
            }
            set
            {
                _customer = value;
                OnPropertyChanged();
            }
        }     
    }
}
