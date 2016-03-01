using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.App.Runtime;

namespace mRemoteNG.Config.Import
{
	public class PortScan
	{
		public static void Import(IEnumerable hosts, Connection.Protocol.Protocols protocol, TreeNode parentTreeNode)
		{
			foreach (Tools.PortScan.ScanHost host in hosts) {
				Connection.Protocol.Protocols finalProtocol = default(Connection.Protocol.Protocols);
				bool protocolValid = false;

				TreeNode treeNode = mRemoteNG.Tree.Node.AddNode(mRemoteNG.Tree.Node.Type.Connection, host.HostNameWithoutDomain);

				Connection.Info connectionInfo = new Connection.Info();
				connectionInfo.Inherit = new Connection.Info.Inheritance(connectionInfo);

				connectionInfo.Name = host.HostNameWithoutDomain;
				connectionInfo.Hostname = host.HostName;

				switch (protocol) {
					case mRemoteNG.Connection.Protocol.Protocols.SSH2:
						if (host.SSH) {
							finalProtocol = mRemoteNG.Connection.Protocol.Protocols.SSH2;
							protocolValid = true;
						}
						break;
					case mRemoteNG.Connection.Protocol.Protocols.Telnet:
						if (host.Telnet) {
							finalProtocol = mRemoteNG.Connection.Protocol.Protocols.Telnet;
							protocolValid = true;
						}
						break;
					case mRemoteNG.Connection.Protocol.Protocols.HTTP:
						if (host.HTTP) {
							finalProtocol = mRemoteNG.Connection.Protocol.Protocols.HTTP;
							protocolValid = true;
						}
						break;
					case mRemoteNG.Connection.Protocol.Protocols.HTTPS:
						if (host.HTTPS) {
							finalProtocol = mRemoteNG.Connection.Protocol.Protocols.HTTPS;
							protocolValid = true;
						}
						break;
					case mRemoteNG.Connection.Protocol.Protocols.Rlogin:
						if (host.Rlogin) {
							finalProtocol = mRemoteNG.Connection.Protocol.Protocols.Rlogin;
							protocolValid = true;
						}
						break;
					case mRemoteNG.Connection.Protocol.Protocols.RDP:
						if (host.RDP) {
							finalProtocol = mRemoteNG.Connection.Protocol.Protocols.RDP;
							protocolValid = true;
						}
						break;
					case mRemoteNG.Connection.Protocol.Protocols.VNC:
						if (host.VNC) {
							finalProtocol = mRemoteNG.Connection.Protocol.Protocols.VNC;
							protocolValid = true;
						}
						break;
				}

				if (protocolValid) {
					connectionInfo.Protocol = finalProtocol;
					connectionInfo.SetDefaultPort();

					treeNode.Tag = connectionInfo;
					parentTreeNode.Nodes.Add(treeNode);

					if (parentTreeNode.Tag is Container.Info) {
						connectionInfo.Parent = parentTreeNode.Tag;
					}

					mRemoteNG.App.Runtime.ConnectionList.Add(connectionInfo);
				}
			}
		}
	}
}
