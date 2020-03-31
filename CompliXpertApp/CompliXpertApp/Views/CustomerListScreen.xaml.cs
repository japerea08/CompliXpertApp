using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;

namespace CompliXpertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomerListScreen : ContentPage
	{
        CustomerListScreenViewModel listScreenViewModel;
		public CustomerListScreen ()
		{
            listScreenViewModel = new CustomerListScreenViewModel();
            NavigationPage.SetTitleIconImageSource(this, "compli_logo_xsmall.png");
            InitializeComponent ();
            BindingContext = listScreenViewModel;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<LoginViewModel, List<Account>>(listScreenViewModel, Message.AccountListLoaded);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //make a call to the local sqlite DB to populaye the list of customers
            using (CompliXperAppContext context = new CompliXperAppContext())
            {
                listScreenViewModel.Customers = (from customer in context.Customer
                             where customer.CreatedOnMobile == false
                             select new Customer()
                             {
                                 CustomerNumber = customer.CustomerNumber,
                                 CustomerId = customer.CustomerId,
                                 CustomerName = customer.CustomerName,
                                 LegalType = customer.LegalType,
                                 CreatedOnMobile = customer.CreatedOnMobile,
                                 IsPEP = customer.IsPEP,
                                 MailAddress = customer.MailAddress,
                                 Citizenship = customer.Citizenship,
                                 CountryofResidence = customer.CountryofResidence,
                                 Email = customer.Email,
                                 BusinessCode = customer.BusinessCode,
                                 IndustryCode = customer.IndustryCode,
                                 Account = (from account in context.Account
                                            where account.CustomerNumber == customer.CustomerNumber
                                            select new Account()
                                            {
                                                AccountNumber = account.AccountNumber,
                                                AccountType = account.AccountType,
                                                CustomerNumber = account.CustomerNumber,
                                                AccountClassCode = account.AccountClassCode,
                                                BusinessCode = account.BusinessCode,
                                                IndustryCode = account.IndustryCode,
                                                ProductCode = account.ProductCode,
                                                AccountClass = (from accountClass in context.AccountClasses
                                                                where accountClass.AccountClassCode == account.AccountClassCode
                                                                select accountClass).FirstOrDefault(),
                                                CallReport = (from callReport in context.CallReport
                                                              where callReport.AccountNumber == account.AccountNumber
                                                              select callReport).ToList()
                                            }).ToList()
                             }).ToList();

                listScreenViewModel.NewContactList = context.NewContacts.ToList();

                listScreenViewModel.Prospects = (from customer in context.Customer
                             where customer.CreatedOnMobile == true
                             select new Customer()
                             {
                                 CustomerNumber = customer.CustomerNumber,
                                 CustomerId = customer.CustomerId,
                                 CustomerName = customer.CustomerName,
                                 LegalType = customer.LegalType,
                                 CreatedOnMobile = customer.CreatedOnMobile,
                                 IsPEP = customer.IsPEP,
                                 MailAddress = customer.MailAddress,
                                 Citizenship = customer.Citizenship,
                                 CountryofResidence = customer.CountryofResidence,
                                 Email = customer.Email,
                                 BusinessCode = customer.BusinessCode,
                                 IndustryCode = customer.IndustryCode,
                                 Account = (from account in context.Account
                                            where account.CustomerNumber == customer.CustomerNumber
                                            select new Account()
                                            {
                                                AccountNumber = account.AccountNumber,
                                                AccountType = account.AccountType,
                                                CustomerNumber = account.CustomerNumber,
                                                AccountClassCode = account.AccountClassCode,
                                                BusinessCode = account.BusinessCode,
                                                IndustryCode = account.IndustryCode,
                                                ProductCode = account.ProductCode,
                                                AccountClass = (from accountClass in context.AccountClasses
                                                                where accountClass.AccountClassCode == account.AccountClassCode
                                                                select accountClass).FirstOrDefault(),
                                                CallReport = (from callReport in context.CallReport
                                                              where callReport.AccountNumber == account.AccountNumber
                                                              select callReport).ToList()
                                            }).ToList()
                             }).ToList();
            }
            listScreenViewModel.CreateGroups();
            listScreenViewModel.CheckForNewData();
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await App.Current.MainPage.DisplayAlert("Are you sure you want to Sign Off?", "", "Yes", "No"))
                {
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            });
            return true;

        }
    }
}