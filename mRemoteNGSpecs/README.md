# mRemoteNGSpecs — end-to-end & specification tests

NUnit-based test project covering the embedded **SFTP browser** and **xterm.js SSH
terminal** (PR #3258), plus the original credential-repository specifications.

## Layers

| Area | Tooling | What it does |
|------|---------|--------------|
| BDD / Cucumber | [Reqnroll](https://reqnroll.net) (`.feature` files → NUnit) | `SftpFileOperations.feature` drives `SftpFileService` against a live SFTP server; `CredentialRepository*.feature` cover the credential repositories |
| Browser E2E | [Playwright for .NET](https://playwright.dev/dotnet/) | Loads the real xterm.js terminal page in Chromium with a stubbed `chrome.webview` bridge; exercises rendering, the host↔page message protocol, ANSI colour, and keyboard input |
| Visual regression | [Verify](https://github.com/VerifyTests/Verify) | A deterministic text snapshot of the rendered terminal buffer and a PNG screenshot snapshot (Playwright captures, Verify compares) |
| Live SFTP server | Docker (`atmoz/sftp`) | Disposable container, started/stopped automatically around the `@sftp` feature |

## Prerequisites

- .NET 10 SDK and **MSBuild** (the referenced `mRemoteNG` project uses T4 templating,
  so build with `MSBuild.exe`, not `dotnet build`).
- **Playwright browsers** — once, after the first build:
  ```pwsh
  pwsh mRemoteNGSpecs/bin/x64/Release/playwright.ps1 install chromium
  ```
- **Docker** — only for the `@sftp` scenarios. If Docker is unavailable those
  scenarios are *ignored*, not failed, so the rest of the suite still runs.

## Running

```pwsh
# everything
dotnet test mRemoteNGSpecs/mRemoteNGSpecs.csproj -c Release -p:Platform=x64 --no-build

# just the xterm.js browser tests
dotnet test ... --filter "FullyQualifiedName~XtermTerminalTests"

# just the SFTP Cucumber scenarios (needs Docker)
dotnet test ... --filter "TestCategory=sftp"

# skip the pixel screenshot baseline (e.g. on a host with different fonts)
dotnet test ... --filter "TestCategory!=Visual"
```

## Visual baselines

Verify snapshots live next to `XtermTerminalTests.cs`:

- `*.verified.txt` — rendered terminal text; deterministic and portable.
- `*.verified.png` — terminal screenshot; **rendering depends on OS/fonts**, so
  regenerate it on the target environment if the pixel diff fails.

To (re)generate a baseline, delete the `*.verified.*` file (or accept the
`*.received.*` Verify produces on mismatch) and re-run the test. `*.received.*`
files are git-ignored and must never be committed.
