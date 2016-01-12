﻿Imports System.Runtime.InteropServices
Imports System.Text
Imports mRemoteNG.My

Namespace Tools
    Public Class ProcessController
#Region "Public Methods"
        Public Function Start(ByVal fileName As String, Optional ByVal arguments As CommandLineArguments = Nothing) As Boolean
            With Process.StartInfo
                .UseShellExecute = False
                .FileName = fileName
                If arguments IsNot Nothing Then .Arguments = arguments.ToString()
            End With

            If Not Process.Start() Then Return False
            GetMainWindowHandle()

            Return True
        End Function

        Public Function SetControlVisible(ByVal className As String, ByVal text As String, Optional ByVal visible As Boolean = True) As Boolean
            If Process Is Nothing OrElse Process.HasExited Then Return False
            If Handle = IntPtr.Zero Then Return False

            Dim controlHandle As IntPtr = GetControlHandle(className, text)
            If controlHandle = IntPtr.Zero Then Return False

            Dim nCmdShow As Integer
            If visible Then
                nCmdShow = Win32.SW_SHOW
            Else
                nCmdShow = Win32.SW_HIDE
            End If

            Win32.ShowWindow(controlHandle, nCmdShow)

            Return True
        End Function

        Public Function SetControlText(ByVal className As String, ByVal oldText As String, ByVal newText As String) As Boolean
            If Process Is Nothing OrElse Process.HasExited Then Return False
            If Handle = IntPtr.Zero Then Return False

            Dim controlHandle As IntPtr = GetControlHandle(className, oldText)
            If controlHandle = IntPtr.Zero Then Return False

            Dim result As IntPtr = Win32.SendMessage(controlHandle, Win32.WM_SETTEXT, 0, New StringBuilder(newText))
            If Not result.ToInt32() = Win32.TRUE Then Return False

            Return True
        End Function

        Public Function SelectListBoxItem(ByVal itemText As String) As Boolean
            If Process Is Nothing OrElse Process.HasExited Then Return False
            If Handle = IntPtr.Zero Then Return False

            Dim listBoxHandle As IntPtr = GetControlHandle("ListBox")
            If listBoxHandle = IntPtr.Zero Then Return False

            Dim result As IntPtr = Win32.SendMessage(listBoxHandle, Win32.LB_SELECTSTRING, -1, New StringBuilder(itemText))
            If result.ToInt32() = Win32.LB_ERR Then Return False

            Return True
        End Function

        Public Function ClickButton(ByVal text As String) As Boolean
            If Process Is Nothing OrElse Process.HasExited Then Return False
            If Handle = IntPtr.Zero Then Return False

            Dim buttonHandle As IntPtr = GetControlHandle("Button", text)
            If buttonHandle = IntPtr.Zero Then Return False

            Dim buttonControlId As Integer = Win32.GetDlgCtrlID(buttonHandle)
            Win32.SendMessage(Handle, Win32.WM_COMMAND, buttonControlId, buttonHandle)

            Return True
        End Function

        Public Sub WaitForExit()
            If Process Is Nothing OrElse Process.HasExited Then Return
            Process.WaitForExit()
        End Sub
#End Region

#Region "Protected Fields"
        Protected Process As New Process
        Protected Handle As IntPtr = IntPtr.Zero
        Protected Controls As New List(Of IntPtr)
#End Region

#Region "Protected Methods"
        Protected Function GetMainWindowHandle() As IntPtr
            If Process Is Nothing OrElse Process.HasExited Then Return IntPtr.Zero

            Process.WaitForInputIdle(Settings.MaxPuttyWaitTime * 1000)

            Handle = IntPtr.Zero
            Dim startTicks As Integer = Environment.TickCount
            While Handle = IntPtr.Zero And Environment.TickCount < startTicks + (Settings.MaxPuttyWaitTime * 1000)
                Process.Refresh()
                Handle = Process.MainWindowHandle
                If Handle = IntPtr.Zero Then Threading.Thread.Sleep(0)
            End While

            Return Handle
        End Function

        Protected Function GetControlHandle(ByVal className As String, Optional ByVal text As String = "") As IntPtr
            If Process Is Nothing OrElse Process.HasExited Then Return IntPtr.Zero
            If Handle = IntPtr.Zero Then Return IntPtr.Zero

            If Controls.Count = 0 Then
                Controls = EnumWindows.EnumChildWindows(Handle)
            End If

            Dim stringBuilder As New System.Text.StringBuilder
            Dim controlHandle As IntPtr = IntPtr.Zero
            For Each control As IntPtr In Controls
                Win32.GetClassName(control, stringBuilder, stringBuilder.Capacity)
                If (stringBuilder.ToString() = className) Then
                    If String.IsNullOrEmpty(text) Then
                        controlHandle = control
                        Exit For
                    Else
                        Win32.SendMessage(control, Win32.WM_GETTEXT, stringBuilder.Capacity, stringBuilder)
                        If (stringBuilder.ToString() = text) Then
                            controlHandle = control
                            Exit For
                        End If
                    End If
                End If
            Next

            Return controlHandle
        End Function
#End Region

#Region "Win32"
        ' ReSharper disable ClassNeverInstantiated.Local
        Private Class Win32
            ' ReSharper restore ClassNeverInstantiated.Local
            ' ReSharper disable InconsistentNaming
            ' ReSharper disable UnusedMethodReturnValue.Local
            <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
            Public Shared Sub GetClassName(ByVal hWnd As IntPtr, ByVal lpClassName As System.Text.StringBuilder, ByVal nMaxCount As Integer)
            End Sub

            <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
            Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
            End Function

            <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
            Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As System.Text.StringBuilder) As IntPtr
            End Function

            <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
            Public Shared Function GetDlgCtrlID(ByVal hwndCtl As Integer) As Integer
            End Function

            <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
            Public Shared Function ShowWindow(ByVal hWnd As IntPtr, ByVal nCmdShow As Integer) As Boolean
            End Function

            Public Const LB_ERR As Integer = -1
            Public Const LB_SELECTSTRING As Integer = &H18C

            Public Const WM_SETTEXT As Integer = &HC
            Public Const WM_GETTEXT As Integer = &HD
            Public Const WM_COMMAND As Integer = &H111

            Public Const SW_HIDE As Integer = 0
            Public Const SW_SHOW As Integer = 5

            Public Const [TRUE] As Integer = 1
            ' ReSharper restore UnusedMethodReturnValue.Local
            ' ReSharper restore InconsistentNaming
        End Class
#End Region
    End Class
End Namespace

