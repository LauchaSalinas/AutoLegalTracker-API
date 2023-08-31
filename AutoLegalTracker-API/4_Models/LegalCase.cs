using System;
namespace AutoLegalTracker_API.Models
{
	public class LegalCase
	{
		public LegalCase()
		{

		}
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public int userId { get; set; }
        public string Jurisdiction { get; set; }
        public string? CaseNumber { get; set; }
        

        public List<LegalNotification> LegalNotifications { get; set; } = new List<LegalNotification>();
    }
}

