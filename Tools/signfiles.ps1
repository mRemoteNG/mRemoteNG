param(
    [string]
    [Parameter(Mandatory=$true)]
    # Folder path that contains the files you would like to sign. Recursive.
    $PathToSignableFiles,

    [string]
    # The code signing certificate to use when signing the files.
    $CertificatePath = "C:\mRemoteNG_code_signing_cert.pfx",

    [SecureString]
    # Password to unlock the code signing certificate.
    $CertificatePassword = (Get-Credential -Message "Enter password for the mRemoteNG code signing certificate" -UserName "USERNAME NOT NEEDED").Password,

    [string[]]
    # File names to exclude from signing
    $Exclude
)


$timeserver = "http://timestamp.verisign.com/scripts/timstamp.dll"
$cert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2($CertificatePath, $CertificatePassword)


Write-Output "Getting files from path: $PathToSignableFiles"
$signableFiles = Get-ChildItem -Path $PathToSignableFiles -Recurse | ?{$_.Extension -match "dll|exe|msi"} | ?{$Exclude -notcontains $_.Name}
Write-Output "Signable files count: $($signableFiles.Count)"

foreach ($file in $signableFiles) {
    Set-AuthenticodeSignature -Certificate $cert -TimestampServer $timeserver -IncludeChain all -FilePath $file.FullName
}