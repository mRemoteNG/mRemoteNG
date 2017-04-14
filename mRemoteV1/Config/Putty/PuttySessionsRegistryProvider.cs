using Microsoft.Win32;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;
using System;
using System.Collections.Generic;
using System.Management;
using System.Security.Principal;
using System.Web;


namespace mRemoteNG.Config.Putty
{
	public class PuttySessionsRegistryProvider : AbstractPuttySessionsProvider
	{
        private const string PuttySessionsKey = "Software\\SimonTatham\\PuTTY\\Sessions";
        private static ManagementEventWatcher _eventWatcher;

        #region Public Methods
		public override string[] GetSessionNames(bool raw = false)
		{
            var sessionsKey = Registry.CurrentUser.OpenSubKey(PuttySessionsKey);
			if (sessionsKey == null) return new string[] {};

            var sessionNames = new List<string>();
			foreach (var sessionName in sessionsKey.GetSubKeyNames())
			{
			    sessionNames.Add(raw ? sessionName : HttpUtility.UrlDecode(sessionName.Replace("+", "%2B")));
			}
				
			if (raw && !sessionNames.Contains("Default%20Settings"))
				sessionNames.Insert(0, "Default%20Settings");
			else if (!sessionNames.Contains("Default Settings"))
				sessionNames.Insert(0, "Default Settings");
				
			return sessionNames.ToArray();
		}
			
		public override PuttySessionInfo GetSession(string sessionName)
		{
            var sessionsKey = Registry.CurrentUser.OpenSubKey(PuttySessionsKey);
            var sessionKey = sessionsKey?.OpenSubKey(sessionName);
			if (sessionKey == null)	return null;
				
			sessionName = HttpUtility.UrlDecode(sessionName.Replace("+", "%2B"));

		    var sessionInfo = new PuttySessionInfo
		    {
		        PuttySession = sessionName,
		        Name = sessionName,
		        Hostname = Convert.ToString(sessionKey.GetValue("HostName")),
		        Username = Convert.ToString(sessionKey.GetValue("UserName"))
		    };
            var protocol = Convert.ToString(sessionKey.GetValue("Protocol")) ?? "ssh";
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
                    var sshVersionObject = sessionKey.GetValue("SshProt");
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
			sessionInfo.Port = Convert.ToInt32(sessionKey.GetValue("PortNumber"));
				
			return sessionInfo;
		}
			
		public override void StartWatcher()
		{
			if (_eventWatcher != null) return;
				
			try
			{
                var currentUserSid = WindowsIdentity.GetCurrent().User?.Value;
                var key = string.Join("\\", currentUserSid, PuttySessionsKey).Replace("\\", "\\\\");
                var query = new WqlEventQuery($"SELECT * FROM RegistryTreeChangeEvent WHERE Hive = \'HKEY_USERS\' AND RootPath = \'{key}\'");
				_eventWatcher = new ManagementEventWatcher(query);
				_eventWatcher.EventArrived += OnManagementEventArrived;
				_eventWatcher.Start();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("PuttySessions.Watcher.StartWatching() failed.", ex, MessageClass.WarningMsg);
			}
		}
			
		public override void StopWatcher()
		{
			if (_eventWatcher == null) return;
			_eventWatcher.Stop();
			_eventWatcher.Dispose();
		}
        #endregion
		
		private void OnManagementEventArrived(object sender, EventArrivedEventArgs e)
		{
			RaiseSessionChangedEvent(new PuttySessionChangedEventArgs());
		}
	}
}