@echo off

SET VCVARSALL="%ProgramFiles(x86)%\Microsoft Visual Studio 9.0\VC\vcvarsall.bat"
SET DEVENV="devenv.exe"
SET MAKENSIS="%ProgramFiles(x86)%\NSIS\makensis.exe"
SET RAR="%ProgramFiles%\WinRAR\Rar.exe"
SET BINARYZIP="%~dp0\Release\mRemoteNG-1.63.zip"
SET PORTABLEZIP="%~dp0\Release\mRemoteNG-Portable-1.63.zip"

call %VCVARSALL% x86

rmdir /s /q "%~dp0\mRemoteV1\bin"
rmdir /s /q "%~dp0\mRemoteV1\obj"

%DEVENV% "%~dp0\mRemoteV1.sln" /build "Release"
%DEVENV% "%~dp0\mRemoteV1.sln" /build "Release Portable"

mkdir "%~dp0\Release"

%MAKENSIS% "%~dp0\Installer\mRemote.nsi"

del /f /q %BINARYZIP%

%RAR% a -m5 -r -ep1 %BINARYZIP% "%~dp0\mRemoteV1\bin\Release\*.*"
%RAR% a -m5 -r -ep1 %BINARYZIP% "%~dp0\Installer\Dependencies\*.*"
%RAR% a -m5 -ep %BINARYZIP% "%~dp0\*.TXT"

del /f /q %PORTABLEZIP%

%RAR% a -m5 -r -ep1 %PORTABLEZIP% "%~dp0\mRemoteV1\bin\Release Portable\*.*"
%RAR% a -m5 -r -ep1 %PORTABLEZIP% "%~dp0\Installer\Dependencies\*.*"
%RAR% a -m5 -ep %PORTABLEZIP% "%~dp0\*.TXT"
