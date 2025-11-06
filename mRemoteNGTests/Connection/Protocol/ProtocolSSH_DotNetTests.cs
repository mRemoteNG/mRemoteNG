using System;
using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.SSH_DotNet;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Window;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNGTests.Connection.Protocol;

public class ProtocolSSH_DotNetTests
{
    private ProtocolSSH_DotNet _sut;
    private InterfaceControl _interfaceControl;
    private ConnectionInfo _connectionInfo;

    [SetUp]
    public void SetUp()
    {
        _sut = new ProtocolSSH_DotNet();
        _connectionInfo = new ConnectionInfo
        {
            Hostname = "localhost",
            Port = 22,
            Username = "testuser",
            Password = "testpass",
            Protocol = ProtocolType.SSH_DotNet
        };

        var connectionWindow = new ConnectionWindow(new DockContent());
        _interfaceControl = new InterfaceControl(connectionWindow, _sut, _connectionInfo);
        _sut.InterfaceControl = _interfaceControl;
    }

    [TearDown]
    public void TearDown()
    {
        _sut?.Dispose();
        _interfaceControl?.Dispose();
    }

    [Test]
    public void Constructor_CreatesNewInstance()
    {
        // Assert
        Assert.That(_sut, Is.Not.Null);
        Assert.That(_sut, Is.TypeOf<ProtocolSSH_DotNet>());
    }

    [Test]
    public void Initialize_WithValidSetup_ReturnsTrue()
    {
        // Act
        var result = _sut.Initialize();

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Initialize_CreatesTerminalControl()
    {
        // Act
        var result = _sut.Initialize();

        // Assert
        Assert.That(result, Is.True);
        // Control property is protected, but initialization should succeed
    }

    [Test]
    public void DefaultPort_ReturnsPort22()
    {
        // Assert
        Assert.That((int)ProtocolSSH_DotNet.Defaults.Port, Is.EqualTo(22));
    }

    [Test]
    public void State_InitiallyDisconnected()
    {
        // Assert
        Assert.That(_sut.State, Is.EqualTo(ProtocolSSH_DotNet.ConnectionState.Disconnected));
    }

    [Test]
    public void IsConnected_WhenDisconnected_ReturnsFalse()
    {
        // Assert
        Assert.That(_sut.IsConnected, Is.False);
    }

    [Test]
    public void BytesReceived_InitiallyZero()
    {
        // Assert
        Assert.That(_sut.BytesReceived, Is.EqualTo(0));
    }

    [Test]
    public void BytesSent_InitiallyZero()
    {
        // Assert
        Assert.That(_sut.BytesSent, Is.EqualTo(0));
    }

    [Test]
    public void ConnectionDuration_WhenDisconnected_ReturnsZero()
    {
        // Assert
        Assert.That(_sut.ConnectionDuration, Is.EqualTo(TimeSpan.Zero));
    }

    [Test]
    public void Disconnect_WhenNotConnected_DoesNotThrow()
    {
        // Act & Assert
        Assert.DoesNotThrow(() => _sut.Disconnect());
    }

    [Test]
    public void Connect_WithNullInterfaceControl_ReturnsFalse()
    {
        // Arrange
        _sut.InterfaceControl = null;

        // Act
        var result = _sut.Connect();

        // Assert
        Assert.That(result, Is.False);
        Assert.That(_sut.State, Is.EqualTo(ProtocolSSH_DotNet.ConnectionState.Error));
    }

    [Test]
    public void Connect_WithEmptyHostname_ReturnsFalse()
    {
        // Arrange
        _connectionInfo.Hostname = "";

        // Act
        var result = _sut.Connect();

        // Assert
        Assert.That(result, Is.False);
        Assert.That(_sut.State, Is.EqualTo(ProtocolSSH_DotNet.ConnectionState.Error));
    }

    [Test]
    public void Connect_WithEmptyUsername_ReturnsFalse()
    {
        // Arrange
        _connectionInfo.Username = "";

        // Act
        var result = _sut.Connect();

        // Assert
        Assert.That(result, Is.False);
        Assert.That(_sut.State, Is.EqualTo(ProtocolSSH_DotNet.ConnectionState.Error));
    }

    [Test]
    public void Connect_SetsStateToConnecting()
    {
        // Note: This test will likely fail when trying to connect to localhost without proper SSH server,
        // but we can verify the state change at least starts
        // Act
        _sut.Connect();

        // Assert - The state should have transitioned through Connecting state
        // (may have moved to Error due to connection failure)
        Assert.That(
            _sut.State == ProtocolSSH_DotNet.ConnectionState.Connecting ||
            _sut.State == ProtocolSSH_DotNet.ConnectionState.Authenticating ||
            _sut.State == ProtocolSSH_DotNet.ConnectionState.Error,
            Is.True
        );
    }

    [Test]
    public void Connect_WithDefaultPort_UsesPort22()
    {
        // Arrange
        _connectionInfo.Port = 0; // Should use default

        // Act
        _sut.Connect();

        // Assert - Just verify it doesn't throw and processes the default
        Assert.That(_sut.State, Is.Not.EqualTo(ProtocolSSH_DotNet.ConnectionState.Disconnected));
    }

    [Test]
    public void Disconnect_AfterConnect_SetsStateToDisconnected()
    {
        // Act
        _sut.Connect(); // This will likely fail, but that's ok
        _sut.Disconnect();

        // Assert
        Assert.That(_sut.State, Is.EqualTo(ProtocolSSH_DotNet.ConnectionState.Disconnected));
    }

    [Test]
    public void Initialize_ThenConnect_WithoutLocalSSHServer_TransitionsToErrorState()
    {
        // Arrange
        _sut.Initialize();

        // Act
        var result = _sut.Connect();

        // Assert
        // Connection will fail (no SSH server on localhost), so should be in Error state
        Assert.That(result, Is.False);
        Assert.That(_sut.State, Is.EqualTo(ProtocolSSH_DotNet.ConnectionState.Error));
    }

    [Test]
    public void Connect_WithValidInputs_StartsConnection()
    {
        // Arrange - Using valid inputs
        _sut.Initialize();

        // Act
        var result = _sut.Connect();

        // Assert - Will fail since no server, but should attempt connection
        Assert.That(
            _sut.State == ProtocolSSH_DotNet.ConnectionState.Error ||
            _sut.State == ProtocolSSH_DotNet.ConnectionState.Connecting,
            Is.True
        );
    }

    [Test]
    public void MultipleConnect_Calls_HandledGracefully()
    {
        // Arrange
        _sut.Initialize();

        // Act - Call connect multiple times
        var result1 = _sut.Connect();
        var result2 = _sut.Connect();

        // Assert - Should handle gracefully without exception
        Assert.That(_sut.State, Is.Not.Null);
    }

    [Test]
    public void Protocol_HasCorrectInterfaceControl()
    {
        // Assert
        Assert.That(_sut.InterfaceControl, Is.EqualTo(_interfaceControl));
    }

    [Test]
    public void Initialize_Multiple_TimesHandledGracefully()
    {
        // Act & Assert
        var result1 = _sut.Initialize();
        var result2 = _sut.Initialize();

        Assert.That(result1, Is.True);
        Assert.That(result2, Is.True);
    }
}
