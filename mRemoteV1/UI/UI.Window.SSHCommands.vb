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
            Private _getSessionsThread As Thread
            Private _retrieved As Boolean = False
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

            Private Sub TextBox1_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSSHCommand.KeyUp
                If e.KeyCode = Keys.Enter Then
                    e.SuppressKeyPress = True

                    For Each _connection As mRemoteNG.Connection.Info In mRemoteNG.App.Runtime.ConnectionList
                        For Each _base As mRemoteNG.Connection.Protocol.Base In _connection.OpenConnections
                            If _base.InterfaceControl.Info.Protocol = mRemoteNG.Connection.Protocol.Protocols.SSH1 Or _
                                _base.InterfaceControl.Info.Protocol = mRemoteNG.Connection.Protocol.Protocols.SSH2 Then
                                Dim bb As mRemoteNG.Connection.Protocol.SSH2 = _base
                                Dim line As Integer = txtSSHCommand.GetLineFromCharIndex(txtSSHCommand.SelectionStart)
                                Dim strLine As String = txtSSHCommand.Lines(line - 1)

                                For Each ch As Char In strLine
                                    Dim val As System.Windows.Forms.Keys = DirectCast([Enum].Parse(GetType(Keys), ch.ToString().ToUpper()), Keys)
                                    Win32.SendMessage(bb.PuttyHandle, Win32.WM_KEYDOWN, val, 0)
                                Next

                                Win32.SendMessage(bb.PuttyHandle, Win32.WM_KEYDOWN, Keys.Enter, 0)

                            End If
                        Next
                    Next
                End If
            End Sub

        End Class
    End Namespace
End Namespace