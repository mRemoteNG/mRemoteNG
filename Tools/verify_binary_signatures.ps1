param (
    [string]
    [Parameter(Mandatory=$true)]
    $TargetDir,

    [string]
    [Parameter(Mandatory=$true)]
    $ConfigurationName,
    
    [string]
    [Parameter(Mandatory=$true)]
    # The code signing certificate to use when signing the files.
    $CertificatePath
)

Write-Output "===== Beginning $($PSCmdlet.MyInvocation.MyCommand) ====="

#  validate release versions and if the certificate is available
if ($ConfigurationName -match "Release") {
	
	if ($CertificatePath -eq "" -or !(Test-Path -Path $CertificatePath -PathType Leaf))
	{
	    Write-Output "Certificate is not present - files likely not signed - we won't verify file signatures."
	    return
	}
		
    Write-Output "Verifying signature of binaries"
    Write-Output "Getting files from path: $TargetDir"
    $signableFiles = Get-ChildItem -Path $TargetDir -Recurse | ?{$_.Extension -match "dll|exe|msi"}
    Write-Output "Signable files count: $($signableFiles.Count)"
    $badSignatureFound = $false
    foreach ($file in $signableFiles) {
        $signature = Get-AuthenticodeSignature -FilePath $file.FullName
        if ($signature.Status -ne "Valid") {
            Write-Warning "File $($file.FullName) does not have a valid signature."
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