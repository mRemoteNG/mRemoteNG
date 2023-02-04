param (
    [string]
    $SolutionDir,
    [string]
    $BuildConfiguration
)


$targetVersionedFile = "$SolutionDir\mRemoteNG\bin\x64\$BuildConfiguration\mRemoteNG.exe"
$version = &"$SolutionDir\Tools\exes\sigcheck.exe" /accepteula -q -n $targetVersionedFile
$src = $SolutionDir + "mRemoteNGInstaller\Installer\bin\$BuildConfiguration\en-US\mRemoteNG-Installer.msi"
$dst = $SolutionDir + "mRemoteNG\bin\x64\$BuildConfiguration\mRemoteNG-Installer-" + $version + ".msi"

# Copy file
Copy-Item $src -Destination $dst -Force
