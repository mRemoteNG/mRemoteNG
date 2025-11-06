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
using mRemoteNG.UI.TaskDialog;
using mRemoteNG.UI.Tabs;

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
        private double _peakReceiveRate = 0;  // bytes/sec
        private double _peakSendRate = 0;     // bytes/sec

        // Error tracking for smart disconnect detection
        private bool _hadRecentError = false;
        private DateTime _lastErrorTime;
        private string _lastErrorMessage = "";
        private Exception _lastException = null;
        private CancellationTokenSource _errorCancellationSource = new CancellationTokenSource();

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
                SSHDotNetDiagnostics.LogDebug("Protocol: Initializing ProtocolSSH_DotNet");

                // Create and initialize terminal control
                _terminalControl = new SshTerminalControl();
                _terminalControl.Initialize();

                // Subscribe to terminal resize events to notify SSH server
                _terminalControl.TerminalResized += OnTerminalResized;

                Control = _terminalControl;

                // Call base initialization
                bool baseResult = base.Initialize();

                SSHDotNetDiagnostics.LogDebug("Protocol: Initialization complete");
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
            SSHDotNetDiagnostics.LogDebug("Protocol: Connect() called");
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

                // Configure keep-alive (uses default 5s interval for fast disconnect detection)
                SSHConnectionManager.ConfigureKeepAlive(_sshClient);

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

                // Create shell stream with correct pixel dimensions
                try
                {
                    // Calculate pixel dimensions from actual control size to ensure accuracy
                    // This is more reliable than using cached CharWidth/CharHeight which might be defaults
                    int cols = _terminalControl.Columns;
                    int rows = _terminalControl.Rows;
                    int charW = _terminalControl.CharWidth;
                    int charH = _terminalControl.CharHeight;

                    SSHDotNetDiagnostics.LogDebug($"Protocol: Terminal metrics - Cols={cols}, Rows={rows}, CharW={charW}, CharH={charH}");

                    // Calculate pixel dimensions
                    uint widthPixels = (uint)(cols * charW);
                    uint heightPixels = (uint)(rows * charH);

                    // Validate that pixel dimensions are reasonable (not using defaults)
                    if (charW < 6 || charW > 20 || charH < 10 || charH > 30)
                    {
                        SSHDotNetDiagnostics.LogWarning($"Protocol: Character dimensions seem wrong ({charW}x{charH}), using control size instead");
                        // Fallback: use actual control dimensions as pixels (less accurate but better than defaults)
                        widthPixels = (uint)_terminalControl.Width;
                        heightPixels = (uint)_terminalControl.Height;
                    }

                    SSHDotNetDiagnostics.LogInfo($"Protocol: Creating shell with dimensions {cols}x{rows} ({widthPixels}x{heightPixels} px)");

                    _shellStream = SSHConnectionManager.CreateShellStream(
                        _sshClient,
                        "xterm-256color",
                        (uint)cols,
                        (uint)rows,
                        widthPixels,
                        heightPixels,
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

                SSHDotNetDiagnostics.LogDebug("Protocol: Output reading and input writing tasks started");

                // Execute opening command if configured
                if (!string.IsNullOrEmpty(connectionInfo.OpeningCommand))
                {
                    SSHDotNetDiagnostics.LogDebug($"Protocol: Executing opening command: {connectionInfo.OpeningCommand}");
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

                // Automatically focus the terminal control so user can start typing immediately
                if (_terminalControl != null && !_terminalControl.IsDisposed)
                {
                    _terminalControl.Invoke((Action)(() =>
                    {
                        _terminalControl.Focus();
                        SSHDotNetDiagnostics.LogDebug("Protocol: Terminal control focused after connection");

                        // Force resize notification to ensure SSH pty matches actual viewport size
                        // This is necessary because the control may have been resized after shell stream creation
                        Task.Delay(100).ContinueWith(_ =>
                        {
                            if (!_terminalControl.IsDisposed)
                            {
                                _terminalControl.Invoke((Action)(() =>
                                {
                                    _terminalControl.ForceResizeNotification();
                                    SSHDotNetDiagnostics.LogDebug("Protocol: Forced terminal resize notification after connection");
                                }));
                            }
                        });
                    }));
                }

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
            // Track error details for smart disconnect detection
            _hadRecentError = true;
            _lastErrorTime = DateTime.Now;
            _lastErrorMessage = e.Exception.Message;
            _lastException = e.Exception;

            SSHDotNetDiagnostics.LogException("Protocol: SSH client error event", e.Exception);
            Event_ErrorOccured(this, $"SSH Error: {e.Exception.Message}", null);

            // Cancel read/write operations immediately to trigger disconnect handling
            try
            {
                _errorCancellationSource?.Cancel();
                SSHDotNetDiagnostics.LogDebug("Protocol: Cancelled read/write operations due to SSH error");
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Protocol: Error cancelling operations", ex);
            }
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
                SSHDotNetDiagnostics.LogDebug("Protocol: Disconnect() called");

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
                    SSHDotNetDiagnostics.LogException("Protocol: Error displaying connection closed message", ex);
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

                // Log session statistics before completing disconnection
                LogSessionStatistics();

                State = ConnectionState.Disconnected;
                SSHDotNetDiagnostics.LogDebug("Protocol: Disconnection complete");

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

        /// <summary>
        /// Log comprehensive session statistics
        /// </summary>
        private void LogSessionStatistics()
        {
            try
            {
                if (_connectionStartTime == default(DateTime))
                    return;  // Never connected

                TimeSpan duration = DateTime.Now - _connectionStartTime;

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

                SSHDotNetDiagnostics.LogInfo(stats.ToString().TrimEnd());
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Protocol: Error logging session statistics", ex);
            }
        }

        /// <summary>
        /// Format bytes into human-readable format (KB, MB, GB)
        /// </summary>
        private string FormatBytes(long bytes)
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
            SSHDotNetDiagnostics.LogDebug("Output: Starting output reading loop");

            byte[] buffer = new byte[4096];
            int consecutiveEmptyReads = 0;
            const int MAX_EMPTY_READS = 100;

            long totalBytes = 0;
            DateTime lastRateLog = DateTime.Now;
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
                            SSHDotNetDiagnostics.LogRawDataBinary(buffer, bytesRead, "Received");

                            SSHDotNetDiagnostics.LogTrace($"Output: Read {bytesRead} bytes (total: {_bytesReceived})");

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

                            // Log data rate periodically and track peak
                            var elapsed = DateTime.Now - lastRateLog;
                            if (elapsed.TotalSeconds >= RATE_LOG_INTERVAL_SECONDS)
                            {
                                double rate = totalBytes / elapsed.TotalSeconds;

                                // Track peak receive rate
                                if (rate > _peakReceiveRate)
                                    _peakReceiveRate = rate;

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

                            await Task.Delay(100, linkedToken);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        SSHDotNetDiagnostics.LogDebug("Output: Reading cancelled");
                        break;
                    }
                    catch (IOException ioEx)
                    {
                        // Track this as an error for smart disconnect detection
                        _hadRecentError = true;
                        _lastErrorTime = DateTime.Now;
                        _lastErrorMessage = ioEx.Message;
                        _lastException = ioEx;

                        SSHDotNetDiagnostics.LogException("Output: I/O error reading stream", ioEx);
                        break;
                    }
                    catch (Exception ex)
                    {
                        // Track this as an error for smart disconnect detection
                        _hadRecentError = true;
                        _lastErrorTime = DateTime.Now;
                        _lastErrorMessage = ex.Message;
                        _lastException = ex;

                        SSHDotNetDiagnostics.LogException("Output: Error in read loop", ex);
                        // Continue trying to read
                        await Task.Delay(100, linkedToken);
                    }
                    }

                    SSHDotNetDiagnostics.LogDebug("Output: Output reading loop ended");

                    // Check if we should trigger disconnection event
                    // Use the ORIGINAL cancellation token to check if this was a manual disconnect
                    // If only the error token was cancelled, we should still handle the disconnect
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        SSHDotNetDiagnostics.LogDebug("Output: Connection appears to be closed by remote");

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
                            SSHDotNetDiagnostics.LogException("Output: Error displaying connection closed message", displayEx);
                        }

                        // Smart disconnect detection: Determine if this was an error disconnect or clean exit
                        // If an error occurred within the last 5 seconds, treat as error disconnect
                        bool isErrorDisconnect = _hadRecentError &&
                                                (DateTime.Now - _lastErrorTime).TotalSeconds < 5;

                        if (isErrorDisconnect)
                        {
                            // Error disconnect - keep tab open, show error popup with "OK" and "Go to Tab" buttons
                            SSHDotNetDiagnostics.LogDebug("Output: Error disconnect detected - keeping tab open");
                            Event_Disconnected(this, "Connection closed due to error", null);
                            ShowErrorDisconnectDialog();
                        }
                        else
                        {
                            // Clean exit (user typed 'exit') - auto-close tab like PuTTY SSH
                            SSHDotNetDiagnostics.LogDebug("Output: Clean disconnect detected - auto-closing tab");
                            Event_Closed(this);
                        }
                    }
                }
                catch (Exception ex)
                {
                    SSHDotNetDiagnostics.LogException("Output: Fatal error in output reading", ex);
                    Event_ErrorOccured(this, $"Output reading error: {ex.Message}", null);
                }
            } // End of using linkedCts
        }

        private async Task WriteInputAsync(CancellationToken cancellationToken)
        {
            SSHDotNetDiagnostics.LogDebug("Input: Starting event-driven input writing loop (zero-delay)");

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
                            SSHDotNetDiagnostics.LogRawDataBinary(inputData, inputData.Length, "Sent");
                            SSHDotNetDiagnostics.LogTrace($"Input: Sent {inputData.Length} bytes immediately (total: {_bytesSent})");
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        SSHDotNetDiagnostics.LogDebug("Input: Writing cancelled");
                        break;
                    }
                    catch (IOException ioEx)
                    {
                        // Track this as an error for smart disconnect detection
                        _hadRecentError = true;
                        _lastErrorTime = DateTime.Now;
                        _lastErrorMessage = ioEx.Message;
                        _lastException = ioEx;

                        SSHDotNetDiagnostics.LogException("Input: I/O error writing to stream", ioEx);
                        break;
                    }
                    catch (Exception ex)
                    {
                        // Track this as an error for smart disconnect detection
                        _hadRecentError = true;
                        _lastErrorTime = DateTime.Now;
                        _lastErrorMessage = ex.Message;
                        _lastException = ex;

                        SSHDotNetDiagnostics.LogException("Input: Error in writing loop", ex);

                        // If client is disconnected, break out of loop
                        if (ex.Message.Contains("not connected", StringComparison.OrdinalIgnoreCase) ||
                            ex.Message.Contains("disconnected", StringComparison.OrdinalIgnoreCase))
                        {
                            SSHDotNetDiagnostics.LogWarning("Input: Client disconnected, exiting write loop");
                            break;
                        }

                        await Task.Delay(100, linkedToken);
                    }
                    }

                    SSHDotNetDiagnostics.LogDebug("Input: Input writing loop ended");

                    // Check if we should trigger disconnection event
                    // Use the ORIGINAL cancellation token to check if this was a manual disconnect
                    // (only if we haven't already been cancelled by the read loop)
                    if (!cancellationToken.IsCancellationRequested && _hadRecentError)
                    {
                        SSHDotNetDiagnostics.LogDebug("Input: Write loop detected error disconnect");

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
                            SSHDotNetDiagnostics.LogException("Input: Error displaying connection closed message", displayEx);
                        }

                        // Smart disconnect detection: Check if error was recent (within last 5 seconds)
                        bool isErrorDisconnect = _hadRecentError &&
                                                (DateTime.Now - _lastErrorTime).TotalSeconds < 5;

                        if (isErrorDisconnect)
                        {
                            // Error disconnect - keep tab open, show error popup
                            SSHDotNetDiagnostics.LogDebug("Input: Error disconnect detected - keeping tab open");
                            Event_Disconnected(this, "Connection closed due to error", null);
                            ShowErrorDisconnectDialog();
                        }
                        else
                        {
                            // Clean exit - auto-close tab
                            SSHDotNetDiagnostics.LogDebug("Input: Clean disconnect detected - auto-closing tab");
                            Event_Closed(this);
                        }
                    }
                }
                catch (Exception ex)
                {
                    SSHDotNetDiagnostics.LogException("Input: Fatal error in input writing", ex);
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
                    SSHDotNetDiagnostics.LogInfo($"Protocol: Sent window change request to SSH server: {e.Columns}x{e.Rows} ({e.WidthPixels}x{e.HeightPixels} px)");
                }
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Protocol: Error sending window change request", ex);
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

                    SSHDotNetDiagnostics.LogDebug($"Protocol: Error dialog dismissed, user chose: {result}");
                }
                catch (Exception ex)
                {
                    SSHDotNetDiagnostics.LogException("Protocol: Error showing disconnect dialog", ex);
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
                    SSHDotNetDiagnostics.LogDebug("Protocol: Focused disconnected tab");
                }
                else
                {
                    SSHDotNetDiagnostics.LogWarning("Protocol: Could not focus tab - InterfaceControl.Parent is not a ConnectionTab");
                }
            }
            catch (Exception ex)
            {
                SSHDotNetDiagnostics.LogException("Protocol: Error focusing tab", ex);
            }
        }

        #endregion
    }
}
