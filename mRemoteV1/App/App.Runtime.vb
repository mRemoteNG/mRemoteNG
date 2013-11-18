Imports System.ComponentModel
Imports mRemoteNG.Forms
Imports mRemoteNG.Config
Imports log4net
Imports mRemoteNG.Messages
Imports mRemoteNG.Connection
Imports mRemoteNG.Tools
Imports mRemoteNG.Forms.OptionsPages
Imports PSTaskDialog
Imports mRemoteNG.Config.Putty
Imports WeifenLuo.WinFormsUI.Docking
Imports System.IO
Imports Crownwood
Imports System.Threading
Imports System.Xml
Imports System.Environment
Imports System.Management
Imports Microsoft.Win32
Imports Timer = System.Timers.Timer

Namespace App
    Public Class Runtime
        Private Sub New()
            ' Fix Warning 292 CA1053 : Microsoft.Design : Because type 'Native' contains only 'static' ('Shared' in Visual Basic) members, add a default private constructor to prevent the compiler from adding a default public constructor.
        End Sub

#Region "Public Properties"
        Public Shared Property MainForm As frmMain

        Private Shared _connectionList As Connection.List
        Public Shared Property ConnectionList() As List
            Get
                Return _connectionList
            End Get
            Set(ByVal value As List)
                _connectionList = value
            End Set
        End Property

        Private Shared _previousConnectionList As Connection.List
        Public Shared Property PreviousConnectionList() As List
            Get
                Return _previousConnectionList
            End Get
            Set(ByVal value As List)
                _previousConnectionList = value
            End Set
        End Property

        Private Shared _containerList As Container.List
        Public Shared Property ContainerList() As Container.List
            Get
                Return _containerList
            End Get
            Set(ByVal value As Container.List)
                _containerList = value
            End Set
        End Property

        Private Shared _previousContainerList As Container.List
        Public Shared Property PreviousContainerList() As Container.List
            Get
                Return _previousContainerList
            End Get
            Set(ByVal value As Container.List)
                _previousContainerList = value
            End Set
        End Property

        Private Shared _credentialList As Credential.List
        Public Shared Property CredentialList() As Credential.List
            Get
                Return _credentialList
            End Get
            Set(ByVal value As Credential.List)
                _credentialList = value
            End Set
        End Property

        Private Shared _previousCredentialList As Credential.List
        Public Shared Property PreviousCredentialList() As Credential.List
            Get
                Return _previousCredentialList
            End Get
            Set(ByVal value As Credential.List)
                _previousCredentialList = value
            End Set
        End Property


        Private Shared _windowList As UI.Window.List
        Public Shared Property WindowList() As UI.Window.List
            Get
                Return _windowList
            End Get
            Set(ByVal value As UI.Window.List)
                _windowList = value
            End Set
        End Property

        Private Shared _messageCollector As Messages.Collector
        Public Shared Property MessageCollector() As Collector
            Get
                Return _messageCollector
            End Get
            Set(ByVal value As Collector)
                _messageCollector = value
            End Set
        End Property

        Private Shared _notificationAreaIcon As Tools.Controls.NotificationAreaIcon
        Public Shared Property NotificationAreaIcon() As Tools.Controls.NotificationAreaIcon
            Get
                Return _notificationAreaIcon
            End Get
            Set(ByVal value As Tools.Controls.NotificationAreaIcon)
                _notificationAreaIcon = value
            End Set
        End Property

        Private Shared _systemMenu As Tools.SystemMenu
        Public Shared Property SystemMenu() As SystemMenu
            Get
                Return _systemMenu
            End Get
            Set(ByVal value As SystemMenu)
                _systemMenu = value
            End Set
        End Property

        Private Shared _log As log4net.ILog
        Public Shared Property Log() As ILog
            Get
                Return _log
            End Get
            Set(ByVal value As ILog)
                _log = value
            End Set
        End Property

        Private Shared _isConnectionsFileLoaded As Boolean
        Public Shared Property IsConnectionsFileLoaded() As Boolean
            Get
                Return _isConnectionsFileLoaded
            End Get
            Set(ByVal value As Boolean)
                _isConnectionsFileLoaded = value
            End Set
        End Property

        Private Shared WithEvents _timerSqlWatcher As Timers.Timer
        Public Shared Property TimerSqlWatcher() As Timer
            Get
                Return _timerSqlWatcher
            End Get
            Set(ByVal value As Timer)
                _timerSqlWatcher = value
            End Set
        End Property

        Private Shared _lastSqlUpdate As Date
        Public Shared Property LastSqlUpdate() As Date
            Get
                Return _lastSqlUpdate
            End Get
            Set(ByVal value As Date)
                _lastSqlUpdate = value
            End Set
        End Property

        Private Shared _lastSelected As String
        Public Shared Property LastSelected() As String
            Get
                Return _lastSelected
            End Get
            Set(ByVal value As String)
                _lastSelected = value
            End Set
        End Property

        Private Shared _defaultConnection As mRemoteNG.Connection.Info
        Public Shared Property DefaultConnection() As Connection.Info
            Get
                Return _defaultConnection
            End Get
            Set(ByVal value As Connection.Info)
                _defaultConnection = value
            End Set
        End Property

        Private Shared _defaultInheritance As mRemoteNG.Connection.Info.Inheritance
        Public Shared Property DefaultInheritance() As Connection.Info.Inheritance
            Get
                Return _defaultInheritance
            End Get
            Set(ByVal value As Connection.Info.Inheritance)
                _defaultInheritance = value
            End Set
        End Property

        Private Shared _externalTools As New ArrayList()
        Public Shared Property ExternalTools() As ArrayList
            Get
                Return _externalTools
            End Get
            Set(ByVal value As ArrayList)
                _externalTools = value
            End Set
        End Property

#End Region

