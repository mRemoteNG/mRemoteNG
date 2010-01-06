Imports mRemote.App.Runtime
Imports AxViewerX
Imports System.ComponentModel

Namespace Connection
    Namespace Protocol
        Public Class VNC
            Inherits Connection.Protocol.Base

#Region "Properties"
            Public Property SmartSize() As Boolean
                Get
                    If VNC.StretchMode = ViewerX.ScreenStretchMode.SSM_NONE Then
                        Return False
                    Else
                        Return True
                    End If
                End Get
                Set(ByVal value As Boolean)
                    If value = False Then
                        VNC.StretchMode = ViewerX.ScreenStretchMode.SSM_NONE
                    Else
                        If Info.VNCSmartSizeMode = SmartSizeMode.SmartSFree Then
                            VNC.StretchMode = ViewerX.ScreenStretchMode.SSM_FREE
                        Else
                            VNC.StretchMode = ViewerX.ScreenStretchMode.SSM_ASPECT
                        End If
                    End If
                End Set
            End Property

            Public Property ViewOnly() As Boolean
                Get
                    Return VNC.ViewOnly
                End Get
                Set(ByVal value As Boolean)
                    VNC.ViewOnly = value
                End Set
            End Property
#End Region

#Region "Private Declarations"
            Private VNC As AxCSC_ViewerXControl
            Private Info As Connection.Info
#End Region

#Region "Public Methods"
            Public Sub New()
                Me.Control = New AxCSC_ViewerXControl
                SetupLicense(Me.Control)
            End Sub

            Public Overrides Function SetProps() As Boolean
                MyBase.SetProps()

                Try
                    VNC = Me.Control

                    Info = Me.InterfaceControl.Info

                    VNC.BeginInit()

                    VNC.Port = Me.Info.Port
                    VNC.HostIP = Me.Info.Hostname

                    If Info.VNCCompression <> Compression.CompNone Then
                        VNC.JPEGCompression = True
                        VNC.JPEGCompressionLevel = Info.VNCCompression
                    End If

                    Select Case Info.VNCEncoding
                        Case Encoding.EncCorre
                            VNC.Encoding = ViewerX.VNCEncoding.RFB_CORRE
                        Case Encoding.EncHextile
                            VNC.Encoding = ViewerX.VNCEncoding.RFB_HEXTILE
                        Case Encoding.EncRaw
                            VNC.Encoding = ViewerX.VNCEncoding.RFB_RAW
                        Case Encoding.EncRRE
                            VNC.Encoding = ViewerX.VNCEncoding.RFB_RRE
                        Case Encoding.EncTight
                            VNC.Encoding = ViewerX.VNCEncoding.RFB_TIGHT
                        Case Encoding.EncZlib
                            VNC.Encoding = ViewerX.VNCEncoding.RFB_ZLIB
                        Case Encoding.EncZLibHex
                            VNC.Encoding = ViewerX.VNCEncoding.RFB_ZLIBHEX
                        Case Encoding.EncZRLE
                            VNC.Encoding = ViewerX.VNCEncoding.RFB_ZRLE
                    End Select

                    If Info.VNCAuthMode = AuthMode.AuthWin Then
                        VNC.LoginType = ViewerX.ViewerLoginType.VLT_MSWIN
                        VNC.MsUser = Me.Info.Username
                        VNC.MsDomain = Me.Info.Domain
                        VNC.MsPassword = Me.Info.Password
                    Else
                        VNC.LoginType = ViewerX.ViewerLoginType.VLT_VNC
                        VNC.Password = Me.Info.Password
                    End If

                    Select Case Info.VNCProxyType
                        Case ProxyType.ProxyNone
                            VNC.ProxyType = ViewerX.ConnectionProxyType.VPT_NONE
                        Case ProxyType.ProxyHTTP
                            VNC.ProxyType = ViewerX.ConnectionProxyType.VPT_HTTP
                        Case ProxyType.ProxySocks5
                            VNC.ProxyType = ViewerX.ConnectionProxyType.VPT_SOCKS5
                        Case ProxyType.ProxyUltra
                            VNC.ProxyType = ViewerX.ConnectionProxyType.VPT_ULTRA_REPEATER
                    End Select

                    If Info.VNCProxyType <> ProxyType.ProxyNone Then
                        VNC.ProxyIP = Info.VNCProxyIP
                        VNC.ProxyPort = Info.VNCProxyPort
                        VNC.ProxyUser = Info.VNCProxyUsername
                        VNC.ProxyPassword = Info.VNCProxyPassword
                    End If

                    If Info.VNCColors = Colors.Col8Bit Then
                        VNC.RestrictPixel = True
                    Else
                        VNC.RestrictPixel = False
                    End If

                    Select Case Info.VNCSmartSizeMode
                        Case SmartSizeMode.SmartSNo
                            VNC.StretchMode = ViewerX.ScreenStretchMode.SSM_NONE
                        Case SmartSizeMode.SmartSFree
                            VNC.StretchMode = ViewerX.ScreenStretchMode.SSM_FREE
                        Case SmartSizeMode.SmartSAspect
                            VNC.StretchMode = ViewerX.ScreenStretchMode.SSM_ASPECT
                    End Select

                    VNC.ViewOnly = Info.VNCViewOnly

                    VNC.ConnectingText = Language.Base.Connecting & " (SmartCode VNC viewer)"
                    VNC.DisconnectedText = Language.Base.Disconnected
                    VNC.MessageBoxes = False
                    VNC.EndInit()

                    Return True
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "VNC SetProps failed" & vbNewLine & ex.Message, True)
                    Return False
                End Try
            End Function

            Public Overrides Function Connect() As Boolean
                Me.SetEventHandlers()

                Try
                    VNC.ConnectAsync()
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "Opening connection failed" & vbNewLine & ex.Message)
                    Return False
                End Try

                Return True
            End Function

            Public Overrides Sub Disconnect()
                Try
                    VNC.Disconnect()
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "VNC Disconnect failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Sub SendSpecialKeys(ByVal Keys As SpecialKeys)
                Try
                    Select Case Keys
                        Case SpecialKeys.CtrlAltDel
                            VNC.SendCAD()
                        Case SpecialKeys.CtrlEsc
                            VNC.SendCtrlEsq()
                    End Select
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "VNC SendSpecialKeys failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Sub ToggleSmartSize()
                Try
                    SmartSize = Not SmartSize
                    RefreshScreen()
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "VNC ToggleSmartSize failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Sub ToggleViewOnly()
                Try
                    ViewOnly = Not ViewOnly
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "VNC ToggleViewOnly failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub


            Public Sub StartChat()
                Try
                    If VNC.Capabilities.Chat = True Then
                        VNC.OpenChat()
                    Else
                        mC.AddMessage(Messages.MessageClass.InformationMsg, "VNC Server doesn't support chat")
                    End If
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "VNC StartChat failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Sub StartFileTransfer()
                Try
                    If VNC.Capabilities.FileTransfer = True Then
                        VNC.OpenFileTransfer()
                    Else
                        mC.AddMessage(Messages.MessageClass.InformationMsg, "VNC Server doesn't support file transfers")
                    End If
                Catch ex As Exception

                End Try
            End Sub

            Public Sub RefreshScreen()
                Try
                    VNC.RequestRefresh()
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "VNC RefreshScreen failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Private Methods"
            Private Sub SetEventHandlers()
                Try
                    AddHandler VNC.ConnectedEvent, AddressOf VNCEvent_Connected
                    AddHandler VNC.Disconnected, AddressOf VNCEvent_Disconnected
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "VNC SetEventHandlers failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Shared Sub SetupLicense(ByVal vncCtrl As Control)
                Try
                    Dim f As System.Reflection.FieldInfo
                    f = GetType(AxHost).GetField("licenseKey", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance)
                    f.SetValue(vncCtrl, "{072169039103041044176252035252117103057101225235137221179204110241121074}")
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "VNC SetupLicense failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Private Events & Handlers"
            Private Sub VNCEvent_Connected(ByVal sender As Object, ByVal e As EventArgs)
                MyBase.Event_Connected(Me)
            End Sub

            Private Sub VNCEvent_Disconnected(ByVal sender As Object, ByVal e As EventArgs)
                MyBase.Event_Disconnected(sender, e.ToString)
                MyBase.Close()
            End Sub
