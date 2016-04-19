Namespace UI
    Namespace Window
        Partial Public Class Announcement
#Region " Windows Form Designer generated code "
            Friend WithEvents webBrowser As System.Windows.Forms.WebBrowser

            Private Sub InitializeComponent()
                Me.webBrowser = New System.Windows.Forms.WebBrowser()
                Me.SuspendLayout()
                '
                'webBrowser
                '
                Me.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill
                Me.webBrowser.Location = New System.Drawing.Point(0, 0)
                Me.webBrowser.MinimumSize = New System.Drawing.Size(20, 20)
                Me.webBrowser.Name = "webBrowser"
                Me.webBrowser.Size = New System.Drawing.Size(549, 474)
                Me.webBrowser.TabIndex = 0
                '
                'Announcement
                '
                Me.ClientSize = New System.Drawing.Size(549, 474)
                Me.Controls.Add(Me.webBrowser)
                Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.Icon = Global.mRemote3G.My.Resources.Resources.News_Icon
                Me.Name = "Announcement"
                Me.TabText = "Announcement"
                Me.Text = "Announcement"
                Me.ResumeLayout(False)

            End Sub
#End Region
        End Class
    End Namespace
End Namespace
