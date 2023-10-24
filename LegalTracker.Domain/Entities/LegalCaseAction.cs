using System.ComponentModel.DataAnnotations.Schema;

namespace LegalTracker.Domain.Entities
{
    public class LegalCaseAction
    {
        public LegalCaseAction()
        {
            LegalCaseAttributesToAdd = new HashSet<LegalCaseAttribute>();
            LegalCaseAttributesToDelete = new HashSet<LegalCaseAttribute>();
            UserTypes = new HashSet<UserType>();
        }
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

        public int? NotificationConditionId { get; set; }
        public virtual NotificationCondition NotificationCondition { get; set; } // Navigation property

        // ChangeAttribute action
        public ICollection<LegalCaseAttribute> LegalCaseAttributesToAdd { get; set; }
        public ICollection<LegalCaseAttribute> LegalCaseAttributesToDelete { get; set; }


        // Response action
        public int? LegalResponseTemplateId { get; set; }
        public virtual LegalResponseTemplate LegalResponseTemplate { get; set; } // Navigation property

        // Email action
        public int? EmailTemplateId { get; set; }
        public virtual EmailTemplate EmailTemplate { get; set; } // Navigation property

        // MedicalAppointment action
        public bool AssignMedicalAppointment { get; set; } = false; // true = assign medical appointment, false = do not assign medical appointment

        // Roles
        /// <summary>
        /// UserTypeAllowedToUseAction
        /// </summary>
        public ICollection<UserType> UserTypes { get; set; }
    }
}
