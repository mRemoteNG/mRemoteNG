param (
    [string]
    $SolutionDir
)


$targetVersionedFile = "$SolutionDir\mRemoteNG\bin\x64\Release\mRemoteNG.exe"
$version = &"$SolutionDir\Tools\exes\sigcheck.exe" /accepteula -q -n $targetVersionedFile
$src = $SolutionDir + "mRemoteNGInstaller\Installer\bin\Release\en-US\mRemoteNG-Installer.msi"
$dst = $SolutionDir + "mRemoteNG\bin\x64\Release\mRemoteNG-Installer-" + $version + ".msi"

# Copy file
Copy-Item $src -Destination $dst -Force
