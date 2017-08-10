param (
    [string]
    [Parameter(Mandatory=$true)]
    #
    $Owner,

    [string]
    [Parameter(Mandatory=$true)]
    #
    $Repository,

    [string]
    [Parameter(Mandatory=$true)]
    #
    $ReleaseId,

    [string]
    [Parameter(Mandatory=$true)]
    # The OAuth2 token to use for authentication.
    $AuthToken
)

. "$PSScriptRoot\github_functions.ps1"

Edit-GitHubRelease -Owner $Owner -Repository $Repository -ReleaseId $ReleaseId -AuthToken $AuthToken -IsDraft $false