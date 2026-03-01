# mRemoteNG Architecture

This document describes the high-level architecture of mRemoteNG, a multi-protocol remote connections manager for Windows.

## System Layers

```
+------------------------------------------------------+
|                    UI Layer                           |
|  Forms / Windows / Controls / Menus / Themes         |
+------------------------------------------------------+
|                 Connection Layer                      |
|  ConnectionInfo / ConnectionInitiator / Protocols     |
+------------------------------------------------------+
|               Configuration Layer                     |
|  Serializers / Settings / DataProviders               |
+------------------------------------------------------+
|                Security Layer                         |
|  Encryption / KeyDerivation / CredentialProviders     |
+------------------------------------------------------+
|              External Connectors                      |
|  1Password / SecretServer / Passwordstate / OpenBao   |
+------------------------------------------------------+
```

## Core Components

### Connection Data Model (`Connection/`)

The connection hierarchy is:

- **`AbstractConnectionRecord`** — Base class with all connection properties (hostname, port, protocol, credentials, RDP settings, etc.)
- **`ConnectionInfo`** — A single connection node. Adds inheritance support and runtime state (open connections list).
- **`ContainerInfo`** — A folder that extends `ConnectionInfo` with a `Children` list. Implements `INotifyCollectionChanged`.
- **`RootNodeInfo`** — The root folder. Holds the master password hash and encryption settings.

Property inheritance: each `ConnectionInfo` has a `ConnectionInfoInheritance` object that specifies which properties to inherit from the parent `ContainerInfo`.

### Connection Tree (`Tree/`)

- **`ConnectionTreeModel`** — The in-memory model holding one or more `RootNodeInfo` root nodes. Raises collection/property changed events for UI binding.
- **`NodeSearcher`** — Provides search over the tree model.
- **`ConnectionTreeSearchTextFilter`** — Filters the tree by name, protocol, or tag.

### Protocol Layer (`Connection/Protocol/`)

- **`ProtocolBase`** — Abstract base for all protocols. Manages lifecycle events (Connecting, Connected, Disconnected, Closed, Error) and hosts a UI control in a `ConnectionTab`.
- Protocol implementations: `RdpProtocol`, `ProtocolSSH1`/`ProtocolSSH2`, `ProtocolVNC`, `ProtocolTelnet`, `ProtocolHTTP`/`ProtocolHTTPS`, `PuttyBase`, `IntApp` (external apps), etc.
- **`ConnectionInitiator`** — Orchestrates opening connections: selects protocol, creates panel, wires events, manages reconnection.

### Configuration (`Config/`)

- **Serializers** — Read/write connection data in multiple formats:
  - `XmlConnectionsSerializer` / `XmlConnectionsDeserializer` — XML (confCons.xml)
  - `JsonConnectionsSerializer` — JSON export (v1.80+)
  - `DataTableSerializer` / `DataTableDeserializer` — SQL Server backend
- **Settings** — `SettingsLoader` / `SettingsSaver` manage app settings, toolbar positions, and external apps.
- **DataProviders** — `FileDataProvider`, `SqlDataProvider` abstract storage access. Implement the `IDataProvider<T>` interface.
- **Loaders** — `XmlConnectionsLoader` and `SqlConnectionsLoader` orchestrate the loading process. These have been refactored to use **Dependency Injection**, allowing them to be tested autonomously by injecting mock providers, meta-data retrievers, and version verifiers.

### Security (`Security/`)

- **`AeadCryptographyProvider`** — AES-256-GCM encryption via BouncyCastle. Used for encrypting passwords in confCons.xml.
- **`Pbkdf2KeyDerivationFunction`** — PBKDF2-HMAC-SHA1 key derivation. Configurable iteration count (600,000 default in v1.80+).
- **`ICryptographyProvider`** interface — Abstracts encryption so the system can evolve to new algorithms.
- Legacy support: `AesCryptographyProvider` (CBC mode) for reading older files.

## Testability and Dependency Injection

Recent architectural improvements have focused on decoupling core logic from external dependencies (UI, Filesystem, SQL Server) to enable autonomous testing:

