using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.My;
using mRemoteNG.Tree;
using System;
using System.Threading;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.UI.Window
{
	public partial class SessionsWindow : BaseWindow
	{
        #region Private Fields
		private Thread _getSessionsThread;
		private bool _retrieved = false;
        #endregion
		
        #region Public Methods
		public SessionsWindow(DockContent panel)
		{
			WindowType = WindowType.Sessions;
			DockPnl = panel;
			InitializeComponent();
		}
				
		public void GetSessions(bool Auto = false)
		{
			ClearList();
			if (Auto)
			{
				_retrieved = false;
				if (!Settings.Default.AutomaticallyGetSessionInfo)
				{
					return ;
				}
			}
					
			try
			{
                ConnectionInfo connectionInfo = ConnectionTree.SelectedNode.Tag as ConnectionInfo;
				if (connectionInfo == null)
				{
					return ;
				}
						
				if (!(connectionInfo.Protocol == ProtocolType.RDP | connectionInfo.Protocol == ProtocolType.ICA))
				{
					return ;
				}
						
				BackgroundData data = new BackgroundData();
				data.Hostname = connectionInfo.Hostname;
				data.Username = connectionInfo.Username;
				data.Password = connectionInfo.Password;
				data.Domain = connectionInfo.Domain;
						
				if (Settings.Default.EmptyCredentials == "custom")
				{
					if (string.IsNullOrEmpty(data.Username))
					{
                        data.Username = Settings.Default.DefaultUsername;
					}
							
					if (string.IsNullOrEmpty(data.Password))
					{
                        data.Password = Security.Crypt.Decrypt(Settings.Default.DefaultPassword, App.Info.GeneralAppInfo.EncryptionKey);
					}
							
					if (string.IsNullOrEmpty(data.Domain))
					{
                        data.Domain = Settings.Default.DefaultDomain;
					}
				}
						
				if (_getSessionsThread != null)
				{
					if (_getSessionsThread.IsAlive)
					{
						_getSessionsThread.Abort();
					}
				}
                _getSessionsThread = new Thread(GetSessionsAsync);
				_getSessionsThread.SetApartmentState(ApartmentState.STA);
				_getSessionsThread.IsBackground = true;
				_getSessionsThread.Start(data);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "UI.Window.Sessions.GetSessions() failed." + Environment.NewLine + ex.Message, true);
			}
		}
				
		public void KillSession()
		{
			if (sessionList.SelectedItems.Count == 0)
			{
				return ;
			}

            ConnectionInfo connectionInfo = ConnectionTree.SelectedNode.Tag as ConnectionInfo;
			if (connectionInfo == null)
			{
				return ;
			}
					
			if (!(connectionInfo.Protocol == ProtocolType.RDP))
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
				data.SessionId = int.Parse(sessionId);
						
				Thread thread = new Thread(KillSessionAsync);
				thread.SetApartmentState(ApartmentState.STA);
				thread.IsBackground = true;
				thread.Start(data);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "UI.Window.Sessions.KillSession() failed." + Environment.NewLine + ex.Message, true);
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
				
		private void GetSessionsAsync(object dataObject)
		{
			BackgroundData data = dataObject as BackgroundData;
			if (data == null)
			{
				return ;
			}
					
			Security.Impersonator impersonator = new Security.Impersonator();
			ProtocolRDP.TerminalSessions terminalSessions = new ProtocolRDP.TerminalSessions();
			int serverHandle = 0;
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
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strSessionGetFailed + Environment.NewLine + ex.Message, true);
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
        private void GetSessions(ProtocolRDP.TerminalSessions terminalSessions, int serverHandle)
		{
			ProtocolRDP.SessionsCollection rdpSessions = terminalSessions.GetSessions(serverHandle);
			foreach (ProtocolRDP.Session session in rdpSessions)
			{
				ListViewItem item = new ListViewItem();
				item.Tag = session.SessionId;
				item.Text = session.SessionUser;
				item.SubItems.Add(session.SessionState);
				item.SubItems.Add(session.SessionName.Replace(Environment.NewLine, ""));
				AddToList(item);
			}
		}
				
		private void KillSessionAsync(object dataObject)
		{
			BackgroundData data = dataObject as BackgroundData;
			if (data == null)
			{
				return ;
			}
					
			Security.Impersonator impersonator = new Security.Impersonator();
			ProtocolRDP.TerminalSessions terminalSessions = new ProtocolRDP.TerminalSessions();
			int serverHandle = 0;
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
					terminalSessions.KillSession(
                        serverHandle, data.SessionId);
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
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strSessionKillFailed + Environment.NewLine + ex.Message, true);
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
			mRemoteNG.Connection.ConnectionInfo connectionInfo = default(mRemoteNG.Connection.ConnectionInfo);
            if (mRemoteNG.Tree.ConnectionTree.TreeView == null || mRemoteNG.Tree.ConnectionTree.SelectedNode == null)
			{
				connectionInfo = null;
			}
			else
			{
                connectionInfo = mRemoteNG.Tree.ConnectionTree.SelectedNode.Tag as ConnectionInfo;
			}
					
			if (connectionInfo == null)
			{
				sessionMenuLogoff.Enabled = false;
				sessionMenuRetrieve.Enabled = false;
				sessionMenuRetrieve.Text = Language.strMenuSessionRetrieve;
				return ;
			}
					
			if (connectionInfo.Protocol == mRemoteNG.Connection.Protocol.ProtocolType.RDP & sessionList.SelectedItems.Count > 0)
			{
				sessionMenuLogoff.Enabled = true;
			}
			else
			{
				sessionMenuLogoff.Enabled = false;
			}
					
			if (connectionInfo.Protocol == mRemoteNG.Connection.Protocol.ProtocolType.RDP | connectionInfo.Protocol == mRemoteNG.Connection.Protocol.ProtocolType.ICA)
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
			public int SessionId;
		}
        #endregion
	}
}