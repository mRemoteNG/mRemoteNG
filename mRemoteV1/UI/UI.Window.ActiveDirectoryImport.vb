Imports mRemote3G.App
Imports WeifenLuo.WinFormsUI.Docking

Namespace UI

    Namespace Window
        Public Class ActiveDirectoryImport
            Inherits Base

#Region "Constructors"

            Public Sub New(panel As DockContent)
                InitializeComponent()

                Runtime.FontOverride(Me)

                WindowType = Type.ActiveDirectoryImport
                DockPnl = panel
            End Sub

#End Region

#Region "Private Methods"

#Region "Event Handlers"

            Private Sub ADImport_Load(sender As Object, e As EventArgs) Handles Me.Load
                ApplyLanguage()

                txtDomain.Text = ActiveDirectoryTree.Domain
                EnableDisableImportButton()
            End Sub

            Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
                Import.ImportFromActiveDirectory(ActiveDirectoryTree.ADPath)
                DialogResult = DialogResult.OK
                Close()
            End Sub

            Private Shared Sub txtDomain_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) _
                Handles txtDomain.PreviewKeyDown
                If e.KeyCode = Keys.Enter Then e.IsInputKey = True
            End Sub

            Private Sub txtDomain_KeyDown(sender As Object, e As KeyEventArgs) Handles txtDomain.KeyDown
                If e.KeyCode = Keys.Enter Then
                    ChangeDomain()
                    e.SuppressKeyPress = True
                End If
            End Sub

            Private Sub btnChangeDomain_Click(sender As Object, e As EventArgs) Handles btnChangeDomain.Click
                ChangeDomain()
            End Sub

            Private Sub ActiveDirectoryTree_ADPathChanged(sender As Object) Handles ActiveDirectoryTree.ADPathChanged
                EnableDisableImportButton()
            End Sub

#End Region

            Private Sub ApplyLanguage()
                btnImport.Text = Language.Language.strButtonImport
                lblDomain.Text = Language.Language.strLabelDomain
                btnChangeDomain.Text = Language.Language.strButtonChange
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