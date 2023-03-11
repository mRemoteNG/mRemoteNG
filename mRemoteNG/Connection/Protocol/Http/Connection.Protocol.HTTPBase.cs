using System;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using mRemoteNG.Tools;
using mRemoteNG.App;
using mRemoteNG.UI.Tabs;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.Connection.Protocol.Http
{
    [SupportedOSPlatform("windows")]
    public class HTTPBase : ProtocolBase
    {
        #region Private Properties

        private Control _wBrowser;
        private string _tabTitle;
        protected string httpOrS;
        protected int defaultPort;

        #endregion

        #region Public Methods

        protected HTTPBase(RenderingEngine renderingEngine)
        {
            try
            {
                if (renderingEngine == RenderingEngine.EdgeChromium)
                {
                    Control = new WebView2()
                    {
                        Dock = DockStyle.Fill,
                    };
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
                    var edge = (WebView2)_wBrowser;
                    
                    edge.CoreWebView2InitializationCompleted += Edge_CoreWebView2InitializationCompleted;
                }
                else
                {
                    var objWebBrowser = (WebBrowser)_wBrowser;
                    objWebBrowser.ScrollBarsEnabled = true;

                    // http://stackoverflow.com/questions/4655662/how-to-ignore-script-errors-in-webbrowser
                    objWebBrowser.ScriptErrorsSuppressed = true;

                    objWebBrowser.Navigated += WBrowser_Navigated;
                    objWebBrowser.DocumentTitleChanged += WBrowser_DocumentTitleChanged;
                }

                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.HttpSetPropsFailed, ex);
                return false;
            }
        }

        public override bool Connect()
        {
            try
            {
                if (InterfaceControl.Info.RenderingEngine == RenderingEngine.EdgeChromium)
                {
                    ((WebView2)_wBrowser).Source = new Uri(GetUrl());
                }
                else
                {
                    ((WebBrowser)_wBrowser).Navigate(GetUrl());
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

        #endregion

        #region Private Methods

        private string GetUrl()
        {
            try
            {
                var strHost = InterfaceControl.Info.Hostname;

                if (InterfaceControl.Info.Port != defaultPort)
                {
                    if (strHost.EndsWith("/"))
                        strHost = strHost.Substring(0, strHost.Length - 1);

                    if (strHost.Contains(httpOrS + "://") == false)
                        strHost = httpOrS + "://" + strHost;

                    strHost = strHost + ":" + InterfaceControl.Info.Port;
                }
                else
                {
                    if (strHost.Contains(httpOrS + "://") == false)
                        strHost = httpOrS + "://" + strHost;
                }

                return strHost;
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
            if (!(_wBrowser is WebBrowser objWebBrowser)) return;

            // This can only be set once the WebBrowser control is shown, it will throw a COM exception otherwise.
            objWebBrowser.AllowWebBrowserDrop = false;

            objWebBrowser.Navigated -= WBrowser_Navigated;
        }

        private void WBrowser_DocumentTitleChanged(object sender, EventArgs e)
        {
            try
            {
                if (!(InterfaceControl.Parent is ConnectionTab tabP)) return;
                string shortTitle;
                if (((WebBrowser)_wBrowser).DocumentTitle.Length >= 15)
                {
                    shortTitle = ((WebBrowser)_wBrowser).DocumentTitle.Substring(0, 10) + "...";
                }
                else
                {
                    shortTitle = ((WebBrowser)_wBrowser).DocumentTitle;
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

        #region Enums

        public enum RenderingEngine
        {
            [LocalizedAttributes.LocalizedDescription(nameof(Language.HttpInternetExplorer))]
            IE = 1,

            [LocalizedAttributes.LocalizedDescription(nameof(Language.HttpCEF))]
            EdgeChromium = 2
        }

        #endregion
    }
}
