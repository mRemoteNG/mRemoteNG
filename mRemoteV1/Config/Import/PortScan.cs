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
using mRemoteNG.App;


namespace mRemoteNG.Config.Import
{
	public class PortScan
	{
		public static void Import(IEnumerable hosts, Connection.Protocol.Protocols protocol, TreeNode parentTreeNode)
		{
			foreach (Tools.PortScan.ScanHost host in hosts)
			{
				Connection.Protocol.Protocols finalProtocol = default(Connection.Protocol.Protocols);
				bool protocolValid = false;
					
				TreeNode treeNode = Tree.Node.AddNode(Tree.Node.Type.Connection, host.HostNameWithoutDomain);
					
				Connection.ConnectionRecordImp connectionInfo = new Connection.ConnectionRecordImp();
				connectionInfo.Inherit = new Connection.ConnectionRecordImp.ConnectionRecordInheritanceImp(connectionInfo);
					
				connectionInfo.Name = host.HostNameWithoutDomain;
				connectionInfo.Hostname = host.HostName;
					
				switch (protocol)
				{
					case Connection.Protocol.Protocols.SSH2:
						if (host.SSH)
						{
							finalProtocol = Connection.Protocol.Protocols.SSH2;
							protocolValid = true;
						}
						break;
					case Connection.Protocol.Protocols.Telnet:
						if (host.Telnet)
						{
							finalProtocol = Connection.Protocol.Protocols.Telnet;
							protocolValid = true;
						}
						break;
					case Connection.Protocol.Protocols.HTTP:
						if (host.HTTP)
						{
							finalProtocol = Connection.Protocol.Protocols.HTTP;
							protocolValid = true;
						}
						break;
					case Connection.Protocol.Protocols.HTTPS:
						if (host.HTTPS)
						{
							finalProtocol = Connection.Protocol.Protocols.HTTPS;
							protocolValid = true;
						}
						break;
					case Connection.Protocol.Protocols.Rlogin:
						if (host.Rlogin)
						{
							finalProtocol = Connection.Protocol.Protocols.Rlogin;
							protocolValid = true;
						}
						break;
					case Connection.Protocol.Protocols.RDP:
						if (host.RDP)
						{
							finalProtocol = Connection.Protocol.Protocols.RDP;
							protocolValid = true;
						}
						break;
					case Connection.Protocol.Protocols.VNC:
						if (host.VNC)
						{
							finalProtocol = Connection.Protocol.Protocols.VNC;
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
						
					if (parentTreeNode.Tag is Container.Info)
					{
						connectionInfo.Parent = (Container.Info)parentTreeNode.Tag;
					}
						
					Runtime.ConnectionList.Add(connectionInfo);
				}
			}
		}
	}
}
