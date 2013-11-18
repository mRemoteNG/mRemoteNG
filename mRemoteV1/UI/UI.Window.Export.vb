Imports WeifenLuo.WinFormsUI.Docking
Imports mRemoteNG.App.Runtime

Namespace UI
    Namespace Window
        Public Class Export
            Inherits Base
#Region "Public Properties"
            Public Property TreeNode() As TreeNode
#End Region

#Region "Constructors"
            Public Sub New(ByVal panel As DockContent, Optional ByVal treeNode As TreeNode = Nothing)
                InitializeComponent()

                WindowType = Type.Export
                DockPnl = panel

                Me.TreeNode = treeNode
            End Sub
#End Region

#Region "Private Methods"
#Region "Event Handlers"
            Private Sub Export_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
                ApplyLanguage()
            End Sub

            Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnCancel.Click
                Close()
            End Sub

            Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnOK.Click
                Try
                    If TreeNode Is Nothing Then TreeNode = Windows.treeForm.tvConnections.Nodes(0)

                    Dim saveSecurity As New Security.Save()

                    saveSecurity.Username = lvSecurity.Items(0).Checked
                    saveSecurity.Password = lvSecurity.Items(1).Checked
                    saveSecurity.Domain = lvSecurity.Items(2).Checked
                    saveSecurity.Inheritance = lvSecurity.Items(3).Checked

                    SaveConnectionsAs(TreeNode, saveSecurity)

                    Close()
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage("UI.Window.Export.btnOK_Click() failed.", ex, , True)
                End Try
            End Sub
#End Region

            Private Sub ApplyLanguage()
                Text = My.Language.strExport
                TabText = My.Language.strExport

                lblUncheckProperties.Text = My.Language.strUncheckProperties
                lblMremoteXMLOnly.Text = My.Language.strPropertiesWillOnlyBeSavedMRemoteXML

                lvSecurity.Items(0).Text = My.Language.strCheckboxUsername
                lvSecurity.Items(1).Text = My.Language.strCheckboxPassword
                lvSecurity.Items(2).Text = My.Language.strCheckboxDomain
                lvSecurity.Items(3).Text = My.Language.strCheckboxInheritance

                btnOK.Text = My.Language.strButtonOK
                btnCancel.Text = My.Language.strButtonCancel
            End Sub
#End Region

        End Class
    End Namespace
End Namespace