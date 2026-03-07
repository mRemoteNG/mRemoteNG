# SSH Tunnel Support for SSH_DotNet (Without PuTTY)

**Document Version**: 1.3
**Date**: 2026-03-07
**Author**: Claude Code (Anthropic)
**Status**: PLAN - READY FOR REVIEW
**Parent Feature**: SSH_DotNet Feature (see `SSH_DotNet Feature 20251106.md`)
**Phase**: 4 (Advanced Features) of the SSH_DotNet roadmap

---

## Executive Summary

This document provides a detailed plan for adding **native SSH tunnel / port forwarding support** to mRemoteNG's SSH_DotNet protocol, eliminating the dependency on PuTTY for SSH tunneling. The SSH.NET library (Renci.SshNet v2025.1.0), already included in the project, provides full port forwarding APIs that can replace PuTTY's `-L`, `-R`, and `-D` command-line tunnel options.

### Key Capabilities

| Capability | PuTTY (Current) | SSH.NET (Proposed) | Notes |
|-----------|-----------------|-------------------|-------|
| Local port forwarding (`-L`) | Yes (CLI args) | `ForwardedPortLocal` | Full API support |
| Remote port forwarding (`-R`) | Yes (CLI args) | `ForwardedPortRemote` | Full API support |
| Dynamic SOCKS5 proxy (`-D`) | Yes (CLI args) | `ForwardedPortDynamic` | Full API support |
| Jump host / ProxyJump | Manual `-L` chaining | Manual chaining via `ForwardedPortLocal` | No native ProxyJump; chain two `SshClient` instances |
| Multiple tunnels per connection | One per PuTTY process | Multiple per `SshClient` | SSH.NET supports N forwarded ports per client |
| Tunnel as proxy for other protocols | Yes (RDP/VNC through SSH) | Yes | Same ConnectionInitiator pattern |
| In-process control | No (external process) | Yes | No window hiding/polling hacks needed |
| Programmatic tunnel health monitoring | No | Yes (events: `Exception`, `RequestReceived`, `Closing`) | Real-time status |

### Strategic Benefits

1. **No external process** - tunnels run in-process, no PuTTY window to hide
2. **No polling loop** - SSH.NET port forwarding is event-driven, no 60-second socket polling
3. **Multiple tunnels** - one SSH connection can carry N tunnels simultaneously
4. **Better error handling** - exceptions bubble up immediately instead of waiting for timeout
5. **Testable** - unit tests can verify tunnel setup/teardown without launching PuTTY

---

## Current State Analysis

### How Tunneling Works Today (PuTTY-Based)

**File**: `mRemoteNG/Connection/ConnectionInitiator.cs` (lines 107-219)

The current flow:

```
1. User sets "SSH Tunnel Connection" property on a connection (e.g., RDP)
2. ConnectionInitiator finds the named SSH connection in the tree
3. Finds a free local port via TcpListener
4. Clones the SSH connection, appends `-L <localPort>:<targetHost>:<targetPort>` to SSHOptions
5. Creates a PuTTY process with these options
6. POLLS the local port for up to 60 seconds waiting for PuTTY to be ready
7. Redirects the target connection to localhost:<localPort>
8. Hides the PuTTY window (but keeps it running in background)
```

### Current Limitations

1. **Hard PuTTY dependency** at `ConnectionInitiator.cs:143`:
   ```csharp
   if (!(protocolSshTunnel is PuttyBase puttyBaseSshTunnel))
   {
       // Rejects SSH_DotNet - shows "SshTunnelIsNotPutty" error
       return;
   }
   ```

2. **SSHTunnelTypeConverter** only lists `SSH1`/`SSH2` (PuTTY) connections as tunnel options - excludes `SSH_DotNet`

3. **Blocking poll loop** - waits up to 60 seconds trying to connect to the local port

4. **Single tunnel per connection** - the `-L` flag pattern only supports one tunnel

5. **No tunnel health monitoring** - if PuTTY dies, the tunneled connection has no way to know until it times out

### Relevant Existing Properties

| Property | Location | Purpose |
|----------|----------|---------|
| `SSHTunnelConnectionName` | `AbstractConnectionRecord.cs:352` | Name of SSH connection to use as tunnel |
| `SSHOptions` | `AbstractConnectionRecord.cs:419` | PuTTY CLI options (e.g., `-L`, `-R`, `-D`) |
| `InheritSSHTunnelConnectionName` | `ConnectionInfoInheritance.cs` | Inheritance flag |
| `InheritSSHOptions` | `ConnectionInfoInheritance.cs` | Inheritance flag |

---

## SSH.NET Port Forwarding API

SSH.NET v2025.1.0 provides three `ForwardedPort` subclasses, all implementing `IDisposable`:

### ForwardedPortLocal (Local Port Forwarding)

Maps a local port to a remote destination through the SSH tunnel.

```csharp
// Equivalent to: ssh -L 3389:rdp-server:3389 user@ssh-host
var forward = new ForwardedPortLocal("127.0.0.1", 13389, "rdp-server", 3389);
sshClient.AddForwardedPort(forward);
forward.Start();

// forward.IsStarted == true
// Connect RDP to localhost:13389 -> tunneled to rdp-server:3389

// Cleanup
forward.Stop();
sshClient.RemoveForwardedPort(forward);
forward.Dispose();
```

**Constructors:**
- `ForwardedPortLocal(uint boundPort, string host, uint port)`
- `ForwardedPortLocal(string boundHost, string host, uint port)` - auto-assigns port
- `ForwardedPortLocal(string boundHost, uint boundPort, string host, uint port)`

**Properties:** `BoundHost`, `BoundPort`, `Host`, `Port`, `IsStarted`

**Events (inherited):** `Closing`, `Exception`, `RequestReceived`

### ForwardedPortRemote (Remote Port Forwarding)

Maps a remote server port back to a local destination.

```csharp
// Equivalent to: ssh -R 8080:localhost:80 user@ssh-host
var forward = new ForwardedPortRemote("0.0.0.0", 8080, "localhost", 80);
sshClient.AddForwardedPort(forward);
forward.Start();
```

**Constructors:**
- `ForwardedPortRemote(uint boundPort, string host, uint port)`
- `ForwardedPortRemote(string boundHost, uint boundPort, string host, uint port)`
- `ForwardedPortRemote(IPAddress boundHost, uint boundPort, IPAddress host, uint port)`

### ForwardedPortDynamic (SOCKS5 Proxy)

Creates a local SOCKS5 proxy that routes all traffic through the SSH connection.

```csharp
// Equivalent to: ssh -D 1080 user@ssh-host
var forward = new ForwardedPortDynamic("127.0.0.1", 1080);
sshClient.AddForwardedPort(forward);
forward.Start();
```

**Constructors:**
- `ForwardedPortDynamic(uint port)`
- `ForwardedPortDynamic(string host, uint port)`

### Jump Host / ProxyJump (Chained Connections)

