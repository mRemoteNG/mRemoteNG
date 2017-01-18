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

        [SetUp]
        public void Setup()
        {
            var cryptoProvider = new CryptographyProviderFactory().CreateAeadCryptographyProvider(BlockCipherEngines.AES, BlockCipherModes.GCM);
            _serializer = new XmlCredentialRecordSerializer(cryptoProvider);
        }

        [Test]
        public void ProducesValidXml()
        {
            var cred = new CredentialRecord { Name = "testcred", Username = "davids", Domain = "mydomain", Password = "mypass".ConvertToSecureString() };
            var serialized = _serializer.Serialize(new[] { cred }, "mypass".ConvertToSecureString());
            Assert.DoesNotThrow(() => XDocument.Parse(serialized));
        }

        [Test]
        public void AllCredentialsSerialized()
        {
            var cred = new CredentialRecord { Name = "testcred", Username = "davids", Domain = "mydomain", Password = "mypass".ConvertToSecureString() };
            var cred2 = new CredentialRecord { Name = "testcred2", Username = "admin", Domain = "otherdomain", Password = "somepass".ConvertToSecureString() };
            var serialized = _serializer.Serialize(new[] { cred, cred2 }, "mypass".ConvertToSecureString());
            var serializedCount = XDocument.Parse(serialized).Descendants("Credential").Count();
            Assert.That(serializedCount, Is.EqualTo(2));
        }
    }
}