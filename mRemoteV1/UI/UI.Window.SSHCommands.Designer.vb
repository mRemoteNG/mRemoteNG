Namespace UI
    Namespace Window
        Partial Public Class SSHCommands
#Region " Windows Form Designer generated code "
            Private components As System.ComponentModel.IContainer

            Private Sub InitializeComponent()
                Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SSHCommands))
                Me.txtSSHCommand = New System.Windows.Forms.RichTextBox()
                Me.lstCommands = New System.Windows.Forms.ListBox()
                Me.SuspendLayout()
                '
                'txtSSHCommand
                '
                Me.txtSSHCommand.Dock = System.Windows.Forms.DockStyle.Fill
                Me.txtSSHCommand.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.txtSSHCommand.Location = New System.Drawing.Point(0, 0)
                Me.txtSSHCommand.MinimumSize = New System.Drawing.Size(0, 150)
                Me.txtSSHCommand.Name = "txtSSHCommand"
                Me.txtSSHCommand.Size = New System.Drawing.Size(796, 378)
                Me.txtSSHCommand.TabIndex = 0
                Me.txtSSHCommand.Text = ""
                '
                'lstCommands
                '
                Me.lstCommands.Dock = System.Windows.Forms.DockStyle.Right
                Me.lstCommands.FormattingEnabled = True
                Me.lstCommands.Location = New System.Drawing.Point(676, 0)
                Me.lstCommands.Name = "lstCommands"
                Me.lstCommands.Size = New System.Drawing.Size(120, 378)
                Me.lstCommands.TabIndex = 1
                '
                'SSHCommands
                '
                Me.ClientSize = New System.Drawing.Size(796, 378)
                Me.Controls.Add(Me.lstCommands)
                Me.Controls.Add(Me.txtSSHCommand)
                Me.DockPnl = Me
                Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.HideOnClose = True
                Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
                Me.Name = "SSHCommands"
                Me.TabText = "SSH command"
                Me.Text = "Send Command"
                Me.ResumeLayout(False)

            End Sub
            'Friend WithEvents txtSSHCommand As mRemoteNG.Controls.TextBox
            Friend WithEvents txtSSHCommand As System.Windows.Forms.RichTextBox
            Friend WithEvents lstCommands As System.Windows.Forms.ListBox
            'Friend WithEvents txtSSHCommand As rich
#End Region
        End Class
    End Namespace
End Namespace
