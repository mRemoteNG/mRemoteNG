using System;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Net.Http;
using System.Threading;
using System.Reflection;
using mRemoteNG.App.Info;
using mRemoteNG.Security.SymmetricEncryption;
using System.Security.Cryptography;
using System.Threading.Tasks;
using mRemoteNG.Properties;
#if !PORTABLE
using mRemoteNG.Tools;

#else
using System.Windows.Forms;

#endif
// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.App.Update
{
    public class AppUpdater
    {
        private WebProxy _webProxy;
        private HttpClient _httpClient;
        // private Thread _getUpdateInfoThread;
        // private Thread _getChangeLogThread;
        private CancellationTokenSource changeLogCancelToken;
        private CancellationTokenSource getUpdateInfoCancelToken;

        #region Public Properties

        public UpdateInfo CurrentUpdateInfo { get; private set; }

        /*
        public string ChangeLog { get; private set; }
        */

        public bool IsGetUpdateInfoRunning
        {
            get
            {
                return getUpdateInfoCancelToken != null;

            }
        }

        private bool IsGetChangeLogRunning
        {
            get
            {
                return changeLogCancelToken != null;
            }
        }

        /* TODO: Review later
        public bool IsDownloadUpdateRunning
        {
            get { return _downloadUpdateWebClient != null; }
        }
        */

        #endregion

        #region Public Methods

        public AppUpdater()
        {
            SetDefaultProxySettings();
        }

        private void SetDefaultProxySettings()
        {
            var shouldWeUseProxy = Settings.Default.UpdateUseProxy;
            var proxyAddress = Settings.Default.UpdateProxyAddress;
            var port = Settings.Default.UpdateProxyPort;
            var useAuthentication = Settings.Default.UpdateProxyUseAuthentication;
            var username = Settings.Default.UpdateProxyAuthUser;
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            var password = cryptographyProvider.Decrypt(Settings.Default.UpdateProxyAuthPass, Runtime.EncryptionKey);

            SetProxySettings(shouldWeUseProxy, proxyAddress, port, useAuthentication, username, password);
        }

        public void SetProxySettings(bool useProxy,
                                     string address,
                                     int port,
                                     bool useAuthentication,
                                     string username,
                                     string password)
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

        /*
        public void GetUpdateInfoAsync()
        {
            if (IsGetUpdateInfoRunning)
            {
                _getUpdateInfoThread.Abort();
            }

            _getUpdateInfoThread = new Thread(GetUpdateInfo);
            _getUpdateInfoThread.SetApartmentState(ApartmentState.STA);
            _getUpdateInfoThread.IsBackground = true;
            _getUpdateInfoThread.Start();
        }
        
        public void GetChangeLogAsync()
        {
            if (CurrentUpdateInfo == null || !CurrentUpdateInfo.IsValid)
            {
                throw new InvalidOperationException(
                                                    "CurrentUpdateInfo is not valid. GetUpdateInfoAsync() must be called before calling GetChangeLogAsync().");
            }

            if (IsGetChangeLogRunning)
            {
                _getChangeLogThread.Abort();
            }

            _getChangeLogThread = new Thread(GetChangeLog);
            _getChangeLogThread.SetApartmentState(ApartmentState.STA);
            _getChangeLogThread.IsBackground = true;
            _getChangeLogThread.Start();
        }
        */

        public async Task DownloadUpdateAsync(IProgress<int> progress)
        {
            if (IsGetUpdateInfoRunning)
            {
                getUpdateInfoCancelToken.Cancel();
                getUpdateInfoCancelToken.Dispose();
                getUpdateInfoCancelToken = null;

                throw new InvalidOperationException("A previous call to DownloadUpdateAsync() is still in progress.");
            }

            if (CurrentUpdateInfo == null || !CurrentUpdateInfo.IsValid)
            {
                throw new InvalidOperationException(
                                                    "CurrentUpdateInfo is not valid. GetUpdateInfoAsync() must be called before calling DownloadUpdateAsync().");
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
                getUpdateInfoCancelToken = new CancellationTokenSource();
                using (var response = await _httpClient.GetAsync(CurrentUpdateInfo.DownloadAddress,
                           HttpCompletionOption.ResponseHeadersRead, getUpdateInfoCancelToken.Token))
                {
                    var buffer = new byte[8192];
                    var totalBytes = response.Content.Headers.ContentLength ?? 0;
                    var readBytes = 0L;

                    await using var httpStream =
                        await response.Content.ReadAsStreamAsync(getUpdateInfoCancelToken.Token);
                    await using var fileStream = new FileStream(CurrentUpdateInfo.UpdateFilePath, FileMode.Create,
                        FileAccess.Write, FileShare.None, buffer.Length, true);

                    while (readBytes <= totalBytes || !getUpdateInfoCancelToken.IsCancellationRequested)
                    {
                        var bytesRead = await httpStream.ReadAsync(buffer, 0, buffer.Length, getUpdateInfoCancelToken.Token);
                        if (bytesRead == 0)
                        {
                            progress.Report(100);
                            break;
                        }

                        await fileStream.WriteAsync(buffer, 0, bytesRead, getUpdateInfoCancelToken.Token);

                        readBytes += bytesRead;

                        var percentComplete = (int)(readBytes * 100 / totalBytes);
                        progress.Report(percentComplete);
                    }
                }
            } finally{
                getUpdateInfoCancelToken.Dispose();
                getUpdateInfoCancelToken = null;
            }
        }

        #endregion

        #region Private Properties

        /*
         TODO: Review this part
        private HttpClient DownloadUpdateWebClient
        {
            get
            {
                if (_downloadUpdateWebClient != null)
                {
                    return _downloadUpdateWebClient;
                }

                _downloadUpdateWebClient = CreateHttpClient();

                // TODO: _downloadUpdateWebClient.DownloadProgressChanged += DownloadUpdateProgressChanged;
                // TODO: _downloadUpdateWebClient.DownloadFileCompleted += DownloadUpdateCompleted;

                return _downloadUpdateWebClient;
            }
        }
        */

        #endregion

        #region Private Methods

        private void UpdateHttpClient()
        {
            if (_httpClient != null)
            {
                _httpClient.Dispose();
            }

            var httpClientHandler = new HttpClientHandler();
            if (_webProxy != null)
            {
                httpClientHandler.UseProxy = true;
                httpClientHandler.Proxy = _webProxy;
            }
            _httpClient = new HttpClient(httpClientHandler);
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(GeneralAppInfo.UserAgent);
        }

        /* TODO: Review this part
        private static DownloadStringCompletedEventArgs NewDownloadStringCompletedEventArgs(string result,
                                                                                            Exception exception,
                                                                                            bool cancelled,
                                                                                            object userToken)
        {
            var type = typeof(DownloadStringCompletedEventArgs);
            const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            Type[] argumentTypes = {typeof(string), typeof(Exception), typeof(bool), typeof(object)};
            var constructor = type.GetConstructor(bindingFlags, null, argumentTypes, null);
            object[] arguments = {result, exception, cancelled, userToken};

            if (constructor == null)
                return null;
            return (DownloadStringCompletedEventArgs)constructor.Invoke(arguments);
        }

 
        private DownloadStringCompletedEventArgs DownloadString(Uri address)
        {
            var result = string.Empty;
            Exception exception = null;
            var cancelled = false;

            try
            {
                result = httpClient.GetStringAsync(address).Result;
            }
            catch (ThreadAbortException)
            {
                cancelled = true;
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            return NewDownloadStringCompletedEventArgs(result, exception, cancelled, null);
        }
        */

        public async Task GetUpdateInfoAsync()
        {
            if (IsGetUpdateInfoRunning)
            {
                getUpdateInfoCancelToken.Cancel();
                getUpdateInfoCancelToken.Dispose();
                getUpdateInfoCancelToken = null;
            }

            try
            {
                getUpdateInfoCancelToken = new CancellationTokenSource();
                var updateInfo = await _httpClient.GetStringAsync(UpdateChannelInfo.GetUpdateChannelInfo(), getUpdateInfoCancelToken.Token);
                CurrentUpdateInfo = UpdateInfo.FromString(updateInfo);
                Settings.Default.CheckForUpdatesLastCheck = DateTime.UtcNow;

                if (!Settings.Default.UpdatePending)
                {
                    Settings.Default.UpdatePending = IsUpdateAvailable();
                }
            }
            finally
            {
                getUpdateInfoCancelToken.Dispose();
                getUpdateInfoCancelToken = null;
            }
        }

        public async Task<string> GetChangeLogAsync()
        {
            if (IsGetChangeLogRunning)
            {
                changeLogCancelToken.Cancel();
                changeLogCancelToken.Dispose();
                changeLogCancelToken = null;
            }

            try
            {
                changeLogCancelToken = new CancellationTokenSource();
                return await _httpClient.GetStringAsync(CurrentUpdateInfo.ChangeLogAddress, changeLogCancelToken.Token);
            }
            finally
            {
                changeLogCancelToken.Dispose();
                changeLogCancelToken = null;
            }
        }

        /* TODO
        private void DownloadUpdateProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadUpdateProgressChangedEventEvent?.Invoke(sender, e);
        }
        */

        /* TODO
        private void DownloadUpdateCompleted(object sender, AsyncCompletedEventArgs e)
        {
            var raiseEventArgs = e;

            if (!e.Cancelled && e.Error == null)
            {
                try
                {
#if !PORTABLE
                    var updateAuthenticode = new Authenticode(CurrentUpdateInfo.UpdateFilePath)
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

                    using (var cksum = SHA512.Create())
                    {
                        using (var stream = File.OpenRead(CurrentUpdateInfo.UpdateFilePath))
                        {
                            var hash = cksum.ComputeHash(stream);
                            var hashString = BitConverter.ToString(hash).Replace("-", "").ToUpperInvariant();
                            if (!hashString.Equals(CurrentUpdateInfo.Checksum))
                                throw new Exception("SHA512 Hashes didn't match!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    raiseEventArgs = new AsyncCompletedEventArgs(ex, false, null);
                }
            }

            if (raiseEventArgs.Cancelled || raiseEventArgs.Error != null)
            {
                File.Delete(CurrentUpdateInfo.UpdateFilePath);
            }

            DownloadUpdateCompletedEventEvent?.Invoke(this, raiseEventArgs);

            _downloadUpdateWebClient.Dispose();
            _downloadUpdateWebClient = null;
        }*/

        #endregion

        #region Events
        /* TODO:
        private AsyncCompletedEventHandler GetUpdateInfoCompletedEventEvent;

        public event AsyncCompletedEventHandler GetUpdateInfoCompletedEvent
        {
            add
            {
                GetUpdateInfoCompletedEventEvent =
                    (AsyncCompletedEventHandler)Delegate.Combine(GetUpdateInfoCompletedEventEvent, value);
            }
            remove
            {
                GetUpdateInfoCompletedEventEvent =
                    (AsyncCompletedEventHandler)Delegate.Remove(GetUpdateInfoCompletedEventEvent, value);
            }
        }

        private AsyncCompletedEventHandler GetChangeLogCompletedEventEvent;

        public event AsyncCompletedEventHandler GetChangeLogCompletedEvent
        {
            add
            {
                GetChangeLogCompletedEventEvent =
                    (AsyncCompletedEventHandler)Delegate.Combine(GetChangeLogCompletedEventEvent, value);
            }
            remove
            {
                GetChangeLogCompletedEventEvent =
                    (AsyncCompletedEventHandler)Delegate.Remove(GetChangeLogCompletedEventEvent, value);
            }
        }

        private DownloadProgressChangedEventHandler DownloadUpdateProgressChangedEventEvent;

        public event DownloadProgressChangedEventHandler DownloadUpdateProgressChangedEvent
        {
            add
            {
                DownloadUpdateProgressChangedEventEvent =
                    (DownloadProgressChangedEventHandler)Delegate.Combine(DownloadUpdateProgressChangedEventEvent,
                                                                          value);
            }
            remove
            {
                DownloadUpdateProgressChangedEventEvent =
                    (DownloadProgressChangedEventHandler)Delegate.Remove(DownloadUpdateProgressChangedEventEvent,
                                                                         value);
            }
        }

        private AsyncCompletedEventHandler DownloadUpdateCompletedEventEvent;

        public event AsyncCompletedEventHandler DownloadUpdateCompletedEvent
        {
            add
            {
                DownloadUpdateCompletedEventEvent =
                    (AsyncCompletedEventHandler)Delegate.Combine(DownloadUpdateCompletedEventEvent, value);
            }
            remove
            {
                DownloadUpdateCompletedEventEvent =
                    (AsyncCompletedEventHandler)Delegate.Remove(DownloadUpdateCompletedEventEvent, value);
            }
        }
        */
        #endregion
    }
}