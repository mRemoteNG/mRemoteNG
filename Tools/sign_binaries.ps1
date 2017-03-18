param (
    [string]
    [Parameter(Mandatory=$true)]
    $SolutionDir,

    [string]
    [Parameter(Mandatory=$true)]
    $TargetDir,

    [string]
    [Parameter(Mandatory=$true)]
    $ConfigurationName,

    [string]
    # The code signing certificate to use when signing the files.
    $CertificatePath,

    [string]
    # Password to unlock the code signing certificate.
    $CertificatePassword
)

Write-Output "===== Beginning $($PSCmdlet.MyInvocation.MyCommand) ====="


$timeserver = "http://timestamp.verisign.com/scripts/timstamp.dll"


if ($ConfigurationName -notmatch "Release") {
    Write-Output "This is not a release build - we won't sign files."
    return
}

if ($CertificatePath -eq "" -or !(Test-Path -Path $CertificatePath -PathType Leaf)) {
    Write-Output "Certificate is not present - we won't sign files."
    return
}

if ($CertificatePassword -eq "") {
    Write-Output "No certificate password was provided - we won't sign files."
    return
}

try {
    $cert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2($CertificatePath, (ConvertTo-SecureString -String $CertificatePassword -AsPlainText -Force)) -ErrorAction Stop
} catch {
    Write-Output "Certificate password was not correct - we won't sign files."
    return
}

# Sign MSI if we are building a release version and the certificate is available
Write-Output "Signing Binaries"
Write-Output "Getting files from path: $TargetDir"
$signableFiles = Get-ChildItem -Path $TargetDir -Recurse | ?{$_.Extension -match "dll|exe|msi"} | ?{$Exclude -notcontains $_.Name}
Write-Output "Signable files count: $($signableFiles.Count)"

foreach ($file in $signableFiles) {
    Set-AuthenticodeSignature -Certificate $cert -TimestampServer $timeserver -IncludeChain all -FilePath $file.FullName
}


Write-Output ""