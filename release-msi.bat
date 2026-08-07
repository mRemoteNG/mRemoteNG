@echo off
:: Builds mRemoteNG (Release|x64) and packages the MSI installer via the existing WiX project.
:: Requires WiX Toolset v3.x (https://wixtoolset.org, or: winget install WiXToolset.WiXToolset)
setlocal

set "ROOT=%~dp0"
set "VSWHERE=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"

if not exist "%VSWHERE%" (
    echo [ERROR] Visual Studio Installer not found. Install Visual Studio 2026 ^(or Build Tools^) with the .NET desktop workload.
    exit /b 1
)

for /f "usebackq tokens=*" %%i in (`"%VSWHERE%" -latest -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do set "MSBUILD=%%i"

if not defined MSBUILD (
    echo [ERROR] MSBuild.exe not found. Install Visual Studio 2026 ^(or Build Tools^) with the .NET desktop / MSBuild workload.
    exit /b 1
)

if not defined WIX (
    echo [ERROR] WiX Toolset v3.x is not installed ^(WIX environment variable is not set^).
    echo         Install it with: winget install WiXToolset.WiXToolset
    echo         then re-open the terminal and run this script again.
    exit /b 1
)

echo Using MSBuild: %MSBUILD%
echo Building mRemoteNG ^(Release^|x64^)...
"%MSBUILD%" "%ROOT%mRemoteNG\mRemoteNG.csproj" /t:Restore;Build /p:Configuration=Release /p:Platform=x64 /m /nologo /v:minimal
if errorlevel 1 (
    echo [ERROR] Application build failed.
    exit /b 1
)

:: If a previous MSI exists and is locked (e.g. registered by Windows Installer), delete it
:: with elevation before WiX tries to overwrite it — otherwise light.exe fails with LGHT0001.
set "MSI=%ROOT%mRemoteNGInstaller\Installer\bin\x64\Release\en-US\mRemoteNG-Installer.msi"
if exist "%MSI%" (
    del /f /q "%MSI%" >nul 2>&1
    if exist "%MSI%" (
        echo [INFO] MSI is locked; re-launching with elevation to delete it...
        powershell -NoProfile -Command "Start-Process cmd -ArgumentList '/c del /f /q \"%MSI%\"' -Verb RunAs -Wait"
        if exist "%MSI%" (
            echo [ERROR] Could not delete locked MSI. Close any program that has it open and retry.
            exit /b 1
        )
    )
)

echo Building MSI installer...
:: ponytail: WixTargetsPath is forced because VS's own MSBuildExtensionsPath32 no longer
:: points at the classic shared "%ProgramFiles(x86)%\MSBuild" folder where WiX v3 installs its targets.
"%MSBUILD%" "%ROOT%mRemoteNGInstaller\Installer\Installer.wixproj" /t:Build /p:Configuration=Release /p:Platform=x64 /p:SolutionDir=%ROOT% "/p:WixTargetsPath=%ProgramFiles(x86)%\MSBuild\Microsoft\WiX\v3.x\Wix.targets" "/p:WixCATargetsPath=%ProgramFiles(x86)%\MSBuild\Microsoft\WiX\v3.x\wix.ca.targets" /nologo /v:minimal
if errorlevel 1 (
    echo [ERROR] Installer build failed.
    exit /b 1
)

if exist "%MSI%" (
    echo Done. MSI: %MSI%
) else (
    echo [WARN] Build reported success but the MSI was not found at: %MSI%
)

endlocal
