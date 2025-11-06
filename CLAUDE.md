# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

mRemoteNG is an open-source, multi-protocol, tabbed remote connections manager for Windows. It supports 14+ protocols including RDP, VNC, SSH, Telnet, HTTP/HTTPS, and more.

- **Technology**: .NET 9.0 Windows Forms + WPF hybrid application
- **Architectures**: x64 and ARM64
- **Target OS**: Windows 10.0.26100.0+

## Common Development Commands

### Building the Project

**Important**: Always use PowerShell with Visual Studio MSBuild for building this project.

```powershell
# Restore NuGet packages
powershell.exe -Command "cd 'C:\Data\Temp\Source\mRemoteNG'; & 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' mRemoteNG.sln -t:Restore -p:Configuration=Debug -p:Platform=x64"

# Build Debug configuration (x64)
powershell.exe -Command "cd 'C:\Data\Temp\Source\mRemoteNG'; & 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' mRemoteNG.sln -p:Configuration=Debug -p:Platform=x64 -v:minimal"

# Build Release configuration (x64)
powershell.exe -Command "cd 'C:\Data\Temp\Source\mRemoteNG'; & 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' mRemoteNG.sln -p:Configuration=Release -p:Platform=x64 -v:minimal"

# Build for ARM64
powershell.exe -Command "cd 'C:\Data\Temp\Source\mRemoteNG'; & 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' mRemoteNG.sln -p:Configuration=Release -p:Platform=ARM64 -v:minimal"
```

**Note**: The build process includes T4 template transformation for `mRemoteNG/Properties/AssemblyInfo.tt`. This is typically handled by Visual Studio or the CI pipeline.

### Running Tests

**Important**: Always use PowerShell with Visual Studio tools for running tests.

```powershell
# Build test project first
powershell.exe -Command "cd 'C:\Data\Temp\Source\mRemoteNG'; & 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' mRemoteNGTests/mRemoteNGTests.csproj -p:Configuration=Debug -p:Platform=x64 -v:minimal"

# Run all unit tests using VSTest (NUnit)
powershell.exe -Command "cd 'C:\Data\Temp\Source\mRemoteNG'; $env:PATH = 'C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\Extensions\TestPlatform' + ';' + $env:PATH; & vstest.console.exe 'mRemoteNGTests\bin\x64\Debug\net9.0-windows10.0.26100.0\mRemoteNGTests.dll'"

# Run a specific test class
powershell.exe -Command "cd 'C:\Data\Temp\Source\mRemoteNG'; $env:PATH = 'C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\Extensions\TestPlatform' + ';' + $env:PATH; & vstest.console.exe 'mRemoteNGTests\bin\x64\Debug\net9.0-windows10.0.26100.0\mRemoteNGTests.dll' /TestCaseFilter:'ClassName~OptionsFormTests'"

# Run with detailed output
powershell.exe -Command "cd 'C:\Data\Temp\Source\mRemoteNG'; $env:PATH = 'C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\Extensions\TestPlatform' + ';' + $env:PATH; & vstest.console.exe 'mRemoteNGTests\bin\x64\Debug\net9.0-windows10.0.26100.0\mRemoteNGTests.dll' /Logger:'console;verbosity=detailed'"
```

**Test Frameworks**:
- Unit tests: NUnit 3 with NSubstitute for mocking
- BDD tests: Gherkin/Cucumber in mRemoteNGSpecs project

### Build Configurations

- **Debug|x64** / **Debug|arm64**: Debug builds with symbols
- **Release|x64** / **Release|arm64**: Production builds
- **Release Portable**: Standalone portable build (no installer)
- **Release Installer and Portable**: MSI installer + portable ZIP

## High-Level Architecture

### Project Structure

The solution consists of 3 main projects:

1. **mRemoteNG** (main application): Contains all UI, business logic, and protocol implementations
2. **ExternalConnectors**: Pluggable credential/authentication providers (AWS, CyberArk, Vault, etc.)
3. **ObjectListView.NetCore**: Custom UI control library for enhanced list views

### Architectural Layers

```
┌─────────────────────────────────────────┐
│  UI Layer (mRemoteNG/UI/)               │
│  Forms, Controls, Panels, Tabs          │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│  Business Logic                         │
│  Connection management, Containers      │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│  Protocol Abstraction                   │
│  ProtocolBase, 14+ protocol types       │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│  Data Access (mRemoteNG/Config/)        │
│  XML/SQL/CSV serialization              │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│  Cross-cutting (Security, Tools, etc.)  │
└─────────────────────────────────────────┘
```

