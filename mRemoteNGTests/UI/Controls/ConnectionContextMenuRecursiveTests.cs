using System.Collections.Generic;
using System.Threading;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.UI.Controls;
using mRemoteNG.App;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls
{
    [TestFixture]
    public class ConnectionContextMenuRecursiveTests
    {
        private class MockProtocol : ProtocolBase
        {
            public bool IsDisconnected { get; private set; }

            public override void Disconnect()
            {
                IsDisconnected = true;
            }
        }

        [Test]
        public void DisconnectConnection_ShouldDisconnectRecursively()
        {
            // Arrange
            // Ensure no confirmation dialog
            mRemoteNG.Properties.Settings.Default.ConfirmCloseConnection = (int)mRemoteNG.Config.ConfirmCloseEnum.Never;

            var rootContainer = new ContainerInfo { Name = "Root" };
            var childConnection = new ConnectionInfo { Name = "Child" };
            var subContainer = new ContainerInfo { Name = "SubContainer" };
            var grandchildConnection = new ConnectionInfo { Name = "Grandchild" };

            rootContainer.AddChild(childConnection);
            rootContainer.AddChild(subContainer);
            subContainer.AddChild(grandchildConnection);

            var childProtocol = new MockProtocol();
            childConnection.OpenConnections.Add(childProtocol);

            var grandchildProtocol = new MockProtocol();
            grandchildConnection.OpenConnections.Add(grandchildProtocol);

            ConnectionContextMenu? menu = null;
            var thread = new Thread(() =>
            {
                menu = new ConnectionContextMenu(null!);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            // Act
            // We need to run this on STA thread as well because ConnectionContextMenu is a Control
            // and might access UI properties, although DisconnectConnectionInternal mostly works with logic.
            // But to be safe and consistent with instantiation:
            
            thread = new Thread(() =>
            {
                menu!.DisconnectConnection(rootContainer);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            // Assert
            Assert.That(childProtocol.IsDisconnected, Is.True, "Direct child connection should be disconnected");
            Assert.That(grandchildProtocol.IsDisconnected, Is.True, "Grandchild connection (in subfolder) should be disconnected");
        }
    }
}
