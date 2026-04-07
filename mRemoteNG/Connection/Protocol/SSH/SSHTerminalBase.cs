using System;
using System.IO;
using System.Linq;
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
    public class SshTerminalBase : ProtocolBase
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
                _userDataFolder = Path.Combine(
                    Path.GetTempPath(),
                    "mRemoteNG_WebView2_SSH",
                    Guid.NewGuid().ToString());

                // Per-session resource folder to prevent tampering
                _resourceFolder = Path.Combine(
                    Path.GetTempPath(),
                    "mRemoteNG_xterm",
                    Guid.NewGuid().ToString());
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
                Runtime.MessageCollector.AddExceptionStackTrace("SshTerminal: Initialize failed", ex);
                return false;
            }
        }

        private async Task InitializeWebView2Async()
        {
            try
            {
                _webView2Environment = await CoreWebView2Environment.CreateAsync(null, _userDataFolder);
                await _webView2.EnsureCoreWebView2Async(_webView2Environment);

                _webView2.CoreWebView2.SetVirtualHostNameToFolderMapping(
                    "xterm.local",
                    _resourceFolder,
                    CoreWebView2HostResourceAccessKind.Allow);

                _webView2.CoreWebView2.WebMessageReceived += OnWebMessageReceived;
                _webView2.CoreWebView2.NewWindowRequested += (s, e) => e.Handled = true;

                string htmlPath = Path.Combine(_resourceFolder, "xterm-terminal.html");
                if (!File.Exists(htmlPath))
                    throw new FileNotFoundException("Terminal HTML resource not found.", htmlPath);

                _webView2.CoreWebView2.Navigate("https://xterm.local/xterm-terminal.html");
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("SshTerminal: WebView2 init failed", ex);
            }
        }

        private void OnWebView2InitCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (!e.IsSuccess)
                Runtime.MessageCollector.AddExceptionStackTrace("SshTerminal: WebView2 init error", e.InitializationException);
        }

        #endregion

        #region Connect

        public override bool Connect()
        {
            try
            {
                if (_initTask != null && !_initTask.IsCompleted)
                {
                    _initTask.ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                            Runtime.MessageCollector.AddExceptionStackTrace("SshTerminal: Init task failed", t.Exception);
                    }, TaskScheduler.Default);
                }

                // Do NOT call base.Connect() here — it fires ConnectedEvent prematurely.
                // The Connected event will be fired once SSH is actually established in ConnectSshAsync.
                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("SshTerminal: Connect failed", ex);
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
                string username = string.IsNullOrEmpty(info.Username) ? Environment.UserName : info.Username;

                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                    $"SshTerminal: Connecting to {hostname}:{port} as {username}");

                var connectionInfo = new Renci.SshNet.ConnectionInfo(
                    hostname, port, username,
                    BuildAuthMethods(username, info.Password));

                _sshClient = new SshClient(connectionInfo);
                _sshClient.HostKeyReceived += OnHostKeyReceived;

                await Task.Run(() => _sshClient.Connect());

                if (!_sshClient.IsConnected)
                {
                    PostOutputToTerminal("\r\n\x1b[31mSSH connection failed.\x1b[0m\r\n");
                    Event_ErrorOccured(this, "SSH connection failed", null);
                    return;
                }

                StartShellSession();
                await ExecuteOpeningCommand();

                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                    $"SshTerminal: Connected to {hostname}:{port}");
            }
            catch (Exception ex)
            {
                string msg = $"SSH connection failed: {ex.Message}";
                PostOutputToTerminal($"\r\n\x1b[31m{msg}\x1b[0m\r\n");
                Runtime.MessageCollector.AddExceptionStackTrace("SshTerminal: SSH connect failed", ex);
                InvokeOnUiThread(() => Event_ErrorOccured(this, msg, null));
            }
        }

        private static AuthenticationMethod[] BuildAuthMethods(string username, string password)
        {
            var methods = new System.Collections.Generic.List<AuthenticationMethod>();

            if (!string.IsNullOrEmpty(password))
                methods.Add(new PasswordAuthenticationMethod(username, password));

            var kbInteractive = new KeyboardInteractiveAuthenticationMethod(username);
            kbInteractive.AuthenticationPrompt += (s, e) =>
            {
                foreach (var prompt in e.Prompts)
                {
                    if (prompt.Request.Contains("password", StringComparison.OrdinalIgnoreCase))
                        prompt.Response = password ?? "";
                }
            };
            methods.Add(kbInteractive);

            return methods.ToArray();
        }

        private void StartShellSession()
        {
            _shellStream = _sshClient.CreateShellStream(
                "xterm-256color", _termCols, _termRows, 0, 0, 8192);

            _sshConnected = true;
            _readCts = new CancellationTokenSource();
            _ = ReadShellStreamAsync(_readCts.Token);

            _webView2.Invoke(() => Event_Connected(this));
        }

        private async Task ExecuteOpeningCommand()
        {
            string cmd = InterfaceControl.Info?.OpeningCommand;
            if (string.IsNullOrEmpty(cmd)) return;
            await Task.Delay(500);
            WriteToShell(cmd.TrimEnd() + "\n");
        }

        private void InvokeOnUiThread(Action action)
        {
            try { _webView2?.Invoke(action); }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
        }

        private void OnHostKeyReceived(object sender, Renci.SshNet.Common.HostKeyEventArgs e)
        {
            // For now, accept all host keys with a warning log.
            // A full implementation would check a known_hosts file and prompt the user.
            string fingerprint = BitConverter.ToString(e.FingerPrint).Replace("-", ":");
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                $"SshTerminal: Host key fingerprint: {fingerprint} ({e.HostKeyName})", true);
            e.CanTrust = true;
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
                    if (bytesRead <= 0) continue;

                    if (!PostDataToWebView(buffer, bytesRead))
                        break;
                }
            }
            catch (OperationCanceledException) { /* expected on close */ }
            catch (ObjectDisposedException) { /* expected on close */ }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("SshTerminal: Read loop error", ex);
            }

            if (!ct.IsCancellationRequested)
                OnConnectionClosed();
        }

        private bool PostDataToWebView(byte[] buffer, int length)
        {
            string base64 = Convert.ToBase64String(buffer, 0, length);
            string json = JsonSerializer.Serialize(new { type = "output", data = base64 });

            try
            {
                _webView2?.Invoke(() =>
                {
                    if (_webView2?.CoreWebView2 != null)
                        _webView2.CoreWebView2.PostWebMessageAsString(json);
                });
                return true;
            }
            catch (ObjectDisposedException) { return false; }
            catch (InvalidOperationException) { return false; }
        }

        private void OnConnectionClosed()
        {
            PostOutputToTerminal("\r\n\x1b[33mConnection closed.\x1b[0m\r\n");
            InvokeOnUiThread(() => Event_Disconnected(this, "SSH connection closed", null));
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
                    Runtime.MessageCollector.AddExceptionStackTrace("SshTerminal: Write failed", ex);
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
            catch (ObjectDisposedException) { /* shutting down */ }
            catch (InvalidOperationException) { /* shutting down */ }
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
                        _ = ConnectSshAsync();
                        break;

                    case "input":
                        WriteToShell(root.GetProperty("data").GetString());
                        break;

                    case "resize":
                        _termCols = root.GetProperty("cols").GetUInt32();
                        _termRows = root.GetProperty("rows").GetUInt32();
                        if (_shellStream != null && _sshConnected)
                        {
                            try { _shellStream.ChangeWindowSize(_termCols, _termRows, 0, 0); }
                            catch (Exception ex)
                            {
                                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                    $"SshTerminal: Resize failed: {ex.Message}", true);
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("SshTerminal: Message handling error", ex);
            }
        }

        #endregion

        #region Focus

        public override void Focus()
        {
            try
            {
                _webView2?.Focus();
                if (_webView2?.CoreWebView2 != null)
                {
                    string json = JsonSerializer.Serialize(new { type = "focus" });
                    _webView2.CoreWebView2.PostWebMessageAsString(json);
                }
            }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
        }

        #endregion

        #region Close & Cleanup

        public override void Close()
        {
            try
            {
                _readCts?.Cancel();
                _readCts?.Dispose();
                _readCts = null;

                _sshConnected = false;

                _shellStream?.Dispose();
                _shellStream = null;

                if (_sshClient?.IsConnected == true)
                    _sshClient.Disconnect();
                _sshClient?.Dispose();
                _sshClient = null;

                if (_initTask != null && !_initTask.IsCompleted)
                    _initTask.ContinueWith(_ => CleanupWebView2(), TaskScheduler.Default);
                else
                    CleanupWebView2();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("SshTerminal: Close error", ex);
            }

            base.Close();
        }

        private void CleanupWebView2()
        {
            try
            {
                _webView2Environment = null;
                TryDeleteTempDirectory(_userDataFolder, "mRemoteNG_WebView2_SSH");
                TryDeleteTempDirectory(_resourceFolder, "mRemoteNG_xterm");
                _userDataFolder = null;
                _resourceFolder = null;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                    $"SshTerminal: Cleanup warning: {ex.Message}", true);
            }
        }

        private static void TryDeleteTempDirectory(string directoryPath, string expectedMarker)
        {
            if (string.IsNullOrEmpty(directoryPath) || !Directory.Exists(directoryPath))
                return;

            try
            {
                string tempPath = Path.GetFullPath(Path.GetTempPath());
                string fullPath = Path.GetFullPath(directoryPath);
                if (fullPath.StartsWith(tempPath, StringComparison.OrdinalIgnoreCase)
                    && fullPath.Contains(expectedMarker, StringComparison.OrdinalIgnoreCase))
                {
                    Directory.Delete(directoryPath, true);
                }
            }
            catch { /* best effort cleanup */ }
        }

        #endregion

        #region Resource Extraction

        private void ExtractResources()
        {
            Directory.CreateDirectory(_resourceFolder);

            string[] resourceNames = { "xterm-terminal.html", "xterm.min.js", "xterm.css", "addon-fit.min.js" };

            var assembly = Assembly.GetExecutingAssembly();

            foreach (var fileName in resourceNames)
            {
                string targetPath = Path.Combine(_resourceFolder, fileName);

                // Always overwrite in per-session folder (fresh extraction each session)
                string resourceName = FindResourceName(assembly, fileName);
                if (resourceName == null)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                        $"SshTerminal: Embedded resource not found: {fileName}");
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
            string searchName = fileName.Replace("-", "_").Replace("/", ".");
            return assembly.GetManifestResourceNames().FirstOrDefault(name =>
                name.EndsWith(searchName, StringComparison.OrdinalIgnoreCase)
                || name.EndsWith(fileName, StringComparison.OrdinalIgnoreCase)
                || name.EndsWith(fileName.Replace("-", "_"), StringComparison.OrdinalIgnoreCase));
        }

        #endregion
    }
}
