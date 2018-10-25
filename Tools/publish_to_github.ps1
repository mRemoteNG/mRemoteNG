param (
    [string]
    [Parameter(Mandatory=$true)]
    #
    $Owner,

    [string]
    [Parameter(Mandatory=$true)]
    #
    $Repository,

    [string]
    [Parameter(Mandatory=$true)]
    #
    $ReleaseTitle,

    [string]
    [Parameter(Mandatory=$true)]
    #
    $TagName,

    [string]
    [Parameter(Mandatory=$true)]
    # Either the SHA of the commit to target or the branch name.
    $TargetCommitish,

    [string]
    [Parameter(Mandatory=$true)]
    #
    $Description,

    [string]
    [Parameter(Mandatory=$true)]
    [ValidateSet("true","false")]
    # true/false
    $IsDraft,

    [string]
    [Parameter(Mandatory=$true)]
    [ValidateSet("true","false")]
    # true/false
    $IsPrerelease,

    [string]
    [Parameter(Mandatory=$true)]
    # Path to the folder which contains release assets to upload
    $ReleaseFolderPath,

    [string]
    [Parameter(Mandatory=$true)]
    # The OAuth2 token to use for authentication.
    $AuthToken,

    [switch]
    # Enable this switch to treat $Description as a Base64 encoded string. It will be decoded before being used elsewhere in the script.
    $DescriptionIsBase64Encoded
)


if ($DescriptionIsBase64Encoded) {
    $Description = ([System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($Description)))
}


. "$PSScriptRoot\github_functions.ps1"


$releaseFolderItems = Get-ChildItem -Path $ReleaseFolderPath
$mrngPortablePath = ($releaseFolderItems | ?{$_.Name -match "portable-[\d\.]+\.zip"}).FullName
$mrngNormalPath = ($releaseFolderItems | ?{$_.Name -match "installer-[\d\.]+\.msi"}).FullName
$mrngPortableSymbolsPath = ($releaseFolderItems | ?{$_.Name -match "mremoteng-portable-symbols-[\d\.]+\.zip"}).FullName
$mrngNormalSymbolsPath = ($releaseFolderItems | ?{$_.Name -match "mremoteng-symbols-[\d\.]+\.zip"}).FullName


$release = Publish-GitHubRelease -Owner $Owner -Repository $Repository -ReleaseTitle $ReleaseTitle -TagName $TagName -TargetCommitish $TargetCommitish -Description $Description -IsDraft ([bool]::Parse($IsDraft)) -IsPrerelease ([bool]::Parse($IsPrerelease)) -AuthToken $AuthToken
$zipUpload = Upload-GitHubReleaseAsset -UploadUri $release.upload_url -FilePath $mrngPortablePath -ContentType "application/zip" -AuthToken $AuthToken -Label "Portable Edition (zip)"
$msiUpload = Upload-GitHubReleaseAsset -UploadUri $release.upload_url -FilePath $mrngNormalPath -ContentType "application/octet-stream" -AuthToken $AuthToken -Label "Normal Edition (msi)"

$portableEditionSymbols = Upload-GitHubReleaseAsset -UploadUri $release.upload_url -FilePath $mrngPortableSymbolsPath -ContentType "application/zip" -AuthToken $AuthToken -Label "Portable Edition Debug Symbols"
$normalEditionSymbols = Upload-GitHubReleaseAsset -UploadUri $release.upload_url -FilePath $mrngNormalSymbolsPath -ContentType "application/zip" -AuthToken $AuthToken -Label "Normal Edition Debug Symbols"
Write-Output (Get-GitHubRelease -Owner $Owner -Repository $Repository -ReleaseId $release.id -AuthToken $AuthToken)