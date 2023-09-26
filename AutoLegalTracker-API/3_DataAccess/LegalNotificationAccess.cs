using AutoLegalTracker_API.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace AutoLegalTracker_API.DataAccess
{
    public class LegalNotificationDataAccess
    {
        private readonly ALTContext _altContext;
        public LegalNotificationDataAccess(ALTContext altContext)
        {
            _altContext = altContext;
        }

        public List<LegalNotification> GetLegalNotificationsFromLegalCase(LegalCase legalCase)
        {
            var legalcases = _altContext.LegalNotifications.Where(x => x.LegalCaseId == legalCase.Id);
            return legalcases.ToList();
        }

        public async Task<List<LegalNotification>> getAllNotificationsUnseen(User user)
        {
            return await _altContext.LegalNotifications
                .Where(notifications => notifications.Read == false &&
                       notifications.LegalCase.UserId == user.Id)
                .ToListAsync();
        }
    }
}
