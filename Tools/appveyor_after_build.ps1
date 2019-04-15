if([string]::IsNullOrEmpty($Env:APPVEYOR_BUILD_FOLDER)) {
    Write-Output "NOT running via Appveyor - Exiting"
    Exit
}

$appvDir = $Env:APPVEYOR_BUILD_FOLDER

Write-Output "Appveyor Build Dir: '$($appvDir)'"
$ConfigurationName = $Env:CONFIGURATION.Trim()
Write-Output "Config Name (tirmmed): '$($ConfigurationName)'"


$SIGCHECK="Tools\exes\sigcheck.exe"
$SEVENZIP="Tools\7zip\7za.exe"

if ($ConfigurationName -eq "Release Portable") {
    Write-Output "Packaging Release Portable ZIP"
   
    $version = & $SIGCHECK /accepteula -q -n "mRemoteV1\bin\$($ConfigurationName)\mRemoteNG.exe"

    Write-Output "Version is $($version)"

    $PortableZip="Release\mRemoteNG-Portable-$($version).zip"

    Remove-Item -Recurse "mRemoteV1\bin\package" -ErrorAction SilentlyContinue | Out-Null
    New-Item "mRemoteV1\bin\package" -ItemType  "directory" | Out-Null
    
    Copy-Item "mRemoteV1\Resources\PuTTYNG.exe" -Destination "mRemoteV1\bin\package"

    Copy-Item "mRemoteV1\bin\$ConfigurationName\*" -Destination "mRemoteV1\bin\package" -Recurse -Force -Exclude *.pdb
    Copy-Item "*.txt" -Destination "mRemoteV1\bin\package"

    Write-Output "Creating portable ZIP file $($PortableZip)"
    Remove-Item -Force  $PortableZip -ErrorAction SilentlyContinue
    & $SEVENZIP a -bt -bd -bb1 -mx=9 -tzip -y -r $PortableZip ".\mRemoteV1\bin\package\*.*"
}
else {
    Write-Output "We will not zip anything - this isnt a portable release build."
}

Write-Output ""
Write-Output ""

if ($ConfigurationName -match "Release" -And $ConfigurationName -ne "Release Installer") {
    Write-Output "Packaging debug symbols"
   
    $version = & $SIGCHECK /accepteula -q -n "mRemoteV1\bin\$($ConfigurationName)\mRemoteNG.exe"

    Write-Output "Version is $($version)"

    if ($ConfigurationName -match "Portable") {
        $zipFilePrefix = "mRemoteNG-Portable-symbols"
    } else {
        $zipFilePrefix = "mRemoteNG-symbols"
    }

    $outputZipPath="Release\$zipFilePrefix-$($version).zip"

    Write-Output "Creating debug symbols ZIP file $($outputZipPath)"
    Remove-Item -Force  $outputZipPath -ErrorAction SilentlyContinue
    $SymPath = (Join-Path -Path mRemoteV1\bin\$($ConfigurationName) -ChildPath "*.pdb")
    if(Test-Path "$SymPath") {
    	& $SEVENZIP a -bt -bd -bb1 -mx=9 -tzip -y -r $outputZipPath "$SymPath"
    } else {
    	Write-Output "No Debugging Symbols Found..."
    }
    	
}
else {
    Write-Output "We will not package debug symbols for this configuration $($ConfigurationName)"
}

Write-Output ""