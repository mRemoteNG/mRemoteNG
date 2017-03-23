param (
    [string]
    [Parameter(Mandatory=$true)]
    $SolutionDir,

    [string]
    [Parameter(Mandatory=$true)]
    $TargetDir,

    [string]
    [Parameter(Mandatory=$true)]
    $ConfigurationName
)

Write-Output "===== Beginning $($PSCmdlet.MyInvocation.MyCommand) ====="
$path_packageZipScript = Join-Path -Path $SolutionDir -ChildPath "Tools\build-relport.cmd"


# Package Zip
if ($ConfigurationName -match "Release" -and $ConfigurationName -match "Portable") {
    Write-Output "Packaging Release Portable ZIP"
    & $path_packageZipScript
}
else {
    Write-Output "We will not zip anything - this isnt a portable release build."
}

Write-Output ""