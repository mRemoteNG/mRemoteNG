# GitHub Copilot Instructions for mRemoteNG

## Project Overview

mRemoteNG is an open-source, multi-protocol, tabbed remote connections manager for Windows. It's a fork of mRemote that allows users to view and manage all their remote connections (RDP, VNC, SSH, Telnet, HTTP/HTTPS, rlogin, Raw Socket, PowerShell remoting, AnyDesk) in a simple yet powerful interface.

## Technology Stack

- **Language**: C# with latest language version
- **Framework**: .NET 10.0 for Windows (net10.0-windows10.0.26100.0)
- **UI Framework**: Windows Forms with WPF support
- **Target Platforms**: x64 and ARM64
- **Testing Framework**: NUnit with NSubstitute for mocking
- **Build System**: MSBuild with .NET SDK-style projects

## Building the Project

### Prerequisites
- Visual Studio 2026 (version 18.4.0 or later)
- .NET 10.0 Desktop Runtime
- Windows 10/11 or Windows Server 2022+

### Build Commands
```powershell
# Restore NuGet packages
dotnet restore

# Build the solution
msbuild mRemoteNG.sln -p:Configuration=Release -p:Platform=x64

# Or for ARM64
msbuild mRemoteNG.sln -p:Configuration=Release -p:Platform=arm64
```

### Running Tests
The project uses NUnit for testing. Test projects are in `mRemoteNGTests/` directory.

```powershell
dotnet test mRemoteNGTests/mRemoteNGTests.csproj
```

## Code Organization

### Main Projects
- **mRemoteNG**: Main application project
- **mRemoteNGTests**: Unit and integration tests
- **mRemoteNGSpecs**: Specification tests
- **ObjectListView.NetCore**: Custom list view control
- **ExternalConnectors**: External protocol connector implementations
- **mRemoteNGDocumentation**: reStructuredText documentation

### Key Directories in mRemoteNG Project
- `App/`: Application startup and initialization
- `Connection/`: Connection models, protocols, and management
- `Config/`: Configuration and serialization (XML, CSV)
- `Container/`: Container/folder node implementations
- `Credential/`: Credential storage and repositories
- `Language/`: Localization resource files (.resx)
- `Security/`: Authentication, encryption, and password management
- `Tree/`: Connection tree UI and management
- `UI/`: User interface components and forms
- `Tools/`: Utility classes and helpers

## Code Style and Conventions

### EditorConfig Settings
The project uses EditorConfig (`.editorconfig` in `mRemoteNG/` directory) with these key rules:

- **Indentation**: 4 spaces (no tabs)
- **Line Endings**: CRLF (Windows-style)
- **Charset**: UTF-8 with BOM for C# files
- **New Lines**: Opening braces on new lines (Allman style)
- **Using Directives**: Sort System directives first
- **this. Qualifier**: Do not use `this.` prefix unless necessary

### Naming Conventions
- **Classes/Interfaces**: PascalCase (e.g., `ConnectionInfo`, `IInheritable`)
- **Methods**: PascalCase (e.g., `GetConnection`, `SaveToXml`)
- **Properties**: PascalCase (e.g., `ConnectionName`, `Port`)
- **Fields**: Use camelCase for private fields, consider `_` prefix for backing fields
- **Constants**: PascalCase

### Code Patterns

#### Inheritance System
mRemoteNG has a sophisticated property inheritance system for connection settings:

1. Properties can be inherited from parent containers/folders
2. Each inheritable property has a corresponding `Inherit<PropertyName>` boolean in `ConnectionInfoInheritance` class
3. Use `IInheritable` interface for objects that support inheritance
4. Example: `ConnectionFrameColor` property has `InheritConnectionFrameColor` boolean

#### Serialization
The project supports multiple serialization formats:

**XML Serialization** (`Config/Serializers/ConnectionSerializers/Xml/`):
- Node-based serializers (e.g., `XmlConnectionNodeSerializer28.cs`)
- Deserializers handle backward compatibility
- Attributes for properties and inheritance flags
- Always maintain backward compatibility - old files must still load

**CSV Serialization** (`Config/Serializers/ConnectionSerializers/Csv/`):
- Export connections to CSV format
- Include both value and inheritance columns
- Maintain column order consistency

#### Localization
All user-facing strings must be localized:

1. Add entries to `Language/Language.resx` (English base)
2. Use `Language.ResourceName` to access strings in code
3. Resource naming:
   - Properties: `PropertyName` (e.g., `ConnectionFrameColor`)
   - Descriptions: `PropertyDescription<PropertyName>`
   - Enum values: `<EnumName><Value>` (e.g., `FrameColorRed`)
