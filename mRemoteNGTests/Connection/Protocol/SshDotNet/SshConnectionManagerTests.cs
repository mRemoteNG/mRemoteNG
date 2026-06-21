using mRemoteNG.Connection.Protocol.SshDotNet;
using NUnit.Framework;
using System;
using Renci.SshNet;

namespace mRemoteNGTests.Connection.Protocol.SshDotNet
{
    [TestFixture]
    [Category("Unit")]
    public class SshConnectionManagerTests
    {
        [SetUp]
        public void Setup()
        {
            // Reset diagnostic flags to default state
            SshDotNetDiagnostics.VerboseLogging = false;
            SshDotNetDiagnostics.TraceLogging = false;
        }

        [TearDown]
        public void TearDown()
        {
            // Reset to defaults after tests
            SshDotNetDiagnostics.VerboseLogging = false;
            SshDotNetDiagnostics.TraceLogging = false;
        }

        #region CreateConnection Tests

        [Test]
        public void CreateConnection_ThrowsArgumentException_WhenHostnameIsNull()
        {
            // Arrange
            string hostname = null;
            int port = 22;
            string username = "testuser";
            using var authMethods0 = new PasswordAuthenticationMethod("testuser", "password");
            var authMethods = new[] { authMethods0 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                SshConnectionManager.CreateConnection(hostname, port, username, authMethods));
        }

        [Test]
        public void CreateConnection_ThrowsArgumentException_WhenHostnameIsEmpty()
        {
            // Arrange
            string hostname = "";
            int port = 22;
            string username = "testuser";
            using var authMethods0 = new PasswordAuthenticationMethod("testuser", "password");
            var authMethods = new[] { authMethods0 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                SshConnectionManager.CreateConnection(hostname, port, username, authMethods));
        }

        [Test]
        public void CreateConnection_ThrowsArgumentOutOfRangeException_WhenPortIsZero()
        {
            // Arrange
            string hostname = "localhost";
            int port = 0;
            string username = "testuser";
            using var authMethods0 = new PasswordAuthenticationMethod("testuser", "password");
            var authMethods = new[] { authMethods0 };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                SshConnectionManager.CreateConnection(hostname, port, username, authMethods));
        }

