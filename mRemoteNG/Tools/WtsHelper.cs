using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools
{
    /// <summary>
    /// Wraps the Windows Terminal Services (WTS) API for querying and managing
    /// RDP sessions on local or remote machines.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class WtsHelper
    {
        private static readonly IntPtr WTS_CURRENT_SERVER = IntPtr.Zero;

        #region P/Invoke declarations

        [DllImport("wtsapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr WTSOpenServer(string pServerName);

        [DllImport("wtsapi32.dll")]
        private static extern void WTSCloseServer(IntPtr hServer);

        [DllImport("wtsapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool WTSEnumerateSessions(
            IntPtr hServer,
            int reserved,
            int version,
            ref IntPtr ppSessionInfo,
            ref int pCount);

        [DllImport("wtsapi32.dll")]
        private static extern void WTSFreeMemory(IntPtr pMemory);

        [DllImport("wtsapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool WTSQuerySessionInformation(
            IntPtr hServer,
            int sessionId,
            WtsInfoClass wtsInfoClass,
            out IntPtr ppBuffer,
            out uint pBytesReturned);

        [DllImport("wtsapi32.dll", SetLastError = true)]
        private static extern bool WTSDisconnectSession(IntPtr hServer, int sessionId, bool bWait);

        [StructLayout(LayoutKind.Sequential)]
        private struct WtsSessionInfo
        {
            public int SessionId;
            // Keep as IntPtr to avoid CLR freeing strings individually;
            // the entire block is freed with a single WTSFreeMemory call.
            public IntPtr pWinStationName;
            public WtsConnectStateClass State;
        }

        private enum WtsConnectStateClass
        {
            WTSActive,
            WTSConnected,
            WTSConnectQuery,
            WTSShadow,
            WTSDisconnected,
            WTSIdle,
            WTSListen,
            WTSReset,
            WTSDown,
            WTSInit
        }

        private enum WtsInfoClass
        {
            WTSInitialProgram = 0,
            WTSApplicationName = 1,
            WTSWorkingDirectory = 2,
            WTSOEMId = 3,
            WTSSessionId = 4,
            WTSUserName = 5,
            WTSWinStationName = 6,
            WTSDomainName = 7,
            WTSConnectState = 8,
            WTSClientBuildNumber = 9,
            WTSClientName = 10,
            WTSClientDirectory = 11,
            WTSClientProductId = 12,
            WTSClientHardwareId = 13,
            WTSClientAddress = 14,
            WTSClientDisplay = 15,
            WTSClientProtocolType = 16,
        }

        #endregion

        /// <summary>
        /// Enumerates all RDP sessions on the specified server.
        /// </summary>
        /// <param name="serverName">Hostname or IP. Use <c>null</c> or <c>"."</c> for the local machine.</param>
        /// <returns>List of session entries.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the WTS API call fails.</exception>
        public static IList<RdpSessionEntry> EnumerateSessions(string serverName)
        {
            bool isLocal = IsLocalMachine(serverName);
            IntPtr hServer = IntPtr.Zero;

            try
            {
                hServer = isLocal ? WTS_CURRENT_SERVER : WTSOpenServer(serverName);
                if (!isLocal && hServer == IntPtr.Zero)
                    throw new InvalidOperationException(
                        $"Cannot connect to '{serverName}'. Error code: {Marshal.GetLastWin32Error()}");

                IntPtr sessionInfoPtr = IntPtr.Zero;
                int sessionCount = 0;

                if (!WTSEnumerateSessions(hServer, 0, 1, ref sessionInfoPtr, ref sessionCount))
                    throw new InvalidOperationException(
                        $"WTSEnumerateSessions failed. Error code: {Marshal.GetLastWin32Error()}");

                try
                {
                    return ParseSessionInfo(hServer, sessionInfoPtr, sessionCount);
                }
                finally
                {
                    WTSFreeMemory(sessionInfoPtr);
                }
            }
            finally
            {
                if (!isLocal && hServer != IntPtr.Zero)
                    WTSCloseServer(hServer);
            }
        }

        /// <summary>
        /// Disconnects the specified session on the server.
        /// </summary>
        /// <param name="serverName">Hostname or IP. Use <c>null</c> or <c>"."</c> for the local machine.</param>
        /// <param name="sessionId">The session ID to disconnect.</param>
        /// <exception cref="InvalidOperationException">Thrown when the WTS API call fails.</exception>
        public static void DisconnectSession(string serverName, int sessionId)
        {
            bool isLocal = IsLocalMachine(serverName);
            IntPtr hServer = IntPtr.Zero;

            try
            {
                hServer = isLocal ? WTS_CURRENT_SERVER : WTSOpenServer(serverName);
                if (!isLocal && hServer == IntPtr.Zero)
                    throw new InvalidOperationException(
                        $"Cannot connect to '{serverName}'. Error code: {Marshal.GetLastWin32Error()}");

                if (!WTSDisconnectSession(hServer, sessionId, false))
                    throw new InvalidOperationException(
                        $"WTSDisconnectSession failed. Error code: {Marshal.GetLastWin32Error()}");
            }
            finally
            {
                if (!isLocal && hServer != IntPtr.Zero)
                    WTSCloseServer(hServer);
            }
        }

        private static bool IsLocalMachine(string? serverName)
        {
            return string.IsNullOrEmpty(serverName)
                || string.Equals(serverName, ".", StringComparison.Ordinal)
                || serverName.Equals(Environment.MachineName, StringComparison.OrdinalIgnoreCase);
        }

        private static List<RdpSessionEntry> ParseSessionInfo(
            IntPtr hServer, IntPtr sessionInfoPtr, int sessionCount)
        {
            var sessions = new List<RdpSessionEntry>(sessionCount);
            int structSize = Marshal.SizeOf<WtsSessionInfo>();

            for (int i = 0; i < sessionCount; i++)
            {
                IntPtr current = IntPtr.Add(sessionInfoPtr, i * structSize);
                var info = Marshal.PtrToStructure<WtsSessionInfo>(current);

                string stationName = Marshal.PtrToStringUni(info.pWinStationName) ?? string.Empty;
                string userName = QuerySessionString(hServer, info.SessionId, WtsInfoClass.WTSUserName);
                string clientName = QuerySessionString(hServer, info.SessionId, WtsInfoClass.WTSClientName);
                string domainName = QuerySessionString(hServer, info.SessionId, WtsInfoClass.WTSDomainName);

                sessions.Add(new RdpSessionEntry
                {
                    SessionId = info.SessionId,
                    SessionName = stationName,
                    UserName = string.IsNullOrEmpty(domainName) ? userName : $"{domainName}\\{userName}",
                    ClientName = clientName,
                    State = info.State.ToString(),
                    StateText = GetStateText(info.State)
                });
            }

            return sessions;
        }

        private static string QuerySessionString(IntPtr hServer, int sessionId, WtsInfoClass infoClass)
        {
            if (!WTSQuerySessionInformation(hServer, sessionId, infoClass, out IntPtr buffer, out _))
                return string.Empty;

            try
            {
                return Marshal.PtrToStringUni(buffer) ?? string.Empty;
            }
            finally
            {
                WTSFreeMemory(buffer);
            }
        }

        private static string GetStateText(WtsConnectStateClass state) => state switch
        {
            WtsConnectStateClass.WTSActive => "Active",
            WtsConnectStateClass.WTSConnected => "Connected",
            WtsConnectStateClass.WTSConnectQuery => "Connect Query",
            WtsConnectStateClass.WTSShadow => "Shadow",
            WtsConnectStateClass.WTSDisconnected => "Disconnected",
            WtsConnectStateClass.WTSIdle => "Idle",
            WtsConnectStateClass.WTSListen => "Listen",
            WtsConnectStateClass.WTSReset => "Reset",
            WtsConnectStateClass.WTSDown => "Down",
            WtsConnectStateClass.WTSInit => "Init",
            _ => state.ToString()
        };
    }

    /// <summary>
    /// Represents a single RDP session entry returned by the WTS API.
    /// </summary>
    public class RdpSessionEntry
    {
        public int SessionId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string SessionName { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        /// <summary>Raw state enum name (e.g. "WTSActive").</summary>
        public string State { get; set; } = string.Empty;
        /// <summary>Human-readable state text (e.g. "Active").</summary>
        public string StateText { get; set; } = string.Empty;
    }
}
