Imports System.Runtime.InteropServices

Namespace App
    Public Class NativeMethods
        Private Sub New()
            ' Fix Warning 292 CA1053 : Microsoft.Design : Because type 'Native' contains only 'static' ('Shared' in Visual Basic) members, add a default private constructor to prevent the compiler from adding a default public constructor.
        End Sub

#Region "Functions"
        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
        Public Shared Function AppendMenu(ByVal hMenu As IntPtr, ByVal uFlags As Int32, ByVal uIDNewItem As IntPtr, ByVal lpNewItem As String) As Boolean
        End Function

        <DllImport("user32.dll", SetLastError := True, CharSet := CharSet.Auto)>
        Public Shared Function CreatePopupMenu() As IntPtr
        End Function

        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
        Public Shared Function FindWindowEx(ByVal parentHandle As IntPtr, ByVal childAfter As IntPtr, ByVal lclassName As String, ByVal windowTitle As String) As IntPtr
        End Function

        <DllImport("user32.dll", SetLastError := True, CharSet := CharSet.Auto)>
        Public Shared Function GetForegroundWindow() As IntPtr
        End Function

        <DllImport("user32.dll", SetLastError := True, CharSet := CharSet.Auto)>
        Public Shared Function GetSystemMenu(hWnd As IntPtr, bRevert As Boolean) As IntPtr
        End Function

        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
        Public Shared Function InsertMenu(ByVal hMenu As IntPtr, ByVal uPosition As Integer, ByVal uFlags As Integer, ByVal uIDNewItem As IntPtr, ByVal lpNewItem As String) As Boolean
        End Function

        <DllImport("user32.dll", SetLastError := True, CharSet := CharSet.Auto)>
        Public Shared Function IsIconic(hWnd As IntPtr) As Integer
        End Function

        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
        Public Shared Function MoveWindow(ByVal hWnd As IntPtr, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal repaint As Boolean) As Boolean
        End Function

        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
        Public Shared Function PostMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As Integer, ByVal lParam As Integer) As Boolean
        End Function

        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
        Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wparam As Integer, ByVal lparam As Integer) As Integer
        End Function

        <DllImport("user32.dll", SetLastError := True, CharSet := CharSet.Auto)>
        Public Shared Function SetClipboardViewer(hWndNewViewer As IntPtr) As IntPtr
        End Function

        <DllImport("user32.dll", SetLastError := True, CharSet := CharSet.Auto)>
        Public Shared Function SetForegroundWindow(hWnd As IntPtr) As Boolean
        End Function

        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
        Public Shared Function SetMenuItemBitmaps(ByVal hMenu As IntPtr, ByVal uPosition As Integer, ByVal uFlags As Integer, ByVal hBitmapUnchecked As IntPtr, ByVal hBitmapChecked As IntPtr) As Boolean
        End Function

        <DllImport("user32.dll", SetLastError := True, CharSet := CharSet.Auto)>
        Public Shared Function SetParent(hWndChild As IntPtr, hWndNewParent As IntPtr) As Long
        End Function

        <DllImport("user32.dll", SetLastError := True, CharSet := CharSet.Auto)>
        Public Shared Function SetWindowLong(hWnd As IntPtr, nIndex As Integer, dwNewLong As Long) As Integer
        End Function

        <DllImport("user32.dll", SetLastError := True, CharSet := CharSet.Auto)>
        Public Shared Function ShowWindow(hWnd As IntPtr, nCmdShow As Integer) As Integer
        End Function

        <DllImport("user32.dll", SetLastError := True, CharSet := CharSet.Auto)>
        Public Shared Function WindowFromPoint(point As Point) As IntPtr
        End Function

#End Region

#Region "Structures"

        <StructLayout(LayoutKind.Sequential)>
        Public Structure WINDOWPOS
            Private ReadOnly hwnd As IntPtr
            Private ReadOnly hwndInsertAfter As IntPtr
            Public x As Integer
            Public y As Integer
            Public cx As Integer
            Public cy As Integer
            Public flags As Integer
        End Structure

#End Region

#Region "Helpers"

        Public Shared Function MAKELONG(ByRef wLow As Int32, ByRef wHigh As Int32) As Int32
            Return wLow Or wHigh << 16
        End Function

        Public Shared Function MAKELPARAM(ByRef wLow As Int32, ByRef wHigh As Int32) As Int32
            Return MAKELONG(wLow, wHigh)
        End Function

        Public Shared Function LOWORD(ByRef value As Int32) As Int32
            Return value And &HFFFF
        End Function

        Public Shared Function LOWORD(ByRef value As IntPtr) As Int32
            Return LOWORD(value.ToInt32)
        End Function

        Public Shared Function HIWORD(ByRef value As Int32) As Int32
            Return value >> 16
        End Function

        Public Shared Function HIWORD(ByRef value As IntPtr) As Int32
            Return HIWORD(value.ToInt32)
        End Function

