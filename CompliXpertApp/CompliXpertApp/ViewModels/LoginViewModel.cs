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
                    List<Account> accounts = await Task.Run(() => GetAccountsAsync());
                    IsBusy = false;
                    await App.Current.MainPage.Navigation.PushAsync(new AccountListScreen());
                    MessagingCenter.Send<LoginViewModel, List<Account>>(this, Message.AccountListLoaded, accounts);
                    CanAttemptLogin(true);
                }
                else
                {
                    //grab accounts from file
                    List<Account> accounts = await Task.Run(() => GetJsonAsync());
                    //add to the database
                    await Task.Run(() => AddAccountsAsync(accounts));
                    IsBusy = false;
                    await App.Current.MainPage.Navigation.PushAsync(new AccountListScreen());
                    //pass our list
                    MessagingCenter.Send<LoginViewModel, List<Account>>(this, Message.AccountListLoaded, accounts);
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
                if (context.Account.Any())
                    return true;
                else
                    return false;
            }
        }
        public async Task<List<Account>> GetAccountsAsync()
        {
            using (var context = new CompliXperAppContext())
            {
                //get all accounts 
                var accounts = context.Account;
                foreach (var account in accounts)
                {
                    account.CallReport = (
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
                                LastUpdated = _report.LastUpdated
                            }
                        ).ToArray();
                }
                return await accounts.ToListAsync();
            }
        }
        public async Task AddAccountsAsync(List<Account> accounts)
        {
            using (var context = new CompliXperAppContext())
            {
                context.Account.AddRange(accounts);
                List<CallReport> callreports = new List<CallReport>();
                foreach (Account account in accounts)
                {
                    callreports = account.CallReport.ToList();
                }
                context.CallReport.AddRange(callreports);
                await context.SaveChangesAsync();
            }
        }
        public async Task<List<Account>> GetJsonAsync()
        {
            return JsonConvert.DeserializeObject<List<Account>>(await DependencyService.Get<IRWExternalStorage>().ReadFileAsync());
        }
    }
}
