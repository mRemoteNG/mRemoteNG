Imports System.ComponentModel
Imports System.Reflection
Imports mRemote3G.App
Imports mRemote3G.Config.Putty
Imports mRemote3G.Connection.Protocol
Imports mRemote3G.Messages
Imports mRemote3G.Tools

Namespace Connection
    <DefaultProperty("Name")>
    Public Class Info

#Region "Public Properties"

#Region "Display"

        <LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameName"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionName")>
        Public Overridable Property Name As String = Language.Language.strNewConnection

        Private _description As String = My.Settings.ConDefaultDescription

        <LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDescription"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDescription")>
        Public Overridable Property Description As String
            Get
                Return GetInheritedPropertyValue("Description", _description)
            End Get
            Set
                _description = value
            End Set
        End Property

        Private _icon As String = My.Settings.ConDefaultIcon

        <LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
            TypeConverter(GetType(Icon)),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameIcon"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionIcon")>
        Public Overridable Property Icon As String
            Get
                Return GetInheritedPropertyValue("Icon", _icon)
            End Get
            Set
                _icon = value
            End Set
        End Property

        Private _panel As String = Language.Language.strGeneral

        <LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePanel"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPanel")>
        Public Overridable Property Panel As String
            Get
                Return GetInheritedPropertyValue("Panel", _panel)
            End Get
            Set
                _panel = value
            End Set
        End Property

#End Region

#Region "Connection"

        Private _hostname As String = String.Empty

        <LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameAddress"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAddress")>
        Public Overridable Property Hostname As String
            Get
                Return _hostname.Trim()
            End Get
            Set
                If String.IsNullOrEmpty(value) Then
                    _hostname = String.Empty
                Else
                    _hostname = value.Trim()
                End If
            End Set
        End Property

        Private _username As String = My.Settings.ConDefaultUsername

        <LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUsername"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUsername")>
        Public Overridable Property Username As String
            Get
                Return GetInheritedPropertyValue("Username", _username)
            End Get
            Set
                _username = value.Trim()
            End Set
        End Property

        Private _password As String = My.Settings.ConDefaultPassword

        <LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePassword"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPassword"),
            PasswordPropertyText(True)>
        Public Overridable Property Password As String
            Get
                Return GetInheritedPropertyValue("Password", _password)
            End Get
            Set
                _password = value
            End Set
        End Property

        Private _domain As String = My.Settings.ConDefaultDomain

        <LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDomain"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDomain")>
        Public Property Domain As String
            Get
                Return GetInheritedPropertyValue("Domain", _domain).Trim()
            End Get
            Set
                _domain = value.Trim()
            End Set
        End Property

#End Region