### Core Concepts

#### Connection Model

The connection model uses an inheritance-based hierarchy:

- **AbstractConnectionRecord**: Base class for all connections
  - Contains ~200+ properties covering all protocols
  - Implements `INotifyPropertyChanged` for UI data binding

- **ConnectionInfo**: Concrete connection implementation
  - Represents a single connection (RDP, VNC, SSH, etc.)
  - Supports property inheritance from parent containers
  - `Inheritance` property controls which properties inherit from parent

- **ContainerInfo**: Folder/group of connections
  - Organizes connections hierarchically
  - Can set default values that children inherit
  - Implements parent-child relationships

#### Protocol Architecture

All protocols extend **ProtocolBase** and follow a factory pattern:

- **ProtocolFactory**: Creates protocol instances based on `ProtocolType` enum
- **ProtocolBase**: Abstract base with common lifecycle events
  - Events: `Connecting`, `Connected`, `Disconnected`, `Error`, `Closing`, `Closed`
  - Manages `InterfaceControl` for rendering the connection UI
  - Handles reconnection logic with timers

Supported protocols are in `mRemoteNG/Connection/Protocol/`:
- RDP: `Connection.Protocol.RDP/` (native Windows RDP via COM)
- VNC: `Connection.Protocol.VNC/` (VncSharpCore library)
- SSH: `Connection.Protocol.SSH/` (SSH.NET library, supports SSH1/SSH2)
- And 11+ more protocol types

#### Configuration/Persistence

Configuration uses a provider pattern with multiple storage backends:

- **FileDataProvider**: XML file storage (default: `~/.mRemoteNG/confcons.xml`)
  - AES-256 encryption for sensitive data
  - Versioned schema (v2.8): `mRemoteNG/Schemas/mremoteng_confcons_v2_8.xsd`

- **SqlDataProvider**: SQL Server/MySQL database storage for multi-user scenarios
  - Remote synchronization via `RemoteConnectionsSyncronizer`

- **CSV Export**: `CsvConnectionsSerializerMremotengFormat` for bulk export

Serializers are versioned and maintain backward compatibility. The XML format uses inheritance attributes (e.g., `InheritConnectionFrameColor="true"`) to track which properties are inherited.

#### Security

Security components in `mRemoteNG/Security/`:

- **Encryption**: AES-256 via `ICryptographyProvider`
- **Password storage**: `SecureString` wrappers and DPAPI encryption
- **Master password**: Protects connection file encryption keys
- **External credentials**: Integration with Vault, AWS, CyberArk via ExternalConnectors

### Key Directories

| Directory | Purpose |
|-----------|---------|
| `mRemoteNG/App/` | Application startup, initialization, DI setup |
| `mRemoteNG/Connection/` | Connection model, protocol abstractions |
| `mRemoteNG/Connection/Protocol/` | Protocol implementations (RDP, VNC, SSH, etc.) |
| `mRemoteNG/Config/` | Configuration loading/saving, serializers |
| `mRemoteNG/Config/Serializers/` | XML/SQL/CSV serialization logic |
| `mRemoteNG/Container/` | Folder/group model |
| `mRemoteNG/Credential/` | Credential records and repositories |
| `mRemoteNG/Security/` | Cryptography, authentication |
| `mRemoteNG/Tree/` | Tree model for connection hierarchy |
| `mRemoteNG/UI/` | All Windows Forms UI components |
| `mRemoteNG/UI/Forms/` | Main windows and dialogs |
| `mRemoteNG/UI/Controls/` | Custom controls (ConnectionTree, etc.) |
| `mRemoteNG/UI/Panels/` | Dockable panels (DockPanelSuite) |
| `mRemoteNG/Tools/` | Utilities and helpers |
| `ExternalConnectors/` | External credential provider plugins |
| `mRemoteNGTests/` | NUnit unit tests |

## Important Patterns and Conventions

### Design Patterns

- **Factory Pattern**: `ProtocolFactory`, credential providers
- **Strategy Pattern**: Protocol implementations, serializers
- **Observer Pattern**: `INotifyPropertyChanged` for UI binding, protocol events
- **Repository Pattern**: `IConnectionsLoader`/`ISaver` for data access
- **Template Method**: `ProtocolBase` with virtual methods for subclasses

### Property Inheritance

mRemoteNG has a sophisticated property inheritance system where child connections inherit properties from parent containers:

