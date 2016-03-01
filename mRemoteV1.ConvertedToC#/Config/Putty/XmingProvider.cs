using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Connection.Protocol;
using System.Text.RegularExpressions;

namespace mRemoteNG.Config.Putty
{
	public class XmingProvider : Provider
	{

		#region "Public Methods"
		public override string[] GetSessionNames(bool raw = false)
		{
			string sessionsFolderPath = GetSessionsFolderPath();
			if (!Directory.Exists(sessionsFolderPath))
				return new string[];

			List<string> sessionNames = new List<string>();
			foreach (string sessionName in Directory.GetFiles(sessionsFolderPath)) {
				sessionName = Path.GetFileName(sessionName);
				if (raw) {
					sessionNames.Add(sessionName);
				} else {
					sessionNames.Add(System.Web.HttpUtility.UrlDecode(sessionName.Replace("+", "%2B")));
				}
			}

			if (raw) {
				// Do not localize
				if (!sessionNames.Contains("Default%20Settings")) {
					sessionNames.Insert(0, "Default%20Settings");
				}
			} else {
				if (!sessionNames.Contains("Default Settings")) {
					sessionNames.Insert(0, "Default Settings");
				}
			}

			List<string> registrySessionNames = new List<string>();
			foreach (string sessionName in RegistryProvider.GetSessionNames(raw)) {
				registrySessionNames.Add(string.Format(RegistrySessionNameFormat, sessionName));
			}

			sessionNames.AddRange(registrySessionNames);
			sessionNames.Sort();

			return sessionNames.ToArray();
		}

		public override Connection.PuttySession.Info GetSession(string sessionName)
		{
			string registrySessionName = GetRegistrySessionName(sessionName);
			if (!string.IsNullOrEmpty(registrySessionName)) {
				return ModifyRegistrySessionInfo(RegistryProvider.GetSession(registrySessionName));
			}

			string sessionsFolderPath = GetSessionsFolderPath();
			if (!Directory.Exists(sessionsFolderPath))
				return null;

			string sessionFile = Path.Combine(sessionsFolderPath, sessionName);
			if (!File.Exists(sessionFile))
				return null;

			sessionName = System.Web.HttpUtility.UrlDecode(sessionName.Replace("+", "%2B"));

			SessionFileReader sessionFileReader = new SessionFileReader(sessionFile);
			Connection.PuttySession.Info sessionInfo = new Connection.PuttySession.Info();
			var _with1 = sessionInfo;
			_with1.PuttySession = sessionName;
			_with1.Name = sessionName;
			_with1.Hostname = sessionFileReader.GetValue("HostName");
			_with1.Username = sessionFileReader.GetValue("UserName");
			string protocol = sessionFileReader.GetValue("Protocol");
			if (protocol == null)
				protocol = "ssh";
			switch (protocol.ToLowerInvariant()) {
				case "raw":
					_with1.Protocol = Protocols.RAW;
					break;
				case "rlogin":
					_with1.Protocol = Protocols.Rlogin;
					break;
				case "serial":
					return null;
				case "ssh":
					object sshVersionObject = sessionFileReader.GetValue("SshProt");
					if (sshVersionObject != null) {
						int sshVersion = Convert.ToInt32(sshVersionObject);
						if (sshVersion >= 2) {
							_with1.Protocol = Protocols.SSH2;
						} else {
							_with1.Protocol = Protocols.SSH1;
						}
					} else {
						_with1.Protocol = Protocols.SSH2;
					}
					break;
				case "telnet":
					_with1.Protocol = Protocols.Telnet;
					break;
				default:
					return null;
			}
			_with1.Port = sessionFileReader.GetValue("PortNumber");

			return sessionInfo;
		}

		public override void StartWatcher()
		{
			RegistryProvider.StartWatcher();
			RegistryProvider.SessionChanged += OnRegistrySessionChanged;

			if (_eventWatcher != null)
				return;

			try {
				_eventWatcher = new FileSystemWatcher(GetSessionsFolderPath());
				_eventWatcher.NotifyFilter = (NotifyFilters.FileName | NotifyFilters.LastWrite);
				_eventWatcher.Changed += OnFileSystemEventArrived;
				_eventWatcher.Created += OnFileSystemEventArrived;
				_eventWatcher.Deleted += OnFileSystemEventArrived;
				_eventWatcher.Renamed += OnFileSystemEventArrived;
				_eventWatcher.EnableRaisingEvents = true;
			} catch (Exception ex) {
				Runtime.MessageCollector.AddExceptionMessage("XmingPortablePuttySessions.Watcher.StartWatching() failed.", ex, MessageClass.WarningMsg, true);
			}
		}

		public override void StopWatcher()
		{
			RegistryProvider.StopWatcher();
			RegistryProvider.SessionChanged -= OnRegistrySessionChanged;

			if (_eventWatcher == null)
				return;
			_eventWatcher.EnableRaisingEvents = false;
			_eventWatcher.Dispose();
			_eventWatcher = null;
		}
		#endregion

		#region "Private Fields"
		private const string RegistrySessionNameFormat = "{0} [registry]";

