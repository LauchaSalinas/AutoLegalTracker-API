using System.ComponentModel.DataAnnotations.Schema;

namespace AutoLegalTracker_API.Models
{
    public class LegalNotification
    {
        public LegalNotification()
        {
            NotificationReferences = new HashSet<NotificationReference>();
        }
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? NotificationDate { get; set; }
        public bool ActionHasBeenTaken { get; set; } = false;
        public int LegalCaseId { get; set; }
        public virtual LegalCase LegalCase { get; set; } // Navigation property
        public bool Read { get; set; } = false;
        
        [ForeignKey("MedicalAppointment")]
        public int? MedicalAppointmentId { get; set; }
        public virtual MedicalAppointment MedicalAppointment { get; set; } // Navigation property

        #region Scraped Data

        public string? NotificationType { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public string Title { get; set; }
        public string? Body { get; set; }
        public string? Observation { get; set; }
        public string ScrapUrl { get; set; }
        public bool Signed { get; set; } = false;

        public virtual ICollection<NotificationReference> NotificationReferences { get; set; }
        
        #endregion Scraped Data
    }

    public class NotificationReference
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string? Value { get; set; }
        public int LegalNotificationId { get; set; }
    }
}
