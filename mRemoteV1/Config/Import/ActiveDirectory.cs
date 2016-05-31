using System;
using System.Windows.Forms;
using System.DirectoryServices;
using mRemoteNG.App;
using System.Text.RegularExpressions;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;

namespace mRemoteNG.Config.Import
{
	public class ActiveDirectory
	{
		public static void Import(string ldapPath, TreeNode parentTreeNode)
		{
			try
			{
				var treeNode = ConnectionTreeNode.AddNode(TreeNodeType.Container);
					
				var containerInfo = new ContainerInfo();
				containerInfo.TreeNode = treeNode;
				containerInfo.ConnectionInfo = new ConnectionInfo(containerInfo);
				
				var name = "";
				var match = Regex.Match(ldapPath, "ou=([^,]*)", RegexOptions.IgnoreCase);
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
				if (ConnectionTreeNode.GetNodeType(parentTreeNode) == TreeNodeType.Container)
				{
					containerInfo.Parent = (ContainerInfo)parentTreeNode.Tag;
				}
				else
				{
					containerInfo.ConnectionInfo.Inheritance.DisableInheritance();
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
				Runtime.MessageCollector.AddExceptionMessage("Config.Import.ActiveDirectory.Import() failed.", ex, logOnly: true);
			}
		}
			
		private static void ImportComputers(string ldapPath, TreeNode parentTreeNode)
		{
			try
			{
			    const string ldapFilter = "(objectClass=computer)";
					
				var ldapSearcher = new DirectorySearcher();
				var ldapResults = default(SearchResultCollection);
				var ldapResult = default(SearchResult);
					
				ldapSearcher.SearchRoot = new DirectoryEntry(ldapPath);
				ldapSearcher.PropertiesToLoad.AddRange(new[] {"securityEquals", "cn"});
				ldapSearcher.Filter = ldapFilter;
				ldapSearcher.SearchScope = SearchScope.OneLevel;
					
				ldapResults = ldapSearcher.FindAll();
					
				foreach (SearchResult tempLoopVar_ldapResult in ldapResults)
				{
					ldapResult = tempLoopVar_ldapResult;
					var with_2 = ldapResult.GetDirectoryEntry();
					var displayName = Convert.ToString(with_2.Properties["cn"].Value);
					var description = Convert.ToString(with_2.Properties["Description"].Value);
					var hostName = Convert.ToString(with_2.Properties["dNSHostName"].Value);
						
					var treeNode = ConnectionTreeNode.AddNode(TreeNodeType.Connection, displayName);
						
					var connectionInfo = new ConnectionInfo();
					var inheritanceInfo = new ConnectionInfoInheritance(connectionInfo, true);
					inheritanceInfo.Description = false;
					if (parentTreeNode.Tag is ContainerInfo)
					{
						connectionInfo.Parent = (ContainerInfo)parentTreeNode.Tag;
					}
					connectionInfo.Inheritance = inheritanceInfo;
					connectionInfo.Name = displayName;
					connectionInfo.Hostname = hostName;
					connectionInfo.Description = description;
					connectionInfo.TreeNode = treeNode;
					treeNode.Name = displayName;
					treeNode.Tag = connectionInfo; //set the nodes tag to the conI
					//add connection to connections
                    Runtime.ConnectionList.Add(connectionInfo);
						
					parentTreeNode.Nodes.Add(treeNode);
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage("Config.Import.ActiveDirectory.ImportComputers() failed.", ex, logOnly: true);
			}
		}
	}
}