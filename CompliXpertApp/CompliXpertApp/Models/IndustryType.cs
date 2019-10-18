using System.ComponentModel.DataAnnotations;

namespace CompliXpertApp.Models
{
    public class IndustryType
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
