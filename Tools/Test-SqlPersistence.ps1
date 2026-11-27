[CmdletBinding()]
param(
    [switch]$NoBuild,
    [string]$ResultsDirectory = ('C:\data\tests\tools-mRemoteNG\sql-persistence-' + (Get-Date -Format yyyyMMdd-HHmmss))
)

$ErrorActionPreference = 'Stop'
$repository = Split-Path $PSScriptRoot -Parent
$containers = @()
$tunnel = $null
$testVariables = @('MREMOTENG_DISPOSABLE_SQL', 'MREMOTENG_TEST_MSSQL', 'MREMOTENG_TEST_MYSQL',
    'MREMOTENG_TEST_SQL_PASSWORD', 'MSSQL_SA_PASSWORD', 'MYSQL_ROOT_PASSWORD')
$previous = @{}
foreach ($name in $testVariables) { $previous[$name] = [Environment]::GetEnvironmentVariable($name, 'Process') }

try {
    New-Item -ItemType Directory -Path $ResultsDirectory -Force | Out-Null
    if (-not $NoBuild) {
        $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
        $msbuild = & $vswhere -latest -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe | Select-Object -First 1
        if (-not $msbuild) { throw 'Visual Studio MSBuild with desktop/COM and T4 tooling is required.' }
        & $msbuild "$repository\mRemoteNG.sln" -restore -p:Configuration=Release -p:Platform=x64 -verbosity:minimal *> "$ResultsDirectory\build.log"
        if ($LASTEXITCODE -ne 0) { throw "Build failed; see $ResultsDirectory\build.log" }
    }
    $env:MREMOTENG_TEST_SQL_PASSWORD = 'Synthetic!' + [Guid]::NewGuid().ToString('N') + 'a9'
    $env:MSSQL_SA_PASSWORD = $env:MREMOTENG_TEST_SQL_PASSWORD
    $env:MYSQL_ROOT_PASSWORD = $env:MREMOTENG_TEST_SQL_PASSWORD
    $suffix = [Guid]::NewGuid().ToString('N').Substring(0, 12)
    $sql = "mremoteng-test-sql-$suffix"
    $mysql = "mremoteng-test-mysql-$suffix"
    & podman create --name $sql -e ACCEPT_EULA=Y -e MSSQL_PID=Developer -e MSSQL_SA_PASSWORD -p '127.0.0.1::1433' mcr.microsoft.com/mssql/server:2019-latest | Out-Null
    if ($LASTEXITCODE -ne 0) { throw 'Could not create SQL Server test container.' }
    $containers += $sql
    & podman create --name $mysql -e MYSQL_ROOT_PASSWORD -e 'MYSQL_ROOT_HOST=%' -p '127.0.0.1::3306' docker.io/library/mysql:8.4 | Out-Null
    if ($LASTEXITCODE -ne 0) { throw 'Could not create MySQL test container.' }
    $containers += $mysql
    & podman start $sql $mysql | Out-Null
    if ($LASTEXITCODE -ne 0) { throw 'Could not start test containers.' }
    $env:MREMOTENG_TEST_MSSQL = (& podman port $sql 1433/tcp).Trim()
    if ($LASTEXITCODE -ne 0) { throw 'SQL Server port lookup failed.' }
    $env:MREMOTENG_TEST_MYSQL = (& podman port $mysql 3306/tcp).Trim()
    if ($LASTEXITCODE -ne 0) { throw 'MySQL port lookup failed.' }
    # WSL localhost forwarding can be unavailable; tunnel only these test ports.
    $machine = (& podman machine inspect | ConvertFrom-Json) | Where-Object State -eq 'running'
    if ($LASTEXITCODE -ne 0 -or @($machine).Count -ne 1) { throw 'One running local Podman machine is required.' }
    $sshArguments = @('-N', '-i', ('"' + $machine.SSHConfig.IdentityPath + '"'),
        '-p', $machine.SSHConfig.Port, '-o', 'BatchMode=yes', '-o', 'ExitOnForwardFailure=yes',
        '-o', 'StrictHostKeyChecking=accept-new', '-o', ('"UserKnownHostsFile=' + (Join-Path $ResultsDirectory 'known_hosts') + '"'))
    foreach ($name in @('MREMOTENG_TEST_MSSQL', 'MREMOTENG_TEST_MYSQL')) {
        $remote = [Environment]::GetEnvironmentVariable($name, 'Process')
        if ($remote -notmatch '^127\.0\.0\.1:([0-9]+)$') { throw 'Expected a loopback container port.' }
        $listener = [Net.Sockets.TcpListener]::new([Net.IPAddress]::Loopback, 0)
        $listener.Start()
        $localPort = $listener.LocalEndpoint.Port
        $listener.Stop()
        $sshArguments += @('-L', "127.0.0.1:${localPort}:$remote")
        [Environment]::SetEnvironmentVariable($name, "127.0.0.1:$localPort", 'Process')
    }
    $sshArguments += ($machine.SSHConfig.RemoteUsername + '@127.0.0.1')
    $tunnel = Start-Process ssh.exe -ArgumentList $sshArguments -WindowStyle Hidden -PassThru -RedirectStandardError (Join-Path $ResultsDirectory 'tunnel.log')
    Start-Sleep -Seconds 1
    if ($tunnel.HasExited) { throw "Test port tunnel failed; see $ResultsDirectory\tunnel.log" }
    $env:MREMOTENG_DISPOSABLE_SQL = '1'
    & dotnet test "$repository\mRemoteNGTests\mRemoteNGTests.csproj" -c Release -p:Platform=x64 --no-build --no-restore --filter 'FullyQualifiedName~SqlPersistenceIntegrationTests' --results-directory $ResultsDirectory --logger 'trx;LogFileName=sql-persistence.trx'
    if ($LASTEXITCODE -ne 0) { throw "SQL persistence tests failed; see $ResultsDirectory" }
}
finally {
    if ($tunnel -and -not $tunnel.HasExited) { Stop-Process -Id $tunnel.Id; $tunnel.WaitForExit() }
    foreach ($container in $containers) {
        & podman rm -f -v $container | Out-Null
        if ($LASTEXITCODE -ne 0) { Write-Warning "Could not remove test container $container" }
    }
    foreach ($name in $testVariables) { [Environment]::SetEnvironmentVariable($name, $previous[$name], 'Process') }
}
