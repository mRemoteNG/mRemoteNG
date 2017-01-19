using System.Linq;
using System.Xml.Linq;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using NUnit.Framework;


namespace mRemoteNGTests.Config.Serializers
{
    public class XmlCredentialSerializerTests
    {
        private XmlCredentialRecordSerializer _serializer;
        private ICryptographyProvider _cryptoProvider;
        private ICredentialRecord _cred1;

        [SetUp]
        public void Setup()
        {
            _cryptoProvider = new CryptographyProviderFactory().CreateAeadCryptographyProvider(BlockCipherEngines.AES, BlockCipherModes.GCM);
            _serializer = new XmlCredentialRecordSerializer(_cryptoProvider);
            _cred1 = new CredentialRecord { Title = "testcred", Username = "davids", Domain = "mydomain", Password = "mypass".ConvertToSecureString() };
        }

        [Test]
        public void ProducesValidXml()
        {
            var serialized = _serializer.Serialize(new[] { _cred1 }, "mypass".ConvertToSecureString());
            Assert.DoesNotThrow(() => XDocument.Parse(serialized));
        }

        [Test]
        public void AllCredentialsSerialized()
        {
            var cred2 = new CredentialRecord { Title = "testcred2", Username = "admin", Domain = "otherdomain", Password = "somepass".ConvertToSecureString() };
            var serialized = _serializer.Serialize(new[] { _cred1, cred2 }, "mypass".ConvertToSecureString());
            var serializedCount = XDocument.Parse(serialized).Descendants("Credential").Count();
            Assert.That(serializedCount, Is.EqualTo(2));
        }

        [Test]
        public void IncludesEncryptionEngineParameter()
        {
            var serialized = _serializer.Serialize(new[] { _cred1 }, "mypass".ConvertToSecureString());
            var xdoc = XDocument.Parse(serialized);
            Assert.That(xdoc.Root?.Attribute("EncryptionEngine")?.Value, Is.EqualTo(_cryptoProvider.CipherEngine.ToString()));
        }

        [Test]
        public void IncludesBlockCipherModeParameter()
        {
            var serialized = _serializer.Serialize(new[] { _cred1 }, "mypass".ConvertToSecureString());
            var xdoc = XDocument.Parse(serialized);
            Assert.That(xdoc.Root?.Attribute("BlockCipherMode")?.Value, Is.EqualTo(_cryptoProvider.CipherMode.ToString()));
        }

        [Test]
        public void IncludesKdfIterationsParameter()
        {
            var serialized = _serializer.Serialize(new[] { _cred1 }, "mypass".ConvertToSecureString());
            var xdoc = XDocument.Parse(serialized);
            Assert.That(xdoc.Root?.Attribute("KdfIterations")?.Value, Is.EqualTo(_cryptoProvider.KeyDerivationIterations.ToString()));
        }

        [Test]
        public void IncludesSchemaVersionParameter()
        {
            var serialized = _serializer.Serialize(new[] { _cred1 }, "mypass".ConvertToSecureString());
            var xdoc = XDocument.Parse(serialized);
            Assert.That(xdoc.Root?.Attribute("SchemaVersion")?.Value, Is.EqualTo(_serializer.SchemaVersion));
        }
    }
}