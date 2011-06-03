Imports System.Windows.Forms
Imports System.ComponentModel
Imports mRemoteNG.Tools.LocalizedAttributes
Imports mRemoteNG.App.Runtime

Namespace Connection
    <DefaultProperty("Name")> _
    Public Class Info
#Region "Properties"
#Region "1 Display"
        Private _Name As String = My.Resources.strNewConnection
        <LocalizedCategory("strCategoryDisplay", 1), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameName"), _
            LocalizedDescription("strPropertyDescriptionName")> _
        Public Property Name() As String
            Get
                Return Me._Name
            End Get
            Set(ByVal value As String)
                Me._Name = value
            End Set
        End Property

        Private _Description As String = My.Settings.ConDefaultDescription
        <LocalizedCategory("strCategoryDisplay", 1), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameDescription"), _
            LocalizedDescription("strPropertyDescriptionDescription")> _
        Public Property Description() As String
            Get
                If Me._Inherit.Description And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.Description
                Else
                    Return Me._Description
                End If
            End Get
            Set(ByVal value As String)
                Me._Description = value
            End Set
        End Property

        Private _Icon As String = My.Settings.ConDefaultIcon
        <LocalizedCategory("strCategoryDisplay", 1), _
            TypeConverter(GetType(mRemoteNG.Connection.Icon)), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameIcon"), _
            LocalizedDescription("strPropertyDescriptionIcon")> _
        Public Property Icon() As String
            Get
                If Me._Inherit.Icon And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.Icon
                Else
                    Return Me._Icon
                End If
            End Get
            Set(ByVal value As String)
                Me._Icon = value
            End Set
        End Property

        Private _Panel As String = My.Resources.strGeneral
        <LocalizedCategory("strCategoryDisplay", 1), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNamePanel"), _
            LocalizedDescription("strPropertyDescriptionPanel")> _
            Public Property Panel() As String
            Get
                If Me._Inherit.Panel And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.Panel
                Else
                    Return Me._Panel
                End If
            End Get
            Set(ByVal value As String)
                Me._Panel = value
            End Set
        End Property
#End Region
#Region "2 Connection"
        Private _Hostname As String = ""
        <LocalizedCategory("strCategoryConnection", 2), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameAddress"), _
            LocalizedDescription("strPropertyDescriptionAddress")> _
        Public Property Hostname() As String
            Get
                Return Me._Hostname
            End Get
            Set(ByVal value As String)
                Me._Hostname = value
            End Set
        End Property

        Private _Username As String = My.Settings.ConDefaultUsername
        <LocalizedCategory("strCategoryConnection", 2), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameUsername"), _
            LocalizedDescription("strPropertyDescriptionUsername")> _
        Public Property Username() As String
            Get
                If Me._Inherit.Username And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.Username
                Else
                    Return Me._Username
                End If
            End Get
            Set(ByVal value As String)
                Me._Username = value
            End Set
        End Property

        Private _Password As String = My.Settings.ConDefaultPassword
        <LocalizedCategory("strCategoryConnection", 2), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNamePassword"), _
            LocalizedDescription("strPropertyDescriptionPassword"), _
            PasswordPropertyText(True)> _
        Public Property Password() As String
            Get
                If Me._Inherit.Password And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.Password
                Else
                    Return Me._Password
                End If
            End Get
            Set(ByVal value As String)
                Me._Password = value
            End Set
        End Property

        Private _Domain As String = My.Settings.ConDefaultDomain
        <LocalizedCategory("strCategoryConnection", 2), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameDomain"), _
            LocalizedDescription("strPropertyDescriptionDomain")> _
        Public Property Domain() As String
            Get
                If Me._Inherit.Domain And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.Domain
                Else
                    Return Me._Domain
                End If
            End Get
            Set(ByVal value As String)
                Me._Domain = value
            End Set
        End Property
#End Region
#Region "3 Protocol"
        Private _Protocol As Connection.Protocol.Protocols = Tools.Misc.StringToEnum(GetType(Connection.Protocol.Protocols), My.Settings.ConDefaultProtocol)
        <LocalizedCategory("strCategoryProtocol", 3), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameProtocol"), _
            LocalizedDescription("strPropertyDescriptionProtocol"), _
            TypeConverter(GetType(Tools.Misc.EnumTypeConverter))> _
        Public Property Protocol() As Connection.Protocol.Protocols
            Get
                If Me._Inherit.Protocol And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.Protocol
                Else
                    Return Me._Protocol
                End If
            End Get
            Set(ByVal value As Connection.Protocol.Protocols)
                Me._Protocol = value
            End Set
        End Property

        Private _ExtApp As String = My.Settings.ConDefaultExtApp
        <LocalizedCategory("strCategoryProtocol", 3), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameExternalTool"), _
            LocalizedDescription("strPropertyDescriptionExternalTool"), _
            TypeConverter(GetType(Tools.ExternalAppsTypeConverter))> _
        Public Property ExtApp() As String
            Get
                If Me._Inherit.ExtApp And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.ExtApp
                Else
                    Return Me._ExtApp
                End If
            End Get
            Set(ByVal value As String)
                Me._ExtApp = value
            End Set
        End Property

        Private _Port As Integer
        <LocalizedCategory("strCategoryProtocol", 3), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNamePort"), _
            LocalizedDescription("strPropertyDescriptionPort")> _
        Public Property Port() As Integer
            Get
                If Me._Inherit.Port And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.Port
                Else
                    Return Me._Port
                End If
            End Get
            Set(ByVal value As Integer)
                Me._Port = value
            End Set
        End Property

        Private _PuttySession As String = My.Settings.ConDefaultPuttySession
        <LocalizedCategory("strCategoryProtocol", 3), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNamePuttySession"), _
            LocalizedDescription("strPropertyDescriptionPuttySession"), _
            TypeConverter(GetType(mRemoteNG.Connection.PuttySession))> _
        Public Property PuttySession() As String
            Get
                If Me._Inherit.PuttySession And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.PuttySession
                Else
                    Return Me._PuttySession
                End If
            End Get
            Set(ByVal value As String)
                Me._PuttySession = value
            End Set
        End Property

        Private _ICAEncryption As Connection.Protocol.ICA.EncryptionStrength = Tools.Misc.StringToEnum(GetType(mRemoteNG.Connection.Protocol.ICA.EncryptionStrength), My.Settings.ConDefaultICAEncryptionStrength)
        <LocalizedCategory("strCategoryProtocol", 3), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameEncryptionStrength"), _
            LocalizedDescription("strPropertyDescriptionEncryptionStrength"), _
            TypeConverter(GetType(Tools.Misc.EnumTypeConverter))> _
        Public Property ICAEncryption() As Connection.Protocol.ICA.EncryptionStrength
            Get
                If Me._Inherit.ICAEncryption And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.ICAEncryption
                Else
                    Return Me._ICAEncryption
                End If
            End Get
            Set(ByVal value As Connection.Protocol.ICA.EncryptionStrength)
                _ICAEncryption = value
            End Set
        End Property

        Private _UseConsoleSession As Boolean = My.Settings.ConDefaultUseConsoleSession
        <LocalizedCategory("strCategoryProtocol", 3), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameUseConsoleSession"), _
            LocalizedDescription("strPropertyDescriptionUseConsoleSession"), _
            TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
        Public Property UseConsoleSession() As Boolean
            Get
                If Me._Inherit.UseConsoleSession And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.UseConsoleSession
                Else
                    Return Me._UseConsoleSession
                End If
            End Get
            Set(ByVal value As Boolean)
                Me._UseConsoleSession = value
            End Set
        End Property

        Private _RDPAuthenticationLevel As Connection.Protocol.RDP.AuthenticationLevel = Tools.Misc.StringToEnum(GetType(mRemoteNG.Connection.Protocol.RDP.AuthenticationLevel), My.Settings.ConDefaultRDPAuthenticationLevel)
        <LocalizedCategory("strCategoryProtocol", 3), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameAuthenticationLevel"), _
            LocalizedDescription("strPropertyDescriptionAuthenticationLevel"), _
            TypeConverter(GetType(Tools.Misc.EnumTypeConverter))> _
        Public Property RDPAuthenticationLevel() As Connection.Protocol.RDP.AuthenticationLevel
            Get
                If Me._Inherit.RDPAuthenticationLevel And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.RDPAuthenticationLevel
                Else
                    Return Me._RDPAuthenticationLevel
                End If
            End Get
            Set(ByVal value As Connection.Protocol.RDP.AuthenticationLevel)
                _RDPAuthenticationLevel = value
            End Set
        End Property

        Private _RenderingEngine As Connection.Protocol.HTTPBase.RenderingEngine = Tools.Misc.StringToEnum(GetType(mRemoteNG.Connection.Protocol.HTTPBase.RenderingEngine), My.Settings.ConDefaultRenderingEngine)
        <LocalizedCategory("strCategoryProtocol", 3), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameRenderingEngine"), _
            LocalizedDescription("strPropertyDescriptionRenderingEngine"), _
            TypeConverter(GetType(Tools.Misc.EnumTypeConverter))> _
        Public Property RenderingEngine() As Connection.Protocol.HTTPBase.RenderingEngine
            Get
                If Me._Inherit.RenderingEngine And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.RenderingEngine
                Else
                    Return Me._RenderingEngine
                End If
            End Get
            Set(ByVal value As Connection.Protocol.HTTPBase.RenderingEngine)
                _RenderingEngine = value
            End Set
        End Property
