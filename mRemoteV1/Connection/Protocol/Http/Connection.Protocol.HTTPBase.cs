using System;
using System.Windows.Forms;
using mRemoteNG.Tools;
using mRemoteNG.App;
using TabPage = Crownwood.Magic.Controls.TabPage;

//using SHDocVw;

namespace mRemoteNG.Connection.Protocol.Http
{
	public class HTTPBase : ProtocolBase
	{
        #region Private Properties
		private Control wBrowser;
		public string httpOrS;
		public int defaultPort;
		private string tabTitle;
        #endregion
				
        #region Public Methods
		public HTTPBase(RenderingEngine RenderingEngine)
		{
			try
			{
				if (RenderingEngine == RenderingEngine.Gecko)
				{
                    Control = new MiniGeckoBrowser.MiniGeckoBrowser();
					((MiniGeckoBrowser.MiniGeckoBrowser)Control).XULrunnerPath = Convert.ToString(Settings.Default.XULRunnerPath);
				}
				else
				{
                    Control = new WebBrowser();
				}
						
				NewExtended();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strHttpConnectionFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		public virtual void NewExtended()
		{
		}
				
		public override bool Initialize()
		{
			base.Initialize();
					
			try
			{
			    TabPage objTabPage = InterfaceControl.Parent as TabPage;
			    if (objTabPage != null) tabTitle = objTabPage.Title;
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
					MiniGeckoBrowser.MiniGeckoBrowser objMiniGeckoBrowser = wBrowser as MiniGeckoBrowser.MiniGeckoBrowser;
				    if (objMiniGeckoBrowser != null)
				    {
				        objMiniGeckoBrowser.TitleChanged += geckoBrowser_DocumentTitleChanged;
				        objMiniGeckoBrowser.LastTabRemoved += wBrowser_LastTabRemoved;
				    }
				}
				else
				{
                    WebBrowser objWebBrowser = (WebBrowser)wBrowser;
                    //SHDocVw.WebBrowserClass objAxWebBrowser = (SHDocVw.WebBrowserClass)objWebBrowser.ActiveXInstance;
					objWebBrowser.ScrollBarsEnabled = true;
                    objWebBrowser.Navigated += wBrowser_Navigated;
					objWebBrowser.DocumentTitleChanged += wBrowser_DocumentTitleChanged;
				    //objWebBrowser.NewWindow3 += wBrowser_NewWindow3;
				    //objAxWebBrowser.NewWindow3 += wBrowser_NewWindow3;
				}
				
				return true;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strHttpSetPropsFailed + Environment.NewLine + ex.Message, true);
				return false;
			}
		}
				
		public override bool Connect()
		{
			try
			{
				string strHost = Convert.ToString(InterfaceControl.Info.Hostname);
				string strAuth = "";

                if (((int)Force & (int)ConnectionInfo.Force.NoCredentials) != (int)ConnectionInfo.Force.NoCredentials && !string.IsNullOrEmpty(InterfaceControl.Info.Username) && !string.IsNullOrEmpty(InterfaceControl.Info.Password))
				{
					strAuth = "Authorization: Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(InterfaceControl.Info.Username + ":" + InterfaceControl.Info.Password)) + Environment.NewLine;
				}
						
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
                        ((MiniGeckoBrowser.MiniGeckoBrowser)wBrowser).Navigate(strHost + ":" + InterfaceControl.Info.Port);
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
						((MiniGeckoBrowser.MiniGeckoBrowser)wBrowser).Navigate(strHost);
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
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strHttpConnectFailed + Environment.NewLine + ex.Message, true);
				return false;
			}
		}
        #endregion
				
        #region Private Methods
        #endregion
				
        #region Events
		private void wBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
		{
			WebBrowser objWebBrowser = wBrowser as WebBrowser;
			if (objWebBrowser == null)
			{
				return ;
			}
					
			// This can only be set once the WebBrowser control is shown, it will throw a COM exception otherwise.
			objWebBrowser.AllowWebBrowserDrop = false;
					
			objWebBrowser.Navigated -= wBrowser_Navigated;
		}

