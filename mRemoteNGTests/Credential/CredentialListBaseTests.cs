using System.Collections.Generic;
using mRemoteNG.Credential;
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
            Assert.That(_credentialList, Is.AssignableTo<IList<ICredential>>());
        }

        [Test]
        public void ImplementsICredentialRepository()
        {
            Assert.That(_credentialList, Is.AssignableTo<ICredentialList>());
        }
    }
}