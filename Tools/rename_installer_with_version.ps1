$solutionDir = $args[0]
$renameTarget = $solutionDir + "InstallerProjects\Installer\bin\Release\en-US\mRemoteNG-Installer.msi"

Write-Host $solutionDir
Write-Host $renameTarget

$targetVersionedFile = "$solutionDir\mRemoteV1\bin\Release\mRemoteNG.exe"
$version = &"$solutionDir\Tools\sigcheck.exe" /accepteula -q -n $targetVersionedFile


$renameTargetFileObject = Get-Item -Path $renameTarget -ErrorAction SilentlyContinue
if ($renameTargetFileObject)
{
    # Build the new file name
    $oldFileName = $renameTargetFileObject.Name
    $newFileName = $oldFileName -replace "$("\"+$renameTargetFileObject.Extension)",$("-"+$version+$renameTargetFileObject.Extension)
    Write-Host $oldFileName
    Write-Host $newFileName

    # Delete any items that already exist with the new name (effectively an overwrite)
    Remove-Item -Path "$($renameTargetFileObject.Directory.FullName)\$newFileName" -ErrorAction SilentlyContinue

    # Rename file
    Rename-Item -Path $renameTarget -NewName $newFileName -ErrorAction SilentlyContinue
}