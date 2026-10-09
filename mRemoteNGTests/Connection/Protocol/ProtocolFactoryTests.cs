using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Terminal;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol;

// Coverage for the per-connection terminal backend switch: SSH connections keep the embedded
// PuTTYNG terminal by default and embed the Windows console (ProtocolTerminal) when the user
// selects TerminalBackend.WindowsTerminal.
[TestFixture]
public class ProtocolFactoryTests
{
    private ProtocolFactory _factory;

    [SetUp]
    public void Setup()
    {
        _factory = new ProtocolFactory();
    }

    [TestCase(ProtocolType.SSH1)]
    [TestCase(ProtocolType.SSH2)]
    public void SshDefaultsToPuttyBackend(ProtocolType protocol)
    {
        var connectionInfo = new ConnectionInfo
        {
            Protocol = protocol,
            TerminalBackend = TerminalBackend.PuttyNg
        };

        var result = _factory.CreateProtocol(connectionInfo);

        Assert.That(result, Is.InstanceOf<PuttyBase>());
    }

    [TestCase(ProtocolType.SSH1)]
    [TestCase(ProtocolType.SSH2)]
    public void SshUsesTerminalBackendWhenWindowsTerminalSelected(ProtocolType protocol)
    {
        var connectionInfo = new ConnectionInfo
        {
            Protocol = protocol,
            TerminalBackend = TerminalBackend.WindowsTerminal
        };

        var result = _factory.CreateProtocol(connectionInfo);

        Assert.That(result, Is.InstanceOf<ProtocolTerminal>());
    }

    [Test]
    public void TelnetIgnoresTerminalBackend()
    {
        var connectionInfo = new ConnectionInfo
        {
            Protocol = ProtocolType.Telnet,
            TerminalBackend = TerminalBackend.WindowsTerminal
        };

        var result = _factory.CreateProtocol(connectionInfo);

        Assert.That(result, Is.Not.InstanceOf<ProtocolTerminal>());
    }
}
