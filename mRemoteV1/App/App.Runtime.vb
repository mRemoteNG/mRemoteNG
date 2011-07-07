Imports WeifenLuo.WinFormsUI.Docking
Imports System.IO
Imports Crownwood
Imports System.Threading
Imports System.Xml
Imports System.Environment
Imports System.Management

Namespace App
    Public Class Runtime
#Region "Public Declarations"
        Public Shared sL As Config.Settings.Load
        Public Shared sS As Config.Settings.Save

        Public Shared cL As Connection.List
        Public Shared prevCL As Connection.List
        Public Shared ctL As Container.List
        Public Shared prevCTL As Container.List
        Public Shared crL As Credential.List
        Public Shared prevCRL As Credential.List

        Public Shared wL As UI.Window.List
        Public Shared mC As Messages.Collector

        Public Shared SysTrayIcon As Tools.Controls.SysTrayIcon
        Public Shared SysMenu As Tools.SystemMenu

        Public Shared log As log4net.ILog

        Public Shared IsUpdateAvailable As Boolean
        Public Shared IsAnnouncementAvailable As Boolean
        Public Shared ConnectionsFileLoaded As Boolean

        Public Shared WithEvents tmrSqlWatcher As Timers.Timer
        Public Shared LastSQLUpdate As Date

        Public Shared LastSelected As String

        Public Shared DefaultConnection As mRemoteNG.Connection.Info
        Public Shared DefaultInheritance As mRemoteNG.Connection.Info.Inheritance

        Public Shared ExtApps As New ArrayList()
#End Region

        Public Class Windows
            Public Shared treeForm As UI.Window.Tree
            Public Shared treePanel As New DockContent
            Public Shared configForm As UI.Window.Config
            Public Shared configPanel As New DockContent
            Public Shared errorsForm As UI.Window.ErrorsAndInfos
            Public Shared errorsPanel As New DockContent
            Public Shared sessionsForm As UI.Window.Sessions
            Public Shared sessionsPanel As New DockContent
            Public Shared screenshotForm As UI.Window.ScreenshotManager
            Public Shared screenshotPanel As New DockContent
            Public Shared quickyForm As UI.Window.QuickConnect
            Public Shared quickyPanel As New DockContent
            Public Shared optionsForm As frmOptions
            Public Shared optionsPanel As New DockContent
            Public Shared saveasForm As UI.Window.SaveAs
            Public Shared saveasPanel As New DockContent
            Public Shared aboutForm As UI.Window.About
            Public Shared aboutPanel As New DockContent
            Public Shared updateForm As UI.Window.Update
            Public Shared updatePanel As New DockContent
            Public Shared sshtransferForm As UI.Window.SSHTransfer
            Public Shared sshtransferPanel As New DockContent
            Public Shared adimportForm As UI.Window.ADImport
            Public Shared adimportPanel As New DockContent
            Public Shared helpForm As UI.Window.Help
            Public Shared helpPanel As New DockContent
            Public Shared externalappsForm As UI.Window.ExternalApps
            Public Shared externalappsPanel As New DockContent
            Public Shared portscanForm As UI.Window.PortScan
            Public Shared portscanPanel As New DockContent
            Public Shared ultravncscForm As UI.Window.UltraVNCSC
            Public Shared ultravncscPanel As New DockContent
            Public Shared componentscheckForm As UI.Window.ComponentsCheck
            Public Shared componentscheckPanel As New DockContent
            Public Shared AnnouncementForm As UI.Window.Announcement
            Public Shared AnnouncementPanel As New DockContent

            Public Shared Sub Show(ByVal WindowType As UI.Window.Type, Optional ByVal PortScanMode As Tools.PortScan.PortScanMode = Tools.PortScan.PortScanMode.Normal)
                Try
                    Select Case WindowType
                        Case UI.Window.Type.About
                            Windows.aboutForm = New UI.Window.About(Windows.aboutPanel)
                            Windows.aboutPanel = Windows.aboutForm

                            Windows.aboutForm.Show(frmMain.pnlDock)
                        Case UI.Window.Type.ADImport
                            Windows.adimportForm = New UI.Window.ADImport(Windows.adimportPanel)
                            Windows.adimportPanel = Windows.adimportForm

                            Windows.adimportPanel.Show(frmMain.pnlDock)
                        Case UI.Window.Type.Options
                            Windows.optionsForm = New frmOptions(Windows.optionsPanel)
                            Windows.optionsForm.Show(frmMain.pnlDock)
                        Case UI.Window.Type.SaveAs
                            Windows.saveasForm = New UI.Window.SaveAs(Windows.saveasPanel)
                            Windows.saveasPanel = Windows.saveasForm

                            Windows.saveasForm.Show(frmMain.pnlDock)
                        Case UI.Window.Type.SSHTransfer
                            Windows.sshtransferForm = New UI.Window.SSHTransfer(Windows.sshtransferPanel)
                            Windows.sshtransferPanel = Windows.sshtransferForm

                            Windows.sshtransferForm.Show(frmMain.pnlDock)
                        Case UI.Window.Type.Update
                            Windows.updateForm = New UI.Window.Update(Windows.updatePanel)
                            Windows.updatePanel = Windows.updateForm

                            Windows.updateForm.Show(frmMain.pnlDock)
                        Case UI.Window.Type.Help
                            Windows.helpForm = New UI.Window.Help(Windows.helpPanel)
                            Windows.helpPanel = Windows.helpForm

                            Windows.helpForm.Show(frmMain.pnlDock)
                        Case UI.Window.Type.ExternalApps
                            Windows.externalappsForm = New UI.Window.ExternalApps(Windows.externalappsPanel)
                            Windows.externalappsPanel = Windows.externalappsForm

                            Windows.externalappsForm.Show(frmMain.pnlDock)
                        Case UI.Window.Type.PortScan
                            Windows.portscanForm = New UI.Window.PortScan(Windows.portscanPanel, PortScanMode)
                            Windows.portscanPanel = Windows.portscanForm

                            Windows.portscanForm.Show(frmMain.pnlDock)
                        Case UI.Window.Type.UltraVNCSC
                            Windows.ultravncscForm = New UI.Window.UltraVNCSC(Windows.ultravncscPanel)
                            Windows.ultravncscPanel = Windows.ultravncscForm

                            Windows.ultravncscForm.Show(frmMain.pnlDock)
                        Case UI.Window.Type.ComponentsCheck
                            Windows.componentscheckForm = New UI.Window.ComponentsCheck(Windows.componentscheckPanel)
                            Windows.componentscheckPanel = Windows.componentscheckForm

                            Windows.componentscheckForm.Show(frmMain.pnlDock)
                        Case UI.Window.Type.Announcement
                            Windows.AnnouncementForm = New UI.Window.Announcement(AnnouncementPanel)
                            Windows.AnnouncementPanel = Windows.AnnouncementForm

                            Windows.AnnouncementForm.Show(frmMain.pnlDock)
                    End Select
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "Show (App.Runtime.Windows) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Shared Sub ShowUpdatesTab()
                Windows.optionsForm = New frmOptions(Windows.optionsPanel)
                Windows.optionsForm.Show(frmMain.pnlDock, 5)
            End Sub
        End Class

        Public Class Screens
            Public Shared Sub SendFormToScreen(ByVal Screen As Screen)
                Dim wasMax As Boolean

                If frmMain.WindowState = FormWindowState.Maximized Then
                    wasMax = True
                    frmMain.WindowState = FormWindowState.Normal
                End If

                frmMain.Location = Screen.Bounds.Location

                If wasMax Then
                    frmMain.WindowState = FormWindowState.Maximized
                End If
            End Sub

            Public Shared Sub SendPanelToScreen(ByVal Panel As DockContent, ByVal Screen As Screen)
                Panel.DockState = DockState.Float
                Panel.ParentForm.Left = Screen.Bounds.Location.X
                Panel.ParentForm.Top = Screen.Bounds.Location.Y
            End Sub
        End Class

        Public Class Startup
            Public Shared Sub CreatePanels()
                Windows.configForm = New UI.Window.Config(Windows.configPanel)
                Windows.configPanel = Windows.configForm

                Windows.treeForm = New UI.Window.Tree(Windows.treePanel)
                Windows.treePanel = Windows.treeForm
                Tree.Node.TreeView = Windows.treeForm.tvConnections

                Windows.errorsForm = New UI.Window.ErrorsAndInfos(Windows.errorsPanel)
                Windows.errorsPanel = Windows.errorsForm

                Windows.sessionsForm = New UI.Window.Sessions(Windows.sessionsPanel)
                Windows.sessionsPanel = Windows.sessionsForm

                Windows.screenshotForm = New UI.Window.ScreenshotManager(Windows.screenshotPanel)
                Windows.screenshotPanel = Windows.screenshotForm

                Windows.quickyForm = New UI.Window.QuickConnect(Windows.quickyPanel)
                Windows.quickyPanel = Windows.quickyForm

                Windows.updateForm = New UI.Window.Update(Windows.updatePanel)
                Windows.updatePanel = Windows.updateForm

                Windows.AnnouncementForm = New UI.Window.Announcement(Windows.AnnouncementPanel)
                Windows.AnnouncementPanel = Windows.AnnouncementForm
            End Sub

            Public Shared Sub SetDefaultLayout()
                frmMain.pnlDock.Visible = False

                frmMain.pnlDock.DockLeftPortion = 0.25
                frmMain.pnlDock.DockRightPortion = 0.25
                frmMain.pnlDock.DockTopPortion = 0.25
                frmMain.pnlDock.DockBottomPortion = 0.25

                Windows.treePanel.Show(frmMain.pnlDock, DockState.DockLeft)
                Windows.configPanel.Show(frmMain.pnlDock)
                Windows.configPanel.DockTo(Windows.treePanel.Pane, DockStyle.Bottom, -1)

                Windows.quickyPanel.Show(frmMain.pnlDock, DockState.DockBottomAutoHide)
                Windows.screenshotPanel.Show(Windows.quickyPanel.Pane, Windows.quickyPanel)
                Windows.sessionsPanel.Show(Windows.quickyPanel.Pane, Windows.screenshotPanel)
                Windows.errorsPanel.Show(Windows.quickyPanel.Pane, Windows.sessionsPanel)

                Windows.screenshotForm.Hide()
                Windows.quickyForm.Hide()

                frmMain.pnlDock.Visible = True
            End Sub

            Public Shared Sub GetConnectionIcons()
                Dim iPath As String = My.Application.Info.DirectoryPath & "\Icons\"

                If Directory.Exists(iPath) = False Then
                    Exit Sub
                End If

                For Each f As String In Directory.GetFiles(iPath, "*.ico", SearchOption.AllDirectories)
                    Dim fInfo As New FileInfo(f)

                    Array.Resize(Connection.Icon.Icons, Connection.Icon.Icons.Length + 1)
                    Connection.Icon.Icons.SetValue(fInfo.Name.Replace(".ico", ""), Connection.Icon.Icons.Length - 1)
                Next
            End Sub

            Public Shared Sub GetPuttySessions()
                Connection.PuttySession.PuttySessions = Connection.Protocol.PuttyBase.GetSessions()
            End Sub

            Public Shared Sub CreateLogger()
                log4net.Config.XmlConfigurator.Configure(New FileInfo("mRemoteNG.exe.config"))
                log = log4net.LogManager.GetLogger("mRemoteNG.Log")
                log.InfoFormat("{0} started.", My.Application.Info.ProductName)
                log.InfoFormat("Command Line: {0}", Environment.GetCommandLineArgs)
                Try
                    Dim servicePack As Integer
                    For Each managementObject As ManagementObject In New ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem").Get()
                        servicePack = managementObject.GetPropertyValue("ServicePackMajorVersion")
                        If servicePack = 0 Then
                            log.InfoFormat("{0} {1}", managementObject.GetPropertyValue("Caption").Trim, managementObject.GetPropertyValue("OSArchitecture"))
                        Else
                            log.InfoFormat("{0} Service Pack {1} {2}", managementObject.GetPropertyValue("Caption").Trim, servicePack.ToString, managementObject.GetPropertyValue("OSArchitecture"))
                        End If
                    Next
                Catch ex As Exception
                    log.WarnFormat("Error retrieving operating system information from WMI. {0}", ex.Message)
                End Try
                log.InfoFormat("Microsoft .NET Framework {0}", System.Environment.Version.ToString)
