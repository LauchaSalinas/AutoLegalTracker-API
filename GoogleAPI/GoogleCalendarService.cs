using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;

namespace GoogleAPI
{
    public class GoogleCalendarService
    {
        #region Constructor
        private CalendarService? _calendarService;
        private readonly GoogleOAuth2Service _googleOAuth2Service;

        public GoogleCalendarService(GoogleOAuth2Service googleOAuth2Service)
        {
            _googleOAuth2Service = googleOAuth2Service;
        }

        public GoogleCalendarService Set(GoogleAPIUserDTO user)
        {
            _calendarService = _googleOAuth2Service.Set(user).GetCalendarService();
            return this;
        }
        
        #endregion Constructor

        #region Calendar Methods

        public async Task<Google.Apis.Calendar.v3.Data.Calendar> CreateCalendar(string calendarName)
        {
            // Create a calendar
            var calendar = new Google.Apis.Calendar.v3.Data.Calendar()
            {
                Summary = calendarName
            };
            var request = _calendarService.Calendars.Insert(calendar);
            var response = await request.ExecuteAsync();
            return response;
        }

        public async Task<List<Google.Apis.Calendar.v3.Data.Event>> GetCalendarEvents(string calendarId, DateTime start, DateTime end)
        {
            // convet to datetime offset
            var startOffset = new DateTimeOffset(start);
            var endOffset = new DateTimeOffset(end);

            // Create an event
            var request = _calendarService.Events.List(calendarId);
            request.TimeMin = startOffset.UtcDateTime;
            request.TimeMax = endOffset.UtcDateTime;
            var response = await request.ExecuteAsync();
            return response.Items.ToList();
        }

        public async Task<List<Google.Apis.Calendar.v3.Data.CalendarListEntry>> GetCalendars()
        {
            // Create an event
            var request = _calendarService.CalendarList.List();
            try
            {
                var response = await request.ExecuteAsync();
                return response.Items.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion Calendar Methods

        #region Event Methods

        // get freebusy
        public async Task<FreeBusyResponse> GetFreeBusy(string calendarId, DateTime start, DateTime end)
        {
            // convet to datetime offset
            var startOffset = new DateTimeOffset(start);
            var endOffset = new DateTimeOffset(end);

            // Create an event
            var request = _calendarService.Freebusy.Query(new FreeBusyRequest()
            {
                TimeMinDateTimeOffset = startOffset,
                TimeMaxDateTimeOffset = endOffset,
                Items = new List<FreeBusyRequestItem>()
                {
                    new FreeBusyRequestItem()
                    {
                        Id = calendarId
                    }
                }
            });
            var response = await request.ExecuteAsync();
            return response;
        }

        public async Task<Event> CreateEvent(string calendarId, string eventName, DateTime eventStart, DateTime eventEnd)
        {
            // Create an event
            var newEvent = new Event()
            {
                Summary = eventName,
                Start = new EventDateTime()
                {
                    DateTime = eventStart,
                    TimeZone = "America/Argentina/Buenos_Aires"
                },
                End = new EventDateTime()
                {
                    DateTime = eventEnd,
                    TimeZone = "America/Argentina/Buenos_Aires"
                }
            };
            var request = _calendarService.Events.Insert(newEvent, calendarId);
            var response = await request.ExecuteAsync();
            return response;
        }

        #endregion Event Methods
    }
}
