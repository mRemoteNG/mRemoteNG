Imports mRemoteNG.App.Native
Imports System.Threading
Imports Microsoft.Win32
Imports System.Drawing
Imports mRemoteNG.App.Runtime

Namespace Connection
    Namespace Protocol
        Public Class PuttyBase
            Inherits Connection.Protocol.Base

#Region "Constants"
            Private Const IDM_RECONF As Int32 = &H50 ' PuTTY Settings Menu ID
#End Region

#Region "Private Properties"
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

            'Private borderWidth As Integer = frmMain.Size.Width - frmMain.ClientSize.Width
            'Private borderHeight As Integer = frmMain.Size.Height - frmMain.ClientSize.Height
            Private Shared _BorderSize As Size
            Public Shared Property BorderSize() As Size
                Get
                    Return _BorderSize
                End Get
                Set(ByVal value As Size)
                    _BorderSize = value
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
                    PuttyProcess = New Process

                    With PuttyProcess.StartInfo
                        .FileName = _PuttyPath

                        Select Case Me._PuttyProtocol
                            Case Putty_Protocol.raw
                                .Arguments = "-load " & """" & Me.InterfaceControl.Info.PuttySession & """" & " -" & Me._PuttyProtocol.ToString & " -P " & Me.InterfaceControl.Info.Port & " """ & Me.InterfaceControl.Info.Hostname & """"
                            Case Putty_Protocol.rlogin
                                .Arguments = "-load " & """" & Me.InterfaceControl.Info.PuttySession & """" & " -" & Me._PuttyProtocol.ToString & " -P " & Me.InterfaceControl.Info.Port & " """ & Me.InterfaceControl.Info.Hostname & """"
                            Case Putty_Protocol.ssh
                                Dim UserArg As String = ""
                                Dim PassArg As String = ""

                                If My.Settings.EmptyCredentials = "windows" Then
                                    UserArg = " -l """ & Environment.UserName & """"
                                ElseIf My.Settings.EmptyCredentials = "custom" Then
                                    UserArg = " -l """ & My.Settings.DefaultUsername & """"
                                    PassArg = " -pw """ & Security.Crypt.Decrypt(My.Settings.DefaultPassword, App.Info.General.EncryptionKey) & """"
                                End If

                                If Me.InterfaceControl.Info.Username <> "" Then
                                    UserArg = " -l """ & Me.InterfaceControl.Info.Username & """"
                                End If

                                If Me.InterfaceControl.Info.Password <> "" Then
                                    PassArg = " -pw """ & Me.InterfaceControl.Info.Password & """"
                                End If

                                .Arguments = "-load " & """" & Me.InterfaceControl.Info.PuttySession & """" & " -" & Me._PuttyProtocol.ToString & " -" & Me._PuttySSHVersion & UserArg & PassArg & " -P " & Me.InterfaceControl.Info.Port & " """ & Me.InterfaceControl.Info.Hostname & """"
                            Case Putty_Protocol.telnet
                                .Arguments = "-load " & """" & Me.InterfaceControl.Info.PuttySession & """" & " -" & Me._PuttyProtocol.ToString & " -P " & Me.InterfaceControl.Info.Port & " """ & Me.InterfaceControl.Info.Hostname & """"
                            Case Putty_Protocol.serial
                                .Arguments = "-load " & """" & Me.InterfaceControl.Info.PuttySession & """" & " -" & Me._PuttyProtocol.ToString & " -P " & Me.InterfaceControl.Info.Port & " """ & Me.InterfaceControl.Info.Hostname & """"
                        End Select

                        'REMOVE IN RELEASE!
                        'mC.AddMessage(Messages.MessageClass.InformationMsg, "PuTTY Arguments: " & .Arguments, True)
                    End With

                    PuttyProcess.EnableRaisingEvents = True
                    AddHandler PuttyProcess.Exited, AddressOf ProcessExited

                    PuttyProcess.Start()
                    PuttyProcess.WaitForInputIdle()

                    Dim TryCount As Integer = 0
                    Do Until PuttyProcess.MainWindowHandle <> IntPtr.Zero And Me.InterfaceControl.Handle <> IntPtr.Zero And Me.PuttyProcess.MainWindowTitle <> "Default IME"
                        If TryCount >= My.Settings.MaxPuttyWaitTime * 2 Then
                            Exit Do
                        End If

                        PuttyProcess.Refresh()

                        Thread.Sleep(500)

                        TryCount += 1
                    Loop

                    PuttyHandle = PuttyProcess.MainWindowHandle


                    MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, My.Resources.strPuttyStuff, True)

                    MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, String.Format(My.Resources.strPuttyHandle, PuttyHandle.ToString), True)
                    MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, String.Format(My.Resources.strPuttyTitle, PuttyProcess.MainWindowTitle), True)
                    MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, String.Format(My.Resources.strPuttyParentHandle, Me.InterfaceControl.Parent.Handle.ToString), True)

                    SetParent(PuttyHandle, InterfaceControl.Parent.Handle)
                    ShowWindow(PuttyHandle, SW_SHOWMAXIMIZED)
                    Resize()

                    MyBase.Connect()
                    Return True
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strPuttyConnectionFailed & vbNewLine & ex.Message)
                    Return False
                End Try
            End Function

            Public Overrides Sub Focus()
                Try
                    'SetForegroundWindow(PuttyHandle)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strPuttyFocusFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Overrides Sub Resize()
                Try
                    MoveWindow(PuttyHandle, -SystemInformation.FrameBorderSize.Width, -(SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height), Me.InterfaceControl.Width + (SystemInformation.FrameBorderSize.Width * 2), Me.InterfaceControl.Height + SystemInformation.CaptionHeight + (SystemInformation.FrameBorderSize.Height * 2), True)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strPuttyResizeFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Overrides Sub Close()
                Try
                    If PuttyProcess.HasExited = False Then
                        PuttyProcess.Kill()
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strPuttyKillFailed & vbNewLine & ex.Message, True)
                End Try

                Try
                    PuttyProcess.Dispose()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strPuttyDisposeFailed & vbNewLine & ex.Message, True)
                End Try

                MyBase.Close()
            End Sub

            Public Sub ShowSettingsDialog()
                Try
                    PostMessage(Me.PuttyHandle, WM_SYSCOMMAND, IDM_RECONF, 0)
                    SetForegroundWindow(Me.PuttyHandle)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strPuttyShowSettingsDialogFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Public Shared Methods"
            Public Shared Function GetSessions() As Array
                Try
                    Dim regKey As RegistryKey
                    regKey = Registry.CurrentUser.OpenSubKey("Software\SimonTatham\PuTTY\Sessions")

                    Dim arrKeys() As String
                    arrKeys = regKey.GetSubKeyNames()
                    Array.Resize(arrKeys, arrKeys.Length + 1)
                    arrKeys(arrKeys.Length - 1) = "Default Settings"

                    For i As Integer = 0 To arrKeys.Length - 1
                        arrKeys(i) = System.Web.HttpUtility.UrlDecode(arrKeys(i))
                    Next

                    Return arrKeys
                Catch ex As Exception
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, My.Resources.strPuttyGetSessionsFailed & vbNewLine & ex.Message, True)
                    Return Nothing
                End Try
            End Function

            Public Shared Sub StartPutty()
                Try
                    Dim p As Process
                    Dim pSI As New ProcessStartInfo
                    pSI.FileName = PuttyPath

                    p = Process.Start(pSI)
                    p.WaitForExit()

                    mRemoteNG.Connection.PuttySession.PuttySessions = GetSessions()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strPuttyStartFailed & vbNewLine & ex.Message, True)
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
                ssh1 = 1
                ssh2 = 2
            End Enum
#End Region

        End Class
    End Namespace
End Namespace
