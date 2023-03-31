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

. "$PSScriptRoot\github_functions.ps1"

Write-Output ""
Write-Output "+===========================================================================================+"
Write-Output "|                          Beginning mRemoteNG Installer Post Build                         |"
Write-Output "+===========================================================================================+"
Format-Table -AutoSize -Wrap -InputObject @{
    "SolutionDir" = $SolutionDir
    "TargetDir" = $TargetDir
    "TargetFileName" = $TargetFileName
    "ConfigurationName" = $ConfigurationName
    "CertificatePath" = $CertificatePath
    "ExcludeFromSigning" = $ExcludeFromSigning
}

$IsAppVeyor = !([string]::IsNullOrEmpty($Env:APPVEYOR_BUILD_FOLDER))

if ( $IsAppVeyor -and ($ConfigurationName.ToUpper() -match "RELEASE") -and (($env:APPVEYOR_PROJECT_NAME).ToUpper() -notcontains "(CI)") ) {

    & "$PSScriptRoot\sign_binaries.ps1" -TargetDir $TargetDir -CertificatePath $CertificatePath -CertificatePassword $CertificatePassword -ConfigurationName $ConfigurationName -Exclude $ExcludeFromSigning -SolutionDir $SolutionDir

    & "$PSScriptRoot\verify_binary_signatures.ps1" -TargetDir $TargetDir -ConfigurationName $ConfigurationName -CertificatePath $CertificatePath -SolutionDir $SolutionDir
    
    & "$PSScriptRoot\rename_and_copy_installer.ps1" -SolutionDir $SolutionDir -BuildConfiguration $ConfigurationName.Trim()
    
    & "$PSScriptRoot\create_upg_chk_files.ps1" -WebsiteTargetOwner $env:WEBSITE_TARGET_OWNER -WebsiteTargetRepository $env:WEBSITE_TARGET_REPOSITORY -PreTagName $env:NightlyBuildTagName -TagName $env:APPVEYOR_BUILD_VERSION -ProjectName $env:APPVEYOR_PROJECT_NAME
    
    & "$PSScriptRoot\update_and_upload_website_release_json_file.ps1" -WebsiteTargetOwner $env:WEBSITE_TARGET_OWNER -WebsiteTargetRepository $env:WEBSITE_TARGET_REPOSITORY -PreTagName $env:NightlyBuildTagName -TagName $env:APPVEYOR_BUILD_VERSION -ProjectName $env:APPVEYOR_PROJECT_NAME

    & "$PSScriptRoot\update_and_upload_assemblyinfocs.ps1"

}


Write-Output "End mRemoteNG Installer Post Build"
Write-Output ""
