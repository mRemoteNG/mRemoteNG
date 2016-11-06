#Requires -Version 4.0

$file = gci ..\Release | sort LastWriteTime | select -last 1 | % { $_.FullName }

$version = $file.tostring().Split("-")[2].trim(".zip")
Write-Host Version: $version

Write-Host dURL:
Write-Host clURL:

$hash = Get-FileHash -Algorithm MD5 $file | % { $_.Hash }
Write-Host MD5: $hash