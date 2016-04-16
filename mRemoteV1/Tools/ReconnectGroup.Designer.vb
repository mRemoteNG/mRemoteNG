Namespace Tools
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ReconnectGroup
        Inherits System.Windows.Forms.UserControl

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
            Me.components = New System.ComponentModel.Container
            Me.grpAutomaticReconnect = New System.Windows.Forms.GroupBox
            Me.lblAnimation = New System.Windows.Forms.Label
            Me.btnClose = New System.Windows.Forms.Button
            Me.lblServerStatus = New System.Windows.Forms.Label
            Me.chkReconnectWhenReady = New System.Windows.Forms.CheckBox
            Me.pbServerStatus = New System.Windows.Forms.PictureBox
            Me.tmrAnimation = New System.Windows.Forms.Timer(Me.components)
            Me.grpAutomaticReconnect.SuspendLayout()
            CType(Me.pbServerStatus, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'grpAutomaticReconnect
            '
            Me.grpAutomaticReconnect.BackColor = System.Drawing.Color.White
            Me.grpAutomaticReconnect.Controls.Add(Me.lblAnimation)
            Me.grpAutomaticReconnect.Controls.Add(Me.btnClose)
            Me.grpAutomaticReconnect.Controls.Add(Me.lblServerStatus)
            Me.grpAutomaticReconnect.Controls.Add(Me.chkReconnectWhenReady)
            Me.grpAutomaticReconnect.Controls.Add(Me.pbServerStatus)
            Me.grpAutomaticReconnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.grpAutomaticReconnect.Location = New System.Drawing.Point(3, 0)
            Me.grpAutomaticReconnect.Name = "grpAutomaticReconnect"
            Me.grpAutomaticReconnect.Size = New System.Drawing.Size(171, 98)
            Me.grpAutomaticReconnect.TabIndex = 8
            Me.grpAutomaticReconnect.TabStop = False
            Me.grpAutomaticReconnect.Text = "Automatisches wiederverbinden"
            '
            'lblAnimation
            '
            Me.lblAnimation.Location = New System.Drawing.Point(124, 22)
            Me.lblAnimation.Name = "lblAnimation"
            Me.lblAnimation.Size = New System.Drawing.Size(32, 17)
            Me.lblAnimation.TabIndex = 8
            '
            'btnClose
            '
            Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnClose.Location = New System.Drawing.Point(6, 67)
            Me.btnClose.Name = "btnClose"
            Me.btnClose.Size = New System.Drawing.Size(159, 23)
            Me.btnClose.TabIndex = 7
            Me.btnClose.Text = "&Schließen"
            Me.btnClose.UseVisualStyleBackColor = True
            '
            'lblServerStatus
            '
            Me.lblServerStatus.AutoSize = True
            Me.lblServerStatus.Location = New System.Drawing.Point(15, 24)
            Me.lblServerStatus.Name = "lblServerStatus"
            Me.lblServerStatus.Size = New System.Drawing.Size(74, 13)
            Me.lblServerStatus.TabIndex = 3
            Me.lblServerStatus.Text = "Server Status:"
            '
            'chkReconnectWhenReady
            '
            Me.chkReconnectWhenReady.AutoSize = True
            Me.chkReconnectWhenReady.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.chkReconnectWhenReady.Location = New System.Drawing.Point(18, 44)
            Me.chkReconnectWhenReady.Name = "chkReconnectWhenReady"
            Me.chkReconnectWhenReady.Size = New System.Drawing.Size(129, 17)
            Me.chkReconnectWhenReady.TabIndex = 6
            Me.chkReconnectWhenReady.Text = "Verbinden wenn bereit"
            Me.chkReconnectWhenReady.UseVisualStyleBackColor = True
            '
            'pbServerStatus
            '
            Me.pbServerStatus.Image = Global.mRemote3G.My.Resources.Resources.HostStatus_Check
            Me.pbServerStatus.Location = New System.Drawing.Point(99, 23)
            Me.pbServerStatus.Name = "pbServerStatus"
            Me.pbServerStatus.Size = New System.Drawing.Size(16, 16)
            Me.pbServerStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
            Me.pbServerStatus.TabIndex = 5
            Me.pbServerStatus.TabStop = False
            '
            'tmrAnimation
            '
            Me.tmrAnimation.Enabled = True
            Me.tmrAnimation.Interval = 200
            '
            'ReconnectGroup
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.Controls.Add(Me.grpAutomaticReconnect)
            Me.Name = "ReconnectGroup"
            Me.Size = New System.Drawing.Size(228, 138)
            Me.grpAutomaticReconnect.ResumeLayout(False)
            Me.grpAutomaticReconnect.PerformLayout()
            CType(Me.pbServerStatus, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents grpAutomaticReconnect As System.Windows.Forms.GroupBox
        Friend WithEvents btnClose As System.Windows.Forms.Button
        Friend WithEvents lblServerStatus As System.Windows.Forms.Label
        Friend WithEvents chkReconnectWhenReady As System.Windows.Forms.CheckBox
        Friend WithEvents pbServerStatus As System.Windows.Forms.PictureBox
        Friend WithEvents tmrAnimation As System.Windows.Forms.Timer
        Friend WithEvents lblAnimation As System.Windows.Forms.Label

    End Class
End NameSpace