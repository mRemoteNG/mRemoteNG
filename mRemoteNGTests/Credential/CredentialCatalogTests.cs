using mRemoteNG.Credential;
using NUnit.Framework;


namespace mRemoteNGTests.Credential
{
    public class CredentialCatalogTests
    {
        private CredentialCatalog _credentialCatalog;

        [SetUp]
        public void Setup()
        {
            _credentialCatalog = new CredentialCatalog();
        }

        [Test]
        public void CredentialCatalogExists()
        {
            Assert.That(_credentialCatalog, Is.Not.Null);
        }
    }
}