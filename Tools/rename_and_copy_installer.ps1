param (
    [string]
    $SolutionDir,
    [string]
    $BuildConfiguration
)

Write-Output "===== Begin rename_and_copy_installer.ps1 ====="

$targetVersionedFile = "$SolutionDir\mRemoteNG\bin\x64\$BuildConfiguration\mRemoteNG.exe"
#$fileversion = &"$SolutionDir\Tools\exes\sigcheck.exe" /accepteula -q -n $targetVersionedFile
#$prodversion = ((Get-Item -Path $targetVersionedFile).VersionInfo | Select-Object -Property ProductVersion)."ProductVersion"
$fileversion = ((Get-Item -Path $targetVersionedFile).VersionInfo | Select-Object -Property FileVersion)."FileVersion"
$msiversion = $fileversion

# determine update channel
if ($env:APPVEYOR_PROJECT_NAME -match "(Nightly)") {
    write-host "UpdateChannel = Nightly"
    $msiversion = "$msiversion-NB"
} elseif ($env:APPVEYOR_PROJECT_NAME -match "(Preview)") {
    write-host "UpdateChannel = Preview"
    $msiversion = "$msiversion-PB"
} elseif ($env:APPVEYOR_PROJECT_NAME -match "(Stable)") {
    write-host "UpdateChannel = Stable"
} else {
}

$srcMsi = $SolutionDir + "mRemoteNGInstaller\Installer\bin\x64\$BuildConfiguration\en-US\mRemoteNG-Installer*.msi"
$dstMsi = $SolutionDir + "mRemoteNG\bin\x64\$BuildConfiguration\mRemoteNG-Installer-" + $msiversion + ".msi"

# Copy file
Copy-Item $srcMsi -Destination $dstMsi -Force

