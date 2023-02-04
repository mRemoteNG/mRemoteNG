using System.Threading;
using System.Windows.Forms;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class OptionsTabsPanelPageTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void TabsPanelPageLinkExistsInListView()
        {
            ListViewTester listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            Assert.That(listViewTester.Items[3].Text, Does.Match("Tabs & Panels"));
        }

        [Test]
        public void TabsPanelIconShownInListView()
        {
            ListViewTester listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            Assert.That(listViewTester.Items[3].ImageList, Is.Not.Null);
        }

        [Test]
        public void SelectingTabsPanelPageLoadsSettings()
        {
            ListViewTester listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            listViewTester.Select("Tabs & Panels");
            CheckBox checkboxTester = _optionsForm.FindControl<CheckBox>("chkAlwaysShowPanelTabs");
            Assert.That(checkboxTester.Text, Does.Match("Always show panel tabs"));
        }
    }
}