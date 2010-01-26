Imports System.Windows.Forms
Imports System.Threading
Imports AxMSTSCLib
Imports EOLWTSCOM
Imports System.ComponentModel
Imports mRemote.App.Runtime

Namespace Connection
    Namespace Protocol
        Public Class RDP
            Inherits Connection.Protocol.Base

#Region "Properties"
            Private _SmartSize As Boolean
            Public Property SmartSize() As Boolean
                Get
                    Return Me.RDP.AdvancedSettings4.SmartSizing
                End Get
                Set(ByVal value As Boolean)
                    Me.RDP.AdvancedSettings4.SmartSizing = value
                End Set
            End Property

            Private _Fullscreen As Boolean
            Public Property Fullscreen() As Boolean
                Get
                    Return Me.RDP.FullScreen
                End Get
                Set(ByVal value As Boolean)
                    Me.RDP.FullScreen = value
                End Set
            End Property
#End Region

#Region "Private Declarations"
            Private RDP As AxMsRdpClient5
            Private Info As Connection.Info
            Private RDPVersion As Version
#End Region

#Region "Public Methods"
            Public Sub New()
                Me.Control = New AxMsRdpClient5
            End Sub

            Public Overrides Function SetProps() As Boolean
                MyBase.SetProps()

                Try
                    RDP = Me.Control
                    Info = Me.InterfaceControl.Info

                    Try
                        RDP.CreateControl()

                        Do Until Me.RDP.Created
                            Thread.Sleep(10)
                            System.Windows.Forms.Application.DoEvents()
                        Loop
                    Catch comEx As System.Runtime.InteropServices.COMException
                        mC.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't create RDP control, please check mRemote requirements." & vbNewLine & vbNewLine & comEx.Message)
                        RDP.Dispose()
                        Return False
                    End Try
 

                    Me.RDPVersion = New Version(RDP.Version)

                    RDP.Server = Me.Info.Hostname

                    Me.SetCredentials()
                    Me.SetResolution()
                    Me.RDP.FullScreenTitle = Me.Info.Name

                    'not user changeable
                    RDP.AdvancedSettings2.GrabFocusOnConnect = True
                    RDP.AdvancedSettings3.EnableAutoReconnect = True
                    RDP.AdvancedSettings3.MaxReconnectAttempts = 3
                    RDP.AdvancedSettings2.keepAliveInterval = 60000 'in milliseconds (10.000 = 10 seconds)
                    RDP.AdvancedSettings5.AuthenticationLevel = 0
                    RDP.AdvancedSettings.EncryptionEnabled = 1

                    RDP.AdvancedSettings2.overallConnectionTimeout = 20

                    RDP.AdvancedSettings2.BitmapPeristence = Me.Info.CacheBitmaps

                    Me.SetUseConsoleSession()
                    Me.SetPort()
                    Me.SetRedirectKeys()
                    Me.SetRedirection()
                    Me.SetAuthenticationLevel()
                    Me.SetRDGateway()

                    RDP.ColorDepth = Int(Me.Info.Colors)

                    Me.SetPerformanceFlags()

                    RDP.ConnectingText = Language.Base.Connecting

                    Return True
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "RDP SetProps failed" & vbNewLine & ex.Message, True)
                    Return False
                End Try
            End Function

            Public Overrides Function Connect() As Boolean
                Me.SetEventHandlers()

                Try
                    RDP.Connect()
                    MyBase.Connect()
                    Return True
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "Opening connection failed!" & vbNewLine & ex.Message)
                End Try

                Return False
            End Function

            Public Overrides Sub Disconnect()
                Try
                    RDP.Disconnect()
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "RDP Disconnect failed, trying to close" & vbNewLine & ex.Message, True)
                    MyBase.Close()
                End Try
            End Sub

            Public Sub ToggleFullscreen()
                Try
                    Me.Fullscreen = Not Me.Fullscreen
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "RDP ToggleFullscreen failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Sub ToggleSmartSize()
                Try
                    Me.SmartSize = Not Me.SmartSize
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "RDP ToggleSmartSize failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Overrides Sub Focus()
                Try
                    If RDP.ContainsFocus = False Then
                        RDP.Focus()
                    End If
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "RDP Focus failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Private Methods"
            Private Sub SetRDGateway()
                Try
                    If RDP.TransportSettings.GatewayIsSupported = 1 Then
                        mC.AddMessage(Messages.MessageClass.InformationMsg, "RD Gateway is supported", True)
                        RDP.TransportSettings.GatewayHostname = Me.Info.RDGatewayHostname
                    Else
                        mC.AddMessage(Messages.MessageClass.InformationMsg, "RD Gateway is not supported", True)
                    End If
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "RDP SetRDGateway failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SetUseConsoleSession()
                Try
                    If (Me.Force And Connection.Info.Force.UseConsoleSession) = Connection.Info.Force.UseConsoleSession Then
                        If RDPVersion < Versions.RDC61 Then
                            mC.AddMessage(Messages.MessageClass.InformationMsg, "Setting Console switch for RDC 6.0", True)
                            RDP.AdvancedSettings2.ConnectToServerConsole = True
                        Else
                            mC.AddMessage(Messages.MessageClass.InformationMsg, "Setting Console switch for RDC 6.1", True)
                            RDP.AdvancedSettings6.ConnectToAdministerServer = True
                        End If
                    ElseIf (Me.Force And Connection.Info.Force.DontUseConsoleSession) = Connection.Info.Force.DontUseConsoleSession Then
                        If RDPVersion < Versions.RDC61 Then
                            mC.AddMessage(Messages.MessageClass.InformationMsg, "Setting Console switch for RDC 6.0", True)
                            RDP.AdvancedSettings2.ConnectToServerConsole = False
                        Else
                            mC.AddMessage(Messages.MessageClass.InformationMsg, "Setting Console switch for RDC 6.1", True)
                            RDP.AdvancedSettings6.ConnectToAdministerServer = False
                        End If
                    Else
                        If RDPVersion < Versions.RDC61 Then
                            mC.AddMessage(Messages.MessageClass.InformationMsg, "Setting Console switch for RDC 6.0", True)
                            RDP.AdvancedSettings2.ConnectToServerConsole = Me.Info.UseConsoleSession
                        Else
                            mC.AddMessage(Messages.MessageClass.InformationMsg, "Setting Console switch for RDC 6.1", True)
                            RDP.AdvancedSettings6.ConnectToAdministerServer = Me.Info.UseConsoleSession
                        End If
                    End If
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "RDP SetUseConsoleSession failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SetCredentials()
                Try
                    Dim _user As String = Me.Info.Username
                    Dim _pass As String = Me.Info.Password
                    Dim _dom As String = Me.Info.Domain

                    If _user = "" Then
                        Select Case My.Settings.EmptyCredentials
                            Case "windows"
                                RDP.UserName = Environment.UserName
                            Case "custom"
                                RDP.UserName = My.Settings.DefaultUsername
                        End Select
                    Else
                        RDP.UserName = _user
                    End If

                    If _pass = "" Then
                        Select Case My.Settings.EmptyCredentials
                            Case "custom"
                                If My.Settings.DefaultPassword <> "" Then
                                    RDP.AdvancedSettings2.ClearTextPassword = Security.Crypt.Decrypt(My.Settings.DefaultPassword, App.Info.General.EncryptionKey)
                                End If
                        End Select
                    Else
                        RDP.AdvancedSettings2.ClearTextPassword = _pass
                    End If

                    If _dom = "" Then
                        Select Case My.Settings.EmptyCredentials
                            Case "windows"
                                RDP.Domain = Environment.UserDomainName
                            Case "custom"
                                RDP.Domain = My.Settings.DefaultDomain
                        End Select
                    Else
                        RDP.Domain = _dom
                    End If
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "RDP SetCredentials failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SetResolution()
                Try
                    If (Me.Force And Connection.Info.Force.Fullscreen) = Connection.Info.Force.Fullscreen Then
                        RDP.FullScreen = True
                        RDP.DesktopWidth = Screen.FromControl(frmMain).Bounds.Width
                        RDP.DesktopHeight = Screen.FromControl(frmMain).Bounds.Height

                        Exit Sub
                    End If

                    Select Case Me.InterfaceControl.Info.Resolution
                        Case RDPResolutions.FitToWindow
                            RDP.DesktopWidth = Me.InterfaceControl.Size.Width
                            RDP.DesktopHeight = Me.InterfaceControl.Size.Height
                        Case RDPResolutions.SmartSize
                            RDP.AdvancedSettings.SmartSizing = True
                            RDP.DesktopWidth = Me.InterfaceControl.Size.Width
                            RDP.DesktopHeight = Me.InterfaceControl.Size.Height
                        Case RDPResolutions.Fullscreen
                            RDP.FullScreen = True
                            RDP.DesktopWidth = Screen.FromControl(frmMain).Bounds.Width
                            RDP.DesktopHeight = Screen.FromControl(frmMain).Bounds.Height
                        Case Else
                            RDP.DesktopWidth = Resolutions.Items(Int(Me.Info.Resolution)).Width
                            RDP.DesktopHeight = Resolutions.Items(Int(Me.Info.Resolution)).Height
                    End Select
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "RDP SetResolution failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SetPort()
                Try
                    If Me.Info.Port <> Connection.Protocol.RDP.Defaults.Port Then
                        RDP.AdvancedSettings2.RDPPort = Me.Info.Port
                    End If
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "RDP SetPort failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SetRedirectKeys()
                Try
                    If Me.Info.RedirectKeys Then
                        RDP.AdvancedSettings2.ContainerHandledFullScreen = 1
                        RDP.AdvancedSettings2.DisplayConnectionBar = False
                        RDP.AdvancedSettings2.PinConnectionBar = False
                        RDP.FullScreen = True
                    End If
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "RDP SetRedirectKeys failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SetRedirection()
                Try
                    RDP.AdvancedSettings2.RedirectDrives = Me.Info.RedirectDiskDrives
                    RDP.AdvancedSettings2.RedirectPorts = Me.Info.RedirectPorts
                    RDP.AdvancedSettings2.RedirectPrinters = Me.Info.RedirectPrinters
                    RDP.AdvancedSettings2.RedirectSmartCards = Me.Info.RedirectSmartCards
                    RDP.SecuredSettings2.AudioRedirectionMode = Int(Me.Info.RedirectSound)
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "RDP SetRedirection failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SetPerformanceFlags()
                Try
                    Dim pFlags As Integer
                    If Me.Info.DisplayThemes = False Then
                        pFlags += Int(Connection.Protocol.RDP.RDPPerformanceFlags.DisableThemes)
                    End If

                    If Me.Info.DisplayWallpaper = False Then
                        pFlags += Int(Connection.Protocol.RDP.RDPPerformanceFlags.DisableWallpaper)
                    End If

                    RDP.AdvancedSettings2.PerformanceFlags = pFlags
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "RDP SetPerformanceFlags failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SetAuthenticationLevel()
                Try
                    RDP.AdvancedSettings5.AuthenticationLevel = Me.Info.RDPAuthenticationLevel
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "RDP SetAuthenticationLevel failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SetEventHandlers()
                Try
                    AddHandler RDP.OnConnecting, AddressOf RDPEvent_OnConnecting
                    AddHandler RDP.OnConnected, AddressOf RDPEvent_OnConnected
                    AddHandler RDP.OnFatalError, AddressOf RDPEvent_OnFatalError
                    AddHandler RDP.OnDisconnected, AddressOf RDPEvent_OnDisconnected
                    AddHandler RDP.OnLeaveFullScreenMode, AddressOf RDPEvent_OnLeaveFullscreenMode
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "RDP SetEventHandlers failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Private Events & Handlers"
            Private Sub RDPEvent_OnFatalError(ByVal sender As Object, ByVal e As AxMSTSCLib.IMsTscAxEvents_OnFatalErrorEvent)
                MyBase.Event_ErrorOccured(Me, e.errorCode)
            End Sub

            Private Sub RDPEvent_OnDisconnected(ByVal sender As Object, ByVal e As AxMSTSCLib.IMsTscAxEvents_OnDisconnectedEvent)
                MyBase.Event_Disconnected(Me, e.discReason)

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

            Private Sub RDPEvent_OnConnecting(ByVal sender As Object, ByVal e As System.EventArgs)
                MyBase.Event_Connecting(Me)
            End Sub

            Private Sub RDPEvent_OnConnected(ByVal sender As Object, ByVal e As System.EventArgs)
                MyBase.Event_Connected(Me)
            End Sub

            Private Sub RDPEvent_OnLeaveFullscreenMode(ByVal sender As Object, ByVal e As System.EventArgs)
                RaiseEvent LeaveFullscreen(Me, e)
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
                <Description(Language.Base.RDP256Colors)> _
                Colors256 = 8
                <Description("16 Bit")> _
                Colors16Bit = 16
                <Description("24 Bit")> _
                Colors24Bit = 24
                <Description("32 Bit")> _
                Colors32Bit = 32
            End Enum

            Public Enum RDPSounds
                <Description(Language.Base.RDPSoundBringToThisComputer)> _
                BringToThisComputer = 0
                <Description(Language.Base.RDPSoundLeaveAtRemoteComputer)> _
                LeaveAtRemoteComputer = 1
                <Description(Language.Base.RDPSoundDoNotPlay)> _
                DoNotPlay = 2
            End Enum

            Private Enum RDPPerformanceFlags
                <Description("Disable Wallpaper")> _
                DisableWallpaper = 1
                <Description("Disable Full Window drag")> _
                DisableFullWindowDrag = 2
                <Description("Disable Menu Animations")> _
                DisableMenuAnimations = 4
                <Description("Disable Themes")> _
                DisableThemes = 8
                <Description("Disable Cursor Shadow")> _
                DisableCursorShadow = 20
                <Description("Disable Cursor blinking")> _
                DisableCursorBlinking = 40
            End Enum

            Public Enum RDPResolutions
                <Description(Language.Base.RDPFitToPanel)> _
                FitToWindow
                <Description(Language.Base.Fullscreen)> _
                Fullscreen
                <Description(Language.Base.RDPSmartSize)> _
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
                <Description(Language.Base.AlwaysConnectEvenIfAuthFails)> _
                NoAuth = 0
                <Description(Language.Base.DontConnectWhenAuthFails)> _
                AuthRequired = 1
                <Description(Language.Base.WarnIfAuthFails)> _
                WarnOnFailedAuth = 2
            End Enum
