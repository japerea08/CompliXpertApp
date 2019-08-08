using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
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
        private List<Country> countries;
        //constructor
        public AddProspectScreenViewModel()
        {
            Prospect = new Customer();
            AddProspectCommand = new Command(async () => await AddProspectAsync(), () => canAdd);
            //get a list of countries
            using (var context = new CompliXperAppContext())
            {
                Countries = context.Countries.ToList();
            }
        }
        //properties
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
                if (String.IsNullOrEmpty(customerName) == false && String.IsNullOrEmpty(legalType) == false && String.IsNullOrWhiteSpace(legalType) == false && String.IsNullOrWhiteSpace(customerName) == false)
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
            Prospect.Citizenship = Citizenship.Code;
            Prospect.CountryofResidence = CountryofResidence.Code;
            //add the new propsect to the DB here
            if(Prospect != null)
            {
                using (CompliXperAppContext context = new CompliXperAppContext())
                {
                    //get the latest Customer Number
                    int lastCustomerNumber = await context.Customer
                                                    .OrderByDescending(c => c.CustomerId)
                                                    .Select(c => c.CustomerId).LastOrDefaultAsync();
                    Prospect.CustomerNumber = lastCustomerNumber + 999900001;
                    Prospect.CustomerId = Prospect.CustomerNumber;
                    context.Add(Prospect);
                    try
                    {
                        await context.SaveChangesAsync();
                    }
                    catch (DbUpdateException e)
                    {
                        Console.WriteLine(e.InnerException);
                    }
                }
                await App.Current.MainPage.Navigation.PopAsync();
            }
        }
    }
}
