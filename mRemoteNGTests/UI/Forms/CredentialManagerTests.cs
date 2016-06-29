using mRemoteNG.Credential;
using mRemoteNG.UI.Forms;
using NUnit.Framework;
using NUnit.Extensions.Forms;

namespace mRemoteNGTests.UI.Forms
{
    [TestFixture]
    public class CredentialManagerTests
    {
        private FrmCredentialManager _credentialManager;
        private CredentialList _credentialList;

        [SetUp]
        public void Setup()
        {
            _credentialList = new CredentialList();
            _credentialManager = new FrmCredentialManager(_credentialList);
            _credentialManager.Show();
        }

        [TearDown]
        public void Teardown()
        {
            _credentialManager.Dispose();
            while (_credentialManager.Disposing) ;
            _credentialManager = null;
        }

        [Test]
        public void PasswordFormText()
        {
            var formTester = new FormTester(_credentialManager.Name);
            Assert.That(formTester.Count, Is.EqualTo(1));
        }
    }
}