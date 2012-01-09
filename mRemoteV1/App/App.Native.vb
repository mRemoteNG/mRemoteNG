Imports System.Runtime.InteropServices

Namespace App
    Public Class Native
#Region "Dll Imports"
        <DllImport("user32.dll", EntryPoint:="GetWindowThreadProcessId", SetLastError:=True, CharSet:=CharSet.Unicode, ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
        Public Shared Function GetWindowThreadProcessId(ByVal hWnd As Long, ByVal lpdwProcessId As Long) As Long
        End Function

        <DllImport("user32.dll", SetLastError:=True)> _
        Public Shared Function FindWindow(ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
        End Function

        <DllImport("user32.dll", SetLastError:=True)> _
        Public Shared Function SetParent(ByVal hWndChild As IntPtr, ByVal hWndNewParent As IntPtr) As Long
        End Function

        <DllImport("user32.dll", EntryPoint:="GetWindowLongA", SetLastError:=True)> _
        Public Shared Function GetWindowLong(ByVal hWnd As IntPtr, ByVal nIndex As Integer) As Long
        End Function

        <DllImport("user32.dll")> _
        Public Shared Function SetWindowLong(ByVal hWnd As IntPtr, ByVal nIndex As Integer, ByVal dwNewLong As Long) As Integer
        End Function

        <DllImport("user32.dll", SetLastError:=True)> _
        Public Shared Function SetWindowPos(ByVal hWnd As IntPtr, ByVal hWndInsertAfter As Long, ByVal x As Long, ByVal y As Long, ByVal cx As Long, ByVal cy As Long, ByVal wFlags As Long) As Long
        End Function

        <DllImport("user32.dll", SetLastError:=True)> _
        Public Shared Function MoveWindow(ByVal hWnd As IntPtr, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal repaint As Boolean) As Boolean
        End Function

        <DllImport("user32.dll", EntryPoint:="PostMessageA", SetLastError:=True)> _
        Public Shared Function PostMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As Integer, ByVal lParam As Integer) As Boolean
        End Function

        <DllImport("user32")> _
        Public Shared Function ShowWindow(ByVal hWnd As IntPtr, ByVal nCmdShow As Integer) As Integer
        End Function

        <DllImport("user32.dll")> _
        Public Shared Function BringWindowToTop(ByVal hWnd As IntPtr) As Boolean
        End Function

        <DllImport("user32.dll")> _
        Public Shared Function SetForegroundWindow(ByVal hWnd As IntPtr) As Boolean
        End Function

        <DllImport("user32.dll")> _
        Public Shared Function GetForegroundWindow() As IntPtr
        End Function

        <DllImport("user32.dll", EntryPoint:="GetWindowTextLengthA")> _
        Public Shared Function GetWindowTextLength(ByVal hWnd As IntPtr) As Long
        End Function

        <DllImport("user32.dll")> _
        Public Shared Function IsIconic(ByVal hWnd As IntPtr) As Integer
        End Function

        <DllImport("user32.dll")> _
        Public Shared Function GetSystemMenu(ByVal hWnd As IntPtr, ByVal bRevert As Boolean) As IntPtr
        End Function

        <DllImport("user32.dll")> _
        Public Shared Function AppendMenu(ByVal hMenu As IntPtr, ByVal uFlags As Int32, ByVal uIDNewItem As IntPtr, ByVal lpNewItem As String) As Boolean
        End Function

        <DllImport("user32")> _
        Public Shared Function InsertMenu(ByVal hMenu As IntPtr, ByVal uPosition As Integer, ByVal uFlags As Integer, ByVal uIDNewItem As IntPtr, ByVal lpNewItem As String) As Boolean
        End Function

        <DllImport("user32")> _
        Public Shared Function SetMenuItemBitmaps(ByVal hMenu As IntPtr, ByVal uPosition As Integer, ByVal uFlags As Integer, ByVal hBitmapUnchecked As IntPtr, ByVal hBitmapChecked As IntPtr) As Boolean
        End Function

        <DllImport("user32")> _
        Public Shared Function CreatePopupMenu() As IntPtr
        End Function

        <DllImport("user32")> _
        Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wparam As Integer, ByVal lparam As Integer) As Integer
        End Function

        <DllImport("user32")> _
        Public Shared Function MapVirtualKey(ByVal wCode As Integer, ByVal wMapType As Integer) As Integer
        End Function

        <DllImport("User32")> _
        Public Shared Function SetClipboardViewer(ByVal hWndNewViewer As IntPtr) As IntPtr
        End Function

        <DllImport("User32")> _
        Public Shared Function SendMessage(ByVal Handle As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
        End Function

        <DllImport("user32")> _
        Public Shared Function PostMessage(ByVal hWnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Long
        End Function

        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
        Public Shared Function FindWindowEx(ByVal parentHandle As IntPtr, _
                     ByVal childAfter As IntPtr, _
                     ByVal lclassName As String, _
                     ByVal windowTitle As String) As IntPtr
        End Function

        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
        Public Shared Function SetFocus(ByVal hwnd As IntPtr) As IntPtr
        End Function
#End Region

#Region "Constants"
        Public Const SWP_NOOWNERZORDER As Integer = 512
        Public Const SWP_NOREDRAW As Integer = 8
        Public Const SWP_NOZORDER As Integer = 4
        Public Const SWP_SHOWWINDOW As Integer = 64
        Public Const WS_EX_MDICHILD As Integer = 64
        Public Const SWP_FRAMECHANGED As Integer = 32
        Public Const SWP_NOACTIVATE As Integer = 16
        Public Const SWP_ASYNCWINDOWPOS As Integer = 16384
        Public Const SWP_NOMOVE As Integer = 2
        Public Const SWP_NOSIZE As Integer = 1
        Public Const GWL_STYLE As Integer = (-16)
        Public Const WS_VISIBLE As Integer = 268435456
        Public Const WM_CLOSE As Integer = 16
        Public Const WS_CHILD As Integer = 1073741824
        Public Const WS_MAXIMIZE As Integer = 16777216
        Public Const SW_SHOWMAXIMIZED As Integer = 3
        Public Const SW_RESTORE As Integer = 9

        Public Const MF_STRING As Integer = &H0
        Public Const MF_SEPARATOR As Integer = &H800&
        Public Const MF_BYCOMMAND As Integer = &H0
        Public Const MF_BYPOSITION As Integer = &H400
        Public Const MF_POPUP As Integer = &H10

        Public Const WM_ENTERSIZEMOVE As Integer = &H231
        Public Const WM_EXITSIZEMOVE As Integer = &H232

        Public Const WM_GETTEXT As Integer = &HD
        Public Const WM_ACTIVATEAPP As Integer = &H1C
        Public Const WM_WINDOWPOSCHANGED As Integer = &H47

        Public Const WM_SYSCOMMAND As Integer = &H112

        Public Const WM_LBUTTONDOWN As Integer = &H201
        Public Const WM_LBUTTONUP As Integer = &H202
        Public Const WM_RBUTTONDOWN As Integer = &H204
        Public Const WM_RBUTTONUP As Integer = &H205

        Public Const WM_KEYDOWN As Integer = &H100
        Public Const WM_KEYUP As Integer = &H101

        Public Const VK_CONTROL As Integer = &H11
        Public Const VK_C As Integer = &H67

        Public Const WM_DRAWCLIPBOARD As Integer = 776
        Public Const WM_CHANGECBCHAIN As Integer = 781
#End Region
    End Class
End Namespace