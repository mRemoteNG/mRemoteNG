Imports WeifenLuo.WinFormsUI.Docking
Imports mRemoteNG.App.Runtime

Namespace UI
    Namespace Window
        Public Class ErrorsAndInfos
            Inherits UI.Window.Base

#Region "Form Init"
            Friend WithEvents pbError As System.Windows.Forms.PictureBox
            Friend WithEvents lblMsgDate As System.Windows.Forms.Label
            Friend WithEvents lvErrorCollector As System.Windows.Forms.ListView
            Friend WithEvents clmMessage As System.Windows.Forms.ColumnHeader
            Friend WithEvents txtMsgText As System.Windows.Forms.TextBox
            Friend WithEvents imgListMC As System.Windows.Forms.ImageList
            Private components As System.ComponentModel.IContainer
            Friend WithEvents cMenMC As System.Windows.Forms.ContextMenuStrip
            Friend WithEvents cMenMCCopy As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenMCDelete As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents pnlErrorMsg As System.Windows.Forms.Panel
            Private Sub InitializeComponent()
                Me.components = New System.ComponentModel.Container
                Me.pnlErrorMsg = New System.Windows.Forms.Panel
                Me.txtMsgText = New System.Windows.Forms.TextBox
                Me.lblMsgDate = New System.Windows.Forms.Label
                Me.pbError = New System.Windows.Forms.PictureBox
                Me.lvErrorCollector = New System.Windows.Forms.ListView
                Me.clmMessage = New System.Windows.Forms.ColumnHeader
                Me.cMenMC = New System.Windows.Forms.ContextMenuStrip(Me.components)
                Me.cMenMCCopy = New System.Windows.Forms.ToolStripMenuItem
                Me.cMenMCDelete = New System.Windows.Forms.ToolStripMenuItem
                Me.imgListMC = New System.Windows.Forms.ImageList(Me.components)
                Me.pnlErrorMsg.SuspendLayout()
                CType(Me.pbError, System.ComponentModel.ISupportInitialize).BeginInit()
                Me.cMenMC.SuspendLayout()
                Me.SuspendLayout()
                '
                'pnlErrorMsg
                '
                Me.pnlErrorMsg.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlErrorMsg.BackColor = System.Drawing.SystemColors.Control
                Me.pnlErrorMsg.Controls.Add(Me.txtMsgText)
                Me.pnlErrorMsg.Controls.Add(Me.lblMsgDate)
                Me.pnlErrorMsg.Controls.Add(Me.pbError)
                Me.pnlErrorMsg.Location = New System.Drawing.Point(0, 1)
                Me.pnlErrorMsg.Name = "pnlErrorMsg"
                Me.pnlErrorMsg.Size = New System.Drawing.Size(198, 232)
                Me.pnlErrorMsg.TabIndex = 20
                '
                'txtMsgText
                '
                Me.txtMsgText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.txtMsgText.BorderStyle = System.Windows.Forms.BorderStyle.None
                Me.txtMsgText.Location = New System.Drawing.Point(40, 20)
                Me.txtMsgText.Multiline = True
                Me.txtMsgText.Name = "txtMsgText"
                Me.txtMsgText.ReadOnly = True
                Me.txtMsgText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
                Me.txtMsgText.Size = New System.Drawing.Size(158, 211)
                Me.txtMsgText.TabIndex = 30
                '
                'lblMsgDate
                '
                Me.lblMsgDate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.lblMsgDate.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblMsgDate.Location = New System.Drawing.Point(40, 5)
                Me.lblMsgDate.Name = "lblMsgDate"
                Me.lblMsgDate.Size = New System.Drawing.Size(155, 13)
                Me.lblMsgDate.TabIndex = 40
                '
                'pbError
                '
                Me.pbError.InitialImage = Nothing
                Me.pbError.Location = New System.Drawing.Point(2, 5)
                Me.pbError.Name = "pbError"
                Me.pbError.Size = New System.Drawing.Size(32, 32)
                Me.pbError.TabIndex = 0
                Me.pbError.TabStop = False
                '
                'lvErrorCollector
                '
                Me.lvErrorCollector.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.lvErrorCollector.BorderStyle = System.Windows.Forms.BorderStyle.None
                Me.lvErrorCollector.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.clmMessage})
                Me.lvErrorCollector.ContextMenuStrip = Me.cMenMC
                Me.lvErrorCollector.FullRowSelect = True
                Me.lvErrorCollector.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
                Me.lvErrorCollector.HideSelection = False
                Me.lvErrorCollector.Location = New System.Drawing.Point(203, 1)
                Me.lvErrorCollector.Name = "lvErrorCollector"
                Me.lvErrorCollector.ShowGroups = False
                Me.lvErrorCollector.Size = New System.Drawing.Size(413, 232)
                Me.lvErrorCollector.SmallImageList = Me.imgListMC
                Me.lvErrorCollector.TabIndex = 10
                Me.lvErrorCollector.UseCompatibleStateImageBehavior = False
                Me.lvErrorCollector.View = System.Windows.Forms.View.Details
                '
                'clmMessage
                '
                Me.clmMessage.Text = My.Resources.strColumnMessage
                Me.clmMessage.Width = 184
                '
                'cMenMC
                '
                Me.cMenMC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.cMenMC.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cMenMCCopy, Me.cMenMCDelete})
                Me.cMenMC.Name = "cMenMC"
                Me.cMenMC.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
                Me.cMenMC.Size = New System.Drawing.Size(137, 48)
                '
                'cMenMCCopy
                '
                Me.cMenMCCopy.Image = Global.mRemoteNG.My.Resources.Resources.Copy
                Me.cMenMCCopy.Name = "cMenMCCopy"
                Me.cMenMCCopy.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
                Me.cMenMCCopy.Size = New System.Drawing.Size(136, 22)
                Me.cMenMCCopy.Text = My.Resources.strMenuCopy
                '
                'cMenMCDelete
                '
                Me.cMenMCDelete.Image = Global.mRemoteNG.My.Resources.Resources.Delete
                Me.cMenMCDelete.Name = "cMenMCDelete"
                Me.cMenMCDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete
                Me.cMenMCDelete.Size = New System.Drawing.Size(136, 22)
                Me.cMenMCDelete.Text = My.Resources.strMenuDelete
                '
                'imgListMC
                '
                Me.imgListMC.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
                Me.imgListMC.ImageSize = New System.Drawing.Size(16, 16)
                Me.imgListMC.TransparentColor = System.Drawing.Color.Transparent
                '
                'ErrorsAndInfos
                '
                Me.ClientSize = New System.Drawing.Size(617, 233)
                Me.Controls.Add(Me.lvErrorCollector)
                Me.Controls.Add(Me.pnlErrorMsg)
                Me.HideOnClose = True
                Me.Icon = Global.mRemoteNG.My.Resources.Resources.Info_Icon
                Me.Name = "ErrorsAndInfos"
                Me.TabText = My.Resources.strMenuNotifications
                Me.Text = My.Resources.strMenuNotifications
                Me.pnlErrorMsg.ResumeLayout(False)
                Me.pnlErrorMsg.PerformLayout()
                CType(Me.pbError, System.ComponentModel.ISupportInitialize).EndInit()
                Me.cMenMC.ResumeLayout(False)
                Me.ResumeLayout(False)

            End Sub
