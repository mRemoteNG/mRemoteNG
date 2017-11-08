#Requires -Version 4.0
param (
    [string]
    [Parameter(Mandatory=$true)]
    $TagName,

    [string]
    [Parameter(Mandatory=$true)]
    [ValidateSet("Stable","Beta","Development")]
    $UpdateChannel
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
    $hash = Get-FileHash -Algorithm SHA512 $MsiFile | % { $_.Hash }

    $fileContents = `
"Version: $version
dURL: https://github.com/mRemoteNG/mRemoteNG/releases/download/$TagName/$($MsiFile.Name)
clURL: https://raw.githubusercontent.com/mRemoteNG/mRemoteNG/$TagName/CHANGELOG.TXT
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
    $hash = Get-FileHash -Algorithm SHA512 $ZipFile | % { $_.Hash }

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
        [ValidateSet("Stable","Beta","Development")]
        $UpdateChannel,

        [string]
        [Parameter(Mandatory=$true)]
        [ValidateSet("Normal","Portable")]
        $Type
    )

    $fileName = ""

    if ($UpdateChannel -eq "Beta") { $fileName += "beta-" }
    elseif ($UpdateChannel -eq "Development") { $fileName += "dev-" }

    $fileName += "update"

    if ($Type -eq "Portable") { $fileName += "-portable" }

    $fileName += ".txt"

    Write-Output $fileName
}





$releaseFolder = Join-Path -Path $PSScriptRoot -ChildPath "..\Release" -Resolve

# build msi update file
$msiFile = Get-ChildItem -Path "$releaseFolder\*.msi" | sort LastWriteTime | select -last 1
$msiUpdateContents = New-MsiUpdateFileContent -MsiFile $msiFile -TagName $TagName
$msiUpdateFileName = Resolve-UpdateCheckFileName -UpdateChannel $UpdateChannel -Type Normal
Write-Output "`n`nMSI Update Check File Contents ($msiUpdateFileName)`n------------------------------"
Tee-Object -InputObject $msiUpdateContents -FilePath "$releaseFolder\$msiUpdateFileName"


# build zip update file
$zipFile = Get-ChildItem -Path "$releaseFolder\*.zip" | sort LastWriteTime | select -last 1
$zipUpdateContents = New-ZipUpdateFileContent -ZipFile $zipFile -TagName $TagName
$zipUpdateFileName = Resolve-UpdateCheckFileName -UpdateChannel $UpdateChannel -Type Portable
Write-Output "`n`nZip Update Check File Contents ($zipUpdateFileName)`n------------------------------"
Tee-Object -InputObject $zipUpdateContents -FilePath "$releaseFolder\$zipUpdateFileName"