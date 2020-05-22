using System;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using mRemoteNG.App;
using mRemoteNG.UI.Tabs;


namespace mRemoteNG.Connection.Protocol.Http
{
    public class HTTPBase : ProtocolBase
    {
        #region Private Properties

        private Control wBrowser;
        protected string httpOrS;
        protected int defaultPort;
        private string tabTitle;
        private bool browserInitialised = false;
        private bool connectCalled = false;

        #endregion

        #region Public Methods

        protected HTTPBase()
        {
            try
            {
                Control = new ChromiumWebBrowser("about:blank")
                {
                    Dock = DockStyle.Fill,
                };
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strHttpConnectionFailed, ex);
            }
        }

        public override bool Initialize()
        {
            base.Initialize();

            try
            {
                if (InterfaceControl.Parent is ConnectionTab objConnectionTab) tabTitle = objConnectionTab.TabText;
            }
            catch (Exception)
            {
                tabTitle = "";
            }

            try
            {
                wBrowser = Control;

                var CEFBrowser = (ChromiumWebBrowser)wBrowser;
                if (CEFBrowser != null)
                {
                    CEFBrowser.LoadingStateChanged += cefBrowser_LoadingStateChanged;
                    CEFBrowser.TitleChanged += wBrowser_DocumentTitleChanged;
                    CEFBrowser.DownloadHandler = new DownloadHandler();
                }
                else
                {
                    throw new Exception("Failed to initialize CEF Rendering Engine.");
                }


                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strHttpSetPropsFailed, ex);
                return false;
            }
        }

        public override bool Connect()
        {
            try
            {
                if (browserInitialised) ((ChromiumWebBrowser)wBrowser).Load(GetURL());
                
                base.Connect();
                connectCalled = true;
                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strHttpConnectFailed, ex);
                return false;
            }
        }

        #endregion

        #region Private Methods

        private string GetURL()
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
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strHttpFailedURLBuild, ex);
                return string.Empty;
            }
        }

        #endregion

        #region Events

        private void cefBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            browserInitialised = !e.IsLoading;
            if (browserInitialised)
            {
                // Unhook the loading state changes now, as navigation is done by the user on links in the control
                ((ChromiumWebBrowser)wBrowser).LoadingStateChanged -= cefBrowser_LoadingStateChanged;

                // If this Connection has already been asked to connect but the browser hadn't finished initalising
                // then the connect wouldn't have been allowed to take place, so now we can call it!
                if (connectCalled)
                {
                    Connect();
                }
            }
        }

        private void wBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (!(wBrowser is WebBrowser objWebBrowser)) return;

            // This can only be set once the WebBrowser control is shown, it will throw a COM exception otherwise.
            objWebBrowser.AllowWebBrowserDrop = false;

            objWebBrowser.Navigated -= wBrowser_Navigated;
        }

        private void wBrowser_DocumentTitleChanged(object sender, EventArgs e)
        {
            try
            {
                if (!(InterfaceControl.Parent is ConnectionTab tabP)) return;
                string shortTitle;

                
                {
                    if (((TitleChangedEventArgs)e).Title.Length >= 15)
                    {
                        shortTitle = ((TitleChangedEventArgs)e).Title.Substring(0, 10) + "...";
                    }
                    else
                    {
                        shortTitle = ((TitleChangedEventArgs)e).Title;
                    }
                }

                if (!string.IsNullOrEmpty(tabTitle))
                {
                   tabP.TabText = tabTitle + @" - " + shortTitle;
                }
                else
                {
                   tabP.TabText = shortTitle;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strHttpDocumentTileChangeFailed, ex);
            }
        }

        #endregion
    }
}
