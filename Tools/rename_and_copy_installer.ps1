param (
    [string]
    $SolutionDir,
    [string]
    $BuildConfiguration
)

Write-Output ""
Write-Output "===== Begin rename_and_copy_installer.ps1 ====="

$targetVersionedFile = "$SolutionDir\mRemoteNG\bin\x64\$BuildConfiguration\mRemoteNG.exe"
#$fileversion = &"$SolutionDir\Tools\exes\sigcheck.exe" /accepteula -q -n $targetVersionedFile
#$prodversion = ((Get-Item -Path $targetVersionedFile).VersionInfo | Select-Object -Property ProductVersion)."ProductVersion"
$fileversion = ((Get-Item -Path $targetVersionedFile).VersionInfo | Select-Object -Property FileVersion)."FileVersion"
$msiversion = $fileversion

# determine update channel
if ($env:APPVEYOR_PROJECT_NAME -match "(Nightly)") {
    Write-Output "UpdateChannel = Nightly"
    $msiversion = "$msiversion-NB"
} elseif ($env:APPVEYOR_PROJECT_NAME -match "(Preview)") {
    Write-Output "UpdateChannel = Preview"
    $msiversion = "$msiversion-PB"
} elseif ($env:APPVEYOR_PROJECT_NAME -match "(Stable)") {
    Write-Output "UpdateChannel = Stable"
} else {
}

$srcMsi = $SolutionDir + "mRemoteNGInstaller\Installer\bin\x64\$BuildConfiguration\en-US\mRemoteNG-Installer*.msi"
$dstMsi = $SolutionDir + "Release\mRemoteNG-Installer-" + $msiversion + ".msi"
$srcSymbols = $SolutionDir + "mRemoteNGInstaller\Installer\bin\x64\$BuildConfiguration\en-US\mRemoteNG-Installer-Symbols*.zip"
$dstSymbols = $SolutionDir + "Release\mRemoteNG-Installer-Symbols-" + $msiversion + ".zip"

# Copy files
Copy-Item $srcMsi -Destination $dstMsi -Force
Copy-Item $srcSymbols -Destination $dstSymbols -Force

Write-Output "End rename_and_copy_installer.ps1"
Write-Output ""
