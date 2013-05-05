Imports System.Windows.Forms
Imports mRemoteNG.App.Runtime

Namespace Tools
    Public Class Controls
        Public Class ComboBoxItem
            Private _Text As String
            Public Property Text() As String
                Get
                    Return Me._Text
                End Get
                Set(ByVal value As String)
                    Me._Text = value
                End Set
            End Property

            Private _Tag As Object
            Public Property Tag() As Object
                Get
                    Return Me._Tag
                End Get
                Set(ByVal value As Object)
                    Me._Tag = value
                End Set
            End Property

            Public Sub New(ByVal Text As String, Optional ByVal Tag As Object = Nothing)
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
            Private _nI As NotifyIcon

            Private _cMen As ContextMenuStrip
            Private _cMenCons As ToolStripMenuItem
            Private _cMenSep1 As ToolStripSeparator
            Private _cMenExit As ToolStripMenuItem

            Private _Disposed As Boolean
            Public Property Disposed() As Boolean
                Get
                    Return _Disposed
                End Get
                Set(ByVal value As Boolean)
                    _Disposed = value
                End Set
            End Property


            'Public Event MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
            'Public Event MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)


            Public Sub New()
                Try
                    Me._cMenCons = New ToolStripMenuItem
                    Me._cMenCons.Text = My.Language.strConnections
                    Me._cMenCons.Image = My.Resources.Root

                    Me._cMenSep1 = New ToolStripSeparator

                    Me._cMenExit = New ToolStripMenuItem
                    Me._cMenExit.Text = My.Language.strMenuExit
                    AddHandler Me._cMenExit.Click, AddressOf cMenExit_Click

                    Me._cMen = New ContextMenuStrip
                    Me._cMen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                    Me._cMen.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
                    Me._cMen.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._cMenCons, Me._cMenSep1, Me._cMenExit})

                    Me._nI = New NotifyIcon
                    Me._nI.Text = "mRemote"
                    Me._nI.BalloonTipText = "mRemote"
                    Me._nI.Icon = My.Resources.mRemote_Icon
                    Me._nI.ContextMenuStrip = Me._cMen
                    Me._nI.Visible = True

                    AddHandler Me._nI.MouseClick, AddressOf nI_MouseClick
                    AddHandler Me._nI.MouseDoubleClick, AddressOf nI_MouseDoubleClick
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Creating new SysTrayIcon failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Sub Dispose()
                Try
                    Me._nI.Visible = False
                    Me._nI.Dispose()
                    Me._cMen.Dispose()
                    Me._Disposed = True
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Disposing SysTrayIcon failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub nI_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
                If e.Button = MouseButtons.Right Then
                    Me._cMenCons.DropDownItems.Clear()

                    For Each tNode As TreeNode In App.Runtime.Windows.treeForm.tvConnections.Nodes
                        AddNodeToMenu(tNode.Nodes, Me._cMenCons)
                    Next
                End If
            End Sub

            Private Sub AddNodeToMenu(ByVal tnc As TreeNodeCollection, ByVal menToolStrip As ToolStripMenuItem)
                Try
                    For Each tNode As TreeNode In tnc
                        Dim tMenItem As New ToolStripMenuItem()
                        tMenItem.Text = tNode.Text
                        tMenItem.Tag = tNode

                        If Tree.Node.GetNodeType(tNode) = Tree.Node.Type.Container Then
                            tMenItem.Image = My.Resources.Folder
                            tMenItem.Tag = tNode.Tag

                            menToolStrip.DropDownItems.Add(tMenItem)
                            AddNodeToMenu(tNode.Nodes, tMenItem)
                        Else
                            tMenItem.Image = My.Resources.Play
                            tMenItem.Tag = tNode.Tag

                            menToolStrip.DropDownItems.Add(tMenItem)
                        End If

                        AddHandler tMenItem.MouseDown, AddressOf ConMenItem_MouseDown
                    Next
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "AddNodeToMenu failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub nI_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
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
                    App.Runtime.NotificationAreaIcon.Dispose()
                    App.Runtime.NotificationAreaIcon = Nothing
                End If
            End Sub

            Private Sub HideForm()
                frmMain.Hide()
                frmMain.PreviousWindowState = frmMain.WindowState
            End Sub

            Private Sub ConMenItem_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
                If e.Button = MouseButtons.Left Then
                    If TypeOf sender.Tag Is mRemoteNG.Connection.Info Then
                        If frmMain.Visible = False Then
                            ShowForm()
                        End If
                        App.Runtime.OpenConnection(sender.Tag)
                    End If
                End If
            End Sub

            Private Sub cMenExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
                App.Runtime.Shutdown.Quit()
            End Sub
        End Class

        Public Shared Function ConnectionsSaveAsDialog() As SaveFileDialog
            Dim saveFileDialog As New SaveFileDialog()
            saveFileDialog.CheckPathExists = True
            saveFileDialog.InitialDirectory = App.Info.Connections.DefaultConnectionsPath
            saveFileDialog.FileName = App.Info.Connections.DefaultConnectionsFile
            saveFileDialog.OverwritePrompt = True

            saveFileDialog.Filter = My.Language.strFiltermRemoteXML & "|*.xml|" & My.Language.strFilterAll & "|*.*"

            Return saveFileDialog
        End Function

        Public Shared Function ConnectionsExportDialog() As SaveFileDialog
            Dim saveFileDialog As New SaveFileDialog()
            saveFileDialog.CheckPathExists = True
            saveFileDialog.InitialDirectory = App.Info.Connections.DefaultConnectionsPath
            saveFileDialog.FileName = App.Info.Connections.DefaultConnectionsFile
            saveFileDialog.OverwritePrompt = True

            saveFileDialog.Filter = My.Language.strFiltermRemoteXML & "|*.xml|" & My.Language.strFiltermRemoteCSV & "|*.csv|" & My.Language.strFiltervRD2008CSV & "|*.csv|" & My.Language.strFilterAll & "|*.*"

            Return saveFileDialog
        End Function

        Public Shared Function ConnectionsLoadDialog() As OpenFileDialog
            Dim lDlg As New OpenFileDialog()
            lDlg.CheckFileExists = True
            lDlg.InitialDirectory = App.Info.Connections.DefaultConnectionsPath
            lDlg.Filter = My.Language.strFiltermRemoteXML & "|*.xml|" & My.Language.strFilterAll & "|*.*"

            Return lDlg
        End Function

        Public Shared Function ImportConnectionsRdpFileDialog() As OpenFileDialog
            Dim openFileDialog As New OpenFileDialog()
            openFileDialog.CheckFileExists = True
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
            openFileDialog.Filter = String.Join("|", {My.Language.strFilterRDP, "*.rdp", My.Language.strFilterAll, "*.*"})
            openFileDialog.Multiselect = True
            Return openFileDialog
        End Function

        Public Class TreeNodeSorter
            Implements IComparer

            Public Property Sorting As System.Windows.Forms.SortOrder

            Public Sub New(Optional ByVal sortOrder As SortOrder = SortOrder.None)
                MyBase.New()
                Sorting = sortOrder
            End Sub

            Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
                Dim tx As TreeNode = CType(x, TreeNode)
                Dim ty As TreeNode = CType(y, TreeNode)

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
    End Class
End Namespace
