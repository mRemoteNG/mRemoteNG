using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Images;
using System;
using System.Windows.Forms;
using mRemoteNG.Messages;
using mRemoteNG.Root.PuttySessions;

namespace mRemoteNG.Tree
{
	public class ConnectionTreeNode
    {
        #region Public Methods
		public static string GetConstantID(TreeNode node)
		{
			if (GetNodeType(node) == TreeNodeType.Connection)
				return (node.Tag as ConnectionInfo).ConstantID;
			else if (GetNodeType(node) == TreeNodeType.Container)
				return (node.Tag as ContainerInfo).ConnectionInfo.ConstantID;
				
			return null;
		}
		
		public static TreeNode GetNodeFromPositionID(int id)
		{
			foreach (ConnectionInfo connection in Runtime.ConnectionList)
			{
				if (connection.PositionID == id)
				{
					if (connection.IsContainer)
						return (connection.Parent as ContainerInfo).TreeNode;
					else
						return connection.TreeNode;
				}
			}
				
			return null;
		}
		
		public static TreeNode GetNodeFromConstantID(string id)
		{
            foreach (ConnectionInfo connectionInfo in Runtime.ConnectionList)
			{
				if (connectionInfo.ConstantID == id)
				{
					if (connectionInfo.IsContainer)
						return (connectionInfo.Parent as ContainerInfo).TreeNode;
					else
						return connectionInfo.TreeNode;
				}
			}
				
			return null;
		}
		
		public static TreeNodeType GetNodeType(TreeNode treeNode)
		{
			try
			{
                if (treeNode == null || treeNode.Tag == null)
					return TreeNodeType.None;
					
				if (treeNode.Tag is PuttySessionsNodeInfo)
					return TreeNodeType.PuttyRoot;
				else if (treeNode.Tag is Root.RootNodeInfo)
					return TreeNodeType.Root;
				else if (treeNode.Tag is ContainerInfo)
					return TreeNodeType.Container;
				else if (treeNode.Tag is PuttySessionInfo)
					return TreeNodeType.PuttySession;
				else if (treeNode.Tag is ConnectionInfo)
					return TreeNodeType.Connection;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "Couldn\'t get node type" + Environment.NewLine + ex.Message, true);
			}
				
			return TreeNodeType.None;
		}
		
		public static TreeNodeType GetNodeTypeFromString(string str)
		{
			try
			{
				switch (str.ToLower())
				{
					case "root":
						return TreeNodeType.Root;
					case "container":
						return TreeNodeType.Container;
					case "connection":
						return TreeNodeType.Connection;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "Couldn\'t get node type from string" + Environment.NewLine + ex.Message, true);
			}
				
			return TreeNodeType.None;
		}
		
		public static bool IsEmpty(TreeNode treeNode)
		{
			try
			{
				if (treeNode.Nodes.Count <= 0)
					return false;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "IsEmpty (Tree.Node) failed" + Environment.NewLine + ex.Message, true);
			}
				
