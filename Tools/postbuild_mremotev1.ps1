param (
    [string]
    [Parameter(Mandatory=$true)]
    $SolutionDir,

    [string]
    [Parameter(Mandatory=$true)]
    $TargetDir,

    [string]
    [Parameter(Mandatory=$true)]
    $TargetFileName,

    [string]
    [Parameter(Mandatory=$true)]
    $ConfigurationName
)

$certificatePath = "C:\mRemoteNG_code_signing_cert.pfx"
$path_signFilesScript = Join-Path -Path $SolutionDir -ChildPath "Tools\signfiles.ps1"
$path_packageZipScript = Join-Path -Path $SolutionDir -ChildPath "Tools\build-relport.cmd"
$path_HelpFilesDir = Join-Path -Path $TargetDir -ChildPath "Help"
$path_outputExe = Join-Path -Path $TargetDir -ChildPath $TargetFileName
$path_editBin = @((Resolve-Path -Path "C:\Program Files*\Microsoft Visual Studio*\VC\bin\editbin.exe").Path)[0]



# Copy PuTTYNG
Write-Host "Copy PUTTYNG to correct directory" -ForegroundColor Green
Copy-Item -Path (Join-Path -Path $SolutionDir -ChildPath "mRemoteV1\Resources\PuTTYNG.exe") -Destination $TargetDir -Force



# Move Help files
Write-Host "Move Help files to correct directory" -ForegroundColor Green
if (Test-Path -Path $path_HelpFilesDir) {
    Remove-Item -Path $path_HelpFilesDir -Recurse -Force
}
Move-Item -Path (Join-Path -Path $TargetDir -ChildPath "Resources\Help") -Destination $path_HelpFilesDir -Force
Start-Sleep -Seconds 2
Remove-Item -Path (Join-Path -Path $TargetDir -ChildPath "Resources") -Recurse -Force



# Set LargeAddressAware
Write-Host "Set LargeAddressAware on binary" -ForegroundColor Green
& $path_editBin "/largeaddressaware $path_outputExe"



# Remove unnecessary files from Release versions
if (($ConfigurationName -match "Release") -and (Test-Path -Path $certificatePath -PathType Leaf)) {
    Write-Host "Removing unnecessary files from Release versions" -ForegroundColor Green
    Remove-Item -Path (Join-Path -Path $TargetDir -ChildPath "app.publish") -Recurse -Force
    Remove-Item -Path $TargetDir -Include @(
        "*.pdb",
        "*.publish",
        "*.xml",
        "*.backup",
        "*.log",
        "*vshost*",
        "*.tmp"
    )
}



# Package Zip
if ($ConfigurationName -match "Release" -and $ConfigurationName -match "Portable") {
    Write-Host "Package ZIP Release Portable" -ForegroundColor Green
    & $path_packageZipScript
}