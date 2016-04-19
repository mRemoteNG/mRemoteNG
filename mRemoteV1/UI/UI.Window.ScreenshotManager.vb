Imports System.ComponentModel
Imports System.Drawing.Imaging
Imports System.IO
Imports mRemote3G.App
Imports mRemote3G.Forms
Imports mRemote3G.Messages
Imports mRemote3G.My.Resources
Imports mRemote3G.Tools
Imports WeifenLuo.WinFormsUI.Docking

Namespace UI

    Namespace Window
        Public Class ScreenshotManager
            Inherits Base

#Region "Form Init"

            Friend WithEvents msMain As MenuStrip
            Friend WithEvents mMenFile As ToolStripMenuItem
            Friend WithEvents mMenFileSaveAll As ToolStripMenuItem
            Friend WithEvents mMenFileRemoveAll As ToolStripMenuItem
            Friend WithEvents cMenScreenshot As ContextMenuStrip
            Private components As IContainer
            Friend WithEvents cMenScreenshotCopy As ToolStripMenuItem
            Friend WithEvents cMenScreenshotSave As ToolStripMenuItem
            Friend WithEvents dlgSaveSingleImage As SaveFileDialog
            Friend WithEvents dlgSaveAllImages As FolderBrowserDialog
            Friend WithEvents flpScreenshots As FlowLayoutPanel

            Private Sub InitializeComponent()
                Me.components = New System.ComponentModel.Container
                Me.flpScreenshots = New FlowLayoutPanel
                Me.msMain = New MenuStrip
                Me.mMenFile = New ToolStripMenuItem
                Me.mMenFileSaveAll = New ToolStripMenuItem
                Me.mMenFileRemoveAll = New ToolStripMenuItem
                Me.cMenScreenshot = New ContextMenuStrip(Me.components)
                Me.cMenScreenshotCopy = New ToolStripMenuItem
                Me.cMenScreenshotSave = New ToolStripMenuItem
                Me.dlgSaveSingleImage = New SaveFileDialog
                Me.dlgSaveAllImages = New FolderBrowserDialog
                Me.msMain.SuspendLayout()
                Me.cMenScreenshot.SuspendLayout()
                Me.SuspendLayout()
                '
                'flpScreenshots
                '
                Me.flpScreenshots.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
                                                   Or AnchorStyles.Left) _
                                                  Or AnchorStyles.Right),
                                                 AnchorStyles)
                Me.flpScreenshots.AutoScroll = True
                Me.flpScreenshots.Location = New Point(0, 26)
                Me.flpScreenshots.Name = "flpScreenshots"
                Me.flpScreenshots.Size = New Size(542, 296)
                Me.flpScreenshots.TabIndex = 0
                '
                'msMain
                '
                Me.msMain.Font = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point,
                                          CType(0, Byte))
                Me.msMain.Items.AddRange(New ToolStripItem() {Me.mMenFile})
                Me.msMain.Location = New Point(0, 0)
                Me.msMain.Name = "msMain"
                Me.msMain.RenderMode = ToolStripRenderMode.Professional
                Me.msMain.Size = New Size(542, 24)
                Me.msMain.TabIndex = 1
                Me.msMain.Text = "MenuStrip1"
                '
                'mMenFile
                '
                Me.mMenFile.DropDownItems.AddRange(New ToolStripItem() {Me.mMenFileSaveAll, Me.mMenFileRemoveAll})
                Me.mMenFile.Image = Resources.File
                Me.mMenFile.Name = "mMenFile"
                Me.mMenFile.Size = New Size(51, 20)
                Me.mMenFile.Text = "&File"
                '
                'mMenFileSaveAll
                '
                Me.mMenFileSaveAll.Image = Screenshot_Save
                Me.mMenFileSaveAll.Name = "mMenFileSaveAll"
                Me.mMenFileSaveAll.Size = New Size(128, 22)
                Me.mMenFileSaveAll.Text = "Save All"
                '
                'mMenFileRemoveAll
                '
                Me.mMenFileRemoveAll.Image = Screenshot_Delete
                Me.mMenFileRemoveAll.Name = "mMenFileRemoveAll"
                Me.mMenFileRemoveAll.Size = New Size(128, 22)
                Me.mMenFileRemoveAll.Text = "Remove All"
                '
                'cMenScreenshot
                '
                Me.cMenScreenshot.Items.AddRange(New ToolStripItem() {Me.cMenScreenshotCopy, Me.cMenScreenshotSave})
                Me.cMenScreenshot.Name = "cMenScreenshot"
                Me.cMenScreenshot.Size = New Size(100, 48)
                '
                'cMenScreenshotCopy
                '
                Me.cMenScreenshotCopy.Image = Screenshot_Copy
                Me.cMenScreenshotCopy.Name = "cMenScreenshotCopy"
                Me.cMenScreenshotCopy.Size = New Size(99, 22)
                Me.cMenScreenshotCopy.Text = "Copy"
                '
                'cMenScreenshotSave
                '
                Me.cMenScreenshotSave.Image = Screenshot_Save
                Me.cMenScreenshotSave.Name = "cMenScreenshotSave"
                Me.cMenScreenshotSave.Size = New Size(99, 22)
                Me.cMenScreenshotSave.Text = "Save"
                '
                'dlgSaveSingleImage
                '
                Me.dlgSaveSingleImage.Filter =
                    "Graphics Interchange Format File (.gif)|*.gif|Joint Photographic Experts Group Fi" &
                    "le (.jpeg)|*.jpeg|Joint Photographic Experts Group File (.jpg)|*.jpg|Portable Ne" &
                    "twork Graphics File (.png)|*.png"
                Me.dlgSaveSingleImage.FilterIndex = 4
                '
                'ScreenshotManager
                '
                Me.ClientSize = New Size(542, 323)
                Me.Controls.Add(Me.flpScreenshots)
                Me.Controls.Add(Me.msMain)
                Me.HideOnClose = True
                Me.Icon = Screenshot_Icon
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

            Private Sub ScreenshotManager_Load(sender As Object, e As EventArgs) Handles Me.Load
                ApplyLanguage()
            End Sub

            Private Sub ApplyLanguage()
                mMenFile.Text = Language.Language.strMenuFile
                mMenFileSaveAll.Text = Language.Language.strSaveAll
                mMenFileRemoveAll.Text = Language.Language.strRemoveAll
                cMenScreenshotCopy.Text = Language.Language.strMenuCopy
                cMenScreenshotSave.Text = Language.Language.strSave
                dlgSaveSingleImage.Filter = Language.Language.strSaveImageFilter
                TabText = Language.Language.strScreenshots
                Text = Language.Language.strScreenshots
            End Sub

