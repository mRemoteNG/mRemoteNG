using System.Threading;
using System.Windows.Forms;
using mRemoteNG.UI.Window;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;
using WeifenLuo.WinFormsUI.ThemeVS2015;

namespace mRemoteNGTests.UI.Window
{
    [Apartment(ApartmentState.STA)]
    public class ConnectionSearchWindowTests
    {
        private Form _host;
        private DockPanel _dockPanel;
        private ConnectionTreeWindow _treeWindow;
        private ConnectionSearchWindow _searchWindow;

        [SetUp]
        public void Setup()
        {
            _host = new Form
            {
                Width = 800,
                Height = 600,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.Manual,
                Location = new System.Drawing.Point(-10000, -10000)
            };
            _dockPanel = new DockPanel
            {
                Dock = DockStyle.Fill,
                DocumentStyle = DocumentStyle.DockingWindow,
                Theme = new VS2015LightTheme()
            };
            _host.Controls.Add(_dockPanel);
            _host.Show();

            _treeWindow = new ConnectionTreeWindow(new DockContent());
            _treeWindow.Show(_dockPanel, DockState.DockLeft);
            _searchWindow = new ConnectionSearchWindow(_treeWindow);
        }

        [TearDown]
        public void Teardown()
        {
            _searchWindow?.Dispose();
            _treeWindow?.Dispose();
            _host?.Dispose();
        }

        [Test]
        public void SearchBarStartsInTheConnectionTreeWindow()
        {
            Assert.That(_treeWindow.SearchBarIsHostedHere);
        }

        [Test]
        public void ShowingTheSearchWindowMovesTheSearchBarIntoIt()
        {
            _searchWindow.ShowIn(_dockPanel);

            Assert.That(_searchWindow.IsShowing);
            Assert.That(_treeWindow.searchBoxLayoutPanel.Parent, Is.SameAs(_searchWindow));
        }

        [Test]
        public void FirstShowDocksTheSearchWindowNextToTheTree()
        {
            _searchWindow.ShowIn(_dockPanel);

            Assert.That(_searchWindow.DockState, Is.EqualTo(DockState.DockLeft));
        }

        [Test]
        public void HidingTheSearchWindowReturnsTheSearchBar()
        {
            _searchWindow.ShowIn(_dockPanel);
            _searchWindow.Hide();

            Assert.That(_searchWindow.IsShowing, Is.False);
            Assert.That(_treeWindow.SearchBarIsHostedHere);
        }

        [Test]
        public void ClosingTheSearchWindowReturnsTheSearchBar()
        {
            _searchWindow.ShowIn(_dockPanel);
            _searchWindow.Close();

            Assert.That(_treeWindow.SearchBarIsHostedHere);
        }

        [Test]
        public void ReturnedSearchBarIsDockedBetweenTheMenuAndTheTree()
        {
            _searchWindow.ShowIn(_dockPanel);
            _searchWindow.Hide();

            Control.ControlCollection controls = _treeWindow.Controls;
            Assert.That(controls.GetChildIndex(_treeWindow.searchBoxLayoutPanel),
                Is.EqualTo(controls.GetChildIndex(_treeWindow.ConnectionTree) + 1));
            Assert.That(controls.GetChildIndex(_treeWindow.msMain),
                Is.GreaterThan(controls.GetChildIndex(_treeWindow.searchBoxLayoutPanel)));
        }

        [Test]
        public void DisposingTheSearchWindowKeepsTheSearchBar()
        {
            _searchWindow.ShowIn(_dockPanel);
            _searchWindow.Dispose();

            Assert.That(_treeWindow.SearchBarIsHostedHere);
            Assert.That(_treeWindow.searchBoxLayoutPanel.IsDisposed, Is.False);
        }

        [Test]
        public void ShowingAgainAfterHidingRestoresThePreviousPlace()
        {
            _searchWindow.ShowIn(_dockPanel);
            _searchWindow.DockState = DockState.DockRight;
            _searchWindow.Hide();

            _searchWindow.ShowIn(_dockPanel);

            Assert.That(_searchWindow.DockState, Is.EqualTo(DockState.DockRight));
            Assert.That(_treeWindow.searchBoxLayoutPanel.Parent, Is.SameAs(_searchWindow));
        }
    }
}
