param (
    [string]
    $SourcePath,

    [string]
    $DestinationDir
)

Write-Host $SourcePath
Write-Host $DestinationDir

if (!(Test-Path -Path $DestinationDir))
{
    New-Item -Path $DestinationDir -ItemType "directory"
}

$sourceFiles = Get-ChildItem -Path $SourcePath -Recurse | ?{$_.Extension -match "exe|msi"}
foreach ($item in $sourceFiles)
{
    $item.Name
    (Get-FileHash -Path $item.fullName -Algorithm SHA512).Hash
    Copy-Item -Path $item.FullName -Destination $DestinationDir -Force
}