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
        private static readonly TimeSpan TestTimeout = TimeSpan.FromSeconds(30);
        private static readonly TimeSpan ShutdownTimeout = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Runs <paramref name="testAction"/> on an STA thread with a real message loop, so
        /// docking layout work that relies on posted messages completes.
        /// </summary>
        private static void RunWithMessagePump(Action testAction)
        {
            Exception caught = null;
            Form pump = null;
            using var pumpReady = new ManualResetEventSlim(false);

            var thread = new Thread(() =>
            {
                pump = new Form
                {
                    ShowInTaskbar = false,
                    StartPosition = FormStartPosition.Manual,
                    Location = new System.Drawing.Point(-10000, -10000)
                };

                _ = pump.Handle; // Force handle creation so the loop below can be posted to.
                pumpReady.Set();

                pump.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        testAction();
                    }
                    catch (Exception ex)
                    {
                        caught = ex;
                    }
                    finally
                    {
                        Application.ExitThread();
                    }
                }));

                Application.Run(new ApplicationContext());
                pump.Dispose();
            })
            {
                // Last-resort safety net: a wedged UI thread must never hold the test run open.
                IsBackground = true
            };

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            pumpReady.Wait(ShutdownTimeout);

            if (!thread.Join(TestTimeout))
            {
                // Ask the loop to unwind. This only lands if the thread is still pumping;
                // when it is not, IsBackground keeps the timeout from hanging the run.
                try
                {
                    pump?.BeginInvoke(new Action(Application.ExitThread));
                }
                catch (InvalidOperationException)
                {
                    // Handle was never created, or the form was disposed as the loop
                    // unwound (ObjectDisposedException derives from this).
                }

                thread.Join(ShutdownTimeout);
                Assert.Fail($"Test timed out after {TestTimeout.TotalSeconds} seconds");
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
