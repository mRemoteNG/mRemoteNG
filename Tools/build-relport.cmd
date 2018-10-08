@echo off

setlocal enabledelayedexpansion

set SOLUTIONDIR=%~dp0..

rem Windows Sysinternals Sigcheck from http://technet.microsoft.com/en-us/sysinternals/bb897441
set SIGCHECK="%SOLUTIONDIR%\Tools\exes\sigcheck.exe"
set SEVENZIP="%SOLUTIONDIR%\Tools\7zip\7za.exe"

set VCVARSALL="%ProgramFiles(x86)%\Microsoft Visual Studio 14.0\VC\vcvarsall.bat"
call %VCVARSALL% x86

IF NOT "%~1"=="build" goto skipbuild
echo Building...
"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe" "%SOLUTIONDIR%\mRemoteV1.sln" /Rebuild "Release Portable"

:skipbuild
IF NOT EXIST "%SOLUTIONDIR%\mRemoteV1\bin\Release Portable\mRemoteNG.exe" echo Did you forget to build? && goto end
set SIGCHECK=!SIGCHECK:"=^"!
set SIGCHECK=!SIGCHECK: =^^ !
set SIGCHECK=!SIGCHECK:(=^^(!
set SIGCHECK=!SIGCHECK:)=^^)!
for /F "usebackq delims=. tokens=1-4" %%i in (`!SIGCHECK! /accepteula -q -n "%SOLUTIONDIR%\mRemoteV1\bin\Release Portable\mRemoteNG.exe"`) do (
   set PRODUCT_VERSION_SHORT=%%i.%%j
   set PRODUCT_VERSION=%%i.%%j.%%k.%%l
)
echo Version is %PRODUCT_VERSION%

set PORTABLEZIP="%SOLUTIONDIR%\Release\mRemoteNG-Portable-%PRODUCT_VERSION%.zip"

rmdir /S /Q "%SOLUTIONDIR%\mRemoteV1\bin\package"
mkdir "%SOLUTIONDIR%\mRemoteV1\bin\package"
copy "%SOLUTIONDIR%\*.txt" "%SOLUTIONDIR%\mRemoteV1\bin\package"
copy "%SOLUTIONDIR%\Installer Projects\Installer\Dependencies\PuTTYNG.exe" "%SOLUTIONDIR%\mRemoteV1\bin\package"

xcopy /S /Y "%SOLUTIONDIR%\mRemoteV1\bin\Release Portable" "%SOLUTIONDIR%\mRemoteV1\bin\package"

echo Creating portable ZIP file...
echo %PORTABLEZIP% 
del /f /q %PORTABLEZIP% > nul 2>&1
%SEVENZIP% a -bt -mx=9 -tzip -y -r %PORTABLEZIP% "%SOLUTIONDIR%\mRemoteV1\bin\package\*.*"
%SEVENZIP% a -bt -mx=9 -tzip -y %PORTABLEZIP% "%SOLUTIONDIR%\*.TXT"

:end