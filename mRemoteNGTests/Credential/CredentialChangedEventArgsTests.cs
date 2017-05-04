using System;
using mRemoteNG.Credential;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Credential
{
    public class CredentialChangedEventArgsTests
    {
        private ICredentialRecord _credentialRecord;
        private ICredentialRepository _credentialRepository;

        [SetUp]
        public void Setup()
        {
            _credentialRecord = Substitute.For<ICredentialRecord>();
            _credentialRepository = Substitute.For<ICredentialRepository>();
        }

        [Test]
        public void CantProvideNullCredentialRecord()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new CredentialChangedEventArgs(null, _credentialRepository));
        }

        [Test]
        public void CantProvideNullCredentialRepository()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new CredentialChangedEventArgs(_credentialRecord, null));
        }

        [Test]
        public void CredentialRecordPropertySetFromCtor()
        {
            var sut = new CredentialChangedEventArgs(_credentialRecord, _credentialRepository);
            Assert.That(sut.CredentialRecord, Is.EqualTo(_credentialRecord));
        }

        [Test]
        public void RepositoryPropertySetFromCtor()
        {
            var sut = new CredentialChangedEventArgs(_credentialRecord, _credentialRepository);
            Assert.That(sut.Repository, Is.EqualTo(_credentialRepository));
        }
    }
}