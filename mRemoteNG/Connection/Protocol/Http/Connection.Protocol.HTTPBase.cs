using System;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
using mRemoteNG.Tools;
using mRemoteNG.App;
using mRemoteNG.UI.Tabs;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;
using System.Windows.Forms.VisualStyles;
using System.IO;
using System.Threading.Tasks;


namespace mRemoteNG.Connection.Protocol.Http
{
    [SupportedOSPlatform("windows")]
    public class HTTPBase : ProtocolBase
    {
        #region Private Properties

        private Control? _wBrowser;
        private string _tabTitle = string.Empty;
        protected string httpOrS = string.Empty;
        protected int defaultPort;
        private string _userDataFolder = string.Empty;
        private CoreWebView2Environment? _webView2Environment;
        private Task? _webView2InitializationTask;
        private ToolStrip? _navigationBar;
        private ToolStripTextBox? _urlBox;

        #endregion

        #region Public Methods

        protected HTTPBase(RenderingEngine renderingEngine)
        {
            try
            {
                if (renderingEngine == RenderingEngine.EdgeChromium)
                {
                    Control = new Microsoft.Web.WebView2.WinForms.WebView2()
                    {
                        Dock = DockStyle.Fill,
                    };
                }
                else if (renderingEngine == RenderingEngine.ExternalBrowser)
                {
                    // No embedded control — URL will be opened in the OS default browser on Connect()
                }
                else
                {
                    Control = new WebBrowser();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.HttpConnectionFailed, ex);
            }
        }

        public override bool Initialize()
        {
            if (InterfaceControl.Info.RenderingEngine == RenderingEngine.ExternalBrowser)
                return base.Initialize();

            base.Initialize();

            try
            {
                if (InterfaceControl.Parent is ConnectionTab objConnectionTab) _tabTitle = objConnectionTab.TabText;
            }
            catch (Exception)
            {
                _tabTitle = "";
            }

            try
            {
                _wBrowser = Control;

                if (InterfaceControl.Info.RenderingEngine == RenderingEngine.EdgeChromium)
                {
                    if (_wBrowser is Microsoft.Web.WebView2.WinForms.WebView2 edge)
                    {
                        edge.CoreWebView2InitializationCompleted += Edge_CoreWebView2InitializationCompleted;

                        if (InterfaceControl.Info.UsePersistentBrowser)
                        {
                            _userDataFolder = Path.Combine(
                                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                "mRemoteNG",
                                "BrowserProfiles",
                                InterfaceControl.Info.ConstantID
                            );
                        }
                        else
                        {
                            _userDataFolder = Path.Combine(
                                Path.GetTempPath(),
                                "mRemoteNG_WebView2",
                                Guid.NewGuid().ToString()
                            );
                        }

                        // Initialize WebView2 with unique user data folder asynchronously
                        _webView2InitializationTask = InitializeWebView2Async(edge);
                    }
                }
                else
                {
                    if (_wBrowser is not WebBrowser objWebBrowser) return false;
                    objWebBrowser.ScrollBarsEnabled = true;

                    // http://stackoverflow.com/questions/4655662/how-to-ignore-script-errors-in-webbrowser
                    objWebBrowser.ScriptErrorsSuppressed = InterfaceControl.Info.ScriptErrorsSuppressed;

                    objWebBrowser.Navigated += WBrowser_Navigated;
                    objWebBrowser.DocumentTitleChanged += WBrowser_DocumentTitleChanged;
                }

                if (InterfaceControl.Info.ShowBrowserNavigationBar)
                    AddNavigationBar();

                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.HttpSetPropsFailed, ex);
                return false;
            }
        }

        private void AddNavigationBar()
        {
            if (_wBrowser == null) return;

            _navigationBar = new ToolStrip { GripStyle = ToolStripGripStyle.Hidden };

            var btnBack = new ToolStripButton("◄") { ToolTipText = "Back", DisplayStyle = ToolStripItemDisplayStyle.Text };
            var btnForward = new ToolStripButton("►") { ToolTipText = "Forward", DisplayStyle = ToolStripItemDisplayStyle.Text };
            var btnRefresh = new ToolStripButton("↻") { ToolTipText = "Refresh", DisplayStyle = ToolStripItemDisplayStyle.Text };
            _urlBox = new ToolStripTextBox { Width = 400, AutoSize = false };
            var btnGo = new ToolStripButton("Go") { DisplayStyle = ToolStripItemDisplayStyle.Text };

            btnBack.Click += (s, e) => NavigateBack();
            btnForward.Click += (s, e) => NavigateForward();
            btnRefresh.Click += (s, e) => NavigateRefresh();
            btnGo.Click += (s, e) => NavigateTo(_urlBox.Text);
            _urlBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    NavigateTo(_urlBox.Text);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            };

