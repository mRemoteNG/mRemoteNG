using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.My;

namespace mRemoteNG.Config.Putty
{
	public abstract class Provider
	{
		#region "Public Methods"
		private TreeNode _rootTreeNode;
		public TreeNode RootTreeNode {
			get {
				if (_rootTreeNode == null)
					_rootTreeNode = CreateRootTreeNode();
				return _rootTreeNode;
			}
		}

		private Root.PuttySessions.Info _rootInfo;
		public Root.PuttySessions.Info RootInfo {
			get {
				if (_rootInfo == null)
					_rootInfo = CreateRootInfo();
				return _rootInfo;
			}
		}

		public abstract string[] GetSessionNames(bool raw = false);
		public abstract Connection.PuttySession.Info GetSession(string sessionName);

		public virtual Connection.PuttySession.Info[] GetSessions()
		{
			List<Connection.PuttySession.Info> sessionList = new List<Connection.PuttySession.Info>();
			Connection.Info sessionInfo = null;
			foreach (string sessionName in GetSessionNames(true)) {
				sessionInfo = GetSession(sessionName);
				if (sessionInfo == null || string.IsNullOrEmpty(sessionInfo.Hostname))
					continue;
				sessionList.Add(sessionInfo);
			}
			return sessionList.ToArray();
		}


		public virtual void StartWatcher()
		{
		}


		public virtual void StopWatcher()
		{
		}
		#endregion

		#region "Public Events"
		public event SessionChangedEventHandler SessionChanged;
		public delegate void SessionChangedEventHandler(object sender, SessionChangedEventArgs e);
		#endregion

		#region "Public Classes"
		public class SessionChangedEventArgs : EventArgs
		{
		}
		#endregion

		#region "Protected Methods"
		private delegate TreeNode CreateRootTreeNodeDelegate();
		protected virtual TreeNode CreateRootTreeNode()
		{
			TreeView treeView = mRemoteNG.Tree.Node.TreeView;
			if (treeView == null)
				return null;
			if (treeView.InvokeRequired) {
				return treeView.Invoke(new CreateRootTreeNodeDelegate(CreateRootTreeNode));
			}

			TreeNode newTreeNode = new TreeNode();
			RootInfo.TreeNode = newTreeNode;

			newTreeNode.Name = _rootInfo.Name;
			newTreeNode.Text = _rootInfo.Name;
			newTreeNode.Tag = _rootInfo;
			newTreeNode.ImageIndex = mRemoteNG.Images.Enums.TreeImage.PuttySessions;
			newTreeNode.SelectedImageIndex = mRemoteNG.Images.Enums.TreeImage.PuttySessions;

			return newTreeNode;
		}

		protected virtual Root.PuttySessions.Info CreateRootInfo()
		{
			Root.PuttySessions.Info newRootInfo = new Root.PuttySessions.Info();

			if (string.IsNullOrEmpty(mRemoteNG.My.Settings.PuttySavedSessionsName)) {
				newRootInfo.Name = Language.strPuttySavedSessionsRootName;
			} else {
				newRootInfo.Name = mRemoteNG.My.Settings.PuttySavedSessionsName;
			}

			if (string.IsNullOrEmpty(mRemoteNG.My.Settings.PuttySavedSessionsPanel)) {
				newRootInfo.Panel = Language.strGeneral;
			} else {
				newRootInfo.Panel = mRemoteNG.My.Settings.PuttySavedSessionsPanel;
			}

			return newRootInfo;
		}

		protected virtual void OnSessionChanged(SessionChangedEventArgs e)
		{
			if (SessionChanged != null) {
				SessionChanged(this, new SessionChangedEventArgs());
			}
		}
		#endregion
	}
}
