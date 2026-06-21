# SSH_DotNet — Certificate Login Feature & Quality‑Gate Remediation Plan

**Date:** 2026‑06‑21
**Branch:** `feature/ssh_dotnet_terminal`
**Upstream PR under review:** [mRemoteNG/mRemoteNG #2997](https://github.com/mRemoteNG/mRemoteNG/pull/2997)
**Author:** Dawie Joubert (with Claude Opus 4.8)

---

## 1. Executive Summary

We are adding **SSH key / certificate authentication** (private keys, with and without passphrase
protection) to the new `SSH_DotNet` protocol. Before the maintainer will accept the feature, the PR
must pass two automated quality gates that currently **fail**:

| Gate | Status | Headline findings |
|------|--------|-------------------|
| **GitHub Copilot review** | 127 comments across 37 files | ~50 generic `catch (Exception)` blocks; dead stores; non‑`readonly` fields; null‑deref risk; manual dispose in `finally`; implicit LINQ filtering |
| **SonarCloud Quality Gate** | **FAILED** | **2 Security Hotspots**; **Reliability Rating E** on new code (needs ≥ A); 40 annotations (11 *failure*, 29 *warning*) |

The strategic insight driving this plan: **the same architectural weaknesses that fail the quality
gate are the seams where certificate auth must plug in.** A 267‑line `Connect()` god‑method, an
auth provider with a `null`‑returning placeholder (`TryCreateKeyAuthenticationFromConnectionInfo`),
and an unfinished `// TODO` for key auth are *both* the Sonar complaints *and* the exact extension
points for the new feature. We therefore **restructure first, then graft the feature into the clean
seams**, so the new code is born quality‑gate‑compliant rather than retrofitted.

**Sequencing principle:** every phase ends with a green build and green tests (run locally on
Windows 10 via the documented `-p:SupportedOSPlatformVersion=10.0.19041.0` override). Because
SSH_DotNet is **unreleased**, no phase needs to preserve its persisted format — we rename its
identifiers and serialized strings wholesale to the final clean scheme (§3.4). No phase changes the
persisted format of any **released** protocol.

---

## 2. Current‑State Architecture

### 2.1 SSH_DotNet component map

| File | Role | Quality debt |
|------|------|--------------|
| `Connection/Protocol/SSH_DotNet/ProtocolSSH_DotNet.cs` | Protocol lifecycle, `Connect()`, I/O tasks, disposal | 28 Copilot + 13 Sonar; **Connect() = 267 lines**, high cognitive complexity (**Connect ~45**); ~24 generic catches; `DateTime.Now` timing; broken `IDisposable`; undisposed CTS |
| `UI/Controls/SshTerminalControl.cs` | VtNetCore terminal control, rendering, input | 37 Copilot + 11 Sonar; **worst cognitive complexity (up to 98)**; dead stores; ~30 generic catches; dispose‑in‑finally; static‑able helpers |
| `Connection/Protocol/SSH_DotNet/SSHAuthenticationProvider.cs` | Builds `AuthenticationMethod[]` | 2 Copilot + 4 Sonar; **key‑auth placeholder returns null**; unused params; if/else→ternary; generic catch; `// TODO` |
| `Connection/Protocol/SSH_DotNet/SSHConnectionManager.cs` | SSH client factory, keep‑alive, shell stream | naming (S101), `getSSHConnectionInfoByName` should be static, implicit filtering |
| `Connection/Protocol/SSH_DotNet/SSHTunnelManager.cs` | Port‑forward management | naming (S101), `OnForwardRequestReceived` static |
| `Connection/Protocol/SSH_DotNet/ShellStreamExtensions.cs` | **Reflection‑based** pty resize | **Security Hotspot (S3011)**; generic catch |
| `Connection/Protocol/SSH_DotNet/SSHDotNetDiagnostics.cs` | Trace/diagnostic logging | naming (S101); non‑`readonly` `_connectionStopwatch`; string interpolation in log templates (S2629) |
| `Connection/Protocol/SSH_DotNet/PortForwardRuleParser.cs` | Parses port‑forward rules | (clean) |

### 2.2 Authentication flow (today)

```
ProtocolSSH_DotNet.Connect()                      // 174–441, reads Username/Password off ConnectionInfo
  └─ SSHAuthenticationProvider.GetAuthenticationMethods(username, password, connectionInfo)  // L226
        ├─ password present  → PasswordAuthenticationMethod                  ✅ works
        ├─ TryCreateKeyAuthenticationFromConnectionInfo(username, connInfo)  ❌ placeholder → null (the // TODO)
        └─ keyboard‑interactive (auto‑answers password prompts)             ✅ works
```

`SSHAuthenticationProvider` **already** contains working low‑level key loaders —
`CreatePrivateKeyAuth(username, path, passphrase=null)` and `CreatePrivateKeyAuthFromString(...)` —
which already handle the with/without‑passphrase cases via SSH.NET `PrivateKeyFile`. **The feature
is therefore ~70% data‑plumbing, not crypto:** add connection properties, serialize them
(passphrase encrypted), expose them in the property grid, and have the placeholder read them.

### 2.3 Connection‑property infrastructure (the addition template)

`AbstractConnectionRecord` has **no** key/passphrase/certificate property today. The most recent
SSH_DotNet property, `SSHDotNetPortForwardRules`, is the canonical template and touches exactly
**7 files + 1 inherit flag + localization**:

| Concern | File | Reference |
|---------|------|-----------|
| Property + backing field | `Connection/AbstractConnectionRecord.cs` | L426–434 (`SSHDotNetPortForwardRules`) |
| Inherit flag | `Connection/ConnectionInfoInheritance.cs` | L172–176 (`public bool SSHDotNetPortForwardRules`) |
| XML write | `Config/.../Xml/XmlConnectionNodeSerializer28.cs` | value L72; inherit L222–223 |
| XML read | `Config/.../Xml/XmlConnectionsDeserializer.cs` | value L497; inherit L515 |
| CSV write | `Config/.../Csv/CsvConnectionsSerializerMremotengFormat.cs` | header + value + inherit columns |
| CSV read | `Config/.../Csv/CsvConnectionsDeserializerMremotengFormat.cs` | matching columns |
| Localization | `Language/Language.resx` (+ `Language.Designer.cs`) | `SshPortForwardRules`, `PropertyDescriptionSshPortForwardRules` |

**Secret vs plaintext serialization (critical for the passphrase):**

```csharp
// SECRET (encrypted) — XmlConnectionNodeSerializer28.cs L61 / Deserializer L218
element.Add(new XAttribute("Password", _cryptographyProvider.Encrypt(connectionInfo.Password, _encryptionKey)));
connectionInfo.Password = _decryptor.Decrypt(xmlnode.GetAttributeAsString("Password"));

// PLAINTEXT — XmlConnectionNodeSerializer28.cs L72 / Deserializer L497
element.Add(new XAttribute("SSHDotNetPortForwardRules", connectionInfo.SSHDotNetPortForwardRules));
connectionInfo.SSHDotNetPortForwardRules = xmlnode.GetAttributeAsString("SSHDotNetPortForwardRules");
```

→ **Key‑file path = plaintext pattern. Passphrase = encrypted (`Password`) pattern**, gated by the
`_saveFilter.SavePassword` flag, written as `""` when the filter says don't save secrets.

**Verified — inheritance auto‑registers (de‑risk):** `ConnectionInfoInheritance.GetProperties()` is
**reflection‑based** (`typeof(ConnectionInfoInheritance).GetProperties().Where(FilterProperty)`), and
`FilterProperty` excludes only `EverythingInherited`, `Parent`, `InheritanceActive`. Therefore
**adding a new `bool` inherit property is automatically picked up** by `TurnOn/OffInheritance`,
`SetAllValues`, `EverythingIsInherited`, and the inherit grid — there is **no explicit list to
update**. The "7 files" touch‑point count is complete for inheritance.

> **4th backend — SQL Server/MySQL — intentionally out of scope** (decided, §5.6): the `Sql/`
> `DataTableSerializer`/`DataTableDeserializer` use an explicit, version‑migrated `tblCons` schema and
> already carry **no** SSH_DotNet properties. We match that precedent (XML + CSV only) and document it
> as a known limitation. So the touch‑points stay **XML + CSV**, not XML + CSV + SQL.

---

## 3. Quality‑Gate Findings — Architectural Analysis

The findings cluster into a small number of root causes. Fixing the *cause* (not each instance)
clears the gate and yields the seams the feature needs.

### 3.1 SonarCloud — Reliability "E" drivers (the *failure*-level annotations)

These are what flip the Reliability Rating to E and **must** be fixed to pass the gate.

| Root cause | Sonar rule | Where | Architectural remedy |
|------------|-----------|-------|----------------------|
| **`DateTime.Now` for elapsed‑time** (×5) | S6561/S2925 family | `ProtocolSSH_DotNet` (`_connectionStartTime`), `SSHDotNetDiagnostics` | Use a **monotonic clock**: `System.Diagnostics.Stopwatch` (the class already has `_connectionStopwatch`) or `Environment.TickCount64`. Never subtract wall‑clock times. |
| **Broken `IDisposable` pattern** | S3881 | `ProtocolSSH_DotNet` | Implement the canonical `Dispose(bool)` + `GC.SuppressFinalize`; idempotent; no throwing. |
| **Undisposed `CancellationTokenSource`** (`_cancellationTokenSource`, `_errorCancellationSource`) | S2930 | `ProtocolSSH_DotNet` | Dispose in `Dispose(bool)`; null‑out after cancel; guard re‑entry. |
| **`await` the async API** ("Await ConnectAsync instead") | S6966 | `ProtocolSSH_DotNet` connect path | **Decided: fully async** (§4.3) — implement `ConnectCoreAsync` (`await ConnectAsync` + async I/O) in Phase 4, bridged from the sync `Connect()` override at one documented boundary. `ProtocolBase` signature unchanged. |

### 3.2 SonarCloud — Security Hotspots (2) — gate blocker

Both hotspots stem from **`ShellStreamExtensions` using reflection to send a window‑change
(pty resize) request** to SSH.NET's internal channel (**S3011 — accessibility bypass**).

> **Caveat — identify both hotspots precisely.** Only **one** hotspot is confirmed from the cached
> GitHub annotations (the S3011 accessibility bypass in `ShellStreamExtensions`). The SonarCloud PR
> data has aged out, so the **2nd hotspot is unconfirmed** — most likely the second reflection call
> in `ShellStreamExtensions` (field access + invoke), but possibly a separate item (e.g. a crypto or
> process/`StartInfo` usage). **Action:** re-run the SonarCloud analysis (or open the project's
> Security Hotspots view) to enumerate both before starting Phase 2.

**Pinned version (verified):** `SSH.NET` = **2025.1.0** (`Directory.Packages.props:59`); a legacy
`Renci.SshNet.Async` **1.4.0** is also referenced (see §4.3 — likely now redundant).

**Remediation options (in order of preference):**
1. **Replace reflection with a public API — including *bumping* SSH.NET.** Updating the SSH.NET
   package **is in scope for this plan.** Check whether any SSH.NET release ≥ 2025.1.0 exposes a
   public `ShellStream` pty‑resize / `SendWindowChangeRequest`; if one does, **update the package and
   delete the reflection entirely** (best outcome — removes the hotspot at the source). Note: as of
   2025.1.0 SSH.NET has **no documented public resize** on `ShellStream` (long‑standing gap), so this
   may not be available — confirm against the actual release notes/API before relying on it.
2. If no public API exists, **encapsulate** the reflection in one tiny, audited, well‑documented
   method with validated inputs (dimensions are bounded ints we generate, never user strings),
   and mark the hotspot **reviewed/safe** in SonarCloud with that justification.
3. Long‑term: contribute the resize API upstream to SSH.NET so it’s first‑class.

The passphrase work introduces **no new hotspots** provided we (a) never log secrets and
(b) reuse the existing `ICryptographyProvider` (no home‑rolled crypto).

### 3.3 Copilot + Sonar — Maintainability themes (the *warning* level)

| Theme | Rules | Count | Remedy |
|-------|-------|------:|--------|
| **Generic `catch (Exception)`** | Copilot "Generic catch clause", S2221/S112 | ~50 | Catch **specific** SSH.NET/IO exceptions (`SshConnectionException`, `SshAuthenticationException`, `SshOperationTimeoutException`, `SocketException`, `IOException`, `ObjectDisposedException`). Where a terminal‑survivability catch‑all is genuinely required, funnel through **one** documented helper (`SafeExecute`/`TryLog`) and let truly‑unexpected exceptions propagate. |
| **Cognitive Complexity > 15** | S3776 | methods at 17/26/27/45/**98** | **Decompose** (see §4.2). The 98 and 45 are the terminal input/render and `Connect()`. |
| **Class names not PascalCase** | S101 | 5 classes | Rename **types** `ProtocolSSH_DotNet→ProtocolSshDotNet`, `SSHAuthenticationProvider→SshAuthenticationProvider`, `SSHConnectionManager→SshConnectionManager`, `SSHTunnelManager→SshTunnelManager`, `SSHDotNetDiagnostics→SshDotNetDiagnostics`. Since the feature is unreleased we **also** rename the enum value, namespace, and serialized strings (§3.4). Use Serena `rename_symbol` for types; namespace/folder rename is a separate mechanical step (see §3.4 caveat). |
| **Methods should be static** | S2325 | ~10 | `getSSHConnectionInfoByName`, `GetCtrlSequence`, `GetAltSequence`, `GetKeySequence`, `GetPrintableSequence`, `ParseColor`, `FormatBytes`, `OnForwardRequestReceived`, `Serialize`, etc. → `static`. |
| **Non‑`readonly` fields** | S2933 | `_peakSendRate`, `_errorCancellationSource`, `_cursorColor`, `_connectionStopwatch` | Mark `readonly` where only assigned in ctor. |
| **Dead stores / unread fields** | S1854/S1144 | `font`, `lines`, `endY`, `_streamAttached`, `_sshStream` | Remove. |
| **Possible null dereference** | S2259 / Copilot | `_terminalControl` may be null at access (`ProtocolSSH_DotNet` ~L325) | Guard with null checks / early‑return; the `Connect()` decomposition (§4.2) already null‑checks `_terminalControl` before `StartTerminalSession` — fold the scattered accesses behind that guarantee. |
| **Unused parameters** | S1172 | `username`, `connectionInfo` in placeholder | Resolved naturally when the placeholder is implemented (§5). |
| **String interpolation in log templates** | S2629 | `SshDotNetDiagnostics.LogXxx($"...")` | Guard with level checks so interpolation is not evaluated when disabled, or pass structured args. (Also keeps secrets out of logs.) |
| **Misc smells** | S1066 (merge `if`), S3267 (explicit `.Where`), S2930 (using vs finally), S6610 (`StartsWith(char)`), S108 (empty block), S1135 (`// TODO`) | scattered | Mechanical fixes; the `// TODO` is closed by shipping the feature. |

### 3.4 No backward‑compatibility constraint — full rename freedom

**The SSH_DotNet feature is unreleased — no user has saved connections with it — so there is NO
wire‑format to preserve.** (Confirmed by the maintainer/author: "no one is using this yet; we can
restructure and do any changes required to be code‑quality compliant.") We therefore rename
**everything**, including persisted identifiers, to fully satisfy S101 and produce a clean, final
naming scheme **before** the feature ships:

| Item | From | To |
|------|------|-----|
| Protocol enum value | `ProtocolType.SSH_DotNet` | `ProtocolType.SshDotNet` |
| Namespace / folder | `…Connection.Protocol.SSH_DotNet` | `…Connection.Protocol.SshDotNet` |
| Protocol class | `ProtocolSSH_DotNet` | `ProtocolSshDotNet` |
| Helper classes | `SSHAuthenticationProvider`, `SSHConnectionManager`, `SSHTunnelManager`, `SSHDotNetDiagnostics` | `SshAuthenticationProvider`, `SshConnectionManager`, `SshTunnelManager`, `SshDotNetDiagnostics` |
| Existing serialized property | `SSHDotNetPortForwardRules` (attr string + C# member) | `SshDotNetPortForwardRules` |
| New serialized properties | — | `SshDotNetPrivateKeyFile`, `SshDotNetPrivateKeyPassphrase` |

Guidance:
- Use Serena `rename_symbol` to update every reference; rename files/folders to match the types.
- Serialized **attribute strings** are renamed in lock‑step with the members (XML + CSV
  write/read, CSV header columns, inherit flags `Inherit…`).
- Keep a consistent `SshDotNet` prefix on serialized property keys to avoid collisions with the
  legacy PuTTY‑based `SSH` protocol’s attributes in the flat XML namespace.
- **Stability still applies to OTHER protocols:** only `SSH_DotNet`‑related identifiers/strings
  change. Do not touch any released protocol’s enum value or attribute names.
- Because we control all current `SSH_DotNet` test/sample files, migrate or regenerate any local
  `confcons.xml` used in tests to the new names (no production migration shim needed).

**Verified specifics that shape the rename:**
- The `ProtocolType` enum is **serialized by member name** (no `[XmlEnum]`/converter;
  `XmlConnectionsDeserializer.cs:285` uses `GetAttributeAsEnum<ProtocolType>("Protocol")`). So
  renaming `SSH_DotNet→SshDotNet` changes the persisted `Protocol="…"` string. Note: **Sonar S101
  flagged the 5 *classes*, not the enum member** — the enum/namespace rename is our consistency
  choice (safe, recommended), not a gate requirement. The numeric value (`= 15`) is irrelevant since
  serialization is name‑based; migrate local test `confcons.xml` accordingly.
- **Namespace/folder rename is NOT a `rename_symbol` op.** Renaming `…Protocol.SSH_DotNet` →
  `…Protocol.SshDotNet` is a mechanical change across every `namespace …` declaration and every
  `using …` in **production *and* tests**, plus a physical folder move (`SSH_DotNet/`→`SshDotNet/`).
  The SDK‑style csproj uses implicit globbing, so no project‑file edit is needed; but plan a build +
  full‑text sweep to catch stragglers.

### 3.5 Test‑project structural debt

The PR carries **duplicate test files** in two folders (e.g.
`mRemoteNGTests/Connection/Protocol/SSH_DotNet/SSHConnectionManagerTests.cs` **and**
`mRemoteNGTests/Connection/Protocol/SSHConnectionManagerTests.cs`; same for `ProtocolSSH_DotNetTests`).
This inflates the review (34 of 127 Copilot comments are on the duplicated test files) and risks
divergence. **Consolidate to the `SSH_DotNet/` subfolder; delete the duplicates.**

---

## 4. Target Architecture

### 4.1 Authentication abstraction

Reshape `SshAuthenticationProvider` around an explicit, testable strategy:

```
SshAuthenticationProvider (static factory, pure, no instance state)
  GetAuthenticationMethods(SshAuthContext ctx) : AuthenticationMethod[]
     ctx = { Username, Password, PrivateKeyPath, Passphrase, AllowKeyboardInteractive }
     order:
       1. if PrivateKeyPath set      → BuildPrivateKeyMethod(ctx)        // with/without passphrase
       2. if Password set            → PasswordAuthenticationMethod
       3. if AllowKeyboardInteractive→ KeyboardInteractiveMethod
```

- Introduce a small `SshAuthContext` (or reuse `ConnectionInfo` directly but read it in **one**
  place) so the placeholder’s unused‑parameter smell disappears and the method is unit‑testable
  without a full `ConnectionInfo`.
- `BuildPrivateKeyMethod` collapses the L149 if/else into the existing loader call (passphrase may
  be null/empty → loader already handles both). Fixes S3923 + closes the `// TODO`.

### 4.2 `Connect()` decomposition (kills the complexity + makes the auth seam)

Split the 267‑line `Connect()` into focused, individually‑testable steps, each returning a small
result so the orchestrator stays linear (complexity ≤ 15):

```
Connect()
  ├─ TryResolveConnectionParameters(out ConnParams p)     // hostname/port/username validation
  ├─ BuildAuthentication(p)            ← cert auth enters HERE (reads key path + passphrase)
  ├─ EstablishSshClient(p, auth)                          // create + keep‑alive + connect, specific catches
  ├─ ConfigureTunnels(client, p)                          // port‑forward rules
  ├─ (TunnelOnlyMode? return)                             // early exit
  └─ StartTerminalSession(client, p)                      // shell stream + I/O tasks + opening cmd + focus
```

This is the single most valuable refactor: it clears S3776 on `Connect()` **and** gives the feature
a 1‑line insertion point (`BuildAuthentication`) instead of editing a god‑method.

> These steps compose **`ConnectCoreAsync`** (§4.3); the I/O‑bound ones are async
> (`EstablishSshClientAsync`, `StartTerminalSessionAsync`). The diagram shows the logical sequence,
> not sync signatures.

`EstablishSshClient` is built on the **`ISshClientAdapter` seam** (decided — §10.4): `SshConnectionManager`
returns the interface, `ProtocolSshDotNet` depends on it, and a fake adapter drives the pipeline tests.

**Also decompose `SshTerminalControl`** (separate from `Connect()`): it holds the **worst** cognitive
complexity (≈98) and ~30 generic catches. Extract the large input/render method into smaller units
(e.g. key‑sequence mapping — already candidates for `static` per S2325 — separated from rendering and
from the paint loop) and route its catch‑alls through the single documented survivability helper.
This is its own slice of Phase 4, not covered by the `Connect()` diagram above.

### 4.3 Async & lifetime policy — **DECISION: go fully async** (with a documented sync boundary)

**Decision (2026‑06‑21):** adopt async/await end‑to‑end for the SSH_DotNet connection pipeline
(SSH.NET `ConnectAsync`, async read/write loops, `CancellationToken` throughout). This resolves
S6966 by *actually* awaiting the async API rather than calling the sync one. **Verified:** the pinned
**SSH.NET 2025.1.0 has native `ConnectAsync(CancellationToken)`**, so this needs no extra package. The
legacy **`Renci.SshNet.Async` 1.4.0** is almost certainly redundant once we use native async —
**evaluate and remove it** if no remaining code references its extension methods (dependency hygiene).

**Hard constraint (verified):** `ProtocolBase.Connect()` is `public virtual bool Connect()` —
**synchronous, returns `bool`**, shared by ~14 protocols, and consumed **inline** by
`ConnectionInitiator` (`if (newProtocol.Connect() == false)` at `ConnectionInitiator.cs:165/228/304`).
We **must not change that base signature** — it would touch every released protocol (violates the
"don't touch released protocols" principle, §3.4) and is out of scope for this PR.

**Threading (verified — important):** the `ConnectionInitiator.cs:304` call site runs **inline on the
UI thread** — the surrounding method manipulates `FrmMain.Default.SelectedConnection` and the
connection form directly. Therefore the sync bridge **must not block‑wait the async core on the UI
thread** (classic sync‑over‑async deadlock). Run `ConnectCoreAsync` on a worker (`Task.Run`) with
`ConfigureAwait(false)` throughout the core, and marshal only terminal/UI callbacks back via
`Control.Invoke`. This is now a *requirement*, not a "verify."

**Therefore "fully async" = async core + one explicit, documented sync boundary:**
- Implement the pipeline as `Task<bool> ConnectCoreAsync(ConnParams, CancellationToken)` —
  `await client.ConnectAsync(ct)`, async shell setup, async I/O loops.
- Bridge at the override: `public override bool Connect()` runs `ConnectCoreAsync` to completion at
  the single boundary — **on a worker thread** (`Task.Run`, `ConfigureAwait(false)` throughout the
  core), because the caller is on the UI thread (confirmed above). Do **not** block‑wait on the UI
  thread. Marshal only terminal/UI callbacks back via `Control.Invoke`.
- Keep `ConfigureAwait(false)` in all non‑UI core code; marshal terminal/UI updates explicitly.
- *(Stretch, separate PR — NOT here:* a future refactor could add an async connect path to
  `ProtocolBase` for all protocols. Explicitly deferred.)*

- **Disposal:** canonical `IDisposable` (`Dispose(bool)`), disposing the `ISshClientAdapter` (wrapping
  `_sshClient`), `_shellStream`, `_cancellationTokenSource`, `_errorCancellationSource`,
  `_tunnelManager`; idempotent; never throws.
  Consider `IAsyncDisposable` for the async I/O loops (await graceful task shutdown before dispose).
- **Cancellation:** one CTS per connection lifetime; cancel → await tasks (bounded) → dispose → null.

### 4.4 Logging & secrets policy

- `SshDotNetDiagnostics`: never log passphrase, key contents, or password. Log key **file name**
  only (already the pattern). Guard interpolation behind level checks (S2629).
- All secrets flow only through `ICryptographyProvider`; nothing is written to disk in plaintext.

---

## 5. Certificate Login Feature Design

### 5.1 New connection properties (2)

| Property | Type | Secret? | Serialized key | Notes |
|----------|------|---------|----------------|-------|
| `SshDotNetPrivateKeyFile` | `string` (path) | No (plaintext) | `SshDotNetPrivateKeyFile` | Path to OpenSSH/PEM/PuTTY‑exported private key (and optional `*-cert.pub` OpenSSH certificate). |
| `SshDotNetPrivateKeyPassphrase` | `string` | **Yes (encrypted)** | `SshDotNetPrivateKeyPassphrase` | Optional. Empty ⇒ unprotected key. Mirrors `Password` exactly (encrypt on write, decrypt on read, honour `SavePassword` filter). |

> Serialized keys use the final PascalCase scheme (see §3.4) since the feature is unreleased.

Design decisions:
- **No new auth‑mode enum.** Presence of `SshDotNetPrivateKeyFile` enables key auth; key is tried
  **before** password (matches OpenSSH client behaviour). Keeps UI minimal and avoids extra state.
- **Both passphrase cases** (the explicit requirement) are handled by the existing loader: empty
  passphrase → `new PrivateKeyFile(path)`; non‑empty → `new PrivateKeyFile(path, passphrase)`.
- **Passphrase fallback (optional, behind review):** if passphrase is empty but `Password` is set,
  we *may* try `Password` as the passphrase. Default **off** to avoid surprising secret reuse;
  document and make it a deliberate choice.
- **Inheritance:** both properties get `Inherit*` flags so folder‑level defaults work (e.g. a team
  folder pointing at a shared key path), consistent with every other property.
- **Scope boundary — key source:** v1 sources the key from a **file path**. The auth provider already
  has `CreatePrivateKeyAuthFromString` (key *content* as a string), so sourcing the key from an
  **external credential provider** (Vault/CyberArk/etc.) is a natural, deliberately‑deferred follow‑up
  — note it explicitly so the maintainer sees it's intentional, not an oversight.

### 5.2 Property‑grid / UI

- Add both properties under the existing **Protocol** localized category with
  `AttributeUsedInProtocol(ProtocolType.SshDotNet)` (post‑rename) so they show only for SSH_DotNet connections.
- Key‑file path: ideally a file‑picker editor (reuse any existing path‑editor `UITypeEditor` in the
  property grid; otherwise plain string is acceptable for v1).
- Passphrase: use the **same password editor/masking** as `Password` (so it renders as •••• and is
  treated as a secret by the grid).
- Localization strings in `Language.resx`: `SshDotNetPrivateKeyFile`,
  `PropertyDescriptionSshDotNetPrivateKeyFile`, `SshDotNetPrivateKeyPassphrase`,
  `PropertyDescriptionSshDotNetPrivateKeyPassphrase` (+ generated `Language.Designer.cs`).

### 5.3 Auth wiring (the payoff)

`BuildAuthentication` reads the two new props off `ConnectionInfo`, builds an `SshAuthContext`, and
`SshAuthenticationProvider.GetAuthenticationMethods(ctx)` does:

```csharp
if (!string.IsNullOrEmpty(ctx.PrivateKeyPath))
    methods.Add(SshAuthenticationProvider.CreatePrivateKeyAuth(ctx.Username, ctx.PrivateKeyPath, ctx.Passphrase));
```

The previously‑`null` placeholder is deleted; its unused‑parameter and `// TODO` smells vanish.

### 5.4 Error handling (feature‑specific, quality‑compliant from birth)

Map key‑auth failures to **specific** exceptions with actionable messages (no generic catch):
- `FileNotFoundException` → "Private key file not found: …"
- `SshPassPhraseNullOrEmptyException` / invalid passphrase → "Incorrect or missing key passphrase"
- malformed key → "Unsupported or corrupt private key format"

### 5.5 Security checklist for the feature

- [ ] Passphrase encrypted at rest via `ICryptographyProvider` (never plaintext in XML/CSV).
- [ ] Passphrase respects `SavePassword` save‑filter (written as `""` when secrets aren’t saved).
- [ ] Passphrase/key contents never logged; only key file name logged.
- [ ] Key file read with least privilege; handle access‑denied explicitly.
- [ ] No new reflection / accessibility bypass (no new hotspots).
- [ ] XSD (`Schemas/mremoteng_confcons_v2_8.xsd`) — **verified NOT used to validate XML file loads**
      (the deserializer only does `ValidateConnectionFileVersion()`; the XSD is referenced solely by
      `SqlServerPage` for the SQL backend). So updating it is **optional/informational for XML**;
      relevant only if we add SQL support (see §5.6). Safe to defer for XML‑only.

### 5.6 SQL Server / MySQL backend — gap & decision

**Verified gap (not in the original "7 files" template):** mRemoteNG has a third persistence backend
— **SQL Server / MySQL** — via `Sql/DataTableSerializer` + `Sql/DataTableDeserializer`, with the
`tblCons` schema **versioned by explicit DDL upgraders** (`SqlVersionNNToMMUpgrader`,
`ALTER TABLE tblCons ADD COLUMN …`). Unlike XML/CSV, this is **explicit, column‑by‑column** (no
reflection): `CreateSchema` (~172 lines) and `SerializeConnectionInfo` (~267 lines) list every column.

**Key fact:** the existing `SSHDotNetPortForwardRules` (and the rest of the SSH_DotNet feature)
**already has NO SQL support** — it appears in zero SQL files. So the feature is *already*
XML/CSV‑only; the cert properties would inherit that limitation by default.

**Cost to add SQL (per new property):**
- *Easy part:* 4 columns (2 value + 2 `Inherit…`) in `CreateSchema`, `SerializeConnectionInfo`
  (passphrase encrypted, honour `SaveFilter`), and `DataTableDeserializer`.
- *Harder part:* a **new schema‑version upgrader** (`SqlVersion30To31Upgrader`) with `ALTER TABLE`
  DDL **for both SQL Server *and* MySQL** (dialect differences), a **schema version bump**, the
  fresh‑install create‑table script, and **DB‑backed integration testing** (can't unit‑test; needs a
  live DB). This touches **shared, released** SQL infrastructure and every SQL user's upgrade path.

**DECISION (2026‑06‑21): Option A — skip SQL** (match the existing SSH_DotNet precedent).
- XML + CSV only for the cert properties; **no** `DataTableSerializer`/`DataTableDeserializer`/schema‑
  migration changes. Lowest risk, no changes to the shared versioned SQL schema, fully verifiable on
  Win10, and the quality gate doesn't flag it.
- **Document as a known limitation** (CHANGELOG / `IMPLEMENTATION_NOTES`): "SSH_DotNet connection
  properties — including key file/passphrase and port‑forward rules — are persisted to the XML and CSV
  back‑ends only; SQL Server/MySQL storage is a future enhancement."
- *Deferred follow‑up (not this plan):* full SQL support = 4 columns in `CreateSchema` /
  `SerializeConnectionInfo` / `DataTableDeserializer` + a cross‑dialect `SqlVersion30To31` migration +
  version bump + DB integration tests, ideally backfilling the existing SSH_DotNet props for parity.
- Consequence: the XSD (§5.5) needs **no** change for this plan.

---

## 6. Implementation Phases (each ends green)

> Use Serena for all navigation/edits (symbolic tools); run tests locally with
> `-p:SupportedOSPlatformVersion=10.0.19041.0`; build with VS 2026 MSBuild
> (`C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe`).

**Delivery strategy (DECISION 2026‑06‑21): split into two PRs.**
- **PR 1 — Quality remediation of #2997** = Phases **0–5** (baseline, reliability, hotspots, rename,
  `Connect()`+`SshTerminalControl` decomposition + async pipeline + `ISshClientAdapter` seam, test
  dedup). Goal: #2997 passes both gates with a review‑friendly diff and gets accepted. The
  `BuildAuthentication` seam exists but still only does password + keyboard‑interactive (no behaviour
  change). Ship the CHANGELOG entry for the refactor here.
- **PR 2 — Certificate login (follow‑up)** = Phases **6–8** (+ Phase 9 docs), built on PR 1’s merged,
  clean base: add the two properties + serialization, wire key auth into `BuildAuthentication`, UI,
  and the full auth/security/fixture test suite.
- Dependency direction is clean (PR 2 depends on PR 1; never the reverse). Each phase still ends green
  so PR 1 can be opened as soon as Phase 5 completes.

**Commit cadence (DECISION — per request): commit after every phase.** Each phase ends green
(VS 2026 build + `Category=Unit` tests pass on Win10) **and is committed** to the active PR branch
before the next phase begins — one focused commit per phase (large phases like Phase 4 may use a few
sub‑step commits). This keeps history bisectable and each step reviewable/revertible. Conventions:
- PR 1 (quality) messages: `refactor(ssh_dotnet): <phase summary>`; PR 2 (feature): `feat(ssh_dotnet): …`.
- Each phase below lists its **Commit:** line. End every message with the
  `Co-Authored-By: Claude Opus 4.8 <noreply@anthropic.com>` trailer (repo convention).
- Do **not** push until the relevant PR is ready (commit locally each phase; push when opening PR 1 / PR 2).

### Phase 0 — Baseline & guardrails  ·  *(PR 1)*
- Capture current green test baseline (the 103 unrelated failures noted separately — exclude/triage
  so they don’t mask regressions).
- Add an `.editorconfig`/analyzer note encoding the conventions in §7 so new code self‑checks.
- **Commit:** `chore(ssh_dotnet): add quality conventions/editorconfig + test baseline`

### Phase 1 — Reliability fixes (clears Sonar "E"; low risk, mechanical)
- Replace `DateTime.Now` elapsed‑time with `Stopwatch`/`TickCount64` (Protocol + Diagnostics).
- Implement canonical `IDisposable`; dispose both `CancellationTokenSource`s; `readonly` fields.
- *(S6966 — "await ConnectAsync" — is resolved by the async pipeline in Phase 4, per the §4.3
  decision, not here, since it's structural rather than mechanical.)*
- **Gate effect:** Reliability rating → A for the timing/dispose drivers cleared here; S6966 closes in Phase 4.
- **Commit:** `refactor(ssh_dotnet): fix reliability bugs (monotonic timing, IDisposable, dispose CTS)`

### Phase 2 — Security hotspots (clears the 2 hotspots)
- First **enumerate both hotspots** from SonarCloud (only 1 confirmed — §3.2 caveat).
- Investigate a public SSH.NET resize API — **including bumping the `SSH.NET` package** (in scope) if
  a newer release exposes `ShellStream` pty‑resize; if so, update the package and delete the
  reflection. Otherwise encapsulate + justify the hotspot (§3.2 option 2).
- **Gate effect:** 2 hotspots resolved → Quality Gate security condition passes.
- **Commit:** `refactor(ssh_dotnet): remove/justify reflection pty-resize (security hotspots)`

### Phase 3 — Naming & static (S101/S2325) via Serena `rename_symbol` — **full rename (unreleased)**
- Rename the 5 classes **and** the `ProtocolType.SSH_DotNet` enum value → `SshDotNet`, the
  `…Protocol.SSH_DotNet` namespace/folder → `SshDotNet`, and the existing `SSHDotNetPortForwardRules`
  member **and** its serialized attribute strings → `SshDotNetPortForwardRules` (per §3.4 — no
  back‑compat constraint). Update `ProtocolFactory`, `AttributeUsedInProtocol(...)`, XML/CSV
  read+write, CSV header columns, and inherit flags in lock‑step.
- Rename files/folders to match types; verify via build + full‑text search that no `SSH_DotNet`
  identifier or attribute string remains (only the legacy `SSH` protocol’s strings stay).
- Regenerate/migrate any local test `confcons.xml` samples to the new attribute names.
- Make pure helpers `static`; remove dead fields/stores; merge `if`s; explicit `.Where`.
- **Commit(s):** `refactor(ssh_dotnet): PascalCase rename (types, enum, namespace, serialized strings)`
  then `refactor(ssh_dotnet): make pure helpers static; remove dead code` (split the rename from the
  static/cleanup so the rename commit is a pure rename — easy to review).

### Phase 4 — `Connect()` decomposition + async pipeline + exception specificity (S3776/S2221/S6966)
- Apply §4.2 split; replace generic catches with specific exceptions / single documented helper.
- **Introduce the `ISshClientAdapter` seam** (decided — §10.4): production wrapper over `SshClient` +
  a test fake; `SshConnectionManager` returns the interface; `ProtocolSshDotNet` depends on it.
- **Decompose `SshTerminalControl`** too (the ≈98‑complexity method + ~30 catches — §4.2), not just
  `Connect()`.
- **Drop `Renci.SshNet.Async`** if the native SSH.NET 2025.1.0 async surface leaves it unreferenced.
- **Implement the async pipeline (§4.3 decision):** `Task<bool> ConnectCoreAsync(…, CancellationToken)`
  using `await adapter.ConnectAsync(ct)` + async I/O; bridge from the sync `Connect()` override at one
  documented boundary. **First confirm `ConnectionInitiator`'s calling thread** (see §4.3) and choose
  the non‑deadlocking bridge accordingly. Do **not** change `ProtocolBase.Connect()`'s signature.
- Add the seam‑based pipeline/cancellation/dispose unit tests (§10.5).
- Re‑run complexity locally; ensure every method ≤ 15.
- **Commit(s):** `refactor(ssh_dotnet): add ISshClientAdapter seam + fake` ·
  `refactor(ssh_dotnet): decompose Connect() into async pipeline (≤15 complexity)` ·
  `refactor(ssh_dotnet): decompose SshTerminalControl + specific exceptions` (sub‑step commits for a
  large phase, each green).

### Phase 5 — Test consolidation
- **Diff the duplicate copies before deleting** (`…/SSH_DotNet/*` vs `…/Protocol/*`) — they may have
  diverged; keep the superset, don't blindly keep one folder. Then delete the duplicates; keep the
  `SshDotNet/` subfolder copies; ensure all compile & run on Win10 (filter `Category=Unit`).
- **Ordering tip:** run this **before** Phase 3 if practical, so we don't rename/move files we're
  about to delete (avoids churn on the duplicated copies).
- Introduce the `[Category]` taxonomy (§10.1) as files are touched.
- **Commit:** `test(ssh_dotnet): consolidate duplicate test files; add [Category] taxonomy`
  *(→ open **PR 1** after this phase)*

### Phase 6 — Certificate feature: properties + serialization  ·  *(PR 2)*
- Add `SshDotNetPrivateKeyFile` (plaintext) and `SshDotNetPrivateKeyPassphrase` (encrypted) across
  the **7 files + inherit flags + Language.resx**, mirroring `SshDotNetPortForwardRules`/`Password`.
  (Inheritance auto‑registers — §2.3 verified note.)
- **CSV positional‑column invariant (bit us during the merge):** the CSV header is a fixed string and
  values are written positionally. Add the 2 value columns **and** 2 inherit columns to **both** the
  header *and* the row writer *and* the deserializer at matching positions; add a column‑count assert.
- **Passphrase decrypt resilience:** decryption can fail under a wrong master password, exactly like
  `Password`. Ensure the deserializer tolerates a failed/empty passphrase decrypt (no crash; surface
  as empty so the user is re‑prompted), mirroring the existing `Password` decrypt path.
- **XML round‑trip is auto‑covered** by the reflection‑driven `XmlSerializationLifeCycleTests` (§10.2)
  — it will **fail** if write *or* read is incomplete, so just keep it green (no new XML test needed).
- **Add explicit CSV tests** (positional, not auto‑covered) + the security serialization tests
  (ciphertext‑at‑rest, SaveFilter→`""`, no‑leak‑in‑logs) per §10.2/§10.6.
- Add a test that **loads a connections file lacking the new attributes** → defaults cleanly.
- **Commit:** `feat(ssh_dotnet): add private-key file/passphrase connection properties (XML+CSV)`

### Phase 7 — Certificate feature: auth wiring + UI  ·  *(PR 2)*
- Implement `SshAuthContext` + `GetAuthenticationMethods(ctx)`; delete the null placeholder.
- Wire `BuildAuthentication`; add property‑grid editors/masking + localization.
- **Commit:** `feat(ssh_dotnet): wire certificate (private-key) authentication + property-grid UI`

### Phase 8 — Feature tests & manual verification
- Implement the **auth matrix** (§10.3) as `Unit` tests against real key fixtures (§10.8) — assert
  on SSH.NET concrete types + `.Username` (no mocking needed for construction).
- Add `[Category("Integration")]` end‑to‑end tests (opt‑in) against a real/Docker SSH server for true
  key authentication (unencrypted + passphrase + wrong passphrase).
- Manual matrix: ed25519 & RSA keys, encrypted & unencrypted, OpenSSH & PEM; happy path + each
  failure message; folder‑inherited key path.
- **Commit:** `test(ssh_dotnet): auth matrix + security tests + runtime key fixtures`

### Phase 9 — Gate dry‑run & docs  ·  *(PR 2)*
- Re‑run Copilot/Sonar expectations mentally against §7 checklist; update `CHANGELOG.md`,
  `IMPLEMENTATION_NOTES.md`, and the connection‑property docs.
- **Document the SQL‑backend limitation** (§5.6) in CHANGELOG/notes.
- **Commit:** `docs(ssh_dotnet): changelog + cert-login notes + SQL limitation`
  *(→ open **PR 2** after this phase)*

---

## 7. Quality Conventions Checklist (keep NEW code compliant)

- [ ] **No** `catch (Exception)` — catch specific types; one documented survivability helper max.
- [ ] **No** `DateTime.Now`/`DateTime.UtcNow` for durations — use `Stopwatch`/`TickCount64`.
- [ ] Every method **Cognitive Complexity ≤ 15**.
- [ ] Types **PascalCase**. **Released** protocols' serialized strings/enum values **unchanged**;
      `SSH_DotNet` identifiers/strings renamed wholesale per §3.4 (unreleased).
- [ ] Pure helpers `static`; ctor‑only fields `readonly`; no dead stores/unused params.
- [ ] `IDisposable` follows the canonical pattern; all `IDisposable` members disposed.
- [ ] Secrets only via `ICryptographyProvider`; never logged; honour `SavePassword`.
- [ ] No reflection/accessibility bypass without an audited, justified wrapper.
- [ ] New connection property ⇒ all 7 serialization touch‑points + inherit flag + localization +
      round‑trip test (incl. backward‑compat load).

---

## 8. Risks & Mitigations

| Risk | Impact | Mitigation |
|------|--------|------------|
| Renames break serialized format | **N/A — feature unreleased**, no saved production data | Rename enum/strings/identifiers freely; only migrate our own local test samples; never touch released protocols’ strings |
| SSH.NET lacks public resize API | Hotspot can’t be deleted | Encapsulate + mark reviewed‑safe; bounded inputs; track upstream SSH.NET |
| `Connect()` refactor regresses behaviour | Broken connections | Decompose with tests at each step; keep behaviour identical; manual matrix |
| **Sync‑over‑async bridge** at `Connect()` boundary (S6966 fix) | UI‑thread deadlock / hang on connect | Confirm `ConnectionInitiator` calling thread; run async core with `ConfigureAwait(false)`; if caller may be on UI thread, execute core off‑thread and marshal only UI callbacks. **Do not** change `ProtocolBase` signature (would touch released protocols) |
| Passphrase stored encrypted but key file world‑readable | Key exposure outside our control | Document that key‑file security is the user’s OS responsibility; we protect only stored passphrase |
| Win11‑only test execution | Can’t verify locally | Documented `SupportedOSPlatformVersion=10.0.19041.0` local override; CI runs on Win11 |
| Quality gate evaluates **new code** only | Pre‑existing debt may still warn | Focus fixes on PR‑touched files; new feature code must be clean from the start |

---

## 9. Acceptance Criteria (maps to the gates)

1. SonarCloud Quality Gate **passes**: Reliability ≥ A, **0** open Security Hotspots on new code.
2. Copilot review: no remaining generic‑catch / dead‑store / dispose‑pattern / naming findings on
   touched files.
3. Certificate auth works for: unprotected key, passphrase‑protected key, wrong passphrase (clear
   error), missing file (clear error), and coexistence with password/keyboard‑interactive.
4. `confcons.xml`/CSV **round‑trip** for the new properties; a connections file that **lacks** the
   new attributes still loads and defaults cleanly.
5. Full solution builds green (VS 2026); SSH_DotNet `Unit` tests green on Win10 via the local override
   (`Category=Unit`); reflection‑driven `XmlSerializationLifeCycleTests` green (proves XML round‑trip
   of the new props); explicit CSV + security serialization tests green.
6. No duplicate test files; cognitive complexity ≤ 15 across touched methods.
7. Auth matrix (§10.3) and security tests (§10.6) implemented and passing; `[Category]` taxonomy in place.

---

## 10. Testing Strategy (detailed)

> **Verified against the existing test infrastructure** (`mRemoteNGTests`). The headline insight:
> the connection model’s round‑trip tests are **reflection‑driven**, so the new properties get
> **free XML coverage** — but **CSV is positional and needs explicit work**. Build on the existing
> harness; don’t reinvent it.

### 10.1 Test taxonomy & categories (NEW — define and apply)

The suite currently mixes fast logic tests with UI/STA and (future) server‑dependent tests, and can
only **run** on Win10 via the `-p:SupportedOSPlatformVersion=10.0.19041.0` override (≈103 pre‑existing
failures are environment/UI). Introduce NUnit `[Category]` traits so runs are deterministic:

| Category | Meaning | Runs in CI / locally |
|----------|---------|----------------------|
| `Unit` | Pure logic, no SSH server, no UI thread. Auth‑method construction, serialization, parsers, dispose. | Always (fast) |
| `Integration` | Needs a reachable SSH endpoint (real connect/auth). | Opt‑in / nightly; **not** required for the gate |
| `RequiresUI` | WinForms control / STA‑thread (terminal rendering). | Win11 / STA only |

Local fast loop: filter to `Unit` (`/TestCaseFilter:'Category=Unit'`) so cert work isn’t masked by
the unrelated UI failures.

### 10.2 Serialization coverage — lean on the reflection guardrail

| Format | Mechanism | Coverage for the 2 new props | Action |
|--------|-----------|------------------------------|--------|
| **XML values** | `IntegrationTests/XmlSerializationLifeCycleTests.AllPropertiesCorrectWhenSerializingThenDeserializing` populates via reflection (`RandomizeValues()` mutates **all settable props**) and asserts each `GetSerializableProperties()` entry round‑trips. The new props are **not** in the exclusion list → auto‑included. | **AUTOMATIC.** Test **fails** if XML write/read is incomplete. | No new test needed; just wire XML write **and** read in Phase 6 and watch this test stay green. |
| **XML inheritance** | `AllInheritanceCorrectWhenSerializingThenDeserializing` + `GetRandomizedInheritance` (reflection over inheritance bool props). | **AUTOMATIC** for the new `Inherit*` flags. | None. |
| **CSV** | `Csv/CsvConnectionsSerializerMremotengFormatTests` uses a **hand‑built** `BuildConnectionInfo` and asserts **specific positional columns** — *not* reflection. | **NOT covered.** | **Add explicit CSV tests** (below) + the column‑count assert (§Phase 6). Highest‑risk area — positional drift bit us in the merge. |

CSV tests to add (Phase 6):
- Serialize a connection with `SshDotNetPrivateKeyFile` + passphrase → assert the two value columns
  and two `Inherit…` columns appear at the **correct positions** and header/row counts match.
- Round‑trip through `CsvConnectionsDeserializerMremotengFormat` → values restored; passphrase
  honours `SaveFilter`.
- Guard: assert `header.Split(';').Length == row.Split(';').Length` for a fully‑populated connection.

> Also check consumers of the stale `TestHelpers/SerializableConnectionInfoAllPropertiesOfType<T>`
> (an explicit list already missing `SSHDotNetPortForwardRules`). If a test asserts completeness via
> it, update it; if it’s narrow‑purpose, leave it (don’t expand scope needlessly).

> **⚠ The XML round‑trip guardrail does NOT prove encryption‑at‑rest.**
> `AllPropertiesCorrectWhenSerializingThenDeserializing` compares the property **value** after a full
> round‑trip — it passes whether the on‑disk passphrase is **ciphertext or plaintext** (plaintext‑in →
> plaintext‑out still round‑trips). So if the passphrase is mistakenly serialized like the *key path*
> (plaintext) instead of like `Password` (encrypted), this test stays **green** while silently leaking
> the secret. The **§10.6 ciphertext‑at‑rest test is the only guard** against that — it is mandatory,
> not optional.

### 10.3 Authentication tests — close the real gap

Existing `SSHAuthenticationProviderTests` cover **only validation** (null/empty username, null/empty
path, `FileNotFoundException`) using a `_tempKeyFile`. **No test loads a real, valid key.** Add a
fixture (see §10.8) and the following `Unit` tests against the reshaped
`GetAuthenticationMethods(SshAuthContext)` and `CreatePrivateKeyAuth`:

| Case | Expectation |
|------|-------------|
| Key path set, unencrypted key | result contains `PrivateKeyAuthenticationMethod` with correct `Username` |
| Key path set, correct passphrase | loads; contains `PrivateKeyAuthenticationMethod` |
| Key path set, **wrong** passphrase | throws the **specific** SSH.NET passphrase exception (not generic) |
| Key path set, missing file | `FileNotFoundException` with actionable message |
| Key + password both set | **both** methods present, **key ordered first** (OpenSSH behaviour) |
| Key only (no password) | password method absent; key + keyboard‑interactive present |
| Passphrase fallback flag (if enabled) | uses `Password` as passphrase only when explicitly enabled |

Assertion pattern (matches existing tests): `Assert.That(m, Is.TypeOf<PrivateKeyAuthenticationMethod>())`
and check `.Username` — SSH.NET concrete types, **no mocking framework needed** for construction.

### 10.4 Testability seam for the SSH client (enables pipeline/dispose/cancellation unit tests)

Today `SSHConnectionManager.CreateConnection` returns SSH.NET’s concrete `SshClient`, so
`Connect()`/`ConnectCoreAsync`, error mapping, disposal and cancellation are **only** testable against
a real server. **DECISION (2026‑06‑21): add the seam.** Introduce a thin `ISshClientAdapter`
(`ConnectAsync(ct)`, `CreateShellStream(...)`, `ErrorOccurred`, `Dispose`/`DisposeAsync`) with a
production wrapper over `SshClient` and a **fake** for tests. `SshConnectionManager` becomes a factory
returning `ISshClientAdapter`; `ProtocolSshDotNet` depends on the interface. This makes the async
pipeline unit‑testable (simulate connect‑ok, auth‑fail, timeout, cancellation) without a server, and
is implemented as part of the Phase‑4 restructure (§4.2/§4.3).

### 10.5 Async pipeline & lifecycle tests (Phase 4)

With the seam in place (`Unit`):
- `ConnectCoreAsync` honours a cancelled `CancellationToken` (no hang; returns/throws promptly).
- Auth failure → `Connect()` returns `false`, `State == Error`, error event raised with a specific message.
- **Sync bridge doesn’t deadlock:** test the bridge from both an MTA worker thread and (if reachable)
  a synchronization‑context thread, asserting completion within a timeout.
- `Dispose()` is **idempotent** (double‑dispose no‑throw) and disposes the `ISshClientAdapter`,
  `_shellStream`, both CTS, `_tunnelManager` (verify via fake/flags).
- Cancellation stops the I/O loops (tasks complete) before disposal.

### 10.6 Security tests (make the §5.5 checklist executable — `Unit`)

- **Ciphertext at rest:** serialize a connection with a known passphrase; assert the raw
  `SshDotNetPrivateKeyPassphrase` XML attribute value **is not** the plaintext (is ciphertext) and
  **decrypts back** to the original.
- **SaveFilter honoured:** serialize with `new SaveFilter { SavePassword = false }` (the ctor is
  `SaveFilter(bool disableEverything=false)` with settable `SavePassword`; **not** a `savePassword`
  ctor param) → the passphrase attribute is `""` (mirror existing
  `Password`/`SerializerRespectsSaveFilterSettings` tests).
- **No secret leakage in logs:** capture `Runtime.MessageCollector` during a key load with passphrase
  and assert the passphrase string **never appears** in any emitted message (only the key file name).

### 10.7 UI / property‑grid (`RequiresUI`, lightweight)

- The new properties are visible for `SshDotNet` and hidden for other protocols
  (`AttributeUsedInProtocol`), and the passphrase uses the password (masked) editor. Keep these thin;
  rely on existing property‑grid test patterns if present, else cover via the attribute metadata in a
  `Unit` test (reflect the attributes — no UI needed).

### 10.8 Test fixtures — key material

Need ed25519 + RSA keys, each **encrypted and unencrypted**. **DECISION (2026‑06‑21): generate at
test time** using **BouncyCastle** (already referenced) — **no key material committed** to the repo
(secret‑scanner‑clean), deterministic per run. Build a `TestKeyFactory` helper that emits OpenSSH/PEM
keys (encrypted with a known passphrase + unencrypted) to temp files via `OneTimeSetUp`, cleaned in
`OneTimeTearDown` (extend the existing `_tempKeyFile` pattern). Verify the generated formats actually
load through SSH.NET `PrivateKeyFile` (guards against BouncyCastle/SSH.NET format mismatches).

### 10.9 Per‑phase test deliverables

| Phase | Test deliverable |
|-------|------------------|
| 1 (reliability) | Dispose idempotency + timing‑uses‑Stopwatch tests (where seam allows) |
| 3 (rename) | Existing tests compile under new names/namespace; **migrate sample `confcons.xml`** to new attribute strings |
| 4 (decompose+async) | Seam‑based pipeline/cancellation/dispose tests (§10.5) |
| 5 (dedup) | **Diff the duplicate test copies before deleting** (they may have diverged — keep the superset); confirm no test lost |
| 6 (properties) | XML auto‑coverage stays green; **add CSV column/round‑trip tests** (§10.2); security serialization tests (§10.6) |
| 7 (auth+UI) | Auth matrix (§10.3); attribute‑visibility test (§10.7) |
| 8 | Integration (`[Category("Integration")]`, opt‑in) against a real/Docker SSH server for true end‑to‑end key auth; manual matrix |

---

## 11. Appendix — Key references discovered

- Auth seam: `SSHAuthenticationProvider.GetAuthenticationMethods` called at `ProtocolSSH_DotNet.cs:226`;
  placeholder `TryCreateKeyAuthenticationFromConnectionInfo` returns `null` (the `// TODO`).
- Working loaders already present: `CreatePrivateKeyAuth` / `CreatePrivateKeyAuthFromString`
  (handle with/without passphrase).
- Property template: `AbstractConnectionRecord.cs:426–434`; inherit `ConnectionInfoInheritance.cs:172–176`.
- Secret serialization: encrypt `XmlConnectionNodeSerializer28.cs:61`, decrypt `XmlConnectionsDeserializer.cs:218`.
- Plaintext serialization: write `…Serializer28.cs:72`, read `…Deserializer.cs:497`, inherit `:515`.
- Gate data pulled from PR #2997: Copilot 127 comments/37 files; SonarCloud 40 annotations
  (11 failure / 29 warning), QG failed on 2 hotspots + Reliability E.
- **Test infrastructure (verified):**
  - `ConnectionInfo.GetSerializableProperties()` (`ConnectionInfo.cs:144`) = reflection + exclusion
    list (`Parent,Name,Hostname,Port,Inheritance,…`) — new props auto‑included.
  - `TestHelpers/Randomizer.cs:79` `RandomizeValues<T>()` = reflection over all settable props (auto).
  - `IntegrationTests/XmlSerializationLifeCycleTests.cs` `AllPropertiesCorrectWhenSerializingThenDeserializing`
    (L103) + `AllInheritanceCorrectWhenSerializingThenDeserializing` (L131) = auto round‑trip guardrails.
  - `Csv/CsvConnectionsSerializerMremotengFormatTests` = positional, hand‑built `BuildConnectionInfo`
    (L135) — **not** auto‑covered.
  - `Connection/Protocol/SSH_DotNet/SSHAuthenticationProviderTests.cs` = validation‑only today
    (`_tempKeyFile`); no successful key‑load test → the feature‑test gap.
  - `TestHelpers/SerializableConnectionInfoAllPropertiesOfType<T>` = stale explicit list (lacks
    `SSHDotNetPortForwardRules`).
```
