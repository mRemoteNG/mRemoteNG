Imports System.ComponentModel
Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Text
Imports Crownwood.Magic.Controls
Imports Microsoft.Win32
Imports mRemote3G.App
Imports mRemote3G.Config
Imports mRemote3G.Config.Putty
Imports mRemote3G.Config.Settings
Imports mRemote3G.Connection
Imports mRemote3G.Connection.Protocol
Imports mRemote3G.Controls
Imports mRemote3G.Messages
Imports mRemote3G.My
Imports mRemote3G.Themes
Imports mRemote3G.Tools
Imports mRemote3G.Tree
Imports mRemote3G.UI.Window
Imports PSTaskDialog
Imports WeifenLuo.WinFormsUI.Docking
Imports List = mRemote3G.UI.Window.List

Namespace Forms
    Public Class frmMain
        Private _previousWindowState As FormWindowState

        Public Property PreviousWindowState As FormWindowState
            Get
                Return _previousWindowState
            End Get
            Set
                _previousWindowState = Value
            End Set
        End Property

        Public Shared Event clipboardchange()
        Private fpChainedWindowHandle As IntPtr

#Region "Properties"

        Private _isClosing As Boolean = False

        Public ReadOnly Property IsClosing As Boolean
            Get
                Return _isClosing
            End Get
        End Property

        Private _usingSqlServer As Boolean = False

        Public Property UsingSqlServer As Boolean
            Get
                Return _usingSqlServer
            End Get
            Set
                If _usingSqlServer = Value Then Return
                _usingSqlServer = Value
                UpdateWindowTitle()
            End Set
        End Property

        Private _connectionsFileName As String = Nothing

        Public Property ConnectionsFileName As String
            Get
                Return _connectionsFileName
            End Get
            Set
                If _connectionsFileName = Value Then Return
                _connectionsFileName = Value
                UpdateWindowTitle()
            End Set
        End Property

        Private _showFullPathInTitle As Boolean = My.Settings.ShowCompleteConsPathInTitle

        Public Property ShowFullPathInTitle As Boolean
            Get
                Return _showFullPathInTitle
            End Get
            Set
                If _showFullPathInTitle = Value Then Return
                _showFullPathInTitle = Value
                UpdateWindowTitle()
            End Set
        End Property

        Private _selectedConnection As Info = Nothing

        Public Property SelectedConnection As Info
            Get
                Return _selectedConnection
            End Get
            Set
                If _selectedConnection Is Value Then Return
                _selectedConnection = Value
                UpdateWindowTitle()
            End Set
        End Property

#End Region

#Region "Startup & Shutdown"

        Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load
            Runtime.MainForm = Me

            Runtime.Startup.CheckCompatibility()

            Runtime.Startup.CreateLogger()

            ' Create gui config load and save objects
            Dim SettingsLoad As New Load(Me)

            ' Load GUI Configuration
            SettingsLoad.Load()

            Debug.Print("---------------------------" & vbNewLine & "[START] - " & Now)

            Runtime.Startup.ParseCommandLineArgs()

            ApplyLanguage()
            PopulateQuickConnectProtocolMenu()

            AddHandler ThemeManager.ThemeChanged, AddressOf ApplyThemes
            ApplyThemes()

            fpChainedWindowHandle = NativeMethods.SetClipboardViewer(Me.Handle)

            Runtime.MessageCollector = New Collector(Runtime.Windows.errorsForm)

            Runtime.WindowList = New List

            IeBrowserEmulation.Register()

            Runtime.Startup.GetConnectionIcons()
            Runtime.Windows.treePanel.Focus()

            Node.TreeView = Runtime.Windows.treeForm.tvConnections

            If My.Settings.FirstStart And
               Not My.Settings.LoadConsFromCustomLocation And
               Not File.Exists(Runtime.GetStartupConnectionFileName()) Then
                Runtime.NewConnections(Runtime.GetStartupConnectionFileName())
            End If

            'LoadCredentials()
            Runtime.LoadConnections()
            If Not Runtime.IsConnectionsFileLoaded Then
                System.Windows.Forms.Application.Exit()
                Return
            End If

            Sessions.StartWatcher()

            If My.Settings.StartupComponentsCheck Then
                Runtime.Windows.Show(Type.ComponentsCheck)
            End If

#If PORTABLE Then
        mMenInfoAnnouncements.Visible = False
        mMenToolsUpdate.Visible = False
        mMenInfoSep2.Visible = False
