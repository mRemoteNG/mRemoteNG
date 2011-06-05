Imports mRemoteNG.App.Runtime
Imports System.Threading
Imports AxWFICALib
Imports mRemoteNG.Tools.LocalizedAttributes

Namespace Connection
    Namespace Protocol
        Public Class ICA
            Inherits Connection.Protocol.Base

#Region "Private Properties"
            Private ICA As AxICAClient
            Private Info As Connection.Info
#End Region

#Region "Public Methods"
            Public Sub New()
                Try
                    Me.Control = New AxICAClient
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strIcaControlFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Overrides Function SetProps() As Boolean
                MyBase.SetProps()

                Try
                    ICA = Me.Control
                    Info = Me.InterfaceControl.Info

                    ICA.CreateControl()

                    Do Until Me.ICA.Created
                        Thread.Sleep(10)
                        System.Windows.Forms.Application.DoEvents()
                    Loop

                    ICA.Address = Info.Hostname

                    Me.SetCredentials()

                    Me.SetResolution()
                    Me.SetColors()

                    Me.SetSecurity()

                    'Disable hotkeys for international users
                    ICA.Hotkey1Shift = Nothing
                    ICA.Hotkey1Char = Nothing
                    ICA.Hotkey2Shift = Nothing
                    ICA.Hotkey2Char = Nothing
                    ICA.Hotkey3Shift = Nothing
                    ICA.Hotkey3Char = Nothing
                    ICA.Hotkey4Shift = Nothing
                    ICA.Hotkey4Char = Nothing
                    ICA.Hotkey5Shift = Nothing
                    ICA.Hotkey5Char = Nothing
                    ICA.Hotkey6Shift = Nothing
                    ICA.Hotkey6Char = Nothing
                    ICA.Hotkey7Shift = Nothing
                    ICA.Hotkey7Char = Nothing
                    ICA.Hotkey8Shift = Nothing
                    ICA.Hotkey8Char = Nothing
                    ICA.Hotkey9Shift = Nothing
                    ICA.Hotkey9Char = Nothing
                    ICA.Hotkey10Shift = Nothing
                    ICA.Hotkey10Char = Nothing
                    ICA.Hotkey11Shift = Nothing
                    ICA.Hotkey11Char = Nothing

                    ICA.PersistentCacheEnabled = Info.CacheBitmaps

                    ICA.Title = Info.Name

                    Return True
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strIcaSetPropsFailed & vbNewLine & ex.Message, True)
                    Return False
                End Try
            End Function

            Public Overrides Function Connect() As Boolean
                Me.SetEventHandlers()

                Try
                    ICA.Connect()
                    MyBase.Connect()
                    Return True
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strIcaConnectionFailed & vbNewLine & ex.Message)
                    Return False
                End Try
            End Function
#End Region