#Region "Protocol"

        Private _protocol As Protocols = Misc.StringToEnum(GetType(Protocols), My.Settings.ConDefaultProtocol)

        <LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameProtocol"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionProtocol"),
            TypeConverter(GetType(Misc.EnumTypeConverter))>
        Public Overridable Property Protocol As Protocols
            Get
                Return GetInheritedPropertyValue("Protocol", _protocol)
            End Get
            Set
                _protocol = value
            End Set
        End Property

        Private _extApp As String = My.Settings.ConDefaultExtApp

        <LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameExternalTool"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionExternalTool"),
            TypeConverter(GetType(ExternalToolsTypeConverter))>
        Public Property ExtApp As String
            Get
                Return GetInheritedPropertyValue("ExtApp", _extApp)
            End Get
            Set
                _extApp = value
            End Set
        End Property

        Private _port As Integer

        <LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePort"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPort")>
        Public Overridable Property Port As Integer
            Get
                Return GetInheritedPropertyValue("Port", _port)
            End Get
            Set
                _port = value
            End Set
        End Property

        Private _puttySession As String = My.Settings.ConDefaultPuttySession

        <LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePuttySession"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPuttySession"),
            TypeConverter(GetType(Sessions.SessionList))>
        Public Overridable Property PuttySession As String
            Get
                Return GetInheritedPropertyValue("PuttySession", _puttySession)
            End Get
            Set
                _puttySession = value
            End Set
        End Property

        Private _useConsoleSession As Boolean = My.Settings.ConDefaultUseConsoleSession

        <LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUseConsoleSession"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUseConsoleSession"),
            TypeConverter(GetType(Misc.YesNoTypeConverter))>
        Public Property UseConsoleSession As Boolean
            Get
                Return GetInheritedPropertyValue("UseConsoleSession", _useConsoleSession)
            End Get
            Set
                _useConsoleSession = value
            End Set
        End Property

        Private _
            _rdpAuthenticationLevel As RDP.AuthenticationLevel = Misc.StringToEnum(GetType(RDP.AuthenticationLevel),
                                                                                   My.Settings.
                                                                                      ConDefaultRDPAuthenticationLevel)

        <LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameAuthenticationLevel"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAuthenticationLevel"),
            TypeConverter(GetType(Misc.EnumTypeConverter))>
        Public Property RDPAuthenticationLevel As RDP.AuthenticationLevel
            Get
                Return GetInheritedPropertyValue("RDPAuthenticationLevel", _rdpAuthenticationLevel)
            End Get
            Set
                _rdpAuthenticationLevel = value
            End Set
        End Property

        Private _loadBalanceInfo As String = My.Settings.ConDefaultLoadBalanceInfo

        <LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameLoadBalanceInfo"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionLoadBalanceInfo")>
        Public Property LoadBalanceInfo As String
            Get
                Return GetInheritedPropertyValue("LoadBalanceInfo", _loadBalanceInfo).Trim()
            End Get
            Set
                _loadBalanceInfo = value.Trim()
            End Set
        End Property

        Private _
            _renderingEngine As HTTPBase.RenderingEngine = Misc.StringToEnum(GetType(HTTPBase.RenderingEngine),
                                                                             My.Settings.ConDefaultRenderingEngine)

        <LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRenderingEngine"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRenderingEngine"),
            TypeConverter(GetType(Misc.EnumTypeConverter))>
        Public Property RenderingEngine As HTTPBase.RenderingEngine
            Get
                Return GetInheritedPropertyValue("RenderingEngine", _renderingEngine)
            End Get
            Set
                _renderingEngine = value
            End Set
        End Property

        Private _useCredSsp As Boolean = My.Settings.ConDefaultUseCredSsp

        <LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 3),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUseCredSsp"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUseCredSsp"),
            TypeConverter(GetType(Misc.YesNoTypeConverter))>
        Public Property UseCredSsp As Boolean
            Get
                Return GetInheritedPropertyValue("UseCredSsp", _useCredSsp)
            End Get
            Set
                _useCredSsp = value
            End Set
        End Property

#End Region

