Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class frmMain
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
            Me.pnlDock = New WeifenLuo.WinFormsUI.Docking.DockPanel()
            Me.VS2012LightTheme1 = New WeifenLuo.WinFormsUI.Docking.VS2012LightTheme()
            Me.msMain = New System.Windows.Forms.MenuStrip()
            Me.mMenFile = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenFileNewConnection = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenFileNewFolder = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenFileSep1 = New System.Windows.Forms.ToolStripSeparator()
            Me.mMenFileNew = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenFileLoad = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenFileSave = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenFileSaveAs = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenFileSep2 = New System.Windows.Forms.ToolStripSeparator()
            Me.mMenFileDelete = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenFileRename = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenFileDuplicate = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenFileSep3 = New System.Windows.Forms.ToolStripSeparator()
            Me.mMenFileImport = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenFileImportFromFile = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenFileImportFromActiveDirectory = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenFileImportFromPortScan = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenFileExport = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenFileSep4 = New System.Windows.Forms.ToolStripSeparator()
            Me.mMenFileExit = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenView = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenViewAddConnectionPanel = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenViewConnectionPanels = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenViewSep1 = New System.Windows.Forms.ToolStripSeparator()
            Me.mMenViewConnections = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenViewConfig = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenViewErrorsAndInfos = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenViewScreenshotManager = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
            Me.mMenViewJumpTo = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenViewJumpToConnectionsConfig = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenViewJumpToSessionsScreenshots = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenViewJumpToErrorsInfos = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenViewResetLayout = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenViewSep2 = New System.Windows.Forms.ToolStripSeparator()
            Me.mMenViewQuickConnectToolbar = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenViewExtAppsToolbar = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenViewSep3 = New System.Windows.Forms.ToolStripSeparator()
            Me.mMenViewFullscreen = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenTools = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenToolsSSHTransfer = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenToolsUVNCSC = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenToolsExternalApps = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenToolsPortScan = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenToolsSep1 = New System.Windows.Forms.ToolStripSeparator()
            Me.mMenToolsComponentsCheck = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenToolsOptions = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenInfo = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenInfoHelp = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenInfoSep1 = New System.Windows.Forms.ToolStripSeparator()
            Me.mMenInfoWebsite = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenInfoBugReport = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
            Me.mMenInfoAnnouncements = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenToolsUpdate = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenInfoSep2 = New System.Windows.Forms.ToolStripSeparator()
            Me.mMenInfoAbout = New System.Windows.Forms.ToolStripMenuItem()
            Me.mMenSep3 = New System.Windows.Forms.ToolStripSeparator()
            Me.lblQuickConnect = New System.Windows.Forms.ToolStripLabel()
            Me.cmbQuickConnect = New mRemote3G.Controls.QuickConnectComboBox()
            Me.tsContainer = New System.Windows.Forms.ToolStripContainer()
            Me.tsExternalTools = New System.Windows.Forms.ToolStrip()
            Me.cMenExtAppsToolbar = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.cMenToolbarShowText = New System.Windows.Forms.ToolStripMenuItem()
            Me.tsQuickConnect = New System.Windows.Forms.ToolStrip()
            Me.btnQuickConnect = New mRemote3G.Controls.ToolStripSplitButton()
            Me.mnuQuickConnectProtocol = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.btnConnections = New System.Windows.Forms.ToolStripDropDownButton()
            Me.mnuConnections = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
            Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripButton3 = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripSplitButton1 = New System.Windows.Forms.ToolStripDropDownButton()
            Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
            Me.tmrAutoSave = New System.Windows.Forms.Timer(Me.components)
            Me.msMain.SuspendLayout()
            Me.tsContainer.ContentPanel.SuspendLayout()
            Me.tsContainer.TopToolStripPanel.SuspendLayout()
            Me.tsContainer.SuspendLayout()
            Me.cMenExtAppsToolbar.SuspendLayout()
            Me.tsQuickConnect.SuspendLayout()
            Me.ToolStrip1.SuspendLayout()
            Me.SuspendLayout()
            '
            'pnlDock
            '
            Me.pnlDock.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlDock.DockBackColor = System.Drawing.SystemColors.Control
            Me.pnlDock.DockLeftPortion = 230.0R
            Me.pnlDock.DockRightPortion = 230.0R
            Me.pnlDock.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingSdi
            Me.pnlDock.Location = New System.Drawing.Point(0, 0)
            Me.pnlDock.Name = "pnlDock"
            Me.pnlDock.Size = New System.Drawing.Size(842, 470)
            Me.pnlDock.SupportDeeplyNestedContent = True
            Me.pnlDock.TabIndex = 13
            Me.pnlDock.Theme = Me.VS2012LightTheme1
            '
            'msMain
            '
            Me.msMain.Dock = System.Windows.Forms.DockStyle.None
            Me.msMain.GripMargin = New System.Windows.Forms.Padding(0)
            Me.msMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible
            Me.msMain.ImageScalingSize = New System.Drawing.Size(20, 20)
            Me.msMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mMenFile, Me.mMenView, Me.mMenTools, Me.mMenInfo})
            Me.msMain.Location = New System.Drawing.Point(4, 0)
            Me.msMain.Name = "msMain"
            Me.msMain.Padding = New System.Windows.Forms.Padding(2, 2, 0, 2)
            Me.msMain.Size = New System.Drawing.Size(215, 28)
            Me.msMain.Stretch = False
            Me.msMain.TabIndex = 16
            Me.msMain.Text = "Main Toolbar"
            '
            'mMenFile
            '
            Me.mMenFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mMenFileNewConnection, Me.mMenFileNewFolder, Me.mMenFileSep1, Me.mMenFileNew, Me.mMenFileLoad, Me.mMenFileSave, Me.mMenFileSaveAs, Me.mMenFileSep2, Me.mMenFileDelete, Me.mMenFileRename, Me.mMenFileDuplicate, Me.mMenFileSep3, Me.mMenFileImport, Me.mMenFileExport, Me.mMenFileSep4, Me.mMenFileExit})
            Me.mMenFile.Name = "mMenFile"
            Me.mMenFile.Size = New System.Drawing.Size(44, 24)
            Me.mMenFile.Text = "&File"
            '
            'mMenFileNewConnection
            '
            Me.mMenFileNewConnection.Image = CType(resources.GetObject("mMenFileNewConnection.Image"), System.Drawing.Image)
            Me.mMenFileNewConnection.Name = "mMenFileNewConnection"
            Me.mMenFileNewConnection.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
            Me.mMenFileNewConnection.Size = New System.Drawing.Size(340, 26)
            Me.mMenFileNewConnection.Text = "New Connection"
            '
            'mMenFileNewFolder
            '
            Me.mMenFileNewFolder.Image = CType(resources.GetObject("mMenFileNewFolder.Image"), System.Drawing.Image)
            Me.mMenFileNewFolder.Name = "mMenFileNewFolder"
            Me.mMenFileNewFolder.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
            Me.mMenFileNewFolder.Size = New System.Drawing.Size(340, 26)
            Me.mMenFileNewFolder.Text = "New Folder"
            '
            'mMenFileSep1
            '
            Me.mMenFileSep1.Name = "mMenFileSep1"
            Me.mMenFileSep1.Size = New System.Drawing.Size(337, 6)
            '
            'mMenFileNew
            '
            Me.mMenFileNew.Image = CType(resources.GetObject("mMenFileNew.Image"), System.Drawing.Image)
            Me.mMenFileNew.Name = "mMenFileNew"
            Me.mMenFileNew.Size = New System.Drawing.Size(340, 26)
            Me.mMenFileNew.Text = "New Connection File"
            '
            'mMenFileLoad
            '
            Me.mMenFileLoad.Image = CType(resources.GetObject("mMenFileLoad.Image"), System.Drawing.Image)
            Me.mMenFileLoad.Name = "mMenFileLoad"
            Me.mMenFileLoad.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
            Me.mMenFileLoad.Size = New System.Drawing.Size(340, 26)
            Me.mMenFileLoad.Text = "Open Connection File..."
            '
            'mMenFileSave
            '
            Me.mMenFileSave.Image = CType(resources.GetObject("mMenFileSave.Image"), System.Drawing.Image)
            Me.mMenFileSave.Name = "mMenFileSave"
            Me.mMenFileSave.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
            Me.mMenFileSave.Size = New System.Drawing.Size(340, 26)
            Me.mMenFileSave.Text = "Save Connection File"
            '
            'mMenFileSaveAs
            '
            Me.mMenFileSaveAs.Image = CType(resources.GetObject("mMenFileSaveAs.Image"), System.Drawing.Image)
            Me.mMenFileSaveAs.Name = "mMenFileSaveAs"
            Me.mMenFileSaveAs.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
            Me.mMenFileSaveAs.Size = New System.Drawing.Size(340, 26)
            Me.mMenFileSaveAs.Text = "Save Connection File As..."
            '
            'mMenFileSep2
            '
            Me.mMenFileSep2.Name = "mMenFileSep2"
            Me.mMenFileSep2.Size = New System.Drawing.Size(337, 6)
            '
            'mMenFileDelete
            '
            Me.mMenFileDelete.Image = CType(resources.GetObject("mMenFileDelete.Image"), System.Drawing.Image)
            Me.mMenFileDelete.Name = "mMenFileDelete"
            Me.mMenFileDelete.Size = New System.Drawing.Size(340, 26)
            Me.mMenFileDelete.Text = "Delete..."
            '
            'mMenFileRename
            '
            Me.mMenFileRename.Image = CType(resources.GetObject("mMenFileRename.Image"), System.Drawing.Image)
            Me.mMenFileRename.Name = "mMenFileRename"
            Me.mMenFileRename.Size = New System.Drawing.Size(340, 26)
            Me.mMenFileRename.Text = "Rename"
            '
            'mMenFileDuplicate
            '
            Me.mMenFileDuplicate.Image = CType(resources.GetObject("mMenFileDuplicate.Image"), System.Drawing.Image)
            Me.mMenFileDuplicate.Name = "mMenFileDuplicate"
            Me.mMenFileDuplicate.Size = New System.Drawing.Size(340, 26)
            Me.mMenFileDuplicate.Text = "Duplicate"
            '
            'mMenFileSep3
            '
            Me.mMenFileSep3.Name = "mMenFileSep3"
            Me.mMenFileSep3.Size = New System.Drawing.Size(337, 6)
            '
            'mMenFileImport
            '
            Me.mMenFileImport.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mMenFileImportFromFile, Me.mMenFileImportFromActiveDirectory, Me.mMenFileImportFromPortScan})
            Me.mMenFileImport.Name = "mMenFileImport"
            Me.mMenFileImport.Size = New System.Drawing.Size(340, 26)
            Me.mMenFileImport.Text = "&Import"
            '
            'mMenFileImportFromFile
            '
            Me.mMenFileImportFromFile.Name = "mMenFileImportFromFile"
            Me.mMenFileImportFromFile.Size = New System.Drawing.Size(284, 26)
            Me.mMenFileImportFromFile.Text = "Import from &File..."
            '
            'mMenFileImportFromActiveDirectory
            '
            Me.mMenFileImportFromActiveDirectory.Name = "mMenFileImportFromActiveDirectory"
            Me.mMenFileImportFromActiveDirectory.Size = New System.Drawing.Size(284, 26)
            Me.mMenFileImportFromActiveDirectory.Text = "Import from &Active Directory..."
            '
            'mMenFileImportFromPortScan
            '
            Me.mMenFileImportFromPortScan.Name = "mMenFileImportFromPortScan"
            Me.mMenFileImportFromPortScan.Size = New System.Drawing.Size(284, 26)
            Me.mMenFileImportFromPortScan.Text = "Import from &Port Scan..."
            '
            'mMenFileExport
            '
            Me.mMenFileExport.Name = "mMenFileExport"
            Me.mMenFileExport.Size = New System.Drawing.Size(340, 26)
            Me.mMenFileExport.Text = "&Export to File..."
            '
            'mMenFileSep4
            '
            Me.mMenFileSep4.Name = "mMenFileSep4"
            Me.mMenFileSep4.Size = New System.Drawing.Size(337, 6)
            '
            'mMenFileExit
            '
            Me.mMenFileExit.Image = CType(resources.GetObject("mMenFileExit.Image"), System.Drawing.Image)
            Me.mMenFileExit.Name = "mMenFileExit"
            Me.mMenFileExit.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F4), System.Windows.Forms.Keys)
            Me.mMenFileExit.Size = New System.Drawing.Size(340, 26)
            Me.mMenFileExit.Text = "Exit"
            '
            'mMenView
            '
            Me.mMenView.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mMenViewAddConnectionPanel, Me.mMenViewConnectionPanels, Me.mMenViewSep1, Me.mMenViewConnections, Me.mMenViewConfig, Me.mMenViewErrorsAndInfos, Me.mMenViewScreenshotManager, Me.ToolStripSeparator1, Me.mMenViewJumpTo, Me.mMenViewResetLayout, Me.mMenViewSep2, Me.mMenViewQuickConnectToolbar, Me.mMenViewExtAppsToolbar, Me.mMenViewSep3, Me.mMenViewFullscreen})
            Me.mMenView.Name = "mMenView"
            Me.mMenView.Size = New System.Drawing.Size(53, 24)
            Me.mMenView.Text = "&View"
            '
            'mMenViewAddConnectionPanel
            '
            Me.mMenViewAddConnectionPanel.Image = CType(resources.GetObject("mMenViewAddConnectionPanel.Image"), System.Drawing.Image)
            Me.mMenViewAddConnectionPanel.Name = "mMenViewAddConnectionPanel"
            Me.mMenViewAddConnectionPanel.Size = New System.Drawing.Size(279, 26)
            Me.mMenViewAddConnectionPanel.Text = "Add Connection Panel"
            '
            'mMenViewConnectionPanels
            '
            Me.mMenViewConnectionPanels.Image = CType(resources.GetObject("mMenViewConnectionPanels.Image"), System.Drawing.Image)
            Me.mMenViewConnectionPanels.Name = "mMenViewConnectionPanels"
            Me.mMenViewConnectionPanels.Size = New System.Drawing.Size(279, 26)
            Me.mMenViewConnectionPanels.Text = "Connection Panels"
            '
            'mMenViewSep1
            '
            Me.mMenViewSep1.Name = "mMenViewSep1"
            Me.mMenViewSep1.Size = New System.Drawing.Size(276, 6)
            '
            'mMenViewConnections
            '
            Me.mMenViewConnections.Checked = True
            Me.mMenViewConnections.CheckState = System.Windows.Forms.CheckState.Checked
            Me.mMenViewConnections.Image = CType(resources.GetObject("mMenViewConnections.Image"), System.Drawing.Image)
            Me.mMenViewConnections.Name = "mMenViewConnections"
            Me.mMenViewConnections.Size = New System.Drawing.Size(279, 26)
            Me.mMenViewConnections.Text = "Connections"
            '
            'mMenViewConfig
            '
            Me.mMenViewConfig.Checked = True
            Me.mMenViewConfig.CheckState = System.Windows.Forms.CheckState.Checked
            Me.mMenViewConfig.Image = CType(resources.GetObject("mMenViewConfig.Image"), System.Drawing.Image)
            Me.mMenViewConfig.Name = "mMenViewConfig"
            Me.mMenViewConfig.Size = New System.Drawing.Size(279, 26)
            Me.mMenViewConfig.Text = "Config"
            '
            'mMenViewErrorsAndInfos
            '
            Me.mMenViewErrorsAndInfos.Checked = True
            Me.mMenViewErrorsAndInfos.CheckState = System.Windows.Forms.CheckState.Checked
            Me.mMenViewErrorsAndInfos.Image = CType(resources.GetObject("mMenViewErrorsAndInfos.Image"), System.Drawing.Image)
            Me.mMenViewErrorsAndInfos.Name = "mMenViewErrorsAndInfos"
            Me.mMenViewErrorsAndInfos.Size = New System.Drawing.Size(279, 26)
            Me.mMenViewErrorsAndInfos.Text = "Errors and Infos"
            '
            'mMenViewScreenshotManager
            '
            Me.mMenViewScreenshotManager.Image = CType(resources.GetObject("mMenViewScreenshotManager.Image"), System.Drawing.Image)
            Me.mMenViewScreenshotManager.Name = "mMenViewScreenshotManager"
            Me.mMenViewScreenshotManager.Size = New System.Drawing.Size(279, 26)
            Me.mMenViewScreenshotManager.Text = "Screenshot Manager"
            '
            'ToolStripSeparator1
            '
            Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
            Me.ToolStripSeparator1.Size = New System.Drawing.Size(276, 6)
            '
            'mMenViewJumpTo
            '
            Me.mMenViewJumpTo.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mMenViewJumpToConnectionsConfig, Me.mMenViewJumpToSessionsScreenshots, Me.mMenViewJumpToErrorsInfos})
            Me.mMenViewJumpTo.Image = CType(resources.GetObject("mMenViewJumpTo.Image"), System.Drawing.Image)
            Me.mMenViewJumpTo.Name = "mMenViewJumpTo"
            Me.mMenViewJumpTo.Size = New System.Drawing.Size(279, 26)
            Me.mMenViewJumpTo.Text = "Jump To"
            '
            'mMenViewJumpToConnectionsConfig
            '
            Me.mMenViewJumpToConnectionsConfig.Image = CType(resources.GetObject("mMenViewJumpToConnectionsConfig.Image"), System.Drawing.Image)
            Me.mMenViewJumpToConnectionsConfig.Name = "mMenViewJumpToConnectionsConfig"
            Me.mMenViewJumpToConnectionsConfig.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
            Me.mMenViewJumpToConnectionsConfig.Size = New System.Drawing.Size(316, 26)
            Me.mMenViewJumpToConnectionsConfig.Text = "Connections && Config"
            '
            'mMenViewJumpToSessionsScreenshots
            '
            Me.mMenViewJumpToSessionsScreenshots.Image = CType(resources.GetObject("mMenViewJumpToSessionsScreenshots.Image"), System.Drawing.Image)
            Me.mMenViewJumpToSessionsScreenshots.Name = "mMenViewJumpToSessionsScreenshots"
            Me.mMenViewJumpToSessionsScreenshots.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
            Me.mMenViewJumpToSessionsScreenshots.Size = New System.Drawing.Size(316, 26)
            Me.mMenViewJumpToSessionsScreenshots.Text = "Sessions && Screenshots"
            '
            'mMenViewJumpToErrorsInfos
            '
            Me.mMenViewJumpToErrorsInfos.Image = CType(resources.GetObject("mMenViewJumpToErrorsInfos.Image"), System.Drawing.Image)
            Me.mMenViewJumpToErrorsInfos.Name = "mMenViewJumpToErrorsInfos"
            Me.mMenViewJumpToErrorsInfos.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
            Me.mMenViewJumpToErrorsInfos.Size = New System.Drawing.Size(316, 26)
            Me.mMenViewJumpToErrorsInfos.Text = "Errors && Infos"
            '
            'mMenViewResetLayout
            '
            Me.mMenViewResetLayout.Image = CType(resources.GetObject("mMenViewResetLayout.Image"), System.Drawing.Image)
            Me.mMenViewResetLayout.Name = "mMenViewResetLayout"
            Me.mMenViewResetLayout.Size = New System.Drawing.Size(279, 26)
            Me.mMenViewResetLayout.Text = "Reset Layout"
            '
            'mMenViewSep2
            '
            Me.mMenViewSep2.Name = "mMenViewSep2"
            Me.mMenViewSep2.Size = New System.Drawing.Size(276, 6)
            '
            'mMenViewQuickConnectToolbar
            '
            Me.mMenViewQuickConnectToolbar.Image = CType(resources.GetObject("mMenViewQuickConnectToolbar.Image"), System.Drawing.Image)
            Me.mMenViewQuickConnectToolbar.Name = "mMenViewQuickConnectToolbar"
            Me.mMenViewQuickConnectToolbar.Size = New System.Drawing.Size(279, 26)
            Me.mMenViewQuickConnectToolbar.Text = "Quick Connect Toolbar"
            '
            'mMenViewExtAppsToolbar
            '
            Me.mMenViewExtAppsToolbar.Image = CType(resources.GetObject("mMenViewExtAppsToolbar.Image"), System.Drawing.Image)
            Me.mMenViewExtAppsToolbar.Name = "mMenViewExtAppsToolbar"
            Me.mMenViewExtAppsToolbar.Size = New System.Drawing.Size(279, 26)
            Me.mMenViewExtAppsToolbar.Text = "External Applications Toolbar"
            '
            'mMenViewSep3
            '
            Me.mMenViewSep3.Name = "mMenViewSep3"
            Me.mMenViewSep3.Size = New System.Drawing.Size(276, 6)
            '
            'mMenViewFullscreen
            '
            Me.mMenViewFullscreen.Image = CType(resources.GetObject("mMenViewFullscreen.Image"), System.Drawing.Image)
            Me.mMenViewFullscreen.Name = "mMenViewFullscreen"
            Me.mMenViewFullscreen.ShortcutKeys = System.Windows.Forms.Keys.F11
            Me.mMenViewFullscreen.Size = New System.Drawing.Size(279, 26)
            Me.mMenViewFullscreen.Text = "Full Screen"
            '
            'mMenTools
            '
            Me.mMenTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mMenToolsSSHTransfer, Me.mMenToolsUVNCSC, Me.mMenToolsExternalApps, Me.mMenToolsPortScan, Me.mMenToolsSep1, Me.mMenToolsComponentsCheck, Me.mMenToolsOptions})
            Me.mMenTools.Name = "mMenTools"
            Me.mMenTools.Size = New System.Drawing.Size(56, 24)
            Me.mMenTools.Text = "&Tools"
            '
            'mMenToolsSSHTransfer
            '
            Me.mMenToolsSSHTransfer.Image = CType(resources.GetObject("mMenToolsSSHTransfer.Image"), System.Drawing.Image)
            Me.mMenToolsSSHTransfer.Name = "mMenToolsSSHTransfer"
            Me.mMenToolsSSHTransfer.Size = New System.Drawing.Size(221, 26)
            Me.mMenToolsSSHTransfer.Text = "SSH File Transfer"
            '
            'mMenToolsUVNCSC
            '
            Me.mMenToolsUVNCSC.Image = CType(resources.GetObject("mMenToolsUVNCSC.Image"), System.Drawing.Image)
            Me.mMenToolsUVNCSC.Name = "mMenToolsUVNCSC"
            Me.mMenToolsUVNCSC.Size = New System.Drawing.Size(221, 26)
            Me.mMenToolsUVNCSC.Text = "UltraVNC SingleClick"
            Me.mMenToolsUVNCSC.Visible = False
            '
            'mMenToolsExternalApps
            '
            Me.mMenToolsExternalApps.Image = CType(resources.GetObject("mMenToolsExternalApps.Image"), System.Drawing.Image)
            Me.mMenToolsExternalApps.Name = "mMenToolsExternalApps"
            Me.mMenToolsExternalApps.Size = New System.Drawing.Size(221, 26)
            Me.mMenToolsExternalApps.Text = "External Tools"
            '
            'mMenToolsPortScan
            '
            Me.mMenToolsPortScan.Image = CType(resources.GetObject("mMenToolsPortScan.Image"), System.Drawing.Image)
            Me.mMenToolsPortScan.Name = "mMenToolsPortScan"
            Me.mMenToolsPortScan.Size = New System.Drawing.Size(221, 26)
            Me.mMenToolsPortScan.Text = "Port Scan"
            '
            'mMenToolsSep1
            '
            Me.mMenToolsSep1.Name = "mMenToolsSep1"
            Me.mMenToolsSep1.Size = New System.Drawing.Size(218, 6)
            '
            'mMenToolsComponentsCheck
            '
            Me.mMenToolsComponentsCheck.Image = CType(resources.GetObject("mMenToolsComponentsCheck.Image"), System.Drawing.Image)
            Me.mMenToolsComponentsCheck.Name = "mMenToolsComponentsCheck"
            Me.mMenToolsComponentsCheck.Size = New System.Drawing.Size(221, 26)
            Me.mMenToolsComponentsCheck.Text = "Components Check"
            '
            'mMenToolsOptions
            '
            Me.mMenToolsOptions.Image = CType(resources.GetObject("mMenToolsOptions.Image"), System.Drawing.Image)
            Me.mMenToolsOptions.Name = "mMenToolsOptions"
            Me.mMenToolsOptions.Size = New System.Drawing.Size(221, 26)
            Me.mMenToolsOptions.Text = "Options"
            '
            'mMenInfo
            '
            Me.mMenInfo.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mMenInfoHelp, Me.mMenInfoSep1, Me.mMenInfoWebsite, Me.mMenInfoBugReport, Me.ToolStripSeparator2, Me.mMenInfoAnnouncements, Me.mMenToolsUpdate, Me.mMenInfoSep2, Me.mMenInfoAbout})
            Me.mMenInfo.Name = "mMenInfo"
            Me.mMenInfo.Size = New System.Drawing.Size(53, 24)
            Me.mMenInfo.Text = "&Help"
            Me.mMenInfo.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal
            '
            'mMenInfoHelp
            '
            Me.mMenInfoHelp.Image = CType(resources.GetObject("mMenInfoHelp.Image"), System.Drawing.Image)
            Me.mMenInfoHelp.Name = "mMenInfoHelp"
            Me.mMenInfoHelp.ShortcutKeys = System.Windows.Forms.Keys.F1
            Me.mMenInfoHelp.Size = New System.Drawing.Size(230, 26)
            Me.mMenInfoHelp.Text = "mRemoteNG Help"
            '
            'mMenInfoSep1
            '
            Me.mMenInfoSep1.Name = "mMenInfoSep1"
            Me.mMenInfoSep1.Size = New System.Drawing.Size(227, 6)
            '
            'mMenInfoWebsite
            '
            Me.mMenInfoWebsite.Image = CType(resources.GetObject("mMenInfoWebsite.Image"), System.Drawing.Image)
            Me.mMenInfoWebsite.Name = "mMenInfoWebsite"
            Me.mMenInfoWebsite.Size = New System.Drawing.Size(230, 26)
            Me.mMenInfoWebsite.Text = "Website"
            '
            'mMenInfoBugReport
            '
            Me.mMenInfoBugReport.Image = CType(resources.GetObject("mMenInfoBugReport.Image"), System.Drawing.Image)
            Me.mMenInfoBugReport.Name = "mMenInfoBugReport"
            Me.mMenInfoBugReport.Size = New System.Drawing.Size(230, 26)
            Me.mMenInfoBugReport.Text = "Report a Bug"
            '
            'ToolStripSeparator2
            '
            Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
            Me.ToolStripSeparator2.Size = New System.Drawing.Size(227, 6)
            '
            'mMenInfoAnnouncements
            '
            Me.mMenInfoAnnouncements.Image = CType(resources.GetObject("mMenInfoAnnouncements.Image"), System.Drawing.Image)
            Me.mMenInfoAnnouncements.Name = "mMenInfoAnnouncements"
            Me.mMenInfoAnnouncements.Size = New System.Drawing.Size(230, 26)
            Me.mMenInfoAnnouncements.Text = "Announcements"
            '
            'mMenToolsUpdate
            '
            Me.mMenToolsUpdate.Image = CType(resources.GetObject("mMenToolsUpdate.Image"), System.Drawing.Image)
            Me.mMenToolsUpdate.Name = "mMenToolsUpdate"
            Me.mMenToolsUpdate.Size = New System.Drawing.Size(230, 26)
            Me.mMenToolsUpdate.Text = "Check for Updates"
            '
            'mMenInfoSep2
            '
            Me.mMenInfoSep2.Name = "mMenInfoSep2"
            Me.mMenInfoSep2.Size = New System.Drawing.Size(227, 6)
            '
            'mMenInfoAbout
            '
            Me.mMenInfoAbout.Image = CType(resources.GetObject("mMenInfoAbout.Image"), System.Drawing.Image)
            Me.mMenInfoAbout.Name = "mMenInfoAbout"
            Me.mMenInfoAbout.Size = New System.Drawing.Size(230, 26)
            Me.mMenInfoAbout.Text = "About mRemote3G"
            '
            'mMenSep3
            '
            Me.mMenSep3.Name = "mMenSep3"
            Me.mMenSep3.Size = New System.Drawing.Size(211, 6)
            '
            'lblQuickConnect
            '
            Me.lblQuickConnect.Name = "lblQuickConnect"
            Me.lblQuickConnect.Size = New System.Drawing.Size(66, 22)
            Me.lblQuickConnect.Text = "&Connect:"
            '
            'cmbQuickConnect
            '
            Me.cmbQuickConnect.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
            Me.cmbQuickConnect.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
            Me.cmbQuickConnect.FlatStyle = System.Windows.Forms.FlatStyle.System
            Me.cmbQuickConnect.Margin = New System.Windows.Forms.Padding(1, 0, 3, 0)
            Me.cmbQuickConnect.Name = "cmbQuickConnect"
            Me.cmbQuickConnect.Size = New System.Drawing.Size(200, 25)
            '
            'tsContainer
            '
            '
            'tsContainer.BottomToolStripPanel
            '
            Me.tsContainer.BottomToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
            '
            'tsContainer.ContentPanel
            '
            Me.tsContainer.ContentPanel.Controls.Add(Me.pnlDock)
            Me.tsContainer.ContentPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
            Me.tsContainer.ContentPanel.Size = New System.Drawing.Size(842, 470)
            Me.tsContainer.Dock = System.Windows.Forms.DockStyle.Fill
            '
            'tsContainer.LeftToolStripPanel
            '
            Me.tsContainer.LeftToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
            Me.tsContainer.Location = New System.Drawing.Point(0, 0)
            Me.tsContainer.Name = "tsContainer"
            '
            'tsContainer.RightToolStripPanel
            '
            Me.tsContainer.RightToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
            Me.tsContainer.Size = New System.Drawing.Size(842, 523)
            Me.tsContainer.TabIndex = 17
            Me.tsContainer.Text = "ToolStripContainer1"
            '
            'tsContainer.TopToolStripPanel
            '
            Me.tsContainer.TopToolStripPanel.Controls.Add(Me.tsExternalTools)
            Me.tsContainer.TopToolStripPanel.Controls.Add(Me.tsQuickConnect)
            Me.tsContainer.TopToolStripPanel.Controls.Add(Me.msMain)
            Me.tsContainer.TopToolStripPanel.Controls.Add(Me.ToolStrip1)
            Me.tsContainer.TopToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
            '
            'tsExternalTools
            '
            Me.tsExternalTools.ContextMenuStrip = Me.cMenExtAppsToolbar
            Me.tsExternalTools.Dock = System.Windows.Forms.DockStyle.None
            Me.tsExternalTools.ImageScalingSize = New System.Drawing.Size(20, 20)
            Me.tsExternalTools.Location = New System.Drawing.Point(228, 0)
            Me.tsExternalTools.MaximumSize = New System.Drawing.Size(0, 25)
            Me.tsExternalTools.Name = "tsExternalTools"
            Me.tsExternalTools.Size = New System.Drawing.Size(111, 25)
            Me.tsExternalTools.TabIndex = 17
            '
            'cMenExtAppsToolbar
            '
            Me.cMenExtAppsToolbar.ImageScalingSize = New System.Drawing.Size(20, 20)
            Me.cMenExtAppsToolbar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cMenToolbarShowText})
            Me.cMenExtAppsToolbar.Name = "cMenToolbar"
            Me.cMenExtAppsToolbar.Size = New System.Drawing.Size(152, 30)
            '
            'cMenToolbarShowText
            '
            Me.cMenToolbarShowText.Checked = True
            Me.cMenToolbarShowText.CheckState = System.Windows.Forms.CheckState.Checked
            Me.cMenToolbarShowText.Name = "cMenToolbarShowText"
            Me.cMenToolbarShowText.Size = New System.Drawing.Size(151, 26)
            Me.cMenToolbarShowText.Text = "Show Text"
            '
            'tsQuickConnect
            '
            Me.tsQuickConnect.Dock = System.Windows.Forms.DockStyle.None
            Me.tsQuickConnect.ImageScalingSize = New System.Drawing.Size(20, 20)
            Me.tsQuickConnect.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblQuickConnect, Me.cmbQuickConnect, Me.btnQuickConnect, Me.btnConnections})
            Me.tsQuickConnect.Location = New System.Drawing.Point(3, 28)
            Me.tsQuickConnect.MaximumSize = New System.Drawing.Size(0, 25)
            Me.tsQuickConnect.Name = "tsQuickConnect"
            Me.tsQuickConnect.Size = New System.Drawing.Size(417, 25)
            Me.tsQuickConnect.TabIndex = 18
            '
            'btnQuickConnect
            '
            Me.btnQuickConnect.DropDown = Me.mnuQuickConnectProtocol
            Me.btnQuickConnect.Image = CType(resources.GetObject("btnQuickConnect.Image"), System.Drawing.Image)
            Me.btnQuickConnect.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.btnQuickConnect.Margin = New System.Windows.Forms.Padding(0, 1, 3, 2)
            Me.btnQuickConnect.Name = "btnQuickConnect"
            Me.btnQuickConnect.Size = New System.Drawing.Size(102, 22)
            Me.btnQuickConnect.Text = "Connect"
            '
            'mnuQuickConnectProtocol
            '
            Me.mnuQuickConnectProtocol.ImageScalingSize = New System.Drawing.Size(20, 20)
            Me.mnuQuickConnectProtocol.Name = "mnuQuickConnectProtocol"
            Me.mnuQuickConnectProtocol.ShowCheckMargin = True
            Me.mnuQuickConnectProtocol.ShowImageMargin = False
            Me.mnuQuickConnectProtocol.Size = New System.Drawing.Size(67, 4)
            '
            'btnConnections
            '
            Me.btnConnections.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.btnConnections.DropDown = Me.mnuConnections
            Me.btnConnections.Image = CType(resources.GetObject("btnConnections.Image"), System.Drawing.Image)
            Me.btnConnections.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
            Me.btnConnections.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.btnConnections.Name = "btnConnections"
            Me.btnConnections.Size = New System.Drawing.Size(30, 22)
            Me.btnConnections.Text = "Connections"
            '
            'mnuConnections
            '
            Me.mnuConnections.ImageScalingSize = New System.Drawing.Size(20, 20)
            Me.mnuConnections.Name = "mnuConnections"
            Me.mnuConnections.Size = New System.Drawing.Size(67, 4)
            '
            'ToolStrip1
            '
            Me.ToolStrip1.Dock = System.Windows.Forms.DockStyle.None
            Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
            Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1, Me.ToolStripButton2, Me.ToolStripButton3, Me.ToolStripSplitButton1})
            Me.ToolStrip1.Location = New System.Drawing.Point(3, 28)
            Me.ToolStrip1.MaximumSize = New System.Drawing.Size(0, 25)
            Me.ToolStrip1.Name = "ToolStrip1"
            Me.ToolStrip1.Size = New System.Drawing.Size(279, 25)
            Me.ToolStrip1.TabIndex = 19
            Me.ToolStrip1.Visible = False
            '
            'ToolStripButton1
            '
            Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.ToolStripButton1.Name = "ToolStripButton1"
            Me.ToolStripButton1.Size = New System.Drawing.Size(67, 22)
            Me.ToolStripButton1.Text = "Connect"
            '
            'ToolStripButton2
            '
            Me.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.ToolStripButton2.Name = "ToolStripButton2"
            Me.ToolStripButton2.Size = New System.Drawing.Size(85, 22)
            Me.ToolStripButton2.Text = "Screenshot"
            '
            'ToolStripButton3
            '
            Me.ToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.ToolStripButton3.Name = "ToolStripButton3"
            Me.ToolStripButton3.Size = New System.Drawing.Size(62, 22)
            Me.ToolStripButton3.Text = "Refresh"
            '
            'ToolStripSplitButton1
            '
            Me.ToolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.ToolStripSplitButton1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1, Me.ToolStripMenuItem2})
            Me.ToolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.ToolStripSplitButton1.Name = "ToolStripSplitButton1"
            Me.ToolStripSplitButton1.Size = New System.Drawing.Size(14, 22)
            Me.ToolStripSplitButton1.Text = "Special Keys"
            '
            'ToolStripMenuItem1
            '
            Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
            Me.ToolStripMenuItem1.Size = New System.Drawing.Size(161, 26)
            Me.ToolStripMenuItem1.Text = "Ctrl-Alt-Del"
            '
            'ToolStripMenuItem2
            '
            Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
            Me.ToolStripMenuItem2.Size = New System.Drawing.Size(161, 26)
            Me.ToolStripMenuItem2.Text = "Ctrl-Esc"
            '
            'tmrAutoSave
            '
            Me.tmrAutoSave.Interval = 10000
            '
            'frmMain
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(842, 523)
            Me.Controls.Add(Me.tsContainer)
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MainMenuStrip = Me.msMain
            Me.Name = "frmMain"
            Me.Opacity = 0R
            Me.Text = "mRemote3G"
            Me.msMain.ResumeLayout(False)
            Me.msMain.PerformLayout()
            Me.tsContainer.ContentPanel.ResumeLayout(False)
            Me.tsContainer.TopToolStripPanel.ResumeLayout(False)
            Me.tsContainer.TopToolStripPanel.PerformLayout()
            Me.tsContainer.ResumeLayout(False)
            Me.tsContainer.PerformLayout()
            Me.cMenExtAppsToolbar.ResumeLayout(False)
            Me.tsQuickConnect.ResumeLayout(False)
            Me.tsQuickConnect.PerformLayout()
            Me.ToolStrip1.ResumeLayout(False)
            Me.ToolStrip1.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents pnlDock As WeifenLuo.WinFormsUI.Docking.DockPanel
        Friend WithEvents msMain As System.Windows.Forms.MenuStrip
        Friend WithEvents mMenFile As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenView As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenTools As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents lblQuickConnect As System.Windows.Forms.ToolStripLabel
        Friend WithEvents mMenInfo As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenFileNew As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenFileLoad As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenFileSave As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenFileSaveAs As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenFileSep1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents mMenFileExit As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenToolsSep1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents mMenToolsOptions As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenInfoHelp As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenInfoWebsite As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenInfoSep1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents mMenInfoAbout As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenViewConnectionPanels As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenViewSep1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents mMenViewConnections As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenViewConfig As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenViewErrorsAndInfos As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenViewScreenshotManager As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenViewAddConnectionPanel As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents cmbQuickConnect As Controls.QuickConnectComboBox
        Friend WithEvents mMenViewSep2 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents mMenViewFullscreen As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenToolsSSHTransfer As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsContainer As System.Windows.Forms.ToolStripContainer
        Friend WithEvents mMenToolsExternalApps As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tmrAutoSave As System.Windows.Forms.Timer
        Friend WithEvents tsExternalTools As System.Windows.Forms.ToolStrip
        Friend WithEvents mMenViewExtAppsToolbar As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents cMenExtAppsToolbar As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents cMenToolbarShowText As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenToolsPortScan As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsQuickConnect As System.Windows.Forms.ToolStrip
        Friend WithEvents mMenViewQuickConnectToolbar As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenSep3 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents mMenViewSep3 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents btnQuickConnect As Controls.ToolStripSplitButton
        Friend WithEvents mMenViewJumpTo As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenViewJumpToConnectionsConfig As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenViewJumpToSessionsScreenshots As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenViewJumpToErrorsInfos As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents mMenToolsUVNCSC As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenToolsComponentsCheck As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenInfoAnnouncements As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenInfoSep2 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents mMenInfoBugReport As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents mMenToolsUpdate As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenViewResetLayout As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenFileDuplicate As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenFileSep2 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents mMenFileNewConnection As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenFileNewFolder As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenFileSep3 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents mMenFileDelete As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenFileRename As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenFileSep4 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
        Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
        Friend WithEvents ToolStripButton2 As System.Windows.Forms.ToolStripButton
        Friend WithEvents ToolStripButton3 As System.Windows.Forms.ToolStripButton
        Friend WithEvents ToolStripSplitButton1 As System.Windows.Forms.ToolStripDropDownButton
        Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mnuQuickConnectProtocol As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents btnConnections As System.Windows.Forms.ToolStripDropDownButton
        Friend WithEvents mnuConnections As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents mMenFileExport As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenFileImportFromFile As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenFileImportFromActiveDirectory As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenFileImportFromPortScan As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mMenFileImport As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents VS2012LightTheme1 As WeifenLuo.WinFormsUI.Docking.VS2012LightTheme
    End Class
End Namespace