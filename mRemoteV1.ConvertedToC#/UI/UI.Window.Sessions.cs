using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using mRemoteNG.My;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App.Runtime;

namespace mRemoteNG.UI
{
	namespace Window
	{
		public partial class Sessions : Base
		{
			#region "Private Fields"
			private Thread _getSessionsThread;
				#endregion
			private bool _retrieved = false;

			#region "Public Methods"
			public Sessions(DockContent panel)
			{
				Load += Sessions_Load;
				WindowType = Type.Sessions;
				DockPnl = panel;
				InitializeComponent();
			}

			public void GetSessions(bool auto = false)
			{
				ClearList();
				if (auto) {
					_retrieved = false;
					if (!Settings.AutomaticallyGetSessionInfo)
						return;
				}

				try {
					mRemoteNG.Connection.Info connectionInfo = mRemoteNG.Tree.Node.SelectedNode.Tag as mRemoteNG.Connection.Info;
					if (connectionInfo == null)
						return;

					if (!(connectionInfo.Protocol == mRemoteNG.Connection.Protocol.Protocols.RDP | connectionInfo.Protocol == mRemoteNG.Connection.Protocol.Protocols.ICA))
						return;

					BackgroundData data = new BackgroundData();
					var _with1 = data;
					_with1.Hostname = connectionInfo.Hostname;
					_with1.Username = connectionInfo.Username;
					_with1.Password = connectionInfo.Password;
					_with1.Domain = connectionInfo.Domain;

					if (Settings.EmptyCredentials == "custom") {
						if (string.IsNullOrEmpty(_with1.Username)) {
							_with1.Username = Settings.DefaultUsername;
						}

						if (string.IsNullOrEmpty(_with1.Password)) {
							_with1.Password = mRemoteNG.Security.Crypt.Decrypt(Settings.DefaultPassword, mRemoteNG.App.Info.General.EncryptionKey);
						}

						if (string.IsNullOrEmpty(_with1.Domain)) {
							_with1.Domain = Settings.DefaultDomain;
						}
					}

					if (_getSessionsThread != null) {
						if (_getSessionsThread.IsAlive)
							_getSessionsThread.Abort();
					}
					_getSessionsThread = new Thread(GetSessionsBackground);
					_getSessionsThread.SetApartmentState(ApartmentState.STA);
					_getSessionsThread.IsBackground = true;
					_getSessionsThread.Start(data);
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "UI.Window.Sessions.GetSessions() failed." + Constants.vbNewLine + ex.Message, true);
				}
			}

			public void KillSession()
			{
				if (sessionList.SelectedItems.Count == 0)
					return;

				mRemoteNG.Connection.Info connectionInfo = mRemoteNG.Tree.Node.SelectedNode.Tag as mRemoteNG.Connection.Info;
				if (connectionInfo == null)
					return;

				if (!(connectionInfo.Protocol == mRemoteNG.Connection.Protocol.Protocols.RDP))
					return;

				foreach (ListViewItem lvItem in sessionList.SelectedItems) {
					KillSession(connectionInfo.Hostname, connectionInfo.Username, connectionInfo.Password, connectionInfo.Domain, lvItem.Tag);
				}
			}

			public void KillSession(string hostname, string username, string password, string domain, string sessionId)
			{
				try {
					BackgroundData data = new BackgroundData();
					var _with2 = data;
					_with2.Hostname = hostname;
					_with2.Username = username;
					_with2.Password = password;
					_with2.Domain = domain;
					_with2.SessionId = sessionId;

					Thread thread = new Thread(KillSessionBackground);
					thread.SetApartmentState(ApartmentState.STA);
					thread.IsBackground = true;
					thread.Start(data);
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "UI.Window.Sessions.KillSession() failed." + Constants.vbNewLine + ex.Message, true);
				}
			}
			#endregion

			#region "Private Methods"
			#region "Form Stuff"
			private void Sessions_Load(object sender, EventArgs e)
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
					return;

