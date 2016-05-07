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
using System.DirectoryServices;
using mRemoteNG.App;
using System.Text.RegularExpressions;
using mRemoteNG.My;
using mRemoteNG.Connection;
using mRemoteNG.Container;


namespace mRemoteNG.Config.Import
{
	public class ActiveDirectory
	{
		public static void Import(string ldapPath, TreeNode parentTreeNode)
		{
			try
			{
				TreeNode treeNode = Tree.ConnectionTreeNode.AddNode(Tree.TreeNodeType.Container);
					
				ContainerInfo containerInfo = new ContainerInfo();
				containerInfo.TreeNode = treeNode;
				containerInfo.ConnectionInfo = new ConnectionInfo(containerInfo);
				
				string name = "";
				Match match = Regex.Match(ldapPath, "ou=([^,]*)", RegexOptions.IgnoreCase);
				if (match.Success)
				{
					name = match.Groups[1].Captures[0].Value;
				}
				else
				{
					name = Language.strActiveDirectory;
				}
					
				containerInfo.Name = name;
					
				// We can only inherit from a container node, not the root node or connection nodes
				if (Tree.ConnectionTreeNode.GetNodeType(parentTreeNode) == Tree.TreeNodeType.Container)
				{
					containerInfo.Parent = parentTreeNode.Tag;
				}
				else
				{
					containerInfo.ConnectionInfo.Inherit.TurnOffInheritanceCompletely();
				}
					
				treeNode.Text = name;
				treeNode.Name = name;
				treeNode.Tag = containerInfo;
                Runtime.ContainerList.Add(containerInfo);
					
				ImportComputers(ldapPath, treeNode);
					
				parentTreeNode.Nodes.Add(treeNode);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(message: "Config.Import.ActiveDirectory.Import() failed.", ex: ex, logOnly: true);
			}
		}
			
		private static void ImportComputers(string ldapPath, TreeNode parentTreeNode)
		{
			try
			{
				string strDisplayName = "";
				string strDescription = "";
				string strHostName = "";
					
				const string ldapFilter = "(objectClass=computer)";
					
				DirectorySearcher ldapSearcher = new DirectorySearcher();
				SearchResultCollection ldapResults = default(SearchResultCollection);
				SearchResult ldapResult = default(SearchResult);
					
				ldapSearcher.SearchRoot = new DirectoryEntry(ldapPath);
				ldapSearcher.PropertiesToLoad.AddRange(new[] {"securityEquals", "cn"});
				ldapSearcher.Filter = ldapFilter;
				ldapSearcher.SearchScope = SearchScope.OneLevel;
					
				ldapResults = ldapSearcher.FindAll();
					
				foreach (SearchResult tempLoopVar_ldapResult in ldapResults)
				{
					ldapResult = tempLoopVar_ldapResult;
					System.DirectoryServices.DirectoryEntry with_2 = ldapResult.GetDirectoryEntry();
					strDisplayName = Convert.ToString(with_2.Properties["cn"].Value);
					strDescription = Convert.ToString(with_2.Properties["Description"].Value);
					strHostName = Convert.ToString(with_2.Properties["dNSHostName"].Value);
						
					TreeNode treeNode = Tree.ConnectionTreeNode.AddNode(Tree.TreeNodeType.Connection, strDisplayName);
						
					ConnectionInfo connectionInfo = new ConnectionInfo();
					ConnectionInfoInheritance inheritanceInfo = new ConnectionInfoInheritance(connectionInfo, true);
					inheritanceInfo.Description = false;
					if (parentTreeNode.Tag is ContainerInfo)
					{
						connectionInfo.Parent = (ContainerInfo)parentTreeNode.Tag;
					}
					connectionInfo.Inherit = inheritanceInfo;
					connectionInfo.Name = strDisplayName;
					connectionInfo.Hostname = strHostName;
					connectionInfo.Description = strDescription;
					connectionInfo.TreeNode = treeNode;
					treeNode.Name = strDisplayName;
					treeNode.Tag = connectionInfo; //set the nodes tag to the conI
					//add connection to connections
                    Runtime.ConnectionList.Add(connectionInfo);
						
					parentTreeNode.Nodes.Add(treeNode);
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(message: "Config.Import.ActiveDirectory.ImportComputers() failed.", ex: ex, logOnly: true);
			}
		}
	}
}