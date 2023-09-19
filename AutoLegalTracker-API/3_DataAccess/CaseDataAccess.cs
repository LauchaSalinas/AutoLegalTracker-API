﻿using AutoLegalTracker_API.Models;
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

        // Other methods for LegalCase entity using _context
    }
}
