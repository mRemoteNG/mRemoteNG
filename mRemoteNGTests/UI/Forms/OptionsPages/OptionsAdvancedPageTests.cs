using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using mRemoteNG.App;
using mRemoteNG.Config.Putty;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.Properties;
using mRemoteNG.UI.Forms.OptionsPages;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    [NonParallelizable]
    public class OptionsAdvancedPageTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void AdvancedPageLinkExistsInListView()
        {
            ListViewTester listViewTester = new("lstOptionPages", _optionsForm);
            Assert.That(listViewTester.Items[10].Text, Does.Match("Advanced"));
        }

        [Test]
        public void AdvancedIconShownInListView()
        {
            ListViewTester listViewTester = new("lstOptionPages", _optionsForm);
            Assert.That(listViewTester.Items[10].ImageList, Is.Not.Null);
        }

        [Test]
        public void SelectingAdvancedPageLoadsSettings()
        {
            ListViewTester listViewTester = new("lstOptionPages", _optionsForm);
            listViewTester.Select("Advanced");
            
            CheckBox checkboxTester = _optionsForm.FindControl<CheckBox>("chkAutomaticReconnect");
            Assert.That(checkboxTester.Text, Is.EqualTo("Automatically try to reconnect when disconnected from server (RDP && ICA only)"));

            CheckBox showPuttySessionsCheckBox = _optionsForm.FindControl<CheckBox>("chkShowPuttySessionsInTree");
            Assert.That(showPuttySessionsCheckBox.Text, Is.EqualTo("Show PuTTY saved sessions in connection tree"));
        }

        [Test]
        public void SavingAdvancedPagePersistsPuttySessionsVisibilitySetting()
        {
            bool originalValue = OptionsAdvancedPage.Default.ShowPuttySessionsInTree;
            try
            {
                ListViewTester listViewTester = new("lstOptionPages", _optionsForm);
                listViewTester.Select("Advanced");

                CheckBox showPuttySessionsCheckBox = _optionsForm.FindControl<CheckBox>("chkShowPuttySessionsInTree");
                showPuttySessionsCheckBox.Checked = !originalValue;

                _optionsForm.SaveAllOptions();

                Assert.That(OptionsAdvancedPage.Default.ShowPuttySessionsInTree, Is.EqualTo(!originalValue));
            }
            finally
            {
                OptionsAdvancedPage.Default.ShowPuttySessionsInTree = originalValue;
            }
        }

        [Test]
        public void UpdatePuttySessionsVisibility_RemovesAndRestoresPuttyRootNode()
        {
            var manager = PuttySessionsManager.Instance;
            bool originalSetting = OptionsAdvancedPage.Default.ShowPuttySessionsInTree;
            RootPuttySessionsNodeInfo[] originalManagerRoots = manager.RootPuttySessionsNodes.ToArray();
            ConnectionTreeModel originalModel = Runtime.ConnectionsService.ConnectionTreeModel;

            RootNodeInfo regularRoot = new RootNodeInfo(RootNodeType.Connection);
            RootPuttySessionsNodeInfo puttyRoot = new RootPuttySessionsNodeInfo();
            ConnectionTreeModel testModel = new ConnectionTreeModel();

            try
            {
                testModel.AddRootNode(regularRoot);
                testModel.AddRootNode(puttyRoot);
                typeof(mRemoteNG.Connection.ConnectionsService)
                    .GetProperty(nameof(Runtime.ConnectionsService.ConnectionTreeModel), BindingFlags.Instance | BindingFlags.Public)!
                    .SetValue(Runtime.ConnectionsService, testModel);

                manager.RootPuttySessionsNodes.Clear();
                manager.RootPuttySessionsNodes.Add(puttyRoot);

                using AdvancedPage page = new AdvancedPage();
                page.chkShowPuttySessionsInTree.Checked = false;
                typeof(AdvancedPage).GetMethod("UpdatePuttySessionsVisibility", BindingFlags.Instance | BindingFlags.NonPublic)!
                    .Invoke(page, null);
                Assert.That(testModel.RootNodes, Has.Count.EqualTo(1));
                Assert.That(testModel.RootNodes, Does.Not.Contain(puttyRoot));

                page.chkShowPuttySessionsInTree.Checked = true;
                typeof(AdvancedPage).GetMethod("UpdatePuttySessionsVisibility", BindingFlags.Instance | BindingFlags.NonPublic)!
                    .Invoke(page, null);
                Assert.That(testModel.RootNodes, Contains.Item(puttyRoot));
                Assert.That(testModel.RootNodes.IndexOf(puttyRoot), Is.EqualTo(1));
            }
            finally
            {
                OptionsAdvancedPage.Default.ShowPuttySessionsInTree = originalSetting;
                manager.RootPuttySessionsNodes.Clear();
                manager.RootPuttySessionsNodes.AddRange(originalManagerRoots);
                typeof(mRemoteNG.Connection.ConnectionsService)
                    .GetProperty(nameof(Runtime.ConnectionsService.ConnectionTreeModel), BindingFlags.Instance | BindingFlags.Public)!
                    .SetValue(Runtime.ConnectionsService, originalModel);
            }
        }
    }
}