        private void wBrowser_NewWindow3(ref object ppDisp, ref bool Cancel, uint dwFlags, string bstrUrlContext, string bstrUrl)
		{
			if ((dwFlags & (long)NWMF.NWMF_OVERRIDEKEY) > 0)
			{
				Cancel = false;
			}
			else
			{
				Cancel = true;
			}
		}
				
		private void wBrowser_LastTabRemoved(object sender)
		{
            Close();
		}
				
		private void wBrowser_DocumentTitleChanged(object sender, EventArgs e)
		{
			try
			{
			    var tabP = InterfaceControl.Parent as TabPage;

			    if (tabP != null)
				{
					string shortTitle;
							
					if (InterfaceControl.Info.RenderingEngine == RenderingEngine.Gecko)
					{
						if (((MiniGeckoBrowser.MiniGeckoBrowser) wBrowser).Title.Length >= 30)
						{
							shortTitle = ((MiniGeckoBrowser.MiniGeckoBrowser) wBrowser).Title.Substring(0, 29) + " ...";
						}
						else
						{
							shortTitle = ((MiniGeckoBrowser.MiniGeckoBrowser) wBrowser).Title;
						}
					}
					else
					{
						if (((WebBrowser) wBrowser).DocumentTitle.Length >= 30)
						{
							shortTitle = ((WebBrowser) wBrowser).DocumentTitle.Substring(0, 29) + " ...";
						}
						else
						{
							shortTitle = ((WebBrowser) wBrowser).DocumentTitle;
						}
					}
							
					if (!string.IsNullOrEmpty(tabTitle))
					{
						tabP.Title = tabTitle + " - " + shortTitle;
					}
					else
					{
						tabP.Title = shortTitle;
					}
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, Language.strHttpDocumentTileChangeFailed + Environment.NewLine + ex.Message, true);
			}
		}


        private void geckoBrowser_DocumentTitleChanged(object sender, string e)
        {
            try
            {
                var tabP = InterfaceControl.Parent as TabPage;

                if (tabP != null)
                {
                    string shortTitle;

                    if (InterfaceControl.Info.RenderingEngine == RenderingEngine.Gecko)
                    {
                        if (((MiniGeckoBrowser.MiniGeckoBrowser) wBrowser).Title.Length >= 30)
                        {
                            shortTitle = ((MiniGeckoBrowser.MiniGeckoBrowser) wBrowser).Title.Substring(0, 29) + " ...";
                        }
                        else
                        {
                            shortTitle = ((MiniGeckoBrowser.MiniGeckoBrowser) wBrowser).Title;
                        }
                    }
                    else
                    {
                        if (((WebBrowser) wBrowser).DocumentTitle.Length >= 30)
                        {
                            shortTitle = ((WebBrowser) wBrowser).DocumentTitle.Substring(0, 29) + " ...";
                        }
                        else
                        {
                            shortTitle = ((WebBrowser) wBrowser).DocumentTitle;
                        }
                    }

                    if (!string.IsNullOrEmpty(tabTitle))
                    {
                        tabP.Title = tabTitle + " - " + shortTitle;
                    }
                    else
                    {
                        tabP.Title = shortTitle;
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, Language.strHttpDocumentTileChangeFailed + Environment.NewLine + ex.Message, true);
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
				
		private enum NWMF
		{
			// ReSharper disable InconsistentNaming
			NWMF_UNLOADING = 0x1,
			NWMF_USERINITED = 0x2,
			NWMF_FIRST = 0x4,
			NWMF_OVERRIDEKEY = 0x8,
			NWMF_SHOWHELP = 0x10,
			NWMF_HTMLDIALOG = 0x20,
			NWMF_FROMDIALOGCHILD = 0x40,
			NWMF_USERREQUESTED = 0x80,
			NWMF_USERALLOWED = 0x100,
			NWMF_FORCEWINDOW = 0x10000,
			NWMF_FORCETAB = 0x20000,
			NWMF_SUGGESTWINDOW = 0x40000,
			NWMF_SUGGESTTAB = 0x80000,
			NWMF_INACTIVETAB = 0x100000
			// ReSharper restore InconsistentNaming
		}
        #endregion
	}
}
