Imports System.Threading
Imports mRemoteNG.My
Imports WeifenLuo.WinFormsUI.Docking
Imports mRemoteNG.App.Runtime
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows
Imports mRemoteNG.Tools.ProcessController

Namespace UI
    Namespace Window
        Public Class SSHCommands
            Inherits Base
#Region "Private Fields"
            Private lastKeyEvent As KeyEventArgs = New KeyEventArgs(Keys.BrowserBack)
#End Region

#Region "Public Methods"
            Public Sub New(ByVal panel As DockContent)
                WindowType = Type.SSHCommands
                DockPnl = panel
                InitializeComponent()
            End Sub
#End Region
            Private Sub SSHCommands_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load


            End Sub

            Private Sub txtSSHCommand_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSSHCommand.KeyDown
                System.Console.WriteLine("keycode:" + Str(e.KeyCode) + "-keydata:" + Str(e.KeyData) + "keyvalue:" + Str(e.KeyValue) + "-modidfiers:" + Str(e.Modifiers))
                If e.Alt And e.KeyValue = Keys.Alt Then Return
                If e.Control And e.KeyValue = Keys.Control Then Return
                If e.Shift And e.KeyValue = Keys.ShiftKey Then Return

               

                For Each _connection As mRemoteNG.Connection.Info In mRemoteNG.App.Runtime.ConnectionList
                    For Each _base As mRemoteNG.Connection.Protocol.Base In _connection.OpenConnections
                        If _base.InterfaceControl.Info.Protocol = mRemoteNG.Connection.Protocol.Protocols.SSH2 Then
                            Dim bb As mRemoteNG.Connection.Protocol.SSH2 = _base
                            If e.KeyCode = Keys.Oem102 Then
                                Win32.SendMessage(bb.PuttyHandle, Win32.WM_CHAR, Keys.OemPipe, 0)
                            End If
                            If Not e.Shift Then
                                Win32.SendMessage(bb.PuttyHandle, Win32.WM_KEYDOWN, e.KeyData, 0)
                            Else
                                Win32.SendMessage(bb.PuttyHandle, Win32.WM_CHAR, e.KeyData, 0)
                            End If
                        End If
                    Next
                Next
                For Each _connection As mRemoteNG.Connection.Info In mRemoteNG.App.Runtime.ConnectionList
                    For Each _base As mRemoteNG.Connection.Protocol.Base In _connection.OpenConnections
                        If _base.InterfaceControl.Info.Protocol = mRemoteNG.Connection.Protocol.Protocols.SSH1 Then
                            Dim bb As mRemoteNG.Connection.Protocol.SSH1 = _base
                            If Not e.Shift Then
                                Win32.SendMessage(bb.PuttyHandle, Win32.WM_KEYDOWN, e.KeyData, 0)
                            Else
                                Win32.SendMessage(bb.PuttyHandle, Win32.WM_CHAR, e.KeyData, 0)
                            End If
                        End If
                    Next
                Next

            End Sub

            Private Sub txtSSHCommand_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSSHCommand.KeyUp
                'If Not lastKeyEvent.Equals(e) Then
                '    For Each _connection As mRemoteNG.Connection.Info In mRemoteNG.App.Runtime.ConnectionList
                '        For Each _base As mRemoteNG.Connection.Protocol.Base In _connection.OpenConnections
                '            If _base.InterfaceControl.Info.Protocol = mRemoteNG.Connection.Protocol.Protocols.SSH2 Then
                '                Dim bb As mRemoteNG.Connection.Protocol.SSH2 = _base
                '                Win32.SendMessage(bb.PuttyHandle, Win32.WM_KEYUP, e.KeyData, 0)
                '            End If
                '        Next
                '    Next
                '    For Each _connection As mRemoteNG.Connection.Info In mRemoteNG.App.Runtime.ConnectionList
                '        For Each _base As mRemoteNG.Connection.Protocol.Base In _connection.OpenConnections
                '            If _base.InterfaceControl.Info.Protocol = mRemoteNG.Connection.Protocol.Protocols.SSH1 Then
                '                Dim bb As mRemoteNG.Connection.Protocol.SSH1 = _base
                '                Win32.SendMessage(bb.PuttyHandle, Win32.WM_KEYUP, e.KeyData, 0)
                '            End If
                '        Next
                '    Next
                'End If
                'lastKeyEvent = e
            End Sub
        End Class
    End Namespace
End Namespace