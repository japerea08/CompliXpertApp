using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompliXpertApp.Models
{
    public partial class CallReportType
    {
        [Key]
        public string Type { get; set; }
        public string Description { get; set; }
        ICollection<CallReportQuestions> CallReportQuestions { get; set; }
    }
}
