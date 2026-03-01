using System.Collections.Generic;
using System.Runtime.Versioning;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.MiscSerializers
{
    [SupportedOSPlatform("windows")]
    public class PortScanDeserializer(ProtocolType targetProtocolType) : IDeserializer<IEnumerable<ScanHost>, ConnectionTreeModel>
    {
        private readonly ProtocolType _targetProtocolType = targetProtocolType;

        public ConnectionTreeModel Deserialize(IEnumerable<ScanHost> scannedHosts)
        {
            ConnectionTreeModel connectionTreeModel = new();
            RootNodeInfo root = new(RootNodeType.Connection);
            connectionTreeModel.AddRootNode(root);

            foreach (ScanHost host in scannedHosts)
                ImportScannedHost(host, root);

            return connectionTreeModel;
        }

        private void ImportScannedHost(ScanHost host, ContainerInfo parentContainer)
        {
            ProtocolType finalProtocol = default(ProtocolType);
            bool protocolValid = true;

            switch (_targetProtocolType)
            {
                case ProtocolType.SSH2:
                    if (host.Ssh)
                        finalProtocol = ProtocolType.SSH2;
                    break;
                case ProtocolType.Telnet:
                    if (host.Telnet)
                        finalProtocol = ProtocolType.Telnet;
                    break;
                case ProtocolType.HTTP:
                    if (host.Http)
                        finalProtocol = ProtocolType.HTTP;
                    break;
                case ProtocolType.HTTPS:
                    if (host.Https)
                        finalProtocol = ProtocolType.HTTPS;
                    break;
                case ProtocolType.Rlogin:
                    if (host.Rlogin)
                        finalProtocol = ProtocolType.Rlogin;
                    break;
                case ProtocolType.RDP:
                    if (host.Rdp)
                        finalProtocol = ProtocolType.RDP;
                    break;
                case ProtocolType.VNC:
                    if (host.Vnc)
                        finalProtocol = ProtocolType.VNC;
                    break;
                case ProtocolType.ARD:
                    if (host.Vnc)
                        finalProtocol = ProtocolType.ARD;
                    break;
                default:
                    protocolValid = false;
                    break;
            }

            if (!protocolValid) return;
            ConnectionInfo newConnectionInfo = new()
            {
                Name = host.HostNameWithoutDomain,
                Hostname = string.IsNullOrEmpty(host.HostName) ? host.HostIp : host.HostName,
                Protocol = finalProtocol
            };
            newConnectionInfo.SetDefaultPort();

            parentContainer.AddChild(newConnectionInfo);
        }
    }
}