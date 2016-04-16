Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmChoosePanel
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
            Me.cbPanels = New System.Windows.Forms.ComboBox
            Me.btnOK = New System.Windows.Forms.Button
            Me.lblDescription = New System.Windows.Forms.Label
            Me.btnNew = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.SuspendLayout()
            '
            'cbPanels
            '
            Me.cbPanels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cbPanels.FormattingEnabled = True
            Me.cbPanels.Location = New System.Drawing.Point(73, 48)
            Me.cbPanels.Name = "cbPanels"
            Me.cbPanels.Size = New System.Drawing.Size(168, 21)
            Me.cbPanels.TabIndex = 10
            '
            'btnOK
            '
            Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnOK.Location = New System.Drawing.Point(97, 85)
            Me.btnOK.Name = "btnOK"
            Me.btnOK.Size = New System.Drawing.Size(69, 23)
            Me.btnOK.TabIndex = 20
            Me.btnOK.Text = Language.Language.strButtonOK
            Me.btnOK.UseVisualStyleBackColor = True
            '
            'lblDescription
            '
            Me.lblDescription.Location = New System.Drawing.Point(7, 8)
            Me.lblDescription.Name = "lblDescription"
            Me.lblDescription.Size = New System.Drawing.Size(234, 29)
            Me.lblDescription.TabIndex = 0
            Me.lblDescription.Text = Language.Language.strLabelSelectPanel
            '
            'btnNew
            '
            Me.btnNew.Image = Global.mRemote3G.My.Resources.Resources.Panel_Add
            Me.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnNew.Location = New System.Drawing.Point(10, 45)
            Me.btnNew.Name = "btnNew"
            Me.btnNew.Size = New System.Drawing.Size(57, 27)
            Me.btnNew.TabIndex = 40
            Me.btnNew.Text = Language.Language.strButtonNew
            Me.btnNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnNew.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(172, 85)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(69, 23)
            Me.btnCancel.TabIndex = 30
            Me.btnCancel.Text = Language.Language.strButtonCancel
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'frmChoosePanel
            '
            Me.AcceptButton = Me.btnOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(253, 120)
            Me.ControlBox = False
            Me.Controls.Add(Me.lblDescription)
            Me.Controls.Add(Me.btnNew)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOK)
            Me.Controls.Add(Me.cbPanels)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.Icon = Global.mRemote3G.My.Resources.Resources.Panels_Icon
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmChoosePanel"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = Language.Language.strTitleSelectPanel
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents cbPanels As System.Windows.Forms.ComboBox
        Friend WithEvents btnOK As System.Windows.Forms.Button
        Friend WithEvents lblDescription As System.Windows.Forms.Label
        Friend WithEvents btnNew As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
    End Class
End NameSpace