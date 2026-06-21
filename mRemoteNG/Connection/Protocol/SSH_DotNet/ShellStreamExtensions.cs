using System;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace mRemoteNG.Connection.Protocol.SSH_DotNet
{
    /// <summary>
    /// Extension methods for Renci.SshNet.ShellStream to support dynamic terminal resizing.
    /// </summary>
    public static class ShellStreamExtensions
    {
        /// <summary>
        /// Sends a window change request to the SSH server to update the terminal size.
        /// </summary>
        /// <param name="stream">The ShellStream to resize</param>
        /// <param name="columns">New terminal width in columns</param>
        /// <param name="rows">New terminal height in rows</param>
        /// <param name="width">New terminal width in pixels (0 = auto)</param>
        /// <param name="height">New terminal height in pixels (0 = auto)</param>
        /// <remarks>
        /// Thin, intent-revealing wrapper over <see cref="ShellStream.ChangeWindowSize"/> (a public
        /// API since SSH.NET 2024+). Resize failures are logged but non-fatal: a resize racing with a
        /// disconnect must never crash the terminal. (Previously this used reflection to reach an
        /// internal channel method, which SonarCloud flagged as an accessibility-bypass hotspot.)
        /// </remarks>
        public static void SendWindowChangeRequest(this ShellStream stream,
            uint columns, uint rows, uint width, uint height)
        {
            if (stream == null)
                return;

            try
            {
                stream.ChangeWindowSize(columns, rows, width, height);
                SSHDotNetDiagnostics.LogDebug($"ShellStreamExtensions: Sent window change request: {columns}x{rows}");
            }
            catch (ObjectDisposedException)
            {
                // Stream already closed (resize raced a disconnect) — nothing to resize.
            }
            catch (SshException ex)
            {
                SSHDotNetDiagnostics.LogException("ShellStreamExtensions: Error sending window change request", ex);
            }
        }
    }
}
