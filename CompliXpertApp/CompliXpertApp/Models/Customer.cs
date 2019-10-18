using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompliXpertApp.Models
{
    public partial class Customer
    {
        public Customer()
        {
            IsPEP = false;
        }
        [Key]
        public int CustomerNumber { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string LegalType { get; set; }
        public bool CreatedOnMobile { get; set; }
        public bool IsPEP { get; set; }
        public string MailAddress { get; set; }
        public int? Citizenship { get; set; }
        public int? CountryofResidence { get; set; }
        public string Email { get; set; }
        public string BusinessCode { get; set; }
        public string IndustryCode { get; set; }
        public ICollection<Account> Account { get; set; }
    }
}
