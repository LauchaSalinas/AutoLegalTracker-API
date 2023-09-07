﻿namespace AutoLegalTracker_API.Models
{
    public class MedicalAppointment
    {
        public MedicalAppointment()
        {

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool? PatientAttended { get; set; }
        public int LegalNotificationId { get; set; }

        public virtual LegalNotification LegalNotification { get; set; } // Navigation property
    }
}
