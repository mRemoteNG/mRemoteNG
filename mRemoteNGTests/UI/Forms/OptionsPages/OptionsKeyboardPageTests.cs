using System;
using NUnit.Framework;
using mRemoteNGTests.UI.Forms;
using NUnit.Extensions.Forms;

namespace mRemoteNGTests.UI.Forms.OptionsPages
{
    [TestFixture]
    public class OptionsKeyboardPageTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void KeyboardPageLinkExistsInListView()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            Assert.That(listViewTester.Items[7].Text, Does.Match("Keyboard"));
        }

        [Test]
        public void KeyboardIconShownInListView()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            Assert.That(listViewTester.Items[7].ImageList, Is.Not.Null);
        }

        [Test]
        public void SelectingKeyboardPageLoadsSettings()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            listViewTester.Select("Keyboard");
            ButtonTester buttonTester = new ButtonTester("btnResetKeyboardShortcuts", _optionsForm);
            Assert.That(buttonTester.Text, Does.Match("Reset"));
        }
    }
}