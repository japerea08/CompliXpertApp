using CompliXpertApp.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace CompliXpertApp.ViewModels
{
    class AccountListScreenViewModel : INotifyPropertyChanged
    {
        public AccountListScreenViewModel(List<Account> accounts)
        {
            Accounts = accounts;
        }

        //properties
        public List<Account> Accounts { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
