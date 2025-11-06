using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Versioning;
using Renci.SshNet;
using Renci.SshNet.Common;
using mRemoteNG.UI.Controls;

namespace mRemoteNG.Connection.Protocol.SSH_DotNet
{
    [SupportedOSPlatform("windows")]
    public class ProtocolSSH_DotNet : ProtocolBase
    {
        #region State Enumeration

        public enum ConnectionState
        {
            Disconnected,
            Connecting,
            Authenticating,
            Connected,
            Disconnecting,
            Error
        }

        #endregion

        #region Private Fields

        private SshClient _sshClient;
        private ShellStream _shellStream;
        private SshTerminalControl _terminalControl;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _outputReadTask;
        private Task _inputWriteTask;

        private ConnectionState _state = ConnectionState.Disconnected;
        private DateTime _connectionStartTime;
        private long _bytesReceived = 0;
        private long _bytesSent = 0;

        #endregion

        #region Public Properties

        public ConnectionState State
        {
            get => _state;
            private set
            {
                if (_state != value)
                {
                    _state = value;
                    SSHDotNetDiagnostics.LogDebug($"Protocol: State changed to {_state}");
                }
            }
        }

        public bool IsConnected => State == ConnectionState.Connected;

        public long BytesReceived => _bytesReceived;

        public long BytesSent => _bytesSent;

        public TimeSpan ConnectionDuration
        {
            get
            {
                if (IsConnected)
                    return DateTime.Now - _connectionStartTime;
                return TimeSpan.Zero;
            }
        }

        #endregion

        #region Default Port

        public enum Defaults
        {
            Port = 22
        }

        #endregion

        #region Constructor

        public ProtocolSSH_DotNet()
        {
            SSHDotNetDiagnostics.LogDebug("Protocol: ProtocolSSH_DotNet instance created");
        }

        #endregion

        #region Initialization

        public override bool Initialize()
        {
            try
            {
                SSHDotNetDiagnostics.LogInfo("Protocol: Initializing ProtocolSSH_DotNet");

                // Create and initialize terminal control
                _terminalControl = new SshTerminalControl();
                _terminalControl.Initialize();

                Control = _terminalControl;

                // Call base initialization
                bool baseResult = base.Initialize();

                SSHDotNetDiagnostics.LogInfo("Protocol: Initialization complete");
                return baseResult;
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Protocol: Initialization failed", ex);
                return false;
            }
        }

        #endregion

        #region Connection Methods

