using AutoLegalTracker_API.Models;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Apis.Util;
using Google.Apis.Services;
using System.Net;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Oauth2.v2;
using Google.Apis.Oauth2.v2.Data;

public class OneTimeToken
{
    public string oneTimeToken { get; set; }
}

namespace ALTDeployTest.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UserController : ControllerBase
    {
        private IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        [Route("login")]
        public dynamic LogIn([FromBody] object optData)
        {
            if (optData == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            // Deserialize JSON data from post request
            OneTimeToken data = JsonConvert.DeserializeObject<OneTimeToken>(optData.ToString());
            string oneTimeTokenString = data.oneTimeToken;

            // MISING VALIDATION OF TOKEN

            // Exchange google token for acces token and refresh token
            // MISING SET VARIABLES IN APPSETTINGS.JSON
            string clientID = _configuration["OAUTH2_CLIENT_ID"] ?? string.Empty;
            string clientSecret = _configuration["OAUTH2_CLIENT_SECRET"] ?? string.Empty;
            string redirectUri = _configuration["OAUTH2_REDIRECT_URI"] ?? string.Empty;

            Console.WriteLine("clientID" + clientID);
            Console.WriteLine("clientSecret" + clientSecret);
            Console.WriteLine("redirectUri" + redirectUri);


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

            // Make a request to the google api to get the user info
            Userinfo meObject = new UserinfoResource.V2Resource.MeResource.GetRequest(meService).Execute();

            // MISING VALIDATION OF USER INFO
            // MISING TRY CATCH FOR EXCEPTIONS

            // Get the JWT configuration from the appsettings.json file
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
                // create custom claims
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

            // Return the JWT token
            return StatusCode(StatusCodes.Status200OK, new { token = new JwtSecurityTokenHandler().WriteToken(jwtToken) });
        }
    }
}
