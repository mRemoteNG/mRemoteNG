param (
    [string]
    [Parameter(Mandatory=$true)]
    $SolutionDir,

    [string]
    [Parameter(Mandatory=$true)]
    $TargetDir
)

Write-Output "===== Beginning $($PSCmdlet.MyInvocation.MyCommand) ====="
Write-Output "Copying PUTTYNG to correct directory"
Copy-Item -Path (Join-Path -Path $SolutionDir -ChildPath "mRemoteV1\Resources\PuTTYNG.exe") -Destination $TargetDir -Force

Write-Output ""