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

    $debugFile = Join-Path -Path $TargetDir -ChildPath "mRemoteNG.pdb"

    Write-Output "Creating debug symbols ZIP file $($outputZipPath) from $($debugFile)"

    # AppVeyor build
    if(!([string]::IsNullOrEmpty($Env:APPVEYOR_BUILD_FOLDER))) {
        $outputZipPath = Join-Path -Path $SolutionDir -ChildPath "Release\$zipFilePrefix-$($version).zip"
        7z a $outputZipPath $debugFile
    }
    # Local build
    else {
        $outputZipPath = "$($SolutionDir)Release\$zipFilePrefix-$($version).zip"
        Compress-Archive $debugFile $outputZipPath -Force
    }
}

Write-Output ""

# Package portable release zip file
if ($ConfigurationName -eq "Release Portable") {
    Write-Output "Packaging Release Portable ZIP"

    # Exclude debug symbols from folder
    $FileExclude = @("*.pdb")
    $Source = Get-ChildItem -Recurse -Path $TargetDir -Exclude $FileExclude

    Write-Output "Creating portable ZIP file $($outputZipPath) from $($Source)"

    # AppVeyor build
    if(!([string]::IsNullOrEmpty($Env:APPVEYOR_BUILD_FOLDER))) {
        $outputZipPath = Join-Path -Path $SolutionDir -ChildPath "Release\mRemoteNG-Portable-$($version).zip"
        7z a -spf $outputZipPath $Source
    }
    # Local build
    else {
        $outputZipPath="$($SolutionDir)\Release\mRemoteNG-Portable-$($version).zip"
        Compress-Archive $Source $outputZipPath -Force
    }
}

Write-Output ""