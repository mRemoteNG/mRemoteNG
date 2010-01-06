Imports System.Windows.Forms
Imports System.Threading
Imports System.ComponentModel
Imports mRemote.Tools.Controls
Imports mRemote.Tools.Misc.PropertyGridCategory
Imports mRemote.Tools.Misc.PropertyGridValue
Imports mRemote.App.Runtime

Namespace Connection
    <DefaultProperty("Name")> _
    Public Class Info
#Region "Properties"
#Region "1 Display"
        Private _Name As String = "New Connection"
        <Category(Category1 & Language.Base.Props_Display), _
           Browsable(True), _
           DisplayName(Language.Base.Props_Name), _
           Description(Language.Base.Descr_Name)> _
        Public Property Name() As String
            Get
                Return Me._Name
            End Get
            Set(ByVal value As String)
                Me._Name = value
            End Set
        End Property

        Private _Description As String = My.Settings.ConDefaultDescription
        <Category(Category1 & Language.Base.Props_Display), _
            Browsable(True), _
            DisplayName(Language.Base.Props_Description), _
            Description(Language.Base.Descr_Description)> _
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
        <Category(Category1 & Language.Base.Props_Display), _
            TypeConverter(GetType(mRemote.Connection.Icon)), _
            Browsable(True), _
            DisplayName(Language.Base.Props_Icon), _
            Description(Language.Base.Descr_Icon)> _
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

        Private _Panel As String = My.Settings.ConDefaultPanel
        <Category(Category1 & Language.Base.Props_Display), _
            Browsable(True), _
            DisplayName(Language.Base.Props_Panel), _
            Description(Language.Base.Descr_Panel)> _
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
        <Category(Category2 & Language.Base.Props_Connection), _
            Browsable(True), _
            DisplayName(Language.Base.Props_HostnameIP), _
            Description(Language.Base.Descr_HostnameIP)> _
        Public Property Hostname() As String
            Get
                Return Me._Hostname
            End Get
            Set(ByVal value As String)
                Me._Hostname = value
            End Set
        End Property

        Private _Username As String = My.Settings.ConDefaultUsername
        <Category(Category2 & Language.Base.Props_Connection), _
            Browsable(True), _
            DisplayName(Language.Base.Props_Username), _
            Description(Language.Base.Descr_Username)> _
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
        <Category(Category2 & Language.Base.Props_Connection), _
            Browsable(True), _
            DisplayName(Language.Base.Props_Password), _
            Description(Language.Base.Descr_Password), _
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
        <Category(Category2 & Language.Base.Props_Connection), _
            Browsable(True), _
            DisplayName(Language.Base.Props_Domain), _
            Description(Language.Base.Descr_Domain)> _
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
        <Category(Category3 & Language.Base.Props_Protocol), _
            Browsable(True), _
            DisplayName(Language.Base.Props_Protocol), _
            Description(Language.Base.Descr_Protocol), _
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
        <Category(Category3 & Language.Base.Props_Protocol), _
            Browsable(True), _
            DisplayName(Language.Base.Props_ExtApp), _
            Description(Language.Base.Descr_ExtApp), _
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
        <Category(Category3 & Language.Base.Props_Protocol), _
            Browsable(True), _
            DisplayName(Language.Base.Props_Port), _
            Description(Language.Base.Descr_Port)> _
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
        <Category(Category3 & Language.Base.Props_Protocol), _
            Browsable(True), _
            DisplayName(Language.Base.Props_PuttySession), _
            Description(Language.Base.Descr_PuttySession), _
            TypeConverter(GetType(mRemote.Connection.PuttySession))> _
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

        Private _ICAEncryption As Connection.Protocol.ICA.EncryptionStrength = Tools.Misc.StringToEnum(GetType(mRemote.Connection.Protocol.ICA.EncryptionStrength), My.Settings.ConDefaultICAEncryptionStrength)
        <Category(Category3 & Language.Base.Props_Protocol), _
            Browsable(True), _
            DisplayName(Language.Base.Props_EncryrptionStrength), _
            Description(Language.Base.Descr_EncryptionStrength), _
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
        <Category(Category3 & Language.Base.Props_Protocol), _
            Browsable(True), _
            DisplayName(Language.Base.Props_UseConsoleSession), _
            Description(Language.Base.Descr_UseConsoleSession), _
            TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
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

        Private _RDPAuthenticationLevel As Connection.Protocol.RDP.AuthenticationLevel = Tools.Misc.StringToEnum(GetType(mRemote.Connection.Protocol.RDP.AuthenticationLevel), My.Settings.ConDefaultRDPAuthenticationLevel)
        <Category(Category3 & Language.Base.Props_Protocol), _
            Browsable(True), _
            DisplayName(Language.Base.Props_AuthenticationLevel), _
            Description(Language.Base.Descr_AuthenticationLevel), _
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

        Private _RenderingEngine As Connection.Protocol.HTTPBase.RenderingEngine = Tools.Misc.StringToEnum(GetType(mRemote.Connection.Protocol.HTTPBase.RenderingEngine), My.Settings.ConDefaultRenderingEngine)
        <Category(Category3 & Language.Base.Props_Protocol), _
            Browsable(True), _
            DisplayName(Language.Base.Props_RenderingEngine), _
            Description(Language.Base.Descr_RenderingEngine), _
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
#Region "4 Appearance"
        Private _Resolution As Connection.Protocol.RDP.RDPResolutions = Tools.Misc.StringToEnum(GetType(Connection.Protocol.RDP.RDPResolutions), My.Settings.ConDefaultResolution)
        <Category(Category4 & Language.Base.Props_Appearance), _
            Browsable(True), _
            DisplayName(Language.Base.Props_Resolution), _
            Description(Language.Base.Descr_Resolution), _
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
        <Category(Category4 & Language.Base.Props_Appearance), _
            Browsable(True), _
            DisplayName(Language.Base.Props_Colors), _
            Description(Language.Base.Descr_Colors), _
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
        <Category(Category4 & Language.Base.Props_Appearance), _
            Browsable(True), _
            DisplayName(Language.Base.Props_CacheBitmaps), _
            Description(Language.Base.Descr_CacheBitmaps), _
            TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
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
        <Category(Category4 & Language.Base.Props_Appearance), _
            Browsable(True), _
            DisplayName(Language.Base.Props_DisplayWallpaper), _
            Description(Language.Base.Descr_DisplayWallpaper), _
            TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
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
        <Category(Category4 & Language.Base.Props_Appearance), _
            Browsable(True), _
            DisplayName(Language.Base.Props_DisplayThemes), _
            Description(Language.Base.Descr_DisplayThemes), _
            TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
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
#End Region
#Region "5 Redirect"
        Private _RedirectKeys As Boolean = My.Settings.ConDefaultRedirectKeys
        <Category(Category5 & Language.Base.Props_Redirect), _
            Browsable(True), _
            DisplayName(Language.Base.Props_RedKeyCombinations), _
            Description(Language.Base.Descr_RedKeyCombinations), _
            TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
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
        <Category(Category5 & Language.Base.Props_Redirect), _
            Browsable(True), _
            DisplayName(Language.Base.Props_RedDiskDrives), _
            Description(Language.Base.Descr_RedDiskDrives), _
            TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
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
        <Category(Category5 & Language.Base.Props_Redirect), _
            Browsable(True), _
            DisplayName(Language.Base.Props_RedPrinters), _
            Description(Language.Base.Descr_RedPrinters), _
            TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
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
        <Category(Category5 & Language.Base.Props_Redirect), _
            Browsable(True), _
            DisplayName(Language.Base.Props_RedPorts), _
            Description(Language.Base.Descr_RedPorts), _
            TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
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
        <Category(Category5 & Language.Base.Props_Redirect), _
            Browsable(True), _
            DisplayName(Language.Base.Props_RedSmartCards), _
            Description(Language.Base.Descr_RedSmartCards), _
            TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
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
        <Category(Category5 & Language.Base.Props_Redirect), _
            Browsable(True), _
            DisplayName(Language.Base.Props_RedSounds), _
            Description(Language.Base.Descr_RedSound), _
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
#Region "6 Misc"
        Private _PreExtApp As String = My.Settings.ConDefaultPreExtApp
        <Category(Category6 & Language.Base.Props_Misc), _
            Browsable(True), _
            DisplayName(Language.Base.Props_PreExtApp), _
            Description(Language.Base.Descr_PreExtApp), _
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
        <Category(Category6 & Language.Base.Props_Misc), _
            Browsable(True), _
            DisplayName(Language.Base.Props_PostExtApp), _
            Description(Language.Base.Descr_PostExtApp), _
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

        Private _MacAddress As String = My.Settings.conDefaultMacAddress
        <Category(Category6 & Language.Base.Props_Misc), _
            Browsable(True), _
            DisplayName(Language.Base.Props_MacAddress), _
            Description(Language.Base.Descr_MacAddress)> _
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
        <Category(Category6 & Language.Base.Props_Misc), _
            Browsable(True), _
            DisplayName(Language.Base.Props_UserField), _
            Description(Language.Base.Descr_UserField)> _
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
#Region "VNC"
        Private _VNCCompression As mRemote.Connection.Protocol.VNC.Compression = Tools.Misc.StringToEnum(GetType(Connection.Protocol.VNC.Compression), My.Settings.ConDefaultVNCCompression)
        <Category(Category4 & Language.Base.Props_Appearance), _
           Browsable(True), _
           DisplayName(Language.Base.Props_Compression), _
           Description(Language.Base.Descr_Compression), _
           TypeConverter(GetType(Tools.Misc.EnumTypeConverter))> _
       Public Property VNCCompression() As mRemote.Connection.Protocol.VNC.Compression
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
            Set(ByVal value As mRemote.Connection.Protocol.VNC.Compression)
                _VNCCompression = value
            End Set
        End Property

        Private _VNCEncoding As mRemote.Connection.Protocol.VNC.Encoding = Tools.Misc.StringToEnum(GetType(Connection.Protocol.VNC.Encoding), My.Settings.ConDefaultVNCEncoding)
        <Category(Category4 & Language.Base.Props_Appearance), _
           Browsable(True), _
           DisplayName(Language.Base.Props_Encoding), _
           Description(Language.Base.Descr_Encoding), _
           TypeConverter(GetType(Tools.Misc.EnumTypeConverter))> _
        Public Property VNCEncoding() As mRemote.Connection.Protocol.VNC.Encoding
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
            Set(ByVal value As mRemote.Connection.Protocol.VNC.Encoding)
                _VNCEncoding = value
            End Set
        End Property


        Private _VNCAuthMode As mRemote.Connection.Protocol.VNC.AuthMode = Tools.Misc.StringToEnum(GetType(Connection.Protocol.VNC.AuthMode), My.Settings.ConDefaultVNCAuthMode)
        <Category(Category2 & Language.Base.Props_Connection), _
            Browsable(True), _
            DisplayName(Language.Base.Props_AuthMode), _
            Description(Language.Base.Descr_AuthMode), _
            TypeConverter(GetType(Tools.Misc.EnumTypeConverter))> _
        Public Property VNCAuthMode() As mRemote.Connection.Protocol.VNC.AuthMode
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
            Set(ByVal value As mRemote.Connection.Protocol.VNC.AuthMode)
                _VNCAuthMode = value
            End Set
        End Property

        Private _VNCProxyType As mRemote.Connection.Protocol.VNC.ProxyType = Tools.Misc.StringToEnum(GetType(Connection.Protocol.VNC.ProxyType), My.Settings.ConDefaultVNCProxyType)
        <Category(Category6 & Language.Base.Props_Misc), _
           Browsable(True), _
           DisplayName(Language.Base.Props_ProxyType), _
           Description(Language.Base.Descr_ProxyType), _
           TypeConverter(GetType(Tools.Misc.EnumTypeConverter))> _
        Public Property VNCProxyType() As mRemote.Connection.Protocol.VNC.ProxyType
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
            Set(ByVal value As mRemote.Connection.Protocol.VNC.ProxyType)
                _VNCProxyType = value
            End Set
        End Property

        Private _VNCProxyIP As String = My.Settings.ConDefaultVNCProxyIP
        <Category(Category6 & Language.Base.Props_Misc), _
            Browsable(True), _
            DisplayName(Language.Base.Props_ProxyIP), _
            Description(Language.Base.Descr_ProxyIP)> _
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
        <Category(Category6 & Language.Base.Props_Misc), _
            Browsable(True), _
            DisplayName(Language.Base.Props_ProxyPort), _
            Description(Language.Base.Descr_ProxyPort)> _
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
        <Category(Category6 & Language.Base.Props_Misc), _
            Browsable(True), _
            DisplayName(Language.Base.Props_ProxyUsername), _
            Description(Language.Base.Props_ProxyUsername)> _
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
        <Category(Category6 & Language.Base.Props_Misc), _
            Browsable(True), _
            DisplayName(Language.Base.Props_ProxyPassword), _
            Description(Language.Base.Descr_ProxyPassword), _
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

        Private _VNCColors As mRemote.Connection.Protocol.VNC.Colors = Tools.Misc.StringToEnum(GetType(Connection.Protocol.VNC.Colors), My.Settings.ConDefaultVNCColors)
        <Category(Category4 & Language.Base.Props_Appearance), _
           Browsable(True), _
           DisplayName(Language.Base.Props_Colors), _
           Description(Language.Base.Descr_Colors), _
           TypeConverter(GetType(Tools.Misc.EnumTypeConverter))> _
       Public Property VNCColors() As mRemote.Connection.Protocol.VNC.Colors
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
            Set(ByVal value As mRemote.Connection.Protocol.VNC.Colors)
                _VNCColors = value
            End Set
        End Property

        Private _VNCSmartSizeMode As mRemote.Connection.Protocol.VNC.SmartSizeMode = Tools.Misc.StringToEnum(GetType(Connection.Protocol.VNC.SmartSizeMode), My.Settings.ConDefaultVNCSmartSizeMode)
        <Category(Category4 & Language.Base.Props_Appearance), _
           Browsable(True), _
           DisplayName(Language.Base.Props_SmartSizeMode), _
           Description(Language.Base.Descr_SmartSizeMode), _
           TypeConverter(GetType(Tools.Misc.EnumTypeConverter))> _
        Public Property VNCSmartSizeMode() As mRemote.Connection.Protocol.VNC.SmartSizeMode
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
            Set(ByVal value As mRemote.Connection.Protocol.VNC.SmartSizeMode)
                _VNCSmartSizeMode = value
            End Set
        End Property

        Private _VNCViewOnly As Boolean = My.Settings.ConDefaultVNCViewOnly
        <Category(Category4 & Language.Base.Props_Appearance), _
           Browsable(True), _
           DisplayName(Language.Base.Props_ViewOnly), _
           Description(Language.Base.Descr_ViewOnly), _
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
                mC.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't set default port" & vbNewLine & ex.Message)
            End Try
        End Sub
