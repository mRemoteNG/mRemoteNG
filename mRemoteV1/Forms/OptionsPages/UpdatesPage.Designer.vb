Namespace Forms.OptionsPages
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class UpdatesPage
        Inherits OptionsPage

        'UserControl overrides dispose to clean up the component list.
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UpdatesPage))
            Me.lblUpdatesExplanation = New System.Windows.Forms.Label()
            Me.pnlUpdateCheck = New System.Windows.Forms.Panel()
            Me.btnUpdateCheckNow = New System.Windows.Forms.Button()
            Me.chkCheckForUpdatesOnStartup = New System.Windows.Forms.CheckBox()
            Me.cboUpdateCheckFrequency = New System.Windows.Forms.ComboBox()
            Me.pnlProxy = New System.Windows.Forms.Panel()
            Me.pnlProxyBasic = New System.Windows.Forms.Panel()
            Me.lblProxyAddress = New System.Windows.Forms.Label()
            Me.txtProxyAddress = New System.Windows.Forms.TextBox()
            Me.lblProxyPort = New System.Windows.Forms.Label()
            Me.numProxyPort = New System.Windows.Forms.NumericUpDown()
            Me.chkUseProxyForAutomaticUpdates = New System.Windows.Forms.CheckBox()
            Me.chkUseProxyAuthentication = New System.Windows.Forms.CheckBox()
            Me.pnlProxyAuthentication = New System.Windows.Forms.Panel()
            Me.lblProxyUsername = New System.Windows.Forms.Label()
            Me.txtProxyUsername = New System.Windows.Forms.TextBox()
            Me.lblProxyPassword = New System.Windows.Forms.Label()
            Me.txtProxyPassword = New System.Windows.Forms.TextBox()
            Me.btnTestProxy = New System.Windows.Forms.Button()
            Me.pnlUpdateCheck.SuspendLayout()
            Me.pnlProxy.SuspendLayout()
            Me.pnlProxyBasic.SuspendLayout()
            CType(Me.numProxyPort, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlProxyAuthentication.SuspendLayout()
            Me.SuspendLayout()
            '
            'lblUpdatesExplanation
            '
            Me.lblUpdatesExplanation.Location = New System.Drawing.Point(3, 0)
            Me.lblUpdatesExplanation.Name = "lblUpdatesExplanation"
            Me.lblUpdatesExplanation.Size = New System.Drawing.Size(536, 40)
            Me.lblUpdatesExplanation.TabIndex = 3
            Me.lblUpdatesExplanation.Text = "mRemote3G can periodically connect to the mRemote3G website to check for updates " &
        "and product announcements."
            '
            'pnlUpdateCheck
            '
            Me.pnlUpdateCheck.Controls.Add(Me.btnUpdateCheckNow)
            Me.pnlUpdateCheck.Controls.Add(Me.chkCheckForUpdatesOnStartup)
            Me.pnlUpdateCheck.Controls.Add(Me.cboUpdateCheckFrequency)
            Me.pnlUpdateCheck.Location = New System.Drawing.Point(0, 48)
            Me.pnlUpdateCheck.Name = "pnlUpdateCheck"
            Me.pnlUpdateCheck.Size = New System.Drawing.Size(610, 120)
            Me.pnlUpdateCheck.TabIndex = 4
            '
            'btnUpdateCheckNow
            '
            Me.btnUpdateCheckNow.Location = New System.Drawing.Point(3, 80)
            Me.btnUpdateCheckNow.Name = "btnUpdateCheckNow"
            Me.btnUpdateCheckNow.Size = New System.Drawing.Size(120, 32)
            Me.btnUpdateCheckNow.TabIndex = 2
            Me.btnUpdateCheckNow.Text = "Check Now"
            Me.btnUpdateCheckNow.UseVisualStyleBackColor = True
            '
            'chkCheckForUpdatesOnStartup
            '
            Me.chkCheckForUpdatesOnStartup.AutoSize = True
            Me.chkCheckForUpdatesOnStartup.Location = New System.Drawing.Point(3, 8)
            Me.chkCheckForUpdatesOnStartup.Name = "chkCheckForUpdatesOnStartup"
            Me.chkCheckForUpdatesOnStartup.Size = New System.Drawing.Size(213, 17)
            Me.chkCheckForUpdatesOnStartup.TabIndex = 0
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
            Me.cboUpdateCheckFrequency.TabIndex = 1
            '
            'pnlProxy
            '
            Me.pnlProxy.Controls.Add(Me.pnlProxyBasic)
            Me.pnlProxy.Controls.Add(Me.chkUseProxyForAutomaticUpdates)
            Me.pnlProxy.Controls.Add(Me.chkUseProxyAuthentication)
            Me.pnlProxy.Controls.Add(Me.pnlProxyAuthentication)
            Me.pnlProxy.Controls.Add(Me.btnTestProxy)
            Me.pnlProxy.Location = New System.Drawing.Point(0, 200)
            Me.pnlProxy.Name = "pnlProxy"
            Me.pnlProxy.Size = New System.Drawing.Size(610, 224)
            Me.pnlProxy.TabIndex = 5
            '
            'pnlProxyBasic
            '
            Me.pnlProxyBasic.Controls.Add(Me.lblProxyAddress)
            Me.pnlProxyBasic.Controls.Add(Me.txtProxyAddress)
            Me.pnlProxyBasic.Controls.Add(Me.lblProxyPort)
            Me.pnlProxyBasic.Controls.Add(Me.numProxyPort)
            Me.pnlProxyBasic.Enabled = False
            Me.pnlProxyBasic.Location = New System.Drawing.Point(3, 32)
            Me.pnlProxyBasic.Name = "pnlProxyBasic"
            Me.pnlProxyBasic.Size = New System.Drawing.Size(604, 40)
            Me.pnlProxyBasic.TabIndex = 1
            '
            'lblProxyAddress
            '
            Me.lblProxyAddress.Location = New System.Drawing.Point(8, 4)
            Me.lblProxyAddress.Name = "lblProxyAddress"
            Me.lblProxyAddress.Size = New System.Drawing.Size(96, 24)
            Me.lblProxyAddress.TabIndex = 0
            Me.lblProxyAddress.Text = "Address:"
            Me.lblProxyAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'txtProxyAddress
            '
            Me.txtProxyAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.txtProxyAddress.Location = New System.Drawing.Point(104, 8)
            Me.txtProxyAddress.Name = "txtProxyAddress"
            Me.txtProxyAddress.Size = New System.Drawing.Size(240, 20)
            Me.txtProxyAddress.TabIndex = 1
            '
            'lblProxyPort
            '
            Me.lblProxyPort.Location = New System.Drawing.Point(350, 5)
            Me.lblProxyPort.Name = "lblProxyPort"
            Me.lblProxyPort.Size = New System.Drawing.Size(64, 23)
            Me.lblProxyPort.TabIndex = 2
            Me.lblProxyPort.Text = "Port:"
            Me.lblProxyPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'numProxyPort
            '
            Me.numProxyPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.numProxyPort.Location = New System.Drawing.Point(420, 8)
            Me.numProxyPort.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
            Me.numProxyPort.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.numProxyPort.Name = "numProxyPort"
            Me.numProxyPort.Size = New System.Drawing.Size(64, 20)
            Me.numProxyPort.TabIndex = 3
            Me.numProxyPort.Value = New Decimal(New Integer() {80, 0, 0, 0})
            '
            'chkUseProxyForAutomaticUpdates
            '
            Me.chkUseProxyForAutomaticUpdates.AutoSize = True
            Me.chkUseProxyForAutomaticUpdates.Location = New System.Drawing.Point(3, 8)
            Me.chkUseProxyForAutomaticUpdates.Name = "chkUseProxyForAutomaticUpdates"
            Me.chkUseProxyForAutomaticUpdates.Size = New System.Drawing.Size(168, 17)
            Me.chkUseProxyForAutomaticUpdates.TabIndex = 0
            Me.chkUseProxyForAutomaticUpdates.Text = "Use a proxy server to connect"
            Me.chkUseProxyForAutomaticUpdates.UseVisualStyleBackColor = True
            '
            'chkUseProxyAuthentication
            '
            Me.chkUseProxyAuthentication.AutoSize = True
            Me.chkUseProxyAuthentication.Enabled = False
            Me.chkUseProxyAuthentication.Location = New System.Drawing.Point(27, 80)
            Me.chkUseProxyAuthentication.Name = "chkUseProxyAuthentication"
            Me.chkUseProxyAuthentication.Size = New System.Drawing.Size(216, 17)
            Me.chkUseProxyAuthentication.TabIndex = 2
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
            Me.pnlProxyAuthentication.Location = New System.Drawing.Point(3, 104)
            Me.pnlProxyAuthentication.Name = "pnlProxyAuthentication"
            Me.pnlProxyAuthentication.Size = New System.Drawing.Size(604, 72)
            Me.pnlProxyAuthentication.TabIndex = 3
            '
            'lblProxyUsername
            '
            Me.lblProxyUsername.Location = New System.Drawing.Point(8, 4)
            Me.lblProxyUsername.Name = "lblProxyUsername"
            Me.lblProxyUsername.Size = New System.Drawing.Size(96, 24)
            Me.lblProxyUsername.TabIndex = 0
            Me.lblProxyUsername.Text = "Username:"
            Me.lblProxyUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'txtProxyUsername
            '
            Me.txtProxyUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.txtProxyUsername.Location = New System.Drawing.Point(104, 8)
            Me.txtProxyUsername.Name = "txtProxyUsername"
            Me.txtProxyUsername.Size = New System.Drawing.Size(240, 20)
            Me.txtProxyUsername.TabIndex = 1
            '
            'lblProxyPassword
            '
            Me.lblProxyPassword.Location = New System.Drawing.Point(8, 36)
            Me.lblProxyPassword.Name = "lblProxyPassword"
            Me.lblProxyPassword.Size = New System.Drawing.Size(96, 24)
            Me.lblProxyPassword.TabIndex = 2
            Me.lblProxyPassword.Text = "Password:"
            Me.lblProxyPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'txtProxyPassword
            '
            Me.txtProxyPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.txtProxyPassword.Location = New System.Drawing.Point(104, 40)
            Me.txtProxyPassword.Name = "txtProxyPassword"
            Me.txtProxyPassword.Size = New System.Drawing.Size(240, 20)
            Me.txtProxyPassword.TabIndex = 3
            Me.txtProxyPassword.UseSystemPasswordChar = True
            '
            'btnTestProxy
            '
            Me.btnTestProxy.Location = New System.Drawing.Point(3, 184)
            Me.btnTestProxy.Name = "btnTestProxy"
            Me.btnTestProxy.Size = New System.Drawing.Size(120, 32)
            Me.btnTestProxy.TabIndex = 4
            Me.btnTestProxy.Text = "Test Proxy"
            Me.btnTestProxy.UseVisualStyleBackColor = True
            '
            'UpdatesPage
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.lblUpdatesExplanation)
            Me.Controls.Add(Me.pnlUpdateCheck)
            Me.Controls.Add(Me.pnlProxy)
            Me.Name = "UpdatesPage"
            Me.PageIcon = CType(resources.GetObject("$this.PageIcon"), System.Drawing.Icon)
            Me.Size = New System.Drawing.Size(610, 489)
            Me.pnlUpdateCheck.ResumeLayout(False)
            Me.pnlUpdateCheck.PerformLayout()
            Me.pnlProxy.ResumeLayout(False)
            Me.pnlProxy.PerformLayout()
            Me.pnlProxyBasic.ResumeLayout(False)
            Me.pnlProxyBasic.PerformLayout()
            CType(Me.numProxyPort, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlProxyAuthentication.ResumeLayout(False)
            Me.pnlProxyAuthentication.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
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

    End Class
End Namespace