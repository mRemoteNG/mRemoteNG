Imports WeifenLuo.WinFormsUI.Docking
Imports mRemoteNG.App.Runtime

Namespace UI
    Namespace Window
        Public Class SaveAs
            Inherits UI.Window.Base

#Region "Form Init"
            Friend WithEvents btnCancel As System.Windows.Forms.Button
            Friend WithEvents lvSecurity As System.Windows.Forms.ListView
            Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
            Friend WithEvents btnOK As System.Windows.Forms.Button
            Friend WithEvents lblMremoteXMLOnly As System.Windows.Forms.Label
            Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
            Friend WithEvents Label1 As System.Windows.Forms.Label

            Private Sub InitializeComponent()
                Dim ListViewItem1 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Username")
                Dim ListViewItem2 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Password")
                Dim ListViewItem3 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Domain")
                Dim ListViewItem4 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Inheritance")
                Me.btnCancel = New System.Windows.Forms.Button
                Me.btnOK = New System.Windows.Forms.Button
                Me.lvSecurity = New System.Windows.Forms.ListView
                Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
                Me.Label1 = New System.Windows.Forms.Label
                Me.lblMremoteXMLOnly = New System.Windows.Forms.Label
                Me.PictureBox1 = New System.Windows.Forms.PictureBox
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
                Me.btnCancel.TabIndex = 110
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
                Me.btnOK.TabIndex = 100
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
                Me.lvSecurity.TabIndex = 20
                Me.lvSecurity.UseCompatibleStateImageBehavior = False
                Me.lvSecurity.View = System.Windows.Forms.View.Details
                '
                'ColumnHeader1
                '
                Me.ColumnHeader1.Width = 110
                '
                'Label1
                '
                Me.Label1.AutoSize = True
                Me.Label1.Location = New System.Drawing.Point(12, 11)
                Me.Label1.Name = "Label1"
                Me.Label1.Size = New System.Drawing.Size(244, 13)
                Me.Label1.TabIndex = 10
                Me.Label1.Text = "Uncheck the properties you want not to be saved!"
                '
                'lblMremoteXMLOnly
                '
                Me.lblMremoteXMLOnly.AutoSize = True
                Me.lblMremoteXMLOnly.ForeColor = System.Drawing.Color.DarkRed
                Me.lblMremoteXMLOnly.Location = New System.Drawing.Point(37, 33)
                Me.lblMremoteXMLOnly.Name = "lblMremoteXMLOnly"
                Me.lblMremoteXMLOnly.Size = New System.Drawing.Size(345, 13)
                Me.lblMremoteXMLOnly.TabIndex = 111
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
                'SaveAs
                '
                Me.AcceptButton = Me.btnOK
                Me.CancelButton = Me.btnCancel
                Me.ClientSize = New System.Drawing.Size(534, 396)
                Me.Controls.Add(Me.PictureBox1)
                Me.Controls.Add(Me.lblMremoteXMLOnly)
                Me.Controls.Add(Me.Label1)
                Me.Controls.Add(Me.lvSecurity)
                Me.Controls.Add(Me.btnCancel)
                Me.Controls.Add(Me.btnOK)
                Me.Icon = Global.mRemoteNG.My.Resources.Resources.Connections_SaveAs_Icon
                Me.Name = "SaveAs"
                Me.TabText = "Save Connections As"
                Me.Text = "Save Connections As"
                CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
                Me.ResumeLayout(False)
                Me.PerformLayout()

            End Sub
#End Region

#Region "Public Properties"
            Private _Export As Boolean
            Public Property Export() As Boolean
                Get
                    Return _Export
                End Get
                Set(ByVal value As Boolean)
                    _Export = value
                End Set
            End Property

            Private _TreeNode As TreeNode
            Public Property TreeNode() As TreeNode
                Get
                    Return _TreeNode
                End Get
                Set(ByVal value As TreeNode)
                    _TreeNode = value
                End Set
            End Property
#End Region

#Region "Public Methods"
            Public Sub New(ByVal Panel As DockContent)
                Me.New(Panel, False, Nothing)
            End Sub

            Public Sub New(ByVal Panel As DockContent, ByVal Export As Boolean, ByVal TreeNode As TreeNode)
                Me.WindowType = Type.SaveAs
                Me.DockPnl = Panel
                Me.InitializeComponent()

                If Export Then
                    Me.SetFormText(My.Resources.strExport)
                Else
                    Me.SetFormText(My.Resources.strMenuSaveConnectionFileAs)
                End If

                Me._Export = Export
                Me._TreeNode = TreeNode
            End Sub
#End Region

#Region "Form Stuff"
            Private Sub SaveAs_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
                ApplyLanguage()
            End Sub

            Private Sub ApplyLanguage()
                lvSecurity.Items(0).Text = My.Resources.strCheckboxUsername
                lvSecurity.Items(1).Text = My.Resources.strCheckboxPassword
                lvSecurity.Items(2).Text = My.Resources.strCheckboxDomain
                lvSecurity.Items(3).Text = My.Resources.strCheckboxInheritance
                btnCancel.Text = My.Resources.strButtonCancel
                btnOK.Text = My.Resources.strButtonOK
                Label1.Text = My.Resources.strUncheckProperties
                lblMremoteXMLOnly.Text = My.Resources.strPropertiesWillOnlyBeSavedMRemoteXML
                TabText = My.Resources.strMenuSaveConnectionFileAs
                Text = My.Resources.strMenuSaveConnectionFileAs
            End Sub

            Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
                Me.Close()
            End Sub

            Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
                Try
                    If _TreeNode Is Nothing Then
                        _TreeNode = App.Runtime.Windows.treeForm.tvConnections.Nodes(0)
                    End If

                    Dim sS As New Security.Save()

                    sS.Username = Me.lvSecurity.Items(0).Checked
                    sS.Password = Me.lvSecurity.Items(1).Checked
                    sS.Domain = Me.lvSecurity.Items(2).Checked
                    sS.Inheritance = Me.lvSecurity.Items(3).Checked

                    App.Runtime.SaveConnectionsAs(sS, _TreeNode)
                    Me.Close()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "btnOK_Click (UI.Window.SaveAs) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

        End Class
    End Namespace
End Namespace