        public override bool Connect()
        {
            SSHDotNetDiagnostics.LogInfo("Protocol: Connect() called");
            State = ConnectionState.Connecting;
            _connectionStartTime = DateTime.Now;
            SSHDotNetDiagnostics.StartConnectionTimer();

            try
            {
                // Validate connection info
                if (InterfaceControl?.Info == null)
                {
                    SSHDotNetDiagnostics.LogError("Protocol: InterfaceControl.Info is null");
                    State = ConnectionState.Error;
                    Event_ErrorOccured(this, "Connection information is missing", null);
                    return false;
                }

                var connectionInfo = InterfaceControl.Info;
                string hostname = connectionInfo.Hostname;
                int port = connectionInfo.Port != 0 ? connectionInfo.Port : (int)Defaults.Port;
                string username = connectionInfo.Username;
                string password = connectionInfo.Password;

                // Validate required fields
                if (string.IsNullOrEmpty(hostname))
                {
                    SSHDotNetDiagnostics.LogError("Protocol: Hostname is empty");
                    State = ConnectionState.Error;
                    Event_ErrorOccured(this, "Hostname cannot be empty", null);
                    return false;
                }

                if (string.IsNullOrEmpty(username))
                {
                    SSHDotNetDiagnostics.LogError("Protocol: Username is empty");
                    State = ConnectionState.Error;
                    Event_ErrorOccured(this, "Username cannot be empty", null);
                    return false;
                }

                SSHDotNetDiagnostics.LogInfo($"Protocol: Connecting to {username}@{hostname}:{port}");

                // Fire connecting event
                Event_Connecting(this);

                // Build authentication methods
                State = ConnectionState.Authenticating;
                AuthenticationMethod[] authMethods;

                try
                {
                    authMethods = SSHAuthenticationProvider.GetAuthenticationMethods(
                        username, password, connectionInfo);
                }
                catch (Exception authEx)
                {
                    SSHDotNetDiagnostics.LogException("Protocol: Failed to create authentication methods", authEx);
                    State = ConnectionState.Error;
                    Event_ErrorOccured(this, $"Authentication setup failed: {authEx.Message}", null);
                    return false;
                }

                // Create SSH client
                try
                {
                    _sshClient = SSHConnectionManager.CreateConnection(
                        hostname, port, username, authMethods, TimeSpan.FromSeconds(30));
                }
                catch (Exception createEx)
                {
                    SSHDotNetDiagnostics.LogException("Protocol: Failed to create SSH client", createEx);
                    State = ConnectionState.Error;
                    Event_ErrorOccured(this, $"Failed to create SSH client: {createEx.Message}", null);
                    return false;
                }

                // Configure keep-alive
                SSHConnectionManager.ConfigureKeepAlive(_sshClient, TimeSpan.FromSeconds(30));

                // Attach error handler
                _sshClient.ErrorOccurred += OnSshClientError;

                // Connect
                try
                {
                    SSHConnectionManager.Connect(_sshClient);
                }
                catch (SshAuthenticationException authEx)
                {
                    SSHDotNetDiagnostics.LogError($"Protocol: Authentication failed - {authEx.Message}");
                    State = ConnectionState.Error;
                    Event_ErrorOccured(this, $"Authentication failed: {authEx.Message}", null);
                    CleanupConnection();
                    return false;
                }
                catch (SshConnectionException connEx)
                {
                    SSHDotNetDiagnostics.LogError($"Protocol: Connection failed - {connEx.Message}");
                    State = ConnectionState.Error;
                    Event_ErrorOccured(this, $"Connection failed: {connEx.Message}", null);
                    CleanupConnection();
                    return false;
                }
                catch (System.Net.Sockets.SocketException sockEx)
                {
                    SSHDotNetDiagnostics.LogError($"Protocol: Network error - {sockEx.Message}");
                    State = ConnectionState.Error;
                    Event_ErrorOccured(this, $"Network error: {sockEx.Message}", null);
                    CleanupConnection();
                    return false;
                }
                catch (Exception connEx)
                {
                    SSHDotNetDiagnostics.LogException("Protocol: Connection failed", connEx);
                    State = ConnectionState.Error;
                    Event_ErrorOccured(this, $"Connection failed: {connEx.Message}", null);
                    CleanupConnection();
                    return false;
                }

                // Log connection info
                string connInfo = SSHConnectionManager.GetConnectionInfo(_sshClient);
                SSHDotNetDiagnostics.LogInfo($"Protocol: {connInfo}");

                // Create shell stream
                try
                {
                    _shellStream = SSHConnectionManager.CreateShellStream(
                        _sshClient,
                        "xterm-256color",
                        (uint)_terminalControl.Columns,
                        (uint)_terminalControl.Rows,
                        0,
                        0,
                        1024);
                }
                catch (Exception shellEx)
                {
                    SSHDotNetDiagnostics.LogException("Protocol: Failed to create shell stream", shellEx);
                    State = ConnectionState.Error;
                    Event_ErrorOccured(this, $"Failed to create shell: {shellEx.Message}", null);
                    CleanupConnection();
                    return false;
                }

                // Attach terminal to shell stream
                SSHDotNetDiagnostics.LogDebug("Protocol: Attaching terminal to shell stream");
                _terminalControl.AttachSshStream(_shellStream);

                // Start reading output and writing input
                _cancellationTokenSource = new CancellationTokenSource();
                _outputReadTask = Task.Run(() => ReadOutputAsync(_cancellationTokenSource.Token));
                _inputWriteTask = Task.Run(() => WriteInputAsync(_cancellationTokenSource.Token));

                SSHDotNetDiagnostics.LogInfo("Protocol: Output reading and input writing tasks started");

                // Execute opening command if configured
                if (!string.IsNullOrEmpty(connectionInfo.OpeningCommand))
                {
                    SSHDotNetDiagnostics.LogInfo($"Protocol: Executing opening command: {connectionInfo.OpeningCommand}");
                    try
                    {
                        _shellStream.WriteLine(connectionInfo.OpeningCommand);
                    }
                    catch (Exception cmdEx)
                    {
                        SSHDotNetDiagnostics.LogException("Protocol: Failed to execute opening command", cmdEx);
                        // Non-fatal, continue
                    }
                }

                // Success
                State = ConnectionState.Connected;
                SSHDotNetDiagnostics.StopConnectionTimer($"Full connection to {hostname}");
                SSHDotNetDiagnostics.LogInfo("Protocol: Connection established successfully");

                Event_Connected(this);

                return true;
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Protocol: Unexpected error during connection", ex);
                State = ConnectionState.Error;
                Event_ErrorOccured(this, $"Unexpected error: {ex.Message}", null);
                CleanupConnection();
                return false;
            }
        }

