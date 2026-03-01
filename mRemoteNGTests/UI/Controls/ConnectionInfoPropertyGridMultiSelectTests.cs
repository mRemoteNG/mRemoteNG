using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.UI.Controls.ConnectionInfoPropertyGrid;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls
{
    [TestFixture]
    public class ConnectionInfoPropertyGridMultiSelectTests
    {
        private ConnectionInfoPropertyGrid _propertyGrid;

        [SetUp]
        public void Setup()
        {
            // We need to initialize the PropertyGrid on a thread with a message pump (STA), 
            // but for unit tests checking logic, we might get away with it if we don't show it.
            // However, PropertyGrid controls often require UI thread.
            // mRemoteNGTests usually run in STA.
            _propertyGrid = new ConnectionInfoPropertyGrid();
        }

        [TearDown]
        public void Teardown()
        {
            _propertyGrid?.Dispose();
        }

        [Test]
        public void IntersectionOfPropertiesIsShown_WhenSelectingDifferentProtocols()
        {
            // Arrange
            var rdpConnection = new ConnectionInfo
            {
                Name = "RDP Connection",
                Protocol = ProtocolType.RDP
            };

            var sshConnection = new ConnectionInfo
            {
                Name = "SSH Connection",
                Protocol = ProtocolType.SSH2
            };

            // Act
            _propertyGrid.SelectedConnectionInfos = new[] { rdpConnection, sshConnection };

            // Assert
            var browsableProperties = _propertyGrid.BrowsableProperties;

            // RDP has "RedirectDiskDrives"
            // SSH does NOT have "RedirectDiskDrives" (it's not valid for SSH)
            // So "RedirectDiskDrives" should NOT be in the intersection.
            Assert.That(browsableProperties, Does.Not.Contain("RedirectDiskDrives"), "Protocol-specific property 'RedirectDiskDrives' should not be shown for RDP+SSH selection.");

            // "Name" is common to both.
            Assert.That(browsableProperties, Contains.Item("Name"), "Common property 'Name' should be shown.");
            
            // "Hostname" is common to both.
            Assert.That(browsableProperties, Contains.Item("Hostname"), "Common property 'Hostname' should be shown.");
        }

        [Test]
        public void UnionOfPropertiesIsShown_WhenSelectingSameProtocol()
        {
            // Arrange
            var rdpConnection1 = new ConnectionInfo
            {
                Name = "RDP Connection 1",
                Protocol = ProtocolType.RDP
            };

            var rdpConnection2 = new ConnectionInfo
            {
                Name = "RDP Connection 2",
                Protocol = ProtocolType.RDP
            };

            // Act
            _propertyGrid.SelectedConnectionInfos = new[] { rdpConnection1, rdpConnection2 };

            // Assert
            var browsableProperties = _propertyGrid.BrowsableProperties;

            // RDP has "RedirectDiskDrives"
            Assert.That(browsableProperties, Contains.Item("RedirectDiskDrives"), "RDP property 'RedirectDiskDrives' should be shown when both are RDP.");
            Assert.That(browsableProperties, Contains.Item("Name"), "Common property 'Name' should be shown.");
        }
    }
}
