using LegalTracker.DataAccess.Repositories;
using LegalTracker.DataAccess.Repositories.Impl;

using LegalTracker.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace LegalTracker.Business
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
            var cases = await _legalCaseDataAccess.GetCasesByUserId(user);

            // return cases
            return cases;
        }

        public async Task<List<LegalCase>> GetAutomatedCases(User user)
        {
            // get cases from database
            var cases = await _legalCaseAccessGeneric.Query(legalCase => legalCase.UserId == user.Id && legalCase.ClosedAt == null);
            return cases.ToList();
        }

        public async Task<List<LegalCase>> GetCasesWithPendingEventsNextWeek(User user)
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

        public async Task<List<LegalCase>> GetNewCasesInThisMonth(User user)
        {
            try
            {
                //TODO terminar metodo nuevo casos del mes
                // Current Month
                var CurrentMonth = DateTime.Now.Month;

                //Checking 
                var casesCreatedInThisMonth = await _legalCaseAccessGeneric.Query(legalCase => legalCase.CreatedAt.Month == CurrentMonth && legalCase.UserId == user.Id); 

                return casesCreatedInThisMonth.ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while getting cases with the number of new cases in this month.", ex);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void GetActionsForCase(){}

        public async Task<LegalCase> GetCaseById(User user, int id)
        {
            // get case from database
            var legalCase = await _legalCaseAccessGeneric.Query(legalCase => legalCase.Id == id && legalCase.UserId == user.Id);
            return legalCase.FirstOrDefault();
        }

        #endregion Private Methods

    }
}
