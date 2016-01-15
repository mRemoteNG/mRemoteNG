Imports System.Windows.Forms
Imports System.Threading
Imports AxMSTSCLib
Imports EOLWTSCOM
Imports System.ComponentModel
Imports mRemoteNG.Messages
Imports mRemoteNG.App.Runtime
Imports mRemoteNG.Tools.LocalizedAttributes
Imports MSTSCLib

Namespace Connection
    Namespace Protocol
        Public Class RDP
            Inherits Base
#Region "Properties"
            Public Property SmartSize() As Boolean
                Get
                    Return _rdpClient.AdvancedSettings2.SmartSizing
                End Get
                Set(ByVal value As Boolean)
                    _rdpClient.AdvancedSettings2.SmartSizing = value
                    ReconnectForResize()
                End Set
            End Property

            Public Property Fullscreen() As Boolean
                Get
                    Return _rdpClient.FullScreen
                End Get
                Set(ByVal value As Boolean)
                    _rdpClient.FullScreen = value
                    ReconnectForResize()
                End Set
            End Property

            Private _redirectKeys As Boolean = False
            Public Property RedirectKeys As Boolean
                Get
                    Return _redirectKeys
                End Get
                Set(value As Boolean)
                    _redirectKeys = value
                    Try
                        If Not _redirectKeys Then Return

                        Debug.Assert(_rdpClient.SecuredSettingsEnabled)
                        Dim msRdpClientSecuredSettings As MSTSCLib.IMsRdpClientSecuredSettings = _rdpClient.SecuredSettings2
                        msRdpClientSecuredSettings.KeyboardHookMode = 1 ' Apply key combinations at the remote server.
                    Catch ex As Exception
                        MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpSetRedirectKeysFailed & vbNewLine & ex.Message, True)
                    End Try
                End Set
            End Property
#End Region

#Region "Private Declarations"
            Private _rdpClient As MsRdpClient5NotSafeForScripting
            Private _rdpVersion As Version
            Private _connectionInfo As Info
            Private _loginComplete As Boolean
#End Region

#Region "Public Methods"
            Public Sub New()
                Control = New AxMsRdpClient5NotSafeForScripting
            End Sub

            Public Overrides Function SetProps() As Boolean
                MyBase.SetProps()

                Try
                    Control.CreateControl()
                    _connectionInfo = InterfaceControl.Info

                    Try
                        Do Until Control.Created
                            Thread.Sleep(0)
                            System.Windows.Forms.Application.DoEvents()
                        Loop

                        _rdpClient = CType(Control, AxMsRdpClient5NotSafeForScripting).GetOcx()
                    Catch ex As Runtime.InteropServices.COMException
                        MessageCollector.AddExceptionMessage(My.Language.strRdpControlCreationFailed, ex)
                        Control.Dispose()
                        Return False
                    End Try

                    _rdpVersion = New Version(_rdpClient.Version)

                    _rdpClient.Server = Me._connectionInfo.Hostname

                    Me.SetCredentials()
                    Me.SetResolution()
                    Me._rdpClient.FullScreenTitle = Me._connectionInfo.Name

                    'not user changeable
                    _rdpClient.AdvancedSettings2.GrabFocusOnConnect = True
                    _rdpClient.AdvancedSettings3.EnableAutoReconnect = True
                    _rdpClient.AdvancedSettings3.MaxReconnectAttempts = My.Settings.RdpReconnectionCount
                    _rdpClient.AdvancedSettings2.keepAliveInterval = 60000 'in milliseconds (10.000 = 10 seconds)
                    _rdpClient.AdvancedSettings5.AuthenticationLevel = 0
                    _rdpClient.AdvancedSettings2.EncryptionEnabled = 1

                    _rdpClient.AdvancedSettings2.overallConnectionTimeout = 20

                    _rdpClient.AdvancedSettings2.BitmapPeristence = Me._connectionInfo.CacheBitmaps
                    If _rdpVersion >= Versions.RDC61 Then
                        _rdpClient.AdvancedSettings7.EnableCredSspSupport = _connectionInfo.UseCredSsp
                    End If

                    Me.SetUseConsoleSession()
                    Me.SetPort()
                    RedirectKeys = _connectionInfo.RedirectKeys
                    Me.SetRedirection()
                    Me.SetAuthenticationLevel()
                    SetLoadBalanceInfo()
                    Me.SetRdGateway()

                    _rdpClient.ColorDepth = Int(Me._connectionInfo.Colors)

                    Me.SetPerformanceFlags()

                    _rdpClient.ConnectingText = My.Language.strConnecting

                    Control.Anchor = AnchorStyles.None

                    Return True
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpSetPropsFailed & vbNewLine & ex.Message, True)
                    Return False
                End Try
            End Function

            Public Overrides Function Connect() As Boolean
                _loginComplete = False
                SetEventHandlers()

                Try
                    _rdpClient.Connect()
                    MyBase.Connect()
                    Return True
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpConnectionOpenFailed & vbNewLine & ex.Message)
                End Try

                Return False
            End Function

            Public Overrides Sub Disconnect()
                Try
                    _rdpClient.Disconnect()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpDisconnectFailed & vbNewLine & ex.Message, True)
                    MyBase.Close()
                End Try
            End Sub

            Public Sub ToggleFullscreen()
                Try
                    Me.Fullscreen = Not Me.Fullscreen
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpToggleFullscreenFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Sub ToggleSmartSize()
                Try
                    Me.SmartSize = Not Me.SmartSize
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpToggleSmartSizeFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Overrides Sub Focus()
                Try
                    If Control.ContainsFocus = False Then
                        Control.Focus()
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpFocusFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private _controlBeginningSize As New Size
            Public Overrides Sub ResizeBegin(ByVal sender As Object, ByVal e As EventArgs)
                _controlBeginningSize = Control.Size
            End Sub

            Public Overrides Sub Resize(ByVal sender As Object, ByVal e As EventArgs)
                If DoResize() And _controlBeginningSize.IsEmpty Then
                    ReconnectForResize()
                End If
                MyBase.Resize(sender, e)
            End Sub

            Public Overrides Sub ResizeEnd(ByVal sender As Object, ByVal e As EventArgs)
                DoResize()
                If Not Control.Size = _controlBeginningSize Then
                    ReconnectForResize()
                End If
                _controlBeginningSize = Size.Empty
            End Sub