#Region "RD Gateway"

        Private _
            _rdGatewayUsageMethod As RDP.RDGatewayUsageMethod = Misc.StringToEnum(GetType(RDP.RDGatewayUsageMethod),
                                                                                  My.Settings.
                                                                                     ConDefaultRDGatewayUsageMethod)

        <LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayUsageMethod"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayUsageMethod"),
            TypeConverter(GetType(Misc.EnumTypeConverter))>
        Public Property RDGatewayUsageMethod As RDP.RDGatewayUsageMethod
            Get
                Return GetInheritedPropertyValue("RDGatewayUsageMethod", _rdGatewayUsageMethod)
            End Get
            Set
                _rdGatewayUsageMethod = value
            End Set
        End Property

        Private _rdGatewayHostname As String = My.Settings.ConDefaultRDGatewayHostname

        <LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayHostname"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayHostname")>
        Public Property RDGatewayHostname As String
            Get
                Return GetInheritedPropertyValue("RDGatewayHostname", _rdGatewayHostname).Trim()
            End Get
            Set
                _rdGatewayHostname = value.Trim()
            End Set
        End Property

        Private _
            _rdGatewayUseConnectionCredentials As RDP.RDGatewayUseConnectionCredentials =
                Misc.StringToEnum(GetType(RDP.RDGatewayUseConnectionCredentials),
                                  My.Settings.ConDefaultRDGatewayUseConnectionCredentials)

        <LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayUseConnectionCredentials"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayUseConnectionCredentials"),
            TypeConverter(GetType(Misc.EnumTypeConverter))>
        Public Property RDGatewayUseConnectionCredentials As RDP.RDGatewayUseConnectionCredentials
            Get
                Return _
                    GetInheritedPropertyValue("RDGatewayUseConnectionCredentials", _rdGatewayUseConnectionCredentials)
            End Get
            Set
                _rdGatewayUseConnectionCredentials = value
            End Set
        End Property

        Private _rdGatewayUsername As String = My.Settings.ConDefaultRDGatewayUsername

        <LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayUsername"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayUsername")>
        Public Property RDGatewayUsername As String
            Get
                Return GetInheritedPropertyValue("RDGatewayUsername", _rdGatewayUsername).Trim()
            End Get
            Set
                _rdGatewayUsername = value.Trim()
            End Set
        End Property

        Private _rdGatewayPassword As String = My.Settings.ConDefaultRDGatewayPassword

        <LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayPassword"),
            LocalizedAttributes.LocalizedDescription("strPropertyNameRDGatewayPassword"),
            PasswordPropertyText(True)>
        Public Property RDGatewayPassword As String
            Get
                Return GetInheritedPropertyValue("RDGatewayPassword", _rdGatewayPassword)
            End Get
            Set
                _rdGatewayPassword = value
            End Set
        End Property

        Private _rdGatewayDomain As String = My.Settings.ConDefaultRDGatewayDomain

        <LocalizedAttributes.LocalizedCategory("strCategoryGateway", 4),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRDGatewayDomain"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRDGatewayDomain")>
        Public Property RDGatewayDomain As String
            Get
                Return GetInheritedPropertyValue("RDGatewayDomain", _rdGatewayDomain).Trim()
            End Get
            Set
                _rdGatewayDomain = value.Trim()
            End Set
        End Property

#End Region

#Region "Appearance"

        Private _
            _resolution As RDP.RDPResolutions = Misc.StringToEnum(GetType(RDP.RDPResolutions),
                                                                  My.Settings.ConDefaultResolution)

        <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameResolution"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionResolution"),
            TypeConverter(GetType(Misc.EnumTypeConverter))>
        Public Property Resolution As RDP.RDPResolutions
            Get
                Return GetInheritedPropertyValue("Resolution", _resolution)
            End Get
            Set
                _resolution = value
            End Set
        End Property

        Private _automaticResize As Boolean = My.Settings.ConDefaultAutomaticResize

        <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameAutomaticResize"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAutomaticResize"),
            TypeConverter(GetType(Misc.YesNoTypeConverter))>
        Public Property AutomaticResize As Boolean
            Get
                Return GetInheritedPropertyValue("AutomaticResize", _automaticResize)
            End Get
            Set
                _automaticResize = value
            End Set
        End Property

        Private _colors As RDP.RDPColors = Misc.StringToEnum(GetType(RDP.RDPColors), My.Settings.ConDefaultColors)

        <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameColors"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionColors"),
            TypeConverter(GetType(Misc.EnumTypeConverter))>
        Public Property Colors As RDP.RDPColors
            Get
                Return GetInheritedPropertyValue("Colors", _colors)
            End Get
            Set
                _colors = value
            End Set
        End Property

        Private _cacheBitmaps As Boolean = My.Settings.ConDefaultCacheBitmaps

        <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameCacheBitmaps"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionCacheBitmaps"),
            TypeConverter(GetType(Misc.YesNoTypeConverter))>
        Public Property CacheBitmaps As Boolean
            Get
                Return GetInheritedPropertyValue("CacheBitmaps", _cacheBitmaps)
            End Get
            Set
                _cacheBitmaps = value
            End Set
        End Property

        Private _displayWallpaper As Boolean = My.Settings.ConDefaultDisplayWallpaper

        <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDisplayWallpaper"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDisplayWallpaper"),
            TypeConverter(GetType(Misc.YesNoTypeConverter))>
        Public Property DisplayWallpaper As Boolean
            Get
                Return GetInheritedPropertyValue("DisplayWallpaper", _displayWallpaper)
            End Get
            Set
                _displayWallpaper = value
            End Set
        End Property

        Private _displayThemes As Boolean = My.Settings.ConDefaultDisplayThemes

        <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDisplayThemes"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDisplayThemes"),
            TypeConverter(GetType(Misc.YesNoTypeConverter))>
        Public Property DisplayThemes As Boolean
            Get
                Return GetInheritedPropertyValue("DisplayThemes", _displayThemes)
            End Get
            Set
                _displayThemes = value
            End Set
        End Property

        Private _enableFontSmoothing As Boolean = My.Settings.ConDefaultEnableFontSmoothing

        <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameEnableFontSmoothing"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEnableFontSmoothing"),
            TypeConverter(GetType(Misc.YesNoTypeConverter))>
        Public Property EnableFontSmoothing As Boolean
            Get
                Return GetInheritedPropertyValue("EnableFontSmoothing", _enableFontSmoothing)
            End Get
            Set
                _enableFontSmoothing = value
            End Set
        End Property

        Private _enableDesktopComposition As Boolean = My.Settings.ConDefaultEnableDesktopComposition

        <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameEnableDesktopComposition"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEnableDesktopComposition"),
            TypeConverter(GetType(Misc.YesNoTypeConverter))>
        Public Property EnableDesktopComposition As Boolean
            Get
                Return GetInheritedPropertyValue("EnableDesktopComposition", _enableDesktopComposition)
            End Get
            Set
                _enableDesktopComposition = value
            End Set
        End Property

