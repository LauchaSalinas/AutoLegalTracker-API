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

        public UserBusiness(IConfiguration configuration, JwtBusiness jwtBusiness)
		{
            _configuration = configuration;
            _jwtBusiness = jwtBusiness;
		}

		public async Task<string> ExchangeToken (string oneTimeTokenString)
		{
            string clientID = _configuration["OAUTH2_CLIENT_ID"] ?? string.Empty;
            string clientSecret = _configuration["OAUTH2_CLIENT_SECRET"] ?? string.Empty;
            string redirectUri = _configuration["OAUTH2_REDIRECT_URI"] ?? string.Empty;

            // Exchange the one time token for a TokenResponse with an access token and a refresh token
            TokenResponse tokenResponse = new AuthorizationCodeTokenRequest
            {
                Code = oneTimeTokenString,
                ClientId = clientID,
                ClientSecret = clientSecret,
                RedirectUri = redirectUri,
            }.ExecuteAsync(new HttpClient(), GoogleAuthConsts.OidcTokenUrl, CancellationToken.None, SystemClock.Default).Result;

            // Define the google authentification flow and initialize it with the clientID and clientSecret
            GoogleAuthorizationCodeFlow.Initializer initializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets() { ClientId = clientID, ClientSecret = clientSecret },
            };
            GoogleAuthorizationCodeFlow flow = new GoogleAuthorizationCodeFlow(initializer);

            // Define a UserCredential with the flow and the tokenResponse to be able to make requests to the google api
            UserCredential credential = new UserCredential(
            flow,
            "user",
            tokenResponse);

            // Define a service to make requests to the google api
            var meService = new Oauth2Service(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = clientID
            });

            //get the audience from the google api
            //var tokenInfo = meService.Tokeninfo().Execute();
            //tokenInfo.ExpiresIn

            // Make a request to the google api to get the user info
            Userinfo meObject = new UserinfoResource.V2Resource.MeResource.GetRequest(meService).Execute();

            // TODO: Missing validation of the user info
            // TODO: Mising try catch
            // TODO: Create custom exception
            // TODO: Missing logging

            // TODO: Missing saving of the user info in the database
            var user = new User
            {
                Id = 0,
                Sub = meObject.Id,
                Name = String.Concat(meObject.Name, " ", meObject.FamilyName),
                Email = meObject.Email,
                WebCredentialUser = null,
                WebCredentialPassword = null
            };
            var userList = await _userAccess.Query(user => user.Sub == meObject.Id);
            User userFound = userList.FirstOrDefault() ?? throw new ApplicationException("User was not found in the database");


            var jwtToken = _jwtBusiness.CreateJwt(meObject);

            var returnToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);


            return returnToken;
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

