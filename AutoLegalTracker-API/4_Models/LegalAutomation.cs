namespace AutoLegalTracker_API.Models
{
    public class LegalAutomation
    {
        public LegalAutomation()
        {

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Active { get; set; } = true;
        public int UserId { get; set; }
        public string CaseCaptionContains { get; set; }
        public string CaseCaptionDoesNotContain { get; set; }
        public string NotificationBodyContains { get; set; } // probably this will switch to a list of strings
        public string NotificationBodyDoesNotContain { get; set; } // probably this will switch to a list of strings
        public string NotificationFromContains { get; set; }
        public string NotificationFromDoesNotContain { get; set; }
        public string NotificationToContains { get; set; }
        public string NotificationToDoesNotContain { get; set; }
        public string JurisdictionContains { get; set; } // probably this will switch to a list of strings
        public string JurisdictionDoesNotContain { get; set; } // probably this will switch to a list of strings
        public virtual User User { get; set; } // Navigation property   
        public virtual List<LegalNotification> LegalNotifications { get; set; } = new List<LegalNotification>();
    }
}