#End Region

#Region "Constants"
        ' GetWindowLong
        Public Const GWL_STYLE As Integer = (- 16)

        ' AppendMenu / ModifyMenu / DeleteMenu / RemoveMenu
        Public Const MF_BYCOMMAND As Integer = &H0
        Public Const MF_BYPOSITION As Integer = &H400
        Public Const MF_STRING As Integer = &H0
        Public Const MF_POPUP As Integer = &H10
        Public Const MF_SEPARATOR As Integer = &H800

        ' WM_LBUTTONDOWN / WM_LBUTTONUP
        Public Const MK_LBUTTON As Integer = &H1

        ' ShowWindow
        Public Const SW_SHOWMAXIMIZED As Integer = 3
        Public Const SW_RESTORE As Integer = 9

        ' SetWindowPos / WM_WINDOWPOSCHANGING / WM_WINDOWPOSCHANGED
        Public Const SWP_NOSIZE As Integer = &H1
        Public Const SWP_NOMOVE As Integer = &H2
        Public Const SWP_NOZORDER As Integer = &H4
        Public Const SWP_NOREDRAW As Integer = &H8
        Public Const SWP_NOACTIVATE As Integer = &H10
        Public Const SWP_DRAWFRAME As Integer = &H20
        Public Const SWP_FRAMECHANGED As Integer = &H20
        Public Const SWP_SHOWWINDOW As Integer = &H40
        Public Const SWP_HIDEWINDOW As Integer = &H80
        Public Const SWP_NOCOPYBITS As Integer = &H100
        Public Const SWP_NOOWNERZORDER As Integer = &H200
        Public Const SWP_NOSENDCHANGING As Integer = &H400
        Public Const SWP_NOCLIENTSIZE As Integer = &H800
        Public Const SWP_NOCLIENTMOVE As Integer = &H1000
        Public Const SWP_DEFERERASE As Integer = &H2000
        Public Const SWP_ASYNCWINDOWPOS As Integer = &H4000
        Public Const SWP_STATECHANGED As Integer = &H8000

        ' WM_ACTIVATE
        Public Const WA_INACTIVE As Integer = &H0
        Public Const WA_ACTIVE As Integer = &H1
        Public Const WA_CLICKACTIVE As Integer = &H2

        ' Window Messages
        Public Const WM_CREATE As Integer = &H1
        Public Const WM_DESTROY As Integer = &H2
        Public Const WM_ACTIVATE As Integer = &H6
        Public Const WM_GETTEXT As Integer = &HD
        Public Const WM_CLOSE As Integer = &H10
        Public Const WM_ACTIVATEAPP As Integer = &H1C
        Public Const WM_MOUSEACTIVATE As Integer = &H21
        Public Const WM_WINDOWPOSCHANGED As Integer = &H47
        Public Const WM_KEYDOWN As Integer = &H100
        Public Const WM_KEYUP As Integer = &H101
        Public Const WM_SYSCOMMAND As Integer = &H112
        Public Const WM_MOUSEMOVE As Integer = &H200
        Public Const WM_LBUTTONDOWN As Integer = &H201
        Public Const WM_LBUTTONUP As Integer = &H202
        Public Const WM_RBUTTONDOWN As Integer = &H204
        Public Const WM_RBUTTONUP As Integer = &H205
        Public Const WM_MBUTTONDOWN As Integer = &H207
        Public Const WM_MBUTTONUP As Integer = &H208
        Public Const WM_XBUTTONDOWN As Integer = &H20B
        Public Const WM_XBUTTONUP As Integer = &H20C
        Public Const WM_PARENTNOTIFY As Integer = &H210
        Public Const WM_ENTERSIZEMOVE As Integer = &H231
        Public Const WM_EXITSIZEMOVE As Integer = &H232
        Public Const WM_DRAWCLIPBOARD As Integer = &H308
        Public Const WM_CHANGECBCHAIN As Integer = &H30D

        ' Window Styles
        Public Const WS_MAXIMIZE As Integer = &H1000000
        Public Const WS_VISIBLE As Integer = &H10000000
        Public Const WS_CHILD As Integer = &H40000000
        Public Const WS_EX_MDICHILD As Integer = &H40

        ' Virtual Key Codes
        Public Const VK_CONTROL As Integer = &H11
        Public Const VK_C As Integer = &H67

#End Region
    End Class
End Namespace