#If Not PORTABLE Then
                log.InfoFormat("{0} {1}", My.Application.Info.ProductName.ToString, My.Application.Info.Version.ToString)
#Else
                log.InfoFormat("{0} {1} {2}", My.Application.Info.ProductName.ToString, My.Application.Info.Version.ToString, My.Resources.strLabelPortableEdition)
#End If
                log.InfoFormat("System Culture: {0}/{1}", Threading.Thread.CurrentThread.CurrentUICulture.Name, Threading.Thread.CurrentThread.CurrentUICulture.NativeName)
            End Sub

            Public Shared Sub UpdateCheck()
                If My.Settings.CheckForUpdatesAsked And My.Settings.CheckForUpdatesOnStartup Then
                    If My.Settings.UpdatePending Or My.Settings.CheckForUpdatesLastCheck < Date.Now.Subtract(TimeSpan.FromDays(My.Settings.CheckForUpdatesFrequencyDays)) Then
                        frmMain.tmrShowUpdate.Enabled = True
                        Windows.updateForm.CheckForUpdate()
                        AddHandler Windows.updateForm.UpdateCheckCompleted, AddressOf UpdateCheckComplete
                    End If
                End If
            End Sub

            Private Shared Sub UpdateCheckComplete(ByVal UpdateAvailable As Boolean)
                My.Settings.CheckForUpdatesLastCheck = Date.Now
                My.Settings.UpdatePending = UpdateAvailable
                IsUpdateAvailable = UpdateAvailable
            End Sub

            Public Shared Sub AnnouncementCheck()
                If My.Settings.CheckForUpdatesAsked And My.Settings.CheckForUpdatesOnStartup And App.Editions.Spanlink.Enabled = False Then
                    If My.Settings.CheckForUpdatesLastCheck < Date.Now.Subtract(TimeSpan.FromDays(My.Settings.CheckForUpdatesFrequencyDays)) Then
                        frmMain.tmrShowUpdate.Enabled = True
                        Windows.AnnouncementForm.CheckForAnnouncement()
                        AddHandler Windows.AnnouncementForm.AnnouncementCheckCompleted, AddressOf AnnouncementCheckComplete
                    End If
                End If
            End Sub

            Private Shared Sub AnnouncementCheckComplete(ByVal AnnouncementAvailable As Boolean)
                My.Settings.CheckForUpdatesLastCheck = Date.Now
                IsAnnouncementAvailable = AnnouncementAvailable
            End Sub

            Public Shared Sub ParseCommandLineArgs()
                Try
                    Dim cmd As New Tools.Misc.CMDArguments(Environment.GetCommandLineArgs)

                    Dim ConsParam As String = ""
                    If cmd("cons") IsNot Nothing Then
                        ConsParam = "cons"
                    End If
                    If cmd("c") IsNot Nothing Then
                        ConsParam = "c"
                    End If

                    Dim ResetPosParam As String = ""
                    If cmd("resetpos") IsNot Nothing Then
                        ResetPosParam = "resetpos"
                    End If
                    If cmd("rp") IsNot Nothing Then
                        ResetPosParam = "rp"
                    End If

                    Dim ResetPanelsParam As String = ""
                    If cmd("resetpanels") IsNot Nothing Then
                        ResetPanelsParam = "resetpanels"
                    End If
                    If cmd("rpnl") IsNot Nothing Then
                        ResetPanelsParam = "rpnl"
                    End If

                    Dim ResetToolbarsParam As String = ""
                    If cmd("resettoolbar") IsNot Nothing Then
                        ResetToolbarsParam = "resettoolbar"
                    End If
                    If cmd("rtbr") IsNot Nothing Then
                        ResetToolbarsParam = "rtbr"
                    End If

                    If cmd("reset") IsNot Nothing Then
                        ResetPosParam = "rp"
                        ResetPanelsParam = "rpnl"
                        ResetToolbarsParam = "rtbr"
                    End If

                    Dim NoReconnectParam As String = ""
                    If cmd("noreconnect") IsNot Nothing Then
                        NoReconnectParam = "noreconnect"
                    End If
                    If cmd("norc") IsNot Nothing Then
                        NoReconnectParam = "norc"
                    End If

                    If ConsParam <> "" Then
                        If File.Exists(cmd(ConsParam)) = False Then
                            If File.Exists(My.Application.Info.DirectoryPath & "\" & cmd(ConsParam)) Then
                                My.Settings.LoadConsFromCustomLocation = True
                                My.Settings.CustomConsPath = My.Application.Info.DirectoryPath & "\" & cmd(ConsParam)
                                Exit Sub
                            ElseIf File.Exists(App.Info.Connections.DefaultConnectionsPath & "\" & cmd(ConsParam)) Then
                                My.Settings.LoadConsFromCustomLocation = True
                                My.Settings.CustomConsPath = App.Info.Connections.DefaultConnectionsPath & "\" & cmd(ConsParam)
                                Exit Sub
                            End If
                        Else
                            My.Settings.LoadConsFromCustomLocation = True
                            My.Settings.CustomConsPath = cmd(ConsParam)
                            Exit Sub
                        End If
                    End If

                    If ResetPosParam <> "" Then
                        My.Settings.MainFormKiosk = False
                        My.Settings.MainFormLocation = New Point(999, 999)
                        My.Settings.MainFormSize = New Size(900, 600)
                        My.Settings.MainFormState = FormWindowState.Normal
                    End If

                    If ResetPanelsParam <> "" Then
                        My.Settings.ResetPanels = True
                    End If

                    If NoReconnectParam <> "" Then
                        My.Settings.NoReconnect = True
                    End If

                    If ResetToolbarsParam <> "" Then
                        My.Settings.ResetToolbars = True
                    End If
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strCommandLineArgsCouldNotBeParsed & vbNewLine & ex.Message)
                End Try
            End Sub

            Public Shared Sub CreateSQLUpdateHandlerAndStartTimer()
                If My.Settings.UseSQLServer = True Then
                    AddHandler Tools.Misc.SQLUpdateCheckFinished, AddressOf SQLUpdateCheckFinished
                    tmrSqlWatcher = New Timers.Timer(3000)
                    tmrSqlWatcher.Start()
                End If
            End Sub

            Public Shared Sub DestroySQLUpdateHandlerAndStopTimer()
                Try
                    LastSQLUpdate = Nothing
                    RemoveHandler Tools.Misc.SQLUpdateCheckFinished, AddressOf SQLUpdateCheckFinished
                    If tmrSqlWatcher IsNot Nothing Then
                        tmrSqlWatcher.Stop()
                        tmrSqlWatcher.Close()
                    End If
                Catch ex As Exception
                End Try
            End Sub
        End Class

        Public Class Shutdown
            Public Shared Sub Quit()
                frmMain.Close()
            End Sub

            Public Shared Sub BeforeQuit()
                Try
                    If App.Runtime.SysTrayIcon IsNot Nothing Then
                        If App.Runtime.SysTrayIcon.Disposed = False Then
                            App.Runtime.SysTrayIcon.Dispose()
                        End If
                    End If

                    If My.Settings.SaveConsOnExit Then
                        SaveConnections()
                    End If

                    If Editions.Spanlink.Enabled Then
                        If SaveReport() Then
                            ' ToDo: Change Report.log location
                            File.Delete(My.Application.Info.DirectoryPath & "\Report.log")
                        End If
                    End If

                    sS.Save()
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strSettingsCouldNotBeSavedOrTrayDispose & vbNewLine & ex.Message, True)
                End Try
            End Sub
        End Class

