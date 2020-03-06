using System;
using System.ComponentModel.DataAnnotations;

namespace CompliXpertApp.Models
{
    public class Note
    {
        [Key]
        public int NoteId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public bool CreatedonMobile { get; set; }
        public int? CallReportId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Date
        {
            get
            {
                return CreatedDate.ToShortDateString();
            }
        }
        public string Time
        {
            get
            {
                return CreatedDate.ToShortTimeString();
            }
        }
    }
}