#End If

            Runtime.Startup.CreateSQLUpdateHandlerAndStartTimer()

            AddSysMenuItems()
            AddHandler SystemEvents.DisplaySettingsChanged, AddressOf DisplayChanged

            Me.Opacity = 1
        End Sub

        Private Sub ApplyLanguage()
            mMenFile.Text = Language.Language.strMenuFile
            mMenFileNew.Text = Language.Language.strMenuNewConnectionFile
            mMenFileNewConnection.Text = Language.Language.strNewConnection
            mMenFileNewFolder.Text = Language.Language.strNewFolder
            mMenFileLoad.Text = Language.Language.strMenuOpenConnectionFile
            mMenFileSave.Text = Language.Language.strMenuSaveConnectionFile
            mMenFileSaveAs.Text = Language.Language.strMenuSaveConnectionFileAs
            mMenFileImport.Text = Language.Language.strImportMenuItem
            mMenFileImportFromFile.Text = Language.Language.strImportFromFileMenuItem
            mMenFileImportFromActiveDirectory.Text = Language.Language.strImportAD
            mMenFileImportFromPortScan.Text = Language.Language.strImportPortScan
            mMenFileExport.Text = Language.Language.strExportToFileMenuItem
            mMenFileExit.Text = Language.Language.strMenuExit

            mMenView.Text = Language.Language.strMenuView
            mMenViewAddConnectionPanel.Text = Language.Language.strMenuAddConnectionPanel
            mMenViewConnectionPanels.Text = Language.Language.strMenuConnectionPanels
            mMenViewConnections.Text = Language.Language.strMenuConnections
            mMenViewConfig.Text = Language.Language.strMenuConfig
            mMenViewErrorsAndInfos.Text = Language.Language.strMenuNotifications
            mMenViewScreenshotManager.Text = Language.Language.strScreenshots
            mMenViewJumpTo.Text = Language.Language.strMenuJumpTo
            mMenViewJumpToConnectionsConfig.Text = Language.Language.strMenuConnectionsAndConfig
            mMenViewJumpToSessionsScreenshots.Text = Language.Language.strMenuSessionsAndScreenshots
            mMenViewJumpToErrorsInfos.Text = Language.Language.strMenuNotifications
            mMenViewResetLayout.Text = Language.Language.strMenuResetLayout
            mMenViewQuickConnectToolbar.Text = Language.Language.strMenuQuickConnectToolbar
            mMenViewExtAppsToolbar.Text = Language.Language.strMenuExternalToolsToolbar
            mMenViewFullscreen.Text = Language.Language.strMenuFullScreen

            mMenTools.Text = Language.Language.strMenuTools
            mMenToolsSSHTransfer.Text = Language.Language.strMenuSSHFileTransfer
            mMenToolsExternalApps.Text = Language.Language.strMenuExternalTools
            mMenToolsPortScan.Text = Language.Language.strMenuPortScan
            mMenToolsComponentsCheck.Text = Language.Language.strComponentsCheck
            mMenToolsUpdate.Text = Language.Language.strMenuCheckForUpdates
            mMenToolsOptions.Text = Language.Language.strMenuOptions

            mMenInfo.Text = Language.Language.strMenuHelp
            mMenInfoHelp.Text = Language.Language.strMenuHelpContents
            mMenInfoBugReport.Text = Language.Language.strMenuReportBug
            mMenInfoWebsite.Text = Language.Language.strMenuWebsite
            mMenInfoAbout.Text = Language.Language.strMenuAbout
            mMenInfoAnnouncements.Text = Language.Language.strMenuAnnouncements

            lblQuickConnect.Text = Language.Language.strLabelConnect
            btnQuickConnect.Text = Language.Language.strMenuConnect
            btnConnections.Text = Language.Language.strMenuConnections

            cMenToolbarShowText.Text = Language.Language.strMenuShowText

            ToolStripButton1.Text = Language.Language.strConnect
            ToolStripButton2.Text = Language.Language.strScreenshot
            ToolStripButton3.Text = Language.Language.strRefresh

            ToolStripSplitButton1.Text = Language.Language.strSpecialKeys
            ToolStripMenuItem1.Text = Language.Language.strKeysCtrlAltDel
            ToolStripMenuItem2.Text = Language.Language.strKeysCtrlEsc
        End Sub

        Public Sub ApplyThemes()
            With ThemeManager.ActiveTheme
                pnlDock.DockBackColor = .WindowBackgroundColor
                tsContainer.BackColor = .ToolbarBackgroundColor
                tsContainer.ForeColor = .ToolbarTextColor
                tsContainer.TopToolStripPanel.BackColor = .ToolbarBackgroundColor
                tsContainer.TopToolStripPanel.ForeColor = .ToolbarTextColor
                tsContainer.BottomToolStripPanel.BackColor = .ToolbarBackgroundColor
                tsContainer.BottomToolStripPanel.ForeColor = .ToolbarTextColor
                tsContainer.LeftToolStripPanel.BackColor = .ToolbarBackgroundColor
                tsContainer.LeftToolStripPanel.ForeColor = .ToolbarTextColor
                tsContainer.RightToolStripPanel.BackColor = .ToolbarBackgroundColor
                tsContainer.RightToolStripPanel.ForeColor = .ToolbarTextColor
                tsContainer.ContentPanel.BackColor = .ToolbarBackgroundColor
                tsContainer.ContentPanel.ForeColor = .ToolbarTextColor
                msMain.BackColor = .ToolbarBackgroundColor
                msMain.ForeColor = .ToolbarTextColor
                ApplyMenuColors(msMain.Items)
                tsExternalTools.BackColor = .ToolbarBackgroundColor
                tsExternalTools.ForeColor = .ToolbarTextColor
                tsQuickConnect.BackColor = .ToolbarBackgroundColor
                tsQuickConnect.ForeColor = .ToolbarTextColor
            End With
        End Sub

        Private Shared Sub ApplyMenuColors(itemCollection As ToolStripItemCollection)
            With ThemeManager.ActiveTheme
                Dim menuItem As ToolStripMenuItem
                For Each item As ToolStripItem In itemCollection
                    item.BackColor = .MenuBackgroundColor
                    item.ForeColor = .MenuTextColor

                    menuItem = TryCast(item, ToolStripMenuItem)
                    If menuItem IsNot Nothing Then
                        ApplyMenuColors(menuItem.DropDownItems)
                    End If
                Next
            End With
        End Sub

        Private Sub frmMain_Shown(sender As Object, e As EventArgs) Handles Me.Shown
#If PORTABLE Then
        Return
