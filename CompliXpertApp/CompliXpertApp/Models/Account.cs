using System.Collections.Generic;

namespace CompliXpertApp.Models
{
    public partial class Account
    {
        public Account()
        {
            CallReport = new HashSet<CallReport>();
        }

        public int AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string AccountClass { get; set; }
        public int? CustomerNumber { get; set; }
        public ICollection<CallReport> CallReport { get; set; }
        public Customer CustomerNumberNavigation { get; set; }
        public override string ToString()
        {
            return AccountNumber.ToString();
        }
    }
}
