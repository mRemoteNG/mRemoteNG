Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports mRemoteNG.Messages
Imports mRemoteNG.App.Native
Imports System.Threading
Imports Microsoft.Win32
Imports System.Drawing
Imports mRemoteNG.App.Runtime
Imports System.Text.RegularExpressions
Imports mRemoteNG.Tools

Namespace Connection
    Namespace Protocol
        Public Class PuttyBase
            Inherits Connection.Protocol.Base

#Region "Constants"
            Private Const IDM_RECONF As Int32 = &H50 ' PuTTY Settings Menu ID
#End Region

#Region "Private Properties"
            Dim _isPuttyNg As Boolean
#End Region

#Region "Public Properties"
            Private _PuttyProtocol As Putty_Protocol
            Public Property PuttyProtocol() As Putty_Protocol
                Get
                    Return Me._PuttyProtocol
                End Get
                Set(ByVal value As Putty_Protocol)
                    Me._PuttyProtocol = value
                End Set
            End Property

            Private _PuttySSHVersion As Putty_SSHVersion
            Public Property PuttySSHVersion() As Putty_SSHVersion
                Get
                    Return Me._PuttySSHVersion
                End Get
                Set(ByVal value As Putty_SSHVersion)
                    Me._PuttySSHVersion = value
                End Set
            End Property

            Private _PuttyHandle As IntPtr
            Public Property PuttyHandle() As IntPtr
                Get
                    Return Me._PuttyHandle
                End Get
                Set(ByVal value As IntPtr)
                    Me._PuttyHandle = value
                End Set
            End Property

            Private _PuttyProcess As Process
            Public Property PuttyProcess() As Process
                Get
                    Return Me._PuttyProcess
                End Get
                Set(ByVal value As Process)
                    Me._PuttyProcess = value
                End Set
            End Property

            Private Shared _PuttyPath As String
            Public Shared Property PuttyPath() As String
                Get
                    Return _PuttyPath
                End Get
                Set(ByVal value As String)
                    _PuttyPath = value
                End Set
            End Property

            Public ReadOnly Property Focused() As Boolean
                Get
                    If GetForegroundWindow() = PuttyHandle Then
                        Return True
                    Else
                        Return False
                    End If
                End Get
            End Property
#End Region

#Region "Private Events & Handlers"
            Private Sub ProcessExited(ByVal sender As Object, ByVal e As System.EventArgs)
                MyBase.Event_Closed(Me)
            End Sub
#End Region

