using System.Collections.Generic;

namespace CompliXpertApp.Models
{
    public partial class Account
    {
        public Account()
        {
            CallReport = new HashSet<CallReport>();
            FatcaQuestionnaire = new HashSet<FatcaQuestionnaire>();
        }

        public int AccountNumber { get; set; }
        public int CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public string Country { get; set; }
        public string AccountType { get; set; }
        public string AccountClass { get; set; }
        public string AccountStatus { get; set; }

        public ICollection<CallReport> CallReport { get; set; }
        public ICollection<FatcaQuestionnaire> FatcaQuestionnaire { get; set; }
        public override string ToString()
        {
            return AccountNumber.ToString() + " " + CustomerName;
        }
    }
}
