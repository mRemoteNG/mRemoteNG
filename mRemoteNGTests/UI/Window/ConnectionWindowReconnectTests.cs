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
using NSubstitute;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNGTests.UI.Window
{
    public class ConnectionWindowReconnectTests
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
                        form.Close();
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
        public void Reconnect_ReusesExistingTabAndPreservesTabIndex() => RunWithMessagePump(() =>
        {
            var previousInitiator = Runtime.ConnectionInitiator;

            // Access internal settings classes via reflection (they are internal sealed)
            var optionsType = typeof(FrmMain).Assembly.GetType("mRemoteNG.Properties.OptionsTabsPanelsPage");
            var optionsDefault = optionsType?.GetProperty("Default", BindingFlags.Static | BindingFlags.Public)?.GetValue(null);
            var alwaysShowProp = optionsType?.GetProperty("AlwaysShowPanelSelectionDlg");
            bool previousAlwaysShowPanelSelectionDlg = (bool)(alwaysShowProp?.GetValue(optionsDefault) ?? false);

            var settingsType = typeof(FrmMain).Assembly.GetType("mRemoteNG.Properties.Settings");
            var settingsDefault = settingsType?.GetProperty("Default", BindingFlags.Static | BindingFlags.Public)?.GetValue(null);
            var confirmCloseProp = settingsType?.GetProperty("ConfirmCloseConnection");
            int previousConfirmCloseConnection = (int)(confirmCloseProp?.GetValue(settingsDefault) ?? 0);

            // Use a lightweight host form with a DockPanel instead of FrmMain.Default,
            // which triggers heavy app initialization and deadlocks the message pump.
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
                alwaysShowProp?.SetValue(optionsDefault, false);
                confirmCloseProp?.SetValue(settingsDefault, (int)ConfirmCloseEnum.Never);

                IProtocolFactory protocolFactory = Substitute.For<IProtocolFactory>();
                protocolFactory.CreateProtocol(Arg.Any<ConnectionInfo>()).Returns(_ => new TestProtocol());

                ITunnelPortValidator tunnelPortValidator = Substitute.For<ITunnelPortValidator>();
                Runtime.ConnectionInitiator = new ConnectionInitiator(protocolFactory, tunnelPortValidator);

                hostForm.Show();
                Application.DoEvents();
                Application.DoEvents();

                using var connectionWindow = new ConnectionWindow(new DockContent(), "Reconnect Test");
                connectionWindow.Show(hostDockPanel, DockState.Document);
                Application.DoEvents();
                Application.DoEvents();
                Application.DoEvents();

                var reconnectTarget = new ConnectionInfo
                {
                    Name = "ReconnectTarget",
                    Hostname = "127.0.0.1",
                    Protocol = ProtocolType.RDP,
                    Panel = "General"
                };

                var secondConnection = new ConnectionInfo
                {
                    Name = "OtherConnection",
                    Hostname = "127.0.0.2",
                    Protocol = ProtocolType.RDP,
                    Panel = "General"
                };

                // Pump events to ensure the ConnectionWindow's internal DockPanel is fully initialized
                Application.DoEvents();

                ConnectionTab? firstTab = connectionWindow.AddConnectionTab(reconnectTarget);
                if (firstTab == null)
                {
                    // Retry after pumping more events - DockPanel may need additional layout passes
                    Application.DoEvents();
                    Application.DoEvents();
                    firstTab = connectionWindow.AddConnectionTab(reconnectTarget);
                }
                Assert.That(firstTab, Is.Not.Null, "Failed to create first tab after retry — DockPanel infrastructure not available in test context.");

                ConnectionTab? secondTab = connectionWindow.AddConnectionTab(secondConnection);
                if (secondTab == null)
                {
                    Application.DoEvents();
                    Application.DoEvents();
                    secondTab = connectionWindow.AddConnectionTab(secondConnection);
                }
                Assert.That(secondTab, Is.Not.Null, "Failed to create second tab after retry.");

                AttachTestInterface(firstTab, reconnectTarget);
                firstTab.DockHandler.Activate();
                Application.DoEvents();

                int indexBeforeReconnect = GetTabIndex(connectionWindow, reconnectTarget);
                Assert.That(indexBeforeReconnect, Is.EqualTo(0), "Precondition failed: reconnect target tab should start at index 0.");

                InvokeReconnect(connectionWindow);

                bool reconnectCompleted = SpinWait.SpinUntil(() =>
                {
                    Application.DoEvents();
                    return FindTab(connectionWindow, reconnectTarget)?.Tag is InterfaceControl;
                }, TimeSpan.FromSeconds(5));

                Assert.That(reconnectCompleted, Is.True, "Reconnect did not complete within timeout.");

                int indexAfterReconnect = GetTabIndex(connectionWindow, reconnectTarget);
                int tabCountAfterReconnect = GetConnectionTabs(connectionWindow).Length;

                Assert.That(tabCountAfterReconnect, Is.EqualTo(2), "Reconnect should not create extra tabs.");
                Assert.That(indexAfterReconnect, Is.EqualTo(indexBeforeReconnect),
                    "Reconnect should reuse the existing tab instead of recreating it at the end.");
            }
            finally
            {
                Runtime.ConnectionInitiator = previousInitiator;
                alwaysShowProp?.SetValue(optionsDefault, previousAlwaysShowPanelSelectionDlg);
                confirmCloseProp?.SetValue(settingsDefault, previousConfirmCloseConnection);
                hostForm.Close();
                hostForm.Dispose();
            }
        });

        private static void AttachTestInterface(ConnectionTab tab, ConnectionInfo connectionInfo)
        {
            var protocol = new TestProtocol();
            var interfaceControl = new InterfaceControl(tab, protocol, connectionInfo)
            {
                OriginalInfo = connectionInfo
            };

            protocol.InterfaceControl = interfaceControl;
            tab.Tag = interfaceControl;
        }

        private static ConnectionTab[] GetConnectionTabs(ConnectionWindow connectionWindow)
        {
            FieldInfo dockField = typeof(ConnectionWindow).GetField("connDock", BindingFlags.Instance | BindingFlags.NonPublic)
                ?? throw new AssertionException("Failed to resolve ConnectionWindow.connDock field.");
            var dockPanel = dockField.GetValue(connectionWindow) as DockPanel
                ?? throw new AssertionException("Failed to resolve connection dock panel.");

            return dockPanel.DocumentsToArray().OfType<ConnectionTab>().ToArray();
        }

        private static ConnectionTab? FindTab(ConnectionWindow connectionWindow, ConnectionInfo connectionInfo)
        {
            return GetConnectionTabs(connectionWindow).FirstOrDefault(tab => RepresentsConnection(tab, connectionInfo));
        }

        private static int GetTabIndex(ConnectionWindow connectionWindow, ConnectionInfo connectionInfo)
        {
            ConnectionTab[] tabs = GetConnectionTabs(connectionWindow);
            for (int i = 0; i < tabs.Length; i++)
            {
                if (RepresentsConnection(tabs[i], connectionInfo))
                    return i;
            }

            return -1;
        }

        private static bool RepresentsConnection(ConnectionTab tab, ConnectionInfo connectionInfo)
        {
            if (tab.Tag is InterfaceControl interfaceControl)
                return interfaceControl.Info == connectionInfo || interfaceControl.OriginalInfo == connectionInfo;

            if (tab.Tag is ConnectionInfo taggedConnectionInfo)
                return taggedConnectionInfo == connectionInfo;

            return tab.TrackedConnectionInfo == connectionInfo;
        }

        private static void InvokeReconnect(ConnectionWindow connectionWindow)
        {
            MethodInfo reconnectMethod = typeof(ConnectionWindow).GetMethod("Reconnect", BindingFlags.Instance | BindingFlags.NonPublic)
                ?? throw new AssertionException("Failed to resolve ConnectionWindow.Reconnect method.");
            reconnectMethod.Invoke(connectionWindow, null);
            Application.DoEvents();
        }

        private sealed class TestProtocol : ProtocolBase
        {
            public override bool Initialize()
            {
                if (InterfaceControl.Parent != null)
                    InterfaceControl.Parent.Tag = InterfaceControl;

                return true;
            }

            public override bool Connect()
            {
                return true;
            }

            public override void Close()
            {
            }
        }
    }
}
