using System;
using System.Reflection;
using Renci.SshNet;

namespace mRemoteNG.Connection.Protocol.SSH_DotNet
{
    /// <summary>
    /// Extension methods for Renci.SshNet.ShellStream to support dynamic terminal resizing
    /// </summary>
    public static class ShellStreamExtensions
    {
        /// <summary>
        /// Sends a window change request to the SSH server to update the terminal size.
        /// Uses reflection to access the internal _channel field and invoke SendWindowChangeRequest.
        /// </summary>
        /// <param name="stream">The ShellStream to resize</param>
        /// <param name="columns">New terminal width in columns</param>
        /// <param name="rows">New terminal height in rows</param>
        /// <param name="width">New terminal width in pixels (0 = auto)</param>
        /// <param name="height">New terminal height in pixels (0 = auto)</param>
        /// <remarks>
        /// This is a workaround because Renci.SshNet.ShellStream doesn't expose a public method
        /// for changing terminal dimensions after creation. The method uses reflection to access
        /// the private _channel field (ChannelSession) and calls its SendWindowChangeRequest method.
        /// </remarks>
        public static void SendWindowChangeRequest(this ShellStream stream,
            uint columns, uint rows, uint width, uint height)
        {
            try
            {
                // Access private _channel field via reflection
                var channelField = stream.GetType().GetField("_channel",
                    BindingFlags.NonPublic | BindingFlags.Instance);

                if (channelField == null)
                {
                    SSHDotNetDiagnostics.LogWarning("ShellStreamExtensions: Could not find _channel field via reflection");
                    return;
                }

                var channel = channelField.GetValue(stream);
                if (channel == null)
                {
                    SSHDotNetDiagnostics.LogWarning("ShellStreamExtensions: _channel field is null");
                    return;
                }

                // Retrieve SendWindowChangeRequest method from _channel
                var method = channel.GetType().GetMethod("SendWindowChangeRequest",
                    BindingFlags.Public | BindingFlags.Instance);

                if (method == null)
                {
                    SSHDotNetDiagnostics.LogWarning("ShellStreamExtensions: Could not find SendWindowChangeRequest method via reflection");
                    return;
                }

                // Invoke method to update terminal dimensions
                method.Invoke(channel, new object[] { columns, rows, width, height });

                SSHDotNetDiagnostics.LogDebug($"ShellStreamExtensions: Successfully sent window change request: {columns}x{rows}");
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("ShellStreamExtensions: Error sending window change request", ex);
            }
        }
    }
}
