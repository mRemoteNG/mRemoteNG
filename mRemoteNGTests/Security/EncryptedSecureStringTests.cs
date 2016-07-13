using mRemoteNG.Security;
using NUnit.Framework;

namespace mRemoteNGTests.Security
{
    [TestFixture()]
    public class EncryptedSecureStringTests
    {
        private readonly string _clearTextData = "MySecureData";

        [Test]
        public void CanCreateEncryptedSecureString()
        {
            Assert.That(new EncryptedSecureString(), Is.InstanceOf<EncryptedSecureString>());
        }

        [Test]
        public void CanAssignStringValue()
        {
            var encryptedSecString = new EncryptedSecureString();
            TestDelegate testDelegate = () => encryptedSecString.SetValue(_clearTextData);
            Assert.DoesNotThrow(testDelegate);
        }

        [Test]
        public void EncryptedValueIsSameAsOriginalValue()
        {
            var encryptedSecString = new EncryptedSecureString();
            encryptedSecString.SetValue(_clearTextData);
            var decryptedData = encryptedSecString.GetClearTextValue();
            Assert.That(decryptedData, Is.EqualTo(_clearTextData));
        }

        [Test]
        public void CreatingMultipleEncryptedSecureStrings()
        {
            var encString1 = new EncryptedSecureString();
            encString1.SetValue(_clearTextData);
            var encString2 = new EncryptedSecureString();
            encString2.SetValue("somevalue");
            var decryptedString = encString1.GetClearTextValue();
            Assert.That(decryptedString, Is.EqualTo(_clearTextData));
        }
    }
}