#End Region

#Region "Redirect"

        Private _redirectKeys As Boolean = My.Settings.ConDefaultRedirectKeys

        <LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectKeys"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectKeys"),
            TypeConverter(GetType(Misc.YesNoTypeConverter))>
        Public Property RedirectKeys As Boolean
            Get
                Return GetInheritedPropertyValue("RedirectKeys", _redirectKeys)
            End Get
            Set
                _redirectKeys = value
            End Set
        End Property

        Private _redirectDiskDrives As Boolean = My.Settings.ConDefaultRedirectDiskDrives

        <LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectDrives"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectDrives"),
            TypeConverter(GetType(Misc.YesNoTypeConverter))>
        Public Property RedirectDiskDrives As Boolean
            Get
                Return GetInheritedPropertyValue("RedirectDiskDrives", _redirectDiskDrives)
            End Get
            Set
                _redirectDiskDrives = value
            End Set
        End Property

        Private _redirectPrinters As Boolean = My.Settings.ConDefaultRedirectPrinters

        <LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectPrinters"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectPrinters"),
            TypeConverter(GetType(Misc.YesNoTypeConverter))>
        Public Property RedirectPrinters As Boolean
            Get
                Return GetInheritedPropertyValue("RedirectPrinters", _redirectPrinters)
            End Get
            Set
                _redirectPrinters = value
            End Set
        End Property

        Private _redirectPorts As Boolean = My.Settings.ConDefaultRedirectPorts

        <LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectPorts"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectPorts"),
            TypeConverter(GetType(Misc.YesNoTypeConverter))>
        Public Property RedirectPorts As Boolean
            Get
                Return GetInheritedPropertyValue("RedirectPorts", _redirectPorts)
            End Get
            Set
                _redirectPorts = value
            End Set
        End Property

        Private _redirectSmartCards As Boolean = My.Settings.ConDefaultRedirectSmartCards

        <LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectSmartCards"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectSmartCards"),
            TypeConverter(GetType(Misc.YesNoTypeConverter))>
        Public Property RedirectSmartCards As Boolean
            Get
                Return GetInheritedPropertyValue("RedirectSmartCards", _redirectSmartCards)
            End Get
            Set
                _redirectSmartCards = value
            End Set
        End Property

        Private _
            _redirectSound As RDP.RDPSounds = Misc.StringToEnum(GetType(RDP.RDPSounds),
                                                                My.Settings.ConDefaultRedirectSound)

        <LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 6),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameRedirectSounds"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRedirectSounds"),
            TypeConverter(GetType(Misc.EnumTypeConverter))>
        Public Property RedirectSound As RDP.RDPSounds
            Get
                Return GetInheritedPropertyValue("RedirectSound", _redirectSound)
            End Get
            Set
                _redirectSound = value
            End Set
        End Property

