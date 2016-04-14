using System.Collections.Generic;
using System;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Threading;
using mRemoteNG.Tools;
using System.Reflection;


namespace mRemoteNG.App
{
	public class Update
	{
        #region Events
		private AsyncCompletedEventHandler GetUpdateInfoCompletedEventEvent;
		public event AsyncCompletedEventHandler GetUpdateInfoCompletedEvent
		{
			add
			{
				GetUpdateInfoCompletedEventEvent = (AsyncCompletedEventHandler) System.Delegate.Combine(GetUpdateInfoCompletedEventEvent, value);
			}
			remove
			{
				GetUpdateInfoCompletedEventEvent = (AsyncCompletedEventHandler) System.Delegate.Remove(GetUpdateInfoCompletedEventEvent, value);
			}
		}
			
		private AsyncCompletedEventHandler GetChangeLogCompletedEventEvent;
		public event AsyncCompletedEventHandler GetChangeLogCompletedEvent
		{
			add
			{
				GetChangeLogCompletedEventEvent = (AsyncCompletedEventHandler) System.Delegate.Combine(GetChangeLogCompletedEventEvent, value);
			}
			remove
			{
				GetChangeLogCompletedEventEvent = (AsyncCompletedEventHandler) System.Delegate.Remove(GetChangeLogCompletedEventEvent, value);
			}
		}
			
		private AsyncCompletedEventHandler GetAnnouncementInfoCompletedEventEvent;
		public event AsyncCompletedEventHandler GetAnnouncementInfoCompletedEvent
		{
			add
			{
				GetAnnouncementInfoCompletedEventEvent = (AsyncCompletedEventHandler) System.Delegate.Combine(GetAnnouncementInfoCompletedEventEvent, value);
			}
			remove
			{
				GetAnnouncementInfoCompletedEventEvent = (AsyncCompletedEventHandler) System.Delegate.Remove(GetAnnouncementInfoCompletedEventEvent, value);
			}
		}
			
		private DownloadProgressChangedEventHandler DownloadUpdateProgressChangedEventEvent;
		public event DownloadProgressChangedEventHandler DownloadUpdateProgressChangedEvent
		{
			add
			{
				DownloadUpdateProgressChangedEventEvent = (DownloadProgressChangedEventHandler) System.Delegate.Combine(DownloadUpdateProgressChangedEventEvent, value);
			}
			remove
			{
				DownloadUpdateProgressChangedEventEvent = (DownloadProgressChangedEventHandler) System.Delegate.Remove(DownloadUpdateProgressChangedEventEvent, value);
			}
		}
			
		private AsyncCompletedEventHandler DownloadUpdateCompletedEventEvent;
		public event AsyncCompletedEventHandler DownloadUpdateCompletedEvent
		{
			add
			{
				DownloadUpdateCompletedEventEvent = (AsyncCompletedEventHandler) System.Delegate.Combine(DownloadUpdateCompletedEventEvent, value);
			}
			remove
			{
				DownloadUpdateCompletedEventEvent = (AsyncCompletedEventHandler) System.Delegate.Remove(DownloadUpdateCompletedEventEvent, value);
			}
		}
        #endregion
			
        #region Public Properties
		private UpdateInfo _currentUpdateInfo;
        public UpdateInfo CurrentUpdateInfo
		{
			get
			{
				return _currentUpdateInfo;
			}
		}
			
		private string _changeLog;
        public string ChangeLog
		{
			get
			{
				return _changeLog;
			}
		}
			
		private AnnouncementInfo _currentAnnouncementInfo;
        public AnnouncementInfo CurrentAnnouncementInfo
		{
			get
			{
				return _currentAnnouncementInfo;
			}
		}
			
        public bool IsGetUpdateInfoRunning
		{
			get
			{
				if (_getUpdateInfoThread != null)
				{
					if (_getUpdateInfoThread.IsAlive)
					{
						return true;
					}
				}
				return false;
			}
		}
			
        public bool IsGetChangeLogRunning
		{
			get
			{
				if (_getChangeLogThread != null)
				{
					if (_getChangeLogThread.IsAlive)
					{
						return true;
					}
				}
				return false;
			}
		}
			
        public bool IsGetAnnouncementInfoRunning
		{
			get
			{
				if (_getAnnouncementInfoThread != null)
				{
					if (_getAnnouncementInfoThread.IsAlive)
					{
						return true;
					}
				}
				return false;
			}
		}
			
