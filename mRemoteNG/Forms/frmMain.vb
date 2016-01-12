Imports System.IO
Imports mRemoteNG.App
Imports mRemoteNG.My
Imports SharedLibraryNG
Imports System.Text
Imports WeifenLuo.WinFormsUI.Docking
Imports mRemoteNG.App.Runtime
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports Crownwood
Imports mRemoteNG.App.Native
Imports PSTaskDialog
Imports mRemoteNG.Config
Imports mRemoteNG.Themes

Public Class frmMain
    Private _previousWindowState As FormWindowState
    Public Property PreviousWindowState As FormWindowState
        Get
            Return _previousWindowState
        End Get
        Set(value As FormWindowState)
            _previousWindowState = value
        End Set
    End Property
    Public Shared Event clipboardchange()
    Private fpChainedWindowHandle As IntPtr

#Region "Properties"
    Private _isClosing As Boolean = False
    Public ReadOnly Property IsClosing() As Boolean
        Get
            Return _isClosing
        End Get
    End Property

    Private _usingSqlServer As Boolean = False
    Public Property UsingSqlServer As Boolean
        Get
            Return _usingSqlServer
        End Get
        Set(value As Boolean)
            If _usingSqlServer = value Then Return
            _usingSqlServer = value
            UpdateWindowTitle()
        End Set
    End Property

    Private _connectionsFileName As String = Nothing
    Public Property ConnectionsFileName As String
        Get
            Return _connectionsFileName
        End Get
        Set(value As String)
            If _connectionsFileName = value Then Return
            _connectionsFileName = value
            UpdateWindowTitle()
        End Set
    End Property

    Private _showFullPathInTitle As Boolean = My.Settings.ShowCompleteConsPathInTitle
    Public Property ShowFullPathInTitle As Boolean
        Get
            Return _showFullPathInTitle
        End Get
        Set(value As Boolean)
            If _showFullPathInTitle = value Then Return
            _showFullPathInTitle = value
            UpdateWindowTitle()
        End Set
    End Property

    Private _selectedConnection As Connection.Info = Nothing
    Public Property SelectedConnection As Connection.Info
        Get
            Return _selectedConnection
        End Get
        Set(value As Connection.Info)
            If _selectedConnection Is value Then Return
            _selectedConnection = value
            UpdateWindowTitle()
        End Set
    End Property
#End Region

#Region "Startup & Shutdown"
    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MainForm = Me

        Startup.CheckCompatibility()

        Startup.CreateLogger()

        ' Create gui config load and save objects
        Dim SettingsLoad As New Config.Settings.Load(Me)

        ' Load GUI Configuration
        SettingsLoad.Load()

        Debug.Print("---------------------------" & vbNewLine & "[START] - " & Now)

        Startup.ParseCommandLineArgs()

        ApplyLanguage()
        PopulateQuickConnectProtocolMenu()

        AddHandler ThemeManager.ThemeChanged, AddressOf ApplyThemes
        ApplyThemes()

        fpChainedWindowHandle = SetClipboardViewer(Me.Handle)

        MessageCollector = New Messages.Collector(Windows.errorsForm)

        WindowList = New UI.Window.List

        Tools.IeBrowserEmulation.Register()

        Startup.GetConnectionIcons()
        Windows.treePanel.Focus()

        Tree.Node.TreeView = Windows.treeForm.tvConnections

        If My.Settings.FirstStart And _
                Not My.Settings.LoadConsFromCustomLocation And _
                Not IO.File.Exists(GetStartupConnectionFileName()) Then
            NewConnections(GetStartupConnectionFileName())
        End If

        'LoadCredentials()
        LoadConnections()
        If Not IsConnectionsFileLoaded Then
            System.Windows.Forms.Application.Exit()
            Return
        End If

        Putty.Sessions.StartWatcher()

        If My.Settings.StartupComponentsCheck Then
            Windows.Show(UI.Window.Type.ComponentsCheck)
        End If

#If PORTABLE Then
        mMenInfoAnnouncements.Visible = False
        mMenToolsUpdate.Visible = False
        mMenInfoSep2.Visible = False
