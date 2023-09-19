using System.ComponentModel.DataAnnotations.Schema;

namespace AutoLegalTracker_API.Models
{
    public class LegalCaseAction
    {
        public LegalCaseAction() { }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        // Condition
        public int? LegalCaseConditionId { get; set; }
        public virtual LegalCaseCondition LegalCaseCondition { get; set; } // Navigation property

        public int? NotificationCondtionId { get; set; }
        public virtual NotificationCondition NotificationCondition { get; set; } // Navigation property
        
        // ChangeAttribute action
        public List<LegalCaseAction_LegalCaseAttribute> LegalCaseAttributesToAddOrDelete { get; set; } = new List<LegalCaseAction_LegalCaseAttribute>();
        
        // Response action
        public int? LegalResponseTemplateId { get; set; }
        public virtual LegalResponseTemplate LegalResponseTemplate { get; set; } // Navigation property
        
        // Email action
        public int? EmailTemplateId { get; set; }
        public virtual EmailTemplate EmailTemplate { get; set; } // Navigation property
        
        // MedicalAppointment action
        public bool AssignMedicalAppointment { get; set; } = false; // true = assign medical appointment, false = do not assign medical appointment

        // Roles
        public List<LegalCaseAction_UserType> UserTypeAllowedToUseAction { get; set; } = new List<LegalCaseAction_UserType>();
    }

    public class LegalCaseAction_LegalCaseAttribute
    {
        public int Id { get; set; }
        public int LegalCaseActionId { get; set; }
        public LegalCaseAction LegalCaseAction { get; set; }

        public int LegalCaseAttributeId { get; set; }
        public LegalCaseAttribute LegalCaseAttribute { get; set; }
        public bool Add { get; set; } = false; // true = attribute must be removed, false = attribute must not be removed
    }

    public class LegalCaseAction_UserType
    {
        public int Id { get; set; }
        public int LegalCaseActionId { get; set; }
        public LegalCaseAction LegalCaseAction { get; set; }

        public int UserTypeId { get; set; }
        public UserType UserType { get; set; }
    }
}
