using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic; // Add this namespace

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
        public DateTime? PossibleCourtDate { get; set; }
        public List<RequestedAnalysis> RequestedAnalyses { get; set; } = new List<RequestedAnalysis>(); // Similar to Peritajes
        public int UserId { get; set; }
        public string Jurisdiction { get; set; }
        public string? CaseNumber { get; set; }
        public bool ExpenseAdvancesPaid { get; set; } // Anticipo de gastos pagos

        [InverseProperty("RequestedCourtOrder")]
        public List<CourtOrderRequested> RequestedCourtOrders { get; set; } = new List<CourtOrderRequested>(); // Similar to Oficios Judiciales

        [InverseProperty("SentCourtOrder")]
        public List<CourtOrderSent> SentCourtOrders { get; set; } = new List<CourtOrderSent>(); // Similar to Oficios Judiciales

        // [NotMapped]
        // public List<CaseAction> AvailableActions { get; set; } = new List<CaseAction>();
        public virtual List<LegalCaseAttribute> LegalCaseAttributes { get; set; } = new List<LegalCaseAttribute>(); // Navigation property
        public virtual User User { get; set; } // Navigation property
        public virtual List<LegalNotification> LegalNotifications { get; set; } = new List<LegalNotification>();
    }

    // Case Action needs to fit in some place between legal case attribute and legal response template
    public class CaseAction
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        Action Action { get; set; } // or maybe is it a Func? // or maybe a Legal
    }

    public class RequestedAnalysis
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CourtOrderRequested
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [ForeignKey("RequestedCourtOrder")]
        public int RequestedCourtOrderId { get; set; }

        [InverseProperty("RequestedCourtOrders")]
        public virtual LegalCase RequestedCourtOrder { get; set; }
    }

    public class CourtOrderSent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [ForeignKey("SentCourtOrder")]
        public int SentCourtOrderId { get; set; }

        [InverseProperty("SentCourtOrders")]
        public virtual LegalCase SentCourtOrder { get; set; }
    }
}