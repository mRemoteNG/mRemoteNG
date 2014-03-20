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
                txtSSHCommand.ContextMenu = New ContextMenu()
            End Sub

            Private Sub txtSSHCommand_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSSHCommand.KeyDown
                If e.KeyCode = Keys.Up Then
                    e.SuppressKeyPress = True
                End If
                If e.KeyCode = Keys.Back Then
                    Dim index As Integer = txtSSHCommand.SelectionStart
                    Dim currentLine As Integer = txtSSHCommand.GetLineFromCharIndex(index)
                    Dim currentColumn As Integer = index - txtSSHCommand.GetFirstCharIndexFromLine(currentLine)
                    If currentColumn <> 1 Then
                        e.SuppressKeyPress = True
                    End If
                End If
                For Each _connection As mRemoteNG.Connection.Info In mRemoteNG.App.Runtime.ConnectionList
                    For Each _base As mRemoteNG.Connection.Protocol.Base In _connection.OpenConnections
                        If _base.InterfaceControl.Info.Protocol = mRemoteNG.Connection.Protocol.Protocols.SSH2 Then
                            Dim _processHandler As mRemoteNG.Connection.Protocol.SSH2 = _base
                            Win32.PostMessage(_processHandler.PuttyHandle, Win32.WM_KEYDOWN, e.KeyData, 0)
                        End If
                    Next
                Next
                For Each _connection As mRemoteNG.Connection.Info In mRemoteNG.App.Runtime.ConnectionList
                    For Each _base As mRemoteNG.Connection.Protocol.Base In _connection.OpenConnections
                        If _base.InterfaceControl.Info.Protocol = mRemoteNG.Connection.Protocol.Protocols.SSH1 Then
                            Dim _processHandler As mRemoteNG.Connection.Protocol.SSH1 = _base
                            Win32.PostMessage(_processHandler.PuttyHandle, Win32.WM_KEYDOWN, e.KeyData, 0)
                        End If
                    Next
                Next
            End Sub

        End Class
    End Namespace
End Namespace