#Region "Default Connection"
        Public Shared Function DefaultConnectionFromSettings() As mRemoteNG.Connection.Info
            DefaultConnection = New mRemoteNG.Connection.Info
            DefaultConnection.IsDefault = True

            Return DefaultConnection
        End Function

        Public Shared Sub DefaultConnectionToSettings()
            With DefaultConnection
                My.Settings.ConDefaultDescription = .Description
                My.Settings.ConDefaultIcon = .Icon
                My.Settings.ConDefaultUsername = .Username
                My.Settings.ConDefaultPassword = .Password
                My.Settings.ConDefaultDomain = .Domain
                My.Settings.ConDefaultProtocol = .Protocol.ToString
                My.Settings.ConDefaultPuttySession = .PuttySession
                My.Settings.ConDefaultICAEncryptionStrength = .ICAEncryption.ToString
                My.Settings.ConDefaultRDPAuthenticationLevel = .RDPAuthenticationLevel.ToString
                My.Settings.ConDefaultUseConsoleSession = .UseConsoleSession
                My.Settings.ConDefaultRenderingEngine = .RenderingEngine.ToString
                My.Settings.ConDefaultResolution = .Resolution.ToString
                My.Settings.ConDefaultColors = .Colors.ToString
                My.Settings.ConDefaultCacheBitmaps = .CacheBitmaps
                My.Settings.ConDefaultDisplayWallpaper = .DisplayWallpaper
                My.Settings.ConDefaultDisplayThemes = .DisplayThemes
                My.Settings.ConDefaultEnableFontSmoothing = .EnableFontSmoothing
                My.Settings.ConDefaultEnableDesktopComposition = .EnableDesktopComposition
                My.Settings.ConDefaultRedirectKeys = .RedirectKeys
                My.Settings.ConDefaultRedirectDiskDrives = .RedirectDiskDrives
                My.Settings.ConDefaultRedirectPrinters = .RedirectPrinters
                My.Settings.ConDefaultRedirectPorts = .RedirectPorts
                My.Settings.ConDefaultRedirectSmartCards = .RedirectSmartCards
                My.Settings.ConDefaultRedirectSound = .RedirectSound.ToString
                My.Settings.ConDefaultPreExtApp = .PreExtApp
                My.Settings.ConDefaultPostExtApp = .PostExtApp
                My.Settings.ConDefaultMacAddress = .MacAddress
                My.Settings.ConDefaultUserField = .UserField
                My.Settings.ConDefaultVNCAuthMode = .VNCAuthMode.ToString
                My.Settings.ConDefaultVNCColors = .VNCColors.ToString
                My.Settings.ConDefaultVNCCompression = .VNCCompression.ToString
                My.Settings.ConDefaultVNCEncoding = .VNCEncoding.ToString
                My.Settings.ConDefaultVNCProxyIP = .VNCProxyIP
                My.Settings.ConDefaultVNCProxyPassword = .VNCProxyPassword
                My.Settings.ConDefaultVNCProxyPort = .VNCProxyPort
                My.Settings.ConDefaultVNCProxyType = .VNCProxyType.ToString
                My.Settings.ConDefaultVNCProxyUsername = .VNCProxyUsername
                My.Settings.ConDefaultVNCSmartSizeMode = .VNCSmartSizeMode.ToString
                My.Settings.ConDefaultVNCViewOnly = .VNCViewOnly
                My.Settings.ConDefaultExtApp = .ExtApp
                My.Settings.ConDefaultRDGatewayUsageMethod = .RDGatewayUsageMethod.ToString
                My.Settings.ConDefaultRDGatewayHostname = .RDGatewayHostname
                My.Settings.ConDefaultRDGatewayUsername = .RDGatewayUsername
                My.Settings.ConDefaultRDGatewayPassword = .RDGatewayPassword
                My.Settings.ConDefaultRDGatewayDomain = .RDGatewayDomain
                My.Settings.ConDefaultRDGatewayUseConnectionCredentials = .RDGatewayUseConnectionCredentials.ToString
            End With
        End Sub
#End Region

#Region "Default Inheritance"
        Public Shared Function DefaultInheritanceFromSettings() As mRemoteNG.Connection.Info.Inheritance
            DefaultInheritance = New mRemoteNG.Connection.Info.Inheritance(Nothing)
            DefaultInheritance.IsDefault = True

            Return DefaultInheritance
        End Function

        Public Shared Sub DefaultInheritanceToSettings()
            With DefaultInheritance
                My.Settings.InhDefaultDescription = .Description
                My.Settings.InhDefaultIcon = .Icon
                My.Settings.InhDefaultPanel = .Panel
                My.Settings.InhDefaultUsername = .Username
                My.Settings.InhDefaultPassword = .Password
                My.Settings.InhDefaultDomain = .Domain
                My.Settings.InhDefaultProtocol = .Protocol
                My.Settings.InhDefaultPort = .Port
                My.Settings.InhDefaultPuttySession = .PuttySession
                My.Settings.InhDefaultUseConsoleSession = .UseConsoleSession
                My.Settings.InhDefaultRenderingEngine = .RenderingEngine
                My.Settings.InhDefaultICAEncryptionStrength = .ICAEncryption
                My.Settings.InhDefaultRDPAuthenticationLevel = .RDPAuthenticationLevel
                My.Settings.InhDefaultResolution = .Resolution
                My.Settings.InhDefaultColors = .Colors
                My.Settings.InhDefaultCacheBitmaps = .CacheBitmaps
                My.Settings.InhDefaultDisplayWallpaper = .DisplayWallpaper
                My.Settings.InhDefaultDisplayThemes = .DisplayThemes
                My.Settings.InhDefaultEnableFontSmoothing = .EnableFontSmoothing
                My.Settings.InhDefaultEnableDesktopComposition = .EnableDesktopComposition
                My.Settings.InhDefaultRedirectKeys = .RedirectKeys
                My.Settings.InhDefaultRedirectDiskDrives = .RedirectDiskDrives
                My.Settings.InhDefaultRedirectPrinters = .RedirectPrinters
                My.Settings.InhDefaultRedirectPorts = .RedirectPorts
                My.Settings.InhDefaultRedirectSmartCards = .RedirectSmartCards
                My.Settings.InhDefaultRedirectSound = .RedirectSound
                My.Settings.InhDefaultPreExtApp = .PreExtApp
                My.Settings.InhDefaultPostExtApp = .PostExtApp
                My.Settings.InhDefaultMacAddress = .MacAddress
                My.Settings.InhDefaultUserField = .UserField
                ' VNC inheritance
                My.Settings.InhDefaultVNCAuthMode = .VNCAuthMode
                My.Settings.InhDefaultVNCColors = .VNCColors
                My.Settings.InhDefaultVNCCompression = .VNCCompression
                My.Settings.InhDefaultVNCEncoding = .VNCEncoding
                My.Settings.InhDefaultVNCProxyIP = .VNCProxyIP
                My.Settings.InhDefaultVNCProxyPassword = .VNCProxyPassword
                My.Settings.InhDefaultVNCProxyPort = .VNCProxyPort
                My.Settings.InhDefaultVNCProxyType = .VNCProxyType
                My.Settings.InhDefaultVNCProxyUsername = .VNCProxyUsername
                My.Settings.InhDefaultVNCSmartSizeMode = .VNCSmartSizeMode
                My.Settings.InhDefaultVNCViewOnly = .VNCViewOnly
                ' Ext. App inheritance
                My.Settings.InhDefaultExtApp = .ExtApp
                ' RDP gateway inheritance
                My.Settings.InhDefaultRDGatewayUsageMethod = .RDGatewayUsageMethod
                My.Settings.InhDefaultRDGatewayHostname = .RDGatewayHostname
                My.Settings.InhDefaultRDGatewayUsername = .RDGatewayUsername
                My.Settings.InhDefaultRDGatewayPassword = .RDGatewayPassword
                My.Settings.InhDefaultRDGatewayDomain = .RDGatewayDomain
                My.Settings.InhDefaultRDGatewayUseConnectionCredentials = .RDGatewayUseConnectionCredentials
            End With
        End Sub