        private void OnSshClientError(object sender, Renci.SshNet.Common.ExceptionEventArgs e)
        {
            SSHDotNetDiagnostics.LogException("Protocol: SSH client error event", e.Exception);
            Event_ErrorOccured(this, $"SSH Error: {e.Exception.Message}", null);
        }

        private void CleanupConnection()
        {
            SSHDotNetDiagnostics.LogDebug("Protocol: Cleaning up failed connection");

            try
            {
                if (_sshClient != null)
                {
                    if (_sshClient.IsConnected)
                        _sshClient.Disconnect();
                    _sshClient.Dispose();
                    _sshClient = null;
                }
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Protocol: Error during connection cleanup", ex);
            }
        }

        #endregion

        #region Disconnection Methods

        public override void Disconnect()
        {
            try
            {
                State = ConnectionState.Disconnecting;
                SSHDotNetDiagnostics.LogInfo("Protocol: Disconnect() called");

                // Cancel output reading and input writing
                _cancellationTokenSource?.Cancel();

                // Wait for output task to complete
                if (_outputReadTask != null)
                {
                    try
                    {
                        _outputReadTask.Wait(TimeSpan.FromSeconds(2));
                    }
                    catch (AggregateException)
                    {
                        // Task was cancelled, this is expected
                    }
                }

                // Wait for input task to complete
                if (_inputWriteTask != null)
                {
                    try
                    {
                        _inputWriteTask.Wait(TimeSpan.FromSeconds(2));
                    }
                    catch (AggregateException)
                    {
                        // Task was cancelled, this is expected
                    }
                }

                // Close shell stream
                try
                {
                    _shellStream?.Dispose();
                    _shellStream = null;
                }
                catch (Exception ex)
                {
                    SSHDotNetDiagnostics.LogException("Protocol: Error disposing shell stream", ex);
                }

                // Close SSH client
                try
                {
                    if (_sshClient?.IsConnected ?? false)
                        _sshClient.Disconnect();
                    _sshClient?.Dispose();
                    _sshClient = null;
                }
                catch (Exception ex)
                {
                    SSHDotNetDiagnostics.LogException("Protocol: Error disconnecting SSH client", ex);
                }

                // Detach terminal
                try
                {
                    _terminalControl?.DetachSshStream();
                }
                catch (Exception ex)
                {
                    SSHDotNetDiagnostics.LogException("Protocol: Error detaching terminal", ex);
                }

                State = ConnectionState.Disconnected;
                SSHDotNetDiagnostics.LogInfo("Protocol: Disconnection complete");

                Event_Disconnected(this, "User initiated disconnection", null);
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Protocol: Error during disconnection", ex);
                State = ConnectionState.Error;
                Event_ErrorOccured(this, $"Disconnection error: {ex.Message}", null);
            }
            finally
            {
                base.Disconnect();
            }
        }

        #endregion

        #region Output Reading