#End Region

#Region "Public Methods"

            Public Sub New(Panel As DockContent)
                Me.WindowType = Type.ScreenshotManager
                Me.DockPnl = Panel
                Me.InitializeComponent()
            End Sub

            Public Sub AddScreenshot(Screenshot As Image)
                Try
                    Dim nPB As New PictureBox
                    With nPB
                        AddHandler .MouseDown, AddressOf Me.pbScreenshot_MouseDown

                        .Parent = Me.flpScreenshots
                        .SizeMode = PictureBoxSizeMode.StretchImage
                        .BorderStyle = BorderStyle.FixedSingle
                        .ContextMenuStrip = Me.cMenScreenshot
                        .Image = Screenshot
                        .Size = New Size(100, 100) _
                        'New Size((Screenshot.Width / 100) * 20, (Screenshot.Height / 100) * 20)
                        .Show()
                    End With

                    Dim nBtn As New Button
                    With nBtn
                        AddHandler .Click, AddressOf btnCloseScreenshot_Click

                        .Parent = nPB
                        .FlatStyle = FlatStyle.Flat
                        .Text = "×"
                        .Size = New Size(22, 22)
                        .Location = New Point(nPB.Width - .Width, - 1)
                        .Show()
                    End With

                    Me.Show(frmMain.pnlDock)
                Catch ex As Exception
                   App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "AddScreenshot (UI.Window.ScreenshotManager) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

#End Region

