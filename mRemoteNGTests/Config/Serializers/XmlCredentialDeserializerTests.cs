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
            _deserializer = new XmlCredentialDeserializer();
        }

        [Test]
        public void HasCorrectId()
        {
            var id = Guid.NewGuid();
            var xml = $"<Credentials EncryptionEngine=\"AES\" BlockCipherMode=\"GCM\" KdfIterations=\"1000\" SchemaVersion=\"1.0\">\r\n  <Credential Id=\"{id}\" Title=\"testcred\" Username=\"myuser\" Password=\"{GeneratePass("abc")}\" />\r\n</Credentials>";
            var creds = _deserializer.Deserialize(xml, _key);
            Assert.That(creds.First().Id, Is.EqualTo(id));
        }

        [Test]
        public void HasCorrectTitle()
        {
            const string title = "testtitle";
            var xml = $"<Credentials EncryptionEngine=\"AES\" BlockCipherMode=\"GCM\" KdfIterations=\"1000\" SchemaVersion=\"1.0\">\r\n  <Credential Id=\"256f4c8c-5819-4226-ad55-6f1f341b5449\" Title=\"{title}\" Username=\"myuser\" Password=\"{GeneratePass("abc")}\" />\r\n</Credentials>";
            var creds = _deserializer.Deserialize(xml, _key);
            Assert.That(creds.First().Title, Is.EqualTo(title));
        }

        [Test]
        public void HasCorrectUsername()
        {
            const string username = "myuser";
            var xml = $"<Credentials EncryptionEngine=\"AES\" BlockCipherMode=\"GCM\" KdfIterations=\"1000\" SchemaVersion=\"1.0\">\r\n  <Credential Id=\"256f4c8c-5819-4226-ad55-6f1f341b5449\" Title=\"testtitle\" Username=\"{username}\" Password=\"{GeneratePass("abc")}\" />\r\n</Credentials>";
            var creds = _deserializer.Deserialize(xml, _key);
            Assert.That(creds.First().Username, Is.EqualTo(username));
        }

        [Test]
        public void HasCorrectPassword()
        {
            const string plaintextPassword = "mypassword";
            var xml = $"<Credentials EncryptionEngine=\"AES\" BlockCipherMode=\"GCM\" KdfIterations=\"1000\" SchemaVersion=\"1.0\">\r\n  <Credential Id=\"256f4c8c-5819-4226-ad55-6f1f341b5449\" Title=\"testtitle\" Username=\"myuser\" Password=\"{GeneratePass(plaintextPassword)}\" />\r\n</Credentials>";
            var creds = _deserializer.Deserialize(xml, _key);
            Assert.That(creds.First().Password.ConvertToUnsecureString(), Is.EqualTo(plaintextPassword));
        }

        [Test]
        public void DeserializesAllCredentials()
        {
            var xml = $"<Credentials EncryptionEngine=\"AES\" BlockCipherMode=\"GCM\" KdfIterations=\"1000\" SchemaVersion=\"1.0\">\r\n  <Credential Id=\"256f4c8c-5819-4226-ad55-6f1f341b5449\" Title=\"testtitle\" Username=\"myuser\" Password=\"{GeneratePass("abc")}\" />\r\n  <Credential Id=\"356f4c8c-5819-4226-ad55-6f1f341b5449\" Title=\"othertitle\" Username=\"someuser\" Password=\"{GeneratePass("abc")}\" />\r\n</Credentials>";
            var creds = _deserializer.Deserialize(xml, _key);
            Assert.That(creds.Count(), Is.EqualTo(2));
        }

        [Test]
        public void CanDecryptNonStandardEncryptions()
        {
            var otherCryptoProvider = new CryptographyProviderFactory().CreateAeadCryptographyProvider(BlockCipherEngines.Serpent, BlockCipherModes.CCM);
            otherCryptoProvider.KeyDerivationIterations = 2000;
            const string plaintextPassword = "mypassword";
            var encryptedPassword = otherCryptoProvider.Encrypt(plaintextPassword, _key);
            var xml =
                $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Credentials EncryptionEngine=\"Serpent\" BlockCipherMode=\"CCM\" KdfIterations=\"2000\" SchemaVersion=\"1.0\">\r\n  <Credential Id=\"faadd345-6c68-4891-9897-d22525ec7c58\" Title=\"testcred\" Username=\"davids\" Password=\"{encryptedPassword}\" />\r\n</Credentials>";
            var creds = _deserializer.Deserialize(xml, _key);
            Assert.That(creds.First().Password.ConvertToUnsecureString(), Is.EqualTo(plaintextPassword));
        }

        private string GeneratePass(string plaintext)
        {
            return _cryptographyProvider.Encrypt(plaintext, _key);
        }
    }
}