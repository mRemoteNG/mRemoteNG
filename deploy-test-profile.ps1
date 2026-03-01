param([string]$Config = "Release")
$ErrorActionPreference = "Stop"

$profileDir = "$PSScriptRoot\.test-profile"
$targetDir  = "$PSScriptRoot\mRemoteNG\bin\x64\$Config"

if (-not (Test-Path $profileDir)) {
    Write-Error ".test-profile/ directory not found. Create it first."
    exit 1
}
if (-not (Test-Path "$targetDir\mRemoteNG.exe")) {
    Write-Error "Build output not found at $targetDir. Run build.ps1 first."
    exit 1
}

# Create Settings subdirectory (portable mode stores ALL config here)
$settingsDir = "$targetDir\Settings"
if (-not (Test-Path $settingsDir)) { New-Item -ItemType Directory -Path $settingsDir | Out-Null }

# Copy all config files into Settings/
Copy-Item "$profileDir\mRemoteNG.settings" "$settingsDir\mRemoteNG.settings" -Force
Copy-Item "$profileDir\extApps.xml"        "$settingsDir\extApps.xml" -Force
Copy-Item "$profileDir\confCons.xml"       "$settingsDir\confCons.xml" -Force

# Adapt settings: update paths for current build location
$content = Get-Content "$settingsDir\mRemoteNG.settings" -Raw

# Update PuTTY path
$content = $content -replace '<setting name="CustomPuttyPath">[^<]*</setting>',
    "<setting name=`"CustomPuttyPath`">$targetDir\PuTTYNG.exe</setting>"

# Clear custom console path (use default)
$content = $content -replace '<setting name="CustomConsPath">[^<]*</setting>',
    '<setting name="CustomConsPath"></setting>'

# Update LogFilePath to Settings directory
$content = $content -replace '<setting name="LogFilePath">[^<]*</setting>',
    "<setting name=`"LogFilePath`">$settingsDir\mRemoteNG.log</setting>"

# Ensure debug logging is enabled
$content = $content -replace '<setting name="TextLogMessageWriterWriteDebugMsgs">[^<]*</setting>',
    '<setting name="TextLogMessageWriterWriteDebugMsgs">True</setting>'

$content | Set-Content "$settingsDir\mRemoteNG.settings" -Encoding UTF8 -NoNewline

Write-Host "  Deployed mRemoteNG.settings + extApps.xml + confCons.xml to Settings/"
Write-Host "  Updated LogFilePath -> $settingsDir\mRemoteNG.log"
Write-Host ""
Write-Host "Test profile deployed to: $targetDir"
Write-Host "Launch: $targetDir\mRemoteNG.exe"
