Namespace UI
    Namespace Window
        Partial Public Class Update
#Region " Windows Form Designer generated code "
            Friend WithEvents lblStatus As System.Windows.Forms.Label
            Friend WithEvents txtChangeLog As System.Windows.Forms.TextBox
            Friend WithEvents prgbDownload As System.Windows.Forms.ProgressBar
            Friend WithEvents btnDownload As System.Windows.Forms.Button
            Friend WithEvents lblChangeLogLabel As System.Windows.Forms.Label
            Friend WithEvents pnlUpdate As System.Windows.Forms.Panel
            Friend WithEvents lblLatestVersionLabel As System.Windows.Forms.Label
            Friend WithEvents lblInstalledVersionLabel As System.Windows.Forms.Label
            Friend WithEvents lblLatestVersion As System.Windows.Forms.Label
            Friend WithEvents lblInstalledVersion As System.Windows.Forms.Label
            Friend WithEvents pbUpdateImage As System.Windows.Forms.PictureBox
            Friend WithEvents btnCheckForUpdate As System.Windows.Forms.Button

            Private Sub InitializeComponent()
                Me.btnCheckForUpdate = New System.Windows.Forms.Button()
                Me.pnlUpdate = New System.Windows.Forms.Panel()
                Me.lblChangeLogLabel = New System.Windows.Forms.Label()
                Me.btnDownload = New System.Windows.Forms.Button()
                Me.prgbDownload = New System.Windows.Forms.ProgressBar()
                Me.txtChangeLog = New System.Windows.Forms.TextBox()
                Me.lblStatus = New System.Windows.Forms.Label()
                Me.lblLatestVersionLabel = New System.Windows.Forms.Label()
                Me.lblInstalledVersionLabel = New System.Windows.Forms.Label()
                Me.lblLatestVersion = New System.Windows.Forms.Label()
                Me.lblInstalledVersion = New System.Windows.Forms.Label()
                Me.pbUpdateImage = New System.Windows.Forms.PictureBox()
                Me.pnlUpdate.SuspendLayout()
                CType(Me.pbUpdateImage, System.ComponentModel.ISupportInitialize).BeginInit()
                Me.SuspendLayout()
                '
                'btnCheckForUpdate
                '
                Me.btnCheckForUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnCheckForUpdate.Location = New System.Drawing.Point(16, 104)
                Me.btnCheckForUpdate.Name = "btnCheckForUpdate"
                Me.btnCheckForUpdate.Size = New System.Drawing.Size(104, 32)
                Me.btnCheckForUpdate.TabIndex = 5
                Me.btnCheckForUpdate.Text = "Check Again"
                Me.btnCheckForUpdate.UseVisualStyleBackColor = True
                '
                'pnlUpdate
                '
                Me.pnlUpdate.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlUpdate.Controls.Add(Me.lblChangeLogLabel)
                Me.pnlUpdate.Controls.Add(Me.btnDownload)
                Me.pnlUpdate.Controls.Add(Me.prgbDownload)
                Me.pnlUpdate.Controls.Add(Me.txtChangeLog)
                Me.pnlUpdate.Location = New System.Drawing.Point(16, 152)
                Me.pnlUpdate.Name = "pnlUpdate"
                Me.pnlUpdate.Size = New System.Drawing.Size(718, 248)
                Me.pnlUpdate.TabIndex = 6
                Me.pnlUpdate.Visible = False
                '
                'lblChangeLogLabel
                '
                Me.lblChangeLogLabel.AutoSize = True
                Me.lblChangeLogLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblChangeLogLabel.Location = New System.Drawing.Point(0, 0)
                Me.lblChangeLogLabel.Name = "lblChangeLogLabel"
                Me.lblChangeLogLabel.Size = New System.Drawing.Size(79, 13)
                Me.lblChangeLogLabel.TabIndex = 0
                Me.lblChangeLogLabel.Text = "Change Log:"
                '
                'btnDownload
                '
                Me.btnDownload.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
                Me.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnDownload.Location = New System.Drawing.Point(0, 216)
                Me.btnDownload.Name = "btnDownload"
                Me.btnDownload.Size = New System.Drawing.Size(144, 32)
                Me.btnDownload.TabIndex = 2
                Me.btnDownload.Text = "Download and Install"
                Me.btnDownload.UseVisualStyleBackColor = True
                '
                'prgbDownload
                '
                Me.prgbDownload.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.prgbDownload.Location = New System.Drawing.Point(160, 224)
                Me.prgbDownload.Name = "prgbDownload"
                Me.prgbDownload.Size = New System.Drawing.Size(542, 23)
                Me.prgbDownload.TabIndex = 3
                Me.prgbDownload.Visible = False
                '
                'txtChangeLog
                '
                Me.txtChangeLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.txtChangeLog.BorderStyle = System.Windows.Forms.BorderStyle.None
                Me.txtChangeLog.Cursor = System.Windows.Forms.Cursors.Arrow
                Me.txtChangeLog.Location = New System.Drawing.Point(16, 24)
                Me.txtChangeLog.Multiline = True
                Me.txtChangeLog.Name = "txtChangeLog"
                Me.txtChangeLog.ReadOnly = True
                Me.txtChangeLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
                Me.txtChangeLog.Size = New System.Drawing.Size(699, 181)
                Me.txtChangeLog.TabIndex = 1
                Me.txtChangeLog.TabStop = False
                '
                'lblStatus
                '
                Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
                Me.lblStatus.Location = New System.Drawing.Point(12, 16)
                Me.lblStatus.Name = "lblStatus"
                Me.lblStatus.Size = New System.Drawing.Size(660, 23)
                Me.lblStatus.TabIndex = 0
                Me.lblStatus.Text = "Status"
                Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
                '
                'lblLatestVersionLabel
                '
                Me.lblLatestVersionLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblLatestVersionLabel.Location = New System.Drawing.Point(16, 72)
                Me.lblLatestVersionLabel.Name = "lblLatestVersionLabel"
                Me.lblLatestVersionLabel.Size = New System.Drawing.Size(120, 16)
                Me.lblLatestVersionLabel.TabIndex = 3
                Me.lblLatestVersionLabel.Text = "Current version:"
                Me.lblLatestVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
                '
                'lblInstalledVersionLabel
                '
                Me.lblInstalledVersionLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblInstalledVersionLabel.Location = New System.Drawing.Point(16, 48)
                Me.lblInstalledVersionLabel.Name = "lblInstalledVersionLabel"
                Me.lblInstalledVersionLabel.Size = New System.Drawing.Size(120, 16)
                Me.lblInstalledVersionLabel.TabIndex = 1
                Me.lblInstalledVersionLabel.Text = "Installed version:"
                Me.lblInstalledVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
                '
                'lblLatestVersion
                '
                Me.lblLatestVersion.Location = New System.Drawing.Point(136, 72)
                Me.lblLatestVersion.Name = "lblLatestVersion"
                Me.lblLatestVersion.Size = New System.Drawing.Size(104, 16)
                Me.lblLatestVersion.TabIndex = 4
                Me.lblLatestVersion.Text = "Version"
                Me.lblLatestVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
                '
                'lblInstalledVersion
                '
                Me.lblInstalledVersion.Location = New System.Drawing.Point(136, 48)
                Me.lblInstalledVersion.Name = "lblInstalledVersion"
                Me.lblInstalledVersion.Size = New System.Drawing.Size(104, 16)
                Me.lblInstalledVersion.TabIndex = 2
                Me.lblInstalledVersion.Text = "Version"
                Me.lblInstalledVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
                '
                'pbUpdateImage
                '
                Me.pbUpdateImage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pbUpdateImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.pbUpdateImage.Cursor = System.Windows.Forms.Cursors.Hand
                Me.pbUpdateImage.Location = New System.Drawing.Point(246, 48)
                Me.pbUpdateImage.Name = "pbUpdateImage"
                Me.pbUpdateImage.Size = New System.Drawing.Size(468, 60)
                Me.pbUpdateImage.TabIndex = 45
                Me.pbUpdateImage.TabStop = False
                Me.pbUpdateImage.Visible = False
                '
                'Update
                '
                Me.ClientSize = New System.Drawing.Size(734, 418)
                Me.Controls.Add(Me.pbUpdateImage)
                Me.Controls.Add(Me.lblLatestVersionLabel)
                Me.Controls.Add(Me.lblInstalledVersionLabel)
                Me.Controls.Add(Me.lblLatestVersion)
                Me.Controls.Add(Me.btnCheckForUpdate)
                Me.Controls.Add(Me.lblInstalledVersion)
                Me.Controls.Add(Me.pnlUpdate)
                Me.Controls.Add(Me.lblStatus)
                Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.Icon = Global.mRemote3G.My.Resources.Resources.Update_Icon
                Me.Name = "Update"
                Me.TabText = "Update"
                Me.Text = "Update"
                Me.pnlUpdate.ResumeLayout(False)
                Me.pnlUpdate.PerformLayout()
                CType(Me.pbUpdateImage, System.ComponentModel.ISupportInitialize).EndInit()
                Me.ResumeLayout(False)

            End Sub
#End Region
        End Class
    End Namespace
End Namespace
