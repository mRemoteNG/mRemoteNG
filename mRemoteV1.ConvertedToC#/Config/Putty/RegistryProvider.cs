using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Management;
using mRemoteNG.App;
using mRemoteNG.Messages;
using Microsoft.Win32;
using mRemoteNG.Connection.Protocol;
using System.Security.Principal;

namespace mRemoteNG.Config.Putty
{
	public class RegistryProvider : Provider
	{

		#region "Public Methods"
		public override string[] GetSessionNames(bool raw = false)
		{
			RegistryKey sessionsKey = Registry.CurrentUser.OpenSubKey(PuttySessionsKey);
			if (sessionsKey == null)
				return new string[];

			List<string> sessionNames = new List<string>();
			foreach (string sessionName in sessionsKey.GetSubKeyNames()) {
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

			return sessionNames.ToArray();
		}

		public override Connection.PuttySession.Info GetSession(string sessionName)
		{
			RegistryKey sessionsKey = Registry.CurrentUser.OpenSubKey(PuttySessionsKey);
			if (sessionsKey == null)
				return null;

			RegistryKey sessionKey = sessionsKey.OpenSubKey(sessionName);
			if (sessionKey == null)
				return null;

			sessionName = System.Web.HttpUtility.UrlDecode(sessionName.Replace("+", "%2B"));

			Connection.PuttySession.Info sessionInfo = new Connection.PuttySession.Info();
			var _with1 = sessionInfo;
			_with1.PuttySession = sessionName;
			_with1.Name = sessionName;
			_with1.Hostname = sessionKey.GetValue("HostName");
			_with1.Username = sessionKey.GetValue("UserName");
			string protocol = sessionKey.GetValue("Protocol");
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
					object sshVersionObject = sessionKey.GetValue("SshProt");
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
			_with1.Port = sessionKey.GetValue("PortNumber");

			return sessionInfo;
		}

		public override void StartWatcher()
		{
			if (_eventWatcher != null)
				return;

			try {
				string currentUserSid = WindowsIdentity.GetCurrent().User.Value;
				string key = string.Join("\\", {
					currentUserSid,
					PuttySessionsKey
				}).Replace("\\", "\\\\");
				WqlEventQuery query = new WqlEventQuery(string.Format("SELECT * FROM RegistryTreeChangeEvent WHERE Hive = 'HKEY_USERS' AND RootPath = '{0}'", key));
				_eventWatcher = new ManagementEventWatcher(query);
				_eventWatcher.EventArrived += OnManagementEventArrived;
				_eventWatcher.Start();
			} catch (Exception ex) {
				Runtime.MessageCollector.AddExceptionMessage("PuttySessions.Watcher.StartWatching() failed.", ex, MessageClass.WarningMsg, true);
			}
		}

		public override void StopWatcher()
		{
			if (_eventWatcher == null)
				return;
			_eventWatcher.Stop();
			_eventWatcher.Dispose();
			_eventWatcher = null;
		}
		#endregion

		#region "Private Fields"
		private const string PuttySessionsKey = "Software\\SimonTatham\\PuTTY\\Sessions";
			#endregion
		private static ManagementEventWatcher _eventWatcher;

		#region "Private Methods"
		private void OnManagementEventArrived(object sender, EventArrivedEventArgs e)
		{
			OnSessionChanged(new SessionChangedEventArgs());
		}
		#endregion
	}
}