        public bool IsDownloadUpdateRunning
		{
			get
			{
				return (_downloadUpdateWebClient != null);
			}
		}
        #endregion
			
        #region Public Methods
		public Update()
		{
			SetProxySettings();
		}
			
		public void SetProxySettings()
		{
			SetProxySettings(System.Convert.ToBoolean(My.Settings.Default.UpdateUseProxy), System.Convert.ToString(My.Settings.Default.UpdateProxyAddress), System.Convert.ToInt32(My.Settings.Default.UpdateProxyPort), System.Convert.ToBoolean(My.Settings.Default.UpdateProxyUseAuthentication), System.Convert.ToString(My.Settings.Default.UpdateProxyAuthUser), Security.Crypt.Decrypt(System.Convert.ToString(My.Settings.Default.UpdateProxyAuthPass), Info.General.EncryptionKey));
		}
			
		public void SetProxySettings(bool useProxy, string address, int port, bool useAuthentication, string username, string password)
		{
			if (useProxy && !string.IsNullOrEmpty(address))
			{
				if (!(port == 0))
				{
					_webProxy = new WebProxy(address, port);
				}
				else
				{
					_webProxy = new WebProxy(address);
				}
					
				if (useAuthentication)
				{
					_webProxy.Credentials = new NetworkCredential(username, password);
				}
				else
				{
					_webProxy.Credentials = null;
				}
			}
			else
			{
				_webProxy = null;
			}
		}
			
		public bool IsUpdateAvailable()
		{
			if (_currentUpdateInfo == null || !_currentUpdateInfo.IsValid)
			{
				return false;
			}
				
			return _currentUpdateInfo.Version > (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.Version;
		}
			
		public bool IsAnnouncementAvailable()
		{
			if (_currentAnnouncementInfo == null || (!_currentAnnouncementInfo.IsValid || string.IsNullOrEmpty(_currentAnnouncementInfo.Name)))
			{
				return false;
			}
				
			return !(_currentAnnouncementInfo.Name == My.Settings.Default.LastAnnouncement);
		}
			
		public void GetUpdateInfoAsync()
		{
			if (IsGetUpdateInfoRunning)
			{
				_getUpdateInfoThread.Abort();
			}
				
			_getUpdateInfoThread = new Thread(new System.Threading.ThreadStart(GetUpdateInfo));
			_getUpdateInfoThread.SetApartmentState(ApartmentState.STA);
			_getUpdateInfoThread.IsBackground = true;
			_getUpdateInfoThread.Start();
		}
			
		public void GetChangeLogAsync()
		{
			if (_currentUpdateInfo == null || !_currentUpdateInfo.IsValid)
			{
				throw (new InvalidOperationException("CurrentUpdateInfo is not valid. GetUpdateInfoAsync() must be called before calling GetChangeLogAsync()."));
			}
				
			if (IsGetChangeLogRunning)
			{
				_getChangeLogThread.Abort();
			}
				
			_getChangeLogThread = new Thread(new System.Threading.ThreadStart(GetChangeLog));
			_getChangeLogThread.SetApartmentState(ApartmentState.STA);
			_getChangeLogThread.IsBackground = true;
			_getChangeLogThread.Start();
		}
			
		public void GetAnnouncementInfoAsync()
		{
			if (IsGetAnnouncementInfoRunning)
			{
				_getAnnouncementInfoThread.Abort();
			}
				
			_getAnnouncementInfoThread = new Thread(new System.Threading.ThreadStart(GetAnnouncementInfo));
			_getAnnouncementInfoThread.SetApartmentState(ApartmentState.STA);
			_getAnnouncementInfoThread.IsBackground = true;
			_getAnnouncementInfoThread.Start();
		}
			
		public void DownloadUpdateAsync()
		{
			if (_downloadUpdateWebClient != null)
			{
				throw (new InvalidOperationException("A previous call to DownloadUpdateAsync() is still in progress."));
			}
				
			if (_currentUpdateInfo == null || !_currentUpdateInfo.IsValid)
			{
				throw (new InvalidOperationException("CurrentUpdateInfo is not valid. GetUpdateInfoAsync() must be called before calling DownloadUpdateAsync()."));
			}
				
			_currentUpdateInfo.UpdateFilePath = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetRandomFileName(), "exe"));
			DownloadUpdateWebClient.DownloadFileAsync(CurrentUpdateInfo.DownloadAddress, _currentUpdateInfo.UpdateFilePath);
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
			
