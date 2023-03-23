using System;
using System.Linq;
using mRemoteNG.Config.Serializers.CredentialSerializer;
using mRemoteNG.Security;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.CredentialSerializers;

public class XmlCredentialRecordDeserializerTests
{
    private XmlCredentialRecordDeserializer _deserializer;
    private readonly Guid _id = Guid.NewGuid();
    private const string Title = "sometitle";
    private const string Username = "myusername";
    private const string Domain = "mydomain";
    private const string PlaintextPassword = "mypassword";


    [SetUp]
    public void Setup()
    {
        _deserializer = new XmlCredentialRecordDeserializer();
    }

    [Test]
    public void HasCorrectId()
    {
        var xml = GenerateXml();
        var creds = _deserializer.Deserialize(xml);
        Assert.That(creds.First().Id, Is.EqualTo(_id));
    }

    [Test]
    public void HasCorrectTitle()
    {
        var xml = GenerateXml();
        var creds = _deserializer.Deserialize(xml);
        Assert.That(creds.First().Title, Is.EqualTo(Title));
    }

    [Test]
    public void HasCorrectUsername()
    {
        var xml = GenerateXml();
        var creds = _deserializer.Deserialize(xml);
        Assert.That(creds.First().Username, Is.EqualTo(Username));
    }

    [Test]
    public void HasCorrectDomain()
    {
        var xml = GenerateXml();
        var creds = _deserializer.Deserialize(xml);
        Assert.That(creds.First().Domain, Is.EqualTo(Domain));
    }

    [Test]
    public void HasCorrectPassword()
    {
        var xml = GenerateXml();
        var creds = _deserializer.Deserialize(xml);
        Assert.That(creds.First().Password.ConvertToUnsecureString(), Is.EqualTo(PlaintextPassword));
    }

    [Test]
    public void DeserializesAllCredentials()
    {
        var xml = GenerateXml();
        var creds = _deserializer.Deserialize(xml);
        Assert.That(creds.Count(), Is.EqualTo(2));
    }

    [Test]
    public void CanDecryptNonStandardEncryptions()
    {
        var xml = GenerateXml(BlockCipherEngines.Serpent, BlockCipherModes.EAX, 3000);
        var creds = _deserializer.Deserialize(xml);
        Assert.That(creds.First().Password.ConvertToUnsecureString(), Is.EqualTo(PlaintextPassword));
    }


    private string GenerateXml(BlockCipherEngines engine = BlockCipherEngines.AES,
        BlockCipherModes mode = BlockCipherModes.GCM, int interations = 1000)
    {
        return
            $"<Credentials EncryptionEngine=\"{engine}\" BlockCipherMode=\"{mode}\" KdfIterations=\"{interations}\" SchemaVersion=\"1.0\">" +
            $"<Credential Id=\"{_id}\" Title=\"{Title}\" Username=\"{Username}\" Domain=\"{Domain}\" Password=\"{PlaintextPassword}\" />" +
            $"<Credential Id=\"{Guid.NewGuid()}\" Title=\"{Title}\" Username=\"{Username}\" Domain=\"{Domain}\" Password=\"{PlaintextPassword}\" />" +
            "</Credentials>";
    }
}