using System.ComponentModel.DataAnnotations.Schema;

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
        public bool ActionHasBeenTaken { get; set; } = false;
        public int LegalCaseId { get; set; }
        public virtual LegalCase LegalCase { get; set; } // Navigation property
        public bool Read { get; set; } = false;
        //public bool UseAutomation { get; set; }
        //public int? LegalAutomationId { get; set; }
        //public virtual LegalAutomation LegalAutomation { get; set; } // Navigation property
        [ForeignKey("MedicalAppointment")]
        public int? MedicalAppointmentId { get; set; }
        public virtual MedicalAppointment MedicalAppointment { get; set; } // Navigation property

        public string To { get; set; }
        public string From { get; set; }
    }
}
