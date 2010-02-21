Imports WeifenLuo.WinFormsUI.Docking
Imports mRemote.App.Runtime

Namespace UI
    Namespace Window
        Public Class Options
            Inherits UI.Window.Base

#Region "Form Init"
            Friend WithEvents btnCancel As System.Windows.Forms.Button
            Friend WithEvents btnOK As System.Windows.Forms.Button
            Friend WithEvents tabConnections As Crownwood.Magic.Controls.TabPage
            Friend WithEvents tabTabs As Crownwood.Magic.Controls.TabPage
            Friend WithEvents tabUpdates As Crownwood.Magic.Controls.TabPage
            Friend WithEvents tabAdvanced As Crownwood.Magic.Controls.TabPage
            Friend WithEvents tabAppearance As Crownwood.Magic.Controls.TabPage
            Friend WithEvents tabStartupExit As Crownwood.Magic.Controls.TabPage
            Friend WithEvents chkConfirmExit As System.Windows.Forms.CheckBox
            Friend WithEvents chkSaveConsOnExit As System.Windows.Forms.CheckBox
            Friend WithEvents chkShowSystemTrayIcon As System.Windows.Forms.CheckBox
            Friend WithEvents chkShowDescriptionTooltipsInTree As System.Windows.Forms.CheckBox
            Friend WithEvents chkShowLogonInfoOnTabs As System.Windows.Forms.CheckBox
            Friend WithEvents chkOpenNewTabRightOfSelected As System.Windows.Forms.CheckBox
            Friend WithEvents chkSingleClickOnConnectionOpensIt As System.Windows.Forms.CheckBox
            Friend WithEvents radCredentialsCustom As System.Windows.Forms.RadioButton
            Friend WithEvents radCredentialsWindows As System.Windows.Forms.RadioButton
            Friend WithEvents radCredentialsNoInfo As System.Windows.Forms.RadioButton
            Friend WithEvents lblDefaultCredentials As System.Windows.Forms.Label
            Friend WithEvents txtCredentialsDomain As System.Windows.Forms.TextBox
            Friend WithEvents txtCredentialsPassword As System.Windows.Forms.TextBox
            Friend WithEvents txtCredentialsUsername As System.Windows.Forms.TextBox
            Friend WithEvents lblCredentialsDomain As System.Windows.Forms.Label
            Friend WithEvents lblCredentialsPassword As System.Windows.Forms.Label
            Friend WithEvents lblCredentialsUsername As System.Windows.Forms.Label
            Friend WithEvents chkWriteLogFile As System.Windows.Forms.CheckBox
            Friend WithEvents chkUseCustomPuttyPath As System.Windows.Forms.CheckBox
            Friend WithEvents chkAutomaticallyGetSessionInfo As System.Windows.Forms.CheckBox
            Friend WithEvents btnBrowseCustomPuttyPath As System.Windows.Forms.Button
            Friend WithEvents txtCustomPuttyPath As System.Windows.Forms.TextBox
            Friend WithEvents btnLaunchPutty As System.Windows.Forms.Button
            Friend WithEvents lblConfigurePuttySessions As System.Windows.Forms.Label
            Friend WithEvents chkShowFullConnectionsFilePathInTitle As System.Windows.Forms.CheckBox
            Friend WithEvents chkShowProtocolOnTabs As System.Windows.Forms.CheckBox
            Friend WithEvents chkUseProxyForAutomaticUpdates As System.Windows.Forms.CheckBox
            Friend WithEvents txtProxyAddress As System.Windows.Forms.TextBox
            Friend WithEvents lblProxyPort As System.Windows.Forms.Label
            Friend WithEvents lblProxyAddress As System.Windows.Forms.Label
            Friend WithEvents lblProxyUsername As System.Windows.Forms.Label
            Friend WithEvents txtProxyUsername As System.Windows.Forms.TextBox
            Friend WithEvents chkUseProxyAuthentication As System.Windows.Forms.CheckBox
            Friend WithEvents lblProxyPassword As System.Windows.Forms.Label
            Friend WithEvents txtProxyPassword As System.Windows.Forms.TextBox
            Friend WithEvents pnlProxyAuthentication As System.Windows.Forms.Panel
            Friend WithEvents pnlProxyBasic As System.Windows.Forms.Panel
            Friend WithEvents numProxyPort As System.Windows.Forms.NumericUpDown
            Friend WithEvents Label2 As System.Windows.Forms.Label
            Friend WithEvents numPuttyWaitTime As System.Windows.Forms.NumericUpDown
            Friend WithEvents Label1 As System.Windows.Forms.Label
            Friend WithEvents chkReconnectOnStart As System.Windows.Forms.CheckBox
            Friend WithEvents numAutoSave As System.Windows.Forms.NumericUpDown
            Friend WithEvents lblAutoSave2 As System.Windows.Forms.Label
            Friend WithEvents lblAutoSave1 As System.Windows.Forms.Label
            Friend WithEvents chkUseSQLServer As System.Windows.Forms.CheckBox
            Friend WithEvents lblSQLInfo As System.Windows.Forms.Label
            Friend WithEvents txtSQLPassword As System.Windows.Forms.TextBox
            Friend WithEvents txtSQLUsername As System.Windows.Forms.TextBox
            Friend WithEvents txtSQLServer As System.Windows.Forms.TextBox
            Friend WithEvents lblSQLPassword As System.Windows.Forms.Label
            Friend WithEvents lblSQLServer As System.Windows.Forms.Label
            Friend WithEvents lblSQLUsername As System.Windows.Forms.Label
            Friend WithEvents grpExperimental As System.Windows.Forms.GroupBox
            Friend WithEvents chkSingleInstance As System.Windows.Forms.CheckBox
            Friend WithEvents chkHostnameLikeDisplayName As System.Windows.Forms.CheckBox
            Friend WithEvents chkDoubleClickClosesTab As System.Windows.Forms.CheckBox
            Friend WithEvents chkAutomaticReconnect As System.Windows.Forms.CheckBox
            Friend WithEvents chkAlwaysShowPanelSelectionDlg As System.Windows.Forms.CheckBox
            Friend WithEvents chkMinimizeToSystemTray As System.Windows.Forms.CheckBox
            Friend WithEvents chkSingleClickOnOpenedConnectionSwitchesToIt As System.Windows.Forms.CheckBox
            Friend WithEvents btnTestProxy As System.Windows.Forms.Button
            Friend WithEvents Label3 As System.Windows.Forms.Label
            Friend WithEvents numUVNCSCPort As System.Windows.Forms.NumericUpDown
            Friend WithEvents chkProperInstallationOfComponentsAtStartup As System.Windows.Forms.CheckBox
            Friend WithEvents lblXulRunnerPath As System.Windows.Forms.Label
            Friend WithEvents btnBrowseXulRunnerPath As System.Windows.Forms.Button
            Friend WithEvents txtXULrunnerPath As System.Windows.Forms.TextBox
            Friend WithEvents pnlAutoSave As System.Windows.Forms.Panel
            Friend WithEvents pnlDefaultCredentials As System.Windows.Forms.Panel
            Friend WithEvents pnlProxy As System.Windows.Forms.Panel
            Friend WithEvents chkEncryptCompleteFile As System.Windows.Forms.CheckBox
            Friend WithEvents pnlStartup As System.Windows.Forms.Panel
            Friend WithEvents pnlAppearance As System.Windows.Forms.Panel
            Friend WithEvents pnlTabsAndPanels As System.Windows.Forms.Panel
            Friend WithEvents pnlConnections As System.Windows.Forms.Panel
            Friend WithEvents pnlUpdates As System.Windows.Forms.Panel
            Friend WithEvents pnlAdvanced As System.Windows.Forms.Panel
            Friend WithEvents chkUseOnlyErrorsAndInfosPanel As System.Windows.Forms.CheckBox
            Friend WithEvents lblSwitchToErrorsAndInfos As System.Windows.Forms.Label
            Friend WithEvents chkMCInformation As System.Windows.Forms.CheckBox
            Friend WithEvents chkMCErrors As System.Windows.Forms.CheckBox
            Friend WithEvents chkMCWarnings As System.Windows.Forms.CheckBox
            Friend WithEvents chkCheckForUpdatesOnStartup As System.Windows.Forms.CheckBox
            Friend WithEvents cboUpdateCheckFrequency As System.Windows.Forms.ComboBox
            Friend WithEvents lblUpdatesExplanation As System.Windows.Forms.Label
            Friend WithEvents pnlUpdateCheck As System.Windows.Forms.Panel
            Friend WithEvents btnUpdateCheckNow As System.Windows.Forms.Button
            Friend WithEvents TabController As Crownwood.Magic.Controls.TabControl

            Private Sub InitializeComponent()
                Me.TabController = New Crownwood.Magic.Controls.TabControl
                Me.tabUpdates = New Crownwood.Magic.Controls.TabPage
                Me.pnlUpdates = New System.Windows.Forms.Panel
                Me.lblUpdatesExplanation = New System.Windows.Forms.Label
                Me.pnlUpdateCheck = New System.Windows.Forms.Panel
                Me.btnUpdateCheckNow = New System.Windows.Forms.Button
                Me.chkCheckForUpdatesOnStartup = New System.Windows.Forms.CheckBox
                Me.cboUpdateCheckFrequency = New System.Windows.Forms.ComboBox
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
                Me.tabStartupExit = New Crownwood.Magic.Controls.TabPage
                Me.pnlStartup = New System.Windows.Forms.Panel
                Me.chkSaveConsOnExit = New System.Windows.Forms.CheckBox
                Me.chkProperInstallationOfComponentsAtStartup = New System.Windows.Forms.CheckBox
                Me.chkConfirmExit = New System.Windows.Forms.CheckBox
                Me.chkSingleInstance = New System.Windows.Forms.CheckBox
                Me.chkReconnectOnStart = New System.Windows.Forms.CheckBox
                Me.tabAppearance = New Crownwood.Magic.Controls.TabPage
                Me.pnlAppearance = New System.Windows.Forms.Panel
                Me.chkShowDescriptionTooltipsInTree = New System.Windows.Forms.CheckBox
                Me.chkMinimizeToSystemTray = New System.Windows.Forms.CheckBox
                Me.chkShowSystemTrayIcon = New System.Windows.Forms.CheckBox
                Me.chkShowFullConnectionsFilePathInTitle = New System.Windows.Forms.CheckBox
                Me.tabTabs = New Crownwood.Magic.Controls.TabPage
                Me.pnlTabsAndPanels = New System.Windows.Forms.Panel
                Me.chkUseOnlyErrorsAndInfosPanel = New System.Windows.Forms.CheckBox
                Me.lblSwitchToErrorsAndInfos = New System.Windows.Forms.Label
                Me.chkMCInformation = New System.Windows.Forms.CheckBox
                Me.chkMCErrors = New System.Windows.Forms.CheckBox
                Me.chkMCWarnings = New System.Windows.Forms.CheckBox
                Me.chkOpenNewTabRightOfSelected = New System.Windows.Forms.CheckBox
                Me.chkShowProtocolOnTabs = New System.Windows.Forms.CheckBox
                Me.chkDoubleClickClosesTab = New System.Windows.Forms.CheckBox
                Me.chkShowLogonInfoOnTabs = New System.Windows.Forms.CheckBox
                Me.chkAlwaysShowPanelSelectionDlg = New System.Windows.Forms.CheckBox
                Me.tabConnections = New Crownwood.Magic.Controls.TabPage
                Me.pnlConnections = New System.Windows.Forms.Panel
                Me.chkSingleClickOnConnectionOpensIt = New System.Windows.Forms.CheckBox
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
                Me.chkSingleClickOnOpenedConnectionSwitchesToIt = New System.Windows.Forms.CheckBox
                Me.pnlAutoSave = New System.Windows.Forms.Panel
                Me.lblAutoSave1 = New System.Windows.Forms.Label
                Me.numAutoSave = New System.Windows.Forms.NumericUpDown
                Me.lblAutoSave2 = New System.Windows.Forms.Label
                Me.grpExperimental = New System.Windows.Forms.GroupBox
                Me.chkUseSQLServer = New System.Windows.Forms.CheckBox
                Me.lblSQLInfo = New System.Windows.Forms.Label
                Me.lblSQLUsername = New System.Windows.Forms.Label
                Me.txtSQLPassword = New System.Windows.Forms.TextBox
                Me.lblSQLServer = New System.Windows.Forms.Label
                Me.txtSQLUsername = New System.Windows.Forms.TextBox
                Me.lblSQLPassword = New System.Windows.Forms.Label
                Me.txtSQLServer = New System.Windows.Forms.TextBox
                Me.chkHostnameLikeDisplayName = New System.Windows.Forms.CheckBox
                Me.tabAdvanced = New Crownwood.Magic.Controls.TabPage
                Me.pnlAdvanced = New System.Windows.Forms.Panel
                Me.chkWriteLogFile = New System.Windows.Forms.CheckBox
                Me.chkAutomaticallyGetSessionInfo = New System.Windows.Forms.CheckBox
                Me.lblXulRunnerPath = New System.Windows.Forms.Label
                Me.chkEncryptCompleteFile = New System.Windows.Forms.CheckBox
                Me.btnBrowseXulRunnerPath = New System.Windows.Forms.Button
                Me.chkUseCustomPuttyPath = New System.Windows.Forms.CheckBox
                Me.txtXULrunnerPath = New System.Windows.Forms.TextBox
                Me.txtCustomPuttyPath = New System.Windows.Forms.TextBox
                Me.Label3 = New System.Windows.Forms.Label
                Me.btnBrowseCustomPuttyPath = New System.Windows.Forms.Button
                Me.Label2 = New System.Windows.Forms.Label
                Me.btnLaunchPutty = New System.Windows.Forms.Button
                Me.numUVNCSCPort = New System.Windows.Forms.NumericUpDown
                Me.lblConfigurePuttySessions = New System.Windows.Forms.Label
                Me.numPuttyWaitTime = New System.Windows.Forms.NumericUpDown
                Me.chkAutomaticReconnect = New System.Windows.Forms.CheckBox
                Me.Label1 = New System.Windows.Forms.Label
                Me.btnOK = New System.Windows.Forms.Button
                Me.btnCancel = New System.Windows.Forms.Button
                Me.TabController.SuspendLayout()
                Me.tabUpdates.SuspendLayout()
                Me.pnlUpdates.SuspendLayout()
                Me.pnlUpdateCheck.SuspendLayout()
                Me.pnlProxy.SuspendLayout()
                Me.pnlProxyBasic.SuspendLayout()
                CType(Me.numProxyPort, System.ComponentModel.ISupportInitialize).BeginInit()
                Me.pnlProxyAuthentication.SuspendLayout()
                Me.tabStartupExit.SuspendLayout()
                Me.pnlStartup.SuspendLayout()
                Me.tabAppearance.SuspendLayout()
                Me.pnlAppearance.SuspendLayout()
                Me.tabTabs.SuspendLayout()
                Me.pnlTabsAndPanels.SuspendLayout()
                Me.tabConnections.SuspendLayout()
                Me.pnlConnections.SuspendLayout()
                Me.pnlDefaultCredentials.SuspendLayout()
                Me.pnlAutoSave.SuspendLayout()
                CType(Me.numAutoSave, System.ComponentModel.ISupportInitialize).BeginInit()
                Me.grpExperimental.SuspendLayout()
                Me.tabAdvanced.SuspendLayout()
                Me.pnlAdvanced.SuspendLayout()
                CType(Me.numUVNCSCPort, System.ComponentModel.ISupportInitialize).BeginInit()
                CType(Me.numPuttyWaitTime, System.ComponentModel.ISupportInitialize).BeginInit()
                Me.SuspendLayout()
                '
                'TabController
                '
                Me.TabController.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.TabController.Appearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiBox
                Me.TabController.IDEPixelArea = True
                Me.TabController.Location = New System.Drawing.Point(0, 0)
                Me.TabController.Name = "TabController"
                Me.TabController.SelectedIndex = 3
                Me.TabController.SelectedTab = Me.tabConnections
                Me.TabController.Size = New System.Drawing.Size(573, 522)
                Me.TabController.TabIndex = 10
                Me.TabController.TabPages.AddRange(New Crownwood.Magic.Controls.TabPage() {Me.tabStartupExit, Me.tabAppearance, Me.tabTabs, Me.tabConnections, Me.tabUpdates, Me.tabAdvanced})
                '
                'tabUpdates
                '
                Me.tabUpdates.Controls.Add(Me.pnlUpdates)
                Me.tabUpdates.Icon = Global.mRemote.My.Resources.Resources.Info_Icon
                Me.tabUpdates.Location = New System.Drawing.Point(0, 0)
                Me.tabUpdates.Name = "tabUpdates"
                Me.tabUpdates.Selected = False
                Me.tabUpdates.Size = New System.Drawing.Size(573, 492)
                Me.tabUpdates.TabIndex = 5000
                Me.tabUpdates.Title = "Updates"
                '
                'pnlUpdates
                '
                Me.pnlUpdates.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlUpdates.AutoScroll = True
                Me.pnlUpdates.Controls.Add(Me.lblUpdatesExplanation)
                Me.pnlUpdates.Controls.Add(Me.pnlUpdateCheck)
                Me.pnlUpdates.Controls.Add(Me.pnlProxy)
                Me.pnlUpdates.Location = New System.Drawing.Point(3, 3)
                Me.pnlUpdates.Name = "pnlUpdates"
                Me.pnlUpdates.Size = New System.Drawing.Size(567, 486)
                Me.pnlUpdates.TabIndex = 51
                '
                'lblUpdatesExplanation
                '
                Me.lblUpdatesExplanation.Location = New System.Drawing.Point(16, 16)
                Me.lblUpdatesExplanation.Name = "lblUpdatesExplanation"
                Me.lblUpdatesExplanation.Size = New System.Drawing.Size(536, 40)
                Me.lblUpdatesExplanation.TabIndex = 136
                Me.lblUpdatesExplanation.Text = "mRemoteNG can periodically connect to the mRemoteNG website to check for updates " & _
                    "and product announcements."
                '
                'pnlUpdateCheck
                '
                Me.pnlUpdateCheck.Controls.Add(Me.btnUpdateCheckNow)
                Me.pnlUpdateCheck.Controls.Add(Me.chkCheckForUpdatesOnStartup)
                Me.pnlUpdateCheck.Controls.Add(Me.cboUpdateCheckFrequency)
                Me.pnlUpdateCheck.Location = New System.Drawing.Point(16, 64)
                Me.pnlUpdateCheck.Name = "pnlUpdateCheck"
                Me.pnlUpdateCheck.Size = New System.Drawing.Size(536, 120)
                Me.pnlUpdateCheck.TabIndex = 137
                '
                'btnUpdateCheckNow
                '
                Me.btnUpdateCheckNow.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnUpdateCheckNow.Location = New System.Drawing.Point(8, 80)
                Me.btnUpdateCheckNow.Name = "btnUpdateCheckNow"
                Me.btnUpdateCheckNow.Size = New System.Drawing.Size(80, 32)
                Me.btnUpdateCheckNow.TabIndex = 136
                Me.btnUpdateCheckNow.Text = "Check Now"
                Me.btnUpdateCheckNow.UseVisualStyleBackColor = True
                '
                'chkCheckForUpdatesOnStartup
                '
                Me.chkCheckForUpdatesOnStartup.AutoSize = True
                Me.chkCheckForUpdatesOnStartup.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkCheckForUpdatesOnStartup.Location = New System.Drawing.Point(8, 8)
                Me.chkCheckForUpdatesOnStartup.Name = "chkCheckForUpdatesOnStartup"
                Me.chkCheckForUpdatesOnStartup.Size = New System.Drawing.Size(231, 19)
                Me.chkCheckForUpdatesOnStartup.TabIndex = 31
                Me.chkCheckForUpdatesOnStartup.Text = "Check for updates and announcements"
                Me.chkCheckForUpdatesOnStartup.UseVisualStyleBackColor = True
                '
                'cboUpdateCheckFrequency
                '
                Me.cboUpdateCheckFrequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
                Me.cboUpdateCheckFrequency.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.cboUpdateCheckFrequency.FormattingEnabled = True
                Me.cboUpdateCheckFrequency.Location = New System.Drawing.Point(48, 40)
                Me.cboUpdateCheckFrequency.Name = "cboUpdateCheckFrequency"
                Me.cboUpdateCheckFrequency.Size = New System.Drawing.Size(128, 23)
                Me.cboUpdateCheckFrequency.TabIndex = 135
                '
                'pnlProxy
                '
                Me.pnlProxy.Controls.Add(Me.pnlProxyBasic)
                Me.pnlProxy.Controls.Add(Me.chkUseProxyForAutomaticUpdates)
                Me.pnlProxy.Controls.Add(Me.chkUseProxyAuthentication)
                Me.pnlProxy.Controls.Add(Me.pnlProxyAuthentication)
                Me.pnlProxy.Controls.Add(Me.btnTestProxy)
                Me.pnlProxy.Location = New System.Drawing.Point(16, 216)
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
                Me.txtProxyAddress.Size = New System.Drawing.Size(240, 23)
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
                Me.numProxyPort.Size = New System.Drawing.Size(64, 23)
                Me.numProxyPort.TabIndex = 5001
                Me.numProxyPort.Value = New Decimal(New Integer() {80, 0, 0, 0})
                '
                'chkUseProxyForAutomaticUpdates
                '
                Me.chkUseProxyForAutomaticUpdates.AutoSize = True
                Me.chkUseProxyForAutomaticUpdates.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkUseProxyForAutomaticUpdates.Location = New System.Drawing.Point(8, 8)
                Me.chkUseProxyForAutomaticUpdates.Name = "chkUseProxyForAutomaticUpdates"
                Me.chkUseProxyForAutomaticUpdates.Size = New System.Drawing.Size(177, 19)
                Me.chkUseProxyForAutomaticUpdates.TabIndex = 80
                Me.chkUseProxyForAutomaticUpdates.Text = "Use a proxy server to connect"
                Me.chkUseProxyForAutomaticUpdates.UseVisualStyleBackColor = True
                '
                'chkUseProxyAuthentication
                '
                Me.chkUseProxyAuthentication.AutoSize = True
                Me.chkUseProxyAuthentication.Enabled = False
                Me.chkUseProxyAuthentication.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkUseProxyAuthentication.Location = New System.Drawing.Point(32, 80)
                Me.chkUseProxyAuthentication.Name = "chkUseProxyAuthentication"
                Me.chkUseProxyAuthentication.Size = New System.Drawing.Size(236, 19)
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
                Me.txtProxyUsername.Size = New System.Drawing.Size(240, 23)
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
                Me.txtProxyPassword.Size = New System.Drawing.Size(240, 23)
                Me.txtProxyPassword.TabIndex = 4
                Me.txtProxyPassword.UseSystemPasswordChar = True
                '
                'btnTestProxy
                '
                Me.btnTestProxy.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnTestProxy.Location = New System.Drawing.Point(8, 184)
                Me.btnTestProxy.Name = "btnTestProxy"
                Me.btnTestProxy.Size = New System.Drawing.Size(80, 32)
                Me.btnTestProxy.TabIndex = 111
                Me.btnTestProxy.Text = "Test Proxy"
                Me.btnTestProxy.UseVisualStyleBackColor = True
                '
                'tabStartupExit
                '
                Me.tabStartupExit.Controls.Add(Me.pnlStartup)
                Me.tabStartupExit.Icon = Global.mRemote.My.Resources.Resources.StartupExit_Icon
                Me.tabStartupExit.Location = New System.Drawing.Point(0, 0)
                Me.tabStartupExit.Name = "tabStartupExit"
                Me.tabStartupExit.Selected = False
                Me.tabStartupExit.Size = New System.Drawing.Size(573, 492)
                Me.tabStartupExit.TabIndex = 1000
                Me.tabStartupExit.Title = "Startup/Exit"
                '
                'pnlStartup
                '
                Me.pnlStartup.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlStartup.AutoScroll = True
                Me.pnlStartup.Controls.Add(Me.chkSaveConsOnExit)
                Me.pnlStartup.Controls.Add(Me.chkProperInstallationOfComponentsAtStartup)
                Me.pnlStartup.Controls.Add(Me.chkConfirmExit)
                Me.pnlStartup.Controls.Add(Me.chkSingleInstance)
                Me.pnlStartup.Controls.Add(Me.chkReconnectOnStart)
                Me.pnlStartup.Location = New System.Drawing.Point(3, 3)
                Me.pnlStartup.Name = "pnlStartup"
                Me.pnlStartup.Size = New System.Drawing.Size(567, 486)
                Me.pnlStartup.TabIndex = 51
                '
                'chkSaveConsOnExit
                '
                Me.chkSaveConsOnExit.AutoSize = True
                Me.chkSaveConsOnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkSaveConsOnExit.Location = New System.Drawing.Point(9, 3)
                Me.chkSaveConsOnExit.Name = "chkSaveConsOnExit"
                Me.chkSaveConsOnExit.Size = New System.Drawing.Size(153, 19)
                Me.chkSaveConsOnExit.TabIndex = 10
                Me.chkSaveConsOnExit.Text = "Save connections on exit"
                Me.chkSaveConsOnExit.UseVisualStyleBackColor = True
                '
                'chkProperInstallationOfComponentsAtStartup
                '
                Me.chkProperInstallationOfComponentsAtStartup.AutoSize = True
                Me.chkProperInstallationOfComponentsAtStartup.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkProperInstallationOfComponentsAtStartup.Location = New System.Drawing.Point(9, 97)
                Me.chkProperInstallationOfComponentsAtStartup.Name = "chkProperInstallationOfComponentsAtStartup"
                Me.chkProperInstallationOfComponentsAtStartup.Size = New System.Drawing.Size(292, 19)
                Me.chkProperInstallationOfComponentsAtStartup.TabIndex = 50
                Me.chkProperInstallationOfComponentsAtStartup.Text = "Check proper installation of components at startup"
                Me.chkProperInstallationOfComponentsAtStartup.UseVisualStyleBackColor = True
                '
                'chkConfirmExit
                '
                Me.chkConfirmExit.AutoSize = True
                Me.chkConfirmExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkConfirmExit.Location = New System.Drawing.Point(9, 26)
                Me.chkConfirmExit.Name = "chkConfirmExit"
                Me.chkConfirmExit.Size = New System.Drawing.Size(245, 19)
                Me.chkConfirmExit.TabIndex = 20
                Me.chkConfirmExit.Text = "Confirm exit if there are open connections"
                Me.chkConfirmExit.UseVisualStyleBackColor = True
                '
                'chkSingleInstance
                '
                Me.chkSingleInstance.AutoSize = True
                Me.chkSingleInstance.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkSingleInstance.Location = New System.Drawing.Point(9, 74)
                Me.chkSingleInstance.Name = "chkSingleInstance"
                Me.chkSingleInstance.Size = New System.Drawing.Size(411, 19)
                Me.chkSingleInstance.TabIndex = 50
                Me.chkSingleInstance.Text = "Allow only a single instance of the application (mRemote restart required)"
                Me.chkSingleInstance.UseVisualStyleBackColor = True
                '
                'chkReconnectOnStart
                '
                Me.chkReconnectOnStart.AutoSize = True
                Me.chkReconnectOnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkReconnectOnStart.Location = New System.Drawing.Point(9, 51)
                Me.chkReconnectOnStart.Name = "chkReconnectOnStart"
                Me.chkReconnectOnStart.Size = New System.Drawing.Size(296, 19)
                Me.chkReconnectOnStart.TabIndex = 40
                Me.chkReconnectOnStart.Text = "Reconnect to previously opened sessions on startup"
                Me.chkReconnectOnStart.UseVisualStyleBackColor = True
                '
                'tabAppearance
                '
                Me.tabAppearance.Controls.Add(Me.pnlAppearance)
                Me.tabAppearance.Icon = Global.mRemote.My.Resources.Resources.Appearance_Icon
                Me.tabAppearance.Location = New System.Drawing.Point(0, 0)
                Me.tabAppearance.Name = "tabAppearance"
                Me.tabAppearance.Selected = False
                Me.tabAppearance.Size = New System.Drawing.Size(573, 492)
                Me.tabAppearance.TabIndex = 2000
                Me.tabAppearance.Title = "Appearance"
                '
                'pnlAppearance
                '
                Me.pnlAppearance.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlAppearance.AutoScroll = True
                Me.pnlAppearance.Controls.Add(Me.chkShowDescriptionTooltipsInTree)
                Me.pnlAppearance.Controls.Add(Me.chkMinimizeToSystemTray)
                Me.pnlAppearance.Controls.Add(Me.chkShowSystemTrayIcon)
                Me.pnlAppearance.Controls.Add(Me.chkShowFullConnectionsFilePathInTitle)
                Me.pnlAppearance.Location = New System.Drawing.Point(3, 3)
                Me.pnlAppearance.Name = "pnlAppearance"
                Me.pnlAppearance.Size = New System.Drawing.Size(567, 486)
                Me.pnlAppearance.TabIndex = 41
                '
                'chkShowDescriptionTooltipsInTree
                '
                Me.chkShowDescriptionTooltipsInTree.AutoSize = True
                Me.chkShowDescriptionTooltipsInTree.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkShowDescriptionTooltipsInTree.Location = New System.Drawing.Point(9, 3)
                Me.chkShowDescriptionTooltipsInTree.Name = "chkShowDescriptionTooltipsInTree"
                Me.chkShowDescriptionTooltipsInTree.Size = New System.Drawing.Size(256, 19)
                Me.chkShowDescriptionTooltipsInTree.TabIndex = 10
                Me.chkShowDescriptionTooltipsInTree.Text = "Show description tooltips in connection tree"
                Me.chkShowDescriptionTooltipsInTree.UseVisualStyleBackColor = True
                '
                'chkMinimizeToSystemTray
                '
                Me.chkMinimizeToSystemTray.AutoSize = True
                Me.chkMinimizeToSystemTray.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkMinimizeToSystemTray.Location = New System.Drawing.Point(9, 82)
                Me.chkMinimizeToSystemTray.Name = "chkMinimizeToSystemTray"
                Me.chkMinimizeToSystemTray.Size = New System.Drawing.Size(153, 19)
                Me.chkMinimizeToSystemTray.TabIndex = 40
                Me.chkMinimizeToSystemTray.Text = "Minimize to System Tray"
                Me.chkMinimizeToSystemTray.UseVisualStyleBackColor = True
                '
                'chkShowSystemTrayIcon
                '
                Me.chkShowSystemTrayIcon.AutoSize = True
                Me.chkShowSystemTrayIcon.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkShowSystemTrayIcon.Location = New System.Drawing.Point(9, 59)
                Me.chkShowSystemTrayIcon.Name = "chkShowSystemTrayIcon"
                Me.chkShowSystemTrayIcon.Size = New System.Drawing.Size(184, 19)
                Me.chkShowSystemTrayIcon.TabIndex = 30
                Me.chkShowSystemTrayIcon.Text = "Always show System Tray Icon"
                Me.chkShowSystemTrayIcon.UseVisualStyleBackColor = True
                '
                'chkShowFullConnectionsFilePathInTitle
                '
                Me.chkShowFullConnectionsFilePathInTitle.AutoSize = True
                Me.chkShowFullConnectionsFilePathInTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkShowFullConnectionsFilePathInTitle.Location = New System.Drawing.Point(9, 26)
                Me.chkShowFullConnectionsFilePathInTitle.Name = "chkShowFullConnectionsFilePathInTitle"
                Me.chkShowFullConnectionsFilePathInTitle.Size = New System.Drawing.Size(267, 19)
                Me.chkShowFullConnectionsFilePathInTitle.TabIndex = 20
                Me.chkShowFullConnectionsFilePathInTitle.Text = "Show full connections file path in window title"
                Me.chkShowFullConnectionsFilePathInTitle.UseVisualStyleBackColor = True
                '
                'tabTabs
                '
                Me.tabTabs.Controls.Add(Me.pnlTabsAndPanels)
                Me.tabTabs.Icon = Global.mRemote.My.Resources.Resources.Tab_Icon
                Me.tabTabs.Location = New System.Drawing.Point(0, 0)
                Me.tabTabs.Name = "tabTabs"
                Me.tabTabs.Selected = False
                Me.tabTabs.Size = New System.Drawing.Size(573, 492)
                Me.tabTabs.TabIndex = 3000
                Me.tabTabs.Title = "Tabs && Panels"
                '
                'pnlTabsAndPanels
                '
                Me.pnlTabsAndPanels.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlTabsAndPanels.AutoScroll = True
                Me.pnlTabsAndPanels.Controls.Add(Me.chkUseOnlyErrorsAndInfosPanel)
                Me.pnlTabsAndPanels.Controls.Add(Me.lblSwitchToErrorsAndInfos)
                Me.pnlTabsAndPanels.Controls.Add(Me.chkMCInformation)
                Me.pnlTabsAndPanels.Controls.Add(Me.chkMCErrors)
                Me.pnlTabsAndPanels.Controls.Add(Me.chkMCWarnings)
                Me.pnlTabsAndPanels.Controls.Add(Me.chkOpenNewTabRightOfSelected)
                Me.pnlTabsAndPanels.Controls.Add(Me.chkShowProtocolOnTabs)
                Me.pnlTabsAndPanels.Controls.Add(Me.chkDoubleClickClosesTab)
                Me.pnlTabsAndPanels.Controls.Add(Me.chkShowLogonInfoOnTabs)
                Me.pnlTabsAndPanels.Controls.Add(Me.chkAlwaysShowPanelSelectionDlg)
                Me.pnlTabsAndPanels.Location = New System.Drawing.Point(3, 3)
                Me.pnlTabsAndPanels.Name = "pnlTabsAndPanels"
                Me.pnlTabsAndPanels.Size = New System.Drawing.Size(567, 486)
                Me.pnlTabsAndPanels.TabIndex = 51
                '
                'chkUseOnlyErrorsAndInfosPanel
                '
                Me.chkUseOnlyErrorsAndInfosPanel.AutoSize = True
                Me.chkUseOnlyErrorsAndInfosPanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkUseOnlyErrorsAndInfosPanel.Location = New System.Drawing.Point(9, 146)
                Me.chkUseOnlyErrorsAndInfosPanel.Name = "chkUseOnlyErrorsAndInfosPanel"
                Me.chkUseOnlyErrorsAndInfosPanel.Size = New System.Drawing.Size(307, 19)
                Me.chkUseOnlyErrorsAndInfosPanel.TabIndex = 51
                Me.chkUseOnlyErrorsAndInfosPanel.Text = "Use only Notifications panel (no messagebox popups)"
                Me.chkUseOnlyErrorsAndInfosPanel.UseVisualStyleBackColor = True
                '
                'lblSwitchToErrorsAndInfos
                '
                Me.lblSwitchToErrorsAndInfos.AutoSize = True
                Me.lblSwitchToErrorsAndInfos.Location = New System.Drawing.Point(9, 171)
                Me.lblSwitchToErrorsAndInfos.Name = "lblSwitchToErrorsAndInfos"
                Me.lblSwitchToErrorsAndInfos.Size = New System.Drawing.Size(179, 15)
                Me.lblSwitchToErrorsAndInfos.TabIndex = 52
                Me.lblSwitchToErrorsAndInfos.Text = "Switch to Notifications panel on:"
                '
                'chkMCInformation
                '
                Me.chkMCInformation.AutoSize = True
                Me.chkMCInformation.Enabled = False
                Me.chkMCInformation.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkMCInformation.Location = New System.Drawing.Point(25, 191)
                Me.chkMCInformation.Name = "chkMCInformation"
                Me.chkMCInformation.Size = New System.Drawing.Size(91, 19)
                Me.chkMCInformation.TabIndex = 53
                Me.chkMCInformation.Text = "Informations"
                Me.chkMCInformation.UseVisualStyleBackColor = True
                '
                'chkMCErrors
                '
                Me.chkMCErrors.AutoSize = True
                Me.chkMCErrors.Enabled = False
                Me.chkMCErrors.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkMCErrors.Location = New System.Drawing.Point(223, 191)
                Me.chkMCErrors.Name = "chkMCErrors"
                Me.chkMCErrors.Size = New System.Drawing.Size(53, 19)
                Me.chkMCErrors.TabIndex = 55
                Me.chkMCErrors.Text = "Errors"
                Me.chkMCErrors.UseVisualStyleBackColor = True
                '
                'chkMCWarnings
                '
                Me.chkMCWarnings.AutoSize = True
                Me.chkMCWarnings.Enabled = False
                Me.chkMCWarnings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkMCWarnings.Location = New System.Drawing.Point(132, 191)
                Me.chkMCWarnings.Name = "chkMCWarnings"
                Me.chkMCWarnings.Size = New System.Drawing.Size(73, 19)
                Me.chkMCWarnings.TabIndex = 54
                Me.chkMCWarnings.Text = "Warnings"
                Me.chkMCWarnings.UseVisualStyleBackColor = True
                '
                'chkOpenNewTabRightOfSelected
                '
                Me.chkOpenNewTabRightOfSelected.AutoSize = True
                Me.chkOpenNewTabRightOfSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkOpenNewTabRightOfSelected.Location = New System.Drawing.Point(9, 3)
                Me.chkOpenNewTabRightOfSelected.Name = "chkOpenNewTabRightOfSelected"
                Me.chkOpenNewTabRightOfSelected.Size = New System.Drawing.Size(309, 19)
                Me.chkOpenNewTabRightOfSelected.TabIndex = 10
                Me.chkOpenNewTabRightOfSelected.Text = "Open new tab to the right of the currently selected tab"
                Me.chkOpenNewTabRightOfSelected.UseVisualStyleBackColor = True
                '
                'chkShowProtocolOnTabs
                '
                Me.chkShowProtocolOnTabs.AutoSize = True
                Me.chkShowProtocolOnTabs.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkShowProtocolOnTabs.Location = New System.Drawing.Point(9, 49)
                Me.chkShowProtocolOnTabs.Name = "chkShowProtocolOnTabs"
                Me.chkShowProtocolOnTabs.Size = New System.Drawing.Size(180, 19)
                Me.chkShowProtocolOnTabs.TabIndex = 30
                Me.chkShowProtocolOnTabs.Text = "Show protocols on tab names"
                Me.chkShowProtocolOnTabs.UseVisualStyleBackColor = True
                '
                'chkDoubleClickClosesTab
                '
                Me.chkDoubleClickClosesTab.AutoSize = True
                Me.chkDoubleClickClosesTab.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkDoubleClickClosesTab.Location = New System.Drawing.Point(9, 72)
                Me.chkDoubleClickClosesTab.Name = "chkDoubleClickClosesTab"
                Me.chkDoubleClickClosesTab.Size = New System.Drawing.Size(170, 19)
                Me.chkDoubleClickClosesTab.TabIndex = 40
                Me.chkDoubleClickClosesTab.Text = "Double click on tab closes it"
                Me.chkDoubleClickClosesTab.UseVisualStyleBackColor = True
                '
                'chkShowLogonInfoOnTabs
                '
                Me.chkShowLogonInfoOnTabs.AutoSize = True
                Me.chkShowLogonInfoOnTabs.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkShowLogonInfoOnTabs.Location = New System.Drawing.Point(9, 26)
                Me.chkShowLogonInfoOnTabs.Name = "chkShowLogonInfoOnTabs"
                Me.chkShowLogonInfoOnTabs.Size = New System.Drawing.Size(227, 19)
                Me.chkShowLogonInfoOnTabs.TabIndex = 20
                Me.chkShowLogonInfoOnTabs.Text = "Show logon information on tab names"
                Me.chkShowLogonInfoOnTabs.UseVisualStyleBackColor = True
                '
                'chkAlwaysShowPanelSelectionDlg
                '
                Me.chkAlwaysShowPanelSelectionDlg.AutoSize = True
                Me.chkAlwaysShowPanelSelectionDlg.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkAlwaysShowPanelSelectionDlg.Location = New System.Drawing.Point(9, 95)
                Me.chkAlwaysShowPanelSelectionDlg.Name = "chkAlwaysShowPanelSelectionDlg"
                Me.chkAlwaysShowPanelSelectionDlg.Size = New System.Drawing.Size(349, 19)
                Me.chkAlwaysShowPanelSelectionDlg.TabIndex = 50
                Me.chkAlwaysShowPanelSelectionDlg.Text = "Always show panel selection dialog when opening connectins"
                Me.chkAlwaysShowPanelSelectionDlg.UseVisualStyleBackColor = True
                '
                'tabConnections
                '
                Me.tabConnections.Controls.Add(Me.pnlConnections)
                Me.tabConnections.Icon = Global.mRemote.My.Resources.Resources.Root_Icon
                Me.tabConnections.Location = New System.Drawing.Point(0, 0)
                Me.tabConnections.Name = "tabConnections"
                Me.tabConnections.Size = New System.Drawing.Size(573, 492)
                Me.tabConnections.TabIndex = 4000
                Me.tabConnections.Title = "Connections"
                '
                'pnlConnections
                '
                Me.pnlConnections.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlConnections.AutoScroll = True
                Me.pnlConnections.Controls.Add(Me.chkSingleClickOnConnectionOpensIt)
                Me.pnlConnections.Controls.Add(Me.pnlDefaultCredentials)
                Me.pnlConnections.Controls.Add(Me.chkSingleClickOnOpenedConnectionSwitchesToIt)
                Me.pnlConnections.Controls.Add(Me.pnlAutoSave)
                Me.pnlConnections.Controls.Add(Me.grpExperimental)
                Me.pnlConnections.Controls.Add(Me.chkHostnameLikeDisplayName)
                Me.pnlConnections.Location = New System.Drawing.Point(3, 3)
                Me.pnlConnections.Name = "pnlConnections"
                Me.pnlConnections.Size = New System.Drawing.Size(567, 486)
                Me.pnlConnections.TabIndex = 173
                '
                'chkSingleClickOnConnectionOpensIt
                '
                Me.chkSingleClickOnConnectionOpensIt.AutoSize = True
                Me.chkSingleClickOnConnectionOpensIt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkSingleClickOnConnectionOpensIt.Location = New System.Drawing.Point(9, 3)
                Me.chkSingleClickOnConnectionOpensIt.Name = "chkSingleClickOnConnectionOpensIt"
                Me.chkSingleClickOnConnectionOpensIt.Size = New System.Drawing.Size(207, 19)
                Me.chkSingleClickOnConnectionOpensIt.TabIndex = 10
                Me.chkSingleClickOnConnectionOpensIt.Text = "Single click on connection opens it"
                Me.chkSingleClickOnConnectionOpensIt.UseVisualStyleBackColor = True
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
                Me.pnlDefaultCredentials.Location = New System.Drawing.Point(9, 255)
                Me.pnlDefaultCredentials.Name = "pnlDefaultCredentials"
                Me.pnlDefaultCredentials.Size = New System.Drawing.Size(500, 175)
                Me.pnlDefaultCredentials.TabIndex = 172
                '
                'radCredentialsCustom
                '
                Me.radCredentialsCustom.AutoSize = True
                Me.radCredentialsCustom.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.radCredentialsCustom.Location = New System.Drawing.Point(21, 69)
                Me.radCredentialsCustom.Name = "radCredentialsCustom"
                Me.radCredentialsCustom.Size = New System.Drawing.Size(97, 19)
                Me.radCredentialsCustom.TabIndex = 110
                Me.radCredentialsCustom.TabStop = True
                Me.radCredentialsCustom.Text = "the following:"
                Me.radCredentialsCustom.UseVisualStyleBackColor = True
                '
                'lblDefaultCredentials
                '
                Me.lblDefaultCredentials.AutoSize = True
                Me.lblDefaultCredentials.Location = New System.Drawing.Point(8, 9)
                Me.lblDefaultCredentials.Name = "lblDefaultCredentials"
                Me.lblDefaultCredentials.Size = New System.Drawing.Size(287, 15)
                Me.lblDefaultCredentials.TabIndex = 80
                Me.lblDefaultCredentials.Text = "For empty Username, Password or Domain fields use:"
                '
                'radCredentialsNoInfo
                '
                Me.radCredentialsNoInfo.AutoSize = True
                Me.radCredentialsNoInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.radCredentialsNoInfo.Location = New System.Drawing.Point(21, 31)
                Me.radCredentialsNoInfo.Name = "radCredentialsNoInfo"
                Me.radCredentialsNoInfo.Size = New System.Drawing.Size(104, 19)
                Me.radCredentialsNoInfo.TabIndex = 90
                Me.radCredentialsNoInfo.TabStop = True
                Me.radCredentialsNoInfo.Text = "no information"
                Me.radCredentialsNoInfo.UseVisualStyleBackColor = True
                '
                'radCredentialsWindows
                '
                Me.radCredentialsWindows.AutoSize = True
                Me.radCredentialsWindows.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.radCredentialsWindows.Location = New System.Drawing.Point(21, 50)
                Me.radCredentialsWindows.Name = "radCredentialsWindows"
                Me.radCredentialsWindows.Size = New System.Drawing.Size(258, 19)
                Me.radCredentialsWindows.TabIndex = 100
                Me.radCredentialsWindows.TabStop = True
                Me.radCredentialsWindows.Text = "my current credentials (windows logon info)"
                Me.radCredentialsWindows.UseVisualStyleBackColor = True
                '
                'txtCredentialsDomain
                '
                Me.txtCredentialsDomain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.txtCredentialsDomain.Enabled = False
                Me.txtCredentialsDomain.Location = New System.Drawing.Point(130, 147)
                Me.txtCredentialsDomain.Name = "txtCredentialsDomain"
                Me.txtCredentialsDomain.Size = New System.Drawing.Size(150, 23)
                Me.txtCredentialsDomain.TabIndex = 170
                '
                'lblCredentialsUsername
                '
                Me.lblCredentialsUsername.AutoSize = True
                Me.lblCredentialsUsername.Enabled = False
                Me.lblCredentialsUsername.Location = New System.Drawing.Point(39, 95)
                Me.lblCredentialsUsername.Name = "lblCredentialsUsername"
                Me.lblCredentialsUsername.Size = New System.Drawing.Size(63, 15)
                Me.lblCredentialsUsername.TabIndex = 120
                Me.lblCredentialsUsername.Text = "Username:"
                '
                'txtCredentialsPassword
                '
                Me.txtCredentialsPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.txtCredentialsPassword.Enabled = False
                Me.txtCredentialsPassword.Location = New System.Drawing.Point(130, 120)
                Me.txtCredentialsPassword.Name = "txtCredentialsPassword"
                Me.txtCredentialsPassword.Size = New System.Drawing.Size(150, 23)
                Me.txtCredentialsPassword.TabIndex = 150
                Me.txtCredentialsPassword.UseSystemPasswordChar = True
                '
                'lblCredentialsPassword
                '
                Me.lblCredentialsPassword.AutoSize = True
                Me.lblCredentialsPassword.Enabled = False
                Me.lblCredentialsPassword.Location = New System.Drawing.Point(39, 123)
                Me.lblCredentialsPassword.Name = "lblCredentialsPassword"
                Me.lblCredentialsPassword.Size = New System.Drawing.Size(60, 15)
                Me.lblCredentialsPassword.TabIndex = 140
                Me.lblCredentialsPassword.Text = "Password:"
                '
                'txtCredentialsUsername
                '
                Me.txtCredentialsUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.txtCredentialsUsername.Enabled = False
                Me.txtCredentialsUsername.Location = New System.Drawing.Point(130, 93)
                Me.txtCredentialsUsername.Name = "txtCredentialsUsername"
                Me.txtCredentialsUsername.Size = New System.Drawing.Size(150, 23)
                Me.txtCredentialsUsername.TabIndex = 130
                '
                'lblCredentialsDomain
                '
                Me.lblCredentialsDomain.AutoSize = True
                Me.lblCredentialsDomain.Enabled = False
                Me.lblCredentialsDomain.Location = New System.Drawing.Point(39, 150)
                Me.lblCredentialsDomain.Name = "lblCredentialsDomain"
                Me.lblCredentialsDomain.Size = New System.Drawing.Size(52, 15)
                Me.lblCredentialsDomain.TabIndex = 160
                Me.lblCredentialsDomain.Text = "Domain:"
                '
                'chkSingleClickOnOpenedConnectionSwitchesToIt
                '
                Me.chkSingleClickOnOpenedConnectionSwitchesToIt.AutoSize = True
                Me.chkSingleClickOnOpenedConnectionSwitchesToIt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkSingleClickOnOpenedConnectionSwitchesToIt.Location = New System.Drawing.Point(9, 26)
                Me.chkSingleClickOnOpenedConnectionSwitchesToIt.Name = "chkSingleClickOnOpenedConnectionSwitchesToIt"
                Me.chkSingleClickOnOpenedConnectionSwitchesToIt.Size = New System.Drawing.Size(277, 19)
                Me.chkSingleClickOnOpenedConnectionSwitchesToIt.TabIndex = 20
                Me.chkSingleClickOnOpenedConnectionSwitchesToIt.Text = "Single click on opened connection switches to it"
                Me.chkSingleClickOnOpenedConnectionSwitchesToIt.UseVisualStyleBackColor = True
                '
                'pnlAutoSave
                '
                Me.pnlAutoSave.Controls.Add(Me.lblAutoSave1)
                Me.pnlAutoSave.Controls.Add(Me.numAutoSave)
                Me.pnlAutoSave.Controls.Add(Me.lblAutoSave2)
                Me.pnlAutoSave.Location = New System.Drawing.Point(9, 67)
                Me.pnlAutoSave.Name = "pnlAutoSave"
                Me.pnlAutoSave.Size = New System.Drawing.Size(500, 29)
                Me.pnlAutoSave.TabIndex = 171
                '
                'lblAutoSave1
                '
                Me.lblAutoSave1.AutoSize = True
                Me.lblAutoSave1.Location = New System.Drawing.Point(3, 9)
                Me.lblAutoSave1.Name = "lblAutoSave1"
                Me.lblAutoSave1.Size = New System.Drawing.Size(94, 15)
                Me.lblAutoSave1.TabIndex = 40
                Me.lblAutoSave1.Text = "Auto Save every:"
                '
                'numAutoSave
                '
                Me.numAutoSave.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.numAutoSave.Location = New System.Drawing.Point(168, 6)
                Me.numAutoSave.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
                Me.numAutoSave.Name = "numAutoSave"
                Me.numAutoSave.Size = New System.Drawing.Size(53, 23)
                Me.numAutoSave.TabIndex = 50
                '
                'lblAutoSave2
                '
                Me.lblAutoSave2.AutoSize = True
                Me.lblAutoSave2.Location = New System.Drawing.Point(233, 9)
                Me.lblAutoSave2.Name = "lblAutoSave2"
                Me.lblAutoSave2.Size = New System.Drawing.Size(152, 15)
                Me.lblAutoSave2.TabIndex = 60
                Me.lblAutoSave2.Text = "Minutes (0 means disabled)"
                '
                'grpExperimental
                '
                Me.grpExperimental.BackColor = System.Drawing.Color.IndianRed
                Me.grpExperimental.Controls.Add(Me.chkUseSQLServer)
                Me.grpExperimental.Controls.Add(Me.lblSQLInfo)
                Me.grpExperimental.Controls.Add(Me.lblSQLUsername)
                Me.grpExperimental.Controls.Add(Me.txtSQLPassword)
                Me.grpExperimental.Controls.Add(Me.lblSQLServer)
                Me.grpExperimental.Controls.Add(Me.txtSQLUsername)
                Me.grpExperimental.Controls.Add(Me.lblSQLPassword)
                Me.grpExperimental.Controls.Add(Me.txtSQLServer)
                Me.grpExperimental.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.grpExperimental.Location = New System.Drawing.Point(9, 102)
                Me.grpExperimental.Name = "grpExperimental"
                Me.grpExperimental.Size = New System.Drawing.Size(500, 147)
                Me.grpExperimental.TabIndex = 70
                Me.grpExperimental.TabStop = False
                Me.grpExperimental.Text = "EXPERIMENTAL"
                '
                'chkUseSQLServer
                '
                Me.chkUseSQLServer.AutoSize = True
                Me.chkUseSQLServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkUseSQLServer.Location = New System.Drawing.Point(16, 20)
                Me.chkUseSQLServer.Name = "chkUseSQLServer"
                Me.chkUseSQLServer.Size = New System.Drawing.Size(248, 19)
                Me.chkUseSQLServer.TabIndex = 50
                Me.chkUseSQLServer.Text = "Use SQL Server to load && save connections"
                Me.chkUseSQLServer.UseVisualStyleBackColor = True
                '
                'lblSQLInfo
                '
                Me.lblSQLInfo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.lblSQLInfo.Enabled = False
                Me.lblSQLInfo.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World)
                Me.lblSQLInfo.Location = New System.Drawing.Point(291, 11)
                Me.lblSQLInfo.Name = "lblSQLInfo"
                Me.lblSQLInfo.Size = New System.Drawing.Size(203, 131)
                Me.lblSQLInfo.TabIndex = 120
                Me.lblSQLInfo.Text = "Please see Help - Getting started - SQL Configuration for more Info!"
                Me.lblSQLInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                '
                'lblSQLUsername
                '
                Me.lblSQLUsername.AutoSize = True
                Me.lblSQLUsername.Enabled = False
                Me.lblSQLUsername.Location = New System.Drawing.Point(36, 77)
                Me.lblSQLUsername.Name = "lblSQLUsername"
                Me.lblSQLUsername.Size = New System.Drawing.Size(63, 15)
                Me.lblSQLUsername.TabIndex = 80
                Me.lblSQLUsername.Text = "Username:"
                '
                'txtSQLPassword
                '
                Me.txtSQLPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.txtSQLPassword.Enabled = False
                Me.txtSQLPassword.Location = New System.Drawing.Point(126, 101)
                Me.txtSQLPassword.Name = "txtSQLPassword"
                Me.txtSQLPassword.Size = New System.Drawing.Size(153, 23)
                Me.txtSQLPassword.TabIndex = 110
                Me.txtSQLPassword.UseSystemPasswordChar = True
                '
                'lblSQLServer
                '
                Me.lblSQLServer.AutoSize = True
                Me.lblSQLServer.Enabled = False
                Me.lblSQLServer.Location = New System.Drawing.Point(36, 50)
                Me.lblSQLServer.Name = "lblSQLServer"
                Me.lblSQLServer.Size = New System.Drawing.Size(66, 15)
                Me.lblSQLServer.TabIndex = 60
                Me.lblSQLServer.Text = "SQL Server:"
                '
                'txtSQLUsername
                '
                Me.txtSQLUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.txtSQLUsername.Enabled = False
                Me.txtSQLUsername.Location = New System.Drawing.Point(126, 74)
                Me.txtSQLUsername.Name = "txtSQLUsername"
                Me.txtSQLUsername.Size = New System.Drawing.Size(153, 23)
                Me.txtSQLUsername.TabIndex = 90
                '
                'lblSQLPassword
                '
                Me.lblSQLPassword.AutoSize = True
                Me.lblSQLPassword.Enabled = False
                Me.lblSQLPassword.Location = New System.Drawing.Point(36, 104)
                Me.lblSQLPassword.Name = "lblSQLPassword"
                Me.lblSQLPassword.Size = New System.Drawing.Size(60, 15)
                Me.lblSQLPassword.TabIndex = 100
                Me.lblSQLPassword.Text = "Password:"
                '
                'txtSQLServer
                '
                Me.txtSQLServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.txtSQLServer.Enabled = False
                Me.txtSQLServer.Location = New System.Drawing.Point(126, 47)
                Me.txtSQLServer.Name = "txtSQLServer"
                Me.txtSQLServer.Size = New System.Drawing.Size(153, 23)
                Me.txtSQLServer.TabIndex = 70
                '
                'chkHostnameLikeDisplayName
                '
                Me.chkHostnameLikeDisplayName.AutoSize = True
                Me.chkHostnameLikeDisplayName.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkHostnameLikeDisplayName.Location = New System.Drawing.Point(9, 49)
                Me.chkHostnameLikeDisplayName.Name = "chkHostnameLikeDisplayName"
                Me.chkHostnameLikeDisplayName.Size = New System.Drawing.Size(360, 19)
                Me.chkHostnameLikeDisplayName.TabIndex = 30
                Me.chkHostnameLikeDisplayName.Text = "Set hostname like display name when creating new connections"
                Me.chkHostnameLikeDisplayName.UseVisualStyleBackColor = True
                '
                'tabAdvanced
                '
                Me.tabAdvanced.Controls.Add(Me.pnlAdvanced)
                Me.tabAdvanced.Icon = Global.mRemote.My.Resources.Resources.Config_Icon
                Me.tabAdvanced.Location = New System.Drawing.Point(0, 0)
                Me.tabAdvanced.Name = "tabAdvanced"
                Me.tabAdvanced.Selected = False
                Me.tabAdvanced.Size = New System.Drawing.Size(573, 492)
                Me.tabAdvanced.TabIndex = 5000
                Me.tabAdvanced.Title = "Advanced"
                '
                'pnlAdvanced
                '
                Me.pnlAdvanced.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlAdvanced.AutoScroll = True
                Me.pnlAdvanced.Controls.Add(Me.chkWriteLogFile)
                Me.pnlAdvanced.Controls.Add(Me.chkAutomaticallyGetSessionInfo)
                Me.pnlAdvanced.Controls.Add(Me.lblXulRunnerPath)
                Me.pnlAdvanced.Controls.Add(Me.chkEncryptCompleteFile)
                Me.pnlAdvanced.Controls.Add(Me.btnBrowseXulRunnerPath)
                Me.pnlAdvanced.Controls.Add(Me.chkUseCustomPuttyPath)
                Me.pnlAdvanced.Controls.Add(Me.txtXULrunnerPath)
                Me.pnlAdvanced.Controls.Add(Me.txtCustomPuttyPath)
                Me.pnlAdvanced.Controls.Add(Me.Label3)
                Me.pnlAdvanced.Controls.Add(Me.btnBrowseCustomPuttyPath)
                Me.pnlAdvanced.Controls.Add(Me.Label2)
                Me.pnlAdvanced.Controls.Add(Me.btnLaunchPutty)
                Me.pnlAdvanced.Controls.Add(Me.numUVNCSCPort)
                Me.pnlAdvanced.Controls.Add(Me.lblConfigurePuttySessions)
                Me.pnlAdvanced.Controls.Add(Me.numPuttyWaitTime)
                Me.pnlAdvanced.Controls.Add(Me.chkAutomaticReconnect)
                Me.pnlAdvanced.Controls.Add(Me.Label1)
                Me.pnlAdvanced.Location = New System.Drawing.Point(3, 3)
                Me.pnlAdvanced.Name = "pnlAdvanced"
                Me.pnlAdvanced.Size = New System.Drawing.Size(567, 486)
                Me.pnlAdvanced.TabIndex = 135
                '
                'chkWriteLogFile
                '
                Me.chkWriteLogFile.AutoSize = True
                Me.chkWriteLogFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkWriteLogFile.Location = New System.Drawing.Point(9, 3)
                Me.chkWriteLogFile.Name = "chkWriteLogFile"
                Me.chkWriteLogFile.Size = New System.Drawing.Size(190, 19)
                Me.chkWriteLogFile.TabIndex = 10
                Me.chkWriteLogFile.Text = "Write log file (mRemoteNG.log)"
                Me.chkWriteLogFile.UseVisualStyleBackColor = True
                '
                'chkAutomaticallyGetSessionInfo
                '
                Me.chkAutomaticallyGetSessionInfo.AutoSize = True
                Me.chkAutomaticallyGetSessionInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkAutomaticallyGetSessionInfo.Location = New System.Drawing.Point(9, 49)
                Me.chkAutomaticallyGetSessionInfo.Name = "chkAutomaticallyGetSessionInfo"
                Me.chkAutomaticallyGetSessionInfo.Size = New System.Drawing.Size(224, 19)
                Me.chkAutomaticallyGetSessionInfo.TabIndex = 20
                Me.chkAutomaticallyGetSessionInfo.Text = "Automatically get session information"
                Me.chkAutomaticallyGetSessionInfo.UseVisualStyleBackColor = True
                '
                'lblXulRunnerPath
                '
                Me.lblXulRunnerPath.AutoSize = True
                Me.lblXulRunnerPath.Location = New System.Drawing.Point(9, 220)
                Me.lblXulRunnerPath.Name = "lblXulRunnerPath"
                Me.lblXulRunnerPath.Size = New System.Drawing.Size(93, 15)
                Me.lblXulRunnerPath.TabIndex = 133
                Me.lblXulRunnerPath.Text = "XULrunner path:"
                '
                'chkEncryptCompleteFile
                '
                Me.chkEncryptCompleteFile.AutoSize = True
                Me.chkEncryptCompleteFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkEncryptCompleteFile.Location = New System.Drawing.Point(8, 26)
                Me.chkEncryptCompleteFile.Name = "chkEncryptCompleteFile"
                Me.chkEncryptCompleteFile.Size = New System.Drawing.Size(198, 19)
                Me.chkEncryptCompleteFile.TabIndex = 20
                Me.chkEncryptCompleteFile.Text = "Encrypt complete connection file"
                Me.chkEncryptCompleteFile.UseVisualStyleBackColor = True
                '
                'btnBrowseXulRunnerPath
                '
                Me.btnBrowseXulRunnerPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnBrowseXulRunnerPath.Location = New System.Drawing.Point(297, 236)
                Me.btnBrowseXulRunnerPath.Name = "btnBrowseXulRunnerPath"
                Me.btnBrowseXulRunnerPath.Size = New System.Drawing.Size(75, 23)
                Me.btnBrowseXulRunnerPath.TabIndex = 132
                Me.btnBrowseXulRunnerPath.Text = "Browse..."
                Me.btnBrowseXulRunnerPath.UseVisualStyleBackColor = True
                '
                'chkUseCustomPuttyPath
                '
                Me.chkUseCustomPuttyPath.AutoSize = True
                Me.chkUseCustomPuttyPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkUseCustomPuttyPath.Location = New System.Drawing.Point(9, 95)
                Me.chkUseCustomPuttyPath.Name = "chkUseCustomPuttyPath"
                Me.chkUseCustomPuttyPath.Size = New System.Drawing.Size(153, 19)
                Me.chkUseCustomPuttyPath.TabIndex = 30
                Me.chkUseCustomPuttyPath.Text = "Use custom PuTTY path:"
                Me.chkUseCustomPuttyPath.UseVisualStyleBackColor = True
                '
                'txtXULrunnerPath
                '
                Me.txtXULrunnerPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.txtXULrunnerPath.Location = New System.Drawing.Point(27, 238)
                Me.txtXULrunnerPath.Name = "txtXULrunnerPath"
                Me.txtXULrunnerPath.Size = New System.Drawing.Size(264, 23)
                Me.txtXULrunnerPath.TabIndex = 131
                '
                'txtCustomPuttyPath
                '
                Me.txtCustomPuttyPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.txtCustomPuttyPath.Enabled = False
                Me.txtCustomPuttyPath.Location = New System.Drawing.Point(27, 118)
                Me.txtCustomPuttyPath.Name = "txtCustomPuttyPath"
                Me.txtCustomPuttyPath.Size = New System.Drawing.Size(264, 23)
                Me.txtCustomPuttyPath.TabIndex = 40
                '
                'Label3
                '
                Me.Label3.AutoSize = True
                Me.Label3.Location = New System.Drawing.Point(9, 281)
                Me.Label3.Name = "Label3"
                Me.Label3.Size = New System.Drawing.Size(196, 15)
                Me.Label3.TabIndex = 120
                Me.Label3.Text = "UltraVNC SingleClick Listening Port:"
                '
                'btnBrowseCustomPuttyPath
                '
                Me.btnBrowseCustomPuttyPath.Enabled = False
                Me.btnBrowseCustomPuttyPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnBrowseCustomPuttyPath.Location = New System.Drawing.Point(297, 116)
                Me.btnBrowseCustomPuttyPath.Name = "btnBrowseCustomPuttyPath"
                Me.btnBrowseCustomPuttyPath.Size = New System.Drawing.Size(75, 23)
                Me.btnBrowseCustomPuttyPath.TabIndex = 50
                Me.btnBrowseCustomPuttyPath.Text = "Browse..."
                Me.btnBrowseCustomPuttyPath.UseVisualStyleBackColor = True
                '
                'Label2
                '
                Me.Label2.AutoSize = True
                Me.Label2.Location = New System.Drawing.Point(434, 189)
                Me.Label2.Name = "Label2"
                Me.Label2.Size = New System.Drawing.Size(50, 15)
                Me.Label2.TabIndex = 77
                Me.Label2.Text = "seconds"
                '
                'btnLaunchPutty
                '
                Me.btnLaunchPutty.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnLaunchPutty.Image = Global.mRemote.My.Resources.Resources.PuttyConfig
                Me.btnLaunchPutty.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
                Me.btnLaunchPutty.Location = New System.Drawing.Point(297, 153)
                Me.btnLaunchPutty.Name = "btnLaunchPutty"
                Me.btnLaunchPutty.Size = New System.Drawing.Size(110, 23)
                Me.btnLaunchPutty.TabIndex = 70
                Me.btnLaunchPutty.Text = "Launch PuTTY"
                Me.btnLaunchPutty.TextAlign = System.Drawing.ContentAlignment.MiddleRight
                Me.btnLaunchPutty.UseVisualStyleBackColor = True
                '
                'numUVNCSCPort
                '
                Me.numUVNCSCPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.numUVNCSCPort.Location = New System.Drawing.Point(225, 277)
                Me.numUVNCSCPort.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
                Me.numUVNCSCPort.Name = "numUVNCSCPort"
                Me.numUVNCSCPort.Size = New System.Drawing.Size(72, 23)
                Me.numUVNCSCPort.TabIndex = 130
                Me.numUVNCSCPort.Value = New Decimal(New Integer() {5500, 0, 0, 0})
                '
                'lblConfigurePuttySessions
                '
                Me.lblConfigurePuttySessions.AutoSize = True
                Me.lblConfigurePuttySessions.Location = New System.Drawing.Point(9, 157)
                Me.lblConfigurePuttySessions.Name = "lblConfigurePuttySessions"
                Me.lblConfigurePuttySessions.Size = New System.Drawing.Size(250, 15)
                Me.lblConfigurePuttySessions.TabIndex = 60
                Me.lblConfigurePuttySessions.Text = "To configure PuTTY sessions click this button:"
                '
                'numPuttyWaitTime
                '
                Me.numPuttyWaitTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.numPuttyWaitTime.Location = New System.Drawing.Point(379, 185)
                Me.numPuttyWaitTime.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
                Me.numPuttyWaitTime.Name = "numPuttyWaitTime"
                Me.numPuttyWaitTime.Size = New System.Drawing.Size(49, 23)
                Me.numPuttyWaitTime.TabIndex = 76
                Me.numPuttyWaitTime.Value = New Decimal(New Integer() {5, 0, 0, 0})
                '
                'chkAutomaticReconnect
                '
                Me.chkAutomaticReconnect.AutoSize = True
                Me.chkAutomaticReconnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkAutomaticReconnect.Location = New System.Drawing.Point(9, 72)
                Me.chkAutomaticReconnect.Name = "chkAutomaticReconnect"
                Me.chkAutomaticReconnect.Size = New System.Drawing.Size(447, 19)
                Me.chkAutomaticReconnect.TabIndex = 25
                Me.chkAutomaticReconnect.Text = "Automatically try to reconnect when disconnected from server (RDP && ICA only)"
                Me.chkAutomaticReconnect.UseVisualStyleBackColor = True
                '
                'Label1
                '
                Me.Label1.AutoSize = True
                Me.Label1.Location = New System.Drawing.Point(9, 188)
                Me.Label1.Name = "Label1"
                Me.Label1.Size = New System.Drawing.Size(154, 15)
                Me.Label1.TabIndex = 75
                Me.Label1.Text = "Maximum PuTTY wait time:"
                '
                'btnOK
                '
                Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnOK.Location = New System.Drawing.Point(584, 496)
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
                Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnCancel.Location = New System.Drawing.Point(665, 496)
                Me.btnCancel.Name = "btnCancel"
                Me.btnCancel.Size = New System.Drawing.Size(75, 23)
                Me.btnCancel.TabIndex = 11000
                Me.btnCancel.Text = "&Cancel"
                Me.btnCancel.UseVisualStyleBackColor = True
                '
                'Options
                '
                Me.CancelButton = Me.btnCancel
                Me.ClientSize = New System.Drawing.Size(742, 523)
                Me.Controls.Add(Me.TabController)
                Me.Controls.Add(Me.btnCancel)
                Me.Controls.Add(Me.btnOK)
                Me.Icon = Global.mRemote.My.Resources.Resources.Options_Icon
                Me.Name = "Options"
                Me.TabText = "Options"
                Me.Text = "Options"
                Me.TabController.ResumeLayout(False)
                Me.tabUpdates.ResumeLayout(False)
                Me.pnlUpdates.ResumeLayout(False)
                Me.pnlUpdateCheck.ResumeLayout(False)
                Me.pnlUpdateCheck.PerformLayout()
                Me.pnlProxy.ResumeLayout(False)
                Me.pnlProxy.PerformLayout()
                Me.pnlProxyBasic.ResumeLayout(False)
                Me.pnlProxyBasic.PerformLayout()
                CType(Me.numProxyPort, System.ComponentModel.ISupportInitialize).EndInit()
                Me.pnlProxyAuthentication.ResumeLayout(False)
                Me.pnlProxyAuthentication.PerformLayout()
                Me.tabStartupExit.ResumeLayout(False)
                Me.pnlStartup.ResumeLayout(False)
                Me.pnlStartup.PerformLayout()
                Me.tabAppearance.ResumeLayout(False)
                Me.pnlAppearance.ResumeLayout(False)
                Me.pnlAppearance.PerformLayout()
                Me.tabTabs.ResumeLayout(False)
                Me.pnlTabsAndPanels.ResumeLayout(False)
                Me.pnlTabsAndPanels.PerformLayout()
                Me.tabConnections.ResumeLayout(False)
                Me.pnlConnections.ResumeLayout(False)
                Me.pnlConnections.PerformLayout()
                Me.pnlDefaultCredentials.ResumeLayout(False)
                Me.pnlDefaultCredentials.PerformLayout()
                Me.pnlAutoSave.ResumeLayout(False)
                Me.pnlAutoSave.PerformLayout()
                CType(Me.numAutoSave, System.ComponentModel.ISupportInitialize).EndInit()
                Me.grpExperimental.ResumeLayout(False)
                Me.grpExperimental.PerformLayout()
                Me.tabAdvanced.ResumeLayout(False)
                Me.pnlAdvanced.ResumeLayout(False)
                Me.pnlAdvanced.PerformLayout()
                CType(Me.numUVNCSCPort, System.ComponentModel.ISupportInitialize).EndInit()
                CType(Me.numPuttyWaitTime, System.ComponentModel.ISupportInitialize).EndInit()
                Me.ResumeLayout(False)

            End Sub
