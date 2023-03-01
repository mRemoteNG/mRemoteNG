$githubUrl = 'https://api.github.com'
# GitHub doesn't support the default powershell protocol (TLS 1.0)
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

#$ConfigurationName = $ConfigurationName.Trim()
if (![string]::IsNullOrEmpty($env:APPVEYOR_REPO_NAME)) {
    $CURRENT_GITHUB_USER = $env:APPVEYOR_REPO_NAME.Split("/")[0]
    Install-Module -Name PowerShellForGitHub -Scope CurrentUser
    Set-GitHubConfiguration -DisableTelemetry
    $PSDefaultParameterValues["*-GitHub*:AccessToken"] = "$env:ACCESS_TOKEN"    
}

Function ConvertFrom-Base64($base64) {
    return [System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($base64))
}

Function ConvertTo-Base64($plain) {
    return [Convert]::ToBase64String([Text.Encoding]::UTF8.GetBytes($plain))
}

function Publish-GitHubRelease {
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

    $body = New-GitHubReleaseRequestBody -TagName $TagName -TargetCommitish $TargetCommitish -ReleaseTitle $ReleaseTitle -Description $Description -IsDraft $IsDraft -IsPrerelease $IsPrerelease
    $req_publishRelease = Invoke-WebRequest -Uri "$githubUrl/repos/$Owner/$Repository/releases" -Method Post -Headers @{"Authorization"="token $AuthToken"} -Body $body -ErrorAction Stop
    $response_publishRelease = ConvertFrom-Json -InputObject $req_publishRelease.Content

    Write-Output $response_publishRelease
}


function Edit-GitHubRelease {
    param (
        [string]
        #[Parameter(Mandatory=$true)]
        #
        $Owner,

        [string]
        #[Parameter(Mandatory=$true)]
        #
        $Repository,

        [string]
        #[Parameter(Mandatory=$true)]
        #
        $ReleaseId,

        [string]
        #
        $ReleaseTitle,

        [string]
        #
        $TagName,

        [string]
        # Either the SHA of the commit to target or the branch name.
        $TargetCommitish,

        [string]
        #
        $Description,

        [bool]
        #
        $IsDraft,

        [bool]
        #
        $IsPrerelease,

        [string]
        #[Parameter(Mandatory=$true)]
        # The OAuth2 token to use for authentication.
        $AuthToken
    )

    $body_params = @{
        "TagName" = $TagName
        "TargetCommitish" = $TargetCommitish
        "ReleaseTitle" = $ReleaseTitle
        "Description" = $Description
    }
    if ($PSBoundParameters.ContainsKey("IsDraft")) { $body_params.Add("IsDraft", $IsDraft) }
    if ($PSBoundParameters.ContainsKey("IsPrerelease")) { $body_params.Add("IsPrerelease", $IsPrerelease) }

    $body = New-GitHubReleaseRequestBody @body_params
    $req_editRelease = Invoke-WebRequest -Uri "$githubUrl/repos/$Owner/$Repository/releases/$ReleaseId" -Method Post -Headers @{"Authorization"="token $AuthToken"} -Body $body -ErrorAction Stop
    $response_editRelease = ConvertFrom-Json -InputObject $req_editRelease.Content

    Write-Output $response_editRelease
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
        $ReleaseId,

        [string]
        [Parameter(Mandatory=$true)]
        # The OAuth2 token to use for authentication.
        $AuthToken
    )

    $req_getRelease = Invoke-WebRequest -Uri "$githubUrl/repos/$Owner/$Repository/releases/$ReleaseId" -Method Get -Headers @{"Authorization"="token $AuthToken"} -ErrorAction Stop
    $response_getRelease = ConvertFrom-Json -InputObject $req_getRelease.Content

    Write-Output $response_getRelease
}


function Upload-GitHubReleaseAsset {
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
        $AuthToken,

        [string]
        # A short description label for the asset
        $Label = ""
    )

    $UploadUri = $UploadUri -replace "(\{[\w,\?]*\})$"
    $files = Get-Item -Path $FilePath

    $labelParam = ""
    if ($Label -ne "") {
        $labelParam = "&label=$Label"
    }

    # Get-Item could produce an array of files if a wildcard is provided. (C:\*.txt)
    # Upload each matching item individually
    foreach ($file in $files) {
        Write-Output "Uploading asset to GitHub release: '$($file.FullName)'"
        $req_uploadZipAsset = Invoke-WebRequest -Uri "$($UploadUri)?name=$($file.Name)$labelParam" -Method Post -Headers @{"Authorization"="token $AuthToken"} -ContentType $ContentType -InFile $file.FullName -ErrorAction Stop
    }
}


function New-GitHubReleaseRequestBody {
    param (
        [string]
        # 
        $TagName,

        [string]
        # Either the SHA of the commit to target or the branch name.
        $TargetCommitish,

        [string]
        # Title of the release
        $ReleaseTitle,

        [string]
        # Description of the release
        $Description,

        [bool]
        # Is this a draft?
        $IsDraft,

        [bool]
        # Is this a pre-release?
        $IsPrerelease
    )

    $body_params = [ordered]@{}
    if ($TagName -ne "")         { $body_params.Add("tag_name", $TagName) }
    if ($TargetCommitish -ne "") { $body_params.Add("target_commitish", $TargetCommitish) }
    if ($ReleaseTitle -ne "")    { $body_params.Add("name", $ReleaseTitle) }
    if ($Description -ne "")     { $body_params.Add("body", $Description) }
    if ($PSBoundParameters.ContainsKey("IsDraft"))         { $body_params.Add("draft", $IsDraft) }
    if ($PSBoundParameters.ContainsKey("IsPrerelease"))    { $body_params.Add("prerelease", $IsPrerelease) }

    $json_body = ConvertTo-Json -InputObject $body_params -Compress
    Write-Output $json_body
}