#End If

        Startup.CreateSQLUpdateHandlerAndStartTimer()

        AddSysMenuItems()
        AddHandler Microsoft.Win32.SystemEvents.DisplaySettingsChanged, AddressOf DisplayChanged

        Me.Opacity = 1

        KeyboardShortcuts.RequestKeyNotifications(Handle)
    End Sub

    Private Sub ApplyLanguage()
        mMenFile.Text = My.Language.strMenuFile
        mMenFileNew.Text = My.Language.strMenuNewConnectionFile
        mMenFileNewConnection.Text = My.Language.strNewConnection
        mMenFileNewFolder.Text = My.Language.strNewFolder
        mMenFileLoad.Text = My.Language.strMenuOpenConnectionFile
        mMenFileSave.Text = My.Language.strMenuSaveConnectionFile
        mMenFileSaveAs.Text = My.Language.strMenuSaveConnectionFileAs
        mMenFileImport.Text = Language.strImportMenuItem
        mMenFileImportFromFile.Text = Language.strImportFromFileMenuItem
        mMenFileImportFromActiveDirectory.Text = Language.strImportAD
        mMenFileImportFromPortScan.Text = Language.strImportPortScan
        mMenFileExport.Text = Language.strExportToFileMenuItem
        mMenFileExit.Text = My.Language.strMenuExit

        mMenView.Text = My.Language.strMenuView
        mMenViewAddConnectionPanel.Text = My.Language.strMenuAddConnectionPanel
        mMenViewConnectionPanels.Text = My.Language.strMenuConnectionPanels
        mMenViewConnections.Text = My.Language.strMenuConnections
        mMenViewConfig.Text = My.Language.strMenuConfig
        mMenViewSessions.Text = My.Language.strMenuSessions
        mMenViewErrorsAndInfos.Text = My.Language.strMenuNotifications
        mMenViewScreenshotManager.Text = My.Language.strScreenshots
        mMenViewJumpTo.Text = My.Language.strMenuJumpTo
        mMenViewJumpToConnectionsConfig.Text = My.Language.strMenuConnectionsAndConfig
        mMenViewJumpToSessionsScreenshots.Text = My.Language.strMenuSessionsAndScreenshots
        mMenViewJumpToErrorsInfos.Text = My.Language.strMenuNotifications
        mMenViewResetLayout.Text = My.Language.strMenuResetLayout
        mMenViewQuickConnectToolbar.Text = My.Language.strMenuQuickConnectToolbar
        mMenViewExtAppsToolbar.Text = My.Language.strMenuExternalToolsToolbar
        mMenViewFullscreen.Text = My.Language.strMenuFullScreen

        mMenTools.Text = My.Language.strMenuTools
        mMenToolsSSHTransfer.Text = My.Language.strMenuSSHFileTransfer
        mMenToolsExternalApps.Text = My.Language.strMenuExternalTools
        mMenToolsPortScan.Text = My.Language.strMenuPortScan
        mMenToolsComponentsCheck.Text = My.Language.strComponentsCheck
        mMenToolsUpdate.Text = My.Language.strMenuCheckForUpdates
        mMenToolsOptions.Text = My.Language.strMenuOptions

        mMenInfo.Text = My.Language.strMenuHelp
        mMenInfoHelp.Text = My.Language.strMenuHelpContents
        mMenInfoForum.Text = My.Language.strMenuSupportForum
        mMenInfoBugReport.Text = My.Language.strMenuReportBug
        mMenInfoDonate.Text = My.Language.strMenuDonate
        mMenInfoWebsite.Text = My.Language.strMenuWebsite
        mMenInfoAbout.Text = My.Language.strMenuAbout
        mMenInfoAnnouncements.Text = My.Language.strMenuAnnouncements

        lblQuickConnect.Text = My.Language.strLabelConnect
        btnQuickConnect.Text = My.Language.strMenuConnect
        btnConnections.Text = My.Language.strMenuConnections

        cMenToolbarShowText.Text = My.Language.strMenuShowText

        ToolStripButton1.Text = My.Language.strConnect
        ToolStripButton2.Text = My.Language.strScreenshot
        ToolStripButton3.Text = My.Language.strRefresh

        ToolStripSplitButton1.Text = My.Language.strSpecialKeys
        ToolStripMenuItem1.Text = My.Language.strKeysCtrlAltDel
        ToolStripMenuItem2.Text = My.Language.strKeysCtrlEsc
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
        If Not My.Settings.CheckForUpdatesAsked Then
            Dim commandButtons() As String = {My.Language.strAskUpdatesCommandRecommended, My.Language.strAskUpdatesCommandCustom, My.Language.strAskUpdatesCommandAskLater}
            cTaskDialog.ShowTaskDialogBox(Me, My.Application.Info.ProductName, My.Language.strAskUpdatesMainInstruction, String.Format(My.Language.strAskUpdatesContent, My.Application.Info.ProductName), "", "", "", "", String.Join("|", commandButtons), eTaskDialogButtons.None, eSysIcons.Question, eSysIcons.Question)
            If cTaskDialog.CommandButtonResult = 0 Or cTaskDialog.CommandButtonResult = 1 Then
                My.Settings.CheckForUpdatesAsked = True
            End If
            If cTaskDialog.CommandButtonResult = 1 Then
                Windows.ShowUpdatesTab()
            End If
            Return
        End If

        If Not My.Settings.CheckForUpdatesOnStartup Then Return

        Dim nextUpdateCheck As Date = My.Settings.CheckForUpdatesLastCheck.Add(TimeSpan.FromDays(My.Settings.CheckForUpdatesFrequencyDays))
        If My.Settings.UpdatePending Or Date.UtcNow > nextUpdateCheck Then
            If Not IsHandleCreated Then CreateHandle() ' Make sure the handle is created so that InvokeRequired returns the correct result
            Startup.CheckForUpdate()
            Startup.CheckForAnnouncement()
        End If
    End Sub

    Private Sub frmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not (WindowList Is Nothing OrElse WindowList.Count = 0) Then
            Dim connectionWindow As UI.Window.Connection
            Dim openConnections As Integer = 0
            For Each window As UI.Window.Base In WindowList
                connectionWindow = TryCast(window, UI.Window.Connection)
                If connectionWindow IsNot Nothing Then
                    openConnections = openConnections + connectionWindow.TabController.TabPages.Count
                End If
            Next

            If openConnections > 0 And _
                    (My.Settings.ConfirmCloseConnection = ConfirmClose.All Or _
                    (My.Settings.ConfirmCloseConnection = ConfirmClose.Multiple And openConnections > 1) Or _
                     My.Settings.ConfirmCloseConnection = ConfirmClose.Exit) Then
                Dim result As DialogResult = cTaskDialog.MessageBox(Me, My.Application.Info.ProductName, My.Language.strConfirmExitMainInstruction, "", "", "", My.Language.strCheckboxDoNotShowThisMessageAgain, eTaskDialogButtons.YesNo, eSysIcons.Question, Nothing)
                If cTaskDialog.VerificationChecked Then
                    My.Settings.ConfirmCloseConnection = My.Settings.ConfirmCloseConnection - 1
                End If
                If result = DialogResult.No Then
                    e.Cancel = True
                    Return
                End If
            End If
        End If

        Shutdown.Cleanup()

        _isClosing = True

        If WindowList IsNot Nothing Then
            For Each window As UI.Window.Base In WindowList
                window.Close()
            Next
        End If

        Shutdown.StartUpdate()

        Debug.Print("[END] - " & Now)
    End Sub