        private async Task ReadOutputAsync(CancellationToken cancellationToken)
        {
            SSHDotNetDiagnostics.LogInfo("Output: Starting output reading loop");

            byte[] buffer = new byte[4096];
            int consecutiveEmptyReads = 0;
            const int MAX_EMPTY_READS = 100;

            long totalBytes = 0;
            DateTime lastRateLog = DateTime.Now;
            const int RATE_LOG_INTERVAL_SECONDS = 30;

            try
            {
                while (!cancellationToken.IsCancellationRequested && _shellStream != null)
                {
                    try
                    {
                        // Don't check DataAvailable - it's unreliable on SSH.NET ShellStream
                        // Just call ReadAsync which will block until data arrives or timeout
                        int bytesRead = await _shellStream.ReadAsync(
                            buffer, 0, buffer.Length, cancellationToken);

                        if (bytesRead > 0)
                        {
                            consecutiveEmptyReads = 0;
                            _bytesReceived += bytesRead;
                            totalBytes += bytesRead;

                            // Log raw data if enabled
                            SSHDotNetDiagnostics.LogRawDataBinary(buffer, bytesRead, "Received");

                            SSHDotNetDiagnostics.LogDebug($"Output: Read {bytesRead} bytes (total: {_bytesReceived})");

                            // Convert to string
                            string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                            // Send to terminal control
                            if (_terminalControl != null && !_terminalControl.IsDisposed)
                            {
                                _terminalControl.Invoke((Action)(() =>
                                {
                                    try
                                    {
                                        _terminalControl.WriteOutput(data);
                                    }
                                    catch (Exception writeEx)
                                    {
                                        SSHDotNetDiagnostics.LogException("Output: Error writing to terminal", writeEx);
                                    }
                                }));
                            }

                            // Log data rate periodically
                            var elapsed = DateTime.Now - lastRateLog;
                            if (elapsed.TotalSeconds >= RATE_LOG_INTERVAL_SECONDS)
                            {
                                double rate = totalBytes / elapsed.TotalSeconds;
                                SSHDotNetDiagnostics.LogInfo($"Output: Data rate: {rate:F0} bytes/sec");

                                if (rate > 100000) // > 100 KB/s
                                {
                                    SSHDotNetDiagnostics.LogWarning($"Output: High data rate detected ({rate:F0} bytes/sec), may impact performance");
                                }

                                totalBytes = 0;
                                lastRateLog = DateTime.Now;
                            }
                        }
                        else
                        {
                            // Read returned 0 bytes, connection might be closed
                            consecutiveEmptyReads++;
                            SSHDotNetDiagnostics.LogDebug($"Output: Empty read #{consecutiveEmptyReads}");

                            if (consecutiveEmptyReads >= MAX_EMPTY_READS)
                            {
                                SSHDotNetDiagnostics.LogWarning("Output: Too many empty reads, connection may be closed");
                                break;
                            }

                            await Task.Delay(100, cancellationToken);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        SSHDotNetDiagnostics.LogInfo("Output: Reading cancelled");
                        break;
                    }
                    catch (IOException ioEx)
                    {
                        SSHDotNetDiagnostics.LogException("Output: I/O error reading stream", ioEx);
                        break;
                    }
                    catch (Exception ex)
                    {
                        SSHDotNetDiagnostics.LogException("Output: Error in read loop", ex);
                        // Continue trying to read
                        await Task.Delay(100, cancellationToken);
                    }
                }

                SSHDotNetDiagnostics.LogInfo("Output: Output reading loop ended");

                // Check if we should trigger disconnection event
                if (!cancellationToken.IsCancellationRequested)
                {
                    SSHDotNetDiagnostics.LogInfo("Output: Connection appears to be closed by remote");
                    Event_Disconnected(this, "Connection closed by remote host", null);
                }
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Output: Fatal error in output reading", ex);
                Event_ErrorOccured(this, $"Output reading error: {ex.Message}", null);
            }
        }

        private async Task WriteInputAsync(CancellationToken cancellationToken)
        {
            SSHDotNetDiagnostics.LogInfo("Input: Starting input writing loop");

            try
            {
                while (!cancellationToken.IsCancellationRequested && _shellStream != null && _terminalControl != null)
                {
                    try
                    {
                        // Poll terminal control for user input
                        _terminalControl.ReadInput(out byte[] inputData);

                        if (inputData != null && inputData.Length > 0)
                        {
                            // Write input to SSH shell stream
                            await _shellStream.WriteAsync(inputData, 0, inputData.Length, cancellationToken);
                            await _shellStream.FlushAsync(cancellationToken);

                            _bytesSent += inputData.Length;

                            // Log raw data if enabled
                            SSHDotNetDiagnostics.LogRawDataBinary(inputData, inputData.Length, "Sent");
                            SSHDotNetDiagnostics.LogDebug($"Input: Sent {inputData.Length} bytes (total: {_bytesSent})");
                        }
                        else
                        {
                            // No input available, wait a bit before polling again
                            await Task.Delay(50, cancellationToken);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        SSHDotNetDiagnostics.LogInfo("Input: Writing cancelled");
                        break;
                    }
                    catch (IOException ioEx)
                    {
                        SSHDotNetDiagnostics.LogException("Input: I/O error writing to stream", ioEx);
                        break;
                    }
                    catch (Exception ex)
                    {
                        SSHDotNetDiagnostics.LogException("Input: Error in writing loop", ex);
                        await Task.Delay(100, cancellationToken);
                    }
                }

                SSHDotNetDiagnostics.LogInfo("Input: Input writing loop ended");
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Input: Fatal error in input writing", ex);
            }
        }

        #endregion
    }
}
