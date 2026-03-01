# Contributing to mRemoteNG

Thank you for your interest in contributing to mRemoteNG! This guide will help you get started.

## Getting Started

### Prerequisites

- **Windows 10/11** (mRemoteNG is a Windows-only WinForms application)
- **Visual Studio 2022 or later** with the following workloads:
  - .NET desktop development
  - Desktop development with C++  (for COM reference support)
- **.NET 10 SDK** (or later)
- **Git**

### Building

> **Important:** Do NOT use `dotnet build` — it fails on COM references (`MSB4803`).
> mRemoteNG requires full MSBuild from Visual Studio.

```powershell
# Clone the repository
git clone https://github.com/mRemoteNG/mRemoteNG.git
cd mRemoteNG

# Build using the build script (auto-detects VS installation)
pwsh -NoProfile -ExecutionPolicy Bypass -File build.ps1
```

The output will be in `mRemoteNG/bin/x64/Release/`.

### Running Tests

```powershell
# Build test projects (from VS Developer Command Prompt)
msbuild mRemoteNGTests\mRemoteNGTests.csproj -restore -m "-verbosity:minimal" "-p:Configuration=Release" "-p:Platform=x64"

# Run tests (use the DLL path, not the .csproj)
dotnet test "mRemoteNGTests\bin\x64\Release\mRemoteNGTests.dll" --verbosity normal
```

## Branch Naming Convention

| Prefix | When | Example |
|--------|------|---------|
| `fix/<issue>-<desc>` | Bug fix | `fix/2735-rdp-smartsize-focus` |
| `feat/<issue>-<desc>` | New feature | `feat/1634-protocol-token` |
| `security/<desc>` | Security hardening | `security/ldap-sanitizer` |
| `chore/<desc>` | Infra, deps, CI | `chore/sqlclient-sni-runtime` |

**Rules:**
- Include the issue number after `/` when an issue exists
- Lowercase, kebab-case, max 50 chars after prefix
- No tool prefixes (`codex/`, `copilot/`) in branch names

## Pull Request Workflow

1. Fork the repository
2. Create a branch from `v1.78.2-dev`: `git checkout -b fix/<issue>-<desc>`
3. Make your changes
4. Ensure all tests pass locally
5. Push and create a PR targeting `v1.78.2-dev` on upstream

### PR Guidelines

- Keep PRs focused — one fix or feature per PR
- Include the issue number in the PR title (e.g., "Fix #2735: RDP SmartSize focus loss")
- Add tests for new functionality
- Don't introduce new warnings

## Architecture Overview

See [ARCHITECTURE.md](ARCHITECTURE.md) for a detailed breakdown of the system layers.

Key directories:
- `mRemoteNG/Connection/` — Connection data model and protocol implementations
- `mRemoteNG/Config/` — Serializers, settings, and data providers
- `mRemoteNG/Security/` — Encryption, key derivation, credential handling
- `mRemoteNG/UI/` — WinForms controls, windows, menus, and theme support
- `mRemoteNG/Tree/` — Connection tree model and operations
- `ExternalConnectors/` — Vault integrations (1Password, SecretServer, Passwordstate)

## Code Style

- Follow existing patterns in the codebase
- Use `[SupportedOSPlatform("windows")]` on all new public classes
- Prefer `new()` target-typed construction where the type is obvious
- Use file-scoped namespaces only in new files if the surrounding code uses them
- No unnecessary `using` statements

## Reporting Issues

- Use [GitHub Issues](https://github.com/mRemoteNG/mRemoteNG/issues) to report bugs
- Include your mRemoteNG version, OS version, and steps to reproduce
- Attach the error log from `%AppData%\mRemoteNG\` if applicable
