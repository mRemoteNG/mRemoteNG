using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Themes;
using mRemoteNG.UI.Window;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;
using WeifenLuo.WinFormsUI.ThemeVS2015;

namespace mRemoteNGTests.UI.Window
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class ConnectionWindowTests
    {
        private Form _hostForm;
        private DockPanel _dockPanel;
        private DockContent _panelHost;
        private ConnectionWindow _connectionWindow;

        [SetUp]
        public void Setup()
        {
            _hostForm = new Form
            {
                Width = 800,
                Height = 600,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.Manual,
                Location = new Point(-10000, -10000)
            };

            _dockPanel = new DockPanel
            {
                Dock = DockStyle.Fill,
                DocumentStyle = DocumentStyle.DockingWindow,
                Theme = new VS2015LightTheme()
            };
            _dockPanel.Theme.Extender.DockPaneStripFactory = new MremoteDockPaneStripFactory();

            _hostForm.Controls.Add(_dockPanel);
            _hostForm.Show();

            _panelHost = new DockContent();
            _connectionWindow = new ConnectionWindow(_panelHost);
            _connectionWindow.Show(_dockPanel, DockState.Document);
        }

        [TearDown]
        public void Teardown()
        {
            _connectionWindow.Dispose();
            _panelHost.Dispose();
            _hostForm.Dispose();
        }

        [Test]
        public void OpenConnectionCount_EmptyPanel_IsZero()
        {
            Assert.That(_connectionWindow.OpenConnectionCount, Is.EqualTo(0));
        }

        [Test]
        public void OpenConnectionCount_TwoConnectionTabs_IsTwo()
        {
            _connectionWindow.AddConnectionTab(new ConnectionInfo { Name = "Build server" });
            _connectionWindow.AddConnectionTab(new ConnectionInfo { Name = "Database server" });
            Application.DoEvents();

            Assert.That(_connectionWindow.OpenConnectionCount, Is.EqualTo(2));
        }

        [Test]
        public void OpenConnectionCount_HiddenToolWindowInPanel_IsNotCounted()
        {
            // The Options window is hosted inside the General panel with HideOnClose,
            // so closing it leaves it registered in the panel's dock in the Hidden state.
            using var toolWindow = new DockContent { HideOnClose = true, TabText = "Options" };
            toolWindow.Show(_connectionWindow.connDock, DockState.Document);
            Application.DoEvents();
            toolWindow.DockHandler.Hide();
            Application.DoEvents();

            Assert.That(toolWindow.DockPanel, Is.SameAs(_connectionWindow.connDock), "precondition: hidden content stays registered");
            Assert.That(_connectionWindow.OpenConnectionCount, Is.EqualTo(0));
        }

        [Test]
        public void OpenConnectionCount_VisibleToolWindowInPanel_IsNotCounted()
        {
            using var toolWindow = new DockContent { TabText = "Options" };
            toolWindow.Show(_connectionWindow.connDock, DockState.Document);
            Application.DoEvents();

            Assert.That(_connectionWindow.OpenConnectionCount, Is.EqualTo(0));
        }
    }
}
