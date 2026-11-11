using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Themes;
using mRemoteNG.UI.Tabs;
using mRemoteNG.UI.Window;
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
                // An exception thrown inside a window procedure must fail the test. Without a
                // handler WinForms shows a ThreadExceptionDialog that nobody answers; when the
                // timeout below unwinds it, WinForms exits the application, closes every open
                // form, and a second exception from there crashes the test host. The handler is
                // per thread, so it does not affect other fixtures. ThrowException mode is no
                // alternative: on .NET the exception cannot cross the native window procedure
                // frame and the runtime fails fast.
                Application.ThreadException += (_, args) =>
                {
                    caught ??= args.Exception;
                    Application.ExitThread();
                };

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
                        // The first exception is the one to report; a window procedure may already
                        // have recorded one through the ThreadException handler above.
                        caught ??= ex;
                    }
                    finally
                    {
                        Application.ExitThread();
                    }
                }));

                try
                {
                    Application.Run(new ApplicationContext());
                }
                catch (Exception ex)
                {
                    // Thrown by the message loop itself rather than by testAction; without this it
                    // would take the thread down unreported and leave the test to time out.
                    caught ??= ex;
                }

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
                ExceptionDispatchInfo.Capture(caught).Throw(); // Keeps the stack trace of the pump thread.
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
        public void ClosingAConnectionTabWhileMinimized_DoesNotThrow() => RunWithMessagePump(() =>
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

            var connectionTab = new ConnectionTab
            {
                Text = "Doc1",
                TabText = "Doc1",
                DockAreas = DockAreas.Document | DockAreas.Float,
                silentClose = true,
                protocolClose = true
            };

            connectionTab.Show(dockPanel, DockState.Document);
            Application.DoEvents();

            connectionTab.MinimizeToBottomAutoHide();
            Application.DoEvents();

            Assert.That(connectionTab.DockState, Is.EqualTo(DockState.DockBottomAutoHide), "ConnectionTab should be auto-hidden at the bottom before closing");

            Assert.DoesNotThrow(() =>
            {
                connectionTab.Close();
                Application.DoEvents();
            }, "Closing a connection tab while minimized to bottom auto-hide must not throw");
        });

        [Test]
        public void ClosingAConnectionTabBeforeItsProtocolAttaches_DoesNotThrow() => RunWithMessagePump(() =>
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

            using var panelHost = new DockContent();
            using var connectionWindow = new ConnectionWindow(panelHost);
            connectionWindow.Show(dockPanel, DockState.Document);

            // AddConnectionTab stores the ConnectionInfo in Tag; the protocol replaces it with its
            // InterfaceControl only once it initializes. The tab can be closed before that happens,
            // e.g. when the protocol could not be created or the user closes it while it connects.
            ConnectionTab connectionTab = connectionWindow.AddConnectionTab(new ConnectionInfo { Name = "Not yet connected" });
            connectionTab.silentClose = true;
            Application.DoEvents();

            Assert.DoesNotThrow(() =>
            {
                connectionTab.Close();
                Application.DoEvents();
            }, "Closing a connection tab before its protocol attached must not throw");
        });

        [Test]
        public void RestoringDockAreasWhileStillMinimized_DoesNotThrow() => RunWithMessagePump(() =>
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

            Assert.That(connectionTab.DockState, Is.EqualTo(DockState.DockBottomAutoHide), "ConnectionTab should be auto-hidden at the bottom");

            // Reproduces the crash scenario from the stack trace: forcing a restore of the
            // original DockAreas (which lack DockBottom) while still auto-hidden at the bottom
            // must not surface the "DockAreas conflicts with current DockState" exception.
            MethodInfo restoreMethod = typeof(ConnectionTab).GetMethod(
                "RestoreDockAreasAfterMinimize",
                BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(restoreMethod, Is.Not.Null, "Could not find RestoreDockAreasAfterMinimize method");

            Assert.DoesNotThrow(() =>
            {
                restoreMethod.Invoke(connectionTab, null);
                Application.DoEvents();
            }, "Restoring DockAreas while still minimized must not throw");

            // Closing without it asks for confirmation, and a dialog nobody answers holds the test.
            connectionTab.protocolClose = true;
            connectionTab.Close();
            Application.DoEvents();
        });

        [Test]
        public void ReplacingAConnectionTab_CanKeepItsOriginalDocumentIndex() => RunWithMessagePump(() =>
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

            using var panelHost = new DockContent();
            using var connectionWindow = new ConnectionWindow(panelHost);
            connectionWindow.Show(dockPanel, DockState.Document);

            ConnectionTab doc1 = connectionWindow.AddConnectionTab(new ConnectionInfo { Name = "Doc1" });
            ConnectionTab doc2 = connectionWindow.AddConnectionTab(new ConnectionInfo { Name = "Doc2" });
            ConnectionTab doc3 = connectionWindow.AddConnectionTab(new ConnectionInfo { Name = "Doc3" });

            Application.DoEvents();

            DockPane targetPane = doc2.Pane;
            int targetIndex = targetPane.DisplayingContents.IndexOf(doc2);

            doc2.protocolClose = true;
            doc2.Close();
            Application.DoEvents();

            ConnectionTab replacement = connectionWindow.AddConnectionTab(new ConnectionInfo { Name = "Doc2" }, targetPane, targetIndex);
            Application.DoEvents();

            // The order of the tabs in the pane; DocumentsToArray lists documents in the order they were added.
            string[] tabOrder = targetPane.Contents
                .OfType<ConnectionTab>()
                .Select(tab => tab.TabText)
                .ToArray();

            Assert.That(replacement.Pane, Is.SameAs(targetPane), "Replacement tab should remain in the original pane");
            Assert.That(tabOrder, Is.EqualTo(new[] { "Doc1", "Doc2", "Doc3" }), "Replacement tab should return to the original tab order");
        });

        [Test]
        public void ReplacingTheOnlyTabInAFloatingPane_CanKeepItsOriginalPane() => RunWithMessagePump(() =>
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

            using var panelHost = new DockContent();
            using var connectionWindow = new ConnectionWindow(panelHost);
            connectionWindow.Show(dockPanel, DockState.Document);

            ConnectionTab doc1 = connectionWindow.AddConnectionTab(new ConnectionInfo { Name = "Doc1" });
            ConnectionTab doc2 = connectionWindow.AddConnectionTab(new ConnectionInfo { Name = "Doc2" });

            doc2.Show(connectionWindow.connDock, DockState.Float);
            Application.DoEvents();

            DockPane targetPane = doc2.Pane;
            Assert.That(doc2.DockState, Is.EqualTo(DockState.Float), "Doc2 should be in a floating pane");
            Assert.That(targetPane.DisplayingContents.Count, Is.EqualTo(1), "Doc2 should be the only tab in its pane");

            var reconnectPlacement = connectionWindow.PrepareReconnectPlacement(doc2);
            Application.DoEvents();

            Assert.That(reconnectPlacement.targetPanePlaceholder, Is.Not.Null, "Reconnect placement should keep the floating pane alive");
            DockContent placeholder = reconnectPlacement.targetPanePlaceholder!;
            Assert.That(placeholder.Pane, Is.SameAs(targetPane), "Placeholder should be added to the original pane");

            doc2.protocolClose = true;
            doc2.Close();
            Application.DoEvents();

            Assert.That(targetPane.DockPanel, Is.SameAs(connectionWindow.connDock), "Floating pane should remain attached while the replacement is opened");

            ConnectionTab replacement = connectionWindow.AddConnectionTab(new ConnectionInfo { Name = "Doc2" }, reconnectPlacement.targetPane, reconnectPlacement.targetContentIndex);
            // Disposing the connection window closes its floating window, which closes the tabs it
            // holds. Closing without it asks for confirmation, and a dialog nobody answers holds the test.
            replacement.protocolClose = true;
            placeholder.Close();
            Application.DoEvents();

            Assert.That(replacement.Pane, Is.SameAs(targetPane), "Replacement tab should remain in the original floating pane");
            Assert.That(replacement.DockState, Is.EqualTo(DockState.Float), "Replacement tab should stay floated");
            Assert.That(doc1.DockState, Is.EqualTo(DockState.Document), "Other tabs should remain unchanged");
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

        [Test]
        public void DocumentTabText_GetsAtLeastTheWidthItWasMeasuredAt() => RunWithMessagePump(() =>
        {
            (Form hostForm, DockPanel dockPanel) = ShowHostedDockPanel();
            using (hostForm)
            {
                // A plain document carries only the close button; connection tabs carry the
                // close and the minimize button, which is where the sizing used to fall short.
                var logsDocument = new DockContent { Text = "Logs", TabText = "Logs", CloseButton = true, CloseButtonVisible = true };
                var shortName = CreateConnectionTab("SRV");
                var mediumName = CreateConnectionTab("Build agent 7");

                logsDocument.Show(dockPanel, DockState.Document);
                shortName.Show(dockPanel, DockState.Document);
                mediumName.Show(dockPanel, DockState.Document);
                Application.DoEvents();

                DockPaneStripNG dockPaneStrip = FindDockPaneStripNG(dockPanel);
                Assert.That(dockPaneStrip, Is.Not.Null, "Could not find DockPaneStripNG control");
                dockPaneStrip.Refresh();
                Application.DoEvents();

                Assert.Multiple(() =>
                {
                    AssertTextFitsItsRectangle(dockPaneStrip, 0, logsDocument);
                    AssertTextFitsItsRectangle(dockPaneStrip, 1, shortName);
                    AssertTextFitsItsRectangle(dockPaneStrip, 2, mediumName);
                });
            }
        });

        [Test]
        public void DocumentTabWidth_GrowsWithTheLengthOfTheName() => RunWithMessagePump(() =>
        {
            (Form hostForm, DockPanel dockPanel) = ShowHostedDockPanel();
            using (hostForm)
            {
                var shortName = CreateConnectionTab("SRV");
                var mediumName = CreateConnectionTab("Build agent 7");
                var longName = CreateConnectionTab("Production database cluster in the Frankfurt data centre");

                shortName.Show(dockPanel, DockState.Document);
                mediumName.Show(dockPanel, DockState.Document);
                longName.Show(dockPanel, DockState.Document);
                Application.DoEvents();

                DockPaneStripNG dockPaneStrip = FindDockPaneStripNG(dockPanel);
                Assert.That(dockPaneStrip, Is.Not.Null, "Could not find DockPaneStripNG control");
                dockPaneStrip.Refresh();
                Application.DoEvents();

                int shortWidth = GetTabWidth(dockPaneStrip, 0);
                int mediumWidth = GetTabWidth(dockPaneStrip, 1);
                int longWidth = GetTabWidth(dockPaneStrip, 2);

                Assert.Multiple(() =>
                {
                    Assert.That(mediumWidth, Is.GreaterThan(shortWidth), "A longer name should get a wider tab");
                    Assert.That(longWidth, Is.GreaterThan(mediumWidth), "A longer name should get a wider tab");
                    Assert.That(longWidth, Is.EqualTo(dockPaneStrip.LogicalToDeviceUnits(200)),
                                "A very long name should be capped at the DPI-scaled maximum tab width");
                });
            }
        });

        private static (Form hostForm, DockPanel dockPanel) ShowHostedDockPanel()
        {
            var hostForm = new Form
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

            return (hostForm, dockPanel);
        }

        private static ConnectionTab CreateConnectionTab(string name)
        {
            return new ConnectionTab
            {
                Text = name,
                TabText = name,
                CloseButton = true,
                CloseButtonVisible = true,
                DockAreas = DockAreas.Document | DockAreas.Float
            };
        }

        private static void AssertTextFitsItsRectangle(DockPaneStripNG dockPaneStrip, int index, IDockContent content)
        {
            Rectangle textRectangle = dockPaneStrip.GetDocumentTextRectangle(index);
            int measuredWidth = dockPaneStrip.MeasureDocumentTabText(content).Width;

            Assert.That(textRectangle.Width, Is.GreaterThanOrEqualTo(measuredWidth),
                        $"The text rectangle of \"{content.DockHandler.TabText}\" is narrower than the text it was sized for");
        }

        private static int GetTabWidth(DockPaneStripNG dockPaneStrip, int index)
        {
            object tab = GetTabAtIndex(dockPaneStrip, index);
            PropertyInfo tabWidthProperty = tab.GetType().GetProperty("TabWidth");
            Assert.That(tabWidthProperty, Is.Not.Null, "Could not find TabWidth property");

            return (int)tabWidthProperty.GetValue(tab);
        }

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
            PropertyInfo tabsProperty = typeof(DockPaneStripBase).GetProperty("Tabs", BindingFlags.Instance | BindingFlags.NonPublic);
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
