param (
    [string]
    [Parameter(Mandatory=$true)]
    $SolutionDir,

    [string]
    [Parameter(Mandatory=$true)]
    $TargetDir
)

Write-Output "===== Beginning $($PSCmdlet.MyInvocation.MyCommand) ====="
Write-Output "Copying THEMES folder to output"

$sourceFiles = [io.path]::combine($SolutionDir , 'mRemoteV1\Resources\Themes' ) 
$DestinationDir = [io.path]::combine($TargetDir , 'Themes') 

robocopy $sourceFiles $DestinationDir *.vstheme /s

Write-Output ""