#End If
            ' Temporary
            Return
            If Not My.Settings.CheckForUpdatesAsked Then
                Dim commandButtons() As String =
                        {Language.Language.strAskUpdatesCommandRecommended, Language.Language.strAskUpdatesCommandCustom,
                         Language.Language.strAskUpdatesCommandAskLater}
                cTaskDialog.ShowTaskDialogBox(Me, Application.Info.ProductName,
                                              Language.Language.strAskUpdatesMainInstruction,
                                              String.Format(Language.Language.strAskUpdatesContent,
                                                            Application.Info.ProductName), "", "", "", "",
                                              String.Join("|", commandButtons), eTaskDialogButtons.None,
                                              eSysIcons.Question, eSysIcons.Question)
                If cTaskDialog.CommandButtonResult = 0 Or cTaskDialog.CommandButtonResult = 1 Then
                    My.Settings.CheckForUpdatesAsked = True
                End If
                If cTaskDialog.CommandButtonResult = 1 Then
                    Runtime.Windows.ShowUpdatesTab()
                End If
                Return
            End If

            If Not My.Settings.CheckForUpdatesOnStartup Then Return

            Dim nextUpdateCheck As Date =
                    My.Settings.CheckForUpdatesLastCheck.Add(TimeSpan.FromDays(My.Settings.CheckForUpdatesFrequencyDays))
            If My.Settings.UpdatePending Or Date.UtcNow > nextUpdateCheck Then
                If Not IsHandleCreated Then CreateHandle() _
                ' Make sure the handle is created so that InvokeRequired returns the correct result
                Runtime.Startup.CheckForUpdate()
                Runtime.Startup.CheckForAnnouncement()
            End If
        End Sub

        Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
            If Not (Runtime.WindowList Is Nothing OrElse Runtime.WindowList.Count = 0) Then
                Dim connectionWindow As UI.Window.Connection
                Dim openConnections = 0
                For Each window As UI.Window.Base In Runtime.WindowList
                    connectionWindow = TryCast(window, UI.Window.Connection)
                    If connectionWindow IsNot Nothing Then
                        openConnections = openConnections + connectionWindow.TabController.TabPages.Count
                    End If
                Next

                If openConnections > 0 And
                   (My.Settings.ConfirmCloseConnection = ConfirmClose.All Or
                    (My.Settings.ConfirmCloseConnection = ConfirmClose.Multiple And openConnections > 1) Or
                    My.Settings.ConfirmCloseConnection = ConfirmClose.Exit) Then
                    Dim result As DialogResult = cTaskDialog.MessageBox(Me, Application.Info.ProductName,
                                                                        Language.Language.strConfirmExitMainInstruction,
                                                                        "", "", "",
                                                                        Language.Language.
                                                                           strCheckboxDoNotShowThisMessageAgain,
                                                                        eTaskDialogButtons.YesNo, eSysIcons.Question,
                                                                        Nothing)
                    If cTaskDialog.VerificationChecked Then
                        My.Settings.ConfirmCloseConnection = My.Settings.ConfirmCloseConnection - 1
                    End If
                    If result = DialogResult.No Then
                        e.Cancel = True
                        Return
                    End If
                End If
            End If

            Runtime.Shutdown.Cleanup()

            _isClosing = True

            If Runtime.WindowList IsNot Nothing Then
                For Each window As UI.Window.Base In Runtime.WindowList
                    window.Close()
                Next
            End If

            Runtime.Shutdown.StartUpdate()

            Debug.Print("[END] - " & Now)
        End Sub

#End Region

#Region "Timer"

        Private Sub tmrAutoSave_Tick(sender As Object, e As EventArgs) Handles tmrAutoSave.Tick
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Doing AutoSave", True)
            Runtime.SaveConnections()
        End Sub

#End Region

#Region "Ext Apps Toolbar"

        Private Sub cMenToolbarShowText_Click(sender As Object, e As EventArgs) Handles cMenToolbarShowText.Click
            SwitchToolBarText(Not cMenToolbarShowText.Checked)
        End Sub

        Public Sub AddExternalToolsToToolBar()
            Try
                For index As Integer = tsExternalTools.Items.Count - 1 To 0 Step -1
                    tsExternalTools.Items(index).Dispose()
                Next
                tsExternalTools.Items.Clear()

                Dim button As ToolStripButton
                For Each tool As ExternalTool In Runtime.ExternalTools
                    button = tsExternalTools.Items.Add(tool.DisplayName, tool.Image, AddressOf tsExtAppEntry_Click)

                    If cMenToolbarShowText.Checked = True Then
                        button.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
                    Else
                        If button.Image IsNot Nothing Then
                            button.DisplayStyle = ToolStripItemDisplayStyle.Image
                        Else
                            button.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
                        End If
                    End If

                    button.Tag = tool
                Next
            Catch ex As Exception
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    String.Format(
                                                        Language.Language.strErrorAddExternalToolsToToolBarFailed,
                                                        ex.ToString()), True)
            End Try
        End Sub

        Private Sub tsExtAppEntry_Click(sender As Object, e As EventArgs)
            Dim extA As ExternalTool = sender.Tag

            If Node.GetNodeType(Node.SelectedNode) = Node.Type.Connection Or
               Node.GetNodeType(Node.SelectedNode) = Node.Type.PuttySession Then
                extA.Start(Node.SelectedNode.Tag)
            Else
                extA.Start()
            End If
        End Sub

        Public Sub SwitchToolBarText(show As Boolean)
            For Each tItem As ToolStripButton In tsExternalTools.Items
                If show = True Then
                    tItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
                Else
                    If tItem.Image IsNot Nothing Then
                        tItem.DisplayStyle = ToolStripItemDisplayStyle.Image
                    Else
                        tItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
                    End If
                End If
            Next

            cMenToolbarShowText.Checked = show
        End Sub

#End Region

#Region "Menu"

