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

Write-Output "+===========================================================================================+"
Write-Output "|                               Beginning mRemoteNG Post Build                              |"
Write-Output "+===========================================================================================+"
Format-Table -AutoSize -Wrap -InputObject @{
    "SolutionDir" = $SolutionDir
    "TargetDir" = $TargetDir
    "TargetFileName" = $TargetFileName
    "ConfigurationName" = $ConfigurationName
    "CertificatePath" = $CertificatePath
    "ExcludeFromSigning" = $ExcludeFromSigning
}

# Move dlls resurses into folder
#Remove-Item -Path "$TargetDir\libs" -Recurse -ErrorAction Ignore
#New-Item -ItemType "directory" -Force -Path $TargetDir -Name "libs"
#Move-Item -Path "$TargetDir\*.dll" -Destination "$TargetDir\libs" -force


###

# Move lang resurses into folder
#Remove-Item -Path "$TargetDir\languages" -Recurse -ErrorAction Ignore
#New-Item -ItemType "directory" -Force -Path $TargetDir -Name "languages"
#"cs-CZ,de,el,en-US,es-AR,es,fr,hu,it,lt,ja-JP,ko-KR,nb-NO,nl,pt,pt-BR,pl,ru,uk,tr-TR,zh-CN,zh-TW,fi-FI".Split(",") | ForEach {
#    Move-Item -Path "$TargetDir\$_" -Destination "$TargetDir\languages" -force
# }
###

# Currently targeting x64, shouldn't need to manually set LargeAddressAware...
#& "$PSScriptRoot\set_LargeAddressAware.ps1" -TargetDir $TargetDir -TargetFileName $TargetFileName
& "$PSScriptRoot\verify_LargeAddressAware.ps1" -TargetDir $TargetDir -TargetFileName $TargetFileName

& "$PSScriptRoot\tidy_files_for_release.ps1" -TargetDir $TargetDir -ConfigurationName $ConfigurationName

& "$PSScriptRoot\sign_binaries.ps1" -TargetDir $TargetDir -CertificatePath $CertificatePath -CertificatePassword $CertificatePassword -ConfigurationName $ConfigurationName -Exclude $ExcludeFromSigning -SolutionDir $SolutionDir

& "$PSScriptRoot\verify_binary_signatures.ps1" -TargetDir $TargetDir -ConfigurationName $ConfigurationName -CertificatePath $CertificatePath -SolutionDir $SolutionDir

& "$PSScriptRoot\zip_files.ps1" -SolutionDir $SolutionDir -TargetDir $TargetDir -ConfigurationName $ConfigurationName

if (![string]::IsNullOrEmpty($CURRENT_GITHUB_USER)) {

    & "$PSScriptRoot\create_upg_chk_files.ps1" -WebsiteTargetOwner $CURRENT_GITHUB_USER -WebsiteTargetRepository $env:WEBSITE_TARGET_REPOSITORY -PreTagName $env:NightlyBuildTagName -TagName $env:APPVEYOR_BUILD_VERSION -ProjectName $env:APPVEYOR_PROJECT_NAME

    & "$PSScriptRoot\update_and_upload_website_release_json_file.ps1" -WebsiteTargetOwner $CURRENT_GITHUB_USER -WebsiteTargetRepository $env:WEBSITE_TARGET_REPOSITORY -PreTagName $env:NightlyBuildTagName -TagName $env:APPVEYOR_BUILD_VERSION -ProjectName $env:APPVEYOR_PROJECT_NAME

}

Write-Output "End mRemoteNG Post Build"
