using System;
using System.Linq;
using mRemoteNG.Connection.Protocol.SSH_DotNet;
using NUnit.Framework;
using Renci.SshNet;

namespace mRemoteNGTests.Connection.Protocol;

public class SSHConnectionManagerTests
{
    [Test]
    public void CreateConnection_WithValidParameters_ReturnsSshClient()
    {
        // Arrange
        const string hostname = "localhost";
        const int port = 22;
        const string username = "testuser";
        using var authMethod = new PasswordAuthenticationMethod(username, "password");
        var authMethods = new AuthenticationMethod[] { authMethod };

        // Act
        using var client = SSHConnectionManager.CreateConnection(hostname, port, username, authMethods);

        // Assert
        Assert.That(client, Is.Not.Null);
        Assert.That(client, Is.TypeOf<SshClient>());
        Assert.That(client.ConnectionInfo.Host, Is.EqualTo(hostname));
        Assert.That(client.ConnectionInfo.Port, Is.EqualTo(port));
        Assert.That(client.ConnectionInfo.Username, Is.EqualTo(username));
    }

    [Test]
    public void CreateConnection_WithCustomTimeout_SetsSshClientTimeout()
    {
        // Arrange
        const string hostname = "localhost";
        const int port = 22;
        const string username = "testuser";
        using var authMethod = new PasswordAuthenticationMethod(username, "password");
        var authMethods = new AuthenticationMethod[] { authMethod };
        var customTimeout = TimeSpan.FromSeconds(60);

        // Act
        using var client = SSHConnectionManager.CreateConnection(hostname, port, username, authMethods, customTimeout);

        // Assert
        Assert.That(client.ConnectionInfo.Timeout, Is.EqualTo(customTimeout));
    }

