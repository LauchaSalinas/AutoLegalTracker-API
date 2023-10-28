using LegalTracker.Domain.Entities;

namespace LegalTracker.Business;

public static class UserCommon
{
    public static GoogleAPI.GoogleAPIUserDTO ToGoogleAPI(this User user)
    {
        return new GoogleAPI.GoogleAPIUserDTO
        {
            AccessToken = user.GoogleOAuth2AccessToken,
            RefreshToken = user.GoogleOAuth2RefreshToken,
            IdToken = user.GoogleOAuth2IdToken,
            TokenCreatedAt = user.GoogleOAuth2TokenCreatedAt,
            TokenExpiration = user.GoogleOAuth2TokenExpiration
        };
    }
}
