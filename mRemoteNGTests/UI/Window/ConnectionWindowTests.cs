using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
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
    }
}
