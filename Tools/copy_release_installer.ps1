$sourcePath = $args[0]
$destinationDir = $args[1]

Write-Host $sourcePath
Write-Host $destinationDir

if (!(Test-Path -Path $destinationDir))
{
    New-Item -Path $destinationDir -ItemType "directory"
}

$sourceFiles = Get-ChildItem -Path $sourcePath -Recurse | ?{$_.Extension -match "exe|msi"}
foreach ($item in $sourceFiles)
{
    Copy-Item -Path $item.FullName -Destination $destinationDir -Force
}