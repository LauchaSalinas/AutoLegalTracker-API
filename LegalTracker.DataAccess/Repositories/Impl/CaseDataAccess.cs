using LegalTracker.Domain.Entities;
using LegalTracker.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LegalTracker.DataAccess.Repositories.Impl
{
    public class LegalCaseDataAccessAsync
    {
        private readonly ALTContext _context;

        public LegalCaseDataAccessAsync(ALTContext context)
        {
            _context = context;
        }

        public async Task<List<LegalCase>> GetCasesWithPendingEventsNextWeek(User user)
        {
            try
            {
                // Calculate the start and end dates for next week
                DateTime nextWeekStart = DateTime.Today.AddDays(7 - (int)DateTime.Today.DayOfWeek);
                DateTime nextWeekEnd = nextWeekStart.AddDays(7);

                    var casesWithPendingEvents = await _context.LegalCases
                        .Where(legalCase => legalCase.UserId == user.Id)
                        .Include(legalCase => legalCase.LegalNotifications)
                        .ThenInclude(notification => notification.MedicalAppointment)
                        .Where(legalCase => legalCase.LegalNotifications
                            .Any(notification => notification.MedicalAppointment != null && 
                                                notification.MedicalAppointment.StartDate >= nextWeekStart && 
                                                notification.MedicalAppointment.StartDate < nextWeekEnd))
                        .ToListAsync();
                return casesWithPendingEvents;

                // That query on top its the same as this one:
                /*
                SELECT [c].[Id], [c].[Caption], [c].[Description], [c].[CreatedAt], [c].[UpdatedAt], [c].[ClosedAt], [c].[PossibleCourtDate], [c].[UserId], [c].[Jurisdiction], [c].[CaseNumber], [c].[ExpenseAdvancesPaid]
                FROM [LegalCases] AS [c]
                WHERE ([c].[UserId] = @__user_Id_0) AND EXISTS (
                    SELECT 1
                    FROM [LegalNotifications] AS [l]
                    LEFT JOIN [MedicalAppointments] AS [m] ON [l].[MedicalAppointmentId] = [m].[Id]
                    WHERE [c].[Id] = [l].[LegalCaseId] AND ([m].[StartDate] >= @__nextWeekStart_1) AND ([m].[StartDate] < @__nextWeekEnd_2) AND [m].[Id] IS NOT NULL
                )
                
                */
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while getting cases with pending events for next week.", ex);
            }
        }

        public async Task<LegalCase> GetCaseById(int legalCaseId)
        {
            return await _context.LegalCases.SingleAsync(legalCase => legalCase.Id == legalCaseId);
        }

        public async Task<List<LegalCase>> GetCasesByUserId(User user)
        {
            //Disabling Eager Loading with AsNoTracking
            return await _context.LegalCases
                .Where(legalCase => legalCase.UserId == user.Id && legalCase.ClosedAt == null)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<LegalCase?> GetLastScrappedLegalCase(int userId)
        {
            return await _context.LegalCases.Where(legalCase => legalCase.UserId == userId).OrderByDescending(legalCase => legalCase.CreatedAt).FirstOrDefaultAsync();
        }

        public async Task AddRangeAsync(IEnumerable<LegalCase> legalCasesToAdd)
        {
            await _context.LegalCases.AddRangeAsync(legalCasesToAdd);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LegalCase>> GetCasesToScrap(int userId)
        {
            var dateToFilter = DateTime.Now.AddDays(-1);
            return await _context.LegalCases.Where(
                legalCase => legalCase.UserId == userId && 
                legalCase.ClosedAt == null && 
                (!legalCase.LastScrappedAt.HasValue || legalCase.LastScrappedAt.Value < dateToFilter) &&
                !_context.LegalNotifications.Select(ln => ln.LegalCaseId).Contains(legalCase.Id))
                .ToListAsync();
        }

        internal async Task<bool> hasMoreLegalNotificationsToFill(int userId)
        {
            
            return await _context.LegalNotifications.AnyAsync(ln => 
                                                                ln.Body == null 
                                                                && ln.LegalCase.User.Id == userId
                                                                );
        }

        internal async Task<IEnumerable<LegalCase>> GetCasesWithEmptyNotifications(int userId)
        {
            var dateToFilter = DateTime.Now.AddDays(-5);

            var result = from lc in _context.LegalCases
                         join ln in _context.LegalNotifications on lc.Id equals ln.LegalCaseId
                         where ln.ScrapUrl != null
                            && ln.Body == null
                            && lc.UserId == userId
                            && lc.ClosedAt == null
                            // && (ln.LegalCaseLastScrappedAt == null || ln.LegalCaseLastScrappedAt > dateToFilter)
                            && ln.NotificationDate > dateToFilter
                         select lc;

            return await result.ToListAsync();
        }

        internal Task UpdateLastScrappedAt(LegalCase legalCase)
        {
            legalCase.LastScrappedAt = DateTime.Now;
            return _context.SaveChangesAsync();
        }

        //make function to find by list of cases
        public async Task<List<LegalCase>> GetCasesByScrapUrl(List<string> caseScrapUrl)
        {
            return await _context.LegalCases.Where(legalCase => caseScrapUrl.Contains(legalCase.ScrapUrl)).ToListAsync();
        }

        internal LegalCase AddOrUpdate(LegalCase legalcaseToUpdate)
        {
            // check if the case exist to update or add it if doesnt exists
            var legalCase = _context.LegalCases.Where(x => x.ScrapUrl == legalcaseToUpdate.ScrapUrl).FirstOrDefault();
            if (legalCase != null)
            {
                legalCase.Caption = legalcaseToUpdate.Caption;
                legalCase.Description = legalcaseToUpdate.Description;
                legalCase.CaseNumber = legalcaseToUpdate.CaseNumber;
                legalCase.Jurisdiction = legalcaseToUpdate.Jurisdiction;
                legalCase.UpdatedAt = DateTime.Now;
            }
            else
            {
                _context.LegalCases.Add(legalcaseToUpdate);
            }
            _context.SaveChanges();
            // get the id of the updated case or the new case
            legalcaseToUpdate = _context.LegalCases.Where(x => x.ScrapUrl == legalcaseToUpdate.ScrapUrl).FirstOrDefault();
            return legalcaseToUpdate;
        }

        //public async Task<List<LegalCase>> getAllNotificationsUnseen(User user)
        //{
        //    //Disabling Eager Loading with AsNoTracking
        //    return await _context.LegalCases
        //        .Where(legalCase => legalCase.UserId == user.Id && legalCase.ClosedAt == null)
        //        .Include(legalCase => legalCase.)

        //        .ToListAsync();
        //}

        /*
           SELECT COUNT(LegalNotifications.Id)
           FROM LegalNotifications, LegalCases, Users
           WHERE LegalNotifications.LegalCaseId = LegalCases.Id AND
                 LegalCases.UserId = Users.Id AND
                 UserId = 5
           GROUP BY LegalCases.Id 
        */
        // Other methods for LegalCase entity using _context
    }
}