#End Region

#Region "Resolution"
            Public Class Resolution
                Private _Width As Integer
                Public Property Width() As Integer
                    Get
                        Return Me._Width
                    End Get
                    Set(ByVal value As Integer)
                        Me._Width = value
                    End Set
                End Property

                Private _Height As Integer
                Public Property Height() As Integer
                    Get
                        Return Me._Height
                    End Get
                    Set(ByVal value As Integer)
                        Me._Height = value
                    End Set
                End Property

                Public Sub New(ByVal Width As Integer, ByVal Height As Integer)
                    Me._Width = Width
                    Me._Height = Height
                End Sub
            End Class

            Public Class Resolutions
                Public Shared List As New Collection

                Public Shared Sub AddResolutions()
                    Try
                        For Each RDPResolution As RDPResolutions In [Enum].GetValues(GetType(RDPResolutions))
                            If RDPResolution = RDPResolutions.FitToWindow Or RDPResolution = RDPResolutions.SmartSize Or RDPResolution = RDPResolutions.Fullscreen Then
                                Resolutions.Add(New Resolution(0, 0))
                            Else
                                Dim ResSize() As String = Split([Enum].GetName(GetType(RDPResolutions), RDPResolution), "x")
                                Dim ResWidth As String = ResSize(0).Substring(3)
                                Dim ResHeight As String = ResSize(1)
                                Resolutions.Add(New Resolution(ResWidth, ResHeight))
                            End If
                        Next
                    Catch ex As Exception
                        mC.AddMessage(Messages.MessageClass.ErrorMsg, "RDP AddResolutions failed" & vbNewLine & ex.Message, True)
                    End Try
                End Sub


                Public Shared ReadOnly Property Items(ByVal Index As Object) As Resolution
                    Get
                        If TypeOf Index Is Resolution Then
                            Return Index
                        Else
                            Return CType(List.Item(Index + 1), Resolution)
                        End If
                    End Get
                End Property

                Public Shared ReadOnly Property ItemsCount() As Integer
                    Get
                        Return List.Count
                    End Get
                End Property

                Public Shared Function Add(ByVal nRes As Resolution) As Resolution
                    Try
                        List.Add(nRes)

                        Return nRes
                    Catch ex As Exception
                        mC.AddMessage(Messages.MessageClass.ErrorMsg, "RDP Add (Resolutions) failed" & vbNewLine & ex.Message, True)
                    End Try

                    Return Nothing
                End Function
            End Class
