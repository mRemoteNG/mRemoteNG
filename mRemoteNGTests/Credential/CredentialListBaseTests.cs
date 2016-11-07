using System;
using System.Collections.Generic;
using mRemoteNG.Credential;
using NSubstitute;
using NUnit.Framework;


namespace mRemoteNGTests.Credential
{
    public class CredentialListBaseTests
    {
        private CredentialListBase _credentialList;

        [SetUp]
        public void Setup()
        {
            _credentialList = new CredentialListBase();
        }

        [Test]
        public void ImplementsIList()
        {
            Assert.That(_credentialList, Is.AssignableTo<IList<ICredentialRecord>>());
        }

        [Test]
        public void ImplementsICredentialRepository()
        {
            Assert.That(_credentialList, Is.AssignableTo<ICredentialList>());
        }

        [Test]
        public void GetCredentialByUuid()
        {
            var targetUuid = Guid.NewGuid();
            var credential = Substitute.For<ICredentialRecord>();
            credential.UniqueId.Returns(targetUuid);
            _credentialList.Add(credential);
            Assert.That(_credentialList.GetCredential(targetUuid), Is.EqualTo(credential));
        }

        [Test]
        public void GetCredentialByUuidNullOnNoMatch()
        {
            Assert.That(_credentialList.GetCredential(Guid.NewGuid()), Is.Null);
        }

        [Test]
        public void ContainsUsingUuidFindsCredential()
        {
            var credential = Substitute.For<CredentialRecord>(Guid.NewGuid());
            _credentialList.Add(credential);
            Assert.That(_credentialList.Contains(credential.UniqueId), Is.True);
        }

        [Test]
        public void ContainsUsingUuidReturnsFalseOnFailure()
        {
            Assert.That(_credentialList.Contains(Guid.NewGuid()), Is.False);
        }
    }
}