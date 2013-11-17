Imports System.ComponentModel
Imports mRemoteNG.Tools.LocalizedAttributes

Namespace Connection
    Partial Public Class Info
        Public Class Inheritance
#Region "Public Properties"
#Region "General"
            <LocalizedCategory("strCategoryGeneral", 1), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameAll"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionAll"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property EverythingInherited() As Boolean
                Get
                    If CacheBitmaps And Colors And Description And DisplayThemes And DisplayWallpaper _
                    And EnableFontSmoothing And EnableDesktopComposition And Domain And Icon And Password _
                    And Port And Protocol And PuttySession And RedirectDiskDrives And RedirectKeys _
                    And RedirectPorts And RedirectPrinters And RedirectSmartCards And RedirectSound And Resolution _
                    And AutomaticResize And UseConsoleSession And UseCredSsp And RenderingEngine And UserField _
                    And ExtApp And Username And Panel And ICAEncryption And RDPAuthenticationLevel _
                    And LoadBalanceInfo And PreExtApp And PostExtApp And MacAddress And VNCAuthMode _
                    And VNCColors And VNCCompression And VNCEncoding And VNCProxyIP And VNCProxyPassword _
                    And VNCProxyPort And VNCProxyType And VNCProxyUsername Then
                        Return True
                    Else
                        Return False
                    End If
                End Get
                Set(ByVal value As Boolean)
                    SetAllValues(value)
                End Set
            End Property
