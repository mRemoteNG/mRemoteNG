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
Write-Output "===== Begin $($PSCmdlet.MyInvocation.MyCommand) ====="

$IsAppVeyor = !([string]::IsNullOrEmpty($Env:APPVEYOR_BUILD_FOLDER))

$ConfigurationName = $ConfigurationName.Trim()
Write-Output "Config Name (trimmed): '$($ConfigurationName)'"
$exe = Join-Path -Path $TargetDir -ChildPath $TargetFileName
$version = ((Get-Item -Path $exe).VersionInfo | Select-Object -Property ProductVersion)."ProductVersion"
Write-Output "Version is $($version)"

# determine update channel
if ($env:APPVEYOR_PROJECT_NAME -match "(Nightly)") {
    Write-Output "UpdateChannel = Nightly"
    $ModifiedVersion = "$version-NB"
} elseif ($env:APPVEYOR_PROJECT_NAME -match "(Preview)") {
    Write-Output "UpdateChannel = Preview"
    $ModifiedVersion = "$version-PB"
} elseif ($env:APPVEYOR_PROJECT_NAME -match "(Stable)") {
    Write-Output "UpdateChannel = Stable"
    $ModifiedVersion = $version
} else {
}

# Fix for AppVeyor
if($IsAppVeyor) {
    if(!(Test-Path "Release")) {
        New-Item -ItemType Directory -Force -Path "Release" | Out-Null
    }
}

# Package debug symbols zip file
if ($ConfigurationName -match "Release") {

    Write-Output "Packaging debug symbols"

    if ($ConfigurationName -match "Portable") {
        $zipFilePrefix = "mRemoteNG-Portable-symbols"
    } else {
        $zipFilePrefix = "mRemoteNG-Installer-symbols"
    }

    $debugFile = Join-Path -Path $TargetDir -ChildPath "mRemoteNG.pdb"

    # AppVeyor build
    if ($IsAppVeyor) {
        $outputZipPath = Join-Path -Path $SolutionDir -ChildPath "Release\$zipFilePrefix-$($ModifiedVersion).zip"
        7z a $outputZipPath $debugFile
    }
    # Local build
    else {
		if (!(Test-Path -Path $debugFile -PathType Leaf))
		{
			$outputZipPath = "$($SolutionDir)Release\$zipFilePrefix-$($ModifiedVersion).zip"
			Compress-Archive $debugFile $outputZipPath -Force
		} else {
			Write-Output "File do not exist:" $debugFile", nothing to compress"
		}
    }

    Remove-Item $debugFile
}

# Package portable release zip file
if ($ConfigurationName -eq "Release Portable") {
    Write-Output "Packaging portable ZIP file"

    # AppVeyor build
    if ($IsAppVeyor) {
        $outputZipPath = Join-Path -Path $SolutionDir -ChildPath "Release\mRemoteNG-Portable-$($ModifiedVersion).zip"
        7z a -bt -bd -bb1 -mx=9 -tzip -y -r $outputZipPath $TargetDir\*
    }
    # Local build
    else {
		if ($Source)
		{
			$outputZipPath="$($SolutionDir)\Release\mRemoteNG-Portable-$($ModifiedVersion).zip"
			Compress-Archive $Source $outputZipPath -Force
		} else {
			Write-Output "File do not exist:" $Source", nothing to compress"
		}
    }
}

Write-Output "End $($PSCmdlet.MyInvocation.MyCommand)"
Write-Output ""