#Region "File"

        Private Sub mMenFile_DropDownOpening(sender As Object, e As EventArgs) Handles mMenFile.DropDownOpening
            Select Case Node.GetNodeType(Node.SelectedNode)
                Case Node.Type.Root
                    mMenFileNewConnection.Enabled = True
                    mMenFileNewFolder.Enabled = True
                    mMenFileDelete.Enabled = False
                    mMenFileRename.Enabled = True
                    mMenFileDuplicate.Enabled = False
                    mMenFileDelete.Text = Language.Language.strMenuDelete
                    mMenFileRename.Text = Language.Language.strMenuRenameFolder
                    mMenFileDuplicate.Text = Language.Language.strMenuDuplicate
                Case Node.Type.Container
                    mMenFileNewConnection.Enabled = True
                    mMenFileNewFolder.Enabled = True
                    mMenFileDelete.Enabled = True
                    mMenFileRename.Enabled = True
                    mMenFileDuplicate.Enabled = True
                    mMenFileDelete.Text = Language.Language.strMenuDeleteFolder
                    mMenFileRename.Text = Language.Language.strMenuRenameFolder
                    mMenFileDuplicate.Text = Language.Language.strMenuDuplicateFolder
                Case Node.Type.Connection
                    mMenFileNewConnection.Enabled = True
                    mMenFileNewFolder.Enabled = True
                    mMenFileDelete.Enabled = True
                    mMenFileRename.Enabled = True
                    mMenFileDuplicate.Enabled = True
                    mMenFileDelete.Text = Language.Language.strMenuDeleteConnection
                    mMenFileRename.Text = Language.Language.strMenuRenameConnection
                    mMenFileDuplicate.Text = Language.Language.strMenuDuplicateConnection
                Case Node.Type.PuttyRoot, Node.Type.PuttySession
                    mMenFileNewConnection.Enabled = False
                    mMenFileNewFolder.Enabled = False
                    mMenFileDelete.Enabled = False
                    mMenFileRename.Enabled = False
                    mMenFileDuplicate.Enabled = False
                    mMenFileDelete.Text = Language.Language.strMenuDelete
                    mMenFileRename.Text = Language.Language.strMenuRename
                    mMenFileDuplicate.Text = Language.Language.strMenuDuplicate
                Case Else
                    mMenFileNewConnection.Enabled = True
                    mMenFileNewFolder.Enabled = True
                    mMenFileDelete.Enabled = False
                    mMenFileRename.Enabled = False
                    mMenFileDuplicate.Enabled = False
                    mMenFileDelete.Text = Language.Language.strMenuDelete
                    mMenFileRename.Text = Language.Language.strMenuRename
                    mMenFileDuplicate.Text = Language.Language.strMenuDuplicate
            End Select
        End Sub

        Private Shared Sub mMenFileNewConnection_Click(sender As Object, e As EventArgs) _
            Handles mMenFileNewConnection.Click
            Runtime.Windows.treeForm.AddConnection()
            Runtime.SaveConnectionsBG()
        End Sub

        Private Shared Sub mMenFileNewFolder_Click(sender As Object, e As EventArgs) Handles mMenFileNewFolder.Click
            Runtime.Windows.treeForm.AddFolder()
            Runtime.SaveConnectionsBG()
        End Sub

        Private Shared Sub mMenFileNew_Click(sender As Object, e As EventArgs) Handles mMenFileNew.Click
            Dim saveFileDialog As SaveFileDialog = Tools.Controls.ConnectionsSaveAsDialog
            If Not saveFileDialog.ShowDialog() = DialogResult.OK Then Return

            Runtime.NewConnections(saveFileDialog.FileName)
        End Sub

        Private Shared Sub mMenFileLoad_Click(sender As Object, e As EventArgs) Handles mMenFileLoad.Click
            If Runtime.IsConnectionsFileLoaded Then
                Select Case _
                    MsgBox(Language.Language.strSaveConnectionsFileBeforeOpeningAnother,
                           MsgBoxStyle.YesNoCancel Or MsgBoxStyle.Question)
                    Case MsgBoxResult.Yes
                        Runtime.SaveConnections()
                    Case MsgBoxResult.Cancel
                        Return
                End Select
            End If

            Runtime.LoadConnections(True)
        End Sub

        Private Shared Sub mMenFileSave_Click(sender As Object, e As EventArgs) Handles mMenFileSave.Click
            Runtime.SaveConnections()
        End Sub

        Private Shared Sub mMenFileSaveAs_Click(sender As Object, e As EventArgs) Handles mMenFileSaveAs.Click
            Runtime.SaveConnectionsAs()
        End Sub

        Private Shared Sub mMenFileDelete_Click(sender As Object, e As EventArgs) Handles mMenFileDelete.Click
            Node.DeleteSelectedNode()
            Runtime.SaveConnectionsBG()
        End Sub

        Private Shared Sub mMenFileRename_Click(sender As Object, e As EventArgs) Handles mMenFileRename.Click
            Node.StartRenameSelectedNode()
            Runtime.SaveConnectionsBG()
        End Sub

        Private Shared Sub mMenFileDuplicate_Click(sender As Object, e As EventArgs) Handles mMenFileDuplicate.Click
            Node.CloneNode(Node.SelectedNode)
            Runtime.SaveConnectionsBG()
        End Sub

        Private Shared Sub mMenFileImportFromFile_Click(sender As Object, e As EventArgs) _
            Handles mMenFileImportFromFile.Click
            Import.ImportFromFile(Runtime.Windows.treeForm.tvConnections.Nodes(0),
                                  Runtime.Windows.treeForm.tvConnections.SelectedNode)
        End Sub

        Private Shared Sub mMenFileImportFromActiveDirectory_Click(sender As Object, e As EventArgs) _
            Handles mMenFileImportFromActiveDirectory.Click
            Runtime.Windows.Show(Type.ActiveDirectoryImport)
        End Sub

        Private Shared Sub mMenFileImportFromPortScan_Click(sender As Object, e As EventArgs) _
            Handles mMenFileImportFromPortScan.Click
            Runtime.Windows.Show(Type.PortScan, True)
        End Sub

        Private Shared Sub mMenFileExport_Click(sender As Object, e As EventArgs) Handles mMenFileExport.Click
            Export.ExportToFile(Runtime.Windows.treeForm.tvConnections.Nodes(0),
                                Runtime.Windows.treeForm.tvConnections.SelectedNode)
        End Sub

        Private Shared Sub mMenFileExit_Click(sender As Object, e As EventArgs) Handles mMenFileExit.Click
            Runtime.Shutdown.Quit()
        End Sub

#End Region

