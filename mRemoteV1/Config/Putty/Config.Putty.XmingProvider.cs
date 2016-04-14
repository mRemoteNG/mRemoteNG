using System.Collections.Generic;
using System;
using AxWFICALib;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
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
        #region Public Methods
		public override string[] GetSessionNames(bool raw = false)
		{
			string sessionsFolderPath = GetSessionsFolderPath();
			if (!Directory.Exists(sessionsFolderPath))
			{
				return new string[] {};
			}
				
			List<string> sessionNames = new List<string>();
			foreach (string sessionName in Directory.GetFiles(sessionsFolderPath))
			{
				string _sessionFileName = Path.GetFileName(sessionName);
				if (raw)
				{
                    sessionNames.Add(_sessionFileName);
				}
				else
				{
                    sessionNames.Add(System.Web.HttpUtility.UrlDecode(_sessionFileName.Replace("+", "%2B")));
				}
			}
				
			if (raw)
			{
				if (!sessionNames.Contains("Default%20Settings")) // Do not localize
				{
					sessionNames.Insert(0, "Default%20Settings");
				}
			}
			else
			{
				if (!sessionNames.Contains("Default Settings"))
				{
					sessionNames.Insert(0, "Default Settings");
				}
			}
				
			List<string> registrySessionNames = new List<string>();
			foreach (string sessionName in RegistryProvider.GetSessionNames(raw))
			{
				registrySessionNames.Add(string.Format(RegistrySessionNameFormat, sessionName));
			}
				
			sessionNames.AddRange(registrySessionNames);
			sessionNames.Sort();
				
			return sessionNames.ToArray();
		}
			
		public override Connection.PuttySession.Info GetSession(string sessionName)
		{
			string registrySessionName = GetRegistrySessionName(sessionName);
			if (!string.IsNullOrEmpty(registrySessionName))
			{
				return ModifyRegistrySessionInfo(RegistryProvider.GetSession(registrySessionName));
			}
				
			string sessionsFolderPath = GetSessionsFolderPath();
			if (!Directory.Exists(sessionsFolderPath))
			{
				return null;
			}
				
			string sessionFile = Path.Combine(sessionsFolderPath, sessionName);
			if (!File.Exists(sessionFile))
			{
				return null;
			}
				
			sessionName = System.Web.HttpUtility.UrlDecode(sessionName.Replace("+", "%2B"));
				
			SessionFileReader sessionFileReader = new SessionFileReader(sessionFile);
			Connection.PuttySession.Info sessionInfo = new Connection.PuttySession.Info();
			sessionInfo.PuttySession = sessionName;
			sessionInfo.Name = sessionName;
			sessionInfo.Hostname = sessionFileReader.GetValue("HostName");
			sessionInfo.Username = sessionFileReader.GetValue("UserName");
			string protocol = sessionFileReader.GetValue("Protocol");
			if (protocol == null)
			{
				protocol = "ssh";
			}
			switch (protocol.ToLowerInvariant())
			{
				case "raw":
					sessionInfo.Protocol = Protocols.RAW;
					break;
				case "rlogin":
					sessionInfo.Protocol = Protocols.Rlogin;
					break;
				case "serial":
					return null;
				case "ssh":
					object sshVersionObject = sessionFileReader.GetValue("SshProt");
					if (sshVersionObject != null)
					{
						int sshVersion = System.Convert.ToInt32(sshVersionObject);
						if (sshVersion >= 2)
						{
							sessionInfo.Protocol = Protocols.SSH2;
						}
						else
						{
							sessionInfo.Protocol = Protocols.SSH1;
						}
					}
					else
					{
						sessionInfo.Protocol = Protocols.SSH2;
					}
					break;
				case "telnet":
					sessionInfo.Protocol = Protocols.Telnet;
					break;
				default:
					return null;
			}
			sessionInfo.Port = System.Convert.ToInt32(sessionFileReader.GetValue("PortNumber"));
			
			return sessionInfo;
		}
			
		public override void StartWatcher()
		{
			RegistryProvider.StartWatcher();
			RegistryProvider.SessionChanged += OnRegistrySessionChanged;
				
			if (_eventWatcher != null)
			{
				return ;
			}
				
			try
			{
				_eventWatcher = new FileSystemWatcher(GetSessionsFolderPath());
				_eventWatcher.NotifyFilter = (System.IO.NotifyFilters) (NotifyFilters.FileName | NotifyFilters.LastWrite);
				_eventWatcher.Changed += OnFileSystemEventArrived;
				_eventWatcher.Created += OnFileSystemEventArrived;
				_eventWatcher.Deleted += OnFileSystemEventArrived;
				_eventWatcher.Renamed += OnFileSystemEventArrived;
				_eventWatcher.EnableRaisingEvents = true;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("XmingPortablePuttySessions.Watcher.StartWatching() failed.", ex, MessageClass.WarningMsg, true);
			}
		}
			
		public override void StopWatcher()
		{
			RegistryProvider.StopWatcher();
			RegistryProvider.SessionChanged -= OnRegistrySessionChanged;
				
			if (_eventWatcher == null)
			{
				return ;
			}
			_eventWatcher.EnableRaisingEvents = false;
			_eventWatcher.Dispose();
			_eventWatcher = null;
		}
        #endregion
			
        #region Private Fields
		private const string RegistrySessionNameFormat = "{0} [registry]";
		private const string RegistrySessionNamePattern = "(.*)\\ \\[registry\\]";
			
		private static readonly RegistryProvider RegistryProvider = new RegistryProvider();
		private static FileSystemWatcher _eventWatcher;
        #endregion
			
        #region Private Methods
		private static string GetPuttyConfPath()
		{
			string puttyPath = "";
			if (My.Settings.Default.UseCustomPuttyPath)
			{
				puttyPath = System.Convert.ToString(My.Settings.Default.CustomPuttyPath);
			}
			else
			{
				puttyPath = App.Info.General.PuttyPath;
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
			{
				return string.Empty;
			}
				
			GroupCollection groups = matches[0].Groups;
			if (groups.Count < 1)
			{
				return string.Empty; // This should always include at least one item, but check anyway
			}
				
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
			
        #region Private Classes
		private class PuttyConfFileReader
		{
			public PuttyConfFileReader(string puttyConfFile)
			{
				_puttyConfFile = puttyConfFile;
			}
				
			private string _puttyConfFile;
			private bool _configurationLoaded = false;
			private Dictionary<string, string> _configuration = new Dictionary<string, string>();
				
			private void LoadConfiguration()
			{
				_configurationLoaded = true;
				try
				{
					if (!File.Exists(_puttyConfFile))
					{
						return ;
					}
					using (StreamReader streamReader = new StreamReader(_puttyConfFile))
					{
						string line = "";
						do
						{
							line = streamReader.ReadLine();
							if (line == null)
							{
								break;
							}
							line = line.Trim();
							if (line == string.Empty)
							{
								continue; // Blank line
							}
							if (line.Substring(0, 1) == ";")
							{
								continue; // Comment
							}
							string[] parts = line.Split(new char[] {'='}, 2);
							if (parts.Length < 2)
							{
								continue;
							}
							if (_configuration.ContainsKey(parts[0]))
							{
								continue; // As per http://www.straightrunning.com/XmingNotes/portableputty.php only first entry is used
							}
							_configuration.Add(parts[0], parts[1]);
						} while (true);
					}
						
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddExceptionMessage("PuttyConfFileReader.LoadConfiguration() failed.", ex, MessageClass.ErrorMsg, true);
				}
			}
				
			public string GetValue(string setting)
			{
				if (!_configurationLoaded)
				{
					LoadConfiguration();
				}
				if (!_configuration.ContainsKey(setting))
				{
					return string.Empty;
				}
				return _configuration[setting];
			}
		}
			
		private class SessionFileReader
		{
			public SessionFileReader(string sessionFile)
			{
				_sessionFile = sessionFile;
			}
				
			private string _sessionFile;
			private bool _sessionInfoLoaded = false;
			private Dictionary<string, string> _sessionInfo = new Dictionary<string, string>();
				
			private void LoadSessionInfo()
			{
				_sessionInfoLoaded = true;
				try
				{
					if (!File.Exists(_sessionFile))
					{
						return ;
					}
					using (StreamReader streamReader = new StreamReader(_sessionFile))
					{
						string line = "";
						do
						{
							line = streamReader.ReadLine();
							if (line == null)
							{
								break;
							}
							string[] parts = line.Split(new char[] {'\\'});
							if (parts.Length < 2)
							{
								continue;
							}
							_sessionInfo.Add(parts[0], parts[1]);
						} while (true);
					}
						
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddExceptionMessage("SessionFileReader.LoadSessionInfo() failed.", ex, MessageClass.ErrorMsg, true);
				}
			}
				
			public string GetValue(string setting)
			{
				if (!_sessionInfoLoaded)
				{
					LoadSessionInfo();
				}
				if (!_sessionInfo.ContainsKey(setting))
				{
					return string.Empty;
				}
				return _sessionInfo[setting];
			}
		}
        #endregion
	}
}
