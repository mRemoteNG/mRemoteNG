Imports System.Runtime.InteropServices

Namespace Tools
    Public Class WindowPlacement

#Region "Windows API"

#Region "Functions"

        Private Declare Function GetWindowPlacement Lib "user32"(hWnd As IntPtr, ByRef lpwndpl As WINDOWPLACEMENT) _
            As Boolean
        Private Declare Function SetWindowPlacement Lib "user32"(hWnd As IntPtr, ByRef lpwndpl As WINDOWPLACEMENT) _
            As Boolean

#End Region

#Region "Structures"

        Private Structure WINDOWPLACEMENT
            Public length As UInteger
            Public flags As UInteger
            Public showCmd As UInteger
            Public ptMinPosition As POINT
            Public ptMaxPosition As POINT
            Public rcNormalPosition As RECT
        End Structure

        Private Structure POINT
            Public x As Long
            Public y As Long
        End Structure

        Private Structure RECT
            Public left As Long
            Public top As Long
            Public right As Long
            Public bottom As Long
        End Structure

#End Region

#Region "Constants"
        ' WINDOWPLACEMENT.flags values
        Private Const WPF_SETMINPOSITION As UInteger = &H1
        Private Const WPF_RESTORETOMAXIMIZED As UInteger = &H2
        Private Const WPF_ASYNCWINDOWPLACEMENT As UInteger = &H4

        ' WINDOWPLACEMENT.showCmd values
        Private Const SW_HIDE As UInteger = 0
        Private Const SW_SHOWNORMAL As UInteger = 1
        Private Const SW_SHOWMINIMIZED As UInteger = 2
        Private Const SW_SHOWMAXIMIZED As UInteger = 3
        Private Const SW_MAXIMIZE As UInteger = 3
        Private Const SW_SHOWNOACTIVATE As UInteger = 4
        Private Const SW_SHOW As UInteger = 5
        Private Const SW_MINIMIZE As UInteger = 6
        Private Const SW_SHOWMINNOACTIVE As UInteger = 7
        Private Const SW_SHOWNA As UInteger = 8
        Private Const SW_RESTORE As UInteger = 9

#End Region

#End Region

#Region "Private Variables"

        Private _form As Form

#End Region

#Region "Constructors/Destructors"

        Public Sub New(ByRef form As Form)
            _form = form
        End Sub

#End Region

#Region "Public Properties"

        Public Property Form As Form
            Get
                Return _form
            End Get
            Set
                _form = value
            End Set
        End Property

        Public Property RestoreToMaximized As Boolean
            Get
                Dim windowPlacement As WINDOWPLACEMENT = GetWindowPlacement()
                Return windowPlacement.flags And WPF_RESTORETOMAXIMIZED
            End Get
            Set
                Dim windowPlacement As WINDOWPLACEMENT = GetWindowPlacement()
                If value Then
                    windowPlacement.flags = windowPlacement.flags Or WPF_RESTORETOMAXIMIZED
                Else
                    windowPlacement.flags = windowPlacement.flags And Not WPF_RESTORETOMAXIMIZED
                End If
                SetWindowPlacement(windowPlacement)
            End Set
        End Property

#End Region

#Region "Private Functions"

        Private Function GetWindowPlacement() As WINDOWPLACEMENT
            If _form Is Nothing Then
                Throw New NullReferenceException("WindowPlacement.Form is not set.")
            End If
            Dim windowPlacement As WINDOWPLACEMENT
            windowPlacement.length = Marshal.SizeOf(windowPlacement)
            Try
                GetWindowPlacement(_form.Handle, windowPlacement)
                Return windowPlacement
            Catch ex As Exception
                Throw
            End Try
        End Function

        Private Function SetWindowPlacement(windowPlacement As WINDOWPLACEMENT) As Boolean
            If _form Is Nothing Then
                Throw New NullReferenceException("WindowPlacement.Form is not set.")
            End If
            windowPlacement.length = Marshal.SizeOf(windowPlacement)
            Try
                Return SetWindowPlacement(_form.Handle, windowPlacement)
            Catch ex As Exception
                Throw
            End Try
        End Function

#End Region
    End Class
End Namespace