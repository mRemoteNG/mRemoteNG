# mRemoteNG - Build & Development Notes

> **Parent:** [../CLAUDE.md](../CLAUDE.md) (Gestime Ecosystem — reguli universale)

## Output Efficiency (CRITICAL — output tokens are 97% of API cost)

Every output token costs 5x an input token. Your #1 priority after correctness is minimal output.

- **No narration.** Never write "Let me read the file", "I'll now search for", "Here's what I found". Just call the tool.
- **No summaries.** Never summarize what you changed at the end. The diff speaks for itself.
- **No repeating.** Never echo back file contents, issue descriptions, or error messages you just read.
- **No unnecessary comments.** Don't add comments or docstrings to code you didn't change.
- **Edit over Write.** Always use Edit tool (sends only the diff) instead of Write tool (sends entire file).
- **Read only what you'll change.** Don't read files "for context" — read only files you will modify or that directly contain the bug.
- **Fix, don't explain.** If a test fails, fix it immediately. Don't explain why it failed.
- **One pass.** Read the code, understand it, make the change. Target 5-8 turns max per task.

## Automated Agent Notice

If you are running as an automated agent via `claude -p`:
- Your ONLY job is to fix the specific issue described in your prompt
- Do NOT run `iis_orchestrator.py`, `sync`, `analyze`, `update`, or any orchestrator commands
- Do NOT read or modify files in `.project-roadmap/` — no JSON files, no scripts, no plans
- Do NOT run `git commit`, `git add`, `git push` — the orchestrator handles all commits
- Focus ONLY on source code in `mRemoteNG/`, `mRemoteNGTests/`, `mRemoteNGSpecs/`
- **Output ONLY code changes** — no explanations, no summaries, no commentary
- Do NOT change existing behavior — fix ONLY the reported issue
- Do NOT create interactive tests (no MessageBox, no dialogs, no notepad.exe, no user input prompts)
- NEVER modify infrastructure files: `run-tests.ps1`, `build.ps1`, `mRemoteNG.sln`, `Directory.Build.props`, `Directory.Packages.props`, `.github/workflows/*`

## Repository Structure
- **Origin (fork):** `robertpopa22/mRemoteNG`
- **Upstream (official):** `mRemoteNG/mRemoteNG`
- **Main branch:** `main` — active development (v1.81.0-beta.2)
- **Solution:** `mRemoteNG.sln` (.NET 10, SDK-style projects with COM references)

## Build Instructions

**Do NOT use `dotnet build`** — fails with `MSB4803` on COM references (`MSTSCLib` RDP ActiveX control). Must use full VS BuildTools MSBuild.

### Commands:
```powershell
# Full build (restore + compile):
pwsh -NoProfile -ExecutionPolicy Bypass -File "D:\github\mRemoteNG\build.ps1"

# Fast incremental (skip restore):
pwsh -NoProfile -ExecutionPolicy Bypass -File "D:\github\mRemoteNG\build.ps1" -NoRestore

# Self-contained (embeds .NET runtime, output: bin\x64\Release\win-x64-sc\):
pwsh -NoProfile -ExecutionPolicy Bypass -File "D:\github\mRemoteNG\build.ps1" -SelfContained
```

`build.ps1` auto-detects VS installation (VS2026 > VS2022). Self-contained uses `-t:Publish` and restore MUST include `/p:PublishReadyToRun=true` (NETSDK1094).

## Testing

### Run tests (preferred):
```powershell
# Headless (CI/orchestrator):
pwsh -NoProfile -ExecutionPolicy Bypass -File "D:\github\mRemoteNG\run-tests.ps1" -Headless

# Skip build (fast iteration):
pwsh -NoProfile -ExecutionPolicy Bypass -File "D:\github\mRemoteNG\run-tests.ps1" -Headless -NoBuild

# Bash runner (fastest, no build):
bash run-tests-core.sh
```

### Single test group:
```bash
dotnet test "mRemoteNGTests/bin/x64/Release/mRemoteNGTests.dll" --results-directory /tmp/mrt --verbosity normal --filter "FullyQualifiedName~mRemoteNGTests.Tools"
```

