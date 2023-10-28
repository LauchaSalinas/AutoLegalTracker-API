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

namespace GoogleAPI
{
    public class GoogleOAuth2Service
    {
        #region Constructor

        private readonly GoogleApiSettings _googleApiSettings;
        private readonly GoogleAuthorizationCodeFlow _flow; // Flow: https://frontegg.com/blog/oauth-flows
        private UserCredential? _credential;

        public GoogleOAuth2Service(GoogleApiSettings googleApiSettings)
        {
            _googleApiSettings = googleApiSettings;
            _flow = GetGoogleAuthorizationCodeFlow();
        }

        public GoogleOAuth2Service Set(GoogleAPIUserDTO user)
        {
            var tokenResponse = GetTokenResponse(user.AccessToken, user.RefreshToken, user.IdToken, user.TokenCreatedAt, user.TokenExpiration);
            _credential = GetUserCredential(_flow, tokenResponse);
            return this;
        }

        private GoogleAuthorizationCodeFlow GetGoogleAuthorizationCodeFlow()
        {
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
                ClientSecrets = new ClientSecrets() { ClientId = _googleApiSettings.ClientID, ClientSecret = _googleApiSettings.ClientSecret },
                Scopes = scopes
            };
            GoogleAuthorizationCodeFlow flow = new GoogleAuthorizationCodeFlow(initializer);

            return flow;
        }
        #endregion Constructor

        #region GetServices for each API

        public CalendarService GetCalendarService()
        {
            string clientID = _googleApiSettings.ClientID ?? string.Empty;
            var calendarService = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credential,
                ApplicationName = clientID
            });
            return calendarService;
        }

        public DriveService GetDriveService()
        {
            string clientID = _googleApiSettings.ClientID ?? string.Empty;
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
            TokenResponse tokenResponse = await new AuthorizationCodeTokenRequest
            {
                Code = oneTimeTokenString,
                ClientId = _googleApiSettings.ClientID,
                ClientSecret = _googleApiSettings.ClientSecret,
                RedirectUri = _googleApiSettings.RedirectURI
            }.ExecuteAsync(new HttpClient(), GoogleAuthConsts.OidcTokenUrl, CancellationToken.None, SystemClock.Default);
            return tokenResponse;
        }
        private TokenResponse GetTokenResponse(string accessToken, string refreshToken, string tokenId, DateTime issuedAt, long expiresInSeconds)
        {
            var token = new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                IdToken = tokenId,
                IssuedUtc = issuedAt,
                ExpiresInSeconds = expiresInSeconds
            };
            // check if the token is expired
            // TODO LSalinas/JGonzalez is saying its expired when it's not, need to check this
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
            var initializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets() { ClientId = _googleApiSettings.ClientID, ClientSecret = _googleApiSettings.ClientSecret },
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
            // Define a service to make requests to the google api
            var meService = new Oauth2Service(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _googleApiSettings.ClientID
            });

            // Make a request to the google api to get the user info
            Userinfo meObject = meService.Userinfo.V2.Me.Get().Execute();
            return meObject;
        }
        #endregion User API

    }
}
