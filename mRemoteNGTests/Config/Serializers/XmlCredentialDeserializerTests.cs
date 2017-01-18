using System;
using System.Linq;
using System.Security;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Security;
using NUnit.Framework;


namespace mRemoteNGTests.Config.Serializers
{
    public class XmlCredentialDeserializerTests
    {
        private XmlCredentialDeserializer _deserializer;
        private ICryptographyProvider _cryptographyProvider;
        private readonly SecureString _key = "myencrptionpass".ConvertToSecureString();

        [SetUp]
        public void Setup()
        {
            _cryptographyProvider = new CryptographyProviderFactory().CreateAeadCryptographyProvider(BlockCipherEngines.AES, BlockCipherModes.GCM);
            _deserializer = new XmlCredentialDeserializer(_cryptographyProvider);
        }

        [Test]
        public void HasCorrectId()
        {
            var id = Guid.NewGuid();
            var xml = $"<Credentials>\r\n  <Credential Id=\"{id}\" Name=\"testcred\" Username=\"myuser\" Password=\"{GeneratePass("abc")}\" />\r\n</Credentials>";
            var creds = _deserializer.Deserialize(xml, _key);
            Assert.That(creds.First().Id, Is.EqualTo(id));
        }

        [Test]
        public void HasCorrectTitle()
        {
            const string title = "testtitle";
            var xml = $"<Credentials>\r\n  <Credential Id=\"256f4c8c-5819-4226-ad55-6f1f341b5449\" Name=\"{title}\" Username=\"myuser\" Password=\"{GeneratePass("abc")}\" />\r\n</Credentials>";
            var creds = _deserializer.Deserialize(xml, _key);
            Assert.That(creds.First().Name, Is.EqualTo(title));
        }

        [Test]
        public void HasCorrectUsername()
        {
            const string username = "myuser";
            var xml = $"<Credentials>\r\n  <Credential Id=\"256f4c8c-5819-4226-ad55-6f1f341b5449\" Name=\"testtitle\" Username=\"{username}\" Password=\"{GeneratePass("abc")}\" />\r\n</Credentials>";
            var creds = _deserializer.Deserialize(xml, _key);
            Assert.That(creds.First().Username, Is.EqualTo(username));
        }

        [Test]
        public void HasCorrectPassword()
        {
            const string plaintextPassword = "mypassword";
            var xml = $"<Credentials>\r\n  <Credential Id=\"256f4c8c-5819-4226-ad55-6f1f341b5449\" Name=\"testtitle\" Username=\"myuser\" Password=\"{GeneratePass(plaintextPassword)}\" />\r\n</Credentials>";
            var creds = _deserializer.Deserialize(xml, _key);
            Assert.That(creds.First().Password.ConvertToUnsecureString(), Is.EqualTo(plaintextPassword));
        }

        [Test]
        public void DeserializesAllCredentials()
        {
            var xml = $"<Credentials>\r\n  <Credential Id=\"256f4c8c-5819-4226-ad55-6f1f341b5449\" Name=\"testtitle\" Username=\"myuser\" Password=\"{GeneratePass("abc")}\" />\r\n  <Credential Id=\"356f4c8c-5819-4226-ad55-6f1f341b5449\" Name=\"othertitle\" Username=\"someuser\" Password=\"{GeneratePass("abc")}\" />\r\n</Credentials>";
            var creds = _deserializer.Deserialize(xml, _key);
            Assert.That(creds.Count(), Is.EqualTo(2));
        }

        private string GeneratePass(string plaintext)
        {
            return _cryptographyProvider.Encrypt(plaintext, _key);
        }
    }
}