#Region "View"

        Private Sub mMenView_DropDownOpening(sender As Object, e As EventArgs) Handles mMenView.DropDownOpening
            Me.mMenViewConnections.Checked = Not Runtime.Windows.treeForm.IsHidden
            Me.mMenViewConfig.Checked = Not Runtime.Windows.configForm.IsHidden
            Me.mMenViewErrorsAndInfos.Checked = Not Runtime.Windows.errorsForm.IsHidden
            Me.mMenViewScreenshotManager.Checked = Not Runtime.Windows.screenshotForm.IsHidden

            Me.mMenViewExtAppsToolbar.Checked = tsExternalTools.Visible
            Me.mMenViewQuickConnectToolbar.Checked = tsQuickConnect.Visible

            Me.mMenViewConnectionPanels.DropDownItems.Clear()

            For i = 0 To Runtime.WindowList.Count - 1
                Dim _
                    tItem As _
                        New ToolStripMenuItem(Runtime.WindowList(i).Text, Runtime.WindowList(i).Icon.ToBitmap,
                                              AddressOf ConnectionPanelMenuItem_Click)
                tItem.Tag = Runtime.WindowList(i)

                Me.mMenViewConnectionPanels.DropDownItems.Add(tItem)
            Next

            If Me.mMenViewConnectionPanels.DropDownItems.Count > 0 Then
                Me.mMenViewConnectionPanels.Enabled = True
            Else
                Me.mMenViewConnectionPanels.Enabled = False
            End If
        End Sub

        Private Sub ConnectionPanelMenuItem_Click(sender As Object, e As EventArgs)
            TryCast(sender.Tag, UI.Window.Base).Show(Me.pnlDock)
            TryCast(sender.Tag, UI.Window.Base).Focus()
        End Sub

        Private Sub mMenViewConnections_Click(sender As Object, e As EventArgs) Handles mMenViewConnections.Click
            If Me.mMenViewConnections.Checked = False Then
                Runtime.Windows.treePanel.Show(Me.pnlDock)
                Me.mMenViewConnections.Checked = True
            Else
                Runtime.Windows.treePanel.Hide()
                Me.mMenViewConnections.Checked = False
            End If
        End Sub

        Private Sub mMenViewConfig_Click(sender As Object, e As EventArgs) Handles mMenViewConfig.Click
            If Me.mMenViewConfig.Checked = False Then
                Runtime.Windows.configPanel.Show(Me.pnlDock)
                Me.mMenViewConfig.Checked = True
            Else
                Runtime.Windows.configPanel.Hide()
                Me.mMenViewConfig.Checked = False
            End If
        End Sub

        Private Sub mMenViewErrorsAndInfos_Click(sender As Object, e As EventArgs) Handles mMenViewErrorsAndInfos.Click
            If Me.mMenViewErrorsAndInfos.Checked = False Then
                Runtime.Windows.errorsPanel.Show(Me.pnlDock)
                Me.mMenViewErrorsAndInfos.Checked = True
            Else
                Runtime.Windows.errorsPanel.Hide()
                Me.mMenViewErrorsAndInfos.Checked = False
            End If
        End Sub

        Private Sub mMenViewScreenshotManager_Click(sender As Object, e As EventArgs) _
            Handles mMenViewScreenshotManager.Click
            If Me.mMenViewScreenshotManager.Checked = False Then
                Runtime.Windows.screenshotPanel.Show(Me.pnlDock)
                Me.mMenViewScreenshotManager.Checked = True
            Else
                Runtime.Windows.screenshotPanel.Hide()
                Me.mMenViewScreenshotManager.Checked = False
            End If
        End Sub

        Private Sub mMenViewJumpToConnectionsConfig_Click(sender As Object, e As EventArgs) _
            Handles mMenViewJumpToConnectionsConfig.Click
            If pnlDock.ActiveContent Is Runtime.Windows.treePanel Then
                Runtime.Windows.configForm.Activate()
            Else
                Runtime.Windows.treeForm.Activate()
            End If
        End Sub

        Private Sub mMenViewJumpToErrorsInfos_Click(sender As Object, e As EventArgs) _
            Handles mMenViewJumpToErrorsInfos.Click
            Runtime.Windows.errorsForm.Activate()
        End Sub

        Private Sub mMenViewResetLayout_Click(sender As Object, e As EventArgs) Handles mMenViewResetLayout.Click
            If _
                MsgBox(Language.Language.strConfirmResetLayout, MsgBoxStyle.Question Or MsgBoxStyle.YesNo) =
                MsgBoxResult.Yes Then
                Runtime.Startup.SetDefaultLayout()
            End If
        End Sub

        Private Sub mMenViewAddConnectionPanel_Click(sender As Object, e As EventArgs) _
            Handles mMenViewAddConnectionPanel.Click
            Runtime.AddPanel()
        End Sub

        Private Sub mMenViewExtAppsToolbar_Click(sender As Object, e As EventArgs) Handles mMenViewExtAppsToolbar.Click
            If mMenViewExtAppsToolbar.Checked = False Then
                tsExternalTools.Visible = True
                mMenViewExtAppsToolbar.Checked = True
            Else
                tsExternalTools.Visible = False
                mMenViewExtAppsToolbar.Checked = False
            End If
        End Sub

        Private Sub mMenViewQuickConnectToolbar_Click(sender As Object, e As EventArgs) _
            Handles mMenViewQuickConnectToolbar.Click
            If mMenViewQuickConnectToolbar.Checked = False Then
                tsQuickConnect.Visible = True
                mMenViewQuickConnectToolbar.Checked = True
            Else
                tsQuickConnect.Visible = False
                mMenViewQuickConnectToolbar.Checked = False
            End If
        End Sub

        Public Fullscreen As New Misc.Fullscreen(Me)

        Private Sub mMenViewFullscreen_Click(sender As Object, e As EventArgs) Handles mMenViewFullscreen.Click
            Fullscreen.Value = Not Fullscreen.Value
            mMenViewFullscreen.Checked = Fullscreen.Value
        End Sub

