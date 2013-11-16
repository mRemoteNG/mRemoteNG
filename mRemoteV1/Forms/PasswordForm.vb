Imports mRemoteNG.My

Namespace Forms
    Public Class PasswordForm
#Region "Public Properties"
        Public Property Verify As Boolean = True

        Public ReadOnly Property Password As String
            Get
                If Verify Then
                    Return txtVerify.Text
                Else
                    Return txtPassword.Text
                End If
            End Get
        End Property
#End Region


#Region "Constructors"
        Public Sub New(Optional ByVal passwordName As String = Nothing, Optional ByVal verify As Boolean = True)
            ' This call is required by the designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            _passwordName = passwordName
            Me.Verify = verify
        End Sub
#End Region

#Region "Event Handlers"
        Private Sub frmPassword_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            ApplyLanguage()

            If Not Verify Then
                Height = Height - (txtVerify.Top - txtPassword.Top)
                lblVerify.Visible = False
                txtVerify.Visible = False
            End If
        End Sub

        Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnCancel.Click
            DialogResult = DialogResult.Cancel
        End Sub

        Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnOK.Click
            If Verify Then
                If VerifyPassword() Then DialogResult = DialogResult.OK
            Else
                DialogResult = DialogResult.OK
            End If
        End Sub

        Private Sub txtPassword_TextChanged(sender As System.Object, e As EventArgs) Handles txtPassword.TextChanged, txtVerify.TextChanged
            HideStatus()
        End Sub
#End Region

#Region "Private Fields"
        Private ReadOnly _passwordName As String
#End Region

#Region "Private Methods"
        Private Sub ApplyLanguage()
            If String.IsNullOrEmpty(_passwordName) Then
                Text = Language.strTitlePassword
            Else
                Text = String.Format(Language.strTitlePasswordWithName, _passwordName)
            End If

            lblPassword.Text = Language.strLabelPassword
            lblVerify.Text = Language.strLabelVerify

            btnCancel.Text = Language.strButtonCancel
            btnOK.Text = Language.strButtonOK
        End Sub

        Private Function VerifyPassword() As Boolean
            If txtPassword.Text.Length >= 3 Then
                If txtPassword.Text = txtVerify.Text Then
                    Return True
                Else
                    ShowStatus(Language.strPasswordStatusMustMatch)
                    Return False
                End If
            Else
                ShowStatus(Language.strPasswordStatusTooShort)
                Return False
            End If
        End Function

        Private Sub ShowStatus(ByVal status As String)
            lblStatus.Visible = True
            lblStatus.Text = status
        End Sub

        Private Sub HideStatus()
            lblStatus.Visible = False
        End Sub
#End Region
    End Class
End Namespace