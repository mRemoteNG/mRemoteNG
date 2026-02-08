using System;
using System.Collections.Generic;
using System.Management;
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
        private string CurrentUserSid { get; } = WindowsIdentity.GetCurrent().User?.Value;
        private static ManagementEventWatcher _eventWatcher;

        #region Public Methods

        public override string[] GetSessionNames(bool raw = false)
        {
            RegistryKey sessionsKey = Registry.CurrentUser.OpenSubKey(PuttySessionsKey);
            if (sessionsKey == null) return Array.Empty<string>();

            List<string> sessionNames = new();
            foreach (string sessionName in sessionsKey.GetSubKeyNames())
            {
                sessionNames.Add(raw ? sessionName : PuttySessionNameDecoder.Decode(sessionName));
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

            RegistryKey sessionsKey = Registry.CurrentUser.OpenSubKey(PuttySessionsKey);
            RegistryKey sessionKey = sessionsKey?.OpenSubKey(sessionName);
            if (sessionKey == null) return null;

            sessionName = PuttySessionNameDecoder.Decode(sessionName);

            PuttySessionInfo sessionInfo = new()
            {
                PuttySession = sessionName,
                Name = sessionName,
                Hostname = sessionKey.GetValue("HostName")?.ToString() ?? "",
                Username = sessionKey.GetValue("UserName")?.ToString() ?? ""
            };


            string protocol = string.IsNullOrEmpty(sessionKey.GetValue("Protocol")?.ToString())
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
                    int.TryParse(sessionKey.GetValue("SshProt")?.ToString(), out int sshVersion);
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

            int.TryParse(sessionKey.GetValue("PortNumber")?.ToString(), out int portNumber);
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
                string keyName = string.Join("\\", CurrentUserSid, PuttySessionsKey).Replace("\\", "\\\\");
                RegistryKey sessionsKey = Registry.Users.OpenSubKey(keyName);
                if (sessionsKey == null)
                {
                    Registry.Users.CreateSubKey(keyName);
                }
                WqlEventQuery query = new($"SELECT * FROM RegistryTreeChangeEvent WHERE Hive = \'HKEY_USERS\' AND RootPath = \'{keyName}\'");
                _eventWatcher = new ManagementEventWatcher(query);
                _eventWatcher.EventArrived += OnManagementEventArrived;
                _eventWatcher.Start();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("PuttySessions.Watcher.StartWatching() failed.", ex, MessageClass.WarningMsg);
                _eventWatcher?.Stop();
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
