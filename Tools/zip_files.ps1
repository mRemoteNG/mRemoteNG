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

Write-Output ""
Write-Output "===== Beginning $($PSCmdlet.MyInvocation.MyCommand) ====="

$ConfigurationName = $ConfigurationName.Trim()
Write-Output "Config Name (trimmed): '$($ConfigurationName)'"
$exe = Join-Path -Path $TargetDir -ChildPath $TargetFileName
$version = ((Get-Item -Path $exe).VersionInfo | Select-Object -Property ProductVersion)."ProductVersion"
Write-Output "Version is $($version)"

# Fix for AppVeyor
if(!([string]::IsNullOrEmpty($Env:APPVEYOR_BUILD_FOLDER))) {
    if(!(test-path "Release")) {
        New-Item -ItemType Directory -Force -Path "Release" | Out-Null
    }
}

# Package debug symbols zip file
if ($ConfigurationName -match "Release") {
    Write-Output "Packaging debug symbols"

    if ($ConfigurationName -match "Portable") {
        $zipFilePrefix = "mRemoteNG-Portable-symbols"
    } else {
        $zipFilePrefix = "mRemoteNG-symbols"
    }

    $debugFile = Join-Path -Path $TargetDir -ChildPath "mRemoteNG.pdb"

    # AppVeyor build
    if(!([string]::IsNullOrEmpty($Env:APPVEYOR_BUILD_FOLDER))) {
        $outputZipPath = Join-Path -Path $SolutionDir -ChildPath "Release\$zipFilePrefix-$($version).zip"
        7z a $outputZipPath $debugFile
    }
    # Local build
    else {
		if (!(Test-Path -Path $debugFile -PathType Leaf))
		{
			$outputZipPath = "$($SolutionDir)Release\$zipFilePrefix-$($version).zip"
			Compress-Archive $debugFile $outputZipPath -Force
		} else {
			write-host "File do not exist:" $debugFile", nothing to compress"
		}
    }

    Remove-Item $debugFile
}

# Package portable release zip file
if ($ConfigurationName -eq "Release Portable") {
    Write-Output "Packaging portable ZIP file"

    # AppVeyor build
    if(!([string]::IsNullOrEmpty($Env:APPVEYOR_BUILD_FOLDER))) {
        $outputZipPath = Join-Path -Path $SolutionDir -ChildPath "Release\mRemoteNG-Portable-$($version).zip"
        7z a -bt -bd -bb1 -mx=9 -tzip -y -r $outputZipPath $TargetDir\*
    }
    # Local build
    else {
		if ($Source)
		{
			$outputZipPath="$($SolutionDir)\Release\mRemoteNG-Portable-$($version).zip"
			Compress-Archive $Source $outputZipPath -Force
		} else {
			write-host "File do not exist:" $Source", nothing to compress"
		}
    }
}

Write-Output ""