#End Region

#Region "Private Methods"
            Private Function DoResize() As Boolean
                Control.Location = InterfaceControl.Location
                If Not Control.Size = InterfaceControl.Size And Not InterfaceControl.Size = Size.Empty Then
                    Control.Size = InterfaceControl.Size
                    Return True
                Else
                    Return False
                End If
            End Function

            Private Sub ReconnectForResize()
                If _rdpVersion < Versions.RDC80 Then Return

                If Not _loginComplete Then Return

                If Not InterfaceControl.Info.AutomaticResize Then Return

                If Not (InterfaceControl.Info.Resolution = RDPResolutions.FitToWindow Or _
                        InterfaceControl.Info.Resolution = RDPResolutions.Fullscreen) Then Return

                If SmartSize Then Return

                Dim size As Size
                If Not Fullscreen Then
                    size = Control.Size
                Else
                    size = Screen.FromControl(Control).Bounds.Size
                End If

                Dim msRdpClient8 As IMsRdpClient8 = _rdpClient
                msRdpClient8.Reconnect(size.Width, size.Height)
            End Sub

            Private Sub SetRdGateway()
                Try
                    If _rdpClient.TransportSettings.GatewayIsSupported = 0 Then
                        MessageCollector.AddMessage(MessageClass.InformationMsg, My.Language.strRdpGatewayNotSupported, True)
                        Return
                    Else
                        MessageCollector.AddMessage(MessageClass.InformationMsg, My.Language.strRdpGatewayIsSupported, True)
                    End If

                    If Not _connectionInfo.RDGatewayUsageMethod = RDGatewayUsageMethod.Never Then
                        _rdpClient.TransportSettings.GatewayUsageMethod = _connectionInfo.RDGatewayUsageMethod
                        _rdpClient.TransportSettings.GatewayHostname = _connectionInfo.RDGatewayHostname
                        _rdpClient.TransportSettings.GatewayProfileUsageMethod = 1 ' TSC_PROXY_PROFILE_MODE_EXPLICIT
                        If _connectionInfo.RDGatewayUseConnectionCredentials = RDGatewayUseConnectionCredentials.SmartCard Then
                            _rdpClient.TransportSettings.GatewayCredsSource = 1 ' TSC_PROXY_CREDS_MODE_SMARTCARD
                        End If
                        If _rdpVersion >= Versions.RDC61 And Not ((Force And Info.Force.NoCredentials) = Info.Force.NoCredentials) Then
                            If _connectionInfo.RDGatewayUseConnectionCredentials = RDGatewayUseConnectionCredentials.Yes Then
                                _rdpClient.TransportSettings2.GatewayUsername = _connectionInfo.Username
                                _rdpClient.TransportSettings2.GatewayPassword = _connectionInfo.Password
                                _rdpClient.TransportSettings2.GatewayDomain = _connectionInfo.Domain
                            ElseIf _connectionInfo.RDGatewayUseConnectionCredentials = RDGatewayUseConnectionCredentials.SmartCard Then
                                _rdpClient.TransportSettings2.GatewayCredSharing = 0
                            Else
                                _rdpClient.TransportSettings2.GatewayUsername = _connectionInfo.RDGatewayUsername
                                _rdpClient.TransportSettings2.GatewayPassword = _connectionInfo.RDGatewayPassword
                                _rdpClient.TransportSettings2.GatewayDomain = _connectionInfo.RDGatewayDomain
                                _rdpClient.TransportSettings2.GatewayCredSharing = 0
                            End If
                        End If
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strRdpSetGatewayFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SetUseConsoleSession()
                Try
                    Dim value As Boolean

                    If (Force And Info.Force.UseConsoleSession) = Info.Force.UseConsoleSession Then
                        value = True
                    ElseIf (Force And Info.Force.DontUseConsoleSession) = Info.Force.DontUseConsoleSession Then
                        value = False
                    Else
                        value = _connectionInfo.UseConsoleSession
                    End If

                    If _rdpVersion >= Versions.RDC61 Then
                        MessageCollector.AddMessage(MessageClass.InformationMsg, String.Format(My.Language.strRdpSetConsoleSwitch, "6.1"), True)
                        _rdpClient.AdvancedSettings7.ConnectToAdministerServer = value
                    Else
                        MessageCollector.AddMessage(MessageClass.InformationMsg, String.Format(My.Language.strRdpSetConsoleSwitch, "6.0"), True)
                        _rdpClient.AdvancedSettings2.ConnectToServerConsole = value
                    End If
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage(My.Language.strRdpSetConsoleSessionFailed, ex, MessageClass.ErrorMsg, True)
                End Try
            End Sub

            Private Sub SetCredentials()
                Try
                    If (Force And Info.Force.NoCredentials) = Info.Force.NoCredentials Then Return

                    Dim userName As String = _connectionInfo.Username
                    Dim password As String = _connectionInfo.Password
                    Dim domain As String = _connectionInfo.Domain

                    If userName = "" Then
                        Select Case My.Settings.EmptyCredentials
                            Case "windows"
                                _rdpClient.UserName = Environment.UserName
                            Case "custom"
                                _rdpClient.UserName = My.Settings.DefaultUsername
                        End Select
                    Else
                        _rdpClient.UserName = userName
                    End If

                    If password = "" Then
                        Select Case My.Settings.EmptyCredentials
                            Case "custom"
                                If My.Settings.DefaultPassword <> "" Then
                                    _rdpClient.AdvancedSettings2.ClearTextPassword = Security.Crypt.Decrypt(My.Settings.DefaultPassword, App.Info.General.EncryptionKey)
                                End If
                        End Select
                    Else
                        _rdpClient.AdvancedSettings2.ClearTextPassword = password
                    End If

                    If domain = "" Then
                        Select Case My.Settings.EmptyCredentials
                            Case "windows"
                                _rdpClient.Domain = Environment.UserDomainName
                            Case "custom"
                                _rdpClient.Domain = My.Settings.DefaultDomain
                        End Select
                    Else
                        _rdpClient.Domain = domain
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpSetCredentialsFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SetResolution()
                Try
                    If (Me.Force And Connection.Info.Force.Fullscreen) = Connection.Info.Force.Fullscreen Then
                        _rdpClient.FullScreen = True
                        _rdpClient.DesktopWidth = Screen.FromControl(frmMain).Bounds.Width
                        _rdpClient.DesktopHeight = Screen.FromControl(frmMain).Bounds.Height

                        Exit Sub
                    End If

                    Select Case Me.InterfaceControl.Info.Resolution
                        Case RDPResolutions.FitToWindow, RDPResolutions.SmartSize
                            _rdpClient.DesktopWidth = InterfaceControl.Size.Width
                            _rdpClient.DesktopHeight = InterfaceControl.Size.Height
                        Case RDPResolutions.Fullscreen
                            _rdpClient.FullScreen = True
                            _rdpClient.DesktopWidth = Screen.FromControl(frmMain).Bounds.Width
                            _rdpClient.DesktopHeight = Screen.FromControl(frmMain).Bounds.Height
                        Case Else
                            Dim resolution As Rectangle = GetResolutionRectangle(_connectionInfo.Resolution)
                            _rdpClient.DesktopWidth = resolution.Width
                            _rdpClient.DesktopHeight = resolution.Height
                    End Select
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpSetResolutionFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SetPort()
                Try
                    If _connectionInfo.Port <> Defaults.Port Then
                        _rdpClient.AdvancedSettings2.RDPPort = _connectionInfo.Port
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpSetPortFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SetRedirection()
                Try
                    _rdpClient.AdvancedSettings2.RedirectDrives = Me._connectionInfo.RedirectDiskDrives
                    _rdpClient.AdvancedSettings2.RedirectPorts = Me._connectionInfo.RedirectPorts
                    _rdpClient.AdvancedSettings2.RedirectPrinters = Me._connectionInfo.RedirectPrinters
                    _rdpClient.AdvancedSettings2.RedirectSmartCards = Me._connectionInfo.RedirectSmartCards
                    _rdpClient.SecuredSettings2.AudioRedirectionMode = Int(Me._connectionInfo.RedirectSound)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpSetRedirectionFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SetPerformanceFlags()
                Try
                    Dim pFlags As Integer
                    If Me._connectionInfo.DisplayThemes = False Then
                        pFlags += Int(Connection.Protocol.RDP.RDPPerformanceFlags.DisableThemes)
                    End If

                    If Me._connectionInfo.DisplayWallpaper = False Then
                        pFlags += Int(Connection.Protocol.RDP.RDPPerformanceFlags.DisableWallpaper)
                    End If

                    If Me._connectionInfo.EnableFontSmoothing Then
                        pFlags += Int(Connection.Protocol.RDP.RDPPerformanceFlags.EnableFontSmoothing)
                    End If

                    If Me._connectionInfo.EnableDesktopComposition Then
                        pFlags += Int(Connection.Protocol.RDP.RDPPerformanceFlags.EnableDesktopComposition)
                    End If

                    _rdpClient.AdvancedSettings2.PerformanceFlags = pFlags
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpSetPerformanceFlagsFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SetAuthenticationLevel()
                Try
                    _rdpClient.AdvancedSettings5.AuthenticationLevel = Me._connectionInfo.RDPAuthenticationLevel
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpSetAuthenticationLevelFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SetLoadBalanceInfo()
                If String.IsNullOrEmpty(_connectionInfo.LoadBalanceInfo) Then Return
                Try
                    _rdpClient.AdvancedSettings2.LoadBalanceInfo = _connectionInfo.LoadBalanceInfo
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage("Unable to set load balance info.", ex)
                End Try
            End Sub

            Private Sub SetEventHandlers()
                Try
                    AddHandler _rdpClient.OnConnecting, AddressOf RDPEvent_OnConnecting
                    AddHandler _rdpClient.OnConnected, AddressOf RDPEvent_OnConnected
                    AddHandler _rdpClient.OnLoginComplete, AddressOf RDPEvent_OnLoginComplete
                    AddHandler _rdpClient.OnFatalError, AddressOf RDPEvent_OnFatalError
                    AddHandler _rdpClient.OnDisconnected, AddressOf RDPEvent_OnDisconnected
                    AddHandler _rdpClient.OnLeaveFullScreenMode, AddressOf RDPEvent_OnLeaveFullscreenMode
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpSetEventHandlersFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Private Events & Handlers"
            Private Sub RDPEvent_OnFatalError(ByVal errorCode As Integer)
                Event_ErrorOccured(Me, errorCode)
            End Sub

            Private Sub RDPEvent_OnDisconnected(ByVal discReason As Integer)
                Const UI_ERR_NORMAL_DISCONNECT As Integer = &HB08
                If Not discReason = UI_ERR_NORMAL_DISCONNECT Then
                    Dim reason As String = _rdpClient.GetErrorDescription(discReason, _rdpClient.ExtendedDisconnectReason)
                    Event_Disconnected(Me, discReason & vbCrLf & reason)
                End If

                If My.Settings.ReconnectOnDisconnect Then
                    ReconnectGroup = New ReconnectGroup
                    ReconnectGroup.Left = (Control.Width / 2) - (ReconnectGroup.Width / 2)
                    ReconnectGroup.Top = (Control.Height / 2) - (ReconnectGroup.Height / 2)
                    ReconnectGroup.Parent = Control
                    ReconnectGroup.Show()
                    tmrReconnect.Enabled = True
                Else
                    Close()
                End If
            End Sub

            Private Sub RDPEvent_OnConnecting()
                Event_Connecting(Me)
            End Sub

            Private Sub RDPEvent_OnConnected()
                Event_Connected(Me)
            End Sub

            Private Sub RDPEvent_OnLoginComplete()
                _loginComplete = True
            End Sub

            Private Sub RDPEvent_OnLeaveFullscreenMode()
                Fullscreen = False
                RaiseEvent LeaveFullscreen(Me, New EventArgs())
            End Sub
