!include "MUI.nsh"
!include "WordFunc.nsh"
!insertmacro VersionCompare

!DEFINE PRODUCT_VERSION_MAJOR 1
!DEFINE PRODUCT_VERSION_MINOR 61

!DEFINE PRODUCT_VERSION "${PRODUCT_VERSION_MAJOR}.${PRODUCT_VERSION_MINOR}"
!DEFINE PRODUCT_VERSION_LONG "${PRODUCT_VERSION_MAJOR}.${PRODUCT_VERSION_MINOR}.0.0"

; Global Variables
Var InstallDotNET

; Basic Config
Name "mRemoteNG ${PRODUCT_VERSION}"
OutFile "bin\mRemoteNG-Installer-${PRODUCT_VERSION}.exe"
SetCompressor /SOLID lzma
InstallDir "$PROGRAMFILES\mRemoteNG"
InstallDirRegKey HKLM "Software\mRemoteNG" "InstallPath"
RequestExecutionLevel admin

; Version Information
VIProductVersion ${PRODUCT_VERSION_LONG}
!DEFINE LANG_ENGLISH "1033-English"
VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName" "mRemoteNG"
VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductVersion" ${PRODUCT_VERSION_LONG}
VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalCopyright" "Copyright © 2007-2010 Felix Deimel, Riley McArdle"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileDescription" "mRemoteNG ${PRODUCT_VERSION} Installer"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion" ${PRODUCT_VERSION_LONG}

; Design
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP "header.bmp" ; optional
!define MUI_HEADERIMAGE_BITMAP_NOSTRETCH
!define MUI_HEADERIMAGE_UNBITMAP "header.bmp" ; optional
!define MUI_HEADERIMAGE_UNBITMAP_NOSTRETCH
!define MUI_HEADER_TRANSPARENT_TEXT
!define MUI_WELCOMEFINISHPAGE_BITMAP "welcomefinish.bmp"
!define MUI_WELCOMEFINISHPAGE_BITMAP_NOSTRETCH
!define MUI_UNWELCOMEFINISHPAGE_BITMAP "welcomefinish.bmp"
!define MUI_UNWELCOMEFINISHPAGE_BITMAP_NOSTRETCH

; Install Pages
!insertmacro MUI_PAGE_LICENSE "..\COPYING.TXT"
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!define MUI_FINISHPAGE_NOAUTOCLOSE

; Finish Page
!define MUI_FINISHPAGE_RUN_NOTCHECKED
!define MUI_FINISHPAGE_RUN_Text "Launch mRemoteNG Now"
!define MUI_FINISHPAGE_RUN "$INSTDIR\mRemoteNG.exe"
!insertmacro MUI_PAGE_FINISH

; Uninstall Pages
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

; Set Language
!insertmacro MUI_LANGUAGE "English"

Function .onInit
	; Check .NET version
	StrCpy $InstallDotNET "No"
	Call GetDotNETVersion
	Pop $0

	${If} $0 == "not found"
		StrCpy $InstallDotNET "Yes"
	${EndIf}

	StrCpy $0 $0 "" 1 # skip "v"

	${VersionCompare} $0 "2.0" $1
	${If} $1 == 2
		StrCpy $InstallDotNET "Yes"
	${EndIf}

	${If} $InstallDotNET == "Yes"
		MessageBox MB_OK|MB_ICONEXCLAMATION "mRemoteNG requires Microsoft .NET Framework 2.0."
		Quit
	${EndIf}
FunctionEnd

Section "" ; Install
	SetOutPath $INSTDIR

	; AddFiles
	File /r /x "mRemoteNG.vshost.*" "..\mRemoteV1\bin\Release\*.*"
	File /r "Dependencies\*.*"
	File "..\*.txt"

	; Uninstaller
	WriteUninstaller "$INSTDIR\Uninstall.exe"

	; Register ActiveX components
	RegDLL "$INSTDIR\eolwtscom.dll"
	RegDLL "$INSTDIR\scvncctrl.dll"
 
	; Start Menu
	CreateDirectory "$SMPROGRAMS\mRemoteNG"
	CreateShortCut "$SMPROGRAMS\mRemoteNG\mRemoteNG.lnk" "$INSTDIR\mRemoteNG.exe"
	CreateShortCut "$SMPROGRAMS\mRemoteNG\Uninstall.lnk" "$INSTDIR\Uninstall.exe"

	; Registry
	WriteRegStr HKLM "Software\mRemoteNG" "InstallPath" $INSTDIR
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemoteNG" "DisplayName" "mRemoteNG"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemoteNG" "Publisher" "mRemoteNG"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemoteNG" "DisplayIcon" "$INSTDIR\mRemoteNG.exe"
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemoteNG" "EstimatedSize" 5816

	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemoteNG" "DisplayVersion" ${PRODUCT_VERSION}
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemoteNG" "VersionMajor" ${PRODUCT_VERSION_MAJOR}
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemoteNG" "VersionMinor" ${PRODUCT_VERSION_MINOR}

	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemoteNG" "UninstallString" '"$INSTDIR\Uninstall.exe"'
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemoteNG" "NoModify" 1
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemoteNG" "NoRepair" 1
SectionEnd

Section "un.Uninstall"
	; Unregister ActiveX components
	UnregDLL "$INSTDIR\eolwtscom.dll"
	UnregDLL "$INSTDIR\scvncctrl.dll"

	; Delete Files
	RMDIR /r $INSTDIR

	; Start Menu
	Delete "$SMPROGRAMS\mRemoteNG\mRemoteNG.lnk"
	Delete "$SMPROGRAMS\mRemoteNG\Uninstall.lnk"
	RMDir "$SMPROGRAMS\mRemoteNG"

	; Registry
	DeleteRegValue HKLM "Software\mRemoteNG" "InstallPath"
	DeleteRegKey /ifempty HKLM "Software\mRemoteNG"
	DeleteRegKey /ifempty HKCU "Software\mRemoteNG"
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemoteNG"
SectionEnd

Function GetDotNETVersion
	Push $0
	Push $1

	System::Call "mscoree::GetCORVersion(w .r0, i ${NSIS_MAX_STRLEN}, *i) i .r1"
	StrCmp $1 "error" 0 +2
	StrCpy $0 "not found"

	Pop $1
	Exch $0
FunctionEnd
