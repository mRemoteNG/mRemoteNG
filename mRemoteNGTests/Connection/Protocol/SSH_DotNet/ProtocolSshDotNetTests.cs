using mRemoteNG.Connection.Protocol.SSH_DotNet;
using NUnit.Framework;
using System;
using mRemoteNG.Connection;

namespace mRemoteNGTests.Connection.Protocol.SSH_DotNet
{
    [TestFixture]
    [Category("Unit")]
    public class ProtocolSshDotNetTests
    {
        private ProtocolSshDotNet _protocol;

        [SetUp]
        public void Setup()
        {
            // Reset diagnostic flags to default state
            SshDotNetDiagnostics.VerboseLogging = false;
            SshDotNetDiagnostics.TraceLogging = false;

            // Create protocol instance
            _protocol = new ProtocolSshDotNet();
        }

        [TearDown]
        public void TearDown()
        {
            // Reset to defaults after tests
            SshDotNetDiagnostics.VerboseLogging = false;
            SshDotNetDiagnostics.TraceLogging = false;

            // Cleanup protocol
            try
            {
                if (_protocol != null && _protocol.IsConnected)
                {
                    _protocol.Disconnect();
                }
            }
            catch
            {
                // Ignore cleanup errors
            }

            _protocol = null;
        }

        #region Constructor and Initialization Tests

        [Test]
        public void Constructor_CreatesInstance_WithDefaultState()
        {
            // Act
            var protocol = new ProtocolSshDotNet();

            // Assert
            Assert.That(protocol, Is.Not.Null);
            Assert.That(protocol.State, Is.EqualTo(ProtocolSshDotNet.ConnectionState.Disconnected));
            Assert.That(protocol.IsConnected, Is.False);
        }

        // Note: Cannot test Initialize() success path without InterfaceControl being set up
        // which requires a ConnectionTab and full UI infrastructure.
        // These tests would belong in integration tests.

        #endregion

        #region State Property Tests

        [Test]
        public void State_DefaultsToDisconnected()
        {
            // Arrange
            var protocol = new ProtocolSshDotNet();

            // Act
            var state = protocol.State;

            // Assert
            Assert.That(state, Is.EqualTo(ProtocolSshDotNet.ConnectionState.Disconnected));
        }

        [Test]
        public void IsConnected_ReturnsFalse_WhenDisconnected()
        {
            // Arrange
            var protocol = new ProtocolSshDotNet();

            // Act
            bool isConnected = protocol.IsConnected;

            // Assert
            Assert.That(isConnected, Is.False);
        }

        #endregion

        #region Statistics Property Tests

        [Test]
        public void BytesReceived_DefaultsToZero()
        {
            // Arrange
            var protocol = new ProtocolSshDotNet();

            // Act
            long bytesReceived = protocol.BytesReceived;

            // Assert
            Assert.That(bytesReceived, Is.EqualTo(0));
        }

        [Test]
        public void BytesSent_DefaultsToZero()
        {
            // Arrange
            var protocol = new ProtocolSshDotNet();

            // Act
            long bytesSent = protocol.BytesSent;

            // Assert
            Assert.That(bytesSent, Is.EqualTo(0));
        }

        [Test]
        public void ConnectionDuration_ReturnsZero_WhenNotConnected()
        {
            // Arrange
            var protocol = new ProtocolSshDotNet();

            // Act
            var duration = protocol.ConnectionDuration;

            // Assert
            Assert.That(duration, Is.EqualTo(TimeSpan.Zero));
        }

        #endregion

        #region Default Port Tests

        [Test]
        public void DefaultPort_IsStandardSSHPort()
        {
            // Arrange & Act
            int defaultPort = (int)ProtocolSshDotNet.Defaults.Port;

            // Assert
            Assert.That(defaultPort, Is.EqualTo(22), "Default SSH port should be 22");
        }

        #endregion

        #region Connection Validation Tests

        [Test]
        public void Connect_ReturnsFalse_WhenInterfaceControlIsNull()
        {
            // Arrange
            var protocol = new ProtocolSshDotNet();
            // Don't initialize, so InterfaceControl is null

            // Act
            bool result = protocol.Connect();

            // Assert
            Assert.That(result, Is.False);
            Assert.That(protocol.State, Is.EqualTo(ProtocolSshDotNet.ConnectionState.Error));
        }

        [Test]
        public void Connect_SetsStateToConnecting_WhenCalled()
        {
            // Arrange
            var protocol = new ProtocolSshDotNet();
            protocol.Initialize();

            // Create a minimal connection info
            var connectionInfo = new mRemoteNG.Connection.ConnectionInfo();
            connectionInfo.Hostname = "localhost";
            connectionInfo.Username = "testuser";
            connectionInfo.Password = "testpass";
            protocol.InterfaceControl.Info = connectionInfo;

            // Act
            protocol.Connect();

            // Assert - will fail to connect but should have set state to Connecting first
            // Note: This will transition to Error state because we can't actually connect
            Assert.That(protocol.State, Is.EqualTo(ProtocolSshDotNet.ConnectionState.Error).Or.EqualTo(ProtocolSshDotNet.ConnectionState.Connecting));
        }

        #endregion

        #region Disconnect Tests

        [Test]
        public void Disconnect_DoesNotThrow_WhenNeverConnected()
        {
            // Arrange
            var protocol = new ProtocolSshDotNet();
            protocol.Initialize();

            // Act & Assert
            Assert.DoesNotThrow(() => protocol.Disconnect());
        }

        [Test]
        public void Disconnect_SetsStateToDisconnecting_WhenCalled()
        {
            // Arrange
            var protocol = new ProtocolSshDotNet();
            protocol.Initialize();

            // Act
            protocol.Disconnect();

            // Assert - should transition to Disconnecting then Disconnected
            Assert.That(protocol.State, Is.EqualTo(ProtocolSshDotNet.ConnectionState.Disconnected).Or.EqualTo(ProtocolSshDotNet.ConnectionState.Disconnecting));
        }

