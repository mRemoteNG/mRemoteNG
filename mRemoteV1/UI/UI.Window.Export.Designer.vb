Namespace UI
    Namespace Window
        Public Partial Class Export
            Inherits Base
#Region " Windows Form Designer generated code "
            Friend WithEvents btnCancel As System.Windows.Forms.Button
            Friend WithEvents lvSecurity As System.Windows.Forms.ListView
            Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
            Friend WithEvents btnOK As System.Windows.Forms.Button
            Friend WithEvents lblMremoteXMLOnly As System.Windows.Forms.Label
            Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
            Friend WithEvents lblUncheckProperties As System.Windows.Forms.Label

            Private Sub InitializeComponent()
                Dim ListViewItem1 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Username")
                Dim ListViewItem2 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Password")
                Dim ListViewItem3 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Domain")
                Dim ListViewItem4 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Inheritance")
                Me.btnCancel = New System.Windows.Forms.Button()
                Me.btnOK = New System.Windows.Forms.Button()
                Me.lvSecurity = New System.Windows.Forms.ListView()
                Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
                Me.lblUncheckProperties = New System.Windows.Forms.Label()
                Me.lblMremoteXMLOnly = New System.Windows.Forms.Label()
                Me.PictureBox1 = New System.Windows.Forms.PictureBox()
                CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
                Me.SuspendLayout()
                '
                'btnCancel
                '
                Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
                Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnCancel.Location = New System.Drawing.Point(457, 370)
                Me.btnCancel.Name = "btnCancel"
                Me.btnCancel.Size = New System.Drawing.Size(75, 23)
                Me.btnCancel.TabIndex = 4
                Me.btnCancel.Text = "&Cancel"
                Me.btnCancel.UseVisualStyleBackColor = True
                '
                'btnOK
                '
                Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnOK.Location = New System.Drawing.Point(376, 370)
                Me.btnOK.Name = "btnOK"
                Me.btnOK.Size = New System.Drawing.Size(75, 23)
                Me.btnOK.TabIndex = 3
                Me.btnOK.Text = "&OK"
                Me.btnOK.UseVisualStyleBackColor = True
                '
                'lvSecurity
                '
                Me.lvSecurity.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.lvSecurity.BorderStyle = System.Windows.Forms.BorderStyle.None
                Me.lvSecurity.CheckBoxes = True
                Me.lvSecurity.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
                Me.lvSecurity.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
                ListViewItem1.Checked = True
                ListViewItem1.StateImageIndex = 1
                ListViewItem2.Checked = True
                ListViewItem2.StateImageIndex = 1
                ListViewItem3.Checked = True
                ListViewItem3.StateImageIndex = 1
                ListViewItem4.Checked = True
                ListViewItem4.StateImageIndex = 1
                Me.lvSecurity.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem1, ListViewItem2, ListViewItem3, ListViewItem4})
                Me.lvSecurity.Location = New System.Drawing.Point(0, 55)
                Me.lvSecurity.MultiSelect = False
                Me.lvSecurity.Name = "lvSecurity"
                Me.lvSecurity.Scrollable = False
                Me.lvSecurity.Size = New System.Drawing.Size(532, 309)
                Me.lvSecurity.TabIndex = 2
                Me.lvSecurity.UseCompatibleStateImageBehavior = False
                Me.lvSecurity.View = System.Windows.Forms.View.Details
                '
                'ColumnHeader1
                '
                Me.ColumnHeader1.Width = 110
                '
                'Label1
                '
                Me.lblUncheckProperties.AutoSize = True
                Me.lblUncheckProperties.Location = New System.Drawing.Point(12, 11)
                Me.lblUncheckProperties.Name = "lblUncheckProperties"
                Me.lblUncheckProperties.Size = New System.Drawing.Size(244, 13)
                Me.lblUncheckProperties.TabIndex = 0
                Me.lblUncheckProperties.Text = "Uncheck the properties you want not to be saved!"
                '
                'lblMremoteXMLOnly
                '
                Me.lblMremoteXMLOnly.AutoSize = True
                Me.lblMremoteXMLOnly.ForeColor = System.Drawing.Color.DarkRed
                Me.lblMremoteXMLOnly.Location = New System.Drawing.Point(37, 33)
                Me.lblMremoteXMLOnly.Name = "lblMremoteXMLOnly"
                Me.lblMremoteXMLOnly.Size = New System.Drawing.Size(345, 13)
                Me.lblMremoteXMLOnly.TabIndex = 1
                Me.lblMremoteXMLOnly.Text = "(These properties will only be saved if you select a mRemote file format!)"
                '
                'PictureBox1
                '
                Me.PictureBox1.Image = Global.mRemoteNG.My.Resources.Resources.WarningSmall
                Me.PictureBox1.Location = New System.Drawing.Point(15, 31)
                Me.PictureBox1.Name = "PictureBox1"
                Me.PictureBox1.Size = New System.Drawing.Size(16, 16)
                Me.PictureBox1.TabIndex = 112
                Me.PictureBox1.TabStop = False
                '
                'Export
                '
                Me.AcceptButton = Me.btnOK
                Me.CancelButton = Me.btnCancel
                Me.ClientSize = New System.Drawing.Size(534, 396)
                Me.Controls.Add(Me.PictureBox1)
                Me.Controls.Add(Me.lblMremoteXMLOnly)
                Me.Controls.Add(Me.lblUncheckProperties)
                Me.Controls.Add(Me.lvSecurity)
                Me.Controls.Add(Me.btnCancel)
                Me.Controls.Add(Me.btnOK)
                Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.Icon = Global.mRemoteNG.My.Resources.Resources.Connections_SaveAs_Icon
                Me.Name = "Export"
                Me.TabText = "Export Connections"
                Me.Text = "Export Connections"
                CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
                Me.ResumeLayout(False)
                Me.PerformLayout()

            End Sub
#End Region
        End Class
    End Namespace
End Namespace