#Region "Classes"
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
            Public Shared exportForm As UI.Window.Export
            Public Shared exportPanel As New DockContent
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

            Public Shared Sub Show(ByVal windowType As UI.Window.Type, Optional ByVal portScanMode As PortScan.PortScanMode = PortScan.PortScanMode.Normal)
                Try
                    Select Case windowType
                        Case UI.Window.Type.About
                            If aboutForm Is Nothing OrElse aboutForm.IsDisposed Then
                                aboutForm = New UI.Window.About(aboutPanel)
                                aboutPanel = aboutForm
                            End If

                            aboutForm.Show(frmMain.pnlDock)
                        Case UI.Window.Type.ADImport
                            If adimportForm Is Nothing OrElse adimportForm.IsDisposed Then
                                adimportForm = New UI.Window.ADImport(adimportPanel)
                                adimportPanel = adimportForm
                            End If

                            adimportPanel.Show(frmMain.pnlDock)
                        Case UI.Window.Type.Options
                            Using optionsForm As New OptionsForm()
                                optionsForm.ShowDialog(frmMain)
                            End Using
                        Case UI.Window.Type.Export
                            If exportForm Is Nothing OrElse exportForm.IsDisposed Then
                                exportForm = New UI.Window.Export(exportPanel)
                                exportPanel = exportForm
                            End If

                            exportForm.Show(frmMain.pnlDock)
                        Case UI.Window.Type.SSHTransfer
                            sshtransferForm = New UI.Window.SSHTransfer(sshtransferPanel)
                            sshtransferPanel = sshtransferForm

                            sshtransferForm.Show(frmMain.pnlDock)
                        Case UI.Window.Type.Update
                            If updateForm Is Nothing OrElse updateForm.IsDisposed Then
                                updateForm = New UI.Window.Update(updatePanel)
                                updatePanel = updateForm
                            End If

                            updateForm.Show(frmMain.pnlDock)
                        Case UI.Window.Type.Help
                            If helpForm Is Nothing OrElse helpForm.IsDisposed Then
                                helpForm = New UI.Window.Help(helpPanel)
                                helpPanel = helpForm
                            End If

                            helpForm.Show(frmMain.pnlDock)
                        Case UI.Window.Type.ExternalApps
                            If externalappsForm Is Nothing OrElse externalappsForm.IsDisposed Then
                                externalappsForm = New UI.Window.ExternalApps(externalappsPanel)
                                externalappsPanel = externalappsForm
                            End If

                            externalappsForm.Show(frmMain.pnlDock)
                        Case UI.Window.Type.PortScan
                            portscanForm = New UI.Window.PortScan(portscanPanel, portScanMode)
                            portscanPanel = portscanForm

                            portscanForm.Show(frmMain.pnlDock)
                        Case UI.Window.Type.UltraVNCSC
                            If ultravncscForm Is Nothing OrElse ultravncscForm.IsDisposed Then
                                ultravncscForm = New UI.Window.UltraVNCSC(ultravncscPanel)
                                ultravncscPanel = ultravncscForm
                            End If

                            ultravncscForm.Show(frmMain.pnlDock)
                        Case UI.Window.Type.ComponentsCheck
                            If componentscheckForm Is Nothing OrElse componentscheckForm.IsDisposed Then
                                componentscheckForm = New UI.Window.ComponentsCheck(componentscheckPanel)
                                componentscheckPanel = componentscheckForm
                            End If

                            componentscheckForm.Show(frmMain.pnlDock)
                        Case UI.Window.Type.Announcement
                            If AnnouncementForm Is Nothing OrElse AnnouncementForm.IsDisposed Then
                                AnnouncementForm = New UI.Window.Announcement(AnnouncementPanel)
                                AnnouncementPanel = AnnouncementForm
                            End If

                            AnnouncementForm.Show(frmMain.pnlDock)
                    End Select
                Catch ex As Exception
                    MessageCollector.AddMessage(MessageClass.ErrorMsg, "App.Runtime.Windows.Show() failed." & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Shared Sub ShowUpdatesTab()
                Using optionsForm As New OptionsForm()
                    optionsForm.ShowDialog(frmMain, GetType(UpdatesPage))
                End Using
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
            Public Shared Sub CheckCompatibility()
                CheckFipsPolicy()
                CheckLenovoAutoScrollUtility()
            End Sub

            Private Shared Sub CheckFipsPolicy()
                Dim regKey As RegistryKey

                Dim isFipsPolicyEnabled As Boolean = False

                ' Windows XP/Windows Server 2003
                regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("System\CurrentControlSet\Control\Lsa")
                If regKey IsNot Nothing Then
                    If Not regKey.GetValue("FIPSAlgorithmPolicy") = 0 Then isFipsPolicyEnabled = True
                End If

                ' Windows Vista/Windows Server 2008 and newer
                regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("System\CurrentControlSet\Control\Lsa\FIPSAlgorithmPolicy")
                If regKey IsNot Nothing Then
                    If Not regKey.GetValue("Enabled") = 0 Then isFipsPolicyEnabled = True
                End If

                If isFipsPolicyEnabled Then
                    MessageBox.Show(frmMain, String.Format(My.Language.strErrorFipsPolicyIncompatible, My.Application.Info.ProductName), My.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    [Exit](1)
                End If
            End Sub

            Private Shared Sub CheckLenovoAutoScrollUtility()
                If Not My.Settings.CompatibilityWarnLenovoAutoScrollUtility Then Return

                Dim proccesses() As Process = {}
                Try
                    proccesses = Process.GetProcessesByName("virtscrl")
                Catch
                End Try
                If proccesses.Length = 0 Then Return

                cTaskDialog.MessageBox(Application.ProductName, My.Language.strCompatibilityProblemDetected, String.Format(My.Language.strCompatibilityLenovoAutoScrollUtilityDetected, System.Windows.Forms.Application.ProductName), "", "", My.Language.strCheckboxDoNotShowThisMessageAgain, eTaskDialogButtons.OK, eSysIcons.Warning, Nothing)
                If cTaskDialog.VerificationChecked Then
                    My.Settings.CompatibilityWarnLenovoAutoScrollUtility = False
                End If
            End Sub

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

                Windows.updateForm = New UI.Window.Update(Windows.updatePanel)
                Windows.updatePanel = Windows.updateForm

                Windows.AnnouncementForm = New UI.Window.Announcement(Windows.AnnouncementPanel)
                Windows.AnnouncementPanel = Windows.AnnouncementForm
            End Sub

            Public Shared Sub SetDefaultLayout()
                frmMain.pnlDock.Visible = False

                frmMain.pnlDock.DockLeftPortion = frmMain.pnlDock.Width * 0.2
                frmMain.pnlDock.DockRightPortion = frmMain.pnlDock.Width * 0.2
                frmMain.pnlDock.DockTopPortion = frmMain.pnlDock.Height * 0.25
                frmMain.pnlDock.DockBottomPortion = frmMain.pnlDock.Height * 0.25

                Windows.treePanel.Show(frmMain.pnlDock, DockState.DockLeft)
                Windows.configPanel.Show(frmMain.pnlDock)
                Windows.configPanel.DockTo(Windows.treePanel.Pane, DockStyle.Bottom, -1)

                Windows.screenshotForm.Hide()

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

            Public Shared Sub CreateLogger()
                log4net.Config.XmlConfigurator.Configure()

                Dim logFilePath As String
#If Not PORTABLE Then
                logFilePath = Path.Combine(GetFolderPath(SpecialFolder.LocalApplicationData), Application.ProductName)
#Else
                logFilePath = Application.StartupPath
#End If
                Dim logFileName As String = Path.ChangeExtension(Application.ProductName, ".log")
                Dim logFile As String = Path.Combine(logFilePath, logFileName)

                Dim repository As Repository.ILoggerRepository = LogManager.GetRepository()
                Dim appenders As Appender.IAppender() = repository.GetAppenders()
                Dim fileAppender As Appender.FileAppender
                For Each appender As Appender.IAppender In appenders
                    fileAppender = TryCast(appender, Appender.FileAppender)
                    If Not (fileAppender Is Nothing OrElse Not fileAppender.Name = "LogFileAppender") Then
                        fileAppender.File = logFile
                        fileAppender.ActivateOptions()
                    End If
                Next

                Log = LogManager.GetLogger("Logger")

                If My.Settings.WriteLogFile Then
#If Not PORTABLE Then
                    Log.InfoFormat("{0} {1} starting.", Application.ProductName, Application.ProductVersion)
#Else
                    Log.InfoFormat("{0} {1} {2} starting.", Application.ProductName, Application.ProductVersion, My.Language.strLabelPortableEdition)
#End If
                    Log.InfoFormat("Command Line: {0}", Environment.GetCommandLineArgs)

                    Dim osVersion As String = String.Empty
                    Dim servicePack As String = String.Empty
                    Try
                        For Each managementObject As ManagementObject In New ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem WHERE Primary=True").Get()
                            osVersion = managementObject.GetPropertyValue("Caption").Trim()
                            Dim servicePackNumber As Integer = managementObject.GetPropertyValue("ServicePackMajorVersion")
                            If Not servicePackNumber = 0 Then servicePack = String.Format("Service Pack {0}", servicePackNumber)
                        Next
                    Catch ex As Exception
                        Log.WarnFormat("Error retrieving operating system information from WMI. {0}", ex.Message)
                    End Try

                    Dim architecture As String = String.Empty
                    Try
                        For Each managementObject As ManagementObject In New ManagementObjectSearcher("SELECT * FROM Win32_Processor WHERE DeviceID='CPU0'").Get()
                            Dim addressWidth As Integer = managementObject.GetPropertyValue("AddressWidth")
                            architecture = String.Format("{0}-bit", addressWidth)
                        Next
                    Catch ex As Exception
                        Log.WarnFormat("Error retrieving operating system address width from WMI. {0}", ex.Message)
                    End Try

                    Log.InfoFormat(String.Join(" ", Array.FindAll(New String() {osVersion, servicePack, architecture}, Function(s) Not String.IsNullOrEmpty(s))))

                    Log.InfoFormat("Microsoft .NET CLR {0}", Version.ToString)
                    Log.InfoFormat("System Culture: {0}/{1}", Thread.CurrentThread.CurrentUICulture.Name, Thread.CurrentThread.CurrentUICulture.NativeName)
                End If
            End Sub

            Private Shared _appUpdate As Update
            Public Shared Sub CheckForUpdate()
                If _appUpdate Is Nothing Then
                    _appUpdate = New Update
                ElseIf _appUpdate.IsGetUpdateInfoRunning Then
                    Return
                End If

                Dim nextUpdateCheck As Date = My.Settings.CheckForUpdatesLastCheck.Add(TimeSpan.FromDays(My.Settings.CheckForUpdatesFrequencyDays))
                If Not My.Settings.UpdatePending And Date.UtcNow < nextUpdateCheck Then Return

                AddHandler _appUpdate.GetUpdateInfoCompletedEvent, AddressOf GetUpdateInfoCompleted
                _appUpdate.GetUpdateInfoAsync()
            End Sub

            Private Shared Sub GetUpdateInfoCompleted(ByVal sender As Object, ByVal e As AsyncCompletedEventArgs)
                If MainForm.InvokeRequired Then
                    MainForm.Invoke(New AsyncCompletedEventHandler(AddressOf GetUpdateInfoCompleted), New Object() {sender, e})
                    Return
                End If

                Try
                    RemoveHandler _appUpdate.GetUpdateInfoCompletedEvent, AddressOf GetUpdateInfoCompleted

                    If e.Cancelled Then Return
                    If e.Error IsNot Nothing Then Throw e.Error

                    If _appUpdate.IsUpdateAvailable() Then Windows.Show(UI.Window.Type.Update)
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage("GetUpdateInfoCompleted() failed.", ex, MessageClass.ErrorMsg, True)
                End Try
            End Sub

            Public Shared Sub CheckForAnnouncement()
                If _appUpdate Is Nothing Then
                    _appUpdate = New Update
                ElseIf _appUpdate.IsGetAnnouncementInfoRunning Then
                    Return
                End If

                AddHandler _appUpdate.GetAnnouncementInfoCompletedEvent, AddressOf GetAnnouncementInfoCompleted
                _appUpdate.GetAnnouncementInfoAsync()
            End Sub

            Private Shared Sub GetAnnouncementInfoCompleted(ByVal sender As Object, ByVal e As AsyncCompletedEventArgs)
                If MainForm.InvokeRequired Then
                    MainForm.Invoke(New AsyncCompletedEventHandler(AddressOf GetAnnouncementInfoCompleted), New Object() {sender, e})
                    Return
                End If

                Try
                    RemoveHandler _appUpdate.GetAnnouncementInfoCompletedEvent, AddressOf GetAnnouncementInfoCompleted

                    If e.Cancelled Then Return
                    If e.Error IsNot Nothing Then Throw e.Error

                    If _appUpdate.IsAnnouncementAvailable() Then Windows.Show(UI.Window.Type.Announcement)
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage("GetAnnouncementInfoCompleted() failed.", ex, MessageClass.ErrorMsg, True)
                End Try
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
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strCommandLineArgsCouldNotBeParsed & vbNewLine & ex.Message)
                End Try
            End Sub

            Public Shared Sub CreateSQLUpdateHandlerAndStartTimer()
                If My.Settings.UseSQLServer = True Then
                    AddHandler Tools.Misc.SQLUpdateCheckFinished, AddressOf SQLUpdateCheckFinished
                    TimerSqlWatcher = New Timers.Timer(3000)
                    TimerSqlWatcher.Start()
                End If
            End Sub

            Public Shared Sub DestroySQLUpdateHandlerAndStopTimer()
                Try
                    LastSqlUpdate = Nothing
                    RemoveHandler Tools.Misc.SQLUpdateCheckFinished, AddressOf SQLUpdateCheckFinished
                    If TimerSqlWatcher IsNot Nothing Then
                        TimerSqlWatcher.Stop()
                        TimerSqlWatcher.Close()
                    End If
                Catch ex As Exception
                End Try
            End Sub
        End Class

        Public Class Shutdown
            Public Shared Sub Quit(Optional ByVal updateFilePath As String = Nothing)
                _updateFilePath = updateFilePath
                frmMain.Close()
            End Sub

            Public Shared Sub Cleanup()
                Try
                    Putty.Sessions.StopWatcher()

                    If NotificationAreaIcon IsNot Nothing Then
                        If NotificationAreaIcon.Disposed = False Then
                            NotificationAreaIcon.Dispose()
                        End If
                    End If

                    If My.Settings.SaveConsOnExit Then SaveConnections()

                    Settings.Save.Save()

                    IeBrowserEmulation.Unregister()
                Catch ex As Exception
                    MessageCollector.AddMessage(MessageClass.ErrorMsg, My.Language.strSettingsCouldNotBeSavedOrTrayDispose & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Shared Sub StartUpdate()
                If Not UpdatePending() Then Return
                Try
                    Process.Start(_updateFilePath)
                Catch ex As Exception
                    MessageCollector.AddMessage(MessageClass.ErrorMsg, "The update could not be started." & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Shared _updateFilePath As String = Nothing

            Public Shared ReadOnly Property UpdatePending() As Boolean
                Get
                    Return Not String.IsNullOrEmpty(_updateFilePath)
                End Get
            End Property
        End Class
#End Region

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
                My.Settings.ConDefaultLoadBalanceInfo = .LoadBalanceInfo
                My.Settings.ConDefaultUseConsoleSession = .UseConsoleSession
                My.Settings.ConDefaultUseCredSsp = .UseCredSsp
                My.Settings.ConDefaultRenderingEngine = .RenderingEngine.ToString
                My.Settings.ConDefaultResolution = .Resolution.ToString
                My.Settings.ConDefaultAutomaticResize = .AutomaticResize
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
                My.Settings.InhDefaultUseCredSsp = .UseCredSsp
                My.Settings.InhDefaultRenderingEngine = .RenderingEngine
                My.Settings.InhDefaultICAEncryptionStrength = .ICAEncryption
                My.Settings.InhDefaultRDPAuthenticationLevel = .RDPAuthenticationLevel
                My.Settings.InhDefaultLoadBalanceInfo = .LoadBalanceInfo
                My.Settings.InhDefaultResolution = .Resolution
                My.Settings.InhDefaultAutomaticResize = .AutomaticResize
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
        Public Shared Function AddPanel(Optional ByVal title As String = "", Optional ByVal noTabber As Boolean = False) As System.Windows.Forms.Form
            Try
                If title = "" Then
                    title = My.Language.strNewPanel
                End If

                Dim pnlcForm As New DockContent
                Dim cForm As New UI.Window.Connection(pnlcForm)
                pnlcForm = cForm

                'create context menu
                Dim cMen As New ContextMenuStrip

                'create rename item
                Dim cMenRen As New ToolStripMenuItem
                cMenRen.Text = My.Language.strRename
                cMenRen.Image = My.Resources.Rename
                cMenRen.Tag = pnlcForm
                AddHandler cMenRen.Click, AddressOf cMenConnectionPanelRename_Click

                Dim cMenScreens As New ToolStripMenuItem
                cMenScreens.Text = My.Language.strSendTo
                cMenScreens.Image = My.Resources.Monitor
                cMenScreens.Tag = pnlcForm
                cMenScreens.DropDownItems.Add("Dummy")
                AddHandler cMenScreens.DropDownOpening, AddressOf cMenConnectionPanelScreens_DropDownOpening

                cMen.Items.AddRange(New ToolStripMenuItem() {cMenRen, cMenScreens})

                pnlcForm.TabPageContextMenuStrip = cMen

                TryCast(cForm, UI.Window.Connection).SetFormText(title.Replace("&", "&&"))

                pnlcForm.Show(frmMain.pnlDock, DockState.Document)

                If noTabber Then
                    TryCast(cForm, UI.Window.Connection).TabController.Dispose()
                Else
                    WindowList.Add(cForm)
                End If

                Return cForm
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't add panel" & vbNewLine & ex.Message)
                Return Nothing
            End Try
        End Function

        Private Shared Sub cMenConnectionPanelRename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Try
                Dim conW As UI.Window.Connection
                conW = sender.Tag

                Dim nTitle As String = InputBox(My.Language.strNewTitle & ":", , sender.Tag.Text.Replace("&&", "&"))

                If nTitle <> "" Then
                    conW.SetFormText(nTitle.Replace("&", "&&"))
                End If
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't rename panel" & vbNewLine & ex.Message)
            End Try
        End Sub

        Private Shared Sub cMenConnectionPanelScreens_DropDownOpening(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Try
                Dim cMenScreens As ToolStripMenuItem = sender
                cMenScreens.DropDownItems.Clear()

                For i As Integer = 0 To Screen.AllScreens.Length - 1
                    Dim cMenScreen As New ToolStripMenuItem(My.Language.strScreen & " " & i + 1)
                    cMenScreen.Tag = New ArrayList
                    cMenScreen.Image = My.Resources.Monitor_GoTo
                    TryCast(cMenScreen.Tag, ArrayList).Add(Screen.AllScreens(i))
                    TryCast(cMenScreen.Tag, ArrayList).Add(cMenScreens.Tag)
                    AddHandler cMenScreen.Click, AddressOf cMenConnectionPanelScreen_Click

                    cMenScreens.DropDownItems.Add(cMenScreen)
                Next
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't enumerate screens" & vbNewLine & ex.Message)
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
                ConnectionList = New Connection.List
                ContainerList = New Container.List

                Dim connectionsLoad As New Connections.Load

                If filename = GetDefaultStartupConnectionFileName() Then
                    My.Settings.LoadConsFromCustomLocation = False
                Else
                    My.Settings.LoadConsFromCustomLocation = True
                    My.Settings.CustomConsPath = filename
                End If

                Directory.CreateDirectory(Path.GetDirectoryName(filename))

                ' Use File.Open with FileMode.CreateNew so that we don't overwrite an existing file
                Using fileStream As FileStream = File.Open(filename, FileMode.CreateNew, FileAccess.Write, FileShare.None)
                    Using xmlTextWriter As New XmlTextWriter(fileStream, System.Text.Encoding.UTF8)
                        With xmlTextWriter
                            .Formatting = Formatting.Indented
                            .Indentation = 4

                            .WriteStartDocument()

                            .WriteStartElement("Connections") ' Do not localize
                            .WriteAttributeString("Name", My.Language.strConnections)
                            .WriteAttributeString("Export", "", "False")
                            .WriteAttributeString("Protected", "", "GiUis20DIbnYzWPcdaQKfjE2H5jh//L5v4RGrJMGNXuIq2CttB/d/BxaBP2LwRhY")
                            .WriteAttributeString("ConfVersion", "", "2.5")

                            .WriteEndElement()
                            .WriteEndDocument()

                            .Close()
                        End With
                    End Using
                End Using

                connectionsLoad.ConnectionList = ConnectionList
                connectionsLoad.ContainerList = ContainerList

                Tree.Node.ResetTree()

                connectionsLoad.RootTreeNode = Windows.treeForm.tvConnections.Nodes(0)

                ' Load config
                connectionsLoad.ConnectionFileName = filename
                connectionsLoad.Load(False)

                Windows.treeForm.tvConnections.SelectedNode = connectionsLoad.RootTreeNode
            Catch ex As Exception
                MessageCollector.AddExceptionMessage(My.Language.strCouldNotCreateNewConnectionsFile, ex, MessageClass.ErrorMsg)
            End Try
        End Sub

        Private Shared Sub LoadConnectionsBG(Optional ByVal WithDialog As Boolean = False, Optional ByVal Update As Boolean = False)
            _withDialog = False
            _loadUpdate = True

            Dim t As New Thread(AddressOf LoadConnectionsBGd)
            t.SetApartmentState(Threading.ApartmentState.STA)
            t.Start()
        End Sub

        Private Shared _withDialog As Boolean = False
        Private Shared _loadUpdate As Boolean = False
        Private Shared Sub LoadConnectionsBGd()
            LoadConnections(_withDialog, _loadUpdate)
        End Sub

        Public Shared Sub LoadConnections(Optional ByVal withDialog As Boolean = False, Optional ByVal update As Boolean = False)
            Dim connectionsLoad As New Connections.Load

            Try
                Dim tmrWasEnabled As Boolean
                If TimerSqlWatcher IsNot Nothing Then
                    tmrWasEnabled = TimerSqlWatcher.Enabled

                    If TimerSqlWatcher.Enabled = True Then
                        TimerSqlWatcher.Stop()
                    End If
                End If

                If ConnectionList IsNot Nothing And ContainerList IsNot Nothing Then
                    PreviousConnectionList = ConnectionList.Copy
                    PreviousContainerList = ContainerList.Copy
                End If

                ConnectionList = New Connection.List
                ContainerList = New Container.List

                If Not My.Settings.UseSQLServer Then
                    If withDialog Then
                        Dim loadDialog As OpenFileDialog = Tools.Controls.ConnectionsLoadDialog

                        If loadDialog.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                            connectionsLoad.ConnectionFileName = loadDialog.FileName
                        Else
                            Exit Sub
                        End If
                    Else
                        connectionsLoad.ConnectionFileName = GetStartupConnectionFileName()
                    End If

                    CreateBackupFile(connectionsLoad.ConnectionFileName)
                End If

                connectionsLoad.ConnectionList = ConnectionList
                connectionsLoad.ContainerList = ContainerList

                If PreviousConnectionList IsNot Nothing And PreviousContainerList IsNot Nothing Then
                    connectionsLoad.PreviousConnectionList = PreviousConnectionList
                    connectionsLoad.PreviousContainerList = PreviousContainerList
                End If

                If update = True Then
                    connectionsLoad.PreviousSelected = LastSelected
                End If

                Tree.Node.ResetTree()

                connectionsLoad.RootTreeNode = Windows.treeForm.tvConnections.Nodes(0)

                connectionsLoad.UseSQL = My.Settings.UseSQLServer
                connectionsLoad.SQLHost = My.Settings.SQLHost
                connectionsLoad.SQLDatabaseName = My.Settings.SQLDatabaseName
                connectionsLoad.SQLUsername = My.Settings.SQLUser
                connectionsLoad.SQLPassword = Security.Crypt.Decrypt(My.Settings.SQLPass, Info.General.EncryptionKey)
                connectionsLoad.SQLUpdate = update

                connectionsLoad.Load(False)

                If My.Settings.UseSQLServer = True Then
                    LastSqlUpdate = Now
                Else
                    If connectionsLoad.ConnectionFileName = GetDefaultStartupConnectionFileName() Then
                        My.Settings.LoadConsFromCustomLocation = False
                    Else
                        My.Settings.LoadConsFromCustomLocation = True
                        My.Settings.CustomConsPath = connectionsLoad.ConnectionFileName
                    End If
                End If

                If tmrWasEnabled And TimerSqlWatcher IsNot Nothing Then
                    TimerSqlWatcher.Start()
                End If
            Catch ex As Exception
                If My.Settings.UseSQLServer Then
                    MessageCollector.AddExceptionMessage(My.Language.strLoadFromSqlFailed, ex)
                    Dim commandButtons As String = String.Join("|", {My.Language.strCommandTryAgain, My.Language.strCommandOpenConnectionFile, String.Format(My.Language.strCommandExitProgram, Application.ProductName)})
                    cTaskDialog.ShowCommandBox(Application.ProductName, My.Language.strLoadFromSqlFailed, My.Language.strLoadFromSqlFailedContent, Misc.GetExceptionMessageRecursive(ex), "", "", commandButtons, False, eSysIcons.Error, Nothing)
                    Select Case cTaskDialog.CommandButtonResult
                        Case 0
                            LoadConnections(withDialog, update)
                            Return
                        Case 1
                            My.Settings.UseSQLServer = False
                            LoadConnections(True, update)
                            Return
                        Case Else
                            Application.Exit()
                            Return
                    End Select
                Else
                    If TypeOf ex Is FileNotFoundException And Not withDialog Then
                        MessageCollector.AddExceptionMessage(String.Format(My.Language.strConnectionsFileCouldNotBeLoadedNew, connectionsLoad.ConnectionFileName), ex, MessageClass.InformationMsg)
                        NewConnections(connectionsLoad.ConnectionFileName)
                        Return
                    End If

                    MessageCollector.AddExceptionMessage(String.Format(My.Language.strConnectionsFileCouldNotBeLoaded, connectionsLoad.ConnectionFileName), ex)
                    If Not connectionsLoad.ConnectionFileName = GetStartupConnectionFileName() Then
                        LoadConnections(withDialog, update)
                        Return
                    Else
                        MsgBox(String.Format(My.Language.strErrorStartupConnectionFileLoad, vbNewLine, Application.ProductName, GetStartupConnectionFileName(), Misc.GetExceptionMessageRecursive(ex)), MsgBoxStyle.OkOnly + MsgBoxStyle.Critical)
                        Application.Exit()
                        Return
                    End If
                End If
            End Try
        End Sub

        Protected Shared Sub CreateBackupFile(ByVal fileName As String)
            ' This intentionally doesn't prune any existing backup files. We just assume the user doesn't want any new ones created.
            If My.Settings.BackupFileKeepCount = 0 Then Return

            Try
                Dim backupFileName As String = String.Format(My.Settings.BackupFileNameFormat, fileName, DateTime.UtcNow)
                File.Copy(fileName, backupFileName)
                PruneBackupFiles(fileName)
            Catch ex As Exception
                MessageCollector.AddExceptionMessage(My.Language.strConnectionsFileBackupFailed, ex, MessageClass.WarningMsg)
                Throw
            End Try
        End Sub

        Protected Shared Sub PruneBackupFiles(ByVal baseName As String)
            Dim fileName As String = Path.GetFileName(baseName)
            Dim directoryName As String = Path.GetDirectoryName(baseName)

            If String.IsNullOrEmpty(fileName) Or String.IsNullOrEmpty(directoryName) Then Return

            Dim searchPattern As String = String.Format(My.Settings.BackupFileNameFormat, fileName, "*")
            Dim files As String() = Directory.GetFiles(directoryName, searchPattern)

            If files.Length <= My.Settings.BackupFileKeepCount Then Return

            Array.Sort(files)
            Array.Resize(files, files.Length - My.Settings.BackupFileKeepCount)

            For Each file As String In files
                IO.File.Delete(file)
            Next
        End Sub

        Public Shared Function GetDefaultStartupConnectionFileName() As String
            Dim newPath As String = App.Info.Connections.DefaultConnectionsPath & "\" & Info.Connections.DefaultConnectionsFile
#If Not PORTABLE Then
            Dim oldPath As String = GetFolderPath(SpecialFolder.LocalApplicationData) & "\" & My.Application.Info.ProductName & "\" & Info.Connections.DefaultConnectionsFile
            If File.Exists(oldPath) Then
                Return oldPath
            End If
#End If
            Return newPath
        End Function

        Public Shared Function GetStartupConnectionFileName() As String
            If My.Settings.LoadConsFromCustomLocation = False Then
                Return GetDefaultStartupConnectionFileName()
            Else
                Return My.Settings.CustomConsPath
            End If
        End Function

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
                        ContainerList.Add(nContI)

                        Dim conL As New Config.Connections.Load
                        conL.ConnectionFileName = lD.FileNames(i)
                        conL.RootTreeNode = nNode
                        conL.ConnectionList = App.Runtime.ConnectionList
                        conL.ContainerList = App.Runtime.ContainerList

                        conL.Load(True)

                        Windows.treeForm.tvConnections.SelectedNode.Nodes.Add(nNode)
                    Next
                End If
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strConnectionsFileCouldNotBeImported & vbNewLine & ex.Message)
            End Try
        End Sub

        Public Shared Sub ImportConnectionsRdpFile()
            Try
                Dim openFileDialog As OpenFileDialog = Tools.Controls.ImportConnectionsRdpFileDialog
                If Not openFileDialog.ShowDialog = DialogResult.OK Then Return

                For Each fileName As String In openFileDialog.FileNames
                    Dim lines As String() = File.ReadAllLines(fileName)

                    Dim treeNode As TreeNode = Tree.Node.AddNode(Tree.Node.Type.Connection, Path.GetFileNameWithoutExtension(fileName))

                    Dim connectionInfo As New Connection.Info()
                    connectionInfo.Inherit = New Connection.Info.Inheritance(connectionInfo)

                    connectionInfo.Name = treeNode.Text

                    For Each line As String In lines
                        Dim parts() As String = line.Split(New Char() {":"}, 3)
                        If parts.Length < 3 Then Continue For

                        Dim key As String = parts(0)
                        Dim value As String = parts(2)

                        Select Case LCase(key)
                            Case "full address"
                                Dim uri As New Uri("dummyscheme" + uri.SchemeDelimiter + value)
                                If Not String.IsNullOrEmpty(uri.Host) Then connectionInfo.Hostname = uri.Host
                                If Not uri.Port = -1 Then connectionInfo.Port = uri.Port
                            Case "server port"
                                connectionInfo.Port = value
                            Case "username"
                                connectionInfo.Username = value
                            Case "domain"
                                connectionInfo.Domain = value
                            Case "session bpp"
                                Select Case value
                                    Case 8
                                        connectionInfo.Colors = Protocol.RDP.RDPColors.Colors256
                                    Case 15
                                        connectionInfo.Colors = Protocol.RDP.RDPColors.Colors15Bit
                                    Case 16
                                        connectionInfo.Colors = Protocol.RDP.RDPColors.Colors16Bit
                                    Case 24
                                        connectionInfo.Colors = Protocol.RDP.RDPColors.Colors24Bit
                                    Case 32
                                        connectionInfo.Colors = Protocol.RDP.RDPColors.Colors32Bit
                                End Select
                            Case "bitmapcachepersistenable"
                                If value = 1 Then
                                    connectionInfo.CacheBitmaps = True
                                Else
                                    connectionInfo.CacheBitmaps = False
                                End If
                            Case "screen mode id"
                                If value = 2 Then
                                    connectionInfo.Resolution = Protocol.RDP.RDPResolutions.Fullscreen
                                Else
                                    connectionInfo.Resolution = Protocol.RDP.RDPResolutions.FitToWindow
                                End If
                            Case "connect to console"
                                If value = 1 Then
                                    connectionInfo.UseConsoleSession = True
                                End If
                            Case "disable wallpaper"
                                If value = 1 Then
                                    connectionInfo.DisplayWallpaper = True
                                Else
                                    connectionInfo.DisplayWallpaper = False
                                End If
                            Case "disable themes"
                                If value = 1 Then
                                    connectionInfo.DisplayThemes = True
                                Else
                                    connectionInfo.DisplayThemes = False
                                End If
                            Case "allow font smoothing"
                                If value = 1 Then
                                    connectionInfo.EnableFontSmoothing = True
                                Else
                                    connectionInfo.EnableFontSmoothing = False
                                End If
                            Case "allow desktop composition"
                                If value = 1 Then
                                    connectionInfo.EnableDesktopComposition = True
                                Else
                                    connectionInfo.EnableDesktopComposition = False
                                End If
                            Case "redirectsmartcards"
                                If value = 1 Then
                                    connectionInfo.RedirectSmartCards = True
                                Else
                                    connectionInfo.RedirectSmartCards = False
                                End If
                            Case "redirectdrives"
                                If value = 1 Then
                                    connectionInfo.RedirectDiskDrives = True
                                Else
                                    connectionInfo.RedirectDiskDrives = False
                                End If
                            Case "redirectcomports"
                                If value = 1 Then
                                    connectionInfo.RedirectPorts = True
                                Else
                                    connectionInfo.RedirectPorts = False
                                End If
                            Case "redirectprinters"
                                If value = 1 Then
                                    connectionInfo.RedirectPrinters = True
                                Else
                                    connectionInfo.RedirectPrinters = False
                                End If
                            Case "audiomode"
                                Select Case value
                                    Case 0
                                        connectionInfo.RedirectSound = Protocol.RDP.RDPSounds.BringToThisComputer
                                    Case 1
                                        connectionInfo.RedirectSound = Protocol.RDP.RDPSounds.LeaveAtRemoteComputer
                                    Case 2
                                        connectionInfo.RedirectSound = Protocol.RDP.RDPSounds.DoNotPlay
                                End Select
                        End Select
                    Next

                    treeNode.Tag = connectionInfo
                    Windows.treeForm.tvConnections.SelectedNode.Nodes.Add(treeNode)

                    If Tree.Node.GetNodeType(treeNode.Parent) = Tree.Node.Type.Container Then
                        connectionInfo.Parent = treeNode.Parent.Tag
                    End If

                    ConnectionList.Add(connectionInfo)
                Next
            Catch ex As Exception
                MessageCollector.AddExceptionMessage(My.Language.strRdpFileCouldNotBeImported, ex)
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

                    ConnectionList.Add(nConI)
                End If
            Next
        End Sub

        Public Shared Sub ImportConnectionsFromCSV()

        End Sub

        Public Shared Sub SaveConnectionsBG()
            _saveUpdate = True

            Dim t As New Thread(AddressOf SaveConnectionsBGd)
            t.SetApartmentState(Threading.ApartmentState.STA)
            t.Start()
        End Sub

        Private Shared _saveUpdate As Boolean = False
        Private Shared _saveLock As Object = New Object
        Private Shared Sub SaveConnectionsBGd()
            Monitor.Enter(_saveLock)
            SaveConnections(_saveUpdate)
            Monitor.Exit(_saveLock)
        End Sub

        Public Shared Sub SaveConnections(Optional ByVal Update As Boolean = False)
            If Not IsConnectionsFileLoaded Then Exit Sub

            Dim previousTimerState As Boolean = False

            Try
                If Update = True And My.Settings.UseSQLServer = False Then
                    Exit Sub
                End If

                If TimerSqlWatcher IsNot Nothing Then
                    previousTimerState = TimerSqlWatcher.Enabled
                    TimerSqlWatcher.Enabled = False
                End If

                Dim conS As New Config.Connections.Save

                If Not My.Settings.UseSQLServer Then
                    conS.ConnectionFileName = GetStartupConnectionFileName()
                End If

                conS.ConnectionList = ConnectionList
                conS.ContainerList = ContainerList
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
                    LastSqlUpdate = Now
                End If
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strConnectionsFileCouldNotBeSaved & vbNewLine & ex.Message)
            Finally
                If TimerSqlWatcher IsNot Nothing Then
                    TimerSqlWatcher.Enabled = previousTimerState
                End If
            End Try
        End Sub

        Public Shared Sub SaveConnectionsAs(Optional ByVal rootNode As TreeNode = Nothing, Optional ByVal saveSecurity As Security.Save = Nothing)
            Dim connectionsSave As New Connections.Save
            Dim previousTimerState As Boolean = False

            Try
                If TimerSqlWatcher IsNot Nothing Then
                    previousTimerState = TimerSqlWatcher.Enabled
                    TimerSqlWatcher.Enabled = False
                End If

                Dim export As Boolean = False
                Dim saveAsDialog As SaveFileDialog
                If rootNode Is Nothing Then
                    rootNode = Windows.treeForm.tvConnections.Nodes(0)
                    saveAsDialog = Tools.Controls.ConnectionsSaveAsDialog
                Else
                    export = True
                    saveAsDialog = Tools.Controls.ConnectionsExportDialog
                End If

                If Not saveAsDialog.ShowDialog() = DialogResult.OK Then Return

                connectionsSave.ConnectionFileName = saveAsDialog.FileName

                If export Then
                    Select Case saveAsDialog.FilterIndex
                        Case 1
                            connectionsSave.SaveFormat = Connections.Save.Format.mRXML
                        Case 2
                            connectionsSave.SaveFormat = Connections.Save.Format.mRCSV
                        Case 3
                            connectionsSave.SaveFormat = Connections.Save.Format.vRDCSV
                    End Select
                Else
                    connectionsSave.SaveFormat = Connections.Save.Format.mRXML

                    If connectionsSave.ConnectionFileName = GetDefaultStartupConnectionFileName() Then
                        My.Settings.LoadConsFromCustomLocation = False
                    Else
                        My.Settings.LoadConsFromCustomLocation = True
                        My.Settings.CustomConsPath = connectionsSave.ConnectionFileName
                    End If
                End If

                connectionsSave.ConnectionList = ConnectionList
                connectionsSave.ContainerList = ContainerList
                connectionsSave.RootTreeNode = rootNode

                connectionsSave.Export = export

                If saveSecurity Is Nothing Then saveSecurity = New Security.Save
                connectionsSave.SaveSecurity = saveSecurity

                connectionsSave.Save()
            Catch ex As Exception
                MessageCollector.AddExceptionMessage(String.Format(My.Language.strConnectionsFileCouldNotSaveAs, connectionsSave.ConnectionFileName), ex)
            Finally
                If TimerSqlWatcher IsNot Nothing Then
                    TimerSqlWatcher.Enabled = previousTimerState
                End If
            End Try
        End Sub
#End Region

#Region "Opening Connection"
        Public Shared Function CreateQuickConnect(ByVal connectionString As String, ByVal protocol As Protocol.Protocols) As Connection.Info
            Try
                Dim uri As New Uri("dummyscheme" & uri.SchemeDelimiter & connectionString)

                If String.IsNullOrEmpty(uri.Host) Then Return Nothing

                Dim newConnectionInfo As New Connection.Info

                If My.Settings.IdentifyQuickConnectTabs Then
                    newConnectionInfo.Name = String.Format(My.Language.strQuick, uri.Host)
                Else
                    newConnectionInfo.Name = uri.Host
                End If

                newConnectionInfo.Protocol = protocol
                newConnectionInfo.Hostname = uri.Host
                If uri.Port = -1 Then
                    newConnectionInfo.SetDefaultPort()
                Else
                    newConnectionInfo.Port = uri.Port
                End If
                newConnectionInfo.IsQuickConnect = True

                Return newConnectionInfo
            Catch ex As Exception
                MessageCollector.AddExceptionMessage(My.Language.strQuickConnectFailed, ex, MessageClass.ErrorMsg)
                Return Nothing
            End Try
        End Function

        Public Shared Sub OpenConnection()
            Try
                OpenConnection(Connection.Info.Force.None)
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strConnectionOpenFailed & vbNewLine & ex.Message)
            End Try
        End Sub

        Public Shared Sub OpenConnection(ByVal Force As mRemoteNG.Connection.Info.Force)
            Try
                If Windows.treeForm.tvConnections.SelectedNode.Tag Is Nothing Then
                    Exit Sub
                End If

                If Tree.Node.GetNodeType(Tree.Node.SelectedNode) = Tree.Node.Type.Connection Or _
                   Tree.Node.GetNodeType(Tree.Node.SelectedNode) = Tree.Node.Type.PuttySession Then
                    OpenConnection(Windows.treeForm.tvConnections.SelectedNode.Tag, Force)
                ElseIf Tree.Node.GetNodeType(Tree.Node.SelectedNode) = Tree.Node.Type.Container Then
                    For Each tNode As TreeNode In Tree.Node.SelectedNode.Nodes
                        If Tree.Node.GetNodeType(tNode) = Tree.Node.Type.Connection Or _
                           Tree.Node.GetNodeType(Tree.Node.SelectedNode) = Tree.Node.Type.PuttySession Then
                            If tNode.Tag IsNot Nothing Then
                                OpenConnection(tNode.Tag, Force)
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strConnectionOpenFailed & vbNewLine & ex.Message)
            End Try
        End Sub

        Public Shared Sub OpenConnection(ByVal ConnectionInfo As mRemoteNG.Connection.Info)
            Try
                OpenConnection(ConnectionInfo, Connection.Info.Force.None)
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strConnectionOpenFailed & vbNewLine & ex.Message)
            End Try
        End Sub

        Public Shared Sub OpenConnection(ByVal ConnectionInfo As mRemoteNG.Connection.Info, ByVal ConnectionForm As System.Windows.Forms.Form)
            Try
                OpenConnectionFinal(ConnectionInfo, Connection.Info.Force.None, ConnectionForm)
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strConnectionOpenFailed & vbNewLine & ex.Message)
            End Try
        End Sub

        Public Shared Sub OpenConnection(ByVal ConnectionInfo As mRemoteNG.Connection.Info, ByVal ConnectionForm As System.Windows.Forms.Form, ByVal Force As Connection.Info.Force)
            Try
                OpenConnectionFinal(ConnectionInfo, Force, ConnectionForm)
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strConnectionOpenFailed & vbNewLine & ex.Message)
            End Try
        End Sub

        Public Shared Sub OpenConnection(ByVal ConnectionInfo As mRemoteNG.Connection.Info, ByVal Force As mRemoteNG.Connection.Info.Force)
            Try
                OpenConnectionFinal(ConnectionInfo, Force, Nothing)
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strConnectionOpenFailed & vbNewLine & ex.Message)
            End Try
        End Sub

        Private Shared Sub OpenConnectionFinal(ByVal newConnectionInfo As mRemoteNG.Connection.Info, ByVal Force As mRemoteNG.Connection.Info.Force, ByVal ConForm As System.Windows.Forms.Form)
            Try
                If newConnectionInfo.Hostname = "" And newConnectionInfo.Protocol <> Connection.Protocol.Protocols.IntApp Then
                    MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, My.Language.strConnectionOpenFailedNoHostname)
                    Exit Sub
                End If

                If newConnectionInfo.PreExtApp <> "" Then
                    Dim extA As Tools.ExternalTool = App.Runtime.GetExtAppByName(newConnectionInfo.PreExtApp)
                    If extA IsNot Nothing Then
                        extA.Start(newConnectionInfo)
                    End If
                End If

                If (Force And Connection.Info.Force.DoNotJump) <> Connection.Info.Force.DoNotJump Then
                    If SwitchToOpenConnection(newConnectionInfo) Then
                        Exit Sub
                    End If
                End If

                Dim newProtocol As Protocol.Base
                ' Create connection based on protocol type
                Select Case newConnectionInfo.Protocol
                    Case Protocol.Protocols.RDP
                        newProtocol = New Protocol.RDP
                    Case Protocol.Protocols.VNC
                        newProtocol = New Protocol.VNC
                    Case Protocol.Protocols.SSH1
                        newProtocol = New Protocol.SSH1
                    Case Protocol.Protocols.SSH2
                        newProtocol = New Protocol.SSH2
                    Case Protocol.Protocols.Telnet
                        newProtocol = New Protocol.Telnet
                    Case Protocol.Protocols.Rlogin
                        newProtocol = New Protocol.Rlogin
                    Case Protocol.Protocols.RAW
                        newProtocol = New Protocol.RAW
                    Case Protocol.Protocols.HTTP
                        newProtocol = New Protocol.HTTP(newConnectionInfo.RenderingEngine)
                    Case Protocol.Protocols.HTTPS
                        newProtocol = New Protocol.HTTPS(newConnectionInfo.RenderingEngine)
                    Case Protocol.Protocols.ICA
                        newProtocol = New Protocol.ICA
                    Case Protocol.Protocols.IntApp
                        newProtocol = New Protocol.IntegratedProgram

                        If newConnectionInfo.ExtApp = "" Then
                            Throw New Exception(My.Language.strNoExtAppDefined)
                        End If
                    Case Else
                        Exit Sub
                End Select

                Dim cContainer As Control
                Dim cForm As System.Windows.Forms.Form

                Dim cPnl As String
                If newConnectionInfo.Panel = "" Or (Force And Connection.Info.Force.OverridePanel) = Connection.Info.Force.OverridePanel Or My.Settings.AlwaysShowPanelSelectionDlg Then
                    Dim frmPnl As New frmChoosePanel
                    If frmPnl.ShowDialog = DialogResult.OK Then
                        cPnl = frmPnl.Panel
                    Else
                        Exit Sub
                    End If
                Else
                    cPnl = newConnectionInfo.Panel
                End If

                If ConForm Is Nothing Then
                    cForm = WindowList.FromString(cPnl)
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

                cContainer = TryCast(cForm, UI.Window.Connection).AddConnectionTab(newConnectionInfo)

                If newConnectionInfo.Protocol = Connection.Protocol.Protocols.IntApp Then
                    If App.Runtime.GetExtAppByName(newConnectionInfo.ExtApp).Icon IsNot Nothing Then
                        TryCast(cContainer, Magic.Controls.TabPage).Icon = App.Runtime.GetExtAppByName(newConnectionInfo.ExtApp).Icon
                    End If
                End If

                AddHandler newProtocol.Closed, AddressOf TryCast(cForm, UI.Window.Connection).Prot_Event_Closed

                AddHandler newProtocol.Disconnected, AddressOf Prot_Event_Disconnected
                AddHandler newProtocol.Connected, AddressOf Prot_Event_Connected
                AddHandler newProtocol.Closed, AddressOf Prot_Event_Closed
                AddHandler newProtocol.ErrorOccured, AddressOf Prot_Event_ErrorOccured

                newProtocol.InterfaceControl = New Connection.InterfaceControl(cContainer, newProtocol, newConnectionInfo)

                newProtocol.Force = Force

                If newProtocol.SetProps() = False Then
                    newProtocol.Close()
                    Exit Sub
                End If

                If newProtocol.Connect() = False Then
                    newProtocol.Close()
                    Exit Sub
                End If

                newConnectionInfo.OpenConnections.Add(newProtocol)

                If newConnectionInfo.IsQuickConnect = False Then
                    If newConnectionInfo.Protocol <> Connection.Protocol.Protocols.IntApp Then
                        Tree.Node.SetNodeImage(newConnectionInfo.TreeNode, Images.Enums.TreeImage.ConnectionOpen)
                    Else
                        Dim extApp As Tools.ExternalTool = GetExtAppByName(newConnectionInfo.ExtApp)
                        If extApp IsNot Nothing Then
                            If extApp.TryIntegrate Then
                                If newConnectionInfo.TreeNode IsNot Nothing Then
                                    Tree.Node.SetNodeImage(newConnectionInfo.TreeNode, Images.Enums.TreeImage.ConnectionOpen)
                                End If
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strConnectionOpenFailed & vbNewLine & ex.Message)
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
                MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, String.Format(My.Language.strProtocolEventDisconnected, DisconnectedMessage), True)

                Dim Prot As Connection.Protocol.Base = sender
                If Prot.InterfaceControl.Info.Protocol = Connection.Protocol.Protocols.RDP Then
                    Dim Reason As String() = DisconnectedMessage.Split(vbCrLf)
                    Dim ReasonCode As String = Reason(0)
                    Dim ReasonDescription As String = Reason(1)
                    If ReasonCode > 3 Then
                        If ReasonDescription <> "" Then
                            MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, My.Language.strRdpDisconnected & vbNewLine & ReasonDescription & vbNewLine & String.Format(My.Language.strErrorCode, ReasonCode))
                        Else
                            MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, My.Language.strRdpDisconnected & vbNewLine & String.Format(My.Language.strErrorCode, ReasonCode))
                        End If
                    End If
                End If
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, String.Format(My.Language.strProtocolEventDisconnectFailed, ex.Message), True)
            End Try
        End Sub

        Public Shared Sub Prot_Event_Closed(ByVal sender As Object)
            Try
                Dim Prot As Connection.Protocol.Base = sender

                MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, My.Language.strConnenctionCloseEvent, True)

                MessageCollector.AddMessage(Messages.MessageClass.ReportMsg, String.Format(My.Language.strConnenctionClosedByUser, Prot.InterfaceControl.Info.Hostname, Prot.InterfaceControl.Info.Protocol.ToString, My.User.Name))

                Prot.InterfaceControl.Info.OpenConnections.Remove(Prot)

                If Prot.InterfaceControl.Info.OpenConnections.Count < 1 And Prot.InterfaceControl.Info.IsQuickConnect = False Then
                    Tree.Node.SetNodeImage(Prot.InterfaceControl.Info.TreeNode, Images.Enums.TreeImage.ConnectionClosed)
                End If

                If Prot.InterfaceControl.Info.PostExtApp <> "" Then
                    Dim extA As Tools.ExternalTool = App.Runtime.GetExtAppByName(Prot.InterfaceControl.Info.PostExtApp)
                    If extA IsNot Nothing Then
                        extA.Start(Prot.InterfaceControl.Info)
                    End If
                End If
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strConnenctionCloseEventFailed & vbNewLine & ex.Message, True)
            End Try
        End Sub

        Public Shared Sub Prot_Event_Connected(ByVal sender As Object)
            Dim prot As mRemoteNG.Connection.Protocol.Base = sender

            MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, My.Language.strConnectionEventConnected, True)
            MessageCollector.AddMessage(Messages.MessageClass.ReportMsg, String.Format(My.Language.strConnectionEventConnectedDetail, prot.InterfaceControl.Info.Hostname, prot.InterfaceControl.Info.Protocol.ToString, My.User.Name, prot.InterfaceControl.Info.Description, prot.InterfaceControl.Info.UserField))
        End Sub

        Public Shared Sub Prot_Event_ErrorOccured(ByVal sender As Object, ByVal ErrorMessage As String)
            Try
                MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, My.Language.strConnectionEventErrorOccured, True)

                Dim Prot As Connection.Protocol.Base = sender

                If Prot.InterfaceControl.Info.Protocol = Connection.Protocol.Protocols.RDP Then
                    If ErrorMessage > -1 Then
                        MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, String.Format(My.Language.strConnectionRdpErrorDetail, ErrorMessage, Connection.Protocol.RDP.FatalErrors.GetError(ErrorMessage)))
                    End If
                End If
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strConnectionEventConnectionFailed & vbNewLine & ex.Message, True)
            End Try
        End Sub