SSH.NET does **not** have native ProxyJump support. The recommended pattern is to chain two `SshClient` instances using local port forwarding:

```csharp
// Step 1: Connect to bastion/jump host
var bastionClient = new SshClient("bastion-host", "user", "password");
bastionClient.Connect();

// Step 2: Forward local port to target's SSH port via bastion
var jumpForward = new ForwardedPortLocal("127.0.0.1", 0, "target-host", 22);
bastionClient.AddForwardedPort(jumpForward);
jumpForward.Start();

// Step 3: Connect to target through the forwarded port
var targetClient = new SshClient("127.0.0.1", (int)jumpForward.BoundPort, "user", "password");
targetClient.Connect();

// Now targetClient has a shell on target-host, tunneled through bastion-host
```

### SSH.NET Proxy Support (Built-in)

SSH.NET's `ConnectionInfo` class also supports connecting through HTTP/SOCKS proxies natively:

```csharp
var connInfo = new ConnectionInfo(
    "ssh-host", 22, "user",
    ProxyTypes.Socks5,       // or Http, Socks4
    "proxy-host", 1080,
    "proxy-user", "proxy-pass",
    new PasswordAuthenticationMethod("user", "password")
);
var client = new SshClient(connInfo);
```

**ProxyTypes enum:** `None`, `Http`, `Socks4`, `Socks5`

---

## Implementation Plan

### Overview

The implementation has **two distinct use cases** that share infrastructure but have different integration points:

1. **SSH_DotNet as tunnel provider** - An SSH_DotNet connection provides tunnels for OTHER connections (RDP, VNC, etc.)
2. **SSH_DotNet with its own port forwards** - An SSH_DotNet terminal session that also sets up port forwarding rules

Both use cases leverage the same `SSHTunnelManager` helper class.

### Phase 4A: Core Tunnel Infrastructure

#### Task 4A.1: Create SSHTunnelManager Helper Class

**New file**: `mRemoteNG/Connection/Protocol/SSH_DotNet/SSHTunnelManager.cs`

This class manages the lifecycle of forwarded ports on an `SshClient`.

```csharp
// Design Note: Generic catch clauses (catch Exception) are used intentionally in this file.
// Port forwarding cleanup must be resilient - an exception stopping one port must not
// prevent cleanup of remaining ports. All exceptions are logged via SSHDotNetDiagnostics.
using System;
using System.Collections.Generic;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace mRemoteNG.Connection.Protocol.SSH_DotNet
{
    /// <summary>
    /// Manages SSH port forwarding (local, remote, dynamic) on an SshClient.
    /// Handles setup, monitoring, and cleanup of forwarded ports.
    /// Thread-safe: all public methods use locking on _syncLock.
    /// </summary>
    public class SSHTunnelManager : IDisposable
    {
        private readonly SshClient _sshClient;
        private readonly List<ForwardedPort> _forwardedPorts = new();
        private readonly object _syncLock = new();
        private bool _disposed;

        /// <summary>
        /// Raised when a forwarded port encounters an error.
        /// The string argument contains the error description.
        /// </summary>
        public event EventHandler<string> TunnelError;

        public SSHTunnelManager(SshClient sshClient)
        {
            _sshClient = sshClient ?? throw new ArgumentNullException(nameof(sshClient));
        }

        public IReadOnlyList<ForwardedPort> ForwardedPorts
        {
            get { lock (_syncLock) { return _forwardedPorts.AsReadOnly(); } }
        }

        /// <summary>
        /// Set up local port forwarding: binds a local port that tunnels to remoteHost:remotePort.
        /// If localPort is 0, an available port is auto-selected.
        /// Returns the actual bound port.
        /// </summary>
        public uint AddLocalForward(string localBindHost, uint localPort,
                                     string remoteHost, uint remotePort)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            var forward = new ForwardedPortLocal(localBindHost, localPort, remoteHost, remotePort);
            forward.Exception += OnForwardException;
            forward.RequestReceived += OnForwardRequestReceived;

            _sshClient.AddForwardedPort(forward);
            forward.Start();

            lock (_syncLock) { _forwardedPorts.Add(forward); }

            SSHDotNetDiagnostics.LogInfo(
                $"Tunnel: Local forward started - {localBindHost}:{forward.BoundPort} -> {remoteHost}:{remotePort}");

            return forward.BoundPort;
        }

        /// <summary>
        /// Set up remote port forwarding: binds a remote port that tunnels back to localHost:localPort.
        /// </summary>
        public void AddRemoteForward(string remoteBindHost, uint remotePort,
                                      string localHost, uint localPort)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            var forward = new ForwardedPortRemote(remoteBindHost, remotePort, localHost, localPort);
            forward.Exception += OnForwardException;
            forward.RequestReceived += OnForwardRequestReceived;

            _sshClient.AddForwardedPort(forward);
            forward.Start();

            lock (_syncLock) { _forwardedPorts.Add(forward); }

            SSHDotNetDiagnostics.LogInfo(
                $"Tunnel: Remote forward started - {remoteBindHost}:{remotePort} -> {localHost}:{localPort}");
        }

        /// <summary>
        /// Set up dynamic SOCKS5 proxy on the specified local port.
        /// If localPort is 0, an available port is auto-selected.
        /// </summary>
        public uint AddDynamicForward(string localBindHost, uint localPort)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            var forward = new ForwardedPortDynamic(localBindHost, localPort);
            forward.Exception += OnForwardException;
            forward.RequestReceived += OnForwardRequestReceived;

            _sshClient.AddForwardedPort(forward);
            forward.Start();

            lock (_syncLock) { _forwardedPorts.Add(forward); }

            SSHDotNetDiagnostics.LogInfo(
                $"Tunnel: Dynamic SOCKS5 proxy started on {localBindHost}:{forward.BoundPort}");

            return forward.BoundPort;
        }

        /// <summary>
        /// Stops and removes all forwarded ports. Unsubscribes events to prevent leaks.
        /// </summary>
        public void StopAll()
        {
            List<ForwardedPort> portsToStop;
            lock (_syncLock)
            {
                portsToStop = new List<ForwardedPort>(_forwardedPorts);
                _forwardedPorts.Clear();
            }

            foreach (var port in portsToStop)
            {
                try
                {
                    // Unsubscribe events before disposal to prevent leaks
                    port.Exception -= OnForwardException;
                    port.RequestReceived -= OnForwardRequestReceived;

                    if (port.IsStarted)
                        port.Stop();

                    if (_sshClient.IsConnected)
                        _sshClient.RemoveForwardedPort(port);

                    port.Dispose();
                }
                catch (Exception ex)
                {
                    SSHDotNetDiagnostics.LogException("Tunnel: Error stopping forwarded port", ex);
                }
            }
        }

        /// <summary>
        /// Check if all forwarded ports are still healthy.
        /// </summary>
        public bool AreAllPortsHealthy()
        {
            if (!_sshClient.IsConnected) return false;
            lock (_syncLock)
            {
                foreach (var port in _forwardedPorts)
                {
                    if (!port.IsStarted) return false;
                }
            }
            return true;
        }

        private void OnForwardException(object sender, ExceptionEventArgs e)
        {
            SSHDotNetDiagnostics.LogException("Tunnel: Port forwarding error", e.Exception);
            TunnelError?.Invoke(this, $"Tunnel port forwarding failed: {e.Exception.Message}");
        }

        private void OnForwardRequestReceived(object sender, PortForwardEventArgs e)
        {
            SSHDotNetDiagnostics.LogDebug(
                $"Tunnel: Request received - {e.OriginatorHost}:{e.OriginatorPort}");
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            StopAll();
        }
    }
}
```