#End Region

#Region "Tools"

        Private Sub mMenToolsUpdate_Click(sender As Object, e As EventArgs) Handles mMenToolsUpdate.Click
            Runtime.Windows.Show(Type.Update)
        End Sub

        Private Sub mMenToolsSSHTransfer_Click(sender As Object, e As EventArgs) Handles mMenToolsSSHTransfer.Click
            Runtime.Windows.Show(Type.SSHTransfer)
        End Sub

        Private Sub mMenToolsUVNCSC_Click(sender As Object, e As EventArgs) Handles mMenToolsUVNCSC.Click
            Runtime.Windows.Show(Type.UltraVNCSC)
        End Sub

        Private Sub mMenToolsExternalApps_Click(sender As Object, e As EventArgs) Handles mMenToolsExternalApps.Click
            Runtime.Windows.Show(Type.ExternalApps)
        End Sub

        Private Sub mMenToolsPortScan_Click(sender As Object, e As EventArgs) Handles mMenToolsPortScan.Click
            Runtime.Windows.Show(Type.PortScan, False)
        End Sub

        Private Sub mMenToolsComponentsCheck_Click(sender As Object, e As EventArgs) _
            Handles mMenToolsComponentsCheck.Click
            Runtime.Windows.Show(Type.ComponentsCheck)
        End Sub

        Private Sub mMenToolsOptions_Click(sender As Object, e As EventArgs) Handles mMenToolsOptions.Click
            Runtime.Windows.Show(Type.Options)
        End Sub

#End Region

#Region "Quick Connect"

        Private Sub PopulateQuickConnectProtocolMenu()
            Try
                mnuQuickConnectProtocol.Items.Clear()
                For Each fieldInfo As FieldInfo In GetType(Protocols).GetFields
                    If Not (fieldInfo.Name = "value__" Or fieldInfo.Name = "IntApp") Then
                        Dim menuItem As New ToolStripMenuItem(fieldInfo.Name)
                        If fieldInfo.Name = My.Settings.QuickConnectProtocol Then
                            menuItem.Checked = True
                            btnQuickConnect.Text = My.Settings.QuickConnectProtocol
                        End If
                        mnuQuickConnectProtocol.Items.Add(menuItem)
                    End If
                Next
            Catch ex As Exception
                Runtime.MessageCollector.AddExceptionMessage("PopulateQuickConnectProtocolMenu() failed.", ex,
                                                             MessageClass.ErrorMsg, True)
            End Try
        End Sub

        Private Sub lblQuickConnect_Click(sender As Object, e As EventArgs) Handles lblQuickConnect.Click
            cmbQuickConnect.Focus()
        End Sub

        Private Sub cmbQuickConnect_ConnectRequested(sender As Object,
                                                     e As QuickConnectComboBox.ConnectRequestedEventArgs) _
            Handles cmbQuickConnect.ConnectRequested
            btnQuickConnect_ButtonClick(sender, e)
        End Sub

        Private Sub btnQuickConnect_ButtonClick(sender As Object, e As EventArgs) Handles btnQuickConnect.ButtonClick
            Try
                Dim connectionInfo As Info = Runtime.CreateQuickConnect(cmbQuickConnect.Text.Trim(),
                                                                        Converter.StringToProtocol(
                                                                            My.Settings.QuickConnectProtocol))
                If connectionInfo Is Nothing Then
                    cmbQuickConnect.Focus()
                    Return
                End If

                cmbQuickConnect.Add(connectionInfo)

                Runtime.OpenConnection(connectionInfo, Info.Force.DoNotJump)
            Catch ex As Exception
                Runtime.MessageCollector.AddExceptionMessage("btnQuickConnect_ButtonClick() failed.", ex,
                                                             MessageClass.ErrorMsg, True)
            End Try
        End Sub

        Private Sub cmbQuickConnect_ProtocolChanged(sender As Object, e As QuickConnectComboBox.ProtocolChangedEventArgs) _
            Handles cmbQuickConnect.ProtocolChanged
            SetQuickConnectProtocol(Converter.ProtocolToString(e.Protocol))
        End Sub

        Private Sub btnQuickConnect_DropDownItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) _
            Handles btnQuickConnect.DropDownItemClicked
            SetQuickConnectProtocol(e.ClickedItem.Text)
        End Sub

        Private Sub SetQuickConnectProtocol(protocol As String)
            My.Settings.QuickConnectProtocol = protocol
            btnQuickConnect.Text = protocol
            For Each menuItem As ToolStripMenuItem In mnuQuickConnectProtocol.Items
                If menuItem.Text = protocol Then
                    menuItem.Checked = True
                Else
                    menuItem.Checked = False
                End If
            Next
        End Sub

#End Region

#Region "Info"

        Private Sub mMenInfoHelp_Click(sender As Object, e As EventArgs) Handles mMenInfoHelp.Click
            Runtime.Windows.Show(Type.Help)
        End Sub

        Private Sub mMenInfoBugReport_Click(sender As Object, e As EventArgs) Handles mMenInfoBugReport.Click
            Runtime.GoToBugs()
        End Sub

        Private Sub mMenInfoWebsite_Click(sender As Object, e As EventArgs) Handles mMenInfoWebsite.Click
            Runtime.GoToWebsite()
        End Sub

        Private Sub mMenInfoAnnouncements_Click(sender As Object, e As EventArgs) Handles mMenInfoAnnouncements.Click
            Runtime.Windows.Show(Type.Announcement)
        End Sub

        Private Sub mMenInfoAbout_Click(sender As Object, e As EventArgs) Handles mMenInfoAbout.Click
            Runtime.Windows.Show(Type.About)
        End Sub

#End Region

#End Region

