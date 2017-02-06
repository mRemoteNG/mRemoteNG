using mRemoteNG.Credential;
using NSubstitute;
using NUnit.Framework;


namespace mRemoteNGTests.Credential
{
    public class CredentialRecordCatalogTests
    {
        private CredentialRecordCatalog _credentialRecordCatalog;

        [SetUp]
        public void Setup()
        {
            _credentialRecordCatalog = new CredentialRecordCatalog();
        }

        [Test]
        public void CanAddCredentialList()
        {
            var credential = Substitute.For<ICredentialRecord>();
            var credList = new CredentialListBase(Substitute.For<ICredentialProvider>()) {credential};
            _credentialRecordCatalog.AddCredentialList(credList);
            Assert.That(_credentialRecordCatalog.CredentialRecords, Does.Contain(credential));
        }

        [Test]
        public void CanRemoveCredentialList()
        {
            var credential1 = Substitute.For<ICredentialRecord>();
            var credList1 = new CredentialListBase(Substitute.For<ICredentialProvider>()) { credential1 };
            var credential2 = Substitute.For<ICredentialRecord>();
            var credList2 = new CredentialListBase(Substitute.For<ICredentialProvider>()) { credential2 };
            _credentialRecordCatalog.AddCredentialList(credList1);
            _credentialRecordCatalog.AddCredentialList(credList2);
            _credentialRecordCatalog.RemoveCredentialList(credList1);
            Assert.That(_credentialRecordCatalog.CredentialRecords, Is.EquivalentTo(credList2));
        }

        [Test]
        public void RemovingACredentialListThatIsntAddedDoesNothing()
        {
            var credList1 = new CredentialListBase(Substitute.For<ICredentialProvider>());
            _credentialRecordCatalog.RemoveCredentialList(credList1);
            Assert.That(_credentialRecordCatalog.CredentialRecords, Is.Empty);
        }

        [Test]
        public void ListOfCredentialsIsInitiallyEmpty()
        {
            Assert.That(_credentialRecordCatalog.CredentialRecords, Is.Empty);
        }
    }
}