using System;
using NUnit.Framework;
using mRemoteNGTests.UI.Forms;
using NUnit.Extensions.Forms;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    [TestFixture]
    public class OptionsThemePageTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void ThemePageLinkExistsInListView()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            Assert.That(listViewTester.Items[6].Text, Does.Match("Theme"));
        }

        [Test]
        public void ThemeIconShownInListView()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            Assert.That(listViewTester.Items[6].ImageList, Is.Not.Null);
        }

        [Test]
        public void SelectingThemePageLoadsSettings()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            listViewTester.Select("Theme");
            ButtonTester buttonTester = new ButtonTester("btnThemeNew", _optionsForm);
            Assert.That(buttonTester.Text, Does.Match("New"));
        }
    }
}