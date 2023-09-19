using AutoLegalTracker_API.DataAccess;
using AutoLegalTracker_API.Models;

namespace AutoLegalTracker_API.Business
{
    public class LegalNotificationBusiness
    {
        private readonly LegalNotificationDataAccess _legalNotificationAccess;
        public LegalNotificationBusiness(LegalNotificationDataAccess legalNotificationDataAccess) 
        { 
            _legalNotificationAccess = legalNotificationDataAccess;
        }
        public List<LegalNotification> GetNotifications(LegalCase legalCase)
        {
            // get cases from database
            var notifications = _legalNotificationAccess.GetLegalNotificationsFromLegalCase(legalCase);

            // return cases
            return notifications;
        }
    }
}