#End Region

#Region "External Apps"
        Public Shared Sub GetExtApps()
            Array.Clear(Tools.ExternalToolsTypeConverter.ExternalTools, 0, Tools.ExternalToolsTypeConverter.ExternalTools.Length)
            Array.Resize(Tools.ExternalToolsTypeConverter.ExternalTools, ExternalTools.Count + 1)

            Dim i As Integer = 0

            For Each extA As Tools.ExternalTool In ExternalTools
                Tools.ExternalToolsTypeConverter.ExternalTools(i) = extA.DisplayName

                i += 1
            Next

            Tools.ExternalToolsTypeConverter.ExternalTools(i) = ""
        End Sub

        Public Shared Function GetExtAppByName(ByVal Name As String) As Tools.ExternalTool
            For Each extA As Tools.ExternalTool In ExternalTools
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
            cI.IsQuickConnect = True

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
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strLogWriteToFileFailed)
            End Try
        End Sub

        Public Shared Function SaveReport() As Boolean
            Dim streamReader As StreamReader = Nothing
            Dim streamWriter As StreamWriter = Nothing
            Try
                streamReader = New StreamReader(My.Application.Info.DirectoryPath & "\Report.log")
                Dim text As String = streamReader.ReadToEnd
                streamReader.Close()

                streamWriter = New StreamWriter(App.Info.General.ReportingFilePath, True)
                streamWriter.Write(text)

                Return True
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strLogWriteToFileFinalLocationFailed & vbNewLine & ex.Message, True)
                Return False
            Finally
                If streamReader IsNot Nothing Then
                    streamReader.Close()
                    streamReader.Dispose()
                End If
                If streamWriter IsNot Nothing Then
                    streamWriter.Close()
                    streamWriter.Dispose()
                End If
            End Try
        End Function

        Public Shared Sub SetMainFormText(Optional ByVal ConnectionFileName As String = "")
            Try
                Dim txt As String = My.Application.Info.ProductName

                If ConnectionFileName <> "" And IsConnectionsFileLoaded = True Then
                    If My.Settings.ShowCompleteConsPathInTitle Then
                        txt &= " - " & ConnectionFileName
                    Else
                        txt &= " - " & ConnectionFileName.Substring(ConnectionFileName.LastIndexOf("\") + 1)
                    End If
                End If

                ChangeMainFormText(txt)
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strSettingMainFormTextFailed & vbNewLine & ex.Message, True)
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
                For i As Integer = 0 To WindowList.Count - 1
                    If TypeOf WindowList.Items(i) Is UI.Window.Connection Then
                        Dim conW As UI.Window.Connection = WindowList.Items(i)

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
        Private Shared Sub tmrSqlWatcher_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles _timerSqlWatcher.Elapsed
            Tools.Misc.IsSQLUpdateAvailableBG()
        End Sub

        Private Shared Sub SQLUpdateCheckFinished(ByVal UpdateAvailable As Boolean)
            If UpdateAvailable = True Then
                MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, My.Language.strSqlUpdateCheckUpdateAvailable, True)
                LoadConnectionsBG()
            End If
        End Sub
#End Region

    End Class
End Namespace