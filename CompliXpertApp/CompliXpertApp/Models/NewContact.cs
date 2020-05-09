using System;
using System.ComponentModel.DataAnnotations;

namespace CompliXpertApp.Models
{
    public class NewContact
    {
        [Key]
        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phonenumber { get; set; }
        public string Company { get; set; }
        public string Comments { get; set; }
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
        public string Name
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
