# Code Signing Policy

## Overview

All mRemoteNG release binaries are digitally signed using [SignPath Foundation](https://signpath.org/) code signing certificates. This ensures users can verify the authenticity and integrity of every release.

## Publisher

- **Certificate Holder:** SignPath Foundation
- **Purpose:** Authenticode signing of Windows executables and DLLs
- **SmartScreen:** Yes — signed binaries have Microsoft SmartScreen reputation

## Signing Process

1. **Automated:** All signing happens in CI (GitHub Actions) — no manual signing
2. **Mandatory:** The release workflow **cannot produce artifacts without signing**
3. **Verified:** SignPath verifies that binaries were built from this GitHub repository
4. **Secure:** Private signing keys are stored on SignPath's HSM (Hardware Security Module)

## Team Roles

| Role | Responsibility | Members |
|------|---------------|---------|
| **Author** | Write code, create PRs | All contributors |
| **Reviewer** | Review and approve PRs before merge | @robertpopa22 |
| **Approver** | Approve release signing requests | @robertpopa22 |

## What Gets Signed

- `mRemoteNG.exe` — main application executable
- `ExternalConnectors.dll` — credential provider plugins
- `ObjectListView.dll` — UI component library

## Requirements for Contributors

- All code contributions must go through pull request review
- Multi-factor authentication (MFA) is required for team members with merge access
- No binaries or pre-compiled code may be committed to the repository

## Verification

Users can verify signed binaries by:
1. Right-click the `.exe` > Properties > Digital Signatures tab
2. The signer should show **"SignPath Foundation"**
3. The certificate chain should be valid and trusted

## CI Integration

The signing step is integrated into `.github/workflows/Build_mR-NB.yml`:
- Step `(10a)` uploads unsigned artifacts
- Step `(10b)` submits to SignPath for signing
- Step `(10c)` downloads signed artifacts
- Step `(11)` creates the release with **signed** ZIP files only

If signing fails, the release step is **skipped** — no unsigned binaries are published.

## References

- [SignPath Foundation](https://signpath.org/)
- [SignPath Foundation Terms](https://signpath.org/terms.html)
- [SignPath GitHub Actions](https://github.com/SignPath/github-action-submit-signing-request)
