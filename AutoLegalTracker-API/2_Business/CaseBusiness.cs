using AutoLegalTracker_API.DataAccess;
using AutoLegalTracker_API.WebServices;

using AutoLegalTracker_API.Models;
using Microsoft.EntityFrameworkCore;
using Google.Apis.Drive.v3.Data;

namespace AutoLegalTracker_API.Business
{
    public class CaseBusiness
    {
        #region Constructor
        private readonly IConfiguration _configuration;
        private readonly IDataAccesssAsync<LegalCase> _legalCaseAccessGeneric;
        private readonly LegalCaseDataAccessAsync _legalCaseDataAccess;

        public CaseBusiness(IConfiguration configuration, IDataAccesssAsync<LegalCase> legalCaseAccessGeneric, LegalCaseDataAccessAsync legalCaseDataAccess)
        {
            _configuration = configuration;
            _legalCaseAccessGeneric = legalCaseAccessGeneric;
            _legalCaseDataAccess = legalCaseDataAccess;
        }
        #endregion Constructor

        #region Public Methods
        
        public async Task<List<LegalCase>> GetCases(User user)
        {
            // get cases from database
            var cases = await _legalCaseAccessGeneric.Query(legalCase => legalCase.UserId == user.Id);

            // return cases
            return cases.ToList();
        }

        public async Task<List<LegalCase>> GetAutomatedCases(Models.User user)
        {
            // get cases from database
            var cases = await _legalCaseAccessGeneric.Query(legalCase => legalCase.UserId == user.Id && legalCase.ClosedAt == null);
            return cases.ToList();
        }

        public async Task<List<LegalCase>> GetCasesWithPendingEventsNextWeek(Models.User user)
        {
            try
            {
                // Calculate the start and end dates for next week
                DateTime nextWeekStart = DateTime.Today.AddDays(7 - (int)DateTime.Today.DayOfWeek);
                DateTime nextWeekEnd = nextWeekStart.AddDays(7);

                // Get cases from the database along with their related calendar events for next week
                var casesWithPendingEvents = await _legalCaseDataAccess.GetCasesWithPendingEventsNextWeek(user);

                return casesWithPendingEvents.ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while getting cases with pending events for next week.", ex);
            }
        }

        public async Task<List<LegalCase>> GetNewCasesInThisMonth()
        {
            try
            {
                //TODO terminar metodo nuevo casos del mes
                // First day month
                var FirstDayOfThisMonth = DateTime.Now.Month;


                var casesCreatedInThisMonth = await _legalCaseAccessGeneric.Query(legalCase => legalCase.CreatedAt.Month == FirstDayOfThisMonth); 

                return casesCreatedInThisMonth.ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while getting cases with pending events for next week.", ex);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void GetActionsForCase(){}

        #endregion Private Methods

    }
}
