[CmdletBinding()]
param(
    [Parameter(Mandatory)][string]$InstallerPath,
    [Parameter(Mandatory)][string]$ApplicationDirectory,
    [Parameter(Mandatory)][string]$OutputDirectory
)

$ErrorActionPreference = 'Stop'
$installer = (Resolve-Path -LiteralPath $InstallerPath).Path
$application = (Resolve-Path -LiteralPath $ApplicationDirectory).Path
if (Test-Path -LiteralPath $OutputDirectory) { throw 'Use a new output directory to avoid stale extracted files.' }
New-Item -ItemType Directory -Path $OutputDirectory | Out-Null
$output = (Resolve-Path -LiteralPath $OutputDirectory).Path
$packages = (& dotnet nuget locals global-packages --list) -replace '^global-packages:\s*', ''
if ($LASTEXITCODE -ne 0) { throw 'Could not locate the NuGet package cache.' }
[xml]$project = Get-Content -Raw -LiteralPath (Join-Path $PSScriptRoot '../mRemoteNGInstaller/Installer/Installer.wixproj')
$sdkVersion = $project.Project.Sdk.Split('/')[1]
$wix = Join-Path $packages "wixtoolset.sdk/$sdkVersion/tools/net8.0/wix.dll"
$source = Join-Path $output 'installer.wxs'
$extracted = Join-Path $output 'extracted'
& dotnet $wix msi decompile -acceptEula wix7 $installer -o $source -x $extracted
if ($LASTEXITCODE -ne 0) { throw 'MSI decompilation failed.' }
[xml]$document = Get-Content -Raw -LiteralPath $source
$namespaces = [Xml.XmlNamespaceManager]::new($document.NameTable)
$namespaces.AddNamespace('w', 'http://wixtoolset.org/schemas/v4/wxs')
$installed = [Collections.Generic.Dictionary[string,string]]::new([StringComparer]::OrdinalIgnoreCase)
foreach ($file in $document.SelectNodes('//w:Directory[@Id="INSTALLDIR"]//w:File', $namespaces)) {
    $parts = [Collections.Generic.List[string]]::new()
    $parts.Add($file.GetAttribute('Name'))
    $parent = $file.ParentNode
    while ($parent -and $parent.GetAttribute('Id') -ne 'INSTALLDIR') {
        if ($parent.LocalName -eq 'Directory') { $parts.Insert(0, $parent.GetAttribute('Name')) }
        $parent = $parent.ParentNode
    }
    $relative = $parts -join [IO.Path]::DirectorySeparatorChar
    $installed.Add($relative, (Join-Path $extracted "File/$($file.GetAttribute('Id'))"))
}
$files = @(Get-ChildItem -LiteralPath $application -File -Recurse | Where-Object Extension -ne '.pdb')
if (-not (Test-Path -LiteralPath (Join-Path $application 'mRemoteNG.exe')) -or $files.Count -lt 2) {
    throw 'Expected a complete built application directory.'
}
foreach ($file in $files) {
    $relative = [IO.Path]::GetRelativePath($application, $file.FullName)
    if (-not $installed.ContainsKey($relative)) { throw "MSI is missing $relative" }
    $expected = (Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256).Hash
    $actual = (Get-FileHash -LiteralPath $installed[$relative] -Algorithm SHA256).Hash
    if ($actual -ne $expected) { throw "MSI payload differs from the build: $relative" }
}
$version = [version]$document.Wix.Package.Version
$appVersion = [version][Diagnostics.FileVersionInfo]::GetVersionInfo((Join-Path $application 'mRemoteNG.dll')).FileVersion
if ($version.Major -ne $appVersion.Major -or $version.Minor -ne $appVersion.Minor -or $version.Build -ne $appVersion.Build) {
    throw "Installer version $version does not match application version $appVersion."
}
[ordered]@{
    ProductVersion = $version.ToString()
    ApplicationVersion = $appVersion.ToString()
    PayloadFilesVerified = $files.Count
    TotalInstalledFiles = $installed.Count
    SHA256 = (Get-FileHash -LiteralPath $installer -Algorithm SHA256).Hash
} | ConvertTo-Json | Tee-Object -FilePath (Join-Path $output 'validation.json')