    [Test]
    public void CreateConnection_WithEmptyHostname_ThrowsArgumentException()
    {
        // Arrange
        const string hostname = "";
        const int port = 22;
        const string username = "testuser";
        using var authMethod = new PasswordAuthenticationMethod(username, "password");
        var authMethods = new AuthenticationMethod[] { authMethod };

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            SSHConnectionManager.CreateConnection(hostname, port, username, authMethods));
    }

    [Test]
    public void CreateConnection_WithInvalidPort_ThrowsArgumentOutOfRangeException()
    {
        // Arrange & Act & Assert
        using var authMethod1 = new PasswordAuthenticationMethod("user", "pass");
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            SSHConnectionManager.CreateConnection("localhost", 0, "user",
                new[] { authMethod1 }));

        using var authMethod2 = new PasswordAuthenticationMethod("user", "pass");
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            SSHConnectionManager.CreateConnection("localhost", 65536, "user",
                new[] { authMethod2 }));
    }

    [Test]
    public void CreateConnection_WithEmptyUsername_ThrowsArgumentException()
    {
        // Act & Assert
        using var authMethod = new PasswordAuthenticationMethod("", "pass");
        Assert.Throws<ArgumentException>(() =>
            SSHConnectionManager.CreateConnection("localhost", 22, "",
                new[] { authMethod }));
    }

    [Test]
    public void CreateConnection_WithNullAuthMethods_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            SSHConnectionManager.CreateConnection("localhost", 22, "user", null));
    }

    [Test]
    public void CreateConnection_WithEmptyAuthMethods_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            SSHConnectionManager.CreateConnection("localhost", 22, "user", Array.Empty<AuthenticationMethod>()));
    }

    [Test]
    public void CreateConnection_WithValidPortRange_Succeeds()
    {
        // Arrange
        const string username = "testuser";
        using var authMethod = new PasswordAuthenticationMethod(username, "password");
        var authMethods = new AuthenticationMethod[] { authMethod };

        // Act & Assert - Test minimum valid port
        using var client1 = SSHConnectionManager.CreateConnection("localhost", 1, username, authMethods);
        Assert.That(client1.ConnectionInfo.Port, Is.EqualTo(1));

        // Act & Assert - Test maximum valid port
        using var client2 = SSHConnectionManager.CreateConnection("localhost", 65535, username, authMethods);
        Assert.That(client2.ConnectionInfo.Port, Is.EqualTo(65535));
    }

    [Test]
    public void CreateConnection_UsesUtf8Encoding()
    {
        // Arrange
        const string hostname = "localhost";
        const int port = 22;
        const string username = "testuser";
        using var authMethod = new PasswordAuthenticationMethod(username, "password");
        var authMethods = new AuthenticationMethod[] { authMethod };

        // Act
        using var client = SSHConnectionManager.CreateConnection(hostname, port, username, authMethods);

        // Assert
        Assert.That(client.ConnectionInfo.Encoding, Is.EqualTo(System.Text.Encoding.UTF8));
    }

    [Test]
    public void ConfigureKeepAlive_WithValidClient_SetsKeepAliveInterval()
    {
        // Arrange
        const string hostname = "localhost";
        const int port = 22;
        const string username = "testuser";
        using var authMethod = new PasswordAuthenticationMethod(username, "password");
        var authMethods = new AuthenticationMethod[] { authMethod };
        using var client = SSHConnectionManager.CreateConnection(hostname, port, username, authMethods);
        var keepAliveInterval = TimeSpan.FromSeconds(60);

        // Act
        SSHConnectionManager.ConfigureKeepAlive(client, keepAliveInterval);

        // Assert
        Assert.That(client.KeepAliveInterval, Is.EqualTo(keepAliveInterval));
    }

    [Test]
    public void ConfigureKeepAlive_WithNullClient_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            SSHConnectionManager.ConfigureKeepAlive(null, TimeSpan.FromSeconds(30)));
    }

    [Test]
    public void ConfigureKeepAlive_WithDefaultInterval_UsesValidInterval()
    {
        // Arrange
        const string hostname = "localhost";
        const int port = 22;
        const string username = "testuser";
        using var authMethod = new PasswordAuthenticationMethod(username, "password");
        var authMethods = new AuthenticationMethod[] { authMethod };
        using var client = SSHConnectionManager.CreateConnection(hostname, port, username, authMethods);

        // Act
        SSHConnectionManager.ConfigureKeepAlive(client); // Should use default interval

        // Assert
        Assert.That(client.KeepAliveInterval, Is.Not.EqualTo(TimeSpan.Zero));
        Assert.That(client.KeepAliveInterval.TotalSeconds, Is.GreaterThan(0));
    }

    [Test]
    public void GetConnectionInfo_WithNullClient_ReturnsErrorMessage()
    {
        // Act
        var info = SSHConnectionManager.GetConnectionInfo(null);

        // Assert
        Assert.That(info, Is.Not.Null);
        Assert.That(info, Contains.Substring("null"));
    }

    [Test]
    public void GetConnectionInfo_WithDisconnectedClient_ReturnsNotConnectedMessage()
    {
        // Arrange
        const string hostname = "localhost";
        const int port = 22;
        const string username = "testuser";
        using var authMethod = new PasswordAuthenticationMethod(username, "password");
        var authMethods = new AuthenticationMethod[] { authMethod };
        using var client = SSHConnectionManager.CreateConnection(hostname, port, username, authMethods);

        // Act
        var info = SSHConnectionManager.GetConnectionInfo(client);

        // Assert
        Assert.That(info, Is.Not.Null);
        Assert.That(info, Contains.Substring("not connected"));
    }

    [Test]
    public void CreateConnection_AllAuthMethodsHaveCorrectUsername()
    {
        // Arrange
        const string hostname = "localhost";
        const int port = 22;
        const string username = "testuser";
        using var authMethod1 = new PasswordAuthenticationMethod(username, "password");
        using var authMethod2 = new KeyboardInteractiveAuthenticationMethod(username);
        var authMethods = new AuthenticationMethod[] { authMethod1, authMethod2 };

        // Act
        using var client = SSHConnectionManager.CreateConnection(hostname, port, username, authMethods);

        // Assert
        Assert.That(client.ConnectionInfo.AuthenticationMethods, Is.Not.Null);
        Assert.That(client.ConnectionInfo.AuthenticationMethods.Count, Is.EqualTo(2));
    }

    [Test]
    public void CreateConnection_WithMultipleAuthMethods_PreservesOrder()
    {
        // Arrange
        const string hostname = "localhost";
        const int port = 22;
        const string username = "testuser";
        using var authMethod1 = new PasswordAuthenticationMethod(username, "password");
        using var authMethod2 = new KeyboardInteractiveAuthenticationMethod(username);
        var authMethods = new AuthenticationMethod[] { authMethod1, authMethod2 };

        // Act
        using var client = SSHConnectionManager.CreateConnection(hostname, port, username, authMethods);

        // Assert
        Assert.That(client.ConnectionInfo.AuthenticationMethods[0], Is.TypeOf<PasswordAuthenticationMethod>());
        Assert.That(client.ConnectionInfo.AuthenticationMethods[1], Is.TypeOf<KeyboardInteractiveAuthenticationMethod>());
    }
}
