Imports mRemoteNG.App.Runtime
Imports System.ComponentModel
Imports mRemoteNG.Tools.LocalizedAttributes

Namespace Connection
    Namespace Protocol
        Public Class VNC
            Inherits Connection.Protocol.Base

#Region "Properties"
            Public Property SmartSize() As Boolean
                Get
                    Return VNC.Scaled
                End Get
                Set(ByVal value As Boolean)
                    VNC.Scaled = value
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
            Private VNC As VncSharp.RemoteDesktop
            Private Info As Connection.Info
#End Region

#Region "Public Methods"
            Public Sub New()
                Me.Control = New VncSharp.RemoteDesktop
            End Sub

            Public Overrides Function SetProps() As Boolean
                MyBase.SetProps()

                Try
                    VNC = Me.Control

                    Info = Me.InterfaceControl.Info

                    VNC.VncPort = Me.Info.Port

                    'If Info.VNCCompression <> Compression.CompNone Then
                    '    VNC.JPEGCompression = True
                    '    VNC.JPEGCompressionLevel = Info.VNCCompression
                    'End If

                    'Select Case Info.VNCEncoding
                    '    Case Encoding.EncCorre
                    '        VNC.Encoding = ViewerX.VNCEncoding.RFB_CORRE
                    '    Case Encoding.EncHextile
                    '        VNC.Encoding = ViewerX.VNCEncoding.RFB_HEXTILE
                    '    Case Encoding.EncRaw
                    '        VNC.Encoding = ViewerX.VNCEncoding.RFB_RAW
                    '    Case Encoding.EncRRE
                    '        VNC.Encoding = ViewerX.VNCEncoding.RFB_RRE
                    '    Case Encoding.EncTight
                    '        VNC.Encoding = ViewerX.VNCEncoding.RFB_TIGHT
                    '    Case Encoding.EncZlib
                    '        VNC.Encoding = ViewerX.VNCEncoding.RFB_ZLIB
                    '    Case Encoding.EncZLibHex
                    '        VNC.Encoding = ViewerX.VNCEncoding.RFB_ZLIBHEX
                    '    Case Encoding.EncZRLE
                    '        VNC.Encoding = ViewerX.VNCEncoding.RFB_ZRLE
                    'End Select

                    'If Info.VNCAuthMode = AuthMode.AuthWin Then
                    '    VNC.LoginType = ViewerX.ViewerLoginType.VLT_MSWIN
                    '    VNC.MsUser = Me.Info.Username
                    '    VNC.MsDomain = Me.Info.Domain
                    '    VNC.MsPassword = Me.Info.Password
                    'Else
                    '    VNC.LoginType = ViewerX.ViewerLoginType.VLT_VNC
                    '    VNC.Password = Me.Info.Password
                    'End If

                    'Select Case Info.VNCProxyType
                    '    Case ProxyType.ProxyNone
                    '        VNC.ProxyType = ViewerX.ConnectionProxyType.VPT_NONE
                    '    Case ProxyType.ProxyHTTP
                    '        VNC.ProxyType = ViewerX.ConnectionProxyType.VPT_HTTP
                    '    Case ProxyType.ProxySocks5
                    '        VNC.ProxyType = ViewerX.ConnectionProxyType.VPT_SOCKS5
                    '    Case ProxyType.ProxyUltra
                    '        VNC.ProxyType = ViewerX.ConnectionProxyType.VPT_ULTRA_REPEATER
                    'End Select

                    'If Info.VNCProxyType <> ProxyType.ProxyNone Then
                    '    VNC.ProxyIP = Info.VNCProxyIP
                    '    VNC.ProxyPort = Info.VNCProxyPort
                    '    VNC.ProxyUser = Info.VNCProxyUsername
                    '    VNC.ProxyPassword = Info.VNCProxyPassword
                    'End If

                    'If Info.VNCColors = Colors.Col8Bit Then
                    '    VNC.RestrictPixel = True
                    'Else
                    '    VNC.RestrictPixel = False
                    'End If

                    'VNC.ConnectingText = My.Resources.strInheritConnecting & " (SmartCode VNC viewer)"
                    'VNC.DisconnectedText = My.Resources.strInheritDisconnected
                    'VNC.MessageBoxes = False
                    'VNC.EndInit()

                    Return True
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strVncSetPropsFailed & vbNewLine & ex.Message, True)
                    Return False
                End Try
            End Function

            Public Overrides Function Connect() As Boolean
                Me.SetEventHandlers()

                Try
                    VNC.Connect(Me.Info.Hostname, Me.Info.VNCViewOnly, Info.VNCSmartSizeMode <> SmartSizeMode.SmartSNo)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strVncConnectionOpenFailed & vbNewLine & ex.Message)
                    Return False
                End Try

                Return True
            End Function

            Public Overrides Sub Disconnect()
                Try
                    VNC.Disconnect()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strVncConnectionDisconnectFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Sub SendSpecialKeys(ByVal Keys As SpecialKeys)
                Try
                    Select Case Keys
                        Case SpecialKeys.CtrlAltDel
                            VNC.SendSpecialKeys(SpecialKeys.CtrlAltDel)
                        Case SpecialKeys.CtrlEsc
                            VNC.SendSpecialKeys(SpecialKeys.CtrlEsc)
                    End Select
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strVncSendSpecialKeysFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Sub ToggleSmartSize()
                Try
                    SmartSize = Not SmartSize
                    RefreshScreen()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strVncToggleSmartSizeFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Sub ToggleViewOnly()
                Try
                    ViewOnly = Not ViewOnly
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strVncToggleViewOnlyFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub


            Public Sub StartChat()
                Try
                    'If VNC.Capabilities.Chat = True Then
                    '    VNC.OpenChat()
                    'Else
                    '    mC.AddMessage(Messages.MessageClass.InformationMsg, "VNC Server doesn't support chat")
                    'End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strVncStartChatFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Sub StartFileTransfer()
                Try
                    'If VNC.Capabilities.FileTransfer = True Then
                    '    VNC.OpenFileTransfer()
                    'Else
                    '    mC.AddMessage(Messages.MessageClass.InformationMsg, "VNC Server doesn't support file transfers")
                    'End If
                Catch ex As Exception

                End Try
            End Sub

            Public Sub RefreshScreen()
                Try
                    VNC.FullScreenUpdate()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strVncRefreshFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Private Methods"
            Private Sub SetEventHandlers()
                Try
                    AddHandler VNC.ConnectComplete, AddressOf VNCEvent_Connected
                    AddHandler VNC.ConnectionLost, AddressOf VNCEvent_Disconnected
                    AddHandler mRemoteNG.frmMain.clipboardchange, AddressOf VNCEvent_ClipboardChanged
                    If Not String.IsNullOrEmpty(Info.Password) Then
                        VNC.GetPassword = AddressOf VNCEvent_Authenticate
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strVncSetEventHandlersFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Private Events & Handlers"
            Private Sub VNCEvent_Connected(ByVal sender As Object, ByVal e As EventArgs)
                MyBase.Event_Connected(Me)
                VNC.AutoScroll = Info.VNCSmartSizeMode = SmartSizeMode.SmartSNo
            End Sub

            Private Sub VNCEvent_Disconnected(ByVal sender As Object, ByVal e As EventArgs)
                MyBase.Event_Disconnected(sender, e.ToString)
                MyBase.Close()
            End Sub

            Private Sub VNCEvent_ClipboardChanged()
                Me.VNC.FillServerClipboard()
            End Sub

            Private Function VNCEvent_Authenticate() As String
                Return Info.Password
            End Function
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
                <LocalizedDescription("strNoCompression")> _
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
                <LocalizedDescription("VNC")> _
                AuthVNC
                <LocalizedDescription("Windows")> _
                AuthWin
            End Enum

            Public Enum ProxyType
                <LocalizedDescription("strNone")> _
                ProxyNone
                <LocalizedDescription("strHttp")> _
                ProxyHTTP
                <LocalizedDescription("strSocks5")> _
                ProxySocks5
                <LocalizedDescription("strUltraVncRepeater")> _
                ProxyUltra
            End Enum

            Public Enum Colors
                <LocalizedDescription("strNormal")> _
                ColNormal
                <Description("8-bit")> _
                Col8Bit
            End Enum

            Public Enum SmartSizeMode
                <LocalizedDescription("strNoSmartSize")> _
                SmartSNo
                <LocalizedDescription("strFree")> _
                SmartSFree
                <LocalizedDescription("strAspect")> _
                SmartSAspect
            End Enum
#End Region
        End Class
    End Namespace
End Namespace