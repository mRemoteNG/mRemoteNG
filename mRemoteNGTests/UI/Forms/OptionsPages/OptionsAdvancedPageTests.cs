using System;
using NUnit.Framework;
using mRemoteNGTests.UI.Forms;
using NUnit.Extensions.Forms;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    [TestFixture]
    public class OptionsAdvancedPageTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void AdvancedPageLinkExistsInListView()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            Assert.That(listViewTester.Items[8].Text, Does.Match("Advanced"));
        }

        [Test]
        public void AdvancedIconShownInListView()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            Assert.That(listViewTester.Items[8].ImageList, Is.Not.Null);
        }

        [Test]
        public void SelectingAdvancedPageLoadsSettings()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            listViewTester.Select("Advanced");
            CheckBoxTester checkboxTester = new CheckBoxTester("chkWriteLogFile", _optionsForm);
            Assert.That(checkboxTester.Text, Does.Match("Write log file"));
        }
    }
}