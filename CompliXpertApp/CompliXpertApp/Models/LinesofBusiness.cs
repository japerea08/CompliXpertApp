

using System.ComponentModel.DataAnnotations;

namespace CompliXpertApp.Models
{
    public class LinesofBusiness
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