#End Region

#Region "Panels"
        Public Shared Function AddPanel(Optional ByVal Title As String = "", Optional ByVal NoTabber As Boolean = False) As Form
            Try
                If Title = "" Then
                    Title = My.Resources.strNewPanel
                End If

                Dim pnlcForm As New DockContent
                Dim cForm As New UI.Window.Connection(pnlcForm)
                pnlcForm = cForm

                'create context menu
                Dim cMen As New ContextMenuStrip

                'create rename item
                Dim cMenRen As New ToolStripMenuItem
                cMenRen.Text = My.Resources.strRename
                cMenRen.Image = My.Resources.Rename
                cMenRen.Tag = pnlcForm
                AddHandler cMenRen.Click, AddressOf cMenConnectionPanelRename_Click

                Dim cMenScreens As New ToolStripMenuItem
                cMenScreens.Text = My.Resources.strSendTo
                cMenScreens.Image = My.Resources.Monitor
                cMenScreens.Tag = pnlcForm
                cMenScreens.DropDownItems.Add("Dummy")
                AddHandler cMenScreens.DropDownOpening, AddressOf cMenConnectionPanelScreens_DropDownOpening

                cMen.Items.AddRange(New ToolStripMenuItem() {cMenRen, cMenScreens})

                pnlcForm.TabPageContextMenuStrip = cMen

                TryCast(cForm, UI.Window.Connection).SetFormText(Title.Replace("&", "&&"))

                pnlcForm.Show(frmMain.pnlDock, DockState.Document)

                If NoTabber Then
                    TryCast(cForm, UI.Window.Connection).TabController.Dispose()
                Else
                    wL.Add(cForm)
                End If

                Return cForm
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't add panel" & vbNewLine & ex.Message)
                Return Nothing
            End Try
        End Function

        Private Shared Sub cMenConnectionPanelRename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Try
                Dim conW As UI.Window.Connection
                conW = sender.Tag

                Dim nTitle As String = InputBox(My.Resources.strNewTitle & ":", , sender.Tag.Text.Replace("&&", "&"))

                If nTitle <> "" Then
                    conW.SetFormText(nTitle.Replace("&", "&&"))
                End If
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't rename panel" & vbNewLine & ex.Message)
            End Try
        End Sub

        Private Shared Sub cMenConnectionPanelScreens_DropDownOpening(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Try
                Dim cMenScreens As ToolStripMenuItem = sender
                cMenScreens.DropDownItems.Clear()

                For i As Integer = 0 To Screen.AllScreens.Length - 1
                    Dim cMenScreen As New ToolStripMenuItem(My.Resources.strScreen & " " & i + 1)
                    cMenScreen.Tag = New ArrayList
                    cMenScreen.Image = My.Resources.Monitor_GoTo
                    TryCast(cMenScreen.Tag, ArrayList).Add(Screen.AllScreens(i))
                    TryCast(cMenScreen.Tag, ArrayList).Add(cMenScreens.Tag)
                    AddHandler cMenScreen.Click, AddressOf cMenConnectionPanelScreen_Click

                    cMenScreens.DropDownItems.Add(cMenScreen)
                Next
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't enumerate screens" & vbNewLine & ex.Message)
            End Try
        End Sub

        Private Shared Sub cMenConnectionPanelScreen_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                Dim screen As Screen = TryCast(sender, ToolStripMenuItem).Tag(0)
                Dim panel As DockContent = TryCast(sender, ToolStripMenuItem).Tag(1)
                Screens.SendPanelToScreen(panel, screen)
            Catch ex As Exception
            End Try
        End Sub
#End Region

#Region "Credential Loading/Saving"
        Public Shared Sub LoadCredentials()

        End Sub
#End Region

#Region "Connections Loading/Saving"
        Public Shared Sub NewConnections(ByVal filename As String)
            Try
                cL = New Connection.List
                ctL = New Container.List

                Dim conL As New Config.Connections.Load

                My.Settings.LoadConsFromCustomLocation = False

                Dim xW As New XmlTextWriter(filename, System.Text.Encoding.UTF8)
                xW.Formatting = Formatting.Indented
                xW.Indentation = 4

                xW.WriteStartDocument()
                xW.WriteStartElement(My.Resources.strConnections)
                xW.WriteAttributeString("Export", "", "False")
                xW.WriteAttributeString("Protected", "", "GiUis20DIbnYzWPcdaQKfjE2H5jh//L5v4RGrJMGNXuIq2CttB/d/BxaBP2LwRhY")
                xW.WriteAttributeString("ConfVersion", "", "2.2")

                xW.WriteEndElement()
                xW.WriteEndDocument()

                xW.Close()

                conL.ConnectionList = cL
                conL.ContainerList = ctL
                conL.Import = False

                Tree.Node.ResetTree()

                conL.RootTreeNode = Windows.treeForm.tvConnections.Nodes(0)

                ' Load config
                conL.ConnectionFileName = filename
                conL.Load()
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strCouldNotCreateNewConnectionsFile & vbNewLine & ex.Message)
            End Try
        End Sub

        Private Shared Sub LoadConnectionsBG(Optional ByVal WithDialog As Boolean = False, Optional ByVal Update As Boolean = False)
            _WithDialog = False
            _LoadUpdate = True

            Dim t As New Thread(AddressOf LoadConnectionsBGd)
            t.Start()
        End Sub

        Private Shared _WithDialog As Boolean = False
        Private Shared _LoadUpdate As Boolean = False
        Private Shared Sub LoadConnectionsBGd()
            LoadConnections(_WithDialog, _LoadUpdate)
        End Sub

        Public Shared Sub LoadConnections(Optional ByVal WithDialog As Boolean = False, Optional ByVal Update As Boolean = False)
            Try
                Dim tmrWasEnabled As Boolean
                If tmrSqlWatcher IsNot Nothing Then
                    tmrWasEnabled = tmrSqlWatcher.Enabled

                    If tmrSqlWatcher.Enabled = True Then
                        tmrSqlWatcher.Stop()
                    End If
                End If

                If cL IsNot Nothing And ctL IsNot Nothing Then
                    prevCL = cL.Copy
                    prevCTL = ctL.Copy
                End If

                cL = New Connection.List
                ctL = New Container.List

                Dim conL As New Config.Connections.Load

                If My.Settings.UseSQLServer = False Then
                    If WithDialog Then
                        Dim lD As OpenFileDialog = Tools.Controls.ConnectionsLoadDialog

                        If lD.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                            conL.ConnectionFileName = lD.FileName

                            If conL.ConnectionFileName = App.Info.Connections.DefaultConnectionsPath & "\" & App.Info.Connections.DefaultConnectionsFile Then
                                My.Settings.LoadConsFromCustomLocation = False
                            Else
                                My.Settings.LoadConsFromCustomLocation = True
                                My.Settings.CustomConsPath = conL.ConnectionFileName
                            End If
                        Else
                            Exit Sub
                        End If
                    Else
                        If My.Settings.LoadConsFromCustomLocation = False Then
                            Dim oldPath As String = GetFolderPath(SpecialFolder.LocalApplicationData) & "\" & My.Application.Info.ProductName & "\" & App.Info.Connections.DefaultConnectionsFile
                            Dim newPath As String = App.Info.Connections.DefaultConnectionsPath & "\" & App.Info.Connections.DefaultConnectionsFile
#If Not PORTABLE Then
                            If File.Exists(oldPath) Then
                                conL.ConnectionFileName = oldPath
                            Else
                                conL.ConnectionFileName = newPath
                            End If
#Else
                            conL.ConnectionFileName = newPath
