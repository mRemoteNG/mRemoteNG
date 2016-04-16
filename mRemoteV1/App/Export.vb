Imports mRemote3G.Config.Connections
Imports mRemote3G.Forms
Imports mRemote3G.Security
Imports mRemote3G.Tree

Namespace App
    Public Class Export
        Public Shared Sub ExportToFile(rootTreeNode As TreeNode, selectedTreeNode As TreeNode)
            Try
                Dim exportTreeNode As TreeNode
                Dim saveSecurity As New Save()

                Using exportForm As New ExportForm()
                    With exportForm
                        Select Case Node.GetNodeType(selectedTreeNode)
                            Case Node.Type.Container
                                .SelectedFolder = selectedTreeNode
                            Case Node.Type.Connection
                                If Node.GetNodeType(selectedTreeNode.Parent) = Node.Type.Container Then
                                    .SelectedFolder = selectedTreeNode.Parent
                                End If
                                .SelectedConnection = selectedTreeNode
                        End Select

                        If Not exportForm.ShowDialog(frmMain) = DialogResult.OK Then Return

                        Select Case .Scope
                            Case exportForm.ExportScope.SelectedFolder
                                exportTreeNode = .SelectedFolder
                            Case exportForm.ExportScope.SelectedConnection
                                exportTreeNode = .SelectedConnection
                            Case Else
                                exportTreeNode = rootTreeNode
                        End Select

                        saveSecurity.Username = .IncludeUsername
                        saveSecurity.Password = .IncludePassword
                        saveSecurity.Domain = .IncludeDomain
                        saveSecurity.Inheritance = .IncludeInheritance
                    End With

                    SaveExportFile(exportForm.FileName, exportForm.SaveFormat, exportTreeNode, saveSecurity)
                End Using
            Catch ex As Exception
                Runtime.MessageCollector.AddExceptionMessage("App.Export.ExportToFile() failed.", ex, , True)
            End Try
        End Sub

        Private Shared Sub SaveExportFile(ByVal fileName As String, ByVal saveFormat As Config.Connections.ConnectionsSave.Format, ByVal rootNode As TreeNode, ByVal saveSecurity As Security.Save)
            Dim previousTimerEnabled As Boolean = False

            Try
                If Runtime.TimerSqlWatcher IsNot Nothing Then
                    previousTimerEnabled = Runtime.TimerSqlWatcher.Enabled
                    Runtime.TimerSqlWatcher.Enabled = False
                End If

                Dim connectionsSave As New ConnectionsSave
                With connectionsSave
                    .Export = True
                    .ConnectionFileName = fileName
                    .SaveFormat = saveFormat

                    .ConnectionList = Runtime.ConnectionList
                    .ContainerList = Runtime.ContainerList
                    .RootTreeNode = rootNode

                    .SaveSecurity = saveSecurity
                End With

                connectionsSave.Save()
            Catch ex As Exception
                Runtime.MessageCollector.AddExceptionMessage(
                    String.Format("Export.SaveExportFile(""{0}"") failed.", fileName), ex)
            Finally
                If Runtime.TimerSqlWatcher IsNot Nothing Then
                    Runtime.TimerSqlWatcher.Enabled = previousTimerEnabled
                End If
            End Try
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace