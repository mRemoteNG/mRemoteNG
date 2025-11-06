# Run SSH tests
$env:PATH = 'C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\Extensions\TestPlatform' + ';' + $env:PATH
$testDll = 'mRemoteNGTests\bin\x64\Debug\net9.0-windows10.0.26100.0\mRemoteNGTests.dll'

Write-Host "Running SSH Authentication Provider Tests..."
& vstest.console.exe $testDll /TestCaseFilter:'SSHAuthenticationProviderTests' /Logger:'console'

Write-Host "`nRunning SSH Connection Manager Tests..."
& vstest.console.exe $testDll /TestCaseFilter:'SSHConnectionManagerTests' /Logger:'console'

Write-Host "`nRunning SSH Protocol Tests..."
& vstest.console.exe $testDll /TestCaseFilter:'ProtocolSSH_DotNetTests' /Logger:'console'
