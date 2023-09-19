using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoLegalTracker_API.Models;

public class LegalCaseAttribute
{
    public LegalCaseAttribute()
    {
        LegalCase = new HashSet<LegalCase>();
        LegalCaseActionsWhereItsAdded = new HashSet<LegalCaseAction>();
        LegalCaseActionsWhereItsDeleted = new HashSet<LegalCaseAction>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    /// <summary>
    /// Reference to the LegalCaseAttribute that will be added to the LegalCase when this attribute expires
    /// EF Self Reference
    /// </summary>
    [ForeignKey("LegalCaseAttribute")]
    public int? AttributeToAddWhenExpiredId { get; set; }
    public virtual LegalCaseAttribute? AttributeToAddWhenExpired { get; set; } // Navigation property
    public virtual ICollection<LegalCase> LegalCase { get; set; } // Navigation property

    public ICollection<LegalCaseAction> LegalCaseActionsWhereItsAdded { get; set; }
    public ICollection<LegalCaseAction> LegalCaseActionsWhereItsDeleted { get; set; }
}

