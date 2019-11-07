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
        
        public CompliXpertAppMasterDetailPageMasterViewModel()
        {
            CreateCallReportTapped = false;
            //first list that everyone will see
            MenuItems = new List<CompliXpertAppMasterDetailPageMenuItem>()
            {
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 0, Title = "Customer List", ImageSource = "white_home.png", TargetType = typeof(CustomerListScreen) },
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 1, Title = "Create Call Report", ImageSource = null, TargetType = typeof(CreateCallReportScreen)},
                    new CompliXpertAppMasterDetailPageMenuItem{Id = 2, Title = "Add Prospect", ImageSource = null, TargetType = typeof(AddProspectScreen)},
                    new CompliXpertAppMasterDetailPageMenuItem{Id = 3, Title = "Add New Contact", ImageSource = null, TargetType = typeof(AddNewContactScreen) },
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 3, Title = "Calendar", ImageSource = null},
                    new CompliXpertAppMasterDetailPageMenuItem { Id = 4, Title = "History", ImageSource = null }
            };

            //second list that will contain all the customers in the system
            using (CompliXperAppContext context = new CompliXperAppContext())
            {
                List<Customer> customerList = context.Customer.ToList();
                customers = new List<CompliXpertAppMasterDetailPageMenuItem>();

                foreach(Customer customer in customerList)
                {
                    customers.Add(new CompliXpertAppMasterDetailPageMenuItem {Id = customer.CustomerNumber, Title = customer.CustomerName, TargetType = typeof(SelectAccountforCallReport), Color = "#a1a1a1" });
                }
            }
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
        public CompliXpertAppMasterDetailPageMenuItem MenuItem
        {
            get
            {
                return _menuItem;
            }
            set
            {
                _menuItem = value;
                if (MenuItem == null)
                    return;
                if (MenuItem.Id == 1)
                {
                    //to redo the list
                    RebuildMenu();
                }
                CallScreen(MenuItem);
                OnPropertyChanged();
            }
        }

        private void RebuildMenu()
        {
            List<CompliXpertAppMasterDetailPageMenuItem> items = new List<CompliXpertAppMasterDetailPageMenuItem>();
            //if create call report was tapped to see all customers
            if (CreateCallReportTapped == false)
            {
                //add the list of customers to the menu items          
                items.AddRange(MenuItems);
                items.InsertRange(2, customers);
                MenuItems = items;
                CreateCallReportTapped = true;
            }
            //action to be taken if create call report has already been tapped and you want to close the list
            else
            {
                items.AddRange(MenuItems);
                items.RemoveRange(2, customers.Count);
                MenuItems = items;
                CreateCallReportTapped = false;
            }
        }

        private async void CallScreen(CompliXpertAppMasterDetailPageMenuItem menuItem)
        {
            MenuItem = null;
            var page = (Page) Activator.CreateInstance(menuItem.TargetType);
            //if the user selects the customer list ensure that we are back to the root
            if (menuItem.TargetType.FullName.Equals(typeof(CustomerListScreen).FullName))
            {
                App.Current.MainPage = new NavigationPage(new CompliXpertAppMasterDetailPage());
                await App.Current.MainPage.Navigation.PopToRootAsync();
            }
            else if (menuItem.Id == 1)
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
            else
                await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage() { Detail = new NavigationPage(page) });
            
        }
    }
}
