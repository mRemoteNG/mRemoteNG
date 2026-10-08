using System.Collections.Generic;
using System.Linq;
using mRemoteNG.PluginContracts;
using mRemoteNG.Plugins.PortScan;
using NUnit.Framework;

namespace mRemoteNGTests.Plugins;

[TestFixture]
public class PortScanConnectionRequestBuilderTests
{
    [Test]
    public void Build_ImportsMatchingProtocolUsingHostname()
    {
        PortScanHostResult matchingHost = CreateHost("192.0.2.10", "server.example.com");
        matchingHost.Ssh = true;
        PortScanHostResult nonMatchingHost = CreateHost("192.0.2.11", "other.example.com");

        List<PluginConnectionRequest> requests = PortScanConnectionRequestBuilder.Build(
            [matchingHost, nonMatchingHost],
            PluginProtocolIds.Ssh2,
            useIpAddress: false,
            importAllOpenPorts: false);

        Assert.That(requests, Has.Count.EqualTo(1));
        Assert.That(requests[0].Hostname, Is.EqualTo("server.example.com"));
        Assert.That(requests[0].Name, Is.EqualTo("server"));
        Assert.That(requests[0].Port, Is.Null);
    }

    [Test]
    public void Build_ImportsEveryOpenPortUsingIpAddress()
    {
        PortScanHostResult host = CreateHost("192.0.2.10", "server.example.com");
        host.OpenPorts.AddRange([8080, 8443]);

        List<PluginConnectionRequest> requests = PortScanConnectionRequestBuilder.Build(
            [host],
            PluginProtocolIds.Http,
            useIpAddress: true,
            importAllOpenPorts: true);

        Assert.That(requests.Select(request => (request.Hostname, request.Port)),
            Is.EqualTo(new[] { ("192.0.2.10", (int?)8080), ("192.0.2.10", (int?)8443) }));
        Assert.That(requests, Has.All.Property(nameof(PluginConnectionRequest.ProtocolId)).EqualTo(PluginProtocolIds.Http));
    }

    [Test]
    public void HostNameDisplay_DoesNotRepeatIpAddress()
    {
        PortScanHostResult host = CreateHost("192.0.2.10", "192.0.2.10");

        Assert.That(host.HostNameDisplay, Is.Empty);
        Assert.That(host.HostIp, Is.EqualTo("192.0.2.10"));
    }

    private static PortScanHostResult CreateHost(string ip, string hostname)
    {
        return new PortScanHostResult
        {
            HostIp = ip,
            HostName = hostname,
        };
    }
}
