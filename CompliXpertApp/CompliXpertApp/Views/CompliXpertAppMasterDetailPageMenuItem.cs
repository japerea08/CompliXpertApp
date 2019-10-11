using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompliXpertApp.Views
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