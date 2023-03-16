[CmdletBinding()]

param (
    [string]
    [Parameter(Mandatory=$true)]
    $TargetDir,

    [string]
    [Parameter(Mandatory=$true)]
    $TargetFileName
)

Write-Output ""
Write-Output "===== Begin $($PSCmdlet.MyInvocation.MyCommand) ====="

$path_dumpBin = Join-Path -Path $PSScriptRoot -ChildPath "exes\dumpbin.exe"
$path_outputExe = Join-Path -Path $TargetDir -ChildPath $TargetFileName

# Dump exe header
$output = & "$path_dumpBin" /NOLOGO /HEADERS "$path_outputExe" | Select-String large

if ($null -eq $output)
{
    Write-Warning "Could not validate LargeAddressAware"
}
else
{
    Write-Output $output.ToString().TrimStart(" ")
}

Write-Output "End $($PSCmdlet.MyInvocation.MyCommand)"
Write-Output ""
