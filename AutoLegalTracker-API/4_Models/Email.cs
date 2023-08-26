using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoLegalTracker_API.Models
{
    public class EmailTemplate
    {
        [Key]
        public int Id { get; set; }
        public EmailTemplateEnum emailCode { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public virtual ICollection<EmailLog> EmailLogs { get; set; }
    }

    public class EmailLog
    {
        [Key]
        public int Id { get; set; }
        public string UserTo { get; set; }
        public DateTime EmailDate { get; set; }
        // Email Foreign Key
        public int EmailTemplateId { get; set; }

        // Creating an email object and indicating which property is the FK
        [ForeignKey("EmailTemplateId")]
        public virtual EmailTemplate Email { get; set; }
    }

    public enum EmailTemplateEnum
    {
        NewUser,
        NewCase,
        NewTurn,
        CaseExpirationAlert
    }
}
