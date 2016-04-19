Namespace UI
    Namespace Window
        Partial Public Class ActiveDirectoryImport
            Inherits UI.Window.Base
#Region " Windows Form Designer generated code "

            Private Sub InitializeComponent()
                Me.btnImport = New System.Windows.Forms.Button()
                Me.txtDomain = New System.Windows.Forms.TextBox()
                Me.lblDomain = New System.Windows.Forms.Label()
                Me.btnChangeDomain = New System.Windows.Forms.Button()
                Me.ActiveDirectoryTree = New ADTree.ADtree()
                Me.SuspendLayout()
                '
                'btnImport
                '
                Me.btnImport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.btnImport.Location = New System.Drawing.Point(443, 338)
                Me.btnImport.Name = "btnImport"
                Me.btnImport.Size = New System.Drawing.Size(75, 23)
                Me.btnImport.TabIndex = 4
                Me.btnImport.Text = "&Import"
                Me.btnImport.UseVisualStyleBackColor = True
                '
                'txtDomain
                '
                Me.txtDomain.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.txtDomain.Location = New System.Drawing.Point(12, 25)
                Me.txtDomain.Name = "txtDomain"
                Me.txtDomain.Size = New System.Drawing.Size(425, 20)
                Me.txtDomain.TabIndex = 1
                '
                'lblDomain
                '
                Me.lblDomain.AutoSize = True
                Me.lblDomain.Location = New System.Drawing.Point(9, 9)
                Me.lblDomain.Name = "lblDomain"
                Me.lblDomain.Size = New System.Drawing.Size(46, 13)
                Me.lblDomain.TabIndex = 0
                Me.lblDomain.Text = "Domain:"
                '
                'btnChangeDomain
                '
                Me.btnChangeDomain.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.btnChangeDomain.Location = New System.Drawing.Point(443, 23)
                Me.btnChangeDomain.Name = "btnChangeDomain"
                Me.btnChangeDomain.Size = New System.Drawing.Size(75, 23)
                Me.btnChangeDomain.TabIndex = 2
                Me.btnChangeDomain.Text = "Change"
                Me.btnChangeDomain.UseVisualStyleBackColor = True
                '
                'ActiveDirectoryTree
                '
                Me.ActiveDirectoryTree.ADPath = Nothing
                Me.ActiveDirectoryTree.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.ActiveDirectoryTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.ActiveDirectoryTree.Location = New System.Drawing.Point(12, 52)
                Me.ActiveDirectoryTree.Name = "ActiveDirectoryTree"
                Me.ActiveDirectoryTree.SelectedNode = Nothing
                Me.ActiveDirectoryTree.Size = New System.Drawing.Size(506, 280)
                Me.ActiveDirectoryTree.TabIndex = 3
                '
                'ADImport
                '
                Me.AcceptButton = Me.btnImport
                Me.ClientSize = New System.Drawing.Size(530, 373)
                Me.Controls.Add(Me.ActiveDirectoryTree)
                Me.Controls.Add(Me.lblDomain)
                Me.Controls.Add(Me.txtDomain)
                Me.Controls.Add(Me.btnChangeDomain)
                Me.Controls.Add(Me.btnImport)
                Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.Icon = Global.mRemote3G.My.Resources.Resources.ActiveDirectory_Icon
                Me.Name = "ActiveDirectoryImport"
                Me.TabText = "Active Directory Import"
                Me.Text = "Active Directory Import"
                Me.ResumeLayout(False)
                Me.PerformLayout()

            End Sub
            Private WithEvents btnImport As System.Windows.Forms.Button
            Private WithEvents txtDomain As System.Windows.Forms.TextBox
            Private WithEvents lblDomain As System.Windows.Forms.Label
            Private WithEvents btnChangeDomain As System.Windows.Forms.Button
            Private WithEvents ActiveDirectoryTree As ADTree.ADtree
#End Region
        End Class
    End Namespace
End Namespace

