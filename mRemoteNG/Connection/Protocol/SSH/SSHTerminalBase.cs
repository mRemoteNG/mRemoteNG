using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;
using Renci.SshNet;

namespace mRemoteNG.Connection.Protocol.SSH
{
    [SupportedOSPlatform("windows")]
    public class SSHTerminalBase : ProtocolBase
    {
        #region Fields

        private WebView2 _webView2;
        private CoreWebView2Environment _webView2Environment;
        private Task _initTask;
        private string _userDataFolder;
        private string _resourceFolder;

        private SshClient _sshClient;
        private ShellStream _shellStream;
        private CancellationTokenSource _readCts;

        private uint _termCols = 80;
        private uint _termRows = 24;
        private bool _sshConnected;
        private readonly object _writeLock = new();

        #endregion

        #region Initialize

        public override bool Initialize()
        {
            try
            {
                // Create unique user data folder for WebView2
                _userDataFolder = Path.Combine(
                    Path.GetTempPath(),
                    "mRemoteNG_WebView2_SSH",
                    Guid.NewGuid().ToString());

                // Extract xterm.js resources to a temp folder for virtual host mapping
                _resourceFolder = Path.Combine(
                    Path.GetTempPath(),
                    "mRemoteNG_XtermResources");
                ExtractResources();

                _webView2 = new WebView2 { Dock = DockStyle.Fill };
                Control = _webView2;

                base.Initialize();

                _webView2.CoreWebView2InitializationCompleted += OnWebView2InitCompleted;
                _initTask = InitializeWebView2Async();

                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("SSHTerminal: Initialize failed", ex);
                return false;
            }
        }

        private async Task InitializeWebView2Async()
        {
            try
            {
                _webView2Environment = await CoreWebView2Environment.CreateAsync(null, _userDataFolder);
                await _webView2.EnsureCoreWebView2Async(_webView2Environment);

                // Map virtual host to resource folder so HTML can load JS/CSS files
                _webView2.CoreWebView2.SetVirtualHostNameToFolderMapping(
                    "xterm.local",
                    _resourceFolder,
                    CoreWebView2HostResourceAccessKind.Allow);

                // Register message handler
                _webView2.CoreWebView2.WebMessageReceived += OnWebMessageReceived;

                // Suppress new windows
                _webView2.CoreWebView2.NewWindowRequested += (s, e) => e.Handled = true;

                // Navigate to the terminal HTML
                string htmlPath = Path.Combine(_resourceFolder, "xterm-terminal.html");
                _webView2.CoreWebView2.Navigate($"https://xterm.local/xterm-terminal.html");
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("SSHTerminal: WebView2 init failed", ex);
            }
        }

