using mRemoteNG.App;
using mRemoteNG.Messages;
using System;
using System.Diagnostics;

namespace mRemoteNG.Connection.Protocol.SSH_DotNet
{
    public static class SSHDotNetDiagnostics
    {
        #region Configuration Flags

        /// <summary>
        /// Enable debug-level logging (method entry/exit, state changes, calculations)
        /// Expected volume: 50-100 messages/second during active operation
        /// </summary>
        public static bool VerboseLogging { get; set; } = false;

        /// <summary>
        /// Enable trace-level logging (byte-level operations, every keystroke, every paint)
        /// Expected volume: 1000-10000 messages/second during active operation
        /// WARNING: Extremely verbose, use only for deep debugging
        /// </summary>
        public static bool TraceLogging { get; set; } = false;

        /// <summary>
        /// Enable raw data hex dumps (requires TraceLogging to also be enabled)
        /// </summary>
        public static bool LogRawData { get; set; } = false;

        /// <summary>
        /// Enable escape sequence interpretation logging
        /// </summary>
        public static bool LogEscapeSequences { get; set; } = false;

        /// <summary>
        /// Enable performance metrics logging
        /// </summary>
        public static bool LogPerformanceMetrics { get; set; } = false;

        #endregion

        #region Performance Tracking

        private static Stopwatch _connectionStopwatch = new Stopwatch();

        #endregion

        #region Logging Methods

        /// <summary>
        /// Log a trace message (only if trace logging is enabled via code flag OR user UI settings)
        /// Use for: Byte-level operations, every keystroke, every screen paint, buffer operations
        /// Expected volume: 1000-10000 messages/second during active operation
        /// WARNING: Extremely verbose, use only for deep debugging
        /// </summary>
        public static void LogTrace(string message)
        {
            // Generate trace messages if either:
            // 1. Code-level TraceLogging flag is enabled (developer override), OR
            // 2. User has enabled Trace in Options > Notifications UI
            if (!TraceLogging)
            {
                bool traceEnabledInUI = Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteTraceMsgs
                                     || Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteTraceMsgs
                                     || Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteTraceMsgs;
                if (!traceEnabledInUI) return;
            }

            Runtime.MessageCollector.AddMessage(
                MessageClass.TraceMsg,
                $"[SSH_DotNet TRACE] {message}");
        }

        /// <summary>
        /// Log a debug message (only if verbose logging is enabled)
        /// Use for: Method entry/exit, state changes, property updates, loop lifecycle, internal calculations
        /// Expected volume: 50-100 messages/second during active operation
        /// </summary>
        public static void LogDebug(string message)
        {
            if (!VerboseLogging) return;
            Runtime.MessageCollector.AddMessage(
                MessageClass.DebugMsg,
                $"[SSH_DotNet DEBUG] {message}");
        }

        /// <summary>
        /// Log an informational message (always logged, not gated by flags)
        /// Use for: Connection established/closed, authentication success, major state transitions, user actions
        /// Expected volume: 5-20 messages per connection lifecycle
        /// Guidelines: Only events important enough for users to see in production logs
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
        /// Log raw data in hex format (requires both TraceLogging and LogRawData to be enabled)
        /// Use for: Debugging protocol-level issues, verifying byte sequences
        /// </summary>
        public static void LogRawDataBinary(byte[] data, int length, string context)
        {
            // Require LogRawData flag AND (TraceLogging flag OR UI settings)
            if (!LogRawData) return;

            if (!TraceLogging)
            {
                bool traceEnabledInUI = Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteTraceMsgs
                                     || Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteTraceMsgs
                                     || Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteTraceMsgs;
                if (!traceEnabledInUI) return;
            }

            int displayLength = Math.Min(length, 64);
            string hex = BitConverter.ToString(data, 0, displayLength);
            if (length > 64) hex += "...";

            Runtime.MessageCollector.AddMessage(
                MessageClass.TraceMsg,
                $"[SSH_DotNet TRACE] {context}: [{length} bytes] {hex}");
        }

        /// <summary>
        /// Log escape sequence interpretation (only if LogEscapeSequences flag OR user UI settings enabled)
        /// </summary>
        public static void LogEscapeSequence(string sequence, string interpretation)
        {
            // Generate escape sequence logs if either:
            // 1. Code-level LogEscapeSequences flag is enabled, OR
            // 2. User has enabled Trace in Options > Notifications UI
            if (!LogEscapeSequences)
            {
                bool traceEnabledInUI = Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteTraceMsgs
                                     || Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteTraceMsgs
                                     || Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteTraceMsgs;
                if (!traceEnabledInUI) return;
            }

            Runtime.MessageCollector.AddMessage(
                MessageClass.TraceMsg,
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
