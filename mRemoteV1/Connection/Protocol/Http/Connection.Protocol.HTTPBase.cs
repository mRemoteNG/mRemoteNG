using System;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using mRemoteNG.Tools;
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

        protected HTTPBase(RenderingEngine RenderingEngine)
        {
            try
            {
                if (RenderingEngine == RenderingEngine.CEF)
                {
                    Control = new ChromiumWebBrowser("about:blank")
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
                if (InterfaceControl.Parent is ConnectionTab objConnectionTab) tabTitle = objConnectionTab.TabText;
            }
            catch (Exception)
            {
                tabTitle = "";
            }

            try
            {
                wBrowser = Control;

                if (InterfaceControl.Info.RenderingEngine == RenderingEngine.CEF)
                {
                    var CEFBrowser = (ChromiumWebBrowser)wBrowser;
                    if (CEFBrowser != null)
                    {
                        CEFBrowser.LoadingStateChanged += CefBrowser_LoadingStateChanged;
                        CEFBrowser.TitleChanged += WBrowser_DocumentTitleChanged;
                    }
                    else
                    {
                        throw new Exception("Failed to initialize CEF Rendering Engine.");
                    }
                }
                else
                {
                    var objWebBrowser = (WebBrowser)wBrowser;
                    objWebBrowser.ScrollBarsEnabled = true;

                    // http://stackoverflow.com/questions/4655662/how-to-ignore-script-errors-in-webbrowser
                    objWebBrowser.ScriptErrorsSuppressed = true;

                    objWebBrowser.Navigated += WBrowser_Navigated;
                    objWebBrowser.DocumentTitleChanged += WBrowser_DocumentTitleChanged;
                    browserInitialised = true;
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
                if (InterfaceControl.Info.RenderingEngine == RenderingEngine.CEF)
                {
                    if (browserInitialised)
                    {
                        ((ChromiumWebBrowser)wBrowser).Load(GetURL());
                    }
                }
                else
                {
                    ((WebBrowser)wBrowser).Navigate(GetURL());
                }

                base.Connect();
                connectCalled = true;
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

        private string GetURL()
        {
            try
            {
                var strHost = InterfaceControl.Info.Hostname;
                /* 
                 * Commenting out since this codes doesn't actually do anything at this time...
                 * Possibly related to MR-221 and/or MR-533 ????
                 * 
				string strAuth = "";

                if (((int)Force & (int)ConnectionInfo.Force.NoCredentials) != (int)ConnectionInfo.Force.NoCredentials && !string.IsNullOrEmpty(InterfaceControl.Info.Username) && !string.IsNullOrEmpty(InterfaceControl.Info.Password))
				{
					strAuth = "Authorization: Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(InterfaceControl.Info.Username + ":" + InterfaceControl.Info.Password)) + Environment.NewLine;
				}
				*/
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

        private void CefBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            browserInitialised = !e.IsLoading;
            if (browserInitialised)
            {
                // Unhook the loading state changes now, as navigation is done by the user on links in the control
                ((ChromiumWebBrowser)wBrowser).LoadingStateChanged -= CefBrowser_LoadingStateChanged;

                // If this Connection has already been asked to connect but the browser hadn't finished initalising
                // then the connect wouldn't have been allowed to take place, so now we can call it!
                if (connectCalled)
                {
                    Connect();
                }
            }
        }

        private void WBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (!(wBrowser is WebBrowser objWebBrowser)) return;

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

                if (InterfaceControl.Info.RenderingEngine == RenderingEngine.CEF)
                {
                    if (((TitleChangedEventArgs)e).Title.Length >= 15)
                    {
                        shortTitle = ((TitleChangedEventArgs)e).Title.Substring(0, 10) + "...";
                    }
                    else
                    {
                        shortTitle = ((CefSharp.TitleChangedEventArgs)e).Title;
                    }
                }
                else
                {
                    if (((WebBrowser)wBrowser).DocumentTitle.Length >= 15)
                    {
                        shortTitle = ((WebBrowser)wBrowser).DocumentTitle.Substring(0, 10) + "...";
                    }
                    else
                    {
                        shortTitle = ((WebBrowser)wBrowser).DocumentTitle;
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
                Runtime.MessageCollector.AddExceptionStackTrace(Language.HttpDocumentTileChangeFailed, ex);
            }
        }


        private void geckoBrowser_DocumentTitleChanged(object sender, EventArgs e)
        {
            try
            {
                if (!(InterfaceControl.Parent is ConnectionTab tabP)) return;
                string shortTitle;

                if (((WebBrowser)wBrowser).DocumentTitle.Length >= 15)
                {
                    shortTitle = ((WebBrowser)wBrowser).DocumentTitle.Substring(0, 10) + "...";
                }
                else
                {
                    shortTitle = ((WebBrowser)wBrowser).DocumentTitle;
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
            CEF = 2
        }

        #endregion
    }
}
