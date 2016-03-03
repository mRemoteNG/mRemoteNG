// VBConversions Note: VB project level imports
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
// End of VB project level imports

using System.IO;
//using mRemoteNG.App.Runtime;


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
				
				Connection.Info connectionInfo = new Connection.Info();
				connectionInfo.Inherit = new Connection.Info.Inheritance(connectionInfo);
				connectionInfo.Name = name;
				connectionInfo.TreeNode = treeNode;
				connectionInfo.Parent = containerInfo;
				connectionInfo.IsContainer = true;
				containerInfo.ConnectionInfo = connectionInfo;
				
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
				treeNode.ImageIndex = Images.Enums.TreeImage.Container;
				treeNode.SelectedImageIndex = Images.Enums.TreeImage.Container;
				
				Connections.Load connectionsLoad = new Connections.Load();
				connectionsLoad.ConnectionFileName = fileName;
				connectionsLoad.RootTreeNode = treeNode;
				connectionsLoad.ConnectionList = ConnectionList;
				connectionsLoad.ContainerList = ContainerList;
				
				connectionsLoad.Load_Renamed(true);
				
				ContainerList.Add(containerInfo);
			}
		}
}
