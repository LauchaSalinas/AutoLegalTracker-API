using LegalTracker.Domain.Entities;

namespace LegalTracker.Application.Services;

public class CaseService
{
    public async Task<LegalCase> GetCaseById(User user, int legalCaseId)
    {
        await Task.Delay(1000);
        throw new NotImplementedException();
    }

    public Task<IEnumerable<LegalCase>> GetCases(User user)
    {
        throw new NotImplementedException();
    }
}
