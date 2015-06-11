Imports System.Runtime.InteropServices
Imports System.Text

Namespace App
    Public Class Utils
        <DllImport("USER32.DLL")> _
        Private Shared Function GetShellWindow() As IntPtr
        End Function

        <DllImport("USER32.DLL")> _
        Private Shared Function GetWindowText(ByVal hWnd As IntPtr, ByVal lpString As StringBuilder, ByVal nMaxCount As Integer) As Integer
        End Function

        <DllImport("USER32.DLL")> _
        Private Shared Function GetWindowTextLength(ByVal hWnd As IntPtr) As Integer
        End Function

        <DllImport("user32.dll", SetLastError:=True)> _
        Private Shared Function GetWindowThreadProcessId(ByVal hWnd As IntPtr, <Out()> ByRef lpdwProcessId As UInt32) As UInt32
        End Function

        <DllImport("USER32.DLL")> _
        Private Shared Function IsWindowVisible(ByVal hWnd As IntPtr) As Boolean
        End Function

        Private Delegate Function EnumWindowsProc(ByVal hWnd As IntPtr, ByVal lParam As Integer) As Boolean

        <DllImport("USER32.DLL")> _
        Private Shared Function EnumWindows(ByVal enumFunc As EnumWindowsProc, ByVal lParam As Integer) As Boolean
        End Function

        Private hShellWindow As IntPtr = GetShellWindow()
        Private dictWindows As New Dictionary(Of IntPtr, String)
        Private currentProcessID As Integer


        Public Function GetOpenWindowsFromPID(ByVal processID As Integer) As IDictionary(Of IntPtr, String)
            dictWindows.Clear()
            currentProcessID = processID
            EnumWindows(AddressOf enumWindowsInternal, 0)
            Return dictWindows
        End Function

        Private Function enumWindowsInternal(ByVal hWnd As IntPtr, ByVal lParam As Integer) As Boolean
            If (hWnd <> hShellWindow) Then
                Dim windowPid As UInt32
                If Not IsWindowVisible(hWnd) Then
                    Return True
                End If
                Dim length As Integer = GetWindowTextLength(hWnd)
                If (length = 0) Then
                    Return True
                End If
                GetWindowThreadProcessId(hWnd, windowPid)
                If (windowPid <> currentProcessID) Then
                    Return True
                End If
                Dim stringBuilder As New StringBuilder(length)
                GetWindowText(hWnd, stringBuilder, (length + 1))
                dictWindows.Add(hWnd, stringBuilder.ToString)
            End If
            Return True
        End Function
    End Class
End Namespace
