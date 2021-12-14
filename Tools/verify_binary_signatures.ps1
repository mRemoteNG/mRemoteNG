param (
    [string]
    [Parameter(Mandatory=$true)]
    $TargetDir,

    [string]
    [Parameter(Mandatory=$true)]
    $ConfigurationName,
    
    [string]
    [Parameter(Mandatory=$true)]
    [AllowEmptyString()]
    # The code signing certificate to use when signing the files.
    $CertificatePath,
    
    [string]
    [Parameter(Mandatory=$true)]
    $SolutionDir
)

Write-Output "===== Beginning $($PSCmdlet.MyInvocation.MyCommand) ====="


#  validate release versions and if the certificate value was passed
if ($ConfigurationName -match "Release" -And ($CertificatePath)) {
	
	if(-Not ([string]::IsNullOrEmpty($Env:APPVEYOR_BUILD_FOLDER)) ) {
		$CertificatePath = Join-Path -Path $SolutionDir -ChildPath $CertificatePath
	}
	
	# make sure the cert is actually available
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
    Write-Output "This is not a release build or CertificatePath wasn't provided - we won't verify file signatures."
    Write-Output "Config: $($ConfigurationName)`tCertPath: $($CertificatePath)"
}

Write-Output ""