using Microsoft.EntityFrameworkCore;
using LegalTracker.DataAccess.Persistence;
using LegalTracker.Domain.Entities;

namespace LegalTracker.DataAccess.Repositories.Impl
{
    public class ActionDataAccess
    {
        #region Constructor
        private readonly ALTContext _context;

        public ActionDataAccess(ALTContext context)
        {
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
