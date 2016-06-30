using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Forms;
using NUnit.Framework;
using NUnit.Extensions.Forms;

namespace mRemoteNGTests.UI.Forms
{
    [TestFixture]
    public class CredentialEditorTests
    {
        private FrmCredentialEditor _credentialEditorForm;
        private CredentialList _credentialList;

        [SetUp]
        public void Setup()
        {
            _credentialList = new CredentialList {new CredentialInfo()};
        }

        [TearDown]
        public void TearDown()
        {
            _credentialList = null;
            _credentialEditorForm?.Dispose();
        }

        private void CreateCredentialEditorForm(CredentialInfo credentialInfo = null)
        {
            _credentialEditorForm = new FrmCredentialEditor(_credentialList, credentialInfo);
            _credentialEditorForm.Show();
        }

        [Test]
        public void CredentialEditorFormLoadsWhenNotGivenAnExistingCredentialInfo()
        {
            CreateCredentialEditorForm();
            var credentialFormTester = new FormTester(_credentialEditorForm.Name);
            Assert.That(credentialFormTester.Count, Is.EqualTo(1));
        }

        [Test]
        public void CredentialEditorFormLoadsWhenGivenAnExistingCredentialInfo()
        {
            CreateCredentialEditorForm(new CredentialInfo());
            var credentialFormTester = new FormTester(_credentialEditorForm.Name);
            Assert.That(credentialFormTester.Count, Is.EqualTo(1));
        }

        [Test]
        public void CancelClosesForm()
        {
            CreateCredentialEditorForm();
            var cancelButton = new ButtonTester("btnCancel");
            cancelButton.Click();
            var credentialFormTester = new FormTester(_credentialEditorForm.Name);
            Assert.That(credentialFormTester.Count, Is.EqualTo(0));
        }

        [Test]
        public void EntryNameFieldIsEditable()
        {
            CreateCredentialEditorForm();
            var entryNameField = new TextBoxTester("txtEntryName");
            Assert.That(entryNameField.Properties.Enabled, Is.True);
        }

        [Test]
        public void CueBannerSetOnPasswordFieldWhenEditingAlreadyExistingPassword()
        {
            var credential = new CredentialInfo {Password = "testPassword".ConvertToSecureString()};
            CreateCredentialEditorForm(credential);
            var passwordBox = new TextBoxTester("secureTextBoxManualEntryPassword");
            var cueBannerText = passwordBox.Properties.GetCueBannerText();
            Assert.That(cueBannerText, Is.Not.Null);
        }
    }
}