        #region Private Fields
		private WebProxy _webProxy;
		private Thread _getUpdateInfoThread;
		private Thread _getChangeLogThread;
		private Thread _getAnnouncementInfoThread;
        #endregion
			
        #region Private Methods
		private WebClient CreateWebClient()
		{
			WebClient webClient = new WebClient();
			webClient.Headers.Add("user-agent", Info.General.UserAgent);
			webClient.Proxy = _webProxy;
			return webClient;
		}
			
		private static DownloadStringCompletedEventArgs NewDownloadStringCompletedEventArgs(string result, Exception exception, bool cancelled, object userToken)
		{
			Type type = typeof(DownloadStringCompletedEventArgs);
            const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
			Type[] argumentTypes = new Type[] {typeof(string), typeof(Exception), typeof(bool), typeof(object)};
			ConstructorInfo constructor = type.GetConstructor(bindingFlags, null, argumentTypes, null);
			object[] arguments = new object[] {result, exception, cancelled, userToken};

            return (DownloadStringCompletedEventArgs)constructor.Invoke(arguments);
		}
			
		private DownloadStringCompletedEventArgs DownloadString(Uri address)
		{
			WebClient webClient = CreateWebClient();
			string result = string.Empty;
			Exception exception = null;
			bool cancelled = false;
				
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
			Uri updateFileUri = new Uri(new Uri(System.Convert.ToString(My.Settings.Default.UpdateAddress)), new Uri(Info.Update.FileName, UriKind.Relative));
			DownloadStringCompletedEventArgs e = DownloadString(updateFileUri);
				
			if (!e.Cancelled && e.Error == null)
			{
				_currentUpdateInfo = UpdateInfo.FromString(e.Result);
					
				My.Settings.Default.CheckForUpdatesLastCheck = DateTime.UtcNow;
				if (!My.Settings.Default.UpdatePending)
				{
					My.Settings.Default.UpdatePending = IsUpdateAvailable();
				}
			}
				
			if (GetUpdateInfoCompletedEventEvent != null)
				GetUpdateInfoCompletedEventEvent(this, e);
		}
			
		private void GetChangeLog()
		{
			DownloadStringCompletedEventArgs e = DownloadString(_currentUpdateInfo.ChangeLogAddress);
				
			if (!e.Cancelled && e.Error == null)
			{
				_changeLog = e.Result;
			}
				
			if (GetChangeLogCompletedEventEvent != null)
				GetChangeLogCompletedEventEvent(this, e);
		}
			
		private void GetAnnouncementInfo()
		{
			Uri announcementFileUri = new Uri(System.Convert.ToString(My.Settings.Default.AnnouncementAddress));
			DownloadStringCompletedEventArgs e = DownloadString(announcementFileUri);
				
			if (!e.Cancelled && e.Error == null)
			{
				_currentAnnouncementInfo = AnnouncementInfo.FromString(e.Result);
					
				if (!string.IsNullOrEmpty(_currentAnnouncementInfo.Name))
				{
					My.Settings.Default.LastAnnouncement = _currentAnnouncementInfo.Name;
				}
			}
				
			if (GetAnnouncementInfoCompletedEventEvent != null)
				GetAnnouncementInfoCompletedEventEvent(this, e);
		}
			
		private void DownloadUpdateProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			if (DownloadUpdateProgressChangedEventEvent != null)
				DownloadUpdateProgressChangedEventEvent(sender, e);
		}
			
