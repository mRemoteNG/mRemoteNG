using mRemoteNG.App;
using mRemoteNG.Controls;
using mRemoteNG.Forms;
using mRemoteNG.Messages;
using mRemoteNG.UI.Window;
using NUnit.Extensions.Forms;
using NUnit.Framework;
using System;
using System.Threading;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNGTests.UI.Forms
{
    [TestFixture]
    public class OptionsFormTests : OptionsFormSetupAndTeardown
    {
        [Test]
        public void ClickingCloseButtonClosesTheForm()
        {
            bool eventFired = false;
            _optionsForm.FormClosed += (o, e) => eventFired = true;
            ButtonTester cancelButton = new ButtonTester("CancelButtonControl", _optionsForm);
            cancelButton.Click();
            Assert.That(eventFired, Is.True);
        }

        [Test]
        public void ClickingOKButtonClosesTheForm()
        {
            bool eventFired = false;
            _optionsForm.FormClosed += (o, e) => eventFired = true;
            ButtonTester cancelButton = new ButtonTester("OkButton", _optionsForm);
            cancelButton.Click();
            Assert.That(eventFired, Is.True);
        }

        [Test]
        public void ListViewContainsOptionsPages()
        {
            ListViewTester listViewTester = new ListViewTester("PageListView", _optionsForm);
            Assert.That(listViewTester.Items.Count, Is.EqualTo(8));
        }
    }
}