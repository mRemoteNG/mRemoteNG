using NUnit.Extensions.Forms;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    [TestFixture]
    public class OptionsTabsPanelPageTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void TabsPanelPageLinkExistsInListView()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            Assert.That(listViewTester.Items[2].Text, Does.Match("Tabs & Panels"));
        }

        [Test]
        public void TabsPanelIconShownInListView()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            Assert.That(listViewTester.Items[2].ImageList, Is.Not.Null);
        }

        [Test]
        public void SelectingTabsPanelPageLoadsSettings()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            listViewTester.Select("Tabs & Panels");
            CheckBoxTester checkboxTester = new CheckBoxTester("chkAlwaysShowPanelTabs", _optionsForm);
            Assert.That(checkboxTester.Text, Does.Match("Always show panel tabs"));
        }
    }
}