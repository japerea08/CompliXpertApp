using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompliXpertApp.Models
{
    public partial class CallReportType
    {
        [Key]
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