#End Region

#Region "Enums"
            Public Enum Defaults
                Port = 5900
            End Enum

            Public Enum SpecialKeys
                CtrlAltDel
                CtrlEsc
            End Enum

            Public Enum Compression
                <Description(Language.Base.NoCompression)> _
                CompNone = 99
                <Description("0")> _
                Comp0 = 0
                <Description("1")> _
                Comp1 = 1
                <Description("2")> _
                Comp2 = 2
                <Description("3")> _
                Comp3 = 3
                <Description("4")> _
                Comp4 = 4
                <Description("5")> _
                Comp5 = 5
                <Description("6")> _
                Comp6 = 6
                <Description("7")> _
                Comp7 = 7
                <Description("8")> _
                Comp8 = 8
                <Description("9")> _
                Comp9 = 9
            End Enum

            Public Enum Encoding
                <Description("Raw")> _
                EncRaw
                <Description("RRE")> _
                EncRRE
                <Description("CoRRE")> _
                EncCorre
                <Description("Hextile")> _
                EncHextile
                <Description("Zlib")> _
                EncZlib
                <Description("Tight")> _
                EncTight
                <Description("ZlibHex")> _
                EncZLibHex
                <Description("ZRLE")> _
                EncZRLE
            End Enum

            Public Enum AuthMode
                <Description("VNC")> _
                AuthVNC
                <Description("Windows")> _
                AuthWin
            End Enum

            Public Enum ProxyType
                <Description(Language.Base.None)> _
                ProxyNone
                <Description("HTTP")> _
                ProxyHTTP
                <Description("Socks 5")> _
                ProxySocks5
                <Description("Ultra VNC Repeater")> _
                ProxyUltra
            End Enum

            Public Enum Colors
                <Description(Language.Base.Normal)> _
                ColNormal
                <Description("8-bit")> _
                Col8Bit
            End Enum

            Public Enum SmartSizeMode
                <Description(Language.Base.NoSmartSize)> _
                SmartSNo
                <Description(Language.Base.Free)> _
                SmartSFree
                <Description(Language.Base.Aspect)> _
                SmartSAspect
            End Enum
#End Region
        End Class
    End Namespace
End Namespace