#End Region

#Region "Misc"

        Private _preExtApp As String = My.Settings.ConDefaultPreExtApp

        <LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameExternalToolBefore"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionExternalToolBefore"),
            TypeConverter(GetType(ExternalToolsTypeConverter))>
        Public Overridable Property PreExtApp As String
            Get
                Return GetInheritedPropertyValue("PreExtApp", _preExtApp)
            End Get
            Set
                _preExtApp = value
            End Set
        End Property

        Private _postExtApp As String = My.Settings.ConDefaultPostExtApp

        <LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameExternalToolAfter"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionExternalToolAfter"),
            TypeConverter(GetType(ExternalToolsTypeConverter))>
        Public Overridable Property PostExtApp As String
            Get
                Return GetInheritedPropertyValue("PostExtApp", _postExtApp)
            End Get
            Set
                _postExtApp = value
            End Set
        End Property

        Private _macAddress As String = My.Settings.ConDefaultMacAddress

        <LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameMACAddress"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionMACAddress")>
        Public Overridable Property MacAddress As String
            Get
                Return GetInheritedPropertyValue("MacAddress", _macAddress)
            End Get
            Set
                _macAddress = value
            End Set
        End Property

        Private _userField As String = My.Settings.ConDefaultUserField

        <LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUser1"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUser1")>
        Public Overridable Property UserField As String
            Get
                Return GetInheritedPropertyValue("UserField", _userField)
            End Get
            Set
                _userField = value
            End Set
        End Property

#End Region