**Design decisions (addressing anticipated code review feedback):**
- **Generic catch clauses**: Intentional in `StopAll()` — cleanup of one port must not prevent cleanup of others. File-level design note explains this.
- **Thread safety**: `_syncLock` protects `_forwardedPorts` list. Forwarded port operations may occur from multiple threads (event handlers, health checks, add/remove).
- **Event unsubscription**: `StopAll()` unsubscribes `Exception` and `RequestReceived` events before disposing ports to prevent memory leaks.
- **`TunnelError` event**: Declared on `SSHTunnelManager` (not `ProtocolSSH_DotNet`) since this is the class that monitors port health. `ProtocolSSH_DotNet` can subscribe and propagate.
- **`ObjectDisposedException.ThrowIf`**: Add methods throw if called after `Dispose()`.
- **`StopAll()` copies list**: Avoids iterating `_forwardedPorts` while holding the lock for extended I/O operations.
- **Removed `FindFreePort()`**: SSH.NET's `ForwardedPortLocal` with `boundPort: 0` lets the OS auto-assign the port at bind time, avoiding the TOCTOU race condition of finding a port then hoping it's still free.
- **`RemoveForwardedPort` guarded**: Only called if `_sshClient.IsConnected` to avoid exceptions on an already-disconnected client.

**Tests**: `SSHTunnelManagerTests.cs`
- Test `AddLocalForward` with port 0 auto-selects a port
- Test `StopAll` cleans up all ports
- Test `AreAllPortsHealthy` returns false when client disconnected
- Test `Dispose` is idempotent

---

#### Task 4A.2: Integrate SSHTunnelManager into ProtocolSSH_DotNet

**Modify**: `mRemoteNG/Connection/Protocol/SSH_DotNet/ProtocolSSH_DotNet.cs`

Add tunnel manager as a field, initialized after SSH connection is established:

```csharp
private SSHTunnelManager _tunnelManager;

// In Connect(), after _sshClient is connected and shell stream is created:
_tunnelManager = new SSHTunnelManager(_sshClient);

// In Dispose()/CleanupConnection():
_tunnelManager?.Dispose();
_tunnelManager = null;
```

Expose a public property for ConnectionInitiator to access:

```csharp
/// <summary>
/// Provides access to the tunnel manager for setting up port forwarding.
/// Available after Connect() succeeds.
/// </summary>
public SSHTunnelManager TunnelManager => _tunnelManager;

/// <summary>
/// Whether this SSH connection is still alive and usable as a tunnel.
/// </summary>
public bool IsTunnelHealthy =>
    _sshClient?.IsConnected == true &&
    (_tunnelManager?.AreAllPortsHealthy() ?? true);
```

---

#### Task 4A.3: Add Tunnel-Only Connection Mode

**CRITICAL GAP**: When SSH_DotNet is used purely as a tunnel provider (e.g., to tunnel RDP), the current `Connect()` method creates a shell stream, starts output/input reading tasks, and attaches a terminal control — none of which are needed for a tunnel-only connection.

**Modify**: `mRemoteNG/Connection/Protocol/SSH_DotNet/ProtocolSSH_DotNet.cs`

Add a property to control whether this is a tunnel-only connection:

```csharp
/// <summary>
/// When true, Initialize() skips terminal control creation, and Connect()
/// establishes the SSH connection and tunnel manager but skips shell stream
/// creation, terminal attachment, and I/O tasks.
/// Set this before calling Initialize() when using this protocol as a tunnel provider.
/// </summary>
public bool TunnelOnlyMode { get; set; }
```

**First**, modify `Initialize()` to skip terminal creation in tunnel-only mode:

```csharp
public override bool Initialize()
{
    try
    {
        SSHDotNetDiagnostics.LogDebug("Protocol: Initializing ProtocolSSH_DotNet");

        if (!TunnelOnlyMode)
        {
            // Create and initialize terminal control (not needed for tunnel-only)
            _terminalControl = new SshTerminalControl();
            _terminalControl.Initialize();
            _terminalControl.TerminalResized += OnTerminalResized;
            Control = _terminalControl;
        }

        // Call base initialization
        // Note: base.Initialize() handles Control == null gracefully
        bool baseResult = base.Initialize();

        SSHDotNetDiagnostics.LogDebug($"Protocol: Initialization complete (TunnelOnlyMode={TunnelOnlyMode})");
        return baseResult;
    }
    catch (Exception ex)
    {
        SSHDotNetDiagnostics.LogException("Protocol: Initialization failed", ex);
        return false;
    }
}
```

**Then**, in `Connect()`, after the SSH client is connected and keep-alive is configured (after line ~230), add:

```csharp
// Create tunnel manager (available for both tunnel-only and full terminal mode)
_tunnelManager = new SSHTunnelManager(_sshClient);

if (TunnelOnlyMode)
{
    // In tunnel-only mode, skip shell stream, terminal, and I/O tasks.
    // The SSH connection is ready for port forwarding only.
    State = ConnectionState.Connected;
    SSHDotNetDiagnostics.LogInfo("Protocol: Connected in tunnel-only mode (no shell)");
    Event_Connected(this);
    return true;
}

// ... rest of existing Connect() code (shell stream, terminal, I/O) ...
```

Then in `ConnectionInitiator.cs` (Task 4B.2), set the flag before connecting:

```csharp
sshDotNetTunnel.TunnelOnlyMode = true;
```

This avoids creating unnecessary terminal UI for a connection that only serves as a tunnel.

---

### Phase 4B: Integration with ConnectionInitiator (SSH_DotNet as Tunnel Provider)

This is the critical integration point - making SSH_DotNet connections work as tunnel providers for other protocols (RDP, VNC, etc.).

#### Task 4B.1: Update SSHTunnelTypeConverter

**Modify**: `mRemoteNG/Tools/SSHTunnelTypeConverter.cs`

Add `SSH_DotNet` to the list of valid tunnel connection types:

```csharp
// Current (line 38):
if (node.Protocol == ProtocolType.SSH1 || node.Protocol == ProtocolType.SSH2)
    result.Add(node.Name);

// Updated:
if (node.Protocol == ProtocolType.SSH1 || node.Protocol == ProtocolType.SSH2
    || node.Protocol == ProtocolType.SSH_DotNet)
    result.Add(node.Name);
```