#Region "Connections DropDown"

        Private Sub btnConnections_DropDownOpening(sender As Object, e As EventArgs) _
            Handles btnConnections.DropDownOpening
            btnConnections.DropDownItems.Clear()

            For Each treeNode As TreeNode In Runtime.Windows.treeForm.tvConnections.Nodes
                AddNodeToMenu(treeNode.Nodes, btnConnections)
            Next
        End Sub

        Private Shared Sub AddNodeToMenu(treeNodeCollection As TreeNodeCollection,
                                         toolStripMenuItem As ToolStripDropDownItem)
            Try
                For Each treeNode As TreeNode In treeNodeCollection
                    Dim menuItem As New ToolStripMenuItem()
                    menuItem.Text = treeNode.Text
                    menuItem.Tag = treeNode

                    If Node.GetNodeType(treeNode) = Node.Type.Container Then
                        menuItem.Image = Resources.Folder
                        menuItem.Tag = treeNode.Tag

                        toolStripMenuItem.DropDownItems.Add(menuItem)
                        AddNodeToMenu(treeNode.Nodes, menuItem)
                    ElseIf Node.GetNodeType(treeNode) = Node.Type.Connection Or
                           Node.GetNodeType(treeNode) = Node.Type.PuttySession Then
                        menuItem.Image = Runtime.Windows.treeForm.imgListTree.Images(treeNode.ImageIndex)
                        menuItem.Tag = treeNode.Tag

                        toolStripMenuItem.DropDownItems.Add(menuItem)
                    End If

                    AddHandler menuItem.MouseUp, AddressOf ConnectionsMenuItem_MouseUp
                Next
            Catch ex As Exception
                Runtime.MessageCollector.AddExceptionMessage("frmMain.AddNodeToMenu() failed", ex, MessageClass.ErrorMsg,
                                                             True)
            End Try
        End Sub

        Private Shared Sub ConnectionsMenuItem_MouseUp(sender As Object, e As MouseEventArgs)
            If e.Button = MouseButtons.Left Then
                If TypeOf sender.Tag Is Info Then
                    Runtime.OpenConnection(sender.Tag)
                End If
            End If
        End Sub

#End Region

