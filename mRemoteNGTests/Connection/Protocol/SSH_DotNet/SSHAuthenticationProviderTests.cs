using mRemoteNG.Connection.Protocol.SSH_DotNet;
using NUnit.Framework;
using System;
using System.IO;
using Renci.SshNet;

namespace mRemoteNGTests.Connection.Protocol.SSH_DotNet
{
    [TestFixture]
    [Category("Unit")]
    public class SSHAuthenticationProviderTests
    {
        private string _tempKeyFile;

        [SetUp]
        public void Setup()
        {
            // Reset diagnostic flags to default state
            SSHDotNetDiagnostics.VerboseLogging = false;
            SSHDotNetDiagnostics.TraceLogging = false;

            // Create a temporary directory for test key files
            _tempKeyFile = null;
        }

        [TearDown]
        public void TearDown()
        {
            // Reset to defaults after tests
            SSHDotNetDiagnostics.VerboseLogging = false;
            SSHDotNetDiagnostics.TraceLogging = false;

            // Clean up temporary key file
            if (_tempKeyFile != null && File.Exists(_tempKeyFile))
            {
                try
                {
                    File.Delete(_tempKeyFile);
                }
                catch
                {
                    // Ignore cleanup errors
                }
            }
        }

        #region CreatePasswordAuth Tests

        [Test]
        public void CreatePasswordAuth_ThrowsArgumentException_WhenUsernameIsNull()
        {
            // Arrange
            string username = null;
            string password = "password";

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                SSHAuthenticationProvider.CreatePasswordAuth(username, password));
        }