#Region "Private Methods"

            Private Sub pbScreenshot_MouseDown(sender As Object, e As MouseEventArgs)
                Me.cMenScreenshot.Tag = sender

                If e.Button = MouseButtons.Left Then
                    Me.OpenScreenshot(sender)
                End If
            End Sub

            Private Sub pbScreenshotOpen_MouseDown(sender As Object, e As MouseEventArgs)
                If e.Button = MouseButtons.Left Then
                    Me.CloseOpenedScreenshot(sender.Parent)
                End If
            End Sub

            Private Sub CloseOpenedScreenshot(form As Form)
                form.Close()
            End Sub

            Private Sub OpenScreenshot(sender As PictureBox)
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
                    nForm.FormBorderStyle = FormBorderStyle.None

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
                   App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "OpenScreenshot (UI.Window.ScreenshotManager) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub btnCloseScreenshot_Click(sender As Object, e As EventArgs)
                Try
                    sender.Parent.Dispose()
                Catch ex As Exception
                   App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "btnCloseScreenshot_Click (UI.Window.ScreenshotManager) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub mMenFileRemoveAll_Click(sender As Object, e As EventArgs) Handles mMenFileRemoveAll.Click
                Me.RemoveAllImages()
            End Sub

            Private Sub RemoveAllImages()
                Me.flpScreenshots.Controls.Clear()
            End Sub

            Private Sub mMenFileSaveAll_Click(sender As Object, e As EventArgs) Handles mMenFileSaveAll.Click
                Me.SaveAllImages()
            End Sub

            Private Sub SaveAllImages()
                Try
                    Dim pCount = 1

                    If Me.dlgSaveAllImages.ShowDialog = DialogResult.OK Then
                        For Each fPath As String In _
                            Directory.GetFiles(Me.dlgSaveAllImages.SelectedPath, "Screenshot_*",
                                               SearchOption.TopDirectoryOnly)
                            Dim f As New FileInfo(fPath)

                            Dim fCount As String = f.Name
                            fCount = fCount.Replace(f.Extension, "")
                            fCount = fCount.Replace("Screenshot_", "")

                            pCount = fCount + 1
                        Next

                        For Each ctrl As Control In Me.flpScreenshots.Controls
                            If TypeOf ctrl Is PictureBox Then
                                TryCast(ctrl, PictureBox).Image.Save(
                                    Me.dlgSaveAllImages.SelectedPath & "\Screenshot_" & Misc.LeadingZero(pCount) &
                                    ".png", ImageFormat.Png)
                                pCount += 1
                            End If
                        Next
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        "SaveAllImages (UI.Window.ScreenshotManager) failed" & vbNewLine &
                                                        ex.ToString(), True)
                End Try
            End Sub

            Private Sub cMenScreenshotCopy_Click(sender As Object, e As EventArgs) Handles cMenScreenshotCopy.Click
                Me.CopyImageToClipboard()
            End Sub

            Private Sub CopyImageToClipboard()
                Try
                    Clipboard.SetImage(TryCast(cMenScreenshot.Tag, PictureBox).Image)
                Catch ex As Exception
                   App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "CopyImageToClipboard (UI.Window.ScreenshotManager) failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub cMenScreenshotSave_Click(sender As Object, e As EventArgs) Handles cMenScreenshotSave.Click
                Me.SaveSingleImage()
            End Sub

            Private Sub SaveSingleImage()
                Try
                    If Me.dlgSaveSingleImage.ShowDialog() = DialogResult.OK Then
                        Select Case _
                            LCase(
                                Me.dlgSaveSingleImage.FileName.Substring(
                                    Me.dlgSaveSingleImage.FileName.LastIndexOf(".") + 1))
                            Case "gif"
                                TryCast(Me.cMenScreenshot.Tag, PictureBox).Image.Save(Me.dlgSaveSingleImage.FileName,
                                                                                      ImageFormat.Gif)
                            Case "jpeg"
                                TryCast(Me.cMenScreenshot.Tag, PictureBox).Image.Save(Me.dlgSaveSingleImage.FileName,
                                                                                      ImageFormat.Jpeg)
                            Case "jpg"
                                TryCast(Me.cMenScreenshot.Tag, PictureBox).Image.Save(Me.dlgSaveSingleImage.FileName,
                                                                                      ImageFormat.Jpeg)
                            Case "png"
                                TryCast(Me.cMenScreenshot.Tag, PictureBox).Image.Save(Me.dlgSaveSingleImage.FileName,
                                                                                      ImageFormat.Png)
                        End Select
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        "SaveSingleImage (UI.Window.ScreenshotManager) failed" &
                                                        vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub mMenFile_DropDownOpening(sender As Object, e As EventArgs) Handles mMenFile.DropDownOpening
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