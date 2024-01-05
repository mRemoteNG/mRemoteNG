using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using NUnit.Framework;
using mRemoteNGTests.Properties;
using mRemoteNG.Tree;
using System.Runtime.Versioning;

namespace mRemoteNGTests.Config.Serializers.MiscSerializers;

[SupportedOSPlatform("windows")]
public class SecureCRTFileDeserializerTests
{
    private SecureCRTFileDeserializer _deserializer;
    private ConnectionTreeModel _connectionTreeModel;

    [OneTimeSetUp]
    public void OnetimeSetup()
    {
        var fileContents = Resources.test_securecrt;
        _deserializer = new SecureCRTFileDeserializer();
        _connectionTreeModel = _deserializer.Deserialize(fileContents);        
    }

    [Test]
    public void HaveContainerNamedAllConnectionTypes()
    {
        var rootNode = GetContainerNamed("Connections", _connectionTreeModel.RootNodes);
        var sessionsNode = GetContainerNamed("Sessions", rootNode.Children);
        var allConnectionTypesNode = GetContainerNamed("AllConnectionTypes", sessionsNode.Children);
        Assert.That(allConnectionTypesNode, Is.Not.Null);
        Assert.That(allConnectionTypesNode.IsContainer, Is.True);
        Assert.That(allConnectionTypesNode.Name, Is.EqualTo("AllConnectionTypes"));
    }

    [Test]
    public void TestRawConnectionInfo()
    {
        var rootNode = GetContainerNamed("Connections", _connectionTreeModel.RootNodes);
        var sessionsNode = GetContainerNamed("Sessions", rootNode.Children);
        var allConnectionTypesNode = GetContainerNamed("AllConnectionTypes", sessionsNode.Children);
        var rawNode = GetConnectionNamed("rawsession", allConnectionTypesNode.Children);
        Assert.That(rawNode.Name, Is.EqualTo("rawsession"));
        Assert.That(rawNode.Hostname, Is.EqualTo("rawhost"));
        Assert.That(rawNode.Protocol, Is.EqualTo(ProtocolType.RAW));
        Assert.That(rawNode.Port, Is.EqualTo(23));
        Assert.That(rawNode.Username, Is.EqualTo(""));
    }

    [Test] 
    public void TestRDPConnectionInfo()
    {
        var rootNode = GetContainerNamed("Connections", _connectionTreeModel.RootNodes);
        var sessionsNode = GetContainerNamed("Sessions", rootNode.Children);
        var allConnectionTypesNode = GetContainerNamed("AllConnectionTypes", sessionsNode.Children);
        var rdpNode = GetConnectionNamed("RDPsession", allConnectionTypesNode.Children);
        Assert.That(rdpNode.Name, Is.EqualTo("RDPsession"));
        Assert.That(rdpNode.Hostname, Is.EqualTo("RDPhost"));
        Assert.That(rdpNode.Protocol, Is.EqualTo(ProtocolType.RDP));
        Assert.That(rdpNode.Port, Is.EqualTo(3389));
        Assert.That(rdpNode.Username, Is.EqualTo("RDP\\rdp"));
    }

    [Test]
    public void TestRloginConnection() {
        var rootNode = GetContainerNamed("Connections", _connectionTreeModel.RootNodes);
        var sessionsNode = GetContainerNamed("Sessions", rootNode.Children);
        var allConnectionTypesNode = GetContainerNamed("AllConnectionTypes", sessionsNode.Children);

        var rloginNode = GetConnectionNamed("rloginsession", allConnectionTypesNode.Children);
        Assert.That(rloginNode.Name, Is.EqualTo("rloginsession"));
        Assert.That(rloginNode.Hostname, Is.EqualTo("rloginhost"));
        Assert.That(rloginNode.Protocol, Is.EqualTo(ProtocolType.Rlogin));
        Assert.That(rloginNode.Port, Is.EqualTo(0));
        Assert.That(rloginNode.Username, Is.EqualTo("rloginuser"));
    }

