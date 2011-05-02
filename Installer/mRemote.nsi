!include "MUI.nsh"
!include "WordFunc.nsh"
!insertmacro VersionCompare

!DEFINE PRODUCT_VERSION_MAJOR 1
!DEFINE PRODUCT_VERSION_MINOR 66

!DEFINE PRODUCT_VERSION "${PRODUCT_VERSION_MAJOR}.${PRODUCT_VERSION_MINOR}"
!DEFINE PRODUCT_VERSION_LONG "${PRODUCT_VERSION_MAJOR}.${PRODUCT_VERSION_MINOR}.0.0"

; Global Variables
Var InstallDotNET

; Basic Config
Name "mRemoteNG ${PRODUCT_VERSION}"
OutFile "..\Release\mRemoteNG-Installer-${PRODUCT_VERSION}.exe"
SetCompressor /SOLID lzma
InstallDir "$PROGRAMFILES\mRemoteNG"
InstallDirRegKey HKLM "Software\mRemoteNG" "InstallPath"
RequestExecutionLevel admin

; Version Information
VIProductVersion ${PRODUCT_VERSION_LONG}
VIAddVersionKey "ProductName" "mRemoteNG"
VIAddVersionKey "ProductVersion" ${PRODUCT_VERSION_LONG}
VIAddVersionKey "LegalCopyright" "Copyright © 2007-2009 Felix Deimel, 2010-2011 Riley McArdle"
VIAddVersionKey "FileDescription" "mRemoteNG ${PRODUCT_VERSION} Installer"
VIAddVersionKey "FileVersion" ${PRODUCT_VERSION_LONG}

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
!define MUI_FINISHPAGE_RUN "$INSTDIR\mRemoteNG.exe"
!insertmacro MUI_PAGE_FINISH

; Uninstall Pages
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

; Get Languages
!include "Language\languages.nsi"

; Set finish page text
!define MUI_FINISHPAGE_RUN_Text "$(LaunchMremoteNow)"

Function .onInit
	ClearErrors
	UserInfo::GetName
	IfErrors Win9x
	Pop $0
	UserInfo::GetAccountType
	Pop $1
	# GetOriginalAccountType will check the tokens of the original user of the
	# current thread/process. If the user tokens were elevated or limited for
	# this process, GetOriginalAccountType will return the non-restricted
	# account type.
	# On Vista with UAC, for example, this is not the same value when running
	# with `RequestExecutionLevel user`. GetOriginalAccountType will return
	# "admin" while GetAccountType will return "user".
	StrCmp $1 "Admin" 0 +3
		Goto doit
	StrCmp $1 "Power" 0 +3
		Goto doit
	StrCmp $1 "User" 0 +3
		Goto noop
	StrCmp $1 "Guest" 0 +3
		Goto noop
	MessageBox MB_OK "Unknown error"
	Goto doit

	Win9x:
	doit:
		# We can install
		Call SelectLanguage
		Goto end
	noop:
		MessageBox MB_OK "$(RequiresAdminUser)"
		Quit
	end:
FunctionEnd

Function SelectLanguage
	;Language selection dialog
	Push ""
	Push ${LANG_ENGLISH}
	Push ${LanguageNameEnglish}
	Push ${LANG_GERMAN}
	Push ${LanguageNameGerman}
	Push ${LANG_DUTCH}
	Push ${LanguageNameDutch}
	Push ${LANG_FRENCH}
	Push ${LanguageNameFrench}
	Push ${LANG_POLISH}
	Push ${LanguageNamePolish}
	Push ${LANG_SPANISH}
	Push ${LanguageNameSpanish}
	Push ${LANG_CZECH}
	Push ${LanguageNameCzech}
	Push A ; A means auto count languages
	       ; for the auto count to work the first empty push (Push "") must remain
	LangDLL::LangDialog  "$(InstallerLanguage)" "$(SelectInstallerLanguage)"

	Pop $LANGUAGE
	StrCmp $LANGUAGE "cancel" 0 +2
		Abort

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
		MessageBox MB_OK|MB_ICONEXCLAMATION "$(RequiresNetFramework)"
		Quit
	${EndIf}
FunctionEnd

Section "" ; Install
	SetOutPath $INSTDIR
	SetShellVarContext all
	
	; AddFiles
	File /r /x "mRemoteNG.vshost.*" "..\mRemoteV1\bin\Release\*.*"
	File /r "Dependencies\*.*"
	File "..\*.txt"

	; Uninstaller
	WriteUninstaller "$INSTDIR\Uninstall.exe"

	; Register ActiveX components
	RegDLL "$INSTDIR\eolwtscom.dll"
 
	; Start Menu
	CreateDirectory "$SMPROGRAMS\mRemoteNG"
	CreateShortCut "$SMPROGRAMS\mRemoteNG\mRemoteNG.lnk" "$INSTDIR\mRemoteNG.exe"
	CreateShortCut "$SMPROGRAMS\mRemoteNG\Uninstall.lnk" "$INSTDIR\Uninstall.exe"

	; Registry
	WriteRegStr HKLM "Software\mRemoteNG" "InstallPath" $INSTDIR
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemoteNG" "DisplayName" "mRemoteNG"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemoteNG" "Publisher" "mRemoteNG"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemoteNG" "DisplayIcon" "$INSTDIR\mRemoteNG.exe"
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemoteNG" "EstimatedSize" 6464

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

	; Delete Files
	RMDIR /r $INSTDIR

	; Start Menu
	SetShellVarContext all
	Delete "$SMPROGRAMS\mRemoteNG\mRemoteNG.lnk"
	Delete "$SMPROGRAMS\mRemoteNG\Uninstall.lnk"
	RMDir "$SMPROGRAMS\mRemoteNG"
	SetShellVarContext current
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
