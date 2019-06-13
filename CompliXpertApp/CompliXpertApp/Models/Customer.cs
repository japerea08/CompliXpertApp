using System.Collections.Generic;

namespace CompliXpertApp.Models
{
    public partial class Customer
    {
        public int CustomerNumber { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string LegalType { get; set; }
        public ICollection<Account> Account { get; set; }
    }
}
