param (
    [string]
    [Parameter(Mandatory=$true)]
    $TargetDir
)

Write-Output "===== Beginning $($PSCmdlet.MyInvocation.MyCommand) ====="

$path_HelpFilesDir = Join-Path -Path $TargetDir -ChildPath "Help"

Write-Output "Moving Help files to correct directory"

# Remove stale Help files, if they exist
if (Test-Path -Path $path_HelpFilesDir) {
    Remove-Item -Path $path_HelpFilesDir -Recurse -Force
}

# Move Help files
Move-Item -Path (Join-Path -Path $TargetDir -ChildPath "Resources\Help") -Destination $path_HelpFilesDir -Force
Start-Sleep -Seconds 2
Remove-Item -Path (Join-Path -Path $TargetDir -ChildPath "Resources") -Recurse -Force

Write-Output ""