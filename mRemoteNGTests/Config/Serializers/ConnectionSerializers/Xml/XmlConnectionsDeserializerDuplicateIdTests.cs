using System;
using System.Linq;
using System.Xml.Linq;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;
using mRemoteNG.Tree;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.ConnectionSerializers.Xml;

public class XmlConnectionsDeserializerDuplicateIdTests
{
    private const string Password = "mR3m";
    private const string SharedId = "2f8c7a1e-3b6d-4c9a-9e1f-5a7b8c9d0e1f";
    private const string OtherId = "7d1e2f3a-4b5c-4d6e-8f9a-0b1c2d3e4f5a";
    private const string ThirdId = "c4a5b6d7-8e9f-4a0b-9c1d-2e3f4a5b6c7d";

    // Reuse the same Protected hash from v2.6 fixtures (password = "mR3m", AES-GCM, 1000 iterations).
    private const string RootAttributes =
        """
        Name="Connections" Export="False" EncryptionEngine="AES" BlockCipherMode="GCM"
        KdfIterations="1000" FullFileEncryption="False"
        Protected="8LmIO3+MWBY0zTmfjfOEdCGxhTAwnlohb1veTGNZFt6lAYvY2UOzWyjVzkx6V93smpbP0ZOuexN15u7rvwJEjawC"
        ConfVersion="2.8"
        """;

    private static ConnectionTreeModel Deserialize(string xml)
    {
        var deserializer = new XmlConnectionsDeserializer(() => Password.ConvertToSecureString());
        return deserializer.Deserialize(xml);
    }

    private static string IdOf(ConnectionTreeModel model, string name)
    {
        return model.GetRecursiveChildList().First(node => node.Name == name).ConstantID;
    }

    [Test]
    public void SecondConnectionWithAnIdAlreadyInUseGetsAFreshOne()
    {
        string xml =
            $"""
            <?xml version="1.0" encoding="utf-8"?>
            <Connections {RootAttributes}>
                <Node Name="First" Type="Connection" Id="{SharedId}" />
                <Node Name="Second" Type="Connection" Id="{SharedId}" />
            </Connections>
            """;

        var model = Deserialize(xml);

        Assert.That(IdOf(model, "First"), Is.EqualTo(SharedId));
        Assert.That(IdOf(model, "Second"), Is.Not.EqualTo(SharedId));
        Assert.That(Guid.TryParse(IdOf(model, "Second"), out _), Is.True);
    }

    [Test]
    public void ConnectionInAFolderReusingAnIdFromOutsideItGetsAFreshOne()
    {
        string xml =
            $"""
            <?xml version="1.0" encoding="utf-8"?>
            <Connections {RootAttributes}>
                <Node Name="Outside" Type="Connection" Id="{SharedId}" />
                <Node Name="Folder" Type="Container" Expanded="True" Id="{OtherId}">
                    <Node Name="Inside" Type="Connection" Id="{SharedId}" />
                </Node>
            </Connections>
            """;

        var model = Deserialize(xml);

        Assert.That(IdOf(model, "Outside"), Is.EqualTo(SharedId));
        Assert.That(IdOf(model, "Inside"), Is.Not.EqualTo(SharedId));
    }

    [Test]
    public void DistinctIdsAreKept()
    {
        string xml =
            $"""
            <?xml version="1.0" encoding="utf-8"?>
            <Connections {RootAttributes}>
                <Node Name="First" Type="Connection" Id="{SharedId}" />
                <Node Name="Second" Type="Connection" Id="{OtherId}" />
            </Connections>
            """;

        var model = Deserialize(xml);

        Assert.That(IdOf(model, "First"), Is.EqualTo(SharedId));
        Assert.That(IdOf(model, "Second"), Is.EqualTo(OtherId));
    }

    [Test]
    public void IdsDoNotCarryOverBetweenFiles()
    {
        string xml =
            $"""
            <?xml version="1.0" encoding="utf-8"?>
            <Connections {RootAttributes}>
                <Node Name="First" Type="Connection" Id="{SharedId}" />
            </Connections>
            """;
        var deserializer = new XmlConnectionsDeserializer(() => Password.ConvertToSecureString());

        deserializer.Deserialize(xml);
        var secondLoad = deserializer.Deserialize(xml);

        Assert.That(IdOf(secondLoad, "First"), Is.EqualTo(SharedId));
    }

    [Test]
    public void FolderKeepsItsIdFromTheFile()
    {
        string xml =
            $"""
            <?xml version="1.0" encoding="utf-8"?>
            <Connections {RootAttributes}>
                <Node Name="Folder" Type="Container" Expanded="True" Id="{SharedId}" />
            </Connections>
            """;

        var model = Deserialize(xml);

        Assert.That(IdOf(model, "Folder"), Is.EqualTo(SharedId));
    }

    [Test]
    public void NestedFolderKeepsItsIdFromTheFile()
    {
        string xml =
            $"""
            <?xml version="1.0" encoding="utf-8"?>
            <Connections {RootAttributes}>
                <Node Name="Outer" Type="Container" Expanded="True" Id="{OtherId}">
                    <Node Name="Inner" Type="Container" Expanded="False" Id="{SharedId}">
                        <Node Name="Leaf" Type="Connection" Id="{ThirdId}" />
                    </Node>
                </Node>
            </Connections>
            """;

        var model = Deserialize(xml);

        Assert.That(IdOf(model, "Outer"), Is.EqualTo(OtherId));
        Assert.That(IdOf(model, "Inner"), Is.EqualTo(SharedId));
        Assert.That(IdOf(model, "Leaf"), Is.EqualTo(ThirdId));
    }

    [Test]
    public void FolderReusingAnIdAlreadyInUseGetsAFreshOne()
    {
        string xml =
            $"""
            <?xml version="1.0" encoding="utf-8"?>
            <Connections {RootAttributes}>
                <Node Name="Connection" Type="Connection" Id="{SharedId}" />
                <Node Name="Folder" Type="Container" Expanded="True" Id="{SharedId}" />
            </Connections>
            """;

        var model = Deserialize(xml);

        Assert.That(IdOf(model, "Connection"), Is.EqualTo(SharedId));
        Assert.That(IdOf(model, "Folder"), Is.Not.EqualTo(SharedId));
        Assert.That(Guid.TryParse(IdOf(model, "Folder"), out _), Is.True);
    }

    [Test]
    public void FolderIdIsWrittenBackUnchangedOnSave()
    {
        string xml =
            $"""
            <?xml version="1.0" encoding="utf-8"?>
            <Connections {RootAttributes}>
                <Node Name="Folder" Type="Container" Expanded="True" Id="{SharedId}">
                    <Node Name="Leaf" Type="Connection" Id="{OtherId}" />
                </Node>
            </Connections>
            """;
        var model = Deserialize(xml);
        var cryptoProvider = new CryptoProviderFactory(BlockCipherEngines.AES, BlockCipherModes.GCM).Build();
        var nodeSerializer = new XmlConnectionNodeSerializer29(cryptoProvider, Password.ConvertToSecureString(), new SaveFilter());

        string saved = new XmlConnectionsSerializer(cryptoProvider, nodeSerializer).Serialize(model);

        var folder = XDocument.Parse(saved).Descendants("Node").First(node => (string)node.Attribute("Name") == "Folder");
        Assert.That((string)folder.Attribute("Id"), Is.EqualTo(SharedId));
    }
}
