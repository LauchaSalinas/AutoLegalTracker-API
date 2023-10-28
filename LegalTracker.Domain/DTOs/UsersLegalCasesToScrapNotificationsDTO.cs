namespace LegalTracker.Domain.DTOs;

public class UsersLegalCasesToScrapNotificationsDTO
{
    public UsersLegalCasesToScrapNotificationsDTO()
    {
        LegalCasesToGetNotifications = new List<LegalCaseToScrapNotificationsDTO>();
        WebCredentialUser = string.Empty;
        WebCredentialPassword = string.Empty;
    }
    public int Id { get; set; }
    public string WebCredentialUser { get; set; }
    public string WebCredentialPassword { get; set; }
    public List<LegalCaseToScrapNotificationsDTO> LegalCasesToGetNotifications { get; set; } // TODO FOLLOW FROM HERE< THIS SHOULD BE A DTO I THINK with last notification
}