    [Test]
    public void TestSSH1Connection()
    {
        var rootNode = GetContainerNamed("Connections", _connectionTreeModel.RootNodes);
        var sessionsNode = GetContainerNamed("Sessions", rootNode.Children);
        var allConnectionTypesNode = GetContainerNamed("AllConnectionTypes", sessionsNode.Children);

        var ssh1Node = GetConnectionNamed("ssh1session", allConnectionTypesNode.Children);
        Assert.That(ssh1Node.Name, Is.EqualTo("ssh1session"));
        Assert.That(ssh1Node.Hostname, Is.EqualTo("ssh1host"));
        Assert.That(ssh1Node.Protocol, Is.EqualTo(ProtocolType.SSH1));
        Assert.That(ssh1Node.Port, Is.EqualTo(22));
        Assert.That(ssh1Node.Username, Is.EqualTo("ssh1user"));
    }

    [Test]
    public void TestSSH2Connection()
    {
        var rootNode = GetContainerNamed("Connections", _connectionTreeModel.RootNodes);
        var sessionsNode = GetContainerNamed("Sessions", rootNode.Children);
        var allConnectionTypesNode = GetContainerNamed("AllConnectionTypes", sessionsNode.Children);

        var ssh1Node = GetConnectionNamed("ssh2session", allConnectionTypesNode.Children);
        Assert.That(ssh1Node.Name, Is.EqualTo("ssh2session"));
        Assert.That(ssh1Node.Hostname, Is.EqualTo("ssh2host"));
        Assert.That(ssh1Node.Protocol, Is.EqualTo(ProtocolType.SSH2));
        Assert.That(ssh1Node.Port, Is.EqualTo(22));
        Assert.That(ssh1Node.Username, Is.EqualTo("ssh2user"));
    }

    [Test]
    public void TestTelnetConnection()
    {
        var rootNode = GetContainerNamed("Connections", _connectionTreeModel.RootNodes);
        var sessionsNode = GetContainerNamed("Sessions", rootNode.Children);
        var allConnectionTypesNode = GetContainerNamed("AllConnectionTypes", sessionsNode.Children);

        var ssh1Node = GetConnectionNamed("telnetsession", allConnectionTypesNode.Children);
        Assert.That(ssh1Node.Name, Is.EqualTo("telnetsession"));
        Assert.That(ssh1Node.Hostname, Is.EqualTo("telnethost"));
        Assert.That(ssh1Node.Protocol, Is.EqualTo(ProtocolType.Telnet));
        Assert.That(ssh1Node.Port, Is.EqualTo(23));
        Assert.That(ssh1Node.Username, Is.EqualTo("telnetuser"));
    }

    [Test]
    public void TestDescriptionField()
    {
        var rootNode = GetContainerNamed("Connections", _connectionTreeModel.RootNodes);
        var sessionsNode = GetContainerNamed("Sessions", rootNode.Children);
        var host1Node = GetConnectionNamed("host1.org", sessionsNode.Children);
        Assert.That(host1Node.Description, Is.EqualTo("First Second 123456"));
    }

    [Test]
    public void TestValidateFileStructure() 
    {
        var rootNode = GetContainerNamed("Connections", _connectionTreeModel.RootNodes);
        var sessionsNode = GetContainerNamed("Sessions", rootNode.Children);
        Assert.That(sessionsNode.Children.Count, Is.EqualTo(3));

        var allConnectionTypesNode = GetContainerNamed("AllConnectionTypes", sessionsNode.Children);
        Assert.That(allConnectionTypesNode.Children.Count, Is.EqualTo(6));

        var serverSubfolderNode = GetContainerNamed("server_subfolder", sessionsNode.Children);
        Assert.That(serverSubfolderNode, Is.Not.Null);
        Assert.That(serverSubfolderNode.Children.Count, Is.EqualTo(2));
        var serverSubSubfolderNode = GetContainerNamed("server_subsubfolder", serverSubfolderNode.Children);
        Assert.That(serverSubSubfolderNode, Is.Not.Null);
        Assert.That(serverSubSubfolderNode.Children.Count, Is.EqualTo(2));
    }

    private ContainerInfo GetContainerNamed(string name, IEnumerable<ConnectionInfo> list)
    {
        return list.First(node => node is ContainerInfo && node.Name == name) as ContainerInfo;
    }

    private ConnectionInfo GetConnectionNamed(string name, IEnumerable<ConnectionInfo> list)
    {
        return list.First(node => node is ConnectionInfo && node.Name == name);
    }

    private bool ContainsNodeNamed(string name, IEnumerable<ConnectionInfo> list)
    {
        return list.Any(node => node.Name == name);
    }
}
