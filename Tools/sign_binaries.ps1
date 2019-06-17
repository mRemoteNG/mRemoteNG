param (
    [string]
    [Parameter(Mandatory=$true)]
    $TargetDir,

    [string]
    [Parameter(Mandatory=$true)]
    $ConfigurationName,

    [string[]]
    # File names to exclude from signing
    $Exclude,

    [string]
    [AllowEmptyString()]
    # The code signing certificate to use when signing the files.
    $CertificatePath,

    [string]
    # Password to unlock the code signing certificate.
    $CertificatePassword,
    
    [string]
    $SolutionDir
)

Write-Output "===== Beginning $($PSCmdlet.MyInvocation.MyCommand) ====="


$timeserver = "http://timestamp.verisign.com/scripts/timstamp.dll"


#  validate release versions and if the certificate value was passed
if ($ConfigurationName -match "Release" -And ($CertificatePath)) {

	if(-Not ([string]::IsNullOrEmpty($Env:APPVEYOR_BUILD_FOLDER)) ) {
		$CertificatePath = Join-Path -Path $SolutionDir -ChildPath $CertificatePath
	}

	# make sure the cert is actually available
	if ($CertificatePath -eq "" -or !(Test-Path -Path $CertificatePath -PathType Leaf))
	{
	    Write-Output "Certificate is not present - we won't sign files."
	    return
	}

	if ($CertificatePassword -eq "") {
	    Write-Output "No certificate password was provided - we won't sign files."
	    return
	}

	try {
	    $certKeyStore = [System.Security.Cryptography.X509Certificates.X509KeyStorageFlags]::MachineKeySet
	    $cert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2($CertificatePath, $CertificatePassword, $certKeyStore) -ErrorAction Stop
	} catch {
	    Write-Output "Error loading certificate file - we won't sign files."
	    Write-Output $Error[0]
	    return
	}

	# Sign MSI if we are building a release version and the certificate is available
	Write-Output "Signing Binaries"
	Write-Output "Getting files from path: $TargetDir"
	$signableFiles = Get-ChildItem -Path $TargetDir -Recurse | ?{$_.Extension -match "dll|exe|msi"} | ?{$Exclude -notcontains $_.Name}

	$excluded_files = Get-ChildItem -Path $TargetDir -Recurse | ?{$_.Extension -match "dll|exe|msi"} | ?{$Exclude -contains $_.Name}
	$excluded_files | ForEach-Object `
	    -Begin { Write-Output "The following files were excluded from signing due to being on the exclusion list:" } `
	    -Process { Write-Output "-- $($_.FullName)" }

	Write-Output "Signable files count: $($signableFiles.Count)"


	foreach ($file in $signableFiles) {
	    Set-AuthenticodeSignature -Certificate $cert -TimestampServer $timeserver -IncludeChain all -FilePath $file.FullName
	}


	# Release certificate
	if ($cert -ne $null) {
	    $cert.Dispose()
	}
} else {
    Write-Output "This is not a release build or CertificatePath wasn't provided - we won't sign files."
    Write-Output "Config: $($ConfigurationName)`tCertPath: $($CertificatePath)"
}

Write-Output ""