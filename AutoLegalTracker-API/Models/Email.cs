using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AutoLegalTracker_API.Models
{
    public class Email
    {
        [Key]
        public int Id { get; set; }
        
        public string EmailCode { get; set; }
        
        public string Subject { get; set; }
        public string Body { get; set; }
        
        public virtual ICollection<EmailLog> EmailLogs { get; set; }
    }
}
