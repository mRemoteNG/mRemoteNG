Imports mRemote3G.App
Imports mRemote3G.App.Info
Imports mRemote3G.Connection
Imports mRemote3G.Forms
Imports mRemote3G.Messages
Imports mRemote3G.Tree

Namespace Tools
    Public Class Controls
        Public Class ComboBoxItem
            Private _Text As String

            Public Property Text As String
                Get
                    Return Me._Text
                End Get
                Set
                    Me._Text = value
                End Set
            End Property

            Private _Tag As Object

            Public Property Tag As Object
                Get
                    Return Me._Tag
                End Get
                Set
                    Me._Tag = value
                End Set
            End Property

            Public Sub New(Text As String, Optional ByVal Tag As Object = Nothing)
                Me._Text = Text
                If Tag IsNot Nothing Then
                    Me._Tag = Tag
                End If
            End Sub

            Public Overrides Function ToString() As String
                Return Me._Text
            End Function
        End Class


        Public Class NotificationAreaIcon
            Private ReadOnly _nI As NotifyIcon

            Private ReadOnly _cMen As ContextMenuStrip
            Private ReadOnly _cMenCons As ToolStripMenuItem
            Private ReadOnly _cMenSep1 As ToolStripSeparator
            Private ReadOnly _cMenExit As ToolStripMenuItem

            Private _Disposed As Boolean

            Public Property Disposed As Boolean
                Get
                    Return _Disposed
                End Get
                Set
                    _Disposed = value
                End Set
            End Property


            'Public Event MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
            'Public Event MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)


            Public Sub New()
                Try
                    Me._cMenCons = New ToolStripMenuItem
                    Me._cMenCons.Text = Language.Language.strConnections
                    Me._cMenCons.Image = My.Resources.Root

                    Me._cMenSep1 = New ToolStripSeparator

                    Me._cMenExit = New ToolStripMenuItem
                    Me._cMenExit.Text = Language.Language.strMenuExit
                    AddHandler Me._cMenExit.Click, AddressOf cMenExit_Click

                    Me._cMen = New ContextMenuStrip
                    Me._cMen.Font = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point,
                                             CType(0, Byte))
                    Me._cMen.RenderMode = ToolStripRenderMode.Professional
                    Me._cMen.Items.AddRange(New ToolStripItem() {Me._cMenCons, Me._cMenSep1, Me._cMenExit})

                    Me._nI = New NotifyIcon
                    Me._nI.Text = "mRemote"
                    Me._nI.BalloonTipText = "mRemote"
                    Me._nI.Icon = My.Resources.mRemote_Icon
                    Me._nI.ContextMenuStrip = Me._cMen
                    Me._nI.Visible = True

                    AddHandler Me._nI.MouseClick, AddressOf nI_MouseClick
                    AddHandler Me._nI.MouseDoubleClick, AddressOf nI_MouseDoubleClick
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        "Creating new SysTrayIcon failed" & vbNewLine & ex.ToString(),
                                                        True)
                End Try
            End Sub

            Public Sub Dispose()
                Try
                    Me._nI.Visible = False
                    Me._nI.Dispose()
                    Me._cMen.Dispose()
                    Me._Disposed = True
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        "Disposing SysTrayIcon failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub nI_MouseClick(sender As Object, e As MouseEventArgs)
                If e.Button = MouseButtons.Right Then
                    Me._cMenCons.DropDownItems.Clear()

                    For Each tNode As TreeNode In Runtime.Windows.treeForm.tvConnections.Nodes
                        AddNodeToMenu(tNode.Nodes, Me._cMenCons)
                    Next
                End If
            End Sub

            Private Sub AddNodeToMenu(tnc As TreeNodeCollection, menToolStrip As ToolStripMenuItem)
                Try
                    For Each tNode As TreeNode In tnc
                        Dim tMenItem As New ToolStripMenuItem()
                        tMenItem.Text = tNode.Text
                        tMenItem.Tag = tNode

                        If Node.GetNodeType(tNode) = Node.Type.Container Then
                            tMenItem.Image = My.Resources.Folder
                            tMenItem.Tag = tNode.Tag

                            menToolStrip.DropDownItems.Add(tMenItem)
                            AddNodeToMenu(tNode.Nodes, tMenItem)
                        ElseIf Node.GetNodeType(tNode) = Node.Type.Connection Or
                               Node.GetNodeType(tNode) = Node.Type.PuttySession Then
                            tMenItem.Image = Runtime.Windows.treeForm.imgListTree.Images(tNode.ImageIndex)
                            tMenItem.Tag = tNode.Tag

                            menToolStrip.DropDownItems.Add(tMenItem)
                        End If

                        AddHandler tMenItem.MouseUp, AddressOf ConMenItem_MouseUp
                    Next
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        "AddNodeToMenu failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub nI_MouseDoubleClick(sender As Object, e As MouseEventArgs)
                If frmMain.Visible = True Then
                    HideForm()
                Else
                    ShowForm()
                End If
            End Sub

            Private Sub ShowForm()
                frmMain.Show()
                frmMain.WindowState = frmMain.PreviousWindowState

                If My.Settings.ShowSystemTrayIcon = False Then
                    Runtime.NotificationAreaIcon.Dispose()
                    Runtime.NotificationAreaIcon = Nothing
                End If
            End Sub

            Private Sub HideForm()
                frmMain.Hide()
                frmMain.PreviousWindowState = frmMain.WindowState
            End Sub

            Private Sub ConMenItem_MouseUp(sender As Object, e As MouseEventArgs)
                If e.Button = MouseButtons.Left Then
                    If TypeOf sender.Tag Is Info Then
                        If frmMain.Visible = False Then ShowForm()
                        Runtime.OpenConnection(sender.Tag)
                    End If
                End If
            End Sub

            Private Sub cMenExit_Click(sender As Object, e As EventArgs)
                Runtime.Shutdown.Quit()
            End Sub
        End Class

        Public Shared Function ConnectionsSaveAsDialog() As SaveFileDialog
            Dim saveFileDialog As New SaveFileDialog()
            saveFileDialog.CheckPathExists = True
            saveFileDialog.InitialDirectory = Connections.DefaultConnectionsPath
            saveFileDialog.FileName = Connections.DefaultConnectionsFile
            saveFileDialog.OverwritePrompt = True

            saveFileDialog.Filter = Language.Language.strFiltermRemoteXML & "|*.xml|" & Language.Language.strFilterAll &
                                    "|*.*"

            Return saveFileDialog
        End Function

        Public Shared Function ConnectionsExportDialog() As SaveFileDialog
            Dim saveFileDialog As New SaveFileDialog()
            saveFileDialog.CheckPathExists = True
            saveFileDialog.InitialDirectory = Connections.DefaultConnectionsPath
            saveFileDialog.FileName = Connections.DefaultConnectionsFile
            saveFileDialog.OverwritePrompt = True

            saveFileDialog.Filter = Language.Language.strFiltermRemoteXML & "|*.xml|" &
                                    Language.Language.strFiltermRemoteCSV & "|*.csv|" &
                                    Language.Language.strFiltervRD2008CSV & "|*.csv|" & Language.Language.strFilterAll &
                                    "|*.*"

            Return saveFileDialog
        End Function

        Public Shared Function ConnectionsLoadDialog() As OpenFileDialog
            Dim lDlg As New OpenFileDialog()
            lDlg.CheckFileExists = True
            lDlg.InitialDirectory = Connections.DefaultConnectionsPath
            lDlg.Filter = Language.Language.strFiltermRemoteXML & "|*.xml|" & Language.Language.strFilterAll & "|*.*"

            Return lDlg
        End Function

        Public Shared Function ImportConnectionsRdpFileDialog() As OpenFileDialog
            Dim openFileDialog As New OpenFileDialog()
            openFileDialog.CheckFileExists = True
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
            openFileDialog.Filter = String.Join("|",
                                                {Language.Language.strFilterRDP, "*.rdp", Language.Language.strFilterAll,
                                                 "*.*"})
            openFileDialog.Multiselect = True
            Return openFileDialog
        End Function

        Public Class TreeNodeSorter
            Implements IComparer

            Public Property Sorting As SortOrder

            Public Sub New(Optional ByVal sortOrder As SortOrder = SortOrder.None)
                MyBase.New()
                Sorting = sortOrder
            End Sub

            Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
                Dim tx = CType(x, TreeNode)
                Dim ty = CType(y, TreeNode)

                Select Case Sorting
                    Case SortOrder.Ascending
                        Return String.Compare(tx.Text, ty.Text)
                    Case SortOrder.Descending
                        Return String.Compare(ty.Text, tx.Text)
                    Case Else
                        Return 0
                End Select
            End Function
        End Class

        Private Sub New()
        End Sub
    End Class
End Namespace
