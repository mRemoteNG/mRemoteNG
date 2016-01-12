Namespace Tools
    Public Class SystemMenu
        Public Enum Flags
            MF_STRING = App.Native.MF_STRING
            MF_SEPARATOR = App.Native.MF_SEPARATOR
            MF_BYCOMMAND = App.Native.MF_BYCOMMAND
            MF_BYPOSITION = App.Native.MF_BYPOSITION
            MF_POPUP = App.Native.MF_POPUP

            WM_SYSCOMMAND = App.Native.WM_SYSCOMMAND
        End Enum

        Public SystemMenuHandle As IntPtr
        Public FormHandle As IntPtr

        Public Sub New(ByVal Handle As IntPtr)
            FormHandle = Handle
            SystemMenuHandle = App.Native.GetSystemMenu(FormHandle, False)
        End Sub

        Public Sub Reset()
            SystemMenuHandle = App.Native.GetSystemMenu(FormHandle, True)
        End Sub

        Public Sub AppendMenuItem(ByVal ParentMenu As IntPtr, ByVal Flags As Flags, ByVal ID As Integer, ByVal Text As String)
            App.Native.AppendMenu(ParentMenu, Flags, ID, Text)
        End Sub

        Public Function CreatePopupMenuItem() As IntPtr
            Return App.Native.CreatePopupMenu()
        End Function

        Public Function InsertMenuItem(ByVal SysMenu As IntPtr, ByVal Position As Integer, ByVal Flags As Flags, ByVal SubMenu As IntPtr, ByVal Text As String) As Boolean
            Return App.Native.InsertMenu(SysMenu, Position, Flags, SubMenu, Text)
        End Function

        Public Function SetBitmap(ByVal Menu As IntPtr, ByVal Position As Integer, ByVal Flags As Flags, ByVal Bitmap As Bitmap) As IntPtr
            Return App.Native.SetMenuItemBitmaps(Menu, Position, Flags, Bitmap.GetHbitmap(), Bitmap.GetHbitmap)
        End Function
    End Class
End Namespace