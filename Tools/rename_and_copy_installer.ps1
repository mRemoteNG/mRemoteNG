param (
    [string]
    $SolutionDir,
    [string]
    $BuildConfiguration
)

$ErrorActionPreference = "Stop"

Write-Output ""
Write-Output "    /===== Begin rename_and_copy_installer =====/"

$targetVersionedFile = "$SolutionDir\mRemoteNG\bin\x64\$BuildConfiguration\mRemoteNG.exe"
#$fileversion = &"$SolutionDir\Tools\exes\sigcheck.exe" /accepteula -q -n $targetVersionedFile
#$prodversion = ((Get-Item -Path $targetVersionedFile).VersionInfo | Select-Object -Property ProductVersion)."ProductVersion"
$fileversion = ((Get-Item -Path $targetVersionedFile).VersionInfo | Select-Object -Property FileVersion)."FileVersion"

Write-Output "fileversion: $fileversion"
$msiversion = $fileversion

# determine update channel
if ($env:APPVEYOR_PROJECT_NAME -match "(Nightly)") {
    Write-Output "        UpdateChannel = Nightly"
    $msiversion = "$msiversion-NB"
} elseif ($env:APPVEYOR_PROJECT_NAME -match "(Preview)") {
    Write-Output "        UpdateChannel = Preview"
    $msiversion = "$msiversion-PB"
} elseif ($env:APPVEYOR_PROJECT_NAME -match "(Stable)") {
    Write-Output "        UpdateChannel = Stable"
} else {
}

$dstPath = "$($SolutionDir)Release"
New-Item -Path $dstPath -ItemType Directory -Force
$srcMsi = $SolutionDir + "mRemoteNGInstaller\Installer\bin\x64\$BuildConfiguration\en-US\mRemoteNG-Installer.msi"
$dstMsi = $dstPath + "\mRemoteNG-Installer-" + $msiversion + ".msi"
#$srcSymbols = $SolutionDir + "mRemoteNGInstaller\Installer\bin\x64\$BuildConfiguration\en-US\mRemoteNG-Installer-Symbols*.zip"
#$dstSymbols = $SolutionDir + "Release\mRemoteNG-Installer-Symbols-" + $msiversion + ".zip"

Write-Output "        Copy Installer file:"
Write-Output "          From: $srcMsi"
Write-Output "            To: $dstMsi"
Write-Output ""
# Copy file
try
{
    Copy-Item $srcMsi -Destination $dstMsi -Force -ErrorAction Stop
    #Copy-Item $srcSymbols -Destination $dstSymbols -Force -ErrorAction Stop
    Write-Host "        [Success!]" -ForegroundColor green
}
catch
{
    Write-Host "        [Failure!]" -ForegroundColor red
    Write-Output $Error[0]
    $PSCmdlet.ThrowTerminatingError()
}


Write-Output ""
Write-Output "    /===== End rename_and_copy_installer.ps1 =====/"
Write-Output ""
