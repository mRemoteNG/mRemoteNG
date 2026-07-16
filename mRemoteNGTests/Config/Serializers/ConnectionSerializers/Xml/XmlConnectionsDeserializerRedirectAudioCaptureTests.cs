using System.Linq;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Connection;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.ConnectionSerializers.Xml;

public class XmlConnectionsDeserializerRedirectAudioCaptureTests
{
    private const string Password = "mR3m";

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

    private static ConnectionInfo GetFirstConnection(ConnectionTreeModel model)
    {
        return model.RootNodes[0].Children.First();
    }

    [Test]
    public void RedirectAudioCaptureTrue_NoInheritAttribute_DeserializesValueTrue()
    {
        string xml =
            $"""
            <?xml version="1.0" encoding="utf-8"?>
            <Connections {RootAttributes}>
                <Node Name="TestConn" Type="Connection"
                      RedirectAudioCapture="True" />
            </Connections>
            """;

        var model = Deserialize(xml);
        var connection = GetFirstConnection(model);

        Assert.That(connection.RedirectAudioCapture, Is.True);
    }

    [Test]
    public void RedirectAudioCaptureTrue_NoInheritAttribute_InheritanceDefaultsFalse()
    {
        string xml =
            $"""
            <?xml version="1.0" encoding="utf-8"?>
            <Connections {RootAttributes}>
                <Node Name="TestConn" Type="Connection"
                      RedirectAudioCapture="True" />
            </Connections>
            """;

        var model = Deserialize(xml);
        var connection = GetFirstConnection(model);

        Assert.That(connection.Inheritance.RedirectAudioCapture, Is.False);
    }

    [Test]
    public void InheritRedirectAudioCaptureTrue_DeserializesInheritanceTrue()
    {
        string xml =
            $"""
            <?xml version="1.0" encoding="utf-8"?>
            <Connections {RootAttributes}>
                <Node Name="TestConn" Type="Connection"
                      RedirectAudioCapture="False"
                      InheritRedirectAudioCapture="True" />
            </Connections>
            """;

        var model = Deserialize(xml);
        var connection = GetFirstConnection(model);

        Assert.That(connection.Inheritance.RedirectAudioCapture, Is.True);
    }

    [Test]
    public void InheritRedirectAudioCaptureFalse_DeserializesInheritanceFalse()
    {
        string xml =
            $"""
            <?xml version="1.0" encoding="utf-8"?>
            <Connections {RootAttributes}>
                <Node Name="TestConn" Type="Connection"
                      RedirectAudioCapture="True"
                      InheritRedirectAudioCapture="False" />
            </Connections>
            """;

        var model = Deserialize(xml);
        var connection = GetFirstConnection(model);

        Assert.That(connection.Inheritance.RedirectAudioCapture, Is.False);
    }

    [Test]
    public void RedirectAudioCaptureFalse_WithInheritTrue_ValueRemainsFalse()
    {
        string xml =
            $"""
            <?xml version="1.0" encoding="utf-8"?>
            <Connections {RootAttributes}>
                <Node Name="TestConn" Type="Connection"
                      RedirectAudioCapture="False"
                      InheritRedirectAudioCapture="True" />
            </Connections>
            """;

        var model = Deserialize(xml);
        var connection = GetFirstConnection(model);

        Assert.That(connection.RedirectAudioCapture, Is.False);
    }
}
