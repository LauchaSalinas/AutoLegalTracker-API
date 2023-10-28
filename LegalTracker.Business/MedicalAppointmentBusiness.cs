using Google.Apis.Calendar.v3.Data;
using LegalTracker.DataAccess.Repositories;
using LegalTracker.Domain.Entities;
using GoogleAPI;


namespace LegalTracker.Business
{
    public class MedicalAppointmentBusiness
    {
        #region Constructor
        private readonly IDataAccesssAsync<MedicalAppointment> _medicalAppointmentAccess;
        private readonly GoogleCalendarService _googleCalendarService;

        public MedicalAppointmentBusiness(JwtBusiness jwtBusiness, IDataAccesssAsync<MedicalAppointment> medicalAppointmentAccess, GoogleCalendarService googleCalendarService)
        {
            _medicalAppointmentAccess = medicalAppointmentAccess;
            _googleCalendarService = googleCalendarService;

        }
        #endregion Constructor

        #region Public Methods
        // get freebusy of user from calendar id and date start date end
        public async Task<FreeBusyResponse> GetFreeBusy(User user, string calendarId, DateTime startDate, DateTime endDate)
        {
            // get freebusy from calendar service
            var freeBusy = await _googleCalendarService.Set(user.ToGoogleAPI()).GetFreeBusy(calendarId, startDate, endDate);

            // return freebusy
            return freeBusy;
        }

        //get events from calendar id and date start date end
        public async Task<List<Event>> GetEvents(User user, string calendarId, DateTime startDate, DateTime endDate)
        {
            // get events from calendar service
            var events = await _googleCalendarService.Set(user.ToGoogleAPI()).GetCalendarEvents(calendarId, startDate, endDate);

            // return events
            return events;
        }
        #endregion Public Methods

        #region MedicalAppointmentAssignation Methods

        // THIS IS FROM COPILOT, DONT DELETE YET, USING AS A REFERENCE 
        //public async Task<MedicalAppointment> AssignMedicalAppointment(User user, MedicalAppointment medicalAppointment)
        //{
        //    // get freebusy from calendar service
        //    var freeBusy = await _googleCalendarService.Set(user).GetFreeBusy(medicalAppointment.CalendarId, medicalAppointment.StartDate, medicalAppointment.EndDate);

        //    // check if freebusy is available
        //    if (freeBusy != null && freeBusy.Calendars != null && freeBusy.Calendars.Count > 0)
        //    {
        //        // get calendar from freebusy
        //        var calendar = freeBusy.Calendars[medicalAppointment.CalendarId];

        //        // check if calendar is available
        //        if (calendar != null && calendar.Busy != null && calendar.Busy.Count == 0)
        //        {
        //            // create event
        //            var eventCreated = await _googleCalendarService.Set(user).CreateCalendarEvent(medicalAppointment.CalendarId, medicalAppointment.Title, medicalAppointment.Description, medicalAppointment.StartDate, medicalAppointment.EndDate);

        //            // check if event was created
        //            if (eventCreated != null)
        //            {
        //                // assign calendar event id to medical appointment
        //                medicalAppointment.CalendarEventId = eventCreated.Id;

        //                // save medical appointment
        //                var medicalAppointmentSaved = await _medicalAppointmentAccess.Create(medicalAppointment);

        //                // return medical appointment
        //                return medicalAppointmentSaved;
        //            }
        //        }
        //    }

        //    // return null
        //    return null;
        //}

        // Complete Cron MedAppments Job flow
        // 1. Get all the LegalNotifications from the database
        // 2. Check if the legal notification has an associated LegalAutomation


        #endregion
    }
}