        [Test]
        public void CreateConnection_ThrowsArgumentOutOfRangeException_WhenPortIsNegative()
        {
            // Arrange
            string hostname = "localhost";
            int port = -1;
            string username = "testuser";
            using var authMethods0 = new PasswordAuthenticationMethod("testuser", "password");
            var authMethods = new[] { authMethods0 };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                SshConnectionManager.CreateConnection(hostname, port, username, authMethods));
        }

        [Test]
        public void CreateConnection_ThrowsArgumentOutOfRangeException_WhenPortIsTooLarge()
        {
            // Arrange
            string hostname = "localhost";
            int port = 65536;
            string username = "testuser";
            using var authMethods0 = new PasswordAuthenticationMethod("testuser", "password");
            var authMethods = new[] { authMethods0 };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                SshConnectionManager.CreateConnection(hostname, port, username, authMethods));
        }

        [Test]
        public void CreateConnection_ThrowsArgumentException_WhenUsernameIsNull()
        {
            // Arrange
            string hostname = "localhost";
            int port = 22;
            string username = null;
            using var authMethods0 = new PasswordAuthenticationMethod("testuser", "password");
            var authMethods = new[] { authMethods0 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                SshConnectionManager.CreateConnection(hostname, port, username, authMethods));
        }

        [Test]
        public void CreateConnection_ThrowsArgumentException_WhenUsernameIsEmpty()
        {
            // Arrange
            string hostname = "localhost";
            int port = 22;
            string username = "";
            using var authMethods0 = new PasswordAuthenticationMethod("testuser", "password");
            var authMethods = new[] { authMethods0 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                SshConnectionManager.CreateConnection(hostname, port, username, authMethods));
        }

        [Test]
        public void CreateConnection_ThrowsArgumentException_WhenAuthMethodsIsNull()
        {
            // Arrange
            string hostname = "localhost";
            int port = 22;
            string username = "testuser";
            AuthenticationMethod[] authMethods = null;

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                SshConnectionManager.CreateConnection(hostname, port, username, authMethods));
        }

        [Test]
        public void CreateConnection_ThrowsArgumentException_WhenAuthMethodsIsEmpty()
        {
            // Arrange
            string hostname = "localhost";
            int port = 22;
            string username = "testuser";
            AuthenticationMethod[] authMethods = Array.Empty<AuthenticationMethod>();

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                SshConnectionManager.CreateConnection(hostname, port, username, authMethods));
        }

        [Test]
        public void CreateConnection_ReturnsClient_WithValidParameters()
        {
            // Arrange
            string hostname = "localhost";
            int port = 22;
            string username = "testuser";
            using var authMethods0 = new PasswordAuthenticationMethod("testuser", "password");
            var authMethods = new[] { authMethods0 };

            // Act
            using var client = SshConnectionManager.CreateConnection(hostname, port, username, authMethods);

            // Assert
            Assert.That(client, Is.Not.Null);
            Assert.That(client.ConnectionInfo.Host, Is.EqualTo(hostname));
            Assert.That(client.ConnectionInfo.Port, Is.EqualTo(port));
            Assert.That(client.ConnectionInfo.Username, Is.EqualTo(username));
            Assert.That(client.IsConnected, Is.False);
        }

        [Test]
        public void CreateConnection_SetsCustomTimeout_WhenProvided()
        {
            // Arrange
            string hostname = "localhost";
            int port = 22;
            string username = "testuser";
            using var authMethods0 = new PasswordAuthenticationMethod("testuser", "password");
            var authMethods = new[] { authMethods0 };
            TimeSpan customTimeout = TimeSpan.FromSeconds(60);

            // Act
            using var client = SshConnectionManager.CreateConnection(hostname, port, username, authMethods, customTimeout);

            // Assert
            Assert.That(client, Is.Not.Null);
            Assert.That(client.ConnectionInfo.Timeout, Is.EqualTo(customTimeout));
        }

        [Test]
        public void CreateConnection_SetsDefaultTimeout_WhenNotProvided()
        {
            // Arrange
            string hostname = "localhost";
            int port = 22;
            string username = "testuser";
            using var authMethods0 = new PasswordAuthenticationMethod("testuser", "password");
            var authMethods = new[] { authMethods0 };

            // Act
            using var client = SshConnectionManager.CreateConnection(hostname, port, username, authMethods);

            // Assert
            Assert.That(client, Is.Not.Null);
            Assert.That(client.ConnectionInfo.Timeout, Is.EqualTo(TimeSpan.FromSeconds(15)));
        }

        [Test]
        public void CreateConnection_SetsUtf8Encoding()
        {
            // Arrange
            string hostname = "localhost";
            int port = 22;
            string username = "testuser";
            using var authMethods0 = new PasswordAuthenticationMethod("testuser", "password");
            var authMethods = new[] { authMethods0 };

            // Act
            using var client = SshConnectionManager.CreateConnection(hostname, port, username, authMethods);

            // Assert
            Assert.That(client, Is.Not.Null);
            Assert.That(client.ConnectionInfo.Encoding, Is.EqualTo(System.Text.Encoding.UTF8));
        }

        #endregion

        #region Connect Tests

        [Test]
        public void Connect_ThrowsArgumentNullException_WhenClientIsNull()
        {
            // Arrange
            SshClient client = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                SshConnectionManager.Connect(client));
        }

        // Note: Cannot test actual connection without a real SSH server
        // Integration tests would be needed for Connect() success scenarios

        #endregion

        #region CreateShellStream Tests

        [Test]
        public void CreateShellStream_ThrowsArgumentNullException_WhenClientIsNull()
        {
            // Arrange
            SshClient client = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                SshConnectionManager.CreateShellStream(client));
        }

        [Test]
        public void CreateShellStream_ThrowsInvalidOperationException_WhenClientNotConnected()
        {
            // Arrange
            string hostname = "localhost";
            int port = 22;
            string username = "testuser";
            using var authMethods0 = new PasswordAuthenticationMethod("testuser", "password");
            var authMethods = new[] { authMethods0 };
            using var client = SshConnectionManager.CreateConnection(hostname, port, username, authMethods);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                SshConnectionManager.CreateShellStream(client));
        }

        // Note: Cannot test CreateShellStream success without a connected client
        // Integration tests would be needed for success scenarios

        #endregion

        #region ConfigureKeepAlive Tests

        [Test]
        public void ConfigureKeepAlive_ThrowsArgumentNullException_WhenClientIsNull()
        {
            // Arrange
            SshClient client = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                SshConnectionManager.ConfigureKeepAlive(client));
        }

        [Test]
        public void ConfigureKeepAlive_SetsDefaultInterval_WhenNotProvided()
        {
            // Arrange
            string hostname = "localhost";
            int port = 22;
            string username = "testuser";
            using var authMethods0 = new PasswordAuthenticationMethod("testuser", "password");
            var authMethods = new[] { authMethods0 };
            using var client = SshConnectionManager.CreateConnection(hostname, port, username, authMethods);

            // Act
            SshConnectionManager.ConfigureKeepAlive(client);

            // Assert
            Assert.That(client.KeepAliveInterval, Is.EqualTo(TimeSpan.FromSeconds(5)));
        }

        [Test]
        public void ConfigureKeepAlive_SetsCustomInterval_WhenProvided()
        {
            // Arrange
            string hostname = "localhost";
            int port = 22;
            string username = "testuser";
            using var authMethods0 = new PasswordAuthenticationMethod("testuser", "password");
            var authMethods = new[] { authMethods0 };
            using var client = SshConnectionManager.CreateConnection(hostname, port, username, authMethods);
            TimeSpan customInterval = TimeSpan.FromSeconds(10);

            // Act
            SshConnectionManager.ConfigureKeepAlive(client, customInterval);

            // Assert
            Assert.That(client.KeepAliveInterval, Is.EqualTo(customInterval));
        }

        #endregion

        #region GetConnectionInfo Tests

        [Test]
        public void GetConnectionInfo_ReturnsNullMessage_WhenClientIsNull()
        {
            // Arrange
            SshClient client = null;

            // Act
            string info = SshConnectionManager.GetConnectionInfo(client);

            // Assert
            Assert.That(info, Is.EqualTo("Client is null"));
        }

        [Test]
        public void GetConnectionInfo_ReturnsNotConnectedMessage_WhenClientNotConnected()
        {
            // Arrange
            string hostname = "localhost";
            int port = 22;
            string username = "testuser";
            using var authMethods0 = new PasswordAuthenticationMethod("testuser", "password");
            var authMethods = new[] { authMethods0 };
            using var client = SshConnectionManager.CreateConnection(hostname, port, username, authMethods);

            // Act
            string info = SshConnectionManager.GetConnectionInfo(client);

            // Assert
            Assert.That(info, Is.EqualTo("Client is not connected"));
        }

        // Note: Cannot test GetConnectionInfo with connected client without real SSH server
        // Integration tests would be needed for connected scenarios

        #endregion
    }
}
