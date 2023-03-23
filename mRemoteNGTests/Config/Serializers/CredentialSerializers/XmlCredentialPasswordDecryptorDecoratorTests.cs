using System.Linq;
using System.Security;
using mRemoteNG.Config.Serializers.CredentialSerializer;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.CredentialSerializers;

public class XmlCredentialPasswordDecryptorDecoratorTests
{
    private XmlCredentialPasswordDecryptorDecorator _sut;
    private readonly SecureString _decryptionKey = "myKey1".ConvertToSecureString();
    private string _unencryptedPassword = "myPassword1";

    [SetUp]
    public void Setup()
    {
        var baseDeserializer = new XmlCredentialRecordDeserializer();
        _sut = new XmlCredentialPasswordDecryptorDecorator(baseDeserializer);
    }

    [Test]
    public void OutputedCredentialHasDecryptedPassword()
    {
        var xml = GenerateCredentialXml();
        var output = _sut.Deserialize(xml, _decryptionKey);
        Assert.That(output.First().Password.ConvertToUnsecureString(), Is.EqualTo(_unencryptedPassword));
    }

    [Test]
    public void DecryptionThrowsExceptionWhenAuthHeaderNotFound()
    {
        var xml = GenerateCredentialXml(false);
        Assert.Throws<EncryptionException>(() => _sut.Deserialize(xml, _decryptionKey));
    }


    private string GenerateCredentialXml(bool includeAuthHeader = true)
    {
        var cryptoProvider = new AeadCryptographyProvider();
        var authHeader = includeAuthHeader ? $"Auth=\"{cryptoProvider.Encrypt("someheader", _decryptionKey)}\"" : "";
        var encryptedPassword = cryptoProvider.Encrypt(_unencryptedPassword, _decryptionKey);
        return
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            $"<Credentials EncryptionEngine=\"{cryptoProvider.CipherEngine}\" BlockCipherMode=\"{cryptoProvider.CipherMode}\" KdfIterations=\"{cryptoProvider.KeyDerivationIterations}\" {authHeader} SchemaVersion=\"1.0\">" +
            $"<Credential Id=\"ce6b0397-d476-4ffe-884b-dbe9347a88a8\" Title=\"New Credential\" Username=\"asdfasdf\" Domain=\"\" Password=\"{encryptedPassword}\" />" +
            "</Credentials>";
    }
}