@echo off
:: Builds mRemoteNG (Debug|x64) and launches it.
:: ponytail: skips mRemoteNGTests project (has a pre-existing unrelated test compile error)
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

echo Using MSBuild: %MSBUILD%
echo Building mRemoteNG ^(Debug^|x64^)...
"%MSBUILD%" "%ROOT%mRemoteNG\mRemoteNG.csproj" /t:Restore;Build /p:Configuration=Debug /p:Platform=x64 /m /nologo /v:minimal
if errorlevel 1 (
    echo [ERROR] Build failed.
    exit /b 1
)

echo Build succeeded. Launching mRemoteNG...
start "" "%ROOT%mRemoteNG\bin\x64\Debug\mRemoteNG.exe"

endlocal
