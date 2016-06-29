using mRemoteNG.Credential;
using mRemoteNG.UI.Forms;
using NUnit.Framework;
using NUnit.Extensions.Forms;

namespace mRemoteNGTests.UI.Forms
{
    [TestFixture]
    public class CredentialManagerTests
    {
        private FrmCredentialManager _credentialManagerForm;
        private CredentialList _credentialList;

        [SetUp]
        public void Setup()
        {
            _credentialList = new CredentialList();
            _credentialManagerForm = new FrmCredentialManager(_credentialList);
            _credentialManagerForm.Show();
        }

        [TearDown]
        public void Teardown()
        {
            _credentialManagerForm.Dispose();
            while (_credentialManagerForm.Disposing)
            {
            }
            _credentialManagerForm = null;
        }

        private void AddTestCredentialToList()
        {
            _credentialList.Add(new CredentialInfo());
        }

        [Test]
        public void CredentialManagerFormLoads()
        {
            var credentialFormTester = new FormTester(_credentialManagerForm.Name);
            Assert.That(credentialFormTester.Count, Is.EqualTo(1));
        }

        [Test]
        public void ClickingAddButtonOpensCredentialEditorForm()
        {
            var addCredentialButtonTester = new ButtonTester("btnAddCredential");
            addCredentialButtonTester.Click();
            var credentialEditorFormTester = new FormTester("FrmCredentialEditor");
            Assert.That(credentialEditorFormTester.Properties.Visible, Is.True);
        }

        [Test]
        public void EditButtonIsDiabledWhenCredentialItemNotSelected()
        {
            var editCredentialButtonTester = new ButtonTester("btnEditCredential");
            Assert.That(editCredentialButtonTester.Properties.Enabled, Is.False);
        }

        [Test]
        public void EditButtonIsEnabledWhenCredentialItemIsSelected()
        {
            var editCredentialButtonTester = new ButtonTester("btnEditCredential");
            var credentialListView = new ObjectListViewTester("olvCredentialList");
            AddTestCredentialToList();
            credentialListView.Select(0);
            Assert.That(editCredentialButtonTester.Properties.Enabled, Is.True);
        }

        [Test]
        public void RemoveButtonIsDiabledWhenCredentialItemNotSelected()
        {
            var removeCredentialButton = new ButtonTester("btnRemoveCredential");
            Assert.That(removeCredentialButton.Properties.Enabled, Is.False);
        }

        [Test]
        public void RemoveButtonIsEnabledWhenCredentialItemIsSelected()
        {
            var removeCredentialButton = new ButtonTester("btnRemoveCredential");
            var credentialListView = new ObjectListViewTester("olvCredentialList");
            AddTestCredentialToList();
            credentialListView.Select(0);
            Assert.That(removeCredentialButton.Properties.Enabled, Is.True);
        }

        [Test]
        public void RemovingLastItemInTheListWorks()
        {
            var removeCredentialButton = new ButtonTester("btnRemoveCredential");
            var credentialListView = new ObjectListViewTester("olvCredentialList");
            AddTestCredentialToList();
            credentialListView.Select(0);
            removeCredentialButton.Click();
            Assert.That(credentialListView.GetItemCount(), Is.EqualTo(0));
        }

        [Test]
        public void RemoveButtonRemovesSingleSelectedItem()
        {
            var removeCredentialButton = new ButtonTester("btnRemoveCredential");
            var credentialListView = new ObjectListViewTester("olvCredentialList");
            AddTestCredentialToList();
            AddTestCredentialToList();
            AddTestCredentialToList();
            var testCredentialThatWillBeRemoved = _credentialList[1];
            credentialListView.Select(1);
            removeCredentialButton.Click();
            Assert.That(credentialListView.Contains(testCredentialThatWillBeRemoved), Is.False);
        }

        [Test]
        public void RemoveButtonRemovesMultipleSelectedItem()
        {
            var removeCredentialButton = new ButtonTester("btnRemoveCredential");
            var credentialListView = new ObjectListViewTester("olvCredentialList");
            AddTestCredentialToList();
            AddTestCredentialToList();
            AddTestCredentialToList();
            var itemsToBeRemoved = new[] { _credentialList[1], _credentialList[2] };
            credentialListView.Properties.SelectedObjects = itemsToBeRemoved;
            removeCredentialButton.Click();
            Assert.That(credentialListView.Contains(itemsToBeRemoved), Is.False);
        }

        [Test]
        public void CloseButtonClosesForm()
        {
            var cancelButton = new ButtonTester("btnClose");
            cancelButton.Click();
            var credentialFormTester = new FormTester(_credentialManagerForm.Name);
            Assert.That(credentialFormTester.Count, Is.EqualTo(0));
        }
    }
}