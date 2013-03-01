Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports mRemoteNG.Messages
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
                    _isPuttyNg = IsFilePuttyNg(PuttyPath)

                    PuttyProcess = New Process
                    With PuttyProcess.StartInfo
                        .FileName = _PuttyPath

                        Select Case _PuttyProtocol
                            Case Putty_Protocol.raw
                                .Arguments = "-load " & """" & PuttyEscapeArgument(InterfaceControl.Info.PuttySession) & """" & " -" & _PuttyProtocol.ToString & " -P " & InterfaceControl.Info.Port & " """ & InterfaceControl.Info.Hostname & """"
                            Case Putty_Protocol.rlogin
                                .Arguments = "-load " & """" & PuttyEscapeArgument(InterfaceControl.Info.PuttySession) & """" & " -" & _PuttyProtocol.ToString & " -P " & InterfaceControl.Info.Port & " """ & InterfaceControl.Info.Hostname & """"
                            Case Putty_Protocol.ssh
                                Dim userArgument As String = ""
                                Dim passwordArgument As String = ""

                                If My.Settings.EmptyCredentials = "windows" Then
                                    userArgument = " -l """ & Environment.UserName & """"
                                ElseIf My.Settings.EmptyCredentials = "custom" Then
                                    userArgument = " -l """ & My.Settings.DefaultUsername & """"
                                    passwordArgument = " -pw """ & PuttyEscapeArgument(Security.Crypt.Decrypt(My.Settings.DefaultPassword, App.Info.General.EncryptionKey)) & """"
                                End If

                                If InterfaceControl.Info.Username <> "" Then
                                    userArgument = " -l """ & InterfaceControl.Info.Username & """"
                                End If

                                If InterfaceControl.Info.Password <> "" Then
                                    passwordArgument = " -pw """ & PuttyEscapeArgument(InterfaceControl.Info.Password) & """"
                                End If

                                .Arguments = "-load " & """" & PuttyEscapeArgument(InterfaceControl.Info.PuttySession) & """" & " -" & _PuttyProtocol.ToString & " -" & _PuttySSHVersion & userArgument & passwordArgument & " -P " & InterfaceControl.Info.Port & " """ & InterfaceControl.Info.Hostname & """"
                            Case Putty_Protocol.telnet
                                .Arguments = "-load " & """" & PuttyEscapeArgument(InterfaceControl.Info.PuttySession) & """" & " -" & _PuttyProtocol.ToString & " -P " & InterfaceControl.Info.Port & " """ & InterfaceControl.Info.Hostname & """"
                            Case Putty_Protocol.serial
                                .Arguments = "-load " & """" & PuttyEscapeArgument(InterfaceControl.Info.PuttySession) & """" & " -" & _PuttyProtocol.ToString & " -P " & InterfaceControl.Info.Port & " """ & InterfaceControl.Info.Hostname & """"
                        End Select

                        If _isPuttyNg Then
                            .Arguments = .Arguments & " -hwndparent " & InterfaceControl.Handle.ToString()
                        End If

                        'REMOVE IN RELEASE!
                        'MessageCollector.AddMessage(MessageClass.InformationMsg, "PuTTY Arguments: " & .Arguments, True)
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

            ' Due to the way PuTTY handles command line arguments, backslashes followed by a quotation mark will be removed.
            ' Since all the strings we send to PuTTY are surrounded by quotation marks, we need to escape any trailing
            ' backslashes by adding another. See split_into_argv() in WINDOWS\WINUTILS.C of the PuTTY source for more info.
            Private Shared Function PuttyEscapeArgument(ByVal argument As String) As String
                If argument.EndsWith("\") Then argument = argument & "\"
                Return argument
            End Function

            Public Overrides Sub Focus()
                Try
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
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, My.Language.strPuttyGetSessionsFailed & vbNewLine & ex.Message, True)
                    Return Nothing
                End Try
            End Function

            Public Shared Function IsFilePuttyNg(file As String) As Boolean
                Dim isPuttyNg As Boolean
                Try
                    isPuttyNg = FileVersionInfo.GetVersionInfo(file).InternalName.Contains("PuTTYNG")
                Catch
                    isPuttyNg = False
                End Try
                Return isPuttyNg
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
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strPuttyStartFailed & vbNewLine & ex.Message, True)
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
