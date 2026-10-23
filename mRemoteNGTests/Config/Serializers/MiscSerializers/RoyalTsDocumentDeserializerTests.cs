using System;
using System.Linq;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.MiscSerializers;

public class RoyalTsDocumentDeserializerTests
{
    // Shaped like a document saved by Royal TSX: a flat list of objects linked by ID and
    // ParentID, values as child elements, secrets as encrypted base64.
    internal const string Document =
        """
        <?xml version="1.0" encoding="utf-8"?>
        <RTSZDocument>
          <RoyalDocument>
            <ID>a0000000-0000-4000-8000-000000000001</ID>
            <Name>Lab</Name>
            <PositionNr>0</PositionNr>
          </RoyalDocument>
          <RoyalTrash>
            <ID>a0000000-0000-4000-8000-000000000002</ID>
            <ParentID>a0000000-0000-4000-8000-000000000001</ParentID>
            <PositionNr>2</PositionNr>
          </RoyalTrash>
          <RoyalSSHConnection>
            <ID>a0000000-0000-4000-8000-000000000003</ID>
            <ParentID>a0000000-0000-4000-8000-000000000002</ParentID>
            <Name>Deleted</Name>
            <URI>old.example</URI>
            <PositionNr>2</PositionNr>
          </RoyalSSHConnection>
          <RoyalFolder>
            <ID>a0000000-0000-4000-8000-000000000010</ID>
            <ParentID>a0000000-0000-4000-8000-000000000001</ParentID>
            <Name>Servers</Name>
            <IsExpanded>False</IsExpanded>
            <CredentialMode>2</CredentialMode>
            <CredentialUsername>CORP\admin</CredentialUsername>
            <PositionNr>6</PositionNr>
          </RoyalFolder>
          <RoyalFolder>
            <ID>a0000000-0000-4000-8000-000000000011</ID>
            <ParentID>a0000000-0000-4000-8000-000000000001</ParentID>
            <Name>Web</Name>
            <PositionNr>4</PositionNr>
          </RoyalFolder>
          <RoyalRDSConnection>
            <ID>a0000000-0000-4000-8000-000000000020</ID>
            <ParentID>a0000000-0000-4000-8000-000000000010</ParentID>
            <Name>dc01</Name>
            <Description>Domain controller</Description>
            <URI>dc01.corp.example</URI>
            <RDPPort>4859</RDPPort>
            <CredentialMode>2</CredentialMode>
            <CredentialUsername>CORP\admin</CredentialUsername>
            <CredentialPassword>n61Qj0gmdc2Mq1kZbqvBvwW3wa4b9rC8tH3V0D9Fh4yU3hXz7r0q3qkqE5nRk1v2</CredentialPassword>
            <PositionNr>4</PositionNr>
          </RoyalRDSConnection>
          <RoyalSSHConnection>
            <ID>a0000000-0000-4000-8000-000000000021</ID>
            <ParentID>a0000000-0000-4000-8000-000000000010</ParentID>
            <Name>router</Name>
            <URI>10.0.0.1</URI>
            <Port>2222</Port>
            <IsTelnetConnection>False</IsTelnetConnection>
            <CredentialMode>2</CredentialMode>
            <CredentialUsername>netops</CredentialUsername>
            <CredentialId>a0000000-0000-4000-8000-000000000030</CredentialId>
            <PositionNr>2</PositionNr>
          </RoyalSSHConnection>
          <RoyalSSHConnection>
            <ID>a0000000-0000-4000-8000-000000000022</ID>
            <ParentID>a0000000-0000-4000-8000-000000000010</ParentID>
            <Name>switch</Name>
            <URI>10.0.0.2</URI>
            <IsTelnetConnection>True</IsTelnetConnection>
            <CredentialFromParent>True</CredentialFromParent>
            <PositionNr>6</PositionNr>
          </RoyalSSHConnection>
          <RoyalVNCConnection>
            <ID>a0000000-0000-4000-8000-000000000023</ID>
            <ParentID>a0000000-0000-4000-8000-000000000010</ParentID>
            <Name>kvm</Name>
            <URI>vnc://10.0.0.3/</URI>
            <Port>5901</Port>
            <CredentialMode>4</CredentialMode>
            <CredentialId>a0000000-0000-4000-8000-000000000030</CredentialId>
            <CredentialName>Operator</CredentialName>
            <PositionNr>8</PositionNr>
          </RoyalVNCConnection>
          <RoyalCredential>
            <ID>a0000000-0000-4000-8000-000000000030</ID>
            <ParentID>a0000000-0000-4000-8000-000000000010</ParentID>
            <Name>Operator</Name>
            <UserName>operator</UserName>
            <Password>Zm9vYmFyZm9vYmFyZm9vYmFyZm9vYmFyZm9vYmFyZm9vYmFyZm9vYmFyZm9vYmFy</Password>
            <PositionNr>10</PositionNr>
          </RoyalCredential>
          <RoyalWebConnection>
            <ID>a0000000-0000-4000-8000-000000000040</ID>
            <ParentID>a0000000-0000-4000-8000-000000000011</ParentID>
            <Name>portal</Name>
            <URI>https://portal.example:8443/login</URI>
            <PositionNr>2</PositionNr>
          </RoyalWebConnection>
          <RoyalWebConnection>
            <ID>a0000000-0000-4000-8000-000000000041</ID>
            <ParentID>a0000000-0000-4000-8000-000000000011</ParentID>
            <Name>intranet</Name>
            <URI>http://intranet.example/</URI>
            <PositionNr>4</PositionNr>
          </RoyalWebConnection>
          <RoyalApplicationConnection>
            <ID>a0000000-0000-4000-8000-000000000042</ID>
            <ParentID>a0000000-0000-4000-8000-000000000011</ParentID>
            <Name>notepad</Name>
            <PositionNr>6</PositionNr>
          </RoyalApplicationConnection>
        </RTSZDocument>
        """;

