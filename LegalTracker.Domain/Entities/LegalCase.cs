using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic; // Add this namespace

namespace LegalTracker.Domain.Entities
{
    public class LegalCase
    {
        public LegalCase()
        {
            LegalCaseAttributes = new HashSet<LegalCaseAttribute>();
            LegalNotifications = new HashSet<LegalNotification>();
            RequestedAnalyses = new HashSet<RequestedAnalysis>();
            RequestedCourtOrders = new HashSet<RequestedCourtOrder>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; } // Navigation property

        /// <summary>
        /// Caratula de casusa
        /// </summary>
        public string Caption { get; set; }
        public string? Description { get; set; }
        public string Jurisdiction { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public DateTime? LastScrappedAt { get; set; }
        /// <summary>
        /// Fecha de posible de sentencia o resolución judicial para cobro
        /// </summary>
        public DateTime? PossibleCourtDate { get; set; }
        /// <summary>
        /// Peritajes
        /// </summary>
        public virtual ICollection<RequestedAnalysis> RequestedAnalyses { get; set; }  // Navigation property
        public string CaseNumber { get; set; }
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
        public virtual ICollection<RequestedCourtOrder> RequestedCourtOrders { get; set; } // Navigation property

        public virtual ICollection<LegalCaseAttribute> LegalCaseAttributes { get; set; } // Navigation property

        public virtual ICollection<LegalNotification> LegalNotifications { get; set; } // Navigation property

        public string ScrapUrl { get; set; }
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