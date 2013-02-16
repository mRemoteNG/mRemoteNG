; ---------------------
;      DotNetVer.nsh
;      Written by: David Grinberg
;      Homepage: http://ontheperiphery.veraida.com/
;      Updated By: Brandon Hansen (http://www.remotehams.com/)
; ---------------------
;
; LogicLib extensions for checking Microsoft .NET Framework versions and service packs.
;
; Latests Updates by Brandon Hansen, KG6YPI (RemoteHams.com)
; Dec 26, 2011 - .NET Framework 4.0 detection fixes - client profile not being found
; Dec 07, 2010 - .NET Framework 4.0 detection added by Brandon Hansen (KG6YPI)
;
; Usage examples:
;
; ${If} ${HasDotNet4.0}
;    DetailPrint "Microsoft .NET Framework 4.0 installed."
;    ${If} ${DOTNETVER_4_0} AtLeastDotNetServicePack 1
;        DetailPrint "Microsoft .NET Framework 4.0 is at least SP1."
;    ${Else}
;        DetailPrint "Microsoft .NET Framework 4.0 SP1 not installed."
;    ${EndIf}
;    ${If} ${DOTNETVER_4_0} HasDotNetClientProfile 1
;        DetailPrint "Microsoft .NET Framework 4.0 (Client Profile) available."
;    ${EndIf}
;    ${If} ${DOTNETVER_4_0} HasDotNetFullProfile 1
;        DetailPrint "Microsoft .NET Framework 4.0 (Full Profile) available."
;    ${EndIf}
;    ${If} ${DOTNETVER_4_0} HasDotNetFullProfile 0
;        DetailPrint "Microsoft .NET Framework 4.0 (Full Profile) not available."
;    ${EndIf}
; ${EndIf}
 
 
!verbose push
!verbose 3
 
!ifndef ___DOTNETVER__NSH___
!define ___DOTNETVER__NSH___
 
!include LogicLib.nsh
!include Util.nsh
 
# constants
 
!define DOTNETVER_1_0  "1.0"
!define DOTNETVER_1_1  "1.1"
!define DOTNETVER_2_0  "2.0"
!define DOTNETVER_3_0  "3.0"
!define DOTNETVER_3_5  "3.5"
!define DOTNETVER_4_0  "4.0"
 
# variable declaration
 
Var /GLOBAL __DONTNET_FOUNDVER
 
!macro __DotNetVer_DeclareVars
    !ifndef __DOTNETVER_VARS_DECLARED
        !define __DOTNETVER_VARS_DECLARED
        Var /GLOBAL __DOTNET_1.0
        Var /GLOBAL __DOTNET_1.1
        Var /GLOBAL __DOTNET_2.0
        Var /GLOBAL __DOTNET_3.0
        Var /GLOBAL __DOTNET_3.5
        Var /GLOBAL __DOTNET_4.0
 
        Var /GLOBAL __DOTNETVER_1.0_SP
        Var /GLOBAL __DOTNETVER_1.1_SP
        Var /GLOBAL __DOTNETVER_2.0_SP
        Var /GLOBAL __DOTNETVER_3.0_SP
        Var /GLOBAL __DOTNETVER_3.5_SP
        Var /GLOBAL __DOTNETVER_4.0_SP
 
        Var /GLOBAL __DOTNET_1.0_CLIENT
        Var /GLOBAL __DOTNET_1.1_CLIENT
        Var /GLOBAL __DOTNET_2.0_CLIENT
        Var /GLOBAL __DOTNET_3.0_CLIENT
        Var /GLOBAL __DOTNET_3.5_CLIENT
        Var /GLOBAL __DOTNET_4.0_CLIENT
 
        Var /GLOBAL __DOTNET_1.0_FULL
        Var /GLOBAL __DOTNET_1.1_FULL
        Var /GLOBAL __DOTNET_2.0_FULL
        Var /GLOBAL __DOTNET_3.0_FULL
        Var /GLOBAL __DOTNET_3.5_FULL
        Var /GLOBAL __DOTNET_4.0_FULL
 
        StrCpy $__DOTNET_1.0 0
        StrCpy $__DOTNET_1.1 0
        StrCpy $__DOTNET_2.0 0
        StrCpy $__DOTNET_3.0 0
        StrCpy $__DOTNET_3.5 0
        StrCpy $__DOTNET_4.0 0
 
        StrCpy $__DOTNETVER_1.0_SP 0
        StrCpy $__DOTNETVER_1.1_SP 0
        StrCpy $__DOTNETVER_2.0_SP 0
        StrCpy $__DOTNETVER_3.0_SP 0
        StrCpy $__DOTNETVER_3.5_SP 0
        StrCpy $__DOTNETVER_4.0_SP 0
 
        StrCpy $__DOTNET_1.0_CLIENT 0
        StrCpy $__DOTNET_1.1_CLIENT 0
        StrCpy $__DOTNET_2.0_CLIENT 0
        StrCpy $__DOTNET_3.0_CLIENT 0
        StrCpy $__DOTNET_3.5_CLIENT 0
        StrCpy $__DOTNET_4.0_CLIENT 0
 
        StrCpy $__DOTNET_1.0_FULL 0
        StrCpy $__DOTNET_1.1_FULL 0
        StrCpy $__DOTNET_2.0_FULL 0
        StrCpy $__DOTNET_3.0_FULL 0
        StrCpy $__DOTNET_3.5_FULL 0
        StrCpy $__DOTNET_4.0_FULL 0
 
  !endif
!macroend
 
 
# lazy initialization macro
 
!macro __DotNetVer_InitVars
    # only calculate version once
    StrCmp $__DONTNET_FOUNDVER "" dotnetver.noveryet
        Return
 
    dotnetver.noveryet:
    !insertmacro __DotNetVer_DeclareVars
 
    Push $0    ;registry count
    Push $1    ;registry key
    Push $2    ;version number
    Push $3    ;installed
    Push $4    ;service pack number
    Push $8    ;strLen helper var
 
    StrCpy $0 0
 
    dotnetver.startenum:
 
    EnumRegKey $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP" $0
    StrCmp $1 "" dotnetver.done
 
    IntOp $0 $0 + 1
 
    StrCpy $2 $1 1 0
    StrCmp $2 "v" +1 dotnetver.startenum
    StrCpy $2 $1 3 1
 
    ; Check for .NET 1.0 to 3.5
    ReadRegDWORD $3 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\$1" "Install"
    ReadRegDWORD $4 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\$1" "SP"
    ; This is a sanity check that works on .NET 1.0 to 3.5
    ; if it fails check for dotnet 4
    IntCmp $3 0 dotnetcheck.40
    StrCmp $2 ${DOTNETVER_1_0} dotnetver.10
    StrCmp $2 ${DOTNETVER_1_1} dotnetver.11
    StrCmp $2 ${DOTNETVER_2_0} dotnetver.20
    StrCmp $2 ${DOTNETVER_3_0} dotnetver.30
    StrCmp $2 ${DOTNETVER_3_5} dotnetver.35
    dotnetcheck.40:
    StrCmp $2 ${DOTNETVER_4_0} dotnetver.40
    StrCmp $2 "4" dotnetver.40
 
    Goto dotnetver.startenum
 
    dotnetver.10:
        StrCpy $__DOTNET_1.0 1
        StrCpy $__DOTNETVER_1.0_SP $4
        StrCpy $__DOTNET_1.0_FULL 1
        Goto dotnetver.startenum
    dotnetver.11:
        StrCpy $__DOTNET_1.1 1
        StrCpy $__DOTNETVER_1.1_SP $4
        StrCpy $__DOTNET_1.1_FULL 1
        Goto dotnetver.startenum
    dotnetver.20:
        StrCpy $__DOTNET_2.0 1
        StrCpy $__DOTNETVER_2.0_SP $4
        StrCpy $__DOTNET_2.0_FULL 1
        Goto dotnetver.startenum
    dotnetver.30:
        StrCpy $__DOTNET_3.0 1
        StrCpy $__DOTNETVER_3.0_SP $4
        StrCpy $__DOTNET_3.0_FULL 1
        Goto dotnetver.startenum
    dotnetver.35:
        StrCpy $__DOTNET_3.5 1
        StrCpy $__DOTNETVER_3.5_SP $4
        StrCpy $__DOTNET_3.5_FULL 1
        Goto dotnetver.startenum
    dotnetver.40:
        ; Check for .NET 4.0 (Full Profile)
        ReadRegDWORD $3 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Install"
        ReadRegDWORD $4 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "SP"
	StrLen $8 $3
        IntCmp $8 0 dotnetcheck.40c
        IntCmp $3 0 dotnetcheck.40c
        StrCmp $2 ${DOTNETVER_4_0} dotnetver.40_Full
        StrCmp $2 "4" dotnetver.40_Full
        dotnetcheck.40c:
        ; Check for .NET 4.0 (Client Profile)
        ReadRegDWORD $3 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Client" "Install"
        ReadRegDWORD $4 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Client" "SP"
	StrLen $8 $3
        IntCmp $8 0 dotnetver.startenum
        IntCmp $3 0 dotnetver.startenum
        StrCmp $2 ${DOTNETVER_4_0} dotnetver.40_Client
        StrCmp $2 "4" dotnetver.40_Client
        Goto dotnetver.startenum
    dotnetver.40_Full:
        StrCpy $__DOTNET_4.0 1
        StrCpy $__DOTNETVER_4.0_SP $4
        StrCpy $__DOTNET_4.0_FULL 1
        Goto dotnetcheck.40c ; continue looking for other profiles
    dotnetver.40_Client:
        StrCpy $__DOTNET_4.0 1
        StrCpy $__DOTNETVER_4.0_SP $4
        StrCpy $__DOTNET_4.0_CLIENT 1
        Goto dotnetver.startenum
 
    dotnetver.done:
 
    StrCpy $__DONTNET_FOUNDVER "1"
 
    Pop $8
    Pop $4
    Pop $3
    Pop $2
    Pop $1
    Pop $0
!macroend
 
!macro _HasDotNet _a _b _t _f
    ${CallArtificialFunction} __DotNetVer_InitVars
 
   !insertmacro _= `$__DOTNET_${_b}` `1` `${_t}` `${_f}`
!macroend
 
!macro __DotNetVer_DefineTest Ver
  !define HasDotNet${Ver} `"" HasDotNet ${Ver}`
!macroend
 
!insertmacro __DotNetVer_DefineTest ${DOTNETVER_1_0}
!insertmacro __DotNetVer_DefineTest ${DOTNETVER_1_1}
!insertmacro __DotNetVer_DefineTest ${DOTNETVER_2_0}
!insertmacro __DotNetVer_DefineTest ${DOTNETVER_3_0}
!insertmacro __DotNetVer_DefineTest ${DOTNETVER_3_5}
!insertmacro __DotNetVer_DefineTest ${DOTNETVER_4_0}
 
!macro _AtLeastDotNetServicePack _a _b _t _f
    ${CallArtificialFunction} __DotNetVer_InitVars
 
    !insertmacro _>= `$__DOTNETVER_${_a}_SP` `${_b}` `${_t}` `${_f}`
!macroend
!define AtLeastDotNetServicePack `AtLeastDotNetServicePack`
 
 
!macro _AtMostDotNetServicePack _a _b _t _f
    ${CallArtificialFunction} __DotNetVer_InitVars
 
    !insertmacro _<= `$__DOTNETVER_${_a}_SP` `${_b}` `${_t}` `${_f}`
!macroend
!define AtMostDotNetServicePack `AtMostDotNetServicePack`
 
 
!macro _IsDotNetServicePack _a _b _t _f
    ${CallArtificialFunction} __DotNetVer_InitVars
 
    !insertmacro _= `$__DOTNETVER_${_a}_SP` `${_b}` `${_t}` `${_f}`
!macroend
!define IsDotNetServicePack `IsDotNetServicePack`
 
!macro _HasDotNetClientProfile _a _b _t _f
    ${CallArtificialFunction} __DotNetVer_InitVars
 
    !insertmacro _= `$__DOTNET_${_a}_CLIENT` `${_b}` `${_t}` `${_f}`
!macroend
!define HasDotNetClientProfile `HasDotNetClientProfile`
 
!macro _HasDotNetFullProfile _a _b _t _f
    ${CallArtificialFunction} __DotNetVer_InitVars
 
    !insertmacro _= `$__DOTNET_${_a}_FULL` `${_b}` `${_t}` `${_f}`
!macroend
!define HasDotNetFullProfile `HasDotNetFullProfile`
 
# done
 
!endif # !___DOTNETVER__NSH___
 
!verbose pop