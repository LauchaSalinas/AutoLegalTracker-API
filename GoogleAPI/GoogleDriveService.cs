using Google.Apis.Drive.v3;

namespace GoogleAPI
{
    public class GoogleDriveService
    {
        #region Constructor
        private DriveService? _driveService;
        private readonly GoogleOAuth2Service _googleOAuth2Service;

        public GoogleDriveService(GoogleOAuth2Service googleOAuth2Service)
        {
            _googleOAuth2Service = googleOAuth2Service;
        }

        public GoogleDriveService Set(GoogleAPIUserDTO user)
        {
            _driveService = _googleOAuth2Service.Set(user).GetDriveService();
            return this;
        }

        #endregion Constructor

        #region Folder Methods

        private async Task<string> CreateFolder(string folderName)
        {
            // Create a folder in the root of the drive
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = folderName,
                MimeType = "application/vnd.google-apps.folder"
            };
            var request = _driveService.Files.Create(fileMetadata);
            request.Fields = "id";
            var file = await request.ExecuteAsync();
            return file.Id;
        }

        #endregion Folder Methods
    }
}
