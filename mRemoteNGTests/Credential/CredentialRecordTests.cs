using System;
using System.Security;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using NUnit.Framework;


namespace mRemoteNGTests.Credential
{
    public class CredentialRecordTests
    {
        private CredentialRecord _credentialRecord;

        [SetUp]
        public void Setup()
        {
            _credentialRecord = new CredentialRecord
            {
                Username = "userHere",
                Domain = "domainHere",
                Password = "somepass".ConvertToSecureString()
            };
        }

        [Test]
        public void IdIsUnique()
        {
            var credRecord2 = new CredentialRecord();
            Assert.That(_credentialRecord.Id, Is.Not.EqualTo(credRecord2.Id));
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

        [Test]
        public void CreateWithExistingGuid()
        {
            var customGuid = new Guid();
            _credentialRecord = new CredentialRecord(customGuid);
            Assert.That(_credentialRecord.Id, Is.EqualTo(customGuid));
        }

        [Test]
        public void CopyConstructorGeneratesNewGuid()
        {
            var cred2 = new CredentialRecord(_credentialRecord);
            Assert.That(cred2.Id, Is.Not.EqualTo(_credentialRecord.Id));
        }

        [Test]
        public void CopyConstructorCopiesUsername()
        {
            var cred2 = new CredentialRecord(_credentialRecord);
            Assert.That(cred2.Username, Is.EqualTo(_credentialRecord.Username));
        }

        [Test]
        public void CopyConstructorCopiesPassword()
        {
            var cred2 = new CredentialRecord(_credentialRecord);
            Assert.That(cred2.Password, Is.EqualTo(_credentialRecord.Password));
        }

        [Test]
        public void CopyConstructorCopiesDomain()
        {
            var cred2 = new CredentialRecord(_credentialRecord);
            Assert.That(cred2.Domain, Is.EqualTo(_credentialRecord.Domain));
        }
    }
}