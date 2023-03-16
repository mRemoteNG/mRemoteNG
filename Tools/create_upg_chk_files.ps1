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

Write-Output ""
Write-Output "===== Begin $($PSCmdlet.MyInvocation.MyCommand) ====="

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

if ($UpdateChannel -ne "" -and $ReleaseFolder -ne "" -and $WebsiteTargetOwner -and $WebsiteTargetRepository) {

    $msiFile = Get-ChildItem -Path "$ReleaseFolder\*.msi" -Exclude "*-symbols-*.zip" | Sort-Object LastWriteTime | Select-Object -last 1
    if (![string]::IsNullOrEmpty($msiFile)) {
        $msiUpdateContents = New-MsiUpdateFileContent -MsiFile $msiFile -TagName $ModifiedTagName
        $msiUpdateFileName = Resolve-UpdateCheckFileName -UpdateChannel $UpdateChannel -Type Normal
        Write-Output "`n`nMSI Update Check File Contents ($msiUpdateFileName)`n------------------------------"
        Tee-Object -InputObject $msiUpdateContents -FilePath "$ReleaseFolder\$msiUpdateFileName"
        
        # commit msi update txt file
        if ($env:WEBSITE_UPDATE_ENABLED.ToLower() -eq "true") {
            if ((Test-Path -Path "$ReleaseFolder\$msiUpdateFileName") -and (-not [string]::IsNullOrEmpty($WebsiteTargetRepository))) {
                Write-Output "Publish Update File $msiUpdateFileName to $WebsiteTargetRepository"
                $update_file_content_string = Get-Content "$ReleaseFolder\$msiUpdateFileName" | Out-String
                Set-GitHubContent -OwnerName $WebsiteTargetOwner -RepositoryName $WebsiteTargetRepository -Path $msiUpdateFileName -CommitMessage "Build $ModifiedTagName" -Content $update_file_content_string -BranchName main
            } else {
                Write-Warning "WARNING: Update file does not exist: $ReleaseFolder\$msiUpdateFileName"
            }
        }
    }

    # build zip update file
    $zipFile = Get-ChildItem -Path "$ReleaseFolder\*.zip" -Exclude "*-symbols-*.zip" | Sort-Object LastWriteTime | Select-Object -last 1
    if (![string]::IsNullOrEmpty($zipFile)) {
        $zipUpdateContents = New-ZipUpdateFileContent -ZipFile $zipFile -TagName $ModifiedTagName
        $zipUpdateFileName = Resolve-UpdateCheckFileName -UpdateChannel $UpdateChannel -Type Portable
        Write-Output "`n`nZip Update Check File Contents ($zipUpdateFileName)`n------------------------------"
        Tee-Object -InputObject $zipUpdateContents -FilePath "$ReleaseFolder\$zipUpdateFileName"
        
        # commit zip update txt file
        if ($env:WEBSITE_UPDATE_ENABLED.ToLower() -eq "true") {
            if ((Test-Path -Path "$ReleaseFolder\$zipUpdateFileName") -and (-not [string]::IsNullOrEmpty($WebsiteTargetRepository))) {
                Write-Output "Publish Update File $zipUpdateFileName to $WebsiteTargetRepository"
                $update_file_content_string = Get-Content "$ReleaseFolder\$zipUpdateFileName" | Out-String
                Set-GitHubContent -OwnerName $WebsiteTargetOwner -RepositoryName $WebsiteTargetRepository -Path $zipUpdateFileName -CommitMessage "Build $ModifiedTagName" -Content $update_file_content_string -BranchName main
            } else {
                Write-Warning "WARNING: Update file does not exist: $ReleaseFolder\$zipUpdateFileName"
            }
        }
    }
} else {
    Write-Output "ReleaseFolder not found"
}

Write-Output "End $($PSCmdlet.MyInvocation.MyCommand)"
Write-Output ""
