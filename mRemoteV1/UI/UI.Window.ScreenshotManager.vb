Imports WeifenLuo.WinFormsUI.Docking
Imports mRemoteNG.App.Runtime
Imports System.IO

Namespace UI
    Namespace Window
        Public Class ScreenshotManager
            Inherits UI.Window.Base

#Region "Form Init"
            Friend WithEvents msMain As System.Windows.Forms.MenuStrip
            Friend WithEvents mMenFile As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents mMenFileSaveAll As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents mMenFileRemoveAll As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenScreenshot As System.Windows.Forms.ContextMenuStrip
            Private components As System.ComponentModel.IContainer
            Friend WithEvents cMenScreenshotCopy As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenScreenshotSave As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents dlgSaveSingleImage As System.Windows.Forms.SaveFileDialog
            Friend WithEvents dlgSaveAllImages As System.Windows.Forms.FolderBrowserDialog
            Friend WithEvents flpScreenshots As System.Windows.Forms.FlowLayoutPanel

            Private Sub InitializeComponent()
                Me.components = New System.ComponentModel.Container
                Me.flpScreenshots = New System.Windows.Forms.FlowLayoutPanel
                Me.msMain = New System.Windows.Forms.MenuStrip
                Me.mMenFile = New System.Windows.Forms.ToolStripMenuItem
                Me.mMenFileSaveAll = New System.Windows.Forms.ToolStripMenuItem
                Me.mMenFileRemoveAll = New System.Windows.Forms.ToolStripMenuItem
                Me.cMenScreenshot = New System.Windows.Forms.ContextMenuStrip(Me.components)
                Me.cMenScreenshotCopy = New System.Windows.Forms.ToolStripMenuItem
                Me.cMenScreenshotSave = New System.Windows.Forms.ToolStripMenuItem
                Me.dlgSaveSingleImage = New System.Windows.Forms.SaveFileDialog
                Me.dlgSaveAllImages = New System.Windows.Forms.FolderBrowserDialog
                Me.msMain.SuspendLayout()
                Me.cMenScreenshot.SuspendLayout()
                Me.SuspendLayout()
                '
                'flpScreenshots
                '
                Me.flpScreenshots.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.flpScreenshots.AutoScroll = True
                Me.flpScreenshots.Location = New System.Drawing.Point(0, 26)
                Me.flpScreenshots.Name = "flpScreenshots"
                Me.flpScreenshots.Size = New System.Drawing.Size(542, 296)
                Me.flpScreenshots.TabIndex = 0
                '
                'msMain
                '
                Me.msMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.msMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mMenFile})
                Me.msMain.Location = New System.Drawing.Point(0, 0)
                Me.msMain.Name = "msMain"
                Me.msMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
                Me.msMain.Size = New System.Drawing.Size(542, 24)
                Me.msMain.TabIndex = 1
                Me.msMain.Text = "MenuStrip1"
                '
                'mMenFile
                '
                Me.mMenFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mMenFileSaveAll, Me.mMenFileRemoveAll})
                Me.mMenFile.Image = Global.mRemoteNG.My.Resources.Resources.File
                Me.mMenFile.Name = "mMenFile"
                Me.mMenFile.Size = New System.Drawing.Size(51, 20)
                Me.mMenFile.Text = "&File"
                '
                'mMenFileSaveAll
                '
                Me.mMenFileSaveAll.Image = Global.mRemoteNG.My.Resources.Resources.Screenshot_Save
                Me.mMenFileSaveAll.Name = "mMenFileSaveAll"
                Me.mMenFileSaveAll.Size = New System.Drawing.Size(128, 22)
                Me.mMenFileSaveAll.Text = "Save All"
                '
                'mMenFileRemoveAll
                '
                Me.mMenFileRemoveAll.Image = Global.mRemoteNG.My.Resources.Resources.Screenshot_Delete
                Me.mMenFileRemoveAll.Name = "mMenFileRemoveAll"
                Me.mMenFileRemoveAll.Size = New System.Drawing.Size(128, 22)
                Me.mMenFileRemoveAll.Text = "Remove All"
                '
                'cMenScreenshot
                '
                Me.cMenScreenshot.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cMenScreenshotCopy, Me.cMenScreenshotSave})
                Me.cMenScreenshot.Name = "cMenScreenshot"
                Me.cMenScreenshot.Size = New System.Drawing.Size(100, 48)
                '
                'cMenScreenshotCopy
                '
                Me.cMenScreenshotCopy.Image = Global.mRemoteNG.My.Resources.Resources.Screenshot_Copy
                Me.cMenScreenshotCopy.Name = "cMenScreenshotCopy"
                Me.cMenScreenshotCopy.Size = New System.Drawing.Size(99, 22)
                Me.cMenScreenshotCopy.Text = "Copy"
                '
                'cMenScreenshotSave
                '
                Me.cMenScreenshotSave.Image = Global.mRemoteNG.My.Resources.Resources.Screenshot_Save
                Me.cMenScreenshotSave.Name = "cMenScreenshotSave"
                Me.cMenScreenshotSave.Size = New System.Drawing.Size(99, 22)
                Me.cMenScreenshotSave.Text = "Save"
                '
                'dlgSaveSingleImage
                '
                Me.dlgSaveSingleImage.Filter = "Graphics Interchange Format File (.gif)|*.gif|Joint Photographic Experts Group Fi" & _
                    "le (.jpeg)|*.jpeg|Joint Photographic Experts Group File (.jpg)|*.jpg|Portable Ne" & _
                    "twork Graphics File (.png)|*.png"
                Me.dlgSaveSingleImage.FilterIndex = 4
                '
                'ScreenshotManager
                '
                Me.ClientSize = New System.Drawing.Size(542, 323)
                Me.Controls.Add(Me.flpScreenshots)
                Me.Controls.Add(Me.msMain)
                Me.HideOnClose = True
                Me.Icon = Global.mRemoteNG.My.Resources.Resources.Screenshot_Icon
                Me.MainMenuStrip = Me.msMain
                Me.Name = "ScreenshotManager"
                Me.TabText = "Screenshots"
                Me.Text = "Screenshots"
                Me.msMain.ResumeLayout(False)
                Me.msMain.PerformLayout()
                Me.cMenScreenshot.ResumeLayout(False)
                Me.ResumeLayout(False)
                Me.PerformLayout()

            End Sub
