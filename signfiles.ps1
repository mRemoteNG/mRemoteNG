$timeserver = "http://timestamp.verisign.com/scripts/timstamp.dll"
$certPath = "C:\mRemoteNG_code_signing_cert.pfx"
$certPassword = (Get-Credential -Message "Enter the password for the certificate" -UserName "USERNAME NOT NEEDED").Password
$cert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2($certPath, $certPassword)
$targetPath = $args[0]


Write-Output "Getting files from path: $targetPath"
$signableFiles = Get-ChildItem -Path $targetPath -Recurse | ?{$_.Extension -match "dll|exe|msi"}
Write-Output "Signable files count: $($signableFiles.Count)"
foreach ($file in $signableFiles) {
    Set-AuthenticodeSignature -Certificate $cert -TimestampServer $timeserver -IncludeChain all -FilePath $file.FullName
}