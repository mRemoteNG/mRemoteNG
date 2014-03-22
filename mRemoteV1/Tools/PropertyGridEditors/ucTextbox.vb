Public Class ucTextbox
    Inherits UserControl


    Public Property IsOk As Boolean = True

    Public Property Text As String
        Get
            Return txtValue.Text
        End Get
        Set(value As String)
            txtValue.Text = value
        End Set
    End Property


    Private Sub ucTextbox_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles txtValue.PreviewKeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                Me.IsOk = False
            Case Keys.Enter

        End Select

    End Sub

End Class