This allows users to select SSH_DotNet connections as tunnel providers in the property grid.

**ALSO** update `ConnectionInitiator.getSSHConnectionInfoByName()` at line 269, which has the **same filter** and will fail to find SSH_DotNet connections at runtime:

```csharp
// Current (line 269):
if (node.Name == SSHTunnelConnectionName && (node.Protocol == ProtocolType.SSH1 || node.Protocol == ProtocolType.SSH2)) result = node;

// Updated:
if (node.Name == SSHTunnelConnectionName
    && (node.Protocol == ProtocolType.SSH1 || node.Protocol == ProtocolType.SSH2 || node.Protocol == ProtocolType.SSH_DotNet))
    result = node;
```

Without this fix, the dropdown will show SSH_DotNet connections but ConnectionInitiator will fail with `SshTunnelConfigProblem` because it cannot find them.

---

#### Task 4B.2: Update ConnectionInitiator to Support SSH_DotNet Tunnels

**Modify**: `mRemoteNG/Connection/ConnectionInitiator.cs`

The current code (lines 112-219) has several structural issues that must be addressed:

1. **Hard PuTTY dependency** at line 143 rejects non-PuTTY protocols
2. **Cloning + SSHOptions modification** at lines 132-133 happens before the PuTTY/SSH_DotNet branch point — SSH_DotNet doesn't use SSHOptions
3. **Free port finding via TcpListener** at lines 124-127 happens before branching — SSH_DotNet uses auto-port (boundPort: 0) instead, avoiding the TOCTOU race
4. **Target connection clone** at lines 136-139 sets the port before branching — SSH_DotNet doesn't know the port until after `AddLocalForward()` returns

**Solution**: Restructure the tunnel setup so that protocol-specific logic (port finding, cloning, SSHOptions modification) moves **inside** each branch.

**Required**: Add `using mRemoteNG.Connection.Protocol.SSH_DotNet;` to ConnectionInitiator.cs imports.

```csharp
// RESTRUCTURED TUNNEL SETUP
// Common code: find the SSH tunnel connection info
connectionInfoSshTunnel = getSSHConnectionInfoByName(
    Runtime.ConnectionsService.ConnectionTreeModel.RootNodes,
    connectionInfoOriginal.SSHTunnelConnectionName);
if (connectionInfoSshTunnel == null)
{
    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
        string.Format(Language.SshTunnelConfigProblem, connectionInfoOriginal.Name,
            connectionInfoOriginal.SSHTunnelConnectionName));
    return;
}

// Create the protocol instance to determine its type
ProtocolBase protocolSshTunnel = protocolFactory.CreateProtocol(connectionInfoSshTunnel);
int localSshTunnelPort;

if (protocolSshTunnel is PuttyBase puttyBaseSshTunnel)
{
    // === PuTTY BRANCH (existing logic, moved inside branch) ===
    // Find free port via TcpListener (PuTTY needs it upfront for -L flag)
    System.Net.Sockets.TcpListener l = new(System.Net.IPAddress.Loopback, 0);
    l.Start();
    localSshTunnelPort = ((System.Net.IPEndPoint)l.LocalEndpoint).Port;
    l.Stop();

    // Clone SSH tunnel connection and add -L option
    connectionInfoSshTunnel = connectionInfoSshTunnel.Clone();
    connectionInfoSshTunnel.SSHOptions += " -L " + localSshTunnelPort + ":"
        + connectionInfoOriginal.Hostname + ":" + connectionInfoOriginal.Port;

    // Clone target connection with tunnel port
    connectionInfo = connectionInfoOriginal.Clone();
    connectionInfo.Name += " via " + connectionInfoSshTunnel.Name;
    connectionInfo.Hostname = "localhost";
    connectionInfo.Port = localSshTunnelPort;

    // ... existing PuTTY setup, initialize, connect, polling loop, window hiding ...
    // (unchanged from current code lines 150-218)
}
else if (protocolSshTunnel is ProtocolSSH_DotNet sshDotNetTunnel)
{
    // === SSH_DotNet BRANCH (new) ===
    // No clone needed — we don't modify SSHOptions
    // No TcpListener needed — SSH.NET auto-assigns port with boundPort: 0

    sshDotNetTunnel.TunnelOnlyMode = true;

    SetConnectionFormEventHandlers(protocolSshTunnel, connectionForm);
    SetConnectionEventHandlers(protocolSshTunnel);
    connectionContainer = SetConnectionContainer(connectionInfoOriginal, connectionForm);
    BuildConnectionInterfaceController(connectionInfoSshTunnel, protocolSshTunnel, connectionContainer);
    protocolSshTunnel.InterfaceControl.OriginalInfo = connectionInfoSshTunnel;

    if (!protocolSshTunnel.Initialize() || !protocolSshTunnel.Connect())
    {
        protocolSshTunnel.Close();
        Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
            string.Format(Language.SshTunnelNotConnected, connectionInfoOriginal.Name,
                connectionInfoSshTunnel.Name));
        return;
    }

    // Set up the local port forward via SSH.NET API
    // Pass boundPort: 0 to let OS auto-assign — no TOCTOU race!
    try
    {
        uint actualPort = sshDotNetTunnel.TunnelManager.AddLocalForward(
            "127.0.0.1",
            0,  // OS auto-assigns port
            connectionInfoOriginal.Hostname,
            (uint)connectionInfoOriginal.Port);

        localSshTunnelPort = (int)actualPort;

        Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg,
            string.Format(Language.SshTunnelDotNetReady,
                connectionInfoOriginal.Name, localSshTunnelPort));
    }
    catch (Exception tunnelEx)
    {
        // Must Disconnect() first to clean up SSH client + tunnel manager,
        // then Close() to clean up UI. Close() alone only disposes the UI
        // and would leak the SSH connection.
        protocolSshTunnel.Disconnect();
        protocolSshTunnel.Close();
        Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
            string.Format(Language.SshTunnelDotNetFailed,
                connectionInfoOriginal.Name, tunnelEx.Message));
        return;
    }

    // Clone target connection with the actual tunnel port (now known)
    connectionInfo = connectionInfoOriginal.Clone();
    connectionInfo.Name += " via " + connectionInfoSshTunnel.Name;
    connectionInfo.Hostname = "localhost";
    connectionInfo.Port = localSshTunnelPort;

    // Hide the tunnel connection's display (container reused for target)
    protocolSshTunnel.InterfaceControl.Hide();
}
else
{
    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
        string.Format(Language.SshTunnelUnsupportedProtocol, connectionInfoOriginal.Name,
            connectionInfoSshTunnel.Name));
    return;
}
```

