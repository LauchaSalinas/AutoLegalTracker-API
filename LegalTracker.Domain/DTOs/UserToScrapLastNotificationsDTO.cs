using LegalTracker.Domain.Entities;

namespace LegalTracker.Domain.DTOs;

public class UserToScrapLastNotificationsDTO
{
    public UserToScrapLastNotificationsDTO()
    {
        ScrappedLegalNotifications = new List<LegalCase>();
        WebCredentialUser = string.Empty;
        WebCredentialPassword = string.Empty;
    }
    public int Id { get; set; }
    public string WebCredentialUser { get; set; }
    public string WebCredentialPassword { get; set; }
    public int LastScrappedPage { get; set; } = 1;
    public DateTime? LastScrappedAt { get; set; }
    public List<LegalCase> ScrappedLegalNotifications { get; set; }

}

public class LegalNotificationByNotificationsPageDTO
{
    public LegalNotificationByNotificationsPageDTO()
    {
    }
    public int UserId { get; set; }
    public string CaseNumber { get; set; }
    public string CaseCaption { get; set; }
    public string CaseJurisdiction { get; set; }
    public DateTime NotificationCreatedAt { get; set; }
    public DateTime NotificationNotifiedDate { get; set; }
    public DateTime LastScrappedAt { get; set; }
    public string NotificationTitle { get; set; }
}