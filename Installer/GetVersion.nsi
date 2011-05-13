!define AppFile "$EXEDIR\..\mRemoteV1\bin\Release\mRemoteNG.exe"
!define VersionCmdFile "$EXEDIR\Version.cmd"
!define VersionNshFile "$EXEDIR\Version.nsh"

OutFile "..\Release\GetVersion.exe"
SilentInstall silent
RequestExecutionLevel user

Section
	## Get file version
	GetDllVersion "${AppFile}" $R0 $R1
	IntOp $R3 $R0 / 0x00010000
	IntOp $R4 $R0 & 0x0000FFFF
	IntOp $R5 $R1 / 0x00010000
	IntOp $R6 $R1 & 0x0000FFFF
	StrCpy $R1 "$R3.$R4.$R5.$R6"
	StrCpy $R2 "$R3.$R4"

	FileOpen $R0 "${VersionCmdFile}" w
	FileWrite $R0 '@echo off$\r$\n'
	FileWrite $R0 'SET PRODUCT_VERSION_SHORT=$R2$\r$\n'
	FileClose $R0

	FileOpen $R0 "${VersionNshFile}" w
	FileWrite $R0 '!define PRODUCT_VERSION "$R1"$\r$\n'
	FileWrite $R0 '!define PRODUCT_VERSION_SHORT "$R2"$\r$\n'
	FileWrite $R0 '!define PRODUCT_VERSION_MAJOR "$R3"$\r$\n'
	FileWrite $R0 '!define PRODUCT_VERSION_MINOR "$R4"$\r$\n'
	FileClose $R0
SectionEnd