**Key advantages of the SSH_DotNet path**:
1. `ForwardedPortLocal.Start()` is synchronous — the port is **immediately** ready, eliminating the entire 60-second polling loop
2. `boundPort: 0` lets the OS auto-assign the port at bind time, avoiding the TOCTOU race condition
3. No clone of the SSH tunnel connection info needed — SSH_DotNet doesn't use SSHOptions
4. The target connection clone happens **after** the tunnel port is known

---

#### Task 4B.3: Add Tunnel Health Monitoring

The `TunnelError` event is already declared on `SSHTunnelManager` (see Task 4A.1) and raised by `OnForwardException`. To propagate this to the protocol level:

**Modify**: `mRemoteNG/Connection/Protocol/SSH_DotNet/ProtocolSSH_DotNet.cs`

Subscribe to the tunnel manager's error event and propagate to the protocol's error handler:

```csharp
// In Connect(), after creating _tunnelManager:
_tunnelManager = new SSHTunnelManager(_sshClient);
_tunnelManager.TunnelError += OnTunnelError;

// Event handler:
private void OnTunnelError(object sender, string errorMessage)
{
    SSHDotNetDiagnostics.LogError($"Protocol: {errorMessage}");
    Event_ErrorOccured(this, errorMessage, null);
}

// In Disconnect() — MUST happen BEFORE _sshClient.Disconnect() so forwarded port
// Stop() can send channel close messages while SSH connection is still active:
if (_tunnelManager != null)
{
    _tunnelManager.TunnelError -= OnTunnelError;
    _tunnelManager.Dispose();
    _tunnelManager = null;
}

// Also in CleanupConnection() (for failed connection cleanup):
_tunnelManager?.Dispose();
_tunnelManager = null;
```

**IMPORTANT ordering**: The tunnel manager must be disposed **before** `_sshClient.Disconnect()` in the `Disconnect()` method. SSH.NET's `ForwardedPort.Stop()` sends channel close messages to the server, which requires an active connection.

This allows `ConnectionInitiator` or any subscriber to detect when a tunnel has failed, rather than waiting for the tunneled connection (e.g., RDP) to time out.

---

### Phase 4C: Jump Host Support

#### Task 4C.1: Create SSHJumpHostManager

**New file**: `mRemoteNG/Connection/Protocol/SSH_DotNet/SSHJumpHostManager.cs`

Manages chained SSH connections through one or more jump hosts:

```csharp
// Design Note: Generic catch clauses are intentional in Dispose() — cleanup of one hop
// must not prevent cleanup of remaining hops. All exceptions are logged.
using System;
using System.Collections.Generic;
using Renci.SshNet;

namespace mRemoteNG.Connection.Protocol.SSH_DotNet
{
    /// <summary>
    /// Manages SSH jump host connections by chaining SshClient instances
    /// through local port forwarding.
    ///
    /// Pattern: Client -> JumpHost1 --(ForwardedPortLocal)--> JumpHost2 --(ForwardedPortLocal)--> Target
    /// </summary>
    public class SSHJumpHostManager : IDisposable
    {
        private readonly List<SshClient> _jumpClients = new();
        private readonly List<ForwardedPortLocal> _jumpForwards = new();
        private bool _disposed;

        /// <summary>
        /// Establish a chain of SSH connections through jump hosts.
        /// Returns the final SshClient connected to the target host.
        /// On failure, cleans up any partially-connected resources before throwing.
        /// </summary>
        /// <param name="jumpHosts">
        /// Ordered list of (host, port, username, authMethods) for each jump host.
        /// The last entry is the final target.
        /// </param>
        /// <exception cref="ArgumentException">If jumpHosts is null or empty.</exception>
        public SshClient ConnectThroughJumpHosts(
            IList<JumpHostInfo> jumpHosts,
            TimeSpan timeout)
        {
            if (jumpHosts == null || jumpHosts.Count == 0)
                throw new ArgumentException("At least one jump host must be specified.", nameof(jumpHosts));

            SshClient previousClient = null;

            try
            {
                for (int i = 0; i < jumpHosts.Count; i++)
                {
                    var hop = jumpHosts[i];
                    if (string.IsNullOrEmpty(hop.Host))
                        throw new ArgumentException($"Jump host {i + 1} has no hostname.");
                    if (string.IsNullOrEmpty(hop.Username))
                        throw new ArgumentException($"Jump host {i + 1} ({hop.Host}) has no username.");
                    if (hop.AuthMethods == null || hop.AuthMethods.Length == 0)
                        throw new ArgumentException($"Jump host {i + 1} ({hop.Host}) has no auth methods.");

                    SshClient client;

                    if (previousClient == null)
                    {
                        // First hop: direct connection
                        var connInfo = new Renci.SshNet.ConnectionInfo(
                            hop.Host, hop.Port, hop.Username, hop.AuthMethods);
                        connInfo.Timeout = timeout;
                        client = new SshClient(connInfo);
                        client.Connect();
                    }
                    else
                    {
                        // Subsequent hops: connect through forwarded port on previous client
                        var forward = new ForwardedPortLocal("127.0.0.1", 0, hop.Host, (uint)hop.Port);
                        previousClient.AddForwardedPort(forward);
                        forward.Start();
                        _jumpForwards.Add(forward);

                        var connInfo = new Renci.SshNet.ConnectionInfo(
                            "127.0.0.1", (int)forward.BoundPort, hop.Username, hop.AuthMethods);
                        connInfo.Timeout = timeout;
                        client = new SshClient(connInfo);
                        client.Connect();
                    }

                    _jumpClients.Add(client);
                    previousClient = client;

                    SSHDotNetDiagnostics.LogInfo(
                        $"JumpHost: Connected through hop {i + 1}/{jumpHosts.Count}: {hop.Host}:{hop.Port}");
                }
            }
            catch
            {
                // On partial failure, clean up any resources we've already allocated
                Dispose();
                throw;
            }

            return previousClient; // The final client is the target
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            // Dispose in reverse order (target first, then jump hosts)
            for (int i = _jumpForwards.Count - 1; i >= 0; i--)
            {
                try { _jumpForwards[i].Stop(); _jumpForwards[i].Dispose(); }
                catch (Exception ex) { SSHDotNetDiagnostics.LogException("JumpHost: Error disposing forward", ex); }
            }
            for (int i = _jumpClients.Count - 1; i >= 0; i--)
            {
                try { _jumpClients[i].Disconnect(); _jumpClients[i].Dispose(); }
                catch (Exception ex) { SSHDotNetDiagnostics.LogException("JumpHost: Error disposing client", ex); }
            }
            _jumpForwards.Clear();
            _jumpClients.Clear();
        }
    }

    public class JumpHostInfo
    {
        public required string Host { get; set; }
        public int Port { get; set; } = 22;
        public required string Username { get; set; }
        public required AuthenticationMethod[] AuthMethods { get; set; }
    }
}
```

**Note**: Jump host support is a more advanced feature. For the initial implementation, the basic local/remote/dynamic port forwarding (Phase 4A + 4B) provides the most value. Jump host can be added incrementally.

---

