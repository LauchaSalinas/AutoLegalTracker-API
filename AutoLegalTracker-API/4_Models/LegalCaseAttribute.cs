using System.Reflection;

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
    public int LegalCaseAttributeConditionId { get; set; }
    public virtual LegalCaseAttributeCondition LegalCaseAttributeCondition { get; set; } // Navigation property
    public int LegalCaseId { get; set; }
    public virtual LegalCase LegalCase { get; set; } // Navigation property
}

public class LegalCaseAttributeCondition
{
    public LegalCaseAttributeCondition()
    {

    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<LegalCaseProperty> LegalCaseProperties { get; set; } = new List<LegalCaseProperty>(); // Navigation property
    public List<LegalCaseAttributeCondition> LegalCaseAttributeConditionsToCheck { get; set; } // many to many relationship with self to assign multiple attributes to check before assigning the attribute
    public virtual List<LegalCaseAttribute> LegalCaseAttributes { get; set; } = new List<LegalCaseAttribute>(); // Navigation property
}

public class LegalCaseProperty
{
    public LegalCaseProperty()
    {

    }

    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string PropertyName { get; set; }
    public string PropertyValue { get; set; }
    public LegalCasePropertyValueType PropertyValueType { get; set; }
    public List<LegalCaseAttributeCondition> LegalCaseAttributeConditions { get; set;} = new List<LegalCaseAttributeCondition>(); // Navigation property
}

public enum LegalCasePropertyValueType
{
    String = 1,
    Int = 2,
    DateTime = 3,
    Bool = 4,
}