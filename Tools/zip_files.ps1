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

# Package debug symbols zip file
if ($ConfigurationName -match "Release") {
    Write-Output "Packaging debug symbols"

    if ($ConfigurationName -match "Portable") {
        $zipFilePrefix = "mRemoteNG-Portable-symbols"
    } else {
        $zipFilePrefix = "mRemoteNG-symbols"
    }

    $outputZipPath="$($SolutionDir)Release\$zipFilePrefix-$($version).zip"

    Write-Output "Creating debug symbols ZIP file $($outputZipPath)"
    Remove-Item -Force  $outputZipPath -ErrorAction SilentlyContinue
    Compress-Archive (Join-Path -Path $TargetDir -ChildPath "mRemoteNG.pdb") $outputZipPath -Force
}

Write-Output ""

# Package portable release zip file
if ($ConfigurationName -eq "Release Portable") {
    Write-Output "Packaging Release Portable ZIP"

    $PortableZipDst="$($SolutionDir)\Release\mRemoteNG-Portable-$($version).zip"

    # Excluse debug symbols
    $exclude = @("*.pdb")
    # get files to compress using exclusion filer
    #$files = Get-ChildItem -Recurse -Path $TargetDir -Exclude $exclude

    Write-Output "Creating portable ZIP file $($PortableZipDst)"
    Compress-Archive (Get-ChildItem -Recurse -Path $TargetDir -Exclude $exclude) $PortableZipDst -Force
}

Write-Output ""