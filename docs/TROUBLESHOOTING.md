# Troubleshooting Guide

Common issues and solutions for mRemoteNG.

## Installation & Startup

### "You must install .NET Desktop Runtime to run this application"

mRemoteNG v1.79 and earlier require .NET 10 Desktop Runtime installed separately.

**Solutions:**
- Download and install the [.NET 10 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/10.0) for your platform (x64, x86, or ARM64)
- Or use the **self-contained** build (v1.80+) which bundles the runtime — no separate installation needed

### Application crashes on startup

1. Check the error log at `%AppData%\mRemoteNG\mRemoteNG.log`
2. If the log mentions `BinaryFormatter` — upgrade to v1.79.0+ which removed BinaryFormatter usage
3. Try renaming `%AppData%\mRemoteNG\confCons.xml` to test with a fresh configuration
4. Delete `%AppData%\mRemoteNG\pnlLayout.xml` if the crash is related to panel layout

### Settings path issues

mRemoteNG stores settings in `%AppData%\mRemoteNG\` by default. If you launched from a portable path, settings may be stored next to the executable. Check the startup log for "Settings path:" to see the actual location.

## Connection Issues

### RDP: "An internal error has occurred"

- Ensure the target machine has RDP enabled
- Check that NLA (Network Level Authentication) settings match between client and server
- Try changing the RDP authentication level in connection properties

### RDP: Black screen after connecting

- Try toggling the resolution between "Fit to Window" and a fixed resolution
- Disable bitmap caching in the connection's RDP properties
- This can happen when the RDP ActiveX control loses its window handle during resize

### RDP: SmartSize stops working after alt-tab

This was a known issue fixed in v1.79.0 (PR #3116). Upgrade to v1.79.0+.

### SSH/Telnet: Connection refused

- Verify the SSH server is running on the target: `ssh user@host` from a terminal
- Check the port number (default SSH is 22, Telnet is 23)
- If using PuTTY-based protocols, ensure PuTTY is accessible. Check Tools > Options > Advanced > PuTTY path.

### PuTTY: Session names show as percent-encoded (e.g., `%BC%AD%B9%F6`)

This affects CJK (Chinese/Japanese/Korean) session names on .NET 10. Fixed in v1.80.0 by registering `CodePagesEncodingProvider`. Upgrade to v1.80.0+.

### VNC: "Connection failed" immediately

- VNC servers often use port 5900. Verify the port matches your VNC server configuration.
- Some VNC servers require a specific protocol version. Try adjusting the VNC compression/encoding settings.

## Security

### Forgot master password

There is no password recovery mechanism. The master password encrypts all stored credentials. If lost, you must:
1. Delete or rename `confCons.xml`
2. Re-create your connections
3. Re-enter all credentials

### "Password does not meet complexity requirements" (v1.80+)

v1.80.0 enforces minimum password complexity:
- At least 8 characters
- At least one uppercase letter
- At least one lowercase letter
- At least one digit

### Changing PBKDF2 iterations

In Tools > Options > Security, you can adjust the KDF iteration count. Higher values increase security but slow down file open/save. The default is 600,000. Changes take effect on the next save.

## Build Issues

### `MSB4803: ResolveComReference not supported`

This error occurs when using `dotnet build`. mRemoteNG requires full MSBuild from Visual Studio due to COM references (MSTSCLib for RDP).

**Solution:** Use the build script:
```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File build.ps1
```

### Tests not found or wrong output path

Tests build to `bin\x64\Release\` but `dotnet test --no-build` on the `.csproj` looks in `bin\Release\`.

**Solution:** Always run tests against the DLL directly:
```bash
dotnet test "mRemoteNGTests\bin\x64\Release\mRemoteNGTests.dll" --verbosity normal
```

## Configuration

### Connections file location

- Default: `%AppData%\mRemoteNG\confCons.xml`
- Portable mode: next to `mRemoteNG.exe`
- SQL Server: configured in Tools > Options > Connections

### Backing up connections

mRemoteNG creates automatic backups in the settings folder. You can also:
- File > Save As to save a copy
- Export connections via File > Export (XML or JSON format)

### External tools tokens

Available tokens for external tool arguments:
- `%Hostname%`, `%Port%`, `%Username%`, `%Password%`, `%Domain%`
- `%Name%`, `%Description%`
- `%Protocol%` (v1.79+), `%EnvironmentTags%`, `%FolderPath%` (v1.80+)
- `%MacAddress%`, `%UserField%`
