// Design Note: Generic catch clauses (catch Exception) are used intentionally throughout this file.
// This is protocol/infrastructure code that must remain resilient — an unhandled exception here would
// crash the user's connection or the entire application. All caught exceptions are logged via
// SshDotNetDiagnostics, so no diagnostic information is lost. Splitting into multiple specific
// exception types would add verbosity without changing behavior, since all branches log and
// perform the same recovery action (cleanup, error state, return false, etc.).
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Versioning;
using Renci.SshNet;
using Renci.SshNet.Common;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.TaskDialog;
using mRemoteNG.UI.Tabs;

namespace mRemoteNG.Connection.Protocol.SshDotNet
{
    [SupportedOSPlatform("windows")]
    public class ProtocolSshDotNet : ProtocolBase
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

        private ISshClientAdapter _sshClient;
        private ShellStream _shellStream;
        private SshTerminalControl _terminalControl;
        private SshTunnelManager _tunnelManager;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _outputReadTask;
        private Task _inputWriteTask;

        private ConnectionState _state = ConnectionState.Disconnected;
        private readonly Stopwatch _connectionTimer = new Stopwatch();
        private long _bytesReceived = 0;
        private long _bytesSent = 0;
        private double _peakReceiveRate = 0;  // bytes/sec
        private readonly double _peakSendRate = 0;     // bytes/sec

