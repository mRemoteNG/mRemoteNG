using mRemoteNG.Credential;
using NSubstitute;
using NUnit.Framework;


namespace mRemoteNGTests.Credential
{
    public class CredentialProviderCatalogTests
    {
        private CredentialRepositoryList _credentialCatalog;

        [SetUp]
        public void Setup()
        {
            _credentialCatalog = new CredentialRepositoryList();
        }

        [Test]
        public void ProviderListIsInitiallyEmpty()
        {
            Assert.That(_credentialCatalog.CredentialProviders, Is.Empty);
        }

        [Test]
        public void ProviderIsInListAfterBeingAdded()
        {
            var provider = Substitute.For<ICredentialRepository>();
            _credentialCatalog.AddProvider(provider);
            Assert.That(_credentialCatalog.CredentialProviders, Does.Contain(provider));
        }

        [Test]
        public void ProviderNotInListAfterBeingRemoved()
        {
            var provider1 = Substitute.For<ICredentialRepository>();
            var provider2 = Substitute.For<ICredentialRepository>();
            _credentialCatalog.AddProvider(provider1);
            _credentialCatalog.AddProvider(provider2);
            _credentialCatalog.RemoveProvider(provider1);
            Assert.That(_credentialCatalog.CredentialProviders, Is.EquivalentTo(new[] {provider2}));
        }

        [Test]
        public void RemoveProviderThatIsntInTheList()
        {
            _credentialCatalog.RemoveProvider(Substitute.For<ICredentialRepository>());
            Assert.That(_credentialCatalog.CredentialProviders, Is.Empty);
        }
    }
}