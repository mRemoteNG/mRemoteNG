using System;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using mRemoteNG.App;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection;
using mRemoteNG.Tree;
using mRemoteNG.Container;

namespace mRemoteNG.Config.Import
{
	public class PuttyConnectionManagerImporter
	{
		public static void Import(string fileName, TreeNode parentTreeNode)
		{
			var xmlDocument = new XmlDocument();
			xmlDocument.Load(fileName);
				
			var configurationNode = xmlDocument.SelectSingleNode("/configuration");

		    var rootNodes = configurationNode?.SelectNodes("./root");
		    if (rootNodes == null) return;
            foreach (XmlNode rootNode in rootNodes)
			{
				ImportRootOrContainer(rootNode, parentTreeNode);
			}
		}
			
		private static void ImportRootOrContainer(XmlNode xmlNode, TreeNode parentTreeNode)
		{
            var xmlNodeType = xmlNode?.Attributes?["type"].Value;
			switch (xmlNode?.Name)
			{
				case "root":
					if (string.Compare(xmlNodeType, "database", StringComparison.OrdinalIgnoreCase) != 0)
					{
						throw (new FileFormatException($"Unrecognized root node type ({xmlNodeType})."));
					}
					break;
				case "container":
					if (string.Compare(xmlNodeType, "folder", StringComparison.OrdinalIgnoreCase) != 0)
					{
						throw (new FileFormatException($"Unrecognized root node type ({xmlNodeType})."));
					}
					break;
				default:
					// ReSharper disable once LocalizableElement
					throw (new ArgumentException("Argument must be either a root or a container node.", nameof(xmlNode)));
			}
			
			if (parentTreeNode == null)
			{
				throw (new InvalidOperationException("parentInfo.TreeNode must not be null."));
			}

            var name = xmlNode.Attributes?["name"].Value;

            var treeNode = new TreeNode(name);
			parentTreeNode.Nodes.Add(treeNode);

		    var containerInfo = new ContainerInfo
		    {
		        TreeNode = treeNode,
		        Name = name
		    };
            
            var connectionInfo = CreateConnectionInfo(name);
			connectionInfo.Parent = containerInfo;
			connectionInfo.IsContainer = true;
			containerInfo.CopyFrom(connectionInfo);
			
			// We can only inherit from a container node, not the root node or connection nodes
			if (ConnectionTreeNode.GetNodeType(parentTreeNode) == TreeNodeType.Container)
			{
				containerInfo.Parent = (ContainerInfo)parentTreeNode.Tag;
			}
			else
			{
				connectionInfo.Inheritance.DisableInheritance();
			}
			
			treeNode.Name = name;
			treeNode.Tag = containerInfo;
			treeNode.ImageIndex = (int)TreeImageType.Container;
			treeNode.SelectedImageIndex = (int)TreeImageType.Container;

		    var childNodes = xmlNode.SelectNodes("./*");
            if (childNodes == null) return;
            foreach (XmlNode childNode in childNodes)
			{
				switch (childNode.Name)
				{
					case "container":
						ImportRootOrContainer(childNode, treeNode);
						break;
					case "connection":
						ImportConnection(childNode, treeNode);
						break;
					default:
						throw (new FileFormatException($"Unrecognized child node ({childNode.Name})."));
				}
			}
				
			containerInfo.IsExpanded = bool.Parse(xmlNode?.Attributes?["expanded"].InnerText ?? "false");
			if (containerInfo.IsExpanded)
			{
				treeNode.Expand();
			}
				
			Runtime.ContainerList.Add(containerInfo);
		}
			
		private static void ImportConnection(XmlNode connectionNode, TreeNode parentTreeNode)
		{
            var connectionNodeType = connectionNode.Attributes?["type"].Value;
			if (string.Compare(connectionNodeType, "PuTTY", StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw (new FileFormatException($"Unrecognized connection node type ({connectionNodeType})."));
			}

            var name = connectionNode.Attributes?["name"].Value;
            var treeNode = new TreeNode(name);
			parentTreeNode.Nodes.Add(treeNode);

            var connectionInfo = ConnectionInfoFromXml(connectionNode);
			connectionInfo.TreeNode = treeNode;
			connectionInfo.Parent = (ContainerInfo)parentTreeNode.Tag;
				
			treeNode.Name = name;
			treeNode.Tag = connectionInfo;
			treeNode.ImageIndex = (int)TreeImageType.ConnectionClosed;
            treeNode.SelectedImageIndex = (int)TreeImageType.ConnectionClosed;
				
			Runtime.ConnectionList.Add(connectionInfo);
		}
			
		private static ConnectionInfo CreateConnectionInfo(string name)
		{
            var connectionInfo = new ConnectionInfo();
			connectionInfo.Inheritance = new ConnectionInfoInheritance(connectionInfo);
			connectionInfo.Name = name;
			return connectionInfo;
		}
			
		private static ConnectionInfo ConnectionInfoFromXml(XmlNode xmlNode)
		{
            var connectionInfoNode = xmlNode.SelectSingleNode("./connection_info");

            var name = connectionInfoNode?.SelectSingleNode("./name")?.InnerText;
            var connectionInfo = CreateConnectionInfo(name);

            var protocol = connectionInfoNode?.SelectSingleNode("./protocol")?.InnerText;
			switch (protocol?.ToLowerInvariant())
			{
				case "telnet":
					connectionInfo.Protocol = ProtocolType.Telnet;
					break;
				case "ssh":
					connectionInfo.Protocol = ProtocolType.SSH2;
					break;
				default:
					throw (new FileFormatException($"Unrecognized protocol ({protocol})."));
			}
				
			connectionInfo.Hostname = connectionInfoNode.SelectSingleNode("./host")?.InnerText;
			connectionInfo.Port = Convert.ToInt32(connectionInfoNode.SelectSingleNode("./port")?.InnerText);
			connectionInfo.PuttySession = connectionInfoNode.SelectSingleNode("./session")?.InnerText;
			// ./commandline
			connectionInfo.Description = connectionInfoNode.SelectSingleNode("./description")?.InnerText;

            var loginNode = xmlNode.SelectSingleNode("./login");
			connectionInfo.Username = loginNode?.SelectSingleNode("login")?.InnerText;
			connectionInfo.Password = loginNode?.SelectSingleNode("password")?.InnerText;
			// ./prompt
				
			// ./timeout/connectiontimeout
			// ./timeout/logintimeout
			// ./timeout/passwordtimeout
			// ./timeout/commandtimeout
				
			// ./command/command1
			// ./command/command2
			// ./command/command3
			// ./command/command4
			// ./command/command5
				
			// ./options/loginmacro
			// ./options/postcommands
			// ./options/endlinechar
				
			return connectionInfo;
		}
	}
}