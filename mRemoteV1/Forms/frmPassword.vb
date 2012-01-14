Public Class frmPassword
    Public ReadOnly Property Username() As String
        Get
            Return txtPassword.Text
        End Get
    End Property

    Public ReadOnly Property Password() As String
        Get
            If _Verify = True Then
                Return txtVerify.Text
            Else
                Return txtPassword.Text
            End If
        End Get
    End Property

    Private _Verify As Boolean = True
    Public Property Verify() As Boolean
        Get
            Return _Verify
        End Get
        Set(ByVal value As Boolean)
            _Verify = value
        End Set
    End Property

    Private _UserAndPass As Boolean = False



    Public Sub New(Optional ByVal UserAndPass As Boolean = False, Optional ByVal title As String = "Security")
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Text = title

        If UserAndPass = True Then
            _UserAndPass = True
            lblPassword.Text = "Username:"
            lblVerify.Text = "Password:"
            txtPassword.UseSystemPasswordChar = False
            txtVerify.UseSystemPasswordChar = True
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Verify = True And _UserAndPass = False Then
            If VerifyOK() Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
            End If
        Else
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
    End Sub

    Private Function VerifyOK() As Boolean
        If txtPassword.Text.Length >= 3 Then
            If txtPassword.Text = txtVerify.Text Then
                Return True
            Else
                lblStatus.Visible = True
                lblStatus.Text = "Passwords don't match!"
                Return False
            End If
        Else
            lblStatus.Visible = True
            lblStatus.Text = "3 characters is minimum!"
            Return False
        End If
    End Function

    Private Sub frmPassword_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ApplyLanguage()

        If Verify = False Then
            Me.Height = 124
            Me.lblVerify.Visible = False
            Me.txtVerify.Visible = False
        End If
    End Sub

    Private Sub ApplyLanguage()
        lblPassword.Text = My.Language.strLabelPassword
        lblVerify.Text = My.Language.strLabelVerify
        btnOK.Text = My.Language.strButtonOK
        btnCancel.Text = My.Language.strButtonCancel
        lblStatus.Text = "Status"
        Text = My.Language.strTitlePassword
    End Sub

    Private Sub txtPassword_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPassword.GotFocus
        If Me.txtPassword.TextLength > 0 Then
            Me.txtPassword.SelectionStart = 0
            Me.txtPassword.SelectionLength = Me.txtPassword.TextLength
        End If
    End Sub

    Private Sub txtVerify_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtVerify.GotFocus
        If Me.txtVerify.TextLength > 0 Then
            Me.txtVerify.SelectionStart = 0
            Me.txtVerify.SelectionLength = Me.txtVerify.TextLength
        End If
    End Sub
End Class