if([string]::IsNullOrEmpty($Env:APPVEYOR_BUILD_FOLDER)) {
    Write-Output "NOT running via Appveyor - Exiting"
    Throw "NOT running via Appveyor - Exiting"
}

$appvDir = $Env:APPVEYOR_BUILD_FOLDER
	
$SIGCHECK="Tools\exes\sigcheck.exe"
$SEVENZIP="Tools\7zip\7za.exe"

Write-Output "Appveyor Build Dir: '$($appvDir)'"

Write-Output "Decrypt Cert"
& appveyor-tools\secure-file -decrypt "$($Env:cert_path).enc" -secret "$Env:cert_decrypt_pwd"

if(-Not (Test-Path $Env:cert_path)) {
		Write-Output "decrypt cert does not exist..."
		Throw "Could not decrypt cert"
}

Write-Output "Restoring NuGets"
& nuget restore


Write-Output "Build Release Installer"
& msbuild "$($appvDir)\mRemoteV1.sln" /nologo /t:Clean,Build /p:Configuration="Release Installer" /p:Platform=x86 /p:CertPath="$($Env:cert_path)" /p:CertPassword="$Env:cert_pwd" /m /verbosity:normal /logger:"C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll"

Write-Output "Packaging debug symbols"
   
$version = & $SIGCHECK /accepteula -q -n "mRemoteV1\bin\Release\mRemoteNG.exe"

Write-Output "Version is $($version)"

$zipFilePrefix = "mRemoteNG-symbols"


$outputZipPath="Release\$zipFilePrefix-$($version).zip"

Write-Output "Creating debug symbols ZIP file $($outputZipPath)"
Remove-Item -Force  $outputZipPath -ErrorAction SilentlyContinue
$SymPath = (Join-Path -Path "mRemoteV1\bin\Release" -ChildPath "*.pdb")
if(Test-Path "$SymPath") {
	& $SEVENZIP a -bt -bd -bb1 -mx=9 -tzip -y -r $outputZipPath "$SymPath"
} else {
	Write-Output "No Debugging Symbols Found..."
}
    

Write-Output "Build Release Portable"
& msbuild "$($appvDir)\mRemoteV1.sln" /nologo /t:Clean,Build /p:Configuration="Release Portable" /p:Platform=x86 /p:CertPath="$($Env:cert_path)" /p:CertPassword="$Env:cert_pwd" /m /verbosity:normal /logger:"C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll"


Write-Output "Packaging Release Portable ZIP"

$version = & $SIGCHECK /accepteula -q -n "mRemoteV1\bin\Release Portable\mRemoteNG.exe"

Write-Output "Version is $($version)"

$zipFilePrefix = "mRemoteNG-Portable-symbols"
$outputZipPath="Release\$zipFilePrefix-$($version).zip"

Write-Output "Creating debug symbols ZIP file $($outputZipPath)"
Remove-Item -Force  $outputZipPath -ErrorAction SilentlyContinue
$SymPath = (Join-Path -Path "mRemoteV1\bin\Release Portable" -ChildPath "*.pdb")
if(Test-Path "$SymPath") {
	& $SEVENZIP a -bt -bd -bb1 -mx=9 -tzip -y -r $outputZipPath "$SymPath"
} else {
	Write-Output "No Debugging Symbols Found..."
}

$PortableZip="Release\mRemoteNG-Portable-$($version).zip"

Remove-Item -Recurse "mRemoteV1\bin\package" -ErrorAction SilentlyContinue | Out-Null
New-Item "mRemoteV1\bin\package" -ItemType  "directory" | Out-Null

Copy-Item "mRemoteV1\Resources\PuTTYNG.exe" -Destination "mRemoteV1\bin\package"
Copy-Item "mRemoteV1\Resources\winbox.exe" -Destination "mRemoteV1\bin\package"

Copy-Item "mRemoteV1\bin\Release Portable\*" -Destination "mRemoteV1\bin\package" -Recurse  -Force 
Copy-Item "*.txt" -Destination "mRemoteV1\bin\package"

Write-Output "Creating portable ZIP file $($PortableZip)"
Remove-Item -Force  $PortableZip -ErrorAction SilentlyContinue
& $SEVENZIP a -bt -bd -bb1 -mx=9 -tzip -y -r $PortableZip ".\mRemoteV1\bin\package\*.*"
