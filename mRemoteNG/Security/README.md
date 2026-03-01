# Security Subsystem

This directory contains the encryption, key derivation, and credential management components of mRemoteNG.

## Architecture

```
Security/
  ICryptographyProvider.cs         — Interface for encrypt/decrypt operations
  SymmetricEncryption/
    AeadCryptographyProvider.cs    — AES-256-GCM (AEAD) via BouncyCastle (current default)
    AesCryptographyProvider.cs     — AES-CBC (legacy, for reading old files)
    LegRijndaelCryptographyProvider.cs — Rijndael (legacy compatibility)
  KeyDerivation/
    Pbkdf2KeyDerivationFunction.cs — PBKDF2-HMAC-SHA1 key derivation
  PasswordCreation/
    PasswordIncludesXxx.cs         — Character class generators for password creation
  IKeyProvider.cs                  — Interface for password dialog providers
```

## Encryption Flow

### Encrypting credentials (save)

1. User's master password is obtained via `IKeyProvider` (password dialog)
2. `Pbkdf2KeyDerivationFunction` derives a 256-bit key from the password using PBKDF2-HMAC-SHA1 with a random salt and configurable iterations (default: 600,000)
3. `AeadCryptographyProvider` encrypts the plaintext using AES-256-GCM:
   - Generates a random 96-bit nonce
   - Encrypts with authenticated data (AEAD)
   - Output format: `salt | nonce | ciphertext | GCM-tag`
4. The Base64-encoded result is stored in the XML/SQL field

### Decrypting credentials (load)

1. Master password is obtained from user
2. Salt is extracted from the stored value
3. Key is derived using PBKDF2 with the stored salt and iteration count
4. AES-256-GCM decrypts and verifies the authentication tag
5. If the tag is invalid (wrong password or tampered data), decryption fails

## KDF Iteration Count

The iteration count is configurable in Tools > Options > Security:

| Version | Default Iterations | Maximum |
|---------|-------------------|---------|
| < 1.80  | 1,000             | 50,000  |
| >= 1.80 | 600,000           | 1,000,000 |

The iteration count is stored in the confCons.xml header as the `KdfIterations` attribute. This enables forward compatibility — files encrypted with higher iterations can still be read by code that supports the attribute.

## Master Password

- Stored as a SHA-256 hash in `RootNodeInfo.PasswordString`
- Used only to verify the user knows the password before attempting decryption
- The actual encryption key is derived independently via PBKDF2
- As of v1.80.0, passwords must meet complexity requirements (8+ chars, mixed case, digit)

## External Credential Providers

Located in `ExternalConnectors/`:

| Provider | Protocol | Auth |
|----------|----------|------|
| 1Password CLI | CLI (`op`) | System keychain |
| SecretServer | HTTPS REST | Token-based |
| Passwordstate | HTTPS REST | API key |
| OpenBao | HTTPS REST | Token-based |

As of v1.80.0, all external vault clients enforce HTTPS and validate SSL certificates.

## Security Hardening (v1.80.0)

- PBKDF2 iterations increased from 1,000 to 600,000
- RDP default authentication changed to `WarnOnFailedAuth`
- SSH private key temp files are securely wiped (overwritten before deletion)
- HTTPS and certificate validation enforced for vault clients
- Master password complexity requirements enforced