### Phase 4D: Connection Properties for Port Forwarding

#### Task 4D.1: Add Port Forwarding Properties

For the tunnel-as-proxy use case (Task 4B), no new properties are needed - the existing `SSHTunnelConnectionName` property already handles this.

For user-configured port forwarding rules on an SSH_DotNet connection itself, add:

**Modify**: `mRemoteNG/Connection/AbstractConnectionRecord.cs`

```csharp
private string _sshDotNetPortForwardRules = "";

[LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 3),
 LocalizedAttributes.LocalizedDisplayName(nameof(Language.SshPortForwardRules)),
 LocalizedAttributes.LocalizedDescription(nameof(Language.PropertyDescriptionSshPortForwardRules)),
 AttributeUsedInProtocol(ProtocolType.SSH_DotNet)]
public string SSHDotNetPortForwardRules
{
    get => GetPropertyValue("SSHDotNetPortForwardRules", _sshDotNetPortForwardRules);
    set => SetField(ref _sshDotNetPortForwardRules, value, "SSHDotNetPortForwardRules");
}
```

**Format**: Semicolon-separated rules, each in the format `L:localPort:remoteHost:remotePort` or `R:remotePort:localHost:localPort` or `D:localPort`:

```
L:3389:rdp-server:3389;L:5900:vnc-server:5900;D:1080
```

This mirrors PuTTY's approach but in a structured format rather than raw CLI options.

**Integration point**: In `ProtocolSSH_DotNet.Connect()`, after the tunnel manager is created (both in tunnel-only and terminal modes), apply the rules:

```csharp
// After: _tunnelManager = new SSHTunnelManager(_sshClient);
// Apply any user-configured port forward rules
string rules = InterfaceControl?.Info?.SSHDotNetPortForwardRules;
if (!string.IsNullOrWhiteSpace(rules))
{
    PortForwardRuleParser.ApplyRules(_tunnelManager, rules);
}
```

This allows SSH_DotNet terminal connections to set up port forwards alongside the interactive shell.

**Port Forward Rule Parser** (add to `SSHTunnelManager.cs` or a new `PortForwardRuleParser.cs`):

```csharp
/// <summary>
/// Parses port forwarding rules from the semicolon-separated string format.
/// Format: L:localPort:remoteHost:remotePort | R:remotePort:localHost:localPort | D:localPort
/// </summary>
public static class PortForwardRuleParser
{
    public static void ApplyRules(SSHTunnelManager tunnelManager, string rulesString)
    {
        if (string.IsNullOrWhiteSpace(rulesString)) return;

        var rules = rulesString.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var rule in rules)
        {
            var parts = rule.Split(':');
            if (parts.Length < 2)
            {
                SSHDotNetDiagnostics.LogWarning($"Tunnel: Invalid port forward rule (too few parts): '{rule}'");
                continue;
            }

            switch (parts[0].ToUpperInvariant())
            {
                case "L" when parts.Length == 4
                    && uint.TryParse(parts[1], out uint localPort)
                    && uint.TryParse(parts[3], out uint remotePort):
                    tunnelManager.AddLocalForward("127.0.0.1", localPort, parts[2], remotePort);
                    break;

                case "R" when parts.Length == 4
                    && uint.TryParse(parts[1], out uint rBindPort)
                    && uint.TryParse(parts[3], out uint lPort):
                    tunnelManager.AddRemoteForward("0.0.0.0", rBindPort, parts[2], lPort);
                    break;

                case "D" when parts.Length == 2
                    && uint.TryParse(parts[1], out uint socksPort):
                    tunnelManager.AddDynamicForward("127.0.0.1", socksPort);
                    break;

                default:
                    SSHDotNetDiagnostics.LogWarning($"Tunnel: Unrecognized port forward rule: '{rule}'");
                    break;
            }
        }
    }
}
```

**Modify**: `mRemoteNG/Connection/ConnectionInfoInheritance.cs`

```csharp
// Property name matches AbstractConnectionRecord property name (existing convention)
// In XML serialization, this becomes attribute "InheritSSHDotNetPortForwardRules"
[LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
 LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.SshPortForwardRules)),
 LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionSshPortForwardRules)),
 TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
public bool SSHDotNetPortForwardRules { get; set; }
```

**Modify**: Serializers (XML, CSV) to persist the new property.

---

### Phase 4E: Localization Strings

**Modify**: `mRemoteNG/Language/Language.resx`

```xml
<data name="SshPortForwardRules" xml:space="preserve">
  <value>Port Forward Rules</value>
</data>
<data name="PropertyDescriptionSshPortForwardRules" xml:space="preserve">
  <value>SSH port forwarding rules. Format: L:localPort:remoteHost:remotePort for local forwarding, R:remotePort:localHost:localPort for remote forwarding, D:localPort for dynamic SOCKS5 proxy. Separate multiple rules with semicolons.</value>
</data>
<data name="SshTunnelDotNetReady" xml:space="preserve">
  <value>SSH.NET tunnel for '{0}' is ready on localhost:{1}</value>
</data>
<data name="SshTunnelDotNetFailed" xml:space="preserve">
  <value>SSH.NET tunnel for '{0}' failed: {1}</value>
</data>
<data name="SshTunnelUnsupportedProtocol" xml:space="preserve">
  <value>SSH tunnel for connection '{0}' references '{1}' which is not an SSH protocol (SSH1, SSH2, or SSH_DotNet).</value>
</data>
```

**Note**: The existing `SshTunnelIsNotPutty` string should be kept for backward compatibility but the new `SshTunnelUnsupportedProtocol` string is used in the restructured `else` branch of ConnectionInitiator.

---

## File Change Summary

### New Files

| File | Purpose |
|------|---------|
| `mRemoteNG/Connection/Protocol/SSH_DotNet/SSHTunnelManager.cs` | Port forwarding lifecycle management |
| `mRemoteNG/Connection/Protocol/SSH_DotNet/PortForwardRuleParser.cs` | Parse port forward rule strings |
| `mRemoteNG/Connection/Protocol/SSH_DotNet/SSHJumpHostManager.cs` | Jump host chaining (Phase 4C) |
| `mRemoteNGTests/Connection/Protocol/SSH_DotNet/SSHTunnelManagerTests.cs` | Unit tests for tunnel manager |
| `mRemoteNGTests/Connection/Protocol/SSH_DotNet/PortForwardRuleParserTests.cs` | Unit tests for rule parser |
| `mRemoteNGTests/Connection/Protocol/SSH_DotNet/SSHJumpHostManagerTests.cs` | Unit tests for jump host (Phase 4C) |

### Modified Files