#End Region

#Region "Public Properties"
            Private _PreviousActiveForm As DockContent
            Public Property PreviousActiveForm() As DockContent
                Get
                    Return Me._PreviousActiveForm
                End Get
                Set(ByVal value As DockContent)
                    Me._PreviousActiveForm = value
                End Set
            End Property
#End Region

#Region "Form Stuff"
            Private Sub ErrorsAndInfos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
                ApplyLanguage()
            End Sub

            Private Sub ApplyLanguage()
                clmMessage.Text = My.Resources.strColumnMessage
                cMenMCCopy.Text = My.Resources.strMenuCopy
                cMenMCDelete.Text = My.Resources.strMenuDelete
                TabText = My.Resources.strMenuNotifications
                Text = My.Resources.strMenuNotifications
            End Sub
#End Region

#Region "Public Methods"
            Public Sub New(ByVal Panel As DockContent)
                Me.WindowType = Type.ErrorsAndInfos
                Me.DockPnl = Panel
                Me.InitializeComponent()
                Me.LayoutVertical()
                Me.FillImageList()
            End Sub
#End Region

#Region "Private Methods"
            Private Sub FillImageList()
                Me.imgListMC.Images.Add(My.Resources.InformationSmall)
                Me.imgListMC.Images.Add(My.Resources.WarningSmall)
                Me.imgListMC.Images.Add(My.Resources.ErrorSmall)
            End Sub


            Private _Layout As ControlLayout = ControlLayout.Vertical

            Private Sub LayoutVertical()
                Try
                    Me.pnlErrorMsg.Location = New Point(0, Me.Height - 200)
                    Me.pnlErrorMsg.Size = New Size(Me.Width, Me.Height - Me.pnlErrorMsg.Top)
                    Me.pnlErrorMsg.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
                    Me.txtMsgText.Size = New Size(Me.pnlErrorMsg.Width - Me.pbError.Width - 8, Me.pnlErrorMsg.Height - 20)
                    Me.lvErrorCollector.Location = New Point(0, 0)
                    Me.lvErrorCollector.Size = New Size(Me.Width, Me.Height - Me.pnlErrorMsg.Height - 5)
                    Me.lvErrorCollector.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top

                    Me._Layout = ControlLayout.Vertical
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "LayoutVertical (UI.Window.ErrorsAndInfos) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub LayoutHorizontal()
                Try
                    Me.pnlErrorMsg.Location = New Point(0, 0)
                    Me.pnlErrorMsg.Size = New Size(200, Me.Height)
                    Me.pnlErrorMsg.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Top
                    Me.txtMsgText.Size = New Size(Me.pnlErrorMsg.Width - Me.pbError.Width - 8, Me.pnlErrorMsg.Height - 20)
                    Me.lvErrorCollector.Location = New Point(Me.pnlErrorMsg.Width + 5, 0)
                    Me.lvErrorCollector.Size = New Size(Me.Width - Me.pnlErrorMsg.Width - 5, Me.Height)
                    Me.lvErrorCollector.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top

                    Me._Layout = ControlLayout.Horizontal
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "LayoutHorizontal (UI.Window.ErrorsAndInfos) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub ErrorsAndInfos_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
                Try
                    If Me.Width > Me.Height Then
                        If Me._Layout = ControlLayout.Vertical Then
                            Me.LayoutHorizontal()
                        End If
                    Else
                        If Me._Layout = ControlLayout.Horizontal Then
                            Me.LayoutVertical()
                        End If
                    End If

                    Me.lvErrorCollector.Columns(0).Width = Me.lvErrorCollector.Width - 20
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "ErrorsAndInfos_Resize (UI.Window.ErrorsAndInfos) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub pnlErrorMsg_ResetDefaultStyle()
                Try
                    Me.pnlErrorMsg.BackColor = Color.FromKnownColor(KnownColor.Control)
                    Me.pbError.Image = Nothing
                    Me.txtMsgText.Text = ""
                    Me.txtMsgText.BackColor = Color.FromKnownColor(KnownColor.Control)
                    Me.lblMsgDate.Text = ""
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "pnlErrorMsg_ResetDefaultStyle (UI.Window.ErrorsAndInfos) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub MC_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lvErrorCollector.KeyDown, txtMsgText.KeyDown
                Try
                    If e.KeyCode = Keys.Escape Then
                        Try
                            If Me._PreviousActiveForm IsNot Nothing Then
                                Me._PreviousActiveForm.Show(frmMain.pnlDock)
                            Else
                                App.Runtime.Windows.treeForm.Show(frmMain.pnlDock)
                            End If
                        Catch ex As Exception
                        End Try
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "MC_KeyDown (UI.Window.ErrorsAndInfos) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub lvErrorCollector_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvErrorCollector.SelectedIndexChanged
                Try
                    If Me.lvErrorCollector.SelectedItems.Count = 0 Or Me.lvErrorCollector.SelectedItems.Count > 1 Then
                        Me.pnlErrorMsg_ResetDefaultStyle()
                        Exit Sub
                    End If

                    Dim sItem As ListViewItem = Me.lvErrorCollector.SelectedItems(0)
                    Dim eMsg As Messages.Message = sItem.Tag
                    Select Case eMsg.MsgClass
                        Case Messages.MessageClass.InformationMsg
                            Me.pbError.Image = My.Resources.Information
                            Me.pnlErrorMsg.BackColor = Color.LightSteelBlue
                            Me.txtMsgText.BackColor = Color.LightSteelBlue
                        Case Messages.MessageClass.WarningMsg
                            Me.pbError.Image = My.Resources.Warning
                            Me.pnlErrorMsg.BackColor = Color.Gold
                            Me.txtMsgText.BackColor = Color.Gold
                        Case Messages.MessageClass.ErrorMsg
                            Me.pbError.Image = My.Resources._Error
                            Me.pnlErrorMsg.BackColor = Color.IndianRed
                            Me.txtMsgText.BackColor = Color.IndianRed
                    End Select

                    Me.lblMsgDate.Text = eMsg.MsgDate
                    Me.txtMsgText.Text = eMsg.MsgText
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "lvErrorCollector_SelectedIndexChanged (UI.Window.ErrorsAndInfos) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub cMenMCCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenMCCopy.Click
                Me.CopyMessageToClipboard()
            End Sub

            Private Sub CopyMessageToClipboard()
                Try
                    If Me.lvErrorCollector.SelectedItems.Count > 0 Then
                        Dim strCopyText As String
                        strCopyText = "----------" & vbNewLine

                        For Each lvItem As ListViewItem In Me.lvErrorCollector.SelectedItems
                            strCopyText &= TryCast(lvItem.Tag, Messages.Message).MsgClass.ToString & vbNewLine
                            strCopyText &= TryCast(lvItem.Tag, Messages.Message).MsgDate & vbNewLine
                            strCopyText &= TryCast(lvItem.Tag, Messages.Message).MsgText & vbNewLine
                            strCopyText &= "----------" & vbNewLine
                        Next

                        Clipboard.SetText(strCopyText)
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "CopyMessageToClipboard (UI.Window.ErrorsAndInfos) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub cMenMCDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenMCDelete.Click
                Me.DeleteMessages()
            End Sub

            Private Sub DeleteMessages()
                Try
                    If Me.lvErrorCollector.SelectedItems.Count > 0 Then
                        For Each lvItem As ListViewItem In Me.lvErrorCollector.SelectedItems
                            lvItem.Remove()
                        Next
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "DeleteMessages (UI.Window.ErrorsAndInfos) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

            Public Enum ControlLayout
                Vertical = 0
                Horizontal = 1
            End Enum
        End Class
    End Namespace
End Namespace