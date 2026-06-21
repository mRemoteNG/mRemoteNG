# mRemoteNG Deployment Options

This document explains the two deployment options for mRemoteNG and how to build each version.

## Deployment Types

### 1. Framework-Dependent (FD)
**File suffix: `-FD.zip`**

- **Size**: ~15-25 MB
- **Requirements**: User must have .NET 10 Desktop Runtime installed
- **Startup**: Application checks for .NET runtime and prompts user to download if missing
- **Use case**: Standard release for users comfortable installing prerequisites

### 2. Self-Contained (SC)
**File suffix: `-SC.zip`**

- **Size**: ~80-150 MB
- **Requirements**: None - includes .NET 10 runtime
- **Startup**: No runtime checks performed (runtime is bundled)
- **Use case**: Portable version for users who want zero installation/configuration

## Building Locally

### Framework-Dependent Build

```powershell
# x64
msbuild mRemoteNG.sln -p:Configuration=Release -p:Platform=x64

# ARM64
msbuild mRemoteNG.sln -p:Configuration=Release -p:Platform=ARM64
```

### Self-Contained Build

```powershell
# x64
dotnet publish mRemoteNG\mRemoteNG.csproj `
  --configuration Release `
  --runtime win-x64 `
  --self-contained true `
  -p:Platform=x64 `
  -p:PublishSingleFile=false `
  -p:PublishReadyToRun=true `
  -p:DefineConstants="SELF_CONTAINED"

# ARM64
dotnet publish mRemoteNG\mRemoteNG.csproj `
  --configuration Release `
  --runtime win-arm64 `
  --self-contained true `
  -p:Platform=ARM64 `
  -p:PublishSingleFile=false `
  -p:PublishReadyToRun=true `
  -p:DefineConstants="SELF_CONTAINED"
```

## GitHub Actions Workflow

The new workflow file `Build_and_Release_mR-NB-MultiDeploy.yml` automatically builds all four variants:

1. **x64 Framework-Dependent** - `mRemoteNG-YYYYMMDD-vX.X.X-NB-XXXX-x64-FD.zip`
2. **x64 Self-Contained** - `mRemoteNG-YYYYMMDD-vX.X.X-NB-XXXX-x64-SC.zip`
3. **ARM64 Framework-Dependent** - `mRemoteNG-YYYYMMDD-vX.X.X-NB-XXXX-arm64-FD.zip`
4. **ARM64 Self-Contained** - `mRemoteNG-YYYYMMDD-vX.X.X-NB-XXXX-arm64-SC.zip`

### Workflow Triggers

The workflow runs when:
- You push to `v1.78.2-dev` branch with commit message containing "NB release"
- You manually trigger via workflow_dispatch

### Release Output

All four zip files are uploaded to a single GitHub Release with clear descriptions:
- Framework-Dependent versions are marked as requiring .NET 10 Runtime
- Self-Contained versions are marked as portable/no installation needed

## Code Changes

### ProgramRoot.cs

The `MainAsync` method now uses conditional compilation:

```csharp
#if !SELF_CONTAINED
    // Runtime check code only included in Framework-Dependent builds
    // Checks for .NET Runtime and Visual C++ Redistributable
#endif
```

When building with `-p:DefineConstants="SELF_CONTAINED"`, the runtime checks are completely excluded from the compiled binary.

## Recommendations

### For Users

**Choose Framework-Dependent (FD) if:**
- You don't mind installing .NET 10 Desktop Runtime once
- You want smaller download size
- You're using multiple .NET applications (runtime is shared)

**Choose Self-Contained (SC) if:**
- You want zero installation/setup
- You need portable deployment (USB drive, restricted environments)
- You don't want to deal with prerequisites

### For Distribution

Consider offering both options:
- Make Framework-Dependent the **default/recommended** option (smaller, faster updates)
- Offer Self-Contained as **portable alternative** for special use cases

## Technical Details

### Compilation Symbols

- Framework-Dependent builds: No special symbols
- Self-Contained builds: `SELF_CONTAINED` symbol defined

### Runtime Identifiers

- x64: `win-x64`
- ARM64: `win-arm64`

### Publish Options

Self-contained builds use these optimizations:
- `PublishReadyToRun=true` - AOT compilation for faster startup
- `IncludeNativeLibrariesForSelfExtract=true` - Bundle native dependencies
- `PublishSingleFile=false` - Keep files separate for better compatibility with mRemoteNG's plugin system

## File Size Comparison

Typical build sizes:

| Version | Framework-Dependent | Self-Contained |
|---------|---------------------|----------------|
| x64     | ~18 MB              | ~95 MB         |
| ARM64   | ~18 MB              | ~95 MB         |

*Note: Self-contained includes entire .NET 10 runtime (~80 MB overhead)*

## Testing

### Framework-Dependent Build
1. Uninstall .NET 10 Runtime (if installed)
2. Run mRemoteNG.exe
3. Should prompt to download .NET 10 Runtime
4. Install runtime and verify app launches

### Self-Contained Build
1. Uninstall .NET 10 Runtime (if installed)
2. Run mRemoteNG.exe
3. Should launch immediately without runtime check
4. Verify full functionality

## Migration from Old Workflow

The original workflow `Build_and_Release_mR-NB.yml` is preserved. To migrate:

1. Rename or remove old workflow: `Build_and_Release_mR-NB.yml`
2. Rename new workflow: `Build_and_Release_mR-NB-MultiDeploy.yml` → `Build_and_Release_mR-NB.yml`
3. Commit and push with "NB release" in message