- Each property has a corresponding `Inherit{PropertyName}` boolean
- When `Inherit{PropertyName}` is true, the value comes from the parent
- Serializers must handle both the value AND the inheritance flag
- When adding new properties to `AbstractConnectionRecord`:
  1. Add the property itself
  2. Add `Inherit{PropertyName}` to `ConnectionInfoInheritance`
  3. Update XML/CSV serializers to include both
  4. Add localization strings to `Language/Language.resx`

Example from the Connection Frame Color feature:
```csharp
// In AbstractConnectionRecord.cs
public ConnectionFrameColor ConnectionFrameColor { get; set; }

// In ConnectionInfoInheritance.cs
public bool InheritConnectionFrameColor { get; set; }

// In XmlConnectionNodeSerializer28.cs
writer.WriteAttributeString("ConnectionFrameColor", connectionInfo.ConnectionFrameColor.ToString());
writer.WriteAttributeString("InheritConnectionFrameColor", connectionInfo.Inheritance.InheritConnectionFrameColor.ToString());
```

### Serialization Versioning

When modifying the XML schema:
- Update `XmlConnectionNodeSerializer28.cs` (or create v29 if breaking changes)
- Update `XmlConnectionsDeserializer.cs` to handle new attributes
- Maintain backward compatibility (old files should still load)
- Update `mRemoteNG/Schemas/mremoteng_confcons_v2_8.xsd` if needed

### Localization

All user-facing strings go in `mRemoteNG/Language/Language.resx`:
```xml
<data name="ConnectionFrameColor" xml:space="preserve">
  <value>Connection Frame Color</value>
</data>
```

Access via `Language.strConnectionFrameColor` in code.

## CI/CD Pipeline

GitHub Actions workflow: `.github/workflows/Build_mR-NB.yml`

- **Triggers**:
  - Manual dispatch
  - Push to `v1.78.2-dev` with "NB release" in commit message

- **Matrix builds**: Windows Latest (x64) + Windows 11 ARM (arm64)

- **Steps**:
  1. Checkout code
  2. Setup MSBuild (VS 17.14.12)
  3. Transform T4 templates (`dotnet-t4`)
  4. Restore NuGet packages
  5. Build Release configuration
  6. Extract version from AssemblyInfo.cs
  7. Extract changelog section
  8. Create ZIP archive
  9. Create GitHub pre-release with artifacts

## Development Workflow

### Adding a New Protocol

1. Create new class in `mRemoteNG/Connection/Protocol/` extending `ProtocolBase`
2. Override required methods: `Connect()`, `Disconnect()`, etc.
3. Add to `ProtocolType` enum
4. Update `ProtocolFactory.CreateProtocol()` switch statement
5. Add protocol-specific properties to `AbstractConnectionRecord` if needed
6. Add UI controls for protocol settings in PropertyGrid
7. Add tests in `mRemoteNGTests/`

### Adding a New Connection Property

1. Add property to `AbstractConnectionRecord` (categorize with `[Category("Display")]`)
2. Add inheritance flag to `ConnectionInfoInheritance`
3. Update serializers:
   - `XmlConnectionNodeSerializer28.cs` (write)
   - `XmlConnectionsDeserializer.cs` (read)
   - `CsvConnectionsSerializerMremotengFormat.cs` (CSV export)
4. Add localization strings to `Language/Language.resx`
5. Add tests for serialization round-trip

See `IMPLEMENTATION_NOTES.md` for a detailed example of adding the Connection Frame Color feature.

### Working with External Credential Providers

External credential providers are in the `ExternalConnectors` project:

- **AWS EC2**: Uses AWSSDK.EC2 to fetch instance credentials
- **CyberArk**: Integrates with CyberArk CPS
- **Vault/OpenBao**: Uses VaultSharp for HashiCorp Vault

To add a new provider:
1. Create new class implementing credential repository interface
2. Add to `ExternalConnectors` project
3. Register in credential service factory
4. Add configuration UI in mRemoteNG settings

## Debugging and Troubleshooting

- **Log files**: Check `log4net.config` for logging configuration
- **Encryption issues**: Master password is required to decrypt connection files
- **Protocol errors**: Enable verbose logging in protocol implementations
- **UI binding issues**: Check `INotifyPropertyChanged` events are firing correctly

## Additional Resources

- **Main documentation**: `README.md`
- **Implementation notes**: `IMPLEMENTATION_NOTES.md` (detailed example of Connection Frame Color feature)
- **Visual examples**: `VISUAL_EXAMPLES.md`
- **Panel binding**: `PANEL_BINDING_FEATURE.md`
- **Changelog**: `CHANGELOG.md`
- **Online docs**: https://mremoteng.readthedocs.io/