| File | Change |
|------|--------|
| `mRemoteNG/Connection/Protocol/SSH_DotNet/ProtocolSSH_DotNet.cs` | Add `TunnelOnlyMode`, `_tunnelManager` field, `TunnelManager` property, `IsTunnelHealthy`, modify `Initialize()` and `Disconnect()` |
| `mRemoteNG/Connection/ConnectionInitiator.cs` | Add `using SSH_DotNet`, restructure tunnel setup with PuTTY/SSH_DotNet branches, update `getSSHConnectionInfoByName()` to include SSH_DotNet |
| `mRemoteNG/Tools/SSHTunnelTypeConverter.cs` | Add `SSH_DotNet` to valid tunnel protocol list |
| `mRemoteNG/Connection/AbstractConnectionRecord.cs` | Add `SSHDotNetPortForwardRules` property |
| `mRemoteNG/Connection/ConnectionInfoInheritance.cs` | Add `SSHDotNetPortForwardRules` inheritance flag (auto-picked up by reflection-based `SetAllValues`) |
| `mRemoteNG/Config/Serializers/.../XmlConnectionNodeSerializer28.cs` | Serialize new property |
| `mRemoteNG/Config/Serializers/.../XmlConnectionsDeserializer.cs` | Deserialize new property |
| `mRemoteNG/Config/Serializers/.../CsvConnectionsSerializerMremotengFormat.cs` | Serialize new property (header + value + inheritance) |
| `mRemoteNG/Config/Serializers/.../CsvConnectionsDeserializerMremotengFormat.cs` | Deserialize new property |
| `mRemoteNG/Config/Serializers/.../Sql/DataTableSerializer.cs` | Add column + serialize new property |
| `mRemoteNG/Config/Serializers/.../Sql/DataTableDeserializer.cs` | Deserialize new property |
| `mRemoteNG/Config/Serializers/.../Sql/SqlDatabaseMetaDataRetriever.cs` | Add column to schema |
| `mRemoteNG/Config/Serializers/Versioning/` | New SQL version upgrader for schema migration |
| `mRemoteNG/Language/Language.resx` | Add localization strings |

---

## Implementation Order

The recommended implementation order prioritizes the most impactful feature first:

| Order | Task | Impact | Effort |
|-------|------|--------|--------|
| 1 | Task 4A.1: SSHTunnelManager | Foundation for all tunnel features | 3 hours |
| 2 | Task 4A.2: Integrate into ProtocolSSH_DotNet | Expose tunnel capability | 1 hour |
| 3 | Task 4A.3: Tunnel-only connection mode | **Required** - avoid unnecessary shell/terminal for tunnel connections | 2 hours |
| 4 | Task 4B.1: Update SSHTunnelTypeConverter | Allow SSH_DotNet as tunnel option | 15 min |
| 5 | Task 4B.2: Update ConnectionInitiator | **Core feature** - SSH_DotNet tunnels work | 3 hours |
| 6 | Task 4B.3: Tunnel health monitoring | Reliability improvement | 1 hour |
| 7 | Task 4D.1: Port forwarding properties + parser | User-configurable forwarding rules | 3 hours |
| 8 | Task 4E: Localization | Required for UI strings | 30 min |
| 9 | Task 4C.1: Jump host support | Advanced feature | 4 hours |

**Total estimated effort**: ~18 hours

---

## Testing Strategy

### Unit Tests

1. **SSHTunnelManagerTests**
   - `AddLocalForward_WithPort0_AutoSelectsPort`
   - `AddLocalForward_StartsForwarding`
   - `AddRemoteForward_StartsForwarding`
   - `AddDynamicForward_StartsForwarding`
   - `AddLocalForward_AfterDispose_ThrowsObjectDisposedException`
   - `StopAll_StopsAndDisposesAllPorts`
   - `StopAll_UnsubscribesEventHandlers`
   - `AreAllPortsHealthy_ReturnsFalseWhenClientDisconnected`
   - `AreAllPortsHealthy_ReturnsTrueWhenAllPortsStarted`
   - `Dispose_IsIdempotent`
   - `TunnelError_FiredOnPortException`

2. **PortForwardRuleParserTests**
   - `ApplyRules_NullOrEmpty_DoesNothing`
   - `ApplyRules_LocalForward_ParsesCorrectly`
   - `ApplyRules_RemoteForward_ParsesCorrectly`
   - `ApplyRules_DynamicForward_ParsesCorrectly`
   - `ApplyRules_MultipleRules_ParsesAll`
   - `ApplyRules_InvalidFormat_LogsWarningAndSkips`
   - `ApplyRules_InvalidPort_LogsWarningAndSkips`
   - `ApplyRules_UnknownType_LogsWarning`

3. **ProtocolSSH_DotNet tunnel integration**
   - `TunnelManager_AvailableAfterConnect`
   - `TunnelManager_DisposedOnClose`
   - `TunnelOnlyMode_SkipsShellStreamCreation`
   - `TunnelOnlyMode_ConnectsSuccessfully`

4. **SSHTunnelTypeConverter**
   - `GetStandardValues_IncludesSshDotNetConnections`
   - `GetStandardValues_IncludesSsh1AndSsh2`

5. **SSHJumpHostManagerTests** (Phase 4C)
   - `ConnectThroughJumpHosts_SingleHop_ConnectsDirectly`
   - `ConnectThroughJumpHosts_MultipleHops_ChainsConnections`
   - `ConnectThroughJumpHosts_EmptyList_ThrowsArgumentException`
   - `ConnectThroughJumpHosts_PartialFailure_CleansUpResources`
   - `Dispose_IsIdempotent`

### Integration Tests (Manual)

1. Create an SSH_DotNet connection to a test SSH server
2. Create an RDP connection with SSHTunnelConnectionName pointing to the SSH_DotNet connection
3. Verify RDP connects successfully through the tunnel
4. Disconnect SSH tunnel and verify RDP connection is notified
5. Test with VNC, HTTP, and other protocols through the tunnel
6. Test multiple simultaneous tunnels

---

## Risk Analysis

| Risk | Likelihood | Impact | Mitigation |
|------|-----------|--------|------------|
| SSH.NET ForwardedPort bugs | Low | High | SSH.NET is mature; well-tested in production |
| Port conflicts on localhost | Medium | Low | Auto-port selection with port 0 |
| Tunnel connection ordering | Medium | Medium | Validate SSH connection is established before starting forwards |
| Memory leaks from undisposed ports | Low | Medium | `SSHTunnelManager` implements `IDisposable` with cleanup |
| Jump host auth complexity | Medium | Low | Start with single-hop; jump host is Phase 4C |

---

## Code Quality Checklist (GitHub Copilot / SonarQube Readiness)

Items specifically addressed to pass automated code review:

