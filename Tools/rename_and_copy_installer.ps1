param (
    [string]
    $SolutionDir,
    [string]
    $BuildConfiguration
)

Write-Output ""
Write-Output "    /===== Begin $($PSCmdlet.MyInvocation.MyCommand) =====/"

$targetVersionedFile = "$SolutionDir\mRemoteNG\bin\x64\$BuildConfiguration\mRemoteNG.exe"
#$fileversion = &"$SolutionDir\Tools\exes\sigcheck.exe" /accepteula -q -n $targetVersionedFile
#$prodversion = ((Get-Item -Path $targetVersionedFile).VersionInfo | Select-Object -Property ProductVersion)."ProductVersion"
$fileversion = ((Get-Item -Path $targetVersionedFile).VersionInfo | Select-Object -Property FileVersion)."FileVersion"
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

$srcMsi = $SolutionDir + "mRemoteNGInstaller\Installer\bin\x64\$BuildConfiguration\en-US\mRemoteNG-Installer.msi"
#$dstMsi = $SolutionDir + "Release\mRemoteNG-Installer-" + $msiversion + ".msi"
$dstMsi = $SolutionDir + "mRemoteNG\bin\x64\$BuildConfiguration\mRemoteNG-Installer-" + $msiversion + ".msi"
#$srcSymbols = $SolutionDir + "mRemoteNGInstaller\Installer\bin\x64\$BuildConfiguration\en-US\mRemoteNG-Installer-Symbols*.zip"
#$dstSymbols = $SolutionDir + "Release\mRemoteNG-Installer-Symbols-" + $msiversion + ".zip"

Write-Output "        Copy Installer file:"
Write-Output "          From: $srcMsi"
Write-Output "            To: $dstMsi"
Write-Output ""
# Copy file
try
{
    Copy-Item $srcMsi -Destination $dstMsi -Force -errorAction stop
    Write-Host "        [Success!]" -ForegroundColor green
}
catch
{
    Write-Host "        [Failure!]" -ForegroundColor red
}
#Copy-Item $srcSymbols -Destination $dstSymbols -Force

Write-Output ""
Write-Output "    /===== End $($PSCmdlet.MyInvocation.MyCommand) =====/"
Write-Output ""