#Region "VNC"

        Private _
            _vncCompression As VNC.Compression = Misc.StringToEnum(GetType(VNC.Compression),
                                                                   My.Settings.ConDefaultVNCCompression)

        <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            Browsable(False),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameCompression"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionCompression"),
            TypeConverter(GetType(Misc.EnumTypeConverter))>
        Public Property VNCCompression As VNC.Compression
            Get
                Return GetInheritedPropertyValue("VNCCompression", _vncCompression)
            End Get
            Set
                _vncCompression = value
            End Set
        End Property

        Private _
            _vncEncoding As VNC.Encoding = Misc.StringToEnum(GetType(VNC.Encoding), My.Settings.ConDefaultVNCEncoding)

        <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            Browsable(False),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameEncoding"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionEncoding"),
            TypeConverter(GetType(Misc.EnumTypeConverter))>
        Public Property VNCEncoding As VNC.Encoding
            Get
                Return GetInheritedPropertyValue("VNCEncoding", _vncEncoding)
            End Get
            Set
                _vncEncoding = value
            End Set
        End Property


        Private _
            _vncAuthMode As VNC.AuthMode = Misc.StringToEnum(GetType(VNC.AuthMode), My.Settings.ConDefaultVNCAuthMode)

        <LocalizedAttributes.LocalizedCategory("strCategoryConnection", 2),
            Browsable(False),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameAuthenticationMode"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionAuthenticationMode"),
            TypeConverter(GetType(Misc.EnumTypeConverter))>
        Public Property VNCAuthMode As VNC.AuthMode
            Get
                Return GetInheritedPropertyValue("VNCAuthMode", _vncAuthMode)
            End Get
            Set
                _vncAuthMode = value
            End Set
        End Property

        Private _
            _vncProxyType As VNC.ProxyType = Misc.StringToEnum(GetType(VNC.ProxyType),
                                                               My.Settings.ConDefaultVNCProxyType)

        <LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            Browsable(False),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyType"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyType"),
            TypeConverter(GetType(Misc.EnumTypeConverter))>
        Public Property VNCProxyType As VNC.ProxyType
            Get
                Return GetInheritedPropertyValue("VNCProxyType", _vncProxyType)
            End Get
            Set
                _vncProxyType = value
            End Set
        End Property

        Private _vncProxyIP As String = My.Settings.ConDefaultVNCProxyIP

        <LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            Browsable(False),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyAddress"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyAddress")>
        Public Property VNCProxyIP As String
            Get
                Return GetInheritedPropertyValue("VNCProxyIP", _vncProxyIP)
            End Get
            Set
                _vncProxyIP = value
            End Set
        End Property

        Private _vncProxyPort As Integer = My.Settings.ConDefaultVNCProxyPort

        <LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            Browsable(False),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyPort"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyPort")>
        Public Property VNCProxyPort As Integer
            Get
                Return GetInheritedPropertyValue("VNCProxyPort", _vncProxyPort)
            End Get
            Set
                _vncProxyPort = value
            End Set
        End Property

        Private _vncProxyUsername As String = My.Settings.ConDefaultVNCProxyUsername

        <LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            Browsable(False),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyUsername"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyUsername")>
        Public Property VNCProxyUsername As String
            Get
                Return GetInheritedPropertyValue("VNCProxyUsername", _vncProxyUsername)
            End Get
            Set
                _vncProxyUsername = value
            End Set
        End Property

        Private _vncProxyPassword As String = My.Settings.ConDefaultVNCProxyPassword

        <LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 7),
            Browsable(False),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameVNCProxyPassword"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionVNCProxyPassword"),
            PasswordPropertyText(True)>
        Public Property VNCProxyPassword As String
            Get
                Return GetInheritedPropertyValue("VNCProxyPassword", _vncProxyPassword)
            End Get
            Set
                _vncProxyPassword = value
            End Set
        End Property

        Private _vncColors As VNC.Colors = Misc.StringToEnum(GetType(VNC.Colors), My.Settings.ConDefaultVNCColors)

        <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            Browsable(False),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameColors"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionColors"),
            TypeConverter(GetType(Misc.EnumTypeConverter))>
        Public Property VNCColors As VNC.Colors
            Get
                Return GetInheritedPropertyValue("VNCColors", _vncColors)
            End Get
            Set
                _vncColors = value
            End Set
        End Property

        Private _
            _vncSmartSizeMode As VNC.SmartSizeMode = Misc.StringToEnum(GetType(VNC.SmartSizeMode),
                                                                       My.Settings.ConDefaultVNCSmartSizeMode)

        <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameSmartSizeMode"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionSmartSizeMode"),
            TypeConverter(GetType(Misc.EnumTypeConverter))>
        Public Property VNCSmartSizeMode As VNC.SmartSizeMode
            Get
                Return GetInheritedPropertyValue("VNCSmartSizeMode", _vncSmartSizeMode)
            End Get
            Set
                _vncSmartSizeMode = value
            End Set
        End Property

        Private _vncViewOnly As Boolean = My.Settings.ConDefaultVNCViewOnly

        <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 5),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameViewOnly"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionViewOnly"),
            TypeConverter(GetType(Misc.YesNoTypeConverter))>
        Public Property VNCViewOnly As Boolean
            Get
                Return GetInheritedPropertyValue("VNCViewOnly", _vncViewOnly)
            End Get
            Set
                _vncViewOnly = value
            End Set
        End Property

