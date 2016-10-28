using System.Security;
using mRemoteNG.Credential;
using NUnit.Framework;


namespace mRemoteNGTests.Credential
{
    public class CredentialRecordTests
    {
        private CredentialRecord _credentialRecord;

        [SetUp]
        public void Setup()
        {
            _credentialRecord = new CredentialRecord();
        }

        [Test]
        public void HasUsername()
        {
            Assert.That(_credentialRecord.Username, Is.Not.Null);
        }

        [Test]
        public void PasswordIsSecureString()
        {
            Assert.That(_credentialRecord.Password, Is.TypeOf<SecureString>());
        }

        [Test]
        public void HasDomain()
        {
            Assert.That(_credentialRecord.Domain, Is.Not.Null);
        }
    }
}