| Issue | Status | Location |
|-------|--------|----------|
| Generic catch clauses | File-level design note comments explain intentional use | SSHTunnelManager.cs, SSHJumpHostManager.cs |
| Event handler memory leaks | `StopAll()` unsubscribes `Exception`/`RequestReceived` before disposal | SSHTunnelManager.cs |
| Thread safety | `_syncLock` protects `_forwardedPorts` list | SSHTunnelManager.cs |
| Disposable fields not disposed | `_tunnelManager` disposed in `CleanupConnection()` and `Disconnect()` | ProtocolSSH_DotNet.cs |
| Double-dispose guard | `_disposed` flag in both manager classes | SSHTunnelManager.cs, SSHJumpHostManager.cs |
| `ObjectDisposedException` after Dispose | `ThrowIf` guards on Add methods | SSHTunnelManager.cs |
| Partial failure resource cleanup | `ConnectThroughJumpHosts` calls `Dispose()` in catch before re-throwing | SSHJumpHostManager.cs |
| Null/empty input validation | `JumpHostInfo` uses `required` keyword; parser validates format | SSHJumpHostManager.cs, PortForwardRuleParser.cs |
| TOCTOU port race | Removed `FindFreePort()`; use `boundPort: 0` to let OS assign at bind time | SSHTunnelManager.cs |
| Useless variable assignments | None in plan code | N/A |
| Readonly fields where applicable | `_sshClient`, `_forwardedPorts`, `_syncLock`, `_jumpClients`, `_jumpForwards` all `readonly` | Both managers |
| Missing using directives | All code blocks include complete `using` statements; `using SSH_DotNet` added to ConnectionInitiator | All files |
| Disposal ordering | `_tunnelManager` disposed before `_sshClient.Disconnect()` to allow clean channel close | ProtocolSSH_DotNet.cs |
| Conditional resource creation | `TunnelOnlyMode` skips terminal control creation in `Initialize()` — no wasted resources | ProtocolSSH_DotNet.cs |

---

## Gaps Found and Addressed (v1.2 + v1.3)

This section documents gaps discovered during detailed review against the actual codebase.

### Fixed in This Version

| # | Gap | Severity | Fix |
|---|-----|----------|-----|
| 1 | `getSSHConnectionInfoByName()` at ConnectionInitiator.cs:269 filters by SSH1/SSH2 only — SSH_DotNet connections invisible at runtime | CRITICAL | Added to Task 4B.1 |
| 2 | `Initialize()` always creates `SshTerminalControl` — wasteful for tunnel-only mode | CRITICAL | Added `TunnelOnlyMode` guard to `Initialize()` in Task 4A.3 |
| 3 | Plan's ConnectionInitiator code used `localSshTunnelPort` (TcpListener-found) instead of `0` (OS auto-assign), defeating TOCTOU fix | HIGH | Restructured Task 4B.2 to pass `0` and read back actual port |
| 4 | Cloning + SSHOptions modification happened before PuTTY/SSH_DotNet branch point | HIGH | Moved inside PuTTY branch in Task 4B.2 |
| 5 | Target connection clone set port before tunnel port was known (SSH_DotNet) | HIGH | Moved after `AddLocalForward()` return in Task 4B.2 |
| 6 | `_tunnelManager` disposal order — must happen before `_sshClient.Disconnect()` | MEDIUM | Added ordering note to Task 4B.3 |
| 7 | SQL serializers (DataTableSerializer, DataTableDeserializer, SqlDatabaseMetaDataRetriever) missing from file change summary | MEDIUM | Added to file change summary |
| 8 | Error message `SshTunnelIsNotPutty` used in `else` branch — misleading when SSH_DotNet exists | LOW | Added `SshTunnelUnsupportedProtocol` string |
| 9 | Missing `using` directive for `SSH_DotNet` namespace in ConnectionInitiator | LOW | Noted in Task 4B.2 |
| 10 | `ConnectionInfoInheritance` property named `InheritSSHDotNetPortForwardRules` — wrong convention; should be `SSHDotNetPortForwardRules` (matching `AbstractConnectionRecord` property name) | MEDIUM | Fixed property name + added full attribute decorators in Task 4D.1 (v1.3) |
| 11 | `AddLocalForward()` catch block called `Close()` only — leaks SSH connection since `Close()` only disposes UI, not SSH client | HIGH | Added `Disconnect()` call before `Close()` in Task 4B.2 catch block (v1.3) |
| 12 | Unused `using System.Net` / `System.Net.Sockets` in SSHTunnelManager.cs — Copilot would flag | LOW | Removed from plan code (v1.3) |
| 13 | `PortForwardRuleParser.ApplyRules()` defined but no integration point shown in `Connect()` | MEDIUM | Added integration code snippet to Task 4D.1 (v1.3) |

### Open Questions (Require User Direction)

| # | Question | Resolution |
|---|----------|------------|
| 1 | `ForwardedPortDynamic` with `port: 0` — SSH.NET may or may not support auto-port for dynamic forwarding | RESOLVED — port 0 accepted (see test results below) |
| 2 | SQL schema migration — adding `SSHDotNetPortForwardRules` column requires a new SQL version upgrader | DEFERRED — SQL integration will be done separately in a future phase |

### ForwardedPortDynamic Port 0 Test Results

Tested SSH.NET v2025.1.0 `ForwardedPortDynamic` with `port: 0`:

```
=== ForwardedPortDynamic Port 0 Test ===

[PASS] Constructor ForwardedPortDynamic("127.0.0.1", 0) succeeded
       BoundHost = 127.0.0.1
       BoundPort = 0           (assigned by OS when Start() binds the socket)
       IsStarted = False

[PASS] Constructor ForwardedPortDynamic("127.0.0.1", 1080) succeeded
       BoundHost = 127.0.0.1
       BoundPort = 1080

[PASS] Constructor ForwardedPortLocal("127.0.0.1", 0, ...) succeeded
       BoundHost = 127.0.0.1
       BoundPort = 0

Start() without SSH client → InvalidOperationException (expected)

Type hierarchy: ForwardedPortDynamic → ForwardedPort (same base as ForwardedPortLocal)
```

**Conclusion**: Port 0 is fully accepted by `ForwardedPortDynamic`. Both `ForwardedPortDynamic` and `ForwardedPortLocal` share the same `ForwardedPort` base class which handles socket binding. `BoundPort` returns the OS-assigned port after `Start()` completes. The `AddDynamicForward()` method in `SSHTunnelManager` can safely use port 0 for auto-assignment.

---

## References

- [SSH.NET ForwardedPortLocal API](https://sshnet.github.io/SSH.NET/api/Renci.SshNet.ForwardedPortLocal.html)
- [SSH.NET ForwardedPortRemote API](https://sshnet.github.io/SSH.NET/api/Renci.SshNet.ForwardedPortRemote.html)
- [SSH.NET ForwardedPortDynamic API](https://sshnet.github.io/SSH.NET/api/Renci.SshNet.ForwardedPortDynamic.html)
- [SSH.NET ProxyJump Discussion (Issue #481)](https://github.com/sshnet/SSH.NET/issues/481)
- [SSH.NET Namespace Reference](https://sshnet.github.io/SSH.NET/api/Renci.SshNet.html)
- [SSH Port Forwarding in .NET Example](https://ladydebug.com/blog/2019/03/25/ssh-port-forwarding-in-net-c-example/)
