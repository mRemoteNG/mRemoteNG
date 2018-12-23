param (
    [string]
    [Parameter(Mandatory=$true)]
    $SolutionDir,

    [string]
    [Parameter(Mandatory=$true)]
    $TargetDir
)

Write-Output "===== Beginning $($PSCmdlet.MyInvocation.MyCommand) ====="
Write-Output "Copying TILES folder to output"

$sourceFiles = [io.path]::combine($SolutionDir , 'mRemoteV1\Resources\Tiles' ) 
$DestinationDir = $TargetDir 

robocopy $sourceFiles $DestinationDir *.*

Write-Output ""