#End Region

#Region "Public Events & Handlers"
            Public Event LeaveFullscreen(ByVal sender As Connection.Protocol.RDP, ByVal e As System.EventArgs)
#End Region

#Region "Enums"
            Public Enum Defaults
                Colors = RDPColors.Colors16Bit
                Sounds = RDPSounds.DoNotPlay
                Resolution = RDPResolutions.FitToWindow
                Port = 3389
            End Enum

            Public Enum RDPColors
                <LocalizedDescription("strRDP256Colors")> _
                Colors256 = 8
                <LocalizedDescription("strRDP32768Colors")> _
                Colors15Bit = 15
                <LocalizedDescription("strRDP65536Colors")> _
                Colors16Bit = 16
                <LocalizedDescription("strRDP16777216Colors")> _
                Colors24Bit = 24
                <LocalizedDescription("strRDP4294967296Colors")> _
                Colors32Bit = 32
            End Enum

            Public Enum RDPSounds
                <LocalizedDescription("strRDPSoundBringToThisComputer")> _
                BringToThisComputer = 0
                <LocalizedDescription("strRDPSoundLeaveAtRemoteComputer")> _
                LeaveAtRemoteComputer = 1
                <LocalizedDescription("strRDPSoundDoNotPlay")> _
                DoNotPlay = 2
            End Enum

            Private Enum RDPPerformanceFlags
                <Description("strRDPDisableWallpaper")> _
                DisableWallpaper = &H1
                <Description("strRDPDisableFullWindowdrag")> _
                DisableFullWindowDrag = &H2
                <Description("strRDPDisableMenuAnimations")> _
                DisableMenuAnimations = &H4
                <Description("strRDPDisableThemes")> _
                DisableThemes = &H8
                <Description("strRDPDisableCursorShadow")> _
                DisableCursorShadow = &H20
                <Description("strRDPDisableCursorblinking")> _
                DisableCursorBlinking = &H40
                <Description("strRDPEnableFontSmoothing")> _
                EnableFontSmoothing = &H80
                <Description("strRDPEnableDesktopComposition")> _
                EnableDesktopComposition = &H100
            End Enum

            Public Enum RDPResolutions
                <LocalizedDescription("strRDPFitToPanel")> _
                FitToWindow
                <LocalizedDescription("strFullscreen")> _
                Fullscreen
                <LocalizedDescription("strRDPSmartSize")> _
                SmartSize
                <Description("640x480")> _
                Res640x480
                <Description("800x600")> _
                Res800x600
                <Description("1024x768")> _
                Res1024x768
                <Description("1152x864")> _
                Res1152x864
                <Description("1280x800")> _
                Res1280x800
                <Description("1280x1024")> _
                Res1280x1024
                <Description("1400x1050")> _
                Res1400x1050
                <Description("1440x900")> _
                Res1440x900
                <Description("1600x1024")> _
                Res1600x1024
                <Description("1600x1200")> _
                Res1600x1200
                <Description("1600x1280")> _
                Res1600x1280
                <Description("1680x1050")> _
                Res1680x1050
                <Description("1900x1200")> _
                Res1900x1200
                <Description("1920x1200")> _
                Res1920x1200
                <Description("2048x1536")> _
                Res2048x1536
                <Description("2560x2048")> _
                Res2560x2048
                <Description("3200x2400")> _
                Res3200x2400
                <Description("3840x2400")> _
                Res3840x2400
            End Enum

            Public Enum AuthenticationLevel
                <LocalizedDescription("strAlwaysConnectEvenIfAuthFails")> _
                NoAuth = 0
                <LocalizedDescription("strDontConnectWhenAuthFails")> _
                AuthRequired = 1
                <LocalizedDescription("strWarnIfAuthFails")> _
                WarnOnFailedAuth = 2
            End Enum

            Public Enum RDGatewayUsageMethod
                <LocalizedDescription("strNever")> _
                Never = 0 ' TSC_PROXY_MODE_NONE_DIRECT
                <LocalizedDescription("strAlways")> _
                Always = 1 ' TSC_PROXY_MODE_DIRECT
                <LocalizedDescription("strDetect")> _
                Detect = 2 ' TSC_PROXY_MODE_DETECT
            End Enum

            Public Enum RDGatewayUseConnectionCredentials
                <LocalizedDescription("strUseDifferentUsernameAndPassword")> _
                No = 0
                <LocalizedDescription("strUseSameUsernameAndPassword")> _
                Yes = 1
                <LocalizedDescription("strUseSmartCard")> _
                SmartCard = 2
            End Enum
