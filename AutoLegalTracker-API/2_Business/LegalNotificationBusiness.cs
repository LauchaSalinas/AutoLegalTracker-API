using AutoLegalTracker_API.DataAccess;
using AutoLegalTracker_API.Models;

namespace AutoLegalTracker_API.Business
{
    public class LegalNotificationBusiness
    {
        private readonly LegalNotificationDataAccess _legalNotificationAccess;
        public LegalNotificationBusiness() 
        { 

        }
        public async Task<List<LegalNotification>> GetNotifications(LegalCase legalCase)
        {
            // get cases from database
            var notifications = await _legalNotificationAccess.GetLegalNotificationsFromLegalCase(legalCase);

            // return cases
            return notifications;
        }
    }
}
