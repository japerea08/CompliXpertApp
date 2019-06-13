using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class CustomerListScreenViewModel : AbstractNotifyPropertyChanged
    {
        private bool isBusy = false;
        private List<Customer> _accountList;

        public CustomerListScreenViewModel()
        {
            MessagingCenter.Subscribe<LoginViewModel, List<Customer>>(this, Message.AccountListLoaded, (sender, args) => 
            {
                Customers = args;
            });
            AddProspectCommand = new Command(AddProspect);
        }
        public async void AddProspect()
        {
            await App.Current.MainPage.Navigation.PushAsync(new AddProspectScreen());
        }

        //properties
        private Customer Customer { get; set; }
        public Command AddProspectCommand { get; }
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
        async void GetCustomerMaster(Customer customer)
        {
            CustomerSelected = null;
            IsBusy = true;
            await App.Current.MainPage.Navigation.PushAsync(new CustomerMaster());
            MessagingCenter.Send<CustomerListScreenViewModel, Customer>(this, Message.CustomerLoaded, customer);
        }
    }
}
