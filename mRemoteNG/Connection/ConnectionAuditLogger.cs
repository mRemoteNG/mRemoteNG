using System;
using System.Globalization;
using System.IO;
using System.Runtime.Versioning;
using System.Threading;
using mRemoteNG.App;
using mRemoteNG.App.Info;

namespace mRemoteNG.Connection
{
    [SupportedOSPlatform("windows")]
    public static class ConnectionAuditLogger
    {
        private static readonly Lock _lock = new();
        private const string AuditFileName = "connectionAudit.log";
        private const long MaxFileSize = 10 * 1024 * 1024; // 10 MB

        public static bool Enabled => Properties.OptionsAdvancedPage.Default.EnableConnectionLogging;

        public static void LogConnectionAttempt(ConnectionInfo connectionInfo)
        {
            WriteEntry("CONNECT_ATTEMPT", connectionInfo);
        }

        public static void LogConnectionEstablished(string hostname, string protocol, string user)
        {
            WriteEntry("CONNECTED", hostname, protocol, user, null);
        }

        public static void LogConnectionDisconnected(string hostname, string protocol, string message)
        {
            WriteEntry("DISCONNECTED", hostname, protocol, null, message);
        }

        public static void LogConnectionClosed(string hostname, string protocol, string user)
        {
            WriteEntry("CLOSED", hostname, protocol, user, null);
        }

        public static void LogConnectionError(string hostname, string protocol, string errorMessage, int? errorCode)
        {
            string detail = errorCode.HasValue ? $"{errorMessage} (code: {errorCode})" : errorMessage;
            WriteEntry("ERROR", hostname, protocol, null, detail);
        }

        private static void WriteEntry(string eventType, ConnectionInfo info)
        {
            if (info == null) return;
            WriteEntry(eventType, info.Hostname, info.Protocol.ToString(),
                Environment.UserName, null);
        }

        private static void WriteEntry(string eventType, string hostname, string protocol, string? user, string? detail)
        {
            if (!Enabled) return;

            try
            {
                string filePath = Path.Combine(SettingsFileInfo.SettingsPath, AuditFileName);

                lock (_lock)
                {
                    RotateIfNeeded(filePath);

                    string timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
                    string line = $"{timestamp}\t{eventType}\t{hostname ?? ""}\t{protocol ?? ""}\t{user ?? ""}\t{detail ?? ""}";
                    File.AppendAllText(filePath, line + Environment.NewLine);
                }
            }
            catch
            {
                // Best-effort audit logging - don't crash the app
            }
        }

        private static void RotateIfNeeded(string filePath)
        {
            try
            {
                if (!File.Exists(filePath)) return;
                FileInfo fi = new(filePath);
                if (fi.Length <= MaxFileSize) return;

                string backupPath = filePath + ".1";
                if (File.Exists(backupPath))
                    File.Delete(backupPath);
                File.Move(filePath, backupPath);
            }
            catch
            {
                // Best-effort rotation
            }
        }
    }
}
