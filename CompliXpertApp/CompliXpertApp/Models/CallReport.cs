using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CompliXpertApp.Models
{
    public partial class CallReport
    {
        public bool CreatedOnMobile { get; set; }
        [Key]
        public int CallReportId { get; set; }
        public string Officer { get; set; }
        public string Position { get; set; }
        public DateTime CallDate { get; set; }
        public string Reference { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string LastUpdated { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        [ForeignKey("Account")]
        public int? AccountNumber { get; set; }
        public string CallReportType { get; set; }
        public ICollection<CallReportResponse> Responses { get; set; }
        public string Date
        {
            get
            {
                return CallDate.ToShortDateString();
            }
        }

        public Account AccountNumberNavigation { get; set; }
    }
}
