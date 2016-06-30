using mRemoteNG.Credential;
using mRemoteNG.Security;
using NUnit.Framework;

namespace mRemoteNGTests.Credential
{
    public class CredentialInfoTests
    {
        private CredentialInfo _credentialInfo;

        [SetUp]
        public void Setup()
        {
            _credentialInfo = new CredentialInfo();
        }

        [TearDown]
        public void Teardown()
        {
            _credentialInfo = null;
        }

        [Test]
        public void CredentialInfoIsInitializedWithAUuid()
        {
            Assert.That(_credentialInfo.Uuid, Is.Not.Null);
        }

        [Test]
        public void CredentialInfoIsInitializedWithAName()
        {
            Assert.That(_credentialInfo.Name, Is.Not.Null);
        }

        [Test]
        public void CredentialInfoIsInitializedWithACredentialSource()
        {
            Assert.That(_credentialInfo.CredentialSource, Is.Not.Null);
        }

        [Test]
        public void UuidIsUniqueBetweenCredentialObjects()
        {
            var otherCredential = new CredentialInfo();
            Assert.That(_credentialInfo.Uuid, Is.Not.EqualTo(otherCredential.Uuid));
        }

        [Test]
        public void ClonedCredentialInfoObjectFieldsAreIdenticalToOriginal()
        {
            var clonedCredential = _credentialInfo.Clone();
            foreach (var property in typeof(CredentialInfo).GetProperties())
            {
                Assert.That(property.GetValue(_credentialInfo), Is.EqualTo(property.GetValue(clonedCredential)));
            }
        }

        [Test]
        public void SetPasswordFromUnsecureStringDoesNotCorruptTheString()
        {
            var unsecurePassword = "testPassword";
            _credentialInfo.SetPasswordFromUnsecureString(unsecurePassword);
            Assert.That(_credentialInfo.Password.ConvertToUnsecureString(), Is.EqualTo(unsecurePassword));
        }
    }
}