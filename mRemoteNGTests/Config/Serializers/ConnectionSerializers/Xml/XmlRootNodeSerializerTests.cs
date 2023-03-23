using System;
using System.Collections;
using System.Xml.Linq;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tree.Root;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.ConnectionSerializers.Xml;

public class XmlRootNodeSerializerTests
{
    private XmlRootNodeSerializer _rootNodeSerializer;
    private ICryptographyProvider _cryptographyProvider;
    private RootNodeInfo _rootNodeInfo;
    private Version _version;

    [SetUp]
    public void Setup()
    {
        _rootNodeSerializer = new XmlRootNodeSerializer();
        _cryptographyProvider = new AeadCryptographyProvider();
        _rootNodeInfo = new RootNodeInfo(RootNodeType.Connection);
        _version = new Version(99, 1);
    }

    [Test]
    public void RootElementNamedConnections()
    {
        var element = _rootNodeSerializer.SerializeRootNodeInfo(_rootNodeInfo, _cryptographyProvider, _version);
        Assert.That(element.Name.LocalName, Is.EqualTo("Connections"));
    }

    [Test]
    [SetUICulture("en-US")]
    public void RootNodeInfoNameSerialized()
    {
        var element = _rootNodeSerializer.SerializeRootNodeInfo(_rootNodeInfo, _cryptographyProvider, _version);
        var attributeValue = element.Attribute(XName.Get("Name"))?.Value;
        Assert.That(attributeValue, Is.EqualTo("Connections"));
    }

    [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.AllEngineAndModeCombos))]
    public void EncryptionEngineSerialized(BlockCipherEngines engine, BlockCipherModes mode)
    {
        var cryptoProvider = new CryptoProviderFactory(engine, mode).Build();
        var element = _rootNodeSerializer.SerializeRootNodeInfo(_rootNodeInfo, cryptoProvider, _version);
        var attributeValue = element.Attribute(XName.Get("EncryptionEngine"))?.Value;
        Assert.That(attributeValue, Is.EqualTo(engine.ToString()));
    }

    [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.AllEngineAndModeCombos))]
    public void EncryptionModeSerialized(BlockCipherEngines engine, BlockCipherModes mode)
    {
        var cryptoProvider = new CryptoProviderFactory(engine, mode).Build();
        var element = _rootNodeSerializer.SerializeRootNodeInfo(_rootNodeInfo, cryptoProvider, _version);
        var attributeValue = element.Attribute(XName.Get("BlockCipherMode"))?.Value;
        Assert.That(attributeValue, Is.EqualTo(mode.ToString()));
    }

    [TestCase(1000)]
    [TestCase(1234)]
    [TestCase(9999)]
    [TestCase(10000)]
    public void KdfIterationsSerialized(int iterations)
    {
        _cryptographyProvider.KeyDerivationIterations = iterations;
        var element = _rootNodeSerializer.SerializeRootNodeInfo(_rootNodeInfo, _cryptographyProvider, _version);
        var attributeValue = element.Attribute(XName.Get("KdfIterations"))?.Value;
        Assert.That(attributeValue, Is.EqualTo(iterations.ToString()));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void FullFileEncryptionFlagSerialized(bool fullFileEncryption)
    {
        var element =
            _rootNodeSerializer.SerializeRootNodeInfo(_rootNodeInfo, _cryptographyProvider, _version,
                fullFileEncryption);
        var attributeValue = element.Attribute(XName.Get("FullFileEncryption"))?.Value;
        Assert.That(bool.Parse(attributeValue), Is.EqualTo(fullFileEncryption));
    }

    [TestCase("", "ThisIsNotProtected")]
    [TestCase(null, "ThisIsNotProtected")]
    [TestCase("mR3m", "ThisIsNotProtected")]
    [TestCase("customPassword1", "ThisIsProtected")]
    public void ProtectedStringSerialized(string customPassword, string expectedPlainText)
    {
        _rootNodeInfo.PasswordString = customPassword;
        var element = _rootNodeSerializer.SerializeRootNodeInfo(_rootNodeInfo, _cryptographyProvider, _version);
        var attributeValue = element.Attribute(XName.Get("Protected"))?.Value;
        var attributeValuePlainText =
            _cryptographyProvider.Decrypt(attributeValue, _rootNodeInfo.PasswordString.ConvertToSecureString());
        Assert.That(attributeValuePlainText, Is.EqualTo(expectedPlainText));
    }

    [Test]
    public void ConfVersionSerialized()
    {
        var element = _rootNodeSerializer.SerializeRootNodeInfo(_rootNodeInfo, _cryptographyProvider, _version);
        var attributeValue = element.Attribute(XName.Get("ConfVersion"))?.Value ?? "";
        var confVersion = Version.Parse(attributeValue);
        Assert.That(confVersion, Is.EqualTo(_version));
    }

    private class TestCaseSources
    {
        public static IEnumerable AllEngineAndModeCombos
        {
            get
            {
                foreach (var engine in Enum.GetValues(typeof(BlockCipherEngines)))
                foreach (var mode in Enum.GetValues(typeof(BlockCipherModes)))
                    yield return new TestCaseData(engine, mode);
            }
        }
    }
}