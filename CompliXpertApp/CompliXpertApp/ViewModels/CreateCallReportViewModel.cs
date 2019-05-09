using CompliXpertApp.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CompliXpertApp.ViewModels
{
    class CreateCallReportViewModel : INotifyPropertyChanged
    {
        //attributes
        private bool customerVisitSelected = false;
        private bool fatcaQuestionnaireSelected = false;
        private int selection = -1;
        public CreateCallReportViewModel(Account account)
        {
            Account = account;
        }

        //properties
        public Account Account { get; set; }
        public int SelectedReason
        {
            get
            {
                return selection;
            }
            set
            {
                selection = value;
                if(selection == 0)
                {
                    FatcaSelected = false;
                    CustomerVisitSelected = true;
                }
                else if(selection == 1)
                {
                    CustomerVisitSelected = false;
                    FatcaSelected = true;
                }
            }
        }
        public int AccountNumber { get { return Account.AccountNumber; } }
        public bool CustomerVisitSelected
        {
            get
            {
                return customerVisitSelected;
            }
            set
            {
                customerVisitSelected = value;
                OnPropertyChanged();
            }
        }

        public bool FatcaSelected
        {
            get
            {
                return fatcaQuestionnaireSelected;
            }
            set
            {
                fatcaQuestionnaireSelected = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //methods
        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
