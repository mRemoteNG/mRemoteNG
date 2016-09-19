using System.Collections.Generic;
using System;
using System.Windows.Forms;
using mRemoteNG.Tree;
using mRemoteNG.Connection;
using mRemoteNG.Root.PuttySessions;


namespace mRemoteNG.Config.Putty
{
    public abstract class AbstractPuttySessionsProvider
	{
        #region Public Methods
		private TreeNode _rootTreeNode;
        public TreeNode RootTreeNode
		{
			get
			{
				if (_rootTreeNode == null)
				{
					_rootTreeNode = CreateRootTreeNode();
				}
				return _rootTreeNode;
			}
		}
			
		private RootPuttySessionsNodeInfo _rootInfo;
        public RootPuttySessionsNodeInfo RootInfo => _rootInfo ?? (_rootInfo = CreateRootInfo());

        public abstract string[] GetSessionNames(bool raw = false);
		public abstract PuttySessionInfo GetSession(string sessionName);
			
		public virtual IEnumerable<PuttySessionInfo> GetSessions()
		{
			var sessionList = new List<PuttySessionInfo>();
			foreach (var sessionName in GetSessionNames(true))
			{
				var sessionInfo = GetSession(sessionName);
				if (string.IsNullOrEmpty(sessionInfo?.Hostname))
				{
					continue;
				}
				sessionList.Add(sessionInfo);
			}
			return sessionList;
		}
			
		public virtual void StartWatcher()
		{
				
		}
			
		public virtual void StopWatcher()
		{
				
		}
        #endregion
			
        #region Public Events
		public delegate void SessionChangedEventHandler(object sender, SessionChangedEventArgs e);
		private SessionChangedEventHandler SessionChangedEvent;
			
		public event SessionChangedEventHandler SessionChanged
		{
			add
			{
				SessionChangedEvent = (SessionChangedEventHandler) Delegate.Combine(SessionChangedEvent, value);
			}
			remove
			{
				SessionChangedEvent = (SessionChangedEventHandler) Delegate.Remove(SessionChangedEvent, value);
			}
		}
        #endregion
			
        #region Public Classes
		public class SessionChangedEventArgs : EventArgs
		{
		}
        #endregion
			
        #region Protected Methods
		private delegate TreeNode  CreateRootTreeNodeDelegate();
		protected virtual TreeNode CreateRootTreeNode()
		{
            TreeView treeView = ConnectionTree.TreeView;
			if (treeView == null)
			{
				return null;
			}
			if (treeView.InvokeRequired)
			{
				return (TreeNode)treeView.Invoke(new CreateRootTreeNodeDelegate(CreateRootTreeNode));
			}
				
			TreeNode newTreeNode = new TreeNode();
			RootInfo.TreeNode = newTreeNode;
				
			newTreeNode.Name = _rootInfo.Name;
			newTreeNode.Text = _rootInfo.Name;
			newTreeNode.Tag = _rootInfo;
			newTreeNode.ImageIndex = (int)TreeImageType.PuttySessions;
			newTreeNode.SelectedImageIndex = (int)TreeImageType.PuttySessions;
				
			return newTreeNode;
		}
			
		protected virtual RootPuttySessionsNodeInfo CreateRootInfo()
		{
			var newRootInfo = new RootPuttySessionsNodeInfo();
				
			if (string.IsNullOrEmpty(Convert.ToString(mRemoteNG.Settings.Default.PuttySavedSessionsName)))
			{
				newRootInfo.Name = Language.strPuttySavedSessionsRootName;
			}
			else
			{
				newRootInfo.Name = Convert.ToString(mRemoteNG.Settings.Default.PuttySavedSessionsName);
			}
				
			if (string.IsNullOrEmpty(Convert.ToString(mRemoteNG.Settings.Default.PuttySavedSessionsPanel)))
			{
				newRootInfo.Panel = Language.strGeneral;
			}
			else
			{
				newRootInfo.Panel = Convert.ToString(mRemoteNG.Settings.Default.PuttySavedSessionsPanel);
			}
				
			return newRootInfo;
		}
			
		protected virtual void OnSessionChanged(SessionChangedEventArgs e)
		{
		    SessionChangedEvent?.Invoke(this, new SessionChangedEventArgs());
		}

        #endregion
	}
}