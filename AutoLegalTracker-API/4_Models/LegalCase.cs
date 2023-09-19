using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic; // Add this namespace
using AutoLegalTracker_API.Models;

namespace AutoLegalTracker_API.Models
{
    public class LegalCase
    {
        public LegalCase()
        {

        }

        public int Id { get; set; }
        public int UserId { get; set; }
        /// <summary>
        /// Caratula de casusa
        /// </summary>
        public string Caption { get; set; } 
        public string Description { get; set; }
        public string Jurisdiction { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        /// <summary>
        /// Fecha de posible de sentencia o resolución judicial para cobro
        /// </summary>
        public DateTime? PossibleCourtDate { get; set; }
        /// <summary>
        /// Peritajes
        /// </summary>
        public virtual List<RequestedAnalysis> RequestedAnalyses { get; set; } = new List<RequestedAnalysis>(); // Navigation property
        public string? CaseNumber { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ExpenseAdvances { get; set; } = 0;
        /// <summary>
        /// Anticipo de gastos pagos
        /// </summary>
        public bool ExpenseAdvancesPaid { get; set; } = false;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal LegalCaseHonorarium { get; set; } = 0;

        /// <summary>
        /// Oficios Judiciales
        /// </summary>
        public List<RequestedCourtOrder> RequestedCourtOrders { get; set; } = new List<RequestedCourtOrder>();

        // [NotMapped]
        // public List<CaseAction> AvailableActions { get; set; } = new List<CaseAction>();
        public virtual List<LegalCaseAttribute> LegalCaseAttributes { get; set; } = new List<LegalCaseAttribute>(); // Navigation property
        public virtual User User { get; set; } // Navigation property
        public virtual List<LegalNotification> LegalNotifications { get; set; } = new List<LegalNotification>();
    } 
    

    public class RequestedAnalysis
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LegalCaseId { get; set; }
        public virtual LegalCase LegalCase { get; set; } // Navigation property
        public bool IsFulfilled { get; set; } = false;
    }

    public class RequestedCourtOrder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LegalCaseId { get; set; }
        public virtual LegalCase LegalCase { get; set; } // Navigation property
        public bool IsFulfilled { get; set; } = false;
    }

}