            _navigationBar.Items.Add(btnBack);
            _navigationBar.Items.Add(btnForward);
            _navigationBar.Items.Add(btnRefresh);
            _navigationBar.Items.Add(new ToolStripSeparator());
            _navigationBar.Items.Add(_urlBox);
            _navigationBar.Items.Add(btnGo);

            // Re-arrange: remove browser from InterfaceControl, add navbar (Top), re-add browser (Fill)
            InterfaceControl.Controls.Remove(_wBrowser);
            _navigationBar.Dock = DockStyle.Top;
            InterfaceControl.Controls.Add(_navigationBar);
            _wBrowser.Dock = DockStyle.Fill;
            InterfaceControl.Controls.Add(_wBrowser);

            // Wire navigation events for EdgeChromium
            if (_wBrowser is Microsoft.Web.WebView2.WinForms.WebView2 edge)
            {
                // Hook after CoreWebView2 is initialized
                edge.CoreWebView2InitializationCompleted += (s, e) =>
                {
                    if (!e.IsSuccess || edge.CoreWebView2 == null) return;
                    edge.CoreWebView2.NavigationCompleted += (src, args) =>
                    {
                        if (edge.InvokeRequired)
                            edge.Invoke(new Action(() => _urlBox!.Text = edge.Source?.ToString() ?? string.Empty));
                        else
                            _urlBox!.Text = edge.Source?.ToString() ?? string.Empty;
                    };
                };
            }
            else if (_wBrowser is WebBrowser wb)
            {
                wb.Navigated += (s, e) =>
                {
                    if (_urlBox != null)
                        _urlBox.Text = wb.Url?.ToString() ?? string.Empty;
                };
            }
        }

        private void NavigateBack()
        {
            if (_wBrowser is Microsoft.Web.WebView2.WinForms.WebView2 edge && edge.CoreWebView2 != null)
                edge.CoreWebView2.GoBack();
            else if (_wBrowser is WebBrowser wb && wb.CanGoBack)
                wb.GoBack();
        }

        private void NavigateForward()
        {
            if (_wBrowser is Microsoft.Web.WebView2.WinForms.WebView2 edge && edge.CoreWebView2 != null)
                edge.CoreWebView2.GoForward();
            else if (_wBrowser is WebBrowser wb && wb.CanGoForward)
                wb.GoForward();
        }

        private void NavigateRefresh()
        {
            if (_wBrowser is Microsoft.Web.WebView2.WinForms.WebView2 edge && edge.CoreWebView2 != null)
                edge.CoreWebView2.Reload();
            else if (_wBrowser is WebBrowser wb)
                wb.Refresh();
        }

        private void NavigateTo(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return;
            if (!url.Contains("://", StringComparison.Ordinal))
                url = "https://" + url;
            if (_wBrowser is Microsoft.Web.WebView2.WinForms.WebView2 edge && edge.CoreWebView2 != null)
                edge.CoreWebView2.Navigate(url);
            else if (_wBrowser is WebBrowser wb)
                wb.Navigate(url);
        }

        private async Task InitializeWebView2Async(Microsoft.Web.WebView2.WinForms.WebView2 webView2)
        {
            try
            {
                // Create the WebView2 environment with a unique user data folder
                _webView2Environment = await CoreWebView2Environment.CreateAsync(null, _userDataFolder);
                
                // Initialize the WebView2 control with the custom environment
                await webView2.EnsureCoreWebView2Async(_webView2Environment);
                
                // Prevent popups from opening in new windows
                webView2.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
                webView2.CoreWebView2.ServerCertificateErrorDetected += CoreWebView2_ServerCertificateErrorDetected;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.HttpSetPropsFailed, ex);
            }
        }

        public override bool Connect()
        {
            try
            {
                if (InterfaceControl.Info.RenderingEngine == RenderingEngine.ExternalBrowser)
                {
                    string url = GetUrl();
                    if (!string.IsNullOrEmpty(url))
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url) { UseShellExecute = true });
                    // Return false so ConnectionInitiator closes the empty tab (same pattern as IntegratedProgram external launch)
                    return false;
                }

                if (InterfaceControl.Info.RenderingEngine == RenderingEngine.EdgeChromium)
                {
                    if (_wBrowser is not Microsoft.Web.WebView2.WinForms.WebView2 webView2)
                        return false;

                    // Wait for WebView2 initialization to complete before connecting
                    if (_webView2InitializationTask is { IsCompleted: false })
                    {
                        // Schedule navigation after initialization completes
                        _webView2InitializationTask.ContinueWith(t =>
                        {
                            if (t.IsCompletedSuccessfully && webView2.CoreWebView2 != null)
                            {
                                // Use Invoke to ensure we're on the UI thread
                                if (webView2.InvokeRequired)
                                {
                                    webView2.Invoke(new Action(() => webView2.Source = new Uri(GetUrl())));
                                }
                                else
                                {
                                    webView2.Source = new Uri(GetUrl());
                                }
                            }
                            else if (t.IsFaulted)
                            {
                                Runtime.MessageCollector.AddExceptionStackTrace(Language.HttpConnectFailed, t.Exception);
                            }
                        },
                        // Use UI thread scheduler if available, otherwise use default
                        System.Threading.SynchronizationContext.Current != null
                            ? TaskScheduler.FromCurrentSynchronizationContext()
                            : TaskScheduler.Default);
                    }
                    else if (webView2.CoreWebView2 != null)
                    {
                        // WebView2 is already initialized, navigate immediately
                        webView2.Source = new Uri(GetUrl());
                    }
                }
                else
                {
                    if (_wBrowser is WebBrowser webBrowser)
                        webBrowser.Navigate(GetUrl());
                }

                base.Connect();
                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.HttpConnectFailed, ex);
                return false;
            }
        }

        private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            // Navigate to the popup URL in the current WebView2 rather than suppressing it.
            // This allows pop-up windows (e.g. login dialogs on management interfaces like Intel MEB) to work.
            e.Handled = true;
            if (sender is CoreWebView2 coreWebView2 && !string.IsNullOrEmpty(e.Uri))
            {
                coreWebView2.Navigate(e.Uri);
            }
        }

        private void CoreWebView2_ServerCertificateErrorDetected(object sender, CoreWebView2ServerCertificateErrorDetectedEventArgs e)
        {
            try
            {
                // Only bypass certificate errors for the configured connection host.
                if (!Uri.TryCreate(GetUrl(), UriKind.Absolute, out Uri? configuredUri) ||
                    !Uri.TryCreate(e.RequestUri, UriKind.Absolute, out Uri? requestUri))
                {
                    return;
                }

                if (string.Equals(configuredUri.Host, requestUri.Host, StringComparison.OrdinalIgnoreCase))
                {
                    e.Action = CoreWebView2ServerCertificateErrorAction.AlwaysAllow;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.HttpSetPropsFailed, ex);
            }
        }

        #endregion

        #region Private Methods

        private string GetUrl()
        {
            try
            {
                string rawHost = InterfaceControl.Info.Hostname?.Trim() ?? string.Empty;
                int explicitPort = InterfaceControl.Info.Port;
                string httpPath = InterfaceControl.Info.HttpPath?.Trim() ?? string.Empty;

                // Ensure hostname has a scheme so Uri.TryCreate can parse host and embedded port
                if (!rawHost.Contains("://", StringComparison.Ordinal))
                    rawHost = httpOrS + "://" + rawHost;

                if (!Uri.TryCreate(rawHost, UriKind.Absolute, out Uri? parsed))
                {
                    // Fallback for malformed hostnames
                    return httpOrS + "://" + (InterfaceControl.Info.Hostname?.Trim() ?? string.Empty);
                }

                var builder = new UriBuilder(parsed)
                {
                    Scheme = httpOrS  // Always enforce the correct scheme for this protocol
                };

                // Determine the port to include in the URL:
                // - Explicit port field (if non-default) always wins, preventing double-port issues
                // - Otherwise preserve any port embedded in the hostname field
                // - Otherwise omit port (let the browser use the protocol default)
                if (explicitPort != defaultPort)
                {
                    builder.Port = explicitPort;
                }
                else if (parsed.Port != defaultPort)
                {
                    builder.Port = parsed.Port;
                }
                else
                {
                    builder.Port = -1;
                }

                // Combine the path component from the hostname with the HttpPath setting
                string combinedPath = parsed.AbsolutePath;
                if (!string.IsNullOrEmpty(httpPath))
                {
                    if (!combinedPath.EndsWith('/') && !httpPath.StartsWith('/'))
                        combinedPath = combinedPath + "/" + httpPath;
                    else if (combinedPath.EndsWith('/') && httpPath.StartsWith('/'))
                        combinedPath = combinedPath + httpPath[1..];
                    else
                        combinedPath = combinedPath + httpPath;
                }
                builder.Path = combinedPath;

                return builder.Uri.ToString();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.HttpFailedUrlBuild, ex);
                return string.Empty;
            }
        }

        #endregion

        #region Events

        private void Edge_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            if (!e.IsSuccess)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.HttpFailedUrlBuild, e.InitializationException);
            }
        }

        private void WBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (_wBrowser is not WebBrowser objWebBrowser) return;

            // This can only be set once the WebBrowser control is shown, it will throw a COM exception otherwise.
            objWebBrowser.AllowWebBrowserDrop = false;

            objWebBrowser.Navigated -= WBrowser_Navigated;
        }

        private void WBrowser_DocumentTitleChanged(object sender, EventArgs e)
        {
            try
            {
                if (InterfaceControl.Parent is not ConnectionTab tabP) return;
                if (_wBrowser is not WebBrowser browser) return;
                string shortTitle;
                if (browser.DocumentTitle.Length >= 15)
                {
                    shortTitle = browser.DocumentTitle[..10] + "...";
                }
                else
                {
                    shortTitle = browser.DocumentTitle;
                }

                if (!string.IsNullOrEmpty(_tabTitle))
                {
                   tabP.TabText = _tabTitle + @" - " + shortTitle;
                }
                else
                {
                   tabP.TabText = shortTitle;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.HttpDocumentTileChangeFailed, ex);
            }
        }

        #endregion

        #region Cleanup

        public override void Close()
        {
            try
            {
                // Wait for initialization to complete before disposing (non-blocking approach)
                if (_webView2InitializationTask != null && !_webView2InitializationTask.IsCompleted)
                {
                    // Create a continuation to dispose after initialization completes
                    var cleanupTask = _webView2InitializationTask.ContinueWith(_ => 
                    {
                        DisposeWebView2Environment();
                    }, TaskScheduler.Default); // Use default scheduler to avoid UI thread issues
                    
                    // Give it a reasonable time to complete, but don't block indefinitely
                    // Using a background thread to avoid blocking UI thread
                    Task.Run(() =>
                    {
                        if (!cleanupTask.Wait(TimeSpan.FromSeconds(2)))
                        {
                            // Initialization is taking too long, log and continue
                            Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.WarningMsg, 
                                "WebView2 initialization did not complete in time during cleanup");
                        }
                    });
                }
                else
                {
                    DisposeWebView2Environment();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Error during HTTPBase cleanup", ex);
            }
            
            base.Close();
        }

        private void DisposeWebView2Environment()
        {
            try
            {
                // There is no Dispose method for CoreWebView2Environment, so just set to null
                _webView2Environment = null;
                
                // Clean up the temporary user data folder
                if (!string.IsNullOrEmpty(_userDataFolder) && Directory.Exists(_userDataFolder))
                {
                    try
                    {
                        // Verify the path is within the expected temp directory for safety
                        string tempPath = Path.GetTempPath();
                        string fullUserDataPath = Path.GetFullPath(_userDataFolder);
                        
                        if (fullUserDataPath.StartsWith(Path.GetFullPath(tempPath), StringComparison.OrdinalIgnoreCase) &&
                            fullUserDataPath.Contains("mRemoteNG_WebView2", StringComparison.Ordinal))
                        {
                            Directory.Delete(_userDataFolder, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log but don't throw - cleanup is best effort
                        Runtime.MessageCollector.AddExceptionStackTrace("Failed to clean up WebView2 user data folder", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Error disposing WebView2 environment", ex);
            }
        }

        #endregion

        #region Enums

        public enum RenderingEngine
        {
            [LocalizedAttributes.LocalizedDescription(nameof(Language.HttpInternetExplorer))]
            IE = 1,

            [LocalizedAttributes.LocalizedDescription(nameof(Language.HttpCEF))]
            EdgeChromium = 2,

            [LocalizedAttributes.LocalizedDescription(nameof(Language.HttpExternalBrowser))]
            ExternalBrowser = 3
        }

        #endregion
    }
}
