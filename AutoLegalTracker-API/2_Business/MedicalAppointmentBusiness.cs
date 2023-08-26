using Google.Apis.Calendar.v3.Data;

using AutoLegalTracker_API.Models;
using AutoLegalTracker_API.WebServices;
using AutoLegalTracker_API.DataAccess;

namespace AutoLegalTracker_API.Business
{
    public class MedicalAppointmentBusiness
    {
        #region Constructor
        private readonly IConfiguration _configuration;
        private readonly IDataAccesssAsync<MedicalAppointment> _medicalAppointmentAccess;
        private readonly GoogleCalendarService _googleCalendarService;

        public MedicalAppointmentBusiness(IConfiguration configuration, JwtBusiness jwtBusiness, IDataAccesssAsync<MedicalAppointment> medicalAppointmentAccess, GoogleCalendarService googleCalendarService)
        {
            _configuration = configuration;
            _medicalAppointmentAccess = medicalAppointmentAccess;
            _googleCalendarService = googleCalendarService;

        }
        #endregion Constructor

        #region Public Methods
        // get freebusy of user from calendar id and date start date end
        public async Task<FreeBusyResponse> GetFreeBusy(User user, string calendarId, DateTime startDate, DateTime endDate)
        {
            // get freebusy from calendar service
            var freeBusy = await _googleCalendarService.Set(user).GetFreeBusy(calendarId, startDate, endDate);

            // return freebusy
            return freeBusy;
        }

        //get events from calendar id and date start date end
        public async Task<List<Event>> GetEvents(User user, string calendarId, DateTime startDate, DateTime endDate)
        {
            // get events from calendar service
            var events = await _googleCalendarService.Set(user).GetCalendarEvents(calendarId, startDate, endDate);

            // return events
            return events;
        }
        #endregion Public Methods
    }
}
