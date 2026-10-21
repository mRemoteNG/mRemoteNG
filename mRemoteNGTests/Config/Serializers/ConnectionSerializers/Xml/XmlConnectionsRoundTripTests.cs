using System.Globalization;
using System.Linq;
using System.Threading;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.Terminal;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.ConnectionSerializers.Xml;

/// <summary>
/// The app saves with <see cref="XmlConnectionSerializerFactory"/> and loads with
/// <see cref="XmlConnectionsDeserializer"/>. These must agree on the file format version,
/// or a saved connections file can no longer be opened.
/// </summary>
public class XmlConnectionsRoundTripTests
{
    private ConnectionTreeModel _model;
    private RootNodeInfo _root;
    private ConnectionInfo _connection;

    [SetUp]
    public void Setup()
    {
        _model = new ConnectionTreeModel();
        _root = new RootNodeInfo(RootNodeType.Connection);
        var folder = new ContainerInfo { Name = "folder" };
        _connection = new ConnectionInfo
        {
            Name = "con1",
            Hostname = "server01",
            Username = "user1",
            TerminalBackend = TerminalBackend.WindowsTerminal
        };
        _root.AddChild(folder);
        folder.AddChild(_connection);
        _model.AddRootNode(_root);
    }

    private string SaveLikeTheApp()
    {
        var serializer = new XmlConnectionSerializerFactory().Build(new AeadCryptographyProvider(), _model);
        return serializer.Serialize(_root);
    }

    [Test]
    public void SavedFileVersionIsSupportedByTheLoader()
    {
        var xml = SecureXmlHelper.LoadXmlFromString(SaveLikeTheApp());
        double savedVersion = double.Parse(xml.DocumentElement.GetAttribute("ConfVersion"), CultureInfo.InvariantCulture);

        Assert.That(savedVersion, Is.LessThanOrEqualTo(XmlConnectionsDeserializer.MaxSupportedConfVersion));
    }

    [Test]
    [Apartment(ApartmentState.STA)]
    public void SavedFileCanBeLoadedBack()
    {
        string savedXml = SaveLikeTheApp();

        var deserializer = new XmlConnectionsDeserializer(() => _root.PasswordString.ConvertToSecureString());
        ConnectionTreeModel loaded = deserializer.Deserialize(savedXml);

        ConnectionInfo loadedConnection = loaded.GetRecursiveChildList().Single(c => c.Name == "con1");
        Assert.That(loadedConnection.Hostname, Is.EqualTo("server01"));
        Assert.That(loadedConnection.Username, Is.EqualTo("user1"));
        Assert.That(loadedConnection.TerminalBackend, Is.EqualTo(TerminalBackend.WindowsTerminal));
    }
}
