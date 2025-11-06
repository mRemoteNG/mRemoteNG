using mRemoteNG.Connection.Protocol.SSH_DotNet;
using NUnit.Framework;
using System;
using Renci.SshNet;

namespace mRemoteNGTests.Connection.Protocol.SSH_DotNet
{
    [TestFixture]
    public class SSHConnectionManagerTests
    {
        [SetUp]
        public void Setup()
        {
            // Reset diagnostic flags to default state
            SSHDotNetDiagnostics.VerboseLogging = false;
            SSHDotNetDiagnostics.TraceLogging = false;
        }

        [TearDown]
        public void TearDown()
        {
            // Reset to defaults after tests
            SSHDotNetDiagnostics.VerboseLogging = false;
            SSHDotNetDiagnostics.TraceLogging = false;
        }

        #region CreateConnection Tests

        [Test]
        public void CreateConnection_ThrowsArgumentException_WhenHostnameIsNull()
        {
            // Arrange
            string hostname = null;
            int port = 22;
            string username = "testuser";
            var authMethods = new[] { new PasswordAuthenticationMethod("testuser", "password") };

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                SSHConnectionManager.CreateConnection(hostname, port, username, authMethods));
        }

        [Test]
        public void CreateConnection_ThrowsArgumentException_WhenHostnameIsEmpty()
        {
            // Arrange
            string hostname = "";
            int port = 22;
            string username = "testuser";
            var authMethods = new[] { new PasswordAuthenticationMethod("testuser", "password") };

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                SSHConnectionManager.CreateConnection(hostname, port, username, authMethods));
        }

        [Test]
        public void CreateConnection_ThrowsArgumentOutOfRangeException_WhenPortIsZero()
        {
            // Arrange
            string hostname = "localhost";
            int port = 0;
            string username = "testuser";
            var authMethods = new[] { new PasswordAuthenticationMethod("testuser", "password") };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                SSHConnectionManager.CreateConnection(hostname, port, username, authMethods));
        }

        [Test]
        public void CreateConnection_ThrowsArgumentOutOfRangeException_WhenPortIsNegative()
        {
            // Arrange
            string hostname = "localhost";
            int port = -1;
            string username = "testuser";
            var authMethods = new[] { new PasswordAuthenticationMethod("testuser", "password") };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                SSHConnectionManager.CreateConnection(hostname, port, username, authMethods));
        }

        [Test]
        public void CreateConnection_ThrowsArgumentOutOfRangeException_WhenPortIsTooLarge()
        {
            // Arrange
            string hostname = "localhost";
            int port = 65536;
            string username = "testuser";
            var authMethods = new[] { new PasswordAuthenticationMethod("testuser", "password") };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                SSHConnectionManager.CreateConnection(hostname, port, username, authMethods));
        }

        [Test]
        public void CreateConnection_ThrowsArgumentException_WhenUsernameIsNull()
        {
            // Arrange
            string hostname = "localhost";
            int port = 22;
            string username = null;
            var authMethods = new[] { new PasswordAuthenticationMethod("testuser", "password") };

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                SSHConnectionManager.CreateConnection(hostname, port, username, authMethods));
        }

        [Test]
        public void CreateConnection_ThrowsArgumentException_WhenUsernameIsEmpty()
        {
            // Arrange
            string hostname = "localhost";
            int port = 22;
            string username = "";
            var authMethods = new[] { new PasswordAuthenticationMethod("testuser", "password") };

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                SSHConnectionManager.CreateConnection(hostname, port, username, authMethods));
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
                SSHConnectionManager.CreateConnection(hostname, port, username, authMethods));
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
                SSHConnectionManager.CreateConnection(hostname, port, username, authMethods));
        }

        [Test]
        public void CreateConnection_ReturnsClient_WithValidParameters()
        {
            // Arrange
            string hostname = "localhost";
            int port = 22;
            string username = "testuser";
            var authMethods = new[] { new PasswordAuthenticationMethod("testuser", "password") };

            // Act
            var client = SSHConnectionManager.CreateConnection(hostname, port, username, authMethods);

            // Assert
            Assert.That(client, Is.Not.Null);
            Assert.That(client.ConnectionInfo.Host, Is.EqualTo(hostname));
            Assert.That(client.ConnectionInfo.Port, Is.EqualTo(port));
            Assert.That(client.ConnectionInfo.Username, Is.EqualTo(username));
            Assert.That(client.IsConnected, Is.False);

            // Cleanup
            client?.Dispose();
        }

        [Test]
        public void CreateConnection_SetsCustomTimeout_WhenProvided()
        {
            // Arrange
            string hostname = "localhost";
            int port = 22;
            string username = "testuser";
            var authMethods = new[] { new PasswordAuthenticationMethod("testuser", "password") };
            TimeSpan customTimeout = TimeSpan.FromSeconds(60);

            // Act
            var client = SSHConnectionManager.CreateConnection(hostname, port, username, authMethods, customTimeout);

            // Assert
            Assert.That(client, Is.Not.Null);
            Assert.That(client.ConnectionInfo.Timeout, Is.EqualTo(customTimeout));

            // Cleanup
            client?.Dispose();
        }

        [Test]
        public void CreateConnection_SetsDefaultTimeout_WhenNotProvided()
        {
            // Arrange
            string hostname = "localhost";
            int port = 22;
            string username = "testuser";
            var authMethods = new[] { new PasswordAuthenticationMethod("testuser", "password") };

            // Act
            var client = SSHConnectionManager.CreateConnection(hostname, port, username, authMethods);

            // Assert
            Assert.That(client, Is.Not.Null);
            Assert.That(client.ConnectionInfo.Timeout, Is.EqualTo(TimeSpan.FromSeconds(15)));

            // Cleanup
            client?.Dispose();
        }

        [Test]
        public void CreateConnection_SetsUtf8Encoding()
        {
            // Arrange
            string hostname = "localhost";
            int port = 22;
            string username = "testuser";
            var authMethods = new[] { new PasswordAuthenticationMethod("testuser", "password") };

            // Act
            var client = SSHConnectionManager.CreateConnection(hostname, port, username, authMethods);

            // Assert
            Assert.That(client, Is.Not.Null);
            Assert.That(client.ConnectionInfo.Encoding, Is.EqualTo(System.Text.Encoding.UTF8));

            // Cleanup
            client?.Dispose();
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
                SSHConnectionManager.Connect(client));
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
                SSHConnectionManager.CreateShellStream(client));
        }

        [Test]
        public void CreateShellStream_ThrowsInvalidOperationException_WhenClientNotConnected()
        {
            // Arrange
            string hostname = "localhost";
            int port = 22;
            string username = "testuser";
            var authMethods = new[] { new PasswordAuthenticationMethod("testuser", "password") };
            var client = SSHConnectionManager.CreateConnection(hostname, port, username, authMethods);

            try
            {
                // Act & Assert
                Assert.Throws<InvalidOperationException>(() =>
                    SSHConnectionManager.CreateShellStream(client));
            }
            finally
            {
                // Cleanup
                client?.Dispose();
            }
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
                SSHConnectionManager.ConfigureKeepAlive(client));
        }

        [Test]
        public void ConfigureKeepAlive_SetsDefaultInterval_WhenNotProvided()
        {
            // Arrange
            string hostname = "localhost";
            int port = 22;
            string username = "testuser";
            var authMethods = new[] { new PasswordAuthenticationMethod("testuser", "password") };
            var client = SSHConnectionManager.CreateConnection(hostname, port, username, authMethods);

            try
            {
                // Act
                SSHConnectionManager.ConfigureKeepAlive(client);

                // Assert
                Assert.That(client.KeepAliveInterval, Is.EqualTo(TimeSpan.FromSeconds(5)));
            }
            finally
            {
                // Cleanup
                client?.Dispose();
            }
        }

        [Test]
        public void ConfigureKeepAlive_SetsCustomInterval_WhenProvided()
        {
            // Arrange
            string hostname = "localhost";
            int port = 22;
            string username = "testuser";
            var authMethods = new[] { new PasswordAuthenticationMethod("testuser", "password") };
            var client = SSHConnectionManager.CreateConnection(hostname, port, username, authMethods);
            TimeSpan customInterval = TimeSpan.FromSeconds(10);

            try
            {
                // Act
                SSHConnectionManager.ConfigureKeepAlive(client, customInterval);

                // Assert
                Assert.That(client.KeepAliveInterval, Is.EqualTo(customInterval));
            }
            finally
            {
                // Cleanup
                client?.Dispose();
            }
        }

        #endregion

        #region GetConnectionInfo Tests

        [Test]
        public void GetConnectionInfo_ReturnsNullMessage_WhenClientIsNull()
        {
            // Arrange
            SshClient client = null;

            // Act
            string info = SSHConnectionManager.GetConnectionInfo(client);

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
            var authMethods = new[] { new PasswordAuthenticationMethod("testuser", "password") };
            var client = SSHConnectionManager.CreateConnection(hostname, port, username, authMethods);

            try
            {
                // Act
                string info = SSHConnectionManager.GetConnectionInfo(client);

                // Assert
                Assert.That(info, Is.EqualTo("Client is not connected"));
            }
            finally
            {
                // Cleanup
                client?.Dispose();
            }
        }

        // Note: Cannot test GetConnectionInfo with connected client without real SSH server
        // Integration tests would be needed for connected scenarios

        #endregion
    }
}
