@echo off

setlocal enabledelayedexpansion

rem Windows Sysinternals Sigcheck from http://technet.microsoft.com/en-us/sysinternals/bb897441
set SIGCHECK="%ProgramFiles(x86)%\Sigcheck\sigcheck.exe"
set SEVENZIP="%ProgramFiles%\7-Zip\7z.exe"

set VCVARSALL="%ProgramFiles(x86)%\Microsoft Visual Studio 14.0\VC\vcvarsall.bat"
call %VCVARSALL% x86

IF NOT "%~1"=="build" goto skipbuild
echo Building...
"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe" "%~dp0\mRemoteV1.sln" /Rebuild "Release Portable"

:skipbuild
set SIGCHECK=!SIGCHECK:"=^"!
set SIGCHECK=!SIGCHECK: =^^ !
set SIGCHECK=!SIGCHECK:(=^^(!
set SIGCHECK=!SIGCHECK:)=^^)!
for /F "usebackq delims=. tokens=1-4" %%i in (`!SIGCHECK! /accepteula -q -n "%~dp0\mRemoteV1\bin\Release Portable\mRemoteNG.exe"`) do (
   set PRODUCT_VERSION_SHORT=%%i.%%j
   set PRODUCT_VERSION=%%i.%%j.%%k.%%l
)
echo Version is %PRODUCT_VERSION%

set PORTABLEZIP="%~dp0\Release\mRemoteNG-Portable-%PRODUCT_VERSION%.zip"

rmdir /S /Q %~dp0\mRemoteV1\bin\package
mkdir %~dp0\mRemoteV1\bin\package
copy %~dp0\*.txt %~dp0\mRemoteV1\bin\package
copy "%~dp0\Installer Projects\Installer\Dependencies\PuTTYNG.exe" %~dp0\mRemoteV1\bin\package

xcopy /S /Y "%~dp0\mRemoteV1\bin\Release Portable" %~dp0\mRemoteV1\bin\package

echo Creating portable ZIP file...
echo %PORTABLEZIP% 
del /f /q %PORTABLEZIP% > nul 2>&1
%SEVENZIP% a -bt -mx=9 -mm=LZMA -tzip -y -r %PORTABLEZIP% "%~dp0\mRemoteV1\bin\package\*.*"
%SEVENZIP% a -bt -mx=9 -mm=LZMA -tzip -y %PORTABLEZIP% "%~dp0\*.TXT"