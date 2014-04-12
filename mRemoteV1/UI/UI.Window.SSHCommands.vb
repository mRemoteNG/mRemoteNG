Imports System.Threading
Imports mRemoteNG.My
Imports WeifenLuo.WinFormsUI.Docking
Imports mRemoteNG.App.Runtime
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows
Imports mRemoteNG.Tools.ProcessController
Imports mRemoteNG.Connection.Protocol

Namespace UI
    Namespace Window
        Public Class SSHCommands
            Inherits UI.Window.Base
#Region "Private Fields"
            Private processHandlers() As PuttyBase = Nothing
#End Region

#Region "Public Methods"
            Public Sub New(ByVal panel As DockContent)
                WindowType = Type.SSHCommands
                DockPnl = panel
                InitializeComponent()
            End Sub
#End Region
            Private Sub SSHCommands_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
                'txtSSHCommand.ContextMenu = New ContextMenu()
                Control.CheckForIllegalCrossThreadCalls = False
            End Sub

            Private Sub txtSSHCommand_GotFocus(sender As Object, e As EventArgs) Handles txtSSHCommand.GotFocus
                Try
                    Dim count As Integer

                    For Each _connection As mRemoteNG.Connection.Info In mRemoteNG.App.Runtime.ConnectionList
                        For Each _base As mRemoteNG.Connection.Protocol.Base In _connection.OpenConnections
                            If _base.GetType().IsSubclassOf(GetType(mRemoteNG.Connection.Protocol.PuttyBase)) Then
                                Dim _processHandler As mRemoteNG.Connection.Protocol.SSH2 = _base
                                count = count + 1
                            End If
                        Next
                    Next
                    If count > 0 Then
                        ReDim processHandlers(count - 1)
                        Dim index As Integer
                        For Each _connection As mRemoteNG.Connection.Info In mRemoteNG.App.Runtime.ConnectionList
                            For Each _base As mRemoteNG.Connection.Protocol.Base In _connection.OpenConnections
                                If _base.GetType().IsSubclassOf(GetType(mRemoteNG.Connection.Protocol.PuttyBase)) Then
                                    processHandlers(index) = _base
                                    index = index + 1
                                End If
                            Next
                        Next
                    End If
                    gotoEndOfText()
                Catch ex As Exception

                End Try
            End Sub

            Private Sub txtSSHCommand_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSSHCommand.KeyDown
                If processHandlers Is Nothing Then
                    e.SuppressKeyPress = True
                    Return
                End If

                If e.Modifiers = Keys.Control AndAlso e.KeyCode = Keys.V Then
                    Dim str As String = Clipboard.GetText(TextDataFormat.Text)
                    If str.Contains(Environment.NewLine) Then
                        gotoEndOfText()
                        e.SuppressKeyPress = True
                        Return
                    End If
                End If

                If e.KeyCode = Keys.Up Or e.KeyCode = Keys.Down Then
                    e.SuppressKeyPress = True
                    sendAllKey(Win32.WM_KEYDOWN, e.KeyValue)
                End If
                If e.Control = True And e.KeyCode <> Keys.V Then
                    sendAllKey(Win32.WM_KEYDOWN, e.KeyValue)
                End If
                If e.KeyCode = Keys.Back Then
                    Dim index As Integer = txtSSHCommand.SelectionStart
                    Dim currentLine As Integer = txtSSHCommand.GetLineFromCharIndex(index)
                    Dim currentColumn As Integer = index - txtSSHCommand.GetFirstCharIndexFromLine(currentLine)
                    If currentColumn = 0 Then
                        e.SuppressKeyPress = True
                    End If
                End If

                If e.KeyCode = Keys.Enter Then
                    If txtSSHCommand.SelectionStart > 0 Then
                        Dim line As Integer = txtSSHCommand.GetLineFromCharIndex(txtSSHCommand.SelectionStart)
                        If line > txtSSHCommand.Lines.Length Then
                            line = txtSSHCommand.Lines.Length - 1
                        End If
                        Dim strLine As String = txtSSHCommand.Lines(line)
                        For Each chr1 As Char In strLine
                            Dim keyData As Byte = Convert.ToByte(chr1)
                            sendAllKey(Win32.WM_CHAR, keyData)
                        Next
                    End If
                    sendAllKey(Win32.WM_KEYDOWN, Keys.Enter)
                    gotoEndOfText()
                End If
            End Sub

            Private Sub txtSSHCommand_MouseClick(sender As Object, e As MouseEventArgs) Handles txtSSHCommand.MouseClick
                gotoEndOfText()
            End Sub

            Private Sub gotoEndOfText()
                txtSSHCommand.Clear()
                txtSSHCommand.Text = "Write the command down" + Environment.NewLine
                Application.DoEvents()
                txtSSHCommand.SelectionStart = txtSSHCommand.TextLength
                txtSSHCommand.SelectionLength = 0
                txtSSHCommand.ScrollToCaret()
            End Sub
            Private Sub sendAllKey(keyType As Integer, keyData As Byte)
                For Each proc1 As PuttyBase In processHandlers
                    Win32.PostMessage(proc1.PuttyHandle, keyType, keyData, 0)
                Next
            End Sub

            Private Sub txtSSHCommand_TextChanged(sender As Object, e As EventArgs) Handles txtSSHCommand.TextChanged

            End Sub

            Private Sub txtSSHCommand_KeyUp(sender As Object, e As KeyEventArgs)
                gotoEndOfText()

            End Sub
        End Class
    End Namespace
End Namespace