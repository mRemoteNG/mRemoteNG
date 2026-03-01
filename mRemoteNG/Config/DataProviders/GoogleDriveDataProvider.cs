using System;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using mRemoteNG.App;
using mRemoteNG.Properties;

namespace mRemoteNG.Config.DataProviders
{
    [SupportedOSPlatform("windows")]
    public class GoogleDriveDataProvider : IDataProvider<string>
    {
        private static readonly string[] Scopes = { DriveService.Scope.DriveFile };
        private const string ApplicationName = "mRemoteNG";
        private const string TokenFolderName = "google_drive_token";

        private readonly string _driveFileName;
        private DriveService? _driveService;

        public GoogleDriveDataProvider(string driveFileName)
        {
            if (string.IsNullOrEmpty(driveFileName))
                throw new ArgumentException("Drive file name cannot be null or empty", nameof(driveFileName));
            _driveFileName = driveFileName;
        }

        public string Load()
        {
            try
            {
                DriveService service = GetDriveServiceSync();
                string? fileId = FindFileId(service);

                if (string.IsNullOrEmpty(fileId))
                {
                    Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg,
                        $"Google Drive file '{_driveFileName}' not found. A new file will be created on save.");
                    return "";
                }

                using MemoryStream stream = new();
                var request = service.Files.Get(fileId);
                request.Download(stream);
                stream.Position = 0;

                using StreamReader reader = new(stream);
                string content = reader.ReadToEnd();

                // Cache the file ID for faster access
                OptionsGoogleDrivePage.Default.GoogleDriveFileId = fileId;
                OptionsGoogleDrivePage.Default.Save();

                Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg,
                    $"Loaded connections from Google Drive: {_driveFileName}");
                return content;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(
                    $"Failed to load from Google Drive: {_driveFileName}", ex);
                return "";
            }
        }

        public void Save(string contents)
        {
            try
            {
                DriveService service = GetDriveServiceSync();
                string? fileId = FindFileId(service);

                using MemoryStream stream = new();
                using (StreamWriter writer = new(stream, leaveOpen: true))
                {
                    writer.Write(contents);
                    writer.Flush();
                }
                stream.Position = 0;

                if (string.IsNullOrEmpty(fileId))
                {
                    // Create new file
                    var fileMetadata = new Google.Apis.Drive.v3.Data.File
                    {
                        Name = _driveFileName,
                        MimeType = "application/xml"
                    };

                    var createRequest = service.Files.Create(fileMetadata, stream, "application/xml");
                    createRequest.Fields = "id";
                    var result = createRequest.Upload();

                    if (result.Status == Google.Apis.Upload.UploadStatus.Failed)
                        throw new IOException($"Failed to create Google Drive file: {result.Exception?.Message}");

                    fileId = createRequest.ResponseBody?.Id;
                    if (!string.IsNullOrEmpty(fileId))
                    {
                        OptionsGoogleDrivePage.Default.GoogleDriveFileId = fileId;
                        OptionsGoogleDrivePage.Default.Save();
                    }

                    Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg,
                        $"Created new connection file on Google Drive: {_driveFileName}");
                }
                else
                {
                    // Update existing file
                    var updateRequest = service.Files.Update(
                        new Google.Apis.Drive.v3.Data.File(),
                        fileId,
                        stream,
                        "application/xml");

                    var result = updateRequest.Upload();

                    if (result.Status == Google.Apis.Upload.UploadStatus.Failed)
                        throw new IOException($"Failed to update Google Drive file: {result.Exception?.Message}");

                    Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg,
                        $"Saved connections to Google Drive: {_driveFileName}");
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(
                    $"Failed to save to Google Drive: {_driveFileName}", ex);
            }
        }

        private string? FindFileId(DriveService service)
        {
            // First try the cached file ID
            string cachedId = OptionsGoogleDrivePage.Default.GoogleDriveFileId;
            if (!string.IsNullOrEmpty(cachedId))
            {
                try
                {
                    var getRequest = service.Files.Get(cachedId);
                    getRequest.Fields = "id, name, trashed";
                    var file = getRequest.Execute();
                    if (file != null && file.Trashed != true)
                        return cachedId;
                }
                catch
                {
                    // Cached ID is stale, search by name
                }
            }

            // Search by name
            var listRequest = service.Files.List();
            listRequest.Q = $"name = '{_driveFileName.Replace("'", "\\'", StringComparison.Ordinal)}' and trashed = false";
            listRequest.Fields = "files(id, name)";
            listRequest.PageSize = 1;

            var files = listRequest.Execute();
            return files.Files?.FirstOrDefault()?.Id;
        }

        private DriveService GetDriveServiceSync()
        {
            if (_driveService != null)
                return _driveService;

            _driveService = Task.Run(() => CreateDriveServiceAsync()).GetAwaiter().GetResult();
            return _driveService;
        }

        private static async Task<DriveService> CreateDriveServiceAsync()
        {
            string credentialsPath = OptionsGoogleDrivePage.Default.GoogleDriveCredentialsPath;
            if (string.IsNullOrEmpty(credentialsPath) || !File.Exists(credentialsPath))
                throw new FileNotFoundException(
                    "Google Drive credentials file not found. Please configure the path in Options > Google Drive.");

            string tokenPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                ApplicationName,
                TokenFolderName);

            UserCredential credential;
            using (FileStream stream = new(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(tokenPath, true));
            }

            return new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
        }

        public static async Task<bool> TestConnectionAsync()
        {
            try
            {
                DriveService service = await CreateDriveServiceAsync();
                var aboutRequest = service.About.Get();
                aboutRequest.Fields = "user";
                var about = aboutRequest.Execute();
                return about?.User != null;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<string?> GetAuthenticatedUserAsync()
        {
            try
            {
                DriveService service = await CreateDriveServiceAsync();
                var aboutRequest = service.About.Get();
                aboutRequest.Fields = "user";
                var about = aboutRequest.Execute();
                return about?.User?.EmailAddress ?? about?.User?.DisplayName;
            }
            catch
            {
                return null;
            }
        }

        public static void RevokeAuthorization()
        {
            string tokenPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                ApplicationName,
                TokenFolderName);

            if (Directory.Exists(tokenPath))
                Directory.Delete(tokenPath, true);
        }
    }
}
