using AutoLegalTracker_API._5_WebServices;
using AutoLegalTracker_API.DataAccess;
using AutoLegalTracker_API.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Oauth2.v2;
using Google.Apis.Oauth2.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util;
using System.IdentityModel.Tokens.Jwt;

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

        public async Task<User> GetUser(string oneTimeTokenString)
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
            
            return user;
        }
        public async Task<User> GetUserFromSub(string userSub)
        {
            var userList = await _userAccess.Query(user => user.Sub == userSub);
            User user = userList.FirstOrDefault();
            return user;
        }

        public async Task SetScrappingCredentials(string userSub, string credentialUsername, string credentialPassword)
        {
            // get user model from sub
            // User model = send userSub
            var userList = await _userAccess.Query(user => user.Sub == userSub);
            User user = userList.FirstOrDefault() ?? throw new ApplicationException("User was not found in the database");

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
    }
}

