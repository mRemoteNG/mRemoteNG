using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.UI.Controls;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class RdpConnectingPanelTests
    {
        private Form _window;
        private Panel _host;
        private Panel _rdpSurface;
        private RdpConnectingPanel _cover;

        private sealed class TestWindow : Form
        {
            protected override bool ShowWithoutActivation { get { return true; } }
        }

        [SetUp]
        public void SetUp()
        {
            _window = new TestWindow
            {
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.Manual,
                Location = new Point(-30000, -30000),
                ClientSize = new Size(640, 480),
                Opacity = 0
            };
            _host = new Panel { Dock = DockStyle.Fill };
            _window.Controls.Add(_host);
            _rdpSurface = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            _host.Controls.Add(_rdpSurface);
            _cover = new RdpConnectingPanel(_host, "Connecting test");
            _window.Show();
        }

        [TearDown]
        public void TearDown()
        {
            _window.Dispose();
        }

        [Test]
        public void Show_CoversTheSurfaceWithoutHidingOrDisablingIt()
        {
            _cover.ShowConnecting();

            Assert.That(_cover.Visible, Is.True);
            Assert.That(_host.Controls.GetChildIndex(_cover), Is.Zero);
            Assert.That(_rdpSurface.Visible, Is.True);
            Assert.That(_rdpSurface.Enabled, Is.True);
            Assert.That(_cover.TabStop, Is.False);
        }

        [Test]
        public void Dismiss_RevealsTheSurfaceWithoutDisposingIt()
        {
            _cover.ShowConnecting();
            _cover.Dismiss();

            Assert.That(_cover.Visible, Is.False);
            Assert.That(_rdpSurface.Visible, Is.True);
            Assert.That(_rdpSurface.IsDisposed, Is.False);
        }

        [Test]
        public void ShowAfterDismiss_ReusesThePanelForAnotherConnectionAttempt()
        {
            _cover.ShowConnecting();
            _cover.Dismiss();
            _cover.ShowConnecting();

            Assert.That(_cover.Visible, Is.True);
            Assert.That(_host.Controls.Count, Is.EqualTo(2));
        }

        [Test]
        public void HostResize_KeepsTheConnectingPanelFillingTheHost()
        {
            _cover.ShowConnecting();
            _window.ClientSize = new Size(900, 600);
            _host.PerformLayout();

            Assert.That(_cover.Bounds, Is.EqualTo(_host.DisplayRectangle));
        }

        [Test]
        public void ApplyColors_UpdatesThePanelAndLocalizedMessageWithoutShowingIt()
        {
            Color background = Color.FromArgb(30, 30, 46);
            Color foreground = Color.FromArgb(205, 214, 244);
            _cover.ApplyColors(background, foreground);
            Label message = (Label)_cover.Controls[0];

            Assert.That(_cover.Visible, Is.False);
            Assert.That(_cover.BackColor, Is.EqualTo(background));
            Assert.That(message.BackColor, Is.EqualTo(background));
            Assert.That(message.ForeColor, Is.EqualTo(foreground));
            Assert.That(message.Text, Is.EqualTo("Connecting test"));
        }

        [Test]
        public void ApplyColors_AlsoAcceptsLightThemeColorsWhileVisible()
        {
            _cover.ShowConnecting();
            _cover.ApplyColors(SystemColors.Control, SystemColors.ControlText);

            Assert.That(_cover.Visible, Is.True);
            Assert.That(_cover.BackColor, Is.EqualTo(SystemColors.Control));
            Assert.That(_cover.ForeColor, Is.EqualTo(SystemColors.ControlText));
        }

        [Test]
        public void WorkerThreadDismiss_IsMarshaledToTheUiThread()
        {
            _cover.ShowConnecting();
            Exception failure = null;
            Thread worker = new Thread(() =>
            {
                try { _cover.Dismiss(); }
                catch (Exception ex) { failure = ex; }
            });
            worker.Start();
            Assert.That(worker.Join(2000), Is.True);
            Application.DoEvents();

            Assert.That(failure, Is.Null);
            Assert.That(_cover.Visible, Is.False);
        }

        [Test]
        public void QueuedDismissAfterHostDisposal_DoesNotAccessDisposedControls()
        {
            _cover.ShowConnecting();
            Thread worker = new Thread(() => _cover.Dismiss());
            worker.Start();
            Assert.That(worker.Join(2000), Is.True);
            _host.Dispose();

            Assert.DoesNotThrow(() => Application.DoEvents());
            Assert.DoesNotThrow(() => _cover.ShowConnecting());
            Assert.DoesNotThrow(() => _cover.Dismiss());
        }

        [Test]
        public void DisposeOnUiThread_RemovesOnlyTheCover()
        {
            _cover.ShowConnecting();
            _cover.DisposeOnUiThread();

            Assert.That(_cover.IsDisposed, Is.True);
            Assert.That(_rdpSurface.IsDisposed, Is.False);
            Assert.That(_host.Controls.Count, Is.EqualTo(1));
        }
    }
}
