using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2.Data;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using Google.Apis.Util;
using Google.Apis.Drive.v3;
using Google.Apis.Calendar.v3;

using AutoLegalTracker_API.Models;

namespace AutoLegalTracker_API.WebServices
{
    public class GoogleOAuth2Service
    {
        #region Constructor

        private readonly IConfiguration _configuration;
        private readonly GoogleAuthorizationCodeFlow _flow; // Flow: https://frontegg.com/blog/oauth-flows
        private UserCredential? _credential;

        public GoogleOAuth2Service(IConfiguration configuration)
        {
            _configuration = configuration;
            _flow = GetGoogleAuthorizationCodeFlow();
        }

        public GoogleOAuth2Service Set(User user)
        {
            var tokenResponse = GetTokenResponse(user.GoogleOAuth2AccessToken, user.GoogleOAuth2RefreshToken, user.GoogleOAuth2IdToken);
            _credential = GetUserCredential(_flow, tokenResponse);
            return this;
        }

        private GoogleAuthorizationCodeFlow GetGoogleAuthorizationCodeFlow()
        {
            string clientID = _configuration["OAUTH2_CLIENT_ID"] ?? string.Empty;
            string clientSecret = _configuration["OAUTH2_CLIENT_SECRET"] ?? string.Empty;

            // Define the scopes for the google api
            string[] scopes = new string[]
            {
                Oauth2Service.Scope.UserinfoEmail,
                Oauth2Service.Scope.UserinfoProfile,
                DriveService.Scope.Drive,
                CalendarService.Scope.Calendar
            };

            // Define the google authentification flow and initialize it with the clientID and clientSecret
            GoogleAuthorizationCodeFlow.Initializer initializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets() { ClientId = clientID, ClientSecret = clientSecret },
                Scopes = scopes
            };
            GoogleAuthorizationCodeFlow flow = new GoogleAuthorizationCodeFlow(initializer);

            return flow;
        }
        #endregion Constructor

        #region GetServices for each API

        public CalendarService GetCalendarService()
        {
            string clientID = _configuration["OAUTH2_CLIENT_ID"] ?? string.Empty;
            var calendarService = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credential,
                ApplicationName = clientID
            });
            return calendarService;
        }

        public DriveService GetDriveService()
        {
            string clientID = _configuration["OAUTH2_CLIENT_ID"] ?? string.Empty;
            var driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credential,
                ApplicationName = clientID
            });
            return driveService;
        }

        #endregion GetServices for each API

        #region ExchangeToken
        public async Task<UserCredential> ExchangeToken(string oneTimeTokenString)
        {
            // Exchange the one time token for a TokenResponse with an access token and a refresh token
            var tokenResponse = await GetTokenResponseAsync(oneTimeTokenString);
            var credential = GetUserCredential(_flow, tokenResponse);

            return credential;
        }
        private async Task<TokenResponse> GetTokenResponseAsync(string oneTimeTokenString)
        {
            string clientID = _configuration["OAUTH2_CLIENT_ID"] ?? string.Empty;
            string clientSecret = _configuration["OAUTH2_CLIENT_SECRET"] ?? string.Empty;
            string redirectUri = _configuration["OAUTH2_REDIRECT_URI"] ?? string.Empty;

            TokenResponse tokenResponse = await new AuthorizationCodeTokenRequest
            {
                Code = oneTimeTokenString,
                ClientId = clientID,
                ClientSecret = clientSecret,
                RedirectUri = redirectUri
            }.ExecuteAsync(new HttpClient(), GoogleAuthConsts.OidcTokenUrl, CancellationToken.None, SystemClock.Default);
            return tokenResponse;
        }
        private TokenResponse GetTokenResponse(string accessToken, string refreshToken, string tokenId)
        {
            var token = new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                IdToken = tokenId,
            };
            // check if the token is expired
            // TODO is saying its expired when it's not, need to check this
            if (token.IsExpired(SystemClock.Default))
            {
                // if the token is expired, refresh it
                token = RefreshToken(token);
            }

            return token;
        }
        // create refresh token from access token
        private TokenResponse RefreshToken(TokenResponse token)
        {
            string clientID = _configuration["OAUTH2_CLIENT_ID"] ?? string.Empty;
            string clientSecret = _configuration["OAUTH2_CLIENT_SECRET"] ?? string.Empty;

            var initializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets() { ClientId = clientID, ClientSecret = clientSecret },
            };
            var flow = new GoogleAuthorizationCodeFlow(initializer);
            var credential = new UserCredential(flow, "user", token);
            var refreshToken = credential.Token.RefreshToken;
            var newToken = credential.RefreshTokenAsync(CancellationToken.None).Result;
            return credential.Token;
        }
        private UserCredential GetUserCredential(GoogleAuthorizationCodeFlow flow, TokenResponse tokenResponse)
        {
            // Define a UserCredential with the flow and the tokenResponse to be able to make requests to the google api
            var credential = new UserCredential(
            flow,
            "user",
            tokenResponse);

            return credential;
        }
        #endregion ExchangeToken

        #region User API
        public Userinfo GetMeObject(UserCredential credential)
        {
            string clientID = _configuration["OAUTH2_CLIENT_ID"] ?? string.Empty;
            // Define a service to make requests to the google api
            var meService = new Oauth2Service(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = clientID
            });

            // Make a request to the google api to get the user info
            Userinfo meObject = meService.Userinfo.V2.Me.Get().Execute();
            return meObject;
        }
        #endregion User API

    }
}