#End Region
#Region "4 RD Gateway"
        Private _RDGatewayUsageMethod As mRemoteNG.Connection.Protocol.RDP.RDGatewayUsageMethod = Tools.Misc.StringToEnum(GetType(Connection.Protocol.RDP.RDGatewayUsageMethod), My.Settings.ConDefaultRDGatewayUsageMethod)
        <LocalizedCategory("strCategoryGateway", 4), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameRDGatewayUsageMethod"), _
            LocalizedDescription("strPropertyDescriptionRDGatewayUsageMethod"), _
            TypeConverter(GetType(Tools.Misc.EnumTypeConverter))> _
        Public Property RDGatewayUsageMethod() As mRemoteNG.Connection.Protocol.RDP.RDGatewayUsageMethod
            Get
                If Me._Inherit.RDGatewayUsageMethod And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.RDGatewayUsageMethod
                Else
                    Return _RDGatewayUsageMethod
                End If
            End Get
            Set(ByVal value As mRemoteNG.Connection.Protocol.RDP.RDGatewayUsageMethod)
                _RDGatewayUsageMethod = value
            End Set
        End Property

        Private _RDGatewayHostname As String = My.Settings.ConDefaultRDGatewayHostname
        <LocalizedCategory("strCategoryGateway", 4), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameRDGatewayHostname"), _
            LocalizedDescription("strPropertyDescriptionRDGatewayHostname")> _
        Public Property RDGatewayHostname() As String
            Get
                If Me._Inherit.RDGatewayHostname And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.RDGatewayHostname
                Else
                    Return Me._RDGatewayHostname
                End If
            End Get
            Set(ByVal value As String)
                Me._RDGatewayHostname = value
            End Set
        End Property

        Private _RDGatewayUseConnectionCredentials As mRemoteNG.Connection.Protocol.RDP.RDGatewayUseConnectionCredentials = Tools.Misc.StringToEnum(GetType(Connection.Protocol.RDP.RDGatewayUseConnectionCredentials), My.Settings.ConDefaultRDGatewayUseConnectionCredentials)
        <LocalizedCategory("strCategoryGateway", 4), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameRDGatewayUseConnectionCredentials"), _
            LocalizedDescription("strPropertyDescriptionRDGatewayUseConnectionCredentials"), _
            TypeConverter(GetType(Tools.Misc.EnumTypeConverter))> _
        Public Property RDGatewayUseConnectionCredentials() As mRemoteNG.Connection.Protocol.RDP.RDGatewayUseConnectionCredentials
            Get
                If Me._Inherit.RDGatewayUseConnectionCredentials And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.RDGatewayUseConnectionCredentials
                Else
                    Return _RDGatewayUseConnectionCredentials
                End If
            End Get
            Set(ByVal value As mRemoteNG.Connection.Protocol.RDP.RDGatewayUseConnectionCredentials)
                _RDGatewayUseConnectionCredentials = value
            End Set
        End Property

        Private _RDGatewayUsername As String = My.Settings.ConDefaultRDGatewayUsername
        <LocalizedCategory("strCategoryGateway", 4), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameRDGatewayUsername"), _
            LocalizedDescription("strPropertyDescriptionRDGatewayUsername")> _
        Public Property RDGatewayUsername() As String
            Get
                If Me._Inherit.RDGatewayUsername And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.RDGatewayUsername
                Else
                    Return Me._RDGatewayUsername
                End If
            End Get
            Set(ByVal value As String)
                Me._RDGatewayUsername = value
            End Set
        End Property

        Private _RDGatewayPassword As String = My.Settings.ConDefaultRDGatewayPassword
        <LocalizedCategory("strCategoryGateway", 4), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameRDGatewayPassword"), _
            LocalizedDescription("strPropertyNameRDGatewayPassword"), _
            PasswordPropertyText(True)> _
        Public Property RDGatewayPassword() As String
            Get
                If Me._Inherit.RDGatewayPassword And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.RDGatewayPassword
                Else
                    Return Me._RDGatewayPassword
                End If
            End Get
            Set(ByVal value As String)
                Me._RDGatewayPassword = value
            End Set
        End Property

        Private _RDGatewayDomain As String = My.Settings.ConDefaultRDGatewayDomain
        <LocalizedCategory("strCategoryGateway", 4), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameRDGatewayDomain"), _
            LocalizedDescription("strPropertyDescriptionRDGatewayDomain")> _
        Public Property RDGatewayDomain() As String
            Get
                If Me._Inherit.RDGatewayDomain And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.RDGatewayDomain
                Else
                    Return Me._RDGatewayDomain
                End If
            End Get
            Set(ByVal value As String)
                Me._RDGatewayDomain = value
            End Set
        End Property
