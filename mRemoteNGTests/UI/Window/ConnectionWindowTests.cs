using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.UI;
using mRemoteNG.UI.Tabs;
using mRemoteNG.UI.Window;
using NSubstitute;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;
using WeifenLuo.WinFormsUI.ThemeVS2015;

namespace mRemoteNGTests.UI.Window
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class ConnectionWindowTests
    {
        [Test]
        public void ReconnectAll_WithATabWhoseProtocolHasNotAttached_ReconnectsOnlyTheAttachedTabs()
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

            hostForm.Controls.Add(dockPanel);
            hostForm.Show();

            using var panelHost = new DockContent();
            using var connectionWindow = new ConnectionWindow(panelHost);
            connectionWindow.Show(dockPanel, DockState.Document);

            var connectedInfo = new ConnectionInfo { Name = "Connected" };
            ConnectionTab connectedTab = connectionWindow.AddConnectionTab(connectedInfo);
            ProtocolBase connectedProtocol = Substitute.For<ProtocolBase>();
            // As ProtocolBase.Initialize does once the protocol is up.
            connectedTab.Tag = new InterfaceControl(connectedTab, connectedProtocol, connectedInfo);

            // AddConnectionTab leaves the ConnectionInfo in Tag until the protocol attaches; this
            // tab is still connecting, or its protocol could not be created.
            connectionWindow.AddConnectionTab(new ConnectionInfo { Name = "Still connecting" });

            IConnectionInitiator initiator = Substitute.For<IConnectionInitiator>();

            connectionWindow.ReconnectAll(initiator);

            connectedProtocol.Received(1).Close();
            initiator.Received(1).OpenConnection(connectedInfo, ConnectionInfo.Force.DoNotJump);
            initiator.ReceivedWithAnyArgs(1).OpenConnection(default(ConnectionInfo));
        }

        [Test]
        public void SwitchToOpenConnection_WithAnotherDocumentInThePanel_FindsTheConnectionTab()
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

            hostForm.Controls.Add(dockPanel);
            hostForm.Show();

            using var panelHost = new DockContent();
            using var connectionWindow = new ConnectionWindow(panelHost);
            connectionWindow.Show(dockPanel, DockState.Document);

            // AppWindows.Show docks the Options window as a document of the General connection panel.
            using var otherDocument = new DockContent();
            otherDocument.Show(connectionWindow.connDock, DockState.Document);

            var openInfo = new ConnectionInfo { Name = "Open" };
            ConnectionTab openTab = connectionWindow.AddConnectionTab(openInfo);
            ProtocolBase openProtocol = Substitute.For<ProtocolBase>();
            openTab.Tag = new InterfaceControl(openTab, openProtocol, openInfo);
            openInfo.OpenConnections.Add(openProtocol);

            WindowList previousWindowList = Runtime.WindowList;
            Runtime.WindowList = [connectionWindow];
            try
            {
                Assert.That(new ConnectionInitiator().SwitchToOpenConnection(openInfo), Is.True);
            }
            finally
            {
                Runtime.WindowList = previousWindowList;
            }
        }

        [Test]
        public void SwitchToOpenConnection_WhileTheConnectionIsStillBeingOpened_FindsItsTab()
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

            hostForm.Controls.Add(dockPanel);
            hostForm.Show();

            using var panelHost = new DockContent();
            using var connectionWindow = new ConnectionWindow(panelHost);
            connectionWindow.Show(dockPanel, DockState.Document);

            // While 1Password is read, the tab is there but the protocol is not attached to it yet.
            var openingInfo = new ConnectionInfo { Name = "Opening" };
            ConnectionTab openingTab = connectionWindow.AddConnectionTab(openingInfo);
            var initiator = new ConnectionInitiator();

            WindowList previousWindowList = Runtime.WindowList;
            Runtime.WindowList = [connectionWindow];
            try
            {
                Assert.That(initiator.SwitchToOpenConnection(openingInfo), Is.False);

                // As OpenConnection does once the tab is created
                var openingTabs = (Dictionary<ConnectionTab, ConnectionInfo>)typeof(ConnectionInitiator)
                    .GetField("_openingTabs", BindingFlags.NonPublic | BindingFlags.Instance)!
                    .GetValue(initiator);
                openingTabs.Add(openingTab, openingInfo);

                Assert.That(initiator.SwitchToOpenConnection(openingInfo), Is.True);
                Assert.That(initiator.SwitchToOpenConnection(new ConnectionInfo { Name = "Other" }), Is.False);

                // Closing the tab disposes it
                openingTab.Dispose();

                Assert.That(initiator.SwitchToOpenConnection(openingInfo), Is.False);
            }
            finally
            {
                Runtime.WindowList = previousWindowList;
            }
        }
    }
}
