Imports WeifenLuo.WinFormsUI.Docking
Imports mRemoteNG.App.Runtime

Public Class frmOptions
    Inherits System.Windows.Forms.Form

#Region "Form Init"
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents lvPages As System.Windows.Forms.ListView
    Friend WithEvents imgListPages As System.Windows.Forms.ImageList
    Friend WithEvents chkWriteLogFile As System.Windows.Forms.CheckBox
    Friend WithEvents chkAutomaticallyGetSessionInfo As System.Windows.Forms.CheckBox
    Friend WithEvents lblXulRunnerPath As System.Windows.Forms.Label
    Friend WithEvents chkEncryptCompleteFile As System.Windows.Forms.CheckBox
    Friend WithEvents btnBrowseXulRunnerPath As System.Windows.Forms.Button
    Friend WithEvents chkUseCustomPuttyPath As System.Windows.Forms.CheckBox
    Friend WithEvents txtXULrunnerPath As System.Windows.Forms.TextBox
    Friend WithEvents txtCustomPuttyPath As System.Windows.Forms.TextBox
    Friend WithEvents lblUVNCSCPort As System.Windows.Forms.Label
    Friend WithEvents btnBrowseCustomPuttyPath As System.Windows.Forms.Button
    Friend WithEvents lblSeconds As System.Windows.Forms.Label
    Friend WithEvents btnLaunchPutty As System.Windows.Forms.Button
    Friend WithEvents numUVNCSCPort As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblConfigurePuttySessions As System.Windows.Forms.Label
    Friend WithEvents numPuttyWaitTime As System.Windows.Forms.NumericUpDown
    Friend WithEvents chkAutomaticReconnect As System.Windows.Forms.CheckBox
    Friend WithEvents lblMaximumPuttyWaitTime As System.Windows.Forms.Label
    Friend WithEvents lblUpdatesExplanation As System.Windows.Forms.Label
    Friend WithEvents pnlUpdateCheck As System.Windows.Forms.Panel
    Friend WithEvents btnUpdateCheckNow As System.Windows.Forms.Button
    Friend WithEvents chkCheckForUpdatesOnStartup As System.Windows.Forms.CheckBox
    Friend WithEvents cboUpdateCheckFrequency As System.Windows.Forms.ComboBox
    Friend WithEvents pnlProxy As System.Windows.Forms.Panel
    Friend WithEvents pnlProxyBasic As System.Windows.Forms.Panel
    Friend WithEvents lblProxyAddress As System.Windows.Forms.Label
    Friend WithEvents txtProxyAddress As System.Windows.Forms.TextBox
    Friend WithEvents lblProxyPort As System.Windows.Forms.Label
    Friend WithEvents numProxyPort As System.Windows.Forms.NumericUpDown
    Friend WithEvents chkUseProxyForAutomaticUpdates As System.Windows.Forms.CheckBox
    Friend WithEvents chkUseProxyAuthentication As System.Windows.Forms.CheckBox
    Friend WithEvents pnlProxyAuthentication As System.Windows.Forms.Panel
    Friend WithEvents lblProxyUsername As System.Windows.Forms.Label
    Friend WithEvents txtProxyUsername As System.Windows.Forms.TextBox
    Friend WithEvents lblProxyPassword As System.Windows.Forms.Label
    Friend WithEvents txtProxyPassword As System.Windows.Forms.TextBox
    Friend WithEvents btnTestProxy As System.Windows.Forms.Button
    Friend WithEvents pnlRdpReconnectionCount As System.Windows.Forms.Panel
    Friend WithEvents lblRdpReconnectionCount As System.Windows.Forms.Label
    Friend WithEvents numRdpReconnectionCount As System.Windows.Forms.NumericUpDown
    Friend WithEvents chkSingleClickOnConnectionOpensIt As System.Windows.Forms.CheckBox
    Friend WithEvents chkSingleClickOnOpenedConnectionSwitchesToIt As System.Windows.Forms.CheckBox
    Friend WithEvents pnlAutoSave As System.Windows.Forms.Panel
    Friend WithEvents lblAutoSave1 As System.Windows.Forms.Label
    Friend WithEvents numAutoSave As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblAutoSave2 As System.Windows.Forms.Label
    Friend WithEvents chkHostnameLikeDisplayName As System.Windows.Forms.CheckBox
    Friend WithEvents chkUseOnlyErrorsAndInfosPanel As System.Windows.Forms.CheckBox
    Friend WithEvents lblSwitchToErrorsAndInfos As System.Windows.Forms.Label
    Friend WithEvents chkMCInformation As System.Windows.Forms.CheckBox
    Friend WithEvents chkMCErrors As System.Windows.Forms.CheckBox
    Friend WithEvents chkMCWarnings As System.Windows.Forms.CheckBox
    Friend WithEvents chkOpenNewTabRightOfSelected As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowProtocolOnTabs As System.Windows.Forms.CheckBox
    Friend WithEvents chkDoubleClickClosesTab As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowLogonInfoOnTabs As System.Windows.Forms.CheckBox
    Friend WithEvents chkAlwaysShowPanelSelectionDlg As System.Windows.Forms.CheckBox
    Friend WithEvents lblLanguageRestartRequired As System.Windows.Forms.Label
    Friend WithEvents cboLanguage As System.Windows.Forms.ComboBox
    Friend WithEvents lblLanguage As System.Windows.Forms.Label
    Friend WithEvents chkShowDescriptionTooltipsInTree As System.Windows.Forms.CheckBox
    Friend WithEvents chkMinimizeToSystemTray As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowSystemTrayIcon As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowFullConnectionsFilePathInTitle As System.Windows.Forms.CheckBox
    Friend WithEvents chkConfirmCloseConnection As System.Windows.Forms.CheckBox
    Friend WithEvents chkSaveConsOnExit As System.Windows.Forms.CheckBox
    Friend WithEvents chkProperInstallationOfComponentsAtStartup As System.Windows.Forms.CheckBox
    Friend WithEvents chkConfirmExit As System.Windows.Forms.CheckBox
    Friend WithEvents chkSingleInstance As System.Windows.Forms.CheckBox
    Friend WithEvents chkReconnectOnStart As System.Windows.Forms.CheckBox
    Friend WithEvents tcTabControl As System.Windows.Forms.TabControl
    Friend WithEvents tabStartupExit As System.Windows.Forms.TabPage
    Friend WithEvents tabAppearance As System.Windows.Forms.TabPage
    Friend WithEvents tabTabsAndPanels As System.Windows.Forms.TabPage
    Friend WithEvents tabConnections As System.Windows.Forms.TabPage
    Friend WithEvents tabUpdates As System.Windows.Forms.TabPage
    Friend WithEvents tabAdvanced As System.Windows.Forms.TabPage
    Friend WithEvents tabSQLServer As System.Windows.Forms.TabPage
    Friend WithEvents pnlDefaultCredentials As System.Windows.Forms.Panel
    Friend WithEvents radCredentialsCustom As System.Windows.Forms.RadioButton
    Friend WithEvents lblDefaultCredentials As System.Windows.Forms.Label
    Friend WithEvents radCredentialsNoInfo As System.Windows.Forms.RadioButton
    Friend WithEvents radCredentialsWindows As System.Windows.Forms.RadioButton
    Friend WithEvents txtCredentialsDomain As System.Windows.Forms.TextBox
    Friend WithEvents lblCredentialsUsername As System.Windows.Forms.Label
    Friend WithEvents txtCredentialsPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblCredentialsPassword As System.Windows.Forms.Label
    Friend WithEvents txtCredentialsUsername As System.Windows.Forms.TextBox
    Friend WithEvents lblCredentialsDomain As System.Windows.Forms.Label
    Friend WithEvents chkUseSQLServer As System.Windows.Forms.CheckBox
    Friend WithEvents lblSQLInfo As System.Windows.Forms.Label
    Friend WithEvents lblSQLUsername As System.Windows.Forms.Label
    Friend WithEvents txtSQLPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblSQLServer As System.Windows.Forms.Label
    Friend WithEvents txtSQLUsername As System.Windows.Forms.TextBox
    Friend WithEvents lblSQLPassword As System.Windows.Forms.Label
    Friend WithEvents txtSQLServer As System.Windows.Forms.TextBox
    Friend WithEvents lblExperimental As System.Windows.Forms.Label
    Friend WithEvents lblSQLDatabaseName As System.Windows.Forms.Label
    Friend WithEvents txtSQLDatabaseName As System.Windows.Forms.TextBox
    Private components As System.ComponentModel.IContainer

    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim ListViewItem1 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Startup/Exit", 0)
        Dim ListViewItem2 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Appearance", 1)
        Dim ListViewItem3 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Tabs & Panels", 2)
        Dim ListViewItem4 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Connections", 3)
        Dim ListViewItem5 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("SQL Server", 4)
        Dim ListViewItem6 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Updates", 5)
        Dim ListViewItem7 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Advanced", 6)
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOptions))
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lvPages = New System.Windows.Forms.ListView
        Me.imgListPages = New System.Windows.Forms.ImageList(Me.components)
        Me.lblMaximumPuttyWaitTime = New System.Windows.Forms.Label
        Me.chkAutomaticReconnect = New System.Windows.Forms.CheckBox
        Me.numPuttyWaitTime = New System.Windows.Forms.NumericUpDown
        Me.lblConfigurePuttySessions = New System.Windows.Forms.Label
        Me.numUVNCSCPort = New System.Windows.Forms.NumericUpDown
        Me.btnLaunchPutty = New System.Windows.Forms.Button
        Me.lblSeconds = New System.Windows.Forms.Label
        Me.btnBrowseCustomPuttyPath = New System.Windows.Forms.Button
        Me.lblUVNCSCPort = New System.Windows.Forms.Label
        Me.txtCustomPuttyPath = New System.Windows.Forms.TextBox
        Me.txtXULrunnerPath = New System.Windows.Forms.TextBox
        Me.chkUseCustomPuttyPath = New System.Windows.Forms.CheckBox
        Me.btnBrowseXulRunnerPath = New System.Windows.Forms.Button
        Me.chkEncryptCompleteFile = New System.Windows.Forms.CheckBox
        Me.lblXulRunnerPath = New System.Windows.Forms.Label
        Me.chkAutomaticallyGetSessionInfo = New System.Windows.Forms.CheckBox
        Me.chkWriteLogFile = New System.Windows.Forms.CheckBox
        Me.pnlProxy = New System.Windows.Forms.Panel
        Me.pnlProxyBasic = New System.Windows.Forms.Panel
        Me.lblProxyAddress = New System.Windows.Forms.Label
        Me.txtProxyAddress = New System.Windows.Forms.TextBox
        Me.lblProxyPort = New System.Windows.Forms.Label
        Me.numProxyPort = New System.Windows.Forms.NumericUpDown
        Me.chkUseProxyForAutomaticUpdates = New System.Windows.Forms.CheckBox
        Me.chkUseProxyAuthentication = New System.Windows.Forms.CheckBox
        Me.pnlProxyAuthentication = New System.Windows.Forms.Panel
        Me.lblProxyUsername = New System.Windows.Forms.Label
        Me.txtProxyUsername = New System.Windows.Forms.TextBox
        Me.lblProxyPassword = New System.Windows.Forms.Label
        Me.txtProxyPassword = New System.Windows.Forms.TextBox
        Me.btnTestProxy = New System.Windows.Forms.Button
        Me.pnlUpdateCheck = New System.Windows.Forms.Panel
        Me.btnUpdateCheckNow = New System.Windows.Forms.Button
        Me.chkCheckForUpdatesOnStartup = New System.Windows.Forms.CheckBox
        Me.cboUpdateCheckFrequency = New System.Windows.Forms.ComboBox
        Me.lblUpdatesExplanation = New System.Windows.Forms.Label
        Me.chkHostnameLikeDisplayName = New System.Windows.Forms.CheckBox
        Me.pnlAutoSave = New System.Windows.Forms.Panel
        Me.lblAutoSave1 = New System.Windows.Forms.Label
        Me.numAutoSave = New System.Windows.Forms.NumericUpDown
        Me.lblAutoSave2 = New System.Windows.Forms.Label
        Me.chkSingleClickOnOpenedConnectionSwitchesToIt = New System.Windows.Forms.CheckBox
        Me.chkSingleClickOnConnectionOpensIt = New System.Windows.Forms.CheckBox
        Me.pnlRdpReconnectionCount = New System.Windows.Forms.Panel
        Me.lblRdpReconnectionCount = New System.Windows.Forms.Label
        Me.numRdpReconnectionCount = New System.Windows.Forms.NumericUpDown
        Me.chkAlwaysShowPanelSelectionDlg = New System.Windows.Forms.CheckBox
        Me.chkShowLogonInfoOnTabs = New System.Windows.Forms.CheckBox
        Me.chkDoubleClickClosesTab = New System.Windows.Forms.CheckBox
        Me.chkShowProtocolOnTabs = New System.Windows.Forms.CheckBox
        Me.chkOpenNewTabRightOfSelected = New System.Windows.Forms.CheckBox
        Me.chkMCWarnings = New System.Windows.Forms.CheckBox
        Me.chkMCErrors = New System.Windows.Forms.CheckBox
        Me.chkMCInformation = New System.Windows.Forms.CheckBox
        Me.lblSwitchToErrorsAndInfos = New System.Windows.Forms.Label
        Me.chkUseOnlyErrorsAndInfosPanel = New System.Windows.Forms.CheckBox
        Me.chkShowFullConnectionsFilePathInTitle = New System.Windows.Forms.CheckBox
        Me.chkShowSystemTrayIcon = New System.Windows.Forms.CheckBox
        Me.chkMinimizeToSystemTray = New System.Windows.Forms.CheckBox
        Me.chkShowDescriptionTooltipsInTree = New System.Windows.Forms.CheckBox
        Me.lblLanguage = New System.Windows.Forms.Label
        Me.cboLanguage = New System.Windows.Forms.ComboBox
        Me.lblLanguageRestartRequired = New System.Windows.Forms.Label
        Me.chkReconnectOnStart = New System.Windows.Forms.CheckBox
        Me.chkSingleInstance = New System.Windows.Forms.CheckBox
        Me.chkConfirmExit = New System.Windows.Forms.CheckBox
        Me.chkProperInstallationOfComponentsAtStartup = New System.Windows.Forms.CheckBox
        Me.chkSaveConsOnExit = New System.Windows.Forms.CheckBox
        Me.chkConfirmCloseConnection = New System.Windows.Forms.CheckBox
        Me.tcTabControl = New System.Windows.Forms.TabControl
        Me.tabStartupExit = New System.Windows.Forms.TabPage
        Me.tabAppearance = New System.Windows.Forms.TabPage
        Me.tabTabsAndPanels = New System.Windows.Forms.TabPage
        Me.tabConnections = New System.Windows.Forms.TabPage
        Me.pnlDefaultCredentials = New System.Windows.Forms.Panel
        Me.radCredentialsCustom = New System.Windows.Forms.RadioButton
        Me.lblDefaultCredentials = New System.Windows.Forms.Label
        Me.radCredentialsNoInfo = New System.Windows.Forms.RadioButton
        Me.radCredentialsWindows = New System.Windows.Forms.RadioButton
        Me.txtCredentialsDomain = New System.Windows.Forms.TextBox
        Me.lblCredentialsUsername = New System.Windows.Forms.Label
        Me.txtCredentialsPassword = New System.Windows.Forms.TextBox
        Me.lblCredentialsPassword = New System.Windows.Forms.Label
        Me.txtCredentialsUsername = New System.Windows.Forms.TextBox
        Me.lblCredentialsDomain = New System.Windows.Forms.Label
        Me.tabSQLServer = New System.Windows.Forms.TabPage
        Me.lblExperimental = New System.Windows.Forms.Label
        Me.chkUseSQLServer = New System.Windows.Forms.CheckBox
        Me.lblSQLUsername = New System.Windows.Forms.Label
        Me.txtSQLPassword = New System.Windows.Forms.TextBox
        Me.lblSQLInfo = New System.Windows.Forms.Label
        Me.lblSQLServer = New System.Windows.Forms.Label
        Me.txtSQLUsername = New System.Windows.Forms.TextBox
        Me.txtSQLServer = New System.Windows.Forms.TextBox
        Me.lblSQLPassword = New System.Windows.Forms.Label
        Me.tabUpdates = New System.Windows.Forms.TabPage
        Me.tabAdvanced = New System.Windows.Forms.TabPage
        Me.lblSQLDatabaseName = New System.Windows.Forms.Label
        Me.txtSQLDatabaseName = New System.Windows.Forms.TextBox
        CType(Me.numPuttyWaitTime, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numUVNCSCPort, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlProxy.SuspendLayout()
        Me.pnlProxyBasic.SuspendLayout()
        CType(Me.numProxyPort, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlProxyAuthentication.SuspendLayout()
        Me.pnlUpdateCheck.SuspendLayout()
        Me.pnlAutoSave.SuspendLayout()
        CType(Me.numAutoSave, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlRdpReconnectionCount.SuspendLayout()
        CType(Me.numRdpReconnectionCount, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tcTabControl.SuspendLayout()
        Me.tabStartupExit.SuspendLayout()
        Me.tabAppearance.SuspendLayout()
        Me.tabTabsAndPanels.SuspendLayout()
        Me.tabConnections.SuspendLayout()
        Me.pnlDefaultCredentials.SuspendLayout()
        Me.tabSQLServer.SuspendLayout()
        Me.tabUpdates.SuspendLayout()
        Me.tabAdvanced.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(626, 507)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 10000
        Me.btnOK.Text = "&OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(707, 507)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 11000
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lvPages
        '
        Me.lvPages.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lvPages.FullRowSelect = True
        Me.lvPages.HideSelection = False
        Me.lvPages.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem1, ListViewItem2, ListViewItem3, ListViewItem4, ListViewItem5, ListViewItem6, ListViewItem7})
        Me.lvPages.LabelWrap = False
        Me.lvPages.LargeImageList = Me.imgListPages
        Me.lvPages.Location = New System.Drawing.Point(12, 12)
        Me.lvPages.MultiSelect = False
        Me.lvPages.Name = "lvPages"
        Me.lvPages.Scrollable = False
        Me.lvPages.Size = New System.Drawing.Size(154, 489)
        Me.lvPages.TabIndex = 11001
        Me.lvPages.TileSize = New System.Drawing.Size(154, 30)
        Me.lvPages.UseCompatibleStateImageBehavior = False
        Me.lvPages.View = System.Windows.Forms.View.Tile
        '
        'imgListPages
        '
        Me.imgListPages.ImageStream = CType(resources.GetObject("imgListPages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgListPages.TransparentColor = System.Drawing.Color.Fuchsia
        Me.imgListPages.Images.SetKeyName(0, "StartupExit_Icon.ico")
        Me.imgListPages.Images.SetKeyName(1, "Appearance_Icon.ico")
        Me.imgListPages.Images.SetKeyName(2, "Tab_Icon.ico")
        Me.imgListPages.Images.SetKeyName(3, "Root_Icon.ico")
        Me.imgListPages.Images.SetKeyName(4, "database.bmp")
        Me.imgListPages.Images.SetKeyName(5, "Update_Icon.ico")
        Me.imgListPages.Images.SetKeyName(6, "Config_Icon.ico")
        '
        'lblMaximumPuttyWaitTime
        '
        Me.lblMaximumPuttyWaitTime.AutoSize = True
        Me.lblMaximumPuttyWaitTime.Location = New System.Drawing.Point(3, 188)
        Me.lblMaximumPuttyWaitTime.Name = "lblMaximumPuttyWaitTime"
        Me.lblMaximumPuttyWaitTime.Size = New System.Drawing.Size(135, 13)
        Me.lblMaximumPuttyWaitTime.TabIndex = 75
        Me.lblMaximumPuttyWaitTime.Text = "Maximum PuTTY wait time:"
        '
        'chkAutomaticReconnect
        '
        Me.chkAutomaticReconnect.AutoSize = True
        Me.chkAutomaticReconnect.Location = New System.Drawing.Point(3, 72)
        Me.chkAutomaticReconnect.Name = "chkAutomaticReconnect"
        Me.chkAutomaticReconnect.Size = New System.Drawing.Size(399, 17)
        Me.chkAutomaticReconnect.TabIndex = 25
        Me.chkAutomaticReconnect.Text = "Automatically try to reconnect when disconnected from server (RDP && ICA only)"
        Me.chkAutomaticReconnect.UseVisualStyleBackColor = True
        '
        'numPuttyWaitTime
        '
        Me.numPuttyWaitTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.numPuttyWaitTime.Location = New System.Drawing.Point(373, 185)
        Me.numPuttyWaitTime.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
        Me.numPuttyWaitTime.Name = "numPuttyWaitTime"
        Me.numPuttyWaitTime.Size = New System.Drawing.Size(49, 20)
        Me.numPuttyWaitTime.TabIndex = 76
        Me.numPuttyWaitTime.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'lblConfigurePuttySessions
        '
        Me.lblConfigurePuttySessions.AutoSize = True
        Me.lblConfigurePuttySessions.Location = New System.Drawing.Point(3, 157)
        Me.lblConfigurePuttySessions.Name = "lblConfigurePuttySessions"
        Me.lblConfigurePuttySessions.Size = New System.Drawing.Size(227, 13)
        Me.lblConfigurePuttySessions.TabIndex = 60
        Me.lblConfigurePuttySessions.Text = "To configure PuTTY sessions click this button:"
        '
        'numUVNCSCPort
        '
        Me.numUVNCSCPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.numUVNCSCPort.Location = New System.Drawing.Point(219, 277)
        Me.numUVNCSCPort.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
        Me.numUVNCSCPort.Name = "numUVNCSCPort"
        Me.numUVNCSCPort.Size = New System.Drawing.Size(72, 20)
        Me.numUVNCSCPort.TabIndex = 130
        Me.numUVNCSCPort.Value = New Decimal(New Integer() {5500, 0, 0, 0})
        '
        'btnLaunchPutty
        '
        Me.btnLaunchPutty.Image = Global.mRemoteNG.My.Resources.Resources.PuttyConfig
        Me.btnLaunchPutty.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLaunchPutty.Location = New System.Drawing.Point(291, 153)
        Me.btnLaunchPutty.Name = "btnLaunchPutty"
        Me.btnLaunchPutty.Size = New System.Drawing.Size(110, 23)
        Me.btnLaunchPutty.TabIndex = 70
        Me.btnLaunchPutty.Text = "Launch PuTTY"
        Me.btnLaunchPutty.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnLaunchPutty.UseVisualStyleBackColor = True
        '
        'lblSeconds
        '
        Me.lblSeconds.AutoSize = True
        Me.lblSeconds.Location = New System.Drawing.Point(428, 189)
        Me.lblSeconds.Name = "lblSeconds"
        Me.lblSeconds.Size = New System.Drawing.Size(47, 13)
        Me.lblSeconds.TabIndex = 77
        Me.lblSeconds.Text = "seconds"
        '
        'btnBrowseCustomPuttyPath
        '
        Me.btnBrowseCustomPuttyPath.Enabled = False
        Me.btnBrowseCustomPuttyPath.Location = New System.Drawing.Point(291, 116)
        Me.btnBrowseCustomPuttyPath.Name = "btnBrowseCustomPuttyPath"
        Me.btnBrowseCustomPuttyPath.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowseCustomPuttyPath.TabIndex = 50
        Me.btnBrowseCustomPuttyPath.Text = "Browse..."
        Me.btnBrowseCustomPuttyPath.UseVisualStyleBackColor = True
        '
        'lblUVNCSCPort
        '
        Me.lblUVNCSCPort.AutoSize = True
        Me.lblUVNCSCPort.Location = New System.Drawing.Point(3, 281)
        Me.lblUVNCSCPort.Name = "lblUVNCSCPort"
        Me.lblUVNCSCPort.Size = New System.Drawing.Size(176, 13)
        Me.lblUVNCSCPort.TabIndex = 120
        Me.lblUVNCSCPort.Text = "UltraVNC SingleClick Listening Port:"
        '
        'txtCustomPuttyPath
        '
        Me.txtCustomPuttyPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCustomPuttyPath.Enabled = False
        Me.txtCustomPuttyPath.Location = New System.Drawing.Point(21, 118)
        Me.txtCustomPuttyPath.Name = "txtCustomPuttyPath"
        Me.txtCustomPuttyPath.Size = New System.Drawing.Size(264, 20)
        Me.txtCustomPuttyPath.TabIndex = 40
        '
        'txtXULrunnerPath
        '
        Me.txtXULrunnerPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtXULrunnerPath.Location = New System.Drawing.Point(21, 238)
        Me.txtXULrunnerPath.Name = "txtXULrunnerPath"
        Me.txtXULrunnerPath.Size = New System.Drawing.Size(264, 20)
        Me.txtXULrunnerPath.TabIndex = 131
        '
        'chkUseCustomPuttyPath
        '
        Me.chkUseCustomPuttyPath.AutoSize = True
        Me.chkUseCustomPuttyPath.Location = New System.Drawing.Point(3, 95)
        Me.chkUseCustomPuttyPath.Name = "chkUseCustomPuttyPath"
        Me.chkUseCustomPuttyPath.Size = New System.Drawing.Size(146, 17)
        Me.chkUseCustomPuttyPath.TabIndex = 30
        Me.chkUseCustomPuttyPath.Text = "Use custom PuTTY path:"
        Me.chkUseCustomPuttyPath.UseVisualStyleBackColor = True
        '
        'btnBrowseXulRunnerPath
        '
        Me.btnBrowseXulRunnerPath.Location = New System.Drawing.Point(291, 236)
        Me.btnBrowseXulRunnerPath.Name = "btnBrowseXulRunnerPath"
        Me.btnBrowseXulRunnerPath.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowseXulRunnerPath.TabIndex = 132
        Me.btnBrowseXulRunnerPath.Text = "Browse..."
        Me.btnBrowseXulRunnerPath.UseVisualStyleBackColor = True
        '
        'chkEncryptCompleteFile
        '
        Me.chkEncryptCompleteFile.AutoSize = True
        Me.chkEncryptCompleteFile.Location = New System.Drawing.Point(2, 26)
        Me.chkEncryptCompleteFile.Name = "chkEncryptCompleteFile"
        Me.chkEncryptCompleteFile.Size = New System.Drawing.Size(180, 17)
        Me.chkEncryptCompleteFile.TabIndex = 20
        Me.chkEncryptCompleteFile.Text = "Encrypt complete connection file"
        Me.chkEncryptCompleteFile.UseVisualStyleBackColor = True
        '
        'lblXulRunnerPath
        '
        Me.lblXulRunnerPath.AutoSize = True
        Me.lblXulRunnerPath.Location = New System.Drawing.Point(3, 220)
        Me.lblXulRunnerPath.Name = "lblXulRunnerPath"
        Me.lblXulRunnerPath.Size = New System.Drawing.Size(85, 13)
        Me.lblXulRunnerPath.TabIndex = 133
        Me.lblXulRunnerPath.Text = "XULrunner path:"
        '
        'chkAutomaticallyGetSessionInfo
        '
        Me.chkAutomaticallyGetSessionInfo.AutoSize = True
        Me.chkAutomaticallyGetSessionInfo.Location = New System.Drawing.Point(3, 49)
        Me.chkAutomaticallyGetSessionInfo.Name = "chkAutomaticallyGetSessionInfo"
        Me.chkAutomaticallyGetSessionInfo.Size = New System.Drawing.Size(198, 17)
        Me.chkAutomaticallyGetSessionInfo.TabIndex = 20
        Me.chkAutomaticallyGetSessionInfo.Text = "Automatically get session information"
        Me.chkAutomaticallyGetSessionInfo.UseVisualStyleBackColor = True
        '
        'chkWriteLogFile
        '
        Me.chkWriteLogFile.AutoSize = True
        Me.chkWriteLogFile.Location = New System.Drawing.Point(3, 3)
        Me.chkWriteLogFile.Name = "chkWriteLogFile"
        Me.chkWriteLogFile.Size = New System.Drawing.Size(171, 17)
        Me.chkWriteLogFile.TabIndex = 10
        Me.chkWriteLogFile.Text = "Write log file (mRemoteNG.log)"
        Me.chkWriteLogFile.UseVisualStyleBackColor = True
        '
        'pnlProxy
        '
        Me.pnlProxy.Controls.Add(Me.pnlProxyBasic)
        Me.pnlProxy.Controls.Add(Me.chkUseProxyForAutomaticUpdates)
        Me.pnlProxy.Controls.Add(Me.chkUseProxyAuthentication)
        Me.pnlProxy.Controls.Add(Me.pnlProxyAuthentication)
        Me.pnlProxy.Controls.Add(Me.btnTestProxy)
        Me.pnlProxy.Location = New System.Drawing.Point(3, 200)
        Me.pnlProxy.Name = "pnlProxy"
        Me.pnlProxy.Size = New System.Drawing.Size(536, 224)
        Me.pnlProxy.TabIndex = 134
        '
        'pnlProxyBasic
        '
        Me.pnlProxyBasic.Controls.Add(Me.lblProxyAddress)
        Me.pnlProxyBasic.Controls.Add(Me.txtProxyAddress)
        Me.pnlProxyBasic.Controls.Add(Me.lblProxyPort)
        Me.pnlProxyBasic.Controls.Add(Me.numProxyPort)
        Me.pnlProxyBasic.Enabled = False
        Me.pnlProxyBasic.Location = New System.Drawing.Point(8, 32)
        Me.pnlProxyBasic.Name = "pnlProxyBasic"
        Me.pnlProxyBasic.Size = New System.Drawing.Size(512, 40)
        Me.pnlProxyBasic.TabIndex = 90
        '
        'lblProxyAddress
        '
        Me.lblProxyAddress.Location = New System.Drawing.Point(8, 8)
        Me.lblProxyAddress.Name = "lblProxyAddress"
        Me.lblProxyAddress.Size = New System.Drawing.Size(96, 24)
        Me.lblProxyAddress.TabIndex = 1
        Me.lblProxyAddress.Text = "Address:"
        Me.lblProxyAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtProxyAddress
        '
        Me.txtProxyAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtProxyAddress.Location = New System.Drawing.Point(104, 8)
        Me.txtProxyAddress.Name = "txtProxyAddress"
        Me.txtProxyAddress.Size = New System.Drawing.Size(240, 20)
        Me.txtProxyAddress.TabIndex = 2
        '
        'lblProxyPort
        '
        Me.lblProxyPort.Location = New System.Drawing.Point(320, 8)
        Me.lblProxyPort.Name = "lblProxyPort"
        Me.lblProxyPort.Size = New System.Drawing.Size(64, 23)
        Me.lblProxyPort.TabIndex = 3
        Me.lblProxyPort.Text = "Port:"
        Me.lblProxyPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'numProxyPort
        '
        Me.numProxyPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.numProxyPort.Location = New System.Drawing.Point(384, 8)
        Me.numProxyPort.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
        Me.numProxyPort.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numProxyPort.Name = "numProxyPort"
        Me.numProxyPort.Size = New System.Drawing.Size(64, 20)
        Me.numProxyPort.TabIndex = 5001
        Me.numProxyPort.Value = New Decimal(New Integer() {80, 0, 0, 0})
        '
        'chkUseProxyForAutomaticUpdates
        '
        Me.chkUseProxyForAutomaticUpdates.AutoSize = True
        Me.chkUseProxyForAutomaticUpdates.Location = New System.Drawing.Point(8, 8)
        Me.chkUseProxyForAutomaticUpdates.Name = "chkUseProxyForAutomaticUpdates"
        Me.chkUseProxyForAutomaticUpdates.Size = New System.Drawing.Size(168, 17)
        Me.chkUseProxyForAutomaticUpdates.TabIndex = 80
        Me.chkUseProxyForAutomaticUpdates.Text = "Use a proxy server to connect"
        Me.chkUseProxyForAutomaticUpdates.UseVisualStyleBackColor = True
        '
        'chkUseProxyAuthentication
        '
        Me.chkUseProxyAuthentication.AutoSize = True
        Me.chkUseProxyAuthentication.Enabled = False
        Me.chkUseProxyAuthentication.Location = New System.Drawing.Point(32, 80)
        Me.chkUseProxyAuthentication.Name = "chkUseProxyAuthentication"
        Me.chkUseProxyAuthentication.Size = New System.Drawing.Size(216, 17)
        Me.chkUseProxyAuthentication.TabIndex = 100
        Me.chkUseProxyAuthentication.Text = "This proxy server requires authentication"
        Me.chkUseProxyAuthentication.UseVisualStyleBackColor = True
        '
        'pnlProxyAuthentication
        '
        Me.pnlProxyAuthentication.Controls.Add(Me.lblProxyUsername)
        Me.pnlProxyAuthentication.Controls.Add(Me.txtProxyUsername)
        Me.pnlProxyAuthentication.Controls.Add(Me.lblProxyPassword)
        Me.pnlProxyAuthentication.Controls.Add(Me.txtProxyPassword)
        Me.pnlProxyAuthentication.Enabled = False
        Me.pnlProxyAuthentication.Location = New System.Drawing.Point(8, 104)
        Me.pnlProxyAuthentication.Name = "pnlProxyAuthentication"
        Me.pnlProxyAuthentication.Size = New System.Drawing.Size(512, 72)
        Me.pnlProxyAuthentication.TabIndex = 110
        '
        'lblProxyUsername
        '
        Me.lblProxyUsername.Location = New System.Drawing.Point(8, 8)
        Me.lblProxyUsername.Name = "lblProxyUsername"
        Me.lblProxyUsername.Size = New System.Drawing.Size(96, 24)
        Me.lblProxyUsername.TabIndex = 1
        Me.lblProxyUsername.Text = "Username:"
        Me.lblProxyUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtProxyUsername
        '
        Me.txtProxyUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtProxyUsername.Location = New System.Drawing.Point(104, 8)
        Me.txtProxyUsername.Name = "txtProxyUsername"
        Me.txtProxyUsername.Size = New System.Drawing.Size(240, 20)
        Me.txtProxyUsername.TabIndex = 2
        '
        'lblProxyPassword
        '
        Me.lblProxyPassword.Location = New System.Drawing.Point(8, 40)
        Me.lblProxyPassword.Name = "lblProxyPassword"
        Me.lblProxyPassword.Size = New System.Drawing.Size(96, 24)
        Me.lblProxyPassword.TabIndex = 3
        Me.lblProxyPassword.Text = "Password:"
        Me.lblProxyPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtProxyPassword
        '
        Me.txtProxyPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtProxyPassword.Location = New System.Drawing.Point(104, 40)
        Me.txtProxyPassword.Name = "txtProxyPassword"
        Me.txtProxyPassword.Size = New System.Drawing.Size(240, 20)
        Me.txtProxyPassword.TabIndex = 4
        Me.txtProxyPassword.UseSystemPasswordChar = True
        '
        'btnTestProxy
        '
        Me.btnTestProxy.Location = New System.Drawing.Point(8, 184)
        Me.btnTestProxy.Name = "btnTestProxy"
        Me.btnTestProxy.Size = New System.Drawing.Size(100, 32)
        Me.btnTestProxy.TabIndex = 111
        Me.btnTestProxy.Text = "Test Proxy"
        Me.btnTestProxy.UseVisualStyleBackColor = True
        '
        'pnlUpdateCheck
        '
        Me.pnlUpdateCheck.Controls.Add(Me.btnUpdateCheckNow)
        Me.pnlUpdateCheck.Controls.Add(Me.chkCheckForUpdatesOnStartup)
        Me.pnlUpdateCheck.Controls.Add(Me.cboUpdateCheckFrequency)
        Me.pnlUpdateCheck.Location = New System.Drawing.Point(3, 48)
        Me.pnlUpdateCheck.Name = "pnlUpdateCheck"
        Me.pnlUpdateCheck.Size = New System.Drawing.Size(536, 120)
        Me.pnlUpdateCheck.TabIndex = 137
        '
        'btnUpdateCheckNow
        '
        Me.btnUpdateCheckNow.Location = New System.Drawing.Point(8, 80)
        Me.btnUpdateCheckNow.Name = "btnUpdateCheckNow"
        Me.btnUpdateCheckNow.Size = New System.Drawing.Size(100, 32)
        Me.btnUpdateCheckNow.TabIndex = 136
        Me.btnUpdateCheckNow.Text = "Check Now"
        Me.btnUpdateCheckNow.UseVisualStyleBackColor = True
        '
        'chkCheckForUpdatesOnStartup
        '
        Me.chkCheckForUpdatesOnStartup.AutoSize = True
        Me.chkCheckForUpdatesOnStartup.Location = New System.Drawing.Point(8, 8)
        Me.chkCheckForUpdatesOnStartup.Name = "chkCheckForUpdatesOnStartup"
        Me.chkCheckForUpdatesOnStartup.Size = New System.Drawing.Size(213, 17)
        Me.chkCheckForUpdatesOnStartup.TabIndex = 31
        Me.chkCheckForUpdatesOnStartup.Text = "Check for updates and announcements"
        Me.chkCheckForUpdatesOnStartup.UseVisualStyleBackColor = True
        '
        'cboUpdateCheckFrequency
        '
        Me.cboUpdateCheckFrequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboUpdateCheckFrequency.FormattingEnabled = True
        Me.cboUpdateCheckFrequency.Location = New System.Drawing.Point(48, 40)
        Me.cboUpdateCheckFrequency.Name = "cboUpdateCheckFrequency"
        Me.cboUpdateCheckFrequency.Size = New System.Drawing.Size(128, 21)
        Me.cboUpdateCheckFrequency.TabIndex = 135
        '
        'lblUpdatesExplanation
        '
        Me.lblUpdatesExplanation.Location = New System.Drawing.Point(3, 0)
        Me.lblUpdatesExplanation.Name = "lblUpdatesExplanation"
        Me.lblUpdatesExplanation.Size = New System.Drawing.Size(536, 40)
        Me.lblUpdatesExplanation.TabIndex = 136
        Me.lblUpdatesExplanation.Text = "mRemoteNG can periodically connect to the mRemoteNG website to check for updates " & _
            "and product announcements."
        '
        'chkHostnameLikeDisplayName
        '
        Me.chkHostnameLikeDisplayName.AutoSize = True
        Me.chkHostnameLikeDisplayName.Location = New System.Drawing.Point(3, 49)
        Me.chkHostnameLikeDisplayName.Name = "chkHostnameLikeDisplayName"
        Me.chkHostnameLikeDisplayName.Size = New System.Drawing.Size(328, 17)
        Me.chkHostnameLikeDisplayName.TabIndex = 30
        Me.chkHostnameLikeDisplayName.Text = "Set hostname like display name when creating new connections"
        Me.chkHostnameLikeDisplayName.UseVisualStyleBackColor = True
        '
        'pnlAutoSave
        '
        Me.pnlAutoSave.Controls.Add(Me.lblAutoSave1)
        Me.pnlAutoSave.Controls.Add(Me.numAutoSave)
        Me.pnlAutoSave.Controls.Add(Me.lblAutoSave2)
        Me.pnlAutoSave.Location = New System.Drawing.Point(3, 107)
        Me.pnlAutoSave.Name = "pnlAutoSave"
        Me.pnlAutoSave.Size = New System.Drawing.Size(500, 29)
        Me.pnlAutoSave.TabIndex = 50
        '
        'lblAutoSave1
        '
        Me.lblAutoSave1.AutoSize = True
        Me.lblAutoSave1.Location = New System.Drawing.Point(3, 9)
        Me.lblAutoSave1.Name = "lblAutoSave1"
        Me.lblAutoSave1.Size = New System.Drawing.Size(89, 13)
        Me.lblAutoSave1.TabIndex = 40
        Me.lblAutoSave1.Text = "Auto Save every:"
        '
        'numAutoSave
        '
        Me.numAutoSave.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.numAutoSave.Location = New System.Drawing.Point(168, 6)
        Me.numAutoSave.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        Me.numAutoSave.Name = "numAutoSave"
        Me.numAutoSave.Size = New System.Drawing.Size(53, 20)
        Me.numAutoSave.TabIndex = 51
        '
        'lblAutoSave2
        '
        Me.lblAutoSave2.AutoSize = True
        Me.lblAutoSave2.Location = New System.Drawing.Point(233, 9)
        Me.lblAutoSave2.Name = "lblAutoSave2"
        Me.lblAutoSave2.Size = New System.Drawing.Size(135, 13)
        Me.lblAutoSave2.TabIndex = 60
        Me.lblAutoSave2.Text = "Minutes (0 means disabled)"
        '
        'chkSingleClickOnOpenedConnectionSwitchesToIt
        '
        Me.chkSingleClickOnOpenedConnectionSwitchesToIt.AutoSize = True
        Me.chkSingleClickOnOpenedConnectionSwitchesToIt.Location = New System.Drawing.Point(3, 26)
        Me.chkSingleClickOnOpenedConnectionSwitchesToIt.Name = "chkSingleClickOnOpenedConnectionSwitchesToIt"
        Me.chkSingleClickOnOpenedConnectionSwitchesToIt.Size = New System.Drawing.Size(254, 17)
        Me.chkSingleClickOnOpenedConnectionSwitchesToIt.TabIndex = 20
        Me.chkSingleClickOnOpenedConnectionSwitchesToIt.Text = "Single click on opened connection switches to it"
        Me.chkSingleClickOnOpenedConnectionSwitchesToIt.UseVisualStyleBackColor = True
        '
        'chkSingleClickOnConnectionOpensIt
        '
        Me.chkSingleClickOnConnectionOpensIt.AutoSize = True
        Me.chkSingleClickOnConnectionOpensIt.Location = New System.Drawing.Point(3, 3)
        Me.chkSingleClickOnConnectionOpensIt.Name = "chkSingleClickOnConnectionOpensIt"
        Me.chkSingleClickOnConnectionOpensIt.Size = New System.Drawing.Size(191, 17)
        Me.chkSingleClickOnConnectionOpensIt.TabIndex = 10
        Me.chkSingleClickOnConnectionOpensIt.Text = "Single click on connection opens it"
        Me.chkSingleClickOnConnectionOpensIt.UseVisualStyleBackColor = True
        '
        'pnlRdpReconnectionCount
        '
        Me.pnlRdpReconnectionCount.Controls.Add(Me.lblRdpReconnectionCount)
        Me.pnlRdpReconnectionCount.Controls.Add(Me.numRdpReconnectionCount)
        Me.pnlRdpReconnectionCount.Location = New System.Drawing.Point(3, 72)
        Me.pnlRdpReconnectionCount.Name = "pnlRdpReconnectionCount"
        Me.pnlRdpReconnectionCount.Size = New System.Drawing.Size(500, 29)
        Me.pnlRdpReconnectionCount.TabIndex = 40
        '
        'lblRdpReconnectionCount
        '
        Me.lblRdpReconnectionCount.AutoSize = True
        Me.lblRdpReconnectionCount.Location = New System.Drawing.Point(3, 9)
        Me.lblRdpReconnectionCount.Name = "lblRdpReconnectionCount"
        Me.lblRdpReconnectionCount.Size = New System.Drawing.Size(131, 13)
        Me.lblRdpReconnectionCount.TabIndex = 40
        Me.lblRdpReconnectionCount.Text = "RDP Reconnection Count"
        '
        'numRdpReconnectionCount
        '
        Me.numRdpReconnectionCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.numRdpReconnectionCount.Location = New System.Drawing.Point(168, 6)
        Me.numRdpReconnectionCount.Maximum = New Decimal(New Integer() {20, 0, 0, 0})
        Me.numRdpReconnectionCount.Name = "numRdpReconnectionCount"
        Me.numRdpReconnectionCount.Size = New System.Drawing.Size(53, 20)
        Me.numRdpReconnectionCount.TabIndex = 41
        Me.numRdpReconnectionCount.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'chkAlwaysShowPanelSelectionDlg
        '
        Me.chkAlwaysShowPanelSelectionDlg.AutoSize = True
        Me.chkAlwaysShowPanelSelectionDlg.Location = New System.Drawing.Point(3, 95)
        Me.chkAlwaysShowPanelSelectionDlg.Name = "chkAlwaysShowPanelSelectionDlg"
        Me.chkAlwaysShowPanelSelectionDlg.Size = New System.Drawing.Size(317, 17)
        Me.chkAlwaysShowPanelSelectionDlg.TabIndex = 50
        Me.chkAlwaysShowPanelSelectionDlg.Text = "Always show panel selection dialog when opening connectins"
        Me.chkAlwaysShowPanelSelectionDlg.UseVisualStyleBackColor = True
        '
        'chkShowLogonInfoOnTabs
        '
        Me.chkShowLogonInfoOnTabs.AutoSize = True
        Me.chkShowLogonInfoOnTabs.Location = New System.Drawing.Point(3, 26)
        Me.chkShowLogonInfoOnTabs.Name = "chkShowLogonInfoOnTabs"
        Me.chkShowLogonInfoOnTabs.Size = New System.Drawing.Size(203, 17)
        Me.chkShowLogonInfoOnTabs.TabIndex = 20
        Me.chkShowLogonInfoOnTabs.Text = "Show logon information on tab names"
        Me.chkShowLogonInfoOnTabs.UseVisualStyleBackColor = True
        '
        'chkDoubleClickClosesTab
        '
        Me.chkDoubleClickClosesTab.AutoSize = True
        Me.chkDoubleClickClosesTab.Location = New System.Drawing.Point(3, 72)
        Me.chkDoubleClickClosesTab.Name = "chkDoubleClickClosesTab"
        Me.chkDoubleClickClosesTab.Size = New System.Drawing.Size(159, 17)
        Me.chkDoubleClickClosesTab.TabIndex = 40
        Me.chkDoubleClickClosesTab.Text = "Double click on tab closes it"
        Me.chkDoubleClickClosesTab.UseVisualStyleBackColor = True
        '
        'chkShowProtocolOnTabs
        '
        Me.chkShowProtocolOnTabs.AutoSize = True
        Me.chkShowProtocolOnTabs.Location = New System.Drawing.Point(3, 49)
        Me.chkShowProtocolOnTabs.Name = "chkShowProtocolOnTabs"
        Me.chkShowProtocolOnTabs.Size = New System.Drawing.Size(166, 17)
        Me.chkShowProtocolOnTabs.TabIndex = 30
        Me.chkShowProtocolOnTabs.Text = "Show protocols on tab names"
        Me.chkShowProtocolOnTabs.UseVisualStyleBackColor = True
        '
        'chkOpenNewTabRightOfSelected
        '
        Me.chkOpenNewTabRightOfSelected.AutoSize = True
        Me.chkOpenNewTabRightOfSelected.Location = New System.Drawing.Point(3, 3)
        Me.chkOpenNewTabRightOfSelected.Name = "chkOpenNewTabRightOfSelected"
        Me.chkOpenNewTabRightOfSelected.Size = New System.Drawing.Size(280, 17)
        Me.chkOpenNewTabRightOfSelected.TabIndex = 10
        Me.chkOpenNewTabRightOfSelected.Text = "Open new tab to the right of the currently selected tab"
        Me.chkOpenNewTabRightOfSelected.UseVisualStyleBackColor = True
        '
        'chkMCWarnings
        '
        Me.chkMCWarnings.AutoSize = True
        Me.chkMCWarnings.Enabled = False
        Me.chkMCWarnings.Location = New System.Drawing.Point(126, 191)
        Me.chkMCWarnings.Name = "chkMCWarnings"
        Me.chkMCWarnings.Size = New System.Drawing.Size(71, 17)
        Me.chkMCWarnings.TabIndex = 54
        Me.chkMCWarnings.Text = "Warnings"
        Me.chkMCWarnings.UseVisualStyleBackColor = True
        '
        'chkMCErrors
        '
        Me.chkMCErrors.AutoSize = True
        Me.chkMCErrors.Enabled = False
        Me.chkMCErrors.Location = New System.Drawing.Point(217, 191)
        Me.chkMCErrors.Name = "chkMCErrors"
        Me.chkMCErrors.Size = New System.Drawing.Size(53, 17)
        Me.chkMCErrors.TabIndex = 55
        Me.chkMCErrors.Text = "Errors"
        Me.chkMCErrors.UseVisualStyleBackColor = True
        '
        'chkMCInformation
        '
        Me.chkMCInformation.AutoSize = True
        Me.chkMCInformation.Enabled = False
        Me.chkMCInformation.Location = New System.Drawing.Point(19, 191)
        Me.chkMCInformation.Name = "chkMCInformation"
        Me.chkMCInformation.Size = New System.Drawing.Size(83, 17)
        Me.chkMCInformation.TabIndex = 53
        Me.chkMCInformation.Text = "Informations"
        Me.chkMCInformation.UseVisualStyleBackColor = True
        '
        'lblSwitchToErrorsAndInfos
        '
        Me.lblSwitchToErrorsAndInfos.AutoSize = True
        Me.lblSwitchToErrorsAndInfos.Location = New System.Drawing.Point(3, 171)
        Me.lblSwitchToErrorsAndInfos.Name = "lblSwitchToErrorsAndInfos"
        Me.lblSwitchToErrorsAndInfos.Size = New System.Drawing.Size(159, 13)
        Me.lblSwitchToErrorsAndInfos.TabIndex = 52
        Me.lblSwitchToErrorsAndInfos.Text = "Switch to Notifications panel on:"
        '
        'chkUseOnlyErrorsAndInfosPanel
        '
        Me.chkUseOnlyErrorsAndInfosPanel.AutoSize = True
        Me.chkUseOnlyErrorsAndInfosPanel.Location = New System.Drawing.Point(3, 146)
        Me.chkUseOnlyErrorsAndInfosPanel.Name = "chkUseOnlyErrorsAndInfosPanel"
        Me.chkUseOnlyErrorsAndInfosPanel.Size = New System.Drawing.Size(278, 17)
        Me.chkUseOnlyErrorsAndInfosPanel.TabIndex = 51
        Me.chkUseOnlyErrorsAndInfosPanel.Text = "Use only Notifications panel (no messagebox popups)"
        Me.chkUseOnlyErrorsAndInfosPanel.UseVisualStyleBackColor = True
        '
        'chkShowFullConnectionsFilePathInTitle
        '
        Me.chkShowFullConnectionsFilePathInTitle.AutoSize = True
        Me.chkShowFullConnectionsFilePathInTitle.Location = New System.Drawing.Point(3, 128)
        Me.chkShowFullConnectionsFilePathInTitle.Name = "chkShowFullConnectionsFilePathInTitle"
        Me.chkShowFullConnectionsFilePathInTitle.Size = New System.Drawing.Size(239, 17)
        Me.chkShowFullConnectionsFilePathInTitle.TabIndex = 20
        Me.chkShowFullConnectionsFilePathInTitle.Text = "Show full connections file path in window title"
        Me.chkShowFullConnectionsFilePathInTitle.UseVisualStyleBackColor = True
        '
        'chkShowSystemTrayIcon
        '
        Me.chkShowSystemTrayIcon.AutoSize = True
        Me.chkShowSystemTrayIcon.Location = New System.Drawing.Point(3, 176)
        Me.chkShowSystemTrayIcon.Name = "chkShowSystemTrayIcon"
        Me.chkShowSystemTrayIcon.Size = New System.Drawing.Size(172, 17)
        Me.chkShowSystemTrayIcon.TabIndex = 30
        Me.chkShowSystemTrayIcon.Text = "Always show System Tray Icon"
        Me.chkShowSystemTrayIcon.UseVisualStyleBackColor = True
        '
        'chkMinimizeToSystemTray
        '
        Me.chkMinimizeToSystemTray.AutoSize = True
        Me.chkMinimizeToSystemTray.Location = New System.Drawing.Point(3, 200)
        Me.chkMinimizeToSystemTray.Name = "chkMinimizeToSystemTray"
        Me.chkMinimizeToSystemTray.Size = New System.Drawing.Size(139, 17)
        Me.chkMinimizeToSystemTray.TabIndex = 40
        Me.chkMinimizeToSystemTray.Text = "Minimize to System Tray"
        Me.chkMinimizeToSystemTray.UseVisualStyleBackColor = True
        '
        'chkShowDescriptionTooltipsInTree
        '
        Me.chkShowDescriptionTooltipsInTree.AutoSize = True
        Me.chkShowDescriptionTooltipsInTree.Location = New System.Drawing.Point(3, 104)
        Me.chkShowDescriptionTooltipsInTree.Name = "chkShowDescriptionTooltipsInTree"
        Me.chkShowDescriptionTooltipsInTree.Size = New System.Drawing.Size(231, 17)
        Me.chkShowDescriptionTooltipsInTree.TabIndex = 10
        Me.chkShowDescriptionTooltipsInTree.Text = "Show description tooltips in connection tree"
        Me.chkShowDescriptionTooltipsInTree.UseVisualStyleBackColor = True
        '
        'lblLanguage
        '
        Me.lblLanguage.AutoSize = True
        Me.lblLanguage.Location = New System.Drawing.Point(3, 0)
        Me.lblLanguage.Name = "lblLanguage"
        Me.lblLanguage.Size = New System.Drawing.Size(55, 13)
        Me.lblLanguage.TabIndex = 41
        Me.lblLanguage.Text = "Language"
        '
        'cboLanguage
        '
        Me.cboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLanguage.FormattingEnabled = True
        Me.cboLanguage.Location = New System.Drawing.Point(3, 24)
        Me.cboLanguage.Name = "cboLanguage"
        Me.cboLanguage.Size = New System.Drawing.Size(304, 21)
        Me.cboLanguage.Sorted = True
        Me.cboLanguage.TabIndex = 42
        '
        'lblLanguageRestartRequired
        '
        Me.lblLanguageRestartRequired.AutoSize = True
        Me.lblLanguageRestartRequired.Location = New System.Drawing.Point(3, 56)
        Me.lblLanguageRestartRequired.Name = "lblLanguageRestartRequired"
        Me.lblLanguageRestartRequired.Size = New System.Drawing.Size(380, 13)
        Me.lblLanguageRestartRequired.TabIndex = 43
        Me.lblLanguageRestartRequired.Text = "mRemoteNG must be restarted before changes to the language will take effect."
        '
        'chkReconnectOnStart
        '
        Me.chkReconnectOnStart.AutoSize = True
        Me.chkReconnectOnStart.Location = New System.Drawing.Point(3, 75)
        Me.chkReconnectOnStart.Name = "chkReconnectOnStart"
        Me.chkReconnectOnStart.Size = New System.Drawing.Size(273, 17)
        Me.chkReconnectOnStart.TabIndex = 40
        Me.chkReconnectOnStart.Text = "Reconnect to previously opened sessions on startup"
        Me.chkReconnectOnStart.UseVisualStyleBackColor = True
        '
        'chkSingleInstance
        '
        Me.chkSingleInstance.AutoSize = True
        Me.chkSingleInstance.Location = New System.Drawing.Point(3, 99)
        Me.chkSingleInstance.Name = "chkSingleInstance"
        Me.chkSingleInstance.Size = New System.Drawing.Size(366, 17)
        Me.chkSingleInstance.TabIndex = 50
        Me.chkSingleInstance.Text = "Allow only a single instance of the application (mRemote restart required)"
        Me.chkSingleInstance.UseVisualStyleBackColor = True
        '
        'chkConfirmExit
        '
        Me.chkConfirmExit.AutoSize = True
        Me.chkConfirmExit.Location = New System.Drawing.Point(3, 27)
        Me.chkConfirmExit.Name = "chkConfirmExit"
        Me.chkConfirmExit.Size = New System.Drawing.Size(221, 17)
        Me.chkConfirmExit.TabIndex = 20
        Me.chkConfirmExit.Text = "Confirm exit if there are open connections"
        Me.chkConfirmExit.UseVisualStyleBackColor = True
        '
        'chkProperInstallationOfComponentsAtStartup
        '
        Me.chkProperInstallationOfComponentsAtStartup.AutoSize = True
        Me.chkProperInstallationOfComponentsAtStartup.Location = New System.Drawing.Point(3, 123)
        Me.chkProperInstallationOfComponentsAtStartup.Name = "chkProperInstallationOfComponentsAtStartup"
        Me.chkProperInstallationOfComponentsAtStartup.Size = New System.Drawing.Size(262, 17)
        Me.chkProperInstallationOfComponentsAtStartup.TabIndex = 50
        Me.chkProperInstallationOfComponentsAtStartup.Text = "Check proper installation of components at startup"
        Me.chkProperInstallationOfComponentsAtStartup.UseVisualStyleBackColor = True
        '
        'chkSaveConsOnExit
        '
        Me.chkSaveConsOnExit.AutoSize = True
        Me.chkSaveConsOnExit.Location = New System.Drawing.Point(3, 51)
        Me.chkSaveConsOnExit.Name = "chkSaveConsOnExit"
        Me.chkSaveConsOnExit.Size = New System.Drawing.Size(146, 17)
        Me.chkSaveConsOnExit.TabIndex = 10
        Me.chkSaveConsOnExit.Text = "Save connections on exit"
        Me.chkSaveConsOnExit.UseVisualStyleBackColor = True
        '
        'chkConfirmCloseConnection
        '
        Me.chkConfirmCloseConnection.AutoSize = True
        Me.chkConfirmCloseConnection.Location = New System.Drawing.Point(3, 3)
        Me.chkConfirmCloseConnection.Name = "chkConfirmCloseConnection"
        Me.chkConfirmCloseConnection.Size = New System.Drawing.Size(176, 17)
        Me.chkConfirmCloseConnection.TabIndex = 51
        Me.chkConfirmCloseConnection.Text = "Confirm closing connection tabs"
        Me.chkConfirmCloseConnection.UseVisualStyleBackColor = True
        '
        'tcTabControl
        '
        Me.tcTabControl.Controls.Add(Me.tabStartupExit)
        Me.tcTabControl.Controls.Add(Me.tabAppearance)
        Me.tcTabControl.Controls.Add(Me.tabTabsAndPanels)
        Me.tcTabControl.Controls.Add(Me.tabConnections)
        Me.tcTabControl.Controls.Add(Me.tabSQLServer)
        Me.tcTabControl.Controls.Add(Me.tabUpdates)
        Me.tcTabControl.Controls.Add(Me.tabAdvanced)
        Me.tcTabControl.Location = New System.Drawing.Point(172, 12)
        Me.tcTabControl.Name = "tcTabControl"
        Me.tcTabControl.SelectedIndex = 0
        Me.tcTabControl.Size = New System.Drawing.Size(610, 489)
        Me.tcTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tcTabControl.TabIndex = 5001
        '
        'tabStartupExit
        '
        Me.tabStartupExit.Controls.Add(Me.chkConfirmCloseConnection)
        Me.tabStartupExit.Controls.Add(Me.chkReconnectOnStart)
        Me.tabStartupExit.Controls.Add(Me.chkSaveConsOnExit)
        Me.tabStartupExit.Controls.Add(Me.chkSingleInstance)
        Me.tabStartupExit.Controls.Add(Me.chkProperInstallationOfComponentsAtStartup)
        Me.tabStartupExit.Controls.Add(Me.chkConfirmExit)
        Me.tabStartupExit.Location = New System.Drawing.Point(4, 22)
        Me.tabStartupExit.Name = "tabStartupExit"
        Me.tabStartupExit.Size = New System.Drawing.Size(602, 463)
        Me.tabStartupExit.TabIndex = 0
        Me.tabStartupExit.Text = "Startup/Exit"
        Me.tabStartupExit.UseVisualStyleBackColor = True
        '
        'tabAppearance
        '
        Me.tabAppearance.Controls.Add(Me.lblLanguageRestartRequired)
        Me.tabAppearance.Controls.Add(Me.cboLanguage)
        Me.tabAppearance.Controls.Add(Me.lblLanguage)
        Me.tabAppearance.Controls.Add(Me.chkShowFullConnectionsFilePathInTitle)
        Me.tabAppearance.Controls.Add(Me.chkShowDescriptionTooltipsInTree)
        Me.tabAppearance.Controls.Add(Me.chkShowSystemTrayIcon)
        Me.tabAppearance.Controls.Add(Me.chkMinimizeToSystemTray)
        Me.tabAppearance.Location = New System.Drawing.Point(4, 22)
        Me.tabAppearance.Name = "tabAppearance"
        Me.tabAppearance.Size = New System.Drawing.Size(602, 463)
        Me.tabAppearance.TabIndex = 1
        Me.tabAppearance.Text = "Appearance"
        Me.tabAppearance.UseVisualStyleBackColor = True
        '
        'tabTabsAndPanels
        '
        Me.tabTabsAndPanels.Controls.Add(Me.chkUseOnlyErrorsAndInfosPanel)
        Me.tabTabsAndPanels.Controls.Add(Me.chkOpenNewTabRightOfSelected)
        Me.tabTabsAndPanels.Controls.Add(Me.lblSwitchToErrorsAndInfos)
        Me.tabTabsAndPanels.Controls.Add(Me.chkAlwaysShowPanelSelectionDlg)
        Me.tabTabsAndPanels.Controls.Add(Me.chkMCInformation)
        Me.tabTabsAndPanels.Controls.Add(Me.chkShowLogonInfoOnTabs)
        Me.tabTabsAndPanels.Controls.Add(Me.chkMCErrors)
        Me.tabTabsAndPanels.Controls.Add(Me.chkDoubleClickClosesTab)
        Me.tabTabsAndPanels.Controls.Add(Me.chkMCWarnings)
        Me.tabTabsAndPanels.Controls.Add(Me.chkShowProtocolOnTabs)
        Me.tabTabsAndPanels.Location = New System.Drawing.Point(4, 22)
        Me.tabTabsAndPanels.Name = "tabTabsAndPanels"
        Me.tabTabsAndPanels.Size = New System.Drawing.Size(602, 463)
        Me.tabTabsAndPanels.TabIndex = 2
        Me.tabTabsAndPanels.Text = "Tabs & Panels"
        Me.tabTabsAndPanels.UseVisualStyleBackColor = True
        '
        'tabConnections
        '
        Me.tabConnections.Controls.Add(Me.pnlRdpReconnectionCount)
        Me.tabConnections.Controls.Add(Me.chkSingleClickOnConnectionOpensIt)
        Me.tabConnections.Controls.Add(Me.chkHostnameLikeDisplayName)
        Me.tabConnections.Controls.Add(Me.pnlDefaultCredentials)
        Me.tabConnections.Controls.Add(Me.chkSingleClickOnOpenedConnectionSwitchesToIt)
        Me.tabConnections.Controls.Add(Me.pnlAutoSave)
        Me.tabConnections.Location = New System.Drawing.Point(4, 22)
        Me.tabConnections.Name = "tabConnections"
        Me.tabConnections.Size = New System.Drawing.Size(602, 463)
        Me.tabConnections.TabIndex = 3
        Me.tabConnections.Text = "Connections"
        Me.tabConnections.UseVisualStyleBackColor = True
        '
        'pnlDefaultCredentials
        '
        Me.pnlDefaultCredentials.Controls.Add(Me.radCredentialsCustom)
        Me.pnlDefaultCredentials.Controls.Add(Me.lblDefaultCredentials)
        Me.pnlDefaultCredentials.Controls.Add(Me.radCredentialsNoInfo)
        Me.pnlDefaultCredentials.Controls.Add(Me.radCredentialsWindows)
        Me.pnlDefaultCredentials.Controls.Add(Me.txtCredentialsDomain)
        Me.pnlDefaultCredentials.Controls.Add(Me.lblCredentialsUsername)
        Me.pnlDefaultCredentials.Controls.Add(Me.txtCredentialsPassword)
        Me.pnlDefaultCredentials.Controls.Add(Me.lblCredentialsPassword)
        Me.pnlDefaultCredentials.Controls.Add(Me.txtCredentialsUsername)
        Me.pnlDefaultCredentials.Controls.Add(Me.lblCredentialsDomain)
        Me.pnlDefaultCredentials.Location = New System.Drawing.Point(3, 142)
        Me.pnlDefaultCredentials.Name = "pnlDefaultCredentials"
        Me.pnlDefaultCredentials.Size = New System.Drawing.Size(596, 175)
        Me.pnlDefaultCredentials.TabIndex = 72
        '
        'radCredentialsCustom
        '
        Me.radCredentialsCustom.AutoSize = True
        Me.radCredentialsCustom.Location = New System.Drawing.Point(16, 69)
        Me.radCredentialsCustom.Name = "radCredentialsCustom"
        Me.radCredentialsCustom.Size = New System.Drawing.Size(87, 17)
        Me.radCredentialsCustom.TabIndex = 73
        Me.radCredentialsCustom.Text = "the following:"
        Me.radCredentialsCustom.UseVisualStyleBackColor = True
        '
        'lblDefaultCredentials
        '
        Me.lblDefaultCredentials.AutoSize = True
        Me.lblDefaultCredentials.Location = New System.Drawing.Point(3, 9)
        Me.lblDefaultCredentials.Name = "lblDefaultCredentials"
        Me.lblDefaultCredentials.Size = New System.Drawing.Size(257, 13)
        Me.lblDefaultCredentials.TabIndex = 80
        Me.lblDefaultCredentials.Text = "For empty Username, Password or Domain fields use:"
        '
        'radCredentialsNoInfo
        '
        Me.radCredentialsNoInfo.AutoSize = True
        Me.radCredentialsNoInfo.Checked = True
        Me.radCredentialsNoInfo.Location = New System.Drawing.Point(16, 31)
        Me.radCredentialsNoInfo.Name = "radCredentialsNoInfo"
        Me.radCredentialsNoInfo.Size = New System.Drawing.Size(91, 17)
        Me.radCredentialsNoInfo.TabIndex = 71
        Me.radCredentialsNoInfo.TabStop = True
        Me.radCredentialsNoInfo.Text = "no information"
        Me.radCredentialsNoInfo.UseVisualStyleBackColor = True
        '
        'radCredentialsWindows
        '
        Me.radCredentialsWindows.AutoSize = True
        Me.radCredentialsWindows.Location = New System.Drawing.Point(16, 50)
        Me.radCredentialsWindows.Name = "radCredentialsWindows"
        Me.radCredentialsWindows.Size = New System.Drawing.Size(227, 17)
        Me.radCredentialsWindows.TabIndex = 72
        Me.radCredentialsWindows.Text = "my current credentials (windows logon info)"
        Me.radCredentialsWindows.UseVisualStyleBackColor = True
        '
        'txtCredentialsDomain
        '
        Me.txtCredentialsDomain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCredentialsDomain.Enabled = False
        Me.txtCredentialsDomain.Location = New System.Drawing.Point(125, 147)
        Me.txtCredentialsDomain.Name = "txtCredentialsDomain"
        Me.txtCredentialsDomain.Size = New System.Drawing.Size(150, 20)
        Me.txtCredentialsDomain.TabIndex = 76
        '
        'lblCredentialsUsername
        '
        Me.lblCredentialsUsername.AutoSize = True
        Me.lblCredentialsUsername.Enabled = False
        Me.lblCredentialsUsername.Location = New System.Drawing.Point(34, 95)
        Me.lblCredentialsUsername.Name = "lblCredentialsUsername"
        Me.lblCredentialsUsername.Size = New System.Drawing.Size(58, 13)
        Me.lblCredentialsUsername.TabIndex = 120
        Me.lblCredentialsUsername.Text = "Username:"
        '
        'txtCredentialsPassword
        '
        Me.txtCredentialsPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCredentialsPassword.Enabled = False
        Me.txtCredentialsPassword.Location = New System.Drawing.Point(125, 120)
        Me.txtCredentialsPassword.Name = "txtCredentialsPassword"
        Me.txtCredentialsPassword.Size = New System.Drawing.Size(150, 20)
        Me.txtCredentialsPassword.TabIndex = 75
        Me.txtCredentialsPassword.UseSystemPasswordChar = True
        '
        'lblCredentialsPassword
        '
        Me.lblCredentialsPassword.AutoSize = True
        Me.lblCredentialsPassword.Enabled = False
        Me.lblCredentialsPassword.Location = New System.Drawing.Point(34, 123)
        Me.lblCredentialsPassword.Name = "lblCredentialsPassword"
        Me.lblCredentialsPassword.Size = New System.Drawing.Size(56, 13)
        Me.lblCredentialsPassword.TabIndex = 140
        Me.lblCredentialsPassword.Text = "Password:"
        '
        'txtCredentialsUsername
        '
        Me.txtCredentialsUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCredentialsUsername.Enabled = False
        Me.txtCredentialsUsername.Location = New System.Drawing.Point(125, 93)
        Me.txtCredentialsUsername.Name = "txtCredentialsUsername"
        Me.txtCredentialsUsername.Size = New System.Drawing.Size(150, 20)
        Me.txtCredentialsUsername.TabIndex = 74
        '
        'lblCredentialsDomain
        '
        Me.lblCredentialsDomain.AutoSize = True
        Me.lblCredentialsDomain.Enabled = False
        Me.lblCredentialsDomain.Location = New System.Drawing.Point(34, 150)
        Me.lblCredentialsDomain.Name = "lblCredentialsDomain"
        Me.lblCredentialsDomain.Size = New System.Drawing.Size(46, 13)
        Me.lblCredentialsDomain.TabIndex = 160
        Me.lblCredentialsDomain.Text = "Domain:"
        '
        'tabSQLServer
        '
        Me.tabSQLServer.Controls.Add(Me.lblSQLDatabaseName)
        Me.tabSQLServer.Controls.Add(Me.txtSQLDatabaseName)
        Me.tabSQLServer.Controls.Add(Me.lblExperimental)
        Me.tabSQLServer.Controls.Add(Me.chkUseSQLServer)
        Me.tabSQLServer.Controls.Add(Me.lblSQLUsername)
        Me.tabSQLServer.Controls.Add(Me.txtSQLPassword)
        Me.tabSQLServer.Controls.Add(Me.lblSQLInfo)
        Me.tabSQLServer.Controls.Add(Me.lblSQLServer)
        Me.tabSQLServer.Controls.Add(Me.txtSQLUsername)
        Me.tabSQLServer.Controls.Add(Me.txtSQLServer)
        Me.tabSQLServer.Controls.Add(Me.lblSQLPassword)
        Me.tabSQLServer.Location = New System.Drawing.Point(4, 22)
        Me.tabSQLServer.Name = "tabSQLServer"
        Me.tabSQLServer.Size = New System.Drawing.Size(602, 463)
        Me.tabSQLServer.TabIndex = 6
        Me.tabSQLServer.Text = "SQL Server"
        Me.tabSQLServer.UseVisualStyleBackColor = True
        '
        'lblExperimental
        '
        Me.lblExperimental.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblExperimental.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World)
        Me.lblExperimental.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblExperimental.Location = New System.Drawing.Point(3, 0)
        Me.lblExperimental.Name = "lblExperimental"
        Me.lblExperimental.Size = New System.Drawing.Size(596, 25)
        Me.lblExperimental.TabIndex = 121
        Me.lblExperimental.Text = "EXPERIMENTAL"
        Me.lblExperimental.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'chkUseSQLServer
        '
        Me.chkUseSQLServer.AutoSize = True
        Me.chkUseSQLServer.Location = New System.Drawing.Point(3, 76)
        Me.chkUseSQLServer.Name = "chkUseSQLServer"
        Me.chkUseSQLServer.Size = New System.Drawing.Size(234, 17)
        Me.chkUseSQLServer.TabIndex = 61
        Me.chkUseSQLServer.Text = "Use SQL Server to load && save connections"
        Me.chkUseSQLServer.UseVisualStyleBackColor = True
        '
        'lblSQLUsername
        '
        Me.lblSQLUsername.AutoSize = True
        Me.lblSQLUsername.Enabled = False
        Me.lblSQLUsername.Location = New System.Drawing.Point(23, 158)
        Me.lblSQLUsername.Name = "lblSQLUsername"
        Me.lblSQLUsername.Size = New System.Drawing.Size(58, 13)
        Me.lblSQLUsername.TabIndex = 80
        Me.lblSQLUsername.Text = "Username:"
        '
        'txtSQLPassword
        '
        Me.txtSQLPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSQLPassword.Enabled = False
        Me.txtSQLPassword.Location = New System.Drawing.Point(113, 182)
        Me.txtSQLPassword.Name = "txtSQLPassword"
        Me.txtSQLPassword.Size = New System.Drawing.Size(153, 20)
        Me.txtSQLPassword.TabIndex = 64
        Me.txtSQLPassword.UseSystemPasswordChar = True
        '
        'lblSQLInfo
        '
        Me.lblSQLInfo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSQLInfo.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World)
        Me.lblSQLInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblSQLInfo.Location = New System.Drawing.Point(3, 25)
        Me.lblSQLInfo.Name = "lblSQLInfo"
        Me.lblSQLInfo.Size = New System.Drawing.Size(596, 25)
        Me.lblSQLInfo.TabIndex = 120
        Me.lblSQLInfo.Text = "Please see Help - Getting started - SQL Configuration for more Info!"
        Me.lblSQLInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSQLServer
        '
        Me.lblSQLServer.AutoSize = True
        Me.lblSQLServer.Enabled = False
        Me.lblSQLServer.Location = New System.Drawing.Point(23, 106)
        Me.lblSQLServer.Name = "lblSQLServer"
        Me.lblSQLServer.Size = New System.Drawing.Size(65, 13)
        Me.lblSQLServer.TabIndex = 60
        Me.lblSQLServer.Text = "SQL Server:"
        '
        'txtSQLUsername
        '
        Me.txtSQLUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSQLUsername.Enabled = False
        Me.txtSQLUsername.Location = New System.Drawing.Point(113, 155)
        Me.txtSQLUsername.Name = "txtSQLUsername"
        Me.txtSQLUsername.Size = New System.Drawing.Size(153, 20)
        Me.txtSQLUsername.TabIndex = 63
        '
        'txtSQLServer
        '
        Me.txtSQLServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSQLServer.Enabled = False
        Me.txtSQLServer.Location = New System.Drawing.Point(113, 103)
        Me.txtSQLServer.Name = "txtSQLServer"
        Me.txtSQLServer.Size = New System.Drawing.Size(153, 20)
        Me.txtSQLServer.TabIndex = 62
        '
        'lblSQLPassword
        '
        Me.lblSQLPassword.AutoSize = True
        Me.lblSQLPassword.Enabled = False
        Me.lblSQLPassword.Location = New System.Drawing.Point(23, 185)
        Me.lblSQLPassword.Name = "lblSQLPassword"
        Me.lblSQLPassword.Size = New System.Drawing.Size(56, 13)
        Me.lblSQLPassword.TabIndex = 100
        Me.lblSQLPassword.Text = "Password:"
        '
        'tabUpdates
        '
        Me.tabUpdates.Controls.Add(Me.lblUpdatesExplanation)
        Me.tabUpdates.Controls.Add(Me.pnlUpdateCheck)
        Me.tabUpdates.Controls.Add(Me.pnlProxy)
        Me.tabUpdates.Location = New System.Drawing.Point(4, 22)
        Me.tabUpdates.Name = "tabUpdates"
        Me.tabUpdates.Size = New System.Drawing.Size(602, 463)
        Me.tabUpdates.TabIndex = 4
        Me.tabUpdates.Text = "Updates"
        Me.tabUpdates.UseVisualStyleBackColor = True
        '
        'tabAdvanced
        '
        Me.tabAdvanced.Controls.Add(Me.chkWriteLogFile)
        Me.tabAdvanced.Controls.Add(Me.chkAutomaticallyGetSessionInfo)
        Me.tabAdvanced.Controls.Add(Me.lblXulRunnerPath)
        Me.tabAdvanced.Controls.Add(Me.lblMaximumPuttyWaitTime)
        Me.tabAdvanced.Controls.Add(Me.chkEncryptCompleteFile)
        Me.tabAdvanced.Controls.Add(Me.chkAutomaticReconnect)
        Me.tabAdvanced.Controls.Add(Me.btnBrowseXulRunnerPath)
        Me.tabAdvanced.Controls.Add(Me.numPuttyWaitTime)
        Me.tabAdvanced.Controls.Add(Me.chkUseCustomPuttyPath)
        Me.tabAdvanced.Controls.Add(Me.lblConfigurePuttySessions)
        Me.tabAdvanced.Controls.Add(Me.txtXULrunnerPath)
        Me.tabAdvanced.Controls.Add(Me.numUVNCSCPort)
        Me.tabAdvanced.Controls.Add(Me.txtCustomPuttyPath)
        Me.tabAdvanced.Controls.Add(Me.btnLaunchPutty)
        Me.tabAdvanced.Controls.Add(Me.lblUVNCSCPort)
        Me.tabAdvanced.Controls.Add(Me.lblSeconds)
        Me.tabAdvanced.Controls.Add(Me.btnBrowseCustomPuttyPath)
        Me.tabAdvanced.Location = New System.Drawing.Point(4, 22)
        Me.tabAdvanced.Name = "tabAdvanced"
        Me.tabAdvanced.Size = New System.Drawing.Size(602, 463)
        Me.tabAdvanced.TabIndex = 5
        Me.tabAdvanced.Text = "Advanced"
        Me.tabAdvanced.UseVisualStyleBackColor = True
        '
        'lblSQLDatabaseName
        '
        Me.lblSQLDatabaseName.AutoSize = True
        Me.lblSQLDatabaseName.Enabled = False
        Me.lblSQLDatabaseName.Location = New System.Drawing.Point(23, 132)
        Me.lblSQLDatabaseName.Name = "lblSQLDatabaseName"
        Me.lblSQLDatabaseName.Size = New System.Drawing.Size(56, 13)
        Me.lblSQLDatabaseName.TabIndex = 122
        Me.lblSQLDatabaseName.Text = "Database:"
        '
        'txtSQLDatabaseName
        '
        Me.txtSQLDatabaseName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSQLDatabaseName.Enabled = False
        Me.txtSQLDatabaseName.Location = New System.Drawing.Point(113, 129)
        Me.txtSQLDatabaseName.Name = "txtSQLDatabaseName"
        Me.txtSQLDatabaseName.Size = New System.Drawing.Size(153, 20)
        Me.txtSQLDatabaseName.TabIndex = 123
        '
        'frmOptions
        '
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(794, 542)
        Me.Controls.Add(Me.tcTabControl)
        Me.Controls.Add(Me.lvPages)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = Global.mRemoteNG.My.Resources.Resources.Options_Icon
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOptions"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Options"
        CType(Me.numPuttyWaitTime, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numUVNCSCPort, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlProxy.ResumeLayout(False)
        Me.pnlProxy.PerformLayout()
        Me.pnlProxyBasic.ResumeLayout(False)
        Me.pnlProxyBasic.PerformLayout()
        CType(Me.numProxyPort, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlProxyAuthentication.ResumeLayout(False)
        Me.pnlProxyAuthentication.PerformLayout()
        Me.pnlUpdateCheck.ResumeLayout(False)
        Me.pnlUpdateCheck.PerformLayout()
        Me.pnlAutoSave.ResumeLayout(False)
        Me.pnlAutoSave.PerformLayout()
        CType(Me.numAutoSave, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlRdpReconnectionCount.ResumeLayout(False)
        Me.pnlRdpReconnectionCount.PerformLayout()
        CType(Me.numRdpReconnectionCount, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tcTabControl.ResumeLayout(False)
        Me.tabStartupExit.ResumeLayout(False)
        Me.tabStartupExit.PerformLayout()
        Me.tabAppearance.ResumeLayout(False)
        Me.tabAppearance.PerformLayout()
        Me.tabTabsAndPanels.ResumeLayout(False)
        Me.tabTabsAndPanels.PerformLayout()
        Me.tabConnections.ResumeLayout(False)
        Me.tabConnections.PerformLayout()
        Me.pnlDefaultCredentials.ResumeLayout(False)
        Me.pnlDefaultCredentials.PerformLayout()
        Me.tabSQLServer.ResumeLayout(False)
        Me.tabSQLServer.PerformLayout()
        Me.tabUpdates.ResumeLayout(False)
        Me.tabAdvanced.ResumeLayout(False)
        Me.tabAdvanced.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region

#Region "Private Methods"
    Private Sub LoadOptions()
        Try
            Me.chkSaveConsOnExit.Checked = My.Settings.SaveConsOnExit
            Me.chkConfirmCloseConnection.Checked = My.Settings.ConfirmCloseConnection
            Me.chkConfirmExit.Checked = My.Settings.ConfirmExit
            Me.chkReconnectOnStart.Checked = My.Settings.OpenConsFromLastSession
            Me.chkProperInstallationOfComponentsAtStartup.Checked = My.Settings.StartupComponentsCheck

            Me.cboLanguage.Items.Clear()
            Me.cboLanguage.Items.Add(My.Resources.strLanguageDefault)

            For Each CultureNativeName As String In App.SupportedCultures.CultureNativeNames
                Me.cboLanguage.Items.Add(CultureNativeName)
            Next
            If Not My.Settings.OverrideUICulture = "" And App.SupportedCultures.IsNameSupported(My.Settings.OverrideUICulture) Then
                Me.cboLanguage.SelectedItem = App.SupportedCultures.CultureNativeName(My.Settings.OverrideUICulture)
            End If
            If Me.cboLanguage.SelectedIndex = -1 Then
                Me.cboLanguage.SelectedIndex = 0
            End If

            Me.chkShowDescriptionTooltipsInTree.Checked = My.Settings.ShowDescriptionTooltipsInTree
            Me.chkShowSystemTrayIcon.Checked = My.Settings.ShowSystemTrayIcon
            Me.chkMinimizeToSystemTray.Checked = My.Settings.MinimizeToTray

            Me.chkOpenNewTabRightOfSelected.Checked = My.Settings.OpenTabsRightOfSelected
            Me.chkShowLogonInfoOnTabs.Checked = My.Settings.ShowLogonInfoOnTabs
            Me.chkShowProtocolOnTabs.Checked = My.Settings.ShowProtocolOnTabs
            Me.chkShowFullConnectionsFilePathInTitle.Checked = My.Settings.ShowCompleteConsPathInTitle
            Me.chkDoubleClickClosesTab.Checked = My.Settings.DoubleClickOnTabClosesIt
            Me.chkAlwaysShowPanelSelectionDlg.Checked = My.Settings.AlwaysShowPanelSelectionDlg

            Me.chkSingleClickOnConnectionOpensIt.Checked = My.Settings.SingleClickOnConnectionOpensIt
            Me.chkSingleClickOnOpenedConnectionSwitchesToIt.Checked = My.Settings.SingleClickSwitchesToOpenConnection
            Me.chkHostnameLikeDisplayName.Checked = My.Settings.SetHostnameLikeDisplayName
            Me.numRdpReconnectionCount.Value = My.Settings.RdpReconnectionCount
            Me.numAutoSave.Value = My.Settings.AutoSaveEveryMinutes

            Me.chkUseSQLServer.Checked = My.Settings.UseSQLServer
            Me.txtSQLServer.Text = My.Settings.SQLHost
            Me.txtSQLDatabaseName.Text = My.Settings.SQLDatabaseName
            Me.txtSQLUsername.Text = My.Settings.SQLUser
            Me.txtSQLPassword.Text = mRemoteNG.Security.Crypt.Decrypt(My.Settings.SQLPass, App.Info.General.EncryptionKey)

            Select Case My.Settings.EmptyCredentials
                Case "noinfo"
                    Me.radCredentialsNoInfo.Checked = True
                Case "windows"
                    Me.radCredentialsWindows.Checked = True
                Case "custom"
                    Me.radCredentialsCustom.Checked = True
            End Select

            Me.txtCredentialsUsername.Text = My.Settings.DefaultUsername
            Me.txtCredentialsPassword.Text = mRemoteNG.Security.Crypt.Decrypt(My.Settings.DefaultPassword, App.Info.General.EncryptionKey)
            Me.txtCredentialsDomain.Text = My.Settings.DefaultDomain

            Me.chkUseOnlyErrorsAndInfosPanel.Checked = My.Settings.ShowNoMessageBoxes
            Me.chkMCInformation.Checked = My.Settings.SwitchToMCOnInformation
            Me.chkMCWarnings.Checked = My.Settings.SwitchToMCOnWarning
            Me.chkMCErrors.Checked = My.Settings.SwitchToMCOnError

            chkCheckForUpdatesOnStartup.Checked = My.Settings.CheckForUpdatesOnStartup
            cboUpdateCheckFrequency.Enabled = chkCheckForUpdatesOnStartup.Checked
            cboUpdateCheckFrequency.Items.Clear()
            Dim nDaily As Integer = cboUpdateCheckFrequency.Items.Add(My.Resources.strUpdateFrequencyDaily)
            Dim nWeekly As Integer = cboUpdateCheckFrequency.Items.Add(My.Resources.strUpdateFrequencyWeekly)
            Dim nMonthly As Integer = cboUpdateCheckFrequency.Items.Add(My.Resources.strUpdateFrequencyMonthly)
            Select Case My.Settings.CheckForUpdatesFrequencyDays
                Case Is < 1
                    chkCheckForUpdatesOnStartup.Checked = False
                    cboUpdateCheckFrequency.SelectedIndex = nDaily
                Case 1 ' Daily
                    cboUpdateCheckFrequency.SelectedIndex = nDaily
                Case 7 ' Weekly
                    cboUpdateCheckFrequency.SelectedIndex = nWeekly
                Case 31 ' Monthly
                    cboUpdateCheckFrequency.SelectedIndex = nMonthly
                Case Else
                    Dim nCustom As Integer = cboUpdateCheckFrequency.Items.Add(String.Format(My.Resources.strUpdateFrequencyCustom, My.Settings.CheckForUpdatesFrequencyDays))
                    cboUpdateCheckFrequency.SelectedIndex = nCustom
            End Select

            Me.chkWriteLogFile.Checked = My.Settings.WriteLogFile
            Me.chkEncryptCompleteFile.Checked = My.Settings.EncryptCompleteConnectionsFile
            Me.chkAutomaticallyGetSessionInfo.Checked = My.Settings.AutomaticallyGetSessionInfo
            Me.chkAutomaticReconnect.Checked = My.Settings.ReconnectOnDisconnect
            Me.chkSingleInstance.Checked = My.Settings.SingleInstance
            Me.chkUseCustomPuttyPath.Checked = My.Settings.UseCustomPuttyPath
            Me.txtCustomPuttyPath.Text = My.Settings.CustomPuttyPath
            Me.numPuttyWaitTime.Value = My.Settings.MaxPuttyWaitTime

            Me.chkUseProxyForAutomaticUpdates.Checked = My.Settings.UpdateUseProxy
            Me.btnTestProxy.Enabled = My.Settings.UpdateUseProxy
            Me.pnlProxyBasic.Enabled = My.Settings.UpdateUseProxy

            Me.txtProxyAddress.Text = My.Settings.UpdateProxyAddress
            Me.numProxyPort.Value = My.Settings.UpdateProxyPort

            Me.chkUseProxyAuthentication.Checked = My.Settings.UpdateProxyUseAuthentication
            Me.pnlProxyAuthentication.Enabled = My.Settings.UpdateProxyUseAuthentication

            Me.txtProxyUsername.Text = My.Settings.UpdateProxyAuthUser
            Me.txtProxyPassword.Text = Security.Crypt.Decrypt(My.Settings.UpdateProxyAuthPass, App.Info.General.EncryptionKey)

            Me.numUVNCSCPort.Value = My.Settings.UVNCSCPort

            Me.txtXULrunnerPath.Text = My.Settings.XULRunnerPath
        Catch ex As Exception
            mC.AddMessage(Messages.MessageClass.ErrorMsg, "LoadOptions (UI.Window.Options) failed" & vbNewLine & ex.Message, True)
        End Try
    End Sub

    Private Sub SaveOptions()
        Try
            My.Settings.SaveConsOnExit = Me.chkSaveConsOnExit.Checked
            My.Settings.ConfirmCloseConnection = Me.chkConfirmCloseConnection.Checked
            My.Settings.ConfirmExit = Me.chkConfirmExit.Checked
            My.Settings.OpenConsFromLastSession = Me.chkReconnectOnStart.Checked
            My.Settings.StartupComponentsCheck = Me.chkProperInstallationOfComponentsAtStartup.Checked

            If Me.cboLanguage.SelectedIndex > 0 And App.SupportedCultures.IsNativeNameSupported(Me.cboLanguage.SelectedItem) Then
                My.Settings.OverrideUICulture = App.SupportedCultures.CultureName(Me.cboLanguage.SelectedItem)
            Else
                My.Settings.OverrideUICulture = ""
            End If

            My.Settings.ShowDescriptionTooltipsInTree = Me.chkShowDescriptionTooltipsInTree.Checked
            My.Settings.ShowSystemTrayIcon = Me.chkShowSystemTrayIcon.Checked
            My.Settings.MinimizeToTray = Me.chkMinimizeToSystemTray.Checked

            If My.Settings.ShowSystemTrayIcon Then
                If App.Runtime.SysTrayIcon Is Nothing Then
                    App.Runtime.SysTrayIcon = New Tools.Controls.SysTrayIcon
                End If
            Else
                If App.Runtime.SysTrayIcon IsNot Nothing Then
                    App.Runtime.SysTrayIcon.Dispose()
                    App.Runtime.SysTrayIcon = Nothing
                End If
            End If

            My.Settings.ShowCompleteConsPathInTitle = Me.chkShowFullConnectionsFilePathInTitle.Checked

            My.Settings.OpenTabsRightOfSelected = Me.chkOpenNewTabRightOfSelected.Checked
            My.Settings.ShowLogonInfoOnTabs = Me.chkShowLogonInfoOnTabs.Checked
            My.Settings.ShowProtocolOnTabs = Me.chkShowProtocolOnTabs.Checked
            My.Settings.DoubleClickOnTabClosesIt = Me.chkDoubleClickClosesTab.Checked
            My.Settings.AlwaysShowPanelSelectionDlg = Me.chkAlwaysShowPanelSelectionDlg.Checked

            My.Settings.SingleClickOnConnectionOpensIt = Me.chkSingleClickOnConnectionOpensIt.Checked
            My.Settings.SingleClickSwitchesToOpenConnection = Me.chkSingleClickOnOpenedConnectionSwitchesToIt.Checked
            My.Settings.SetHostnameLikeDisplayName = Me.chkHostnameLikeDisplayName.Checked
            My.Settings.RdpReconnectionCount = Me.numRdpReconnectionCount.Value
            My.Settings.AutoSaveEveryMinutes = Me.numAutoSave.Value

            If My.Settings.AutoSaveEveryMinutes > 0 Then
                frmMain.tmrAutoSave.Interval = My.Settings.AutoSaveEveryMinutes * 60000
                frmMain.tmrAutoSave.Enabled = True
            Else
                frmMain.tmrAutoSave.Enabled = False
            End If

            My.Settings.UseSQLServer = Me.chkUseSQLServer.Checked
            My.Settings.SQLHost = Me.txtSQLServer.Text
            My.Settings.SQLDatabaseName = Me.txtSQLDatabaseName.Text
            My.Settings.SQLUser = Me.txtSQLUsername.Text
            My.Settings.SQLPass = mRemoteNG.Security.Crypt.Encrypt(Me.txtSQLPassword.Text, App.Info.General.EncryptionKey)

            If Me.radCredentialsNoInfo.Checked Then
                My.Settings.EmptyCredentials = "noinfo"
            ElseIf Me.radCredentialsWindows.Checked Then
                My.Settings.EmptyCredentials = "windows"
            ElseIf Me.radCredentialsCustom.Checked Then
                My.Settings.EmptyCredentials = "custom"
            End If

            My.Settings.DefaultUsername = Me.txtCredentialsUsername.Text
            My.Settings.DefaultPassword = mRemoteNG.Security.Crypt.Encrypt(Me.txtCredentialsPassword.Text, App.Info.General.EncryptionKey)
            My.Settings.DefaultDomain = Me.txtCredentialsDomain.Text

            My.Settings.ShowNoMessageBoxes = Me.chkUseOnlyErrorsAndInfosPanel.Checked
            My.Settings.SwitchToMCOnInformation = Me.chkMCInformation.Checked
            My.Settings.SwitchToMCOnWarning = Me.chkMCWarnings.Checked
            My.Settings.SwitchToMCOnError = Me.chkMCErrors.Checked

            My.Settings.CheckForUpdatesOnStartup = chkCheckForUpdatesOnStartup.Checked
            Select Case cboUpdateCheckFrequency.SelectedItem.ToString()
                Case My.Resources.strUpdateFrequencyDaily
                    My.Settings.CheckForUpdatesFrequencyDays = 1
                Case My.Resources.strUpdateFrequencyWeekly
                    My.Settings.CheckForUpdatesFrequencyDays = 7
                Case My.Resources.strUpdateFrequencyMonthly
                    My.Settings.CheckForUpdatesFrequencyDays = 31
                Case Else
                    ' Custom so do not change
            End Select

            My.Settings.WriteLogFile = Me.chkWriteLogFile.Checked
            My.Settings.EncryptCompleteConnectionsFile = Me.chkEncryptCompleteFile.Checked
            My.Settings.AutomaticallyGetSessionInfo = Me.chkAutomaticallyGetSessionInfo.Checked
            My.Settings.ReconnectOnDisconnect = Me.chkAutomaticReconnect.Checked
            My.Settings.SingleInstance = Me.chkSingleInstance.Checked
            My.Settings.UseCustomPuttyPath = Me.chkUseCustomPuttyPath.Checked
            My.Settings.CustomPuttyPath = Me.txtCustomPuttyPath.Text

            If My.Settings.UseCustomPuttyPath Then
                mRemoteNG.Connection.Protocol.PuttyBase.PuttyPath = My.Settings.CustomPuttyPath
            Else
                mRemoteNG.Connection.Protocol.PuttyBase.PuttyPath = My.Application.Info.DirectoryPath & "\putty.exe"
            End If

            My.Settings.MaxPuttyWaitTime = Me.numPuttyWaitTime.Value

            My.Settings.UpdateUseProxy = Me.chkUseProxyForAutomaticUpdates.Checked

            My.Settings.UpdateProxyAddress = Me.txtProxyAddress.Text
            My.Settings.UpdateProxyPort = Me.numProxyPort.Value

            My.Settings.UpdateProxyUseAuthentication = Me.chkUseProxyAuthentication.Checked

            My.Settings.UpdateProxyAuthUser = Me.txtProxyUsername.Text
            My.Settings.UpdateProxyAuthPass = Security.Crypt.Encrypt(Me.txtProxyPassword.Text, App.Info.General.EncryptionKey)

            My.Settings.UVNCSCPort = Me.numUVNCSCPort.Value

            My.Settings.XULRunnerPath = Me.txtXULrunnerPath.Text


            If My.Settings.LoadConsFromCustomLocation = False Then
                App.Runtime.SetMainFormText(App.Info.Connections.DefaultConnectionsPath & "\" & App.Info.Connections.DefaultConnectionsFile)
            Else
                App.Runtime.SetMainFormText(My.Settings.CustomConsPath)
            End If

            App.Runtime.Startup.DestroySQLUpdateHandlerAndStopTimer()

            If My.Settings.UseSQLServer = True Then
                App.Runtime.SetMainFormText("SQL Server")
                App.Runtime.Startup.CreateSQLUpdateHandlerAndStartTimer()
            End If
        Catch ex As Exception
            mC.AddMessage(Messages.MessageClass.ErrorMsg, "SaveOptions (UI.Window.Options) failed" & vbNewLine & ex.Message, True)
        End Try
    End Sub
#End Region

#Region "Private Variables"
    Private _initialTab As Integer = 0
#End Region

#Region "Public Methods"
    Public Sub New(ByVal Panel As DockContent)
        Me.InitializeComponent()
        App.Runtime.FontOverride(Me)
    End Sub
#End Region

#Region "Form Stuff"
    Private Sub Options_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ApplyLanguage()

        If App.Editions.Spanlink.Enabled Then
            ApplySpanlinkEdition()
        End If

        ' Hide the tabs
        tcTabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        tcTabControl.Padding = New Point(0, 0)
        tcTabControl.ItemSize = New Point(0, 1)

        ' Switch to the _initialTab
        tcTabControl.SelectedIndex = _initialTab
        lvPages.Items(_initialTab).Selected = True
    End Sub

    Private Sub ApplySpanlinkEdition()
        chkSaveConsOnExit.Enabled = False
        chkSaveConsOnExit.Visible = False
        chkCheckForUpdatesOnStartup.Enabled = False
        chkCheckForUpdatesOnStartup.Visible = False
        chkReconnectOnStart.Enabled = False
        chkReconnectOnStart.Visible = False
        chkShowFullConnectionsFilePathInTitle.Enabled = False
        chkShowFullConnectionsFilePathInTitle.Visible = False
        chkShowLogonInfoOnTabs.Enabled = False
        chkShowLogonInfoOnTabs.Visible = False
        chkHostnameLikeDisplayName.Enabled = False
        chkHostnameLikeDisplayName.Visible = False
        pnlAutoSave.Enabled = False
        pnlAutoSave.Visible = False
        pnlDefaultCredentials.Enabled = False
        pnlDefaultCredentials.Visible = False
        pnlProxy.Enabled = False
        pnlProxy.Visible = False
        chkEncryptCompleteFile.Enabled = False
        chkEncryptCompleteFile.Visible = False
        chkWriteLogFile.Enabled = False
        chkWriteLogFile.Visible = False
    End Sub

    Private Sub ApplyLanguage()
        lvPages.Items(0).Text = My.Resources.strStartupExit
        lvPages.Items(1).Text = My.Resources.strTabAppearance
        lvPages.Items(2).Text = My.Resources.strTabsAndPanels.Replace("&&", "&")
        lvPages.Items(3).Text = My.Resources.strConnections
        lvPages.Items(4).Text = My.Resources.strSQLServer
        lvPages.Items(5).Text = My.Resources.strTabUpdates
        lvPages.Items(6).Text = My.Resources.strTabAdvanced
        lblUpdatesExplanation.Text = My.Resources.strUpdateCheck
        btnTestProxy.Text = My.Resources.strButtonTestProxy
        lblSeconds.Text = My.Resources.strLabelSeconds
        lblMaximumPuttyWaitTime.Text = My.Resources.strLabelPuttyTimeout
        chkAutomaticReconnect.Text = My.Resources.strCheckboxAutomaticReconnect
        lblProxyAddress.Text = My.Resources.strLabelAddress
        lblProxyPort.Text = My.Resources.strLabelPort
        lblProxyUsername.Text = My.Resources.strLabelUsername
        lblProxyPassword.Text = My.Resources.strLabelPassword
        chkUseProxyAuthentication.Text = My.Resources.strCheckboxProxyAuthentication
        chkUseProxyForAutomaticUpdates.Text = My.Resources.strCheckboxUpdateUseProxy
        lblConfigurePuttySessions.Text = My.Resources.strLabelPuttySessionsConfig
        btnLaunchPutty.Text = My.Resources.strButtonLaunchPutty
        btnBrowseCustomPuttyPath.Text = My.Resources.strButtonBrowse
        chkUseCustomPuttyPath.Text = My.Resources.strCheckboxPuttyPath
        chkAutomaticallyGetSessionInfo.Text = My.Resources.strAutomaticallyGetSessionInfo
        chkWriteLogFile.Text = My.Resources.strWriteLogFile
        chkSingleInstance.Text = My.Resources.strAllowOnlySingleInstance
        chkReconnectOnStart.Text = My.Resources.strReconnectAtStartup
        chkCheckForUpdatesOnStartup.Text = My.Resources.strCheckForUpdatesOnStartup
        chkConfirmCloseConnection.Text = My.Resources.strConfirmCloseConnection
        chkConfirmExit.Text = My.Resources.strConfirmExit
        chkSaveConsOnExit.Text = My.Resources.strSaveConsOnExit
        chkMinimizeToSystemTray.Text = My.Resources.strMinimizeToSysTray
        chkShowFullConnectionsFilePathInTitle.Text = My.Resources.strShowFullConsFilePath
        chkShowSystemTrayIcon.Text = My.Resources.strAlwaysShowSysTrayIcon
        chkShowDescriptionTooltipsInTree.Text = My.Resources.strShowDescriptionTooltips
        chkShowProtocolOnTabs.Text = My.Resources.strShowProtocolOnTabs
        chkShowLogonInfoOnTabs.Text = My.Resources.strShowLogonInfoOnTabs
        chkOpenNewTabRightOfSelected.Text = My.Resources.strOpenNewTabRight
        chkAlwaysShowPanelSelectionDlg.Text = My.Resources.strAlwaysShowPanelSelection
        chkDoubleClickClosesTab.Text = My.Resources.strDoubleClickTabClosesIt
        chkHostnameLikeDisplayName.Text = My.Resources.strSetHostnameLikeDisplayName
        lblExperimental.Text = My.Resources.strExperimental.ToUpper
        chkUseSQLServer.Text = My.Resources.strUseSQLServer
        lblSQLInfo.Text = My.Resources.strSQLInfo
        lblSQLUsername.Text = My.Resources.strLabelUsername
        lblSQLServer.Text = My.Resources.strSQLServer & ":"
        lblSQLDatabaseName.Text = My.Resources.strLabelSQLServerDatabaseName
        lblSQLPassword.Text = My.Resources.strLabelPassword
        lblRdpReconnectionCount.Text = My.Resources.strRdpReconnectCount
        lblAutoSave2.Text = My.Resources.strAutoSaveMins
        lblAutoSave1.Text = My.Resources.strAutoSaveEvery
        lblCredentialsDomain.Text = My.Resources.strLabelDomain
        lblCredentialsPassword.Text = My.Resources.strLabelPassword
        lblCredentialsUsername.Text = My.Resources.strLabelUsername
        radCredentialsCustom.Text = My.Resources.strTheFollowing & ":"
        radCredentialsWindows.Text = My.Resources.strMyCurrentWindowsCreds
        radCredentialsNoInfo.Text = My.Resources.strNoInformation
        lblDefaultCredentials.Text = My.Resources.strEmptyUsernamePasswordDomainFields
        chkSingleClickOnOpenedConnectionSwitchesToIt.Text = My.Resources.strSingleClickOnOpenConnectionSwitchesToIt
        chkSingleClickOnConnectionOpensIt.Text = My.Resources.strSingleClickOnConnectionOpensIt
        lblSwitchToErrorsAndInfos.Text = My.Resources.strSwitchToErrorsAndInfos & ":"
        chkMCErrors.Text = My.Resources.strErrors
        chkMCWarnings.Text = My.Resources.strWarnings
        chkMCInformation.Text = My.Resources.strInformations
        chkUseOnlyErrorsAndInfosPanel.Text = My.Resources.strUseOnlyErrorsAndInfosPanel
        btnOK.Text = My.Resources.strButtonOK
        btnCancel.Text = My.Resources.strButtonCancel
        btnUpdateCheckNow.Text = My.Resources.strCheckNow
        Text = My.Resources.strMenuOptions
        lblUVNCSCPort.Text = My.Resources.strUltraVNCSCListeningPort & ":"
        chkProperInstallationOfComponentsAtStartup.Text = My.Resources.strCheckProperInstallationOfComponentsAtStartup
        lblXulRunnerPath.Text = My.Resources.strXULrunnerPath & ":"
        btnBrowseXulRunnerPath.Text = My.Resources.strButtonBrowse
        chkEncryptCompleteFile.Text = My.Resources.strEncryptCompleteConnectionFile
        lblLanguage.Text = My.Resources.strLanguage
        lblLanguageRestartRequired.Text = String.Format(My.Resources.strLanguageRestartRequired, My.Application.Info.ProductName)
    End Sub

    Public Shadows Sub Show(ByVal DockPanel As DockPanel, Optional ByVal initialTab As Integer = 0)
        Windows.optionsForm.LoadOptions()

        _initialTab = initialTab
        MyBase.ShowDialog(frmMain)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.SaveOptions()
        Me.Close()
    End Sub

    Private Sub lvPages_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvPages.SelectedIndexChanged
        Dim listView As ListView = sender
        If Not listView.SelectedIndices.Count = 0 Then
            tcTabControl.SelectedIndex = listView.SelectedIndices(0)
        End If
    End Sub

    Private Sub lvPages_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvPages.MouseUp
        Dim listView As ListView = sender
        If listView.SelectedIndices.Count = 0 Then
            listView.Items(tcTabControl.SelectedIndex).Selected = True
        End If
    End Sub

    Private Sub radCredentialsCustom_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radCredentialsCustom.CheckedChanged
        Me.lblCredentialsUsername.Enabled = Me.radCredentialsCustom.Checked
        Me.lblCredentialsPassword.Enabled = Me.radCredentialsCustom.Checked
        Me.lblCredentialsDomain.Enabled = Me.radCredentialsCustom.Checked
        Me.txtCredentialsUsername.Enabled = Me.radCredentialsCustom.Checked
        Me.txtCredentialsPassword.Enabled = Me.radCredentialsCustom.Checked
        Me.txtCredentialsDomain.Enabled = Me.radCredentialsCustom.Checked
    End Sub

    Private Sub chkUseSQLServer_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkUseSQLServer.CheckedChanged
        Me.lblSQLServer.Enabled = chkUseSQLServer.Checked
        Me.lblSQLDatabaseName.Enabled = chkUseSQLServer.Checked
        Me.lblSQLUsername.Enabled = chkUseSQLServer.Checked
        Me.lblSQLPassword.Enabled = chkUseSQLServer.Checked
        Me.txtSQLServer.Enabled = chkUseSQLServer.Checked
        Me.txtSQLDatabaseName.Enabled = chkUseSQLServer.Checked
        Me.txtSQLUsername.Enabled = chkUseSQLServer.Checked
        Me.txtSQLPassword.Enabled = chkUseSQLServer.Checked
    End Sub

    Private Sub chkUseOnlyErrorsAndInfosPanel_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.chkMCInformation.Enabled = Me.chkUseOnlyErrorsAndInfosPanel.Checked
        Me.chkMCWarnings.Enabled = Me.chkUseOnlyErrorsAndInfosPanel.Checked
        Me.chkMCErrors.Enabled = Me.chkUseOnlyErrorsAndInfosPanel.Checked
    End Sub

    Private Sub chkUseCustomPuttyPath_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseCustomPuttyPath.CheckedChanged
        Me.txtCustomPuttyPath.Enabled = Me.chkUseCustomPuttyPath.Checked
        Me.btnBrowseCustomPuttyPath.Enabled = Me.chkUseCustomPuttyPath.Checked
    End Sub

    Private Sub btnLaunchPutty_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLaunchPutty.Click
        mRemoteNG.Connection.Protocol.PuttyBase.StartPutty()
    End Sub

    Private Sub btnBrowseCustomPuttyPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseCustomPuttyPath.Click
        Dim oDlg As New OpenFileDialog()
        oDlg.Filter = My.Resources.strFilterApplication & "|*.exe|" & My.Resources.strFilterAll & "|*.*"
        oDlg.FileName = "putty.exe"
        oDlg.CheckFileExists = True
        oDlg.Multiselect = False

        If oDlg.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            Me.txtCustomPuttyPath.Text = oDlg.FileName
        End If

        oDlg = Nothing
    End Sub

    Private Sub btnBrowseXulRunnerPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseXulRunnerPath.Click
        Dim oDlg As New FolderBrowserDialog
        oDlg.ShowNewFolderButton = False

        If oDlg.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            Me.txtXULrunnerPath.Text = oDlg.SelectedPath
        End If

        oDlg = Nothing
    End Sub

    Private Sub chkCheckForUpdatesOnStartup_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCheckForUpdatesOnStartup.CheckedChanged
        cboUpdateCheckFrequency.Enabled = chkCheckForUpdatesOnStartup.Checked
    End Sub

    Private Sub btnUpdateCheckNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateCheckNow.Click
        App.Runtime.Windows.Show(UI.Window.Type.Update)
    End Sub

    Private Sub chkUseProxyForAutomaticUpdates_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseProxyForAutomaticUpdates.CheckedChanged
        Me.pnlProxyBasic.Enabled = Me.chkUseProxyForAutomaticUpdates.Checked
        Me.btnTestProxy.Enabled = Me.chkUseProxyForAutomaticUpdates.Checked

        If Me.chkUseProxyForAutomaticUpdates.Checked Then
            Me.chkUseProxyAuthentication.Enabled = True

            If Me.chkUseProxyAuthentication.Checked Then
                Me.pnlProxyAuthentication.Enabled = True
            End If
        Else
            Me.chkUseProxyAuthentication.Enabled = False
            Me.pnlProxyAuthentication.Enabled = False
        End If
    End Sub

    Private Sub btnTestProxy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTestProxy.Click
        SaveOptions()
        Dim ud As New App.Update()

        If ud.IsProxyOK Then
            MsgBox(My.Resources.strProxyTestSucceeded, MsgBoxStyle.Information)
        Else
            MsgBox(My.Resources.strProxyTestFailed, MsgBoxStyle.Exclamation)
        End If
    End Sub

    Private Sub chkUseProxyAuthentication_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseProxyAuthentication.CheckedChanged
        If Me.chkUseProxyForAutomaticUpdates.Checked Then
            If Me.chkUseProxyAuthentication.Checked Then
                Me.pnlProxyAuthentication.Enabled = True
            Else
                Me.pnlProxyAuthentication.Enabled = False
            End If
        End If
    End Sub
#End Region


End Class
