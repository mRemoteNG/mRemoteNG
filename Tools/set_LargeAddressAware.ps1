param (
    [string]
    [Parameter(Mandatory=$true)]
    $TargetDir,

    [string]
    [Parameter(Mandatory=$true)]
    $TargetFileName
)

Write-Output "===== Beginning $($PSCmdlet.MyInvocation.MyCommand) ====="

# Find editbin.exe
$path_editBin = @((Resolve-Path -Path "C:\Program Files*\Microsoft Visual Studio*\VC\bin\editbin.exe").Path)[0]

# Verify editbin certificate
$microsoft_cert_thumbprint = "3BDA323E552DB1FDE5F4FBEE75D6D5B2B187EEDC"
$editbin_signature = Get-AuthenticodeSignature -FilePath $path_editBin
if (($editbin_signature.Status -ne "Valid") -or ($editbin_signature.SignerCertificate.Thumbprint -ne $microsoft_cert_thumbprint)) {
    Write-Error "Could not validate the signature of editbin.exe - we can not set LargeAddressAware" -ErrorAction Stop
}


$path_outputExe = Join-Path -Path $TargetDir -ChildPath $TargetFileName

# Set LargeAddressAware
Write-Output "Setting LargeAddressAware on binary file `"$path_outputExe`""
& $path_editBin "/largeaddressaware" "$path_outputExe"

Write-Output ""