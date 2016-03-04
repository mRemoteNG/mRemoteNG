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
using System.Threading;
using mRemoteNG.My;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App;


namespace mRemoteNG.UI.Window
{
	public partial class Sessions : Base
	{
        #region Private Fields
		private Thread _getSessionsThread;
		private bool _retrieved = false;
        #endregion
				
        #region Public Methods
		public Sessions(DockContent panel)
		{
			WindowType = Type.Sessions;
			DockPnl = panel;
			InitializeComponent();
		}
				
		public void GetSessions(bool Auto = false)
		{
			ClearList();
			if (Auto)
			{
				_retrieved = false;
				if (!Settings.AutomaticallyGetSessionInfo)
				{
					return ;
				}
			}
					
			try
			{
				mRemoteNG.Connection.Info connectionInfo = mRemoteNG.Tree.Node.SelectedNode.Tag as mRemoteNG.Connection.Info;
				if (connectionInfo == null)
				{
					return ;
				}
						
				if (!(connectionInfo.Protocol == mRemoteNG.Connection.Protocol.Protocols.RDP | connectionInfo.Protocol == mRemoteNG.Connection.Protocol.Protocols.ICA))
				{
					return ;
				}
						
				BackgroundData data = new BackgroundData();
				data.Hostname = connectionInfo.Hostname;
				data.Username = connectionInfo.Username;
				data.Password = connectionInfo.Password;
				data.Domain = connectionInfo.Domain;
						
				if (Settings.EmptyCredentials == "custom")
				{
					if (string.IsNullOrEmpty(data.Username))
					{
						data.Username = Settings.DefaultUsername;
					}
							
					if (string.IsNullOrEmpty(data.Password))
					{
						data.Password = Security.Crypt.Decrypt(Settings.DefaultPassword, App.Info.General.EncryptionKey);
					}
							
					if (string.IsNullOrEmpty(data.Domain))
					{
						data.Domain = Settings.DefaultDomain;
					}
				}
						
				if (_getSessionsThread != null)
				{
					if (_getSessionsThread.IsAlive)
					{
						_getSessionsThread.Abort();
					}
				}
				_getSessionsThread = new Thread(new System.Threading.ThreadStart(GetSessionsBackground));
				_getSessionsThread.SetApartmentState(ApartmentState.STA);
				_getSessionsThread.IsBackground = true;
				_getSessionsThread.Start(data);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "UI.Window.Sessions.GetSessions() failed." + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		public void KillSession()
		{
			if (sessionList.SelectedItems.Count == 0)
			{
				return ;
			}
					
			mRemoteNG.Connection.Info connectionInfo = mRemoteNG.Tree.Node.SelectedNode.Tag as mRemoteNG.Connection.Info;
			if (connectionInfo == null)
			{
				return ;
			}
					
			if (!(connectionInfo.Protocol == mRemoteNG.Connection.Protocol.Protocols.RDP))
			{
				return ;
			}
					
			foreach (ListViewItem lvItem in sessionList.SelectedItems)
			{
				KillSession(connectionInfo.Hostname, connectionInfo.Username, connectionInfo.Password, connectionInfo.Domain, System.Convert.ToString(lvItem.Tag));
			}
		}
				
		public void KillSession(string hostname, string username, string password, string domain, string sessionId)
		{
			try
			{
				BackgroundData data = new BackgroundData();
				data.Hostname = hostname;
				data.Username = username;
				data.Password = password;
				data.Domain = domain;
				data.SessionId = long.Parse(sessionId);
						
				Thread thread = new Thread(new System.Threading.ThreadStart(KillSessionBackground));
				thread.SetApartmentState(ApartmentState.STA);
				thread.IsBackground = true;
				thread.Start(data);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "UI.Window.Sessions.KillSession() failed." + Constants.vbNewLine + ex.Message, true);
			}
		}
        #endregion
				
        #region Private Methods
        #region Form Stuff
		public void Sessions_Load(object sender, EventArgs e)
		{
			ApplyLanguage();
		}
				
		private void ApplyLanguage()
		{
			TabText = Language.strMenuSessions;
			Text = Language.strMenuSessions;
			sessionActivityColumn.Text = Language.strActivity;
			sessionMenuLogoff.Text = Language.strLogOff;
			sessionMenuRetrieve.Text = Language.strMenuSessionRetrieve;
			sessionTypeColumn.Text = Language.strType;
			sessionUsernameColumn.Text = Language.strColumnUsername;
		}
        #endregion
				
		private void GetSessionsBackground(object dataObject)
		{
			BackgroundData data = dataObject as BackgroundData;
			if (data == null)
			{
				return ;
			}
					
			Security.Impersonator impersonator = new Security.Impersonator();
			mRemoteNG.Connection.Protocol.RDP.TerminalSessions terminalSessions = new mRemoteNG.Connection.Protocol.RDP.TerminalSessions();
			long serverHandle = 0;
			try
			{
				impersonator.StartImpersonation(data.Domain, data.Username, data.Password);
						
				serverHandle = terminalSessions.OpenConnection(data.Hostname);
				if (serverHandle == 0)
				{
					return ;
				}
						
				GetSessions(terminalSessions, serverHandle);
						
				_retrieved = true;
			}
			catch (ThreadAbortException)
			{
						
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strSessionGetFailed + Constants.vbNewLine + ex.Message, true);
			}
			finally
			{
				impersonator.StopImpersonation();
				if (!(serverHandle == 0))
				{
					terminalSessions.CloseConnection(serverHandle);
				}
			}
		}
				
