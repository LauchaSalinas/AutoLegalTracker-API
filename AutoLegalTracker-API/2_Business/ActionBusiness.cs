using AutoLegalTracker_API.DataAccess;
using AutoLegalTracker_API.Models;

namespace AutoLegalTracker_API.Business
{
    public class ActionBusiness
    {
        #region Constructor
        private readonly IConfiguration _configuration;
        private readonly ActionDataAccess _actionAccess;
        private readonly ConditionBusiness _conditionBusiness;
        private readonly CaseBusiness _caseBusiness;
        private readonly UserBusiness _userBusiness;
        private readonly LegalNotificationBusiness _legalNotificationBusiness;

        public ActionBusiness(IConfiguration configuration, ActionDataAccess actionAccess, ConditionBusiness conditionBusiness, CaseBusiness caseBusiness, UserBusiness userBusiness, LegalNotificationBusiness legalNotificationBusiness)
        {
            _configuration = configuration;
            _actionAccess = actionAccess;
            _conditionBusiness = conditionBusiness;
            _caseBusiness = caseBusiness;
            _userBusiness = userBusiness;
            _legalNotificationBusiness = legalNotificationBusiness;
        }

        #endregion Constructor

        public async Task<LegalCaseAction> GetActions(User user)
        {
            // here its where we check if the user has permission to get the actions
            // here its where we check if attribute has expired
            throw new NotImplementedException();
        }

        public async Task<LegalCaseAction> CreateAction(LegalCaseAction action, User user)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Method to check if the user has permission to execute the action and if so, execute the action
        /// </summary>
        /// <param name="action"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task ExecuteAction(LegalCaseAction action, User user, LegalCase legalCase)
        {
            // get the action from the database
            var actionFromDb = await _actionAccess.GetActionById(action.Id, user);
            if (actionFromDb == null)
            {
                // throw exception
                throw new NotImplementedException();
            }

            // check if the user has permission to execute the action

            bool userHasPermission = actionFromDb.UserTypeAllowedToUseAction.Exists(x => x.UserType == user.userType);
            bool conditionsAreMet = true;

            if (userHasPermission & conditionsAreMet)
            {
                // execute the action
                await ExecuteAction(action);
            }
            else
            {
                // throw exception
                throw new NotImplementedException();
            }
        }

        private bool CheckCaseConditions(LegalCaseAction action, LegalCase legalCase)
        {
            //check CaseConditions
            if (_conditionBusiness.CheckLegalCaseCondition(action.LegalCaseCondition, legalCase))
                return false;
            return false;
        }

        private bool CheckNotificationCondition(LegalCaseAction action, LegalNotification legalNotification)
        {
            //check CaseConditions
            if (_conditionBusiness.CheckLegalNotificationCondition(action.NotificationCondition, legalNotification))
                return false;
            return false;
        }

        private async Task ExecuteAction(LegalCaseAction legalCaseAction)
        {
            if (legalCaseAction.LegalResponseTemplateId != null)
            {
                // send a response
            }

            if (legalCaseAction.AssignMedicalAppointment)
            {
                // assign a medical appointment
            }

            if (legalCaseAction.EmailTemplateId != null)
            {
                // send an email
            }

            if (legalCaseAction.LegalCaseAttributesToAddOrDelete.Count > 0)
            {
                foreach (var attribute in legalCaseAction.LegalCaseAttributesToAddOrDelete)
                {
                    if (attribute.Add)
                    {
                        //legalNotification.LegalCase.LegalCaseAttributes.Add(attribute.LegalCaseAttribute);
                    }
                    else
                    {
                        //legalNotification.LegalCase.LegalCaseAttributes.Remove(attribute.LegalCaseAttribute);
                    }
                }
            }
        }

        public async Task RunActionsToNewNotifications()
        {
            // get all users
            // get all new notifications for each user
            // check if there are any actions to execute
            // if so, execute them
            var users = await _userBusiness.GetAllUsers();
            foreach (var user in users)
            {
                var legalCases = await _caseBusiness.GetCases(user);

                foreach (var legalCase in legalCases)
                {
                    var actionsToRun = await _actionAccess.GetAllActionsToRunThem(); // TODO bring only the actions for that user
                    foreach (var action in actionsToRun)
                    {
                        bool caseConditionsAreMet;
                        caseConditionsAreMet = CheckCaseConditions(action, legalCase);

                        if (caseConditionsAreMet)
                        {
                            var legalNotifications = await _legalNotificationBusiness.GetNotifications(legalCase);
                            foreach (var legalNotification in legalNotifications)
                            {
                                bool notificationConditionsAreMet;
                                notificationConditionsAreMet = CheckNotificationCondition(action, legalNotification);
                                if (notificationConditionsAreMet)
                                {
                                    await ExecuteAction(action);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
