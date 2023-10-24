using LegalTracker.DataAccess;
using LegalTracker.Domain.Entities;

namespace AutoLegalTracker_API.Business
{
    public class CalendarBusiness
    {
        #region Constructor
        private readonly IDataAccesssAsync<Calendar> _calendarAccess;
        private readonly GoogleCalendarService _googleCalendarService;

        public CalendarBusiness( JwtBusiness jwtBusiness, IDataAccesssAsync<Calendar> calendarAccess, GoogleCalendarService googleCalendarService)
        {
            _calendarAccess = calendarAccess;
            _googleCalendarService = googleCalendarService;
        }
        #endregion Constructor

        #region Public Methods
        // get calendars of user
        public async Task<List<Google.Apis.Calendar.v3.Data.CalendarListEntry>> GetCalendars(User user)
        {
            // get calendars from calendar service
            var calendars = await _googleCalendarService.Set(user).GetCalendars();

            // get the string ids of each calendar
            // var calendarIds = calendars.Select(calendar => calendar.Id).ToList();

            // return calendars
            return calendars;
        }
        #endregion Public Methods
    }
}
