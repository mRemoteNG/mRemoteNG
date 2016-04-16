!include "MUI.nsh"
!include "WordFunc.nsh"
!insertmacro VersionCompare

!include "..\Release\Version.nsh"

; This will be passed in using the /D switch by BUILD.CMD
!ifdef PRODUCT_VERSION_TAG
	!define PRODUCT_VERSION_FRIENDLY "${PRODUCT_VERSION_SHORT}"
	!define PRODUCT_VERSION_TAGGED "${PRODUCT_VERSION_SHORT}-${PRODUCT_VERSION_TAG}"
!else
	!define PRODUCT_VERSION_FRIENDLY "${PRODUCT_VERSION_SHORT}"
	!define PRODUCT_VERSION_TAGGED "${PRODUCT_VERSION_SHORT}"
!endif

; Basic Config
Name "mRemote3G ${PRODUCT_VERSION_FRIENDLY}"
OutFile "..\Release\mRemote3G-Installer-${PRODUCT_VERSION_TAGGED}.exe"
SetCompressor /SOLID lzma
InstallDir "$PROGRAMFILES\mRemote3G"
InstallDirRegKey HKLM "Software\mRemote3G" "InstallPath"
RequestExecutionLevel admin

; Version Information
VIProductVersion ${PRODUCT_VERSION}
VIAddVersionKey "CompanyName" "kmscode"
VIAddVersionKey "ProductName" "mRemote3G"
VIAddVersionKey "ProductVersion" ${PRODUCT_VERSION}
VIAddVersionKey "LegalCopyright" "Copyright © 2015-2016 Sean Kaim - Based off of mRemote 2007-2009 Felix Deimel, & mRemoteNG 2010-2013 Riley McArdlee"
VIAddVersionKey "FileDescription" "mRemote3G ${PRODUCT_VERSION_FRIENDLY} Installer"
VIAddVersionKey "FileVersion" ${PRODUCT_VERSION}

; Design
!define MUI_ICON "Setup_Install.ico"
!define MUI_UNICON "RecycleBin.ico"
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
!define MUI_FINISHPAGE_RUN "$INSTDIR\mRemote3G.exe"
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
		IfSilent +2
			Call SelectLanguage
		Goto end
	noop:
		MessageBox MB_OK "$(RequiresAdminUser)"
		Quit
	end:
FunctionEnd

; http://stackoverflow.com/questions/15227634/check-for-net4-5-with-nsis
; returns a numeric value on the stack, ranging from 0 to 450, 451, 452 or 460. 0 means nothing found, the other values mean at least that version
Function CheckForDotVersion45Up

  ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" Release

	; https://msdn.microsoft.com/en-us/library/hh925568%28v=vs.110%29.aspx#net_d
	; Anything greater than 393295 is acceptable
  IntCmp $0 393295 is46 isbelow46 is46

  isbelow46:
  IntCmp $0 379893 is452 isbelow452 is452

  isbelow452:
  IntCmp $0 378675 is451 isbelow451 is451

  isbelow451:
  IntCmp $0 378389 is45 isbelow45 is45

  isbelow45:
  Push 0
  Return

  is46:
  Push 460
  Return

  is452:
  Push 452
  Return

  is451:
  Push 451
  Return

  is45:
  Push 45
  Return

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
	Push ${LANG_THAI}
	Push ${LanguageNameThai}
	Push A ; A means auto count languages
	       ; for the auto count to work the first empty push (Push "") must remain
	LangDLL::LangDialog  "$(InstallerLanguage)" "$(SelectInstallerLanguage)"

	Pop $LANGUAGE
	StrCmp $LANGUAGE "cancel" 0 +2
		Abort
		
	Call CheckForDotVersion45Up
	Pop $0
	${IfNot} $0 = 460
		MessageBox MB_OK|MB_ICONEXCLAMATION "$(RequiresNetFramework)"
		Quit
	${EndIf}
	
FunctionEnd

Section "" ; Install
	SetOutPath $INSTDIR
	SetShellVarContext all
	
	; AddFiles
	File /r /x "mRemote3G.vshost.*" "..\mRemoteV1\bin\Release\*.*"
	File /r "Dependencies\*.*"
	File "..\*.txt"

	; Uninstaller
	WriteUninstaller "$INSTDIR\Uninstall.exe"
 
	; Start Menu
	CreateDirectory "$SMPROGRAMS\mRemote3G"
	CreateShortCut "$SMPROGRAMS\mRemote3G\$(CreditsLinkName).lnk" "$INSTDIR\CREDITS.TXT"
	CreateShortCut "$SMPROGRAMS\mRemote3G\$(CopyingLinkName).lnk" "$INSTDIR\COPYING.TXT"
	CreateShortCut "$SMPROGRAMS\mRemote3G\mRemote3G.lnk" "$INSTDIR\mRemote3G.exe"
	CreateShortCut "$SMPROGRAMS\mRemote3G\$(UninstallLinkName).lnk" "$INSTDIR\Uninstall.exe"
	CreateShortCut "$SMPROGRAMS\mRemote3G\$(ChangeLogLinkName).lnk" "$INSTDIR\CHANGELOG.TXT"

	; Registry
	WriteRegStr HKLM "Software\mRemote3G" "InstallPath" $INSTDIR
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemote3G" "DisplayName" "mRemote3G"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemote3G" "Publisher" "kmscode"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemote3G" "DisplayIcon" "$INSTDIR\mRemote3G.exe"
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemote3G" "EstimatedSize" 7080

	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemote3G" "DisplayVersion" ${PRODUCT_VERSION}
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemote3G" "VersionMajor" ${PRODUCT_VERSION_MAJOR}
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemote3G" "VersionMinor" ${PRODUCT_VERSION_MINOR}

	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemote3G" "UninstallString" '"$INSTDIR\Uninstall.exe"'
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemote3G" "NoModify" 1
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemote3G" "NoRepair" 1
SectionEnd

Section "un.Uninstall"
	; Delete Files
	RMDIR /r $INSTDIR

	; Start Menu
	SetShellVarContext all
	RMDir /r "$SMPROGRAMS\mRemote3G"
	SetShellVarContext current
	RMDir /r "$SMPROGRAMS\mRemote3G"

	; Registry
	DeleteRegValue HKLM "Software\mRemote3G" "InstallPath"
	DeleteRegKey /ifempty HKLM "Software\mRemote3G"
	DeleteRegKey /ifempty HKCU "Software\mRemote3G"
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemote3G"
SectionEnd
