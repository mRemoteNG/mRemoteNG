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

$certificatePath = "C:\mRemoteNG_code_signing_cert.pfx"
$path_signFilesScript = Join-Path -Path $SolutionDir -ChildPath "Tools\signfiles.ps1"


# Sign MSI if we are building a release version and the certificate is available
if ($ConfigurationName -match "Release") {
    if (Test-Path -Path $certificatePath -PathType Leaf) {
        Write-Output "Signing Binaries"
        powershell.exe -ExecutionPolicy Bypass -File $path_signFilesScript $TargetDir
    } else {
        Write-Output "Certificate is not present - we won't sign files."
    }
} else {
    Write-Output "This is not a release build - we won't sign files."
}

Write-Output ""