Namespace UI
    Namespace Window
        Partial Public Class SSHCommands
#Region " Windows Form Designer generated code "
            Private components As System.ComponentModel.IContainer

            Private Sub InitializeComponent()
                Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SSHCommands))
                Me.txtSSHCommand = New mRemoteNG.Controls.TextBox()
                Me.SuspendLayout()
                '
                'txtSSHCommand
                '
                Me.txtSSHCommand.Dock = System.Windows.Forms.DockStyle.Fill
                Me.txtSSHCommand.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.txtSSHCommand.Location = New System.Drawing.Point(0, 0)
                Me.txtSSHCommand.Multiline = True
                Me.txtSSHCommand.Name = "txtSSHCommand"
                Me.txtSSHCommand.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
                Me.txtSSHCommand.Size = New System.Drawing.Size(242, 173)
                Me.txtSSHCommand.TabIndex = 0
                '
                'SSHCommands
                '
                Me.ClientSize = New System.Drawing.Size(242, 173)
                Me.Controls.Add(Me.txtSSHCommand)
                Me.DockPnl = Me
                Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.HideOnClose = True
                Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
                Me.Name = "SSHCommands"
                Me.TabText = "SSH command"
                Me.Text = "Send Command"
                Me.ResumeLayout(False)
                Me.PerformLayout()

            End Sub
            Friend WithEvents txtSSHCommand As mRemoteNG.Controls.TextBox
#End Region
        End Class
    End Namespace
End Namespace
