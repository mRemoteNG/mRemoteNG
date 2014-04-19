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
                Catch ex As Exception

                End Try
            End Sub

            Private Sub txtSSHCommand_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSSHCommand.KeyDown
                'System.Console.WriteLine("e.KeyCode:" + Convert.ToString(e.KeyCode) + "e.Alt:" + Convert.ToString(e.Alt) + "e.Control:" + Convert.ToString(e.Control) + "e.Modifiers:" + Convert.ToString(e.Modifiers))
                If processHandlers Is Nothing Then
                    e.SuppressKeyPress = True
                    Return
                End If

                If e.KeyCode = Keys.Up Or e.KeyCode = Keys.Down Then
                    e.SuppressKeyPress = True
                    Dim lastCommand As String = ""
                    If lstCommands.SelectedIndex = lstCommands.Items.Count Then
                        lastCommand = lstCommands.Items(lstCommands.Items.Count)
                    End If
                    If e.KeyCode = Keys.Up And lstCommands.SelectedIndex - 1 > -1 And lstCommands.SelectedItem = txtSSHCommand.Text Then
                        lstCommands.SelectedIndex = lstCommands.SelectedIndex - 1
                    End If
                    If e.KeyCode = Keys.Down And lstCommands.SelectedIndex + 1 < lstCommands.Items.Count Then
                        lstCommands.SelectedIndex = lstCommands.SelectedIndex + 1
                    End If
                    lastCommand = lstCommands.SelectedItem
                    txtSSHCommand.Text = lastCommand
                    txtSSHCommand.Select(txtSSHCommand.TextLength, 0)
                End If

                If e.Control = True And e.KeyCode <> Keys.V And e.Alt = False Then
                    sendAllKey(Win32.WM_KEYDOWN, e.KeyValue)
                End If

                If e.KeyCode = Keys.Enter Then
                    ''TODO: before sending any command ctrl+u will be nice
                    'sendAllKey(Win32.WM_CHAR, Keys.Control)
                    'sendAllKey(Win32.WM_CHAR, Keys.U)
                    Dim strLine As String = txtSSHCommand.Text
                    For Each chr1 As Char In strLine
                        Dim keyData As Byte = Convert.ToByte(chr1)
                        sendAllKey(Win32.WM_CHAR, keyData)
                    Next
                    sendAllKey(Win32.WM_KEYDOWN, Keys.Enter)
                End If
            End Sub

            Private Sub gotoEndOfText()
                If txtSSHCommand.Text.Trim() <> "" Then
                    lstCommands.Items.Add(txtSSHCommand.Text.Trim())
                End If
                lstCommands.SelectedIndex = lstCommands.Items.Count - 1
                txtSSHCommand.Clear()
            End Sub
            Private Sub sendAllKey(keyType As Integer, keyData As IntPtr)
                If processHandlers Is Nothing Then
                    Return
                End If
                For Each proc1 As PuttyBase In processHandlers
                    Win32.PostMessage(proc1.PuttyHandle, keyType, keyData, 0)
                Next
            End Sub

            Private Sub txtSSHCommand_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSSHCommand.KeyUp
                If e.KeyCode = Keys.Enter Then
                    gotoEndOfText()
                End If
            End Sub

            Private Sub txtSSHCommand_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles txtSSHCommand.PreviewKeyDown
                ''TODO: if putty out can be read then we can take that string to the scene
                'If e.KeyCode = Keys.Tab Then
                '    Dim strLine As String = txtSSHCommand.Text
                '    For Each chr1 As Char In strLine
                '        Dim keyData As Byte = Convert.ToByte(chr1)
                '        sendAllKey(Win32.WM_CHAR, keyData)
                '    Next
                '    sendAllKey(Win32.WM_KEYDOWN, e.KeyValue)
                'End If
            End Sub
        End Class
    End Namespace
End Namespace