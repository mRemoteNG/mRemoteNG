using System.Linq;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Tools;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.MiscSerializers;

public class PortScanDeserializerTests
{
    private PortScanDeserializer _deserializer;
    private ConnectionInfo _importedConnectionInfo;
    private const string ExpectedHostName = "server1.domain.com";
    private const string ExpectedDisplayName = "server1";
    private const ProtocolType ExpectedProtocolType = ProtocolType.SSH2;


    [OneTimeSetUp]
    public void OnetimeSetup()
    {
        var host = new ScanHost("10.20.30.40")
        {
            HostName = "server1.domain.com",
            Ssh = true
        };
        _deserializer = new PortScanDeserializer(ProtocolType.SSH2);
        var connectionTreeModel = _deserializer.Deserialize(new[] { host });
        var root = connectionTreeModel.RootNodes.First();
        _importedConnectionInfo = root.Children.First();
    }

    [OneTimeTearDown]
    public void OnetimeTeardown()
    {
        _deserializer = null;
        _importedConnectionInfo = null;
    }

    [Test]
    public void DisplayNameImported()
    {
        Assert.That(_importedConnectionInfo.Name, Is.EqualTo(ExpectedDisplayName));
    }

    [Test]
    public void HostNameImported()
    {
        Assert.That(_importedConnectionInfo.Hostname, Is.EqualTo(ExpectedHostName));
    }

    [Test]
    public void ProtocolImported()
    {
        Assert.That(_importedConnectionInfo.Protocol, Is.EqualTo(ExpectedProtocolType));
    }
}