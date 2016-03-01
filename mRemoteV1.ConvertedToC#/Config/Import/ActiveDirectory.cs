using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.DirectoryServices;
using mRemoteNG.App.Runtime;
using System.Text.RegularExpressions;
using mRemoteNG.My;

namespace mRemoteNG.Config.Import
{
	public class ActiveDirectory
	{
		public static void Import(string ldapPath, TreeNode parentTreeNode)
		{
			try {
				TreeNode treeNode = mRemoteNG.Tree.Node.AddNode(mRemoteNG.Tree.Node.Type.Container);

				Container.Info containerInfo = new Container.Info();
				containerInfo.TreeNode = treeNode;
				containerInfo.ConnectionInfo = new Connection.Info(containerInfo);

				string name = null;
				Match match = Regex.Match(ldapPath, "ou=([^,]*)", RegexOptions.IgnoreCase);
				if (match.Success) {
					name = match.Groups[1].Captures[0].Value;
				} else {
					name = Language.strActiveDirectory;
				}

				containerInfo.Name = name;

				// We can only inherit from a container node, not the root node or connection nodes
				if (mRemoteNG.Tree.Node.GetNodeType(parentTreeNode) == mRemoteNG.Tree.Node.Type.Container) {
					containerInfo.Parent = parentTreeNode.Tag;
				} else {
					containerInfo.ConnectionInfo.Inherit.TurnOffInheritanceCompletely();
				}

				treeNode.Text = name;
				treeNode.Name = name;
				treeNode.Tag = containerInfo;
				mRemoteNG.App.Runtime.ContainerList.Add(containerInfo);

				ImportComputers(ldapPath, treeNode);

				parentTreeNode.Nodes.Add(treeNode);
			} catch (Exception ex) {
				mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage("Config.Import.ActiveDirectory.Import() failed.", ex, , true);
			}
		}

		private static void ImportComputers(string ldapPath, TreeNode parentTreeNode)
		{
			try {
				string strDisplayName = null;
				string strDescription = null;
				string strHostName = null;

				const string ldapFilter = "(objectClass=computer)";

				DirectorySearcher ldapSearcher = new DirectorySearcher();
				SearchResultCollection ldapResults = null;
				SearchResult ldapResult = null;

				var _with1 = ldapSearcher;
				_with1.SearchRoot = new DirectoryEntry(ldapPath);
				_with1.PropertiesToLoad.AddRange({
					"securityEquals",
					"cn"
				});
				_with1.Filter = ldapFilter;
				_with1.SearchScope = SearchScope.OneLevel;

				ldapResults = ldapSearcher.FindAll();

				foreach (SearchResult ldapResult_loopVariable in ldapResults) {
					ldapResult = ldapResult_loopVariable;
					var _with2 = ldapResult.GetDirectoryEntry();
					strDisplayName = _with2.Properties("cn").Value;
					strDescription = _with2.Properties("Description").Value;
					strHostName = _with2.Properties("dNSHostName").Value;

					TreeNode treeNode = mRemoteNG.Tree.Node.AddNode(mRemoteNG.Tree.Node.Type.Connection, strDisplayName);

					Connection.Info connectionInfo = new Connection.Info();
					Connection.Info.Inheritance inheritanceInfo = new Connection.Info.Inheritance(connectionInfo, true);
					inheritanceInfo.Description = false;
					if (parentTreeNode.Tag is Container.Info) {
						connectionInfo.Parent = parentTreeNode.Tag;
					}
					connectionInfo.Inherit = inheritanceInfo;
					connectionInfo.Name = strDisplayName;
					connectionInfo.Hostname = strHostName;
					connectionInfo.Description = strDescription;
					connectionInfo.TreeNode = treeNode;
					treeNode.Name = strDisplayName;
					treeNode.Tag = connectionInfo;
					//set the nodes tag to the conI
					//add connection to connections
					mRemoteNG.App.Runtime.ConnectionList.Add(connectionInfo);

					parentTreeNode.Nodes.Add(treeNode);
				}
			} catch (Exception ex) {
				mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage("Config.Import.ActiveDirectory.ImportComputers() failed.", ex, , true);
			}
		}
	}
}