#End Region

#Region "Private Methods"
            Private Sub LoadOptions()
                Try
                    Me.chkSaveConsOnExit.Checked = My.Settings.SaveConsOnExit
                    Me.chkConfirmExit.Checked = My.Settings.ConfirmExit
                    Me.chkReconnectOnStart.Checked = My.Settings.OpenConsFromLastSession
                    Me.chkProperInstallationOfComponentsAtStartup.Checked = My.Settings.StartupComponentsCheck

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
                    Me.numAutoSave.Value = My.Settings.AutoSaveEveryMinutes

                    Me.chkUseSQLServer.Checked = My.Settings.UseSQLServer
                    Me.txtSQLServer.Text = My.Settings.SQLHost
                    Me.txtSQLUsername.Text = My.Settings.SQLUser
                    Me.txtSQLPassword.Text = mRemote.Security.Crypt.Decrypt(My.Settings.SQLPass, App.Info.General.EncryptionKey)

                    Select Case My.Settings.EmptyCredentials
                        Case "noinfo"
                            Me.radCredentialsNoInfo.Checked = True
                        Case "windows"
                            Me.radCredentialsWindows.Checked = True
                        Case "custom"
                            Me.radCredentialsCustom.Checked = True
                    End Select

                    Me.txtCredentialsUsername.Text = My.Settings.DefaultUsername
                    Me.txtCredentialsPassword.Text = mRemote.Security.Crypt.Decrypt(My.Settings.DefaultPassword, App.Info.General.EncryptionKey)
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
                    My.Settings.ConfirmExit = Me.chkConfirmExit.Checked
                    My.Settings.OpenConsFromLastSession = Me.chkReconnectOnStart.Checked
                    My.Settings.StartupComponentsCheck = Me.chkProperInstallationOfComponentsAtStartup.Checked

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
                    My.Settings.AutoSaveEveryMinutes = Me.numAutoSave.Value

                    If My.Settings.AutoSaveEveryMinutes > 0 Then
                        frmMain.tmrAutoSave.Interval = My.Settings.AutoSaveEveryMinutes * 60000
                        frmMain.tmrAutoSave.Enabled = True
                    Else
                        frmMain.tmrAutoSave.Enabled = False
                    End If

                    My.Settings.UseSQLServer = Me.chkUseSQLServer.Checked
                    My.Settings.SQLHost = Me.txtSQLServer.Text
                    My.Settings.SQLUser = Me.txtSQLUsername.Text
                    My.Settings.SQLPass = mRemote.Security.Crypt.Encrypt(Me.txtSQLPassword.Text, App.Info.General.EncryptionKey)

                    If Me.radCredentialsNoInfo.Checked Then
                        My.Settings.EmptyCredentials = "noinfo"
                    ElseIf Me.radCredentialsWindows.Checked Then
                        My.Settings.EmptyCredentials = "windows"
                    ElseIf Me.radCredentialsCustom.Checked Then
                        My.Settings.EmptyCredentials = "custom"
                    End If

                    My.Settings.DefaultUsername = Me.txtCredentialsUsername.Text
                    My.Settings.DefaultPassword = mRemote.Security.Crypt.Encrypt(Me.txtCredentialsPassword.Text, App.Info.General.EncryptionKey)
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
                        mRemote.Connection.Protocol.PuttyBase.PuttyPath = My.Settings.CustomPuttyPath
                    Else
                        mRemote.Connection.Protocol.PuttyBase.PuttyPath = My.Application.Info.DirectoryPath & "\putty.exe"
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

