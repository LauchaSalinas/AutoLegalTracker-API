

using AutoLegalTracker_API.Business;
using AutoLegalTracker_API.DataAccess;
using AutoLegalTracker_API.Models;
using AutoLegalTracker_API.WebServices;

namespace AutoLegalTracker_API.Business
{
    public class CalendarBusiness
    {
        #region Constructor
        private readonly IConfiguration _configuration;
        private readonly IDataAccesssAsync<Calendar> _calendarAccess;
        private readonly GoogleCalendarService _googleCalendarService;

        public CalendarBusiness(IConfiguration configuration, JwtBusiness jwtBusiness, IDataAccesssAsync<Calendar> calendarAccess, GoogleCalendarService googleCalendarService)
        {
            _configuration = configuration;
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
