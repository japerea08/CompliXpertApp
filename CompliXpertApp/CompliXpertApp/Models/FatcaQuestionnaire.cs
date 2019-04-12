namespace CompliXpertApp.Models
{
    public partial class FatcaQuestionnaire
    {
        public int QuestionnaireId { get; set; }
        public string Nationality { get; set; }
        public string ReasonforAlert { get; set; }
        public string CustomerResponse { get; set; }
        public int? AccountNumber { get; set; }

        public Account AccountNumberNavigation { get; set; }
    }
}
