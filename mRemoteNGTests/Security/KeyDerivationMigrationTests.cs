using System.Linq;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;
using mRemoteNG.Security.KeyDerivation;
using mRemoteNG.Security.SymmetricEncryption;
using NUnit.Framework;

namespace mRemoteNGTests.Security
{
    public class KeyDerivationMigrationTests
    {
        private const string PlainText = "MySecret!";
        private readonly SecureString _key = "mypassword111111".ConvertToSecureString();

        [Test]
        public void Sha256AndLegacyDeriveDifferentKeys()
        {
            byte[] salt = new byte[16];
            byte[] legacy = new Pkcs5S2KeyGenerator(256, 1000, KeyDerivationAlgorithms.Pbkdf2Sha1Legacy).DeriveKey("pw", salt);
            byte[] sha256 = new Pkcs5S2KeyGenerator(256, 1000, KeyDerivationAlgorithms.Pbkdf2Sha256).DeriveKey("pw", salt);
            Assert.That(sha256, Is.Not.EquivalentTo(legacy));
        }

        [Test]
        public void Sha256IsStandardPbkdf2OverUtf8()
        {
            byte[] salt = System.Text.Encoding.UTF8.GetBytes("salt");
            byte[] expected = System.Security.Cryptography.Rfc2898DeriveBytes.Pbkdf2(
                System.Text.Encoding.UTF8.GetBytes("pässwörd"), salt, 1000, System.Security.Cryptography.HashAlgorithmName.SHA256, 32);
            byte[] actual = new Pkcs5S2KeyGenerator(256, 1000, KeyDerivationAlgorithms.Pbkdf2Sha256).DeriveKey("pässwörd", salt);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Sha256DistinguishesNonAsciiPasswordsThatLegacyConfuses()
        {
            // U+0161 and U+0061 share a low byte, so the legacy conversion treats them as the same password
            byte[] salt = new byte[16];
            string a = "š", b = "a";
            Assert.That(new Pkcs5S2KeyGenerator(256, 1000).DeriveKey(a, salt),
                        Is.EqualTo(new Pkcs5S2KeyGenerator(256, 1000).DeriveKey(b, salt)));
            Assert.That(new Pkcs5S2KeyGenerator(256, 1000, KeyDerivationAlgorithms.Pbkdf2Sha256).DeriveKey(a, salt),
                        Is.Not.EqualTo(new Pkcs5S2KeyGenerator(256, 1000, KeyDerivationAlgorithms.Pbkdf2Sha256).DeriveKey(b, salt)));
        }

        [TestCase(KeyDerivationAlgorithms.Pbkdf2Sha1Legacy)]
        [TestCase(KeyDerivationAlgorithms.Pbkdf2Sha256)]
        public void RoundTripsWithEachAlgorithm(KeyDerivationAlgorithms algorithm)
        {
            AeadCryptographyProvider provider = new() { KeyDerivationAlgorithm = algorithm, KeyDerivationIterations = 1000 };
            string cipherText = provider.Encrypt(PlainText, _key);
            Assert.That(provider.Decrypt(cipherText, _key), Is.EqualTo(PlainText));
        }

        [Test]
        public void FreshProviderCanDecryptWhatAnotherInstanceEncrypted()
        {
            AeadCryptographyProvider writer = new() { KeyDerivationAlgorithm = KeyDerivationAlgorithms.Pbkdf2Sha256, KeyDerivationIterations = 1000 };
            AeadCryptographyProvider reader = new() { KeyDerivationAlgorithm = KeyDerivationAlgorithms.Pbkdf2Sha256, KeyDerivationIterations = 1000 };
            Assert.That(reader.Decrypt(writer.Encrypt(PlainText, _key), _key), Is.EqualTo(PlainText));
        }

        [Test]
        public void RepeatedEncryptionsStillDiffer()
        {
            AeadCryptographyProvider provider = new() { KeyDerivationIterations = 1000 };
            string[] cipherTexts = Enumerable.Range(0, 5).Select(_ => provider.Encrypt(PlainText, _key)).ToArray();
            Assert.That(cipherTexts.Distinct().Count(), Is.EqualTo(5));
        }

        [Test]
        public void LegacyDataDoesNotDecryptWithSha256()
        {
            string legacyCipherText = new AeadCryptographyProvider { KeyDerivationIterations = 1000 }.Encrypt(PlainText, _key);
            AeadCryptographyProvider sha256 = new() { KeyDerivationAlgorithm = KeyDerivationAlgorithms.Pbkdf2Sha256, KeyDerivationIterations = 1000 };
            Assert.Throws<EncryptionException>(() => sha256.Decrypt(legacyCipherText, _key));
        }

        [Test]
        public void XmlWithoutKdfAlgorithmIsReadAsLegacy()
        {
            XElement element = new("Root",
                new XAttribute("EncryptionEngine", "AES"),
                new XAttribute("BlockCipherMode", "GCM"),
                new XAttribute("KdfIterations", "1000"));
            ICryptographyProvider provider = new CryptoProviderFactoryFromXml(element).Build();
            Assert.That(provider.KeyDerivationAlgorithm, Is.EqualTo(KeyDerivationAlgorithms.Pbkdf2Sha1Legacy));
        }

        [Test]
        public void XmlKdfAlgorithmIsRead()
        {
            XElement element = new("Root",
                new XAttribute("EncryptionEngine", "AES"),
                new XAttribute("BlockCipherMode", "GCM"),
                new XAttribute("KdfIterations", "1000"),
                new XAttribute("KdfAlgorithm", "Pbkdf2Sha256"));
            ICryptographyProvider provider = new CryptoProviderFactoryFromXml(element).Build();
            Assert.That(provider.KeyDerivationAlgorithm, Is.EqualTo(KeyDerivationAlgorithms.Pbkdf2Sha256));
        }

        [TestCase("EncryptionEngine", "NotACipher")]
        [TestCase("BlockCipherMode", "NotAMode")]
        [TestCase("KdfIterations", "lots")]
        [TestCase("KdfAlgorithm", "Md5")]
        public void MalformedEncryptionSettingsThrowInsteadOfFallingBackToLegacy(string attribute, string badValue)
        {
            XElement element = new("Root",
                new XAttribute("EncryptionEngine", "AES"),
                new XAttribute("BlockCipherMode", "GCM"),
                new XAttribute("KdfIterations", "1000"));
            element.SetAttributeValue(attribute, badValue);
            Assert.Throws<EncryptionException>(() => new CryptoProviderFactoryFromXml(element).Build());
        }
    }
}
