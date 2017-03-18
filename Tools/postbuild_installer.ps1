param (
    [string]
    [Parameter(Mandatory=$true)]
    $SolutionDir,

    [string]
    [Parameter(Mandatory=$true)]
    $TargetDir,

    [string]
    [Parameter(Mandatory=$true)]
    $TargetFileName,

    [string]
    [Parameter(Mandatory=$true)]
    $ConfigurationName
)

$certificatePath = "C:\mRemoteNG_code_signing_cert.pfx"
$path_signFilesScript = Join-Path -Path $SolutionDir -ChildPath "Tools\signfiles.ps1"
$path_installerRenameScript = Join-Path -Path $SolutionDir -ChildPath "Tools\rename_installer_with_version.ps1"
$path_copyToReleaseScript = Join-Path -Path $SolutionDir -ChildPath "Tools\copy_release_installer.ps1"


# Sign MSI if we are building a release version and the certificate is available
if (($ConfigurationName -match "Release") -and (Test-Path -Path $certificatePath -PathType Leaf)) {
    powershell.exe -ExecutionPolicy Bypass -File $path_signFilesScript $TargetDir
}


# Rename MSI to include version number
powershell.exe -ExecutionPolicy Bypass -File $path_installerRenameScript -SolutionDir $SolutionDir


# Copy MSI to Release folder
if ($ConfigurationName -match "Release") {
    powershell.exe -ExecutionPolicy Bypass -File $path_copyToReleaseScript -SourcePath $TargetDir -DestinationDir (Join-Path -Path $SolutionDir -ChildPath "Release")
}