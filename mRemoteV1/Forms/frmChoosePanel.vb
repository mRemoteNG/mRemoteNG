Imports mRemoteNG.App.Runtime

Public Class frmChoosePanel
    Public Property Panel() As String
        Get
            Return cbPanels.SelectedItem.ToString
        End Get
        Set(ByVal value As String)
            cbPanels.SelectedItem = value
        End Set
    End Property

    Private Sub frmChoosePanel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ApplyLanguage()

        AddAvailablePanels()
    End Sub

    Private Sub ApplyLanguage()
        btnOK.Text = My.Resources.strButtonOK
        lblDescription.Text = My.Resources.strLabelSelectPanel
        btnNew.Text = My.Resources.strButtonNew
        btnCancel.Text = My.Resources.strButtonCancel
        Text = My.Resources.strTitleSelectPanel
    End Sub

    Private Sub AddAvailablePanels()
        cbPanels.Items.Clear()

        For i As Integer = 0 To WindowList.Count - 1
            cbPanels.Items.Add(WindowList(i).Text.Replace("&&", "&"))
        Next

        If cbPanels.Items.Count > 0 Then
            cbPanels.SelectedItem = cbPanels.Items(0)
            cbPanels.Enabled = True
            btnOK.Enabled = True
        Else
            cbPanels.Enabled = False
            btnOK.Enabled = False
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Dim pnlName As String = InputBox(My.Resources.strPanelName & ":", My.Resources.strNewPanel, My.Resources.strNewPanel)

        If pnlName <> "" Then
            AddPanel(pnlName)
            AddAvailablePanels()
            cbPanels.SelectedItem = pnlName
            cbPanels.Focus()
        End If
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub
End Class