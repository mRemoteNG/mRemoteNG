param (
    [string]
    # Name of the file to find
    $FileName
)



function EditBinCertificateIsValid() {
    param (
        [string]
        $Path
    )

    # Verify file certificate
    $microsoft_cert_thumbprint = "3BDA323E552DB1FDE5F4FBEE75D6D5B2B187EEDC"
    $file_signature = Get-AuthenticodeSignature -FilePath $Path
    if (($file_signature.Status -ne "Valid") -or ($file_signature.SignerCertificate.Thumbprint -ne $microsoft_cert_thumbprint)) {
        Write-Warning "Could not validate the signature of $Path"
        return $false
    } else {
        return $true
    }
}

$rootSearchPaths = @(
    [System.IO.Directory]::EnumerateFileSystemEntries("C:\Program Files", "*Visual Studio*", [System.IO.SearchOption]::TopDirectoryOnly),
    [System.IO.Directory]::EnumerateFileSystemEntries("C:\Program Files (x86)", "*Visual Studio*", [System.IO.SearchOption]::TopDirectoryOnly)
)

# Returns the first full path to the $FileName that our search can find
foreach ($searchPath in $rootSearchPaths) {
    foreach ($visualStudioFolder in $searchPath) {
        $matchingExes = [System.IO.Directory]::EnumerateFileSystemEntries($visualStudioFolder, $FileName, [System.IO.SearchOption]::AllDirectories)
        foreach ($matchingExe in $matchingExes) {
            if (EditBinCertificateIsValid -Path $matchingExe) {
                return $matchingExe
            }
        }
    }
}

Write-Error "Could not find any valid file by the name $FileName." -ErrorAction Stop