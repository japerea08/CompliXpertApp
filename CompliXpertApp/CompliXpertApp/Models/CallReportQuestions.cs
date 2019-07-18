using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompliXpertApp.Models
{
    public partial class CallReportQuestions
    {
        [Key]
        public int QuestionId { get; set; }
        public string QuestionHeader { get; set; }
        public bool Status { get; set; }
        [ForeignKey("CallReportType")]
        public string Type { get; set; }
    }
}
