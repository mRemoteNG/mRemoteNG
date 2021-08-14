param (
    [string]
    [Parameter(Mandatory=$true)]
    $SolutionDir,

    [string]
    [Parameter(Mandatory=$true)]
    $TargetDir,

    [string]
    [Parameter(Mandatory=$true)]
    $ConfigurationName
)

Write-Output "===== Beginning $($PSCmdlet.MyInvocation.MyCommand) ====="

Write-Output "Solution Dir: '$($SolutionDir)'"
Write-Output "Target Dir: '$($TargetDir)'"
$ConfigurationName = $ConfigurationName.Trim()
Write-Output "Config Name (tirmmed): '$($ConfigurationName)'"


# Windows Sysinternals Sigcheck from http://technet.microsoft.com/en-us/sysinternals/bb897441
$SIGCHECK="$($SolutionDir)Tools\exes\sigcheck.exe"
$SEVENZIP="$($SolutionDir)Tools\7zip\7za.exe"

# Package Zip
if ($ConfigurationName -eq "Release Portable") {
    Write-Output "Packaging Release Portable ZIP"
   
    $version = & $SIGCHECK /accepteula -q -n "$($SolutionDir)mRemoteNG\bin\$($ConfigurationName)\mRemoteNG.exe"

    Write-Output "Version is $($version)"

    $PortableZip="$($SolutionDir)Release\mRemoteNG-Portable-$($version).zip"

    $tempFolderPath = Join-Path -Path $SolutionDir -ChildPath "mRemoteNG\bin\package"
    Remove-Item -Recurse $tempFolderPath -ErrorAction SilentlyContinue | Out-Null
    New-Item $tempFolderPath -ItemType  "directory" | Out-Null

    #Write-Output "$($SolutionDir)mRemoteNG\bin\$ConfigurationName" 
    #Write-Output "$($SolutionDir)mRemoteNG\bin\package"
    Copy-Item "$($SolutionDir)mRemoteNG\bin\$ConfigurationName\*" -Destination $tempFolderPath -Recurse  -Force
    # Delete any PDB files that accidentally get copied into the temp folder
    Get-ChildItem -Path $tempFolderPath -Filter "*.pdb" | Remove-Item
    Copy-Item "$($SolutionDir)*.txt" -Destination $tempFolderPath

    Write-Output "Creating portable ZIP file $($PortableZip)"
    Remove-Item -Force  $PortableZip -ErrorAction SilentlyContinue
    Compress-Archive $tempFolderPath\* $PortableZip
}
else {
    Write-Output "We will not zip anything - this isnt a portable release build."
}

Write-Output ""