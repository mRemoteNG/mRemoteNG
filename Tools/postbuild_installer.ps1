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

Write-Output "+=================================================================+"
Write-Output "|             Beginning mRemoteNG Installer Post Build            |"
Write-Output "+=================================================================+"
Write-Output ([pscustomobject]$PSBoundParameters) | Format-Table -AutoSize -Wrap


& "$PSScriptRoot\sign_binaries.ps1" -SolutionDir $SolutionDir -TargetDir $TargetDir -ConfigurationName $ConfigurationName
& "$PSScriptRoot\verify_binary_signatures.ps1" -TargetDir $TargetDir -ConfigurationName $ConfigurationName
& "$PSScriptRoot\rename_installer_with_version.ps1" -SolutionDir $SolutionDir
& "$PSScriptRoot\copy_release_installer.ps1" -SourcePath $TargetDir -DestinationDir (Join-Path -Path $SolutionDir -ChildPath "Release")