#End Region

#Region "Resolution"
            Public Shared Function GetResolutionRectangle(ByVal resolution As RDPResolutions) As Rectangle
                Dim resolutionParts() As String = Nothing
                If Not resolution = RDPResolutions.FitToWindow And _
                   Not resolution = RDPResolutions.Fullscreen And _
                   Not resolution = RDPResolutions.SmartSize Then
                    resolutionParts = resolution.ToString.Replace("Res", "").Split("x")
                End If
                If resolutionParts Is Nothing OrElse Not resolutionParts.Length = 2 Then
                    Return New Rectangle(0, 0, 0, 0)
                Else
                    Return New Rectangle(0, 0, resolutionParts(0), resolutionParts(1))
                End If
            End Function
#End Region

            Public Class Versions
                Public Shared RDC60 As New Version(6, 0, 6000)
                Public Shared RDC61 As New Version(6, 0, 6001)
                Public Shared RDC70 As New Version(6, 1, 7600)
                Public Shared RDC80 As New Version(6, 2, 9200)
                Public Shared RDC81 As New Version(6, 3, 9600)
                Public Shared RDC10 As New Version(10, 0, 10240)
            End Class

#Region "Terminal Sessions"
            Public Class TerminalSessions
                Private ReadOnly _wtsCom As WTSCOM

                Public Sub New()
                    Try
                        _wtsCom = New WTSCOM
                    Catch ex As Exception
                        MessageCollector.AddExceptionMessage("TerminalSessions.New() failed.", ex, MessageClass.ErrorMsg, True)
                    End Try
                End Sub

                Public Function OpenConnection(ByVal hostname As String) As Long
                    If _wtsCom Is Nothing Then Return 0

                    Try
                        Return _wtsCom.WTSOpenServer(hostname)
                    Catch ex As Exception
                        MessageCollector.AddExceptionMessage(My.Language.strRdpOpenConnectionFailed, ex, MessageClass.ErrorMsg, True)
                    End Try
                End Function

                Public Sub CloseConnection(ByVal serverHandle As Long)
                    If _wtsCom Is Nothing Then Return

                    Try
                        _wtsCom.WTSCloseServer(serverHandle)
                    Catch ex As Exception
                        MessageCollector.AddExceptionMessage(My.Language.strRdpCloseConnectionFailed, ex, MessageClass.ErrorMsg, True)
                    End Try
                End Sub

                Public Function GetSessions(ByVal serverHandle As Long) As SessionsCollection
                    If _wtsCom Is Nothing Then Return New SessionsCollection()

                    Dim sessions As New SessionsCollection()

                    Try
                        Dim wtsSessions As WTSSessions = _wtsCom.WTSEnumerateSessions(serverHandle)

                        Dim sessionId As Long
                        Dim sessionUser As String
                        Dim sessionState As Long
                        Dim sessionName As String

                        For Each wtsSession As WTSSession In wtsSessions
                            sessionId = wtsSession.SessionId
                            sessionUser = _wtsCom.WTSQuerySessionInformation(serverHandle, wtsSession.SessionId, 5) ' WFUsername = 5
                            sessionState = wtsSession.State & vbCrLf
                            sessionName = wtsSession.WinStationName & vbCrLf

                            If Not String.IsNullOrEmpty(sessionUser) Then
                                If sessionState = 0 Then
                                    sessions.Add(sessionId, My.Language.strActive, sessionUser, sessionName)
                                Else
                                    sessions.Add(sessionId, My.Language.strInactive, sessionUser, sessionName)
                                End If
                            End If
                        Next
                    Catch ex As Exception
                        MessageCollector.AddExceptionMessage(My.Language.strRdpGetSessionsFailed, ex, MessageClass.ErrorMsg, True)
                    End Try

                    Return sessions
                End Function

                Public Function KillSession(ByVal serverHandle As Long, ByVal sessionId As Long) As Boolean
                    If _wtsCom Is Nothing Then Return False

                    Dim result As Boolean = False

                    Try
                        result = _wtsCom.WTSLogoffSession(serverHandle, sessionId, True)
                    Catch ex As Exception
                        MessageCollector.AddExceptionMessage("TerminalSessions.KillSession() failed.", ex, MessageClass.ErrorMsg, True)
                    End Try

                    Return result
                End Function
            End Class

            Public Class SessionsCollection
                Inherits CollectionBase

                Default Public ReadOnly Property Items(ByVal index As Integer) As Session
                    Get
                        Return CType(List.Item(index), Session)
                    End Get
                End Property

                Public ReadOnly Property ItemsCount() As Integer
                    Get
                        Return List.Count
                    End Get
                End Property

                Public Overloads Function Add(ByVal sessionId As Long, ByVal sessionState As String, ByVal sessionUser As String, ByVal sessionName As String) As Session
                    Dim newSession As New Session

                    Try
                        With newSession
                            .SessionId = sessionId
                            .SessionState = sessionState
                            .SessionUser = sessionUser
                            .SessionName = sessionName
                        End With

                        List.Add(newSession)
                    Catch ex As Exception
                        MessageCollector.AddExceptionMessage(My.Language.strRdpAddSessionFailed, ex, MessageClass.ErrorMsg, True)
                    End Try

                    Return newSession
                End Function

                Public Sub ClearSessions()
                    List.Clear()
                End Sub
            End Class

            Public Class Session
                Inherits CollectionBase

                Public Property SessionId() As Long
                Public Property SessionState() As String
                Public Property SessionUser() As String
                Public Property SessionName() As String
            End Class
