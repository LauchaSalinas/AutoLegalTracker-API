using LegalTracker.DataAccess.Persistence;
using LegalTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LegalTracker.DataAccess.Repositories.Impl
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

        public async Task AddRangeAsync(IEnumerable<LegalNotification> legalNotificationsToAdd)
        {
            _altContext.AddRangeAsync(legalNotificationsToAdd);
            await _altContext.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<LegalNotification> legalNotificationsToUpdate)
        {
            _altContext.UpdateRange(legalNotificationsToUpdate);
            await _altContext.SaveChangesAsync();
        }

        public async Task<LegalNotification> GetLastNotification(int CaseId)
        {
            return await _altContext.LegalNotifications
                .Where(x => x.LegalCaseId == CaseId)
                .OrderByDescending(x => x.NotificationDate)
                .ThenBy(x => x.CreatedAt)
                .FirstOrDefaultAsync();
        }

        internal async Task<IEnumerable<LegalNotification>> GetNotificationsToFill()
        {
            return await _altContext.LegalNotifications
                .Where(x => x.Body == null)
                .Take(500).ToListAsync();
        }

        internal async Task<IEnumerable<LegalNotification>> GetEmptyNotifications(int legalCaseId)
        {
            return await _altContext.LegalNotifications
                    .Where(
                        ln => ln.LegalCaseId == legalCaseId 
                        && ln.Body == null
                        && ln.NotificationDate > DateTime.Now.AddDays(-5)
                        && ln.ScrapUrl != null
                    )
                    .ToListAsync();
        }
    }
}