		private void DownloadUpdateCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
		{
			AsyncCompletedEventArgs raiseEventArgs = e;
				
			if (!e.Cancelled && e.Error == null)
			{
				try
				{
					Authenticode updateAuthenticode = new Authenticode(_currentUpdateInfo.UpdateFilePath);
					updateAuthenticode.RequireThumbprintMatch = true;
					updateAuthenticode.ThumbprintToMatch = _currentUpdateInfo.CertificateThumbprint;
						
					if (!(updateAuthenticode.Verify() == Authenticode.StatusValue.Verified))
					{
						if (updateAuthenticode.Status == Authenticode.StatusValue.UnhandledException)
						{
							throw (updateAuthenticode.Exception);
						}
						else
						{
							throw (new Exception(updateAuthenticode.StatusMessage));
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
				File.Delete(_currentUpdateInfo.UpdateFilePath);
			}
				
			if (DownloadUpdateCompletedEventEvent != null)
				DownloadUpdateCompletedEventEvent(this, raiseEventArgs);
				
			_downloadUpdateWebClient.Dispose();
			_downloadUpdateWebClient = null;
		}
        #endregion
			
        #region Public Classes
		public class UpdateInfo
		{
            #region Public Properties
			public bool IsValid {get; set;}
			public Version Version {get; set;}
			public Uri DownloadAddress {get; set;}
			public string UpdateFilePath {get; set;}
			public Uri ChangeLogAddress {get; set;}
			public Uri ImageAddress {get; set;}
			public Uri ImageLinkAddress {get; set;}
			public string CertificateThumbprint {get; set;}
            #endregion
				
            #region Public Methods
			public static UpdateInfo FromString(string input)
			{
				UpdateInfo newInfo = new UpdateInfo();
				if (string.IsNullOrEmpty(input))
				{
					newInfo.IsValid = false;
				}
				else
				{
					UpdateFile updateFile = new UpdateFile(input);
					newInfo.Version = updateFile.GetVersion("Version");
					newInfo.DownloadAddress = updateFile.GetUri("dURL");
					newInfo.ChangeLogAddress = updateFile.GetUri("clURL");
					newInfo.ImageAddress = updateFile.GetUri("imgURL");
					newInfo.ImageLinkAddress = updateFile.GetUri("imgURLLink");
					newInfo.CertificateThumbprint = updateFile.GetThumbprint("CertificateThumbprint");
					newInfo.IsValid = true;
				}
				return newInfo;
			}
            #endregion
		}
			
		public class AnnouncementInfo
		{
            #region Public Properties
			public bool IsValid {get; set;}
			public string Name {get; set;}
			public Uri Address {get; set;}
            #endregion
				
            #region Public Methods
			public static AnnouncementInfo FromString(string input)
			{
				AnnouncementInfo newInfo = new AnnouncementInfo();
				if (string.IsNullOrEmpty(input))
				{
					newInfo.IsValid = false;
				}
				else
				{
					UpdateFile updateFile = new UpdateFile(input);
					newInfo.Name = updateFile.GetString("Name");
					newInfo.Address = updateFile.GetUri("URL");
					newInfo.IsValid = true;
				}
				return newInfo;
			}
            #endregion
		}
        #endregion
			
        #region Private Classes
		private class UpdateFile
		{
            #region Public Properties
			private Dictionary<string, string> _items = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
			// ReSharper disable MemberCanBePrivate.Local
            public Dictionary<string, string> Items
			{
				// ReSharper restore MemberCanBePrivate.Local
				get
				{
					return _items;
				}
			}
            #endregion
				
            #region Public Methods
			public UpdateFile(string content)
			{
				FromString(content);
			}
				
			// ReSharper disable MemberCanBePrivate.Local
			public void FromString(string content)
			{
				// ReSharper restore MemberCanBePrivate.Local
				if (string.IsNullOrEmpty(content))
				{
				}
				else
				{
					char[] lineSeparators = new char[] {'\n', '\r'};
					char[] keyValueSeparators = new char[] {':', '='};
					char[] commentCharacters = new char[] {'#', ';', '\''};
						
					string[] lines = content.Split(lineSeparators.ToString().ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
					foreach (string line in lines)
					{
						string trimmedLine = line.Trim();
                        if (trimmedLine.Length == 0)
						{
							continue;
						}
                        if (!(trimmedLine.Substring(0, 1).IndexOfAny(commentCharacters.ToString().ToCharArray()) == -1))
						{
							continue;
						}

                        string[] parts = trimmedLine.Split(keyValueSeparators.ToString().ToCharArray(), 2);
						if (!(parts.Length == 2))
						{
							continue;
						}
						string key = System.Convert.ToString(parts[0].Trim());
						string value = System.Convert.ToString(parts[1].Trim());
							
						_items.Add(key, value);
					}
				}
			}
				
			// ReSharper disable MemberCanBePrivate.Local
			public string GetString(string key)
			{
				// ReSharper restore MemberCanBePrivate.Local
				if (!Items.ContainsKey(key))
				{
					return string.Empty;
				}
				return this._items[key];
			}
				
			public Version GetVersion(string key)
			{
				string value = GetString(key);
				if (string.IsNullOrEmpty(value))
				{
					return null;
				}
				return new Version(value);
			}
				
			public Uri GetUri(string key)
			{
				string value = GetString(key);
				if (string.IsNullOrEmpty(value))
				{
					return null;
				}
				return new Uri(value);
			}
				
			public string GetThumbprint(string key)
			{
				return GetString(key).Replace(" ", "").ToUpperInvariant();
			}
        #endregion
		}
        #endregion
	}
}