		private const string RegistrySessionNamePattern = "(.*)\\ \\[registry\\]";
		private static readonly RegistryProvider RegistryProvider = new RegistryProvider();
			#endregion
		private static FileSystemWatcher _eventWatcher;

		#region "Private Methods"
		private static string GetPuttyConfPath()
		{
			string puttyPath = null;
			if (mRemoteNG.My.Settings.UseCustomPuttyPath) {
				puttyPath = mRemoteNG.My.Settings.CustomPuttyPath;
			} else {
				puttyPath = mRemoteNG.App.Info.General.PuttyPath;
			}
			return Path.Combine(Path.GetDirectoryName(puttyPath), "putty.conf");
		}

		private static string GetSessionsFolderPath()
		{
			string puttyConfPath = GetPuttyConfPath();
			PuttyConfFileReader sessionFileReader = new PuttyConfFileReader(puttyConfPath);
			string basePath = Environment.ExpandEnvironmentVariables(sessionFileReader.GetValue("sshk&sess"));
			return Path.Combine(basePath, "sessions");
		}

		private static string GetRegistrySessionName(string sessionName)
		{
			Regex regex = new Regex(RegistrySessionNamePattern);

			MatchCollection matches = regex.Matches(sessionName);
			if (matches.Count < 1)
				return string.Empty;

			GroupCollection groups = matches[0].Groups;
			if (groups.Count < 1)
				return string.Empty;
			// This should always include at least one item, but check anyway

			return groups[1].Value;
		}

		private static Connection.PuttySession.Info ModifyRegistrySessionInfo(Connection.PuttySession.Info sessionInfo)
		{
			sessionInfo.Name = string.Format(RegistrySessionNameFormat, sessionInfo.Name);
			sessionInfo.PuttySession = string.Format(RegistrySessionNameFormat, sessionInfo.PuttySession);
			return sessionInfo;
		}

		private void OnFileSystemEventArrived(object sender, FileSystemEventArgs e)
		{
			OnSessionChanged(new SessionChangedEventArgs());
		}

		private void OnRegistrySessionChanged(object sender, SessionChangedEventArgs e)
		{
			OnSessionChanged(new SessionChangedEventArgs());
		}
		#endregion

		#region "Private Classes"
		private class PuttyConfFileReader
		{
			public PuttyConfFileReader(string puttyConfFile)
			{
				_puttyConfFile = puttyConfFile;
			}

			private readonly string _puttyConfFile;
			private bool _configurationLoaded = false;

			private readonly Dictionary<string, string> _configuration = new Dictionary<string, string>();
			private void LoadConfiguration()
			{
				_configurationLoaded = true;
				try {
					if (!File.Exists(_puttyConfFile))
						return;
					using (StreamReader streamReader = new StreamReader(_puttyConfFile)) {
						string line = null;
						do {
							line = streamReader.ReadLine();
							if (line == null)
								break; // TODO: might not be correct. Was : Exit Do
							line = line.Trim();
							if (line == string.Empty)
								continue;
							// Blank line
							if (line.Substring(0, 1) == ";")
								continue;
							// Comment
							string[] parts = line.Split(new char[] { "=" }, 2);
							if (parts.Length < 2)
								continue;
							if (_configuration.ContainsKey(parts[0]))
								continue;
							// As per http://www.straightrunning.com/XmingNotes/portableputty.php only first entry is used
							_configuration.Add(parts[0], parts[1]);
						} while (true);
					}
				} catch (Exception ex) {
					Runtime.MessageCollector.AddExceptionMessage("PuttyConfFileReader.LoadConfiguration() failed.", ex, MessageClass.ErrorMsg, true);
				}
			}

			public string GetValue(string setting)
			{
				if (!_configurationLoaded)
					LoadConfiguration();
				if (!_configuration.ContainsKey(setting))
					return string.Empty;
				return _configuration[setting];
			}
		}

		private class SessionFileReader
		{
			public SessionFileReader(string sessionFile)
			{
				_sessionFile = sessionFile;
			}

			private readonly string _sessionFile;
			private bool _sessionInfoLoaded = false;

			private readonly Dictionary<string, string> _sessionInfo = new Dictionary<string, string>();
			private void LoadSessionInfo()
			{
				_sessionInfoLoaded = true;
				try {
					if (!File.Exists(_sessionFile))
						return;
					using (StreamReader streamReader = new StreamReader(_sessionFile)) {
						string line = null;
						do {
							line = streamReader.ReadLine();
							if (line == null)
								break; // TODO: might not be correct. Was : Exit Do
							string[] parts = line.Split(new char[] { "\\" });
							if (parts.Length < 2)
								continue;
							_sessionInfo.Add(parts[0], parts[1]);
						} while (true);
					}
				} catch (Exception ex) {
					Runtime.MessageCollector.AddExceptionMessage("SessionFileReader.LoadSessionInfo() failed.", ex, MessageClass.ErrorMsg, true);
				}
			}

			public string GetValue(string setting)
			{
				if (!_sessionInfoLoaded)
					LoadSessionInfo();
				if (!_sessionInfo.ContainsKey(setting))
					return string.Empty;
				return _sessionInfo[setting];
			}
		}
		#endregion
	}
}