#Region "Public Methods"
            Public Sub New()

            End Sub

            Public Overrides Function Connect() As Boolean
                Try
                    _isPuttyNg = IsFilePuttyNg(PuttyPath)

                    PuttyProcess = New Process
                    With PuttyProcess.StartInfo
                        .UseShellExecute = False
                        .FileName = _PuttyPath

                        Dim arguments As New CommandLineArguments
                        arguments.EscapeForShell = False

                        arguments.Add("-load", InterfaceControl.Info.PuttySession)
                        arguments.Add("-" & _PuttyProtocol.ToString)

                        If _PuttyProtocol = Putty_Protocol.ssh Then
                            Dim username As String = ""
                            Dim password As String = ""

                            If Not String.IsNullOrEmpty(InterfaceControl.Info.Username) Then
                                username = InterfaceControl.Info.Username
                            Else
                                If My.Settings.EmptyCredentials = "windows" Then
                                    username = Environment.UserName
                                ElseIf My.Settings.EmptyCredentials = "custom" Then
                                    username = My.Settings.DefaultUsername
                                End If
                            End If

                            If Not String.IsNullOrEmpty(InterfaceControl.Info.Password) Then
                                password = InterfaceControl.Info.Password
                            Else
                                If My.Settings.EmptyCredentials = "custom" Then
                                    password = Security.Crypt.Decrypt(My.Settings.DefaultPassword, App.Info.General.EncryptionKey)
                                End If
                            End If

                            arguments.Add("-" & _PuttySSHVersion)
                            arguments.Add("-l", username)
                            arguments.Add("-pw", password)
                        End If

                        arguments.Add("-P", InterfaceControl.Info.Port.ToString)
                        arguments.Add(InterfaceControl.Info.Hostname)

                        If _isPuttyNg Then
                            arguments.Add("-hwndparent", InterfaceControl.Handle.ToString())
                        End If

                        .Arguments = arguments.ToString
                    End With

                    PuttyProcess.EnableRaisingEvents = True
                    AddHandler PuttyProcess.Exited, AddressOf ProcessExited

                    PuttyProcess.Start()
                    PuttyProcess.WaitForInputIdle(My.Settings.MaxPuttyWaitTime * 1000)

                    Dim startTicks As Integer = Environment.TickCount
                    While PuttyHandle.ToInt32 = 0 And Environment.TickCount < startTicks + (My.Settings.MaxPuttyWaitTime * 1000)
                        If _isPuttyNg Then
                            PuttyHandle = FindWindowEx(InterfaceControl.Handle, 0, vbNullString, vbNullString)
                        Else
                            PuttyProcess.Refresh()
                            PuttyHandle = PuttyProcess.MainWindowHandle
                        End If
                        If PuttyHandle.ToInt32 = 0 Then Thread.Sleep(0)
                    End While

                    If Not _isPuttyNg Then
                        SetParent(PuttyHandle, InterfaceControl.Handle)
                    End If

                    MessageCollector.AddMessage(MessageClass.InformationMsg, My.Language.strPuttyStuff, True)

                    MessageCollector.AddMessage(MessageClass.InformationMsg, String.Format(My.Language.strPuttyHandle, PuttyHandle.ToString), True)
                    MessageCollector.AddMessage(MessageClass.InformationMsg, String.Format(My.Language.strPuttyTitle, PuttyProcess.MainWindowTitle), True)
                    MessageCollector.AddMessage(MessageClass.InformationMsg, String.Format(My.Language.strPuttyParentHandle, InterfaceControl.Parent.Handle.ToString), True)

                    Resize(Me, New EventArgs)

                    MyBase.Connect()
                    Return True
                Catch ex As Exception
                    MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strPuttyConnectionFailed & vbNewLine & ex.Message)
                    Return False
                End Try
            End Function

            Public Overrides Sub Focus()
                Try
                    If ConnectionWindow.InTabDrag Then Return
                    SetForegroundWindow(PuttyHandle)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strPuttyFocusFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Overrides Sub Resize(ByVal sender As Object, ByVal e As EventArgs)
                Try
                    If InterfaceControl.Size = Size.Empty Then Return
                    MoveWindow(PuttyHandle, -SystemInformation.FrameBorderSize.Width, -(SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height), InterfaceControl.Width + (SystemInformation.FrameBorderSize.Width * 2), InterfaceControl.Height + SystemInformation.CaptionHeight + (SystemInformation.FrameBorderSize.Height * 2), True)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strPuttyResizeFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Overrides Sub Close()
                Try
                    If PuttyProcess.HasExited = False Then
                        PuttyProcess.Kill()
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strPuttyKillFailed & vbNewLine & ex.Message, True)
                End Try

                Try
                    PuttyProcess.Dispose()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strPuttyDisposeFailed & vbNewLine & ex.Message, True)
                End Try

                MyBase.Close()
            End Sub

            Public Sub ShowSettingsDialog()
                Try
                    PostMessage(Me.PuttyHandle, WM_SYSCOMMAND, IDM_RECONF, 0)
                    SetForegroundWindow(Me.PuttyHandle)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strPuttyShowSettingsDialogFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Public Shared Methods"
            Public Shared Function IsFilePuttyNg(file As String) As Boolean
                Dim isPuttyNg As Boolean
                Try
                    isPuttyNg = FileVersionInfo.GetVersionInfo(file).InternalName.Contains("PuTTYNG")
                Catch
                    isPuttyNg = False
                End Try
                Return isPuttyNg
            End Function
#End Region

#Region "Enums"
            Public Enum Putty_Protocol
                ssh = 0
                telnet = 1
                rlogin = 2
                raw = 3
                serial = 4
            End Enum

            Public Enum Putty_SSHVersion
                ssh1 = 1
                ssh2 = 2
            End Enum
#End Region
        End Class
    End Namespace
End Namespace
