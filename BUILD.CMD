@echo off

SET VCVARSALL="%ProgramFiles(x86)%\Microsoft Visual Studio 9.0\VC\vcvarsall.bat"
SET DEVENV="devenv.exe"
SET MAKENSIS="%ProgramFiles(x86)%\NSIS\makensis.exe"
SET RAR="%ProgramFiles%\WinRAR\WinRAR.exe"
SET BINARYZIP="%~dp0\Release\mRemoteNG-1.66.zip"
SET PORTABLEZIP="%~dp0\Release\mRemoteNG-Portable-1.66.zip"

call %VCVARSALL% x86 > NUL 2>&1

rmdir /s /q "%~dp0\mRemoteV1\bin" > NUL 2>&1
rmdir /s /q "%~dp0\mRemoteV1\obj" > NUL 2>&1

echo Building release version...
%DEVENV% "%~dp0\mRemoteV1.sln" /build "Release"

echo Building portable version...
%DEVENV% "%~dp0\mRemoteV1.sln" /build "Release Portable"

mkdir "%~dp0\Release"

echo Creating installer package...
%MAKENSIS% "%~dp0\Installer\mRemote.nsi"

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
