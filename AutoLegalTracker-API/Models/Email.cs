using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    public class EmailLog
    {
        [Key]
        public int Id { get; set; }
        public string UserTo { get; set; }
        public string EmailDate { get; set; }
        // Email Foreign Key
        public int EmailId { get; set; }

        // Creating an email object and indicating which property is the FK
        [ForeignKey("EmailId")]
        public virtual Email Email { get; set; }
    }
}
