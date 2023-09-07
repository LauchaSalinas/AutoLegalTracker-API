using Google.Apis.Drive.v3;

using AutoLegalTracker_API.Models;

namespace AutoLegalTracker_API.WebServices
{
    public class GoogleDriveService
    {
        #region Constructor
        private readonly IConfiguration _configuration;
        private DriveService? _driveService;
        private readonly GoogleOAuth2Service _googleOAuth2Service;

        public GoogleDriveService(IConfiguration configuration, GoogleOAuth2Service googleOAuth2Service)
        {
            _configuration = configuration;
            _googleOAuth2Service = googleOAuth2Service;
        }

        public GoogleDriveService Set(User user)
        {
            _driveService = _googleOAuth2Service.Set(user).GetDriveService();
            return this;
        }

        #endregion Constructor

        #region Folder Methods

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

        #endregion Folder Methods
    }
}
