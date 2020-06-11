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
$path_SphinxSourceDir = Join-Path -Path $SolutionDir -ChildPath "mRemoteNG\Documentation"

# Remove stale Help files, if they exist
if (Test-Path -Path $path_HelpFilesDir) {
    Remove-Item -Path $path_HelpFilesDir -Recurse -Force
}

# Build docs
sphinx-build $path_SphinxSourceDir $path_HelpFilesDir

# Place dummy html file if build failed
if (-Not (Test-Path $path_HelpFilesDir\index.html -PathType Leaf)) {
    New-Item -Path $path_HelpFilesDir -ItemType "directory"
    New-Item $path_HelpFilesDir\index.html
    Set-Content $path_HelpFilesDir\index.html 'Welcome to mRemoteNG!'
} 

Write-Output ""