#Region "Window Overrides and DockPanel Stuff"

        Private _inSizeMove As Boolean = False

        Private Sub frmMain_ResizeBegin(sender As Object, e As EventArgs) Handles Me.ResizeBegin
            _inSizeMove = True
        End Sub

        Private Sub frmMain_Resize(sender As Object, e As EventArgs) Handles Me.Resize
            If WindowState = FormWindowState.Minimized Then
                If My.Settings.MinimizeToTray Then
                    If Runtime.NotificationAreaIcon Is Nothing Then
                        Runtime.NotificationAreaIcon = New Tools.Controls.NotificationAreaIcon()
                    End If
                    Hide()
                End If
            Else
                PreviousWindowState = WindowState
            End If
        End Sub

        Private Sub frmMain_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
            _inSizeMove = False

            ' This handles activations from clicks that started a size/move operation
            ActivateConnection()
        End Sub

        Private _inMouseActivate As Boolean = False

        Protected Overloads Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
            Try
                Select Case m.Msg
                    Case NativeMethods.WM_MOUSEACTIVATE
                        _inMouseActivate = True
                    Case NativeMethods.WM_ACTIVATEAPP
                        _inMouseActivate = False
                    Case NativeMethods.WM_ACTIVATE
                        ' Ingore this message if it wasn't triggered by a click
                        If Not NativeMethods.LOWORD(m.WParam) = NativeMethods.WA_CLICKACTIVE Then Exit Select

                        Dim control As Control = FromChildHandle(NativeMethods.WindowFromPoint(MousePosition))
                        If Not IsNothing(control) Then
                            ' Let TreeViews and ComboBoxes get focus but don't simulate a mouse event
                            If TypeOf control Is TreeView Or TypeOf control Is ComboBox Then Exit Select

                            If _
                                control.CanSelect Or TypeOf control Is MenuStrip Or TypeOf control Is ToolStrip Or
                                TypeOf control Is TabControl Or TypeOf control Is Crownwood.Magic.Controls.InertButton _
                                Then
                                ' Simulate a mouse event since one wasn't generated by Windows
                                Dim clientMousePosition As Point = control.PointToClient(MousePosition)
                                NativeMethods.SendMessage(control.Handle, NativeMethods.WM_LBUTTONDOWN,
                                                          NativeMethods.MK_LBUTTON,
                                                          NativeMethods.MAKELPARAM(clientMousePosition.X,
                                                                                   clientMousePosition.Y))

                                control.Focus()
                                Exit Select
                            End If
                        End If

                        ' This handles activations from clicks that did not start a size/move operation
                        ActivateConnection()
                    Case NativeMethods.WM_WINDOWPOSCHANGED
                        ' Ignore this message if the window wasn't activated
                        Dim windowPos As NativeMethods.WINDOWPOS = Marshal.PtrToStructure(m.LParam,
                                                                                          GetType(
                                                                                             NativeMethods.WINDOWPOS))
                        If (Not (windowPos.flags And NativeMethods.SWP_NOACTIVATE) = 0) Then Exit Select

                        ' This handles all other activations
                        If Not _inMouseActivate And Not _inSizeMove Then ActivateConnection()
                    Case NativeMethods.WM_SYSCOMMAND
                        For i = 0 To SysMenSubItems.Length - 1
                            If SysMenSubItems(i) = m.WParam Then
                                Runtime.Screens.SendFormToScreen(Screen.AllScreens(i))
                                Exit For
                            End If
                        Next
                    Case NativeMethods.WM_DRAWCLIPBOARD
                        NativeMethods.SendMessage(fpChainedWindowHandle, m.Msg, m.LParam, m.WParam)
                        RaiseEvent clipboardchange()
                    Case NativeMethods.WM_CHANGECBCHAIN
                        'Send to the next window
                        NativeMethods.SendMessage(fpChainedWindowHandle, m.Msg, m.LParam, m.WParam)
                        fpChainedWindowHandle = m.LParam
                End Select
            Catch ex As Exception
            End Try

            MyBase.WndProc(m)
        End Sub

        Private Sub ActivateConnection()
            If TypeOf pnlDock.ActiveDocument Is UI.Window.Connection Then
                Dim cW As UI.Window.Connection = pnlDock.ActiveDocument
                If cW.TabController.SelectedTab IsNot Nothing Then
                    Dim tab As TabPage = cW.TabController.SelectedTab
                    Dim ifc = TryCast(tab.Tag, InterfaceControl)
                    ifc.Protocol.Focus()
                    TryCast(ifc.FindForm, UI.Window.Connection).RefreshIC()
                End If
            End If
        End Sub

        Private Sub pnlDock_ActiveDocumentChanged(sender As Object, e As EventArgs) _
            Handles pnlDock.ActiveDocumentChanged
            ActivateConnection()
            Dim connectionWindow = TryCast(pnlDock.ActiveDocument, UI.Window.Connection)
            If connectionWindow IsNot Nothing Then connectionWindow.UpdateSelectedConnection()
        End Sub

        Private Sub UpdateWindowTitle()
            If InvokeRequired Then
                Invoke(New MethodInvoker(AddressOf UpdateWindowTitle))
                Return
            End If

            Dim titleBuilder As New StringBuilder(Application.Info.ProductName)
            Const separator = " - "

            If Runtime.IsConnectionsFileLoaded Then
                If UsingSqlServer Then
                    titleBuilder.Append(separator)
                    titleBuilder.Append(Language.Language.strSQLServer.TrimEnd(":"))
                Else
                    If Not String.IsNullOrEmpty(ConnectionsFileName) Then
                        titleBuilder.Append(separator)
                        If My.Settings.ShowCompleteConsPathInTitle Then
                            titleBuilder.Append(ConnectionsFileName)
                        Else
                            titleBuilder.Append(Path.GetFileName(ConnectionsFileName))
                        End If
                    End If
                End If
            End If

            If Not (SelectedConnection Is Nothing OrElse String.IsNullOrEmpty(SelectedConnection.Name)) Then
                titleBuilder.Append(separator)
                titleBuilder.Append(SelectedConnection.Name)
            End If

            Text = titleBuilder.ToString()
        End Sub

        Public Sub ShowHidePanelTabs(Optional closingDocument As DockContent = Nothing)
            Dim newDocumentStyle As DocumentStyle = pnlDock.DocumentStyle

            If My.Settings.AlwaysShowPanelTabs Then
                newDocumentStyle = DocumentStyle.DockingWindow ' Show the panel tabs
            Else
                Dim nonConnectionPanelCount = 0
                For Each document As DockContent In pnlDock.Documents
                    If _
                        (closingDocument Is Nothing OrElse document IsNot closingDocument) And
                        Not TypeOf document Is UI.Window.Connection Then
                        nonConnectionPanelCount = nonConnectionPanelCount + 1
                    End If
                Next

                If nonConnectionPanelCount = 0 Then
                    newDocumentStyle = DocumentStyle.DockingSdi ' Hide the panel tabs
                Else
                    newDocumentStyle = DocumentStyle.DockingWindow ' Show the panel tabs
                End If
            End If

            If Not pnlDock.DocumentStyle = newDocumentStyle Then
                pnlDock.DocumentStyle = newDocumentStyle
                pnlDock.Size = New Size(1, 1)
            End If
        End Sub

        Private Sub SelectTabRelative(relativeIndex As Integer)
            If Not TypeOf pnlDock.ActiveDocument Is UI.Window.Connection Then Return

            Dim connectionWindow As UI.Window.Connection = pnlDock.ActiveDocument
            Dim tabController As TabControl = connectionWindow.TabController

            Dim newIndex As Integer = tabController.SelectedIndex + relativeIndex
            While newIndex < 0 Or newIndex >= tabController.TabPages.Count
                If newIndex < 0 Then newIndex = tabController.TabPages.Count + newIndex
                If newIndex >= tabController.TabPages.Count Then newIndex = newIndex - tabController.TabPages.Count
            End While

            tabController.SelectedIndex = newIndex
        End Sub

#End Region

#Region "Screen Stuff"

        Private Sub DisplayChanged(sender As Object, e As EventArgs)
            ResetSysMenuItems()
            AddSysMenuItems()
        End Sub

        Private ReadOnly SysMenSubItems(50) As Integer

        Private Shared Sub ResetSysMenuItems()
            Runtime.SystemMenu.Reset()
        End Sub

        Private Sub AddSysMenuItems()
            Runtime.SystemMenu = New SystemMenu(Me.Handle)
            Dim popMen As IntPtr = Runtime.SystemMenu.CreatePopupMenuItem()

            For i = 0 To Screen.AllScreens.Length - 1
                SysMenSubItems(i) = 200 + i
                Runtime.SystemMenu.AppendMenuItem(popMen, SystemMenu.Flags.MF_STRING, SysMenSubItems(i),
                                                  Language.Language.strScreen & " " & i + 1)
            Next

            Runtime.SystemMenu.InsertMenuItem(Runtime.SystemMenu.SystemMenuHandle, 0,
                                              SystemMenu.Flags.MF_POPUP Or SystemMenu.Flags.MF_BYPOSITION, popMen,
                                              Language.Language.strSendTo)
            Runtime.SystemMenu.InsertMenuItem(Runtime.SystemMenu.SystemMenuHandle, 1,
                                              SystemMenu.Flags.MF_BYPOSITION Or SystemMenu.Flags.MF_SEPARATOR,
                                              IntPtr.Zero, Nothing)
        End Sub

        Private Sub mnuConnections_Opening(sender As Object, e As CancelEventArgs)
        End Sub

#End Region
    End Class
End Namespace