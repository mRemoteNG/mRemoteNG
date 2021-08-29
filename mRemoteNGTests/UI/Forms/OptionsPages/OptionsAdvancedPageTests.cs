using System.Threading;
using System.Windows.Forms;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class OptionsAdvancedPageTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void AdvancedPageLinkExistsInListView()
        {
            ListViewTester listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            Assert.That(listViewTester.Items[10].Text, Does.Match("Advanced"));
        }

        [Test]
        public void AdvancedIconShownInListView()
        {
            ListViewTester listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            Assert.That(listViewTester.Items[10].ImageList, Is.Not.Null);
        }

        [Test]
        public void SelectingAdvancedPageLoadsSettings()
        {
            ListViewTester listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            listViewTester.Select("Advanced");
            
            CheckBox checkboxTester = _optionsForm.FindControl<CheckBox>("chkAutomaticReconnect");
            Assert.That(checkboxTester.Text, Is.EqualTo("Automatically try to reconnect when disconnected from server (RDP && ICA only)"));
        }
    }
}