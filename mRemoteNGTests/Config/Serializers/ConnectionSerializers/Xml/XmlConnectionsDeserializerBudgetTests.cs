using System.IO;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.ConnectionSerializers.Xml;

public class XmlConnectionsDeserializerBudgetTests
{
    [TestCase(-1)]
    [TestCase(0)]
    [TestCase(999)]
    [TestCase(50001)]
    public void Deserialize_UnsupportedKdfCost_RejectsBeforeAuthentication(int iterations)
    {
        bool prompted = false;
        XmlConnectionsDeserializer deserializer = new(() =>
        {
            prompted = true;
            return mRemoteNG.Tools.Optional<System.Security.SecureString>.Empty;
        });
        string xml = $"<?xml version=\"1.0\" encoding=\"utf-8\"?><Connections Name=\"Synthetic\" ConfVersion=\"2.8\" EncryptionEngine=\"AES\" BlockCipherMode=\"GCM\" KdfIterations=\"{iterations}\" />";

        Assert.Throws<InvalidDataException>(() => deserializer.Deserialize(xml));
        Assert.That(prompted, Is.False);
    }
}
