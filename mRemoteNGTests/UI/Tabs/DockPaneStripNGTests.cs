using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Themes;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;
using WeifenLuo.WinFormsUI.ThemeVS2015;

namespace mRemoteNGTests.UI.Tabs
{
    [TestFixture]
    public class DockPaneStripNGTests
    {
        private static void RunWithMessagePump(Action testAction)
        {
            Exception caught = null;
            var thread = new Thread(() =>
            {
                try
                {
                    testAction();
                }
                catch (Exception ex)
                {
                    caught = ex;
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            if (!thread.Join(TimeSpan.FromSeconds(30)))
            {
                thread.Interrupt();
                Assert.Fail("Test timed out after 30 seconds");
            }

            if (caught != null)
                throw caught;
        }

        [Test]
        public void CancellingATabClose_LeavesTheActiveTabUnchanged() => RunWithMessagePump(() =>
        {
            // Arrange
            using var hostForm = new Form
            {
                Width = 800,
                Height = 600,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.Manual,
                Location = new System.Drawing.Point(-10000, -10000)
            };

            var dockPanel = new DockPanel
            {
                Dock = DockStyle.Fill,
                DocumentStyle = DocumentStyle.DockingWindow,
                Theme = new VS2015LightTheme()
            };

            dockPanel.Theme.Extender.DockPaneStripFactory = new MremoteDockPaneStripFactory();

            hostForm.Controls.Add(dockPanel);
            hostForm.Show();

            var doc1 = new DockContent { Text = "Doc1", CloseButton = true, CloseButtonVisible = true };
            var doc2 = new DockContent { Text = "Doc2", CloseButton = true, CloseButtonVisible = true };
            var doc3 = new DockContent { Text = "Doc3", CloseButton = true, CloseButtonVisible = true };

            doc1.Show(dockPanel, DockState.Document);
            doc2.Show(dockPanel, DockState.Document);
            doc3.Show(dockPanel, DockState.Document);

            Application.DoEvents();

            // Stands in for the user dismissing the close confirmation. No dialog is shown,
            // so the test stays headless.
            doc2.FormClosing += (_, e) => e.Cancel = true;

            doc2.DockHandler.Activate();
            Application.DoEvents();
            Assert.That(doc2.DockHandler.Pane.ActiveContent, Is.SameAs(doc2), "Doc2 should start out active");

            Control dockPaneStrip = FindDockPaneStripNG(dockPanel);
            Assert.That(dockPaneStrip, Is.Not.Null, "Could not find DockPaneStripNG control");

            MethodInfo closeTabMethod = dockPaneStrip.GetType().GetMethod("CloseTab", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(closeTabMethod, Is.Not.Null, "Could not find CloseTab method");

            // Act - close Doc2, which is not the first tab, so DockPanelSuite would otherwise
            // select Doc1 once the close attempt returns.
            closeTabMethod.Invoke(dockPaneStrip, new object[] { 1 });
            Application.DoEvents();

            // Assert
            Assert.That(doc2.DockState, Is.EqualTo(DockState.Document), "Doc2 should still be open");
            Assert.That(doc2.DockHandler.Pane.ActiveContent, Is.SameAs(doc2), "Doc2 should still be the active tab");
        });

        [Test]
        public void ClosingATabThatIsNotCancelled_StillClosesIt() => RunWithMessagePump(() =>
        {
            using var hostForm = new Form
            {
                Width = 800,
                Height = 600,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.Manual,
                Location = new System.Drawing.Point(-10000, -10000)
            };

            var dockPanel = new DockPanel
            {
                Dock = DockStyle.Fill,
                DocumentStyle = DocumentStyle.DockingWindow,
                Theme = new VS2015LightTheme()
            };

            dockPanel.Theme.Extender.DockPaneStripFactory = new MremoteDockPaneStripFactory();

            hostForm.Controls.Add(dockPanel);
            hostForm.Show();

            var doc1 = new DockContent { Text = "Doc1", CloseButton = true, CloseButtonVisible = true };
            var doc2 = new DockContent { Text = "Doc2", CloseButton = true, CloseButtonVisible = true };

            doc1.Show(dockPanel, DockState.Document);
            doc2.Show(dockPanel, DockState.Document);

            Application.DoEvents();

            Control dockPaneStrip = FindDockPaneStripNG(dockPanel);
            Assert.That(dockPaneStrip, Is.Not.Null, "Could not find DockPaneStripNG control");

            MethodInfo closeTabMethod = dockPaneStrip.GetType().GetMethod("CloseTab", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(closeTabMethod, Is.Not.Null, "Could not find CloseTab method");

            closeTabMethod.Invoke(dockPaneStrip, new object[] { 1 });
            Application.DoEvents();

            Assert.That(doc2.IsDisposed || doc2.DockState != DockState.Document, Is.True, "Doc2 should be closed");
            Assert.That(doc1.DockState, Is.EqualTo(DockState.Document), "Doc1 should still be open");
        });

        private static Control FindDockPaneStripNG(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (string.Equals(c.GetType().Name, "DockPaneStripNG", StringComparison.Ordinal))
                    return c;

                var result = FindDockPaneStripNG(c);
                if (result != null) return result;
            }

            return null;
        }
    }
}