        private void OnWebView2InitCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (!e.IsSuccess)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("SSHTerminal: WebView2 init error", e.InitializationException);
            }
        }

        #endregion

        #region Connect

        public override bool Connect()
        {
            try
            {
                if (_initTask != null && !_initTask.IsCompleted)
                {
                    // SSH connection will be triggered when xterm.js sends "ready" message
                    _initTask.ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                        {
                            Runtime.MessageCollector.AddExceptionStackTrace("SSHTerminal: Init task failed", t.Exception);
                        }
                    }, TaskScheduler.Default);
                }

                base.Connect();
                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("SSHTerminal: Connect failed", ex);
                return false;
            }
        }

        private async Task ConnectSshAsync()
        {
            try
            {
                var info = InterfaceControl.Info;
                string hostname = info.Hostname;
                int port = info.Port;
                string username = info.Username;
                string password = info.Password;

                // Fallback to Windows username if empty
                if (string.IsNullOrEmpty(username))
                    username = Environment.UserName;

                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                    $"SSHTerminal: Connecting to {hostname}:{port} as {username}");

                // Create SSH connection
                var connectionInfo = new Renci.SshNet.ConnectionInfo(hostname, port, username,
                    new PasswordAuthenticationMethod(username, password ?? ""));

                _sshClient = new SshClient(connectionInfo);

                // Auto-accept host keys (TODO: host key verification dialog)
                _sshClient.HostKeyReceived += (s, e) => e.CanTrust = true;

                await Task.Run(() => _sshClient.Connect());

                if (!_sshClient.IsConnected)
                {
                    PostOutputToTerminal("\r\n\x1b[31mSSH connection failed.\x1b[0m\r\n");
                    Event_ErrorOccured(this, "SSH connection failed", null);
                    return;
                }

                // Create interactive shell
                _shellStream = _sshClient.CreateShellStream(
                    "xterm-256color",
                    _termCols, _termRows,
                    0, 0,
                    8192);

                _sshConnected = true;

                // Start reading shell output
                _readCts = new CancellationTokenSource();
                _ = ReadShellStreamAsync(_readCts.Token);

                // Fire connected event on UI thread
                _webView2.Invoke(() => Event_Connected(this));

                // Execute opening command if configured
                if (!string.IsNullOrEmpty(InterfaceControl.Info?.OpeningCommand))
                {
                    await Task.Delay(500); // Brief delay to let shell initialize
                    string cmd = InterfaceControl.Info.OpeningCommand.TrimEnd() + "\n";
                    WriteToShell(cmd);
                }

                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                    $"SSHTerminal: Connected to {hostname}:{port}");
            }
            catch (Exception ex)
            {
                string msg = $"SSH connection failed: {ex.Message}";
                PostOutputToTerminal($"\r\n\x1b[31m{msg}\x1b[0m\r\n");
                Runtime.MessageCollector.AddExceptionStackTrace("SSHTerminal: SSH connect failed", ex);
                try { _webView2?.Invoke(() => Event_ErrorOccured(this, msg, null)); } catch { }
            }
        }

        #endregion

        #region Data Flow

        private async Task ReadShellStreamAsync(CancellationToken ct)
        {
            var buffer = new byte[8192];
            try
            {
                while (!ct.IsCancellationRequested && _sshClient?.IsConnected == true && _shellStream?.CanRead == true)
                {
                    int bytesRead = await _shellStream.ReadAsync(buffer, 0, buffer.Length, ct);
                    if (bytesRead > 0)
                    {
                        string base64 = Convert.ToBase64String(buffer, 0, bytesRead);
                        string json = JsonSerializer.Serialize(new { type = "output", data = base64 });

                        try
                        {
                            _webView2?.Invoke(() =>
                            {
                                if (_webView2?.CoreWebView2 != null)
                                    _webView2.CoreWebView2.PostWebMessageAsString(json);
                            });
                        }
                        catch (ObjectDisposedException) { break; }
                        catch (InvalidOperationException) { break; }
                    }
                }
            }
            catch (OperationCanceledException) { }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("SSHTerminal: Read loop error", ex);
            }

            // Connection ended
            if (!ct.IsCancellationRequested)
            {
                try
                {
                    PostOutputToTerminal("\r\n\x1b[33mConnection closed.\x1b[0m\r\n");
                    _webView2?.Invoke(() => Event_Disconnected(this, "SSH connection closed", null));
                }
                catch { }
            }
        }

        private void WriteToShell(string data)
        {
            if (_shellStream == null || !_sshConnected) return;
            lock (_writeLock)
            {
                try
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(data);
                    _shellStream.Write(bytes, 0, bytes.Length);
                    _shellStream.Flush();
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector.AddExceptionStackTrace("SSHTerminal: Write failed", ex);
                }
            }
        }

        private void PostOutputToTerminal(string text)
        {
            try
            {
                string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
                string json = JsonSerializer.Serialize(new { type = "output", data = base64 });
                _webView2?.Invoke(() =>
                {
                    if (_webView2?.CoreWebView2 != null)
                        _webView2.CoreWebView2.PostWebMessageAsString(json);
                });
            }
            catch { }
        }

        #endregion

        #region WebView2 Message Handling

        private void OnWebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            try
            {
                string messageJson = e.TryGetWebMessageAsString();
                using var doc = JsonDocument.Parse(messageJson);
                var root = doc.RootElement;
                string type = root.GetProperty("type").GetString();

                switch (type)
                {
                    case "ready":
                        // xterm.js is ready — start SSH connection
                        _ = ConnectSshAsync();
                        break;

                    case "input":
                        string inputData = root.GetProperty("data").GetString();
                        WriteToShell(inputData);
                        break;

                    case "resize":
                        uint cols = root.GetProperty("cols").GetUInt32();
                        uint rows = root.GetProperty("rows").GetUInt32();
                        _termCols = cols;
                        _termRows = rows;
                        if (_shellStream != null && _sshConnected)
                        {
                            try { _shellStream.ChangeWindowSize(cols, rows, 0, 0); }
                            catch { }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("SSHTerminal: Message handling error", ex);
            }
        }

        #endregion

        #region Focus & Resize

        public override void Focus()
        {
            try
            {
                _webView2?.Focus();
                string json = JsonSerializer.Serialize(new { type = "focus" });
                if (_webView2?.CoreWebView2 != null)
                    _webView2.CoreWebView2.PostWebMessageAsString(json);
            }
            catch { }
        }

        #endregion

        #region Close & Cleanup

        public override void Close()
        {
            try
            {
                _readCts?.Cancel();
                _sshConnected = false;

                _shellStream?.Dispose();
                _shellStream = null;

                if (_sshClient?.IsConnected == true)
                    _sshClient.Disconnect();
                _sshClient?.Dispose();
                _sshClient = null;

                // Clean up WebView2
                if (_initTask != null && !_initTask.IsCompleted)
                {
                    _initTask.ContinueWith(_ => CleanupWebView2(), TaskScheduler.Default);
                }
                else
                {
                    CleanupWebView2();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("SSHTerminal: Close error", ex);
            }

            base.Close();
        }

        private void CleanupWebView2()
        {
            try
            {
                _webView2Environment = null;

                // Clean up user data folder
                if (!string.IsNullOrEmpty(_userDataFolder) && Directory.Exists(_userDataFolder))
                {
                    try
                    {
                        string tempPath = Path.GetTempPath();
                        string fullPath = Path.GetFullPath(_userDataFolder);
                        if (fullPath.StartsWith(Path.GetFullPath(tempPath), StringComparison.OrdinalIgnoreCase)
                            && fullPath.Contains("mRemoteNG_WebView2_SSH"))
                        {
                            Directory.Delete(_userDataFolder, true);
                        }
                    }
                    catch { }
                }
            }
            catch { }
        }

        #endregion

        #region Resource Extraction

        private void ExtractResources()
        {
            Directory.CreateDirectory(_resourceFolder);

            string[] resourceNames = new[]
            {
                "xterm-terminal.html",
                "xterm.min.js",
                "xterm.css",
                "addon-fit.min.js"
            };

            var assembly = Assembly.GetExecutingAssembly();

            foreach (var fileName in resourceNames)
            {
                string targetPath = Path.Combine(_resourceFolder, fileName);

                // Only extract if not already present or if the assembly is newer
                if (File.Exists(targetPath)) continue;

                string resourceName = FindResourceName(assembly, fileName);
                if (resourceName == null)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                        $"SSHTerminal: Embedded resource not found: {fileName}");
                    continue;
                }

                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null) continue;

                using var fs = File.Create(targetPath);
                stream.CopyTo(fs);
            }
        }

        private static string FindResourceName(Assembly assembly, string fileName)
        {
            // Resource names use dots for path separators and may have project namespace prefix
            string searchName = fileName.Replace("-", "_").Replace("/", ".");
            foreach (var name in assembly.GetManifestResourceNames())
            {
                if (name.EndsWith(searchName, StringComparison.OrdinalIgnoreCase)
                    || name.EndsWith(fileName, StringComparison.OrdinalIgnoreCase)
                    || name.EndsWith(fileName.Replace("-", "_"), StringComparison.OrdinalIgnoreCase))
                {
                    return name;
                }
            }
            return null;
        }

        #endregion
    }
}
