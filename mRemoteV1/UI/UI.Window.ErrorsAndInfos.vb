Imports System.ComponentModel
Imports System.Text
Imports mRemote3G.App
Imports mRemote3G.Forms
Imports mRemote3G.Messages
Imports mRemote3G.My.Resources
Imports WeifenLuo.WinFormsUI.Docking

Namespace UI

    Namespace Window
        Public Class ErrorsAndInfos
            Inherits Base

#Region "Form Init"

            Friend WithEvents pbError As PictureBox
            Friend WithEvents lblMsgDate As Label
            Friend WithEvents lvErrorCollector As ListView
            Friend WithEvents clmMessage As ColumnHeader
            Friend WithEvents txtMsgText As TextBox
            Friend WithEvents imgListMC As ImageList
            Private components As IContainer
            Friend WithEvents cMenMC As ContextMenuStrip
            Friend WithEvents cMenMCCopy As ToolStripMenuItem
            Friend WithEvents cMenMCDelete As ToolStripMenuItem
            Friend WithEvents pnlErrorMsg As Panel

            Private Sub InitializeComponent()
                Me.components = New System.ComponentModel.Container()
                Me.pnlErrorMsg = New Panel()
                Me.txtMsgText = New TextBox()
                Me.lblMsgDate = New Label()
                Me.pbError = New PictureBox()
                Me.lvErrorCollector = New ListView()
                Me.clmMessage = CType(New ColumnHeader(), ColumnHeader)
                Me.cMenMC = New ContextMenuStrip(Me.components)
                Me.cMenMCCopy = New ToolStripMenuItem()
                Me.cMenMCDelete = New ToolStripMenuItem()
                Me.imgListMC = New ImageList(Me.components)
                Me.pnlErrorMsg.SuspendLayout()
                CType(Me.pbError, ISupportInitialize).BeginInit()
                Me.cMenMC.SuspendLayout()
                Me.SuspendLayout()
                '
                'pnlErrorMsg
                '
                Me.pnlErrorMsg.Anchor = CType(((AnchorStyles.Bottom Or AnchorStyles.Left) _
                                               Or AnchorStyles.Right),
                                              AnchorStyles)
                Me.pnlErrorMsg.BackColor = SystemColors.Control
                Me.pnlErrorMsg.Controls.Add(Me.txtMsgText)
                Me.pnlErrorMsg.Controls.Add(Me.lblMsgDate)
                Me.pnlErrorMsg.Controls.Add(Me.pbError)
                Me.pnlErrorMsg.Location = New Point(0, 1)
                Me.pnlErrorMsg.Name = "pnlErrorMsg"
                Me.pnlErrorMsg.Size = New Size(198, 232)
                Me.pnlErrorMsg.TabIndex = 20
                '
                'txtMsgText
                '
                Me.txtMsgText.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
                                               Or AnchorStyles.Left) _
                                              Or AnchorStyles.Right),
                                             AnchorStyles)
                Me.txtMsgText.BorderStyle = BorderStyle.None
                Me.txtMsgText.Location = New Point(40, 20)
                Me.txtMsgText.Multiline = True
                Me.txtMsgText.Name = "txtMsgText"
                Me.txtMsgText.ReadOnly = True
                Me.txtMsgText.ScrollBars = ScrollBars.Vertical
                Me.txtMsgText.Size = New Size(158, 211)
                Me.txtMsgText.TabIndex = 30
                '
                'lblMsgDate
                '
                Me.lblMsgDate.Anchor = CType(((AnchorStyles.Top Or AnchorStyles.Left) _
                                              Or AnchorStyles.Right),
                                             AnchorStyles)
                Me.lblMsgDate.Font = New Font("Tahoma", 8.25!, FontStyle.Italic, GraphicsUnit.Point, CType(0, Byte))
                Me.lblMsgDate.Location = New Point(40, 5)
                Me.lblMsgDate.Name = "lblMsgDate"
                Me.lblMsgDate.Size = New Size(155, 13)
                Me.lblMsgDate.TabIndex = 40
                '
                'pbError
                '
                Me.pbError.InitialImage = Nothing
                Me.pbError.Location = New Point(2, 5)
                Me.pbError.Name = "pbError"
                Me.pbError.Size = New Size(32, 32)
                Me.pbError.TabIndex = 0
                Me.pbError.TabStop = False
                '
                'lvErrorCollector
                '
                Me.lvErrorCollector.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
                                                     Or AnchorStyles.Left) _
                                                    Or AnchorStyles.Right),
                                                   AnchorStyles)
                Me.lvErrorCollector.BorderStyle = BorderStyle.None
                Me.lvErrorCollector.Columns.AddRange(New ColumnHeader() {Me.clmMessage})
                Me.lvErrorCollector.ContextMenuStrip = Me.cMenMC
                Me.lvErrorCollector.FullRowSelect = True
                Me.lvErrorCollector.HeaderStyle = ColumnHeaderStyle.None
                Me.lvErrorCollector.HideSelection = False
                Me.lvErrorCollector.Location = New Point(203, 1)
                Me.lvErrorCollector.Name = "lvErrorCollector"
                Me.lvErrorCollector.ShowGroups = False
                Me.lvErrorCollector.Size = New Size(413, 232)
                Me.lvErrorCollector.SmallImageList = Me.imgListMC
                Me.lvErrorCollector.TabIndex = 10
                Me.lvErrorCollector.UseCompatibleStateImageBehavior = False
                Me.lvErrorCollector.View = System.Windows.Forms.View.Details
                '
                'clmMessage
                '
                Me.clmMessage.Text = Language.Language.strColumnMessage
                Me.clmMessage.Width = 184
                '
                'cMenMC
                '
                Me.cMenMC.Font = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point,
                                          CType(0, Byte))
                Me.cMenMC.Items.AddRange(New ToolStripItem() {Me.cMenMCCopy, Me.cMenMCDelete})
                Me.cMenMC.Name = "cMenMC"
                Me.cMenMC.RenderMode = ToolStripRenderMode.Professional
                Me.cMenMC.Size = New Size(153, 70)
                '
                'cMenMCCopy
                '
                Me.cMenMCCopy.Image = Copy
                Me.cMenMCCopy.Name = "cMenMCCopy"
                Me.cMenMCCopy.ShortcutKeys = CType((Keys.Control Or Keys.C), Keys)
                Me.cMenMCCopy.Size = New Size(152, 22)
                Me.cMenMCCopy.Text = Language.Language.strMenuCopy
                '
                'cMenMCDelete
                '
                Me.cMenMCDelete.Image = Delete
                Me.cMenMCDelete.Name = "cMenMCDelete"
                Me.cMenMCDelete.ShortcutKeys = Keys.Delete
                Me.cMenMCDelete.Size = New Size(152, 22)
                Me.cMenMCDelete.Text = Language.Language.strMenuDelete
                '
                'imgListMC
                '
                Me.imgListMC.ColorDepth = ColorDepth.Depth32Bit
                Me.imgListMC.ImageSize = New Size(16, 16)
                Me.imgListMC.TransparentColor = Color.Transparent
                '
                'ErrorsAndInfos
                '
                Me.ClientSize = New Size(617, 233)
                Me.Controls.Add(Me.lvErrorCollector)
                Me.Controls.Add(Me.pnlErrorMsg)
                Me.Font = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point, CType(0, Byte))
                Me.HideOnClose = True
                Me.Icon = Info_Icon
                Me.Name = "ErrorsAndInfos"
                Me.TabText = Language.Language.strMenuNotifications
                Me.Text = "Notifications"
                Me.pnlErrorMsg.ResumeLayout(False)
                Me.pnlErrorMsg.PerformLayout()
                CType(Me.pbError, ISupportInitialize).EndInit()
                Me.cMenMC.ResumeLayout(False)
                Me.ResumeLayout(False)
            End Sub

