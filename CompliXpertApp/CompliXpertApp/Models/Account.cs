using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompliXpertApp.Models
{
    public partial class Account
    {
        public Account()
        {
            CallReport = new HashSet<CallReport>();
        }
        [Key]
        public int AccountNumber { get; set; }
        public string AccountType { get; set; }
        public int? AccountClassCode { get; set; }
        public int? CustomerNumber { get; set; }
        public ICollection<CallReport> CallReport { get; set; }
        public Customer CustomerNumberNavigation { get; set; }
        public AccountClass AccountClass { get; set; }
        //future migrations
        public string ProductCode { get; set; }
        public string BusinessCode { get; set; }
        public string IndustryCode { get; set; }
    }
}