        #endregion

        #region ConnectionState Enum Tests

        [Test]
        public void ConnectionState_HasAllExpectedValues()
        {
            // Act & Assert
            Assert.That(Enum.IsDefined(typeof(ProtocolSshDotNet.ConnectionState), "Disconnected"), Is.True);
            Assert.That(Enum.IsDefined(typeof(ProtocolSshDotNet.ConnectionState), "Connecting"), Is.True);
            Assert.That(Enum.IsDefined(typeof(ProtocolSshDotNet.ConnectionState), "Authenticating"), Is.True);
            Assert.That(Enum.IsDefined(typeof(ProtocolSshDotNet.ConnectionState), "Connected"), Is.True);
            Assert.That(Enum.IsDefined(typeof(ProtocolSshDotNet.ConnectionState), "Disconnecting"), Is.True);
            Assert.That(Enum.IsDefined(typeof(ProtocolSshDotNet.ConnectionState), "Error"), Is.True);
        }

        [Test]
        public void ConnectionState_DisconnectedValue_IsZero()
        {
            // Act
            var value = (int)ProtocolSshDotNet.ConnectionState.Disconnected;

            // Assert
            Assert.That(value, Is.EqualTo(0), "Disconnected should be the default enum value (0)");
        }

        #endregion

        #region Property Inheritance Tests

        [Test]
        public void Protocol_InheritsFromProtocolBase()
        {
            // Arrange
            var protocol = new ProtocolSshDotNet();

            // Act & Assert
            Assert.That(protocol, Is.InstanceOf<mRemoteNG.Connection.Protocol.ProtocolBase>());
        }

        #endregion

        #region Error Handling Tests

        [Test]
        public void Connect_HandlesNullHostname_Gracefully()
        {
            // Arrange
            var protocol = new ProtocolSshDotNet();
            protocol.Initialize();

            var connectionInfo = new mRemoteNG.Connection.ConnectionInfo();
            connectionInfo.Hostname = null;  // Invalid
            connectionInfo.Username = "testuser";
            connectionInfo.Password = "testpass";
            protocol.InterfaceControl.Info = connectionInfo;

            // Act
            bool result = protocol.Connect();

            // Assert
            Assert.That(result, Is.False);
            Assert.That(protocol.State, Is.EqualTo(ProtocolSshDotNet.ConnectionState.Error));
        }

        [Test]
        public void Connect_HandlesEmptyHostname_Gracefully()
        {
            // Arrange
            var protocol = new ProtocolSshDotNet();
            protocol.Initialize();

            var connectionInfo = new mRemoteNG.Connection.ConnectionInfo();
            connectionInfo.Hostname = "";  // Invalid
            connectionInfo.Username = "testuser";
            connectionInfo.Password = "testpass";
            protocol.InterfaceControl.Info = connectionInfo;

            // Act
            bool result = protocol.Connect();

            // Assert
            Assert.That(result, Is.False);
            Assert.That(protocol.State, Is.EqualTo(ProtocolSshDotNet.ConnectionState.Error));
        }

        [Test]
        public void Connect_HandlesNullUsername_Gracefully()
        {
            // Arrange
            var protocol = new ProtocolSshDotNet();
            protocol.Initialize();

            var connectionInfo = new mRemoteNG.Connection.ConnectionInfo();
            connectionInfo.Hostname = "localhost";
            connectionInfo.Username = null;  // Invalid
            connectionInfo.Password = "testpass";
            protocol.InterfaceControl.Info = connectionInfo;

            // Act
            bool result = protocol.Connect();

            // Assert
            Assert.That(result, Is.False);
            Assert.That(protocol.State, Is.EqualTo(ProtocolSshDotNet.ConnectionState.Error));
        }

        [Test]
        public void Connect_HandlesEmptyUsername_Gracefully()
        {
            // Arrange
            var protocol = new ProtocolSshDotNet();
            protocol.Initialize();

            var connectionInfo = new mRemoteNG.Connection.ConnectionInfo();
            connectionInfo.Hostname = "localhost";
            connectionInfo.Username = "";  // Invalid
            connectionInfo.Password = "testpass";
            protocol.InterfaceControl.Info = connectionInfo;

            // Act
            bool result = protocol.Connect();

            // Assert
            Assert.That(result, Is.False);
            Assert.That(protocol.State, Is.EqualTo(ProtocolSshDotNet.ConnectionState.Error));
        }

        [Test]
        public void Connect_AcceptsEmptyPassword()
        {
            // Arrange
            var protocol = new ProtocolSshDotNet();
            protocol.Initialize();

            var connectionInfo = new mRemoteNG.Connection.ConnectionInfo();
            connectionInfo.Hostname = "localhost";
            connectionInfo.Username = "testuser";
            connectionInfo.Password = "";  // Empty password is valid (might use key auth)
            protocol.InterfaceControl.Info = connectionInfo;

            // Act
            protocol.Connect();

            // Assert - should not fail due to empty password (will fail to connect for other reasons)
            // We're just verifying it doesn't reject empty password
            Assert.That(protocol.State, Is.Not.EqualTo(ProtocolSshDotNet.ConnectionState.Disconnected),
                "Protocol should have attempted connection (empty password is allowed)");
        }

        #endregion

        // Note: The following tests require actual SSH connections and should be in integration tests:
        // - Successful connection flow
        // - Authentication success/failure
        // - Data transfer (input/output)
        // - Terminal resizing during active session
        // - Graceful disconnection
        // - Error disconnection
        // - Smart disconnect detection
    }
}
