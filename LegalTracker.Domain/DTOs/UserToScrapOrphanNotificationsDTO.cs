using LegalTracker.Domain.Entities;

namespace LegalTracker.Domain.DTOs;

public class UserToScrapOrphanNotificationsDTO
{
    public UserToScrapOrphanNotificationsDTO()
    {
        OrphanNotifications = new List<OrphanNotificationDTO>();
        WebCredentialUser = string.Empty;
        WebCredentialPassword = string.Empty;
    }
    public int Id { get; set; }
    public string WebCredentialUser { get; set; }
    public string WebCredentialPassword { get; set; }
    public List<OrphanNotificationDTO> OrphanNotifications { get; set; }
}

public class OrphanNotificationDTO
{
    public OrphanNotificationDTO()
    {
    }
    public int UserId { get; set; }
    public string CaseNumber { get; set; }
    public string CaseCaption { get; set; }
    public string CaseJurisdiction { get; set; }
    public DateTime NotificationCreatedAt { get; set; }
    public DateTime NotificationNotifiedDate { get; set; }
    public string NotificationTitle { get; set; }
    public string ScrapUrl { get; set; }
    public LegalCase ScrappedLegalCase { get; set; }
}