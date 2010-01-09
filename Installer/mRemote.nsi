!include "MUI.nsh"

;Basic Config
Name "mRemoteNG"
OutFile bin\mRemoteNG-Installer.exe
InstallDir $PROGRAMFILES\mRemoteNG
InstallDirRegKey HKCU "Software\mRemoteNG" ""

;Design
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

;Install Pages
!insertmacro MUI_PAGE_LICENSE ..\mRemoteV1\bin\Release\License.txt
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!define MUI_FINISHPAGE_NOAUTOCLOSE

;Finish Page
!define MUI_FINISHPAGE_RUN_NOTCHECKED
!define MUI_FINISHPAGE_RUN_Text "Start mRemoteNG"
!define MUI_FINISHPAGE_RUN "$INSTDIR\mRemoteNG.exe"
!insertmacro MUI_PAGE_FINISH

;Uninstall Pages
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

;Set Language
!insertmacro MUI_LANGUAGE "English"

Section ""
	SetOutPath $INSTDIR

	;AddFiles
	File /r /x *.lic ..\mRemoteV1\bin\Release\*.*

	;Uninstaller
	WriteUninstaller "$INSTDIR\Uninstall.exe"

	;Register ActiveX components
	RegDLL "$INSTDIR\eolwtscom.dll"
	RegDLL "$INSTDIR\scvncctrl.dll"
 
	;Start Menu
	CreateDirectory "$SMPROGRAMS\mRemoteNG"
	CreateShortCut "$SMPROGRAMS\mRemoteNG\mRemoteNG.lnk" "$INSTDIR\mRemoteNG.exe"
	CreateShortCut "$SMPROGRAMS\mRemoteNG\Uninstall.lnk" "$INSTDIR\Uninstall.exe"

	;Registry
	WriteRegStr HKCU "Software\mRemoteNG" "" $INSTDIR
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemoteNG" DisplayName "mRemoteNG"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemoteNG" UninstallString '"$INSTDIR\Uninstall.exe"'
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemoteNG" DisplayIcon "$INSTDIR\mRemoteNG.exe"
SectionEnd

Section "un.Uninstall"
	;Unregister ActiveX components
	UnregDLL "$INSTDIR\eolwtscom.dll"
	UnregDLL "$INSTDIR\scvncctrl.dll"

	;Delete Files
	RMDIR /r $INSTDIR

	;Start Menu
	Delete "$SMPROGRAMS\mRemoteNG\mRemote.lnk"
	Delete "$SMPROGRAMS\mRemoteNG\Uninstall.lnk"
	RMDir "$SMPROGRAMS\mRemoteNG"

	;Registry
	DeleteRegKey /ifempty HKCU "Software\mRemoteNG"
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemoteNG"
SectionEnd
