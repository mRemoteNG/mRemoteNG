param (
    [string]
    [Parameter(Mandatory=$true)]
    $TargetDir,

    [string]
    [Parameter(Mandatory=$true)]
    $ConfigurationName
)

Write-Output "===== Beginning $($PSCmdlet.MyInvocation.MyCommand) ====="

# Remove unnecessary files from Release versions
if ($ConfigurationName -match "Release") {
    Write-Output "Removing unnecessary files from Release versions"
    Remove-Item -Path (Join-Path -Path $TargetDir -ChildPath "app.publish") -Recurse -Force
    Remove-Item -Path $TargetDir -Recurse -Include @(
        "*.pdb",
        "*.publish",
        "*.xml",
        "*.backup",
        "*.log",
        "*vshost*",
        "*.tmp"
    )
}
else {
    Write-Output "We will not remove anything - this is not a release build."
}

Write-Output ""