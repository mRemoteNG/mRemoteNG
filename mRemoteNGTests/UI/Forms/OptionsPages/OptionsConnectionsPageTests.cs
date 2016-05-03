using System;
using NUnit.Framework;
using mRemoteNGTests.UI.Forms;
using NUnit.Extensions.Forms;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    [TestFixture]
    public class OptionsConnectionsPageTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void ConnectionsPageLinkExistsInListView()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            Assert.That(listViewTester.Items[3].Text, Does.Match("Connections"));
        }

        [Test]
        public void ConnectionsIconShownInListView()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            Assert.That(listViewTester.Items[3].ImageList, Is.Not.Null);
        }

        [Test]
        public void SelectingConnectionsPageLoadsSettings()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            listViewTester.Select("Connections");
            CheckBoxTester checkboxTester = new CheckBoxTester("chkSingleClickOnConnectionOpensIt", _optionsForm);
            Assert.That(checkboxTester.Text, Does.Match("Single click on connection"));
        }
    }
}