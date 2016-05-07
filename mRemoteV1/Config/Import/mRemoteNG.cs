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
using System.IO;
using mRemoteNG.App;
using mRemoteNG.Images;
using mRemoteNG.Config.Connections;
using mRemoteNG.Container;
using mRemoteNG.Connection;


namespace mRemoteNG.Config.Import
{
	// ReSharper disable once InconsistentNaming
	public class mRemoteNG
	{
		public static void Import(string fileName, TreeNode parentTreeNode)
		{
			string name = Path.GetFileNameWithoutExtension(fileName);
			TreeNode treeNode = new TreeNode(name);
			parentTreeNode.Nodes.Add(treeNode);

		    ContainerInfo containerInfo = new ContainerInfo
		    {
		        TreeNode = treeNode,
		        Name = name
		    };

		    var connectionInfo = new ConnectionInfo();
			connectionInfo.Inherit = new ConnectionInfoInheritance(connectionInfo);
			connectionInfo.Name = name;
			connectionInfo.TreeNode = treeNode;
			connectionInfo.Parent = containerInfo;
			connectionInfo.IsContainer = true;
			containerInfo.ConnectionInfo = connectionInfo;
				
			// We can only inherit from a container node, not the root node or connection nodes
			if (Tree.ConnectionTreeNode.GetNodeType(parentTreeNode) == Tree.TreeNodeType.Container)
			{
				containerInfo.Parent = parentTreeNode.Tag;
			}
			else
			{
				connectionInfo.Inherit.TurnOffInheritanceCompletely();
			}
				
			treeNode.Name = name;
			treeNode.Tag = containerInfo;
			treeNode.ImageIndex = (int)TreeImageType.Container;
			treeNode.SelectedImageIndex = (int)TreeImageType.Container;
				
			ConnectionsLoader connectionsLoad = new ConnectionsLoader();
			connectionsLoad.ConnectionFileName = fileName;
			connectionsLoad.RootTreeNode = treeNode;
            connectionsLoad.ConnectionList = Runtime.ConnectionList;
            connectionsLoad.ContainerList = Runtime.ContainerList;
				
			connectionsLoad.LoadConnections(true);
				
			Runtime.ContainerList.Add(containerInfo);
		}
	}
}