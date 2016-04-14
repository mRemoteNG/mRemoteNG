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
				
			Container.Info containerInfo = new Container.Info();
			containerInfo.TreeNode = treeNode;
			containerInfo.Name = name;
				
			Connection.ConnectionRecordImp connectionInfo = new Connection.ConnectionRecordImp();
			connectionInfo.Inherit = new Connection.ConnectionRecordImp.ConnectionRecordInheritanceImp(connectionInfo);
			connectionInfo.Name = name;
			connectionInfo.TreeNode = treeNode;
			connectionInfo.Parent = containerInfo;
			connectionInfo.IsContainer = true;
			containerInfo.ConnectionRecord = connectionInfo;
				
			// We can only inherit from a container node, not the root node or connection nodes
			if (Tree.Node.GetNodeType(parentTreeNode) == Tree.Node.Type.Container)
			{
				containerInfo.Parent = parentTreeNode.Tag;
			}
			else
			{
				connectionInfo.Inherit.TurnOffInheritanceCompletely();
			}
				
			treeNode.Name = name;
			treeNode.Tag = containerInfo;
			treeNode.ImageIndex = (int)Images.Enums.TreeImage.Container;
			treeNode.SelectedImageIndex = (int)Images.Enums.TreeImage.Container;
				
			Connections.Load connectionsLoad = new Connections.Load();
			connectionsLoad.ConnectionFileName = fileName;
			connectionsLoad.RootTreeNode = treeNode;
            connectionsLoad.ConnectionList = Runtime.ConnectionList;
            connectionsLoad.ContainerList = Runtime.ContainerList;
				
			connectionsLoad.LoadConnections(true);
				
			Runtime.ContainerList.Add(containerInfo);
		}
	}
}
