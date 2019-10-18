using System.ComponentModel.DataAnnotations;

namespace CompliXpertApp.Models
{
    public class ProductCode
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