				Security.Impersonator impersonator = new Security.Impersonator();
				mRemoteNG.Connection.Protocol.RDP.TerminalSessions terminalSessions = new mRemoteNG.Connection.Protocol.RDP.TerminalSessions();
				long serverHandle = 0;
				try {
					var _with3 = data;
					impersonator.StartImpersonation(_with3.Domain, _with3.Username, _with3.Password);

					serverHandle = terminalSessions.OpenConnection(_with3.Hostname);
					if (serverHandle == 0)
						return;

					GetSessions(terminalSessions, serverHandle);

					_retrieved = true;

				} catch (ThreadAbortException ex) {
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, Language.strSessionGetFailed + Constants.vbNewLine + ex.Message, true);
				} finally {
					impersonator.StopImpersonation();
					if (!(serverHandle == 0)) {
						terminalSessions.CloseConnection(serverHandle);
					}
				}
			}

			// Get sessions from an already impersonated and connected TerminalSessions object
			private void GetSessions(mRemoteNG.Connection.Protocol.RDP.TerminalSessions terminalSessions, long serverHandle)
			{
				mRemoteNG.Connection.Protocol.RDP.SessionsCollection rdpSessions = terminalSessions.GetSessions(serverHandle);
				foreach (mRemoteNG.Connection.Protocol.RDP.Session session in rdpSessions) {
					ListViewItem item = new ListViewItem();
					item.Tag = session.SessionId;
					item.Text = session.SessionUser;
					item.SubItems.Add(session.SessionState);
					item.SubItems.Add(Strings.Replace(session.SessionName, Constants.vbNewLine, ""));
					AddToList(item);
				}
			}

			private void KillSessionBackground(object dataObject)
			{
				BackgroundData data = dataObject as BackgroundData;
				if (data == null)
					return;

				Security.Impersonator impersonator = new Security.Impersonator();
				mRemoteNG.Connection.Protocol.RDP.TerminalSessions terminalSessions = new mRemoteNG.Connection.Protocol.RDP.TerminalSessions();
				long serverHandle = 0;
				try {
					var _with4 = data;
					if (string.IsNullOrEmpty(_with4.Username) | string.IsNullOrEmpty(_with4.Password))
						return;

					impersonator.StartImpersonation(_with4.Domain, _with4.Username, _with4.Password);

					serverHandle = terminalSessions.OpenConnection(_with4.Hostname);
					if (!(serverHandle == 0)) {
						terminalSessions.KillSession(serverHandle, _with4.SessionId);
					}

					ClearList();
					GetSessions(terminalSessions, serverHandle);

					_retrieved = true;

				} catch (ThreadAbortException ex) {
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, Language.strSessionKillFailed + Constants.vbNewLine + ex.Message, true);
				} finally {
					impersonator.StopImpersonation();
					if (!(serverHandle == 0)) {
						terminalSessions.CloseConnection(serverHandle);
					}
				}
			}

			public delegate void AddToListCallback(ListViewItem item);
			private void AddToList(ListViewItem item)
			{
				if (sessionList.InvokeRequired) {
					AddToListCallback callback = new AddToListCallback(AddToList);
					sessionList.Invoke(callback, new object[] { item });
				} else {
					sessionList.Items.Add(item);
				}
			}

			public delegate void ClearListCallback();
			private void ClearList()
			{
				if (sessionList.InvokeRequired) {
					ClearListCallback callback = new ClearListCallback(ClearList);
					sessionList.Invoke(callback);
				} else {
					sessionList.Items.Clear();
				}
			}

			private void menuSession_Opening(object sender, System.ComponentModel.CancelEventArgs e)
			{
				mRemoteNG.Connection.Info connectionInfo = null;
				if ((mRemoteNG.Tree.Node.TreeView == null || mRemoteNG.Tree.Node.SelectedNode == null)) {
					connectionInfo = null;
				} else {
					connectionInfo = mRemoteNG.Tree.Node.SelectedNode.Tag as mRemoteNG.Connection.Info;
				}

				if (connectionInfo == null) {
					sessionMenuLogoff.Enabled = false;
					sessionMenuRetrieve.Enabled = false;
					sessionMenuRetrieve.Text = Language.strMenuSessionRetrieve;
					return;
				}

				if (connectionInfo.Protocol == mRemoteNG.Connection.Protocol.Protocols.RDP & sessionList.SelectedItems.Count > 0) {
					sessionMenuLogoff.Enabled = true;
				} else {
					sessionMenuLogoff.Enabled = false;
				}

				if (connectionInfo.Protocol == mRemoteNG.Connection.Protocol.Protocols.RDP | connectionInfo.Protocol == mRemoteNG.Connection.Protocol.Protocols.ICA) {
					sessionMenuRetrieve.Enabled = true;
				} else {
					sessionMenuRetrieve.Enabled = false;
				}

				if (!_retrieved) {
					sessionMenuRetrieve.Text = Language.strMenuSessionRetrieve;
				} else {
					sessionMenuRetrieve.Text = Language.strRefresh;
				}
			}

			private void sessionMenuRetrieve_Click(System.Object sender, EventArgs e)
			{
				GetSessions();
			}

			private void sessionMenuLogoff_Click(System.Object sender, EventArgs e)
			{
				KillSession();
			}
			#endregion

			#region "Private Classes"
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
}