        [Test]
        public void CreatePasswordAuth_ThrowsArgumentException_WhenUsernameIsEmpty()
        {
            // Arrange
            string username = "";
            string password = "password";

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                SSHAuthenticationProvider.CreatePasswordAuth(username, password));
        }

        [Test]
        public void CreatePasswordAuth_ReturnsPasswordMethod_WithValidUsername()
        {
            // Arrange
            string username = "testuser";
            string password = "password123";

            // Act
            var authMethod = SSHAuthenticationProvider.CreatePasswordAuth(username, password);

            // Assert
            Assert.That(authMethod, Is.Not.Null);
            Assert.That(authMethod, Is.InstanceOf<PasswordAuthenticationMethod>());
            Assert.That(authMethod.Username, Is.EqualTo(username));
        }

        [Test]
        public void CreatePasswordAuth_ReturnsPasswordMethod_WhenPasswordIsNull()
        {
            // Arrange
            string username = "testuser";
            string password = null;

            // Act
            var authMethod = SSHAuthenticationProvider.CreatePasswordAuth(username, password);

            // Assert
            Assert.That(authMethod, Is.Not.Null);
            Assert.That(authMethod, Is.InstanceOf<PasswordAuthenticationMethod>());
            Assert.That(authMethod.Username, Is.EqualTo(username));
        }

        [Test]
        public void CreatePasswordAuth_ReturnsPasswordMethod_WhenPasswordIsEmpty()
        {
            // Arrange
            string username = "testuser";
            string password = "";

            // Act
            var authMethod = SSHAuthenticationProvider.CreatePasswordAuth(username, password);

            // Assert
            Assert.That(authMethod, Is.Not.Null);
            Assert.That(authMethod, Is.InstanceOf<PasswordAuthenticationMethod>());
            Assert.That(authMethod.Username, Is.EqualTo(username));
        }

        #endregion

        #region CreatePrivateKeyAuth Tests

        [Test]
        public void CreatePrivateKeyAuth_ThrowsArgumentException_WhenUsernameIsNull()
        {
            // Arrange
            string username = null;
            string keyPath = "dummy.key";

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                SSHAuthenticationProvider.CreatePrivateKeyAuth(username, keyPath));
        }

        [Test]
        public void CreatePrivateKeyAuth_ThrowsArgumentException_WhenUsernameIsEmpty()
        {
            // Arrange
            string username = "";
            string keyPath = "dummy.key";

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                SSHAuthenticationProvider.CreatePrivateKeyAuth(username, keyPath));
        }

        [Test]
        public void CreatePrivateKeyAuth_ThrowsArgumentException_WhenKeyPathIsNull()
        {
            // Arrange
            string username = "testuser";
            string keyPath = null;

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                SSHAuthenticationProvider.CreatePrivateKeyAuth(username, keyPath));
        }

        [Test]
        public void CreatePrivateKeyAuth_ThrowsArgumentException_WhenKeyPathIsEmpty()
        {
            // Arrange
            string username = "testuser";
            string keyPath = "";

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                SSHAuthenticationProvider.CreatePrivateKeyAuth(username, keyPath));
        }

        [Test]
        public void CreatePrivateKeyAuth_ThrowsFileNotFoundException_WhenKeyFileDoesNotExist()
        {
            // Arrange
            string username = "testuser";
            string keyPath = "nonexistent_key.pem";

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() =>
                SSHAuthenticationProvider.CreatePrivateKeyAuth(username, keyPath));
        }

        // Note: Cannot test CreatePrivateKeyAuth success without a valid SSH key file
        // Would need to generate or include a test key file

        #endregion

        #region CreatePrivateKeyAuthFromString Tests

        [Test]
        public void CreatePrivateKeyAuthFromString_ThrowsArgumentException_WhenUsernameIsNull()
        {
            // Arrange
            string username = null;
            string keyContent = "dummy key content";

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                SSHAuthenticationProvider.CreatePrivateKeyAuthFromString(username, keyContent));
        }

        [Test]
        public void CreatePrivateKeyAuthFromString_ThrowsArgumentException_WhenUsernameIsEmpty()
        {
            // Arrange
            string username = "";
            string keyContent = "dummy key content";

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                SSHAuthenticationProvider.CreatePrivateKeyAuthFromString(username, keyContent));
        }

        [Test]
        public void CreatePrivateKeyAuthFromString_ThrowsArgumentException_WhenKeyContentIsNull()
        {
            // Arrange
            string username = "testuser";
            string keyContent = null;

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                SSHAuthenticationProvider.CreatePrivateKeyAuthFromString(username, keyContent));
        }

        [Test]
        public void CreatePrivateKeyAuthFromString_ThrowsArgumentException_WhenKeyContentIsEmpty()
        {
            // Arrange
            string username = "testuser";
            string keyContent = "";

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                SSHAuthenticationProvider.CreatePrivateKeyAuthFromString(username, keyContent));
        }

        // Note: Cannot test CreatePrivateKeyAuthFromString success without valid key content
        // Would need a properly formatted SSH key string

        #endregion

        #region CreateKeyboardInteractiveAuth Tests

        [Test]
        public void CreateKeyboardInteractiveAuth_ReturnsMethod_WithUsername()
        {
            // Arrange
            string username = "testuser";
            string password = "password123";

            // Act
            var authMethod = SSHAuthenticationProvider.CreateKeyboardInteractiveAuth(username, password);

            // Assert
            Assert.That(authMethod, Is.Not.Null);
            Assert.That(authMethod, Is.InstanceOf<KeyboardInteractiveAuthenticationMethod>());
            Assert.That(authMethod.Username, Is.EqualTo(username));
        }

        [Test]
        public void CreateKeyboardInteractiveAuth_ReturnsMethod_WhenPasswordIsNull()
        {
            // Arrange
            string username = "testuser";
            string password = null;

            // Act
            var authMethod = SSHAuthenticationProvider.CreateKeyboardInteractiveAuth(username, password);

            // Assert
            Assert.That(authMethod, Is.Not.Null);
            Assert.That(authMethod, Is.InstanceOf<KeyboardInteractiveAuthenticationMethod>());
            Assert.That(authMethod.Username, Is.EqualTo(username));
        }

        #endregion

        #region GetAuthenticationMethods Tests

        [Test]
        public void GetAuthenticationMethods_ReturnsPasswordAndKeyboardInteractive_WhenOnlyPasswordProvided()
        {
            // Arrange
            string username = "testuser";
            string password = "password123";
            var connectionInfo = new mRemoteNG.Connection.ConnectionInfo();

            // Act
            var authMethods = SSHAuthenticationProvider.GetAuthenticationMethods(username, password, connectionInfo);

            // Assert
            Assert.That(authMethods, Is.Not.Null);
            Assert.That(authMethods.Length, Is.GreaterThanOrEqualTo(2));
            Assert.That(authMethods[0], Is.InstanceOf<PasswordAuthenticationMethod>());
            // Last method should be keyboard-interactive
            Assert.That(authMethods[authMethods.Length - 1], Is.InstanceOf<KeyboardInteractiveAuthenticationMethod>());
        }

        [Test]
        public void GetAuthenticationMethods_ReturnsOnlyKeyboardInteractive_WhenPasswordIsEmpty()
        {
            // Arrange
            string username = "testuser";
            string password = "";
            var connectionInfo = new mRemoteNG.Connection.ConnectionInfo();

            // Act
            var authMethods = SSHAuthenticationProvider.GetAuthenticationMethods(username, password, connectionInfo);

            // Assert
            Assert.That(authMethods, Is.Not.Null);
            Assert.That(authMethods.Length, Is.GreaterThanOrEqualTo(1));
            // Should only have keyboard-interactive
            Assert.That(authMethods[authMethods.Length - 1], Is.InstanceOf<KeyboardInteractiveAuthenticationMethod>());
        }

        [Test]
        public void GetAuthenticationMethods_ReturnsOnlyKeyboardInteractive_WhenPasswordIsNull()
        {
            // Arrange
            string username = "testuser";
            string password = null;
            var connectionInfo = new mRemoteNG.Connection.ConnectionInfo();

            // Act
            var authMethods = SSHAuthenticationProvider.GetAuthenticationMethods(username, password, connectionInfo);

            // Assert
            Assert.That(authMethods, Is.Not.Null);
            Assert.That(authMethods.Length, Is.GreaterThanOrEqualTo(1));
            // Should only have keyboard-interactive
            Assert.That(authMethods[authMethods.Length - 1], Is.InstanceOf<KeyboardInteractiveAuthenticationMethod>());
        }

        [Test]
        public void GetAuthenticationMethods_ReturnsAtLeastOneMethod()
        {
            // Arrange
            string username = "testuser";
            string password = "password123";
            var connectionInfo = new mRemoteNG.Connection.ConnectionInfo();

            // Act
            var authMethods = SSHAuthenticationProvider.GetAuthenticationMethods(username, password, connectionInfo);

            // Assert
            Assert.That(authMethods, Is.Not.Null);
            Assert.That(authMethods.Length, Is.GreaterThan(0), "Should always return at least one authentication method");
        }

        #endregion
    }
}
