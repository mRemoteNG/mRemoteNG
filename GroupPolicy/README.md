# mRemoteNG Group Policy Templates (ADMX/ADML)

Administrative templates for managing mRemoteNG settings via Group Policy.

## Quick Start

### Local Machine

```powershell
Copy-Item mremoteng.admx C:\Windows\PolicyDefinitions\
Copy-Item en-US\mremoteng.adml C:\Windows\PolicyDefinitions\en-US\
```

Then open `gpedit.msc` > **Computer Configuration** > **Administrative Templates** > **mRemoteNG**.

### Active Directory Domain (Central Store)

```powershell
$store = "\\$env:USERDNSDOMAIN\SYSVOL\$env:USERDNSDOMAIN\Policies\PolicyDefinitions"
Copy-Item mremoteng.admx "$store\"
Copy-Item en-US\mremoteng.adml "$store\en-US\"
```

Then open GPMC, create or edit a GPO, and navigate to **Computer Configuration** > **Administrative Templates** > **mRemoteNG**.

## Available Policies

### Credentials

| Policy | Registry Value | Default | Description |
|--------|---------------|---------|-------------|
| Allow saving passwords | `AllowSavePasswords` | Enabled | Hide password fields when disabled |
| Allow saving usernames | `AllowSaveUsernames` | Enabled | Hide username fields when disabled |
| Allow exporting passwords | `AllowExportPasswords` | Enabled | Strip passwords from exports when disabled |
| Allow exporting usernames | `AllowExportUsernames` | Enabled | Strip usernames from exports when disabled |

### Updates

| Policy | Registry Value | Default | Description |
|--------|---------------|---------|-------------|
| Allow checking for updates | `AllowCheckForUpdates` | Enabled | Master switch for all update checks |
| Allow automatic update checks | `AllowCheckForUpdatesAutomatical` | Enabled | Background update checks |
| Allow manual update checks | `AllowCheckForUpdatesManual` | Enabled | User-initiated update checks |

### Notifications

| Policy | Registry Value | Default | Description |
|--------|---------------|---------|-------------|
| Allow file logging | `AllowLogging` | Enabled | Write log files to disk |
| Allow notifications | `AllowNotifications` | Enabled | Show notifications |
| Allow popup notifications | `AllowPopups` | Enabled | Show popup windows |

## Registry Details

All values are stored under `HKEY_LOCAL_MACHINE\SOFTWARE\mRemoteNG\{Category}` as `REG_DWORD`:
- `1` = Enabled (default behavior)
- `0` = Disabled (restricted)

## Requirements

- mRemoteNG 1.81.0 or later
- Windows with Group Policy support (Pro, Enterprise, Education)

## Enterprise Security Scenarios

**Disable password storage (SOC2/ISO 27001 compliance):**
- Set "Allow saving passwords" to **Disabled**
- Set "Allow exporting passwords" to **Disabled**

**Centrally managed updates (SCCM/Intune environments):**
- Set "Allow checking for updates" to **Disabled**

**Silent operation (kiosk/shared workstations):**
- Set "Allow popup notifications" to **Disabled**
