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
    # Path to the zip file to upload with the release
    $ZipFilePath,

    [string]
    [Parameter(Mandatory=$true)]
    #Path to the msi file to upload with the release
    $MsiFilePath,

    [string]
    [Parameter(Mandatory=$true)]
    # The OAuth2 token to use for authentication.
    $AuthToken,

    [switch]
    # Enable this switch to treat $Description as a Base64 encoded string. It will be decoded before being used elsewhere in the script.
    $DescriptionIsBase64Encoded
)


$githubUrl = 'https://api.github.com'
if ($DescriptionIsBase64Encoded) {
    $Description = ([System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($Description)))
}


function Publish-Release {
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

        [bool]
        [Parameter(Mandatory=$true)]
        #
        $IsDraft,

        [bool]
        [Parameter(Mandatory=$true)]
        #
        $IsPrerelease,

        [string]
        [Parameter(Mandatory=$true)]
        # The OAuth2 token to use for authentication.
        $AuthToken
    )

    $body_publishRelease = @{
      "tag_name" = $TagName
      "target_commitish" = $TargetCommitish
      "name" = $ReleaseTitle
      "body" = $Description
      "draft" = $IsDraft
      "prerelease" = $IsPrerelease
    }

    $req_publishRelease = Invoke-WebRequest -Uri "$githubUrl/repos/$Owner/$Repository/releases" -Method Post -Headers @{"Authorization"="token $AuthToken"} -Body (ConvertTo-Json -InputObject $body_publishRelease -Compress) -ErrorAction Stop
    $response_publishRelease = ConvertFrom-Json -InputObject $req_publishRelease.Content

    Write-Output $response_publishRelease
}


function Get-GitHubRelease {
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
        $ReleaseId
    )

    $req_getRelease = Invoke-WebRequest -Uri "$githubUrl/repos/$Owner/$Repository/releases/$ReleaseId" -Method Get -ErrorAction Stop
    $response_getRelease = ConvertFrom-Json -InputObject $req_publishRelease.Content

    Write-Output $response_getRelease
}


function Upload-ReleaseAsset {
    param (
        [string]
        [Parameter(Mandatory=$true)]
        $UploadUri,

        [string]
        [Parameter(Mandatory=$true)]
        # Path to the file to upload with the release
        $FilePath,

        [string]
        [Parameter(Mandatory=$true)]
        # Content type of the file
        $ContentType,

        [string]
        [Parameter(Mandatory=$true)]
        # The OAuth2 token to use for authentication.
        $AuthToken
    )

    $UploadUri = $UploadUri -replace "(\{[\w,\?]*\})$"
    $file = Get-Item -Path $FilePath

    $req_uploadZipAsset = Invoke-WebRequest -Uri "$($UploadUri)?name=$($file.Name)" -Method Post -Headers @{"Authorization"="token $AuthToken"} -ContentType $ContentType -InFile $file.FullName -ErrorAction Stop
}



$release = Publish-Release -Owner $Owner -Repository $Repository -ReleaseTitle $ReleaseTitle -TagName $TagName -TargetCommitish $TargetCommitish -Description $Description -IsDraft ([bool]::Parse($IsDraft)) -IsPrerelease ([bool]::Parse($IsPrerelease)) -AuthToken $AuthToken
$zipUpload = Upload-ReleaseAsset -UploadUri $release.upload_url -FilePath $ZipFilePath -ContentType "application/zip" -AuthToken $AuthToken
$msiUpload = Upload-ReleaseAsset -UploadUri $release.upload_url -FilePath $MsiFilePath -ContentType "application/octet-stream" -AuthToken $AuthToken
Write-Output (Get-GitHubRelease -Owner $Owner -Repository $Repository -ReleaseId $release.id)