#Region "Public Methods"
            Public Sub New(ByVal Panel As DockContent)
                Me.WindowType = Type.Options
                Me.DockPnl = Panel
                Me.InitializeComponent()
                App.Runtime.FontOverride(Me)
            End Sub

            Public Sub ShowUpdatesTab()
                TabController.SelectedTab = tabUpdates
            End Sub
#End Region

#Region "Form Stuff"
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
                Me.lblSQLUsername.Enabled = chkUseSQLServer.Checked
                Me.lblSQLPassword.Enabled = chkUseSQLServer.Checked
                Me.lblSQLInfo.Enabled = chkUseSQLServer.Checked
                Me.txtSQLServer.Enabled = chkUseSQLServer.Checked
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

            Private Sub Options_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
                ApplyLanguage()

                If App.Editions.Spanlink.Enabled Then
                    ApplySpanlinkEdition()
                End If

                Me.TabController.SelectedIndex = 1
                Me.TabController.SelectedIndex = 0
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
                grpExperimental.Enabled = False
                grpExperimental.Visible = False
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
                tabAdvanced.Title = My.Resources.strTabAdvanced
                btnTestProxy.Text = My.Resources.strButtonTestProxy
                Label2.Text = My.Resources.strLabelSeconds
                Label1.Text = My.Resources.strLabelPuttyTimeout
                chkAutomaticReconnect.Text = My.Resources.strCheckboxAutomaticReconnect
                lblProxyAddress.Text = My.Resources.strLabelAddress
                lblProxyPort.Text = Language.Base.Props_Port & ":"
                lblProxyUsername.Text = Language.Base.Props_Username & ":"
                lblProxyPassword.Text = Language.Base.Props_Password & ":"
                chkUseProxyAuthentication.Text = My.Resources.strCheckboxProxyAuthentication
                chkUseProxyForAutomaticUpdates.Text = My.Resources.strCheckboxUpdateUseProxy
                lblConfigurePuttySessions.Text = My.Resources.strLabelPuttySessionsConfig
                btnLaunchPutty.Text = My.Resources.strButtonLaunchPutty
                btnBrowseCustomPuttyPath.Text = My.Resources.strButtonBrowse
                chkUseCustomPuttyPath.Text = My.Resources.strCheckboxPuttyPath
                chkAutomaticallyGetSessionInfo.Text = Language.Base.AutomaticallyGetSessionInfo
                chkWriteLogFile.Text = Language.Base.WriteLogFile
                tabStartupExit.Title = Language.Base.StartupExit
                chkSingleInstance.Text = Language.Base.AllowOnlySingleInstance
                chkReconnectOnStart.Text = Language.Base.ReconnectAtStartup
                chkCheckForUpdatesOnStartup.Text = Language.Base.CheckForUpdatesOnStartup
                chkConfirmExit.Text = Language.Base.ConfirmExit
                chkSaveConsOnExit.Text = Language.Base.SaveConsOnExit
                tabAppearance.Title = Language.Base.Props_Appearance
                chkMinimizeToSystemTray.Text = Language.Base.MinimizeToSysTray
                chkShowFullConnectionsFilePathInTitle.Text = Language.Base.ShowFullConsFilePath
                chkShowSystemTrayIcon.Text = Language.Base.AlwaysShowSysTrayIcon
                chkShowDescriptionTooltipsInTree.Text = Language.Base.ShowDescriptionTooltips
                tabTabs.Title = Language.Base.TabsAndPanels
                chkShowProtocolOnTabs.Text = Language.Base.ShowProtocolOnTabs
                chkShowLogonInfoOnTabs.Text = Language.Base.ShowLogonInfoOnTabs
                chkOpenNewTabRightOfSelected.Text = Language.Base.OpenNewTabRight
                chkAlwaysShowPanelSelectionDlg.Text = Language.Base.AlwaysShowPanelSelection
                chkDoubleClickClosesTab.Text = Language.Base.DoubleClickTabClosesIt
                tabConnections.Title = My.Resources.strConnections
                chkHostnameLikeDisplayName.Text = Language.Base.SetHostnameLikeDisplayName
                grpExperimental.Text = Language.Base.Experimental.ToUpper
                chkUseSQLServer.Text = Language.Base.UseSQLServer
                lblSQLInfo.Text = Language.Base.SQLInfo
                lblSQLUsername.Text = Language.Base.Props_Username & ":"
                lblSQLServer.Text = Language.Base.SQLServer & ":"
                lblSQLPassword.Text = Language.Base.Props_Password & ":"
                lblAutoSave2.Text = Language.Base.AutoSaveMins
                lblAutoSave1.Text = Language.Base.AutoSaveEvery
                lblCredentialsDomain.Text = Language.Base.Props_Domain & ":"
                lblCredentialsPassword.Text = Language.Base.Props_Password & ":"
                lblCredentialsUsername.Text = Language.Base.Props_Username & ":"
                radCredentialsCustom.Text = Language.Base.TheFollowing & ":"
                radCredentialsWindows.Text = Language.Base.MyCurrentWindowsCreds
                radCredentialsNoInfo.Text = Language.Base.NoInformation
                lblDefaultCredentials.Text = Language.Base.EmptyUsernamePasswordDomainFields
                chkSingleClickOnOpenedConnectionSwitchesToIt.Text = Language.Base.SingleClickOnOpenConnectionSwitchesToIt
                chkSingleClickOnConnectionOpensIt.Text = Language.Base.SingleClickOnConnectionOpensIt
                lblSwitchToErrorsAndInfos.Text = Language.Base.SwitchToErrorsAndInfos & ":"
                chkMCErrors.Text = Language.Base.Errors
                chkMCWarnings.Text = Language.Base.Warnings
                chkMCInformation.Text = Language.Base.Informations
                chkUseOnlyErrorsAndInfosPanel.Text = Language.Base.UseOnlyErrorsAndInfosPanel
                btnOK.Text = My.Resources.strButtonOK
                btnCancel.Text = My.Resources.strButtonCancel
                TabText = My.Resources.strMenuOptions
                Text = My.Resources.strMenuOptions
                Label3.Text = Language.Base.UltraVNCSCListeningPort & ":"
                chkProperInstallationOfComponentsAtStartup.Text = Language.Base.CheckProperInstallationOfComponentsAtStartup
                lblXulRunnerPath.Text = Language.Base.XULrunnerPath & ":"
                btnBrowseXulRunnerPath.Text = My.Resources.strButtonBrowse
                chkEncryptCompleteFile.Text = Language.Base.EncryptCompleteConnectionFile
            End Sub

            Public Shadows Sub Show(ByVal DockPanel As DockPanel)
                Windows.optionsForm.LoadOptions()

                MyBase.Show(DockPanel, DockState.Document)
            End Sub

            Private Sub btnLaunchPutty_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLaunchPutty.Click
                mRemote.Connection.Protocol.PuttyBase.StartPutty()
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
                    MsgBox(Language.Base.ProxyTestSucceeded, MsgBoxStyle.Information)
                Else
                    MsgBox(Language.Base.ProxyTestFailed, MsgBoxStyle.Exclamation)
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

            Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
                Me.Close()
            End Sub

            Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
                Me.SaveOptions()
                Me.Close()
            End Sub
#End Region
        End Class
    End Namespace
End Namespace