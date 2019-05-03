using CompliXpertApp.Models;
using System;
using System.Collections.Generic;

namespace CompliXpertApp.ViewModels
{
    class CallReportListViewModel
    {
        //constructor
        public CallReportListViewModel(List<CallReport> callReports)
        {
            CallReports = callReports;
        }
        //properties
        public List<CallReport> CallReports { get; set; }
        public CallReport ReportSelected { get; set; }
        public int CallReportId { get { return ReportSelected.CallReportId; } }
        public string Officer { get { return ReportSelected.Officer; } }
        public string Position { get { return ReportSelected.Position; } }
        public string Reason { get { return ReportSelected.Reason; } }
        public DateTime CallDate { get { return ReportSelected.CallDate.Date; } }
        public string Status { get { return ReportSelected.Status; } }
    }
}
