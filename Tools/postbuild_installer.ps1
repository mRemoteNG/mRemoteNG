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
    $ConfigurationName,

    [string]
    $CertificatePath,

    [string]
    $CertificatePassword,

    [string[]]
    $ExcludeFromSigning
)

Write-Output "+=================================================================+"
Write-Output "|             Beginning mRemoteNG Installer Post Build            |"
Write-Output "+=================================================================+"
Format-Table -AutoSize -Wrap -InputObject @{
    "SolutionDir" = $SolutionDir
    "TargetDir" = $TargetDir
    "TargetFileName" = $TargetFileName
    "ConfigurationName" = $ConfigurationName
    "ExcludeFromSigning" = $ExcludeFromSigning
}


& "$PSScriptRoot\sign_binaries.ps1" -TargetDir $TargetDir -CertificatePath $CertificatePath -CertificatePassword $CertificatePassword -ConfigurationName $ConfigurationName -Exclude $ExcludeFromSigning
& "$PSScriptRoot\verify_binary_signatures.ps1" -TargetDir $TargetDir -ConfigurationName $ConfigurationName -CertificatePath $CertificatePath
& "$PSScriptRoot\rename_installer_with_version.ps1" -SolutionDir $SolutionDir
& "$PSScriptRoot\copy_release_installer.ps1" -SourcePath $TargetDir -DestinationDir (Join-Path -Path $SolutionDir -ChildPath "Release")