using System;
using NUnit.Framework;
using mRemoteNGTests.UI.Forms;
using NUnit.Extensions.Forms;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    [TestFixture]
    public class OptionsSQLPageTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void SQLPageLinkExistsInListView()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            Assert.That(listViewTester.Items[4].Text, Does.Match("SQL Server"));
        }

        [Test]
        public void SQLIconShownInListView()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            Assert.That(listViewTester.Items[4].ImageList, Is.Not.Null);
        }

        [Test]
        public void SelectingSQLPageLoadsSettings()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            listViewTester.Select("SQL Server");
            CheckBoxTester checkboxTester = new CheckBoxTester("chkUseSQLServer", _optionsForm);
            Assert.That(checkboxTester.Text, Does.Match("Use SQL"));
        }
    }
}