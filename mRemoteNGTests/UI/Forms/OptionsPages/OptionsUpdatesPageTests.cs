using System.Threading;
using System.Windows.Forms;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class OptionsUpdatesPageTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void UpdatesPageLinkExistsInListView()
        {
            ListViewTester listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            Assert.That(listViewTester.Items[7].Text, Does.Match("Updates"));
        }

        [Test]
        public void UpdatesIconShownInListView()
        {
            ListViewTester listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            Assert.That(listViewTester.Items[7].ImageList, Is.Not.Null);
        }

        [Test]
        public void SelectingUpdatesPageLoadsSettings()
        {
            ListViewTester listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            listViewTester.Select("Updates");
            CheckBox checkboxTester = _optionsForm.FindControl<CheckBox>("chkCheckForUpdatesOnStartup");
            Assert.That(checkboxTester.Text, Does.Match("Check for updates"));
        }
    }
}