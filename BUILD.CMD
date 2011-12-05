@echo off

REM SET VERSIONTAG=RC5

SET VCVARSALL="%ProgramFiles(x86)%\Microsoft Visual Studio 9.0\VC\vcvarsall.bat"
SET DEVENV="devenv.exe"
SET MAKENSIS="%ProgramFiles(x86)%\NSIS\Unicode\makensis.exe"
SET RAR="%ProgramFiles%\WinRAR\WinRAR.exe"
SET SIGNCMD=signtool.exe sign /n "Next Generation Software" /sha1 "c4ece717747eb7d0cac824f6c7431e5237138b02" /t http://timestamp.verisign.com/scripts/timstamp.dll

call %VCVARSALL% x86 > NUL 2>&1

rmdir /s /q "%~dp0\mRemoteV1\bin" > NUL 2>&1
rmdir /s /q "%~dp0\mRemoteV1\obj" > NUL 2>&1

echo Building release version...
%DEVENV% "%~dp0\mRemoteV1.sln" /build "Release"

echo Building portable version...
%DEVENV% "%~dp0\mRemoteV1.sln" /build "Release Portable"

echo Signing binaries...
%SIGNCMD% "%~dp0\mRemoteV1\bin\Release\de\mRemoteNG.resources.dll" "%~dp0\mRemoteV1\bin\Release\en-US\mRemoteNG.resources.dll" "%~dp0\mRemoteV1\bin\Release\fr\mRemoteNG.resources.dll" "%~dp0\mRemoteV1\bin\Release\AxInterop.WFICALib.dll" "%~dp0\mRemoteV1\bin\Release\AxInterop.MSTSCLib.dll" "%~dp0\mRemoteV1\bin\Release\Interop.WFICALib.dll" "%~dp0\mRemoteV1\bin\Release\Interop.ShDocVw.dll" "%~dp0\mRemoteV1\bin\Release\Interop.MSTSCLib.dll" "%~dp0\mRemoteV1\bin\Release\Interop.EOLWTSCOM.dll" "%~dp0\mRemoteV1\bin\Release\Interop.EOLWTSCOM.dll" "%~dp0\mRemoteV1\bin\Release\mRemoteNG.exe" "%~dp0\mRemoteV1\bin\Release Portable\de\mRemoteNG.resources.dll" "%~dp0\mRemoteV1\bin\Release Portable\en-US\mRemoteNG.resources.dll" "%~dp0\mRemoteV1\bin\Release Portable\fr\mRemoteNG.resources.dll" "%~dp0\mRemoteV1\bin\Release Portable\AxInterop.WFICALib.dll" "%~dp0\mRemoteV1\bin\Release Portable\AxInterop.MSTSCLib.dll" "%~dp0\mRemoteV1\bin\Release Portable\Interop.WFICALib.dll" "%~dp0\mRemoteV1\bin\Release Portable\Interop.ShDocVw.dll" "%~dp0\mRemoteV1\bin\Release Portable\Interop.MSTSCLib.dll" "%~dp0\mRemoteV1\bin\Release Portable\Interop.EOLWTSCOM.dll" "%~dp0\mRemoteV1\bin\Release Portable\Interop.EOLWTSCOM.dll" "%~dp0\mRemoteV1\bin\Release Portable\mRemoteNG.exe"

mkdir "%~dp0\Release"

echo Getting product version...
%MAKENSIS% "%~dp0\Installer\GetVersion.nsi"
"%~dp0\Release\GetVersion.exe"
call "%~dp0\Release\Version.cmd"

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

echo Signing installer package...
%SIGNCMD% %INSTALLEREXE%

echo Creating release ZIP file...
del /f /q %BINARYZIP% > NUL 2>&1
%RAR% a -m5 -r -ep1 -afzip -inul %BINARYZIP% "%~dp0\mRemoteV1\bin\Release\*.*"
%RAR% a -m5 -r -ep1 -afzip -inul %BINARYZIP% "%~dp0\Installer\Dependencies\*.*"
%RAR% a -m5 -ep -afzip -inul %BINARYZIP% "%~dp0\*.TXT"

echo Creating portable ZIP file...
del /f /q %PORTABLEZIP% > NUL 2>&1
%RAR% a -m5 -r -ep1 -afzip -inul %PORTABLEZIP% "%~dp0\mRemoteV1\bin\Release Portable\*.*"
%RAR% a -m5 -r -ep1 -afzip -inul %PORTABLEZIP% "%~dp0\Installer\Dependencies\*.*"
%RAR% a -m5 -ep -afzip -inul %PORTABLEZIP% "%~dp0\*.TXT"
