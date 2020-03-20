using System;
using System.ComponentModel.DataAnnotations;

namespace CompliXpertApp.Models
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
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
