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

                var casesWithPendingEvents = await _context.Set<LegalCase>()
                    .Where(legalCase => legalCase.userId == user.Id)
                    .Include(legalCase => legalCase.LegalNotifications.Select(notification =>
                        notification.MedicalAppointments.Where(appointment =>
                            appointment.StartDate >= nextWeekStart && appointment.StartDate < nextWeekEnd)))
                    .ToListAsync();

                return casesWithPendingEvents;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while getting cases with pending events for next week.", ex);
            }
        }

        // Other methods for LegalCase entity using _context
    }
}
