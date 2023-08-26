using System.Security.Claims;

using AutoLegalTracker_API.WebServices;
using AutoLegalTracker_API.DataAccess;
using AutoLegalTracker_API.Models;

namespace AutoLegalTracker_API.Business
{
    public class UserBusiness
	{
        private readonly IConfiguration _configuration;
        private readonly JwtBusiness _jwtBusiness;
        private readonly IDataAccesssAsync<User> _userAccess;
        private readonly GoogleOAuth2Service _googleOAuth2Service;

        public UserBusiness(IConfiguration configuration, JwtBusiness jwtBusiness, IDataAccesssAsync<User> userAccess, GoogleOAuth2Service _OAuth2Service)
		{
            _configuration = configuration;
            _jwtBusiness = jwtBusiness;
            _userAccess = userAccess;
            _googleOAuth2Service = _OAuth2Service;
        }

        /// <summary>
        /// Once the user has logged in with google, the API exchanges the one-time Token 
        /// for the AccessToken and RefreshToken so we can get the user info from the google api 
        /// and we search for the user in the database. If the user is not found in the database
        /// we create a new user.
        /// </summary>
        /// <param name="oneTimeTokenString"></param>
        /// <returns>User from db or new User</returns>
        public async Task<User> AddOrUpdateUser(string oneTimeTokenString)
        {
            // Exchange the one time token for a TokenResponse with an access token and a refresh token
            var credential = await _googleOAuth2Service.ExchangeToken(oneTimeTokenString);
            // Make a request to the google api to get the user info
            var meObject = _googleOAuth2Service.GetMeObject(credential);
            // Search for the user in the database
            var user = await GetUserFromSub(meObject.Id);
            // If the user is not found in the database create a new user
            if (user == null)
            {
                user = new User
                {
                    Id = 0,
                    Sub = meObject.Id,
                    Name = meObject.Name,
                    FamilyName = meObject.FamilyName,
                    Email = meObject.Email,
                    GoogleProfilePicture = meObject.Picture,
                    WebCredentialUser = null,
                    WebCredentialPassword = null,
                    GoogleOAuth2AccessToken = credential.Token.AccessToken,
                    GoogleOAuth2RefreshToken = credential.Token.RefreshToken,
                    GoogleOAuth2TokenExpiration = credential.Token.ExpiresInSeconds,
                    GoogleOAuth2TokenCreatedAt = credential.Token.IssuedUtc,
                    GoogleOAuth2IdToken = credential.Token.IdToken
                };
                await _userAccess.Insert(user);
            }
            else
            {
                await _userAccess.Update(user);
            }
            
            return user;
        }
        

        public async Task SetScrappingCredentials(User user, string credentialUsername, string credentialPassword)
        {
            // check if credentials are the same that the ones that we have stored
            if (credentialPassword == user.WebCredentialPassword && user.WebCredentialUser == credentialUsername)
                throw new ApplicationException("Credentials have already been set");

            // Test Credentials
            // Send them to the Puppeteer to get the confirmation.
            // TODO SOtero: Implement Puppeteer
            //var successfulLogin = puppetterService.login(credentialUsername, credentialPassword);
            var successfulLogin = true;

            // If they dont work return an error
            if (!successfulLogin)
                throw new ApplicationException("Given credentials did not work for login");

            // If they work store them in the database
            user.WebCredentialUser = credentialUsername;
            user.WebCredentialPassword = credentialPassword;
            try
            {
                await _userAccess.Update(user);
            }
            catch
            {
                throw new Exception();
            }
        }

        public async Task<User> GetUserFromCookie(ClaimsPrincipal claimsPrincipal)
        {
            var userSub = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await GetUserFromSub(userSub);
            return user;
        }
        private async Task<User> GetUserFromSub(string userSub)
        {
            var userList = await _userAccess.Query(user => user.Sub == userSub);
            User user = userList.FirstOrDefault();
            return user;
        }
    }
}

