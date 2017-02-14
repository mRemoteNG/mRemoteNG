using System.Collections.Generic;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.Config.Serializers
{
    public class PortScanDeserializer : IDeserializer
    {
        private readonly IEnumerable<ScanHost> _scannedHosts;
        private readonly ProtocolType _targetProtocolType;

        public PortScanDeserializer(IEnumerable<ScanHost> scannedHosts, ProtocolType targetProtocolType)
        {
            _scannedHosts = scannedHosts;
            _targetProtocolType = targetProtocolType;
        }

        public ConnectionTreeModel Deserialize()
        {
            var connectionTreeModel = new ConnectionTreeModel();
            var root = new RootNodeInfo(RootNodeType.Connection);
            connectionTreeModel.AddRootNode(root);

            foreach (var host in _scannedHosts)
                ImportScannedHost(host, root);

            return connectionTreeModel;
        }

        private void ImportScannedHost(ScanHost host, ContainerInfo parentContainer)
        {
            var finalProtocol = default(ProtocolType);
            var protocolValid = true;

            switch (_targetProtocolType)
            {
                case ProtocolType.SSH2:
                    if (host.SSH)
                        finalProtocol = ProtocolType.SSH2;
                    break;
                case ProtocolType.Telnet:
                    if (host.Telnet)
                        finalProtocol = ProtocolType.Telnet;
                    break;
                case ProtocolType.HTTP:
                    if (host.HTTP)
                        finalProtocol = ProtocolType.HTTP;
                    break;
                case ProtocolType.HTTPS:
                    if (host.HTTPS)
                        finalProtocol = ProtocolType.HTTPS;
                    break;
                case ProtocolType.Rlogin:
                    if (host.Rlogin)
                        finalProtocol = ProtocolType.Rlogin;
                    break;
                case ProtocolType.RDP:
                    if (host.RDP)
                        finalProtocol = ProtocolType.RDP;
                    break;
                case ProtocolType.VNC:
                    if (host.VNC)
                        finalProtocol = ProtocolType.VNC;
                    break;
                default:
                    protocolValid = false;
                    break;
            }

            if (!protocolValid) return;
            var newConnectionInfo = new ConnectionInfo
            {
                Name = host.HostNameWithoutDomain,
                Hostname = host.HostName,
                Protocol = finalProtocol
            };
            newConnectionInfo.SetDefaultPort();

            parentContainer.AddChild(newConnectionInfo);
        }
    }
}