
Imports System.Threading
Imports mRemote3G.App
Imports mRemote3G.App.Info
Imports mRemote3G.Connection.PuttySession
Imports mRemote3G.Messages
Imports mRemote3G.My
Imports mRemote3G.Security
Imports mRemote3G.Tools

Namespace Connection

    Namespace Protocol
        Public Class PuttyBase
            Inherits Base

#Region "Constants"

            Private Const IDM_RECONF As Int32 = &H50 ' PuTTY Settings Menu ID

#End Region

#Region "Private Properties"

            Dim _isPuttyNg As Boolean

#End Region

#Region "Public Properties"

            Private _PuttyProtocol As Putty_Protocol

            Public Property PuttyProtocol As Putty_Protocol
                Get
                    Return Me._PuttyProtocol
                End Get
                Set
                    Me._PuttyProtocol = value
                End Set
            End Property

            Private _PuttySSHVersion As Putty_SSHVersion

            Public Property PuttySSHVersion As Putty_SSHVersion
                Get
                    Return Me._PuttySSHVersion
                End Get
                Set
                    Me._PuttySSHVersion = value
                End Set
            End Property

            Private _PuttyHandle As IntPtr

            Public Property PuttyHandle As IntPtr
                Get
                    Return Me._PuttyHandle
                End Get
                Set
                    Me._PuttyHandle = value
                End Set
            End Property

            Private _PuttyProcess As Process

            Public Property PuttyProcess As Process
                Get
                    Return Me._PuttyProcess
                End Get
                Set
                    Me._PuttyProcess = value
                End Set
            End Property

            Private Shared _PuttyPath As String

            Public Shared Property PuttyPath As String
                Get
                    Return _PuttyPath
                End Get
                Set
                    _PuttyPath = value
                End Set
            End Property

            Public ReadOnly Property Focused As Boolean
                Get
                    If NativeMethods.GetForegroundWindow() = PuttyHandle Then
                        Return True
                    Else
                        Return False
                    End If
                End Get
            End Property

#End Region

#Region "Private Events & Handlers"

            Private Sub ProcessExited(sender As Object, e As EventArgs)
                MyBase.Event_Closed(Me)
            End Sub

#End Region

