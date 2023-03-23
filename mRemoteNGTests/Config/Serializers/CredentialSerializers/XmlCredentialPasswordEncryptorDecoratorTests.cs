using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.CredentialSerializer;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.CredentialSerializers;

public class XmlCredentialPasswordEncryptorDecoratorTests
{
    private XmlCredentialPasswordEncryptorDecorator _sut;
    private const BlockCipherEngines CipherEngine = BlockCipherEngines.Twofish;
    private const BlockCipherModes CipherMode = BlockCipherModes.EAX;
    private const int KdfIterations = 2000;
    private SecureString _key = "myKey1".ConvertToSecureString();

    [SetUp]
    public void Setup()
    {
        var cryptoProvider = SetupCryptoProvider();
        var baseSerializer = SetupBaseSerializer();
        _sut = new XmlCredentialPasswordEncryptorDecorator(cryptoProvider, baseSerializer);
    }


    [Test]
    public void CantPassNullCredentialList()
    {
        Assert.Throws<ArgumentNullException>(() => _sut.Serialize(null, new SecureString()));
    }

    [Test]
    public void EncryptsPasswordAttributesInXml()
    {
        var credList = Substitute.For<IEnumerable<ICredentialRecord>>();
        var output = _sut.Serialize(credList, _key);
        var outputAsXdoc = XDocument.Parse(output);
        var firstElementPassword = outputAsXdoc.Root?.Descendants().First().FirstAttribute.Value;
        Assert.That(firstElementPassword, Is.EqualTo("encrypted"));
    }

    [TestCase("EncryptionEngine", CipherEngine)]
    [TestCase("BlockCipherMode", CipherMode)]
    [TestCase("KdfIterations", KdfIterations)]
    [TestCase("Auth", "encrypted")]
    public void SetsRootNodeEncryptionAttributes(string attributeName, object expectedValue)
    {
        var credList = Substitute.For<IEnumerable<ICredentialRecord>>();
        var output = _sut.Serialize(credList, _key);
        var outputAsXdoc = XDocument.Parse(output);
        var authField = outputAsXdoc.Root?.Attribute(attributeName)?.Value;
        Assert.That(authField, Is.EqualTo(expectedValue.ToString()));
    }

    private ISerializer<IEnumerable<ICredentialRecord>, string> SetupBaseSerializer()
    {
        var baseSerializer = Substitute.For<ISerializer<IEnumerable<ICredentialRecord>, string>>();
        var randomString = Guid.NewGuid().ToString();
        baseSerializer.Serialize(null).ReturnsForAnyArgs(
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<Root>" +
            $"<Element1 Password=\"{randomString}\" />" +
            $"<Element1 Password=\"{randomString}\">" +
            $"<Element1 Password=\"{randomString}\" />" +
            "</Element1>" +
            "</Root>");
        return baseSerializer;
    }

    private ICryptographyProvider SetupCryptoProvider()
    {
        var cryptoProvider = Substitute.For<ICryptographyProvider>();
        cryptoProvider.CipherEngine.Returns(CipherEngine);
        cryptoProvider.CipherMode.Returns(CipherMode);
        cryptoProvider.KeyDerivationIterations.Returns(KdfIterations);
        cryptoProvider.Encrypt(null, null).ReturnsForAnyArgs("encrypted");
        return cryptoProvider;
    }
}