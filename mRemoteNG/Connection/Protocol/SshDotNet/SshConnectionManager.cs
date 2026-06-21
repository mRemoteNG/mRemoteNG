using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace mRemoteNG.Connection.Protocol.SshDotNet
{
    public static class SshConnectionManager
    {
        #region Constants

        private const int DEFAULT_TIMEOUT_SECONDS = 15;  // Reduced from 30s for faster timeout
        private const int DEFAULT_KEEPALIVE_SECONDS = 5;  // Reduced from 30s for faster disconnect detection
        private const int DEFAULT_BUFFER_SIZE = 1024;
        private const string DEFAULT_TERMINAL_NAME = "xterm-256color";

        #endregion

        #region Connection Creation

        /// <summary>
        /// Create and connect an SSH client
        /// </summary>
        public static SshClient CreateConnection(
            string hostname,
            int port,
            string username,
            AuthenticationMethod[] authMethods,
            TimeSpan? timeout = null)
        {
            if (string.IsNullOrEmpty(hostname))
                throw new ArgumentException("Hostname cannot be empty", nameof(hostname));

            if (port <= 0 || port > 65535)
                throw new ArgumentOutOfRangeException(nameof(port), "Port must be between 1 and 65535");

            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));

            if (authMethods == null || authMethods.Length == 0)
                throw new ArgumentException("At least one authentication method is required", nameof(authMethods));

            SshDotNetDiagnostics.LogInfo($"Connection: Creating SSH client for {username}@{hostname}:{port}");
            SshDotNetDiagnostics.LogDebug($"Connection: Using {authMethods.Length} authentication method(s)");

            try
            {
                // Create connection info
                var connectionInfo = new Renci.SshNet.ConnectionInfo(
                    hostname,
                    port,
                    username,
                    authMethods);

                // Set timeout
                var actualTimeout = timeout ?? TimeSpan.FromSeconds(DEFAULT_TIMEOUT_SECONDS);
                connectionInfo.Timeout = actualTimeout;

                SshDotNetDiagnostics.LogDebug($"Connection: Timeout set to {actualTimeout.TotalSeconds}s");

                // Create client
                var client = new SshClient(connectionInfo);

                // Configure client properties
                client.ConnectionInfo.Encoding = System.Text.Encoding.UTF8;

                SshDotNetDiagnostics.LogDebug("Connection: SSH client created, ready to connect");

                return client;
            }
            catch (Exception ex)
            {
                SshDotNetDiagnostics.LogException("Connection: Failed to create SSH client", ex);
                throw;
            }
        }

        /// <summary>
        /// Create an SSH client wrapped in an <see cref="ISshClientAdapter"/> (the seam used by the
        /// protocol so its connection pipeline is unit-testable with a fake).
        /// </summary>
        internal static ISshClientAdapter CreateAdapter(
            string hostname,
            int port,
            string username,
            AuthenticationMethod[] authMethods,
            TimeSpan? timeout = null)
        {
            return new SshClientAdapter(CreateConnection(hostname, port, username, authMethods, timeout));
        }

        /// <summary>
        /// Connect the SSH client with logging
        /// </summary>
        public static void Connect(SshClient client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            SshDotNetDiagnostics.StartConnectionTimer();
            SshDotNetDiagnostics.LogInfo($"Connection: Connecting to {client.ConnectionInfo.Host}:{client.ConnectionInfo.Port}");

            try
            {
                client.Connect();

                SshDotNetDiagnostics.StopConnectionTimer($"Connection to {client.ConnectionInfo.Host}");
                SshDotNetDiagnostics.LogInfo($"Connection: Connected successfully");
                SshDotNetDiagnostics.LogInfo($"Connection: Server version: {client.ConnectionInfo.ServerVersion}");
                SshDotNetDiagnostics.LogInfo($"Connection: Current encryption: {client.ConnectionInfo.CurrentServerEncryption}");

                // Log authentication result
                foreach (var authMethod in client.ConnectionInfo.AuthenticationMethods
                    .Where(am => am.AllowedAuthentications != null && am.AllowedAuthentications.Any()))
                {
                    SshDotNetDiagnostics.LogDebug($"Connection: Server allows: {string.Join(", ", authMethod.AllowedAuthentications)}");
                }
            }
            catch (SshAuthenticationException authEx)
            {
                SshDotNetDiagnostics.LogError($"Connection: Authentication failed - {authEx.Message}");
                throw;
            }
            catch (SshConnectionException connEx)
            {
                SshDotNetDiagnostics.LogError($"Connection: Connection failed - {connEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                SshDotNetDiagnostics.LogException("Connection: Unexpected error during connection", ex);
                throw;
            }
        }

        #endregion

        #region Shell Stream Creation

        /// <summary>
        /// Create a shell stream for terminal interaction
        /// </summary>
        public static ShellStream CreateShellStream(
            SshClient client,
            string terminalName = null,
            uint columns = 80,
            uint rows = 24,
            uint width = 0,
            uint height = 0,
            int bufferSize = DEFAULT_BUFFER_SIZE)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            if (!client.IsConnected)
                throw new InvalidOperationException("SSH client must be connected before creating shell stream");

            string actualTerminalName = terminalName ?? DEFAULT_TERMINAL_NAME;

            SshDotNetDiagnostics.LogDebug($"Shell: Creating shell stream with terminal '{actualTerminalName}' ({columns}x{rows})");

            try
            {
                var shellStream = client.CreateShellStream(
                    actualTerminalName,
                    columns,
                    rows,
                    width,
                    height,
                    bufferSize);

                SshDotNetDiagnostics.LogDebug($"Shell: Shell stream created successfully");
                SshDotNetDiagnostics.LogDebug($"Shell: Buffer size: {bufferSize} bytes");

                return shellStream;
            }
            catch (Exception ex)
            {
                SshDotNetDiagnostics.LogException("Shell: Failed to create shell stream", ex);
                throw;
            }
        }

        #endregion

        #region Keep-Alive Configuration

        /// <summary>
        /// Configure keep-alive for the SSH connection
        /// </summary>
        public static void ConfigureKeepAlive(SshClient client, TimeSpan? interval = null)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            var actualInterval = interval ?? TimeSpan.FromSeconds(DEFAULT_KEEPALIVE_SECONDS);

            SshDotNetDiagnostics.LogDebug($"KeepAlive: Configuring keep-alive interval to {actualInterval.TotalSeconds}s");

            try
            {
                client.KeepAliveInterval = actualInterval;
                SshDotNetDiagnostics.LogDebug("KeepAlive: Keep-alive configured successfully");
            }
            catch (Exception ex)
            {
                SshDotNetDiagnostics.LogException("KeepAlive: Failed to configure keep-alive", ex);
                throw;
            }
        }

        #endregion

        #region Connection Info

        /// <summary>
        /// Get detailed connection information for diagnostics
        /// </summary>
        public static string GetConnectionInfo(SshClient client)
        {
            if (client == null)
                return "Client is null";

            if (!client.IsConnected)
                return "Client is not connected";

            var info = client.ConnectionInfo;
            return $"Host: {info.Host}:{info.Port}, " +
                   $"User: {info.Username}, " +
                   $"Server: {info.ServerVersion}, " +
                   $"Encryption: {info.CurrentServerEncryption}, " +
                   $"MAC: {info.CurrentServerHmacAlgorithm}, " +
                   $"Compression: {info.CurrentServerCompressionAlgorithm}";
        }

        #endregion
    }
}
