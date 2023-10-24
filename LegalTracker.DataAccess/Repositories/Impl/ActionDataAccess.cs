using AutoLegalTracker_API.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoLegalTracker_API.DataAccess
{
    public class ActionDataAccess
    {
        #region Constructor
        private readonly IConfiguration _configuration;
        private readonly ALTContext _context;

        public ActionDataAccess(IConfiguration configuration, ALTContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        #endregion Constructor

        #region Public Methods
        public async Task<LegalCaseAction> GetActions(User user)
        {
           throw new NotImplementedException();
        }

        public async Task CreateAction(LegalCaseAction action, User user)
        {
            throw new NotImplementedException();
        }

        public async Task ExecuteAction(LegalCaseAction action, User user)
        {
            throw new NotImplementedException();
        }

        public async Task<LegalCaseAction> GetActionById(int actionId, User user)
        {
            throw new NotImplementedException();
        }

        public async Task<List<LegalCaseAction>> GetAllActionsToRunThem()
        {
            return _context.Set<LegalCaseAction>().Include(x => x.NotificationCondition).Include(x => x.LegalCaseAttributesToAdd).Where(x => x.UserTypes.Any(UserType => UserType.Name == "System")).ToList();
        }

        #endregion Public Methods
    }
}
