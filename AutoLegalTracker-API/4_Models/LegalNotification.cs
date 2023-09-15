namespace AutoLegalTracker_API.Models
{
    public class LegalNotification
    {
        public LegalNotification()
        {

        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int LegalCaseId { get; set; }
        public virtual LegalCase LegalCase { get; set; } // Navigation property
        public bool Read { get; set; }
        public bool UseAutomation { get; set; }
        public int? LegalAutomationId { get; set; }
        public virtual LegalAutomation LegalAutomation { get; set; } // Navigation property
        public int? MedicalAppointmentId { get; set; }
        public virtual MedicalAppointment MedicalAppointment { get; set; } // Navigation property
    }
}
