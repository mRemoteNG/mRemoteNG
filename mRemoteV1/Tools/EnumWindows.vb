Namespace Tools
    Public Class EnumWindows
        Public Shared Function EnumWindows() As List(Of IntPtr)
            Dim handleList As New List(Of IntPtr)

            HandleLists.Add(handleList)
            Dim handleIndex As Integer = HandleLists.IndexOf(handleList)
            Win32.EnumWindows(AddressOf EnumCallback, handleIndex)
            HandleLists.Remove(handleList)

            Return handleList
        End Function

        Public Shared Function EnumChildWindows(hWndParent As IntPtr) As List(Of IntPtr)
            Dim handleList As New List(Of IntPtr)

            HandleLists.Add(handleList)
            Dim handleIndex As Integer = HandleLists.IndexOf(handleList)
            Win32.EnumChildWindows(hWndParent, AddressOf EnumCallback, handleIndex)
            HandleLists.Remove(handleList)

            Return handleList
        End Function

        Private Shared ReadOnly HandleLists As New List(Of List(Of IntPtr))

        Private Shared Function EnumCallback(hwnd As Integer, lParam As Integer) As Boolean
            HandleLists(lParam).Add(hwnd)
            Return True
        End Function

        ' ReSharper disable ClassNeverInstantiated.Local
        Private Class Win32
            ' ReSharper restore ClassNeverInstantiated.Local
            Public Delegate Function EnumWindowsProc(hwnd As Integer, lParam As Integer) As Boolean

            Declare Function EnumWindows Lib "user32"(lpEnumFunc As EnumWindowsProc, lParam As Integer) As Boolean
            Declare Function EnumChildWindows Lib "user32"(hWndParent As IntPtr, lpEnumFunc As EnumWindowsProc,
                                                           lParam As Integer) As Boolean
        End Class

        Private Sub New()
        End Sub
    End Class
End Namespace
