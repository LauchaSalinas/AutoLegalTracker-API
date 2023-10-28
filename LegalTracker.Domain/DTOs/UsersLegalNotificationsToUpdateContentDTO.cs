namespace LegalTracker.Domain.DTOs;

public class UsersLegalNotificationsToUpdateContentDTO
{
    public UsersLegalNotificationsToUpdateContentDTO()
    {
        LegalNotificationsToFill = new HashSet<LegalNotificationToFillDTO>();
        WebCredentialUser = string.Empty;
        WebCredentialPassword = string.Empty;
    }
    public int Id { get; set; }
    public string WebCredentialUser { get; set; }
    public string WebCredentialPassword { get; set; }
    public ICollection<LegalNotificationToFillDTO> LegalNotificationsToFill { get; set; }
}

public class LegalNotificationToFillDTO
{
    public LegalNotificationToFillDTO()
    {
        NotificationReferences = new HashSet<Entities.NotificationReference>();
    }
    public int Id { get; set; }
    public string Body { get; set; }
    public string ScrapUrl { get; set; }
    public string NotificationType { get; set; }
    public virtual ICollection<Entities.NotificationReference> NotificationReferences { get; set; }
}