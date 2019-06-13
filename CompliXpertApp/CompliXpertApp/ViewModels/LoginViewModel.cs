using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CompliXpertApp.ViewModels
{
    class LoginViewModel : AbstractNotifyPropertyChanged
    {
        private readonly string userNamePlaceholder = "Enter Username";
        private readonly string passwordPlaceholder = "Enter Password";
        private bool isBusy = false;
        private bool canLogin = true;
        private Color usernamePlaceholderColor = Color.Default;
        private Color passwordPlaceholderColor = Color.Default;

        public LoginViewModel()
        {
            //object for sigining in
            User = new User();
            CheckLoginCredentialsCommand = new Command(async () => await CheckLoginCredentialsAsync(), () => canLogin);
        }

        //properties
        public User User { get; set; }
        public Color UsernamePlaceholderColor
        {
            get { return usernamePlaceholderColor; }
            set
            {
                usernamePlaceholderColor = value;
                OnPropertyChanged();
            }
        }
        public Color PasswordPlaceholderColor
        {
            get { return passwordPlaceholderColor; }
            set
            {
                passwordPlaceholderColor = value;
                OnPropertyChanged();
            }
        }
        public string UsernamePlaceholder
        {
            get { return userNamePlaceholder; }
        }
        public string PasswordPlaceholder
        {
            get { return passwordPlaceholder; }
        }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        public string Username
        {
            get { return User.UserName; }
            set { User.UserName = value; }
        }
        public string Password
        {
            get { return User.Password; }
            set { User.Password = value; }
        }

        //methods
        //databind to this command property
        public ICommand CheckLoginCredentialsCommand { get; private set; }

        void CanAttemptLogin(bool value)
        {
            canLogin = value;
            ((Command) CheckLoginCredentialsCommand).ChangeCanExecute();
        }
        async Task CheckLoginCredentialsAsync()
        {
            
            if (String.IsNullOrEmpty(Username) == false && String.IsNullOrEmpty(Password) == false)
            {
                CanAttemptLogin(false);
                IsBusy = true;
                //will only run if the DB has records
                if (await Task.Run(() => DBContainsRecords()))
                {
                    //get everything from DB
                    List<Customer> customers = await Task.Run(() => GetCustomersAsync());
                    IsBusy = false;
                    await App.Current.MainPage.Navigation.PushAsync(new CustomerListScreen());
                    MessagingCenter.Send<LoginViewModel, List<Customer>>(this, Message.AccountListLoaded, customers);
                    CanAttemptLogin(true);
                }
                else
                {
                    //grab accounts from file
                    List<Customer> customers = await Task.Run(() => GetJsonAsync());
                    //add to the database
                    await Task.Run(() => AddCustomersAsync(customers));
                    customers = await Task.Run(() => GetCustomersAsync());
                    IsBusy = false;
                    await App.Current.MainPage.Navigation.PushAsync(new CustomerListScreen());
                    //pass our list
                    MessagingCenter.Send<LoginViewModel, List<Customer>>(this, Message.AccountListLoaded, customers);
                    CanAttemptLogin(true);
                }                
            }
            else
            {
                //change entry color to red
                if (String.IsNullOrEmpty(User.UserName) == true)
                    UsernamePlaceholderColor = Color.Red;
                if (String.IsNullOrEmpty(User.Password) == true)
                    PasswordPlaceholderColor = Color.Red;
            }
        }
        public bool DBContainsRecords()
        {
            using (var context = new CompliXperAppContext())
            {
                if (context.Customer.Any())
                    return true;
                else
                {
                    //context.Database.EnsureDeleted();
                    return false;
                }
            }
        }
        public async Task<List<Customer>> GetCustomersAsync()
        {
            using (var context = new CompliXperAppContext())
            {
                //get all customers 
                var customers = context.Customer;
                foreach (Customer customer in customers)
                {
                    customer.Account = await
                        (
                            from _account in context.Account
                            where _account.CustomerNumber == customer.CustomerNumber
                            select new Account
                            {
                                AccountNumber = _account.AccountNumber,
                                AccountType = _account.AccountType,
                                AccountClass = _account.AccountClass,
                                CustomerNumber = _account.CustomerNumber,
                            }
                        ).ToListAsync();
                    foreach (Account account in customer.Account)
                    {
                        account.CallReport = await (
                            from _report in context.CallReport
                            where _report.AccountNumber == account.AccountNumber
                            select new CallReport
                            {
                                CallReportId = _report.CallReportId,
                                Purpose = _report.Purpose,
                                OfficerComments = _report.OfficerComments,
                                OtherComments = _report.OtherComments,
                                CustomerComments = _report.CustomerComments,
                                AccountNumber = _report.AccountNumber,
                                Officer = _report.Officer,
                                Position = _report.Position,
                                Reason = _report.Reason,
                                CallDate = _report.CallDate.Date,
                                Status = _report.Status,
                                Reference = _report.Reference,
                                ApprovedBy = _report.ApprovedBy,
                                ApprovedDate = _report.ApprovedDate,
                                Nationality = _report.Nationality,
                                ReasonforAlert = _report.ReasonforAlert,
                                CustomerResponse = _report.CustomerResponse,
                                CreatedOnMobile = _report.CreatedOnMobile,
                                LastUpdated = _report.LastUpdated
                            }
                        ).ToListAsync();
                    }  
                }
                return await customers.ToListAsync();
            }
        }
        public async Task AddCustomersAsync(List<Customer> customers)
        {
            using (var context = new CompliXperAppContext())
            {
                context.Customer.AddRange(customers);
                List<Account> accounts = new List<Account>();
                List<CallReport> callreports = new List<CallReport>();
                foreach (Customer customer in customers)
                {
                    accounts = customer.Account.ToList();
                    foreach(Account account in customer.Account)
                    {
                        callreports = account.CallReport.ToList();
                    }
                }
                context.Account.AddRange(accounts);
                context.CallReport.AddRange(callreports);
                await context.SaveChangesAsync();
            }
        }
        public async Task<List<Customer>> GetJsonAsync()
        {
            return JsonConvert.DeserializeObject<List<Customer>>(await DependencyService.Get<IRWExternalStorage>().ReadFileAsync());
        }
    }
}