#End Region

#Region "Fatal Errors"
            Public Class FatalErrors
                Protected Shared _description() As String = { _
                    0 = My.Language.strRdpErrorUnknown, _
                    1 = My.Language.strRdpErrorCode1, _
                    2 = My.Language.strRdpErrorOutOfMemory, _
                    3 = My.Language.strRdpErrorWindowCreation, _
                    4 = My.Language.strRdpErrorCode2, _
                    5 = My.Language.strRdpErrorCode3, _
                    6 = My.Language.strRdpErrorCode4, _
                    7 = My.Language.strRdpErrorConnection, _
                    100 = My.Language.strRdpErrorWinsock _
                }

                Public Shared Function GetError(ByVal id As String) As String
                    Try
                        Return (_description(id))
                    Catch ex As Exception
                        MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strRdpErrorGetFailure & vbNewLine & ex.Message, True)
                        Return String.Format(My.Language.strRdpErrorUnknown, id)
                    End Try
                End Function
            End Class
#End Region

#Region "Reconnect Stuff"
            Private Sub tmrReconnect_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles tmrReconnect.Elapsed
                Dim srvReady As Boolean = Tools.PortScan.Scanner.IsPortOpen(_connectionInfo.Hostname, _connectionInfo.Port)

                ReconnectGroup.ServerReady = srvReady

                If ReconnectGroup.ReconnectWhenReady And srvReady Then
                    'tmrReconnect.Enabled = False
                    'ReconnectGroup.DisposeReconnectGroup()
                    'SetProps()
                    '_rdpClient.Connect()
                    Try
                        tmrReconnect.Enabled = False
                        ReconnectGroup.DisposeReconnectGroup()
                        'SetProps()
                        _rdpClient.Connect()
                    Catch ex As Exception
                        'MessageBox.Show("Error Trying to Reconnect: " & ex.Message & vbCrLf & "Source: " & ex.Source & " Stack Trace: " & ex.StackTrace)

                        _rdpClient.Disconnect()
                        Threading.Thread.Sleep(5000)

                        'tmrReconnect.Enabled = False
                        'ReconnectGroup.DisposeReconnectGroup()
                        'SetProps()
                        _rdpClient.Connect()
                    End Try
                End If
            End Sub
#End Region
        End Class
    End Namespace
End Namespace

