Namespace Forms.OptionsPages
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class AdvancedPage
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AdvancedPage))
            Me.chkWriteLogFile = New System.Windows.Forms.CheckBox()
            Me.chkAutomaticallyGetSessionInfo = New System.Windows.Forms.CheckBox()
            Me.lblMaximumPuttyWaitTime = New System.Windows.Forms.Label()
            Me.chkEncryptCompleteFile = New System.Windows.Forms.CheckBox()
            Me.chkAutomaticReconnect = New System.Windows.Forms.CheckBox()
            Me.numPuttyWaitTime = New System.Windows.Forms.NumericUpDown()
            Me.chkUseCustomPuttyPath = New System.Windows.Forms.CheckBox()
            Me.lblConfigurePuttySessions = New System.Windows.Forms.Label()
            Me.numUVNCSCPort = New System.Windows.Forms.NumericUpDown()
            Me.txtCustomPuttyPath = New System.Windows.Forms.TextBox()
            Me.btnLaunchPutty = New System.Windows.Forms.Button()
            Me.lblUVNCSCPort = New System.Windows.Forms.Label()
            Me.lblSeconds = New System.Windows.Forms.Label()
            Me.btnBrowseCustomPuttyPath = New System.Windows.Forms.Button()
            CType(Me.numPuttyWaitTime, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numUVNCSCPort, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'chkWriteLogFile
            '
            Me.chkWriteLogFile.AutoSize = True
            Me.chkWriteLogFile.Location = New System.Drawing.Point(4, 0)
            Me.chkWriteLogFile.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
            Me.chkWriteLogFile.Name = "chkWriteLogFile"
            Me.chkWriteLogFile.Size = New System.Drawing.Size(226, 21)
            Me.chkWriteLogFile.TabIndex = 17
            Me.chkWriteLogFile.Text = "Write log file (mRemote3G.log)"
            Me.chkWriteLogFile.UseVisualStyleBackColor = True
            '
            'chkAutomaticallyGetSessionInfo
            '
            Me.chkAutomaticallyGetSessionInfo.AutoSize = True
            Me.chkAutomaticallyGetSessionInfo.Location = New System.Drawing.Point(4, 57)
            Me.chkAutomaticallyGetSessionInfo.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
            Me.chkAutomaticallyGetSessionInfo.Name = "chkAutomaticallyGetSessionInfo"
            Me.chkAutomaticallyGetSessionInfo.Size = New System.Drawing.Size(263, 21)
            Me.chkAutomaticallyGetSessionInfo.TabIndex = 19
            Me.chkAutomaticallyGetSessionInfo.Text = "Automatically get session information"
            Me.chkAutomaticallyGetSessionInfo.UseVisualStyleBackColor = True
            '
            'lblMaximumPuttyWaitTime
            '
            Me.lblMaximumPuttyWaitTime.Location = New System.Drawing.Point(4, 228)
            Me.lblMaximumPuttyWaitTime.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
            Me.lblMaximumPuttyWaitTime.Name = "lblMaximumPuttyWaitTime"
            Me.lblMaximumPuttyWaitTime.Size = New System.Drawing.Size(485, 16)
            Me.lblMaximumPuttyWaitTime.TabIndex = 26
            Me.lblMaximumPuttyWaitTime.Text = "Maximum PuTTY wait time:"
            Me.lblMaximumPuttyWaitTime.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'chkEncryptCompleteFile
            '
            Me.chkEncryptCompleteFile.AutoSize = True
            Me.chkEncryptCompleteFile.Location = New System.Drawing.Point(4, 28)
            Me.chkEncryptCompleteFile.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
            Me.chkEncryptCompleteFile.Name = "chkEncryptCompleteFile"
            Me.chkEncryptCompleteFile.Size = New System.Drawing.Size(234, 21)
            Me.chkEncryptCompleteFile.TabIndex = 18
            Me.chkEncryptCompleteFile.Text = "Encrypt complete connection file"
            Me.chkEncryptCompleteFile.UseVisualStyleBackColor = True
            '
            'chkAutomaticReconnect
            '
            Me.chkAutomaticReconnect.AutoSize = True
            Me.chkAutomaticReconnect.Location = New System.Drawing.Point(4, 85)
            Me.chkAutomaticReconnect.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
            Me.chkAutomaticReconnect.Name = "chkAutomaticReconnect"
            Me.chkAutomaticReconnect.Size = New System.Drawing.Size(490, 21)
            Me.chkAutomaticReconnect.TabIndex = 20
            Me.chkAutomaticReconnect.Text = "Automatically try to reconnect when disconnected from server (RDP only)"
            Me.chkAutomaticReconnect.UseVisualStyleBackColor = True
            '
            'numPuttyWaitTime
            '
            Me.numPuttyWaitTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.numPuttyWaitTime.Location = New System.Drawing.Point(497, 225)
            Me.numPuttyWaitTime.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
            Me.numPuttyWaitTime.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
            Me.numPuttyWaitTime.Name = "numPuttyWaitTime"
            Me.numPuttyWaitTime.Size = New System.Drawing.Size(65, 22)
            Me.numPuttyWaitTime.TabIndex = 27
            Me.numPuttyWaitTime.Value = New Decimal(New Integer() {5, 0, 0, 0})
            '
            'chkUseCustomPuttyPath
            '
            Me.chkUseCustomPuttyPath.AutoSize = True
            Me.chkUseCustomPuttyPath.Location = New System.Drawing.Point(4, 113)
            Me.chkUseCustomPuttyPath.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
            Me.chkUseCustomPuttyPath.Name = "chkUseCustomPuttyPath"
            Me.chkUseCustomPuttyPath.Size = New System.Drawing.Size(188, 21)
            Me.chkUseCustomPuttyPath.TabIndex = 21
            Me.chkUseCustomPuttyPath.Text = "Use custom PuTTY path:"
            Me.chkUseCustomPuttyPath.UseVisualStyleBackColor = True
            '
            'lblConfigurePuttySessions
            '
            Me.lblConfigurePuttySessions.Location = New System.Drawing.Point(4, 190)
            Me.lblConfigurePuttySessions.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
            Me.lblConfigurePuttySessions.Name = "lblConfigurePuttySessions"
            Me.lblConfigurePuttySessions.Size = New System.Drawing.Size(485, 16)
            Me.lblConfigurePuttySessions.TabIndex = 24
            Me.lblConfigurePuttySessions.Text = "To configure PuTTY sessions click this button:"
            Me.lblConfigurePuttySessions.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'numUVNCSCPort
            '
            Me.numUVNCSCPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.numUVNCSCPort.Location = New System.Drawing.Point(497, 264)
            Me.numUVNCSCPort.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
            Me.numUVNCSCPort.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
            Me.numUVNCSCPort.Name = "numUVNCSCPort"
            Me.numUVNCSCPort.Size = New System.Drawing.Size(96, 22)
            Me.numUVNCSCPort.TabIndex = 33
            Me.numUVNCSCPort.Value = New Decimal(New Integer() {5500, 0, 0, 0})
            Me.numUVNCSCPort.Visible = False
            '
            'txtCustomPuttyPath
            '
            Me.txtCustomPuttyPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.txtCustomPuttyPath.Enabled = False
            Me.txtCustomPuttyPath.Location = New System.Drawing.Point(28, 142)
            Me.txtCustomPuttyPath.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
            Me.txtCustomPuttyPath.Name = "txtCustomPuttyPath"
            Me.txtCustomPuttyPath.Size = New System.Drawing.Size(461, 22)
            Me.txtCustomPuttyPath.TabIndex = 22
            '
            'btnLaunchPutty
            '
            Me.btnLaunchPutty.Image = Global.mRemote3G.My.Resources.Resources.PuttyConfig
            Me.btnLaunchPutty.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnLaunchPutty.Location = New System.Drawing.Point(497, 183)
            Me.btnLaunchPutty.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
            Me.btnLaunchPutty.Name = "btnLaunchPutty"
            Me.btnLaunchPutty.Size = New System.Drawing.Size(147, 28)
            Me.btnLaunchPutty.TabIndex = 25
            Me.btnLaunchPutty.Text = "Launch PuTTY"
            Me.btnLaunchPutty.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnLaunchPutty.UseVisualStyleBackColor = True
            '
            'lblUVNCSCPort
            '
            Me.lblUVNCSCPort.Location = New System.Drawing.Point(9, 264)
            Me.lblUVNCSCPort.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
            Me.lblUVNCSCPort.Name = "lblUVNCSCPort"
            Me.lblUVNCSCPort.Size = New System.Drawing.Size(485, 16)
            Me.lblUVNCSCPort.TabIndex = 32
            Me.lblUVNCSCPort.Text = "UltraVNC SingleClick Listening Port:"
            Me.lblUVNCSCPort.TextAlign = System.Drawing.ContentAlignment.TopRight
            Me.lblUVNCSCPort.Visible = False
            '
            'lblSeconds
            '
            Me.lblSeconds.AutoSize = True
            Me.lblSeconds.Location = New System.Drawing.Point(571, 228)
            Me.lblSeconds.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
            Me.lblSeconds.Name = "lblSeconds"
            Me.lblSeconds.Size = New System.Drawing.Size(61, 17)
            Me.lblSeconds.TabIndex = 28
            Me.lblSeconds.Text = "seconds"
            '
            'btnBrowseCustomPuttyPath
            '
            Me.btnBrowseCustomPuttyPath.Enabled = False
            Me.btnBrowseCustomPuttyPath.Location = New System.Drawing.Point(497, 139)
            Me.btnBrowseCustomPuttyPath.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
            Me.btnBrowseCustomPuttyPath.Name = "btnBrowseCustomPuttyPath"
            Me.btnBrowseCustomPuttyPath.Size = New System.Drawing.Size(100, 28)
            Me.btnBrowseCustomPuttyPath.TabIndex = 23
            Me.btnBrowseCustomPuttyPath.Text = "Browse..."
            Me.btnBrowseCustomPuttyPath.UseVisualStyleBackColor = True
            '
            'AdvancedPage
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.chkWriteLogFile)
            Me.Controls.Add(Me.chkAutomaticallyGetSessionInfo)
            Me.Controls.Add(Me.lblMaximumPuttyWaitTime)
            Me.Controls.Add(Me.chkEncryptCompleteFile)
            Me.Controls.Add(Me.chkAutomaticReconnect)
            Me.Controls.Add(Me.numPuttyWaitTime)
            Me.Controls.Add(Me.chkUseCustomPuttyPath)
            Me.Controls.Add(Me.lblConfigurePuttySessions)
            Me.Controls.Add(Me.numUVNCSCPort)
            Me.Controls.Add(Me.txtCustomPuttyPath)
            Me.Controls.Add(Me.btnLaunchPutty)
            Me.Controls.Add(Me.lblUVNCSCPort)
            Me.Controls.Add(Me.lblSeconds)
            Me.Controls.Add(Me.btnBrowseCustomPuttyPath)
            Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
            Me.Name = "AdvancedPage"
            Me.PageIcon = CType(resources.GetObject("$this.PageIcon"), System.Drawing.Icon)
            Me.Size = New System.Drawing.Size(813, 602)
            CType(Me.numPuttyWaitTime, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numUVNCSCPort, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents chkWriteLogFile As System.Windows.Forms.CheckBox
        Friend WithEvents chkAutomaticallyGetSessionInfo As System.Windows.Forms.CheckBox
        Friend WithEvents lblMaximumPuttyWaitTime As System.Windows.Forms.Label
        Friend WithEvents chkEncryptCompleteFile As System.Windows.Forms.CheckBox
        Friend WithEvents chkAutomaticReconnect As System.Windows.Forms.CheckBox
        Friend WithEvents numPuttyWaitTime As System.Windows.Forms.NumericUpDown
        Friend WithEvents chkUseCustomPuttyPath As System.Windows.Forms.CheckBox
        Friend WithEvents lblConfigurePuttySessions As System.Windows.Forms.Label
        Friend WithEvents numUVNCSCPort As System.Windows.Forms.NumericUpDown
        Friend WithEvents txtCustomPuttyPath As System.Windows.Forms.TextBox
        Friend WithEvents btnLaunchPutty As System.Windows.Forms.Button
        Friend WithEvents lblUVNCSCPort As System.Windows.Forms.Label
        Friend WithEvents lblSeconds As System.Windows.Forms.Label
        Friend WithEvents btnBrowseCustomPuttyPath As System.Windows.Forms.Button

    End Class
End Namespace