Imports WeifenLuo.WinFormsUI.Docking

Namespace UI
    Namespace Window
        Public Class ADImport
            Inherits UI.Window.Base

#Region "Form Init"
            Friend WithEvents btnCancel As System.Windows.Forms.Button
            Friend WithEvents btnOK As System.Windows.Forms.Button
            Friend WithEvents txtDomain As System.Windows.Forms.TextBox
            Friend WithEvents lblDomain As System.Windows.Forms.Label
            Friend WithEvents btnChangeDomain As System.Windows.Forms.Button
            Friend WithEvents AD As ADTree.ADtree

            Private Sub InitializeComponent()
                Me.btnOK = New System.Windows.Forms.Button
                Me.btnCancel = New System.Windows.Forms.Button
                Me.txtDomain = New System.Windows.Forms.TextBox
                Me.lblDomain = New System.Windows.Forms.Label
                Me.btnChangeDomain = New System.Windows.Forms.Button
                Me.AD = New ADTree.ADtree
                Me.SuspendLayout()
                '
                'btnOK
                '
                Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnOK.Location = New System.Drawing.Point(372, 347)
                Me.btnOK.Name = "btnOK"
                Me.btnOK.Size = New System.Drawing.Size(75, 23)
                Me.btnOK.TabIndex = 100
                Me.btnOK.Text = Global.mRemoteNG.My.Resources.Resources.strButtonOK
                Me.btnOK.UseVisualStyleBackColor = True
                '
                'btnCancel
                '
                Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
                Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnCancel.Location = New System.Drawing.Point(453, 347)
                Me.btnCancel.Name = "btnCancel"
                Me.btnCancel.Size = New System.Drawing.Size(75, 23)
                Me.btnCancel.TabIndex = 110
                Me.btnCancel.Text = Global.mRemoteNG.My.Resources.Resources.strButtonCancel
                Me.btnCancel.UseVisualStyleBackColor = True
                '
                'txtDomain
                '
                Me.txtDomain.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
                Me.txtDomain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.txtDomain.Location = New System.Drawing.Point(57, 348)
                Me.txtDomain.Name = "txtDomain"
                Me.txtDomain.Size = New System.Drawing.Size(100, 20)
                Me.txtDomain.TabIndex = 30
                '
                'lblDomain
                '
                Me.lblDomain.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
                Me.lblDomain.AutoSize = True
                Me.lblDomain.Location = New System.Drawing.Point(5, 351)
                Me.lblDomain.Name = "lblDomain"
                Me.lblDomain.Size = New System.Drawing.Size(46, 13)
                Me.lblDomain.TabIndex = 20
                Me.lblDomain.Text = "Domain:"
                '
                'btnChangeDomain
                '
                Me.btnChangeDomain.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
                Me.btnChangeDomain.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnChangeDomain.Location = New System.Drawing.Point(163, 347)
                Me.btnChangeDomain.Name = "btnChangeDomain"
                Me.btnChangeDomain.Size = New System.Drawing.Size(75, 23)
                Me.btnChangeDomain.TabIndex = 40
                Me.btnChangeDomain.Text = Global.mRemoteNG.My.Resources.Resources.strButtonChange
                Me.btnChangeDomain.UseVisualStyleBackColor = True
                '
                'AD
                '
                Me.AD.ADPath = Nothing
                Me.AD.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.AD.Domain = "akane"
                Me.AD.Location = New System.Drawing.Point(0, 0)
                Me.AD.Name = "AD"
                Me.AD.SelectedNode = Nothing
                Me.AD.Size = New System.Drawing.Size(530, 341)
                Me.AD.TabIndex = 10
                '
                'ADImport
                '
                Me.AcceptButton = Me.btnOK
                Me.CancelButton = Me.btnCancel
                Me.ClientSize = New System.Drawing.Size(530, 373)
                Me.Controls.Add(Me.AD)
                Me.Controls.Add(Me.lblDomain)
                Me.Controls.Add(Me.txtDomain)
                Me.Controls.Add(Me.btnChangeDomain)
                Me.Controls.Add(Me.btnCancel)
                Me.Controls.Add(Me.btnOK)
                Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.Icon = Global.mRemoteNG.My.Resources.Resources.ActiveDirectory_Icon
                Me.Name = "ADImport"
                Me.TabText = "Active Directory Import"
                Me.Text = "Active Directory Import"
                Me.ResumeLayout(False)
                Me.PerformLayout()

            End Sub
#End Region

#Region "Public Methods"
            Public Sub New(ByVal Panel As DockContent)
                Me.WindowType = Type.ADImport
                Me.DockPnl = Panel
                Me.InitializeComponent()
            End Sub
#End Region

#Region "Form Stuff"
            Private Sub ADImport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
                ApplyLanguage()
            End Sub

            Private Sub ApplyLanguage()
                btnOK.Text = My.Resources.strButtonOK
                btnCancel.Text = My.Resources.strButtonCancel
                lblDomain.Text = My.Resources.strLabelDomain
                btnChangeDomain.Text = My.Resources.strButtonChange
            End Sub
#End Region

#Region "Public Properties"
            Private _ADPath As String
            Public Property ADPath() As String
                Get
                    Return _ADPath
                End Get
                Set(ByVal value As String)
                    _ADPath = value
                End Set
            End Property
#End Region

            Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
                Me._ADPath = Me.AD.ADPath
                mRemoteNG.Tree.Node.AddADNodes(Me._ADPath)
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End Sub

            Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
                Me._ADPath = ""
                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()
            End Sub

            Private Sub btnChangeDomain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangeDomain.Click
                Me.AD.Domain = txtDomain.Text
                Me.AD.Refresh()
            End Sub
        End Class
    End Namespace
End Namespace