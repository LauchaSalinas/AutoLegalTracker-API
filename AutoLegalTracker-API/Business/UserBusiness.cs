using System;
using AutoLegalTracker_API.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Oauth2.v2;
using Google.Apis.Oauth2.v2.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Apis.Util;

namespace AutoLegalTracker_API.Business
{
	public class UserBusiness
	{
        private IConfiguration _configuration;
		public UserBusiness(IConfiguration configuration)
		{
            _configuration = configuration;
		}

		public string ExchangeToken (string oneTimeTokenString)
		{
            // Google OAuth2 class - layer business
            // Exchange google token for acces token and refresh token
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

            // Google OAuth2 class - layer business
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

            // Make a request to the google api to get the user info
            Userinfo meObject = new UserinfoResource.V2Resource.MeResource.GetRequest(meService).Execute();

            // MISING VALIDATION OF USER INFO
            // MISING TRY CATCH FOR EXCEPTIONS


            // JWT class - business
            // Get the JWT configuration from the appsettings.json file
            // TODO check the "section" part
            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

            // Create the claims for the JWT token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, meObject.Email),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, meObject.GivenName),
                new Claim(JwtRegisteredClaimNames.FamilyName, meObject.FamilyName),
                // TODO create custom claims
                new Claim("Environment", _configuration["ASPNETCORE_ENVIRONMENT"]??string.Empty)
            };

            // Create the key and the signing credentials for the JWT token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the JWT token
            var jwtToken = new JwtSecurityToken(
                    issuer: _configuration["JWT_ISSUER"] ?? String.Empty,
                    audience: _configuration["JWT_AUDIENCE"] ?? String.Empty,
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: signIn
                );

            var returnToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return returnToken;
        }
    }
}

