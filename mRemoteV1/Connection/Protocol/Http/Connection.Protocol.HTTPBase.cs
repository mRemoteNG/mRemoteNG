using System;
using System.Windows.Forms;
using Gecko;
using mRemoteNG.Tools;
using mRemoteNG.App;
using TabPage = Crownwood.Magic.Controls.TabPage;

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
                    if(!Xpcom.IsInitialized)
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
			    var objTabPage = InterfaceControl.Parent as TabPage;
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
				    var GeckoBrowser = (GeckoWebBrowser) wBrowser;
                    if (GeckoBrowser != null)
                    {
                        GeckoBrowser.DocumentTitleChanged += geckoBrowser_DocumentTitleChanged;
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
			var objWebBrowser = wBrowser as WebBrowser;
			if (objWebBrowser == null) return;
					
			// This can only be set once the WebBrowser control is shown, it will throw a COM exception otherwise.
			objWebBrowser.AllowWebBrowserDrop = false;
					
			objWebBrowser.Navigated -= wBrowser_Navigated;
		}

        private void wBrowser_DocumentTitleChanged(object sender, EventArgs e)
		{
			try
			{
			    var tabP = InterfaceControl.Parent as TabPage;

			    if (tabP == null) return;
			    string shortTitle;
							
			    if (InterfaceControl.Info.RenderingEngine == RenderingEngine.Gecko)
			    {
			        if (((GeckoWebBrowser) wBrowser).DocumentTitle.Length >= 15)
			        {
			            shortTitle = ((GeckoWebBrowser) wBrowser).DocumentTitle.Substring(0, 10) + "...";
			        }
			        else
			        {
			            shortTitle = ((GeckoWebBrowser) wBrowser).DocumentTitle;
			        }
			    }
			    else
			    {
			        if (((WebBrowser) wBrowser).DocumentTitle.Length >= 15)
			        {
			            shortTitle = ((WebBrowser) wBrowser).DocumentTitle.Substring(0, 10) + "...";
			        }
			        else
			        {
			            shortTitle = ((WebBrowser) wBrowser).DocumentTitle;
			        }
			    }
							
			    if (!string.IsNullOrEmpty(tabTitle))
			    {
			        tabP.Title = tabTitle + @" - " + shortTitle;
			    }
			    else
			    {
			        tabP.Title = shortTitle;
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
                var tabP = InterfaceControl.Parent as TabPage;

                if (tabP == null) return;
                string shortTitle;

                if (InterfaceControl.Info.RenderingEngine == RenderingEngine.Gecko)
                {
                    if (((GeckoWebBrowser)wBrowser).DocumentTitle.Length >= 15)
                    {
                        shortTitle = ((GeckoWebBrowser) wBrowser).DocumentTitle.Substring(0, 10) + "...";
                    }
                    else
                    {
                        shortTitle = ((GeckoWebBrowser) wBrowser).DocumentTitle;
                    }
                }
                else
                {
                    if (((WebBrowser) wBrowser).DocumentTitle.Length >= 15)
                    {
                        shortTitle = ((WebBrowser) wBrowser).DocumentTitle.Substring(0, 10) + "...";
                    }
                    else
                    {
                        shortTitle = ((WebBrowser) wBrowser).DocumentTitle;
                    }
                }

                if (!string.IsNullOrEmpty(tabTitle))
                {
                    tabP.Title = tabTitle + @" - " + shortTitle;
                }
                else
                {
                    tabP.Title = shortTitle;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strHttpDocumentTileChangeFailed, ex);
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
