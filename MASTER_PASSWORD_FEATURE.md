# Master Password Feature

## Overview

This feature adds an optional **application-level master password** to mRemoteNG. When configured, the application prompts for the master password on startup before loading any sensitive data (connection files, credentials, encrypted settings).

The master password serves as the **encryption key** for all sensitive settings stored by the application, including:

- Default credentials (username/password/domain)
- SQL database password
- Update proxy password
- Credential repository encryption keys

## How It Works

### Verifier Pattern

The master password is **never stored in plain text**. Instead, a verifier is persisted in user settings (`OptionsSecurityPage.Default.MasterPasswordVerifier`):

1. When setting a master password, a known plaintext string (`"mRemoteNG.MasterPassword.Verifier.v1"`) is encrypted using the user's password and the configured cryptography provider (engine, cipher mode, KDF iterations).
2. The encrypted verifier (with crypto parameters) is stored as XML in settings.
3. On unlock, the verifier is decrypted with the provided password. If the result matches the known plaintext, the password is correct.

### Encryption Key Hierarchy

```
Master Password (user-provided)
    └── Runtime.EncryptionKey (session-level)
        ├── Connection file encryption (confCons.xml)
        ├── Credential repository encryption (credentials.xml)
        └── Settings encryption (default password, SQL password, proxy password)
```

When a master password is active, `Runtime.EncryptionKey` is set to the master password. When no master password is configured, the encryption key falls back to the root node password (or the default `"mR3m"`).

### Migration

When setting or removing the master password, all existing encrypted settings are **automatically re-encrypted** with the new key. This ensures no data is lost when enabling or disabling the feature.

## User Flow

### Enabling

1. **Tools → Master Password** menu item
2. Click **Set Master Password**
3. Enter new password (min 3 characters) and verify
4. All encrypted settings are migrated to the new key

### Startup Unlock

1. Application starts → splash screen closes
2. If master password is configured → password dialog appears
3. User enters password → `MasterPasswordService.TryUnlock()` verifies
4. On success → `Runtime.SetMasterPasswordSession()` sets the encryption key
5. On failure → error message, retry until cancelled (app exits)

### Changing

1. **Tools → Master Password** → **Change Master Password**
2. Enter current password (verified)
3. Enter new password and verify
4. Settings re-encrypted with new key

### Removing

1. **Tools → Master Password** → **Remove**
2. Enter current password (verified)
3. Confirm removal
4. Settings re-encrypted with fallback key

## Files Changed

### New Files (5)

| File | Purpose |
|------|---------|
| `App/MasterPasswordService.cs` | Core logic: set, remove, verify, migrate |
| `App/StartupUnlockService.cs` | Startup unlock flow (master password + connection file) |
| `Security/XmlKeyValidator.cs` | Validate connection/credential file encryption keys |
| `Properties/OptionsSecurityPage.Extensions.cs` | `MasterPasswordVerifier` setting property |
| `UI/Forms/MasterPasswordManager.cs` | Management dialog (set/change/remove) |

### Modified Files (7)

| File | Change |
|------|--------|
| `App/Runtime.cs` | Added `_masterPasswordKey`, `HasActiveMasterPasswordSession`, `SetMasterPasswordSession`, `ClearMasterPasswordSession`, `SetEncryptionKey`, `ResetEncryptionKey`, `UpdateEncryptionKey`, `SyncLoadedCredentialRepositoriesToEncryptionKey` |
| `UI/Forms/frmMain.cs` | Added `StartupUnlockService.EnsureStartupUnlocked()` call before loading connections |
| `UI/Menu/msMain/ToolsMenu.cs` | Added "Master Password" menu item |
| `Config/Serializers/XmlConnectionsDecryptor.cs` | Integrated with `Runtime.HasActiveMasterPasswordSession` and `Runtime.SetEncryptionKey` |
| `UI/Controls/ConnectionInfoPropertyGrid/ConnectionInfoPropertyGrid.cs` | Sync encryption key on root node password change |
| `UI/Forms/FrmPassword.cs` | Added `GetKey(IWin32Window?)` overload |
| `Tools/MiscTools.cs` | Added `PasswordDialog(IWin32Window?, ...)` overload |

### Localization

Added 16 new strings to `Language.resx` and `Language.Designer.cs`:

- `MasterPassword`, `MasterPasswordEnabled`, `MasterPasswordNotSet`
- `MasterPasswordEnabledDescription`, `MasterPasswordNotSetDescription`
- `MasterPasswordSet`, `MasterPasswordChange`, `MasterPasswordCurrent`
- `MasterPasswordNew`, `MasterPasswordVerify`
- `MasterPasswordSaved`, `MasterPasswordRemoved`, `MasterPasswordRemoveConfirm`
- `MasterPasswordInvalid`, `MasterPasswordTooShort`
- `_Remove`

## Security Considerations

- The master password is held in memory as `SecureString` during the session
- The verifier uses the same cryptography provider configured in Security settings (engine, mode, KDF iterations)
- Minimum password length is 3 characters (matching existing `FrmPassword` validation)
- No rate limiting on unlock attempts (consistent with existing connection file password dialog)
- Removing the master password re-encrypts settings with the fallback key (root node password or default)

## Backward Compatibility

- If no master password is configured, the application behaves exactly as before
- Existing connection files and credential repositories continue to work unchanged
- The `HasActiveMasterPasswordSession` flag ensures that when a master password is active, the root node password does not override the encryption key