#End Region

        <Browsable(False)>
        Public Property Inherit As New Inheritance(Me)

        <Browsable(False)>
        Public Property OpenConnections As New ProtoList

        <Browsable(False)>
        Public Property IsContainer As Boolean = False

        <Browsable(False)>
        Public Property IsDefault As Boolean = False

        <Browsable(False)>
        Public Property Parent As Container.Info

        <Browsable(False)>
        Public Property PositionID As Integer = 0

        <Browsable(False)>
        Public Property ConstantID As String = Misc.CreateConstantID

        <Browsable(False)>
        Public Property TreeNode As TreeNode

        <Browsable(False)>
        Public Property IsQuickConnect As Boolean = False

        <Browsable(False)>
        Public Property PleaseConnect As Boolean = False

#End Region

#Region "Constructors"

        Public Sub New()
            SetDefaults()
        End Sub

        Public Sub New(parent As Container.Info)
            SetDefaults()
            IsContainer = True
            Me.Parent = parent
        End Sub

#End Region

#Region "Public Methods"

        Public Function Copy() As Info
            Dim newConnectionInfo As Info = MemberwiseClone()
            newConnectionInfo.ConstantID = Misc.CreateConstantID()
            newConnectionInfo._OpenConnections = New ProtoList
            Return newConnectionInfo
        End Function

        Public Sub SetDefaults()
            If Port = 0 Then SetDefaultPort()
        End Sub

        Public Function GetDefaultPort() As Integer
            Return GetDefaultPort(Protocol)
        End Function

        Public Sub SetDefaultPort()
            Port = GetDefaultPort()
        End Sub

#End Region

#Region "Public Enumerations"

        <Flags>
        Public Enum Force
            None = 0
            UseConsoleSession = 1
            Fullscreen = 2
            DoNotJump = 4
            OverridePanel = 8
            DontUseConsoleSession = 16
            NoCredentials = 32
        End Enum

#End Region

#Region "Private Methods"

        Private Function GetInheritedPropertyValue (Of TPropertyType)(propertyName As String, value As TPropertyType) _
            As TPropertyType
            Dim inheritType As Type = Inherit.GetType()
            Dim inheritPropertyInfo As PropertyInfo = inheritType.GetProperty(propertyName)
            Dim inheritPropertyValue As Boolean = inheritPropertyInfo.GetValue(Inherit, BindingFlags.GetProperty,
                                                                               Nothing, Nothing, Nothing)

            If inheritPropertyValue And Parent IsNot Nothing Then
                Dim parentConnectionInfo As Info
                If IsContainer Then
                    parentConnectionInfo = Parent.Parent.ConnectionInfo
                Else
                    parentConnectionInfo = Parent.ConnectionInfo
                End If

                Dim connectionInfoType As Type = parentConnectionInfo.GetType()
                Dim parentPropertyInfo As PropertyInfo = connectionInfoType.GetProperty(propertyName)
                Dim parentPropertyValue As TPropertyType = parentPropertyInfo.GetValue(parentConnectionInfo,
                                                                                       BindingFlags.GetProperty, Nothing,
                                                                                       Nothing, Nothing)

                Return parentPropertyValue
            Else
                Return value
            End If
        End Function

        Private Shared Function GetDefaultPort(protocol As Protocols) As Integer
            Try
                Select Case protocol
                    Case Protocols.RDP
                        Return RDP.Defaults.Port
                    Case Protocols.VNC
                        Return VNC.Defaults.Port
                    Case Protocols.SSH1
                        Return SSH1.Defaults.Port
                    Case Protocols.SSH2
                        Return SSH2.Defaults.Port
                    Case Protocols.Telnet
                        Return Telnet.Defaults.Port
                    Case Protocols.Rlogin
                        Return Rlogin.Defaults.Port
                    Case Protocols.RAW
                        Return RAW.Defaults.Port
                    Case Protocols.HTTP
                        Return HTTP.Defaults.Port
                    Case Protocols.HTTPS
                        Return HTTPS.Defaults.Port
                    Case Protocols.IntApp
                        Return IntegratedProgram.Defaults.Port
                End Select
            Catch ex As Exception
                Runtime.MessageCollector.AddExceptionMessage(Language.Language.strConnectionSetDefaultPortFailed, ex,
                                                             MessageClass.ErrorMsg)
            End Try
        End Function

#End Region
    End Class
End Namespace