#End Region
#Region "5 Appearance"
        Private _Resolution As Connection.Protocol.RDP.RDPResolutions = Tools.Misc.StringToEnum(GetType(Connection.Protocol.RDP.RDPResolutions), My.Settings.ConDefaultResolution)
        <LocalizedCategory("strCategoryAppearance", 5), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameResolution"), _
            LocalizedDescription("strPropertyDescriptionResolution"), _
            TypeConverter(GetType(Tools.Misc.EnumTypeConverter))> _
        Public Property Resolution() As Connection.Protocol.RDP.RDPResolutions
            Get
                If Me._Inherit.Resolution And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.Resolution
                Else
                    Return Me._Resolution
                End If
            End Get
            Set(ByVal value As Connection.Protocol.RDP.RDPResolutions)
                Me._Resolution = value
            End Set
        End Property

        Private _Colors As Connection.Protocol.RDP.RDPColors = Tools.Misc.StringToEnum(GetType(Connection.Protocol.RDP.RDPColors), My.Settings.ConDefaultColors)
        <LocalizedCategory("strCategoryAppearance", 5), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameColors"), _
            LocalizedDescription("strPropertyDescriptionColors"), _
            TypeConverter(GetType(Tools.Misc.EnumTypeConverter))> _
        Public Property Colors() As Connection.Protocol.RDP.RDPColors
            Get
                If Me._Inherit.Colors And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.Colors
                Else
                    Return Me._Colors
                End If
            End Get
            Set(ByVal value As Connection.Protocol.RDP.RDPColors)
                Me._Colors = value
            End Set
        End Property

        Private _CacheBitmaps As Boolean = My.Settings.ConDefaultCacheBitmaps
        <LocalizedCategory("strCategoryAppearance", 5), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameCacheBitmaps"), _
            LocalizedDescription("strPropertyDescriptionCacheBitmaps"), _
            TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
        Public Property CacheBitmaps() As Boolean
            Get
                If Me._Inherit.CacheBitmaps And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.CacheBitmaps
                Else
                    Return Me._CacheBitmaps
                End If
            End Get
            Set(ByVal value As Boolean)
                Me._CacheBitmaps = value
            End Set
        End Property

        Private _DisplayWallpaper As Boolean = My.Settings.ConDefaultDisplayWallpaper
        <LocalizedCategory("strCategoryAppearance", 5), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameDisplayWallpaper"), _
            LocalizedDescription("strPropertyDescriptionDisplayWallpaper"), _
            TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
        Public Property DisplayWallpaper() As Boolean
            Get
                If Me._Inherit.DisplayWallpaper And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.DisplayWallpaper
                Else
                    Return Me._DisplayWallpaper
                End If
            End Get
            Set(ByVal value As Boolean)
                Me._DisplayWallpaper = value
            End Set
        End Property

        Private _DisplayThemes As Boolean = My.Settings.ConDefaultDisplayThemes
        <LocalizedCategory("strCategoryAppearance", 5), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameDisplayThemes"), _
            LocalizedDescription("strPropertyDescriptionDisplayThemes"), _
            TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
        Public Property DisplayThemes() As Boolean
            Get
                If Me._Inherit.DisplayThemes And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.DisplayThemes
                Else
                    Return Me._DisplayThemes
                End If
            End Get
            Set(ByVal value As Boolean)
                Me._DisplayThemes = value
            End Set
        End Property

        Private _EnableFontSmoothing As Boolean = My.Settings.ConDefaultEnableFontSmoothing
        <LocalizedCategory("strCategoryAppearance", 5), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameEnableFontSmoothing"), _
            LocalizedDescription("strPropertyDescriptionEnableFontSmoothing"), _
            TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
        Public Property EnableFontSmoothing() As Boolean
            Get
                If Me._Inherit.EnableFontSmoothing And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.EnableFontSmoothing
                Else
                    Return Me._EnableFontSmoothing
                End If
            End Get
            Set(ByVal value As Boolean)
                Me._EnableFontSmoothing = value
            End Set
        End Property

        Private _EnableDesktopComposition As Boolean = My.Settings.ConDefaultEnableDesktopComposition
        <LocalizedCategory("strCategoryAppearance", 5), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameEnableDesktopComposition"), _
            LocalizedDescription("strPropertyDescriptionEnableDesktopComposition"), _
            TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
        Public Property EnableDesktopComposition() As Boolean
            Get
                If Me._Inherit.EnableDesktopComposition And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.EnableDesktopComposition
                Else
                    Return Me._EnableDesktopComposition
                End If
            End Get
            Set(ByVal value As Boolean)
                Me._EnableDesktopComposition = value
            End Set
        End Property
#End Region
#Region "6 Redirect"
        Private _RedirectKeys As Boolean = My.Settings.ConDefaultRedirectKeys
        <LocalizedCategory("strCategoryRedirect", 6), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameRedirectKeys"), _
            LocalizedDescription("strPropertyDescriptionRedirectKeys"), _
            TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
        Public Property RedirectKeys() As Boolean
            Get
                If Me._Inherit.RedirectKeys And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.RedirectKeys
                Else
                    Return Me._RedirectKeys
                End If
            End Get
            Set(ByVal value As Boolean)
                Me._RedirectKeys = value
            End Set
        End Property

        Private _RedirectDiskDrives As Boolean = My.Settings.ConDefaultRedirectDiskDrives
        <LocalizedCategory("strCategoryRedirect", 6), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameRedirectDrives"), _
            LocalizedDescription("strPropertyDescriptionRedirectDrives"), _
            TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
        Public Property RedirectDiskDrives() As Boolean
            Get
                If Me._Inherit.RedirectDiskDrives And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.RedirectDiskDrives
                Else
                    Return Me._RedirectDiskDrives
                End If
            End Get
            Set(ByVal value As Boolean)
                Me._RedirectDiskDrives = value
            End Set
        End Property

        Private _RedirectPrinters As Boolean = My.Settings.ConDefaultRedirectPrinters
        <LocalizedCategory("strCategoryRedirect", 6), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameRedirectPrinters"), _
            LocalizedDescription("strPropertyDescriptionRedirectPrinters"), _
            TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
        Public Property RedirectPrinters() As Boolean
            Get
                If Me._Inherit.RedirectPrinters And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.RedirectPrinters
                Else
                    Return Me._RedirectPrinters
                End If
            End Get
            Set(ByVal value As Boolean)
                Me._RedirectPrinters = value
            End Set
        End Property

        Private _RedirectPorts As Boolean = My.Settings.ConDefaultRedirectPorts
        <LocalizedCategory("strCategoryRedirect", 6), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameRedirectPorts"), _
            LocalizedDescription("strPropertyDescriptionRedirectPorts"), _
            TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
        Public Property RedirectPorts() As Boolean
            Get
                If Me._Inherit.RedirectPorts And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.RedirectPorts
                Else
                    Return Me._RedirectPorts
                End If
            End Get
            Set(ByVal value As Boolean)
                Me._RedirectPorts = value
            End Set
        End Property

        Private _RedirectSmartCards As Boolean = My.Settings.ConDefaultRedirectSmartCards
        <LocalizedCategory("strCategoryRedirect", 6), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameRedirectSmartCards"), _
            LocalizedDescription("strPropertyDescriptionRedirectSmartCards"), _
            TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
        Public Property RedirectSmartCards() As Boolean
            Get
                If Me._Inherit.RedirectSmartCards And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.RedirectSmartCards
                Else
                    Return Me._RedirectSmartCards
                End If
            End Get
            Set(ByVal value As Boolean)
                Me._RedirectSmartCards = value
            End Set
        End Property

        Private _RedirectSound As Connection.Protocol.RDP.RDPSounds = Tools.Misc.StringToEnum(GetType(Connection.Protocol.RDP.RDPSounds), My.Settings.ConDefaultRedirectSound)
        <LocalizedCategory("strCategoryRedirect", 6), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameRedirectSounds"), _
            LocalizedDescription("strPropertyDescriptionRedirectSounds"), _
            TypeConverter(GetType(Tools.Misc.EnumTypeConverter))> _
        Public Property RedirectSound() As Connection.Protocol.RDP.RDPSounds
            Get
                If Me._Inherit.RedirectSound And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.RedirectSound
                Else
                    Return Me._RedirectSound
                End If
            End Get
            Set(ByVal value As Connection.Protocol.RDP.RDPSounds)
                Me._RedirectSound = value
            End Set
        End Property
#End Region
#Region "7 Misc"
        Private _PreExtApp As String = My.Settings.ConDefaultPreExtApp
        <LocalizedCategory("strCategoryMiscellaneous", 7), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameExternalToolBefore"), _
            LocalizedDescription("strPropertyDescriptionExternalToolBefore"), _
            TypeConverter(GetType(Tools.ExternalAppsTypeConverter))> _
        Public Property PreExtApp() As String
            Get
                If Me._Inherit.PreExtApp And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.PreExtApp
                Else
                    Return Me._PreExtApp
                End If
            End Get
            Set(ByVal value As String)
                Me._PreExtApp = value
            End Set
        End Property

        Private _PostExtApp As String = My.Settings.ConDefaultPostExtApp
        <LocalizedCategory("strCategoryMiscellaneous", 7), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameExternalToolAfter"), _
            LocalizedDescription("strPropertyDescriptionExternalToolAfter"), _
            TypeConverter(GetType(Tools.ExternalAppsTypeConverter))> _
        Public Property PostExtApp() As String
            Get
                If Me._Inherit.PostExtApp And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.PostExtApp
                Else
                    Return Me._PostExtApp
                End If
            End Get
            Set(ByVal value As String)
                Me._PostExtApp = value
            End Set
        End Property

        Private _MacAddress As String = My.Settings.ConDefaultMacAddress
        <LocalizedCategory("strCategoryMiscellaneous", 7), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameMACAddress"), _
            LocalizedDescription("strPropertyDescriptionMACAddress")> _
        Public Property MacAddress() As String
            Get
                If Me._Inherit.MacAddress And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.MacAddress
                Else
                    Return Me._MacAddress
                End If
            End Get
            Set(ByVal value As String)
                Me._MacAddress = value
            End Set
        End Property

        Private _UserField As String = My.Settings.ConDefaultUserField
        <LocalizedCategory("strCategoryMiscellaneous", 7), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameUser1"), _
            LocalizedDescription("strPropertyDescriptionUser1")> _
        Public Property UserField() As String
            Get
                If Me._Inherit.UserField And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer = True Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.UserField
                Else
                    Return Me._UserField
                End If
            End Get
            Set(ByVal value As String)
                Me._UserField = value
            End Set
        End Property
