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
        public void IsAList()
        {
            Assert.That(_credentialRepository, Is.AssignableTo<IList<ICredential>>());
        }
    }
}