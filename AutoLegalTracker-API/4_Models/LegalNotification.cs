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
        public bool Read { get; set; }
        public bool UseAutomation { get; set; }
        public int? LegalAutomationId { get; set; }
        public List<MedicalAppointment> MedicalAppointments { get; set; } = new List<MedicalAppointment>();
    }
}