#End Region
#Region "8 VNC"
        Private _VNCCompression As mRemoteNG.Connection.Protocol.VNC.Compression = Tools.Misc.StringToEnum(GetType(Connection.Protocol.VNC.Compression), My.Settings.ConDefaultVNCCompression)
        <LocalizedCategory("strCategoryAppearance", 5), _
           Browsable(False), _
            LocalizedDisplayName("strPropertyNameCompression"), _
            LocalizedDescription("strPropertyDescriptionCompression"), _
           TypeConverter(GetType(Tools.Misc.EnumTypeConverter))> _
       Public Property VNCCompression() As mRemoteNG.Connection.Protocol.VNC.Compression
            Get
                If Me._Inherit.VNCCompression And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.VNCCompression
                Else
                    Return _VNCCompression
                End If
            End Get
            Set(ByVal value As mRemoteNG.Connection.Protocol.VNC.Compression)
                _VNCCompression = value
            End Set
        End Property

        Private _VNCEncoding As mRemoteNG.Connection.Protocol.VNC.Encoding = Tools.Misc.StringToEnum(GetType(Connection.Protocol.VNC.Encoding), My.Settings.ConDefaultVNCEncoding)
        <LocalizedCategory("strCategoryAppearance", 5), _
            Browsable(False), _
            LocalizedDisplayName("strPropertyNameEncoding"), _
            LocalizedDescription("strPropertyDescriptionEncoding"), _
            TypeConverter(GetType(Tools.Misc.EnumTypeConverter))> _
        Public Property VNCEncoding() As mRemoteNG.Connection.Protocol.VNC.Encoding
            Get
                If Me._Inherit.VNCEncoding And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.VNCEncoding
                Else
                    Return _VNCEncoding
                End If
            End Get
            Set(ByVal value As mRemoteNG.Connection.Protocol.VNC.Encoding)
                _VNCEncoding = value
            End Set
        End Property


        Private _VNCAuthMode As mRemoteNG.Connection.Protocol.VNC.AuthMode = Tools.Misc.StringToEnum(GetType(Connection.Protocol.VNC.AuthMode), My.Settings.ConDefaultVNCAuthMode)
        <LocalizedCategory("strCategoryConnection", 2), _
            Browsable(False), _
            LocalizedDisplayName("strPropertyNameAuthenticationMode"), _
            LocalizedDescription("strPropertyDescriptionAuthenticationMode"), _
            TypeConverter(GetType(Tools.Misc.EnumTypeConverter))> _
        Public Property VNCAuthMode() As mRemoteNG.Connection.Protocol.VNC.AuthMode
            Get
                If Me._Inherit.VNCAuthMode And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.VNCAuthMode
                Else
                    Return _VNCAuthMode
                End If
            End Get
            Set(ByVal value As mRemoteNG.Connection.Protocol.VNC.AuthMode)
                _VNCAuthMode = value
            End Set
        End Property

        Private _VNCProxyType As mRemoteNG.Connection.Protocol.VNC.ProxyType = Tools.Misc.StringToEnum(GetType(Connection.Protocol.VNC.ProxyType), My.Settings.ConDefaultVNCProxyType)
        <LocalizedCategory("strCategoryMiscellaneous", 7), _
           Browsable(False), _
            LocalizedDisplayName("strPropertyNameVNCProxyType"), _
            LocalizedDescription("strPropertyDescriptionVNCProxyType"), _
           TypeConverter(GetType(Tools.Misc.EnumTypeConverter))> _
        Public Property VNCProxyType() As mRemoteNG.Connection.Protocol.VNC.ProxyType
            Get
                If Me._Inherit.VNCProxyType And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.VNCProxyType
                Else
                    Return _VNCProxyType
                End If
            End Get
            Set(ByVal value As mRemoteNG.Connection.Protocol.VNC.ProxyType)
                _VNCProxyType = value
            End Set
        End Property

        Private _VNCProxyIP As String = My.Settings.ConDefaultVNCProxyIP
        <LocalizedCategory("strCategoryMiscellaneous", 7), _
            Browsable(False), _
            LocalizedDisplayName("strPropertyNameVNCProxyAddress"), _
            LocalizedDescription("strPropertyDescriptionVNCProxyAddress")> _
        Public Property VNCProxyIP() As String
            Get
                If Me._Inherit.VNCProxyIP And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.VNCProxyIP
                Else
                    Return _VNCProxyIP
                End If
            End Get
            Set(ByVal value As String)
                _VNCProxyIP = value
            End Set
        End Property

        Private _VNCProxyPort As Integer = My.Settings.ConDefaultVNCProxyPort
        <LocalizedCategory("strCategoryMiscellaneous", 7), _
            Browsable(False), _
            LocalizedDisplayName("strPropertyNameVNCProxyPort"), _
            LocalizedDescription("strPropertyDescriptionVNCProxyPort")> _
        Public Property VNCProxyPort() As Integer
            Get
                If Me._Inherit.VNCProxyPort And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.VNCProxyPort
                Else
                    Return _VNCProxyPort
                End If
            End Get
            Set(ByVal value As Integer)
                _VNCProxyPort = value
            End Set
        End Property

        Private _VNCProxyUsername As String = My.Settings.ConDefaultVNCProxyUsername
        <LocalizedCategory("strCategoryMiscellaneous", 7), _
            Browsable(False), _
            LocalizedDisplayName("strPropertyNameVNCProxyUsername"), _
            LocalizedDescription("strPropertyDescriptionVNCProxyUsername")> _
        Public Property VNCProxyUsername() As String
            Get
                If Me._Inherit.VNCProxyUsername And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.VNCProxyUsername
                Else
                    Return _VNCProxyUsername
                End If
            End Get
            Set(ByVal value As String)
                _VNCProxyUsername = value
            End Set
        End Property

        Private _VNCProxyPassword As String = My.Settings.ConDefaultVNCProxyPassword
        <LocalizedCategory("strCategoryMiscellaneous", 7), _
            Browsable(False), _
            LocalizedDisplayName("strPropertyNameVNCProxyPassword"), _
            LocalizedDescription("strPropertyDescriptionVNCProxyPassword"), _
            PasswordPropertyText(True)> _
        Public Property VNCProxyPassword() As String
            Get
                If Me._Inherit.VNCProxyPassword And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.VNCProxyPassword
                Else
                    Return _VNCProxyPassword
                End If
            End Get
            Set(ByVal value As String)
                _VNCProxyPassword = value
            End Set
        End Property

        Private _VNCColors As mRemoteNG.Connection.Protocol.VNC.Colors = Tools.Misc.StringToEnum(GetType(Connection.Protocol.VNC.Colors), My.Settings.ConDefaultVNCColors)
        <LocalizedCategory("strCategoryAppearance", 5), _
            Browsable(False), _
            LocalizedDisplayName("strPropertyNameColors"), _
            LocalizedDescription("strPropertyDescriptionColors"), _
            TypeConverter(GetType(Tools.Misc.EnumTypeConverter))> _
       Public Property VNCColors() As mRemoteNG.Connection.Protocol.VNC.Colors
            Get
                If Me._Inherit.VNCColors And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.VNCColors
                Else
                    Return _VNCColors
                End If
            End Get
            Set(ByVal value As mRemoteNG.Connection.Protocol.VNC.Colors)
                _VNCColors = value
            End Set
        End Property

        Private _VNCSmartSizeMode As mRemoteNG.Connection.Protocol.VNC.SmartSizeMode = Tools.Misc.StringToEnum(GetType(Connection.Protocol.VNC.SmartSizeMode), My.Settings.ConDefaultVNCSmartSizeMode)
        <LocalizedCategory("strCategoryAppearance", 5), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameSmartSizeMode"), _
            LocalizedDescription("strPropertyDescriptionSmartSizeMode"), _
            TypeConverter(GetType(Tools.Misc.EnumTypeConverter))> _
        Public Property VNCSmartSizeMode() As mRemoteNG.Connection.Protocol.VNC.SmartSizeMode
            Get
                If Me._Inherit.VNCSmartSizeMode And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.VNCSmartSizeMode
                Else
                    Return _VNCSmartSizeMode
                End If
            End Get
            Set(ByVal value As mRemoteNG.Connection.Protocol.VNC.SmartSizeMode)
                _VNCSmartSizeMode = value
            End Set
        End Property

        Private _VNCViewOnly As Boolean = My.Settings.ConDefaultVNCViewOnly
        <LocalizedCategory("strCategoryAppearance", 5), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameViewOnly"), _
            LocalizedDescription("strPropertyDescriptionViewOnly"), _
            TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
        Public Property VNCViewOnly() As Boolean
            Get
                If Me._Inherit.VNCViewOnly And Me._Parent IsNot Nothing Then
                    Dim parCon As Connection.Info = TryCast(Me._Parent, Container.Info).ConnectionInfo

                    If Me._IsContainer Then
                        Dim curCont As Container.Info = Me._Parent
                        Dim parCont As Container.Info = curCont.Parent
                        parCon = parCont.ConnectionInfo
                    End If

                    Return parCon.VNCViewOnly
                Else
                    Return _VNCViewOnly
                End If
            End Get
            Set(ByVal value As Boolean)
                _VNCViewOnly = value
            End Set
        End Property
