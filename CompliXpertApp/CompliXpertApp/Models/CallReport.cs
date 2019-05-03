using System;

namespace CompliXpertApp.Models
{
    public partial class CallReport
    {
        public int CallReportId { get; set; }
        public string Officer { get; set; }
        public string Position { get; set; }
        public string Reason { get; set; }
        public DateTime CallDate { get; set; }
        public string Status { get; set; }
        public string Reference { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string LastUpdated { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string Purpose { get; set; }
        public string OfficerComments { get; set; }
        public string OtherComments { get; set; }
        public string CustomerComments { get; set; }
        public int? AccountNumber { get; set; }

        public Account AccountNumberNavigation { get; set; }
    }
}
