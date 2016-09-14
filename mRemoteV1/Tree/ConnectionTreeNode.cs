using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using System;
using System.Windows.Forms;
using mRemoteNG.Messages;
using mRemoteNG.Root.PuttySessions;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Tree
{
	public static class ConnectionTreeNode
    {
        //TODO Everything in this class needs to be updated / rewritten to work with the TreeListView/ConnectionTreeModel
		public static TreeNode GetNodeFromConstantID(string id)
		{
            foreach (ConnectionInfo connectionInfo in Runtime.ConnectionList)
			{
				if (connectionInfo.ConstantID == id)
				{
				    if (connectionInfo.IsContainer)
						return connectionInfo.Parent.TreeNode;
				    return connectionInfo.TreeNode;
				}
			}
				
			return null;
		}
		
		public static TreeNodeType GetNodeType(TreeNode treeNode)
		{
			try
			{
                if (treeNode?.Tag == null)
					return TreeNodeType.None;
					
				if (treeNode.Tag is RootPuttySessionsNodeInfo)
					return TreeNodeType.PuttyRoot;
			    if (treeNode.Tag is RootNodeInfo)
			        return TreeNodeType.Root;
			    if (treeNode.Tag is ContainerInfo)
			        return TreeNodeType.Container;
			    if (treeNode.Tag is PuttySessionInfo)
			        return TreeNodeType.PuttySession;
			    if (treeNode.Tag is ConnectionInfo)
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
						defaultName = Language.strNewConnection;
                        treeNode.ImageIndex = (int)TreeImageType.ConnectionClosed;
                        treeNode.SelectedImageIndex = (int)TreeImageType.ConnectionClosed;
						break;
					case TreeNodeType.Container:
						defaultName = Language.strNewFolder;
                        treeNode.ImageIndex = (int)TreeImageType.Container;
                        treeNode.SelectedImageIndex = (int)TreeImageType.Container;
						break;
					case TreeNodeType.Root:
						defaultName = Language.strNewRoot;
                        treeNode.ImageIndex = (int)TreeImageType.Root;
                        treeNode.SelectedImageIndex = (int)TreeImageType.Root;
						break;
				}
					
				treeNode.Name = !string.IsNullOrEmpty(name) ? name : defaultName;
				treeNode.Text = treeNode.Name;
					
				return treeNode;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "AddNode failed" + Environment.NewLine + ex.Message, true);
			}
				
			return null;
		}
    }
}