#End Region

            Public Class Versions
                Public Shared RDC60 As Version = New Version(6, 0, 6000)
                Public Shared RDC61 As Version = New Version(6, 0, 6001)
            End Class

#Region "Terminal Sessions"
            Public Class TerminalSessions
                Dim oWTSCOM As New WTSCOM
                Dim oWTSSessions As New WTSSessions
                Dim oWTSSession As New WTSSession
                Public ServerHandle As Long

                Public Function OpenConnection(ByVal SrvName As String) As Boolean
                    Try
                        ServerHandle = oWTSCOM.WTSOpenServer(SrvName)

                        If ServerHandle <> 0 Then
                            Return True
                        End If
                    Catch ex As Exception
                        mC.AddMessage(Messages.MessageClass.ErrorMsg, "OpenConnection failed (TerminalSessions)" & vbNewLine & ex.Message, True)
                    End Try

                    Return False
                End Function

                Public Sub CloseConnection(ByVal SrvHandle As Long)
                    Try
                        oWTSCOM.WTSCloseServer(ServerHandle)
                        ServerHandle = 0
                    Catch ex As Exception
                        mC.AddMessage(Messages.MessageClass.ErrorMsg, "CloseConnection failed (TerminalSessions)" & vbNewLine & ex.Message, True)
                    End Try
                End Sub

                Public Function GetSessions() As Sessions
                    Dim colSessions As New Sessions

                    Try
                        oWTSSessions = oWTSCOM.WTSEnumerateSessions(ServerHandle)

                        Dim SessionID As Long
                        Dim SessionUser As String
                        Dim SessionState As Long
                        Dim SessionName As String

                        For Each oWTSSession In oWTSSessions
                            SessionID = oWTSSession.SessionId
                            SessionUser = oWTSCOM.WTSQuerySessionInformation(ServerHandle, oWTSSession.SessionId, 5) 'WFUsername = 5
                            SessionState = oWTSSession.State & vbCrLf
                            SessionName = oWTSSession.WinStationName & vbCrLf

                            If SessionUser <> "" Then
                                If SessionState = 0 Then
                                    colSessions.Add(SessionID, Language.Base.Active, SessionUser, SessionName)
                                Else
                                    colSessions.Add(SessionID, Language.Base.Inactive, SessionUser, SessionName)
                                End If
                            End If
                        Next
                    Catch ex As Exception
                        mC.AddMessage(Messages.MessageClass.ErrorMsg, "GetSessions failed (TerminalSessions)" & vbNewLine & ex.Message, True)
                    End Try

                    Return colSessions
                End Function

                Public Function KillSession(ByVal SessionID As Long) As Boolean
                    Return oWTSCOM.WTSLogoffSession(ServerHandle, SessionID, True)
                End Function
            End Class

            Public Class Sessions
                Inherits CollectionBase

                Default Public ReadOnly Property Items(ByVal Index As Integer) As Session
                    Get
                        Return CType(List.Item(Index), Session)
                    End Get
                End Property

                Public ReadOnly Property ItemsCount() As Integer
                    Get
                        Return List.Count
                    End Get
                End Property

                Public Function Add(ByVal SessionID As Long, ByVal SessionState As String, ByVal SessionUser As String, ByVal SessionName As String) As Session
                    Dim newSes As New Session

                    Try
                        With newSes
                            .SessionID = SessionID
                            .SessionState = SessionState
                            .SessionUser = SessionUser
                            .SessionName = SessionName
                        End With

                        List.Add(newSes)
                    Catch ex As Exception
                        mC.AddMessage(Messages.MessageClass.ErrorMsg, "Add Session failed" & vbNewLine & ex.Message, True)
                    End Try

                    Return newSes
                End Function

                Public Sub ClearSessions()
                    List.Clear()
                End Sub
            End Class

            Public Class Session
                Inherits CollectionBase

                Private lngSessionID As Long
                Public Property SessionID() As Long
                    Get
                        Return lngSessionID
                    End Get
                    Set(ByVal Value As Long)
                        lngSessionID = Value
                    End Set
                End Property

                Private lngSessionState As String
                Public Property SessionState() As String
                    Get
                        Return lngSessionState
                    End Get
                    Set(ByVal Value As String)
                        lngSessionState = Value
                    End Set
                End Property

                Private strSessionUser As String
                Public Property SessionUser() As String
                    Get
                        Return strSessionUser
                    End Get
                    Set(ByVal Value As String)
                        strSessionUser = Value
                    End Set
                End Property

                Private strSessionName As String
                Public Property SessionName() As String
                    Get
                        Return strSessionName
                    End Get
                    Set(ByVal Value As String)
                        strSessionName = Value
                    End Set
                End Property
            End Class
