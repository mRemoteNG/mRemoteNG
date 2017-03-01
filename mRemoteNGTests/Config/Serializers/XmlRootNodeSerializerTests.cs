using System;
using System.Collections;
using System.Xml.Linq;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tree.Root;
using NUnit.Framework;


namespace mRemoteNGTests.Config.Serializers
{
    public class XmlRootNodeSerializerTests
    {
        private XmlRootNodeSerializer _rootNodeSerializer;
        private ICryptographyProvider _cryptographyProvider;
        private RootNodeInfo _rootNodeInfo;

        [SetUp]
        public void Setup()
        {
            _rootNodeSerializer = new XmlRootNodeSerializer();
            _cryptographyProvider = new AeadCryptographyProvider();
            _rootNodeInfo = new RootNodeInfo(RootNodeType.Connection);
        }

        [Test]
        public void RootElementNamedConnections()
        {
            var element = _rootNodeSerializer.SerializeRootNodeInfo(_rootNodeInfo, _cryptographyProvider);
            Assert.That(element.Name.LocalName, Is.EqualTo("Connections"));
        }

        [Test]
        public void RootNodeInfoNameSerialized()
        {
            var element = _rootNodeSerializer.SerializeRootNodeInfo(_rootNodeInfo, _cryptographyProvider);
            var attributeValue = element.Attribute(XName.Get("Name"))?.Value;
            Assert.That(attributeValue, Is.EqualTo("Connections"));
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.AllEngineAndModeCombos))]
        public void EncryptionEngineSerialized(BlockCipherEngines engine, BlockCipherModes mode)
        {
            var cryptoProvider = new CryptographyProviderFactory().CreateAeadCryptographyProvider(engine, mode);
            var element = _rootNodeSerializer.SerializeRootNodeInfo(_rootNodeInfo, cryptoProvider);
            var attributeValue = element.Attribute(XName.Get("EncryptionEngine"))?.Value;
            Assert.That(attributeValue, Is.EqualTo(engine.ToString()));
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.AllEngineAndModeCombos))]
        public void EncryptionModeSerialized(BlockCipherEngines engine, BlockCipherModes mode)
        {
            var cryptoProvider = new CryptographyProviderFactory().CreateAeadCryptographyProvider(engine, mode);
            var element = _rootNodeSerializer.SerializeRootNodeInfo(_rootNodeInfo, cryptoProvider);
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
            var element = _rootNodeSerializer.SerializeRootNodeInfo(_rootNodeInfo, _cryptographyProvider);
            var attributeValue = element.Attribute(XName.Get("KdfIterations"))?.Value;
            Assert.That(attributeValue, Is.EqualTo(iterations.ToString()));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void FullFileEncryptionFlagSerialized(bool fullFileEncryption)
        {
            var element = _rootNodeSerializer.SerializeRootNodeInfo(_rootNodeInfo, _cryptographyProvider, fullFileEncryption);
            var attributeValue = element.Attribute(XName.Get("FullFileEncryption"))?.Value;
            Assert.That(attributeValue, Is.EqualTo(fullFileEncryption.ToString()));
        }

        [TestCase("", "ThisIsNotProtected")]
        [TestCase(null, "ThisIsNotProtected")]
        [TestCase("mR3m", "ThisIsNotProtected")]
        [TestCase("customPassword1", "ThisIsProtected")]
        public void ProtectedStringSerialized(string customPassword, string expectedPlainText)
        {
            _rootNodeInfo.PasswordString = customPassword;
            var element = _rootNodeSerializer.SerializeRootNodeInfo(_rootNodeInfo, _cryptographyProvider);
            var attributeValue = element.Attribute(XName.Get("Protected"))?.Value;
            var attributeValuePlainText = _cryptographyProvider.Decrypt(attributeValue, _rootNodeInfo.PasswordString.ConvertToSecureString());
            Assert.That(attributeValuePlainText, Is.EqualTo(expectedPlainText));
        }

        [Test]
        public void ConfVersionSerialized()
        {
            var element = _rootNodeSerializer.SerializeRootNodeInfo(_rootNodeInfo, _cryptographyProvider);
            var attributeValue = element.Attribute(XName.Get("ConfVersion"))?.Value ?? "";
            var versionAsNumber = double.Parse(attributeValue);
            Assert.That(versionAsNumber, Is.GreaterThan(0));
        }

        private class TestCaseSources
        {
            public static IEnumerable AllEngineAndModeCombos
            {
                get
                {
                    foreach (var engine in Enum.GetValues(typeof(BlockCipherEngines)))
                    {
                        foreach (var mode in Enum.GetValues(typeof(BlockCipherModes)))
                        {
                            yield return new TestCaseData(engine, mode);
                        }
                    }
                }
            }
        }
    }
}