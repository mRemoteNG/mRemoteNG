param (	
	[string]
    [Parameter(Mandatory=$true)]
    $SolutionDir,
	
	[string]
    [Parameter(Mandatory=$true)]
    $TargetDir
)

Write-Output "===== Beginning $($PSCmdlet.MyInvocation.MyCommand) ====="

Write-Output "Building HTML-Documentation with Sphinx"

$path_HelpFilesDir = Join-Path -Path $TargetDir -ChildPath "Help"
$path_SphinxSourceDir = Join-Path -Path $SolutionDir -ChildPath "mremoteV1\Documentation"
# Remove stale Help files, if they exist
if (Test-Path -Path $path_HelpFilesDir) {
    Remove-Item -Path $path_HelpFilesDir -Recurse -Force
}
sphinx-build $path_SphinxSourceDir $path_HelpFilesDir

Write-Output ""