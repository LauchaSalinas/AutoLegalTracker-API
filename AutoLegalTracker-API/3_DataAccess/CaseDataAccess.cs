using AutoLegalTracker_API.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoLegalTracker_API.DataAccess
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
            return await _context.LegalNotifications.AnyAsync(ln => ln.Body == null && ln.LegalCase.User.Id == userId);
        }

        internal async Task<IEnumerable<LegalCase>> GetCasesWithEmptyNotifications()
        {
            return await _context.LegalCases
            .Where(
                lc => lc.LegalNotifications
                    .Any(
                        ln => ln.ScrapUrl != null 
                        && ln.Body == null
                        )
                )
            .ToListAsync();
        }

        internal Task UpdateLastScrappedAt(LegalCase legalCase)
        {
            legalCase.LastScrappedAt = DateTime.Now;
            return _context.SaveChangesAsync();
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
