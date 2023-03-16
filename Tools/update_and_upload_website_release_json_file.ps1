#Requires -Version 4.0
param (
    [string]
    [Parameter(Mandatory=$true)]
    $WebsiteTargetOwner,

    [string]
    [Parameter(Mandatory=$true)]
    $WebsiteTargetRepository,

    [string]
    [Parameter(Mandatory=$false)]
    $PreTagName = "",

    [string]
    [Parameter(Mandatory=$true)]
    $TagName,

    [string]
    [Parameter(Mandatory=$true)]
    $ProjectName
)


Write-Output ""
Write-Output "===== Begin $($PSCmdlet.MyInvocation.MyCommand) ====="

$MainRepository = $Env:APPVEYOR_REPO_NAME.Split("/\")[1]

# determine update channel
if ($env:APPVEYOR_PROJECT_NAME -match "(Nightly)") {
    Write-Output "UpdateChannel = Nightly"
    $UpdateChannel = "Nightly"
    $ModifiedTagName = "$PreTagName-$TagName-NB"
} elseif ($env:APPVEYOR_PROJECT_NAME -match "(Preview)") {
    Write-Output "UpdateChannel = Preview"
    $UpdateChannel = "Preview"
    $ModifiedTagName = "v$TagName-PB"
} elseif ($env:APPVEYOR_PROJECT_NAME -match "(Stable)") {
    Write-Output "UpdateChannel = Stable"
    $UpdateChannel = "Stable"
    $ModifiedTagName = "v" + $TagName.Split("-")[0]
} else {
    $UpdateChannel = ""
}

#$buildFolder = Join-Path -Path $PSScriptRoot -ChildPath "..\mRemoteNG\bin\x64\Release" -Resolve -ErrorAction Ignore
$ReleaseFolder = Join-Path -Path $PSScriptRoot -ChildPath "..\Release" -Resolve

if ($UpdateChannel -ne "" -and $ReleaseFolder -ne "" -and $MainRepository -ne "" -and $WebsiteTargetOwner -ne "" -and $WebsiteTargetRepository -ne "" ) {

    $published_at = (Get-Date).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")
    
    # get releases.json from github
    $releases_json = Get-GitHubContent -OwnerName $WebsiteTargetOwner -RepositoryName $WebsiteTargetRepository -Path _data\releases.json
    ConvertFrom-Base64($releases_json.content) | Out-File -FilePath "$ReleaseFolder\releases.json"
    $websiteJsonReleaseFile = Get-ChildItem -Path "$ReleaseFolder\releases.json"

    # installer
    $msiFile = Get-ChildItem -Path "$ReleaseFolder\*.msi" | Sort-Object LastWriteTime | Select-Object -last 1
    if (![string]::IsNullOrEmpty($msiFile)) {
        Write-Output "UpdateChannel: $UpdateChannel"
        Write-Output "msiFile: $msiFile"
        $checksum = (Get-FileHash $msiFile -Algorithm SHA512).Hash
        $file_size = (Get-ChildItem $msiFile).Length
        $a = Get-Content $websiteJsonReleaseFile | ConvertFrom-Json
        switch ($UpdateChannel) {
            "Nightly" {
                $GithubTag = "$((Get-Date).ToUniversalTime().ToString("yyyyMMdd"))-$TagName-NB"
                $html_url = "https://github.com/$WebsiteTargetOwner/$MainRepository/releases/tag/$GithubTag"
                $browser_download_url = "https://github.com/$WebsiteTargetOwner/$MainRepository/releases/download/$GithubTag/$($msiFile.Name)"
                $a.nightlybuild.name = "v$TagName-NB"
                $a.nightlybuild.published_at = $published_at
                $a.nightlybuild.html_url = $html_url
                $a.nightlybuild.assets.installer.browser_download_url = $browser_download_url
                $a.nightlybuild.assets.installer.checksum = $checksum
                $a.nightlybuild.assets.installer.size = $file_size
                break
            }
            "Preview" {
                $GithubTag = "$TagName-PB"
                $html_url = "https://github.com/$WebsiteTargetOwner/$MainRepository/releases/tag/$GithubTag"
                $browser_download_url = "https://github.com/$WebsiteTargetOwner/$MainRepository/releases/download/$GithubTag/$($msiFile.Name)"
                $a.prerelease.name = "v$TagName-PB"
                $a.prerelease.published_at = $published_at
                $a.prerelease.html_url = $html_url
                $a.prerelease.assets.installer.browser_download_url = $browser_download_url
                $a.prerelease.assets.installer.checksum = $checksum
                $a.prerelease.assets.installer.size = $file_size
                break
            }
            "Stable" {
                $GithubTag = "$TagName"
                $html_url = "https://github.com/$WebsiteTargetOwner/$MainRepository/releases/tag/$GithubTag"
                $browser_download_url = "https://github.com/$WebsiteTargetOwner/$MainRepository/releases/download/$GithubTag/$($msiFile.Name)"
                $a.stable.name = "v$TagName"
                $a.stable.published_at = $published_at
                $a.stable.html_url = $html_url
                $a.stable.assets.installer.browser_download_url = $browser_download_url
                $a.stable.assets.installer.checksum = $checksum
                $a.stable.assets.installer.size = $file_size
                break
            }
        }
    }


    # portable
    $zipFile = Get-ChildItem -Path "$ReleaseFolder\*.zip" -Exclude "*-symbols-*.zip" | Sort-Object LastWriteTime | Select-Object -last 1
    if (![string]::IsNullOrEmpty($zipFile)) {
        Write-Output "UpdateChannel: $UpdateChannel"
        Write-Output "zipFile: $zipFile"
        $checksum = (Get-FileHash $zipFile -Algorithm SHA512).Hash
        $file_size = (Get-ChildItem $zipFile).Length
        $a = Get-Content $websiteJsonReleaseFile | ConvertFrom-Json
        switch ($UpdateChannel) {
            "Nightly" {
                $GithubTag = "$((Get-Date).ToUniversalTime().ToString("yyyyMMdd"))-$TagName-NB"
                $html_url = "https://github.com/$WebsiteTargetOwner/$MainRepository/releases/tag/$GithubTag"
                $browser_download_url = "https://github.com/$WebsiteTargetOwner/$MainRepository/releases/download/$GithubTag/$($zipFile.Name)"
                $a.nightlybuild.name = "v$TagName-NB"
                $a.nightlybuild.published_at = $published_at
                $a.nightlybuild.html_url = $html_url
                $a.nightlybuild.assets.portable.browser_download_url = $browser_download_url
                $a.nightlybuild.assets.portable.checksum = $checksum
                $a.nightlybuild.assets.portable.size = $file_size
                break
            }
            "Preview" {
                $GithubTag = "$TagName-PB"
                $html_url = "https://github.com/$WebsiteTargetOwner/$MainRepository/releases/tag/$GithubTag"
                $browser_download_url = "https://github.com/$WebsiteTargetOwner/$MainRepository/releases/download/$GithubTag/$($zipFile.Name)"
                $a.prerelease.name = "v$TagName-PB"
                $a.prerelease.published_at = $published_at
                $a.prerelease.html_url = $html_url
                $a.prerelease.assets.portable.browser_download_url = $browser_download_url
                $a.prerelease.assets.portable.checksum = $checksum
                $a.prerelease.assets.portable.size = $file_size
                break
            }
            "Stable" {
                $GithubTag = "$TagName"
                $html_url = "https://github.com/$WebsiteTargetOwner/$MainRepository/releases/tag/$GithubTag"
                $browser_download_url = "https://github.com/$WebsiteTargetOwner/$MainRepository/releases/download/$GithubTag/$($zipFile.Name)"
                $a.stable.name = "v$TagName"
                $a.stable.published_at = $published_at
                $a.stable.html_url = $html_url
                $a.stable.assets.portable.browser_download_url = $browser_download_url
                $a.stable.assets.portable.checksum = $checksum
                $a.stable.assets.portable.size = $file_size
                break
            }
        }
    }

    $a | ConvertTo-Json -Depth 10 | Set-Content $websiteJsonReleaseFile

    # commit releases.json change
    if ($env:WEBSITE_UPDATE_ENABLED.ToLower() -eq "true") {
        Write-Output "publish releases.json"
        if (Test-Path -Path "$ReleaseFolder\releases.json") {
            $releases_json_string = Get-Content "$ReleaseFolder\releases.json" | Out-String
            Set-GitHubContent -OwnerName $WebsiteTargetOwner -RepositoryName $WebsiteTargetRepository -Path _data\releases.json -CommitMessage "Updated for $UpdateChannel $ModifiedTagName" -Content $releases_json_string -BranchName main
            Write-Output "publish completed"
        }
    }
} else {
    Write-Output "ReleaseFolder not found"
}


Write-Output "End $($PSCmdlet.MyInvocation.MyCommand)"
Write-Output ""
