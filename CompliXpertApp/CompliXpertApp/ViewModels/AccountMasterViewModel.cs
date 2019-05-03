using CompliXpertApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompliXpertApp.ViewModels
{
    class AccountMasterViewModel
    {
        //properties
        public Account Customer { get; set; }
        public int AccountNumber
        {
            get { return Customer.AccountNumber; }
        }
        public int CustomerNumber
        {
            get { return Customer.CustomerNumber; }
        }
        public string Status
        {
            get { return Customer.AccountStatus; }
        }
        public string AccountType
        {
            get { return Customer.AccountType; }
        }
        public string AccountClass
        {
            get { return Customer.AccountClass; }
        }
        public string Country
        {
            get { return Customer.Country; }
        }
        //constructor
        public AccountMasterViewModel(Account account)
        {
            Customer = new Account();
            Customer = account;
        }
    }
}
