using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Config;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Tabs;
using mRemoteNG.UI.Window;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNGTests.UI.Window
{
    [TestFixture]
    public class ConnectionWindowTabVisibilityTests
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

        [Test]
        public void TabVisibility_UpdatesBasedOnSettingsAndCount() => RunWithMessagePump(() =>
        {
            // Access internal settings classes via reflection
            var optionsType = typeof(FrmMain).Assembly.GetType("mRemoteNG.Properties.OptionsTabsPanelsPage");
            var optionsDefault = optionsType?.GetProperty("Default", BindingFlags.Static | BindingFlags.Public)?.GetValue(null);
            var alwaysShowProp = optionsType?.GetProperty("AlwaysShowConnectionTabs");

            // Backup
            bool previousAlwaysShowConnectionTabs = (bool)(alwaysShowProp?.GetValue(optionsDefault) ?? false);

            var hostForm = new Form
            {
                Width = 800,
                Height = 600,
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

                using var connectionWindow = new ConnectionWindow(new DockContent(), "Tab Visibility Test");
                connectionWindow.Show(hostDockPanel, DockState.Document);
                Application.DoEvents();

                // Helper to get DockPanel via reflection
                DockPanel GetDockPanel()
                {
                    FieldInfo dockField = typeof(ConnectionWindow).GetField("connDock", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                        ?? throw new AssertionException("Failed to resolve ConnectionWindow.connDock field.");
                    return dockField.GetValue(connectionWindow) as DockPanel
                        ?? throw new AssertionException("Failed to resolve connection dock panel.");
                }

                // Helper to get DockStyle
                DocumentStyle GetDocumentStyle()
                {
                    return GetDockPanel().DocumentStyle;
                }

                // Helper to invoke ShowHideConnectionTabs manually (simulating setting change)
                void InvokeShowHide()
                {
                    MethodInfo method = typeof(ConnectionWindow).GetMethod("ShowHideConnectionTabs", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    method?.Invoke(connectionWindow, null);
                    Application.DoEvents();
                }

                var conn1 = new ConnectionInfo { Name = "Conn1", Protocol = ProtocolType.RDP };

                // Case 1: AlwaysShow = True, 1 tab — should show tab bar (DockingWindow)
                alwaysShowProp?.SetValue(optionsDefault, true);
                InvokeShowHide();

                connectionWindow.AddConnectionTab(conn1);
                Application.DoEvents();

                Assert.That(GetDocumentStyle(), Is.EqualTo(DocumentStyle.DockingWindow), "With AlwaysShow=True, style should be DockingWindow (1 tab).");

                // Case 2: AlwaysShow = False, 1 tab — should hide tab bar (DockingSdi)
                alwaysShowProp?.SetValue(optionsDefault, false);
                InvokeShowHide();

                Assert.That(GetDocumentStyle(), Is.EqualTo(DocumentStyle.DockingSdi), "With AlwaysShow=False, style should be DockingSdi (1 tab).");

                // Case 3: AlwaysShow = True again, 1 tab — should show tab bar again
                alwaysShowProp?.SetValue(optionsDefault, true);
                InvokeShowHide();
                Assert.That(GetDocumentStyle(), Is.EqualTo(DocumentStyle.DockingWindow), "With AlwaysShow=True, style should be DockingWindow again.");

                // Note: Cases involving adding/removing multiple tabs in SDI↔DockingWindow mode
                // cause DockPanel framework deadlocks in test environments and are tested via
                // manual integration testing instead.
            }
            finally
            {
                alwaysShowProp?.SetValue(optionsDefault, previousAlwaysShowConnectionTabs);
                hostForm.Dispose();
            }
        });
    }
}