		// Get sessions from an already impersonated and connected TerminalSessions object
		private void GetSessions(mRemoteNG.Protocol.Protocol.RDP.TerminalSessions terminalSessions, long serverHandle)
		{
			mRemoteNG.Connection.Protocol.RDP.SessionsCollection rdpSessions = terminalSessions.GetSessions(serverHandle);
			foreach (mRemoteNG.Connection.Protocol.RDP.Session session in rdpSessions)
			{
				ListViewItem item = new ListViewItem();
				item.Tag = session.SessionId;
				item.Text = session.SessionUser;
				item.SubItems.Add(session.SessionState);
				item.SubItems.Add(session.SessionName.Replace(Constants.vbNewLine, ""));
				AddToList(item);
			}
		}
				
		private void KillSessionBackground(object dataObject)
		{
			BackgroundData data = dataObject as BackgroundData;
			if (data == null)
			{
				return ;
			}
					
			Security.Impersonator impersonator = new Security.Impersonator();
			mRemoteNG.Connection.Protocol.RDP.TerminalSessions terminalSessions = new mRemoteNG.Connection.Protocol.RDP.TerminalSessions();
			long serverHandle = 0;
			try
			{
				if (string.IsNullOrEmpty(data.Username) || string.IsNullOrEmpty(data.Password))
				{
					return ;
				}
						
				impersonator.StartImpersonation(data.Domain, data.Username, data.Password);
						
				serverHandle = terminalSessions.OpenConnection(data.Hostname);
				if (!(serverHandle == 0))
				{
					terminalSessions.KillSession(serverHandle, data.SessionId);
				}
						
				ClearList();
				GetSessions(terminalSessions, serverHandle);
						
				_retrieved = true;
			}
			catch (ThreadAbortException)
			{
						
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strSessionKillFailed + Constants.vbNewLine + ex.Message, true);
			}
			finally
			{
				impersonator.StopImpersonation();
				if (!(serverHandle == 0))
				{
					terminalSessions.CloseConnection(serverHandle);
				}
			}
		}
				
		delegate void AddToListCallback(ListViewItem item);
		private void AddToList(ListViewItem item)
		{
			if (sessionList.InvokeRequired)
			{
				AddToListCallback callback = new AddToListCallback(AddToList);
				sessionList.Invoke(callback, new object[] {item});
			}
			else
			{
				sessionList.Items.Add(item);
			}
		}
				
		delegate void ClearListCallback();
		private void ClearList()
		{
			if (sessionList.InvokeRequired)
			{
				ClearListCallback callback = new ClearListCallback(ClearList);
				sessionList.Invoke(callback);
			}
			else
			{
				sessionList.Items.Clear();
			}
		}
				
		private void menuSession_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			mRemoteNG.Connection.Info connectionInfo = default(mRemoteNG.Connection.Info);
			if (mRemoteNG.Tree.Node.TreeView == null || mRemoteNG.Tree.Node.SelectedNode == null)
			{
				connectionInfo = null;
			}
			else
			{
				connectionInfo = mRemoteNG.Tree.Node.SelectedNode.Tag as mRemoteNG.Connection.Info;
			}
					
			if (connectionInfo == null)
			{
				sessionMenuLogoff.Enabled = false;
				sessionMenuRetrieve.Enabled = false;
				sessionMenuRetrieve.Text = Language.strMenuSessionRetrieve;
				return ;
			}
					
			if (connectionInfo.Protocol == mRemoteNG.Connection.Protocol.Protocols.RDP & sessionList.SelectedItems.Count > 0)
			{
				sessionMenuLogoff.Enabled = true;
			}
			else
			{
				sessionMenuLogoff.Enabled = false;
			}
					
			if (connectionInfo.Protocol == mRemoteNG.Connection.Protocol.Protocols.RDP | connectionInfo.Protocol == mRemoteNG.Connection.Protocol.Protocols.ICA)
			{
				sessionMenuRetrieve.Enabled = true;
			}
			else
			{
				sessionMenuRetrieve.Enabled = false;
			}
					
			if (!_retrieved)
			{
				sessionMenuRetrieve.Text = Language.strMenuSessionRetrieve;
			}
			else
			{
				sessionMenuRetrieve.Text = Language.strRefresh;
			}
		}
				
		public void sessionMenuRetrieve_Click(System.Object sender, EventArgs e)
		{
			GetSessions();
		}
				
		public void sessionMenuLogoff_Click(System.Object sender, EventArgs e)
		{
			KillSession();
		}
        #endregion
				
        #region Private Classes
		private class BackgroundData
		{
			public string Hostname;
			public string Username;
			public string Password;
			public string Domain;
			public long SessionId;
		}
        #endregion
	}
}
