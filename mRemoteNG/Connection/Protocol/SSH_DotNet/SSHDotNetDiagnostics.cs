using mRemoteNG.App;
using mRemoteNG.Messages;
using System;
using System.Diagnostics;

namespace mRemoteNG.Connection.Protocol.SSH_DotNet
{
    public static class SSHDotNetDiagnostics
    {
        #region Configuration Flags

        public static bool VerboseLogging { get; set; } = false;
        public static bool LogRawData { get; set; } = false;
        public static bool LogEscapeSequences { get; set; } = false;
        public static bool LogPerformanceMetrics { get; set; } = false;

        #endregion

        #region Performance Tracking

        private static Stopwatch _connectionStopwatch = new Stopwatch();

        #endregion

        #region Logging Methods

        /// <summary>
        /// Log a debug message (only if verbose logging is enabled)
        /// </summary>
        public static void LogDebug(string message)
        {
            if (!VerboseLogging) return;
            Runtime.MessageCollector.AddMessage(
                MessageClass.DebugMsg,
                $"[SSH_DotNet DEBUG] {message}");
        }

        /// <summary>
        /// Log an informational message
        /// </summary>
        public static void LogInfo(string message)
        {
            Runtime.MessageCollector.AddMessage(
                MessageClass.InformationMsg,
                $"[SSH_DotNet] {message}");
        }

        /// <summary>
        /// Log a warning message
        /// </summary>
        public static void LogWarning(string message)
        {
            Runtime.MessageCollector.AddMessage(
                MessageClass.WarningMsg,
                $"[SSH_DotNet WARNING] {message}");
        }

        /// <summary>
        /// Log an error message
        /// </summary>
        public static void LogError(string message)
        {
            Runtime.MessageCollector.AddMessage(
                MessageClass.ErrorMsg,
                $"[SSH_DotNet ERROR] {message}");
        }

        /// <summary>
        /// Log an exception with full stack trace
        /// </summary>
        public static void LogException(string context, Exception ex)
        {
            Runtime.MessageCollector.AddExceptionStackTrace(
                $"[SSH_DotNet] {context}",
                ex,
                MessageClass.ErrorMsg);
        }

        #endregion

        #region Raw Data Logging

        /// <summary>
        /// Log raw data in hex format (only if raw data logging is enabled)
        /// </summary>
        public static void LogRawDataBinary(byte[] data, int length, string context)
        {
            if (!LogRawData) return;

            int displayLength = Math.Min(length, 64);
            string hex = BitConverter.ToString(data, 0, displayLength);
            if (length > 64) hex += "...";

            Runtime.MessageCollector.AddMessage(
                MessageClass.DebugMsg,
                $"[SSH_DotNet RAW] {context}: [{length} bytes] {hex}");
        }

        /// <summary>
        /// Log escape sequence interpretation
        /// </summary>
        public static void LogEscapeSequence(string sequence, string interpretation)
        {
            if (!LogEscapeSequences) return;

            Runtime.MessageCollector.AddMessage(
                MessageClass.DebugMsg,
                $"[SSH_DotNet ESC] {sequence} → {interpretation}");
        }

        #endregion

        #region Performance Tracking

        /// <summary>
        /// Start the connection timer for performance measurement
        /// </summary>
        public static void StartConnectionTimer()
        {
            _connectionStopwatch.Restart();
            if (LogPerformanceMetrics)
                LogDebug("Connection timer started");
        }

        /// <summary>
        /// Stop the connection timer and log elapsed time
        /// </summary>
        public static void StopConnectionTimer(string operation)
        {
            _connectionStopwatch.Stop();
            if (LogPerformanceMetrics)
            {
                LogInfo($"Performance: {operation} completed in {_connectionStopwatch.ElapsedMilliseconds}ms");
            }
        }

        #endregion

        #region Security-Aware Logging

        /// <summary>
        /// Log authentication attempt (never logs credentials)
        /// </summary>
        public static void LogAuthAttempt(string username, string authMethodType)
        {
            // NEVER log password or key content
            LogInfo($"Auth: Attempting {authMethodType} authentication for user '{username}'");
        }

        /// <summary>
        /// Log successful authentication
        /// </summary>
        public static void LogAuthSuccess(string username, string authMethodType)
        {
            LogInfo($"Auth: {authMethodType} authentication succeeded for user '{username}'");
        }

        /// <summary>
        /// Log failed authentication
        /// </summary>
        public static void LogAuthFailure(string username, string authMethodType, string reason)
        {
            LogWarning($"Auth: {authMethodType} authentication failed for user '{username}': {reason}");
        }

        #endregion
    }
}