        // Error tracking for smart disconnect detection
        private bool _hadRecentError = false;
        private DateTime _lastErrorTime;        // wall-clock, for display only
        private long _lastErrorTicks;           // monotonic, for elapsed checks
        private string _lastErrorMessage = "";
        private Exception _lastException = null;
        private readonly CancellationTokenSource _errorCancellationSource = new CancellationTokenSource();

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
                    SshDotNetDiagnostics.LogDebug($"Protocol: State changed to {_state}");
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
                    return _connectionTimer.Elapsed;
                return TimeSpan.Zero;
            }
        }

        /// <summary>
        /// When true, Initialize() skips terminal control creation, and Connect()
        /// establishes the SSH connection and tunnel manager but skips shell stream
        /// creation, terminal attachment, and I/O tasks.
        /// Set this before calling Initialize() when using this protocol as a tunnel provider.
        /// </summary>
        public bool TunnelOnlyMode { get; set; }

        /// <summary>
        /// Provides access to the tunnel manager for setting up port forwarding.
        /// Available after Connect() succeeds.
        /// </summary>
        public SshTunnelManager TunnelManager => _tunnelManager;

        /// <summary>
        /// Whether this SSH connection is still alive and usable as a tunnel.
        /// </summary>
        public bool IsTunnelHealthy =>
            _sshClient?.IsConnected == true &&
            (_tunnelManager?.AreAllPortsHealthy() ?? true);

        #endregion

        #region Default Port

        public enum Defaults
        {
            Port = 22
        }

        #endregion

        #region Constructor

        public ProtocolSshDotNet()
        {
            SshDotNetDiagnostics.LogDebug("Protocol: ProtocolSshDotNet instance created");
        }

        #endregion

        #region Initialization

        public override bool Initialize()
        {
            try
            {
                SshDotNetDiagnostics.LogDebug("Protocol: Initializing ProtocolSshDotNet");

                if (!TunnelOnlyMode)
                {
                    // Create and initialize terminal control (not needed for tunnel-only)
                    _terminalControl = new SshTerminalControl();
                    _terminalControl.Initialize();

                    // Subscribe to terminal resize events to notify SSH server
                    _terminalControl.TerminalResized += OnTerminalResized;

                    Control = _terminalControl;
                }

                // Call base initialization
                // Note: base.Initialize() handles Control == null gracefully
                bool baseResult = base.Initialize();

                SshDotNetDiagnostics.LogDebug($"Protocol: Initialization complete (TunnelOnlyMode={TunnelOnlyMode})");
                return baseResult;
            }
            catch (Exception ex)
            {
                SshDotNetDiagnostics.LogException("Protocol: Initialization failed", ex);
                return false;
            }
        }

        #endregion

        #region Connection Methods

        public override bool Connect()
        {
            SshDotNetDiagnostics.LogDebug("Protocol: Connect() called");
            State = ConnectionState.Connecting;
            _connectionTimer.Restart();
            SshDotNetDiagnostics.StartConnectionTimer();

            try
            {
                if (!TryResolveParameters(out string hostname, out int port, out string username, out string password))
                    return false;

                SshDotNetDiagnostics.LogInfo($"Protocol: Connecting to {username}@{hostname}:{port}");
                Event_Connecting(this);

                State = ConnectionState.Authenticating;
                if (!TryBuildAuthentication(username, password, InterfaceControl.Info, out AuthenticationMethod[] authMethods))
                    return false;

                if (!TryEstablishSshConnection(hostname, port, username, authMethods))
                    return false;

                ConfigureTunnels(InterfaceControl.Info);

                if (TunnelOnlyMode)
                {
                    State = ConnectionState.Connected;
                    SshDotNetDiagnostics.LogInfo("Protocol: Connected in tunnel-only mode (no shell)");
                    Event_Connected(this);
                    return true;
                }

                return TryStartTerminalSession(InterfaceControl.Info, hostname);
            }
            catch (Exception ex)
            {
                SshDotNetDiagnostics.LogException("Protocol: Unexpected error during connection", ex);
                State = ConnectionState.Error;
                Event_ErrorOccured(this, $"Unexpected error: {ex.Message}", null);
                CleanupConnection();
                return false;
            }
        }

        /// <summary>Validates InterfaceControl.Info and resolves the core connection parameters.</summary>
        private bool TryResolveParameters(out string hostname, out int port, out string username, out string password)
        {
            hostname = null; port = 0; username = null; password = null;

            if (InterfaceControl?.Info == null)
            {
                SshDotNetDiagnostics.LogError("Protocol: InterfaceControl.Info is null");
                State = ConnectionState.Error;
                Event_ErrorOccured(this, "Connection information is missing", null);
                return false;
            }

            var connectionInfo = InterfaceControl.Info;
            hostname = connectionInfo.Hostname;
            port = connectionInfo.Port != 0 ? connectionInfo.Port : (int)Defaults.Port;
            username = connectionInfo.Username;
            password = connectionInfo.Password;

            if (string.IsNullOrEmpty(hostname))
            {
                SshDotNetDiagnostics.LogError("Protocol: Hostname is empty");
                State = ConnectionState.Error;
                Event_ErrorOccured(this, "Hostname cannot be empty", null);
                return false;
            }

            if (string.IsNullOrEmpty(username))
            {
                SshDotNetDiagnostics.LogError("Protocol: Username is empty");
                State = ConnectionState.Error;
                Event_ErrorOccured(this, "Username cannot be empty", null);
                return false;
            }

            return true;
        }

        /// <summary>Builds the SSH authentication methods for the connection.</summary>
        private bool TryBuildAuthentication(string username, string password, ConnectionInfo connectionInfo,
            out AuthenticationMethod[] authMethods)
        {
            authMethods = null;
            try
            {
                authMethods = SshAuthenticationProvider.GetAuthenticationMethods(username, password, connectionInfo);
                return true;
            }
            catch (Exception authEx) when (authEx is ArgumentException or FileNotFoundException or SshException)
            {
                SshDotNetDiagnostics.LogException("Protocol: Failed to create authentication methods", authEx);
                State = ConnectionState.Error;
                Event_ErrorOccured(this, $"Authentication setup failed: {authEx.Message}", null);
                return false;
            }
        }

        /// <summary>Creates the SSH client adapter, configures it, and connects to the server.</summary>
        private bool TryEstablishSshConnection(string hostname, int port, string username, AuthenticationMethod[] authMethods)
        {
            try
            {
                _sshClient = SshConnectionManager.CreateAdapter(hostname, port, username, authMethods, TimeSpan.FromSeconds(30));
            }
            catch (ArgumentException createEx)
            {
                SshDotNetDiagnostics.LogException("Protocol: Failed to create SSH client", createEx);
                State = ConnectionState.Error;
                Event_ErrorOccured(this, $"Failed to create SSH client: {createEx.Message}", null);
                return false;
            }

            // Configure keep-alive (uses default 5s interval for fast disconnect detection)
            _sshClient.ConfigureKeepAlive();
            _sshClient.ErrorOccurred += OnSshClientError;

            try
            {
                _sshClient.Connect();
                return true;
            }
            catch (SshAuthenticationException authEx)
            {
                SshDotNetDiagnostics.LogError($"Protocol: Authentication failed - {authEx.Message}");
                State = ConnectionState.Error;
                Event_ErrorOccured(this, $"Authentication failed: {authEx.Message}", null);
                CleanupConnection();
                return false;
            }
            catch (SshConnectionException connEx)
            {
                SshDotNetDiagnostics.LogError($"Protocol: Connection failed - {connEx.Message}");
                State = ConnectionState.Error;
                Event_ErrorOccured(this, $"Connection failed: {connEx.Message}", null);
                CleanupConnection();
                return false;
            }
            catch (System.Net.Sockets.SocketException sockEx)
            {
                SshDotNetDiagnostics.LogError($"Protocol: Network error - {sockEx.Message}");
                State = ConnectionState.Error;
                Event_ErrorOccured(this, $"Network error: {sockEx.Message}", null);
                CleanupConnection();
                return false;
            }
            catch (Exception connEx)
            {
                SshDotNetDiagnostics.LogException("Protocol: Connection failed", connEx);
                State = ConnectionState.Error;
                Event_ErrorOccured(this, $"Connection failed: {connEx.Message}", null);
                CleanupConnection();
                return false;
            }
        }

        /// <summary>Creates the tunnel manager and applies any configured port-forward rules.</summary>
        private void ConfigureTunnels(ConnectionInfo connectionInfo)
        {
            string connInfo = _sshClient.GetConnectionInfo();
            SshDotNetDiagnostics.LogInfo($"Protocol: {connInfo}");

            _tunnelManager = new SshTunnelManager(_sshClient.UnderlyingClient);
            _tunnelManager.TunnelError += OnTunnelError;

            string rules = connectionInfo?.SshDotNetPortForwardRules;
            if (!string.IsNullOrWhiteSpace(rules))
            {
                PortForwardRuleParser.ApplyRules(_tunnelManager, rules);
            }
        }

        /// <summary>Creates the shell stream, attaches the terminal, and starts the I/O tasks.</summary>
        private bool TryStartTerminalSession(ConnectionInfo connectionInfo, string hostname)
        {
            if (!TryCreateShellStream())
                return false;

            // Attach terminal to shell stream
            SshDotNetDiagnostics.LogDebug("Protocol: Attaching terminal to shell stream");
            _terminalControl.AttachSshStream();

            // Start reading output and writing input
            _cancellationTokenSource = new CancellationTokenSource();
            _outputReadTask = Task.Run(() => ReadOutputAsync(_cancellationTokenSource.Token));
            _inputWriteTask = Task.Run(() => WriteInputAsync(_cancellationTokenSource.Token));
            SshDotNetDiagnostics.LogDebug("Protocol: Output reading and input writing tasks started");

            ExecuteOpeningCommand(connectionInfo);

            State = ConnectionState.Connected;
            SshDotNetDiagnostics.StopConnectionTimer($"Full connection to {hostname}");
            SshDotNetDiagnostics.LogInfo("Protocol: Connection established successfully");
            Event_Connected(this);

            FocusTerminalAfterConnect();
            return true;
        }

        /// <summary>Creates the shell stream sized to the current terminal control.</summary>
        private bool TryCreateShellStream()
        {
            try
            {
                if (_terminalControl == null)
                {
                    SshDotNetDiagnostics.LogError("Protocol: Terminal control is null, cannot create shell stream.");
                    State = ConnectionState.Error;
                    Event_ErrorOccured(this, "Terminal control is not available.", null);
                    CleanupConnection();
                    return false;
                }

                int cols = _terminalControl.Columns;
                int rows = _terminalControl.Rows;
                int charW = _terminalControl.CharWidth;
                int charH = _terminalControl.CharHeight;

                SshDotNetDiagnostics.LogDebug($"Protocol: Terminal metrics - Cols={cols}, Rows={rows}, CharW={charW}, CharH={charH}");

                uint widthPixels = (uint)(cols * charW);
                uint heightPixels = (uint)(rows * charH);

                // Validate that pixel dimensions are reasonable (not using defaults)
                if (charW < 6 || charW > 20 || charH < 10 || charH > 30)
                {
                    SshDotNetDiagnostics.LogWarning($"Protocol: Character dimensions seem wrong ({charW}x{charH}), using control size instead");
                    widthPixels = (uint)_terminalControl.Width;
                    heightPixels = (uint)_terminalControl.Height;
                }

                SshDotNetDiagnostics.LogInfo($"Protocol: Creating shell with dimensions {cols}x{rows} ({widthPixels}x{heightPixels} px)");

                _shellStream = _sshClient.CreateShellStream("xterm-256color", (uint)cols, (uint)rows, widthPixels, heightPixels, 1024);
                return true;
            }
            catch (Exception shellEx)
            {
                SshDotNetDiagnostics.LogException("Protocol: Failed to create shell stream", shellEx);
                State = ConnectionState.Error;
                Event_ErrorOccured(this, $"Failed to create shell: {shellEx.Message}", null);
                CleanupConnection();
                return false;
            }
        }

        /// <summary>Writes the configured opening command to the shell, if any. Non-fatal on error.</summary>
        private void ExecuteOpeningCommand(ConnectionInfo connectionInfo)
        {
            if (string.IsNullOrEmpty(connectionInfo.OpeningCommand))
                return;

            SshDotNetDiagnostics.LogDebug($"Protocol: Executing opening command: {connectionInfo.OpeningCommand}");
            try
            {
                _shellStream.WriteLine(connectionInfo.OpeningCommand);
            }
            catch (Exception cmdEx)
            {
                SshDotNetDiagnostics.LogException("Protocol: Failed to execute opening command", cmdEx);
                // Non-fatal, continue
            }
        }

        /// <summary>Focuses the terminal control and forces a resize notification after a successful connect.</summary>
        private void FocusTerminalAfterConnect()
        {
            if (_terminalControl == null || _terminalControl.IsDisposed)
                return;

            _terminalControl.Invoke((Action)(() =>
            {
                _terminalControl.Focus();
                SshDotNetDiagnostics.LogDebug("Protocol: Terminal control focused after connection");

                // Force resize notification to ensure SSH pty matches actual viewport size
                Task.Delay(100).ContinueWith(_ =>
                {
                    if (!_terminalControl.IsDisposed)
                    {
                        _terminalControl.Invoke((Action)(() =>
                        {
                            _terminalControl.ForceResizeNotification();
                            SshDotNetDiagnostics.LogDebug("Protocol: Forced terminal resize notification after connection");
                        }));
                    }
                });
            }));
        }

        private void OnTunnelError(object sender, string errorMessage)
        {
            SshDotNetDiagnostics.LogError($"Protocol: {errorMessage}");
            Event_ErrorOccured(this, errorMessage, null);
        }

        private void OnSshClientError(object sender, Renci.SshNet.Common.ExceptionEventArgs e)
        {
            // Track error details for smart disconnect detection
            _hadRecentError = true;
            _lastErrorTime = DateTime.Now; _lastErrorTicks = Environment.TickCount64;
            _lastErrorMessage = e.Exception.Message;
            _lastException = e.Exception;

            SshDotNetDiagnostics.LogException("Protocol: SSH client error event", e.Exception);
            Event_ErrorOccured(this, $"SSH Error: {e.Exception.Message}", null);

            // Cancel read/write operations immediately to trigger disconnect handling
            try
            {
                _errorCancellationSource?.Cancel();
                SshDotNetDiagnostics.LogDebug("Protocol: Cancelled read/write operations due to SSH error");
            }
            catch (Exception ex)
            {
                SshDotNetDiagnostics.LogException("Protocol: Error cancelling operations", ex);
            }
        }

        private void CleanupConnection()
        {
            SshDotNetDiagnostics.LogDebug("Protocol: Cleaning up failed connection");

            try
            {
                _tunnelManager?.Dispose();
                _tunnelManager = null;
            }
            catch (Exception ex)
            {
                SshDotNetDiagnostics.LogException("Protocol: Error disposing tunnel manager during cleanup", ex);
            }

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
                SshDotNetDiagnostics.LogException("Protocol: Error during connection cleanup", ex);
            }
        }

        #endregion

        #region Disconnection Methods

        public override void Disconnect()
        {
            try
            {
                State = ConnectionState.Disconnecting;
                SshDotNetDiagnostics.LogDebug("Protocol: Disconnect() called");

                // Unsubscribe from terminal events
                if (_terminalControl != null)
                {
                    _terminalControl.TerminalResized -= OnTerminalResized;
                }

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

                // Both I/O tasks have stopped — dispose the per-connection cancellation source (S2930)
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
                _outputReadTask = null;
                _inputWriteTask = null;

                // Close shell stream
                try
                {
                    _shellStream?.Dispose();
                    _shellStream = null;
                }
                catch (Exception ex)
                {
                    SshDotNetDiagnostics.LogException("Protocol: Error disposing shell stream", ex);
                }

                // Dispose tunnel manager BEFORE disconnecting SSH client
                // SSH.NET's ForwardedPort.Stop() sends channel close messages that require an active connection
                try
                {
                    if (_tunnelManager != null)
                    {
                        _tunnelManager.TunnelError -= OnTunnelError;
                        _tunnelManager.Dispose();
                        _tunnelManager = null;
                    }
                }
                catch (Exception ex)
                {
                    SshDotNetDiagnostics.LogException("Protocol: Error disposing tunnel manager", ex);
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
                    SshDotNetDiagnostics.LogException("Protocol: Error disconnecting SSH client", ex);
                }

                // Display connection closed message in terminal
                try
                {
                    if (_terminalControl != null && !_terminalControl.IsDisposed)
                    {
                        _terminalControl.Invoke((Action)(() =>
                        {
                            _terminalControl.DisplayConnectionClosed();
                        }));
                    }
                }
                catch (Exception ex)
                {
                    SshDotNetDiagnostics.LogException("Protocol: Error displaying connection closed message", ex);
                }

                // Detach terminal
                try
                {
                    _terminalControl?.DetachSshStream();
                }
                catch (Exception ex)
                {
                    SshDotNetDiagnostics.LogException("Protocol: Error detaching terminal", ex);
                }

                // Log session statistics before completing disconnection
                LogSessionStatistics();

                State = ConnectionState.Disconnected;
                SshDotNetDiagnostics.LogDebug("Protocol: Disconnection complete");

                Event_Disconnected(this, "User initiated disconnection", null);
            }
            catch (Exception ex)
            {
                SshDotNetDiagnostics.LogException("Protocol: Error during disconnection", ex);
                State = ConnectionState.Error;
                Event_ErrorOccured(this, $"Disconnection error: {ex.Message}", null);
            }
            finally
            {
                base.Disconnect();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
                _errorCancellationSource?.Dispose();
                _shellStream?.Dispose();
                _shellStream = null;
                _tunnelManager?.Dispose();
                _tunnelManager = null;
                _sshClient?.Dispose();
                _sshClient = null;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Log comprehensive session statistics
        /// </summary>
        private void LogSessionStatistics()
        {
            try
            {
                if (_connectionTimer.Elapsed == TimeSpan.Zero)
                    return;  // Never connected

                TimeSpan duration = _connectionTimer.Elapsed;

                // Calculate averages
                double avgReceiveRate = duration.TotalSeconds > 0 ? _bytesReceived / duration.TotalSeconds : 0;
                double avgSendRate = duration.TotalSeconds > 0 ? _bytesSent / duration.TotalSeconds : 0;

                // Build statistics summary
                StringBuilder stats = new StringBuilder();
                stats.AppendLine("=== SSH Session Statistics ===");
                stats.AppendLine($"Duration: {duration.Hours}h {duration.Minutes}m {duration.Seconds}s");
                stats.AppendLine($"Data Received: {FormatBytes(_bytesReceived)} ({_bytesReceived:N0} bytes)");
                stats.AppendLine($"Data Sent: {FormatBytes(_bytesSent)} ({_bytesSent:N0} bytes)");
                stats.AppendLine($"Total Data: {FormatBytes(_bytesReceived + _bytesSent)} ({(_bytesReceived + _bytesSent):N0} bytes)");
                stats.AppendLine($"Avg Receive Rate: {FormatBytes((long)avgReceiveRate)}/s");
                stats.AppendLine($"Avg Send Rate: {FormatBytes((long)avgSendRate)}/s");
                stats.AppendLine($"Peak Receive Rate: {FormatBytes((long)_peakReceiveRate)}/s");
                stats.AppendLine($"Peak Send Rate: {FormatBytes((long)_peakSendRate)}/s");
                stats.AppendLine("==============================");

                SshDotNetDiagnostics.LogInfo(stats.ToString().TrimEnd());
            }
            catch (Exception ex)
            {
                SshDotNetDiagnostics.LogException("Protocol: Error logging session statistics", ex);
            }
        }

        /// <summary>
        /// Format bytes into human-readable format (KB, MB, GB)
        /// </summary>
        private static string FormatBytes(long bytes)
        {
            if (bytes < 1024)
                return $"{bytes} B";
            else if (bytes < 1024 * 1024)
                return $"{bytes / 1024.0:F2} KB";
            else if (bytes < 1024 * 1024 * 1024)
                return $"{bytes / (1024.0 * 1024.0):F2} MB";
            else
                return $"{bytes / (1024.0 * 1024.0 * 1024.0):F2} GB";
        }

        #endregion

        #region Output Reading

        private async Task ReadOutputAsync(CancellationToken cancellationToken)
        {
            SshDotNetDiagnostics.LogDebug("Output: Starting output reading loop");

            byte[] buffer = new byte[4096];
            int consecutiveEmptyReads = 0;
            const int MAX_EMPTY_READS = 100;

            long totalBytes = 0;
            long lastRateLogTicks = Environment.TickCount64;
            const int RATE_LOG_INTERVAL_SECONDS = 30;

            // Create a linked token that responds to both manual cancellation and SSH errors
            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _errorCancellationSource.Token))
            {
                var linkedToken = linkedCts.Token;

                try
                {
                    while (!linkedToken.IsCancellationRequested && _shellStream != null)
                    {
                        try
                        {
                            // Don't check DataAvailable - it's unreliable on SSH.NET ShellStream
                            // Just call ReadAsync which will block until data arrives or timeout
                            int bytesRead = await _shellStream.ReadAsync(
                                buffer, 0, buffer.Length, linkedToken);

                        if (bytesRead > 0)
                        {
                            consecutiveEmptyReads = 0;
                            _bytesReceived += bytesRead;
                            totalBytes += bytesRead;

                            // Log raw data if enabled
                            SshDotNetDiagnostics.LogRawDataBinary(buffer, bytesRead, "Received");

                            SshDotNetDiagnostics.LogTrace($"Output: Read {bytesRead} bytes (total: {_bytesReceived})");

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
                                        SshDotNetDiagnostics.LogException("Output: Error writing to terminal", writeEx);
                                    }
                                }));
                            }

                            // Log data rate periodically and track peak
                            var elapsedMs = Environment.TickCount64 - lastRateLogTicks;
                            if (elapsedMs >= RATE_LOG_INTERVAL_SECONDS * 1000L)
                            {
                                double rate = totalBytes / (elapsedMs / 1000.0);

                                // Track peak receive rate
                                if (rate > _peakReceiveRate)
                                    _peakReceiveRate = rate;

                                SshDotNetDiagnostics.LogInfo($"Output: Data rate: {rate:F0} bytes/sec");

                                if (rate > 100000) // > 100 KB/s
                                {
                                    SshDotNetDiagnostics.LogWarning($"Output: High data rate detected ({rate:F0} bytes/sec), may impact performance");
                                }

                                totalBytes = 0;
                                lastRateLogTicks = Environment.TickCount64;
                            }
                        }
                        else
                        {
                            // Read returned 0 bytes, connection might be closed
                            consecutiveEmptyReads++;
                            SshDotNetDiagnostics.LogDebug($"Output: Empty read #{consecutiveEmptyReads}");

                            if (consecutiveEmptyReads >= MAX_EMPTY_READS)
                            {
                                SshDotNetDiagnostics.LogWarning("Output: Too many empty reads, connection may be closed");
                                break;
                            }

                            await Task.Delay(100, linkedToken);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        SshDotNetDiagnostics.LogDebug("Output: Reading cancelled");
                        break;
                    }
                    catch (IOException ioEx)
                    {
                        // Track this as an error for smart disconnect detection
                        _hadRecentError = true;
                        _lastErrorTime = DateTime.Now; _lastErrorTicks = Environment.TickCount64;
                        _lastErrorMessage = ioEx.Message;
                        _lastException = ioEx;

                        SshDotNetDiagnostics.LogException("Output: I/O error reading stream", ioEx);
                        break;
                    }
                    catch (Exception ex)
                    {
                        // Track this as an error for smart disconnect detection
                        _hadRecentError = true;
                        _lastErrorTime = DateTime.Now; _lastErrorTicks = Environment.TickCount64;
                        _lastErrorMessage = ex.Message;
                        _lastException = ex;

                        SshDotNetDiagnostics.LogException("Output: Error in read loop", ex);
                        // Continue trying to read
                        await Task.Delay(100, linkedToken);
                    }
                    }

                    SshDotNetDiagnostics.LogDebug("Output: Output reading loop ended");

                    // Check if we should trigger disconnection event
                    // Use the ORIGINAL cancellation token to check if this was a manual disconnect
                    // If only the error token was cancelled, we should still handle the disconnect
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        SshDotNetDiagnostics.LogDebug("Output: Connection appears to be closed by remote");

                        // Display connection closed message in terminal
                        try
                        {
                            if (_terminalControl != null && !_terminalControl.IsDisposed)
                            {
                                _terminalControl.Invoke((Action)(() =>
                                {
                                    _terminalControl.DisplayConnectionClosed();
                                }));
                            }
                        }
                        catch (Exception displayEx)
                        {
                            SshDotNetDiagnostics.LogException("Output: Error displaying connection closed message", displayEx);
                        }

                        // Smart disconnect detection: Determine if this was an error disconnect or clean exit
                        // If an error occurred within the last 5 seconds, treat as error disconnect
                        bool isErrorDisconnect = _hadRecentError &&
                                                (Environment.TickCount64 - _lastErrorTicks) < 5000;

                        if (isErrorDisconnect)
                        {
                            // Error disconnect - keep tab open, show error popup with "OK" and "Go to Tab" buttons
                            SshDotNetDiagnostics.LogDebug("Output: Error disconnect detected - keeping tab open");
                            Event_Disconnected(this, "Connection closed due to error", null);
                            ShowErrorDisconnectDialog();
                        }
                        else
                        {
                            // Clean exit (user typed 'exit') - auto-close tab like PuTTY SSH
                            SshDotNetDiagnostics.LogDebug("Output: Clean disconnect detected - auto-closing tab");
                            Event_Closed(this);
                        }
                    }
                }
                catch (Exception ex)
                {
                    SshDotNetDiagnostics.LogException("Output: Fatal error in output reading", ex);
                    Event_ErrorOccured(this, $"Output reading error: {ex.Message}", null);
                }
            } // End of using linkedCts
        }

        private async Task WriteInputAsync(CancellationToken cancellationToken)
        {
            SshDotNetDiagnostics.LogDebug("Input: Starting event-driven input writing loop (zero-delay)");

            // Create a linked token that responds to both manual cancellation and SSH errors
            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _errorCancellationSource.Token))
            {
                var linkedToken = linkedCts.Token;

                try
                {
                    while (!linkedToken.IsCancellationRequested && _shellStream != null && _terminalControl != null)
                {
                    try
                    {
                        // Wait for input to be available (event-driven - ZERO polling delay!)
                        byte[] inputData = await _terminalControl.WaitForInputAsync(linkedToken);

                        if (inputData != null && inputData.Length > 0)
                        {
                            // Write input to SSH shell stream immediately
                            await _shellStream.WriteAsync(inputData, 0, inputData.Length, linkedToken);
                            await _shellStream.FlushAsync(linkedToken);

                            _bytesSent += inputData.Length;

                            // Log raw data if enabled
                            SshDotNetDiagnostics.LogRawDataBinary(inputData, inputData.Length, "Sent");
                            SshDotNetDiagnostics.LogTrace($"Input: Sent {inputData.Length} bytes immediately (total: {_bytesSent})");
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        SshDotNetDiagnostics.LogDebug("Input: Writing cancelled");
                        break;
                    }
                    catch (IOException ioEx)
                    {
                        // Track this as an error for smart disconnect detection
                        _hadRecentError = true;
                        _lastErrorTime = DateTime.Now; _lastErrorTicks = Environment.TickCount64;
                        _lastErrorMessage = ioEx.Message;
                        _lastException = ioEx;

                        SshDotNetDiagnostics.LogException("Input: I/O error writing to stream", ioEx);
                        break;
                    }
                    catch (Exception ex)
                    {
                        // Track this as an error for smart disconnect detection
                        _hadRecentError = true;
                        _lastErrorTime = DateTime.Now; _lastErrorTicks = Environment.TickCount64;
                        _lastErrorMessage = ex.Message;
                        _lastException = ex;

                        SshDotNetDiagnostics.LogException("Input: Error in writing loop", ex);

                        // If client is disconnected, break out of loop
                        if (ex.Message.Contains("not connected", StringComparison.OrdinalIgnoreCase) ||
                            ex.Message.Contains("disconnected", StringComparison.OrdinalIgnoreCase))
                        {
                            SshDotNetDiagnostics.LogWarning("Input: Client disconnected, exiting write loop");
                            break;
                        }

                        await Task.Delay(100, linkedToken);
                    }
                    }

                    SshDotNetDiagnostics.LogDebug("Input: Input writing loop ended");

                    // Check if we should trigger disconnection event
                    // Use the ORIGINAL cancellation token to check if this was a manual disconnect
                    // (only if we haven't already been cancelled by the read loop)
                    if (!cancellationToken.IsCancellationRequested && _hadRecentError)
                    {
                        SshDotNetDiagnostics.LogDebug("Input: Write loop detected error disconnect");

                        // Display connection closed message in terminal
                        try
                        {
                            if (_terminalControl != null && !_terminalControl.IsDisposed)
                            {
                                _terminalControl.Invoke((Action)(() =>
                                {
                                    _terminalControl.DisplayConnectionClosed();
                                }));
                            }
                        }
                        catch (Exception displayEx)
                        {
                            SshDotNetDiagnostics.LogException("Input: Error displaying connection closed message", displayEx);
                        }

                        // Smart disconnect detection: Check if error was recent (within last 5 seconds)
                        bool isErrorDisconnect = _hadRecentError &&
                                                (Environment.TickCount64 - _lastErrorTicks) < 5000;

                        if (isErrorDisconnect)
                        {
                            // Error disconnect - keep tab open, show error popup
                            SshDotNetDiagnostics.LogDebug("Input: Error disconnect detected - keeping tab open");
                            Event_Disconnected(this, "Connection closed due to error", null);
                            ShowErrorDisconnectDialog();
                        }
                        else
                        {
                            // Clean exit - auto-close tab
                            SshDotNetDiagnostics.LogDebug("Input: Clean disconnect detected - auto-closing tab");
                            Event_Closed(this);
                        }
                    }
                }
                catch (Exception ex)
                {
                    SshDotNetDiagnostics.LogException("Input: Fatal error in input writing", ex);
                }
            } // End of using linkedCts
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles terminal resize events and notifies the SSH server to update the pty size
        /// </summary>
        private void OnTerminalResized(object sender, UI.Controls.TerminalResizeEventArgs e)
        {
            try
            {
                if (_shellStream != null && _shellStream.CanWrite)
                {
                    // Notify SSH server of terminal size change (SIGWINCH equivalent)
                    // Include pixel dimensions for apps like htop that need accurate sizing
                    _shellStream.SendWindowChangeRequest(
                        (uint)e.Columns,
                        (uint)e.Rows,
                        (uint)e.WidthPixels,
                        (uint)e.HeightPixels);
                    SshDotNetDiagnostics.LogInfo($"Protocol: Sent window change request to SSH server: {e.Columns}x{e.Rows} ({e.WidthPixels}x{e.HeightPixels} px)");
                }
            }
            catch (Exception ex)
            {
                SshDotNetDiagnostics.LogException("Protocol: Error sending window change request", ex);
            }
        }

        #endregion

        #region Smart Disconnect Handling

        /// <summary>
        /// Shows an error disconnect dialog with expandable technical details
        /// Offers "Yes" to go to tab or "No" to just dismiss
        /// </summary>
        private void ShowErrorDisconnectDialog()
        {
            if (_terminalControl == null) return;

            _terminalControl.Invoke((Action)(() =>
            {
                try
                {
                    // Build technical details for expandable section
                    string technicalDetails = BuildTechnicalDetails();

                    // Show dialog with Yes/No buttons
                    // Yes = Go to Tab, No = Just dismiss
                    DialogResult result = CTaskDialog.MessageBox(
                        _terminalControl.FindForm(),
                        "SSH Connection Error",
                        "The SSH connection was closed unexpectedly.",
                        "The connection to the remote host was terminated due to an error. " +
                        "You can review the terminal output in the disconnected tab.\n\n" +
                        "Would you like to switch to the disconnected tab now?",
                        technicalDetails,  // Expandable technical details
                        "",
                        "",
                        ETaskDialogButtons.YesNo,
                        ESysIcons.Error,
                        ESysIcons.Error
                    );

                    // If user clicked "Yes" (Go to Tab)
                    if (result == DialogResult.Yes)
                    {
                        FocusTab();
                    }

                    SshDotNetDiagnostics.LogDebug($"Protocol: Error dialog dismissed, user chose: {result}");
                }
                catch (Exception ex)
                {
                    SshDotNetDiagnostics.LogException("Protocol: Error showing disconnect dialog", ex);
                }
            }));
        }

        /// <summary>
        /// Builds technical details string for error dialog expandable section
        /// </summary>
        private string BuildTechnicalDetails()
        {
            StringBuilder details = new StringBuilder();

            details.AppendLine($"Error Time: {_lastErrorTime:yyyy-MM-dd HH:mm:ss}");
            details.AppendLine($"Error Type: {_lastException?.GetType().Name ?? "Unknown"}");
            details.AppendLine($"Message: {_lastErrorMessage}");

            if (_lastException != null)
            {
                details.AppendLine();
                details.AppendLine("Stack Trace:");
                details.AppendLine(_lastException.StackTrace);
            }

            return details.ToString();
        }

        /// <summary>
        /// Focuses/activates the connection tab
        /// </summary>
        private void FocusTab()
        {
            try
            {
                if (InterfaceControl?.Parent is ConnectionTab tab)
                {
                    tab.Show(tab.DockPanel);
                    tab.Focus();
                    SshDotNetDiagnostics.LogDebug("Protocol: Focused disconnected tab");
                }
                else
                {
                    SshDotNetDiagnostics.LogWarning("Protocol: Could not focus tab - InterfaceControl.Parent is not a ConnectionTab");
                }
            }
            catch (Exception ex)
            {
                SshDotNetDiagnostics.LogException("Protocol: Error focusing tab", ex);
            }
        }

        #endregion
    }
}
