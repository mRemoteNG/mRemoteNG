#Requires -Version 4.0

Write-Output ""
Write-Output "===== Begin $($PSCmdlet.MyInvocation.MyCommand) ====="

$MainRepository = $Env:APPVEYOR_REPO_NAME.Split("/\")[1]
$IsAppVeyor = !([string]::IsNullOrEmpty($Env:APPVEYOR_BUILD_FOLDER))


if ($IsAppVeyor) {
        
    # determine update channel
    if ($env:APPVEYOR_PROJECT_NAME -match "(Nightly)") {
        Write-Output "UpdateChannel = Nightly"
        $UpdateChannel = "Nightly"
        $ModifiedTagName = "$PreTagName-$TagName-NB"
    } elseif ($env:APPVEYOR_PROJECT_NAME -match "(Preview)") {
        Write-Output "UpdateChannel = Preview"
        $UpdateChannel = "Preview"
        $ModifiedTagName = "v$TagName-PB"
    } elseif ($env:APPVEYOR_PROJECT_NAME -match "(Stable)") {
        Write-Output "UpdateChannel = Stable"
        $UpdateChannel = "Stable"
        $ModifiedTagName = "v" + $TagName.Split("-")[0]
    } else {
        $UpdateChannel = ""
    }

    if ($UpdateChannel -ne "" -and $MainRepository -ne "" ) {

        # commit AssemblyInfo.cs change
        Write-Output "publish AssemblyInfo.cs"

        $buildFolder = Join-Path -Path $PSScriptRoot -ChildPath "..\" -Resolve -ErrorAction Ignore

        if (Test-Path -Path "$buildFolder\mRemoteNG\Properties\AssemblyInfo.cs") {

            $assemblyinfocs_content = [System.String]::Join("`r`n", (Get-Content "$buildFolder\mRemoteNG\Properties\AssemblyInfo.cs"))

            Set-GitHubContent -OwnerName $MainRepository -RepositoryName $MainRepository -Path "mRemoteNG\Properties\AssemblyInfo.cs" -CommitMessage "AssemblyInfo.cs updated for  $UpdateChannel $ModifiedTagName" -Content $assemblyinfocs_content -BranchName main

            Write-Output "publish completed"

        }
    } else {
        Write-Output "Source folder not found"
    }
}


Write-Output "End $($PSCmdlet.MyInvocation.MyCommand)"
Write-Output ""
