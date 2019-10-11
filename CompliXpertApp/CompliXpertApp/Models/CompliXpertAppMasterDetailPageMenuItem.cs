using System;

namespace CompliXpertApp.Models
{

    public class CompliXpertAppMasterDetailPageMenuItem
    {
        public CompliXpertAppMasterDetailPageMenuItem()
        {
           // TargetType = typeof(CompliXpertAppMasterDetailPageDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}