#End Region
#Region "Display"
            <LocalizedCategory("strCategoryDisplay", 2), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameDescription"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionDescription"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property Description() As Boolean = My.Settings.InhDefaultDescription

            <LocalizedCategory("strCategoryDisplay", 2), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameIcon"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionIcon"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property Icon() As Boolean = My.Settings.InhDefaultIcon

            <LocalizedCategory("strCategoryDisplay", 2), _
                LocalizedDisplayNameInheritAttribute("strPropertyNamePanel"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionPanel"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property Panel() As Boolean = My.Settings.InhDefaultPanel
#End Region
#Region "Connection"
            <LocalizedCategory("strCategoryConnection", 3), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameUsername"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionUsername"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property Username() As Boolean = My.Settings.InhDefaultUsername

            <LocalizedCategory("strCategoryConnection", 3), _
                LocalizedDisplayNameInheritAttribute("strPropertyNamePassword"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionPassword"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property Password() As Boolean = My.Settings.InhDefaultPassword

            <LocalizedCategory("strCategoryConnection", 3), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameDomain"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionDomain"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property Domain() As Boolean = My.Settings.InhDefaultDomain
#End Region
#Region "Protocol"
            <LocalizedCategory("strCategoryProtocol", 4), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameProtocol"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionProtocol"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property Protocol() As Boolean = My.Settings.InhDefaultProtocol

            <LocalizedCategory("strCategoryProtocol", 4), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameExternalTool"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionExternalTool"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property ExtApp() As Boolean = My.Settings.InhDefaultExtApp

            <LocalizedCategory("strCategoryProtocol", 4), _
                LocalizedDisplayNameInheritAttribute("strPropertyNamePort"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionPort"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property Port() As Boolean = My.Settings.InhDefaultPort

            <LocalizedCategory("strCategoryProtocol", 4), _
                LocalizedDisplayNameInheritAttribute("strPropertyNamePuttySession"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionPuttySession"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property PuttySession() As Boolean = My.Settings.InhDefaultPuttySession

            <LocalizedCategory("strCategoryProtocol", 4), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameEncryptionStrength"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionEncryptionStrength"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property ICAEncryption() As Boolean = My.Settings.InhDefaultICAEncryptionStrength

            <LocalizedCategory("strCategoryProtocol", 4), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameAuthenticationLevel"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionAuthenticationLevel"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property RDPAuthenticationLevel() As Boolean = My.Settings.InhDefaultRDPAuthenticationLevel

            <LocalizedCategory("strCategoryProtocol", 4), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameLoadBalanceInfo"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionLoadBalanceInfo"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property LoadBalanceInfo() As Boolean = My.Settings.InhDefaultLoadBalanceInfo

            <LocalizedCategory("strCategoryProtocol", 4), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRenderingEngine"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRenderingEngine"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property RenderingEngine() As Boolean = My.Settings.InhDefaultRenderingEngine

            <LocalizedCategory("strCategoryProtocol", 4), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameUseConsoleSession"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionUseConsoleSession"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property UseConsoleSession() As Boolean = My.Settings.InhDefaultUseConsoleSession

            <LocalizedCategory("strCategoryProtocol", 4), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameUseCredSsp"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionUseCredSsp"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property UseCredSsp() As Boolean = My.Settings.InhDefaultUseCredSsp
#End Region
#Region "RD Gateway"
            <LocalizedCategory("strCategoryGateway", 5), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayUsageMethod"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayUsageMethod"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property RDGatewayUsageMethod() As Boolean = My.Settings.InhDefaultRDGatewayUsageMethod

            <LocalizedCategory("strCategoryGateway", 5), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayHostname"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayHostname"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property RDGatewayHostname() As Boolean = My.Settings.InhDefaultRDGatewayHostname

            <LocalizedCategory("strCategoryGateway", 5), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayUseConnectionCredentials"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayUseConnectionCredentials"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property RDGatewayUseConnectionCredentials() As Boolean = My.Settings.InhDefaultRDGatewayUseConnectionCredentials

            <LocalizedCategory("strCategoryGateway", 5), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayUsername"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayUsername"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property RDGatewayUsername() As Boolean = My.Settings.InhDefaultRDGatewayUsername

            <LocalizedCategory("strCategoryGateway", 5), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayPassword"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayPassword"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property RDGatewayPassword() As Boolean = My.Settings.InhDefaultRDGatewayPassword

            <LocalizedCategory("strCategoryGateway", 5), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayDomain"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayDomain"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property RDGatewayDomain() As Boolean = My.Settings.InhDefaultRDGatewayDomain
 #End Region
#Region "Appearance"
            <LocalizedCategory("strCategoryAppearance", 6), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameResolution"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionResolution"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property Resolution() As Boolean = My.Settings.InhDefaultResolution

            <LocalizedCategory("strCategoryAppearance", 6), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameAutomaticResize"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionAutomaticResize"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property AutomaticResize() As Boolean = My.Settings.InhDefaultAutomaticResize

            <LocalizedCategory("strCategoryAppearance", 6), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameColors"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionColors"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property Colors() As Boolean = My.Settings.InhDefaultColors

            <LocalizedCategory("strCategoryAppearance", 6), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameCacheBitmaps"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionCacheBitmaps"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property CacheBitmaps() As Boolean = My.Settings.InhDefaultCacheBitmaps

            <LocalizedCategory("strCategoryAppearance", 6), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameDisplayWallpaper"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionDisplayWallpaper"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property DisplayWallpaper() As Boolean = My.Settings.InhDefaultDisplayWallpaper

            <LocalizedCategory("strCategoryAppearance", 6), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameDisplayThemes"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionDisplayThemes"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property DisplayThemes() As Boolean = My.Settings.InhDefaultDisplayThemes

            <LocalizedCategory("strCategoryAppearance", 6), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameEnableFontSmoothing"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionEnableFontSmoothing"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property EnableFontSmoothing() As Boolean = My.Settings.InhDefaultEnableFontSmoothing

            <LocalizedCategory("strCategoryAppearance", 6), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameEnableDesktopComposition"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionEnableEnableDesktopComposition"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property EnableDesktopComposition() As Boolean = My.Settings.InhDefaultEnableDesktopComposition
#End Region
#Region "Redirect"
            <LocalizedCategory("strCategoryRedirect", 7), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectKeys"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectKeys"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property RedirectKeys() As Boolean = My.Settings.InhDefaultRedirectKeys

            <LocalizedCategory("strCategoryRedirect", 7), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectDrives"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectDrives"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property RedirectDiskDrives() As Boolean = My.Settings.InhDefaultRedirectDiskDrives

            <LocalizedCategory("strCategoryRedirect", 7), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectPrinters"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectPrinters"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property RedirectPrinters() As Boolean = My.Settings.InhDefaultRedirectPrinters

            <LocalizedCategory("strCategoryRedirect", 7), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectPorts"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectPorts"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property RedirectPorts() As Boolean = My.Settings.InhDefaultRedirectPorts

            <LocalizedCategory("strCategoryRedirect", 7), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectSmartCards"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectSmartCards"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property RedirectSmartCards() As Boolean = My.Settings.InhDefaultRedirectSmartCards

            <LocalizedCategory("strCategoryRedirect", 7), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectSounds"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectSounds"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property RedirectSound() As Boolean = My.Settings.InhDefaultRedirectSound
#End Region
#Region "Misc"
            <LocalizedCategory("strCategoryMiscellaneous", 8), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameExternalToolBefore"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionExternalToolBefore"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property PreExtApp() As Boolean = My.Settings.InhDefaultPreExtApp

            <LocalizedCategory("strCategoryMiscellaneous", 8), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameExternalToolAfter"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionExternalToolAfter"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property PostExtApp() As Boolean = My.Settings.InhDefaultPostExtApp

            <LocalizedCategory("strCategoryMiscellaneous", 8), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameMACAddress"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionMACAddress"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property MacAddress() As Boolean = My.Settings.InhDefaultMacAddress

            <LocalizedCategory("strCategoryMiscellaneous", 8), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameUser1"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionUser1"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property UserField() As Boolean = My.Settings.InhDefaultUserField
#End Region
#Region "VNC"
            <LocalizedCategory("strCategoryAppearance", 9), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameCompression"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionCompression"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCCompression() As Boolean = My.Settings.InhDefaultVNCCompression

            <LocalizedCategory("strCategoryAppearance", 9), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameEncoding"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionEncoding"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCEncoding() As Boolean = My.Settings.InhDefaultVNCEncoding

            <LocalizedCategory("strCategoryConnection", 9), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameAuthenticationMode"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionAuthenticationMode"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCAuthMode() As Boolean = My.Settings.InhDefaultVNCAuthMode

            <LocalizedCategory("strCategoryMiscellaneous", 9), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameVNCProxyType"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionVNCProxyType"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCProxyType() As Boolean = My.Settings.InhDefaultVNCProxyType

            <LocalizedCategory("strCategoryMiscellaneous", 9), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameVNCProxyAddress"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionVNCProxyAddress"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCProxyIP() As Boolean = My.Settings.InhDefaultVNCProxyIP

            <LocalizedCategory("strCategoryMiscellaneous", 9), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameVNCProxyPort"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionVNCProxyPort"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCProxyPort() As Boolean = My.Settings.InhDefaultVNCProxyPort

            <LocalizedCategory("strCategoryMiscellaneous", 9), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameVNCProxyUsername"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionVNCProxyUsername"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCProxyUsername() As Boolean = My.Settings.InhDefaultVNCProxyUsername

            <LocalizedCategory("strCategoryMiscellaneous", 9), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameVNCProxyPassword"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionVNCProxyPassword"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCProxyPassword() As Boolean = My.Settings.InhDefaultVNCProxyPassword

            <LocalizedCategory("strCategoryAppearance", 9), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameColors"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionColors"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCColors() As Boolean = My.Settings.InhDefaultVNCColors

            <LocalizedCategory("strCategoryAppearance", 9), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameSmartSizeMode"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionSmartSizeMode"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCSmartSizeMode() As Boolean = My.Settings.InhDefaultVNCSmartSizeMode

            <LocalizedCategory("strCategoryAppearance", 9), _
                LocalizedDisplayNameInheritAttribute("strPropertyNameViewOnly"), _
                LocalizedDescriptionInheritAttribute("strPropertyDescriptionViewOnly"), _
                TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
            Public Property VNCViewOnly() As Boolean = My.Settings.InhDefaultVNCViewOnly
#End Region

            <Browsable(False)> _
            Public Property Parent() As Object

            <Browsable(False)> _
            Public Property IsDefault() As Boolean
#End Region

#Region "Constructors"
            Public Sub New(ByVal parent As Object, Optional ByVal inheritEverything As Boolean = False)
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
            Private Sub SetAllValues(ByVal value As Boolean)
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
                ICAEncryption = value
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
