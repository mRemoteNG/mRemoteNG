using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
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
		#region "Events"
		public event AsyncCompletedEventHandler GetUpdateInfoCompletedEvent;
		public event AsyncCompletedEventHandler GetChangeLogCompletedEvent;
		public event AsyncCompletedEventHandler GetAnnouncementInfoCompletedEvent;
		public event DownloadProgressChangedEventHandler DownloadUpdateProgressChangedEvent;
		public event AsyncCompletedEventHandler DownloadUpdateCompletedEvent;
		#endregion

		#region "Public Properties"
		private UpdateInfo _currentUpdateInfo;
		public UpdateInfo CurrentUpdateInfo {
			get { return _currentUpdateInfo; }
		}

		private string _changeLog;
		public string ChangeLog {
			get { return _changeLog; }
		}

		private AnnouncementInfo _currentAnnouncementInfo;
		public AnnouncementInfo CurrentAnnouncementInfo {
			get { return _currentAnnouncementInfo; }
		}

		public bool IsGetUpdateInfoRunning {
			get {
				if (_getUpdateInfoThread != null) {
					if (_getUpdateInfoThread.IsAlive)
						return true;
				}
				return false;
			}
		}

		public bool IsGetChangeLogRunning {
			get {
				if (_getChangeLogThread != null) {
					if (_getChangeLogThread.IsAlive)
						return true;
				}
				return false;
			}
		}

		public bool IsGetAnnouncementInfoRunning {
			get {
				if (_getAnnouncementInfoThread != null) {
					if (_getAnnouncementInfoThread.IsAlive)
						return true;
				}
				return false;
			}
		}

		public bool IsDownloadUpdateRunning {
			get { return (_downloadUpdateWebClient != null); }
		}
		#endregion

		#region "Public Methods"
		public Update()
		{
			SetProxySettings();
		}

		public void SetProxySettings()
		{
			SetProxySettings(mRemoteNG.My.Settings.UpdateUseProxy, mRemoteNG.My.Settings.UpdateProxyAddress, mRemoteNG.My.Settings.UpdateProxyPort, mRemoteNG.My.Settings.UpdateProxyUseAuthentication, mRemoteNG.My.Settings.UpdateProxyAuthUser, mRemoteNG.Security.Crypt.Decrypt(mRemoteNG.My.Settings.UpdateProxyAuthPass, mRemoteNG.App.Info.General.EncryptionKey));
		}

		public void SetProxySettings(bool useProxy, string address, int port, bool useAuthentication, string username, string password)
		{
			if (useProxy & !string.IsNullOrEmpty(address)) {
				if (!(port == 0)) {
					_webProxy = new WebProxy(address, port);
				} else {
					_webProxy = new WebProxy(address);
				}

				if (useAuthentication) {
					_webProxy.Credentials = new NetworkCredential(username, password);
				} else {
					_webProxy.Credentials = null;
				}
			} else {
				_webProxy = null;
			}
		}

		public bool IsUpdateAvailable()
		{
			if (_currentUpdateInfo == null || !_currentUpdateInfo.IsValid)
				return false;

			return _currentUpdateInfo.Version > mRemoteNG.My.MyProject.Application.Info.Version;
		}

		public bool IsAnnouncementAvailable()
		{
			if (_currentAnnouncementInfo == null || (!_currentAnnouncementInfo.IsValid | string.IsNullOrEmpty(_currentAnnouncementInfo.Name)))
				return false;

			return (!(_currentAnnouncementInfo.Name == mRemoteNG.My.Settings.LastAnnouncement));
		}

		public void GetUpdateInfoAsync()
		{
			if (IsGetUpdateInfoRunning)
				_getUpdateInfoThread.Abort();

			_getUpdateInfoThread = new Thread(GetUpdateInfo);
			var _with1 = _getUpdateInfoThread;
			_with1.SetApartmentState(ApartmentState.STA);
			_with1.IsBackground = true;
			_with1.Start();
		}

		public void GetChangeLogAsync()
		{
			if (_currentUpdateInfo == null || !_currentUpdateInfo.IsValid) {
				throw new InvalidOperationException("CurrentUpdateInfo is not valid. GetUpdateInfoAsync() must be called before calling GetChangeLogAsync().");
			}

			if (IsGetChangeLogRunning)
				_getChangeLogThread.Abort();

			_getChangeLogThread = new Thread(GetChangeLog);
			var _with2 = _getChangeLogThread;
			_with2.SetApartmentState(ApartmentState.STA);
			_with2.IsBackground = true;
			_with2.Start();
		}

		public void GetAnnouncementInfoAsync()
		{
			if (IsGetAnnouncementInfoRunning)
				_getAnnouncementInfoThread.Abort();

			_getAnnouncementInfoThread = new Thread(GetAnnouncementInfo);
			var _with3 = _getAnnouncementInfoThread;
			_with3.SetApartmentState(ApartmentState.STA);
			_with3.IsBackground = true;
			_with3.Start();
		}

		public void DownloadUpdateAsync()
		{
			if (_downloadUpdateWebClient != null) {
				throw new InvalidOperationException("A previous call to DownloadUpdateAsync() is still in progress.");
			}

			if (_currentUpdateInfo == null || !_currentUpdateInfo.IsValid) {
				throw new InvalidOperationException("CurrentUpdateInfo is not valid. GetUpdateInfoAsync() must be called before calling DownloadUpdateAsync().");
			}

			_currentUpdateInfo.UpdateFilePath = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetRandomFileName(), "exe"));
			DownloadUpdateWebClient.DownloadFileAsync(CurrentUpdateInfo.DownloadAddress, _currentUpdateInfo.UpdateFilePath);
		}
		#endregion

		#region "Private Properties"
		private WebClient _downloadUpdateWebClient;
		private WebClient DownloadUpdateWebClient {
			get {
				if (_downloadUpdateWebClient != null)
					return _downloadUpdateWebClient;

				_downloadUpdateWebClient = CreateWebClient();

				_downloadUpdateWebClient.DownloadProgressChanged += DownloadUpdateProgressChanged;
				_downloadUpdateWebClient.DownloadFileCompleted += DownloadUpdateCompleted;

				return _downloadUpdateWebClient;
			}
		}
		#endregion

		#region "Private Fields"
		private WebProxy _webProxy;
		private Thread _getUpdateInfoThread;
		private Thread _getChangeLogThread;
			#endregion
		private Thread _getAnnouncementInfoThread;

		#region "Private Methods"
		private WebClient CreateWebClient()
		{
			WebClient webClient = new WebClient();
			webClient.Headers.Add("user-agent", mRemoteNG.App.Info.General.UserAgent);
			webClient.Proxy = _webProxy;
			return webClient;
		}

		private static DownloadStringCompletedEventArgs NewDownloadStringCompletedEventArgs(string result, Exception exception, bool cancelled, object userToken)
		{
			Type type = typeof(DownloadStringCompletedEventArgs);
			const BindingFlags bindingFlags = bindingFlags.NonPublic | bindingFlags.Instance;
			Type[] argumentTypes = {
				typeof(string),
				typeof(Exception),
				typeof(bool),
				typeof(object)
			};
			ConstructorInfo constructor = type.GetConstructor(bindingFlags, null, argumentTypes, null);
			object[] arguments = {
				result,
				exception,
				cancelled,
				userToken
			};

			return constructor.Invoke(arguments);
		}

		private DownloadStringCompletedEventArgs DownloadString(Uri address)
		{
			WebClient webClient = CreateWebClient();
			string result = string.Empty;
			Exception exception = null;
			bool cancelled = false;

			try {
				result = webClient.DownloadString(address);
			} catch (ThreadAbortException ex) {
				cancelled = true;
			} catch (Exception ex) {
				exception = ex;
			}

			return NewDownloadStringCompletedEventArgs(result, exception, cancelled, null);
		}

		private void GetUpdateInfo()
		{
			Uri updateFileUri = new Uri(new Uri(mRemoteNG.My.Settings.UpdateAddress), new Uri(mRemoteNG.App.Info.Update.FileName, UriKind.Relative));
			DownloadStringCompletedEventArgs e = DownloadString(updateFileUri);

			if (!e.Cancelled & e.Error == null) {
				_currentUpdateInfo = UpdateInfo.FromString(e.Result);

				mRemoteNG.My.Settings.CheckForUpdatesLastCheck = System.DateTime.UtcNow;
				if (!mRemoteNG.My.Settings.UpdatePending) {
					mRemoteNG.My.Settings.UpdatePending = IsUpdateAvailable();
				}
			}

			if (GetUpdateInfoCompletedEvent != null) {
				GetUpdateInfoCompletedEvent(this, e);
			}
		}

		private void GetChangeLog()
		{
			DownloadStringCompletedEventArgs e = DownloadString(_currentUpdateInfo.ChangeLogAddress);

			if (!e.Cancelled & e.Error == null)
				_changeLog = e.Result;

			if (GetChangeLogCompletedEvent != null) {
				GetChangeLogCompletedEvent(this, e);
			}
		}

		private void GetAnnouncementInfo()
		{
			Uri announcementFileUri = new Uri(mRemoteNG.My.Settings.AnnouncementAddress);
			DownloadStringCompletedEventArgs e = DownloadString(announcementFileUri);

			if (!e.Cancelled & e.Error == null) {
				_currentAnnouncementInfo = AnnouncementInfo.FromString(e.Result);

				if (!string.IsNullOrEmpty(_currentAnnouncementInfo.Name)) {
					mRemoteNG.My.Settings.LastAnnouncement = _currentAnnouncementInfo.Name;
				}
			}

			if (GetAnnouncementInfoCompletedEvent != null) {
				GetAnnouncementInfoCompletedEvent(this, e);
			}
		}

		private void DownloadUpdateProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			if (DownloadUpdateProgressChangedEvent != null) {
				DownloadUpdateProgressChangedEvent(sender, e);
			}
		}

		private void DownloadUpdateCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
		{
			AsyncCompletedEventArgs raiseEventArgs = e;

			if (!e.Cancelled & e.Error == null) {
				try {
					Authenticode updateAuthenticode = new Authenticode(_currentUpdateInfo.UpdateFilePath);
					var _with4 = updateAuthenticode;
					_with4.RequireThumbprintMatch = true;
					_with4.ThumbprintToMatch = _currentUpdateInfo.CertificateThumbprint;

					if (!(_with4.Verify() == Authenticode.StatusValue.Verified)) {
						if (_with4.Status == Authenticode.StatusValue.UnhandledException) {
							throw _with4.Exception;
						} else {
							throw new Exception(_with4.StatusMessage);
						}
					}
				} catch (Exception ex) {
					raiseEventArgs = new AsyncCompletedEventArgs(ex, false, null);
				}
			}

			if (raiseEventArgs.Cancelled | raiseEventArgs.Error != null) {
				File.Delete(_currentUpdateInfo.UpdateFilePath);
			}

			if (DownloadUpdateCompletedEvent != null) {
				DownloadUpdateCompletedEvent(this, raiseEventArgs);
			}

			_downloadUpdateWebClient.Dispose();
			_downloadUpdateWebClient = null;
		}
		#endregion

		#region "Public Classes"
		public class UpdateInfo
		{
			#region "Public Properties"
			public bool IsValid { get; set; }
			public Version Version { get; set; }
			public Uri DownloadAddress { get; set; }
			public string UpdateFilePath { get; set; }
			public Uri ChangeLogAddress { get; set; }
			public Uri ImageAddress { get; set; }
			public Uri ImageLinkAddress { get; set; }
			public string CertificateThumbprint { get; set; }
			#endregion

			#region "Public Methods"
			public static UpdateInfo FromString(string input)
			{
				UpdateInfo newInfo = new UpdateInfo();
				var _with5 = newInfo;
				if (string.IsNullOrEmpty(input)) {
					_with5.IsValid = false;
				} else {
					UpdateFile updateFile = new UpdateFile(input);
					_with5.Version = updateFile.GetVersion("Version");
					_with5.DownloadAddress = updateFile.GetUri("dURL");
					_with5.ChangeLogAddress = updateFile.GetUri("clURL");
					_with5.ImageAddress = updateFile.GetUri("imgURL");
					_with5.ImageLinkAddress = updateFile.GetUri("imgURLLink");
					_with5.CertificateThumbprint = updateFile.GetThumbprint("CertificateThumbprint");
					_with5.IsValid = true;
				}
				return newInfo;
			}
			#endregion
		}

		public class AnnouncementInfo
		{
			#region "Public Properties"
			public bool IsValid { get; set; }
			public string Name { get; set; }
			public Uri Address { get; set; }
			#endregion

			#region "Public Methods"
			public static AnnouncementInfo FromString(string input)
			{
				AnnouncementInfo newInfo = new AnnouncementInfo();
				var _with6 = newInfo;
				if (string.IsNullOrEmpty(input)) {
					_with6.IsValid = false;
				} else {
					UpdateFile updateFile = new UpdateFile(input);
					_with6.Name = updateFile.GetString("Name");
					_with6.Address = updateFile.GetUri("URL");
					_with6.IsValid = true;
				}
				return newInfo;
			}
			#endregion
		}
		#endregion

		#region "Private Classes"
		private class UpdateFile
		{
			#region "Public Properties"
			private readonly Dictionary<string, string> _items = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
			// ReSharper disable MemberCanBePrivate.Local
			public Dictionary<string, string> Items {
				// ReSharper restore MemberCanBePrivate.Local
				get { return _items; }
			}
			#endregion

			#region "Public Methods"
			public UpdateFile(string content)
			{
				FromString(content);
			}

			// ReSharper disable MemberCanBePrivate.Local
			public void FromString(string content)
			{
				// ReSharper restore MemberCanBePrivate.Local
				if (string.IsNullOrEmpty(content)) {
				} else {
					char[] lineSeparators = new char[] {
						Strings.Chr(0xa),
						Strings.Chr(0xd)
					};
					char[] keyValueSeparators = new char[] {
						":",
						"="
					};
					char[] commentCharacters = new char[] {
						"#",
						";",
						"'"
					};

					string[] lines = content.Split(lineSeparators, StringSplitOptions.RemoveEmptyEntries);
					foreach (string line in lines) {
						line = line.Trim();
						if (line.Length == 0)
							continue;
						if (!(line.Substring(0, 1).IndexOfAny(commentCharacters) == -1))
							continue;

						string[] parts = line.Split(keyValueSeparators, 2);
						if (!(parts.Length == 2))
							continue;
						string key = parts[0].Trim();
						string value = parts[1].Trim();

						_items.Add(key, value);
					}
				}
			}

			// ReSharper disable MemberCanBePrivate.Local
			public string GetString(string key)
			{
				// ReSharper restore MemberCanBePrivate.Local
				if (!Items.ContainsKey(key))
					return string.Empty;
				return Items[key];
			}

			public Version GetVersion(string key)
			{
				string value = GetString(key);
				if (string.IsNullOrEmpty(value))
					return null;
				return new Version(value);
			}

			public Uri GetUri(string key)
			{
				string value = GetString(key);
				if (string.IsNullOrEmpty(value))
					return null;
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
