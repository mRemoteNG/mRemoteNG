@echo off

setlocal enabledelayedexpansion

set VERSIONTAG=

rem Windows Sysinternals Sigcheck from http://technet.microsoft.com/en-us/sysinternals/bb897441
set SIGCHECK="%ProgramFiles(x86)%\Sigcheck\sigcheck.exe"

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