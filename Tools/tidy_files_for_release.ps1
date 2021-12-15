param (
    [string]
    [Parameter(Mandatory=$true)]
    $TargetDir,

    [string]
    [Parameter(Mandatory=$true)]
    $ConfigurationName
)
Write-Output ""
Write-Output "===== Beginning $($PSCmdlet.MyInvocation.MyCommand) ====="

# Remove unnecessary files from Release versions
if ($ConfigurationName -match "Release") 
{
    $test = Join-Path -Path $TargetDir -ChildPath "app.publish"
    if (Test-Path  $test -PathType Container) 
	{
        Remove-Item -Path (Join-Path -Path $TargetDir -ChildPath "app.publish") -Recurse -Force
    }

    $filesToDelete = Get-ChildItem -Path $TargetDir -Recurse -Include @(
        "*.publish",
        "*.xml",
        "*.backup",
        "*.log",
        "*vshost*",
        "*.tmp"
    ) -Exclude @(
        "mRemoteNG.VisualElementsManifest.xml"
	)
	if ($filesToDelete) 
	{ 
		Write-Output "Unnecessary files are detected and will be removed" 
		Remove-Item -Path $filesToDelete.FullName
		Write-Output $filesToDelete.FullName
	} else 
	{
		Write-Output "  No unnecessary files are detected"
	}
}
else 
{
    Write-Output "We will not remove anything - this is not a release build."
}

Write-Output ""