using mRemoteNG.Credential;
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
        public void CredentialCatalogExists()
        {
            Assert.That(_credentialCatalog, Is.Not.Null);
        }
    }
}