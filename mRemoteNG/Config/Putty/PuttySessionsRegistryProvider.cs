using System;
using System.Collections.Generic;
using System.Management;
using System.Net;
using System.Runtime.Versioning;
using System.Security.Principal;
using Microsoft.Win32;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;


namespace mRemoteNG.Config.Putty
{
    [SupportedOSPlatform("windows")]
    public class PuttySessionsRegistryProvider : AbstractPuttySessionsProvider
    {
        private const string PuttySessionsKey = "Software\\SimonTatham\\PuTTY\\Sessions";
        private static ManagementEventWatcher _eventWatcher;

        #region Public Methods

        public override string[] GetSessionNames(bool raw = false)
        {
            var sessionsKey = Registry.CurrentUser.OpenSubKey(PuttySessionsKey);
            if (sessionsKey == null) return new string[] { };

            var sessionNames = new List<string>();
            foreach (var sessionName in sessionsKey.GetSubKeyNames())
            {
                sessionNames.Add(raw ? sessionName
                                     : WebUtility.UrlDecode(sessionName.Replace("+", "%2B")));
            }

            if (raw && !sessionNames.Contains("Default%20Settings"))
                sessionNames.Insert(0, "Default%20Settings");
            else if (!raw && !sessionNames.Contains("Default Settings"))
                sessionNames.Insert(0, "Default Settings");

            return sessionNames.ToArray();
        }

        public override PuttySessionInfo GetSession(string sessionName)
        {
            if (string.IsNullOrEmpty(sessionName))
                return null;

            var sessionsKey = Registry.CurrentUser.OpenSubKey(PuttySessionsKey);
            var sessionKey = sessionsKey?.OpenSubKey(sessionName);
            if (sessionKey == null) return null;

            sessionName = WebUtility.UrlDecode(sessionName.Replace("+", "%2B"));

            var sessionInfo = new PuttySessionInfo
            {
                PuttySession = sessionName,
                Name = sessionName,
                Hostname = sessionKey.GetValue("HostName")?.ToString() ?? "",
                Username = sessionKey.GetValue("UserName")?.ToString() ?? ""
            };


            var protocol = string.IsNullOrEmpty(sessionKey.GetValue("Protocol")?.ToString())
                ? "ssh"
                : sessionKey.GetValue("Protocol").ToString();

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
                    int.TryParse(sessionKey.GetValue("SshProt")?.ToString(), out var sshVersion);
                    /* Per PUTTY.H in PuTTYNG & PuTTYNG Upstream (PuTTY proper currently)
                     * expect 0 for SSH1, 3 for SSH2 ONLY
                     * 1 for SSH1 with a 2 fallback
                     * 2 for SSH2 with a 1 fallback
                     *
                     * default to SSH2 if any other value is received
                     */
                    sessionInfo.Protocol = sshVersion == 1 || sshVersion == 0 ? ProtocolType.SSH1 : ProtocolType.SSH2;
                    break;
                case "telnet":
                    sessionInfo.Protocol = ProtocolType.Telnet;
                    break;
                default:
                    return null;
            }

            int.TryParse(sessionKey.GetValue("PortNumber")?.ToString(), out var portNumber);
            if (portNumber == default(int))
                sessionInfo.SetDefaultPort();
            else
                sessionInfo.Port = portNumber;

            return sessionInfo;
        }

        public override void StartWatcher()
        {
            if (_eventWatcher != null) return;

            try
            {
                var currentUserSid = WindowsIdentity.GetCurrent().User?.Value;
                var key = string.Join("\\", currentUserSid, PuttySessionsKey).Replace("\\", "\\\\");
                var query = new WqlEventQuery(
                                              $"SELECT * FROM RegistryTreeChangeEvent WHERE Hive = \'HKEY_USERS\' AND RootPath = \'{key}\'");
                _eventWatcher = new ManagementEventWatcher(query);
                _eventWatcher.EventArrived += OnManagementEventArrived;
                _eventWatcher.Start();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("PuttySessions.Watcher.StartWatching() failed.", ex,
                                                             MessageClass.WarningMsg);
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