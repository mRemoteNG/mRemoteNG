using NUnit.Framework;
using NUnit.Extensions.Forms;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    [TestFixture]
    public class OptionsUpdatesPageTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void UpdatesPageLinkExistsInListView()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            Assert.That(listViewTester.Items[5].Text, Does.Match("Updates"));
        }

        [Test]
        public void UpdatesIconShownInListView()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            Assert.That(listViewTester.Items[5].ImageList, Is.Not.Null);
        }

        [Test]
        public void SelectingUpdatesPageLoadsSettings()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            listViewTester.Select("Updates");
            CheckBoxTester checkboxTester = new CheckBoxTester("chkCheckForUpdatesOnStartup", _optionsForm);
            Assert.That(checkboxTester.Text, Does.Match("Check for updates"));
        }
    }
}