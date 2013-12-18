Imports WeifenLuo.WinFormsUI.Docking
Imports mRemoteNG.App

Namespace UI
    Namespace Window
        Public Class ActiveDirectoryImport
            Inherits Base
#Region "Constructors"
            Public Sub New(ByVal panel As DockContent)
                InitializeComponent()

                Runtime.FontOverride(Me)

                WindowType = Type.ActiveDirectoryImport
                DockPnl = panel
            End Sub
#End Region

#Region "Private Methods"
#Region "Event Handlers"
            Private Sub ADImport_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
                ApplyLanguage()

                txtDomain.Text = ActiveDirectoryTree.Domain
                EnableDisableImportButton()
            End Sub

            Private Sub btnImport_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnImport.Click
                Import.ImportFromActiveDirectory(ActiveDirectoryTree.ADPath)
                DialogResult = DialogResult.OK
                Close()
            End Sub

            Private Shared Sub txtDomain_PreviewKeyDown(sender As System.Object, e As PreviewKeyDownEventArgs) Handles txtDomain.PreviewKeyDown
                If e.KeyCode = Keys.Enter Then e.IsInputKey = True
            End Sub

            Private Sub txtDomain_KeyDown(sender As System.Object, e As KeyEventArgs) Handles txtDomain.KeyDown
                If e.KeyCode = Keys.Enter Then
                    ChangeDomain()
                    e.SuppressKeyPress = True
                End If
            End Sub

            Private Sub btnChangeDomain_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnChangeDomain.Click
                ChangeDomain()
            End Sub

            Private Sub ActiveDirectoryTree_ADPathChanged(ByVal sender As Object) Handles ActiveDirectoryTree.ADPathChanged
                EnableDisableImportButton()
            End Sub
#End Region

            Private Sub ApplyLanguage()
                btnImport.Text = My.Language.strButtonImport
                lblDomain.Text = My.Language.strLabelDomain
                btnChangeDomain.Text = My.Language.strButtonChange
            End Sub

            Private Sub ChangeDomain()
                ActiveDirectoryTree.Domain = txtDomain.Text
                ActiveDirectoryTree.Refresh()
            End Sub

            Private Sub EnableDisableImportButton()
                btnImport.Enabled = Not String.IsNullOrEmpty(ActiveDirectoryTree.ADPath)
            End Sub
#End Region
        End Class
    End Namespace
End Namespace