    private RoyalTsDocumentDeserializer _deserializer;
    private ConnectionTreeModel _model;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _deserializer = new RoyalTsDocumentDeserializer();
        _model = _deserializer.Deserialize(Document);
    }

    private ContainerInfo Root => _model.RootNodes.First();

    private ConnectionInfo Node(string name) => _model.GetRecursiveChildList().Single(node => node.Name == name);

    [Test]
    public void DocumentNameIsRead()
    {
        Assert.That(_deserializer.DocumentName, Is.EqualTo("Lab"));
    }

    [Test]
    public void TopLevelFoldersFollowPositionNr()
    {
        Assert.That(Root.Children.Select(node => node.Name), Is.EqualTo(new[] { "Web", "Servers" }));
    }

    [Test]
    public void ConnectionsInAFolderFollowPositionNr()
    {
        var servers = (ContainerInfo)Node("Servers");
        Assert.That(servers.Children.Select(node => node.Name), Is.EqualTo(new[] { "router", "dc01", "switch", "kvm" }));
    }

    [Test]
    public void FolderExpansionIsKept()
    {
        Assert.That(((ContainerInfo)Node("Servers")).IsExpanded, Is.False);
        Assert.That(((ContainerInfo)Node("Web")).IsExpanded, Is.True);
    }

    [Test]
    public void RdsConnectionIsImportedAsRdp()
    {
        var connection = Node("dc01");
        Assert.That(connection.Protocol, Is.EqualTo(ProtocolType.RDP));
        Assert.That(connection.Hostname, Is.EqualTo("dc01.corp.example"));
        Assert.That(connection.Port, Is.EqualTo(4859));
        Assert.That(connection.Description, Is.EqualTo("Domain controller"));
    }

    [Test]
    public void DomainAndUsernameAreSplit()
    {
        var connection = Node("dc01");
        Assert.That(connection.Domain, Is.EqualTo("CORP"));
        Assert.That(connection.Username, Is.EqualTo("admin"));
    }

    [Test]
    public void SshConnectionKeepsItsPortAndUsername()
    {
        var connection = Node("router");
        Assert.That(connection.Protocol, Is.EqualTo(ProtocolType.SSH2));
        Assert.That(connection.Port, Is.EqualTo(2222));
        Assert.That(connection.Username, Is.EqualTo("netops"));
    }

    [Test]
    public void SshConnectionWithTheTelnetFlagIsTelnetOnItsDefaultPort()
    {
        var connection = Node("switch");
        Assert.That(connection.Protocol, Is.EqualTo(ProtocolType.Telnet));
        Assert.That(connection.Port, Is.EqualTo(connection.GetDefaultPort()));
    }

    [Test]
    public void CredentialsFromTheParentFolderBecomeInheritance()
    {
        var connection = Node("switch");
        Assert.That(connection.Inheritance.Username, Is.True);
        Assert.That(connection.Inheritance.Domain, Is.True);
        Assert.That(connection.Inheritance.Password, Is.True);
        Assert.That(connection.Username, Is.EqualTo("admin"));
    }

    [Test]
    public void SharedCredentialGivesItsUsername()
    {
        var connection = Node("kvm");
        Assert.That(connection.Protocol, Is.EqualTo(ProtocolType.VNC));
        Assert.That(connection.Hostname, Is.EqualTo("10.0.0.3"));
        Assert.That(connection.Port, Is.EqualTo(5901));
        Assert.That(connection.Username, Is.EqualTo("operator"));
    }

    [Test]
    public void WebConnectionsKeepTheWholeUrl()
    {
        var portal = Node("portal");
        var intranet = Node("intranet");
        Assert.That(portal.Protocol, Is.EqualTo(ProtocolType.HTTPS));
        Assert.That(portal.Hostname, Is.EqualTo("https://portal.example:8443/login"));
        Assert.That(portal.Port, Is.EqualTo(portal.GetDefaultPort()));
        Assert.That(intranet.Protocol, Is.EqualTo(ProtocolType.HTTP));
        Assert.That(intranet.Hostname, Is.EqualTo("http://intranet.example/"));
    }

    [Test]
    public void ObjectsInTheTrashAreLeftOut()
    {
        Assert.That(_model.GetRecursiveChildList().Any(node => node.Name == "Deleted"), Is.False);
    }

    [Test]
    public void TypesWithoutAnEquivalentAreCountedAndLeftOut()
    {
        Assert.That(_model.GetRecursiveChildList().Any(node => node.Name == "notepad"), Is.False);
        Assert.That(_deserializer.SkippedObjectTypes["RoyalApplicationConnection"], Is.EqualTo(1));
        Assert.That(_deserializer.SkippedObjectTypes, Has.Count.EqualTo(1));
    }

    [Test]
    public void PasswordsAreNotImported()
    {
        Assert.That(Node("dc01").Password, Is.Null.Or.Empty);
        Assert.That(Node("kvm").Password, Is.Null.Or.Empty);
        Assert.That(_deserializer.PasswordsNotImported, Is.EqualTo(2));
    }

    [Test]
    public void AnotherXmlFileIsRejected()
    {
        Assert.Throws<FormatException>(() => new RoyalTsDocumentDeserializer().Deserialize("<Connections />"));
    }
}
