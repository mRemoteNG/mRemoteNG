Namespace Forms.OptionsPages
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ThemePage
        Inherits OptionsPage

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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ThemePage))
            Me.btnThemeDelete = New System.Windows.Forms.Button()
            Me.btnThemeNew = New System.Windows.Forms.Button()
            Me.cboTheme = New System.Windows.Forms.ComboBox()
            Me.ThemePropertyGrid = New System.Windows.Forms.PropertyGrid()
            Me.SuspendLayout()
            '
            'btnThemeDelete
            '
            Me.btnThemeDelete.Location = New System.Drawing.Point(535, 0)
            Me.btnThemeDelete.Name = "btnThemeDelete"
            Me.btnThemeDelete.Size = New System.Drawing.Size(75, 23)
            Me.btnThemeDelete.TabIndex = 6
            Me.btnThemeDelete.Text = "&Delete"
            Me.btnThemeDelete.UseVisualStyleBackColor = True
            '
            'btnThemeNew
            '
            Me.btnThemeNew.Location = New System.Drawing.Point(454, 0)
            Me.btnThemeNew.Name = "btnThemeNew"
            Me.btnThemeNew.Size = New System.Drawing.Size(75, 23)
            Me.btnThemeNew.TabIndex = 5
            Me.btnThemeNew.Text = "&New"
            Me.btnThemeNew.UseVisualStyleBackColor = True
            '
            'cboTheme
            '
            Me.cboTheme.FormattingEnabled = True
            Me.cboTheme.Location = New System.Drawing.Point(3, 1)
            Me.cboTheme.Name = "cboTheme"
            Me.cboTheme.Size = New System.Drawing.Size(445, 21)
            Me.cboTheme.TabIndex = 4
            '
            'ThemePropertyGrid
            '
            Me.ThemePropertyGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.ThemePropertyGrid.Location = New System.Drawing.Point(3, 29)
            Me.ThemePropertyGrid.Name = "ThemePropertyGrid"
            Me.ThemePropertyGrid.Size = New System.Drawing.Size(607, 460)
            Me.ThemePropertyGrid.TabIndex = 7
            Me.ThemePropertyGrid.UseCompatibleTextRendering = True
            '
            'ThemePage
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.btnThemeDelete)
            Me.Controls.Add(Me.btnThemeNew)
            Me.Controls.Add(Me.cboTheme)
            Me.Controls.Add(Me.ThemePropertyGrid)
            Me.Name = "ThemePage"
            Me.PageIcon = CType(resources.GetObject("$this.PageIcon"), System.Drawing.Icon)
            Me.Size = New System.Drawing.Size(610, 489)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents btnThemeDelete As System.Windows.Forms.Button
        Friend WithEvents btnThemeNew As System.Windows.Forms.Button
        Friend WithEvents cboTheme As System.Windows.Forms.ComboBox
        Friend WithEvents ThemePropertyGrid As System.Windows.Forms.PropertyGrid

    End Class
End Namespace