using System.Threading;
using System.Windows.Forms;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class OptionsConnectionsPageTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void ConnectionsPageLinkExistsInListView()
        {
            ListViewTester listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            Assert.That(listViewTester.Items[2].Text, Does.Match("Connections"));
        }

        [Test]
        public void ConnectionsIconShownInListView()
        {
            ListViewTester listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            Assert.That(listViewTester.Items[2].ImageList, Is.Not.Null);
        }

        [Test]
        public void SelectingConnectionsPageLoadsSettings()
        {
            ListViewTester listViewTester = new ListViewTester("lstOptionPages", _optionsForm);
            listViewTester.Select("Connections");
            CheckBox checkboxTester = _optionsForm.FindControl<CheckBox>("chkSingleClickOnConnectionOpensIt");
            Assert.That(checkboxTester.Text, Does.Match("Single click on connection"));
        }
    }
}