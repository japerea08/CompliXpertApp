namespace CompliXpertApp.Models
{
    public partial class CallReport
    {
        public int CallReportId { get; set; }
        public string Purpose { get; set; }
        public string OfficerComments { get; set; }
        public string OtherComments { get; set; }
        public string CustomerComments { get; set; }
        public int? AccountNumber { get; set; }

        public Account AccountNumberNavigation { get; set; }
    }
}
