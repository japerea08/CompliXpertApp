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
        public string UsernamePlaceholder { get; } = "Enter Username";
        public string PasswordPlaceholder { get; } = "Enter Password";
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
                //check CallReportType table and Question tables
                if(await Task.Run(() => DBContainsCallReportTypeQuestionsandCountries()) == false)
                {
                    //get the json for call report questions
                    List<CallReportType> callReportTypes = await Task.Run(() => GetCallReportTypeJsonAsync());
                    List<CallReportQuestions> callReportQuestions = await Task.Run(()=> GetCallReportQuestionsJsonAsync());
                    List<Country> countries = await Task.Run(() => GetCountriesAsync());
                    List<AccountClass> accountClasses = await Task.Run(() => GetAccountClassesAsync());
                    //add to DB
                    await Task.Run(() => InitializeDBAsync(callReportTypes, callReportQuestions, countries, accountClasses));
                }
                //will only run if the DB has records
                if (await Task.Run(() => DBContainsRecords()))
                {
                    //get everything from DB
                    List<Customer> customers = await Task.Run(() => GetCustomersAsync());
                    IsBusy = false;
                    Page page = new CustomerListScreen();
                    //await App.Current.MainPage.Navigation.PushAsync(new CustomerListScreen());
                    await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage());
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
                    //await App.Current.MainPage.Navigation.PushAsync(new CustomerListScreen());
                    await App.Current.MainPage.Navigation.PushAsync(new CompliXpertAppMasterDetailPage());
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
        public bool DBContainsCallReportTypeQuestionsandCountries()
        {
            using (var context = new CompliXperAppContext())
            {
                if (context.CallReportQuestions.Any() && context.CallReportType.Any() && context.Countries.Any() && context.AccountClasses.Any())
                    return true;
                else
                    return false;
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
                    return false;
                }
            }       
        }
        //method that gets all customers from sqlite DB
        public async Task<List<Customer>> GetCustomersAsync()
        {
            using (var context = new CompliXperAppContext())
            {
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
                                CustomerNumber = _account.CustomerNumber,
                                AccountClassCode = _account.AccountClassCode,
                                AccountClass = (from _accountClass in context.AccountClasses
                                                where _accountClass.AccountClassCode == _account.AccountClassCode
                                                select new AccountClass
                                                {
                                                    AccountClassCode = _accountClass.AccountClassCode,
                                                    Description = _accountClass.Description
                                                }).FirstOrDefault()
                            }
                        ).ToListAsync(); 
                }
                return await customers.ToListAsync();
            }
        }
        public async Task InitializeDBAsync(List<CallReportType> types, List<CallReportQuestions> questions, List<Country> countries, List<AccountClass> accountClasses)
        {
            using (var context = new CompliXperAppContext())
            {
                context.CallReportType.AddRange(types);
                context.CallReportQuestions.AddRange(questions);
                context.Countries.AddRange(countries);
                context.AccountClasses.AddRange(accountClasses);
                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.InnerException);
                }
            }           
        }
        public async Task AddCustomersAsync(List<Customer> customers)
        {
            using (var context = new CompliXperAppContext())
            {
                context.Customer.AddRange(customers);
                List<Account> accounts = new List<Account>();
                List<CallReport> callreports = new List<CallReport>();
                List<CallReportResponse> responses = new List<CallReportResponse>();
                foreach (Customer customer in customers)
                {

                    accounts.AddRange(customer.Account);
                    foreach (Account account in customer.Account)
                    {
                        callreports.AddRange(account.CallReport);
                        foreach (CallReport report in account.CallReport)
                        {
                            responses.AddRange(report.Responses);
                        }
                    }
                }
                context.Account.AddRange(accounts);
                context.CallReport.AddRange(callreports);
                context.CallReportResponse.AddRange(responses);
                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.InnerException);
                }
            }
        }
        //returns a list of customers
        public async Task<List<Customer>> GetJsonAsync()
        {
            return JsonConvert.DeserializeObject<List<Customer>>(await DependencyService.Get<IRWExternalStorage>().ReadFileAsync());
        }
        //get call report types
        public async Task<List<CallReportType>> GetCallReportTypeJsonAsync()
        {
            return JsonConvert.DeserializeObject<List<CallReportType>>(await DependencyService.Get<IRWExternalStorage>().GetCallReportTypeAsync());
        }
        //get call report questions
        public async Task<List<CallReportQuestions>> GetCallReportQuestionsJsonAsync()
        {
            return JsonConvert.DeserializeObject<List<CallReportQuestions>>(await DependencyService.Get<IRWExternalStorage>().GetCallReportQuestionsAsync());
        }
        //get countries
        public async Task<List<Country>> GetCountriesAsync()
        {
            return JsonConvert.DeserializeObject<List<Country>>(await DependencyService.Get<IRWExternalStorage>().GetCountriesAsync());
        }
        public async Task<List<AccountClass>>GetAccountClassesAsync()
        {
            return JsonConvert.DeserializeObject<List<AccountClass>>(await DependencyService.Get<IRWExternalStorage>().GetAccountClassesAsync());
        }
    }
}
