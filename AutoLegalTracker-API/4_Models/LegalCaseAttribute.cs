using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoLegalTracker_API.Models;

public class LegalCaseAttribute
{
    public LegalCaseAttribute()
    {

    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    [ForeignKey("LegalCaseAttributeId")]
    public int? AttributeToAddWhenExpiredId { get; set; }
    public virtual LegalCaseAttribute? AttributeToAddWhenExpired { get; set; } // Navigation property
    public int LegalCaseId { get; set; }
    public virtual LegalCase LegalCase { get; set; } // Navigation property
}

public class LegalCaseAttribute_LegalCaseCondition
{
    public int Id { get; set; }
    public int LegalCaseAttributeId { get; set; }
    public LegalCaseAttribute LegalCaseAttribute { get; set; }

    public int LegalCaseConditionId { get; set; }
    public LegalCaseCondition LegalCaseCondition { get; set; }

    public bool MustBePresent { get; set; } // true = attribute must be present, false = attribute must not be present
}

