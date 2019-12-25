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

if(-not [string]::IsNullOrEmpty($Env:APPVEYOR_BUILD_FOLDER)) {
    Write-Output "Too early to run via Appveyor - artifacts don't get generated properly. Exiting"
    Exit
}

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
   
    $version = & $SIGCHECK /accepteula -q -n "$($SolutionDir)mRemoteV1\bin\$($ConfigurationName)\mRemoteNG.exe"

    Write-Output "Version is $($version)"

    $PortableZip="$($SolutionDir)Release\mRemoteNG-Portable-$($version).zip"

    $tempFolderPath = Join-Path -Path $SolutionDir -ChildPath "mRemoteV1\bin\package"
    Remove-Item -Recurse $tempFolderPath -ErrorAction SilentlyContinue | Out-Null
    New-Item $tempFolderPath -ItemType  "directory" | Out-Null
    
    Copy-Item "$($SolutionDir)mRemoteV1\Resources\PuTTYNG.exe" -Destination $tempFolderPath

    #Write-Output "$($SolutionDir)mRemoteV1\bin\$ConfigurationName" 
    #Write-Output "$($SolutionDir)mRemoteV1\bin\package"
    Copy-Item "$($SolutionDir)mRemoteV1\bin\$ConfigurationName\*" -Destination $tempFolderPath -Recurse  -Force
    # Delete any PDB files that accidentally get copied into the temp folder
    Get-ChildItem -Path $tempFolderPath -Filter "*.pdb" | Remove-Item
    Copy-Item "$($SolutionDir)*.txt" -Destination $tempFolderPath

    Write-Output "Creating portable ZIP file $($PortableZip)"
    Remove-Item -Force  $PortableZip -ErrorAction SilentlyContinue
    & $SEVENZIP a -bt -bd -bb1 -mx=9 -tzip -y -r $PortableZip (Join-Path -Path $tempFolderPath -ChildPath "*.*")
    #& $SEVENZIP a -bt -mx=9 -tzip -y $PortableZip "$($SolutionDir)*.TXT"
    (Get-FileHash -Path $PortableZip -Algorithm SHA512).Hash

}
else {
    Write-Output "We will not zip anything - this isnt a portable release build."
}

Write-Output ""