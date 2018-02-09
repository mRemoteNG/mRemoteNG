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

$path_dumpBin = Join-Path -Path $PSScriptRoot -ChildPath "exes\dumpbin.exe"
$path_outputExe = Join-Path -Path $TargetDir -ChildPath $TargetFileName

# Dump exe header
$output = & "$path_dumpBin" /NOLOGO /HEADERS "$path_outputExe" | Select-String large

if ($output -eq $null)
{
    Write-Warning "Could not validate LargeAddressAware"
}
else
{
    Write-Output $output.ToString().TrimStart(" ")
}

Write-Output ""