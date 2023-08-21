using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2.Data;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using Google.Apis.Util;
using Google.Apis.Drive.v3;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;

namespace AutoLegalTracker_API._5_WebServices
{
    public class GoogleOAuth2Service
    {
        #region Constructor
        private readonly IConfiguration _configuration;
        private readonly GoogleAuthorizationCodeFlow _flow; // Flow: https://frontegg.com/blog/oauth-flows

        public GoogleOAuth2Service(IConfiguration configuration)
        {
            _configuration = configuration;
            _flow = GetGoogleAuthorizationCodeFlow();
        }

        private GoogleAuthorizationCodeFlow GetGoogleAuthorizationCodeFlow()
        {
            string clientID = _configuration["OAUTH2_CLIENT_ID"] ?? string.Empty;
            string clientSecret = _configuration["OAUTH2_CLIENT_SECRET"] ?? string.Empty;

            // Define the google authentification flow and initialize it with the clientID and clientSecret
            GoogleAuthorizationCodeFlow.Initializer initializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets() { ClientId = clientID, ClientSecret = clientSecret },
            };
            GoogleAuthorizationCodeFlow flow = new GoogleAuthorizationCodeFlow(initializer);

            return flow;
        }
        #endregion Constructor

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
                RedirectUri = redirectUri,
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
            return token;
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

            //get the audience from the google api
            //var tokenInfo = meService.Tokeninfo().Execute();
            //tokenInfo.ExpiresIn

            // Make a request to the google api to get the user info
            Userinfo meObject = meService.Userinfo.V2.Me.Get().Execute();
            return meObject;
        }
        #endregion User API

        #region Drive API
        private DriveService GetDriveService(UserCredential credential)
        {
            string clientID = _configuration["OAUTH2_CLIENT_ID"] ?? string.Empty;
            // Define a service to make requests to the google api
            var driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = clientID
            });
            return driveService;
        }

        private async Task<string> CreateFolder(DriveService driveService, string folderName)
        {
            // Create a folder in the root of the drive
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = folderName,
                MimeType = "application/vnd.google-apps.folder"
            };
            var request = driveService.Files.Create(fileMetadata);
            request.Fields = "id";
            var file = await request.ExecuteAsync();
            return file.Id;
        }

        #endregion Drive API

        #region Sheets API
        private SheetsService GetSheetsService(UserCredential credential)
        {
            string clientID = _configuration["OAUTH2_CLIENT_ID"] ?? string.Empty;
            // Define a service to make requests to the google api
            var sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = clientID
            });
            return sheetsService;
        }

        private async Task<string> CreateSpreadsheet(SheetsService sheetsService, string spreadsheetName)
        {
            // Create a spreadsheet
            var spreadsheet = new Spreadsheet()
            {
                Properties = new SpreadsheetProperties()
                {
                    Title = spreadsheetName
                }
            };
            var request = sheetsService.Spreadsheets.Create(spreadsheet);
            var response = await request.ExecuteAsync();
            return response.SpreadsheetId;
        }
        #endregion Sheets API

        #region Calendar API
        private CalendarService GetCalendarService(UserCredential credential)
        {
            string clientID = _configuration["OAUTH2_CLIENT_ID"] ?? string.Empty;
            // Define a service to make requests to the google api
            var calendarService = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = clientID
            });
            return calendarService;
        }

        private async Task<string> CreateCalendar(CalendarService calendarService, string calendarName)
        {
            // Create a calendar
            var calendar = new Google.Apis.Calendar.v3.Data.Calendar()
            {
                Summary = calendarName
            };
            var request = calendarService.Calendars.Insert(calendar);
            var response = await request.ExecuteAsync();
            return response.Id;
        }

        private async Task<string> CreateEvent(CalendarService calendarService, string calendarId, string eventName, DateTime eventStart, DateTime eventEnd)
        {
            // Create an event
            var newEvent = new Google.Apis.Calendar.v3.Data.Event()
            {
                Summary = eventName,
                Start = new EventDateTime()
                {
                    DateTime = eventStart,
                    TimeZone = "Europe/Paris"
                },
                End = new EventDateTime()
                {
                    DateTime = eventEnd,
                    TimeZone = "Europe/Paris"
                }
            };
            var request = calendarService.Events.Insert(newEvent, calendarId);
            var response = await request.ExecuteAsync();
            return response.Id;
        }

        // get calendar events
        private async Task<List<Google.Apis.Calendar.v3.Data.Event>> GetEvents(CalendarService calendarService, string calendarId)
        {
            // Create an event
            var request = calendarService.Events.List(calendarId);
            var response = await request.ExecuteAsync();
            return response.Items.ToList();
        }
        #endregion Calendar API
    }
}
