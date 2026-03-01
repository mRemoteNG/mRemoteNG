using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.UI.Tabs;
using mRemoteNG.UI.Window;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNGTests.UI.Window
{
    [TestFixture]
    public class ConnectionWindowNavigationTests
    {
        private static void RunWithMessagePump(Action testAction)
        {
            Exception? caught = null;
            var thread = new Thread(() =>
            {
                var form = new Form
                {
                    Width = 400,
                    Height = 300,
                    ShowInTaskbar = false,
                    StartPosition = FormStartPosition.Manual,
                    Location = new System.Drawing.Point(-10000, -10000)
                };

                form.Load += (_, _) =>
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
                };

                Application.Run(form);
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            if (!thread.Join(TimeSpan.FromSeconds(30)))
            {
                thread.Interrupt();
                Assert.Fail("Test timed out after 30 seconds (message pump deadlock)");
            }

            if (caught != null)
                throw caught;
        }

        private static DockPanel GetConnDock(ConnectionWindow connectionWindow)
        {
            FieldInfo dockField = typeof(ConnectionWindow).GetField("connDock", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                ?? throw new AssertionException("Failed to resolve ConnectionWindow.connDock field.");
            return dockField.GetValue(connectionWindow) as DockPanel
                ?? throw new AssertionException("Failed to resolve connection dock panel.");
        }

        private static ConnectionTab? AddTab(ConnectionWindow connectionWindow, string name)
        {
            var conn = new ConnectionInfo { Name = name, Protocol = ProtocolType.RDP };
            var tab = connectionWindow.AddConnectionTab(conn);
            if (tab == null)
            {
                Application.DoEvents();
                Application.DoEvents();
                tab = connectionWindow.AddConnectionTab(conn);
            }
            return tab;
        }

        [Test]
        public void NavigateToNextTab_AdvancesToNextTab() => RunWithMessagePump(() =>
        {
            var hostForm = new Form
            {
                Width = 800, Height = 600,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.Manual,
                Location = new System.Drawing.Point(-10000, -10000)
            };
            var hostDockPanel = new DockPanel
            {
                Dock = DockStyle.Fill,
                DocumentStyle = DocumentStyle.DockingWindow,
                Theme = new VS2015LightTheme()
            };
            hostForm.Controls.Add(hostDockPanel);

            try
            {
                hostForm.Show();
                Application.DoEvents();

                using var connectionWindow = new ConnectionWindow(new DockContent(), "Nav Test");
                connectionWindow.Show(hostDockPanel, DockState.Document);
                Application.DoEvents();

                ConnectionTab? tab1 = AddTab(connectionWindow, "Tab1");
                ConnectionTab? tab2 = AddTab(connectionWindow, "Tab2");
                Assert.That(tab1, Is.Not.Null, "Tab1 could not be created.");
                Assert.That(tab2, Is.Not.Null, "Tab2 could not be created.");

                // Activate first tab
                tab1!.DockHandler.Activate();
                Application.DoEvents();

                var connDock = GetConnDock(connectionWindow);
                Assert.That(connDock.ActiveContent, Is.EqualTo(tab1), "Precondition: tab1 should be active.");

                connectionWindow.NavigateToNextTab();
                Application.DoEvents();

                Assert.That(connDock.ActiveContent, Is.EqualTo(tab2), "NavigateToNextTab should activate tab2.");
            }
            finally
            {
                hostForm.Close();
                hostForm.Dispose();
            }
        });

        [Test]
        public void NavigateToNextTab_WrapsAroundToFirstTab() => RunWithMessagePump(() =>
        {
            var hostForm = new Form
            {
                Width = 800, Height = 600,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.Manual,
                Location = new System.Drawing.Point(-10000, -10000)
            };
            var hostDockPanel = new DockPanel
            {
                Dock = DockStyle.Fill,
                DocumentStyle = DocumentStyle.DockingWindow,
                Theme = new VS2015LightTheme()
            };
            hostForm.Controls.Add(hostDockPanel);

            try
            {
                hostForm.Show();
                Application.DoEvents();

                using var connectionWindow = new ConnectionWindow(new DockContent(), "Wrap Test");
                connectionWindow.Show(hostDockPanel, DockState.Document);
                Application.DoEvents();

                ConnectionTab? tab1 = AddTab(connectionWindow, "First");
                ConnectionTab? tab2 = AddTab(connectionWindow, "Last");
                Assert.That(tab1, Is.Not.Null, "Tab1 could not be created.");
                Assert.That(tab2, Is.Not.Null, "Tab2 could not be created.");

                // Activate last tab
                tab2!.DockHandler.Activate();
                Application.DoEvents();

                var connDock = GetConnDock(connectionWindow);
                Assert.That(connDock.ActiveContent, Is.EqualTo(tab2), "Precondition: tab2 should be active.");

                connectionWindow.NavigateToNextTab();
                Application.DoEvents();

                Assert.That(connDock.ActiveContent, Is.EqualTo(tab1), "NavigateToNextTab should wrap around to tab1.");
            }
            finally
            {
                hostForm.Close();
                hostForm.Dispose();
            }
        });

        [Test]
        public void NavigateToPreviousTab_MovesToPreviousTab() => RunWithMessagePump(() =>
        {
            var hostForm = new Form
            {
                Width = 800, Height = 600,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.Manual,
                Location = new System.Drawing.Point(-10000, -10000)
            };
            var hostDockPanel = new DockPanel
            {
                Dock = DockStyle.Fill,
                DocumentStyle = DocumentStyle.DockingWindow,
                Theme = new VS2015LightTheme()
            };
            hostForm.Controls.Add(hostDockPanel);

            try
            {
                hostForm.Show();
                Application.DoEvents();

                using var connectionWindow = new ConnectionWindow(new DockContent(), "Prev Test");
                connectionWindow.Show(hostDockPanel, DockState.Document);
                Application.DoEvents();

                ConnectionTab? tab1 = AddTab(connectionWindow, "Alpha");
                ConnectionTab? tab2 = AddTab(connectionWindow, "Beta");
                Assert.That(tab1, Is.Not.Null, "Tab1 could not be created.");
                Assert.That(tab2, Is.Not.Null, "Tab2 could not be created.");

                // Activate second tab
                tab2!.DockHandler.Activate();
                Application.DoEvents();

                var connDock = GetConnDock(connectionWindow);
                Assert.That(connDock.ActiveContent, Is.EqualTo(tab2), "Precondition: tab2 should be active.");

                connectionWindow.NavigateToPreviousTab();
                Application.DoEvents();

                Assert.That(connDock.ActiveContent, Is.EqualTo(tab1), "NavigateToPreviousTab should activate tab1.");
            }
            finally
            {
                hostForm.Close();
                hostForm.Dispose();
            }
        });

        [Test]
        public void NavigateToPreviousTab_WrapsAroundToLastTab() => RunWithMessagePump(() =>
        {
            var hostForm = new Form
            {
                Width = 800, Height = 600,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.Manual,
                Location = new System.Drawing.Point(-10000, -10000)
            };
            var hostDockPanel = new DockPanel
            {
                Dock = DockStyle.Fill,
                DocumentStyle = DocumentStyle.DockingWindow,
                Theme = new VS2015LightTheme()
            };
            hostForm.Controls.Add(hostDockPanel);

            try
            {
                hostForm.Show();
                Application.DoEvents();

                using var connectionWindow = new ConnectionWindow(new DockContent(), "PrevWrap Test");
                connectionWindow.Show(hostDockPanel, DockState.Document);
                Application.DoEvents();

                ConnectionTab? tab1 = AddTab(connectionWindow, "First");
                ConnectionTab? tab2 = AddTab(connectionWindow, "Last");
                Assert.That(tab1, Is.Not.Null, "Tab1 could not be created.");
                Assert.That(tab2, Is.Not.Null, "Tab2 could not be created.");

                // Activate first tab
                tab1!.DockHandler.Activate();
                Application.DoEvents();

                var connDock = GetConnDock(connectionWindow);
                Assert.That(connDock.ActiveContent, Is.EqualTo(tab1), "Precondition: tab1 should be active.");

                connectionWindow.NavigateToPreviousTab();
                Application.DoEvents();

                Assert.That(connDock.ActiveContent, Is.EqualTo(tab2), "NavigateToPreviousTab should wrap around to last tab.");
            }
            finally
            {
                hostForm.Close();
                hostForm.Dispose();
            }
        });

        [Test]
        public void NavigateToTab_ActivatesTabAtIndex() => RunWithMessagePump(() =>
        {
            var hostForm = new Form
            {
                Width = 800, Height = 600,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.Manual,
                Location = new System.Drawing.Point(-10000, -10000)
            };
            var hostDockPanel = new DockPanel
            {
                Dock = DockStyle.Fill,
                DocumentStyle = DocumentStyle.DockingWindow,
                Theme = new VS2015LightTheme()
            };
            hostForm.Controls.Add(hostDockPanel);

            try
            {
                hostForm.Show();
                Application.DoEvents();

                using var connectionWindow = new ConnectionWindow(new DockContent(), "Jump Test");
                connectionWindow.Show(hostDockPanel, DockState.Document);
                Application.DoEvents();

                ConnectionTab? tab1 = AddTab(connectionWindow, "One");
                ConnectionTab? tab2 = AddTab(connectionWindow, "Two");
                ConnectionTab? tab3 = AddTab(connectionWindow, "Three");
                Assert.That(tab1, Is.Not.Null, "Tab1 could not be created.");
                Assert.That(tab2, Is.Not.Null, "Tab2 could not be created.");
                Assert.That(tab3, Is.Not.Null, "Tab3 could not be created.");

                // Activate first tab
                tab1!.DockHandler.Activate();
                Application.DoEvents();

                var connDock = GetConnDock(connectionWindow);

                connectionWindow.NavigateToTab(1);
                Application.DoEvents();

                Assert.That(connDock.ActiveContent, Is.EqualTo(tab2), "NavigateToTab(1) should activate tab2.");

                connectionWindow.NavigateToTab(2);
                Application.DoEvents();

                Assert.That(connDock.ActiveContent, Is.EqualTo(tab3), "NavigateToTab(2) should activate tab3.");
            }
            finally
            {
                hostForm.Close();
                hostForm.Dispose();
            }
        });

        [Test]
        public void NavigateToNextTab_DoesNothingWithSingleTab() => RunWithMessagePump(() =>
        {
            var hostForm = new Form
            {
                Width = 800, Height = 600,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.Manual,
                Location = new System.Drawing.Point(-10000, -10000)
            };
            var hostDockPanel = new DockPanel
            {
                Dock = DockStyle.Fill,
                DocumentStyle = DocumentStyle.DockingWindow,
                Theme = new VS2015LightTheme()
            };
            hostForm.Controls.Add(hostDockPanel);

            try
            {
                hostForm.Show();
                Application.DoEvents();

                using var connectionWindow = new ConnectionWindow(new DockContent(), "Single Tab Test");
                connectionWindow.Show(hostDockPanel, DockState.Document);
                Application.DoEvents();

                ConnectionTab? tab1 = AddTab(connectionWindow, "OnlyTab");
                Assert.That(tab1, Is.Not.Null, "Tab could not be created.");

                tab1!.DockHandler.Activate();
                Application.DoEvents();

                var connDock = GetConnDock(connectionWindow);
                var activeBeforeNav = connDock.ActiveContent;

                // Should not throw and should not change active content
                Assert.DoesNotThrow(() => connectionWindow.NavigateToNextTab(), "NavigateToNextTab should not throw with a single tab.");
                Application.DoEvents();

                Assert.That(connDock.ActiveContent, Is.EqualTo(activeBeforeNav), "Active tab should remain unchanged with a single tab.");
            }
            finally
            {
                hostForm.Close();
                hostForm.Dispose();
            }
        });

        [Test]
        public void NavigateToTab_OutOfRangeIndexDoesNothing() => RunWithMessagePump(() =>
        {
            var hostForm = new Form
            {
                Width = 800, Height = 600,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.Manual,
                Location = new System.Drawing.Point(-10000, -10000)
            };
            var hostDockPanel = new DockPanel
            {
                Dock = DockStyle.Fill,
                DocumentStyle = DocumentStyle.DockingWindow,
                Theme = new VS2015LightTheme()
            };
            hostForm.Controls.Add(hostDockPanel);

            try
            {
                hostForm.Show();
                Application.DoEvents();

                using var connectionWindow = new ConnectionWindow(new DockContent(), "OOB Test");
                connectionWindow.Show(hostDockPanel, DockState.Document);
                Application.DoEvents();

                ConnectionTab? tab1 = AddTab(connectionWindow, "Only");
                Assert.That(tab1, Is.Not.Null, "Tab could not be created.");

                tab1!.DockHandler.Activate();
                Application.DoEvents();

                var connDock = GetConnDock(connectionWindow);
                var activeBefore = connDock.ActiveContent;

                Assert.DoesNotThrow(() => connectionWindow.NavigateToTab(-1), "NavigateToTab(-1) should not throw.");
                Assert.DoesNotThrow(() => connectionWindow.NavigateToTab(99), "NavigateToTab(99) should not throw.");
                Application.DoEvents();

                Assert.That(connDock.ActiveContent, Is.EqualTo(activeBefore), "Active tab should remain unchanged for out-of-range index.");
            }
            finally
            {
                hostForm.Close();
                hostForm.Dispose();
            }
        });
    }
}
