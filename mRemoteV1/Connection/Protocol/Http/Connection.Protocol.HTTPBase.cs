using System;
using System.IO;
using System.Windows.Forms;
using Gecko;
using mRemoteNG.Tools;
using mRemoteNG.App;
using mRemoteNG.UI.Tabs;


namespace mRemoteNG.Connection.Protocol.Http
{
    public class HttpBase : ProtocolBase
    {
        #region Private Properties

        protected string HttpOrHttps;
        protected int DefaultPort;
        private Control _wBrowser;
        private string _tabTitle;

        #endregion

        #region Public Methods

        protected HttpBase(RenderingEngine renderingEngine)
        {
            try
            {
                if (renderingEngine == RenderingEngine.Gecko)
                {
                    if (!Xpcom.IsInitialized)
                        Xpcom.Initialize("Firefox");

                    Control = new GeckoWebBrowser();
                }
                else
                {
                    Control = new WebBrowser();
                }
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
                if (InterfaceControl.Parent is ConnectionTab objConnectionTab) _tabTitle = objConnectionTab.TabText;
            }
            catch (Exception)
            {
                _tabTitle = "";
            }

            try
            {
                _wBrowser = Control;

                if (InterfaceControl.Info.RenderingEngine == RenderingEngine.Gecko)
                {
                    var geckoBrowser = (GeckoWebBrowser)_wBrowser;
                    if (geckoBrowser != null)
                    {
                        geckoBrowser.DocumentTitleChanged += geckoBrowser_DocumentTitleChanged;
                        LauncherDialog.Download += geckoBrowser_LauncherDialog_Download;
                        geckoBrowser.NSSError += CertEvent.GeckoBrowser_NSSError;
                    }
                    else
                    {
                        throw new Exception("Failed to initialize Gecko Rendering Engine.");
                    }
                }
                else
                {
                    var objWebBrowser = (WebBrowser)_wBrowser;
                    objWebBrowser.ScrollBarsEnabled = true;

                    // http://stackoverflow.com/questions/4655662/how-to-ignore-script-errors-in-webbrowser
                    objWebBrowser.ScriptErrorsSuppressed = true;

                    objWebBrowser.Navigated += wBrowser_Navigated;
                    objWebBrowser.DocumentTitleChanged += wBrowser_DocumentTitleChanged;
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
                var strHost = InterfaceControl.Info.Hostname;

                if (InterfaceControl.Info.Port != DefaultPort)
                {
                    if (strHost.EndsWith("/"))
                    {
                        strHost = strHost.Substring(0, strHost.Length - 1);
                    }

                    if (strHost.Contains(HttpOrHttps + "://") == false)
                    {
                        strHost = HttpOrHttps + "://" + strHost;
                    }

                    if (InterfaceControl.Info.RenderingEngine == RenderingEngine.Gecko)
                    {
                        ((GeckoWebBrowser)_wBrowser).Navigate(strHost + ":" + InterfaceControl.Info.Port);
                    }
                    else
                    {
                        ((WebBrowser)_wBrowser).Navigate(strHost + ":" + InterfaceControl.Info.Port);
                    }
                }
                else
                {
                    if (strHost.Contains(HttpOrHttps + "://") == false)
                    {
                        strHost = HttpOrHttps + "://" + strHost;
                    }

                    if (InterfaceControl.Info.RenderingEngine == RenderingEngine.Gecko)
                    {
                        ((GeckoWebBrowser)_wBrowser).Navigate(strHost);
                    }
                    else
                    {
                        ((WebBrowser)_wBrowser).Navigate(strHost);
                    }
                }

                base.Connect();
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

        #endregion

        #region Events

        private void wBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (!(_wBrowser is WebBrowser objWebBrowser)) return;

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

                if (InterfaceControl.Info.RenderingEngine == RenderingEngine.Gecko)
                {
                    if (((GeckoWebBrowser)_wBrowser).DocumentTitle.Length >= 15)
                    {
                        shortTitle = ((GeckoWebBrowser)_wBrowser).DocumentTitle.Substring(0, 10) + "...";
                    }
                    else
                    {
                        shortTitle = ((GeckoWebBrowser)_wBrowser).DocumentTitle;
                    }
                }
                else
                {
                    if (((WebBrowser)_wBrowser).DocumentTitle.Length >= 15)
                    {
                        shortTitle = ((WebBrowser)_wBrowser).DocumentTitle.Substring(0, 10) + "...";
                    }
                    else
                    {
                        shortTitle = ((WebBrowser)_wBrowser).DocumentTitle;
                    }
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
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strHttpDocumentTileChangeFailed, ex);
            }
        }


        private void geckoBrowser_DocumentTitleChanged(object sender, EventArgs e)
        {
            try
            {
                if (!(InterfaceControl.Parent is ConnectionTab tabP)) return;
                string shortTitle;

                if (InterfaceControl.Info.RenderingEngine == RenderingEngine.Gecko)
                {
                    if (((GeckoWebBrowser)_wBrowser).DocumentTitle.Length >= 15)
                    {
                        shortTitle = ((GeckoWebBrowser)_wBrowser).DocumentTitle.Substring(0, 10) + "...";
                    }
                    else
                    {
                        shortTitle = ((GeckoWebBrowser)_wBrowser).DocumentTitle;
                    }
                }
                else
                {
                    if (((WebBrowser)_wBrowser).DocumentTitle.Length >= 15)
                    {
                        shortTitle = ((WebBrowser)_wBrowser).DocumentTitle.Substring(0, 10) + "...";
                    }
                    else
                    {
                        shortTitle = ((WebBrowser)_wBrowser).DocumentTitle;
                    }
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
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strHttpDocumentTileChangeFailed, ex);
            }
        }

        private void geckoBrowser_LauncherDialog_Download(object sender, Gecko.LauncherDialogEvent e)
        {
            var objTarget = Xpcom.CreateInstance<nsILocalFile>("@mozilla.org/file/local;1");
            using (var tmp = new nsAString(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\mremoteng.download"))
            {
                objTarget.InitWithPath(tmp);
            }

            //Save file dialog
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true,
                FileName = e.Filename
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var source = IOService.CreateNsIUri(e.Url);
                var dest = IOService.CreateNsIUri(new Uri(saveFileDialog.FileName).AbsoluteUri);
                var t = (nsAStringBase)new nsAString(Path.GetFileName(saveFileDialog.FileName));
                var persist = Xpcom.CreateInstance<nsIWebBrowserPersist>("@mozilla.org/embedding/browser/nsWebBrowserPersist;1");
                var nst = Xpcom.CreateInstance<nsITransfer>("@mozilla.org/transfer;1");

                nst.Init(source, dest, t, e.Mime, 0, null, persist, false);
                persist.SetPersistFlagsAttribute(2 | 32 | 16384);
                persist.SetProgressListenerAttribute(nst);
                persist.SaveURI(source, null, null, (uint)Gecko.nsIHttpChannelConsts.REFERRER_POLICY_NO_REFERRER, null, null, (nsISupports) dest, null);
            }

            saveFileDialog.Dispose();
        }

        #endregion

        #region Enums

        public enum RenderingEngine
        {
            [LocalizedAttributes.LocalizedDescription("strHttpInternetExplorer")]
            Ie = 1,

            [LocalizedAttributes.LocalizedDescription("strHttpGecko")]
            Gecko = 2
        }

        #endregion
    }
}
