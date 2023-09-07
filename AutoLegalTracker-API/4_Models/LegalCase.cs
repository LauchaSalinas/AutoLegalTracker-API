using System;
namespace AutoLegalTracker_API.Models
{
	public class LegalCase
	{
		public LegalCase()
		{

		}
        public int Id { get; set; }
        public string Caption { get; set; } // Caption is the same as Name or Title
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public int UserId { get; set; }
        public string Jurisdiction { get; set; }
        public string? CaseNumber { get; set; }

        public virtual User User { get; set; } // Navigation property
        public virtual List<LegalNotification> LegalNotifications { get; set; } = new List<LegalNotification>();
    }
}