#End If
                        Else
                            conL.ConnectionFileName = My.Settings.CustomConsPath
                            End If
                        End If

                    If File.Exists(conL.ConnectionFileName) = False Then
                        If WithDialog Then
                            mC.AddMessage(Messages.MessageClass.WarningMsg, String.Format(My.Resources.strConnectionsFileCouldNotBeLoaded, conL.ConnectionFileName))
                        Else
                            mC.AddMessage(Messages.MessageClass.InformationMsg, String.Format(My.Resources.strConnectionsFileCouldNotBeLoadedNew, conL.ConnectionFileName))
                            App.Runtime.NewConnections(conL.ConnectionFileName)
                        End If

                        Exit Sub
                    End If

                    Try
                        If App.Editions.Spanlink.Enabled = False Then
                            File.Copy(conL.ConnectionFileName, conL.ConnectionFileName & "_BAK", True)
                        End If
                    Catch ex As Exception
                        mC.AddMessage(Messages.MessageClass.WarningMsg, My.Resources.strConnectionsFileBackupFailed & vbNewLine & vbNewLine & ex.Message)
                    End Try
                End If

                conL.ConnectionList = cL
                conL.ContainerList = ctL

                If prevCL IsNot Nothing And prevCTL IsNot Nothing Then
                    conL.PreviousConnectionList = prevCL
                    conL.PreviousContainerList = prevCTL
                End If

                If Update = True Then
                    conL.PreviousSelected = LastSelected
                End If

                conL.Import = False

                Tree.Node.ResetTree()

                conL.RootTreeNode = Windows.treeForm.tvConnections.Nodes(0)

                conL.UseSQL = My.Settings.UseSQLServer
                conL.SQLHost = My.Settings.SQLHost
                conL.SQLDatabaseName = My.Settings.SQLDatabaseName
                conL.SQLUsername = My.Settings.SQLUser
                conL.SQLPassword = Security.Crypt.Decrypt(My.Settings.SQLPass, App.Info.General.EncryptionKey)
                conL.SQLUpdate = Update

                conL.Load()

                If My.Settings.UseSQLServer = True Then
                    LastSQLUpdate = Now
                End If

                If tmrWasEnabled And tmrSqlWatcher IsNot Nothing Then
                    tmrSqlWatcher.Start()
                End If
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strConnectionsFileCouldNotBeLoaded & vbNewLine & ex.Message)
            End Try
        End Sub

        Public Shared Sub ImportConnections()
            Try
                Dim lD As OpenFileDialog = Tools.Controls.ConnectionsLoadDialog
                lD.Multiselect = True

                If lD.ShowDialog = DialogResult.OK Then
                    Dim nNode As TreeNode = Nothing

                    For i As Integer = 0 To lD.FileNames.Length - 1
                        nNode = Tree.Node.AddNode(Tree.Node.Type.Container, "Import #" & i)

                        Dim nContI As New mRemoteNG.Container.Info()
                        nContI.TreeNode = nNode
                        nContI.ConnectionInfo = New mRemoteNG.Connection.Info(nContI)

                        If Tree.Node.SelectedNode IsNot Nothing Then
                            If Tree.Node.GetNodeType(Tree.Node.SelectedNode) = Tree.Node.Type.Container Then
                                nContI.Parent = Tree.Node.SelectedNode.Tag
                            End If
                        End If

                        nNode.Tag = nContI
                        ctL.Add(nContI)

                        Dim conL As New Config.Connections.Load
                        conL.ConnectionFileName = lD.FileNames(i)
                        conL.RootTreeNode = nNode
                        conL.Import = True
                        conL.ConnectionList = App.Runtime.cL
                        conL.ContainerList = App.Runtime.ctL

                        conL.Load()

                        Windows.treeForm.tvConnections.SelectedNode.Nodes.Add(nNode)
                    Next
                End If
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strConnectionsFileCouldNotBeImported & vbNewLine & ex.Message)
            End Try
        End Sub

        Public Shared Sub ImportConnectionsFromRDPFiles()
            Try
                Dim lD As OpenFileDialog = Tools.Controls.ConnectionsRDPImportDialog
                lD.Multiselect = True

                If lD.ShowDialog = DialogResult.OK Then
                    For i As Integer = 0 To lD.FileNames.Length - 1
                        Dim lines As String() = File.ReadAllLines(lD.FileNames(i))

                        Dim nNode As TreeNode = Tree.Node.AddNode(Tree.Node.Type.Connection, Path.GetFileNameWithoutExtension(lD.FileNames(i)))

                        Dim nConI As New mRemoteNG.Connection.Info()
                        nConI.Inherit = New Connection.Info.Inheritance(nConI)

                        nConI.Name = nNode.Text

                        For Each l As String In lines
                            Dim pName As String = l.Substring(0, l.IndexOf(":"))
                            Dim pValue As String = l.Substring(l.LastIndexOf(":") + 1)

                            Select Case LCase(pName)
                                Case "full address"
                                    nConI.Hostname = pValue
                                Case "server port"
                                    nConI.Port = pValue
                                Case "username"
                                    nConI.Username = pValue
                                Case "domain"
                                    nConI.Domain = pValue
                                Case "session bpp"
                                    Select Case pValue
                                        Case 8
                                            nConI.Colors = Connection.Protocol.RDP.RDPColors.Colors256
                                        Case 15
                                            nConI.Colors = Connection.Protocol.RDP.RDPColors.Colors15Bit
                                        Case 16
                                            nConI.Colors = Connection.Protocol.RDP.RDPColors.Colors16Bit
                                        Case 24
                                            nConI.Colors = Connection.Protocol.RDP.RDPColors.Colors24Bit
                                        Case 32
                                            nConI.Colors = Connection.Protocol.RDP.RDPColors.Colors32Bit
                                    End Select
                                Case "bitmapcachepersistenable"
                                    If pValue = 1 Then
                                        nConI.CacheBitmaps = True
                                    Else
                                        nConI.CacheBitmaps = False
                                    End If
                                Case "screen mode id"
                                    If pValue = 2 Then
                                        nConI.Resolution = Connection.Protocol.RDP.RDPResolutions.Fullscreen
                                    Else
                                        nConI.Resolution = Connection.Protocol.RDP.RDPResolutions.FitToWindow
                                    End If
                                Case "connect to console"
                                    If pValue = 1 Then
                                        nConI.UseConsoleSession = True
                                    End If
                                Case "disable wallpaper"
                                    If pValue = 1 Then
                                        nConI.DisplayWallpaper = True
                                    Else
                                        nConI.DisplayWallpaper = False
                                    End If
                                Case "disable themes"
                                    If pValue = 1 Then
                                        nConI.DisplayThemes = True
                                    Else
                                        nConI.DisplayThemes = False
                                    End If
                                Case "allow font smoothing"
                                    If pValue = 1 Then
                                        nConI.EnableFontSmoothing = True
                                    Else
                                        nConI.EnableFontSmoothing = False
                                    End If
                                Case "allow desktop composition"
                                    If pValue = 1 Then
                                        nConI.EnableDesktopComposition = True
                                    Else
                                        nConI.EnableDesktopComposition = False
                                    End If
                                Case "redirectsmartcards"
                                    If pValue = 1 Then
                                        nConI.RedirectSmartCards = True
                                    Else
                                        nConI.RedirectSmartCards = False
                                    End If
                                Case "redirectdrives"
                                    If pValue = 1 Then
                                        nConI.RedirectDiskDrives = True
                                    Else
                                        nConI.RedirectDiskDrives = False
                                    End If
                                Case "redirectcomports"
                                    If pValue = 1 Then
                                        nConI.RedirectPorts = True
                                    Else
                                        nConI.RedirectPorts = False
                                    End If
                                Case "redirectprinters"
                                    If pValue = 1 Then
                                        nConI.RedirectPrinters = True
                                    Else
                                        nConI.RedirectPrinters = False
                                    End If
                                Case "audiomode"
                                    Select Case pValue
                                        Case 0
                                            nConI.RedirectSound = Connection.Protocol.RDP.RDPSounds.BringToThisComputer
                                        Case 1
                                            nConI.RedirectSound = Connection.Protocol.RDP.RDPSounds.LeaveAtRemoteComputer
                                        Case 2
                                            nConI.RedirectSound = Connection.Protocol.RDP.RDPSounds.DoNotPlay
                                    End Select
                            End Select
                        Next

                        nNode.Tag = nConI
                        Windows.treeForm.tvConnections.SelectedNode.Nodes.Add(nNode)

                        If Tree.Node.GetNodeType(nNode.Parent) = Tree.Node.Type.Container Then
                            nConI.Parent = nNode.Parent.Tag
                        End If

                        cL.Add(nConI)
                    Next
                End If
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strRdpFileCouldNotBeImported & vbNewLine & vbNewLine & ex.Message)
            End Try
        End Sub

        Public Shared Sub ImportConnectionsFromPortScan(ByVal Hosts As ArrayList, ByVal Protocol As mRemoteNG.Connection.Protocol.Protocols)
            For Each Host As Tools.PortScan.ScanHost In Hosts
                Dim finalProt As mRemoteNG.Connection.Protocol.Protocols
                Dim protOK As Boolean = False

                Dim nNode As TreeNode = Tree.Node.AddNode(Tree.Node.Type.Connection, Host.HostNameWithoutDomain)

                Dim nConI As New mRemoteNG.Connection.Info()
                nConI.Inherit = New Connection.Info.Inheritance(nConI)

                nConI.Name = Host.HostNameWithoutDomain
                nConI.Hostname = Host.HostName

                Select Case Protocol
                    Case Connection.Protocol.Protocols.SSH2
                        If Host.SSH Then
                            finalProt = Connection.Protocol.Protocols.SSH2
                            protOK = True
                        End If
                    Case Connection.Protocol.Protocols.Telnet
                        If Host.Telnet Then
                            finalProt = Connection.Protocol.Protocols.Telnet
                            protOK = True
                        End If
                    Case Connection.Protocol.Protocols.HTTP
                        If Host.HTTP Then
                            finalProt = Connection.Protocol.Protocols.HTTP
                            protOK = True
                        End If
                    Case Connection.Protocol.Protocols.HTTPS
                        If Host.HTTPS Then
                            finalProt = Connection.Protocol.Protocols.HTTPS
                            protOK = True
                        End If
                    Case Connection.Protocol.Protocols.Rlogin
                        If Host.Rlogin Then
                            finalProt = Connection.Protocol.Protocols.Rlogin
                            protOK = True
                        End If
                    Case Connection.Protocol.Protocols.RDP
                        If Host.RDP Then
                            finalProt = Connection.Protocol.Protocols.RDP
                            protOK = True
                        End If
                    Case Connection.Protocol.Protocols.VNC
                        If Host.VNC Then
                            finalProt = Connection.Protocol.Protocols.VNC
                            protOK = True
                        End If
                End Select

                If protOK = False Then
                    nConI = Nothing
                Else
                    nConI.Protocol = finalProt
                    nConI.SetDefaultPort()

                    nNode.Tag = nConI
                    Windows.treeForm.tvConnections.SelectedNode.Nodes.Add(nNode)

                    If Tree.Node.GetNodeType(nNode.Parent) = Tree.Node.Type.Container Then
                        nConI.Parent = nNode.Parent.Tag
                    End If

                    cL.Add(nConI)
                End If
            Next
        End Sub

        Public Shared Sub ImportConnectionsFromCSV()

        End Sub

        Public Shared Sub SaveConnectionsBG()
            _SaveUpdate = True

            Dim t As New Thread(AddressOf SaveConnectionsBGd)
            t.Start()
        End Sub

        Private Shared _SaveUpdate As Boolean = False
        Private Shared _SaveLock As Object = New Object
        Private Shared Sub SaveConnectionsBGd()
            Monitor.Enter(_SaveLock)
            SaveConnections(_SaveUpdate)
            Monitor.Exit(_SaveLock)
        End Sub

        Public Shared Sub SaveConnections(Optional ByVal Update As Boolean = False)
            Try
                If Update = True And My.Settings.UseSQLServer = False Then
                    Exit Sub
                End If

                Dim tmrWasEnabled As Boolean

                If tmrSqlWatcher IsNot Nothing Then
                    tmrWasEnabled = tmrSqlWatcher.Enabled
                    If tmrSqlWatcher.Enabled = True Then
                        tmrSqlWatcher.Stop()
                    End If
                End If

                Dim conS As New Config.Connections.Save

                If My.Settings.UseSQLServer = False Then
                    If My.Settings.LoadConsFromCustomLocation = False Then
                        conS.ConnectionFileName = App.Info.Connections.DefaultConnectionsPath & "\" & App.Info.Connections.DefaultConnectionsFile
                    Else
                        conS.ConnectionFileName = My.Settings.CustomConsPath
                    End If
                End If

                conS.ConnectionList = cL
                conS.ContainerList = ctL
                conS.Export = False
                conS.SaveSecurity = New Security.Save(False)
                conS.RootTreeNode = Windows.treeForm.tvConnections.Nodes(0)

                If My.Settings.UseSQLServer = True Then
                    conS.SaveFormat = Config.Connections.Save.Format.SQL
                    conS.SQLHost = My.Settings.SQLHost
                    conS.SQLDatabaseName = My.Settings.SQLDatabaseName
                    conS.SQLUsername = My.Settings.SQLUser
                    conS.SQLPassword = Security.Crypt.Decrypt(My.Settings.SQLPass, App.Info.General.EncryptionKey)
                End If

                conS.Save()

                If My.Settings.UseSQLServer = True Then
                    LastSQLUpdate = Now
                End If

                If tmrWasEnabled Then
                    tmrSqlWatcher.Start()
                End If
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strConnectionsFileCouldNotBeSaved & vbNewLine & ex.Message)
            End Try
        End Sub

        Public Shared Sub SaveConnectionsAs(ByVal SaveSecurity As Security.Save, ByVal RootNode As TreeNode)
            Dim conS As New Config.Connections.Save
            Try
                Dim tmrWasEnabled As Boolean

                If tmrSqlWatcher IsNot Nothing Then
                    tmrWasEnabled = tmrSqlWatcher.Enabled
                    If tmrSqlWatcher.Enabled = True Then
                        tmrSqlWatcher.Stop()
                    End If
                End If


                Dim sD As SaveFileDialog = Tools.Controls.ConnectionsSaveAsDialog

                If sD.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                    conS.ConnectionFileName = sD.FileName
                Else
                    Exit Sub
                End If

                Select Case sD.FilterIndex
                    Case 1
                        conS.SaveFormat = Config.Connections.Save.Format.mRXML
                    Case 2
                        conS.SaveFormat = Config.Connections.Save.Format.mRCSV
                    Case 3
                        conS.SaveFormat = Config.Connections.Save.Format.vRDCSV
                End Select

                If RootNode Is Windows.treeForm.tvConnections.Nodes(0) Then
                    If conS.SaveFormat <> Config.Connections.Save.Format.mRXML And conS.SaveFormat <> Config.Connections.Save.Format.None Then
                    Else
                        If conS.ConnectionFileName = App.Info.Connections.DefaultConnectionsPath & "\" & App.Info.Connections.DefaultConnectionsFile Then
                            My.Settings.LoadConsFromCustomLocation = False
                        Else
                            My.Settings.LoadConsFromCustomLocation = True
                            My.Settings.CustomConsPath = conS.ConnectionFileName
                        End If
                    End If
                End If

                conS.ConnectionList = cL
                conS.ContainerList = ctL
                If RootNode IsNot Windows.treeForm.tvConnections.Nodes(0) Then
                    conS.Export = True
                End If
                conS.SaveSecurity = SaveSecurity
                conS.RootTreeNode = RootNode

                conS.Save()
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, String.Format(My.Resources.strConnectionsFileCouldNotSaveAs, conS.ConnectionFileName) & vbNewLine & ex.Message)
            End Try

        End Sub
