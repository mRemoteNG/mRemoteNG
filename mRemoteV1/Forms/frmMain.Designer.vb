<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.pnlDock = New WeifenLuo.WinFormsUI.Docking.DockPanel
        Me.msMain = New System.Windows.Forms.MenuStrip
        Me.mMenFile = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenFileNew = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenFileLoad = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenFileSep1 = New System.Windows.Forms.ToolStripSeparator
        Me.mMenFileSave = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenFileSaveAs = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenFileSep2 = New System.Windows.Forms.ToolStripSeparator
        Me.mMenFileExit = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenView = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenViewAddConnectionPanel = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenViewConnectionPanels = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenViewSep1 = New System.Windows.Forms.ToolStripSeparator
        Me.mMenViewConnections = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenViewConfig = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenViewSessions = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenViewErrorsAndInfos = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenViewScreenshotManager = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.mMenViewJumpTo = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenViewJumpToConnectionsConfig = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenViewJumpToSessionsScreenshots = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenViewJumpToErrorsInfos = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenViewSep2 = New System.Windows.Forms.ToolStripSeparator
        Me.mMenViewQuickConnectToolbar = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenViewExtAppsToolbar = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenViewSep3 = New System.Windows.Forms.ToolStripSeparator
        Me.mMenViewFullscreen = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenTools = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenToolsSSHTransfer = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenToolsUVNCSC = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenToolsExternalApps = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenToolsPortScan = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenToolsSep1 = New System.Windows.Forms.ToolStripSeparator
        Me.mMenToolsUpdate = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenToolsComponentsCheck = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenToolsOptions = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenInfo = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenInfoHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenInfoSep1 = New System.Windows.Forms.ToolStripSeparator
        Me.mMenInfoDonate = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenInfoWebsite = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenInfovRD08 = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenInfoSep2 = New System.Windows.Forms.ToolStripSeparator
        Me.mMenInfoAnnouncments = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenInfoAbout = New System.Windows.Forms.ToolStripMenuItem
        Me.mMenSep3 = New System.Windows.Forms.ToolStripSeparator
        Me.lblQuickConnect = New System.Windows.Forms.ToolStripLabel
        Me.cmbQuickConnect = New System.Windows.Forms.ToolStripComboBox
        Me.tsContainer = New System.Windows.Forms.ToolStripContainer
        Me.tsQuickConnect = New System.Windows.Forms.ToolStrip
        Me.btnQuickyPlay = New System.Windows.Forms.ToolStripSplitButton
        Me.mMenQuickyCon = New System.Windows.Forms.ToolStripMenuItem
        Me.tsExtAppsToolbar = New System.Windows.Forms.ToolStrip
        Me.cMenExtAppsToolbar = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cMenToolbarShowText = New System.Windows.Forms.ToolStripMenuItem
        Me.tmrShowUpdate = New System.Windows.Forms.Timer(Me.components)
        Me.tmrAutoSave = New System.Windows.Forms.Timer(Me.components)
        Me.msMain.SuspendLayout()
        Me.tsContainer.ContentPanel.SuspendLayout()
        Me.tsContainer.TopToolStripPanel.SuspendLayout()
        Me.tsContainer.SuspendLayout()
        Me.tsQuickConnect.SuspendLayout()
        Me.cMenExtAppsToolbar.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlDock
        '
        Me.pnlDock.ActiveAutoHideContent = Nothing
        Me.pnlDock.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlDock.DockLeftPortion = 230
        Me.pnlDock.DockRightPortion = 230
        Me.pnlDock.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow
        Me.pnlDock.Location = New System.Drawing.Point(0, 0)
        Me.pnlDock.Name = "pnlDock"
        Me.pnlDock.Size = New System.Drawing.Size(842, 499)
        Me.pnlDock.TabIndex = 13
        '
        'msMain
        '
        Me.msMain.Dock = System.Windows.Forms.DockStyle.None
        Me.msMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msMain.GripMargin = New System.Windows.Forms.Padding(2, 0, 0, 0)
        Me.msMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mMenFile, Me.mMenView, Me.mMenTools, Me.mMenInfo})
        Me.msMain.Location = New System.Drawing.Point(0, 0)
        Me.msMain.Name = "msMain"
        Me.msMain.Size = New System.Drawing.Size(842, 24)
        Me.msMain.TabIndex = 16
        Me.msMain.Text = "Main Toolbar"
        '
        'mMenFile
        '
        Me.mMenFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mMenFileNew, Me.mMenFileLoad, Me.mMenFileSep1, Me.mMenFileSave, Me.mMenFileSaveAs, Me.mMenFileSep2, Me.mMenFileExit})
        Me.mMenFile.Name = "mMenFile"
        Me.mMenFile.Size = New System.Drawing.Size(35, 20)
        Me.mMenFile.Text = "&File"
        '
        'mMenFileNew
        '
        Me.mMenFileNew.Image = Global.mRemote.My.Resources.Resources.Connections_New
        Me.mMenFileNew.Name = "mMenFileNew"
        Me.mMenFileNew.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.mMenFileNew.Size = New System.Drawing.Size(238, 22)
        Me.mMenFileNew.Text = "New Connections"
        '
        'mMenFileLoad
        '
        Me.mMenFileLoad.Image = Global.mRemote.My.Resources.Resources.Connections_Load
        Me.mMenFileLoad.Name = "mMenFileLoad"
        Me.mMenFileLoad.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.mMenFileLoad.Size = New System.Drawing.Size(238, 22)
        Me.mMenFileLoad.Text = "Open Connections"
        '
        'mMenFileSep1
        '
        Me.mMenFileSep1.Name = "mMenFileSep1"
        Me.mMenFileSep1.Size = New System.Drawing.Size(235, 6)
        '
        'mMenFileSave
        '
        Me.mMenFileSave.Image = Global.mRemote.My.Resources.Resources.Connections_Save
        Me.mMenFileSave.Name = "mMenFileSave"
        Me.mMenFileSave.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.mMenFileSave.Size = New System.Drawing.Size(238, 22)
        Me.mMenFileSave.Text = "Save Connections"
        '
        'mMenFileSaveAs
        '
        Me.mMenFileSaveAs.Image = Global.mRemote.My.Resources.Resources.Connections_SaveAs
        Me.mMenFileSaveAs.Name = "mMenFileSaveAs"
        Me.mMenFileSaveAs.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
                    Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.mMenFileSaveAs.Size = New System.Drawing.Size(238, 22)
        Me.mMenFileSaveAs.Text = "Save Connections As"
        '
        'mMenFileSep2
        '
        Me.mMenFileSep2.Name = "mMenFileSep2"
        Me.mMenFileSep2.Size = New System.Drawing.Size(235, 6)
        '
        'mMenFileExit
        '
        Me.mMenFileExit.Image = Global.mRemote.My.Resources.Resources.Quit
        Me.mMenFileExit.Name = "mMenFileExit"
        Me.mMenFileExit.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F4), System.Windows.Forms.Keys)
        Me.mMenFileExit.Size = New System.Drawing.Size(238, 22)
        Me.mMenFileExit.Text = "Exit"
        '
        'mMenView
        '
        Me.mMenView.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mMenViewAddConnectionPanel, Me.mMenViewConnectionPanels, Me.mMenViewSep1, Me.mMenViewConnections, Me.mMenViewConfig, Me.mMenViewSessions, Me.mMenViewErrorsAndInfos, Me.mMenViewScreenshotManager, Me.ToolStripSeparator1, Me.mMenViewJumpTo, Me.mMenViewSep2, Me.mMenViewQuickConnectToolbar, Me.mMenViewExtAppsToolbar, Me.mMenViewSep3, Me.mMenViewFullscreen})
        Me.mMenView.Name = "mMenView"
        Me.mMenView.Size = New System.Drawing.Size(42, 20)
        Me.mMenView.Text = "&View"
        '
        'mMenViewAddConnectionPanel
        '
        Me.mMenViewAddConnectionPanel.Image = Global.mRemote.My.Resources.Resources.Panel_Add
        Me.mMenViewAddConnectionPanel.Name = "mMenViewAddConnectionPanel"
        Me.mMenViewAddConnectionPanel.Size = New System.Drawing.Size(211, 22)
        Me.mMenViewAddConnectionPanel.Text = "Add Connection Panel"
        '
        'mMenViewConnectionPanels
        '
        Me.mMenViewConnectionPanels.Image = Global.mRemote.My.Resources.Resources.Panels
        Me.mMenViewConnectionPanels.Name = "mMenViewConnectionPanels"
        Me.mMenViewConnectionPanels.Size = New System.Drawing.Size(211, 22)
        Me.mMenViewConnectionPanels.Text = "Connection Panels"
        '
        'mMenViewSep1
        '
        Me.mMenViewSep1.Name = "mMenViewSep1"
        Me.mMenViewSep1.Size = New System.Drawing.Size(208, 6)
        '
        'mMenViewConnections
        '
        Me.mMenViewConnections.Checked = True
        Me.mMenViewConnections.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mMenViewConnections.Image = Global.mRemote.My.Resources.Resources.Root
        Me.mMenViewConnections.Name = "mMenViewConnections"
        Me.mMenViewConnections.Size = New System.Drawing.Size(211, 22)
        Me.mMenViewConnections.Text = "Connections"
        '
        'mMenViewConfig
        '
        Me.mMenViewConfig.Checked = True
        Me.mMenViewConfig.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mMenViewConfig.Image = CType(resources.GetObject("mMenViewConfig.Image"), System.Drawing.Image)
        Me.mMenViewConfig.Name = "mMenViewConfig"
        Me.mMenViewConfig.Size = New System.Drawing.Size(211, 22)
        Me.mMenViewConfig.Text = "Config"
        '
        'mMenViewSessions
        '
        Me.mMenViewSessions.Checked = True
        Me.mMenViewSessions.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mMenViewSessions.Image = CType(resources.GetObject("mMenViewSessions.Image"), System.Drawing.Image)
        Me.mMenViewSessions.Name = "mMenViewSessions"
        Me.mMenViewSessions.Size = New System.Drawing.Size(211, 22)
        Me.mMenViewSessions.Text = "Sessions"
        '
        'mMenViewErrorsAndInfos
        '
        Me.mMenViewErrorsAndInfos.Checked = True
        Me.mMenViewErrorsAndInfos.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mMenViewErrorsAndInfos.Image = Global.mRemote.My.Resources.Resources.ErrorsAndInfos
        Me.mMenViewErrorsAndInfos.Name = "mMenViewErrorsAndInfos"
        Me.mMenViewErrorsAndInfos.Size = New System.Drawing.Size(211, 22)
        Me.mMenViewErrorsAndInfos.Text = "Errors and Infos"
        '
        'mMenViewScreenshotManager
        '
        Me.mMenViewScreenshotManager.Image = CType(resources.GetObject("mMenViewScreenshotManager.Image"), System.Drawing.Image)
        Me.mMenViewScreenshotManager.Name = "mMenViewScreenshotManager"
        Me.mMenViewScreenshotManager.Size = New System.Drawing.Size(211, 22)
        Me.mMenViewScreenshotManager.Text = "Screenshot Manager"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(208, 6)
        '
        'mMenViewJumpTo
        '
        Me.mMenViewJumpTo.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mMenViewJumpToConnectionsConfig, Me.mMenViewJumpToSessionsScreenshots, Me.mMenViewJumpToErrorsInfos})
        Me.mMenViewJumpTo.Image = Global.mRemote.My.Resources.Resources.JumpTo
        Me.mMenViewJumpTo.Name = "mMenViewJumpTo"
        Me.mMenViewJumpTo.Size = New System.Drawing.Size(211, 22)
        Me.mMenViewJumpTo.Text = "Jump To"
        '
        'mMenViewJumpToConnectionsConfig
        '
        Me.mMenViewJumpToConnectionsConfig.Name = "mMenViewJumpToConnectionsConfig"
        Me.mMenViewJumpToConnectionsConfig.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
                    Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.mMenViewJumpToConnectionsConfig.Size = New System.Drawing.Size(240, 22)
        Me.mMenViewJumpToConnectionsConfig.Text = "Connections && Config"
        '
        'mMenViewJumpToSessionsScreenshots
        '
        Me.mMenViewJumpToSessionsScreenshots.Name = "mMenViewJumpToSessionsScreenshots"
        Me.mMenViewJumpToSessionsScreenshots.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
                    Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.mMenViewJumpToSessionsScreenshots.Size = New System.Drawing.Size(240, 22)
        Me.mMenViewJumpToSessionsScreenshots.Text = "Sessions && Screenshots"
        '
        'mMenViewJumpToErrorsInfos
        '
        Me.mMenViewJumpToErrorsInfos.Name = "mMenViewJumpToErrorsInfos"
        Me.mMenViewJumpToErrorsInfos.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
                    Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.mMenViewJumpToErrorsInfos.Size = New System.Drawing.Size(240, 22)
        Me.mMenViewJumpToErrorsInfos.Text = "Errors && Infos"
        '
        'mMenViewSep2
        '
        Me.mMenViewSep2.Name = "mMenViewSep2"
        Me.mMenViewSep2.Size = New System.Drawing.Size(208, 6)
        '
        'mMenViewQuickConnectToolbar
        '
        Me.mMenViewQuickConnectToolbar.Image = Global.mRemote.My.Resources.Resources.Play_Quick
        Me.mMenViewQuickConnectToolbar.Name = "mMenViewQuickConnectToolbar"
        Me.mMenViewQuickConnectToolbar.Size = New System.Drawing.Size(211, 22)
        Me.mMenViewQuickConnectToolbar.Text = "Quicky Toolbar"
        '
        'mMenViewExtAppsToolbar
        '
        Me.mMenViewExtAppsToolbar.Image = CType(resources.GetObject("mMenViewExtAppsToolbar.Image"), System.Drawing.Image)
        Me.mMenViewExtAppsToolbar.Name = "mMenViewExtAppsToolbar"
        Me.mMenViewExtAppsToolbar.Size = New System.Drawing.Size(211, 22)
        Me.mMenViewExtAppsToolbar.Text = "External Applications Toolbar"
        '
        'mMenViewSep3
        '
        Me.mMenViewSep3.Name = "mMenViewSep3"
        Me.mMenViewSep3.Size = New System.Drawing.Size(208, 6)
        '
        'mMenViewFullscreen
        '
        Me.mMenViewFullscreen.Image = Global.mRemote.My.Resources.Resources.Fullscreen
        Me.mMenViewFullscreen.Name = "mMenViewFullscreen"
        Me.mMenViewFullscreen.Size = New System.Drawing.Size(211, 22)
        Me.mMenViewFullscreen.Text = "Fullscreen (Kiosk Mode)"
        '
        'mMenTools
        '
        Me.mMenTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mMenToolsSSHTransfer, Me.mMenToolsUVNCSC, Me.mMenToolsExternalApps, Me.mMenToolsPortScan, Me.mMenToolsSep1, Me.mMenToolsUpdate, Me.mMenToolsComponentsCheck, Me.mMenToolsOptions})
        Me.mMenTools.Name = "mMenTools"
        Me.mMenTools.Size = New System.Drawing.Size(45, 20)
        Me.mMenTools.Text = "&Tools"
        '
        'mMenToolsSSHTransfer
        '
        Me.mMenToolsSSHTransfer.Image = Global.mRemote.My.Resources.Resources.SSHTransfer
        Me.mMenToolsSSHTransfer.Name = "mMenToolsSSHTransfer"
        Me.mMenToolsSSHTransfer.Size = New System.Drawing.Size(173, 22)
        Me.mMenToolsSSHTransfer.Text = "SSH File Transfer"
        '
        'mMenToolsUVNCSC
        '
        Me.mMenToolsUVNCSC.Image = Global.mRemote.My.Resources.Resources.UVNC_SC
        Me.mMenToolsUVNCSC.Name = "mMenToolsUVNCSC"
        Me.mMenToolsUVNCSC.Size = New System.Drawing.Size(173, 22)
        Me.mMenToolsUVNCSC.Text = "UltraVNC SingleClick"
        '
        'mMenToolsExternalApps
        '
        Me.mMenToolsExternalApps.Image = CType(resources.GetObject("mMenToolsExternalApps.Image"), System.Drawing.Image)
        Me.mMenToolsExternalApps.Name = "mMenToolsExternalApps"
        Me.mMenToolsExternalApps.Size = New System.Drawing.Size(173, 22)
        Me.mMenToolsExternalApps.Text = "External Applications"
        '
        'mMenToolsPortScan
        '
        Me.mMenToolsPortScan.Image = Global.mRemote.My.Resources.Resources.PortScan
        Me.mMenToolsPortScan.Name = "mMenToolsPortScan"
        Me.mMenToolsPortScan.Size = New System.Drawing.Size(173, 22)
        Me.mMenToolsPortScan.Text = "Port Scan"
        '
        'mMenToolsSep1
        '
        Me.mMenToolsSep1.Name = "mMenToolsSep1"
        Me.mMenToolsSep1.Size = New System.Drawing.Size(170, 6)
        '
        'mMenToolsUpdate
        '
        Me.mMenToolsUpdate.Image = Global.mRemote.My.Resources.Resources.Update
        Me.mMenToolsUpdate.Name = "mMenToolsUpdate"
        Me.mMenToolsUpdate.Size = New System.Drawing.Size(173, 22)
        Me.mMenToolsUpdate.Text = "Update"
        '
        'mMenToolsComponentsCheck
        '
        Me.mMenToolsComponentsCheck.Image = Global.mRemote.My.Resources.Resources.ComponentsCheck
        Me.mMenToolsComponentsCheck.Name = "mMenToolsComponentsCheck"
        Me.mMenToolsComponentsCheck.Size = New System.Drawing.Size(173, 22)
        Me.mMenToolsComponentsCheck.Text = "Components Check"
        '
        'mMenToolsOptions
        '
        Me.mMenToolsOptions.Image = CType(resources.GetObject("mMenToolsOptions.Image"), System.Drawing.Image)
        Me.mMenToolsOptions.Name = "mMenToolsOptions"
        Me.mMenToolsOptions.Size = New System.Drawing.Size(173, 22)
        Me.mMenToolsOptions.Text = "Options"
        '
        'mMenInfo
        '
        Me.mMenInfo.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mMenInfoHelp, Me.mMenInfoSep1, Me.mMenInfoDonate, Me.mMenInfoWebsite, Me.mMenInfovRD08, Me.mMenInfoSep2, Me.mMenInfoAnnouncments, Me.mMenInfoAbout})
        Me.mMenInfo.Name = "mMenInfo"
        Me.mMenInfo.Size = New System.Drawing.Size(41, 20)
        Me.mMenInfo.Text = "&Help"
        Me.mMenInfo.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal
        '
        'mMenInfoHelp
        '
        Me.mMenInfoHelp.Image = CType(resources.GetObject("mMenInfoHelp.Image"), System.Drawing.Image)
        Me.mMenInfoHelp.Name = "mMenInfoHelp"
        Me.mMenInfoHelp.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.mMenInfoHelp.Size = New System.Drawing.Size(202, 22)
        Me.mMenInfoHelp.Text = "mRemoteNG Help"
        '
        'mMenInfoSep1
        '
        Me.mMenInfoSep1.Name = "mMenInfoSep1"
        Me.mMenInfoSep1.Size = New System.Drawing.Size(199, 6)
        '
        'mMenInfoDonate
        '
        Me.mMenInfoDonate.Image = Global.mRemote.My.Resources.Resources.Donate
        Me.mMenInfoDonate.Name = "mMenInfoDonate"
        Me.mMenInfoDonate.Size = New System.Drawing.Size(202, 22)
        Me.mMenInfoDonate.Text = "Donate"
        '
        'mMenInfoWebsite
        '
        Me.mMenInfoWebsite.Image = CType(resources.GetObject("mMenInfoWebsite.Image"), System.Drawing.Image)
        Me.mMenInfoWebsite.Name = "mMenInfoWebsite"
        Me.mMenInfoWebsite.Size = New System.Drawing.Size(202, 22)
        Me.mMenInfoWebsite.Text = "Website"
        '
        'mMenInfovRD08
        '
        Me.mMenInfovRD08.Image = Global.mRemote.My.Resources.Resources.vRD08R1
        Me.mMenInfovRD08.Name = "mMenInfovRD08"
        Me.mMenInfovRD08.Size = New System.Drawing.Size(202, 22)
        Me.mMenInfovRD08.Text = "visionapp Remote Desktop"
        '
        'mMenInfoSep2
        '
        Me.mMenInfoSep2.Name = "mMenInfoSep2"
        Me.mMenInfoSep2.Size = New System.Drawing.Size(199, 6)
        '
        'mMenInfoAnnouncments
        '
        Me.mMenInfoAnnouncments.Image = Global.mRemote.My.Resources.Resources.News
        Me.mMenInfoAnnouncments.Name = "mMenInfoAnnouncments"
        Me.mMenInfoAnnouncments.Size = New System.Drawing.Size(202, 22)
        Me.mMenInfoAnnouncments.Text = "Announcments"
        '
        'mMenInfoAbout
        '
        Me.mMenInfoAbout.Image = Global.mRemote.My.Resources.Resources.mRemote
        Me.mMenInfoAbout.Name = "mMenInfoAbout"
        Me.mMenInfoAbout.Size = New System.Drawing.Size(202, 22)
        Me.mMenInfoAbout.Text = "About"
        '
        'mMenSep3
        '
        Me.mMenSep3.Name = "mMenSep3"
        Me.mMenSep3.Size = New System.Drawing.Size(211, 6)
        '
        'lblQuickConnect
        '
        Me.lblQuickConnect.Name = "lblQuickConnect"
        Me.lblQuickConnect.Size = New System.Drawing.Size(43, 22)
        Me.lblQuickConnect.Text = "&Quicky:"
        '
        'cmbQuickConnect
        '
        Me.cmbQuickConnect.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbQuickConnect.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbQuickConnect.Name = "cmbQuickConnect"
        Me.cmbQuickConnect.Size = New System.Drawing.Size(120, 21)
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
        Me.tsContainer.ContentPanel.Size = New System.Drawing.Size(842, 499)
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
        Me.tsContainer.TopToolStripPanel.Controls.Add(Me.msMain)
        Me.tsContainer.TopToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        '
        'tsQuickConnect
        '
        Me.tsQuickConnect.Dock = System.Windows.Forms.DockStyle.None
        Me.tsQuickConnect.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblQuickConnect, Me.cmbQuickConnect, Me.btnQuickyPlay, Me.mMenQuickyCon})
        Me.tsQuickConnect.Location = New System.Drawing.Point(3, 0)
        Me.tsQuickConnect.Name = "tsQuickConnect"
        Me.tsQuickConnect.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.tsQuickConnect.Size = New System.Drawing.Size(207, 25)
        Me.tsQuickConnect.TabIndex = 18
        '
        'btnQuickyPlay
        '
        Me.btnQuickyPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnQuickyPlay.Image = Global.mRemote.My.Resources.Resources.Play_Quick
        Me.btnQuickyPlay.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnQuickyPlay.Name = "btnQuickyPlay"
        Me.btnQuickyPlay.Size = New System.Drawing.Size(32, 20)
        Me.btnQuickyPlay.Text = "Play"
        '
        'mMenQuickyCon
        '
        Me.mMenQuickyCon.AutoSize = False
        Me.mMenQuickyCon.AutoToolTip = True
        Me.mMenQuickyCon.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.mMenQuickyCon.Image = Global.mRemote.My.Resources.Resources.Root
        Me.mMenQuickyCon.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.mMenQuickyCon.Name = "mMenQuickyCon"
        Me.mMenQuickyCon.Size = New System.Drawing.Size(30, 22)
        Me.mMenQuickyCon.Text = "Connections"
        '
        'tsExtAppsToolbar
        '
        Me.tsExtAppsToolbar.ContextMenuStrip = Me.cMenExtAppsToolbar
        Me.tsExtAppsToolbar.Dock = System.Windows.Forms.DockStyle.None
        Me.tsExtAppsToolbar.Location = New System.Drawing.Point(300, 0)
        Me.tsExtAppsToolbar.Name = "tsExtAppsToolbar"
        Me.tsExtAppsToolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.tsExtAppsToolbar.Size = New System.Drawing.Size(109, 25)
        Me.tsExtAppsToolbar.TabIndex = 17
        '
        'cMenExtAppsToolbar
        '
        Me.cMenExtAppsToolbar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cMenToolbarShowText})
        Me.cMenExtAppsToolbar.Name = "cMenToolbar"
        Me.cMenExtAppsToolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.cMenExtAppsToolbar.Size = New System.Drawing.Size(126, 26)
        '
        'cMenToolbarShowText
        '
        Me.cMenToolbarShowText.Checked = True
        Me.cMenToolbarShowText.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cMenToolbarShowText.Name = "cMenToolbarShowText"
        Me.cMenToolbarShowText.Size = New System.Drawing.Size(125, 22)
        Me.cMenToolbarShowText.Text = "Show Text"
        '
        'tmrShowUpdate
        '
        Me.tmrShowUpdate.Enabled = True
        Me.tmrShowUpdate.Interval = 5000
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
        Me.Icon = Global.mRemote.My.Resources.Resources.mRemote_Icon
        Me.MainMenuStrip = Me.msMain
        Me.MaximumSize = New System.Drawing.Size(20000, 10000)
        Me.Name = "frmMain"
        Me.Text = "mRemote"
        Me.msMain.ResumeLayout(False)
        Me.msMain.PerformLayout()
        Me.tsContainer.ContentPanel.ResumeLayout(False)
        Me.tsContainer.TopToolStripPanel.ResumeLayout(False)
        Me.tsContainer.TopToolStripPanel.PerformLayout()
        Me.tsContainer.ResumeLayout(False)
        Me.tsContainer.PerformLayout()
        Me.tsQuickConnect.ResumeLayout(False)
        Me.tsQuickConnect.PerformLayout()
        Me.cMenExtAppsToolbar.ResumeLayout(False)
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
    Friend WithEvents mMenFileSep1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mMenFileSave As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenFileSaveAs As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenFileSep2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mMenFileExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenToolsUpdate As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenToolsSep1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mMenToolsOptions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenInfoHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenInfoWebsite As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenInfoSep1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mMenInfoAbout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenViewConnectionPanels As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenViewSep1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mMenViewSessions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenViewConnections As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenViewConfig As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenViewErrorsAndInfos As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenViewScreenshotManager As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenViewAddConnectionPanel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbQuickConnect As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents mMenViewSep2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mMenViewFullscreen As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenToolsSSHTransfer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsContainer As System.Windows.Forms.ToolStripContainer
    Friend WithEvents tmrShowUpdate As System.Windows.Forms.Timer
    Friend WithEvents mMenToolsExternalApps As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tmrAutoSave As System.Windows.Forms.Timer
    Friend WithEvents tsExtAppsToolbar As System.Windows.Forms.ToolStrip
    Friend WithEvents mMenViewExtAppsToolbar As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cMenExtAppsToolbar As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cMenToolbarShowText As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenToolsPortScan As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsQuickConnect As System.Windows.Forms.ToolStrip
    Friend WithEvents mMenViewQuickConnectToolbar As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenSep3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mMenInfoDonate As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenViewSep3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnQuickyPlay As System.Windows.Forms.ToolStripSplitButton
    Friend WithEvents mMenQuickyCon As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenViewJumpTo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenViewJumpToConnectionsConfig As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenViewJumpToSessionsScreenshots As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenViewJumpToErrorsInfos As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mMenToolsUVNCSC As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenToolsComponentsCheck As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenInfoAnnouncments As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenInfovRD08 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mMenInfoSep2 As System.Windows.Forms.ToolStripSeparator

End Class
