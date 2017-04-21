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
$path_editBin = & "$PSScriptRoot\find_vstool.ps1" -FileName "editbin.exe"

$path_outputExe = Join-Path -Path $TargetDir -ChildPath $TargetFileName

# Set LargeAddressAware
Write-Output "Setting LargeAddressAware on binary file `"$path_outputExe`""
& $path_editBin "/largeaddressaware" "$path_outputExe"

Write-Output ""