#End Region

#Region "Form Stuff"
            Private Sub ScreenshotManager_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
                ApplyLanguage()
            End Sub

            Private Sub ApplyLanguage()
                mMenFile.Text = My.Resources.strMenuFile
                mMenFileSaveAll.Text = My.Resources.strSaveAll
                mMenFileRemoveAll.Text = My.Resources.strRemoveAll
                cMenScreenshotCopy.Text = My.Resources.strMenuCopy
                cMenScreenshotSave.Text = My.Resources.strSave
                dlgSaveSingleImage.Filter = My.Resources.strSaveImageFilter
                TabText = My.Resources.strScreenshots
                Text = My.Resources.strScreenshots
            End Sub
#End Region

#Region "Public Methods"
            Public Sub New(ByVal Panel As DockContent)
                Me.WindowType = Type.ScreenshotManager
                Me.DockPnl = Panel
                Me.InitializeComponent()
            End Sub

            Public Sub AddScreenshot(ByVal Screenshot As Image)
                Try
                    Dim nPB As New PictureBox
                    With nPB
                        AddHandler .MouseDown, AddressOf Me.pbScreenshot_MouseDown

                        .Parent = Me.flpScreenshots
                        .SizeMode = PictureBoxSizeMode.StretchImage
                        .BorderStyle = BorderStyle.FixedSingle
                        .ContextMenuStrip = Me.cMenScreenshot
                        .Image = Screenshot
                        .Size = New Size(100, 100) 'New Size((Screenshot.Width / 100) * 20, (Screenshot.Height / 100) * 20)
                        .Show()
                    End With

                    Dim nBtn As New Button
                    With nBtn
                        AddHandler .Click, AddressOf btnCloseScreenshot_Click

                        .Parent = nPB
                        .FlatStyle = FlatStyle.Flat
                        .Text = "×"
                        .Size = New Size(22, 22)
                        .Location = New Point(nPB.Width - .Width, -1)
                        .Show()
                    End With

                    Me.Show(frmMain.pnlDock)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "AddScreenshot (UI.Window.ScreenshotManager) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Private Methods"
            Private Sub pbScreenshot_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
                Me.cMenScreenshot.Tag = sender

                If e.Button = System.Windows.Forms.MouseButtons.Left Then
                    Me.OpenScreenshot(sender)
                End If
            End Sub

            Private Sub pbScreenshotOpen_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
                If e.Button = System.Windows.Forms.MouseButtons.Left Then
                    Me.CloseOpenedScreenshot(sender.Parent)
                End If
            End Sub

            Private Sub CloseOpenedScreenshot(ByVal form As Form)
                form.Close()
            End Sub

            Private Sub OpenScreenshot(ByVal sender As PictureBox)
                Try
                    Dim mImage As Image = sender.Image

                    Dim nForm As New Form()
                    nForm.StartPosition = FormStartPosition.CenterParent
                    nForm.ShowInTaskbar = False
                    nForm.ShowIcon = False
                    nForm.MaximizeBox = False
                    nForm.MinimizeBox = False
                    nForm.Width = mImage.Width + 2
                    nForm.Height = mImage.Height + 2
                    nForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None

                    Dim nPB As New PictureBox
                    nPB.Parent = nForm
                    nPB.BorderStyle = BorderStyle.FixedSingle
                    nPB.Location = New Point(0, 0)
                    nPB.SizeMode = PictureBoxSizeMode.AutoSize
                    nPB.Anchor = AnchorStyles.Left Or AnchorStyles.Bottom Or AnchorStyles.Right Or AnchorStyles.Top
                    nPB.Image = mImage
                    nPB.ContextMenuStrip = Me.cMenScreenshot
                    nPB.Show()

                    AddHandler nPB.MouseDown, AddressOf Me.pbScreenshotOpen_MouseDown

                    nForm.ShowDialog()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "OpenScreenshot (UI.Window.ScreenshotManager) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub btnCloseScreenshot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
                Try
                    sender.Parent.Dispose()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "btnCloseScreenshot_Click (UI.Window.ScreenshotManager) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub mMenFileRemoveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenFileRemoveAll.Click
                Me.RemoveAllImages()
            End Sub

            Private Sub RemoveAllImages()
                Me.flpScreenshots.Controls.Clear()
            End Sub

            Private Sub mMenFileSaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenFileSaveAll.Click
                Me.SaveAllImages()
            End Sub

            Private Sub SaveAllImages()
                Try
                    Dim pCount As Integer = 1

                    If Me.dlgSaveAllImages.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                        For Each fPath As String In Directory.GetFiles(Me.dlgSaveAllImages.SelectedPath, "Screenshot_*", SearchOption.TopDirectoryOnly)
                            Dim f As New FileInfo(fPath)

                            Dim fCount As String = f.Name
                            fCount = fCount.Replace(f.Extension, "")
                            fCount = fCount.Replace("Screenshot_", "")

                            pCount = fCount + 1
                        Next

                        For Each ctrl As System.Windows.Forms.Control In Me.flpScreenshots.Controls
                            If TypeOf ctrl Is PictureBox Then
                                TryCast(ctrl, PictureBox).Image.Save(Me.dlgSaveAllImages.SelectedPath & "\Screenshot_" & Tools.Misc.LeadingZero(pCount) & ".png", System.Drawing.Imaging.ImageFormat.Png)
                                pCount += 1
                            End If
                        Next
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SaveAllImages (UI.Window.ScreenshotManager) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub cMenScreenshotCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenScreenshotCopy.Click
                Me.CopyImageToClipboard()
            End Sub

            Private Sub CopyImageToClipboard()
                Try
                    Clipboard.SetImage(TryCast(cMenScreenshot.Tag, PictureBox).Image)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "CopyImageToClipboard (UI.Window.ScreenshotManager) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub cMenScreenshotSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenScreenshotSave.Click
                Me.SaveSingleImage()
            End Sub

            Private Sub SaveSingleImage()
                Try
                    If Me.dlgSaveSingleImage.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                        Select Case LCase(Me.dlgSaveSingleImage.FileName.Substring(Me.dlgSaveSingleImage.FileName.LastIndexOf(".") + 1))
                            Case "gif"
                                TryCast(Me.cMenScreenshot.Tag, PictureBox).Image.Save(Me.dlgSaveSingleImage.FileName, Imaging.ImageFormat.Gif)
                            Case "jpeg"
                                TryCast(Me.cMenScreenshot.Tag, PictureBox).Image.Save(Me.dlgSaveSingleImage.FileName, Imaging.ImageFormat.Jpeg)
                            Case "jpg"
                                TryCast(Me.cMenScreenshot.Tag, PictureBox).Image.Save(Me.dlgSaveSingleImage.FileName, Imaging.ImageFormat.Jpeg)
                            Case "png"
                                TryCast(Me.cMenScreenshot.Tag, PictureBox).Image.Save(Me.dlgSaveSingleImage.FileName, Imaging.ImageFormat.Png)
                        End Select
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SaveSingleImage (UI.Window.ScreenshotManager) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub mMenFile_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles mMenFile.DropDownOpening
                If Me.flpScreenshots.Controls.Count < 1 Then
                    Me.mMenFileSaveAll.Enabled = False
                    Me.mMenFileRemoveAll.Enabled = False
                Else
                    Me.mMenFileSaveAll.Enabled = True
                    Me.mMenFileRemoveAll.Enabled = True
                End If
            End Sub
#End Region

        End Class
    End Namespace
End Namespace