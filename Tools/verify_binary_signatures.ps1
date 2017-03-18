param (
    [string]
    [Parameter(Mandatory=$true)]
    $TargetDir,

    [string]
    [Parameter(Mandatory=$true)]
    $ConfigurationName
)

Write-Output "===== Beginning $($PSCmdlet.MyInvocation.MyCommand) ====="

# Sign MSI if we are building a release version and the certificate is available
if ($ConfigurationName -match "Release") {
    Write-Output "Verifying signature of binaries"
    Write-Output "Getting files from path: $TargetDir"
    $signableFiles = Get-ChildItem -Path $TargetDir -Recurse | ?{$_.Extension -match "dll|exe|msi"}
    Write-Output "Signable files count: $($signableFiles.Count)"
    $badSignatureFound = $false
    foreach ($file in $signableFiles) {
        $signature = Get-AuthenticodeSignature -FilePath $file.FullName
        if ($signature.Status -ne "Valid") {
            Write-Error "File $($file.FullName) does not have a valid signature."
            $badSignatureFound = $true
        }
    }
    if ($badSignatureFound) {
        Write-Output "One or more files were improperly signed."
    } else {
        Write-Output "All files have valid signatures."
    }
} else {
    Write-Output "This is not a release build - we won't verify file signatures."
}

Write-Output ""