#End Region

        Private _Inherit As Inheritance = New Inheritance(Me)
        <Category(""), _
            Browsable(False)> _
        Public Property Inherit() As Inheritance
            Get
                Return Me._Inherit
            End Get
            Set(ByVal value As Inheritance)
                Me._Inherit = value
            End Set
        End Property

        Private _OpenConnections As Connection.Protocol.List
        <Category(""), _
            Browsable(False)> _
        Public Property OpenConnections() As Connection.Protocol.List
            Get
                Return Me._OpenConnections
            End Get
            Set(ByVal value As Connection.Protocol.List)
                Me._OpenConnections = value
            End Set
        End Property

        Private _IsContainer As Boolean = False
        <Category(""), _
            Browsable(False)> _
        Public Property IsContainer() As Boolean
            Get
                Return Me._IsContainer
            End Get
            Set(ByVal value As Boolean)
                Me._IsContainer = value
            End Set
        End Property

        Private _IsDefault As Boolean = False
        <Category(""), _
            Browsable(False)> _
        Public Property IsDefault() As Boolean
            Get
                Return Me._IsDefault
            End Get
            Set(ByVal value As Boolean)
                Me._IsDefault = value
            End Set
        End Property

        Private _Parent As Object
        <Category(""), _
            Browsable(False)> _
        Public Property Parent() As Object
            Get
                Return Me._Parent
            End Get
            Set(ByVal value As Object)
                Me._Parent = value
            End Set
        End Property

        Private _PositionID As Integer = 0
        <Category(""), _
            Browsable(False)> _
        Public Property PositionID() As Integer
            Get
                Return Me._PositionID
            End Get
            Set(ByVal value As Integer)
                _PositionID = value
            End Set
        End Property

        Private _ConstantID As String = Tools.Misc.CreateConstantID
        <Category(""), _
            Browsable(False)> _
        Public Property ConstantID() As String
            Get
                Return _ConstantID
            End Get
            Set(ByVal value As String)
                _ConstantID = value
            End Set
        End Property

        Private _TreeNode As TreeNode
        <Category(""), _
            Browsable(False)> _
        Public Property TreeNode() As TreeNode
            Get
                Return Me._TreeNode
            End Get
            Set(ByVal value As TreeNode)
                Me._TreeNode = value
            End Set
        End Property

        Private _IsQuicky As Boolean = False
        <Category(""), _
            Browsable(False)> _
        Public Property IsQuicky() As Boolean
            Get
                Return Me._IsQuicky
            End Get
            Set(ByVal value As Boolean)
                Me._IsQuicky = value
            End Set
        End Property

        Private _PleaseConnect As Boolean = False
        <Category(""), _
            Browsable(False)> _
        Public Property PleaseConnect() As Boolean
            Get
                Return _PleaseConnect
            End Get
            Set(ByVal value As Boolean)
                _PleaseConnect = value
            End Set
        End Property
#End Region

#Region "Methods"
        Public Function Copy() As Connection.Info
            Return Me.MemberwiseClone
        End Function

        Public Sub New()
            Me._OpenConnections = New Connection.Protocol.List
            Me.SetDefaults()
        End Sub

        Public Sub New(ByVal Parent As Container.Info)
            Me._OpenConnections = New Connection.Protocol.List
            Me.SetDefaults()
            Me._IsContainer = True
            Me._Parent = Parent
        End Sub

        Public Sub SetDefaults()
            If Me.Port = Nothing Then
                Me.SetDefaultPort()
            End If
        End Sub

        Public Sub SetDefaultPort()
            Try
                Select Case Me._Protocol
                    Case Connection.Protocol.Protocols.RDP
                        Me._Port = Connection.Protocol.RDP.Defaults.Port
                    Case Connection.Protocol.Protocols.VNC
                        Me._Port = Connection.Protocol.VNC.Defaults.Port
                    Case Connection.Protocol.Protocols.SSH1
                        Me._Port = Connection.Protocol.SSH1.Defaults.Port
                    Case Connection.Protocol.Protocols.SSH2
                        Me._Port = Connection.Protocol.SSH2.Defaults.Port
                    Case Connection.Protocol.Protocols.Telnet
                        Me._Port = Connection.Protocol.Telnet.Defaults.Port
                    Case Connection.Protocol.Protocols.Rlogin
                        Me._Port = Connection.Protocol.Rlogin.Defaults.Port
                    Case Connection.Protocol.Protocols.RAW
                        Me._Port = Connection.Protocol.RAW.Defaults.Port
                    Case Connection.Protocol.Protocols.HTTP
                        Me._Port = Connection.Protocol.HTTP.Defaults.Port
                    Case Connection.Protocol.Protocols.HTTPS
                        Me._Port = Connection.Protocol.HTTPS.Defaults.Port
                    Case Connection.Protocol.Protocols.ICA
                        Me._Port = Connection.Protocol.ICA.Defaults.Port
                    Case Connection.Protocol.Protocols.IntApp
                        Me._Port = Connection.Protocol.IntApp.Defaults.Port
                End Select
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strConnectionSetDefaultPortFailed & vbNewLine & ex.Message)
            End Try
        End Sub
#End Region

#Region "Inheritance"
        Public Class Inheritance
            Public Sub New(ByVal Parent As Object, Optional ByVal InheritEverything As Boolean = False)
                Me._Parent = Parent

                If InheritEverything = True Then
                    Me.TurnOnInheritanceCompletely()
                End If
            End Sub

            Public Function Copy() As Connection.Info.Inheritance
                Return Me.MemberwiseClone
            End Function

            Public Sub TurnOnInheritanceCompletely()
                SetAllValues(True)
            End Sub

            Public Sub TurnOffInheritanceCompletely()
                SetAllValues(False)
            End Sub

            Private Sub SetAllValues(ByVal val As Boolean)
                Me._CacheBitmaps = val
                Me._Colors = val
                Me._Description = val
                Me._DisplayThemes = val
                Me._DisplayWallpaper = val
                Me._EnableFontSmoothing = val
                Me._EnableDesktopComposition = val
                Me._Domain = val
                Me._Icon = val
                Me._Password = val
                Me._Port = val
                Me._Protocol = val
                Me._PuttySession = val
                Me._RedirectDiskDrives = val
                Me._RedirectKeys = val
                Me._RedirectPorts = val
                Me._RedirectPrinters = val
                Me._RedirectSmartCards = val
                Me._RedirectSound = val
                Me._Resolution = val
                Me._UseConsoleSession = val
                Me._RenderingEngine = val
                Me._Username = val
                Me._Panel = val
                Me._ICAEncryption = val
                Me._RDPAuthenticationLevel = val
                Me._PreExtApp = val
                Me._PostExtApp = val
                Me._MacAddress = val
                Me._UserField = val

                Me._VNCAuthMode = val
                Me._VNCColors = val
                Me._VNCCompression = val
                Me._VNCEncoding = val
                Me._VNCProxyIP = val
                Me._VNCProxyPassword = val
                Me._VNCProxyPort = val
                Me._VNCProxyType = val
                Me._VNCProxyUsername = val
                Me._VNCSmartSizeMode = val
                Me._VNCViewOnly = val
                Me._ExtApp = val

                Me._RDGatewayDomain = val
                Me._RDGatewayHostname = val
                Me._RDGatewayPassword = val
                Me._RDGatewayUsageMethod = val
                Me._RDGatewayUseConnectionCredentials = val
                Me._RDGatewayUsername = val
                'Me._RDPAuthenticationLevel = val
            End Sub

            Private _Parent As Object
            <Category(""), _
               Browsable(False)> _
            Public Property Parent() As Object
                Get
                    Return Me._Parent
                End Get
                Set(ByVal value As Object)
                    Me._Parent = value
                End Set
            End Property

            Private _IsDefault As Boolean = False
            <Category(""), _
                Browsable(False)> _
            Public Property IsDefault() As Boolean
                Get
                    Return Me._IsDefault
                End Get
                Set(ByVal value As Boolean)
                    Me._IsDefault = value
                End Set
            End Property

#Region "1 General"
            <LocalizedCategory("strCategoryGeneral", 1), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameAll"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionAll"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property EverythingInherited() As Boolean
                'LocalizedDisplayName("strPropertyNameInheritAll"), _
                Get
                    If Me._CacheBitmaps And Me._Colors And Me._Description And Me._DisplayThemes And Me._DisplayWallpaper _
                    And Me._EnableFontSmoothing And Me._EnableDesktopComposition _
                    And Me._Domain And Me._Icon And Me._Password And Me._Port And Me._Protocol And Me._PuttySession _
                    And Me._RedirectDiskDrives And Me._RedirectKeys And Me._RedirectPorts And Me._RedirectPrinters _
                    And Me._RedirectSmartCards And Me._RedirectSound And Me._Resolution And Me._UseConsoleSession _
                    And Me._RenderingEngine And Me._UserField And Me._ExtApp And Me._Username And Me._Panel And Me._ICAEncryption And Me._RDPAuthenticationLevel And Me._PreExtApp And Me._PostExtApp _
                    And Me._MacAddress And Me._VNCAuthMode And Me._VNCColors And Me._VNCCompression And Me._VNCEncoding And Me._VNCProxyIP _
                    And Me._VNCProxyPassword And Me._VNCProxyPort And Me._VNCProxyType And Me._VNCProxyUsername Then
                        Return True
                    Else
                        Return False
                    End If
                End Get
                Set(ByVal value As Boolean)
                    If value = True Then
                        Me.TurnOnInheritanceCompletely()
                    Else
                        Me.TurnOffInheritanceCompletely()
                    End If
                End Set
            End Property
#End Region
#Region "2 Display"
            Private _Description As Boolean = My.Settings.InhDefaultDescription
            <LocalizedCategory("strCategoryDisplay", 2), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameDescription"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionDescription"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property Description() As Boolean
                Get
                    Return Me._Description
                End Get
                Set(ByVal value As Boolean)
                    Me._Description = value
                End Set
            End Property

            Private _Icon As Boolean = My.Settings.InhDefaultIcon
            <LocalizedCategory("strCategoryDisplay", 2), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameIcon"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionIcon"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property Icon() As Boolean
                Get
                    Return Me._Icon
                End Get
                Set(ByVal value As Boolean)
                    Me._Icon = value
                End Set
            End Property

            Private _Panel As Boolean = My.Settings.InhDefaultPanel
            <LocalizedCategory("strCategoryDisplay", 2), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNamePanel"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionPanel"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property Panel() As Boolean
                Get
                    Return Me._Panel
                End Get
                Set(ByVal value As Boolean)
                    Me._Panel = value
                End Set
            End Property
#End Region
#Region "3 Connection"
            Private _Username As Boolean = My.Settings.InhDefaultUsername
            <LocalizedCategory("strCategoryConnection", 3), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameUsername"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionUsername"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property Username() As Boolean
                Get
                    Return Me._Username
                End Get
                Set(ByVal value As Boolean)
                    Me._Username = value
                End Set
            End Property

            Private _Password As Boolean = My.Settings.InhDefaultPassword
            <LocalizedCategory("strCategoryConnection", 3), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNamePassword"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionPassword"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property Password() As Boolean
                Get
                    Return Me._Password
                End Get
                Set(ByVal value As Boolean)
                    Me._Password = value
                End Set
            End Property

            Private _Domain As Boolean = My.Settings.InhDefaultDomain
            <LocalizedCategory("strCategoryConnection", 3), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameDomain"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionDomain"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property Domain() As Boolean
                Get
                    Return Me._Domain
                End Get
                Set(ByVal value As Boolean)
                    Me._Domain = value
                End Set
            End Property
#End Region
#Region "4 Protocol"
            Private _Protocol As Boolean = My.Settings.InhDefaultProtocol
            <LocalizedCategory("strCategoryProtocol", 4), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameProtocol"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionProtocol"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property Protocol() As Boolean
                Get
                    Return Me._Protocol
                End Get
                Set(ByVal value As Boolean)
                    Me._Protocol = value
                End Set
            End Property

            Private _ExtApp As Boolean = My.Settings.InhDefaultExtApp
            <LocalizedCategory("strCategoryProtocol", 4), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameExternalTool"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionExternalTool"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property ExtApp() As Boolean
                Get
                    Return Me._ExtApp
                End Get
                Set(ByVal value As Boolean)
                    Me._ExtApp = value
                End Set
            End Property

            Private _Port As Boolean = My.Settings.InhDefaultPort
            <LocalizedCategory("strCategoryProtocol", 4), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNamePort"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionPort"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property Port() As Boolean
                Get
                    Return Me._Port
                End Get
                Set(ByVal value As Boolean)
                    Me._Port = value
                End Set
            End Property

            Private _PuttySession As Boolean = My.Settings.InhDefaultPuttySession
            <LocalizedCategory("strCategoryProtocol", 4), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNamePuttySession"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionPuttySession"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property PuttySession() As Boolean
                Get
                    Return Me._PuttySession
                End Get
                Set(ByVal value As Boolean)
                    Me._PuttySession = value
                End Set
            End Property

            Private _ICAEncryption As Boolean = My.Settings.InhDefaultICAEncryptionStrength
            <LocalizedCategory("strCategoryProtocol", 4), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameEncryptionStrength"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionEncryptionStrength"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property ICAEncryption() As Boolean
                Get
                    Return Me._ICAEncryption
                End Get
                Set(ByVal value As Boolean)
                    Me._ICAEncryption = value
                End Set
            End Property

            Private _RDPAuthenticationLevel As Boolean = My.Settings.InhDefaultRDPAuthenticationLevel
            <LocalizedCategory("strCategoryProtocol", 4), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameAuthenticationLevel"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionAuthenticationLevel"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property RDPAuthenticationLevel() As Boolean
                Get
                    Return Me._RDPAuthenticationLevel
                End Get
                Set(ByVal value As Boolean)
                    Me._RDPAuthenticationLevel = value
                End Set
            End Property

            Private _RenderingEngine As Boolean = My.Settings.InhDefaultRenderingEngine
            <LocalizedCategory("strCategoryProtocol", 4), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRenderingEngine"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRenderingEngine"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property RenderingEngine() As Boolean
                Get
                    Return Me._RenderingEngine
                End Get
                Set(ByVal value As Boolean)
                    Me._RenderingEngine = value
                End Set
            End Property

            Private _UseConsoleSession As Boolean = My.Settings.InhDefaultUseConsoleSession
            <LocalizedCategory("strCategoryProtocol", 4), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameUseConsoleSession"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionUseConsoleSession"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property UseConsoleSession() As Boolean
                Get
                    Return Me._UseConsoleSession
                End Get
                Set(ByVal value As Boolean)
                    Me._UseConsoleSession = value
                End Set
            End Property
#End Region
#Region "5 RD Gateway"
            Private _RDGatewayUsageMethod As Boolean = My.Settings.InhDefaultRDGatewayUsageMethod
            <LocalizedCategory("strCategoryGateway", 5), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayUsageMethod"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayUsageMethod"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property RDGatewayUsageMethod() As Boolean
                Get
                    Return Me._RDGatewayUsageMethod
                End Get
                Set(ByVal value As Boolean)
                    Me._RDGatewayUsageMethod = value
                End Set
            End Property

            Private _RDGatewayHostname As Boolean = My.Settings.InhDefaultRDGatewayHostname
            <LocalizedCategory("strCategoryGateway", 5), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayHostname"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayHostname"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property RDGatewayHostname() As Boolean
                Get
                    Return Me._RDGatewayHostname
                End Get
                Set(ByVal value As Boolean)
                    Me._RDGatewayHostname = value
                End Set
            End Property

            Private _RDGatewayUseConnectionCredentials As Boolean = My.Settings.InhDefaultRDGatewayUseConnectionCredentials
            <LocalizedCategory("strCategoryGateway", 5), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayUseConnectionCredentials"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayUseConnectionCredentials"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property RDGatewayUseConnectionCredentials() As Boolean
                Get
                    Return Me._RDGatewayUseConnectionCredentials
                End Get
                Set(ByVal value As Boolean)
                    Me._RDGatewayUseConnectionCredentials = value
                End Set
            End Property

            Private _RDGatewayUsername As Boolean = My.Settings.InhDefaultRDGatewayUsername
            <LocalizedCategory("strCategoryGateway", 5), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayUsername"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayUsername"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property RDGatewayUsername() As Boolean
                Get
                    Return Me._RDGatewayUsername
                End Get
                Set(ByVal value As Boolean)
                    Me._RDGatewayUsername = value
                End Set
            End Property

            Private _RDGatewayPassword As Boolean = My.Settings.InhDefaultRDGatewayPassword
            <LocalizedCategory("strCategoryGateway", 5), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayPassword"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayPassword"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property RDGatewayPassword() As Boolean
                Get
                    Return Me._RDGatewayPassword
                End Get
                Set(ByVal value As Boolean)
                    Me._RDGatewayPassword = value
                End Set
            End Property

            Private _RDGatewayDomain As Boolean = My.Settings.InhDefaultRDGatewayDomain
            <LocalizedCategory("strCategoryGateway", 5), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayDomain"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayDomain"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property RDGatewayDomain() As Boolean
                Get
                    Return Me._RDGatewayDomain
                End Get
                Set(ByVal value As Boolean)
                    Me._RDGatewayDomain = value
                End Set
            End Property
#End Region
#Region "6 Appearance"
            Private _Resolution As Boolean = My.Settings.InhDefaultResolution
            <LocalizedCategory("strCategoryAppearance", 6), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameResolution"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionResolution"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property Resolution() As Boolean
                Get
                    Return Me._Resolution
                End Get
                Set(ByVal value As Boolean)
                    Me._Resolution = value
                End Set
            End Property

            Private _Colors As Boolean = My.Settings.InhDefaultColors
            <LocalizedCategory("strCategoryAppearance", 6), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameColors"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionColors"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property Colors() As Boolean
                Get
                    Return Me._Colors
                End Get
                Set(ByVal value As Boolean)
                    Me._Colors = value
                End Set
            End Property

            Private _CacheBitmaps As Boolean = My.Settings.InhDefaultCacheBitmaps
            <LocalizedCategory("strCategoryAppearance", 6), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameCacheBitmaps"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionCacheBitmaps"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property CacheBitmaps() As Boolean
                Get
                    Return Me._CacheBitmaps
                End Get
                Set(ByVal value As Boolean)
                    Me._CacheBitmaps = value
                End Set
            End Property

            Private _DisplayWallpaper As Boolean = My.Settings.InhDefaultDisplayWallpaper
            <LocalizedCategory("strCategoryAppearance", 6), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameDisplayWallpaper"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionDisplayWallpaper"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property DisplayWallpaper() As Boolean
                Get
                    Return Me._DisplayWallpaper
                End Get
                Set(ByVal value As Boolean)
                    Me._DisplayWallpaper = value
                End Set
            End Property

            Private _DisplayThemes As Boolean = My.Settings.InhDefaultDisplayThemes
            <LocalizedCategory("strCategoryAppearance", 6), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameDisplayThemes"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionDisplayThemes"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property DisplayThemes() As Boolean
                Get
                    Return Me._DisplayThemes
                End Get
                Set(ByVal value As Boolean)
                    Me._DisplayThemes = value
                End Set
            End Property

            Private _EnableFontSmoothing As Boolean = My.Settings.InhDefaultEnableFontSmoothing
            <LocalizedCategory("strCategoryAppearance", 6), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameEnableFontSmoothing"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionEnableFontSmoothing"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property EnableFontSmoothing() As Boolean
                Get
                    Return Me._EnableFontSmoothing
                End Get
                Set(ByVal value As Boolean)
                    Me._EnableFontSmoothing = value
                End Set
            End Property

            Private _EnableDesktopComposition As Boolean = My.Settings.InhDefaultEnableDesktopComposition
            <LocalizedCategory("strCategoryAppearance", 6), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameEnableDesktopComposition"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionEnableEnableDesktopComposition"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property EnableDesktopComposition() As Boolean
                Get
                    Return Me._EnableDesktopComposition
                End Get
                Set(ByVal value As Boolean)
                    Me._EnableDesktopComposition = value
                End Set
            End Property
#End Region
#Region "7 Redirect"
            Private _RedirectKeys As Boolean = My.Settings.InhDefaultRedirectKeys
            <LocalizedCategory("strCategoryRedirect", 7), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectKeys"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectKeys"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property RedirectKeys() As Boolean
                Get
                    Return Me._RedirectKeys
                End Get
                Set(ByVal value As Boolean)
                    Me._RedirectKeys = value
                End Set
            End Property

            Private _RedirectDiskDrives As Boolean = My.Settings.InhDefaultRedirectDiskDrives
            <LocalizedCategory("strCategoryRedirect", 7), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectDrives"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectDrives"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property RedirectDiskDrives() As Boolean
                Get
                    Return Me._RedirectDiskDrives
                End Get
                Set(ByVal value As Boolean)
                    Me._RedirectDiskDrives = value
                End Set
            End Property

            Private _RedirectPrinters As Boolean = My.Settings.InhDefaultRedirectPrinters
            <LocalizedCategory("strCategoryRedirect", 7), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectPrinters"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectPrinters"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property RedirectPrinters() As Boolean
                Get
                    Return Me._RedirectPrinters
                End Get
                Set(ByVal value As Boolean)
                    Me._RedirectPrinters = value
                End Set
            End Property

            Private _RedirectPorts As Boolean = My.Settings.InhDefaultRedirectPorts
            <LocalizedCategory("strCategoryRedirect", 7), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectPorts"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectPorts"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property RedirectPorts() As Boolean
                Get
                    Return Me._RedirectPorts
                End Get
                Set(ByVal value As Boolean)
                    Me._RedirectPorts = value
                End Set
            End Property

            Private _RedirectSmartCards As Boolean = My.Settings.InhDefaultRedirectSmartCards
            <LocalizedCategory("strCategoryRedirect", 7), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectSmartCards"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectSmartCards"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property RedirectSmartCards() As Boolean
                Get
                    Return Me._RedirectSmartCards
                End Get
                Set(ByVal value As Boolean)
                    Me._RedirectSmartCards = value
                End Set
            End Property

            Private _RedirectSound As Boolean = My.Settings.InhDefaultRedirectSound
            <LocalizedCategory("strCategoryRedirect", 7), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectSounds"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectSounds"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property RedirectSound() As Boolean
                Get
                    Return Me._RedirectSound
                End Get
                Set(ByVal value As Boolean)
                    Me._RedirectSound = value
                End Set
            End Property
#End Region
#Region "8 Misc"
            Private _PreExtApp As Boolean = My.Settings.InhDefaultPreExtApp
            <LocalizedCategory("strCategoryMiscellaneous", 8), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameExternalToolBefore"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionExternalToolBefore"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property PreExtApp() As Boolean
                Get
                    Return Me._PreExtApp
                End Get
                Set(ByVal value As Boolean)
                    Me._PreExtApp = value
                End Set
            End Property

            Private _PostExtApp As Boolean = My.Settings.InhDefaultPostExtApp
            <LocalizedCategory("strCategoryMiscellaneous", 8), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameExternalToolAfter"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionExternalToolAfter"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property PostExtApp() As Boolean
                Get
                    Return Me._PostExtApp
                End Get
                Set(ByVal value As Boolean)
                    Me._PostExtApp = value
                End Set
            End Property

            Private _MacAddress As Boolean = My.Settings.InhDefaultMacAddress
            <LocalizedCategory("strCategoryMiscellaneous", 8), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameMACAddress"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionMACAddress"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property MacAddress() As Boolean
                Get
                    Return Me._MacAddress
                End Get
                Set(ByVal value As Boolean)
                    Me._MacAddress = value
                End Set
            End Property

            Private _UserField As Boolean = My.Settings.InhDefaultUserField
            <LocalizedCategory("strCategoryMiscellaneous", 8), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameUser1"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionUser1"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property UserField() As Boolean
                Get
                    Return Me._UserField
                End Get
                Set(ByVal value As Boolean)
                    Me._UserField = value
                End Set
            End Property
#End Region
#Region "9 VNC"
            Private _VNCCompression As Boolean = My.Settings.InhDefaultVNCCompression
            <LocalizedCategory("strCategoryAppearance", 9), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameCompression"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionCompression"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCCompression() As Boolean
                Get
                    Return _VNCCompression
                End Get
                Set(ByVal value As Boolean)
                    _VNCCompression = value
                End Set
            End Property

            Private _VNCEncoding As Boolean = My.Settings.InhDefaultVNCEncoding
            <LocalizedCategory("strCategoryAppearance", 9), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameEncoding"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionEncoding"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCEncoding() As Boolean
                Get
                    Return _VNCEncoding
                End Get
                Set(ByVal value As Boolean)
                    _VNCEncoding = value
                End Set
            End Property

            Private _VNCAuthMode As Boolean = My.Settings.InhDefaultVNCAuthMode
            <LocalizedCategory("strCategoryConnection", 9), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameAuthenticationMode"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionAuthenticationMode"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCAuthMode() As Boolean
                Get
                    Return _VNCAuthMode
                End Get
                Set(ByVal value As Boolean)
                    _VNCAuthMode = value
                End Set
            End Property

            Private _VNCProxyType As Boolean = My.Settings.InhDefaultVNCProxyType
            <LocalizedCategory("strCategoryMiscellaneous", 9), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameVNCProxyType"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionVNCProxyType"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCProxyType() As Boolean
                Get
                    Return _VNCProxyType
                End Get
                Set(ByVal value As Boolean)
                    _VNCProxyType = value
                End Set
            End Property

            Private _VNCProxyIP As Boolean = My.Settings.InhDefaultVNCProxyIP
            <LocalizedCategory("strCategoryMiscellaneous", 9), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameVNCProxyAddress"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionVNCProxyAddress"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCProxyIP() As Boolean
                Get
                    Return _VNCProxyIP
                End Get
                Set(ByVal value As Boolean)
                    _VNCProxyIP = value
                End Set
            End Property

            Private _VNCProxyPort As Boolean = My.Settings.InhDefaultVNCProxyPort
            <LocalizedCategory("strCategoryMiscellaneous", 9), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameVNCProxyPort"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionVNCProxyPort"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCProxyPort() As Boolean
                Get
                    Return _VNCProxyPort
                End Get
                Set(ByVal value As Boolean)
                    _VNCProxyPort = value
                End Set
            End Property

            Private _VNCProxyUsername As Boolean = My.Settings.InhDefaultVNCProxyUsername
            <LocalizedCategory("strCategoryMiscellaneous", 9), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameVNCProxyUsername"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionVNCProxyUsername"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCProxyUsername() As Boolean
                Get
                    Return _VNCProxyUsername
                End Get
                Set(ByVal value As Boolean)
                    _VNCProxyUsername = value
                End Set
            End Property

            Private _VNCProxyPassword As Boolean = My.Settings.InhDefaultVNCProxyPassword
            <LocalizedCategory("strCategoryMiscellaneous", 9), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameVNCProxyPassword"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionVNCProxyPassword"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCProxyPassword() As Boolean
                Get
                    Return _VNCProxyPassword
                End Get
                Set(ByVal value As Boolean)
                    _VNCProxyPassword = value
                End Set
            End Property

            Private _VNCColors As Boolean = My.Settings.InhDefaultVNCColors
            <LocalizedCategory("strCategoryAppearance", 9), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameColors"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionColors"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCColors() As Boolean
                Get
                    Return _VNCColors
                End Get
                Set(ByVal value As Boolean)
                    _VNCColors = value
                End Set
            End Property

            Private _VNCSmartSizeMode As Boolean = My.Settings.InhDefaultVNCSmartSizeMode
            <LocalizedCategory("strCategoryAppearance", 9), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameSmartSizeMode"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionSmartSizeMode"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCSmartSizeMode() As Boolean
                Get
                    Return _VNCSmartSizeMode
                End Get
                Set(ByVal value As Boolean)
                    _VNCSmartSizeMode = value
                End Set
            End Property

            Private _VNCViewOnly As Boolean = My.Settings.InhDefaultVNCViewOnly
            <LocalizedCategory("strCategoryAppearance", 9), _
                Browsable(True), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameViewOnly"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionViewOnly"), _
                TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCViewOnly() As Boolean
                Get
                    Return _VNCViewOnly
                End Get
                Set(ByVal value As Boolean)
                    _VNCViewOnly = value
                End Set
            End Property
#End Region

        End Class
#End Region

        <Flags()> _
        Public Enum Force
            None = 0
            UseConsoleSession = 1
            Fullscreen = 2
            DoNotJump = 4
            OverridePanel = 8
            DontUseConsoleSession = 16
        End Enum
    End Class
End Namespace