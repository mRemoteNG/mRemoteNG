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
Write-Output "+============================================================+"
Write-Output "|             Beginning mRemoteNG Post Build                 |"
Write-Output "+============================================================+"
Format-Table -AutoSize -Wrap -InputObject @{
    "SolutionDir" = $SolutionDir
    "TargetDir" = $TargetDir
    "TargetFileName" = $TargetFileName
    "ConfigurationName" = $ConfigurationName
    "CertificatePath" = $CertificatePath
    "ExcludeFromSigning" = $ExcludeFromSigning
}


if ( $ConfigurationName -match "Debug" -and ([string]::IsNullOrEmpty($Env:APPVEYOR_BUILD_FOLDER)) ) { return; } #skip when Debug local developer build
if ( $env:APPVEYOR_PROJECT_NAME -match "(CI)" -and -not ([string]::IsNullOrEmpty($Env:APPVEYOR_BUILD_FOLDER)) ) { return; } #skip when AppVeyor (CI) build

$dstPath = "$($SolutionDir)Release"
New-Item -Path $dstPath -ItemType Directory -Force

# $RunInstaller = $TargetDir -match "\\mRemoteNGInstaller\\Installer\\bin\\"
# $RunPortable = ( ($Targetdir -match "\\mRemoteNG\\bin\\") -and -not ($TargetDir -match "\\mRemoteNGInstaller\\Installer\\bin\\") )

if ( ($ConfigurationName -match "Release") -and ($env:APPVEYOR_PROJECT_NAME -notmatch "(CI)") -and -not ([string]::IsNullOrEmpty($env:WEBSITE_TARGET_OWNER)) -and -not ([string]::IsNullOrEmpty($env:WEBSITE_TARGET_REPOSITORY)) ) {

    Write-Output "-Begin Release Portable"

    & "$PSScriptRoot\tidy_files_for_release.ps1" -TargetDir $TargetDir -ConfigurationName $ConfigurationName

    & "$PSScriptRoot\sign_binaries.ps1" -TargetDir $TargetDir -CertificatePath $CertificatePath -CertificatePassword $CertificatePassword -ConfigurationName $ConfigurationName -Exclude $ExcludeFromSigning -SolutionDir $SolutionDir

    & "$PSScriptRoot\verify_binary_signatures.ps1" -TargetDir $TargetDir -ConfigurationName $ConfigurationName -CertificatePath $CertificatePath -SolutionDir $SolutionDir

    & "$PSScriptRoot\zip_files.ps1" -SolutionDir $SolutionDir -TargetDir $TargetDir -ConfigurationName $ConfigurationName

    & "$PSScriptRoot\create_upg_chk_files.ps1" -WebsiteTargetOwner $env:WEBSITE_TARGET_OWNER -WebsiteTargetRepository $env:WEBSITE_TARGET_REPOSITORY -PreTagName $env:NightlyBuildTagName -TagName $env:APPVEYOR_BUILD_VERSION -ProjectName $env:APPVEYOR_PROJECT_NAME

    & "$PSScriptRoot\update_and_upload_website_release_json_file.ps1" -WebsiteTargetOwner $env:WEBSITE_TARGET_OWNER -WebsiteTargetRepository $env:WEBSITE_TARGET_REPOSITORY -PreTagName $env:NightlyBuildTagName -TagName $env:APPVEYOR_BUILD_VERSION -ProjectName $env:APPVEYOR_PROJECT_NAME

    Write-Output "-End Release Portable"

}


Write-Output "End mRemoteNG Post Build"
Write-Output ""

