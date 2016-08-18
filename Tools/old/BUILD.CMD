@echo off

setlocal enabledelayedexpansion

set VERSIONTAG=

set VCVARSALL="%ProgramFiles(x86)%\Microsoft Visual Studio 10.0\VC\vcvarsall.bat"
set DEVENV="devenv.exe"
set MAKENSIS="%ProgramFiles(x86)%\NSIS\Unicode\makensis.exe"
set RAR="%ProgramFiles%\WinRAR\WinRAR.exe"
set SIGNCMD=signtool.exe sign /n "Astrospark Technologies, LLC" /sha1 "1cbd910dbd6e77f26506e7f600736972f700673f" /tr http://timestamp.comodoca.com/rfc3161

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

echo Signing binaries...
%SIGNCMD% ^
	"%~dp0\mRemoteV1\bin\Release\de\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release\el\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release\en-US\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release\es\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release\es-AR\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release\fr\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release\hu\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release\it\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release\nb-NO\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release\nl\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release\pl\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release\pt\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release\pt-BR\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release\ru\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release\uk\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release\zh-CN\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release\zh-TW\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release\AxInterop.MSTSCLib.dll" ^
	"%~dp0\mRemoteV1\bin\Release\AxInterop.WFICALib.dll" ^
	"%~dp0\mRemoteV1\bin\Release\Interop.EOLWTSCOM.dll" ^
	"%~dp0\mRemoteV1\bin\Release\Interop.MSTSCLib.dll" ^
	"%~dp0\mRemoteV1\bin\Release\Interop.ShDocVw.dll" ^
	"%~dp0\mRemoteV1\bin\Release\Interop.WFICALib.dll" ^
	"%~dp0\mRemoteV1\bin\Release\mRemoteNG.exe" ^
	"%~dp0\mRemoteV1\bin\Release Portable\de\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\el\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\en-US\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\es\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\es-AR\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\fr\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\hu\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\it\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\nb-NO\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\nl\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\pl\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\pt\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\pt-BR\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\ru\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\uk\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\zh-CN\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\zh-TW\mRemoteNG.resources.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\AxInterop.MSTSCLib.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\AxInterop.WFICALib.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\Interop.EOLWTSCOM.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\Interop.MSTSCLib.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\Interop.ShDocVw.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\Interop.WFICALib.dll" ^
	"%~dp0\mRemoteV1\bin\Release Portable\mRemoteNG.exe" ^


rem Do not remove the two blank lines above this line.

mkdir "%~dp0\Release" > nul 2>&1

echo Getting product version...
set VERSIONNSH="%~dp0\Release\Version.nsh"
set SIGCHECK=!SIGCHECK:"=^"!
set SIGCHECK=!SIGCHECK: =^^ !
set SIGCHECK=!SIGCHECK:(=^^(!
set SIGCHECK=!SIGCHECK:)=^^)!
for /F "usebackq delims=. tokens=1-4" %%i in (`!SIGCHECK! /accepteula -q -n "%~dp0\mRemoteV1\bin\Release\mRemoteNG.exe"`) do (
   set PRODUCT_VERSION_SHORT=%%i.%%j
   echo ^^!define PRODUCT_VERSION "%%i.%%j.%%k.%%l" > %VERSIONNSH%
   echo ^^!define PRODUCT_VERSION_SHORT "%%i.%%j" >> %VERSIONNSH%
   echo ^^!define PRODUCT_VERSION_MAJOR "%%i" >> %VERSIONNSH%
   echo ^^!define PRODUCT_VERSION_MINOR "%%j" >> %VERSIONNSH%
)
echo Version is %PRODUCT_VERSION_SHORT%

echo Creating installer package...
if defined VERSIONTAG (
	%MAKENSIS% /DPRODUCT_VERSION_TAG=%VERSIONTAG% "%~dp0\Installer\mRemote.nsi"

	set INSTALLEREXE="%~dp0\Release\mRemoteNG-Installer-%PRODUCT_VERSION_SHORT%-%VERSIONTAG%.exe"
	set BINARYZIP="%~dp0\Release\mRemoteNG-%PRODUCT_VERSION_SHORT%-%VERSIONTAG%.zip"
	set PORTABLEZIP="%~dp0\Release\mRemoteNG-Portable-%PRODUCT_VERSION_SHORT%-%VERSIONTAG%.zip"
) else (
	%MAKENSIS% "%~dp0\Installer\mRemote.nsi"

	set INSTALLEREXE="%~dp0\Release\mRemoteNG-Installer-%PRODUCT_VERSION_SHORT%.exe"
	set BINARYZIP="%~dp0\Release\mRemoteNG-%PRODUCT_VERSION_SHORT%.zip"
	set PORTABLEZIP="%~dp0\Release\mRemoteNG-Portable-%PRODUCT_VERSION_SHORT%.zip"
)

del %VERSIONNSH%

echo Signing installer package...
%SIGNCMD% %INSTALLEREXE%

echo Creating release ZIP file...
del /f /q %BINARYZIP% > nul 2>&1
%RAR% a -m5 -r -ep1 -afzip -inul %BINARYZIP% "%~dp0\mRemoteV1\bin\Release\*.*"
%RAR% a -m5 -r -ep1 -afzip -inul %BINARYZIP% "%~dp0\Installer\Dependencies\*.*"
%RAR% a -m5 -ep -afzip -inul %BINARYZIP% "%~dp0\*.TXT"

echo Creating portable ZIP file...
del /f /q %PORTABLEZIP% > nul 2>&1
%RAR% a -m5 -r -ep1 -afzip -inul %PORTABLEZIP% "%~dp0\mRemoteV1\bin\Release Portable\*.*"
%RAR% a -m5 -r -ep1 -afzip -inul %PORTABLEZIP% "%~dp0\Installer\Dependencies\*.*"
%RAR% a -m5 -ep -afzip -inul %PORTABLEZIP% "%~dp0\*.TXT"

echo.
echo Build process complete.
echo.

:END
pause