#End Region

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
            <Category(Category1 & Language.Base.Props_General), _
                          Browsable(True), _
                          DisplayName(Language.Base.Props_InheritEverything), _
                          TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
                      Public Property EverythingInherited() As Boolean
                Get
                    If Me._CacheBitmaps And Me._Colors And Me._Description And Me._DisplayThemes And Me._DisplayWallpaper _
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
            <Category(Category2 & Language.Base.Props_Display), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_Description), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property Description() As Boolean
                Get
                    Return Me._Description
                End Get
                Set(ByVal value As Boolean)
                    Me._Description = value
                End Set
            End Property

            Private _Icon As Boolean = My.Settings.InhDefaultIcon
            <Category(Category2 & Language.Base.Props_Display), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_Icon), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property Icon() As Boolean
                Get
                    Return Me._Icon
                End Get
                Set(ByVal value As Boolean)
                    Me._Icon = value
                End Set
            End Property

            Private _Panel As Boolean = My.Settings.InhDefaultPanel
            <Category(Category2 & Language.Base.Props_Display), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_Panel), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
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
            <Category(Category3 & Language.Base.Props_Connection), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_Username), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property Username() As Boolean
                Get
                    Return Me._Username
                End Get
                Set(ByVal value As Boolean)
                    Me._Username = value
                End Set
            End Property

            Private _Password As Boolean = My.Settings.InhDefaultPassword
            <Category(Category3 & Language.Base.Props_Connection), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_Password), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property Password() As Boolean
                Get
                    Return Me._Password
                End Get
                Set(ByVal value As Boolean)
                    Me._Password = value
                End Set
            End Property

            Private _Domain As Boolean = My.Settings.InhDefaultDomain
            <Category(Category3 & Language.Base.Props_Connection), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_Domain), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
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
            <Category(Category4 & Language.Base.Props_Protocol), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_Protocol), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property Protocol() As Boolean
                Get
                    Return Me._Protocol
                End Get
                Set(ByVal value As Boolean)
                    Me._Protocol = value
                End Set
            End Property

            Private _ExtApp As Boolean = My.Settings.InhDefaultExtApp
            <Category(Category4 & Language.Base.Props_Protocol), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_ExtApp), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property ExtApp() As Boolean
                Get
                    Return Me._ExtApp
                End Get
                Set(ByVal value As Boolean)
                    Me._ExtApp = value
                End Set
            End Property

            Private _Port As Boolean = My.Settings.InhDefaultPort
            <Category(Category4 & Language.Base.Props_Protocol), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_Port), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property Port() As Boolean
                Get
                    Return Me._Port
                End Get
                Set(ByVal value As Boolean)
                    Me._Port = value
                End Set
            End Property

            Private _PuttySession As Boolean = My.Settings.InhDefaultPuttySession
            <Category(Category4 & Language.Base.Props_Protocol), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_PuttySession), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property PuttySession() As Boolean
                Get
                    Return Me._PuttySession
                End Get
                Set(ByVal value As Boolean)
                    Me._PuttySession = value
                End Set
            End Property

            Private _ICAEncryption As Boolean = My.Settings.InhDefaultICAEncryptionStrength
            <Category(Category4 & Language.Base.Props_Protocol), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_EncryrptionStrength), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property ICAEncryption() As Boolean
                Get
                    Return Me._ICAEncryption
                End Get
                Set(ByVal value As Boolean)
                    Me._ICAEncryption = value
                End Set
            End Property

            Private _RDPAuthenticationLevel As Boolean = My.Settings.InhDefaultRDPAuthenticationLevel
            <Category(Category4 & Language.Base.Props_Protocol), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_AuthenticationLevel), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property RDPAuthenticationLevel() As Boolean
                Get
                    Return Me._RDPAuthenticationLevel
                End Get
                Set(ByVal value As Boolean)
                    Me._RDPAuthenticationLevel = value
                End Set
            End Property

            Private _RenderingEngine As Boolean = My.Settings.InhDefaultRenderingEngine
            <Category(Category4 & Language.Base.Props_Protocol), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_RenderingEngine), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property RenderingEngine() As Boolean
                Get
                    Return Me._RenderingEngine
                End Get
                Set(ByVal value As Boolean)
                    Me._RenderingEngine = value
                End Set
            End Property

            Private _UseConsoleSession As Boolean = My.Settings.InhDefaultUseConsoleSession
            <Category(Category4 & Language.Base.Props_Protocol), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_UseConsoleSession), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property UseConsoleSession() As Boolean
                Get
                    Return Me._UseConsoleSession
                End Get
                Set(ByVal value As Boolean)
                    Me._UseConsoleSession = value
                End Set
            End Property
#End Region
#Region "5 Appearance"
            Private _Resolution As Boolean = My.Settings.InhDefaultResolution
            <Category(Category5 & Language.Base.Props_Appearance), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_Resolution), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property Resolution() As Boolean
                Get
                    Return Me._Resolution
                End Get
                Set(ByVal value As Boolean)
                    Me._Resolution = value
                End Set
            End Property

            Private _Colors As Boolean = My.Settings.InhDefaultColors
            <Category(Category5 & Language.Base.Props_Appearance), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_Colors), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property Colors() As Boolean
                Get
                    Return Me._Colors
                End Get
                Set(ByVal value As Boolean)
                    Me._Colors = value
                End Set
            End Property

            Private _CacheBitmaps As Boolean = My.Settings.InhDefaultCacheBitmaps
            <Category(Category5 & Language.Base.Props_Appearance), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_CacheBitmaps), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property CacheBitmaps() As Boolean
                Get
                    Return Me._CacheBitmaps
                End Get
                Set(ByVal value As Boolean)
                    Me._CacheBitmaps = value
                End Set
            End Property

            Private _DisplayWallpaper As Boolean = My.Settings.InhDefaultDisplayWallpaper
            <Category(Category5 & Language.Base.Props_Appearance), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_DisplayWallpaper), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property DisplayWallpaper() As Boolean
                Get
                    Return Me._DisplayWallpaper
                End Get
                Set(ByVal value As Boolean)
                    Me._DisplayWallpaper = value
                End Set
            End Property

            Private _DisplayThemes As Boolean = My.Settings.InhDefaultDisplayThemes
            <Category(Category5 & Language.Base.Props_Appearance), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_DisplayThemes), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property DisplayThemes() As Boolean
                Get
                    Return Me._DisplayThemes
                End Get
                Set(ByVal value As Boolean)
                    Me._DisplayThemes = value
                End Set
            End Property
#End Region
#Region "6 Redirect"
            Private _RedirectKeys As Boolean = My.Settings.InhDefaultRedirectKeys
            <Category(Category6 & Language.Base.Props_Redirect), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_RedKeyCombinations), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property RedirectKeys() As Boolean
                Get
                    Return Me._RedirectKeys
                End Get
                Set(ByVal value As Boolean)
                    Me._RedirectKeys = value
                End Set
            End Property

            Private _RedirectDiskDrives As Boolean = My.Settings.InhDefaultRedirectDiskDrives
            <Category(Category6 & Language.Base.Props_Redirect), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_RedDiskDrives), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property RedirectDiskDrives() As Boolean
                Get
                    Return Me._RedirectDiskDrives
                End Get
                Set(ByVal value As Boolean)
                    Me._RedirectDiskDrives = value
                End Set
            End Property

            Private _RedirectPrinters As Boolean = My.Settings.InhDefaultRedirectPrinters
            <Category(Category6 & Language.Base.Props_Redirect), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_RedPrinters), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property RedirectPrinters() As Boolean
                Get
                    Return Me._RedirectPrinters
                End Get
                Set(ByVal value As Boolean)
                    Me._RedirectPrinters = value
                End Set
            End Property

            Private _RedirectPorts As Boolean = My.Settings.InhDefaultRedirectPorts
            <Category(Category6 & Language.Base.Props_Redirect), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_RedPorts), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property RedirectPorts() As Boolean
                Get
                    Return Me._RedirectPorts
                End Get
                Set(ByVal value As Boolean)
                    Me._RedirectPorts = value
                End Set
            End Property

            Private _RedirectSmartCards As Boolean = My.Settings.InhDefaultRedirectSmartCards
            <Category(Category6 & Language.Base.Props_Redirect), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_RedSmartCards), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property RedirectSmartCards() As Boolean
                Get
                    Return Me._RedirectSmartCards
                End Get
                Set(ByVal value As Boolean)
                    Me._RedirectSmartCards = value
                End Set
            End Property

            Private _RedirectSound As Boolean = My.Settings.InhDefaultRedirectSound
            <Category(Category6 & Language.Base.Props_Redirect), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_RedSounds), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property RedirectSound() As Boolean
                Get
                    Return Me._RedirectSound
                End Get
                Set(ByVal value As Boolean)
                    Me._RedirectSound = value
                End Set
            End Property
#End Region
#Region "7 Misc"
            Private _PreExtApp As Boolean = My.Settings.InhDefaultPreExtApp
            <Category(Category7 & Language.Base.Props_Misc), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_PreExtApp), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property PreExtApp() As Boolean
                Get
                    Return Me._PreExtApp
                End Get
                Set(ByVal value As Boolean)
                    Me._PreExtApp = value
                End Set
            End Property

            Private _PostExtApp As Boolean = My.Settings.InhDefaultPostExtApp
            <Category(Category7 & Language.Base.Props_Misc), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_PostExtApp), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property PostExtApp() As Boolean
                Get
                    Return Me._PostExtApp
                End Get
                Set(ByVal value As Boolean)
                    Me._PostExtApp = value
                End Set
            End Property

            Private _MacAddress As Boolean = My.Settings.InhDefaultMacAddress
            <Category(Category7 & Language.Base.Props_Misc), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_MacAddress), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property MacAddress() As Boolean
                Get
                    Return Me._MacAddress
                End Get
                Set(ByVal value As Boolean)
                    Me._MacAddress = value
                End Set
            End Property

            Private _UserField As Boolean = My.Settings.InhDefaultUserField
            <Category(Category7 & Language.Base.Props_Misc), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_UserField), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property UserField() As Boolean
                Get
                    Return Me._UserField
                End Get
                Set(ByVal value As Boolean)
                    Me._UserField = value
                End Set
            End Property
