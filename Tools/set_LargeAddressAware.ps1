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
#Resolve-Path tends to be faster, but since editbin's are all over the place it's not 100% effective
$editBinPath = @((Resolve-Path -Path "C:\Program Files*\Microsoft Visual Studio*\VC\bin\editbin.exe").Path)

if(!$editBinPath)
{
	# This should work on all VS versions, but doesn't on our Jenkin's build for some reason...
	# This is needed VC Community
	$editBinPath =  @((gci -Path "C:\Program*\Microsoft Visual Studio\" -Filter editbin.exe -Recurse)[0].FullName)
}

# if we STILL can't find it, just return. Same end result NUnit test will fail.
if(!$editBinPath)
{
	echo "Could not find editbin.exe - Can't set LargeAddressAware"
	return
}

echo "editBinPath value:"
echo $editBinPath
# Verify editbin certificate
& "$PSScriptRoot\validate_microsoft_tool.ps1" -FullPath "$editBinPath"


$outputExe = Join-Path -Path $TargetDir -ChildPath $TargetFileName

# Set LargeAddressAware
Write-Output "Setting LargeAddressAware on binary file `"$outputExe`""
& $editBinPath "/largeaddressaware" "$outputExe"

Write-Output ""