<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucTextbox
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
        Me.txtValue = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txtValue
        '
        Me.txtValue.AcceptsReturn = True
        Me.txtValue.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtValue.Location = New System.Drawing.Point(0, 0)
        Me.txtValue.Multiline = True
        Me.txtValue.Name = "txtValue"
        Me.txtValue.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtValue.Size = New System.Drawing.Size(222, 178)
        Me.txtValue.TabIndex = 0
        '
        'ucTextbox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.txtValue)
        Me.Name = "ucTextbox"
        Me.Size = New System.Drawing.Size(222, 178)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtValue As System.Windows.Forms.TextBox

End Class
