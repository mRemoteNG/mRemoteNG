Imports mRemoteNG.App.Runtime

Namespace Connection
    Public Class QuickConnect
        Private Shared qBox As ToolStripComboBox = frmMain.cmbQuickConnect

        Public Class History
            Public Shared Function Exists(ByVal Text As String) As Boolean
                Try
                    For i As Integer = 0 To qBox.Items.Count - 1
                        If qBox.Items(i) = Text Then
                            Return True
                        End If
                    Next
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strQuickConnectHistoryExistsFailed & vbNewLine & ex.Message, True)
                End Try

                Return False
            End Function

            Public Shared Sub Add(ByVal Text As String)
                Try
                    qBox.Items.Insert(0, Text)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strQuickConnectAddFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub
        End Class
    End Class
End Namespace