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
$path_dumpBin = & "$PSScriptRoot\find_vstool.ps1" -FileName "dumpbin.exe"

$path_outputExe = Join-Path -Path $TargetDir -ChildPath $TargetFileName

# Dump exe header
$output = & "$path_dumpBin" /NOLOGO /HEADERS $path_outputExe | Select-String large

if ($output -eq $null)
{
    Write-Warning "Could not validate LargeAddressAware"
}
else
{
    Write-Output $output.ToString().TrimStart(" ")
}



Write-Output ""