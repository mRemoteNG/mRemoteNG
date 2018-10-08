param (
    [string]
    $SolutionDir
)

$renameTarget = $SolutionDir + "InstallerProjects\Installer\bin\Release\en-US\mRemoteNG-Installer.msi"

Write-Host $SolutionDir
Write-Host $renameTarget

$targetVersionedFile = "$SolutionDir\mRemoteV1\bin\Release\mRemoteNG.exe"
$version = &"$SolutionDir\Tools\exes\sigcheck.exe" /accepteula -q -n $targetVersionedFile


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