4. Provide clear, concise descriptions for PropertyGrid tooltips

### Common Patterns

#### Adding a New Connection Property

1. **Add enum** (if needed) in `Connection/` directory
2. **Add property** to `AbstractConnectionRecord.cs` or `ConnectionInfo.cs`
   - Add `[Category("CategoryName")]` attribute for PropertyGrid grouping
   - Add `[Description("Language.PropertyDescription<Name>")]` for tooltip
   - Use appropriate TypeConverter if needed
3. **Add inheritance support** in `ConnectionInfoInheritance.cs`
4. **Update serializers**:
   - XML: Update latest `XmlConnectionNodeSerializer*.cs` and `XmlConnectionsDeserializer.cs`
   - CSV: Update `CsvConnectionsSerializerMremotengFormat.cs`
5. **Add localization** in `Language/Language.resx`
6. **Write tests** in `mRemoteNGTests/Connection/`
7. **Update documentation** in `mRemoteNGDocumentation/` if user-facing

#### UI Controls
- Prefer existing mRemoteNG patterns for UI controls
- Connection panels use `InterfaceControl` base class
- Custom painting: Override `OnPaint` or handle `Paint` event
- Follow Windows Forms best practices

## Testing Guidelines

### Test Structure
- Use NUnit's `[Test]` attribute for test methods
- Use `[SetUp]` and `[TearDown]` for test initialization/cleanup
- Use `[TestCase]` for parameterized tests
- Use NSubstitute for mocking: `Substitute.For<IInterface>()`

### Test Naming
- Use descriptive names: `MethodName_Scenario_ExpectedBehavior`
- Example: `ConnectionInfo_SetPassword_EncryptsValue`

### Test Organization
Mirror the main project structure in `mRemoteNGTests/`:
- `Connection/` for connection tests
- `Security/` for security tests
- `Config/` for configuration/serialization tests

## Important Notes and Pitfalls

### Backward Compatibility
- **Critical**: Never break loading of old connection files
- Always provide default values for new properties
- Test that files without new attributes still load correctly

### Security
- Sensitive data (passwords, credentials) must be encrypted
- Use existing security providers in `Security/` namespace
- Never log or expose credentials in plain text

### Performance
- Connection tree can contain thousands of nodes
- Optimize for large connection files
- Avoid unnecessary UI refreshes
- Use async/await for I/O operations

### Platform-Specific Code
- The project targets both x64 and ARM64
- Avoid platform-specific code unless absolutely necessary
- Test on both platforms when possible

### External Dependencies
- PuTTY for SSH/Telnet (bundled)
- Terminal Service Client for RDP
- Various protocol-specific libraries (see CREDITS.md)

## Documentation

### User Documentation
- Located in `mRemoteNGDocumentation/`
- Written in reStructuredText (.rst)
- Follows ReadTheDocs format
- Include screenshots and examples for new features

### Code Documentation
- Use XML documentation comments for public APIs
- Document complex algorithms and business logic
- Keep implementation notes in `IMPLEMENTATION_NOTES.md` for significant features

## Common Tasks

### Adding a Protocol
1. Create protocol implementation in `Connection/Protocol/`
2. Add protocol enum value to protocol types
3. Implement connection logic
4. Add UI controls if needed
5. Update documentation

### Adding a Theme
1. Add theme files to `Themes/` directory
2. Update theme manager
3. Add documentation in `mRemoteNGDocumentation/themes/`

### Updating Dependencies
- Dependencies are centrally managed in `Directory.Packages.props`
- Use `dotnet restore` after updating
- Test thoroughly after dependency updates

## Version and Release

- Current development version: 1.78.2-dev
- Version is set in `mRemoteNG.csproj`
- Builds are automated via GitHub Actions (`.github/workflows/`)
- Changelog maintained in `CHANGELOG.md`

## Getting Help

- Project website: https://mremoteng.org
- Documentation: https://mremoteng.readthedocs.io
- GitHub Issues: Report bugs and feature requests
- Community: Reddit r/mRemoteNG, Matrix chat

## Summary

When contributing to mRemoteNG:
1. Follow the existing code style (EditorConfig)
2. Maintain backward compatibility with old connection files
3. Localize all user-facing strings
4. Write tests for new functionality
5. Update documentation for user-facing changes
6. Test on both x64 and ARM64 if possible
7. Keep security and performance in mind