#Region "Private Methods"
            Private Sub SetCredentials()
                Try
                    Dim _user As String = Me.Info.Username
                    Dim _pass As String = Me.Info.Password
                    Dim _dom As String = Me.Info.Domain

                    If _user = "" Then
                        Select Case My.Settings.EmptyCredentials
                            Case "windows"
                                ICA.Username = Environment.UserName
                            Case "custom"
                                ICA.Username = My.Settings.DefaultUsername
                        End Select
                    Else
                        ICA.Username = _user
                    End If

                    If _pass = "" Then
                        Select Case My.Settings.EmptyCredentials
                            Case "custom"
                                If My.Settings.DefaultPassword <> "" Then
                                    ICA.SetProp("ClearPassword", Security.Crypt.Decrypt(My.Settings.DefaultPassword, App.Info.General.EncryptionKey))
                                End If
                        End Select
                    Else
                        ICA.SetProp("ClearPassword", _pass)
                    End If

                    If _dom = "" Then
                        Select Case My.Settings.EmptyCredentials
                            Case "windows"
                                ICA.Domain = Environment.UserDomainName
                            Case "custom"
                                ICA.Domain = My.Settings.DefaultDomain
                        End Select
                    Else
                        ICA.Domain = _dom
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strIcaSetCredentialsFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SetResolution()
                Try
                    If (Me.Force And Connection.Info.Force.Fullscreen) = Connection.Info.Force.Fullscreen Then
                        ICA.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, Screen.FromControl(frmMain).Bounds.Width, Screen.FromControl(frmMain).Bounds.Height, 0)
                        ICA.FullScreenWindow()

                        Exit Sub
                    End If

                    Select Case Me.InterfaceControl.Info.Resolution
                        Case RDP.RDPResolutions.FitToWindow
                            ICA.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, Me.InterfaceControl.Size.Width, Me.InterfaceControl.Size.Height, 0)
                        Case RDP.RDPResolutions.SmartSize
                            ICA.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, Me.InterfaceControl.Size.Width, Me.InterfaceControl.Size.Height, 0)
                        Case RDP.RDPResolutions.Fullscreen
                            ICA.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, Screen.FromControl(frmMain).Bounds.Width, Screen.FromControl(frmMain).Bounds.Height, 0)
                            ICA.FullScreenWindow()
                        Case Else
                            ICA.SetWindowSize(WFICALib.ICAWindowType.WindowTypeClient, RDP.Resolutions.Items(Int(Info.Resolution)).Width, RDP.Resolutions.Items(Int(Info.Resolution)).Height, 0)
                    End Select
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strIcaSetResolutionFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SetColors()
                Select Case Info.Colors
                    Case RDP.RDPColors.Colors256
                        ICA.SetProp("DesiredColor", 2)
                    Case RDP.RDPColors.Colors15Bit
                        ICA.SetProp("DesiredColor", 4)
                    Case RDP.RDPColors.Colors16Bit
                        ICA.SetProp("DesiredColor", 4)
                    Case Else
                        ICA.SetProp("DesiredColor", 8)
                End Select
            End Sub

            Private Sub SetSecurity()
                Select Case Info.ICAEncryption
                    Case EncryptionStrength.Encr128BitLogonOnly
                        ICA.Encrypt = True
                        ICA.EncryptionLevelSession = "EncRC5-0"
                    Case EncryptionStrength.Encr40Bit
                        ICA.Encrypt = True
                        ICA.EncryptionLevelSession = "EncRC5-40"
                    Case EncryptionStrength.Encr56Bit
                        ICA.Encrypt = True
                        ICA.EncryptionLevelSession = "EncRC5-56"
                    Case EncryptionStrength.Encr128Bit
                        ICA.Encrypt = True
                        ICA.EncryptionLevelSession = "EncRC5-128"
                End Select
            End Sub

            Private Sub SetEventHandlers()
                Try
                    AddHandler ICA.OnConnecting, AddressOf ICAEvent_OnConnecting
                    AddHandler ICA.OnConnect, AddressOf ICAEvent_OnConnected
                    AddHandler ICA.OnConnectFailed, AddressOf ICAEvent_OnConnectFailed
                    AddHandler ICA.OnDisconnect, AddressOf ICAEvent_OnDisconnect
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strIcaSetEventHandlersFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Private Events & Handlers"
            Private Sub ICAEvent_OnConnecting(ByVal sender As Object, ByVal e As System.EventArgs)
                MyBase.Event_Connecting(Me)
            End Sub

            Private Sub ICAEvent_OnConnected(ByVal sender As Object, ByVal e As System.EventArgs)
                MyBase.Event_Connected(Me)
            End Sub

            Private Sub ICAEvent_OnConnectFailed(ByVal sender As Object, ByVal e As System.EventArgs)
                MyBase.Event_ErrorOccured(Me, e.ToString)
            End Sub

            Private Sub ICAEvent_OnDisconnect(ByVal sender As Object, ByVal e As System.EventArgs)
                MyBase.Event_Disconnected(Me, e.ToString)

                If My.Settings.ReconnectOnDisconnect Then
                    ReconnectGroup = New ReconnectGroup
                    ReconnectGroup.Left = (Control.Width / 2) - (ReconnectGroup.Width / 2)
                    ReconnectGroup.Top = (Control.Height / 2) - (ReconnectGroup.Height / 2)
                    ReconnectGroup.Parent = Control
                    ReconnectGroup.Show()
                    tmrReconnect.Enabled = True
                Else
                    MyBase.Close()
                End If
            End Sub
#End Region

#Region "Reconnect Stuff"
            Private Sub tmrReconnect_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles tmrReconnect.Elapsed
                Dim srvReady As Boolean = Tools.PortScan.Scanner.IsPortOpen(Info.Hostname, Info.Port)

                ReconnectGroup.ServerReady = srvReady

                If ReconnectGroup.ReconnectWhenReady And srvReady Then
                    tmrReconnect.Enabled = False
                    ReconnectGroup.DisposeReconnectGroup()
                    ICA.Connect()
                End If
            End Sub
#End Region

#Region "Enums"
            Public Enum Defaults
                Port = 1494
                EncryptionStrength = 0
            End Enum

            Public Enum EncryptionStrength
                <LocalizedDescription("strEncBasic")> _
                EncrBasic = 1
                <LocalizedDescription("strEnc128BitLogonOnly")> _
                Encr128BitLogonOnly = 127
                <LocalizedDescription("strEnc40Bit")> _
                Encr40Bit = 40
                <LocalizedDescription("strEnc56Bit")> _
                Encr56Bit = 56
                <LocalizedDescription("strEnc128Bit")> _
                Encr128Bit = 128
            End Enum
#End Region
        End Class
    End Namespace
End Namespace