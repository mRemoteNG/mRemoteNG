using mRemoteNG.Credential;
using NSubstitute;
using NUnit.Framework;


namespace mRemoteNGTests.Credential
{
    public class CredentialProviderCatalogTests
    {
        private CredentialProviderCatalog _credentialCatalog;

        [SetUp]
        public void Setup()
        {
            _credentialCatalog = new CredentialProviderCatalog();
        }

        [Test]
        public void ProviderListIsInitiallyEmpty()
        {
            Assert.That(_credentialCatalog.CredentialProviders, Is.Empty);
        }

        [Test]
        public void ProviderIsInListAfterBeingAdded()
        {
            var provider = Substitute.For<ICredentialProvider>();
            _credentialCatalog.AddProvider(provider);
            Assert.That(_credentialCatalog.CredentialProviders, Does.Contain(provider));
        }

        [Test]
        public void ProviderNotInListAfterBeingRemoved()
        {
            var provider1 = Substitute.For<ICredentialProvider>();
            var provider2 = Substitute.For<ICredentialProvider>();
            _credentialCatalog.AddProvider(provider1);
            _credentialCatalog.AddProvider(provider2);
            _credentialCatalog.RemoveProvider(provider1);
            Assert.That(_credentialCatalog.CredentialProviders, Is.EquivalentTo(new[] {provider2}));
        }

        [Test]
        public void RemoveProviderThatIsntInTheList()
        {
            _credentialCatalog.RemoveProvider(Substitute.For<ICredentialProvider>());
            Assert.That(_credentialCatalog.CredentialProviders, Is.Empty);
        }
    }
}