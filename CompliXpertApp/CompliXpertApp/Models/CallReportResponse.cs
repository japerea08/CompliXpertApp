using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompliXpertApp.Models
{
    public class CallReportResponse
    {
        [Key]
        public int ResponseId { get; set; }
        public string Response { get; set; }
        [ForeignKey("CallReportQuestions")]
        public int QuestionId { get; set; }
        [ForeignKey("CallReport")]
        public int CallReportId { get; set; }
    }
}
