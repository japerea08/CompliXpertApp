using System;

namespace CompliXpertApp.Models
{

    public class CompliXpertAppMasterDetailPageMenuItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageSource { get; set; }
        public Type TargetType { get; set; }
    }
}