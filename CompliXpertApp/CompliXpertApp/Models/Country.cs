using System.ComponentModel.DataAnnotations;

namespace CompliXpertApp.Models
{
    public class Country
    {
        [Key]
        public int CountryCode { get; set; }
        public string Description { get; set; }
    }
}
