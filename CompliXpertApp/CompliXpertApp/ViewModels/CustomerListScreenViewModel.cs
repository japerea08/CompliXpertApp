﻿using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        private bool canDownload = false;

        public CustomerListScreenViewModel()
        {
            MessagingCenter.Subscribe<LoginViewModel, List<Customer>>(this, Message.AccountListLoaded, (sender, args) =>
            {
                Customers = args;
            });
            AddProspectCommand = new Command(AddProspect);
            DownloadCallReportsCommand = new Command(async () => await DownloadCallReportsAsync(), () => canDownload);
            //CheckForNewReports();
        }
        public async void AddProspect()
        {
            await App.Current.MainPage.Navigation.PushAsync(new AddProspectScreen());
        }

        //properties
        private Customer Customer { get; set; }
        void CanDownload(bool value)
        {
            canDownload = value;
            ((Command) DownloadCallReportsCommand).ChangeCanExecute();
        }
        public Command AddProspectCommand { get;}
        public Command DownloadCallReportsCommand { get; }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
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
        public Customer CustomerSelected
        {
            get
            {
                return Customer;
            }
            set
            {               
                Customer = value;
                if (Customer == null)
                    return;
                GetCustomerMaster(Customer);
                OnPropertyChanged();
            }
        }
        //methods
        public void CheckForNewReports()
        {
            using (var context = new CompliXperAppContext())
            {
                //get all the Call Reports in the system that were created on the mobile device
                var callReports = context.CallReport
                    .Where(report => report.CreatedOnMobile == true)
                    .ToList();
                if (callReports.Count > 0)
                {
                    CanDownload(true);
                }
                else
                {
                    CanDownload(false);
                }
            }
        }
        async void GetCustomerMaster(Customer customer)
        {
            CustomerSelected = null;
            await App.Current.MainPage.Navigation.PushAsync(new CustomerMaster());
            MessagingCenter.Send<CustomerListScreenViewModel, Customer>(this, Message.CustomerLoaded, customer);
        }

        //download all the new data that has been added...phase 1(just new Call Reports)
        //will become a Http Push request
        public async Task DownloadCallReportsAsync()
        {
            //ask the user if he wants to download
            if (await App.Current.MainPage.DisplayAlert("Downloading all Call Reports will remove the data from the CompliXpert App.", "This action cannot be undone.", "Yes", "Cancel"))
            {
                IsBusy = true;
                using (var context = new CompliXperAppContext())
                {
                    //get all the Call Reports in the system that were created on the mobile device
                    var callReports = await context.CallReport
                        .Where(report => report.CreatedOnMobile == true)
                        .ToListAsync();

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
