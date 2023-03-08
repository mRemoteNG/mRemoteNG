param (
    [string]
    $SolutionDir,
    [string]
    $BuildConfiguration
)


$targetVersionedFile = "$SolutionDir\mRemoteNG\bin\x64\$BuildConfiguration\mRemoteNG.exe"
#$fileversion = &"$SolutionDir\Tools\exes\sigcheck.exe" /accepteula -q -n $targetVersionedFile
$prodversion = ((Get-Item -Path $targetVersionedFile).VersionInfo | Select-Object -Property ProductVersion)."ProductVersion"
$src = $SolutionDir + "mRemoteNGInstaller\Installer\bin\x64\$BuildConfiguration\en-US\mRemoteNG-Installer.msi"
$dst = $SolutionDir + "mRemoteNG\bin\x64\$BuildConfiguration\mRemoteNG-Installer-" + $prodversion + ".msi"

# Copy file
Copy-Item $src -Destination $dst -Force
