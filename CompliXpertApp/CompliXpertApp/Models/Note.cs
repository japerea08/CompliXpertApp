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
    }
}
