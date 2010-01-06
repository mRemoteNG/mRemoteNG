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



    Public Sub New(Optional ByVal UserAndPass As Boolean = False, Optional ByVal Title As String = "Security")
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Text = Title

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
        If Verify = False Then
            Me.Height = 124
            Me.lblVerify.Visible = False
            Me.txtVerify.Visible = False
        End If
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


    Private Sub txtPassword_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPassword.TextChanged
        If txtPassword.Text = "ijustwannaplay" Then
            pbLock.Visible = False
            btnOK.Visible = False
            btnCancel.Visible = False
            txtPassword.Visible = False
            txtVerify.Visible = False
            lblPassword.Visible = False
            lblStatus.Visible = False
            lblVerify.Visible = False
            AcceptButton = Nothing
            CancelButton = Nothing
            BackColor = Color.DimGray
            Me.Text = "SnakeFX Lite"
            pnlImage.Top = (Me.ClientSize.Height / 2) - (Me.pnlImage.Height / 2)
            lblTips.Visible = True

            Easter.Snake.Game.CreatePicBox(pnlImage)
            Easter.Snake.Game.Mode = Easter.Snake.Game.GameMode.Welcome
            Easter.Snake.Game.SetupGame()
            txtSnake.Focus()
        End If
    End Sub

    Private Sub txtSnake_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSnake.KeyDown
        Easter.Snake.Game.CheckKeyPress(e)
    End Sub
End Class