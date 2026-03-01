<#
.SYNOPSIS
    Collects diagnostics information for mRemoteNG troubleshooting.

.DESCRIPTION
    This script gathers mRemoteNG log files, configuration files, and system information
    into a ZIP archive to assist developers in debugging issues.
    
    Collected items:
    - mRemoteNG.log (and rotated logs)
    - Configuration files (*.xml, *.config, *.json) from %APPDATA%\mRemoteNG
    - System Information (OS, .NET versions)
    - Installed mRemoteNG version (if detectable)

.PARAMETER OutputPath
    Optional. The full path for the output ZIP file. Defaults to the current user's Desktop.

.EXAMPLE
    .\CollectDiagnostics.ps1
    Collects diagnostics to the current user's Desktop.
#>

param(
    [string]$OutputPath = "$([Environment]::GetFolderPath('Desktop'))\mRemoteNG_Diagnostics_$(Get-Date -Format 'yyyyMMdd_HHmmss').zip"
)

$ErrorActionPreference = "Stop"

function Get-mRemoteNGVersion {
    $installPaths = @(
        "${env:ProgramFiles(x86)}\mRemoteNG\mRemoteNG.exe",
        "$env:ProgramFiles\mRemoteNG\mRemoteNG.exe"
    )
    
    foreach ($path in $installPaths) {
        if (Test-Path $path) {
            try {
                return (Get-Item $path).VersionInfo.ProductVersion
            } catch {
                return "Unknown (Error reading version from $path)"
            }
        }
    }
    return "Not found in standard locations"
}

try {
    Write-Host "Starting mRemoteNG Diagnostics Collection..." -ForegroundColor Cyan

    $tempDir = Join-Path $env:TEMP ("mRemoteNG_Diagnostics_" + [Guid]::NewGuid().ToString())
    New-Item -ItemType Directory -Path $tempDir -Force | Out-Null
    
    $logsDest = Join-Path $tempDir "Logs"
    $configDest = Join-Path $tempDir "Config"
    $sysInfoFile = Join-Path $tempDir "SystemInfo.txt"

    New-Item -ItemType Directory -Path $logsDest -Force | Out-Null
    New-Item -ItemType Directory -Path $configDest -Force | Out-Null

    # 1. Collect Logs & Configs from AppData
    $appDataPath = "$env:APPDATA\mRemoteNG"
    if (Test-Path $appDataPath) {
        Write-Host "Collecting logs and configs from: $appDataPath"
        
        # Logs
        Get-ChildItem -Path $appDataPath -Filter "mRemoteNG.log*" | Copy-Item -Destination $logsDest
        
        # Configs (xml, config, json)
        Get-ChildItem -Path $appDataPath | Where-Object { $_.Extension -in ".xml", ".config", ".json" } | Copy-Item -Destination $configDest
    } else {
        Write-Warning "mRemoteNG AppData folder not found at: $appDataPath"
        "mRemoteNG AppData folder not found at: $appDataPath" | Out-File $sysInfoFile -Append
    }

    # 2. System Information
    Write-Host "Collecting System Information..."
    
    "=== mRemoteNG Diagnostics Report ===" | Out-File $sysInfoFile
    "Generated: $(Get-Date)" | Out-File $sysInfoFile -Append
    "User: $env:USERNAME" | Out-File $sysInfoFile -Append
    "-------------------------------------" | Out-File $sysInfoFile -Append
    
    "mRemoteNG Version: $(Get-mRemoteNGVersion)" | Out-File $sysInfoFile -Append
    "" | Out-File $sysInfoFile -Append
    
    "=== OS Information ===" | Out-File $sysInfoFile -Append
    try {
        Get-ComputerInfo | Select-Object CsName, OsName, OsVersion, OsBuildNumber, OsArchitecture, WindowsVersion | Out-String | Out-File $sysInfoFile -Append
    } catch {
        "Error getting OS Info: $_" | Out-File $sysInfoFile -Append
    }
    
    "=== .NET Framework Versions ===" | Out-File $sysInfoFile -Append
    try {
        Get-ChildItem 'HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full' -ErrorAction SilentlyContinue | 
            Select-Object Version, Release | Out-String | Out-File $sysInfoFile -Append
    } catch {
        "Error getting .NET Info: $_" | Out-File $sysInfoFile -Append
    }

    "=== Environment Variables (mRemoteNG related) ===" | Out-File $sysInfoFile -Append
    Get-ChildItem Env: | Where-Object { $_.Name -like "*mRemoteNG*" } | Out-String | Out-File $sysInfoFile -Append

    # 3. Zip it
    Write-Host "Creating archive at: $OutputPath"
    if (Test-Path $OutputPath) { Remove-Item $OutputPath -Force }
    Compress-Archive -Path "$tempDir\*" -DestinationPath $OutputPath

    # Cleanup
    Remove-Item $tempDir -Recurse -Force
    
    Write-Host "Success! Diagnostics saved to: $OutputPath" -ForegroundColor Green
    Write-Host "Please review the contents of the zip file before sharing, as it may contain sensitive configuration data." -ForegroundColor Yellow

} catch {
    Write-Error "An error occurred: $_"
    if (Test-Path $tempDir) { Remove-Item $tempDir -Recurse -Force }
}
