Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class PasswordForm
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
            Me.lblPassword = New System.Windows.Forms.Label()
            Me.lblVerify = New System.Windows.Forms.Label()
            Me.btnOK = New System.Windows.Forms.Button()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.lblStatus = New System.Windows.Forms.Label()
            Me.pbLock = New System.Windows.Forms.PictureBox()
            Me.txtVerify = New mRemoteNG.Controls.TextBox()
            Me.txtPassword = New mRemoteNG.Controls.TextBox()
            CType(Me.pbLock, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'lblPassword
            '
            Me.lblPassword.AutoSize = True
            Me.lblPassword.Location = New System.Drawing.Point(82, 12)
            Me.lblPassword.Name = "lblPassword"
            Me.lblPassword.Size = New System.Drawing.Size(56, 13)
            Me.lblPassword.TabIndex = 0
            Me.lblPassword.Text = "Password:"
            '
            'lblVerify
            '
            Me.lblVerify.AutoSize = True
            Me.lblVerify.Location = New System.Drawing.Point(82, 51)
            Me.lblVerify.Name = "lblVerify"
            Me.lblVerify.Size = New System.Drawing.Size(36, 13)
            Me.lblVerify.TabIndex = 2
            Me.lblVerify.Text = "Verify:"
            '
            'btnOK
            '
            Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnOK.Location = New System.Drawing.Point(210, 119)
            Me.btnOK.Name = "btnOK"
            Me.btnOK.Size = New System.Drawing.Size(75, 23)
            Me.btnOK.TabIndex = 5
            Me.btnOK.Text = Global.mRemoteNG.My.Language.strButtonOK
            Me.btnOK.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(291, 119)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.TabIndex = 6
            Me.btnCancel.Text = Global.mRemoteNG.My.Language.strButtonCancel
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'lblStatus
            '
            Me.lblStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblStatus.ForeColor = System.Drawing.Color.Red
            Me.lblStatus.Location = New System.Drawing.Point(85, 90)
            Me.lblStatus.Name = "lblStatus"
            Me.lblStatus.Size = New System.Drawing.Size(281, 13)
            Me.lblStatus.TabIndex = 4
            Me.lblStatus.Text = "Status"
            Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopRight
            Me.lblStatus.Visible = False
            '
            'pbLock
            '
            Me.pbLock.Image = Global.mRemoteNG.My.Resources.Resources.Lock
            Me.pbLock.Location = New System.Drawing.Point(12, 12)
            Me.pbLock.Name = "pbLock"
            Me.pbLock.Size = New System.Drawing.Size(64, 64)
            Me.pbLock.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
            Me.pbLock.TabIndex = 7
            Me.pbLock.TabStop = False
            '
            'txtVerify
            '
            Me.txtVerify.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtVerify.Location = New System.Drawing.Point(85, 67)
            Me.txtVerify.Name = "txtVerify"
            Me.txtVerify.SelectAllOnFocus = True
            Me.txtVerify.Size = New System.Drawing.Size(281, 20)
            Me.txtVerify.TabIndex = 3
            Me.txtVerify.UseSystemPasswordChar = True
            '
            'txtPassword
            '
            Me.txtPassword.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtPassword.Location = New System.Drawing.Point(85, 28)
            Me.txtPassword.Name = "txtPassword"
            Me.txtPassword.SelectAllOnFocus = True
            Me.txtPassword.Size = New System.Drawing.Size(281, 20)
            Me.txtPassword.TabIndex = 1
            Me.txtPassword.UseSystemPasswordChar = True
            '
            'PasswordForm
            '
            Me.AcceptButton = Me.btnOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(378, 154)
            Me.ControlBox = False
            Me.Controls.Add(Me.pbLock)
            Me.Controls.Add(Me.txtVerify)
            Me.Controls.Add(Me.txtPassword)
            Me.Controls.Add(Me.lblStatus)
            Me.Controls.Add(Me.lblVerify)
            Me.Controls.Add(Me.lblPassword)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOK)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "PasswordForm"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Password"
            CType(Me.pbLock, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Private WithEvents txtPassword As Controls.TextBox
        Private WithEvents txtVerify As Controls.TextBox
        Private WithEvents lblPassword As System.Windows.Forms.Label
        Private WithEvents lblVerify As System.Windows.Forms.Label
        Private WithEvents btnOK As System.Windows.Forms.Button
        Private WithEvents btnCancel As System.Windows.Forms.Button
        Private WithEvents lblStatus As System.Windows.Forms.Label
        Private WithEvents pbLock As System.Windows.Forms.PictureBox
    End Class
End Namespace