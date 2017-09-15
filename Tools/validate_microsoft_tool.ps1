# $FullPath Full path to the Microsoft executable to validate
param (
    [string]
    [Parameter(Mandatory=$true)]
    $FullPath
)

$validMSCertThumbprints = @("3BDA323E552DB1FDE5F4FBEE75D6D5B2B187EEDC", "108E2BA23632620C427C570B6D9DB51AC31387FE", "98ED99A67886D020C564923B7DF25E9AC019DF26", "5EAD300DC7E4D637948ECB0ED829A072BD152E17")
$exeSignature = Get-AuthenticodeSignature -FilePath $FullPath
$baseErrorMsg = "Could not validate the certificate of $FullPath. "

if ($exeSignature.Status -ne "Valid") {
    Write-Error -Message ($baseErrorMsg+"The signature was invalid.") -ErrorAction Stop
}
elseif ($validMSCertThumbprints -notcontains $exeSignature.SignerCertificate.Thumbprint) {
    Write-Error -Message ($baseErrorMsg+"The certificate thumbprint ($($exeSignature.SignerCertificate.Thumbprint)) is not trusted.") -ErrorAction Stop
}