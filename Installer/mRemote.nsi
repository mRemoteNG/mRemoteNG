!include "MUI.nsh"

;Basic Config
Name "mRemote"
OutFile %PathAndFilenameForOutput%
InstallDir $PROGRAMFILES\mRemote
InstallDirRegKey HKCU "Software\mRemote" ""

;Design
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP "header.bmp" ; optional
!define MUI_HEADERIMAGE_BITMAP_NOSTRETCH
!define MUI_HEADERIMAGE_UNBITMAP "header.bmp" ; optional
!define MUI_HEADERIMAGE_UNBITMAP_NOSTRETCH
!define MUI_HEADER_TRANSPARENT_TEXT
#!define MUI_BGCOLOR 000000
!define MUI_WELCOMEFINISHPAGE_BITMAP "welcomefinish.bmp"
!define MUI_UNWELCOMEFINISHPAGE_BITMAP "welcomefinish.bmp"


;Install Pages
!insertmacro MUI_PAGE_LICENSE ..\%ReleasePath%\License.txt
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!define MUI_FINISHPAGE_NOAUTOCLOSE

# settings to start application
!define MUI_FINISHPAGE_RUN_NOTCHECKED
!define MUI_FINISHPAGE_RUN_Text "Start mRemote"
!define MUI_FINISHPAGE_RUN "$INSTDIR\mRemote.exe"
!insertmacro MUI_PAGE_FINISH

;Uninstall Pages
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES


;Set Language
!insertmacro MUI_LANGUAGE "English"


Section ""
SetOutPath $INSTDIR

;AddFiles
File /r /x *.lic ..\%ReleasePath%\*.*

;Uninstaller
WriteUninstaller "$INSTDIR\Uninstall.exe"
;Register ActiveX components
RegDLL "$INSTDIR\eolwtscom.dll"
RegDLL "$INSTDIR\scvncctrl.dll"
 
;Start Menu
CreateDirectory "$SMPROGRAMS\mRemote"
CreateShortCut "$SMPROGRAMS\mRemote\mRemote.lnk" "$INSTDIR\mRemote.exe"
CreateShortCut "$SMPROGRAMS\mRemote\Uninstall.lnk" "$INSTDIR\Uninstall.exe"
;Registry
WriteRegStr HKCU "Software\mRemote" "" $INSTDIR

WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemote" DisplayName "mRemote"
WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemote" UninstallString '"$INSTDIR\Uninstall.exe"'
WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemote" DisplayIcon "$INSTDIR\mRemote.exe"
SectionEnd




Section "un.Uninstall"
;Unregister ActiveX components
UnregDLL "$INSTDIR\eolwtscom.dll"
UnregDLL "$INSTDIR\scvncctrl.dll"

;Delete Files
RMDIR /r $INSTDIR

;Start Menu
Delete "$SMPROGRAMS\mRemote\mRemote.lnk"
Delete "$SMPROGRAMS\mRemote\Uninstall.lnk"
RMDir "$SMPROGRAMS\mRemote"
;Registry
DeleteRegKey /ifempty HKCU "Software\mRemote"
DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\mRemote"
SectionEnd