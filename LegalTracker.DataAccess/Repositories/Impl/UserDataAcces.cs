using Microsoft.EntityFrameworkCore;
using LegalTracker.DataAccess.Persistence;
using LegalTracker.Domain.Entities;

namespace LegalTracker.DataAccess;

public class UserDataAccess
{
    private readonly ALTContext _context;

    public UserDataAccess(ALTContext context)
    {
        _context = context;
    }

    public async Task<ICollection<User>> GetUsersToScrapAsync()
    {
        return await _context.Users.Where(u => u.WebCredentialUser != null && u.WebCredentialPassword != null).ToListAsync();
    }

    internal Task UpdateLastScrappedPage(User user, int newLastScrappedPage)
    {
        user.LastScrappedPage = newLastScrappedPage;
        _context.Update(user);
        return _context.SaveChangesAsync();
    }
}