- **Interface Extraction**: Components like `SqlDatabaseMetaDataRetriever` and `SqlDatabaseVersionVerifier` have been abstracted behind interfaces (`ISqlDatabaseMetaDataRetriever`, `ISqlDatabaseVersionVerifier`).
- **Loader Decoupling**: Connection loaders no longer trigger interactive UI (e.g., `MiscTools.PasswordDialog`) directly. Instead, they accept a `Func<string, Optional<SecureString>>` delegate for authentication requests, which can be mocked in automated tests.
- **Storage Abstraction**: By utilizing `IDataProvider<T>` and mocked database connectors, the SQL loading logic can be verified in headless environments without a running SQL Server instance.


### UI Layer (`UI/`)

- **`FrmMain`** — Main application form. Hosts the `DockPanel` (WeifenLuo), toolbars, and menus.
- **Windows** — `ConnectionTreeWindow`, `ConfigWindow`, `ErrorAndInfoWindow`, `ConnectionWindow`, etc. All extend `BaseWindow` (which extends `DockContent`).
- **Controls** — `ConnectionTree` (ObjectListView-based tree), `QuickConnectToolStrip`, `MultiSshToolStrip`, `ExternalToolsToolStrip`, custom `mrng*` themed controls.
- **Menus** — `FileMenu`, `ViewMenu`, `ToolsMenu`, `HelpMenu` — self-contained `ToolStripMenuItem` subclasses.

### Theming (`Themes/`)

- **`ThemeManager`** — Singleton that loads `.vstheme` XML files, manages active theme, and fires `ThemeChanged` events.
- **`ThemeInfo`** — Represents a theme with a DockPanel `Theme` object and an `ExtendedPalette` for custom colors.
- All `mrng*` controls and windows subscribe to `ThemeChanged` to update their appearance live.

### External Connectors (`ExternalConnectors/`)

Credential provider integrations that implement `ICredentialProvider`:
- **1Password CLI** — Parses `op item list/get` JSON output
- **SecretServer** — REST API client (Delinea/Thycotic)
- **Passwordstate** — REST API client (Click Studios)
- **OpenBao** — HashiCorp Vault-compatible API

### Application Bootstrap (`App/`)

- **`ProgramRoot`** — Entry point. Registers encodings, resolves assemblies, creates `FrmMain`.
- **`Runtime`** — Static service locator providing `ConnectionsService`, `ConnectionInitiator`, `MessageCollector`, `CredentialProviderCatalog`, etc.
- **`AppWindows`** — Factory/registry for singleton tool windows.

## Data Flow

### Opening a Connection

1. User double-clicks a node in `ConnectionTree` or uses Quick Connect
2. `ConnectionInitiator.OpenConnection()` is called with a `ConnectionInfo`
3. `ConnectionAuditLogger.LogConnectionAttempt()` records the attempt
4. Protocol instance is created based on `ConnectionInfo.Protocol`
5. A `ConnectionWindow` panel is created (or reused)
6. Protocol control is hosted in an `InterfaceControl` on a `ConnectionTab`
7. Protocol events fire back to `ConnectionInitiator` for lifecycle management

### Saving/Loading Connections

1. On save: `ConnectionTreeModel` -> `XmlConnectionsSerializer` -> encrypted XML -> `FileDataProvider` -> disk
2. On load: disk -> `FileDataProvider` -> encrypted XML -> `XmlConnectionsDeserializer` -> `ConnectionTreeModel`
3. SQL mode follows the same pattern but uses `DataTableSerializer`/`SqlDataProvider`

## Key Design Decisions

- **COM Interop for RDP**: Uses `MSTSCLib` ActiveX control, requiring full MSBuild (not `dotnet build`)
- **BouncyCastle for crypto**: Chosen for AEAD support unavailable in .NET's built-in libraries at the time
- **WeifenLuo DockPanel Suite**: Provides VS-style docking, tabbed windows, and theme support
- **ObjectListView**: Used for the connection tree instead of standard TreeView for virtual mode and custom rendering
- **Testable Architecture**: Strategic move towards dependency injection and interface extraction for core services, enabling high-coverage automated testing without backend infrastructure.
