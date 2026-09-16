using System;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Themes;
using mRemoteNG.UI.Tabs;
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

            DockPaneStripNG dockPaneStrip = FindDockPaneStripNG(dockPanel);
            Assert.That(dockPaneStrip, Is.Not.Null, "Could not find DockPaneStripNG control");

            // Act - close Doc2, which is not the first tab, so DockPanelSuite would otherwise
            // select Doc1 once the close attempt returns.
            var closeTabMethod = typeof(DockPaneStripNG).GetMethod("CloseTab", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            Assert.That(closeTabMethod, Is.Not.Null, "Could not find CloseTab method");
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

            DockPaneStripNG dockPaneStrip = FindDockPaneStripNG(dockPanel);
            Assert.That(dockPaneStrip, Is.Not.Null, "Could not find DockPaneStripNG control");

            var closeTabMethod = typeof(DockPaneStripNG).GetMethod("CloseTab", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            Assert.That(closeTabMethod, Is.Not.Null, "Could not find CloseTab method");
            closeTabMethod.Invoke(dockPaneStrip, new object[] { 1 });
            Application.DoEvents();

            Assert.That(doc2.IsDisposed || doc2.DockState != DockState.Document, Is.True, "Doc2 should be closed");
            Assert.That(doc1.DockState, Is.EqualTo(DockState.Document), "Doc1 should still be open");
        });

        [Test]
        public void MinimizingAConnectionTab_MovesItToBottomAutoHide() => RunWithMessagePump(() =>
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
            var doc2 = new ConnectionTab
            {
                Text = "Doc2",
                TabText = "Doc2",
                CloseButton = true,
                CloseButtonVisible = true,
                DockAreas = DockAreas.Document | DockAreas.Float
            };

            doc1.Show(dockPanel, DockState.Document);
            doc2.Show(dockPanel, DockState.Document);

            Application.DoEvents();

            DockPaneStripNG dockPaneStrip = FindDockPaneStripNG(dockPanel);
            Assert.That(dockPaneStrip, Is.Not.Null, "Could not find DockPaneStripNG control");
            dockPaneStrip.MinimizeConnectionTab(1);
            Application.DoEvents();

            Assert.That(doc2.DockState, Is.EqualTo(DockState.DockBottomAutoHide), "Connection tab should move to bottom auto-hide");
            Assert.That(doc1.DockState, Is.EqualTo(DockState.Document), "Other document tabs should remain unchanged");
        });

        [Test]
        public void ConnectionTabMinimizeToBottomAutoHide_ChangesDockState() => RunWithMessagePump(() =>
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

            var connectionTab = new ConnectionTab
            {
                Text = "Doc1",
                TabText = "Doc1",
                DockAreas = DockAreas.Document | DockAreas.Float
            };

            connectionTab.Show(dockPanel, DockState.Document);
            Application.DoEvents();

            connectionTab.MinimizeToBottomAutoHide();
            Application.DoEvents();

            Assert.That(connectionTab.DockState, Is.EqualTo(DockState.DockBottomAutoHide), "ConnectionTab should move to bottom auto-hide");
            Assert.That((connectionTab.DockAreas & DockAreas.DockBottom), Is.EqualTo(DockAreas.DockBottom), "ConnectionTab should allow bottom docking while minimized");

            connectionTab.Show(dockPanel, DockState.Document);
            Application.DoEvents();

            Assert.That(connectionTab.DockAreas, Is.EqualTo(DockAreas.Document | DockAreas.Float), "ConnectionTab should restore its original docking areas after leaving auto-hide");
        });

        [Test]
        public void MouseMoveInRightToLeft_UsesVisualActionButtonRectangles() => RunWithMessagePump(() =>
        {
            using var hostForm = new Form
            {
                Width = 800,
                Height = 600,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.Manual,
                Location = new Point(-10000, -10000)
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
            var doc2 = new ConnectionTab
            {
                Text = "Doc2",
                TabText = "Doc2",
                CloseButton = true,
                CloseButtonVisible = true,
                DockAreas = DockAreas.Document | DockAreas.Float
            };

            doc1.Show(dockPanel, DockState.Document);
            doc2.Show(dockPanel, DockState.Document);
            doc2.DockHandler.Activate();
            Application.DoEvents();

            DockPaneStripNG dockPaneStrip = FindDockPaneStripNG(dockPanel);
            Assert.That(dockPaneStrip, Is.Not.Null, "Could not find DockPaneStripNG control");

            dockPaneStrip.RightToLeft = RightToLeft.Yes;
            Application.DoEvents();

            object tab = GetTabAtIndex(dockPaneStrip, 1);
            Rectangle? logicalTabRect = (Rectangle?)tab.GetType().GetProperty("Rectangle")?.GetValue(tab);
            Assert.That(logicalTabRect, Is.Not.Null, "Could not get tab rectangle");

            var getCloseButtonRectMethod = typeof(DockPaneStripNG).GetMethod("GetCloseButtonRect", BindingFlags.Instance | BindingFlags.NonPublic);
            var getMinimizeButtonRectMethod = typeof(DockPaneStripNG).GetMethod("GetMinimizeButtonRect", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(getCloseButtonRectMethod, Is.Not.Null, "Could not find GetCloseButtonRect method");
            Assert.That(getMinimizeButtonRectMethod, Is.Not.Null, "Could not find GetMinimizeButtonRect method");

            Rectangle logicalCloseButtonRect = (Rectangle)getCloseButtonRectMethod.Invoke(dockPaneStrip, new object[] { logicalTabRect.Value, doc2 });
            Rectangle logicalMinimizeButtonRect = (Rectangle)getMinimizeButtonRectMethod.Invoke(dockPaneStrip, new object[] { logicalTabRect.Value, doc2 });
            Rectangle visualCloseButtonRect = DrawHelper.RtlTransform(dockPaneStrip, logicalCloseButtonRect);
            Rectangle visualMinimizeButtonRect = DrawHelper.RtlTransform(dockPaneStrip, logicalMinimizeButtonRect);

            Point closeButtonCenter = new(visualCloseButtonRect.Left + visualCloseButtonRect.Width / 2, visualCloseButtonRect.Top + visualCloseButtonRect.Height / 2);
            Cursor.Position = dockPaneStrip.PointToScreen(closeButtonCenter);
            InvokeOnMouseMove(dockPaneStrip, closeButtonCenter);

            Assert.That(GetRectangleProperty(dockPaneStrip, "ActiveClose"), Is.EqualTo(visualCloseButtonRect), "Close hover rectangle should use visual RTL coordinates");

            Point minimizeButtonCenter = new(visualMinimizeButtonRect.Left + visualMinimizeButtonRect.Width / 2, visualMinimizeButtonRect.Top + visualMinimizeButtonRect.Height / 2);
            Cursor.Position = dockPaneStrip.PointToScreen(minimizeButtonCenter);
            InvokeOnMouseMove(dockPaneStrip, minimizeButtonCenter);

            Assert.That(GetRectangleProperty(dockPaneStrip, "ActiveMinimize"), Is.EqualTo(visualMinimizeButtonRect), "Minimize hover rectangle should use visual RTL coordinates");
        });

        private static DockPaneStripNG FindDockPaneStripNG(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is DockPaneStripNG dockPaneStrip)
                    return dockPaneStrip;

                var result = FindDockPaneStripNG(c);
                if (result != null) return result;
            }

            return null;
        }

        private static object GetTabAtIndex(DockPaneStripNG dockPaneStrip, int index)
        {
            PropertyInfo tabsProperty = typeof(DockPaneStrip).GetProperty("Tabs", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(tabsProperty, Is.Not.Null, "Could not find Tabs property");

            object tabs = tabsProperty.GetValue(dockPaneStrip);
            Assert.That(tabs, Is.Not.Null, "Could not read Tabs property");

            PropertyInfo indexer = tabs.GetType().GetProperty("Item");
            Assert.That(indexer, Is.Not.Null, "Could not find Tabs indexer");

            return indexer.GetValue(tabs, new object[] { index });
        }

        private static void InvokeOnMouseMove(Control control, Point point)
        {
            MethodInfo onMouseMoveMethod = control.GetType().GetMethod("OnMouseMove", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(onMouseMoveMethod, Is.Not.Null, "Could not find OnMouseMove method");

            onMouseMoveMethod.Invoke(control, new object[] { new MouseEventArgs(MouseButtons.None, 0, point.X, point.Y, 0) });
            Application.DoEvents();
        }

        private static Rectangle GetRectangleProperty(object target, string propertyName)
        {
            PropertyInfo property = target.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(property, Is.Not.Null, $"Could not find property {propertyName}");

            object value = property.GetValue(target);
            Assert.That(value, Is.Not.Null, $"Property {propertyName} should not be null");

            return (Rectangle)value;
        }
    }
}
