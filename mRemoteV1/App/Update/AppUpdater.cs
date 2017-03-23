using System;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Threading;
using System.Reflection;
using mRemoteNG.App.Info;
using mRemoteNG.Security.SymmetricEncryption;
using System.Security.Cryptography;
#if !PORTABLE
using mRemoteNG.Tools;

#else 
using System.Windows.Forms;
#endif

namespace mRemoteNG.App.Update
{
    public class AppUpdater
    {
        private WebProxy _webProxy;
        private Thread _getUpdateInfoThread;
        private Thread _getChangeLogThread;

        #region Public Properties

        public UpdateInfo CurrentUpdateInfo { get; private set; }

        public string ChangeLog { get; private set; }

        public bool IsGetUpdateInfoRunning => _getUpdateInfoThread != null && _getUpdateInfoThread.IsAlive;

        private bool IsGetChangeLogRunning => _getChangeLogThread != null && _getChangeLogThread.IsAlive;

        public bool IsDownloadUpdateRunning => _downloadUpdateWebClient != null;

        #endregion

        #region Public Methods

        public AppUpdater()
        {
            SetProxySettings();
        }

        private void SetProxySettings()
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
        }

        public bool IsUpdateAvailable()
        {
            if (CurrentUpdateInfo == null || !CurrentUpdateInfo.IsValid)
            {
                return false;
            }

            return CurrentUpdateInfo.Version > GeneralAppInfo.GetApplicationVersion();
        }

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
                throw new InvalidOperationException("CurrentUpdateInfo is not valid. GetUpdateInfoAsync() must be called before calling GetChangeLogAsync().");
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

        public void DownloadUpdateAsync()
        {
            if (_downloadUpdateWebClient != null)
            {
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
            DownloadUpdateWebClient.DownloadFileAsync(CurrentUpdateInfo.DownloadAddress, CurrentUpdateInfo.UpdateFilePath);
        }

        #endregion

        #region Private Properties

        private WebClient _downloadUpdateWebClient;

        private WebClient DownloadUpdateWebClient
        {
            get
            {
                if (_downloadUpdateWebClient != null)
                {
                    return _downloadUpdateWebClient;
                }

                _downloadUpdateWebClient = CreateWebClient();

                _downloadUpdateWebClient.DownloadProgressChanged += DownloadUpdateProgressChanged;
                _downloadUpdateWebClient.DownloadFileCompleted += DownloadUpdateCompleted;

                return _downloadUpdateWebClient;
            }
        }

        #endregion

        #region Private Methods

        private WebClient CreateWebClient()
        {
            var webClient = new WebClient();
            webClient.Headers.Add("user-agent", GeneralAppInfo.UserAgent);
            webClient.Proxy = _webProxy;
            return webClient;
        }

        private static DownloadStringCompletedEventArgs NewDownloadStringCompletedEventArgs(string result,
            Exception exception, bool cancelled, object userToken)
        {
            var type = typeof(DownloadStringCompletedEventArgs);
            const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            Type[] argumentTypes = {typeof(string), typeof(Exception), typeof(bool), typeof(object)};
            var constructor = type.GetConstructor(bindingFlags, null, argumentTypes, null);
            object[] arguments = {result, exception, cancelled, userToken};

            return (DownloadStringCompletedEventArgs) constructor.Invoke(arguments);
        }

        public DownloadStringCompletedEventArgs DownloadString(Uri address)
        {
            var webClient = CreateWebClient();
            var result = string.Empty;
            Exception exception = null;
            var cancelled = false;

            try
            {
                result = webClient.DownloadString(address);
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

        private void GetUpdateInfo()
        {
            var e = DownloadString(UpdateChannelInfo.GetUpdateChannelInfo());

            if (!e.Cancelled && e.Error == null)
            {
                CurrentUpdateInfo = UpdateInfo.FromString(e.Result);

                Settings.Default.CheckForUpdatesLastCheck = DateTime.UtcNow;
                if (!Settings.Default.UpdatePending)
                {
                    Settings.Default.UpdatePending = IsUpdateAvailable();
                }
            }

            GetUpdateInfoCompletedEventEvent?.Invoke(this, e);
        }

        private void GetChangeLog()
        {
            var e = DownloadString(CurrentUpdateInfo.ChangeLogAddress);

            if (!e.Cancelled && e.Error == null)
            {
                ChangeLog = e.Result;
            }

            GetChangeLogCompletedEventEvent?.Invoke(this, e);
        }

        private void DownloadUpdateProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadUpdateProgressChangedEventEvent?.Invoke(sender, e);
        }

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
        }

        #endregion

        #region Events

        private AsyncCompletedEventHandler GetUpdateInfoCompletedEventEvent;

        public event AsyncCompletedEventHandler GetUpdateInfoCompletedEvent
        {
            add
            {
                GetUpdateInfoCompletedEventEvent = (AsyncCompletedEventHandler)Delegate.Combine(GetUpdateInfoCompletedEventEvent, value);
            }
            remove
            {
                GetUpdateInfoCompletedEventEvent = (AsyncCompletedEventHandler)Delegate.Remove(GetUpdateInfoCompletedEventEvent, value);
            }
        }

        private AsyncCompletedEventHandler GetChangeLogCompletedEventEvent;

        public event AsyncCompletedEventHandler GetChangeLogCompletedEvent
        {
            add
            {
                GetChangeLogCompletedEventEvent = (AsyncCompletedEventHandler)Delegate.Combine(GetChangeLogCompletedEventEvent, value);
            }
            remove
            {
                GetChangeLogCompletedEventEvent = (AsyncCompletedEventHandler)Delegate.Remove(GetChangeLogCompletedEventEvent, value);
            }
        }

        private DownloadProgressChangedEventHandler DownloadUpdateProgressChangedEventEvent;

        public event DownloadProgressChangedEventHandler DownloadUpdateProgressChangedEvent
        {
            add
            {
                DownloadUpdateProgressChangedEventEvent = (DownloadProgressChangedEventHandler)Delegate.Combine(DownloadUpdateProgressChangedEventEvent, value);
            }
            remove
            {
                DownloadUpdateProgressChangedEventEvent = (DownloadProgressChangedEventHandler)Delegate.Remove(DownloadUpdateProgressChangedEventEvent, value);
            }
        }

        private AsyncCompletedEventHandler DownloadUpdateCompletedEventEvent;

        public event AsyncCompletedEventHandler DownloadUpdateCompletedEvent
        {
            add
            {
                DownloadUpdateCompletedEventEvent = (AsyncCompletedEventHandler)Delegate.Combine(DownloadUpdateCompletedEventEvent, value);
            }
            remove
            {
                DownloadUpdateCompletedEventEvent = (AsyncCompletedEventHandler)Delegate.Remove(DownloadUpdateCompletedEventEvent, value);
            }
        }

        #endregion
    }
}