using System.Security;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tree.Root;
using NUnit.Framework;
using System.IO;
using System.Xml.Linq;

namespace mRemoteNGTests.Security
{
    [TestFixture]
    public class XmlKeyValidatorTests
    {
        private string _tempFilePath = null!;

        [SetUp]
        public void SetUp()
        {
            _tempFilePath = Path.GetTempFileName();
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_tempFilePath))
                File.Delete(_tempFilePath);
        }

        [Test]
        public void ConnectionFileRequiresPassword_ReturnsFalse_ForNonExistentFile()
        {
            Assert.That(XmlKeyValidator.ConnectionFileRequiresPassword(Path.Combine(Path.GetTempPath(), "nonexistent.xml")), Is.False);
        }

        [Test]
        public void ConnectionFileRequiresPassword_ReturnsFalse_ForEmptyFile()
        {
            File.WriteAllText(_tempFilePath, "");
            Assert.That(XmlKeyValidator.ConnectionFileRequiresPassword(_tempFilePath), Is.False);
        }

        [Test]
        public void ConnectionFileRequiresPassword_ReturnsFalse_ForUnprotectedFile()
        {
            ICryptographyProvider cryptoProvider = new LegacyRijndaelCryptographyProvider();
            SecureString defaultKey = new RootNodeInfo(RootNodeType.Connection).PasswordString.ConvertToSecureString();
            string protectedValue = cryptoProvider.Encrypt("ThisIsNotProtected", defaultKey);

            string xml = $"<Connections Protected=\"{protectedValue}\" />";
            File.WriteAllText(_tempFilePath, xml);

            Assert.That(XmlKeyValidator.ConnectionFileRequiresPassword(_tempFilePath), Is.False);
        }

        [Test]
        public void ConnectionFileRequiresPassword_ReturnsTrue_ForProtectedFile()
        {
            ICryptographyProvider cryptoProvider = new LegacyRijndaelCryptographyProvider();
            SecureString customKey = "testPassword123".ConvertToSecureString();
            string protectedValue = cryptoProvider.Encrypt("ThisIsProtected", customKey);

            string xml = $"<Connections Protected=\"{protectedValue}\" />";
            File.WriteAllText(_tempFilePath, xml);

            Assert.That(XmlKeyValidator.ConnectionFileRequiresPassword(_tempFilePath), Is.True);
        }

        [Test]
        public void ConnectionFileUsesKey_ReturnsTrue_WhenKeyMatches()
        {
            ICryptographyProvider cryptoProvider = new LegacyRijndaelCryptographyProvider();
            SecureString key = "testPassword123".ConvertToSecureString();
            string protectedValue = cryptoProvider.Encrypt("ThisIsProtected", key);

            string xml = $"<Connections Protected=\"{protectedValue}\" />";
            File.WriteAllText(_tempFilePath, xml);

            Assert.That(XmlKeyValidator.ConnectionFileUsesKey(_tempFilePath, key), Is.True);
        }

        [Test]
        public void ConnectionFileUsesKey_ReturnsFalse_WhenKeyDoesNotMatch()
        {
            ICryptographyProvider cryptoProvider = new LegacyRijndaelCryptographyProvider();
            SecureString correctKey = "correctPassword".ConvertToSecureString();
            SecureString wrongKey = "wrongPassword".ConvertToSecureString();
            string protectedValue = cryptoProvider.Encrypt("ThisIsProtected", correctKey);

            string xml = $"<Connections Protected=\"{protectedValue}\" />";
            File.WriteAllText(_tempFilePath, xml);

            Assert.That(XmlKeyValidator.ConnectionFileUsesKey(_tempFilePath, wrongKey), Is.False);
        }

        [Test]
        public void CredentialsFileUsesKey_ReturnsTrue_ForNonExistentFile()
        {
            Assert.That(XmlKeyValidator.CredentialsFileUsesKey(Path.Combine(Path.GetTempPath(), "nonexistent_cred.xml"), "anyKey".ConvertToSecureString()), Is.True);
        }

        [Test]
        public void CredentialsFileUsesKey_ReturnsTrue_WhenKeyMatches()
        {
            ICryptographyProvider cryptoProvider = new LegacyRijndaelCryptographyProvider();
            SecureString key = "credPassword".ConvertToSecureString();
            string authValue = cryptoProvider.Encrypt("AuthVerifier", key);

            string xml = $"<Credentials Auth=\"{authValue}\" />";
            File.WriteAllText(_tempFilePath, xml);

            Assert.That(XmlKeyValidator.CredentialsFileUsesKey(_tempFilePath, key), Is.True);
        }

        [Test]
        public void CredentialsFileUsesKey_ReturnsFalse_WhenKeyDoesNotMatch()
        {
            ICryptographyProvider cryptoProvider = new LegacyRijndaelCryptographyProvider();
            SecureString correctKey = "correctCredPassword".ConvertToSecureString();
            SecureString wrongKey = "wrongCredPassword".ConvertToSecureString();
            string authValue = cryptoProvider.Encrypt("AuthVerifier", correctKey);

            string xml = $"<Credentials Auth=\"{authValue}\" />";
            File.WriteAllText(_tempFilePath, xml);

            Assert.That(XmlKeyValidator.CredentialsFileUsesKey(_tempFilePath, wrongKey), Is.False);
        }
    }
}
