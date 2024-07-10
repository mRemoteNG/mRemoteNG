using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using mRemoteNG.App.Info;
using mRemoteNG.Security.SymmetricEncryption;
using System.Security.Cryptography;
using System.Threading.Tasks;
using mRemoteNG.Properties;
using System.Runtime.Versioning;
#if !PORTABLE
using mRemoteNG.Tools;

#else
using System.Windows.Forms;

#endif
// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.App.Update
{
    [SupportedOSPlatform("windows")]
    public class AppUpdater
    {
        private const int _bufferLength = 8192;
        private WebProxy _webProxy;
        private HttpClient _httpClient;
        private CancellationTokenSource _changeLogCancelToken;
        private CancellationTokenSource _getUpdateInfoCancelToken;

        #region Public Properties

        public UpdateInfo CurrentUpdateInfo { get; private set; }

        public bool IsGetUpdateInfoRunning
        {
            get
            {
                return _getUpdateInfoCancelToken != null;
            }
        }

        private bool IsGetChangeLogRunning
        {
            get
            {
                return _changeLogCancelToken != null;
            }
        }

        #endregion

        #region Public Methods

        public AppUpdater()
        {
            SetDefaultProxySettings();
        }

        private void SetDefaultProxySettings()
        {
            bool shouldWeUseProxy = Properties.OptionsUpdatesPage.Default.UpdateUseProxy;
            string proxyAddress = Properties.OptionsUpdatesPage.Default.UpdateProxyAddress;
            int port = Properties.OptionsUpdatesPage.Default.UpdateProxyPort;
            bool useAuthentication = Properties.OptionsUpdatesPage.Default.UpdateProxyUseAuthentication;
            string username = Properties.OptionsUpdatesPage.Default.UpdateProxyAuthUser;
            LegacyRijndaelCryptographyProvider cryptographyProvider = new();
            string password = cryptographyProvider.Decrypt(Properties.OptionsUpdatesPage.Default.UpdateProxyAuthPass, Runtime.EncryptionKey);

            SetProxySettings(shouldWeUseProxy, proxyAddress, port, useAuthentication, username, password);
        }

        public void SetProxySettings(bool useProxy, string address, int port, bool useAuthentication, string username, string password)
        {
            if (useProxy && !string.IsNullOrEmpty(address))
            {
                _webProxy = port != 0 ? new WebProxy(address, port) : new WebProxy(address);
                _webProxy.Credentials = useAuthentication ? new NetworkCredential(username, password) : null;
            }
            else
            {
                _webProxy = null;
            }

            UpdateHttpClient();
        }

        public bool IsUpdateAvailable()
        {
            if (CurrentUpdateInfo == null || !CurrentUpdateInfo.IsValid)
            {
                return false;
            }

            return CurrentUpdateInfo.Version > GeneralAppInfo.GetApplicationVersion();
        }
        
        public async Task DownloadUpdateAsync(IProgress<int> progress)
        {
            if (IsGetUpdateInfoRunning)
            {
                _getUpdateInfoCancelToken.Cancel();
                _getUpdateInfoCancelToken.Dispose();
                _getUpdateInfoCancelToken = null;

                throw new InvalidOperationException("A previous call to DownloadUpdateAsync() is still in progress.");
            }

            if (CurrentUpdateInfo == null || !CurrentUpdateInfo.IsValid)
            {
                throw new InvalidOperationException("CurrentUpdateInfo is not valid. GetUpdateInfoAsync() must be called before calling DownloadUpdateAsync().");
            }
#if !PORTABLE
            CurrentUpdateInfo.UpdateFilePath = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetRandomFileName(), "msi"));
#else
            var sfd = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                FileName = CurrentUpdateInfo.FileName,
                RestoreDirectory = true
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                CurrentUpdateInfo.UpdateFilePath = sfd.FileName;
            }
            else
            {
                return;
            }
