using mRemoteNG.PluginContracts;

namespace mRemoteNG.Plugins.PortScan;

internal static class PortScanConnectionRequestBuilder
{
    public static List<PluginConnectionRequest> Build(
        IEnumerable<PortScanHostResult> hosts,
        string protocolId,
        bool useIpAddress,
        bool importAllOpenPorts)
    {
        List<PluginConnectionRequest> requests = [];
        foreach (PortScanHostResult host in hosts)
        {
            if (importAllOpenPorts)
            {
                requests.AddRange(host.OpenPorts.Select(port => CreateRequest(host, protocolId, useIpAddress, port)));
            }
            else if (SupportsProtocol(host, protocolId))
            {
                requests.Add(CreateRequest(host, protocolId, useIpAddress));
            }
        }

        return requests;
    }

    private static PluginConnectionRequest CreateRequest(
        PortScanHostResult host,
        string protocolId,
        bool useIpAddress,
        int? port = null)
    {
        return new PluginConnectionRequest
        {
            Hostname = useIpAddress || string.IsNullOrWhiteSpace(host.HostNameDisplay) ? host.HostIp : host.HostName,
            Name = host.HostNameWithoutDomain,
            Port = port,
            ProtocolId = protocolId,
        };
    }

    private static bool SupportsProtocol(PortScanHostResult host, string protocolId)
    {
        return protocolId switch
        {
            PluginProtocolIds.Ard => host.Vnc,
            PluginProtocolIds.Http => host.Http,
            PluginProtocolIds.Https => host.Https,
            PluginProtocolIds.Rdp => host.Rdp,
            PluginProtocolIds.Rlogin => host.Rlogin,
            PluginProtocolIds.Ssh2 => host.Ssh,
            PluginProtocolIds.Telnet => host.Telnet,
            PluginProtocolIds.Vnc => host.Vnc,
            _ => false,
        };
    }
}
