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
    }
}