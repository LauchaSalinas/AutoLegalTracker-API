using LegalTracker.Domain.Entities;

namespace LegalTracker.Domain.DTOs;

public class LegalCaseToScrapNotificationsDTO
{
    public LegalCaseToScrapNotificationsDTO()
    {
        ScrappedLegalNotifications = new HashSet<LegalNotification>();
    }

    public int Id { get; set; }
    /// <summary>
    /// Caratula de casusa
    /// </summary>
    public string Caption { get; set; }
    public string Jurisdiction { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CaseNumber { get; set; }
    public ICollection<LegalNotification> ScrappedLegalNotifications { get; set; } // Navigation property
    public LegalNotification LastScrappedNotification { get; set; }
    public string ScrapUrl { get; set; }
}
