using LegalTracker.Domain.Entities;

namespace LegalTracker.Application.Services;

public class ActionService
{
    public Task <LegalCaseAction> CreateAction(LegalCaseAction action, User user)
    {
        throw new NotImplementedException();
    }

    public Task ExecuteAction(LegalCaseAction action, User user, object legalCase)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<LegalCaseAction>> GetActions(User user)
    {
        await Task.Delay(1000);
        throw new NotImplementedException();
    }
}
