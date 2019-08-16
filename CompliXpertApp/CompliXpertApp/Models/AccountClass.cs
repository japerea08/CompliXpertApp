using System.ComponentModel.DataAnnotations;

namespace CompliXpertApp.Models
{
    public partial class AccountClass
    {
        [Key]
        public int AccountClassCode { get; set; }
        public string Description { get; set; }
    }
}
