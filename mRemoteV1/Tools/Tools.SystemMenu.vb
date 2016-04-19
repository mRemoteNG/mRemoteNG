Imports mRemote3G.App

Namespace Tools
    Public Class SystemMenu
        Public Enum Flags
            MF_STRING = NativeMethods.MF_STRING
            MF_SEPARATOR = NativeMethods.MF_SEPARATOR
            MF_BYCOMMAND = NativeMethods.MF_BYCOMMAND
            MF_BYPOSITION = NativeMethods.MF_BYPOSITION
            MF_POPUP = NativeMethods.MF_POPUP

            WM_SYSCOMMAND = NativeMethods.WM_SYSCOMMAND
        End Enum

        Public SystemMenuHandle As IntPtr
        Public FormHandle As IntPtr

        Public Sub New(Handle As IntPtr)
            FormHandle = Handle
            SystemMenuHandle = NativeMethods.GetSystemMenu(FormHandle, False)
        End Sub

        Public Sub Reset()
            SystemMenuHandle = NativeMethods.GetSystemMenu(FormHandle, True)
        End Sub

        Public Sub AppendMenuItem(ParentMenu As IntPtr, Flags As Flags, ID As Integer, Text As String)
            NativeMethods.AppendMenu(ParentMenu, Flags, ID, Text)
        End Sub

        Public Function CreatePopupMenuItem() As IntPtr
            Return NativeMethods.CreatePopupMenu()
        End Function

        Public Function InsertMenuItem(SysMenu As IntPtr, Position As Integer, Flags As Flags, Submenu As IntPtr,
                                       Text As String) As Boolean
            Return NativeMethods.InsertMenu(SysMenu, Position, Flags, Submenu, Text)
        End Function

        Public Function SetBitmap(Menu As IntPtr, Position As Integer, Flags As Flags, Bitmap As Bitmap) As IntPtr
            Return NativeMethods.SetMenuItemBitmaps(Menu, Position, Flags, Bitmap.GetHbitmap(), Bitmap.GetHbitmap)
        End Function
    End Class
End Namespace