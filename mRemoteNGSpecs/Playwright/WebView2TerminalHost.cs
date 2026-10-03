using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace mRemoteNGSpecs.Playwright
{
    /// <summary>
    /// Hosts the shipped xterm.js terminal page in a real WebView2 control, the
    /// same way <c>SshTerminalBase</c> does: the page is served through a virtual
    /// host mapping and talks to the host over <c>postMessage</c>. Remote
    /// debugging is enabled so Playwright can attach over CDP and drive the page
    /// that is actually running inside WebView2, rather than a stubbed copy in a
    /// plain browser.
    /// </summary>
    internal sealed class WebView2TerminalHost : IDisposable
    {
        private const string VirtualHostName = "xterm.local";
        private const string PageFileName = "xterm-terminal.html";

        private readonly ConcurrentQueue<string> _messagesFromPage = new();
        private readonly string _resourceFolder;
        private readonly string _userDataFolder;

        private Form _form;
        private WebView2 _webView;
        private Thread _uiThread;

        private WebView2TerminalHost(string resourceFolder, string userDataFolder, int debuggingPort)
        {
            _resourceFolder = resourceFolder;
            _userDataFolder = userDataFolder;
            DebuggingPort = debuggingPort;
        }

        public int DebuggingPort { get; }

        public string PageUrl => $"https://{VirtualHostName}/{PageFileName}";

        /// <summary>Every message the page has posted to the host, oldest first.</summary>
        public string[] MessagesFromPage => _messagesFromPage.ToArray();

        /// <summary>
        /// Starts the WinForms UI thread, boots WebView2 and waits for the page to
        /// report that it is ready. Throws if WebView2 is unavailable, which the
        /// caller turns into an inconclusive result.
        /// </summary>
        public static async Task<WebView2TerminalHost> StartAsync(TimeSpan timeout)
        {
            string root = Path.Combine(Path.GetTempPath(), "mRemoteNGSpecs_WebView2_" + Guid.NewGuid().ToString("N"));
            string resourceFolder = Path.Combine(root, "resources");
            string userDataFolder = Path.Combine(root, "userdata");
            Directory.CreateDirectory(resourceFolder);
            Directory.CreateDirectory(userDataFolder);

            // The page is self-contained (CSS/JS inlined), exactly as the app serves it.
            File.WriteAllText(Path.Combine(resourceFolder, PageFileName),
                              XtermPageBuilder.BuildHostedHtml());

            var host = new WebView2TerminalHost(resourceFolder, userDataFolder, FindFreePort());
            await host.RunAsync(timeout);
            return host;
        }

        private async Task RunAsync(TimeSpan timeout)
        {
            var ready = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            _uiThread = new Thread(() =>
            {
                try
                {
                    _form = new Form { Width = 900, Height = 500, ShowInTaskbar = false, Text = "mRemoteNGSpecs terminal host" };
                    _webView = new WebView2 { Dock = DockStyle.Fill };
                    _form.Controls.Add(_webView);

                    _form.Shown += async (_, _) =>
                    {
                        try
                        {
                            var options = new CoreWebView2EnvironmentOptions
                            {
                                AdditionalBrowserArguments = $"--remote-debugging-port={DebuggingPort}"
                            };
                            var environment = await CoreWebView2Environment.CreateAsync(null, _userDataFolder, options);
                            await _webView.EnsureCoreWebView2Async(environment);

                            _webView.CoreWebView2.SetVirtualHostNameToFolderMapping(
                                VirtualHostName, _resourceFolder, CoreWebView2HostResourceAccessKind.Allow);
                            _webView.CoreWebView2.WebMessageReceived += (_, e) =>
                            {
                                string message = e.TryGetWebMessageAsString();
                                _messagesFromPage.Enqueue(message);
                                if (MessageType(message) == "ready")
                                    ready.TrySetResult(true);
                            };

                            _webView.CoreWebView2.Navigate(PageUrl);
                        }
                        catch (Exception ex)
                        {
                            ready.TrySetException(ex);
                        }
                    };

                    Application.Run(_form);
                }
                catch (Exception ex)
                {
                    ready.TrySetException(ex);
                }
            });

            _uiThread.SetApartmentState(ApartmentState.STA);
            _uiThread.IsBackground = true;
            _uiThread.Start();

            using var cancellation = new CancellationTokenSource(timeout);
            var completed = await Task.WhenAny(ready.Task, Task.Delay(Timeout.Infinite, cancellation.Token));
            if (completed != ready.Task)
                throw new TimeoutException($"The terminal page did not report 'ready' within {timeout.TotalSeconds:0}s.");

            await ready.Task;
        }

        /// <summary>Sends terminal output to the page exactly as the SSH session does.</summary>
        public void PostOutput(string text)
        {
            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
            string json = JsonSerializer.Serialize(new { type = "output", data = base64 });
            _webView.Invoke(() => _webView.CoreWebView2.PostWebMessageAsString(json));
        }

        public static string MessageType(string json)
        {
            try
            {
                using var document = JsonDocument.Parse(json);
                return document.RootElement.TryGetProperty("type", out var type) ? type.GetString() : null;
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private static int FindFreePort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

        public void Dispose()
        {
            try
            {
                _form?.Invoke(() =>
                {
                    _webView?.Dispose();
                    _form.Close();
                });
            }
            catch (Exception)
            {
                // The UI thread may already be gone; nothing to clean up there.
            }

            _uiThread?.Join(TimeSpan.FromSeconds(10));

            try
            {
                string root = Directory.GetParent(_resourceFolder)?.FullName;
                if (root != null && Directory.Exists(root))
                    Directory.Delete(root, recursive: true);
            }
            catch (IOException)
            {
                // WebView2 can still hold the user-data folder briefly; leaving a
                // temp directory behind is not worth failing a test over.
            }
        }
    }
}
