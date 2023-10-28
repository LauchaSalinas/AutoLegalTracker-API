using System.Security.Claims;
using LegalTracker.Domain.Entities;

namespace LegalTracker.Application.Services;

public class UserService
{
    public Task<User> GetUserFromCookie(ClaimsPrincipal user)
    {
        throw new NotImplementedException();
    }
}
