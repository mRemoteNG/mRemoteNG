param (
	[string]
    [Parameter(Mandatory=$true)]
    $SolutionDir
)

Write-Output "===== Beginning $($PSCmdlet.MyInvocation.MyCommand) ====="

$path_SphinxSourceDir = Join-Path -Path $SolutionDir -ChildPath "mremoteV1\Documentation\"
$path_SphinxBuildDir = Join-Path -Path $SolutionDir -ChildPath "mremoteV1\Documentation\_build\"
#$buildScript = "$($path_SphinxDocsDir)\make.bat"

Write-Output "Building HTML-Documentation with Sphinx"

sphinx-build $path_SphinxSourceDir $path_SphinxBuildDir
#Start-Process $buildScript -NoNewWindow -Wait

Write-Output ""


#sphinx-build C:\Source\mRemoteNG\mRemoteV1\Documentation\ C:\Source\mRemoteNG\mRemoteV1\Documentation\_build\