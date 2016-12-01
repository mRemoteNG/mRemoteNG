#Requires -Version 4.0

$releaseFolder = Join-Path -Path $PSScriptRoot -ChildPath "..\Release" -Resolve
$tag = Read-Host -Prompt 'Tag name'

Write-Host
Write-Host
Write-Host

Write-Host PORTABLE
Write-Host --------
$file = Get-ChildItem -Path "$releaseFolder\*.zip" | sort LastWriteTime | select -last 1 | % { $_.FullName }
$filename = $file.Split("\") | select -last 1

$version = $file.tostring().Split("-")[2].trim(".zip")
Write-Host Version: $version

Write-Host dURL: https://github.com/mRemoteNG/mRemoteNG/releases/download/$tag/$filename
Write-Host clURL: https://raw.githubusercontent.com/mRemoteNG/mRemoteNG/$tag/CHANGELOG.TXT

$hash = Get-FileHash -Algorithm MD5 $file | % { $_.Hash }
Write-Host Checksum: $hash
	

Write-Host
Write-Host
Write-Host

Write-Host MSI
Write-Host ---
$file = Get-ChildItem -Path "$releaseFolder\*.msi" | sort LastWriteTime | select -last 1 | % { $_.FullName }
$filename = $file.Split("\") | select -last 1

$version = $file.tostring().Split("-")[2].trim(".msi")
Write-Host Version: $version

Write-Host dURL: https://github.com/mRemoteNG/mRemoteNG/releases/download/$tag/$filename
Write-Host clURL: https://raw.githubusercontent.com/mRemoteNG/mRemoteNG/$tag/CHANGELOG.TXT

Write-Host CertificateThumbprint: 0CEA828E5C787EA8AA89268D83816C1EA03375BA
$hash = Get-FileHash -Algorithm MD5 $file | % { $_.Hash }
Write-Host Checksum: $hash