### Critical Rules:
- **`--verbosity normal` ONLY** — minimal/quiet crashes testhost on .NET 10
- **`--results-directory` outside repo** — TestResults inside repo causes cascading crashes
- **DLL path, not .csproj** — `dotnet test --no-build` on .csproj looks in wrong `bin\Release\`
- **No interactive tests** — NEVER create tests with GUI dialogs, message boxes, or user input. Mock all UI dependencies.
- **No `[assembly: Parallelizable]`** — causes race conditions on shared mutable singletons
- **RunWithMessagePump pattern** — for ObjectListView/FrmOptions tests, use `Application.Run(form)` + `Application.ExitThread()` in finally

### The Golden Rule (test failures):
Every test failure MUST be resolved before finishing a task. NO EXCEPTIONS.
1. **Fix the code** if the test caught a real bug
2. **Fix the test** if the test logic is flawed
3. **Remove the test** ONLY if no longer valid
**NEVER use `[Ignore]`** for failing tests.

### 100% DLL Coverage:
`run-tests.ps1` runs parallel groups + sequential Remnants. If coverage gap detected, exit 96. New namespaces: update `$groups` in `run-tests.ps1` or let Remnants handle them.

### Current status: see `test-config.json` (single source of truth for test counts & groups)

## CI/CD
- Runners: `windows-2025-vs2026` with MSBuild 18.x (VS2026)
- Workflows: `pr_validation.yml` (build), `Build_mR-NB.yml` (release), `sonarcloud.yml` (quality gate), `codeql.yml` (security)
- Platforms: x86, x64, ARM64
- Code signing: SignPath Foundation (mandatory — see `CODE_SIGNING_POLICY.md`)
- Version: read from `mRemoteNG/mRemoteNG.csproj` `<Version>` element

## Code Quality — 5 Levels

| Level | Tool | Scope | Config |
|-------|------|-------|--------|
| 1 | .NET Analyzers + Roslynator + Meziantou | Local build (warnings) | `Directory.Build.props`, `.editorconfig` (root + mRemoteNG/) |
| 2 | SonarCloud | Push to `main` (CI) | `.github/workflows/sonarcloud.yml` |
| 3 | CodeQL | Push to `main` + weekly (CI) | `.github/workflows/codeql.yml` |
| 4 | Roslynator | Included in Level 1 (NuGet) | `Directory.Packages.props` |
| 5 | Qodo Code Review | On-demand (AI review) | GitHub App + `qodo-review.sh` |

### Rules:
- **Gradual adoption** — warnings only, NOT `TreatWarningsAsErrors` (legacy codebase)
- Noisy rules suppressed in `.editorconfig` (MA0004 ConfigureAwait, MA0011 IFormatProvider, MA0076 ToString culture)
- `EnforceCodeStyleInBuild=true`, `AnalysisLevel=latest-recommended` in `Directory.Build.props`
- **Două `.editorconfig`**: root (pentru ExternalConnectors, ObjectListView etc.) + `mRemoteNG/.editorconfig` (cu `root=true`, nu moștenește de la root)
- SonarCloud: `SONAR_TOKEN` secret setat, Automatic Analysis DEZACTIVAT pe sonarcloud.io (altfel conflict cu CI scan)
- CodeQL: `build-mode: manual` (COM refs break autobuild), CodeQL Action **v4** (v3 deprecated Dec 2026), Default Setup DEZACTIVAT în repo Settings → Code Security
- **NU există `sonar-project.properties`** — SonarScanner for .NET nu-l suportă, toate setările se dau ca parametri la `dotnet-sonarscanner begin`

### Qodo Code Review:
- GitHub App `qodo-code-review` instalat pe fork — AI-powered review complementar cu static analysis
- **On-demand only** — rulat prin `./qodo-review.sh [commits] [branch]`
- **NU funcționează ca GitHub Action** — Qodo ignoră PR-uri create de bots
- **Targetează doar default branch** — PR-ul trebuie să aibă `main` ca base
- Prinde bugs logice (bounds check, SQL mismatch, plaintext secrets) pe care SonarCloud/CodeQL le ratează

### Lecții setup CI (2026-02-28):
- CodeQL default setup NU coexistă cu workflow custom — trebuie dezactivat în Settings → Code Security
- SonarCloud Automatic Analysis NU coexistă cu CI analysis — trebuie dezactivat în SonarCloud → Administration → Analysis Method
- Meziantou MA0049 (type name matches namespace) e **error** by default — trebuie suprimat explicit pentru legacy code
- `gh run list` pe un fork caută pe upstream — folosește `--repo robertpopa22/mRemoteNG`

## Branch Strategy

| Branch | Purpose |
|--------|---------|
| `main` | Active development — default branch |
| `release/X.Y` | Historical release branches (frozen) |

### Feature branch naming:
| Prefix | When | Example |
|--------|------|---------|
| `fix/<issue>-<desc>` | Bug fix | `fix/2735-rdp-smartsize-focus` |
| `feat/<issue>-<desc>` | New feature | `feat/1634-protocol-token` |
| `security/<desc>` | Security | `security/ldap-sanitizer` |
| `chore/<desc>` | Infra, deps, CI | `chore/sqlclient-sni-runtime` |

Lowercase, kebab-case, max 50 chars after prefix. No tool prefixes.

### Sync upstream:
```bash
git fetch upstream && git merge upstream/v1.78.2-dev
```

## Session Discipline — Build Verification

1. **Run build before ending session** — especially for multi-file changes
2. If build fails, fix BEFORE reporting progress
3. **Never leave uncompilable code** — worse than slower progress
4. Prefer small verified steps over massive unverified refactoring

## Developer Guide

For orchestrator operations, release checklists, IIS system, issue tracking,
PR history, and release status, see: **`.project-roadmap/DEVELOPER_GUIDE.md`**

## Evidence & Scientific Documentation

For the complete evidence trail of the AI-assisted modernization process
(metrics, agent performance, CI data, methodology notes), see: **`docs/EVIDENCE.md`**

## Current Release Status (2026-03-01)

| Metric | Value |
|--------|-------|
| Version | 1.81.0-beta.6 |
| Analyzer warnings | 0 (5,247 eliminated) |
| Tests | 5,963 passed, 0 failures |
| CI status | All 6 workflows GREEN |
| Upstream PR | [#3189](https://github.com/mRemoteNG/mRemoteNG/pull/3189) (release/1.81 → v1.78.2-dev) |
| Nightly release | [Auto-generated on push to main](https://github.com/robertpopa22/mRemoteNG/releases/tag/nightly) |
