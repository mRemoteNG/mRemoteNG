using System;
using System.IO;
using System.Windows.Forms;
using Gecko;
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

        #endregion

        #region Public Methods

        protected HTTPBase(RenderingEngine RenderingEngine)
        {
            try
            {
                if (RenderingEngine == RenderingEngine.Gecko)
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
                if (InterfaceControl.Parent is ConnectionTab objConnectionTab) tabTitle = objConnectionTab.TabText;
            }
            catch (Exception)
            {
                tabTitle = "";
            }

            try
            {
                wBrowser = Control;

                if (InterfaceControl.Info.RenderingEngine == RenderingEngine.Gecko)
                {
                    var GeckoBrowser = (GeckoWebBrowser)wBrowser;
                    if (GeckoBrowser != null)
                    {
                        GeckoBrowser.DocumentTitleChanged += geckoBrowser_DocumentTitleChanged;
                        LauncherDialog.Download += geckoBrowser_LauncherDialog_Download;
                        GeckoBrowser.NSSError += CertEvent.GeckoBrowser_NSSError;
                    }
                    else
                    {
                        throw new Exception("Failed to initialize Gecko Rendering Engine.");
                    }
                }
                else
                {
                    var objWebBrowser = (WebBrowser)wBrowser;
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
                    {
                        strHost = strHost.Substring(0, strHost.Length - 1);
                    }

                    if (strHost.Contains(httpOrS + "://") == false)
                    {
                        strHost = httpOrS + "://" + strHost;
                    }

                    if (InterfaceControl.Info.RenderingEngine == RenderingEngine.Gecko)
                    {
                        ((GeckoWebBrowser)wBrowser).Navigate(strHost + ":" + InterfaceControl.Info.Port);
                    }
                    else
                    {
                        ((WebBrowser)wBrowser).Navigate(strHost + ":" + InterfaceControl.Info.Port);
                    }
                }
                else
                {
                    if (strHost.Contains(httpOrS + "://") == false)
                    {
                        strHost = httpOrS + "://" + strHost;
                    }

                    if (InterfaceControl.Info.RenderingEngine == RenderingEngine.Gecko)
                    {
                        ((GeckoWebBrowser)wBrowser).Navigate(strHost);
                    }
                    else
                    {
                        ((WebBrowser)wBrowser).Navigate(strHost);
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

                if (InterfaceControl.Info.RenderingEngine == RenderingEngine.Gecko)
                {
                    if (((GeckoWebBrowser)wBrowser).DocumentTitle.Length >= 15)
                    {
                        shortTitle = ((GeckoWebBrowser)wBrowser).DocumentTitle.Substring(0, 10) + "...";
                    }
                    else
                    {
                        shortTitle = ((GeckoWebBrowser)wBrowser).DocumentTitle;
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
                    if (((GeckoWebBrowser)wBrowser).DocumentTitle.Length >= 15)
                    {
                        shortTitle = ((GeckoWebBrowser)wBrowser).DocumentTitle.Substring(0, 10) + "...";
                    }
                    else
                    {
                        shortTitle = ((GeckoWebBrowser)wBrowser).DocumentTitle;
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
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strHttpDocumentTileChangeFailed, ex);
            }
        }

        private void geckoBrowser_LauncherDialog_Download(object sender, Gecko.LauncherDialogEvent e)
        {
            nsILocalFile objTarget = Xpcom.CreateInstance<nsILocalFile>("@mozilla.org/file/local;1");

            using (nsAString tmp = new nsAString(@Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\temp.tmp"))
            {
                objTarget.InitWithPath(tmp);
            }

            //Save file dialog
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = e.Filename;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    nsIURI source = IOService.CreateNsIUri(e.Url);
                    nsIURI dest = IOService.CreateNsIUri(new Uri(@saveFileDialog1.FileName).AbsoluteUri);
                    nsAStringBase t = (nsAStringBase)new nsAString(System.IO.Path.GetFileName(@saveFileDialog1.FileName));

                    nsIWebBrowserPersist persist = Xpcom.CreateInstance<nsIWebBrowserPersist>("@mozilla.org/embedding/browser/nsWebBrowserPersist;1");

                    nsITransfer nst = Xpcom.CreateInstance<nsITransfer>("@mozilla.org/transfer;1");
                    nst.Init(source, dest, t, e.Mime, 0, null, persist, false);

                    if (nst != null)
                    {
                        persist.SetPersistFlagsAttribute(2 | 32 | 16384);
                        persist.SetProgressListenerAttribute((nsIWebProgressListener)nst);
                        persist.SaveURI(source, null, null, (uint)Gecko.nsIHttpChannelConsts.REFERRER_POLICY_NO_REFERRER, null, null, (nsISupports)dest, null);
                    }

                    myStream.Close();
                }
            }
        }

        #endregion

        #region Enums

        public enum RenderingEngine
        {
            [LocalizedAttributes.LocalizedDescription("strHttpInternetExplorer")]
            IE = 1,

            [LocalizedAttributes.LocalizedDescription("strHttpGecko")]
            Gecko = 2
        }

        #endregion
    }
}
