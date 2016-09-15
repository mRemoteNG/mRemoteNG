using Microsoft.Win32;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;
using System;
using System.Collections.Generic;
using System.Management;
using System.Security.Principal;


namespace mRemoteNG.Config.Putty
{
	public class PuttySessionsRegistryProvider : PuttySessionsProvider
	{
        #region Private Fields
        private const string PuttySessionsKey = "Software\\SimonTatham\\PuTTY\\Sessions";
        private static ManagementEventWatcher _eventWatcher;
        #endregion

        #region Public Methods
		public override string[] GetSessionNames(bool raw = false)
		{
			RegistryKey sessionsKey = Registry.CurrentUser.OpenSubKey(PuttySessionsKey);
			if (sessionsKey == null)
			{
				return new string[] {};
			}
				
			List<string> sessionNames = new List<string>();
			foreach (string sessionName in sessionsKey.GetSubKeyNames())
			{
				if (raw)
				{
					sessionNames.Add(sessionName);
				}
				else
				{
					sessionNames.Add(System.Web.HttpUtility.UrlDecode(sessionName.Replace("+", "%2B")));
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
				
			return sessionNames.ToArray();
		}
			
		public override PuttySessionInfo GetSession(string sessionName)
		{
			RegistryKey sessionsKey = Registry.CurrentUser.OpenSubKey(PuttySessionsKey);
			if (sessionsKey == null)
			{
				return null;
			}
				
			RegistryKey sessionKey = sessionsKey.OpenSubKey(sessionName);
			if (sessionKey == null)
			{
				return null;
			}
				
			sessionName = System.Web.HttpUtility.UrlDecode(sessionName.Replace("+", "%2B"));
				
			PuttySessionInfo sessionInfo = new PuttySessionInfo();
			sessionInfo.PuttySession = sessionName;
			sessionInfo.Name = sessionName;
			sessionInfo.Hostname = Convert.ToString(sessionKey.GetValue("HostName"));
			sessionInfo.Username = Convert.ToString(sessionKey.GetValue("UserName"));
			string protocol = Convert.ToString(sessionKey.GetValue("Protocol"));
			if (protocol == null)
			{
				protocol = "ssh";
			}
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
					object sshVersionObject = sessionKey.GetValue("SshProt");
					if (sshVersionObject != null)
					{
						int sshVersion = Convert.ToInt32(sshVersionObject);
						if (sshVersion >= 2)
						{
							sessionInfo.Protocol = ProtocolType.SSH2;
						}
						else
						{
							sessionInfo.Protocol = ProtocolType.SSH1;
						}
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
			if (_eventWatcher != null)
			{
				return ;
			}
				
			try
			{
				string currentUserSid = WindowsIdentity.GetCurrent().User.Value;
				string key = Convert.ToString(string.Join("\\", new[] {currentUserSid, PuttySessionsKey}).Replace("\\", "\\\\"));
				WqlEventQuery query = new WqlEventQuery(string.Format("SELECT * FROM RegistryTreeChangeEvent WHERE Hive = \'HKEY_USERS\' AND RootPath = \'{0}\'", key));
				_eventWatcher = new ManagementEventWatcher(query);
				_eventWatcher.EventArrived += OnManagementEventArrived;
				_eventWatcher.Start();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("PuttySessions.Watcher.StartWatching() failed.", ex, MessageClass.WarningMsg, true);
			}
		}
			
		public override void StopWatcher()
		{
			if (_eventWatcher == null)
			{
				return ;
			}
			_eventWatcher.Stop();
			_eventWatcher.Dispose();
			_eventWatcher = null;
		}
        #endregion
		
        #region Private Methods
		private void OnManagementEventArrived(object sender, EventArrivedEventArgs e)
		{
			OnSessionChanged(new SessionChangedEventArgs());
		}
        #endregion
	}
}
