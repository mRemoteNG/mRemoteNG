if([string]::IsNullOrEmpty($Env:APPVEYOR_BUILD_FOLDER)) {
    Write-Output "NOT running via Appveyor - Exiting"
    Exit
}

$appvDir = $Env:APPVEYOR_BUILD_FOLDER

Write-Output "Appveyor Build Dir: '$($appvDir)'"
$ConfigurationName = $Env:CONFIGURATION.Trim()
Write-Output "Config Name (tirmmed): '$($ConfigurationName)'"


$SIGCHECK="$($SolutionDir)Tools\exes\sigcheck.exe"
$SEVENZIP="$($SolutionDir)Tools\7zip\7za.exe"

if ($ConfigurationName -eq "Release Portable") {
    Write-Output "Packaging Release Portable ZIP"
   
    $version = & $SIGCHECK /accepteula -q -n "$($SolutionDir)mRemoteV1\bin\$($ConfigurationName)\mRemoteNG.exe"

    Write-Output "Version is $($version)"

    $PortableZip="$($SolutionDir)Release\mRemoteNG-Portable-$($version).zip"

    Remove-Item -Recurse "$($SolutionDir)mRemoteV1\bin\package" -ErrorAction SilentlyContinue | Out-Null
    New-Item "$($SolutionDir)mRemoteV1\bin\package" -ItemType  "directory" | Out-Null
    
    Copy-Item "$($SolutionDir)mRemoteV1\Resources\PuTTYNG.exe" -Destination "$($SolutionDir)mRemoteV1\bin\package"

    Copy-Item "$($SolutionDir)mRemoteV1\bin\$ConfigurationName\*" -Destination "$($SolutionDir)mRemoteV1\bin\package" -Recurse  -Force 
    Copy-Item "$($SolutionDir)*.txt" -Destination "$($SolutionDir)mRemoteV1\bin\package"

    Write-Output "Creating portable ZIP file $($PortableZip)"
    Remove-Item -Force  $PortableZip -ErrorAction SilentlyContinue
    & $SEVENZIP a -bt -bd -bb1 -mx=9 -tzip -y -r $PortableZip "$($SolutionDir)mRemoteV1\bin\package\*.*"
}
else {
    Write-Output "We will not zip anything - this isnt a portable release build."
}