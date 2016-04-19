Imports System.ComponentModel
Imports mRemote3G.Tools

Namespace Connection
    Partial Public Class Info
        Public Class Inheritance

#Region "Public Properties"

#Region "General"

            <LocalizedAttributes.LocalizedCategory("strCategoryGeneral", 1),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameAll"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionAll"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property EverythingInherited As Boolean
                Get
                    If CacheBitmaps And Colors And Description And DisplayThemes And DisplayWallpaper _
                       And EnableFontSmoothing And EnableDesktopComposition And Domain And Icon And Password _
                       And Port And Protocol And PuttySession And RedirectDiskDrives And RedirectKeys _
                       And RedirectPorts And RedirectPrinters And RedirectSmartCards And RedirectSound And Resolution _
                       And AutomaticResize And UseConsoleSession And UseCredSsp And RenderingEngine And UserField _
                       And ExtApp And Username And Panel And RDPAuthenticationLevel _
                       And LoadBalanceInfo And PreExtApp And PostExtApp And MacAddress And VNCAuthMode _
                       And VNCColors And VNCCompression And VNCEncoding And VNCProxyIP And VNCProxyPassword _
                       And VNCProxyPort And VNCProxyType And VNCProxyUsername Then
                        Return True
                    Else
                        Return False
                    End If
                End Get
                Set
                    SetAllValues(value)
                End Set
            End Property

#End Region

#Region "Display"

            <LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 2),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameDescription"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionDescription"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property Description As Boolean = My.Settings.InhDefaultDescription

            <LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 2),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameIcon"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionIcon"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property Icon As Boolean = My.Settings.InhDefaultIcon

            <LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 2),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNamePanel"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionPanel"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property Panel As Boolean = My.Settings.InhDefaultPanel

#End Region

#Region "Connection"

            <LocalizedAttributes.LocalizedCategory("strCategoryConnection", 3),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameUsername"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionUsername"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property Username As Boolean = My.Settings.InhDefaultUsername

            <LocalizedAttributes.LocalizedCategory("strCategoryConnection", 3),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNamePassword"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionPassword"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property Password As Boolean = My.Settings.InhDefaultPassword

            <LocalizedAttributes.LocalizedCategory("strCategoryConnection", 3),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameDomain"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionDomain"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property Domain As Boolean = My.Settings.InhDefaultDomain

#End Region

#Region "Protocol"

            <LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameProtocol"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionProtocol"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property Protocol As Boolean = My.Settings.InhDefaultProtocol

            <LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameExternalTool"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionExternalTool"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property ExtApp As Boolean = My.Settings.InhDefaultExtApp

            <LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNamePort"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionPort"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property Port As Boolean = My.Settings.InhDefaultPort

            <LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNamePuttySession"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionPuttySession"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property PuttySession As Boolean = My.Settings.InhDefaultPuttySession

            <LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameAuthenticationLevel"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionAuthenticationLevel"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property RDPAuthenticationLevel As Boolean = My.Settings.InhDefaultRDPAuthenticationLevel

            <LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameLoadBalanceInfo"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionLoadBalanceInfo"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property LoadBalanceInfo As Boolean = My.Settings.InhDefaultLoadBalanceInfo

            <LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRenderingEngine"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRenderingEngine"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property RenderingEngine As Boolean = My.Settings.InhDefaultRenderingEngine

            <LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameUseConsoleSession"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionUseConsoleSession"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property UseConsoleSession As Boolean = My.Settings.InhDefaultUseConsoleSession

            <LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameUseCredSsp"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionUseCredSsp"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property UseCredSsp As Boolean = My.Settings.InhDefaultUseCredSsp

#End Region

