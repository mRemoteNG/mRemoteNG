# run-tests.ps1 - Test runner wrapper (v9)
# Handles build, then delegates to run-tests-core.sh (bash) for test execution.
# Bash avoids PowerShell pipeline back-pressure that crashes testhost on .NET 10.
#
# CRITICAL: --verbosity normal required (minimal crashes testhost on .NET 10).
# CRITICAL: --results-directory must be OUTSIDE the repo (TestResults in repo causes crashes).
# CRITICAL: Do NOT create testhost.runtimeconfig.json (BOM from Set-Content crashes testhost).

param(
    [switch]$Headless,
    [switch]$NoBuild,
    [switch]$Sequential
)

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$testDll  = "$repoRoot\mRemoteNGTests\bin\x64\Release\mRemoteNGTests.dll"
$testDir  = "$repoRoot\mRemoteNGTests\bin\x64\Release"

Write-Host "=== mRemoteNG Test Runner v9 ===" -ForegroundColor Cyan

# Build
if (-not $NoBuild) {
    Write-Host "[Build] Full build..." -ForegroundColor Yellow
    & pwsh -NoProfile -ExecutionPolicy Bypass -File "$repoRoot\build.ps1"
    if ($LASTEXITCODE -ne 0) {
        Write-Host "[Build] FAILED" -ForegroundColor Red
        exit 1
    }
}

if (-not (Test-Path $testDll)) {
    Write-Host "[ERROR] Test DLL not found: $testDll" -ForegroundColor Red
    exit 1
}

# Backup test DLL (protect from crash deletion)
$backupDir = "$testDir\.backup"
if (-not (Test-Path $backupDir)) { New-Item -ItemType Directory -Path $backupDir -Force | Out-Null }
Copy-Item $testDll "$backupDir\mRemoteNGTests.dll" -Force

# Delegate to bash runner (avoids PowerShell pipeline back-pressure issues)
$bashScript = "$repoRoot\run-tests-core.sh"
if (-not (Test-Path $bashScript)) {
    Write-Host "[ERROR] Bash runner not found: $bashScript" -ForegroundColor Red
    exit 1
}

Write-Host "[Run] Delegating to bash runner..." -ForegroundColor Yellow
$headlessArg = if ($Headless) { "headless" } else { "full" }
& bash $bashScript $headlessArg
$exitCode = $LASTEXITCODE

# Restore DLL if crash deleted it
if (-not (Test-Path $testDll) -and (Test-Path "$backupDir\mRemoteNGTests.dll")) {
    Write-Host "`n[Recovery] Restoring test DLL from backup" -ForegroundColor Yellow
    Copy-Item "$backupDir\mRemoteNGTests.dll" $testDll -Force
}

exit $exitCode