#End Region

#Region "Opening Connection"
        Private Shared nProt As Connection.Protocol.Base
        Private Shared nCi As Connection.Info

        Public Shared Function CreateQuicky(ByVal ConString As String, Optional ByVal Protocol As Connection.Protocol.Protocols = Connection.Protocol.Protocols.NONE) As Connection.Info
            Try
                Dim Uri As System.Uri = New System.Uri("dummyscheme" + System.Uri.SchemeDelimiter + ConString)

                If Not String.IsNullOrEmpty(Uri.Host) Then
                    nCi = New Connection.Info

                    nCi.Name = String.Format(My.Resources.strQuick, Uri.Host)
                    nCi.Protocol = Protocol
                    nCi.Hostname = Uri.Host
                    If Uri.Port = -1 Then
                        nCi.Port = Nothing
                    Else
                        nCi.Port = Uri.Port
                    End If
                    nCi.IsQuicky = True

                    Windows.quickyForm.ConnectionInfo = nCi

                    If Protocol = Connection.Protocol.Protocols.NONE Then
                        Windows.quickyPanel.Show(frmMain.pnlDock, DockState.DockBottomAutoHide)
                    End If

                    Return nCi
                End If
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strQuickConnectFailed & vbNewLine & ex.Message)
            End Try

            Return Nothing
        End Function

        Public Shared Sub OpenConnection()
            Try
                OpenConnection(Connection.Info.Force.None)
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strConnectionOpenFailed & vbNewLine & ex.Message)
            End Try
        End Sub

        Public Shared Sub OpenConnection(ByVal Force As mRemoteNG.Connection.Info.Force)
            Try
                If Windows.treeForm.tvConnections.SelectedNode.Tag Is Nothing Then
                    Exit Sub
                End If

                If Tree.Node.GetNodeType(Tree.Node.SelectedNode) = Tree.Node.Type.Connection Then
                    OpenConnection(Windows.treeForm.tvConnections.SelectedNode.Tag, Force)
                ElseIf Tree.Node.GetNodeType(Tree.Node.SelectedNode) = Tree.Node.Type.Container Then
                    For Each tNode As TreeNode In Tree.Node.SelectedNode.Nodes
                        If Tree.Node.GetNodeType(tNode) = Tree.Node.Type.Connection Then
                            If tNode.Tag IsNot Nothing Then
                                OpenConnection(tNode.Tag, Force)
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strConnectionOpenFailed & vbNewLine & ex.Message)
            End Try
        End Sub

        Public Shared Sub OpenConnection(ByVal ConnectionInfo As mRemoteNG.Connection.Info)
            Try
                OpenConnection(ConnectionInfo, Connection.Info.Force.None)
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strConnectionOpenFailed & vbNewLine & ex.Message)
            End Try
        End Sub

        Public Shared Sub OpenConnection(ByVal ConnectionInfo As mRemoteNG.Connection.Info, ByVal ConnectionForm As Form)
            Try
                OpenConnectionFinal(ConnectionInfo, Connection.Info.Force.None, ConnectionForm)
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strConnectionOpenFailed & vbNewLine & ex.Message)
            End Try
        End Sub

        Public Shared Sub OpenConnection(ByVal ConnectionInfo As mRemoteNG.Connection.Info, ByVal ConnectionForm As Form, ByVal Force As Connection.Info.Force)
            Try
                OpenConnectionFinal(ConnectionInfo, Force, ConnectionForm)
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strConnectionOpenFailed & vbNewLine & ex.Message)
            End Try
        End Sub

        Public Shared Sub OpenConnection(ByVal ConnectionInfo As mRemoteNG.Connection.Info, ByVal Force As mRemoteNG.Connection.Info.Force)
            Try
                OpenConnectionFinal(ConnectionInfo, Force, Nothing)
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strConnectionOpenFailed & vbNewLine & ex.Message)
            End Try
        End Sub


        Private Shared Sub OpenConnectionFinal(ByVal nCi As mRemoteNG.Connection.Info, ByVal Force As mRemoteNG.Connection.Info.Force, ByVal ConForm As Form)
            Try
                If nCi.Hostname = "" And nCi.Protocol <> Connection.Protocol.Protocols.IntApp Then
                    mC.AddMessage(Messages.MessageClass.WarningMsg, My.Resources.strConnectionOpenFailedNoHostname)
                    Exit Sub
                End If

                If nCi.PreExtApp <> "" Then
                    Dim extA As Tools.ExternalApp = App.Runtime.GetExtAppByName(nCi.PreExtApp)
                    If extA IsNot Nothing Then
                        extA.Start(nCi)
                    End If
                End If

                If (Force And Connection.Info.Force.DoNotJump) <> Connection.Info.Force.DoNotJump Then
                    If SwitchToOpenConnection(nCi) Then
                        Exit Sub
                    End If
                End If

                ' Create connection based on protocol type
                Select Case nCi.Protocol
                    Case Connection.Protocol.Protocols.RDP
                        nProt = New Connection.Protocol.RDP
                    Case Connection.Protocol.Protocols.VNC
                        nProt = New Connection.Protocol.VNC
                    Case Connection.Protocol.Protocols.SSH1
                        nProt = New Connection.Protocol.SSH1
                    Case Connection.Protocol.Protocols.SSH2
                        nProt = New Connection.Protocol.SSH2
                    Case Connection.Protocol.Protocols.Telnet
                        nProt = New Connection.Protocol.Telnet
                    Case Connection.Protocol.Protocols.Rlogin
                        nProt = New Connection.Protocol.Rlogin
                    Case Connection.Protocol.Protocols.RAW
                        nProt = New Connection.Protocol.RAW
                    Case Connection.Protocol.Protocols.HTTP
                        nProt = New Connection.Protocol.HTTP(nCi.RenderingEngine)
                    Case Connection.Protocol.Protocols.HTTPS
                        nProt = New Connection.Protocol.HTTPS(nCi.RenderingEngine)
                    Case Connection.Protocol.Protocols.ICA
                        nProt = New Connection.Protocol.ICA
                    Case Connection.Protocol.Protocols.IntApp
                        nProt = New Connection.Protocol.IntApp

                        If nCi.ExtApp = "" Then
                            Throw New Exception(My.Resources.strNoExtAppDefined)
                        End If
                    Case Else
                        Exit Sub
                End Select

                Dim cContainer As Control
                Dim cForm As Form

                Dim cPnl As String
                If nCi.Panel = "" Or (Force And Connection.Info.Force.OverridePanel) = Connection.Info.Force.OverridePanel Or My.Settings.AlwaysShowPanelSelectionDlg Then
                    Dim frmPnl As New frmChoosePanel
                    If frmPnl.ShowDialog = DialogResult.OK Then
                        cPnl = frmPnl.Panel
                    Else
                        Exit Sub
                    End If
                Else
                    cPnl = nCi.Panel
                End If

                If ConForm Is Nothing Then
                    cForm = wL.FromString(cPnl)
                Else
                    cForm = ConForm
                End If

                If cForm Is Nothing Then
                    cForm = AddPanel(cPnl)
                    cForm.Focus()
                Else
                    TryCast(cForm, UI.Window.Connection).Show(frmMain.pnlDock)
                    TryCast(cForm, UI.Window.Connection).Focus()
                End If

                cContainer = TryCast(cForm, UI.Window.Connection).AddConnectionTab(nCi)

                If nCi.Protocol = Connection.Protocol.Protocols.IntApp Then
                    If App.Runtime.GetExtAppByName(nCi.ExtApp).Icon IsNot Nothing Then
                        TryCast(cContainer, Magic.Controls.TabPage).Icon = App.Runtime.GetExtAppByName(nCi.ExtApp).Icon
                    End If
                End If

                AddHandler nProt.Closed, AddressOf TryCast(cForm, UI.Window.Connection).Prot_Event_Closed

                AddHandler nProt.Disconnected, AddressOf Prot_Event_Disconnected
                AddHandler nProt.Connected, AddressOf Prot_Event_Connected
                AddHandler nProt.Closed, AddressOf Prot_Event_Closed
                AddHandler nProt.ErrorOccured, AddressOf Prot_Event_ErrorOccured

                nProt.InterfaceControl = New Connection.InterfaceControl(cContainer, nProt, nCi)

                nProt.Force = Force

                If nProt.SetProps() = False Then
                    nProt.Close()
                    Exit Sub
                End If

                If nProt.Connect() = False Then
                    nProt.Close()
                    Exit Sub
                End If

                nCi.OpenConnections.Add(nProt)

                If nCi.IsQuicky = False Then
                    If nCi.Protocol <> Connection.Protocol.Protocols.IntApp Then
                        Tree.Node.SetNodeImage(nCi.TreeNode, Images.Enums.TreeImage.ConnectionOpen)
                    Else
                        Dim extApp As Tools.ExternalApp = GetExtAppByName(nCi.ExtApp)
                        If extApp IsNot Nothing Then
                            If extApp.TryIntegrate Then
                                If nCi.TreeNode IsNot Nothing Then
                                    Tree.Node.SetNodeImage(nCi.TreeNode, Images.Enums.TreeImage.ConnectionOpen)
                                End If
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strConnectionOpenFailed & vbNewLine & ex.Message)
            End Try
        End Sub

        Public Shared Function SwitchToOpenConnection(ByVal nCi As Connection.Info) As Boolean
            Dim IC As mRemoteNG.Connection.InterfaceControl = FindConnectionContainer(nCi)

            If IC IsNot Nothing Then
                TryCast(IC.FindForm, UI.Window.Connection).Focus()
                TryCast(IC.FindForm, UI.Window.Connection).Show(frmMain.pnlDock)
                Dim t As Magic.Controls.TabPage = IC.Parent
                t.Selected = True
                Return True
            End If

            Return False
        End Function
