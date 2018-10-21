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
Write-Output "|                  Beginning mRemoteV1 Post Build                 |"
Write-Output "+=================================================================+"
Format-Table -AutoSize -Wrap -InputObject @{
    "SolutionDir" = $SolutionDir
    "TargetDir" = $TargetDir
    "TargetFileName" = $TargetFileName
    "ConfigurationName" = $ConfigurationName
    "ExcludeFromSigning" = $ExcludeFromSigning
}



& "$PSScriptRoot\copy_puttyng.ps1" -SolutionDir $SolutionDir -TargetDir $TargetDir
& "$PSScriptRoot\copy_themes.ps1" -SolutionDir $SolutionDir -TargetDir $TargetDir
& "$PSScriptRoot\move_help_files.ps1" -TargetDir $TargetDir
& "$PSScriptRoot\set_LargeAddressAware.ps1" -TargetDir $TargetDir -TargetFileName $TargetFileName
& "$PSScriptRoot\verify_LargeAddressAware.ps1" -TargetDir $TargetDir -TargetFileName $TargetFileName
& "$PSScriptRoot\tidy_files_for_release.ps1" -TargetDir $TargetDir -ConfigurationName $ConfigurationName
& "$PSScriptRoot\sign_binaries.ps1" -TargetDir $TargetDir -CertificatePath $CertificatePath -CertificatePassword $CertificatePassword -ConfigurationName $ConfigurationName -Exclude $ExcludeFromSigning
& "$PSScriptRoot\verify_binary_signatures.ps1" -TargetDir $TargetDir -ConfigurationName $ConfigurationName -CertificatePath $CertificatePath
& "$PSScriptRoot\zip_symbols.ps1" -SolutionDir $SolutionDir -TargetDir $TargetDir -ConfigurationName $ConfigurationName
& "$PSScriptRoot\zip_portable_files.ps1" -SolutionDir $SolutionDir -TargetDir $TargetDir -ConfigurationName $ConfigurationName