#End Region

#Region "Disconnection Errors"
            Public Class DisconnectionErrors
                Public Shared Code(3080) As String

                Public Shared Function GetError(ByVal id As Integer) As String
                    Try
                        Code(1) = "Local Disconnection (not an error)"
                        Code(2) = "Remote Disconnection by user (not an error)"
                        Code(3) = "Remote Disconnection by server (not an error)"
                        Code(260) = "DNS Lookup Failed"
                        Code(262) = "Out of memory condition"
                        Code(264) = "Connection Timed Out"
                        Code(516) = "WinSock socket connect failed"
                        Code(518) = "Out of memory condition"
                        Code(520) = "Host Not Found error (GetHostByName failed)."
                        Code(772) = "WinSock send call failed"
                        Code(774) = "Out of memory condition"
                        Code(776) = "Invalid IP address specified"
                        Code(1028) = "WinSock recv call failed"
                        Code(1030) = "Invalid Security Data"
                        Code(1032) = "Internal error (code 1032)"
                        Code(1286) = "Invalid Encryption Method"
                        Code(1288) = "DNS Lookup failed"
                        Code(1540) = "GetHostByName call failed"
                        Code(1542) = "Invalid server security data"
                        Code(1544) = "Internal Error (timer error)"
                        Code(1796) = "Timeout occurred"
                        Code(1798) = "Failed to unpack server certificate"
                        Code(2052) = "Bad IP address specified"
                        Code(2054) = "Internal Security Error"
                        Code(2308) = "Socket closed"
                        Code(2310) = "Internal Security Error"
                        Code(2566) = "Internal Security Error"
                        Code(2822) = "Encryption error"
                        Code(2824) = "Your session was taken by someone else"
                        Code(3078) = "Decryption error"

                        Return (Code(id))
                    Catch ex As Exception
                        mC.AddMessage(Messages.MessageClass.ErrorMsg, "GetError failed (DisconnectionErrors)" & vbNewLine & ex.Message, True)
                    End Try

                    Return ""
                End Function
            End Class

            Public Class FatalErrors
                Public Shared Code(110) As String

                Public Shared Function GetError(ByVal id As String) As String
                    Try
                        Code(0) = "An unknown error has occurred"
                        Code(1) = "Internal error code 1"
                        Code(2) = "Out of memory error"
                        Code(3) = "Window creation error"
                        Code(4) = "Internal error code 2"
                        Code(5) = "Internal error code 3 (Invalid state)"
                        Code(100) = "Winsock initialization error"

                        Return (Code(id))
                    Catch ex As Exception
                        mC.AddMessage(Messages.MessageClass.ErrorMsg, "GetError failed (FatalErrors)" & vbNewLine & ex.Message, True)
                    End Try

                    Return ""
                End Function
            End Class
#End Region

#Region "Reconnect Stuff"
            Private Sub tmrReconnect_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles tmrReconnect.Elapsed
                Dim srvReady As Boolean = Tools.PortScan.Scanner.IsPortOpen(Info.Hostname, Info.Port)

                ReconnectGroup.ServerReady = srvReady

                If ReconnectGroup.ReconnectWhenReady And srvReady Then
                    tmrReconnect.Enabled = False
                    ReconnectGroup.DisposeReconnectGroup()
                    'SetProps()
                    RDP.Connect()
                End If
            End Sub
#End Region
            
        End Class
    End Namespace
End Namespace

