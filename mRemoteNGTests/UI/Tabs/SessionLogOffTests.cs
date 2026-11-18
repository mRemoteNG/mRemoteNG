using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Properties;
using mRemoteNG.UI.TaskDialog;
using mRemoteNG.UI.Tabs;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Tabs
{
    [TestFixture]
    [NonParallelizable]
    public class SessionLogOffTests
    {
        private bool _originalConfirm;

        [SetUp]
        public void SetUp()
        {
            _originalConfirm = OptionsAdvancedPage.Default.ConfirmSessionLogOff;
            OptionsAdvancedPage.Default.ConfirmSessionLogOff = true;
        }

        [TearDown]
        public void TearDown()
        {
            OptionsAdvancedPage.Default.ConfirmSessionLogOff = _originalConfirm;
        }

        [Test]
        public void Yes_LogsOffAndKeepsAsking()
        {
            bool proceed = SessionLogOff.ApplyConfirmation(OptionsAdvancedPage.Default, DialogResult.Yes, dontAskAgain: false);

            Assert.That(proceed, Is.True);
            Assert.That(OptionsAdvancedPage.Default.ConfirmSessionLogOff, Is.True);
        }

        [Test]
        public void YesWithDontAskAgain_LogsOffAndStopsAsking()
        {
            bool proceed = SessionLogOff.ApplyConfirmation(OptionsAdvancedPage.Default, DialogResult.Yes, dontAskAgain: true);

            Assert.That(proceed, Is.True);
            Assert.That(OptionsAdvancedPage.Default.ConfirmSessionLogOff, Is.False);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void No_DoesNotLogOffAndKeepsAsking(bool dontAskAgain)
        {
            // Someone who answers No wants to be asked; "Don't ask again" only counts with Yes.
            bool proceed = SessionLogOff.ApplyConfirmation(OptionsAdvancedPage.Default, DialogResult.No, dontAskAgain);

            Assert.That(proceed, Is.False);
            Assert.That(OptionsAdvancedPage.Default.ConfirmSessionLogOff, Is.True);
        }
    }

    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class TaskDialogDefaultToNoTests
    {
        [TestCase(false, DialogResult.Yes)]
        [TestCase(true, DialogResult.No)]
        public void YesNo_EnterPressesTheChosenDefault(bool defaultToNo, DialogResult expected)
        {
            using frmTaskDialog dialog = new()
            {
                MainInstruction = "Log off SERVER01?",
                Buttons = ETaskDialogButtons.YesNo,
                DefaultToNo = defaultToNo,
            };
            dialog.BuildForm();

            Assert.That(((Button)dialog.AcceptButton).DialogResult, Is.EqualTo(expected));
            // Escape still answers No either way.
            Assert.That(((Button)dialog.CancelButton).DialogResult, Is.EqualTo(DialogResult.No));
        }
    }
}
