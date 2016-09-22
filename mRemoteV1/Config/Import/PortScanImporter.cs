using System.Collections;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Tools;


namespace mRemoteNG.Config.Import
{
	public static class PortScanImporter
	{
		public static void Import(IEnumerable hosts, ProtocolType protocol, TreeNode parentTreeNode)
		{
			foreach (ScanHost host in hosts)
			{
				var finalProtocol = default(ProtocolType);
				var protocolValid = false;
					
				var treeNode = Tree.ConnectionTreeNode.AddNode(Tree.TreeNodeType.Connection, host.HostNameWithoutDomain);
					
				var connectionInfo = new ConnectionInfo();
				connectionInfo.Inheritance = new ConnectionInfoInheritance(connectionInfo);
					
				connectionInfo.Name = host.HostNameWithoutDomain;
				connectionInfo.Hostname = host.HostName;
					
				switch (protocol)
				{
					case ProtocolType.SSH2:
						if (host.SSH)
						{
							finalProtocol = ProtocolType.SSH2;
							protocolValid = true;
						}
						break;
					case ProtocolType.Telnet:
						if (host.Telnet)
						{
							finalProtocol = ProtocolType.Telnet;
							protocolValid = true;
						}
						break;
					case ProtocolType.HTTP:
						if (host.HTTP)
						{
							finalProtocol = ProtocolType.HTTP;
							protocolValid = true;
						}
						break;
					case ProtocolType.HTTPS:
						if (host.HTTPS)
						{
							finalProtocol = ProtocolType.HTTPS;
							protocolValid = true;
						}
						break;
					case ProtocolType.Rlogin:
						if (host.Rlogin)
						{
							finalProtocol = ProtocolType.Rlogin;
							protocolValid = true;
						}
						break;
					case ProtocolType.RDP:
						if (host.RDP)
						{
							finalProtocol = ProtocolType.RDP;
							protocolValid = true;
						}
						break;
					case ProtocolType.VNC:
						if (host.VNC)
						{
							finalProtocol = ProtocolType.VNC;
							protocolValid = true;
						}
						break;
				}
					
				if (protocolValid)
				{
					connectionInfo.Protocol = finalProtocol;
					connectionInfo.SetDefaultPort();
						
					treeNode.Tag = connectionInfo;
					parentTreeNode.Nodes.Add(treeNode);
						
					if (parentTreeNode.Tag is ContainerInfo)
					{
						connectionInfo.Parent = (ContainerInfo)parentTreeNode.Tag;
					}
						
					Runtime.ConnectionList.Add(connectionInfo);
				}
			}
		}
	}
}