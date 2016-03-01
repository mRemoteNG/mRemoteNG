using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using mRemoteNG.App.Runtime;

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
			if (mRemoteNG.Tree.Node.GetNodeType(parentTreeNode) == mRemoteNG.Tree.Node.Type.Container) {
				containerInfo.Parent = parentTreeNode.Tag;
			} else {
				connectionInfo.Inherit.TurnOffInheritanceCompletely();
			}

			treeNode.Name = name;
			treeNode.Tag = containerInfo;
			treeNode.ImageIndex = mRemoteNG.Images.Enums.TreeImage.Container;
			treeNode.SelectedImageIndex = mRemoteNG.Images.Enums.TreeImage.Container;

			Connections.Load connectionsLoad = new Connections.Load();
			var _with1 = connectionsLoad;
			_with1.ConnectionFileName = fileName;
			_with1.RootTreeNode = treeNode;
			_with1.ConnectionList = mRemoteNG.App.Runtime.ConnectionList;
			_with1.ContainerList = mRemoteNG.App.Runtime.ContainerList;

			connectionsLoad.Load(true);

			mRemoteNG.App.Runtime.ContainerList.Add(containerInfo);
		}
	}
}
