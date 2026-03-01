using System;
using System.IO;
using System.Net;
using System.Runtime.Versioning;
using System.Text;
using mRemoteNG.App;
using mRemoteNG.Messages;

namespace mRemoteNG.Config.DataProviders
{
    /// <summary>
    /// Data provider that reads/writes connection files via FTP.
    /// Implements feature request #1871: store confCons.xml on an FTP share.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class FtpDataProvider : IDataProvider<string>
    {
        private readonly string _ftpHost;
        private readonly int _ftpPort;
        private readonly string _ftpPath;
        private readonly string _username;
        private readonly string _password;
        private readonly bool _useSsl;

        public FtpDataProvider(string ftpHost, int ftpPort, string ftpPath, string username, string password, bool useSsl = false)
        {
            if (string.IsNullOrWhiteSpace(ftpHost))
                throw new ArgumentException("FTP host cannot be null or empty", nameof(ftpHost));
            if (string.IsNullOrWhiteSpace(ftpPath))
                throw new ArgumentException("FTP remote path cannot be null or empty", nameof(ftpPath));

            _ftpHost = ftpHost.Trim();
            _ftpPort = ftpPort > 0 ? ftpPort : 21;
            _ftpPath = ftpPath.StartsWith('/') ? ftpPath : "/" + ftpPath;
            _username = username ?? "anonymous";
            _password = password ?? "";
            _useSsl = useSsl;
        }

        public string Load()
        {
            string uri = BuildFtpUri();
            try
            {
#pragma warning disable SYSLIB0014
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
#pragma warning restore SYSLIB0014
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(_username, _password);
                request.EnableSsl = _useSsl;
                request.KeepAlive = false;
                request.Timeout = 30000;

                using FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                using Stream stream = response.GetResponseStream();
                using StreamReader reader = new(stream, Encoding.UTF8);
                string content = reader.ReadToEnd();

                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                    $"Loaded connections from FTP: {uri}");
                return content;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(
                    $"Failed to load connections from FTP '{uri}'", ex);
                return "";
            }
        }

        public void Save(string contents)
        {
            string uri = BuildFtpUri();
            try
            {
                byte[] fileContents = Encoding.UTF8.GetBytes(contents);

#pragma warning disable SYSLIB0014
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
#pragma warning restore SYSLIB0014
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(_username, _password);
                request.EnableSsl = _useSsl;
                request.KeepAlive = false;
                request.ContentLength = fileContents.Length;
                request.Timeout = 30000;

                using Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);

                using FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                    $"Synced connections to FTP: {uri}");
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(
                    $"Failed to save connections to FTP '{uri}'", ex);
            }
        }

        private string BuildFtpUri()
        {
            return $"ftp://{_ftpHost.TrimEnd('/')}:{_ftpPort}{_ftpPath}";
        }

        /// <summary>
        /// Tests FTP connectivity by listing the root directory.
        /// </summary>
        public static bool TestConnection(string ftpHost, int ftpPort, string username, string password, bool useSsl)
        {
            try
            {
                string uri = $"ftp://{ftpHost.Trim().TrimEnd('/')}:{ftpPort}/";
#pragma warning disable SYSLIB0014
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
#pragma warning restore SYSLIB0014
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(username ?? "anonymous", password ?? "");
                request.EnableSsl = useSsl;
                request.KeepAlive = false;
                request.Timeout = 10000;

                using FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                return (int)response.StatusCode < 400;
            }
            catch
            {
                return false;
            }
        }
    }
}
