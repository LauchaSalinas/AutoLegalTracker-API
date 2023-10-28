using LegalTracker.Domain.Entities;

namespace LegalTracker.Application.Services;

public class MedicalAppointmentService
{
    public Task<IEnumerable<MedicalAppointment>> GetEvents(User user, string calendarId, DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MedicalAppointment>> GetFreeBusy(User user, string calendarId, DateTime startDateParsed, DateTime endDateParsed)
    {
        throw new NotImplementedException();
    }
}
