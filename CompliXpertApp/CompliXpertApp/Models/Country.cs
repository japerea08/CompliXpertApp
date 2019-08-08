using System.ComponentModel.DataAnnotations;

namespace CompliXpertApp.Models
{
    public class Country
    {
        [Key]
        public int Code { get; set; }
        public string Description { get; set; }
    }
}
