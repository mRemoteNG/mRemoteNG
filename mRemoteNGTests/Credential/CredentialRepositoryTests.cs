using System.Collections.Generic;
using mRemoteNG.Credential;
using NUnit.Framework;


namespace mRemoteNGTests.Credential
{
    public class CredentialRepositoryTests
    {
        private CredentialRepository _credentialRepository;

        [SetUp]
        public void Setup()
        {
            _credentialRepository = new CredentialRepository();
        }

        [Test]
        public void ImplementsIList()
        {
            Assert.That(_credentialRepository, Is.AssignableTo<IList<ICredential>>());
        }

        [Test]
        public void ImplementsICredentialRepository()
        {
            Assert.That(_credentialRepository, Is.AssignableTo<ICredentialRepository>());
        }
    }
}