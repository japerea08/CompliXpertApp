using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class AddProspectScreenViewModel : AbstractNotifyPropertyChanged
    {
        private bool canAdd = false;
        private string customerName = null;
        private string legalType = null;
        private string accountClass = null;
        //constructor
        public AddProspectScreenViewModel()
        {
            Prospect = new Customer();
            ProspectAccount = new Account();
            CustomerNameEntered = false;
            AddProspectCommand = new Command(async () => await AddProspectAsync(), () => canAdd);
            //get a list of countries && account class
            using (var context = new CompliXperAppContext())
            {
                Countries = context.Countries.ToList();
            }
        }
        //properties
        public Account ProspectAccount { get; set; }
        public bool CustomerNameEntered { get; set; }
        public List<AccountClass> AccountClasses { get; set; }
        public List<Country> Countries { get; set; }
        public Country Citizenship { get; set; }
        public Country CountryofResidence { get; set; }
        public Customer Prospect { get; set; }
        public string CustomerName
        {
            get
            {
                return customerName;
            }
            set
            {
                customerName = value;
                if(String.IsNullOrEmpty(customerName) == false || String.IsNullOrWhiteSpace(customerName) == false)
                {
                    CustomerNameEntered = true;
                }
                if (String.IsNullOrEmpty(customerName) == false && String.IsNullOrEmpty(legalType) == false && String.IsNullOrWhiteSpace(legalType) == false && String.IsNullOrWhiteSpace(customerName) == false)
                {
                    CanAdd(true);
                }
                else
                    CanAdd(false);
            }
        }
        public string AccountClass
        {
            get
            {
                return accountClass;
            }
            set
            {
                accountClass = value;
                if (String.IsNullOrEmpty(accountClass) == false && String.IsNullOrEmpty(legalType) == false && String.IsNullOrWhiteSpace(legalType) == false && String.IsNullOrWhiteSpace(customerName) == false && String.IsNullOrEmpty(customerName))
                {
                    CanAdd(true);
                }
                else
                    CanAdd(false);
            }
        }
        public string LegalType
        {
            get
            {
                return legalType;
            }
            set
            {
                legalType = value;

                if (String.IsNullOrEmpty(customerName) == false && String.IsNullOrEmpty(legalType) == false && String.IsNullOrWhiteSpace(legalType) == false && String.IsNullOrWhiteSpace(customerName) == false)
                {
                    CustomerNameEntered = true;
                    CanAdd(true);
                }
                else
                    CanAdd(false);
            }
        }
        public ICommand AddProspectCommand { get; private set; }
        //methods
        void CanAdd(bool value)
        {
            canAdd = value;
            ((Command) AddProspectCommand).ChangeCanExecute();
        }
        async Task AddProspectAsync()
        {
            Prospect.CustomerName = CustomerName;
            Prospect.LegalType = LegalType;
            Prospect.CreatedOnMobile = true;
            //Prospect.Citizenship = Citizenship?.; //now a country object
            //Prospect.CountryofResidence = CountryofResidence?.Code;
            //add the new propsect to the DB here
            if(Prospect != null)
            {
                Random random = new Random();
                using (CompliXperAppContext context = new CompliXperAppContext())
                {
                    //get the latest Customer Number
                    int lastCustomerNumber = (int)await context.Customer
                                                    .OrderByDescending(c => c.CustomerId)
                                                    .Select(c => c.CustomerNumber).LastOrDefaultAsync();

                    Prospect.CustomerNumber = random.Next(lastCustomerNumber, 999999999);
                    Prospect.CustomerId = Prospect.CustomerNumber;
                    context.Add(Prospect);
                    try
                    {
                        await context.SaveChangesAsync();
                        await App.Current.MainPage.Navigation.PopAsync();
                    }
                    catch (DbUpdateException e)
                    {
                        SqliteException ex = (SqliteException) e.InnerException;
                        Console.WriteLine(ex.SqliteErrorCode);
                        if(ex.SqliteErrorCode == 19)
                        {
                            //try another random number via recursion
                            await AddProspectAsync();
                        }
                    }
                }
            }
        }
    }
}
