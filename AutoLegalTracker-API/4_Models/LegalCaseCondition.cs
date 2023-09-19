namespace AutoLegalTracker_API.Models
{
    public class LegalCaseCondition
    {
        public LegalCaseCondition()
        {
            LegalCaseAttributes = new HashSet<LegalCaseAttribute>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        /// <summary>
        /// Attributes that the legal case must have in order to trigger the action
        /// </summary>
        public virtual ICollection<LegalCaseAttribute> LegalCaseAttributes { get; set; }
        public bool ExpensesShouldBePaid { get; set; } = false;
        public string? JurisdictionContains { get; set; }
        public string? JurisdictionDoesNotContain { get; set; }
        public string? CaseCaptionContains { get; set; }
        public string? CaseCaptionDoesNotContain { get; set; }
        public bool AnalysesMustBeReceived { get; set; } = false;
        public bool RequestedCourtOrdersMustBeReceived { get; set; } = false;
    }

    public class NotificationCondition
    {
        public NotificationCondition()
        {

        }
        public int Id { get; set; }
        public string? TitleContains { get; set; }
        public string? TitleDoesNotContain { get; set; }
        public string? BodyContains { get; set; }
        public string? BodyDoesNotContain { get; set; }
        public string? FromContains { get; set; }
        public string? FromDoesNotContain { get; set; }
        public string? ToContains { get; set; }
        public string? ToDoesNotContain { get; set; }
    }
}
