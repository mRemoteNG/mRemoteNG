# Agent Instructions — mRemoteNG Fork

This file is for **all AI agents** (Codex, Gemini, Claude, Copilot, etc.) working on this repository.

## Project Overview

- **Project:** mRemoteNG — remote connections manager (RDP, SSH, VNC, etc.)
- **Stack:** .NET 10, WinForms, COM references (MSTSCLib for RDP ActiveX)
- **Solution:** `mRemoteNG.sln` (SDK-style projects)
- **Fork:** `robertpopa22/mRemoteNG` (upstream: `mRemoteNG/mRemoteNG`)
- **Main branch:** `main` (syncs with upstream `v1.78.2-dev`)
- **Source code:** `mRemoteNG/`, `mRemoteNGTests/`, `mRemoteNGSpecs/`

## Build & Test (CRITICAL)

**Do NOT use `dotnet build`** — it fails with `MSB4803` on COM references (MSTSCLib RDP ActiveX control). You MUST use MSBuild via `build.ps1`.

| Action | Command |
|--------|---------|
| Build (full) | `powershell.exe -NoProfile -ExecutionPolicy Bypass -File "D:\github\mRemoteNG\build.ps1"` |
| Build (fast) | `powershell.exe -NoProfile -ExecutionPolicy Bypass -File "D:\github\mRemoteNG\build.ps1" -NoRestore` |
| Test (all) | `powershell.exe -NoProfile -ExecutionPolicy Bypass -File "D:\github\mRemoteNG\run-tests.ps1" -NoBuild` |
| Test (single group) | `dotnet test "mRemoteNGTests/bin/x64/Release/mRemoteNGTests.dll" --results-directory /tmp/mrt --verbosity normal --filter "FullyQualifiedName~mRemoteNGTests.Tools"` |
| Kill stale tests | `taskkill /F /IM testhost.exe` |

### Test Rules
- **`--verbosity normal` ONLY** — minimal/quiet crashes testhost on .NET 10
- **`--results-directory` outside repo** — TestResults inside repo causes cascading crashes
- **DLL path is `bin\x64\Release\`**, not `bin\Release\` — Platform=x64 changes the output path
- **No interactive tests** — NEVER create tests with GUI dialogs, message boxes, or user input

## Mandatory Workflow (when fixing issues)

Follow these steps IN ORDER. Do NOT skip any step.

### Step 1 — VERIFY & PLAN (before ANY edits)
- The triage suggested files may be WRONG. You MUST verify them.
- Read each suggested file (if they exist). If a suggested file doesn't exist, that's a red flag.
- Search the codebase for keywords from the issue (grep for error messages, class names, symptoms).
- Trace the code path: who calls what, where does the actual bug live?
- If the triage files are wrong, find the CORRECT file(s) with the root cause.
- Write a brief plan (max 5 lines): root cause, which file(s) to change, what to change.
- If previous attempts exist, analyze WHY each one failed — avoid repeating their mistakes.
- Only proceed to Step 2 after you have identified the correct file(s) and have a clear plan.

### Step 2 — IMPLEMENT
- Make changes according to your plan
- Do NOT change existing behavior — only fix the reported issue
- Do ONLY the fix, nothing else

### Step 3 — VERIFY
- Run build: `powershell.exe -NoProfile -ExecutionPolicy Bypass -File "D:\github\mRemoteNG\build.ps1"`
- Run tests: `powershell.exe -NoProfile -ExecutionPolicy Bypass -File "D:\github\mRemoteNG\run-tests.ps1" -NoBuild`

### Step 4 — FIX if needed
- If YOUR change breaks build or tests, fix it
- If tests fail for unrelated reasons, ignore

## Rules (CRITICAL — MUST follow)

1. **Do NOT run git commit, git add, git push, or ANY git operations.** The orchestrator handles all commits.
2. **NEVER modify infrastructure files:** `run-tests.ps1`, `build.ps1`, `mRemoteNG.sln`, `Directory.Build.props`, `Directory.Packages.props`, `.github/workflows/*`
3. **NEVER modify or read files in `.project-roadmap/`.** You are a CODE FIXER, not a project manager.
4. **Do NOT create interactive tests** (no dialogs, MessageBox, notepad.exe, user input prompts)
5. **Do NOT change existing behavior** — only fix the reported issue
6. **ONLY modify files under** `mRemoteNG/`, `mRemoteNGTests/`, or `mRemoteNGSpecs/` directories
7. **Never use `dotnet build`** — use `build.ps1` (COM references require MSBuild)

## Output Efficiency

- No narration. Don't write "Let me read the file" — just read it.
- No summaries. Don't summarize what you changed. The diff speaks for itself.
- No unnecessary comments. Don't add comments or docstrings to code you didn't change.
- Fix, don't explain. If a test fails, fix it immediately.
- One pass. Read the code, understand it, make the change.
