using CompliXpertApp.Helpers;
using CompliXpertApp.Models;
using CompliXpertApp.Views;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private CompliXperAppContext context;

        public LoginViewModel()
        {
            context = new CompliXperAppContext();
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
                //check CallReportType table and Question tables
                if(await Task.Run(() => DBContainsCallReportTypeandQuestions()) == false)
                {
                    //get the json for call report questions
                    List<CallReportType> callReportTypes = await Task.Run(() => GetCallReportTypeJsonAsync());
                    List<CallReportQuestions> callReportQuestions = await Task.Run(()=> GetCallReportQuestionsJsonAsync());
                    //add to DB
                    await Task.Run(() => AddTypeandQuestionsAsync(callReportTypes, callReportQuestions));
                }
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
        public bool DBContainsCallReportTypeandQuestions()
        {
            if (context.CallReportQuestions.Any() || context.CallReportType.Any())
                return true;
            else
                return false;
        }
        public bool DBContainsRecords()
        {
            if (context.Customer.Any())
                return true;
            else
            {
                return false;
            }
        }
        public async Task<List<Customer>> GetCustomersAsync()
        {
            using (context)
            {
                //get all customers 
                var customers = context.Customer;
                //populate the customer's account
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
                }
                return await customers.ToListAsync();
            }
        }
        public async Task AddTypeandQuestionsAsync(List<CallReportType> types, List<CallReportQuestions> questions)
        {
            context.CallReportType.AddRange(types);
            context.CallReportQuestions.AddRange(questions);
            await context.SaveChangesAsync();
        }
        public async Task AddCustomersAsync(List<Customer> customers)
        {
                context.Customer.AddRange(customers);
                List<Account> accounts = new List<Account>();
                List<CallReport> callreports = new List<CallReport>();
                List<CallReportResponse> responses = new List<CallReportResponse>();
                foreach (Customer customer in customers)
                {
                    accounts.AddRange(customer.Account);
                    foreach(Account account in customer.Account)
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
    }
}