#End Region

#Region "Event Handlers"
        Public Shared Sub Prot_Event_Disconnected(ByVal sender As Object, ByVal DisconnectedMessage As String)
            Try
                mC.AddMessage(Messages.MessageClass.InformationMsg, String.Format(My.Resources.strProtocolEventDisconnected, DisconnectedMessage), True)

                Dim Prot As Connection.Protocol.Base = sender
                If Prot.InterfaceControl.Info.Protocol = Connection.Protocol.Protocols.RDP Then
                    Dim Reason As String() = DisconnectedMessage.Split(vbCrLf)
                    Dim ReasonCode As String = Reason(0)
                    Dim ReasonDescription As String = Reason(1)
                    If ReasonCode > 3 Then
                        If ReasonDescription <> "" Then
                            mC.AddMessage(Messages.MessageClass.WarningMsg, My.Resources.strRdpDisconnected & vbNewLine & ReasonDescription & vbNewLine & String.Format(My.Resources.strErrorCode, ReasonCode))
                        Else
                            mC.AddMessage(Messages.MessageClass.WarningMsg, My.Resources.strRdpDisconnected & vbNewLine & String.Format(My.Resources.strErrorCode, ReasonCode))
                        End If
                    End If
                End If
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, String.Format(My.Resources.strProtocolEventDisconnectFailed, ex.Message), True)
            End Try
        End Sub

        Public Shared Sub Prot_Event_Closed(ByVal sender As Object)
            Try
                Dim Prot As Connection.Protocol.Base = sender

                mC.AddMessage(Messages.MessageClass.InformationMsg, My.Resources.strConnenctionCloseEvent, True)

                If App.Editions.Spanlink.Enabled Then
                    mC.AddMessage(Messages.MessageClass.ReportMsg, String.Format(My.Resources.strConnenctionClosedByUserDetail, Prot.InterfaceControl.Info.Hostname, Prot.InterfaceControl.Info.Protocol.ToString, My.User.Name, Prot.InterfaceControl.Info.Description, Prot.InterfaceControl.Info.UserField))
                Else
                    mC.AddMessage(Messages.MessageClass.ReportMsg, String.Format(My.Resources.strConnenctionClosedByUser, Prot.InterfaceControl.Info.Hostname, Prot.InterfaceControl.Info.Protocol.ToString, My.User.Name))
                End If

                Prot.InterfaceControl.Info.OpenConnections.Remove(Prot)

                If Prot.InterfaceControl.Info.OpenConnections.Count < 1 And Prot.InterfaceControl.Info.IsQuicky = False Then
                    Tree.Node.SetNodeImage(Prot.InterfaceControl.Info.TreeNode, Images.Enums.TreeImage.ConnectionClosed)
                End If

                If Prot.InterfaceControl.Info.PostExtApp <> "" Then
                    Dim extA As Tools.ExternalApp = App.Runtime.GetExtAppByName(Prot.InterfaceControl.Info.PostExtApp)
                    If extA IsNot Nothing Then
                        extA.Start(Prot.InterfaceControl.Info)
                    End If
                End If
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strConnenctionCloseEventFailed & vbNewLine & ex.Message, True)
            End Try
        End Sub

        Public Shared Sub Prot_Event_Connected(ByVal sender As Object)
            Dim prot As mRemoteNG.Connection.Protocol.Base = sender

            mC.AddMessage(Messages.MessageClass.InformationMsg, My.Resources.strConnectionEventConnected, True)
            mC.AddMessage(Messages.MessageClass.ReportMsg, String.Format(My.Resources.strConnectionEventConnectedDetail, prot.InterfaceControl.Info.Hostname, prot.InterfaceControl.Info.Protocol.ToString, My.User.Name, prot.InterfaceControl.Info.Description, prot.InterfaceControl.Info.UserField))
        End Sub

        Public Shared Sub Prot_Event_ErrorOccured(ByVal sender As Object, ByVal ErrorMessage As String)
            Try
                mC.AddMessage(Messages.MessageClass.InformationMsg, My.Resources.strConnectionEventErrorOccured, True)

                Dim Prot As Connection.Protocol.Base = sender

                If Prot.InterfaceControl.Info.Protocol = Connection.Protocol.Protocols.RDP Then
                    If ErrorMessage > -1 Then
                        mC.AddMessage(Messages.MessageClass.WarningMsg, String.Format(My.Resources.strConnectionRdpErrorDetail, ErrorMessage, Connection.Protocol.RDP.FatalErrors.GetError(ErrorMessage)))
                    End If
                End If
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strConnectionEventConnectionFailed & vbNewLine & ex.Message, True)
            End Try
        End Sub
