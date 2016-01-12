Imports mRemoteNG.Forms
Imports mRemoteNG.App.Runtime

Namespace App
    Public Class Export
        Public Shared Sub ExportToFile(ByVal rootTreeNode As TreeNode, ByVal selectedTreeNode As TreeNode)
            Try
                Dim exportTreeNode As TreeNode
                Dim saveSecurity As New Security.Save()

                Using exportForm As New ExportForm()
                    With exportForm
                        Select Case Tree.Node.GetNodeType(selectedTreeNode)
                            Case Tree.Node.Type.Container
                                .SelectedFolder = selectedTreeNode
                            Case Tree.Node.Type.Connection
                                If Tree.Node.GetNodeType(selectedTreeNode.Parent) = Tree.Node.Type.Container Then
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
                MessageCollector.AddExceptionMessage("App.Export.ExportToFile() failed.", ex, , True)
            End Try
        End Sub

        Private Shared Sub SaveExportFile(ByVal fileName As String, ByVal saveFormat As Config.Connections.Save.Format, ByVal rootNode As TreeNode, ByVal saveSecurity As Security.Save)
            Dim previousTimerEnabled As Boolean = False

            Try
                If TimerSqlWatcher IsNot Nothing Then
                    previousTimerEnabled = TimerSqlWatcher.Enabled
                    TimerSqlWatcher.Enabled = False
                End If

                Dim connectionsSave As New Config.Connections.Save
                With connectionsSave
                    .Export = True
                    .ConnectionFileName = fileName
                    .SaveFormat = saveFormat

                    .ConnectionList = ConnectionList
                    .ContainerList = ContainerList
                    .RootTreeNode = rootNode

                    .SaveSecurity = saveSecurity
                End With

                connectionsSave.Save()
            Catch ex As Exception
                MessageCollector.AddExceptionMessage(String.Format("Export.SaveExportFile(""{0}"") failed.", fileName), ex)
            Finally
                If TimerSqlWatcher IsNot Nothing Then
                    TimerSqlWatcher.Enabled = previousTimerEnabled
                End If
            End Try
        End Sub
    End Class
End Namespace