#End Region
#Region "VNC"
            Private _VNCCompression As Boolean = My.Settings.InhDefaultVNCCompression
            <Category(Category5 & Language.Base.Props_Appearance), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_Compression), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCCompression() As Boolean
                Get
                    Return _VNCCompression
                End Get
                Set(ByVal value As Boolean)
                    _VNCCompression = value
                End Set
            End Property

            Private _VNCEncoding As Boolean = My.Settings.InhDefaultVNCEncoding
            <Category(Category5 & Language.Base.Props_Appearance), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_Encoding), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCEncoding() As Boolean
                Get
                    Return _VNCEncoding
                End Get
                Set(ByVal value As Boolean)
                    _VNCEncoding = value
                End Set
            End Property

            Private _VNCAuthMode As Boolean = My.Settings.InhDefaultVNCAuthMode
            <Category(Category3 & Language.Base.Props_Connection), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_AuthMode), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCAuthMode() As Boolean
                Get
                    Return _VNCAuthMode
                End Get
                Set(ByVal value As Boolean)
                    _VNCAuthMode = value
                End Set
            End Property

            Private _VNCProxyType As Boolean = My.Settings.InhDefaultVNCProxyType
            <Category(Category7 & Language.Base.Props_Misc), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_ProxyType), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCProxyType() As Boolean
                Get
                    Return _VNCProxyType
                End Get
                Set(ByVal value As Boolean)
                    _VNCProxyType = value
                End Set
            End Property

            Private _VNCProxyIP As Boolean = My.Settings.InhDefaultVNCProxyIP
            <Category(Category7 & Language.Base.Props_Misc), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_ProxyIP), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCProxyIP() As Boolean
                Get
                    Return _VNCProxyIP
                End Get
                Set(ByVal value As Boolean)
                    _VNCProxyIP = value
                End Set
            End Property

            Private _VNCProxyPort As Boolean = My.Settings.InhDefaultVNCProxyPort
            <Category(Category7 & Language.Base.Props_Misc), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_ProxyPort), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCProxyPort() As Boolean
                Get
                    Return _VNCProxyPort
                End Get
                Set(ByVal value As Boolean)
                    _VNCProxyPort = value
                End Set
            End Property

            Private _VNCProxyUsername As Boolean = My.Settings.InhDefaultVNCProxyUsername
            <Category(Category7 & Language.Base.Props_Misc), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_ProxyUsername), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCProxyUsername() As Boolean
                Get
                    Return _VNCProxyUsername
                End Get
                Set(ByVal value As Boolean)
                    _VNCProxyUsername = value
                End Set
            End Property

            Private _VNCProxyPassword As Boolean = My.Settings.InhDefaultVNCProxyPassword
            <Category(Category7 & Language.Base.Props_Misc), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_ProxyPassword), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCProxyPassword() As Boolean
                Get
                    Return _VNCProxyPassword
                End Get
                Set(ByVal value As Boolean)
                    _VNCProxyPassword = value
                End Set
            End Property

            Private _VNCColors As Boolean = My.Settings.InhDefaultVNCColors
            <Category(Category5 & Language.Base.Props_Appearance), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_Colors), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCColors() As Boolean
                Get
                    Return _VNCColors
                End Get
                Set(ByVal value As Boolean)
                    _VNCColors = value
                End Set
            End Property

            Private _VNCSmartSizeMode As Boolean = My.Settings.InhDefaultVNCSmartSizeMode
            <Category(Category5 & Language.Base.Props_Appearance), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_SmartSizeMode), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCSmartSizeMode() As Boolean
                Get
                    Return _VNCSmartSizeMode
                End Get
                Set(ByVal value As Boolean)
                    _VNCSmartSizeMode = value
                End Set
            End Property

            Private _VNCViewOnly As Boolean = My.Settings.InhDefaultVNCViewOnly
            <Category(Category5 & Language.Base.Props_Appearance), _
                Browsable(True), _
                DisplayName(Language.Base.Inherit & " " & Language.Base.Props_ViewOnly), _
                TypeConverter(GetType(mRemote.Tools.Misc.YesNoTypeConverter))> _
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