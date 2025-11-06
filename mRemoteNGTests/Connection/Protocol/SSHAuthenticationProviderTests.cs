using System;
using System.IO;
using System.Linq;
using System.Text;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.SSH_DotNet;
using NUnit.Framework;
using Renci.SshNet;
using ConnectionInfo = mRemoteNG.Connection.ConnectionInfo;

namespace mRemoteNGTests.Connection.Protocol;

public class SSHAuthenticationProviderTests
{
    [Test]
    public void CreatePasswordAuth_WithValidCredentials_ReturnsPasswordAuthenticationMethod()
    {
        // Arrange
        const string username = "testuser";
        const string password = "testpass123";

        // Act
        var authMethod = SSHAuthenticationProvider.CreatePasswordAuth(username, password);

        // Assert
        Assert.That(authMethod, Is.Not.Null);
        Assert.That(authMethod, Is.TypeOf<PasswordAuthenticationMethod>());
    }

    [Test]
    public void CreatePasswordAuth_WithEmptyUsername_ThrowsArgumentException()
    {
        // Arrange
        const string username = "";
        const string password = "testpass";

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            SSHAuthenticationProvider.CreatePasswordAuth(username, password));
    }

    [Test]
    public void CreatePasswordAuth_WithNullUsername_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            SSHAuthenticationProvider.CreatePasswordAuth(null, "password"));
    }

    [Test]
    public void CreatePasswordAuth_WithNullPassword_ReturnsValidAuthMethod()
    {
        // Arrange
        const string username = "testuser";

        // Act
        var authMethod = SSHAuthenticationProvider.CreatePasswordAuth(username, null);

        // Assert
        Assert.That(authMethod, Is.Not.Null);
        Assert.That(authMethod, Is.TypeOf<PasswordAuthenticationMethod>());
    }

    [Test]
    public void CreatePrivateKeyAuth_WithValidKeyFile_ReturnsPrivateKeyAuthenticationMethod()
    {
        // Arrange
        const string username = "testuser";
        const string privateKeyContent = "-----BEGIN OPENSSH PRIVATE KEY-----\nb3BlbnNzaC1rZXktdjEAAAAABG5vbmU" +
            "AAAAEbm9uZQAAAAAAAAABAAAAMwAAAAtzc2gtZWQyNTUxOQAAACD5cJn7qQx1z3Yni5" +
            "VyN7I/bkz5zVVBE0LiGfDnkR7kQwAAAJAtN5o3LTeaNwAAAAtzc2gtZWQyNTUxOQAA" +
            "ACD5cJn7qQx1z3YniVyN7I/bkz5zVVBE0LiGfDnkR7kQwAAAAECAwQFBgcICQ==\n" +
            "-----END OPENSSH PRIVATE KEY-----";

        var tempPath = Path.Combine(Path.GetTempPath(), "test_key_" + Guid.NewGuid() + ".pem");

        try
        {
            // Write test key file
            File.WriteAllText(tempPath, privateKeyContent);

            // Act
            var authMethod = SSHAuthenticationProvider.CreatePrivateKeyAuth(username, tempPath);

            // Assert
            Assert.That(authMethod, Is.Not.Null);
            Assert.That(authMethod, Is.TypeOf<PrivateKeyAuthenticationMethod>());
        }
        finally
        {
            // Cleanup
            if (File.Exists(tempPath))
                File.Delete(tempPath);
        }
    }

    [Test]
    public void CreatePrivateKeyAuth_WithNonexistentFile_ThrowsFileNotFoundException()
    {
        // Arrange
        const string username = "testuser";
        const string nonexistentPath = "/nonexistent/path/to/key.pem";

        // Act & Assert
        Assert.Throws<FileNotFoundException>(() =>
            SSHAuthenticationProvider.CreatePrivateKeyAuth(username, nonexistentPath));
    }

    [Test]
    public void CreatePrivateKeyAuth_WithEmptyUsername_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            SSHAuthenticationProvider.CreatePrivateKeyAuth("", "/some/path/key.pem"));
    }

    [Test]
    public void CreatePrivateKeyAuth_WithEmptyKeyPath_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            SSHAuthenticationProvider.CreatePrivateKeyAuth("testuser", ""));
    }

    [Test]
    public void CreatePrivateKeyAuthFromString_WithValidKeyContent_ReturnsPrivateKeyAuthenticationMethod()
    {
        // Arrange
        const string username = "testuser";
        const string privateKeyContent = "-----BEGIN OPENSSH PRIVATE KEY-----\nb3BlbnNzaC1rZXktdjEAAAAABG5vbmU" +
            "AAAAEbm9uZQAAAAAAAAABAAAAMwAAAAtzc2gtZWQyNTUxOQAAACD5cJn7qQx1z3Yni5" +
            "VyN7I/bkz5zVVBE0LiGfDnkR7kQwAAAJAtN5o3LTeaNwAAAAtzc2gtZWQyNTUxOQAA" +
            "ACD5cJn7qQx1z3YniVyN7I/bkz5zVVBE0LiGfDnkR7kQwAAAAECAwQFBgcICQ==\n" +
            "-----END OPENSSH PRIVATE KEY-----";

        // Act
        var authMethod = SSHAuthenticationProvider.CreatePrivateKeyAuthFromString(username, privateKeyContent);

        // Assert
        Assert.That(authMethod, Is.Not.Null);
        Assert.That(authMethod, Is.TypeOf<PrivateKeyAuthenticationMethod>());
    }

    [Test]
    public void CreatePrivateKeyAuthFromString_WithEmptyUsername_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            SSHAuthenticationProvider.CreatePrivateKeyAuthFromString("", "some key content"));
    }

    [Test]
    public void CreatePrivateKeyAuthFromString_WithEmptyKeyContent_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            SSHAuthenticationProvider.CreatePrivateKeyAuthFromString("testuser", ""));
    }

    [Test]
    public void CreateKeyboardInteractiveAuth_WithValidUsername_ReturnsKeyboardInteractiveAuthenticationMethod()
    {
        // Arrange
        const string username = "testuser";
        const string password = "testpass";

        // Act
        var authMethod = SSHAuthenticationProvider.CreateKeyboardInteractiveAuth(username, password);

        // Assert
        Assert.That(authMethod, Is.Not.Null);
        Assert.That(authMethod, Is.TypeOf<KeyboardInteractiveAuthenticationMethod>());
    }

    [Test]
    public void CreateKeyboardInteractiveAuth_WithNullPassword_ReturnsValidAuthMethod()
    {
        // Arrange
        const string username = "testuser";

        // Act
        var authMethod = SSHAuthenticationProvider.CreateKeyboardInteractiveAuth(username, null);

        // Assert
        Assert.That(authMethod, Is.Not.Null);
        Assert.That(authMethod, Is.TypeOf<KeyboardInteractiveAuthenticationMethod>());
    }

    [Test]
    public void GetAuthenticationMethods_WithPasswordOnly_ReturnsArrayWithPasswordMethod()
    {
        // Arrange
        const string username = "testuser";
        const string password = "testpass123";
        var connectionInfo = new ConnectionInfo();

        // Act
        var authMethods = SSHAuthenticationProvider.GetAuthenticationMethods(username, password, connectionInfo);

        // Assert
        Assert.That(authMethods, Is.Not.Null);
        Assert.That(authMethods.Length, Is.GreaterThan(0));
        Assert.That(authMethods[0], Is.TypeOf<PasswordAuthenticationMethod>());
    }

    [Test]
    public void GetAuthenticationMethods_WithEmptyPassword_ReturnsArrayWithoutPasswordMethod()
    {
        // Arrange
        const string username = "testuser";
        const string password = "";
        var connectionInfo = new ConnectionInfo();

        // Act
        var authMethods = SSHAuthenticationProvider.GetAuthenticationMethods(username, password, connectionInfo);

        // Assert
        Assert.That(authMethods, Is.Not.Null);
        Assert.That(authMethods.Length, Is.GreaterThan(0));
        // First method should be KeyboardInteractive (since no password provided)
        Assert.That(authMethods.Length, Is.EqualTo(1));
        Assert.That(authMethods[0], Is.TypeOf<KeyboardInteractiveAuthenticationMethod>());
    }

    [Test]
    public void GetAuthenticationMethods_AlwaysIncludesKeyboardInteractive()
    {
        // Arrange
        const string username = "testuser";
        const string password = "testpass";
        var connectionInfo = new ConnectionInfo();

        // Act
        var authMethods = SSHAuthenticationProvider.GetAuthenticationMethods(username, password, connectionInfo);

        // Assert
        Assert.That(authMethods, Is.Not.Null);
        var hasKeyboardInteractive = authMethods.Any(m => m is KeyboardInteractiveAuthenticationMethod);
        Assert.That(hasKeyboardInteractive, Is.True);
    }

    [Test]
    public void GetAuthenticationMethods_WithNullConnectionInfo_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            SSHAuthenticationProvider.GetAuthenticationMethods("user", "pass", null));
    }
}
