using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompliXpertApp.Models
{
    public class NewContact
    {
        [Key]
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phonenumber { get; set; }
        public string Company { get; set; }
        public string Comments { get; set; }
        public int AccountNumber { get; set; }
    }
}