#endif
            try
            {
                _getUpdateInfoCancelToken = new CancellationTokenSource();
                using HttpResponseMessage response = await _httpClient.GetAsync(CurrentUpdateInfo.DownloadAddress, HttpCompletionOption.ResponseHeadersRead, _getUpdateInfoCancelToken.Token);
                byte[] buffer = new byte[_bufferLength];
                long totalBytes = response.Content.Headers.ContentLength ?? 0;
                long readBytes = 0L;

                await using (Stream httpStream = await response.Content.ReadAsStreamAsync(_getUpdateInfoCancelToken.Token))
                {
                    await using FileStream fileStream = new(CurrentUpdateInfo.UpdateFilePath, FileMode.Create,
                        FileAccess.Write, FileShare.None, _bufferLength, true);

                    while (readBytes <= totalBytes || !_getUpdateInfoCancelToken.IsCancellationRequested)
                    {
                        int bytesRead =
                            await httpStream.ReadAsync(buffer.AsMemory(0, _bufferLength), _getUpdateInfoCancelToken.Token);
                        if (bytesRead == 0)
                        {
                            progress.Report(100);
                            break;
                        }

                        await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead), _getUpdateInfoCancelToken.Token);

                        readBytes += bytesRead;

                        int percentComplete = (int)(readBytes * 100 / totalBytes);
                        progress.Report(percentComplete);
                    }
                }

#if !PORTABLE
                Authenticode updateAuthenticode = new(CurrentUpdateInfo.UpdateFilePath)
                    {
                        RequireThumbprintMatch = true,
                        ThumbprintToMatch = CurrentUpdateInfo.CertificateThumbprint
                    };

                    if (updateAuthenticode.Verify() != Authenticode.StatusValue.Verified)
                    {
                        if (updateAuthenticode.Status == Authenticode.StatusValue.UnhandledException)
                        {
                            throw updateAuthenticode.Exception;
                        }

                        throw new Exception(updateAuthenticode.GetStatusMessage());
                    }
#endif

                using SHA512 checksum = SHA512.Create();
                await using FileStream stream = File.OpenRead(CurrentUpdateInfo.UpdateFilePath);
                byte[] hash = await checksum.ComputeHashAsync(stream);
                string hashString = BitConverter.ToString(hash).Replace("-", "").ToUpperInvariant();
                if (!hashString.Equals(CurrentUpdateInfo.Checksum))
                    throw new Exception("SHA512 Hashes didn't match!");
            } finally{
                _getUpdateInfoCancelToken?.Dispose();
                _getUpdateInfoCancelToken = null;
            }
        }

        #endregion

        #region Private Methods

        private void UpdateHttpClient()
        {
            if (_httpClient != null)
            {
                _httpClient.Dispose();
            }

            HttpClientHandler httpClientHandler = new();
            if (_webProxy != null)
            {
                httpClientHandler.UseProxy = true;
                httpClientHandler.Proxy = _webProxy;
            }
            _httpClient = new HttpClient(httpClientHandler);
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(GeneralAppInfo.UserAgent);
        }

        public async Task GetUpdateInfoAsync()
        {
            if (IsGetUpdateInfoRunning)
            {
                _getUpdateInfoCancelToken.Cancel();
                _getUpdateInfoCancelToken.Dispose();
                _getUpdateInfoCancelToken = null;
            }

            try
            {
                _getUpdateInfoCancelToken = new CancellationTokenSource();
                string updateInfo = await _httpClient.GetStringAsync(UpdateChannelInfo.GetUpdateChannelInfo(), _getUpdateInfoCancelToken.Token);
                CurrentUpdateInfo = UpdateInfo.FromString(updateInfo);
                Properties.OptionsUpdatesPage.Default.CheckForUpdatesLastCheck = DateTime.UtcNow;

                if (!Properties.OptionsUpdatesPage.Default.UpdatePending)
                {
                    Properties.OptionsUpdatesPage.Default.UpdatePending = IsUpdateAvailable();
                }
            }
            finally
            {
                _getUpdateInfoCancelToken?.Dispose();
                _getUpdateInfoCancelToken = null;
            }
        }

        public async Task<string> GetChangeLogAsync()
        {
            if (IsGetChangeLogRunning)
            {
                _changeLogCancelToken.Cancel();
                _changeLogCancelToken.Dispose();
                _changeLogCancelToken = null;
            }

            try
            {
                _changeLogCancelToken = new CancellationTokenSource();
                return await _httpClient.GetStringAsync(CurrentUpdateInfo.ChangeLogAddress, _changeLogCancelToken.Token);
            }
            finally
            {
                _changeLogCancelToken?.Dispose();
                _changeLogCancelToken = null;
            }
        }

        #endregion
    }
}