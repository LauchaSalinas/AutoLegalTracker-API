using AutoLegalTracker_API.Models;
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

        public async Task<List<LegalNotification>> GetLegalNotificationsFromLegalCase(LegalCase legalCase)
        {
            return await _altContext.LegalNotifications.Where(x => x.LegalCaseId == legalCase.Id).ToListAsync();
        }
    }
}
