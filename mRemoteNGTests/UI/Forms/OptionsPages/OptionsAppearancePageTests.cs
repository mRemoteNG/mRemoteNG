using System;
using NUnit.Framework;
using mRemoteNGTests.UI.Forms;
using NUnit.Extensions.Forms;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    [TestFixture]
    public class OptionsAppearancePageTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void AppearancePageLinkExistsInListView()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            Assert.That(listViewTester.Items[1].Text, Does.Match("Appearance"));
        }

        [Test]
        public void IconShownInListView()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            Assert.That(listViewTester.Items[1].ImageList, Is.Not.Null);
        }

        [Test]
        public void SelectingAppearancePageLoadsSettings()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            listViewTester.Select("Appearance");
            CheckBoxTester checkboxTester = new CheckBoxTester("chkShowSystemTrayIcon", _optionsForm);
            Assert.That(checkboxTester.Text, Does.Match("show notification area icon"));
        }
    }
}