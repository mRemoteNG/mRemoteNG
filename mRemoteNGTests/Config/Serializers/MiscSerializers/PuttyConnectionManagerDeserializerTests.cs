using System.Linq;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNGTests.Properties;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.MiscSerializers;

public class PuttyConnectionManagerDeserializerTests
{
    private PuttyConnectionManagerDeserializer _deserializer;
    private ContainerInfo _rootImportedFolder;
    private const string ExpectedRootFolderName = "test_puttyConnectionManager_database";
    private const string ExpectedConnectionDisplayName = "my ssh connection";
    private const string ExpectedConnectionHostname = "server1.mydomain.com";
    private const string ExpectedConnectionDescription = "My Description Here";
    private const int ExpectedConnectionPort = 22;
    private const ProtocolType ExpectedProtocolType = ProtocolType.SSH2;
    private const string ExpectedPuttySession = "MyCustomPuttySession";
    private const string ExpectedConnectionUsername = "mysshusername";
    private const string ExpectedConnectionPassword = "password123";


    [OneTimeSetUp]
    public void OnetimeSetup()
    {
        var fileContents = Resources.test_puttyConnectionManager_database;
        _deserializer = new PuttyConnectionManagerDeserializer();
        var connectionTreeModel = _deserializer.Deserialize(fileContents);
        var rootNode = connectionTreeModel.RootNodes.First();
        _rootImportedFolder = rootNode.Children.Cast<ContainerInfo>().First();
    }

    [OneTimeTearDown]
    public void OnetimeTeardown()
    {
        _deserializer = null;
        _rootImportedFolder = null;
    }

    [Test]
    public void RootFolderImportedWithCorrectName()
    {
        Assert.That(_rootImportedFolder.Name, Is.EqualTo(ExpectedRootFolderName));
    }

    [Test]
    public void ConnectionDisplayNameImported()
    {
        var connection = GetSshConnection();
        Assert.That(connection.Name, Is.EqualTo(ExpectedConnectionDisplayName));
    }

    [Test]
    public void ConnectionHostNameImported()
    {
        var connection = GetSshConnection();
        Assert.That(connection.Hostname, Is.EqualTo(ExpectedConnectionHostname));
    }

    [Test]
    public void ConnectionDescriptionImported()
    {
        var connection = GetSshConnection();
        Assert.That(connection.Description, Is.EqualTo(ExpectedConnectionDescription));
    }

    [Test]
    public void ConnectionPortImported()
    {
        var connection = GetSshConnection();
        Assert.That(connection.Port, Is.EqualTo(ExpectedConnectionPort));
    }

    [Test]
    public void ConnectionProtocolTypeImported()
    {
        var connection = GetSshConnection();
        Assert.That(connection.Protocol, Is.EqualTo(ExpectedProtocolType));
    }

    [Test]
    public void ConnectionPuttySessionImported()
    {
        var connection = GetSshConnection();
        Assert.That(connection.PuttySession, Is.EqualTo(ExpectedPuttySession));
    }

    [Test]
    public void ConnectionUsernameImported()
    {
        var connection = GetSshConnection();
        Assert.That(connection.Username, Is.EqualTo(ExpectedConnectionUsername));
    }

    [Test]
    public void ConnectionPasswordImported()
    {
        var connection = GetSshConnection();
        Assert.That(connection.Password, Is.EqualTo(ExpectedConnectionPassword));
    }

    private ConnectionInfo GetSshConnection()
    {
        var sshFolder = _rootImportedFolder.Children.OfType<ContainerInfo>().First(node => node.Name == "SSHFolder");
        return sshFolder.Children.First();
    }
}