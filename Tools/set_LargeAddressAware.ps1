[CmdletBinding()]

param (
    [string]
    [Parameter(Mandatory=$true)]
    $TargetDir,

    [string]
    [Parameter(Mandatory=$true)]
    $TargetFileName
)

Write-Output "===== Beginning $($PSCmdlet.MyInvocation.MyCommand) ====="

$path_editBin = Join-Path -Path $PSScriptRoot -ChildPath "exes\editbin.exe"
$path_outputExe = Join-Path -Path $TargetDir -ChildPath $TargetFileName

# Set LargeAddressAware
Write-Output "Setting LargeAddressAware on binary file:`n`"$path_outputExe`" `nwith:`n`"$path_editBin`""
& "$path_editBin" /largeaddressaware "$path_outputExe"

Write-Output ""