#End Region

#Region "External Apps"
        Public Shared Sub GetExtApps()
            Array.Clear(Tools.ExternalAppsTypeConverter.ExternalApps, 0, Tools.ExternalAppsTypeConverter.ExternalApps.Length)
            Array.Resize(Tools.ExternalAppsTypeConverter.ExternalApps, ExtApps.Count + 1)

            Dim i As Integer = 0

            For Each extA As Tools.ExternalApp In ExtApps
                Tools.ExternalAppsTypeConverter.ExternalApps(i) = extA.DisplayName

                i += 1
            Next

            Tools.ExternalAppsTypeConverter.ExternalApps(i) = ""
        End Sub

        Public Shared Function GetExtAppByName(ByVal Name As String) As Tools.ExternalApp
            For Each extA As Tools.ExternalApp In ExtApps
                If extA.DisplayName = Name Then
                    Return extA
                End If
            Next

            Return Nothing
        End Function
#End Region

#Region "Misc"
        Public Shared Sub GoToURL(ByVal URL As String)
            Dim cI As New mRemoteNG.Connection.Info

            cI.Name = ""
            cI.Hostname = URL
            If URL.StartsWith("https:") Then
                cI.Protocol = Connection.Protocol.Protocols.HTTPS
            Else
                cI.Protocol = Connection.Protocol.Protocols.HTTP
            End If
            cI.SetDefaultPort()
            cI.IsQuicky = True

            App.Runtime.OpenConnection(cI, mRemoteNG.Connection.Info.Force.DoNotJump)
        End Sub

        Public Shared Sub GoToWebsite()
            GoToURL(App.Info.General.URLHome)
        End Sub

        Public Shared Sub GoToDonate()
            GoToURL(App.Info.General.URLDonate)
        End Sub

        Public Shared Sub GoToForum()
            GoToURL(App.Info.General.URLForum)
        End Sub

        Public Shared Sub GoToBugs()
            GoToURL(App.Info.General.URLBugs)
        End Sub

        Public Shared Sub Report(ByVal Text As String)
            Try
                Dim sWr As New StreamWriter(My.Application.Info.DirectoryPath & "\Report.log", True)
                sWr.WriteLine(Text)
                sWr.Close()
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strLogWriteToFileFailed)
            End Try
        End Sub

        Public Shared Function SaveReport() As Boolean
            Try
                Dim sRd As New StreamReader(My.Application.Info.DirectoryPath & "\Report.log")
                Dim Text As String = sRd.ReadToEnd
                sRd.Close()

                Dim sWr As New StreamWriter(App.Info.General.ReportingFilePath, True)
                sWr.Write(Text)
                sWr.Close()

                Return True
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strLogWriteToFileFinalLocationFailed & vbNewLine & ex.Message, True)
                Return False
            End Try
        End Function

        Public Shared Sub SetMainFormText(Optional ByVal ConnectionFileName As String = "")
            Try
                Dim txt As String = My.Application.Info.ProductName

                If App.Editions.Spanlink.Enabled Then
                    txt &= " | Spanlink Communications"
                Else
                    If ConnectionFileName <> "" And ConnectionsFileLoaded = True Then
                        If My.Settings.ShowCompleteConsPathInTitle Then
                            txt &= " - " & ConnectionFileName
                        Else
                            txt &= " - " & ConnectionFileName.Substring(ConnectionFileName.LastIndexOf("\") + 1)
                        End If
                    End If
                End If

                ChangeMainFormText(txt)
            Catch ex As Exception
                mC.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strSettingMainFormTextFailed & vbNewLine & ex.Message, True)
            End Try
        End Sub

        Private Delegate Sub ChangeMainFormTextCB(ByVal Text As String)
        Private Shared Sub ChangeMainFormText(ByVal Text As String)
            If frmMain.InvokeRequired = True Then
                Dim d As New ChangeMainFormTextCB(AddressOf ChangeMainFormText)
                frmMain.Invoke(d, New Object() {Text})
            Else
                frmMain.Text = Text
            End If
        End Sub

        Public Shared Function FindConnectionContainer(ByVal ConI As Connection.Info) As Connection.InterfaceControl
            If ConI.OpenConnections.Count > 0 Then
                For i As Integer = 0 To wL.Count - 1
                    If TypeOf wL.Items(i) Is UI.Window.Connection Then
                        Dim conW As UI.Window.Connection = wL.Items(i)

                        If conW.TabController IsNot Nothing Then
                            For Each t As Magic.Controls.TabPage In conW.TabController.TabPages
                                If t.Controls(0) IsNot Nothing Then
                                    If TypeOf t.Controls(0) Is Connection.InterfaceControl Then
                                        Dim IC As Connection.InterfaceControl = t.Controls(0)
                                        If IC.Info Is ConI Then
                                            Return IC
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If
                Next
            End If

            Return Nothing
        End Function

        ' Override the font of all controls in a container with the default font based on the OS version
        Public Shared Sub FontOverride(ByRef ctlParent As Control)
            Dim ctlChild As Control
            For Each ctlChild In ctlParent.Controls
                ctlChild.Font = New System.Drawing.Font(SystemFonts.MessageBoxFont.Name, ctlChild.Font.Size, ctlChild.Font.Style, ctlChild.Font.Unit, ctlChild.Font.GdiCharSet)
                If ctlChild.Controls.Count > 0 Then
                    FontOverride(ctlChild)
                End If
            Next
        End Sub
#End Region

#Region "SQL Watcher"
        Private Shared Sub tmrSqlWatcher_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles tmrSqlWatcher.Elapsed
            Tools.Misc.IsSQLUpdateAvailableBG()
        End Sub

        Private Shared Sub SQLUpdateCheckFinished(ByVal UpdateAvailable As Boolean)
            If UpdateAvailable = True Then
                mC.AddMessage(Messages.MessageClass.InformationMsg, My.Resources.strSqlUpdateCheckUpdateAvailable, True)
                LoadConnectionsBG()
            End If
        End Sub
#End Region

    End Class
End Namespace