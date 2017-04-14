using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.Config.Putty
{
	public class PuttySessionsXmingProvider : AbstractPuttySessionsProvider
	{
        public override RootPuttySessionsNodeInfo RootInfo { get; } = new RootPuttySessionsNodeInfo { Name = "Xming Putty Sessions" };
        private const string RegistrySessionNameFormat = "{0} [registry]";
        private const string RegistrySessionNamePattern = "(.*)\\ \\[registry\\]";
        private static readonly PuttySessionsRegistryProvider PuttySessionsRegistryProvider = new PuttySessionsRegistryProvider();
        private static FileSystemWatcher _eventWatcher;

        #region Public Methods
		public override string[] GetSessionNames(bool raw = false)
		{
			var sessionsFolderPath = GetSessionsFolderPath();
			if (!Directory.Exists(sessionsFolderPath))
			{
				return new string[] {};
			}

            var sessionNames = new List<string>();
			foreach (var sessionName in Directory.GetFiles(sessionsFolderPath))
			{
			    var sessionFileName = Path.GetFileName(sessionName);
			    sessionNames.Add(raw ? sessionFileName : System.Web.HttpUtility.UrlDecode(sessionFileName?.Replace("+", "%2B")));
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

            var registrySessionNames = PuttySessionsRegistryProvider.GetSessionNames(raw).Select(sessionName => string.Format(RegistrySessionNameFormat, sessionName)).ToList();

		    sessionNames.AddRange(registrySessionNames);
			sessionNames.Sort();
				
			return sessionNames.ToArray();
		}
			
		public override PuttySessionInfo GetSession(string sessionName)
		{
            var registrySessionName = GetRegistrySessionName(sessionName);
			if (!string.IsNullOrEmpty(registrySessionName))
			{
				return ModifyRegistrySessionInfo(PuttySessionsRegistryProvider.GetSession(registrySessionName));
			}

            var sessionsFolderPath = GetSessionsFolderPath();
			if (!Directory.Exists(sessionsFolderPath))
			{
				return null;
			}

            var sessionFile = Path.Combine(sessionsFolderPath, sessionName);
			if (!File.Exists(sessionFile))
			{
				return null;
			}
				
			sessionName = System.Web.HttpUtility.UrlDecode(sessionName.Replace("+", "%2B"));

            var sessionFileReader = new SessionFileReader(sessionFile);
		    var sessionInfo = new PuttySessionInfo
		    {
		        PuttySession = sessionName,
		        Name = sessionName,
		        Hostname = sessionFileReader.GetValue("HostName"),
		        Username = sessionFileReader.GetValue("UserName")
		    };
		    var protocol = sessionFileReader.GetValue("Protocol") ?? "ssh";
		    switch (protocol.ToLowerInvariant())
			{
				case "raw":
					sessionInfo.Protocol = ProtocolType.RAW;
					break;
				case "rlogin":
					sessionInfo.Protocol = ProtocolType.Rlogin;
					break;
				case "serial":
					return null;
				case "ssh":
					object sshVersionObject = sessionFileReader.GetValue("SshProt");
					if (sshVersionObject != null)
					{
					    var sshVersion = Convert.ToInt32(sshVersionObject);
					    sessionInfo.Protocol = sshVersion >= 2 ? ProtocolType.SSH2 : ProtocolType.SSH1;
					}
					else
					{
						sessionInfo.Protocol = ProtocolType.SSH2;
					}
					break;
				case "telnet":
					sessionInfo.Protocol = ProtocolType.Telnet;
					break;
				default:
					return null;
			}
			sessionInfo.Port = Convert.ToInt32(sessionFileReader.GetValue("PortNumber"));
			
			return sessionInfo;
		}
			
		public override void StartWatcher()
		{
			PuttySessionsRegistryProvider.StartWatcher();
			PuttySessionsRegistryProvider.PuttySessionChanged += OnRegistrySessionChanged;
				
			if (_eventWatcher != null)
			{
				return;
			}
				
			try
			{
			    _eventWatcher = new FileSystemWatcher(GetSessionsFolderPath())
			    {
			        NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
			    };
			    _eventWatcher.Changed += OnFileSystemEventArrived;
				_eventWatcher.Created += OnFileSystemEventArrived;
				_eventWatcher.Deleted += OnFileSystemEventArrived;
				_eventWatcher.Renamed += OnFileSystemEventArrived;
				_eventWatcher.EnableRaisingEvents = true;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("XmingPortablePuttySessions.Watcher.StartWatching() failed.", ex, MessageClass.WarningMsg);
			}
		}
			
		public override void StopWatcher()
		{
			PuttySessionsRegistryProvider.StopWatcher();
			PuttySessionsRegistryProvider.PuttySessionChanged -= OnRegistrySessionChanged;
				
			if (_eventWatcher == null)
			{
				return;
			}
			_eventWatcher.EnableRaisingEvents = false;
			_eventWatcher.Dispose();
		}
        #endregion
		
        #region Private Methods
		private static string GetPuttyConfPath()
		{
		    var puttyPath = mRemoteNG.Settings.Default.UseCustomPuttyPath ? mRemoteNG.Settings.Default.CustomPuttyPath : App.Info.GeneralAppInfo.PuttyPath;
		    return Path.Combine(puttyPath, "putty.conf");
		}
			
		private static string GetSessionsFolderPath()
		{
            var puttyConfPath = GetPuttyConfPath();
            var sessionFileReader = new PuttyConfFileReader(puttyConfPath);
            var basePath = Environment.ExpandEnvironmentVariables(sessionFileReader.GetValue("sshk&sess"));
			return Path.Combine(basePath, "sessions");
		}
			
		private static string GetRegistrySessionName(string sessionName)
		{
            var regex = new Regex(RegistrySessionNamePattern);

            var matches = regex.Matches(sessionName);
			if (matches.Count < 1)
			{
				return string.Empty;
			}

            var groups = matches[0].Groups;
			return groups.Count < 1 ? string.Empty : groups[1].Value;
		}
			
		private static PuttySessionInfo ModifyRegistrySessionInfo(PuttySessionInfo sessionInfo)
		{
			sessionInfo.Name = string.Format(RegistrySessionNameFormat, sessionInfo.Name);
			sessionInfo.PuttySession = string.Format(RegistrySessionNameFormat, sessionInfo.PuttySession);
			return sessionInfo;
		}
			
		private void OnFileSystemEventArrived(object sender, FileSystemEventArgs e)
		{
			RaiseSessionChangedEvent(new PuttySessionChangedEventArgs());
		}
			
		private void OnRegistrySessionChanged(object sender, PuttySessionChangedEventArgs e)
		{
			RaiseSessionChangedEvent(new PuttySessionChangedEventArgs());
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
			private bool _configurationLoaded;
			private Dictionary<string, string> _configuration = new Dictionary<string, string>();
				
			private void LoadConfiguration()
			{
				_configurationLoaded = true;
				try
				{
					if (!File.Exists(_puttyConfFile))
					{
						return;
					}
					using (var streamReader = new StreamReader(_puttyConfFile))
					{
					    do
						{
							var line = streamReader.ReadLine();
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
                            var parts = line.Split(new[] {'='}, 2);
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
					Runtime.MessageCollector.AddExceptionMessage("PuttyConfFileReader.LoadConfiguration() failed.", ex);
				}
			}
				
			public string GetValue(string setting)
			{
				if (!_configurationLoaded)
				{
					LoadConfiguration();
				}
				return !_configuration.ContainsKey(setting) ? string.Empty : _configuration[setting];
			}
		}
			
		private class SessionFileReader
		{
			public SessionFileReader(string sessionFile)
			{
				_sessionFile = sessionFile;
			}
				
			private string _sessionFile;
			private bool _sessionInfoLoaded;
			private Dictionary<string, string> _sessionInfo = new Dictionary<string, string>();
				
			private void LoadSessionInfo()
			{
				_sessionInfoLoaded = true;
				try
				{
					if (!File.Exists(_sessionFile))
					{
						return;
					}
					using (var streamReader = new StreamReader(_sessionFile))
					{
					    do
						{
							var line = streamReader.ReadLine();
							if (line == null)
							{
								break;
							}
                            var parts = line.Split('\\');
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
					Runtime.MessageCollector.AddExceptionMessage("SessionFileReader.LoadSessionInfo() failed.", ex);
				}
			}
				
			public string GetValue(string setting)
			{
				if (!_sessionInfoLoaded)
				{
					LoadSessionInfo();
				}
				return !_sessionInfo.ContainsKey(setting) ? string.Empty : _sessionInfo[setting];
			}
		}
        #endregion
	}
}