using System;
using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using NSubstitute;
using NUnit.Framework;


namespace mRemoteNGTests.Credential
{
    public class CredentialRepositoryListTests
    {
        private CredentialRepositoryList _credentialCatalog;

        [SetUp]
        public void Setup()
        {
            _credentialCatalog = new CredentialRepositoryList();
        }

        [Test]
        public void RepositoryListIsInitiallyEmpty()
        {
            Assert.That(_credentialCatalog.CredentialProviders, Is.Empty);
        }

        [Test]
        public void RepositoryIsInListAfterBeingAdded()
        {
            var provider = Substitute.For<ICredentialRepository>();
            _credentialCatalog.AddProvider(provider);
            Assert.That(_credentialCatalog.CredentialProviders, Does.Contain(provider));
        }

        [Test]
        public void WillNotAddDuplicateRepositories()
        {
            var provider1 = Substitute.For<ICredentialRepository>();
            var provider2 = Substitute.For<ICredentialRepository>();
            var id = Guid.NewGuid();
            provider1.Config.Id.Returns(id);
            provider2.Config.Id.Returns(id);
            _credentialCatalog.AddProvider(provider1);
            _credentialCatalog.AddProvider(provider2);
            Assert.That(_credentialCatalog.CredentialProviders.Count(), Is.EqualTo(1));
        }

        [Test]
        public void RepositoryNotInListAfterBeingRemoved()
        {
            var provider1 = Substitute.For<ICredentialRepository>();
            provider1.Config.Id.Returns(Guid.NewGuid());
            var provider2 = Substitute.For<ICredentialRepository>();
            provider2.Config.Id.Returns(Guid.NewGuid());
            _credentialCatalog.AddProvider(provider1);
            _credentialCatalog.AddProvider(provider2);
            _credentialCatalog.RemoveProvider(provider1);
            Assert.That(_credentialCatalog.CredentialProviders, Is.EquivalentTo(new[] {provider2}));
        }

        [Test]
        public void TryingToRemoveRepositoryThatIsntInTheListDoesNothing()
        {
            _credentialCatalog.RemoveProvider(Substitute.For<ICredentialRepository>());
            Assert.That(_credentialCatalog.CredentialProviders, Is.Empty);
        }

        [Test]
        public void WeCanRetrieveASpecificCredentialRecord()
        {
            var expectedCredential = Substitute.For<ICredentialRecord>();
            expectedCredential.Id.Returns(new Guid());
            var repo = Substitute.For<ICredentialRepository>();
            repo.CredentialRecords.Returns(new List<ICredentialRecord> {expectedCredential});
            _credentialCatalog.AddProvider(repo);
            var retrievedCredential = _credentialCatalog.GetCredentialRecord(expectedCredential.Id);
            Assert.That(retrievedCredential, Is.EqualTo(expectedCredential));
        }

        [Test]
        public void GetCredentialReturnsNullIfNoRecordFound()
        {
            var repo = Substitute.For<ICredentialRepository>();
            _credentialCatalog.AddProvider(repo);
            var retrievedCredential = _credentialCatalog.GetCredentialRecord(Guid.NewGuid());
            Assert.That(retrievedCredential, Is.Null);
        }
    }
}