#Region "Public Methods"

            Public Sub New()
            End Sub

            Public Overrides Function Connect() As Boolean
                Try
                    _isPuttyNg = (PuttyTypeDetector.GetPuttyType() = PuttyTypeDetector.PuttyType.PuttyNg)

                    PuttyProcess = New Process
                    With PuttyProcess.StartInfo
                        .UseShellExecute = False
                        .FileName = _PuttyPath

                        Dim arguments As New CommandLineArguments
                        arguments.EscapeForShell = False

                        arguments.Add("-load", InterfaceControl.Info.PuttySession)

                        If Not TypeOf InterfaceControl.Info Is PuttyInfo Then
                            arguments.Add("-" & _PuttyProtocol.ToString)

                            If _PuttyProtocol = Putty_Protocol.ssh Then
                                Dim username = ""
                                Dim password = ""

                                If Not String.IsNullOrEmpty(InterfaceControl.Info.Username) Then
                                    username = InterfaceControl.Info.Username
                                Else
                                    If MySettingsProperty.Settings.EmptyCredentials = "windows" Then
                                        username = Environment.UserName
                                    ElseIf MySettingsProperty.Settings.EmptyCredentials = "custom" Then
                                        username = MySettingsProperty.Settings.DefaultUsername
                                    End If
                                End If

                                If Not String.IsNullOrEmpty(InterfaceControl.Info.Password) Then
                                    password = InterfaceControl.Info.Password
                                Else
                                    If MySettingsProperty.Settings.EmptyCredentials = "custom" Then
                                        password = Crypt.Decrypt(MySettingsProperty.Settings.DefaultPassword,
                                                                 General.EncryptionKey)
                                    End If
                                End If

                                arguments.Add("-" & _PuttySSHVersion)

                                If Not ((Force And Info.Force.NoCredentials) = Info.Force.NoCredentials) Then
                                    If Not String.IsNullOrEmpty(username) Then
                                        arguments.Add("-l", username)
                                    End If
                                    If Not String.IsNullOrEmpty(password) Then
                                        arguments.Add("-pw", password)
                                    End If
                                End If
                            End If

                            arguments.Add("-P", InterfaceControl.Info.Port.ToString)
                            arguments.Add(InterfaceControl.Info.Hostname)
                        End If

                        If _isPuttyNg Then
                            arguments.Add("-hwndparent", InterfaceControl.Handle.ToString())
                        End If

                        .Arguments = arguments.ToString
                    End With

                    PuttyProcess.EnableRaisingEvents = True
                    AddHandler PuttyProcess.Exited, AddressOf ProcessExited

                    PuttyProcess.Start()
                    PuttyProcess.WaitForInputIdle(MySettingsProperty.Settings.MaxPuttyWaitTime*1000)

                    Dim startTicks As Integer = Environment.TickCount
                    While _
                        PuttyHandle.ToInt32 = 0 And
                        Environment.TickCount < startTicks + (MySettingsProperty.Settings.MaxPuttyWaitTime*1000)
                        If _isPuttyNg Then
                            PuttyHandle = NativeMethods.FindWindowEx(InterfaceControl.Handle, 0, vbNullString,
                                                                     vbNullString)
                        Else
                            PuttyProcess.Refresh()
                            PuttyHandle = PuttyProcess.MainWindowHandle
                        End If
                        If PuttyHandle.ToInt32 = 0 Then Thread.Sleep(0)
                    End While

                    If Not _isPuttyNg Then
                        NativeMethods.SetParent(PuttyHandle, InterfaceControl.Handle)
                    End If

                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.Language.strPuttyStuff,
                                                        True)

                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                        String.Format(Language.Language.strPuttyHandle,
                                                                      PuttyHandle.ToString), True)
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                        String.Format(Language.Language.strPuttyTitle,
                                                                      PuttyProcess.MainWindowTitle), True)
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                        String.Format(Language.Language.strPuttyParentHandle,
                                                                      InterfaceControl.Parent.Handle.ToString), True)

                    Resize(Me, New EventArgs)

                    MyBase.Connect()
                    Return True
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        Language.Language.strPuttyConnectionFailed & vbNewLine &
                                                        ex.ToString())
                    Return False
                End Try
            End Function

            Public Overrides Sub Focus()
                Try
                    If ConnectionWindow.InTabDrag Then Return
                    NativeMethods.SetForegroundWindow(PuttyHandle)
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        Language.Language.strPuttyFocusFailed & vbNewLine &
                                                        ex.ToString(), True)
                End Try
            End Sub

            Public Overrides Sub Resize(sender As Object, e As EventArgs)
                Try
                    If InterfaceControl.Size = Size.Empty Then Return
                    NativeMethods.MoveWindow(PuttyHandle, -SystemInformation.FrameBorderSize.Width,
                                             -(SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height),
                                             InterfaceControl.Width + (SystemInformation.FrameBorderSize.Width * 2),
                                             InterfaceControl.Height + SystemInformation.CaptionHeight +
                                             (SystemInformation.FrameBorderSize.Height * 2), True)
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        Language.Language.strPuttyResizeFailed & vbNewLine &
                                                        ex.ToString(), True)
                End Try
            End Sub

            Public Overrides Sub Close()
                Try
                    If PuttyProcess.HasExited = False Then
                        PuttyProcess.Kill()
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        Language.Language.strPuttyKillFailed & vbNewLine & ex.ToString(),
                                                        True)
                End Try

                Try
                    PuttyProcess.Dispose()
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        Language.Language.strPuttyDisposeFailed & vbNewLine &
                                                        ex.ToString(), True)
                End Try

                MyBase.Close()
            End Sub

            Public Sub ShowSettingsDialog()
                Try
                    NativeMethods.PostMessage(Me.PuttyHandle, NativeMethods.WM_SYSCOMMAND, IDM_RECONF, 0)
                    NativeMethods.SetForegroundWindow(Me.PuttyHandle)
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        Language.Language.strPuttyShowSettingsDialogFailed & vbNewLine &
                                                        ex.ToString(), True)
                End Try
            End Sub

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
                None = 0
                ssh1 = 1
                ssh2 = 2
            End Enum

#End Region
        End Class
    End Namespace

End Namespace
