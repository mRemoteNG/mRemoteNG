param (
    [string]
    [Parameter(Mandatory=$true)]
    $SolutionDir,

    [string]
    [Parameter(Mandatory=$true)]
    $TargetDir,

    [string]
    [Parameter(Mandatory=$true)]
    $ConfigurationName
)

Write-Output ""
Write-Output "===== Begin $($PSCmdlet.MyInvocation.MyCommand) ====="

$IsAppVeyor = !([string]::IsNullOrEmpty($Env:APPVEYOR_BUILD_FOLDER))

$ConfigurationName = $ConfigurationName.Trim()
$exe = Join-Path -Path $TargetDir -ChildPath $TargetFileName
#$version = ((Get-Item -Path $exe).VersionInfo | Select-Object -Property ProductVersion)."ProductVersion"
$version = $(Get-Item -Path $exe).VersionInfo.FileVersion
Write-Output "FileVersion: $version"


# determine update channel
if ($env:APPVEYOR_PROJECT_NAME -match "(Nightly)") {
    Write-Output "UpdateChannel = Nightly"
    $ModifiedVersion = "$version-NB"
} elseif ($env:APPVEYOR_PROJECT_NAME -match "(Preview)") {
    Write-Output "UpdateChannel = Preview"
    $ModifiedVersion = "$version-PB"
} elseif ($env:APPVEYOR_PROJECT_NAME -match "(Stable)") {
    Write-Output "UpdateChannel = Stable"
    $ModifiedVersion = $version
} else {
}

# Fix for AppVeyor
if($IsAppVeyor) {
    if(!(Test-Path "Release")) {
        New-Item -ItemType Directory -Force -Path "Release" | Out-Null
    }
}

# Package debug symbols zip file
Write-Output "Packaging debug symbols"
$zipFilePrefix = "mRemoteNG-symbols"
$pdbFiles = Get-ChildItem -Path  $SolutionDir -Filter *.pdb -Recurse
$tempPdbPath = (New-TemporaryDirectory)[0]
foreach ($pdbFile in $pdbFiles) {
    if (($pdbFile.FullName).Contains("\$ConfigurationName\")) {
        Copy-Item $pdbFile.FullName -Destination $tempPdbPath -Force
    }
}

if ($IsAppVeyor) {
    # AppVeyor build
    $outputZipPath = Join-Path -Path $SolutionDir -ChildPath "Release\$zipFilePrefix-$($ModifiedVersion).zip"
    Write-Output "outputZipPath: $outputZipPath"
    7z a $outputZipPath "$tempPdbPath\*.pdb"
} 
# else {
#     # Local build
#     $outputZipPath = "$($SolutionDir)Release\$zipFilePrefix-$($ModifiedVersion).zip"
#     Write-Output "outputZipPath: $outputZipPath"
#     Compress-Archive -Path $tempPdbPath -DestinationPath $outputZipPath -Force
# }


# Package portable release zip file
Write-Output "Packaging portable ZIP file"
# AppVeyor build
if ($IsAppVeyor) {
    $outputZipPath = Join-Path -Path $SolutionDir -ChildPath "Release\mRemoteNG-Portable-$($ModifiedVersion).zip"
    7z a -bt -bd -bb1 -mx=9 -tzip -y -r $outputZipPath $TargetDir\*
    Write-Output "Portable ZIP: $outputZipPath"
}
# Local build
else {
    if ($Source)
    {
        $outputZipPath="$($SolutionDir)\Release\mRemoteNG-Portable-$($ModifiedVersion).zip"
        Compress-Archive $Source $outputZipPath -Force
    } else {
        Write-Output "Files do not exist:" $Source", nothing to compress"
    }
}


Write-Output "End $($PSCmdlet.MyInvocation.MyCommand)"
Write-Output ""
