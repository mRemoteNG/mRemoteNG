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


function New-MsiUpdateFileContent {
    param (
        [System.IO.FileInfo]
        [Parameter(Mandatory=$true)]
        $MsiFile,

        [string]
        [Parameter(Mandatory=$true)]
        $TagName
    )
    
    $version = $MsiFile.BaseName -replace "[a-zA-Z-]*"
    $certThumbprint = (Get-AuthenticodeSignature -FilePath $MsiFile).SignerCertificate.Thumbprint
    $hash = Get-FileHash -Algorithm SHA512 $MsiFile | ForEach-Object { $_.Hash }

    $fileContents = `
"Version: $version
dURL: https://github.com/mRemoteNG/mRemoteNG/releases/download/$TagName/$($MsiFile.Name)
clURL: https://raw.githubusercontent.com/mRemoteNG/mRemoteNG/$TagName/CHANGELOG.md
CertificateThumbprint: $certThumbprint
Checksum: $hash"
    Write-Output $fileContents
}


function New-ZipUpdateFileContent {
    param (
        [System.IO.FileInfo]
        [Parameter(Mandatory=$true)]
        $ZipFile,

        [string]
        [Parameter(Mandatory=$true)]
        $TagName
    )
    
    $version = $ZipFile.BaseName -replace "[a-zA-Z-]*"
    $hash = Get-FileHash -Algorithm SHA512 $ZipFile | ForEach-Object { $_.Hash }

    $fileContents = `
"Version: $version
dURL: https://github.com/mRemoteNG/mRemoteNG/releases/download/$TagName/$($ZipFile.Name)
clURL: https://raw.githubusercontent.com/mRemoteNG/mRemoteNG/$TagName/CHANGELOG.TXT
Checksum: $hash"
    Write-Output $fileContents
}


function Resolve-UpdateCheckFileName {
    param (
        [string]
        [Parameter(Mandatory=$true)]
        [ValidateSet("Stable","Preview","Nightly")]
        $UpdateChannel,

        [string]
        [Parameter(Mandatory=$true)]
        [ValidateSet("Normal","Portable")]
        $Type
    )

    $fileName = ""

    if ($UpdateChannel -eq "Preview") { $fileName += "preview-" }
    elseif ($UpdateChannel -eq "Nightly") { $fileName += "nightly-" }

    $fileName += "update"

    if ($Type -eq "Portable") { $fileName += "-portable" }

    $fileName += ".txt"

    Write-Output $fileName
}


Write-Output "Begin create_upg_chk_files.ps1"

# determine update channel
if ($env:APPVEYOR_PROJECT_NAME -match "(Nightly)") {
    write-host "UpdateChannel = Nightly"
    $UpdateChannel = "Nightly"
    $ModifiedTagName = "$PreTagName-$TagName-NB"
} elseif ($env:APPVEYOR_PROJECT_NAME -match "(Preview)") {
    write-host "UpdateChannel = Preview"
    $UpdateChannel = "Preview"
    $ModifiedTagName = "v$TagName-PB"
} elseif ($env:APPVEYOR_PROJECT_NAME -match "(Stable)") {
    write-host "UpdateChannel = Stable"
    $UpdateChannel = "Stable"
    $ModifiedTagName = "v" + $TagName.Split("-")[0]
} else {
    $UpdateChannel = ""
}

$buildFolder = Join-Path -Path $PSScriptRoot -ChildPath "..\mRemoteNG\bin\x64\Release" -Resolve -ErrorAction Ignore

if ($UpdateChannel -ne "" -and $buildFolder -ne "") {
    $releaseFolder = Join-Path -Path $PSScriptRoot -ChildPath "..\Release" -Resolve
    $msiFile = Get-ChildItem -Path "$buildFolder\*.msi" | Sort-Object LastWriteTime | Select-Object -last 1
    if (![string]::IsNullOrEmpty($msiFile)) {
        $msiUpdateContents = New-MsiUpdateFileContent -MsiFile $msiFile -TagName $ModifiedTagName
        $msiUpdateFileName = Resolve-UpdateCheckFileName -UpdateChannel $UpdateChannel -Type Normal
        Write-Output "`n`nMSI Update Check File Contents ($msiUpdateFileName)`n------------------------------"
        Tee-Object -InputObject $msiUpdateContents -FilePath "$releaseFolder\$msiUpdateFileName"
        write-host "msiUpdateFileName $releaseFolder\$msiUpdateFileName"
        
        # commit msi update txt file
        if ($env:WEBSITE_UPDATE_ENABLED.ToLower() -eq "true") {
            if ((Test-Path -Path "$releaseFolder\$msiUpdateFileName") -and (-not [string]::IsNullOrEmpty($WebsiteTargetRepository))) {
                Write-Output "Publish $msiUpdateFileName to $WebsiteTargetRepository"
                $update_file_content_string = Get-Content "$releaseFolder\$msiUpdateFileName" | Out-String
                Set-GitHubContent -OwnerName $WebsiteTargetOwner -RepositoryName $WebsiteTargetRepository -Path $msiUpdateFileName -CommitMessage "Build $ModifiedTagName" -Content $update_file_content_string -BranchName main
            }
        }
    }

    # build zip update file
    $releaseFolder = Join-Path -Path $PSScriptRoot -ChildPath "..\Release" -Resolve
    $zipFile = Get-ChildItem -Path "$releaseFolder\*.zip" -Exclude "*-symbols-*.zip" | Sort-Object LastWriteTime | Select-Object -last 1
    if (![string]::IsNullOrEmpty($zipFile)) {
        $zipUpdateContents = New-ZipUpdateFileContent -ZipFile $zipFile -TagName $ModifiedTagName
        $zipUpdateFileName = Resolve-UpdateCheckFileName -UpdateChannel $UpdateChannel -Type Portable
        Write-Output "`n`nZip Update Check File Contents ($zipUpdateFileName)`n------------------------------"
        Tee-Object -InputObject $zipUpdateContents -FilePath "$releaseFolder\$zipUpdateFileName"
        write-host "zipUpdateFileName $releaseFolder\$zipUpdateFileName"
        
        # commit zip update txt file
        if ($env:WEBSITE_UPDATE_ENABLED.ToLower() -eq "true") {
            if ((Test-Path -Path "$releaseFolder\$zipUpdateFileName") -and (-not [string]::IsNullOrEmpty($WebsiteTargetRepository))) {
                Write-Output "Publish $zipUpdateFileName to $WebsiteTargetRepository"
                $update_file_content_string = Get-Content "$releaseFolder\$zipUpdateFileName" | Out-String
                Set-GitHubContent -OwnerName $WebsiteTargetOwner -RepositoryName $WebsiteTargetRepository -Path $zipUpdateFileName -CommitMessage "Build $ModifiedTagName" -Content $update_file_content_string -BranchName main
            }
        }
    }
} else {
    write-host "BuildFolder not found"
}

Write-Output "End create_upg_chk_files.ps1"
