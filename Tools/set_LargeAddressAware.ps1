param (
    [string]
    [Parameter(Mandatory=$true)]
    $TargetDir,

    [string]
    [Parameter(Mandatory=$true)]
    $TargetFileName
)

Write-Output "===== Beginning $($PSCmdlet.MyInvocation.MyCommand) ====="

$path_editBin = @((Resolve-Path -Path "C:\Program Files*\Microsoft Visual Studio*\VC\bin\editbin.exe").Path)[0]
$path_outputExe = Join-Path -Path $TargetDir -ChildPath $TargetFileName


# Set LargeAddressAware
Write-Output "Setting LargeAddressAware on binary"
& $path_editBin "/largeaddressaware" "$path_outputExe"

Write-Output ""