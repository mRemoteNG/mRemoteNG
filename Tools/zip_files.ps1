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

Write-Output "===== Beginning $($PSCmdlet.MyInvocation.MyCommand) ====="

$ConfigurationName = $ConfigurationName.Trim()
Write-Output "Config Name (trimmed): '$($ConfigurationName)'"
$exe = Join-Path -Path $TargetDir -ChildPath $TargetFileName
$Version = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($exe).FileVersion
Write-Output "Version is $($version)"

# Fix for AppVeyor
if(!([string]::IsNullOrEmpty($Env:APPVEYOR_BUILD_FOLDER))) {
    if(!(test-path "Release")) {
        New-Item -ItemType Directory -Force -Path "Release" | Out-Null
    }
}

# Package debug symbols zip file
if ($ConfigurationName -match "Release") {
    Write-Output "Packaging debug symbols"

    if ($ConfigurationName -match "Portable") {
        $zipFilePrefix = "mRemoteNG-Portable-symbols"
    } else {
        $zipFilePrefix = "mRemoteNG-symbols"
    }

    $outputZipPath = "$($SolutionDir)Release\$zipFilePrefix-$($version).zip"
    $debugFile = Join-Path -Path $TargetDir -ChildPath "mRemoteNG.pdb"

    # Fix for AppVeyor
    if(!([string]::IsNullOrEmpty($Env:APPVEYOR_BUILD_FOLDER))) {
        $outputZipPath = "Release\$zipFilePrefix-$($version).zip"
    }

    Write-Output "Creating debug symbols ZIP file $($outputZipPath) from $($debugFile)"
    Compress-Archive $debugFile $outputZipPath -Force
}

Write-Output ""

# Package portable release zip file
if ($ConfigurationName -eq "Release Portable") {
    Write-Output "Packaging Release Portable ZIP"

    $outputZipPath="$($SolutionDir)\Release\mRemoteNG-Portable-$($version).zip"

    # Fix for AppVeyor
    if(!([string]::IsNullOrEmpty($Env:APPVEYOR_BUILD_FOLDER))) {
        $outputZipPath = "Release\mRemoteNG-Portable-$($version).zip"
    }

    # Exclude debug symbols from folder
    $FileExclude = @("*.pdb")
    $Source = Get-ChildItem -Recurse -Path $TargetDir -Exclude $FileExclude

    Write-Output "Creating portable ZIP file $($outputZipPath) from $($Source)"
    Compress-Archive $Source $outputZipPath -Force
}

Write-Output ""