			return true;
		}
		
		public static TreeNode AddNode(TreeNodeType nodeType, string name = null)
		{
			try
			{
				TreeNode treeNode = new TreeNode();
				string defaultName = "";
					
				switch (nodeType)
				{
					case TreeNodeType.Connection:
					case TreeNodeType.PuttySession:
						defaultName = My.Language.strNewConnection;
                        treeNode.ImageIndex = (int)TreeImageType.ConnectionClosed;
                        treeNode.SelectedImageIndex = (int)TreeImageType.ConnectionClosed;
						break;
					case TreeNodeType.Container:
						defaultName = My.Language.strNewFolder;
                        treeNode.ImageIndex = (int)TreeImageType.Container;
                        treeNode.SelectedImageIndex = (int)TreeImageType.Container;
						break;
					case TreeNodeType.Root:
						defaultName = My.Language.strNewRoot;
                        treeNode.ImageIndex = (int)TreeImageType.Root;
                        treeNode.SelectedImageIndex = (int)TreeImageType.Root;
						break;
				}
					
				if (!string.IsNullOrEmpty(name))
					treeNode.Name = name;
				else
					treeNode.Name = defaultName;
				treeNode.Text = treeNode.Name;
					
				return treeNode;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "AddNode failed" + Environment.NewLine + ex.Message, true);
			}
				
			return null;
		}
		
		public static void CloneNode(TreeNode oldTreeNode, TreeNode parentNode = null)
		{
			try
			{
				if (GetNodeType(oldTreeNode) == TreeNodeType.Connection)
                    CloneConnectionNode(oldTreeNode, parentNode);
				else if (GetNodeType(oldTreeNode) == TreeNodeType.Container)
                    CloneContainerNode(oldTreeNode, parentNode);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, string.Format(My.Language.strErrorCloneNodeFailed, ex.Message));
			}
		}

        private static void CloneContainerNode(TreeNode oldTreeNode, TreeNode parentNode)
        {
            ContainerInfo oldContainerInfo = (ContainerInfo) oldTreeNode.Tag;

            ContainerInfo newContainerInfo = oldContainerInfo.Copy();
            ConnectionInfo newConnectionInfo = oldContainerInfo.ConnectionInfo.Copy();
            newContainerInfo.ConnectionInfo = newConnectionInfo;

            TreeNode newTreeNode = new TreeNode(newContainerInfo.Name);
            newTreeNode.Tag = newContainerInfo;
            newTreeNode.ImageIndex = (int)TreeImageType.Container;
            newTreeNode.SelectedImageIndex = (int)TreeImageType.Container;
            newContainerInfo.ConnectionInfo.Parent = newContainerInfo;

            Runtime.ContainerList.Add(newContainerInfo);

            if (parentNode == null)
            {
                oldTreeNode.Parent.Nodes.Insert(oldTreeNode.Index + 1, newTreeNode);
                ConnectionTree.SelectedNode = newTreeNode;
            }
            else
            {
                parentNode.Nodes.Add(newTreeNode);
            }

            foreach (TreeNode childTreeNode in oldTreeNode.Nodes)
            {
                CloneNode(childTreeNode, newTreeNode);
            }

            newTreeNode.Expand();
        }

        private static void CloneConnectionNode(TreeNode oldTreeNode, TreeNode parentNode)
        {
            ConnectionInfo oldConnectionInfo = (ConnectionInfo)oldTreeNode.Tag;

            ConnectionInfo newConnectionInfo = oldConnectionInfo.Copy();
            ConnectionInfoInheritance newInheritance = oldConnectionInfo.Inherit.Copy();
            newInheritance.Parent = newConnectionInfo;
            newConnectionInfo.Inherit = newInheritance;

            Runtime.ConnectionList.Add(newConnectionInfo);

            TreeNode newTreeNode = new TreeNode(newConnectionInfo.Name);
            newTreeNode.Tag = newConnectionInfo;
            newTreeNode.ImageIndex = (int)TreeImageType.ConnectionClosed;
            newTreeNode.SelectedImageIndex = (int)TreeImageType.ConnectionClosed;

            newConnectionInfo.TreeNode = newTreeNode;

            if (parentNode == null)
            {
                oldTreeNode.Parent.Nodes.Insert(oldTreeNode.Index + 1, newTreeNode);
                ConnectionTree.SelectedNode = newTreeNode;
            }
            else
            {
                ContainerInfo parentContainerInfo = parentNode.Tag as ContainerInfo;
                if (parentContainerInfo != null)
                {
                    newConnectionInfo.Parent = parentContainerInfo;
                }
                parentNode.Nodes.Add(newTreeNode);
            }
        }
		
		public static void SetNodeImage(TreeNode treeNode, TreeImageType Img)
		{
			SetNodeImageIndex(treeNode, (int)Img);
		}
		
        public static void RenameNode(ConnectionInfo connectionInfo, string newName)
        {
            if (newName == null || newName.Length <= 0)
                return;

            connectionInfo.Name = newName;
            if (My.Settings.Default.SetHostnameLikeDisplayName)
                connectionInfo.Hostname = newName;
        }
        #endregion

        #region Private Methods
        private delegate void SetNodeImageIndexDelegate(TreeNode treeNode, int imageIndex);
        private static void SetNodeImageIndex(TreeNode treeNode, int imageIndex)
        {
            if (treeNode == null || treeNode.TreeView == null)
            {
                return;
            }
            if (treeNode.TreeView.InvokeRequired)
            {
                treeNode.TreeView.Invoke(new SetNodeImageIndexDelegate(SetNodeImageIndex), new object[] { treeNode, imageIndex });
                return;
            }

            treeNode.ImageIndex = imageIndex;
            treeNode.SelectedImageIndex = imageIndex;
        }
        #endregion
    }
}