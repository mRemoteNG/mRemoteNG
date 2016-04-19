Namespace Forms.OptionsPages
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ConnectionsPage
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ConnectionsPage))
            Me.pnlRdpReconnectionCount = New System.Windows.Forms.Panel()
            Me.lblRdpReconnectionCount = New System.Windows.Forms.Label()
            Me.numRdpReconnectionCount = New System.Windows.Forms.NumericUpDown()
            Me.chkSingleClickOnConnectionOpensIt = New System.Windows.Forms.CheckBox()
            Me.chkHostnameLikeDisplayName = New System.Windows.Forms.CheckBox()
            Me.pnlDefaultCredentials = New System.Windows.Forms.Panel()
            Me.radCredentialsCustom = New System.Windows.Forms.RadioButton()
            Me.lblDefaultCredentials = New System.Windows.Forms.Label()
            Me.radCredentialsNoInfo = New System.Windows.Forms.RadioButton()
            Me.radCredentialsWindows = New System.Windows.Forms.RadioButton()
            Me.txtCredentialsDomain = New System.Windows.Forms.TextBox()
            Me.lblCredentialsUsername = New System.Windows.Forms.Label()
            Me.txtCredentialsPassword = New System.Windows.Forms.TextBox()
            Me.lblCredentialsPassword = New System.Windows.Forms.Label()
            Me.txtCredentialsUsername = New System.Windows.Forms.TextBox()
            Me.lblCredentialsDomain = New System.Windows.Forms.Label()
            Me.chkSingleClickOnOpenedConnectionSwitchesToIt = New System.Windows.Forms.CheckBox()
            Me.pnlAutoSave = New System.Windows.Forms.Panel()
            Me.lblAutoSave1 = New System.Windows.Forms.Label()
            Me.numAutoSave = New System.Windows.Forms.NumericUpDown()
            Me.lblAutoSave2 = New System.Windows.Forms.Label()
            Me.pnlConfirmCloseConnection = New System.Windows.Forms.Panel()
            Me.lblClosingConnections = New System.Windows.Forms.Label()
            Me.radCloseWarnAll = New System.Windows.Forms.RadioButton()
            Me.radCloseWarnMultiple = New System.Windows.Forms.RadioButton()
            Me.radCloseWarnExit = New System.Windows.Forms.RadioButton()
            Me.radCloseWarnNever = New System.Windows.Forms.RadioButton()
            Me.pnlRdpReconnectionCount.SuspendLayout()
            CType(Me.numRdpReconnectionCount, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlDefaultCredentials.SuspendLayout()
            Me.pnlAutoSave.SuspendLayout()
            CType(Me.numAutoSave, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlConfirmCloseConnection.SuspendLayout()
            Me.SuspendLayout()
            '
            'pnlRdpReconnectionCount
            '
            Me.pnlRdpReconnectionCount.Controls.Add(Me.lblRdpReconnectionCount)
            Me.pnlRdpReconnectionCount.Controls.Add(Me.numRdpReconnectionCount)
            Me.pnlRdpReconnectionCount.Location = New System.Drawing.Point(3, 69)
            Me.pnlRdpReconnectionCount.Name = "pnlRdpReconnectionCount"
            Me.pnlRdpReconnectionCount.Size = New System.Drawing.Size(596, 29)
            Me.pnlRdpReconnectionCount.TabIndex = 10
            '
            'lblRdpReconnectionCount
            '
            Me.lblRdpReconnectionCount.Location = New System.Drawing.Point(6, 9)
            Me.lblRdpReconnectionCount.Name = "lblRdpReconnectionCount"
            Me.lblRdpReconnectionCount.Size = New System.Drawing.Size(288, 13)
            Me.lblRdpReconnectionCount.TabIndex = 0
            Me.lblRdpReconnectionCount.Text = "RDP Reconnection Count"
            Me.lblRdpReconnectionCount.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'numRdpReconnectionCount
            '
            Me.numRdpReconnectionCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.numRdpReconnectionCount.Location = New System.Drawing.Point(300, 7)
            Me.numRdpReconnectionCount.Maximum = New Decimal(New Integer() {20, 0, 0, 0})
            Me.numRdpReconnectionCount.Name = "numRdpReconnectionCount"
            Me.numRdpReconnectionCount.Size = New System.Drawing.Size(53, 20)
            Me.numRdpReconnectionCount.TabIndex = 1
            Me.numRdpReconnectionCount.Value = New Decimal(New Integer() {5, 0, 0, 0})
            '
            'chkSingleClickOnConnectionOpensIt
            '
            Me.chkSingleClickOnConnectionOpensIt.AutoSize = True
            Me.chkSingleClickOnConnectionOpensIt.Location = New System.Drawing.Point(3, 0)
            Me.chkSingleClickOnConnectionOpensIt.Name = "chkSingleClickOnConnectionOpensIt"
            Me.chkSingleClickOnConnectionOpensIt.Size = New System.Drawing.Size(191, 17)
            Me.chkSingleClickOnConnectionOpensIt.TabIndex = 7
            Me.chkSingleClickOnConnectionOpensIt.Text = "Single click on connection opens it"
            Me.chkSingleClickOnConnectionOpensIt.UseVisualStyleBackColor = True
            '
            'chkHostnameLikeDisplayName
            '
            Me.chkHostnameLikeDisplayName.AutoSize = True
            Me.chkHostnameLikeDisplayName.Location = New System.Drawing.Point(3, 46)
            Me.chkHostnameLikeDisplayName.Name = "chkHostnameLikeDisplayName"
            Me.chkHostnameLikeDisplayName.Size = New System.Drawing.Size(328, 17)
            Me.chkHostnameLikeDisplayName.TabIndex = 9
            Me.chkHostnameLikeDisplayName.Text = "Set hostname like display name when creating new connections"
            Me.chkHostnameLikeDisplayName.UseVisualStyleBackColor = True
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
            Me.pnlDefaultCredentials.Location = New System.Drawing.Point(3, 139)
            Me.pnlDefaultCredentials.Name = "pnlDefaultCredentials"
            Me.pnlDefaultCredentials.Size = New System.Drawing.Size(596, 175)
            Me.pnlDefaultCredentials.TabIndex = 12
            '
            'radCredentialsCustom
            '
            Me.radCredentialsCustom.AutoSize = True
            Me.radCredentialsCustom.Location = New System.Drawing.Point(16, 69)
            Me.radCredentialsCustom.Name = "radCredentialsCustom"
            Me.radCredentialsCustom.Size = New System.Drawing.Size(87, 17)
            Me.radCredentialsCustom.TabIndex = 3
            Me.radCredentialsCustom.Text = "the following:"
            Me.radCredentialsCustom.UseVisualStyleBackColor = True
            '
            'lblDefaultCredentials
            '
            Me.lblDefaultCredentials.AutoSize = True
            Me.lblDefaultCredentials.Location = New System.Drawing.Point(3, 9)
            Me.lblDefaultCredentials.Name = "lblDefaultCredentials"
            Me.lblDefaultCredentials.Size = New System.Drawing.Size(257, 13)
            Me.lblDefaultCredentials.TabIndex = 0
            Me.lblDefaultCredentials.Text = "For empty Username, Password or Domain fields use:"
            '
            'radCredentialsNoInfo
            '
            Me.radCredentialsNoInfo.AutoSize = True
            Me.radCredentialsNoInfo.Checked = True
            Me.radCredentialsNoInfo.Location = New System.Drawing.Point(16, 31)
            Me.radCredentialsNoInfo.Name = "radCredentialsNoInfo"
            Me.radCredentialsNoInfo.Size = New System.Drawing.Size(91, 17)
            Me.radCredentialsNoInfo.TabIndex = 1
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
            Me.radCredentialsWindows.TabIndex = 2
            Me.radCredentialsWindows.Text = "my current credentials (windows logon info)"
            Me.radCredentialsWindows.UseVisualStyleBackColor = True
            '
            'txtCredentialsDomain
            '
            Me.txtCredentialsDomain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.txtCredentialsDomain.Enabled = False
            Me.txtCredentialsDomain.Location = New System.Drawing.Point(140, 147)
            Me.txtCredentialsDomain.Name = "txtCredentialsDomain"
            Me.txtCredentialsDomain.Size = New System.Drawing.Size(150, 20)
            Me.txtCredentialsDomain.TabIndex = 9
            '
            'lblCredentialsUsername
            '
            Me.lblCredentialsUsername.Enabled = False
            Me.lblCredentialsUsername.Location = New System.Drawing.Point(37, 95)
            Me.lblCredentialsUsername.Name = "lblCredentialsUsername"
            Me.lblCredentialsUsername.Size = New System.Drawing.Size(97, 13)
            Me.lblCredentialsUsername.TabIndex = 4
            Me.lblCredentialsUsername.Text = "Username:"
            Me.lblCredentialsUsername.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'txtCredentialsPassword
            '
            Me.txtCredentialsPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.txtCredentialsPassword.Enabled = False
            Me.txtCredentialsPassword.Location = New System.Drawing.Point(140, 120)
            Me.txtCredentialsPassword.Name = "txtCredentialsPassword"
            Me.txtCredentialsPassword.Size = New System.Drawing.Size(150, 20)
            Me.txtCredentialsPassword.TabIndex = 7
            Me.txtCredentialsPassword.UseSystemPasswordChar = True
            '
            'lblCredentialsPassword
            '
            Me.lblCredentialsPassword.Enabled = False
            Me.lblCredentialsPassword.Location = New System.Drawing.Point(34, 123)
            Me.lblCredentialsPassword.Name = "lblCredentialsPassword"
            Me.lblCredentialsPassword.Size = New System.Drawing.Size(100, 13)
            Me.lblCredentialsPassword.TabIndex = 6
            Me.lblCredentialsPassword.Text = "Password:"
            Me.lblCredentialsPassword.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'txtCredentialsUsername
            '
            Me.txtCredentialsUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.txtCredentialsUsername.Enabled = False
            Me.txtCredentialsUsername.Location = New System.Drawing.Point(140, 93)
            Me.txtCredentialsUsername.Name = "txtCredentialsUsername"
            Me.txtCredentialsUsername.Size = New System.Drawing.Size(150, 20)
            Me.txtCredentialsUsername.TabIndex = 5
            '
            'lblCredentialsDomain
            '
            Me.lblCredentialsDomain.Enabled = False
            Me.lblCredentialsDomain.Location = New System.Drawing.Point(34, 150)
            Me.lblCredentialsDomain.Name = "lblCredentialsDomain"
            Me.lblCredentialsDomain.Size = New System.Drawing.Size(100, 13)
            Me.lblCredentialsDomain.TabIndex = 8
            Me.lblCredentialsDomain.Text = "Domain:"
            Me.lblCredentialsDomain.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'chkSingleClickOnOpenedConnectionSwitchesToIt
            '
            Me.chkSingleClickOnOpenedConnectionSwitchesToIt.AutoSize = True
            Me.chkSingleClickOnOpenedConnectionSwitchesToIt.Location = New System.Drawing.Point(3, 23)
            Me.chkSingleClickOnOpenedConnectionSwitchesToIt.Name = "chkSingleClickOnOpenedConnectionSwitchesToIt"
            Me.chkSingleClickOnOpenedConnectionSwitchesToIt.Size = New System.Drawing.Size(254, 17)
            Me.chkSingleClickOnOpenedConnectionSwitchesToIt.TabIndex = 8
            Me.chkSingleClickOnOpenedConnectionSwitchesToIt.Text = "Single click on opened connection switches to it"
            Me.chkSingleClickOnOpenedConnectionSwitchesToIt.UseVisualStyleBackColor = True
            '
            'pnlAutoSave
            '
            Me.pnlAutoSave.Controls.Add(Me.lblAutoSave1)
            Me.pnlAutoSave.Controls.Add(Me.numAutoSave)
            Me.pnlAutoSave.Controls.Add(Me.lblAutoSave2)
            Me.pnlAutoSave.Location = New System.Drawing.Point(3, 104)
            Me.pnlAutoSave.Name = "pnlAutoSave"
            Me.pnlAutoSave.Size = New System.Drawing.Size(596, 29)
            Me.pnlAutoSave.TabIndex = 11
            '
            'lblAutoSave1
            '
            Me.lblAutoSave1.Location = New System.Drawing.Point(6, 9)
            Me.lblAutoSave1.Name = "lblAutoSave1"
            Me.lblAutoSave1.Size = New System.Drawing.Size(288, 13)
            Me.lblAutoSave1.TabIndex = 0
            Me.lblAutoSave1.Text = "Auto Save every:"
            Me.lblAutoSave1.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'numAutoSave
            '
            Me.numAutoSave.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.numAutoSave.Location = New System.Drawing.Point(300, 7)
            Me.numAutoSave.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
            Me.numAutoSave.Name = "numAutoSave"
            Me.numAutoSave.Size = New System.Drawing.Size(53, 20)
            Me.numAutoSave.TabIndex = 1
            '
            'lblAutoSave2
            '
            Me.lblAutoSave2.AutoSize = True
            Me.lblAutoSave2.Location = New System.Drawing.Point(359, 9)
            Me.lblAutoSave2.Name = "lblAutoSave2"
            Me.lblAutoSave2.Size = New System.Drawing.Size(135, 13)
            Me.lblAutoSave2.TabIndex = 2
            Me.lblAutoSave2.Text = "Minutes (0 means disabled)"
            '
            'pnlConfirmCloseConnection
            '
            Me.pnlConfirmCloseConnection.Controls.Add(Me.lblClosingConnections)
            Me.pnlConfirmCloseConnection.Controls.Add(Me.radCloseWarnAll)
            Me.pnlConfirmCloseConnection.Controls.Add(Me.radCloseWarnMultiple)
            Me.pnlConfirmCloseConnection.Controls.Add(Me.radCloseWarnExit)
            Me.pnlConfirmCloseConnection.Controls.Add(Me.radCloseWarnNever)
            Me.pnlConfirmCloseConnection.Location = New System.Drawing.Point(3, 320)
            Me.pnlConfirmCloseConnection.Name = "pnlConfirmCloseConnection"
            Me.pnlConfirmCloseConnection.Size = New System.Drawing.Size(596, 137)
            Me.pnlConfirmCloseConnection.TabIndex = 13
            '
            'lblClosingConnections
            '
            Me.lblClosingConnections.AutoSize = True
            Me.lblClosingConnections.Location = New System.Drawing.Point(3, 9)
            Me.lblClosingConnections.Name = "lblClosingConnections"
            Me.lblClosingConnections.Size = New System.Drawing.Size(136, 13)
            Me.lblClosingConnections.TabIndex = 0
            Me.lblClosingConnections.Text = "When closing connections:"
            '
            'radCloseWarnAll
            '
            Me.radCloseWarnAll.AutoSize = True
            Me.radCloseWarnAll.Location = New System.Drawing.Point(16, 31)
            Me.radCloseWarnAll.Name = "radCloseWarnAll"
            Me.radCloseWarnAll.Size = New System.Drawing.Size(194, 17)
            Me.radCloseWarnAll.TabIndex = 1
            Me.radCloseWarnAll.TabStop = True
            Me.radCloseWarnAll.Text = "Warn me when closing connections"
            Me.radCloseWarnAll.UseVisualStyleBackColor = True
            '
            'radCloseWarnMultiple
            '
            Me.radCloseWarnMultiple.AutoSize = True
            Me.radCloseWarnMultiple.Location = New System.Drawing.Point(16, 54)
            Me.radCloseWarnMultiple.Name = "radCloseWarnMultiple"
            Me.radCloseWarnMultiple.Size = New System.Drawing.Size(254, 17)
            Me.radCloseWarnMultiple.TabIndex = 2
            Me.radCloseWarnMultiple.TabStop = True
            Me.radCloseWarnMultiple.Text = "Warn me only when closing multiple connections"
            Me.radCloseWarnMultiple.UseVisualStyleBackColor = True
            '
            'radCloseWarnExit
            '
            Me.radCloseWarnExit.AutoSize = True
            Me.radCloseWarnExit.Location = New System.Drawing.Point(16, 77)
            Me.radCloseWarnExit.Name = "radCloseWarnExit"
            Me.radCloseWarnExit.Size = New System.Drawing.Size(216, 17)
            Me.radCloseWarnExit.TabIndex = 3
            Me.radCloseWarnExit.TabStop = True
            Me.radCloseWarnExit.Text = "Warn me only when exiting mRemote3G"
            Me.radCloseWarnExit.UseVisualStyleBackColor = True
            '
            'radCloseWarnNever
            '
            Me.radCloseWarnNever.AutoSize = True
            Me.radCloseWarnNever.Location = New System.Drawing.Point(16, 100)
            Me.radCloseWarnNever.Name = "radCloseWarnNever"
            Me.radCloseWarnNever.Size = New System.Drawing.Size(226, 17)
            Me.radCloseWarnNever.TabIndex = 4
            Me.radCloseWarnNever.TabStop = True
            Me.radCloseWarnNever.Text = "Do not warn me when closing connections"
            Me.radCloseWarnNever.UseVisualStyleBackColor = True
            '
            'ConnectionsPage
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.pnlRdpReconnectionCount)
            Me.Controls.Add(Me.chkSingleClickOnConnectionOpensIt)
            Me.Controls.Add(Me.chkHostnameLikeDisplayName)
            Me.Controls.Add(Me.pnlDefaultCredentials)
            Me.Controls.Add(Me.chkSingleClickOnOpenedConnectionSwitchesToIt)
            Me.Controls.Add(Me.pnlAutoSave)
            Me.Controls.Add(Me.pnlConfirmCloseConnection)
            Me.Name = "ConnectionsPage"
            Me.PageIcon = CType(resources.GetObject("$this.PageIcon"), System.Drawing.Icon)
            Me.Size = New System.Drawing.Size(610, 489)
            Me.pnlRdpReconnectionCount.ResumeLayout(False)
            CType(Me.numRdpReconnectionCount, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlDefaultCredentials.ResumeLayout(False)
            Me.pnlDefaultCredentials.PerformLayout()
            Me.pnlAutoSave.ResumeLayout(False)
            Me.pnlAutoSave.PerformLayout()
            CType(Me.numAutoSave, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlConfirmCloseConnection.ResumeLayout(False)
            Me.pnlConfirmCloseConnection.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents pnlRdpReconnectionCount As System.Windows.Forms.Panel
        Friend WithEvents lblRdpReconnectionCount As System.Windows.Forms.Label
        Friend WithEvents numRdpReconnectionCount As System.Windows.Forms.NumericUpDown
        Friend WithEvents chkSingleClickOnConnectionOpensIt As System.Windows.Forms.CheckBox
        Friend WithEvents chkHostnameLikeDisplayName As System.Windows.Forms.CheckBox
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
        Friend WithEvents chkSingleClickOnOpenedConnectionSwitchesToIt As System.Windows.Forms.CheckBox
        Friend WithEvents pnlAutoSave As System.Windows.Forms.Panel
        Friend WithEvents lblAutoSave1 As System.Windows.Forms.Label
        Friend WithEvents numAutoSave As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblAutoSave2 As System.Windows.Forms.Label
        Friend WithEvents pnlConfirmCloseConnection As System.Windows.Forms.Panel
        Friend WithEvents lblClosingConnections As System.Windows.Forms.Label
        Friend WithEvents radCloseWarnAll As System.Windows.Forms.RadioButton
        Friend WithEvents radCloseWarnMultiple As System.Windows.Forms.RadioButton
        Friend WithEvents radCloseWarnExit As System.Windows.Forms.RadioButton
        Friend WithEvents radCloseWarnNever As System.Windows.Forms.RadioButton

    End Class
End Namespace