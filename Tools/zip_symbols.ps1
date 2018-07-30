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
if ($ConfigurationName -match "Release") {
    Write-Output "Packaging debug symbols"
   
    $version = & $SIGCHECK /accepteula -q -n "$($SolutionDir)mRemoteV1\bin\$($ConfigurationName)\mRemoteNG.exe"

    Write-Output "Version is $($version)"

    $zipFilePath="$($SolutionDir)Release\mRemoteNG-symbols-$($version).zip"

    Write-Output "Creating debug symbols ZIP file $($zipFilePath)"
    Remove-Item -Force  $zipFilePath -ErrorAction SilentlyContinue
    & $SEVENZIP a -bt -bd -bb1 -mx=9 -tzip -y -r $zipFilePath "$($SolutionDir)mRemoteV1\bin\*.dbg"
}
else {
    Write-Output "We will not package debug symbols - this isnt a release build."
}

Write-Output ""