#Region "RD Gateway"

            <LocalizedAttributes.LocalizedCategory("strCategoryGateway", 5),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRDGatewayUsageMethod"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRDGatewayUsageMethod"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property RDGatewayUsageMethod As Boolean = My.Settings.InhDefaultRDGatewayUsageMethod

            <LocalizedAttributes.LocalizedCategory("strCategoryGateway", 5),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRDGatewayHostname"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRDGatewayHostname"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property RDGatewayHostname As Boolean = My.Settings.InhDefaultRDGatewayHostname

            <LocalizedAttributes.LocalizedCategory("strCategoryGateway", 5),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRDGatewayUseConnectionCredentials"),
                LocalizedAttributes.LocalizedDescriptionInherit _
                    ("strPropertyDescriptionRDGatewayUseConnectionCredentials"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property RDGatewayUseConnectionCredentials As Boolean _
                = My.Settings.InhDefaultRDGatewayUseConnectionCredentials

            <LocalizedAttributes.LocalizedCategory("strCategoryGateway", 5),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRDGatewayUsername"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRDGatewayUsername"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property RDGatewayUsername As Boolean = My.Settings.InhDefaultRDGatewayUsername

            <LocalizedAttributes.LocalizedCategory("strCategoryGateway", 5),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRDGatewayPassword"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRDGatewayPassword"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property RDGatewayPassword As Boolean = My.Settings.InhDefaultRDGatewayPassword

            <LocalizedAttributes.LocalizedCategory("strCategoryGateway", 5),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRDGatewayDomain"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRDGatewayDomain"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property RDGatewayDomain As Boolean = My.Settings.InhDefaultRDGatewayDomain

#End Region

#Region "Appearance"

            <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameResolution"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionResolution"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property Resolution As Boolean = My.Settings.InhDefaultResolution

            <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameAutomaticResize"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionAutomaticResize"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property AutomaticResize As Boolean = My.Settings.InhDefaultAutomaticResize

            <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameColors"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionColors"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property Colors As Boolean = My.Settings.InhDefaultColors

            <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameCacheBitmaps"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionCacheBitmaps"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property CacheBitmaps As Boolean = My.Settings.InhDefaultCacheBitmaps

            <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameDisplayWallpaper"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionDisplayWallpaper"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property DisplayWallpaper As Boolean = My.Settings.InhDefaultDisplayWallpaper

            <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameDisplayThemes"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionDisplayThemes"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property DisplayThemes As Boolean = My.Settings.InhDefaultDisplayThemes

            <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameEnableFontSmoothing"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionEnableFontSmoothing"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property EnableFontSmoothing As Boolean = My.Settings.InhDefaultEnableFontSmoothing

            <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameEnableDesktopComposition"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionEnableEnableDesktopComposition"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property EnableDesktopComposition As Boolean = My.Settings.InhDefaultEnableDesktopComposition

#End Region

#Region "Redirect"

            <LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 7),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRedirectKeys"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRedirectKeys"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property RedirectKeys As Boolean = My.Settings.InhDefaultRedirectKeys

            <LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 7),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRedirectDrives"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRedirectDrives"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property RedirectDiskDrives As Boolean = My.Settings.InhDefaultRedirectDiskDrives

            <LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 7),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRedirectPrinters"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRedirectPrinters"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property RedirectPrinters As Boolean = My.Settings.InhDefaultRedirectPrinters

            <LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 7),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRedirectPorts"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRedirectPorts"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property RedirectPorts As Boolean = My.Settings.InhDefaultRedirectPorts

            <LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 7),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRedirectSmartCards"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRedirectSmartCards"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property RedirectSmartCards As Boolean = My.Settings.InhDefaultRedirectSmartCards

            <LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 7),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRedirectSounds"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRedirectSounds"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property RedirectSound As Boolean = My.Settings.InhDefaultRedirectSound

#End Region

#Region "Misc"

            <LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 8),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameExternalToolBefore"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionExternalToolBefore"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property PreExtApp As Boolean = My.Settings.InhDefaultPreExtApp

            <LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 8),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameExternalToolAfter"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionExternalToolAfter"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property PostExtApp As Boolean = My.Settings.InhDefaultPostExtApp

            <LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 8),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameMACAddress"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionMACAddress"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property MacAddress As Boolean = My.Settings.InhDefaultMacAddress

            <LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 8),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameUser1"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionUser1"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property UserField As Boolean = My.Settings.InhDefaultUserField

#End Region

