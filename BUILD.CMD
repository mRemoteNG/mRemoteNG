@echo off

setlocal enabledelayedexpansion

set VERSIONTAG=

set VCVARSALL="%ProgramFiles(x86)%\Microsoft Visual Studio 14.0\VC\vcvarsall.bat"
set DEVENV="devenv.exe"
set MAKENSIS="%ProgramFiles(x86)%\NSIS\makensis.exe"
set SEVENZIP="%ProgramFiles%\7-Zip\7z.exe"
rem set SIGNCMD=signtool.exe sign /n "Astrospark Technologies, LLC" /sha1 "1cbd910dbd6e77f26506e7f600736972f700673f" /tr http://timestamp.comodoca.com/rfc3161

rem Windows Sysinternals Sigcheck from http://technet.microsoft.com/en-us/sysinternals/bb897441
set SIGCHECK="%ProgramFiles(x86)%\Sigcheck\sigcheck.exe"

call %VCVARSALL% x86

rmdir /s /q "%~dp0\mRemoteV1\bin" > nul 2>&1
rmdir /s /q "%~dp0\mRemoteV1\obj" > nul 2>&1

if exist "%~dp0\mRemoteV1\bin" goto ERROR_RMDIR
if exist "%~dp0\mRemoteV1\obj" goto ERROR_RMDIR
goto NOERROR_RMDIR

:ERROR_RMDIR
echo.
echo Could not clean output directories.
echo.
echo Build process failed.
echo.
goto END

:NOERROR_RMDIR

echo Building release version...
%DEVENV% "%~dp0\mRemoteV1.sln" /build "Release"

echo Building portable version...
%DEVENV% "%~dp0\mRemoteV1.sln" /build "Release Portable"

echo Setting LARGEADDRESSAWARE
editbin /LARGEADDRESSAWARE "%~dp0\mRemoteV1\bin\Release\mRemote3G.exe"

rem echo Signing binaries...
rem %SIGNCMD% ^
rem	"%~dp0\mRemoteV1\bin\Release\de\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\el\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\en-US\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\es\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\es-AR\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\fr\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\hu\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\it\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\nb-NO\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\nl\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\pl\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\pt\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\pt-BR\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\ru\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\uk\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\zh-CN\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\zh-TW\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\AxInterop.MSTSCLib.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\AxInterop.WFICALib.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\Interop.EOLWTSCOM.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\Interop.MSTSCLib.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\Interop.ShDocVw.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\Interop.WFICALib.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release\mRemote3G.exe" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\de\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\el\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\en-US\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\es\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\es-AR\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\fr\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\hu\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\it\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\nb-NO\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\nl\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\pl\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\pt\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\pt-BR\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\ru\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\uk\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\zh-CN\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\zh-TW\mRemote3G.resources.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\AxInterop.MSTSCLib.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\AxInterop.WFICALib.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\Interop.EOLWTSCOM.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\Interop.MSTSCLib.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\Interop.ShDocVw.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\Interop.WFICALib.dll" ^
rem	"%~dp0\mRemoteV1\bin\Release Portable\mRemote3G.exe" ^


rem Do not remove the two blank lines above this line.

mkdir "%~dp0\Release" > nul 2>&1

echo Getting product version...
set VERSIONNSH="%~dp0\Release\Version.nsh"
set SIGCHECK=!SIGCHECK:"=^"!
set SIGCHECK=!SIGCHECK: =^^ !
set SIGCHECK=!SIGCHECK:(=^^(!
set SIGCHECK=!SIGCHECK:)=^^)!
for /F "usebackq delims=. tokens=1-4" %%i in (`!SIGCHECK! /accepteula -q -n "%~dp0\mRemoteV1\bin\Release\mRemote3G.exe"`) do (
   set PRODUCT_VERSION_SHORT=%%i.%%j
   set PRODUCT_VERSION=%%i.%%j.%%k.%%l
   echo ^^!define PRODUCT_VERSION "%%i.%%j.%%k.%%l" > %VERSIONNSH%
   echo ^^!define PRODUCT_VERSION_SHORT "%%i.%%j.%%k.%%l" >> %VERSIONNSH%
   echo ^^!define PRODUCT_VERSION_MAJOR "%%i" >> %VERSIONNSH%
   echo ^^!define PRODUCT_VERSION_MINOR "%%j" >> %VERSIONNSH%
)
echo Version is %PRODUCT_VERSION%

echo Creating installer package...
if defined VERSIONTAG (
	%MAKENSIS% /DPRODUCT_VERSION_TAG=%VERSIONTAG% "%~dp0\Installer\mRemote.nsi"

	set INSTALLEREXE="%~dp0\Release\mRemote3G-Installer-%PRODUCT_VERSION%-%VERSIONTAG%.exe"
	set BINARYZIP="%~dp0\Release\mRemote3G-%PRODUCT_VERSION%-%VERSIONTAG%.zip"
	set PORTABLEZIP="%~dp0\Release\mRemote3G-Portable-%PRODUCT_VERSION%-%VERSIONTAG%.zip"
) else (
	%MAKENSIS% "%~dp0\Installer\mRemote.nsi"

	set INSTALLEREXE="%~dp0\Release\mRemote3G-Installer-%PRODUCT_VERSION%.exe"
	set BINARYZIP="%~dp0\Release\mRemote3G-%PRODUCT_VERSION%.zip"
	set PORTABLEZIP="%~dp0\Release\mRemote3G-Portable-%PRODUCT_VERSION%.zip"
)

del %VERSIONNSH%

rem echo Signing installer package...
rem %SIGNCMD% %INSTALLEREXE%

echo Creating release ZIP file...
echo %BINARYZIP%
del /f /q %BINARYZIP% > nul 2>&1
%SEVENZIP% a -r -tzip -y %BINARYZIP% "%~dp0\mRemoteV1\bin\Release\*.*"
%SEVENZIP% a -r -tzip -y %BINARYZIP% "%~dp0\Installer\Dependencies\*.*"
%SEVENZIP% a -r -tzip -y %BINARYZIP% "%~dp0\*.TXT"

echo Creating portable ZIP file...
echo %PORTABLEZIP% 
del /f /q %PORTABLEZIP% > nul 2>&1
%SEVENZIP% a -r -tzip -y %PORTABLEZIP% "%~dp0\mRemoteV1\bin\Release Portable\*.*"
%SEVENZIP% a -r -tzip -y %PORTABLEZIP% "%~dp0\Installer\Dependencies\*.*"
%SEVENZIP% a -r -tzip -y %PORTABLEZIP% "%~dp0\*.TXT"

echo.
echo Build process complete.
echo.

:END
pause
