param(
    [switch]$SelfContained,
    [switch]$Portable,     # Self-contained + PORTABLE flag (settings in app folder, embeds .NET runtime)
    [switch]$Rebuild,
    [switch]$NoRestore,    # Skip dotnet restore (use for fast incremental builds)
    [string]$Configuration = "Release"
)

# Find the newest VS BuildTools installation (VS2026 > VS2022 > etc.)
$vsBasePaths = @(
    "C:\Program Files\Microsoft Visual Studio",
    "C:\Program Files (x86)\Microsoft Visual Studio"
)

$devShell = $null
foreach ($base in $vsBasePaths) {
    if (Test-Path $base) {
        $versions = Get-ChildItem $base -Directory | Sort-Object Name -Descending
        foreach ($ver in $versions) {
            $editions = @("Enterprise", "Professional", "Community", "BuildTools")
            foreach ($ed in $editions) {
                $candidate = Join-Path $ver.FullName "$ed\Common7\Tools\Launch-VsDevShell.ps1"
                if (Test-Path $candidate) {
                    $devShell = $candidate
                    break
                }
            }
            if ($devShell) { break }
        }
    }
    if ($devShell) { break }
}

if (-not $devShell) {
    throw "No Visual Studio installation found. Install VS2026 or VS2022 BuildTools."
}

Write-Host "Using: $devShell"
& $devShell -Arch amd64

$timer = [System.Diagnostics.Stopwatch]::StartNew()

if ($Portable) {
    Write-Host "Building portable edition (self-contained + PORTABLE flag)..."
    if (-not $NoRestore) {
        dotnet restore "D:\github\mRemoteNG\mRemoteNG.sln" --runtime win-x64
    }
    # Uses Release config (recognized by all projects) with explicit PORTABLE + SC properties.
    # PublishReadyToRun=false avoids NETSDK1094 crossgen2 issue; startup impact is negligible.
    msbuild "D:\github\mRemoteNG\mRemoteNG.sln" -m "-verbosity:minimal" "-p:Configuration=Release" "-p:Platform=x64" "-p:DefineConstants=PORTABLE" "-p:SelfContained=true" "-p:RuntimeIdentifier=win-x64" "-p:PublishReadyToRun=false" "-p:SignAssembly=false" "-p:PublishDir=bin\x64\Portable\" -t:Publish
    Write-Host "Portable output: mRemoteNG\bin\x64\Portable\" -ForegroundColor Green
} elseif ($SelfContained) {
    Write-Host "Building self-contained (embedded .NET runtime, non-portable)..."
    if (-not $NoRestore) {
        dotnet restore "D:\github\mRemoteNG\mRemoteNG.sln" --runtime win-x64 /p:PublishReadyToRun=true
    }
    msbuild "D:\github\mRemoteNG\mRemoteNG.sln" -m "-verbosity:minimal" "-p:Configuration=Release" "-p:Platform=x64" "-p:SelfContained=true" "-p:RuntimeIdentifier=win-x64" "-p:PublishDir=bin\x64\Release\win-x64-sc\" -t:Publish
} else {
    Write-Host "Building framework-dependent..."
    if (-not $NoRestore) {
        dotnet restore "D:\github\mRemoteNG\mRemoteNG.sln"
    }
    $msbuildArgs = @('-m', '-verbosity:minimal', "-p:Configuration=$Configuration", '-p:Platform=x64', '-p:SignAssembly=false')
    if ($Rebuild) { $msbuildArgs += '-t:Rebuild' }
    if ($NoRestore) { $msbuildArgs += '-p:RestorePackages=false' }
    msbuild "D:\github\mRemoteNG\mRemoteNG.sln" @msbuildArgs
}

$timer.Stop()
Write-Host "Build completed in $($timer.Elapsed.TotalSeconds.ToString('F1'))s" -ForegroundColor Cyan