#Region "VNC"

            <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 9),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameCompression"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionCompression"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property VNCCompression As Boolean = My.Settings.InhDefaultVNCCompression

            <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 9),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameEncoding"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionEncoding"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property VNCEncoding As Boolean = My.Settings.InhDefaultVNCEncoding

            <LocalizedAttributes.LocalizedCategory("strCategoryConnection", 9),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameAuthenticationMode"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionAuthenticationMode"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property VNCAuthMode As Boolean = My.Settings.InhDefaultVNCAuthMode

            <LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 9),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameVNCProxyType"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionVNCProxyType"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property VNCProxyType As Boolean = My.Settings.InhDefaultVNCProxyType

            <LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 9),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameVNCProxyAddress"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionVNCProxyAddress"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property VNCProxyIP As Boolean = My.Settings.InhDefaultVNCProxyIP

            <LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 9),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameVNCProxyPort"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionVNCProxyPort"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property VNCProxyPort As Boolean = My.Settings.InhDefaultVNCProxyPort

            <LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 9),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameVNCProxyUsername"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionVNCProxyUsername"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property VNCProxyUsername As Boolean = My.Settings.InhDefaultVNCProxyUsername

            <LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 9),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameVNCProxyPassword"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionVNCProxyPassword"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property VNCProxyPassword As Boolean = My.Settings.InhDefaultVNCProxyPassword

            <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 9),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameColors"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionColors"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property VNCColors As Boolean = My.Settings.InhDefaultVNCColors

            <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 9),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameSmartSizeMode"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionSmartSizeMode"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property VNCSmartSizeMode As Boolean = My.Settings.InhDefaultVNCSmartSizeMode

            <LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 9),
                LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameViewOnly"),
                LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionViewOnly"),
                TypeConverter(GetType(Misc.YesNoTypeConverter))>
            Public Property VNCViewOnly As Boolean = My.Settings.InhDefaultVNCViewOnly

#End Region

            <Browsable(False)>
            Public Property Parent As Object

            <Browsable(False)>
            Public Property IsDefault As Boolean

#End Region

#Region "Constructors"

            Public Sub New(parent As Object, Optional ByVal inheritEverything As Boolean = False)
                Me.Parent = parent
                If inheritEverything Then TurnOnInheritanceCompletely()
            End Sub

#End Region

#Region "Public Methods"

            Public Function Copy() As Inheritance
                Return MemberwiseClone()
            End Function

            Public Sub TurnOnInheritanceCompletely()
                SetAllValues(True)
            End Sub

            Public Sub TurnOffInheritanceCompletely()
                SetAllValues(False)
            End Sub

#End Region

#Region "Private Methods"

            Private Sub SetAllValues(value As Boolean)
                ' Display
                Description = value
                Icon = value
                Panel = value

                ' Connection
                Username = value
                Password = value
                Domain = value

                ' Protocol
                Protocol = value
                ExtApp = value
                Port = value
                PuttySession = value
                RDPAuthenticationLevel = value
                LoadBalanceInfo = value
                RenderingEngine = value
                UseConsoleSession = value
                UseCredSsp = value

                ' RD Gateway
                RDGatewayUsageMethod = value
                RDGatewayHostname = value
                RDGatewayUseConnectionCredentials = value
                RDGatewayUsername = value
                RDGatewayPassword = value
                RDGatewayDomain = value

                ' Appearance
                Resolution = value
                AutomaticResize = value
                Colors = value
                CacheBitmaps = value
                DisplayWallpaper = value
                DisplayThemes = value
                EnableFontSmoothing = value
                EnableDesktopComposition = value

                ' Redirect
                RedirectKeys = value
                RedirectDiskDrives = value
                RedirectPrinters = value
                RedirectPorts = value
                RedirectSmartCards = value
                RedirectSound = value

                ' Misc
                PreExtApp = value
                PostExtApp = value
                MacAddress = value
                UserField = value

                ' VNC
                VNCCompression = value
                VNCEncoding = value
                VNCAuthMode = value
                VNCProxyType = value
                VNCProxyIP = value
                VNCProxyPort = value
                VNCProxyUsername = value
                VNCProxyPassword = value
                VNCColors = value
                VNCSmartSizeMode = value
                VNCViewOnly = value
            End Sub

#End Region
        End Class
    End Class
End Namespace
