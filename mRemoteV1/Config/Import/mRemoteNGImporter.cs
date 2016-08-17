using System.Windows.Forms;
using System.IO;
using mRemoteNG.App;
using mRemoteNG.Config.Connections;
using mRemoteNG.Container;
using mRemoteNG.Connection;
using mRemoteNG.Tree;

namespace mRemoteNG.Config.Import
{
	// ReSharper disable once InconsistentNaming
	public class mRemoteNGImporter
	{
		public static void Import(string fileName, TreeNode parentTreeNode)
		{
			var name = Path.GetFileNameWithoutExtension(fileName);
			var treeNode = new TreeNode(name);
			parentTreeNode.Nodes.Add(treeNode);

		    var containerInfo = new ContainerInfo
		    {
		        TreeNode = treeNode,
		        Name = name,
                IsContainer = true
            };

            containerInfo.Inheritance = new ConnectionInfoInheritance(containerInfo);
			
			// We can only inherit from a container node, not the root node or connection nodes
		    var parent = parentTreeNode.Tag as ContainerInfo;
		    if (parent != null)
				containerInfo.Parent = parent;
			else
                containerInfo.Inheritance.DisableInheritance();
				
			treeNode.Name = name;
			treeNode.Tag = containerInfo;
			treeNode.ImageIndex = (int)TreeImageType.Container;
			treeNode.SelectedImageIndex = (int)TreeImageType.Container;

		    var connectionsLoad = new ConnectionsLoader
		    {
		        ConnectionFileName = fileName,
		        RootTreeNode = treeNode,
		        ConnectionList = Runtime.ConnectionList,
		        ContainerList = Runtime.ContainerList
		    };

		    connectionsLoad.LoadConnections(true);
			Runtime.ContainerList.Add(containerInfo);
		}
	}
}