#End Region

#Region "Timer"
    Private Sub tmrAutoSave_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrAutoSave.Tick
        MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, "Doing AutoSave", True)
        App.Runtime.SaveConnections()
    End Sub
#End Region

#Region "Ext Apps Toolbar"
    Private Sub cMenToolbarShowText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenToolbarShowText.Click
        SwitchToolBarText(Not cMenToolbarShowText.Checked)
    End Sub

    Public Sub AddExternalToolsToToolBar()
        Try
            For index As Integer = tsExternalTools.Items.Count - 1 To 0 Step -1
                tsExternalTools.Items(index).Dispose()
            Next
            tsExternalTools.Items.Clear()

            Dim button As ToolStripButton
            For Each tool As Tools.ExternalTool In ExternalTools
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
            MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, String.Format(My.Language.strErrorAddExternalToolsToToolBarFailed, ex.Message), True)
        End Try
    End Sub

    Private Sub tsExtAppEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim extA As Tools.ExternalTool = sender.Tag

        If Tree.Node.GetNodeType(Tree.Node.SelectedNode) = Tree.Node.Type.Connection Or _
           Tree.Node.GetNodeType(Tree.Node.SelectedNode) = Tree.Node.Type.PuttySession Then
            extA.Start(Tree.Node.SelectedNode.Tag)
        Else
            extA.Start()
        End If
    End Sub

    Public Sub SwitchToolBarText(ByVal show As Boolean)
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
    Private Sub mMenFile_DropDownOpening(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenFile.DropDownOpening
        Select Case Tree.Node.GetNodeType(Tree.Node.SelectedNode)
            Case Tree.Node.Type.Root
                mMenFileNewConnection.Enabled = True
                mMenFileNewFolder.Enabled = True
                mMenFileDelete.Enabled = False
                mMenFileRename.Enabled = True
                mMenFileDuplicate.Enabled = False
                mMenFileDelete.Text = My.Language.strMenuDelete
                mMenFileRename.Text = My.Language.strMenuRenameFolder
                mMenFileDuplicate.Text = My.Language.strMenuDuplicate
            Case Tree.Node.Type.Container
                mMenFileNewConnection.Enabled = True
                mMenFileNewFolder.Enabled = True
                mMenFileDelete.Enabled = True
                mMenFileRename.Enabled = True
                mMenFileDuplicate.Enabled = True
                mMenFileDelete.Text = My.Language.strMenuDeleteFolder
                mMenFileRename.Text = My.Language.strMenuRenameFolder
                mMenFileDuplicate.Text = My.Language.strMenuDuplicateFolder
            Case Tree.Node.Type.Connection
                mMenFileNewConnection.Enabled = True
                mMenFileNewFolder.Enabled = True
                mMenFileDelete.Enabled = True
                mMenFileRename.Enabled = True
                mMenFileDuplicate.Enabled = True
                mMenFileDelete.Text = My.Language.strMenuDeleteConnection
                mMenFileRename.Text = My.Language.strMenuRenameConnection
                mMenFileDuplicate.Text = My.Language.strMenuDuplicateConnection
            Case Tree.Node.Type.PuttyRoot, Tree.Node.Type.PuttySession
                mMenFileNewConnection.Enabled = False
                mMenFileNewFolder.Enabled = False
                mMenFileDelete.Enabled = False
                mMenFileRename.Enabled = False
                mMenFileDuplicate.Enabled = False
                mMenFileDelete.Text = My.Language.strMenuDelete
                mMenFileRename.Text = My.Language.strMenuRename
                mMenFileDuplicate.Text = My.Language.strMenuDuplicate
            Case Else
                mMenFileNewConnection.Enabled = True
                mMenFileNewFolder.Enabled = True
                mMenFileDelete.Enabled = False
                mMenFileRename.Enabled = False
                mMenFileDuplicate.Enabled = False
                mMenFileDelete.Text = My.Language.strMenuDelete
                mMenFileRename.Text = My.Language.strMenuRename
                mMenFileDuplicate.Text = My.Language.strMenuDuplicate
        End Select
    End Sub

    Private Shared Sub mMenFileNewConnection_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles mMenFileNewConnection.Click
        Windows.treeForm.AddConnection()
        SaveConnectionsBG()
    End Sub

    Private Shared Sub mMenFileNewFolder_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles mMenFileNewFolder.Click
        Windows.treeForm.AddFolder()
        SaveConnectionsBG()
    End Sub

    Private Shared Sub mMenFileNew_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles mMenFileNew.Click
        Dim saveFileDialog As SaveFileDialog = Tools.Controls.ConnectionsSaveAsDialog
        If Not saveFileDialog.ShowDialog() = DialogResult.OK Then Return

        NewConnections(saveFileDialog.FileName)
    End Sub

    Private Shared Sub mMenFileLoad_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles mMenFileLoad.Click
        If IsConnectionsFileLoaded Then
            Select Case MsgBox(Language.strSaveConnectionsFileBeforeOpeningAnother, MsgBoxStyle.YesNoCancel Or MsgBoxStyle.Question)
                Case MsgBoxResult.Yes
                    SaveConnections()
                Case MsgBoxResult.Cancel
                    Return
            End Select
        End If

        LoadConnections(True)
    End Sub

    Private Shared Sub mMenFileSave_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles mMenFileSave.Click
        SaveConnections()
    End Sub

    Private Shared Sub mMenFileSaveAs_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles mMenFileSaveAs.Click
        SaveConnectionsAs()
    End Sub

    Private Shared Sub mMenFileDelete_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles mMenFileDelete.Click
        Tree.Node.DeleteSelectedNode()
        SaveConnectionsBG()
    End Sub

    Private Shared Sub mMenFileRename_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles mMenFileRename.Click
        Tree.Node.StartRenameSelectedNode()
        SaveConnectionsBG()
    End Sub

    Private Shared Sub mMenFileDuplicate_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles mMenFileDuplicate.Click
        Tree.Node.CloneNode(Tree.Node.SelectedNode)
        SaveConnectionsBG()
    End Sub

    Private Shared Sub mMenFileImportFromFile_Click(sender As System.Object, e As EventArgs) Handles mMenFileImportFromFile.Click
        Import.ImportFromFile(Windows.treeForm.tvConnections.Nodes(0), Windows.treeForm.tvConnections.SelectedNode)
    End Sub

    Private Shared Sub mMenFileImportFromActiveDirectory_Click(sender As System.Object, e As EventArgs) Handles mMenFileImportFromActiveDirectory.Click
        Windows.Show(UI.Window.Type.ActiveDirectoryImport)
    End Sub

    Private Shared Sub mMenFileImportFromPortScan_Click(sender As System.Object, e As EventArgs) Handles mMenFileImportFromPortScan.Click
        Windows.Show(UI.Window.Type.PortScan, True)
    End Sub

    Private Shared Sub mMenFileExport_Click(sender As System.Object, e As EventArgs) Handles mMenFileExport.Click
        Export.ExportToFile(Windows.treeForm.tvConnections.Nodes(0), Windows.treeForm.tvConnections.SelectedNode)
    End Sub

    Private Shared Sub mMenFileExit_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles mMenFileExit.Click
        Shutdown.Quit()
    End Sub
#End Region

#Region "View"
    Private Sub mMenView_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles mMenView.DropDownOpening
        Me.mMenViewConnections.Checked = Not Windows.treeForm.IsHidden
        Me.mMenViewConfig.Checked = Not Windows.configForm.IsHidden
        Me.mMenViewErrorsAndInfos.Checked = Not Windows.errorsForm.IsHidden
        Me.mMenViewSessions.Checked = Not Windows.sessionsForm.IsHidden
        Me.mMenViewScreenshotManager.Checked = Not Windows.screenshotForm.IsHidden

        Me.mMenViewExtAppsToolbar.Checked = tsExternalTools.Visible
        Me.mMenViewQuickConnectToolbar.Checked = tsQuickConnect.Visible

        Me.mMenViewConnectionPanels.DropDownItems.Clear()

        For i As Integer = 0 To WindowList.Count - 1
            Dim tItem As New ToolStripMenuItem(WindowList(i).Text, WindowList(i).Icon.ToBitmap, AddressOf ConnectionPanelMenuItem_Click)
            tItem.Tag = WindowList(i)

            Me.mMenViewConnectionPanels.DropDownItems.Add(tItem)
        Next

        If Me.mMenViewConnectionPanels.DropDownItems.Count > 0 Then
            Me.mMenViewConnectionPanels.Enabled = True
        Else
            Me.mMenViewConnectionPanels.Enabled = False
        End If
    End Sub

    Private Sub ConnectionPanelMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        TryCast(sender.Tag, UI.Window.Base).Show(Me.pnlDock)
        TryCast(sender.Tag, UI.Window.Base).Focus()
    End Sub

    Private Sub mMenViewSessions_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mMenViewSessions.Click
        If Me.mMenViewSessions.Checked = False Then
            Windows.sessionsPanel.Show(Me.pnlDock)
            Me.mMenViewSessions.Checked = True
        Else
            Windows.sessionsPanel.Hide()
            Me.mMenViewSessions.Checked = False
        End If
    End Sub

    Private Sub mMenViewConnections_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenViewConnections.Click
        If Me.mMenViewConnections.Checked = False Then
            Windows.treePanel.Show(Me.pnlDock)
            Me.mMenViewConnections.Checked = True
        Else
            Windows.treePanel.Hide()
            Me.mMenViewConnections.Checked = False
        End If
    End Sub

    Private Sub mMenViewConfig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenViewConfig.Click
        If Me.mMenViewConfig.Checked = False Then
            Windows.configPanel.Show(Me.pnlDock)
            Me.mMenViewConfig.Checked = True
        Else
            Windows.configPanel.Hide()
            Me.mMenViewConfig.Checked = False
        End If
    End Sub

    Private Sub mMenViewErrorsAndInfos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenViewErrorsAndInfos.Click
        If Me.mMenViewErrorsAndInfos.Checked = False Then
            Windows.errorsPanel.Show(Me.pnlDock)
            Me.mMenViewErrorsAndInfos.Checked = True
        Else
            Windows.errorsPanel.Hide()
            Me.mMenViewErrorsAndInfos.Checked = False
        End If
    End Sub

    Private Sub mMenViewScreenshotManager_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenViewScreenshotManager.Click
        If Me.mMenViewScreenshotManager.Checked = False Then
            Windows.screenshotPanel.Show(Me.pnlDock)
            Me.mMenViewScreenshotManager.Checked = True
        Else
            Windows.screenshotPanel.Hide()
            Me.mMenViewScreenshotManager.Checked = False
        End If
    End Sub

    Private Sub mMenViewJumpToConnectionsConfig_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mMenViewJumpToConnectionsConfig.Click
        If pnlDock.ActiveContent Is Windows.treePanel Then
            Windows.configForm.Activate()
        Else
            Windows.treeForm.Activate()
        End If
    End Sub

    Private Sub mMenViewJumpToSessionsScreenshots_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mMenViewJumpToSessionsScreenshots.Click
        If pnlDock.ActiveContent Is Windows.sessionsPanel Then
            Windows.screenshotForm.Activate()
        Else
            Windows.sessionsForm.Activate()
        End If
    End Sub

    Private Sub mMenViewJumpToErrorsInfos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mMenViewJumpToErrorsInfos.Click
        Windows.errorsForm.Activate()
    End Sub

    Private Sub mMenViewResetLayout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenViewResetLayout.Click
        If MsgBox(My.Language.strConfirmResetLayout, MsgBoxStyle.Question Or MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            App.Runtime.Startup.SetDefaultLayout()
        End If
    End Sub

    Private Sub mMenViewAddConnectionPanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenViewAddConnectionPanel.Click
        AddPanel()
    End Sub

    Private Sub mMenViewExtAppsToolbar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenViewExtAppsToolbar.Click
        If mMenViewExtAppsToolbar.Checked = False Then
            tsExternalTools.Visible = True
            mMenViewExtAppsToolbar.Checked = True
        Else
            tsExternalTools.Visible = False
            mMenViewExtAppsToolbar.Checked = False
        End If
    End Sub

    Private Sub mMenViewQuickConnectToolbar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenViewQuickConnectToolbar.Click
        If mMenViewQuickConnectToolbar.Checked = False Then
            tsQuickConnect.Visible = True
            mMenViewQuickConnectToolbar.Checked = True
        Else
            tsQuickConnect.Visible = False
            mMenViewQuickConnectToolbar.Checked = False
        End If
    End Sub

    Public Fullscreen As New Tools.Misc.Fullscreen(Me)
    Private Sub mMenViewFullscreen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenViewFullscreen.Click
        Fullscreen.Value = Not Fullscreen.Value
        mMenViewFullscreen.Checked = Fullscreen.Value
    End Sub
#End Region

#Region "Tools"
    Private Sub mMenToolsUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenToolsUpdate.Click
        App.Runtime.Windows.Show(UI.Window.Type.Update)
    End Sub

    Private Sub mMenToolsSSHTransfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenToolsSSHTransfer.Click
        App.Runtime.Windows.Show(UI.Window.Type.SSHTransfer)
    End Sub

    Private Sub mMenToolsUVNCSC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenToolsUVNCSC.Click
        App.Runtime.Windows.Show(UI.Window.Type.UltraVNCSC)
    End Sub

    Private Sub mMenToolsExternalApps_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenToolsExternalApps.Click
        App.Runtime.Windows.Show(UI.Window.Type.ExternalApps)
    End Sub

    Private Sub mMenToolsPortScan_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles mMenToolsPortScan.Click
        App.Runtime.Windows.Show(UI.Window.Type.PortScan, False)
    End Sub

    Private Sub mMenToolsComponentsCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenToolsComponentsCheck.Click
        App.Runtime.Windows.Show(UI.Window.Type.ComponentsCheck)
    End Sub

    Private Sub mMenToolsOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenToolsOptions.Click
        App.Runtime.Windows.Show(UI.Window.Type.Options)
    End Sub
#End Region

#Region "Quick Connect"
    Private Sub PopulateQuickConnectProtocolMenu()
        Try
            mnuQuickConnectProtocol.Items.Clear()
            For Each fieldInfo As FieldInfo In GetType(Connection.Protocol.Protocols).GetFields
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
            MessageCollector.AddExceptionMessage("PopulateQuickConnectProtocolMenu() failed.", ex, Messages.MessageClass.ErrorMsg, True)
        End Try
    End Sub

    Private Sub lblQuickConnect_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lblQuickConnect.Click
        cmbQuickConnect.Focus()
    End Sub

    Private Sub cmbQuickConnect_ConnectRequested(ByVal sender As Object, ByVal e As Controls.QuickConnectComboBox.ConnectRequestedEventArgs) Handles cmbQuickConnect.ConnectRequested
        btnQuickConnect_ButtonClick(sender, e)
    End Sub

    Private Sub btnQuickConnect_ButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles btnQuickConnect.ButtonClick
        Try
            Dim connectionInfo As Connection.Info = CreateQuickConnect(cmbQuickConnect.Text.Trim(), Connection.Protocol.Converter.StringToProtocol(My.Settings.QuickConnectProtocol))
            If connectionInfo Is Nothing Then
                cmbQuickConnect.Focus()
                Return
            End If

            cmbQuickConnect.Add(connectionInfo)

            OpenConnection(connectionInfo, Connection.Info.Force.DoNotJump)
        Catch ex As Exception
            MessageCollector.AddExceptionMessage("btnQuickConnect_ButtonClick() failed.", ex, Messages.MessageClass.ErrorMsg, True)
        End Try
    End Sub

    Private Sub cmbQuickConnect_ProtocolChanged(ByVal sender As Object, ByVal e As Controls.QuickConnectComboBox.ProtocolChangedEventArgs) Handles cmbQuickConnect.ProtocolChanged
        SetQuickConnectProtocol(Connection.Protocol.Converter.ProtocolToString(e.Protocol))
    End Sub

    Private Sub btnQuickConnect_DropDownItemClicked(sender As System.Object, e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles btnQuickConnect.DropDownItemClicked
        SetQuickConnectProtocol(e.ClickedItem.Text)
    End Sub

    Private Sub SetQuickConnectProtocol(ByVal protocol As String)
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
    Private Sub mMenInfoHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenInfoHelp.Click
        App.Runtime.Windows.Show(UI.Window.Type.Help)
    End Sub

    Private Sub mMenInfoForum_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenInfoForum.Click
        App.Runtime.GoToForum()
    End Sub

    Private Sub mMenInfoBugReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenInfoBugReport.Click
        App.Runtime.GoToBugs()
    End Sub

    Private Sub mMenInfoWebsite_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenInfoWebsite.Click
        App.Runtime.GoToWebsite()
    End Sub

    Private Sub mMenInfoDonate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenInfoDonate.Click
        App.Runtime.GoToDonate()
    End Sub

    Private Sub mMenInfoAnnouncements_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenInfoAnnouncements.Click
        App.Runtime.Windows.Show(UI.Window.Type.Announcement)
    End Sub

    Private Sub mMenInfoAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenInfoAbout.Click
        App.Runtime.Windows.Show(UI.Window.Type.About)
    End Sub
#End Region
#End Region

#Region "Connections DropDown"
    Private Sub btnConnections_DropDownOpening(ByVal sender As Object, ByVal e As EventArgs) Handles btnConnections.DropDownOpening
        btnConnections.DropDownItems.Clear()

        For Each treeNode As TreeNode In Windows.treeForm.tvConnections.Nodes
            AddNodeToMenu(treeNode.Nodes, btnConnections)
        Next
    End Sub

    Private Shared Sub AddNodeToMenu(ByVal treeNodeCollection As TreeNodeCollection, ByVal toolStripMenuItem As ToolStripDropDownItem)
        Try
            For Each treeNode As TreeNode In treeNodeCollection
                Dim menuItem As New ToolStripMenuItem()
                menuItem.Text = treeNode.Text
                menuItem.Tag = treeNode

                If Tree.Node.GetNodeType(treeNode) = Tree.Node.Type.Container Then
                    menuItem.Image = My.Resources.Folder
                    menuItem.Tag = treeNode.Tag

                    toolStripMenuItem.DropDownItems.Add(menuItem)
                    AddNodeToMenu(treeNode.Nodes, menuItem)
                ElseIf Tree.Node.GetNodeType(treeNode) = Tree.Node.Type.Connection Or _
                       Tree.Node.GetNodeType(treeNode) = Tree.Node.Type.PuttySession Then
                    menuItem.Image = Windows.treeForm.imgListTree.Images(treeNode.ImageIndex)
                    menuItem.Tag = treeNode.Tag

                    toolStripMenuItem.DropDownItems.Add(menuItem)
                End If

                AddHandler menuItem.MouseUp, AddressOf ConnectionsMenuItem_MouseUp
            Next
        Catch ex As Exception
            MessageCollector.AddExceptionMessage("frmMain.AddNodeToMenu() failed", ex, Messages.MessageClass.ErrorMsg, True)
        End Try
    End Sub

    Private Shared Sub ConnectionsMenuItem_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        If e.Button = MouseButtons.Left Then
            If TypeOf sender.Tag Is Connection.Info Then
                App.Runtime.OpenConnection(sender.Tag)
            End If
        End If
    End Sub
#End Region

#Region "Window Overrides and DockPanel Stuff"
    Private _inSizeMove As Boolean = False
    Private Sub frmMain_ResizeBegin(ByVal sender As Object, ByVal e As EventArgs) Handles Me.ResizeBegin
        _inSizeMove = True
    End Sub

    Private Sub frmMain_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Resize
        If WindowState = FormWindowState.Minimized Then
            If My.Settings.MinimizeToTray Then
                If NotificationAreaIcon Is Nothing Then
                    NotificationAreaIcon = New Tools.Controls.NotificationAreaIcon()
                End If
                Hide()
            End If
        Else
            PreviousWindowState = WindowState
        End If
    End Sub

    Private Sub frmMain_ResizeEnd(ByVal sender As Object, ByVal e As EventArgs) Handles Me.ResizeEnd
        _inSizeMove = False

        ' This handles activations from clicks that started a size/move operation
        ActivateConnection()
    End Sub

    Private _inMouseActivate As Boolean = False
    Protected Overloads Overrides Sub WndProc(ByRef m As Message)
        Try
            Select Case m.Msg
                Case WM_MOUSEACTIVATE
                    _inMouseActivate = True
                Case WM_ACTIVATEAPP
                    _inMouseActivate = False
                Case WM_ACTIVATE
                    ' Ingore this message if it wasn't triggered by a click
                    If Not LOWORD(m.WParam) = WA_CLICKACTIVE Then Exit Select

                    Dim control As Control = FromChildHandle(WindowFromPoint(MousePosition))
                    If Not IsNothing(control) Then
                        ' Let TreeViews and ComboBoxes get focus but don't simulate a mouse event
                        If TypeOf control Is TreeView Or TypeOf control Is ComboBox Then Exit Select

                        If control.CanSelect Or TypeOf control Is MenuStrip Or TypeOf control Is ToolStrip Or TypeOf control Is Magic.Controls.TabControl Or TypeOf control Is Magic.Controls.InertButton Then
                            ' Simulate a mouse event since one wasn't generated by Windows
                            Dim clientMousePosition As Point = control.PointToClient(MousePosition)
                            SendMessage(control.Handle, WM_LBUTTONDOWN, MK_LBUTTON, MAKELPARAM(clientMousePosition.X, clientMousePosition.Y))

                            control.Focus()
                            Exit Select
                        End If
                    End If

                    ' This handles activations from clicks that did not start a size/move operation
                    ActivateConnection()
                Case WM_WINDOWPOSCHANGED
                    ' Ignore this message if the window wasn't activated
                    Dim windowPos As WINDOWPOS = Marshal.PtrToStructure(m.LParam, GetType(WINDOWPOS))
                    If (Not (windowPos.flags And SWP_NOACTIVATE) = 0) Then Exit Select

                    ' This handles all other activations
                    If Not _inMouseActivate And Not _inSizeMove Then ActivateConnection()
                Case WM_SYSCOMMAND
                    For i As Integer = 0 To SysMenSubItems.Length - 1
                        If SysMenSubItems(i) = m.WParam Then
                            Screens.SendFormToScreen(Screen.AllScreens(i))
                            Exit For
                        End If
                    Next
                Case WM_DRAWCLIPBOARD
                    SendMessage(fpChainedWindowHandle, m.Msg, m.LParam, m.WParam)
                    RaiseEvent clipboardchange()
                Case WM_CHANGECBCHAIN
                    'Send to the next window
                    SendMessage(fpChainedWindowHandle, m.Msg, m.LParam, m.WParam)
                    fpChainedWindowHandle = m.LParam
                Case KeyboardHook.HookKeyMsg
                    If Not m.WParam.ToInt32() = Win32.WM_KEYDOWN Then Exit Select

                    Select Case KeyboardShortcuts.CommandFromHookKeyMessage(m)
                        Case ShortcutCommand.PreviousTab
                            SelectTabRelative(-1)
                        Case ShortcutCommand.NextTab
                            SelectTabRelative(1)
                    End Select
            End Select
        Catch ex As Exception
        End Try

        MyBase.WndProc(m)
    End Sub

    Private Sub ActivateConnection()
        If TypeOf pnlDock.ActiveDocument Is UI.Window.Connection Then
            Dim cW As UI.Window.Connection = pnlDock.ActiveDocument
            If cW.TabController.SelectedTab IsNot Nothing Then
                Dim tab As Magic.Controls.TabPage = cW.TabController.SelectedTab
                Dim ifc As Connection.InterfaceControl = TryCast(tab.Tag, Connection.InterfaceControl)
                ifc.Protocol.Focus()
                TryCast(ifc.FindForm, UI.Window.Connection).RefreshIC()
            End If
        End If
    End Sub

    Private Sub pnlDock_ActiveDocumentChanged(ByVal sender As Object, ByVal e As EventArgs) Handles pnlDock.ActiveDocumentChanged
        ActivateConnection()
        Dim connectionWindow As UI.Window.Connection = TryCast(pnlDock.ActiveDocument, UI.Window.Connection)
        If connectionWindow IsNot Nothing Then connectionWindow.UpdateSelectedConnection()
    End Sub

    Private Sub UpdateWindowTitle()
        If InvokeRequired Then
            Invoke(New MethodInvoker(AddressOf UpdateWindowTitle))
            Return
        End If

        Dim titleBuilder As New StringBuilder(Application.Info.ProductName)
        Const separator As String = " - "

        If IsConnectionsFileLoaded Then
            If UsingSqlServer Then
                titleBuilder.Append(separator)
                titleBuilder.Append(Language.strSQLServer.TrimEnd(":"))
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
            Dim nonConnectionPanelCount As Integer = 0
            For Each document As DockContent In pnlDock.Documents
                If (closingDocument Is Nothing OrElse document IsNot closingDocument) And Not TypeOf document Is UI.Window.Connection Then
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

    Private Sub SelectTabRelative(ByVal relativeIndex As Integer)
        If Not TypeOf pnlDock.ActiveDocument Is UI.Window.Connection Then Return

        Dim connectionWindow As UI.Window.Connection = pnlDock.ActiveDocument
        Dim tabController As Magic.Controls.TabControl = connectionWindow.TabController

        Dim newIndex As Integer = tabController.SelectedIndex + relativeIndex
        While newIndex < 0 Or newIndex >= tabController.TabPages.Count
            If newIndex < 0 Then newIndex = tabController.TabPages.Count + newIndex
            If newIndex >= tabController.TabPages.Count Then newIndex = newIndex - tabController.TabPages.Count
        End While

        tabController.SelectedIndex = newIndex
    End Sub
#End Region

#Region "Screen Stuff"
    Private Sub DisplayChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ResetSysMenuItems()
        AddSysMenuItems()
    End Sub

    Private SysMenSubItems(50) As Integer
    Private Shared Sub ResetSysMenuItems()
        SystemMenu.Reset()
    End Sub

    Private Sub AddSysMenuItems()
        SystemMenu = New Tools.SystemMenu(Me.Handle)
        Dim popMen As IntPtr = SystemMenu.CreatePopupMenuItem()

        For i As Integer = 0 To Screen.AllScreens.Length - 1
            SysMenSubItems(i) = 200 + i
            SystemMenu.AppendMenuItem(popMen, Tools.SystemMenu.Flags.MF_STRING, SysMenSubItems(i), My.Language.strScreen & " " & i + 1)
        Next

        SystemMenu.InsertMenuItem(SystemMenu.SystemMenuHandle, 0, Tools.SystemMenu.Flags.MF_POPUP Or Tools.SystemMenu.Flags.MF_BYPOSITION, popMen, My.Language.strSendTo)
        SystemMenu.InsertMenuItem(SystemMenu.SystemMenuHandle, 1, Tools.SystemMenu.Flags.MF_BYPOSITION Or Tools.SystemMenu.Flags.MF_SEPARATOR, IntPtr.Zero, Nothing)
    End Sub
#End Region
End Class