#End Region

#Region "Public Properties"

            Private _PreviousActiveForm As DockContent

            Public Property PreviousActiveForm As DockContent
                Get
                    Return Me._PreviousActiveForm
                End Get
                Set
                    Me._PreviousActiveForm = value
                End Set
            End Property

#End Region

#Region "Form Stuff"

            Private Sub ErrorsAndInfos_Load(sender As Object, e As EventArgs) Handles Me.Load
                ApplyLanguage()
            End Sub

            Private Sub ApplyLanguage()
                clmMessage.Text = Language.Language.strColumnMessage
                cMenMCCopy.Text = Language.Language.strMenuNotificationsCopyAll
                cMenMCDelete.Text = Language.Language.strMenuNotificationsDeleteAll
                TabText = Language.Language.strMenuNotifications
                Text = Language.Language.strMenuNotifications
            End Sub

#End Region

#Region "Public Methods"

            Public Sub New(Panel As DockContent)
                Me.WindowType = Type.ErrorsAndInfos
                Me.DockPnl = Panel
                Me.InitializeComponent()
                Me.LayoutVertical()
                Me.FillImageList()
            End Sub

#End Region

#Region "Private Methods"

            Private Sub FillImageList()
                Me.imgListMC.Images.Add(InformationSmall)
                Me.imgListMC.Images.Add(WarningSmall)
                Me.imgListMC.Images.Add(ErrorSmall)
            End Sub


            Private _Layout As ControlLayout = ControlLayout.Vertical

            Private Sub LayoutVertical()
                Try
                    Me.pnlErrorMsg.Location = New Point(0, Me.Height - 200)
                    Me.pnlErrorMsg.Size = New Size(Me.Width, Me.Height - Me.pnlErrorMsg.Top)
                    Me.pnlErrorMsg.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
                    Me.txtMsgText.Size = New Size(Me.pnlErrorMsg.Width - Me.pbError.Width - 8,
                                                  Me.pnlErrorMsg.Height - 20)
                    Me.lvErrorCollector.Location = New Point(0, 0)
                    Me.lvErrorCollector.Size = New Size(Me.Width, Me.Height - Me.pnlErrorMsg.Height - 5)
                    Me.lvErrorCollector.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right Or
                                                 AnchorStyles.Top

                    Me._Layout = ControlLayout.Vertical
                Catch ex As Exception
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "LayoutVertical (UI.Window.ErrorsAndInfos) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub LayoutHorizontal()
                Try
                    Me.pnlErrorMsg.Location = New Point(0, 0)
                    Me.pnlErrorMsg.Size = New Size(200, Me.Height)
                    Me.pnlErrorMsg.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Top
                    Me.txtMsgText.Size = New Size(Me.pnlErrorMsg.Width - Me.pbError.Width - 8,
                                                  Me.pnlErrorMsg.Height - 20)
                    Me.lvErrorCollector.Location = New Point(Me.pnlErrorMsg.Width + 5, 0)
                    Me.lvErrorCollector.Size = New Size(Me.Width - Me.pnlErrorMsg.Width - 5, Me.Height)
                    Me.lvErrorCollector.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right Or
                                                 AnchorStyles.Top

                    Me._Layout = ControlLayout.Horizontal
                Catch ex As Exception
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "LayoutHorizontal (UI.Window.ErrorsAndInfos) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub ErrorsAndInfos_Resize(sender As Object, e As EventArgs) Handles Me.Resize
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
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "ErrorsAndInfos_Resize (UI.Window.ErrorsAndInfos) failed" & vbNewLine & ex.ToString(), True)
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
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "pnlErrorMsg_ResetDefaultStyle (UI.Window.ErrorsAndInfos) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub MC_KeyDown(sender As Object, e As KeyEventArgs) _
                Handles lvErrorCollector.KeyDown, txtMsgText.KeyDown
                Try
                    If e.KeyCode = Keys.Escape Then
                        Try
                            If Me._PreviousActiveForm IsNot Nothing Then
                                Me._PreviousActiveForm.Show(frmMain.pnlDock)
                            Else
                                Runtime.Windows.treeForm.Show(frmMain.pnlDock)
                            End If
                        Catch ex As Exception
                        End Try
                    End If
                Catch ex As Exception
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "MC_KeyDown (UI.Window.ErrorsAndInfos) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub lvErrorCollector_SelectedIndexChanged(sender As Object, e As EventArgs) _
                Handles lvErrorCollector.SelectedIndexChanged
                Try
                    If Me.lvErrorCollector.SelectedItems.Count = 0 Or Me.lvErrorCollector.SelectedItems.Count > 1 Then
                        Me.pnlErrorMsg_ResetDefaultStyle()
                        Exit Sub
                    End If

                    Dim sItem As ListViewItem = Me.lvErrorCollector.SelectedItems(0)
                    Dim eMsg As Message = sItem.Tag
                    Select Case eMsg.MsgClass
                        Case MessageClass.InformationMsg
                            Me.pbError.Image = Information
                            Me.pnlErrorMsg.BackColor = Color.LightSteelBlue
                            Me.txtMsgText.BackColor = Color.LightSteelBlue
                        Case MessageClass.WarningMsg
                            Me.pbError.Image = Warning
                            Me.pnlErrorMsg.BackColor = Color.Gold
                            Me.txtMsgText.BackColor = Color.Gold
                        Case MessageClass.ErrorMsg
                            Me.pbError.Image = _Error
                            Me.pnlErrorMsg.BackColor = Color.IndianRed
                            Me.txtMsgText.BackColor = Color.IndianRed
                    End Select

                    Me.lblMsgDate.Text = eMsg.MsgDate
                    Me.txtMsgText.Text = eMsg.MsgText
                Catch ex As Exception
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "lvErrorCollector_SelectedIndexChanged (UI.Window.ErrorsAndInfos) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub cMenMC_Opening(sender As Object, e As CancelEventArgs) Handles cMenMC.Opening
                If lvErrorCollector.Items.Count Then
                    cMenMCCopy.Enabled = True
                    cMenMCDelete.Enabled = True
                Else
                    cMenMCCopy.Enabled = False
                    cMenMCDelete.Enabled = False
                End If

                If lvErrorCollector.SelectedItems.Count Then
                    cMenMCCopy.Text = Language.Language.strMenuCopy
                    cMenMCDelete.Text = Language.Language.strMenuNotificationsDelete
                Else
                    cMenMCCopy.Text = Language.Language.strMenuNotificationsCopyAll
                    cMenMCDelete.Text = Language.Language.strMenuNotificationsDeleteAll
                End If
            End Sub

            Private Sub cMenMCCopy_Click(sender As Object, e As EventArgs) Handles cMenMCCopy.Click
                CopyMessagesToClipboard()
            End Sub

            Private Sub CopyMessagesToClipboard()
                Try
                    Dim items As IEnumerable
                    If lvErrorCollector.SelectedItems.Count Then
                        items = lvErrorCollector.SelectedItems
                    Else
                        items = lvErrorCollector.Items
                    End If

                    Dim stringBuilder As New StringBuilder
                    stringBuilder.AppendLine("----------")

                    lvErrorCollector.BeginUpdate()

                    Dim message As Message
                    For Each item As ListViewItem In items
                        message = TryCast(item.Tag, Message)
                        If message Is Nothing Then Continue For

                        stringBuilder.AppendLine(message.MsgClass.ToString)
                        stringBuilder.AppendLine(message.MsgDate)
                        stringBuilder.AppendLine(message.MsgText)
                        stringBuilder.AppendLine("----------")
                    Next

                    Clipboard.SetText(stringBuilder.ToString)
                Catch ex As Exception
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "UI.Window.ErrorsAndInfos.CopyMessagesToClipboard() failed." & vbNewLine & ex.ToString(), True)
                Finally
                    lvErrorCollector.EndUpdate()
                End Try
            End Sub

            Private Sub cMenMCDelete_Click(sender As Object, e As EventArgs) Handles cMenMCDelete.Click
                DeleteMessages()
            End Sub

            Private Sub DeleteMessages()
                Try
                    lvErrorCollector.BeginUpdate()

                    If lvErrorCollector.SelectedItems.Count Then
                        For Each item As ListViewItem In lvErrorCollector.SelectedItems
                            item.Remove()
                        Next
                    Else
                        lvErrorCollector.Items.Clear()
                    End If
                Catch ex As Exception
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "UI.Window.ErrorsAndInfos.DeleteMessages() failed" & vbNewLine & ex.ToString(), True)
                Finally
                    lvErrorCollector.EndUpdate()
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