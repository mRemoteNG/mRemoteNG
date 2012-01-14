<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPassword
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
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.txtVerify = New System.Windows.Forms.TextBox
        Me.lblPassword = New System.Windows.Forms.Label
        Me.lblVerify = New System.Windows.Forms.Label
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lblStatus = New System.Windows.Forms.Label
        Me.pbLock = New System.Windows.Forms.PictureBox
        Me.pnlImage = New System.Windows.Forms.Panel
        CType(Me.pbLock, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlImage.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtPassword
        '
        Me.txtPassword.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPassword.Location = New System.Drawing.Point(95, 22)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(184, 20)
        Me.txtPassword.TabIndex = 0
        Me.txtPassword.UseSystemPasswordChar = True
        '
        'txtVerify
        '
        Me.txtVerify.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtVerify.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtVerify.Location = New System.Drawing.Point(95, 61)
        Me.txtVerify.Name = "txtVerify"
        Me.txtVerify.Size = New System.Drawing.Size(184, 20)
        Me.txtVerify.TabIndex = 1
        Me.txtVerify.UseSystemPasswordChar = True
        '
        'lblPassword
        '
        Me.lblPassword.AutoSize = True
        Me.lblPassword.Location = New System.Drawing.Point(87, 6)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(56, 13)
        Me.lblPassword.TabIndex = 2
        Me.lblPassword.Text = My.Language.strLabelPassword
        '
        'lblVerify
        '
        Me.lblVerify.AutoSize = True
        Me.lblVerify.Location = New System.Drawing.Point(87, 45)
        Me.lblVerify.Name = "lblVerify"
        Me.lblVerify.Size = New System.Drawing.Size(36, 13)
        Me.lblVerify.TabIndex = 3
        Me.lblVerify.Text = My.Language.strLabelVerify
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOK.Location = New System.Drawing.Point(210, 101)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(69, 23)
        Me.btnOK.TabIndex = 4
        Me.btnOK.Text = My.Language.strButtonOK
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Location = New System.Drawing.Point(135, 101)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(69, 23)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = My.Language.strButtonCancel
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.ForeColor = System.Drawing.Color.OrangeRed
        Me.lblStatus.Location = New System.Drawing.Point(90, 84)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(189, 14)
        Me.lblStatus.TabIndex = 6
        Me.lblStatus.Text = "Status"
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblStatus.Visible = False
        '
        'pbLock
        '
        Me.pbLock.Image = Global.mRemoteNG.My.Resources.Resources.Lock
        Me.pbLock.Location = New System.Drawing.Point(7, 8)
        Me.pbLock.Name = "pbLock"
        Me.pbLock.Size = New System.Drawing.Size(64, 64)
        Me.pbLock.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.pbLock.TabIndex = 7
        Me.pbLock.TabStop = False
        '
        'pnlImage
        '
        Me.pnlImage.Controls.Add(Me.pbLock)
        Me.pnlImage.Location = New System.Drawing.Point(9, 6)
        Me.pnlImage.Name = "pnlImage"
        Me.pnlImage.Size = New System.Drawing.Size(100, 100)
        Me.pnlImage.TabIndex = 8
        '
        'frmPassword
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(289, 133)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtVerify)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.lblVerify)
        Me.Controls.Add(Me.lblPassword)
        Me.Controls.Add(Me.pnlImage)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPassword"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = My.Language.strTitlePassword
        CType(Me.pbLock, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlImage.ResumeLayout(False)
        Me.pnlImage.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtVerify As System.Windows.Forms.TextBox
    Friend WithEvents lblPassword As System.Windows.Forms.Label
    Friend WithEvents lblVerify As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents pbLock As System.Windows.Forms.PictureBox
    Friend WithEvents pnlImage As System.Windows.Forms.Panel
End Class
