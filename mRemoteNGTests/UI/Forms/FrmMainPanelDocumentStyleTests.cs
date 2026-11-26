using System.Threading;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNGTests.UI.Forms
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class FrmMainPanelDocumentStyleTests
    {
        private ConnectionWindow _generalPanel;
        private ConnectionWindow _secondPanel;
        private DockContent _updateWindow;

        [SetUp]
        public void Setup()
        {
            _generalPanel = new ConnectionWindow(new DockContent(), "General");
            _secondPanel = new ConnectionWindow(new DockContent(), "Servers");
            _updateWindow = new DockContent { TabText = "Update" };
        }

        [TearDown]
        public void Teardown()
        {
            _generalPanel.Dispose();
            _secondPanel.Dispose();
            _updateWindow.Dispose();
        }

        [Test]
        public void ResolvePanelDocumentStyle_AlwaysShowPanelTabsEnabled_ShowsPanelTabs()
        {
            DocumentStyle style = FrmMain.ResolvePanelDocumentStyle(true, [_generalPanel], null);

            Assert.That(style, Is.EqualTo(DocumentStyle.DockingWindow));
        }

        [Test]
        public void ResolvePanelDocumentStyle_OnlyConnectionPanelsOpen_HidesPanelTabs()
        {
            DocumentStyle style = FrmMain.ResolvePanelDocumentStyle(false, [_generalPanel, _secondPanel], null);

            Assert.That(style, Is.EqualTo(DocumentStyle.DockingSdi));
        }

        [Test]
        public void ResolvePanelDocumentStyle_NoDocumentsOpen_HidesPanelTabs()
        {
            DocumentStyle style = FrmMain.ResolvePanelDocumentStyle(false, [], null);

            Assert.That(style, Is.EqualTo(DocumentStyle.DockingSdi));
        }

        [Test]
        public void ResolvePanelDocumentStyle_NonConnectionDocumentOpen_ShowsPanelTabs()
        {
            DocumentStyle style = FrmMain.ResolvePanelDocumentStyle(false, [_generalPanel, _updateWindow], null);

            Assert.That(style, Is.EqualTo(DocumentStyle.DockingWindow));
        }

        [Test]
        public void ResolvePanelDocumentStyle_LastNonConnectionDocumentClosing_HidesPanelTabs()
        {
            DocumentStyle style = FrmMain.ResolvePanelDocumentStyle(false, [_generalPanel, _updateWindow], _updateWindow);

            Assert.That(style, Is.EqualTo(DocumentStyle.DockingSdi));
        }

        [Test]
        public void ResolvePanelDocumentStyle_ConnectionPanelClosingWhileAnotherDocumentStays_ShowsPanelTabs()
        {
            DocumentStyle style = FrmMain.ResolvePanelDocumentStyle(false, [_generalPanel, _updateWindow], _generalPanel);

            Assert.That(style, Is.EqualTo(DocumentStyle.DockingWindow));
        }
    }
}
