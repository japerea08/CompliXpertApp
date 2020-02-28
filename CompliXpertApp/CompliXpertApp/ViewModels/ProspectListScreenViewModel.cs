using System;
using System.Collections.Generic;
using System.Text;
using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using System.Linq;
using Xamarin.Forms;
using CompliXpertApp.Views;

namespace CompliXpertApp.ViewModels
{
    class ProspectListScreenViewModel: AbstractNotifyPropertyChanged
    {
        private List<Customer> _prospectList;
        private bool _prospectsCreated;
        private Customer _prospectSelected;

        public ProspectListScreenViewModel()
        {
            //_prospectSelected = new Customer();
            _prospectList = new List<Customer>();
        }

        public Customer ProspectSelected
        {
            get
            {
                return _prospectSelected;
            }
            set
            {
                _prospectSelected = value;
                if (_prospectSelected == null)
                    return;
                GetCustomerMaster(_prospectSelected);
                OnPropertyChanged();

            }
        }

        public List<Customer> ProspectList
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
        public bool ProspectsCreated
        {
            get
            {
                return _prospectsCreated;
            }
            set
            {
                _prospectsCreated = value;
                OnPropertyChanged();
            }
        }

        async void GetCustomerMaster(Customer prospectSelected)
        {
            ProspectSelected = null;
            await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage() { Detail = new NavigationPage(new CustomerMaster()) });
            MessagingCenter.Send<ProspectListScreenViewModel, Customer>(this, Message.CustomerLoaded, prospectSelected);
        }
        public void InitializeData()
        {
            //check to see if there are any prospects that have been created
            using (CompliXperAppContext context = new CompliXperAppContext())
            {
                ProspectList = (from prospect in context.Customer
                               where prospect.CreatedOnMobile == true
                               select new Customer()
                               {
                                   Account = (from account in context.Account
                                              where account.CustomerNumber == prospect.CustomerNumber
                                              select new Account()
                                              {
                                                  AccountClassCode = account.AccountClassCode,
                                                  AccountNumber = account.AccountNumber,
                                                  AccountType = account.AccountType,
                                                  BusinessCode = account.BusinessCode,
                                                  CustomerNumber = account.CustomerNumber,
                                                  CustomerNumberNavigation = account.CustomerNumberNavigation,
                                                  IndustryCode = account.IndustryCode,
                                                  ProductCode = account.ProductCode,
                                                  CallReport = (from callReport in context.CallReport
                                                                             where callReport.AccountNumber == account.AccountNumber
                                                                             select callReport).ToList(),
                                                  AccountClass = (from accountClass in context.AccountClasses
                                                                  where accountClass.AccountClassCode == account.AccountClassCode
                                                                  select accountClass).FirstOrDefault(),
                                              }).ToList(),
                                   BusinessCode = prospect.BusinessCode,
                                   Citizenship = prospect.Citizenship,
                                   CountryofResidence = prospect.CountryofResidence,
                                   CreatedDate = prospect.CreatedDate,
                                   CreatedOnMobile = prospect.CreatedOnMobile,
                                   CustomerId = prospect.CustomerId,
                                   CustomerName = prospect.CustomerName,
                                   CustomerNumber = prospect.CustomerNumber,
                                   Email = prospect.Email,
                                   IndustryCode = prospect.IndustryCode,
                                   IsPEP = prospect.IsPEP,
                                   LegalType = prospect.LegalType,
                                   MailAddress = prospect.MailAddress
                               }).ToList();
            }
            if (_prospectList.Count == 0)
                ProspectsCreated = false;
            else
                ProspectsCreated = true;
        }
    }
}
