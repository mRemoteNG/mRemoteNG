using System.Collections.Generic;
using System;
using AxWFICALib;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using mRemoteNG.My;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App;


namespace mRemoteNG.UI.Window
{
	public partial class Announcement : Base
	{
        #region Public Methods
		public Announcement(DockContent panel)
		{
			WindowType = Type.Announcement;
			DockPnl = panel;
			InitializeComponent();
		}
        #endregion
				
        #region Private Fields
		private App.Update _appUpdate;
        #endregion
				
        #region Private Methods
		public void Announcement_Load(object sender, EventArgs e)
		{
			webBrowser.Navigated += webBrowser_Navigated;
					
			ApplyLanguage();
			CheckForAnnouncement();
		}
				
		private void ApplyLanguage()
		{
					
		}
				
		private void webBrowser_Navigated(object sender, System.Windows.Forms.WebBrowserNavigatedEventArgs e)
		{
			// This can only be set once the WebBrowser control is shown, it will throw a COM exception otherwise.
			webBrowser.AllowWebBrowserDrop = false;
					
			webBrowser.Navigated -= webBrowser_Navigated;
		}
				
		private void CheckForAnnouncement()
		{
			if (_appUpdate == null)
			{
				_appUpdate = new App.Update();
				_appUpdate.Load += _appUpdate.Update_Load;
			}
			else if (_appUpdate.IsGetAnnouncementInfoRunning)
			{
				return ;
			}
					
			_appUpdate.GetAnnouncementInfoCompletedEvent += GetAnnouncementInfoCompleted;
					
			_appUpdate.GetAnnouncementInfoAsync();
		}
				
		private void GetAnnouncementInfoCompleted(object sender, AsyncCompletedEventArgs e)
		{
			if (InvokeRequired)
			{
				AsyncCompletedEventHandler myDelegate = new AsyncCompletedEventHandler(GetAnnouncementInfoCompleted);
				Invoke(myDelegate, new object[] {sender, e});
				return ;
			}
					
			try
			{
				_appUpdate.GetAnnouncementInfoCompletedEvent -= GetAnnouncementInfoCompleted;
						
				if (e.Cancelled)
				{
					return ;
				}
				if (e.Error != null)
				{
					throw (e.Error);
				}
						
				webBrowser.Navigate(_appUpdate.CurrentAnnouncementInfo.Address);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(Language.strUpdateGetAnnouncementInfoFailed, ex);
			}
		}
        #endregion
	}
}
