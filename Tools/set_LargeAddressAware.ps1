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
$path_editBin =  @((gci -Path "C:\Program*\Microsoft Visual Studio\" -Filter editbin.exe -Recurse).FullName)[0]

# Verify editbin certificate
& "$PSScriptRoot\validate_microsoft_tool.ps1" -Path $path_editBin


$path_outputExe = Join-Path -Path $TargetDir -ChildPath $TargetFileName

# Set LargeAddressAware
Write-Output "Setting LargeAddressAware on binary file `"$path_outputExe`""
& $path_editBin "/largeaddressaware" "$path_outputExe"

Write-Output ""