Imports WeifenLuo.WinFormsUI.Docking
Imports System.IO
Imports mRemoteNG.App.Runtime

Namespace UI
    Namespace Window
        Public Class About
            Inherits UI.Window.Base

#Region "Form Init"
            Friend WithEvents lblCopyright As System.Windows.Forms.Label
            Friend WithEvents lblTitle As System.Windows.Forms.Label
            Friend WithEvents lblVersion As System.Windows.Forms.Label
            Friend WithEvents lblLicense As System.Windows.Forms.Label
            Friend WithEvents txtChangeLog As System.Windows.Forms.TextBox
            Friend WithEvents lblChangeLog As System.Windows.Forms.Label
            Friend WithEvents pnlBottom As System.Windows.Forms.Panel
            Friend WithEvents pbLogo As System.Windows.Forms.PictureBox
            Friend WithEvents lblEdition As System.Windows.Forms.Label
            Friend WithEvents llblFAMFAMFAM As System.Windows.Forms.LinkLabel
            Friend WithEvents llblMagicLibrary As System.Windows.Forms.LinkLabel
            Friend WithEvents llblWeifenLuo As System.Windows.Forms.LinkLabel
            Friend WithEvents pnlTop As System.Windows.Forms.Panel

            Private Sub InitializeComponent()
                Me.pnlTop = New System.Windows.Forms.Panel
                Me.lblEdition = New System.Windows.Forms.Label
                Me.pbLogo = New System.Windows.Forms.PictureBox
                Me.pnlBottom = New System.Windows.Forms.Panel
                Me.llblWeifenLuo = New System.Windows.Forms.LinkLabel
                Me.llblMagicLibrary = New System.Windows.Forms.LinkLabel
                Me.llblFAMFAMFAM = New System.Windows.Forms.LinkLabel
                Me.txtChangeLog = New System.Windows.Forms.TextBox
                Me.lblTitle = New System.Windows.Forms.Label
                Me.lblVersion = New System.Windows.Forms.Label
                Me.lblChangeLog = New System.Windows.Forms.Label
                Me.lblLicense = New System.Windows.Forms.Label
                Me.lblCopyright = New System.Windows.Forms.Label
                Me.pnlTop.SuspendLayout()
                CType(Me.pbLogo, System.ComponentModel.ISupportInitialize).BeginInit()
                Me.pnlBottom.SuspendLayout()
                Me.SuspendLayout()
                '
                'pnlTop
                '
                Me.pnlTop.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlTop.BackColor = System.Drawing.Color.Black
                Me.pnlTop.Controls.Add(Me.lblEdition)
                Me.pnlTop.Controls.Add(Me.pbLogo)
                Me.pnlTop.ForeColor = System.Drawing.Color.White
                Me.pnlTop.Location = New System.Drawing.Point(-1, -1)
                Me.pnlTop.Name = "pnlTop"
                Me.pnlTop.Size = New System.Drawing.Size(788, 145)
                Me.pnlTop.TabIndex = 0
                '
                'lblEdition
                '
                Me.lblEdition.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.lblEdition.BackColor = System.Drawing.Color.Black
                Me.lblEdition.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblEdition.ForeColor = System.Drawing.Color.White
                Me.lblEdition.Location = New System.Drawing.Point(512, 112)
                Me.lblEdition.Name = "lblEdition"
                Me.lblEdition.Size = New System.Drawing.Size(264, 24)
                Me.lblEdition.TabIndex = 0
                Me.lblEdition.Text = "Edition"
                Me.lblEdition.TextAlign = System.Drawing.ContentAlignment.BottomRight
                Me.lblEdition.Visible = False
                '
                'pbLogo
                '
                Me.pbLogo.Image = Global.mRemoteNG.My.Resources.Resources.Logo
                Me.pbLogo.Location = New System.Drawing.Point(8, 8)
                Me.pbLogo.Name = "pbLogo"
                Me.pbLogo.Size = New System.Drawing.Size(492, 128)
                Me.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
                Me.pbLogo.TabIndex = 1
                Me.pbLogo.TabStop = False
                '
                'pnlBottom
                '
                Me.pnlBottom.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlBottom.BackColor = System.Drawing.SystemColors.Control
                Me.pnlBottom.Controls.Add(Me.llblWeifenLuo)
                Me.pnlBottom.Controls.Add(Me.llblMagicLibrary)
                Me.pnlBottom.Controls.Add(Me.llblFAMFAMFAM)
                Me.pnlBottom.Controls.Add(Me.txtChangeLog)
                Me.pnlBottom.Controls.Add(Me.lblTitle)
                Me.pnlBottom.Controls.Add(Me.lblVersion)
                Me.pnlBottom.Controls.Add(Me.lblChangeLog)
                Me.pnlBottom.Controls.Add(Me.lblLicense)
                Me.pnlBottom.Controls.Add(Me.lblCopyright)
                Me.pnlBottom.ForeColor = System.Drawing.SystemColors.ControlText
                Me.pnlBottom.Location = New System.Drawing.Point(-1, 144)
                Me.pnlBottom.Name = "pnlBottom"
                Me.pnlBottom.Size = New System.Drawing.Size(788, 418)
                Me.pnlBottom.TabIndex = 1
                '
                'llblWeifenLuo
                '
                Me.llblWeifenLuo.AutoSize = True
                Me.llblWeifenLuo.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.llblWeifenLuo.ForeColor = System.Drawing.SystemColors.ControlText
                Me.llblWeifenLuo.LinkColor = System.Drawing.Color.Blue
                Me.llblWeifenLuo.Location = New System.Drawing.Point(16, 158)
                Me.llblWeifenLuo.Name = "llblWeifenLuo"
                Me.llblWeifenLuo.Size = New System.Drawing.Size(78, 22)
                Me.llblWeifenLuo.TabIndex = 9
                Me.llblWeifenLuo.TabStop = True
                Me.llblWeifenLuo.Text = "WeifenLuo"
                Me.llblWeifenLuo.UseCompatibleTextRendering = True
                '
                'llblMagicLibrary
                '
                Me.llblMagicLibrary.AutoSize = True
                Me.llblMagicLibrary.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.llblMagicLibrary.ForeColor = System.Drawing.SystemColors.ControlText
                Me.llblMagicLibrary.LinkColor = System.Drawing.Color.Blue
                Me.llblMagicLibrary.Location = New System.Drawing.Point(16, 136)
                Me.llblMagicLibrary.Name = "llblMagicLibrary"
                Me.llblMagicLibrary.Size = New System.Drawing.Size(92, 22)
                Me.llblMagicLibrary.TabIndex = 8
                Me.llblMagicLibrary.TabStop = True
                Me.llblMagicLibrary.Text = "MagicLibrary"
                Me.llblMagicLibrary.UseCompatibleTextRendering = True
                '
                'llblFAMFAMFAM
                '
                Me.llblFAMFAMFAM.AutoSize = True
                Me.llblFAMFAMFAM.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.llblFAMFAMFAM.ForeColor = System.Drawing.SystemColors.ControlText
                Me.llblFAMFAMFAM.LinkColor = System.Drawing.Color.Blue
                Me.llblFAMFAMFAM.Location = New System.Drawing.Point(16, 116)
                Me.llblFAMFAMFAM.Name = "llblFAMFAMFAM"
                Me.llblFAMFAMFAM.Size = New System.Drawing.Size(101, 22)
                Me.llblFAMFAMFAM.TabIndex = 4
                Me.llblFAMFAMFAM.TabStop = True
                Me.llblFAMFAMFAM.Text = "FAMFAMFAM"
                Me.llblFAMFAMFAM.UseCompatibleTextRendering = True
                '
                'txtChangeLog
                '
                Me.txtChangeLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.txtChangeLog.BackColor = System.Drawing.SystemColors.Control
                Me.txtChangeLog.BorderStyle = System.Windows.Forms.BorderStyle.None
                Me.txtChangeLog.Cursor = System.Windows.Forms.Cursors.Default
                Me.txtChangeLog.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.txtChangeLog.ForeColor = System.Drawing.SystemColors.ControlText
                Me.txtChangeLog.Location = New System.Drawing.Point(24, 224)
                Me.txtChangeLog.Multiline = True
                Me.txtChangeLog.Name = "txtChangeLog"
                Me.txtChangeLog.ReadOnly = True
                Me.txtChangeLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
                Me.txtChangeLog.Size = New System.Drawing.Size(760, 192)
                Me.txtChangeLog.TabIndex = 7
                Me.txtChangeLog.TabStop = False
                '
                'lblTitle
                '
                Me.lblTitle.AutoSize = True
                Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblTitle.ForeColor = System.Drawing.SystemColors.ControlText
                Me.lblTitle.Location = New System.Drawing.Point(16, 16)
                Me.lblTitle.Name = "lblTitle"
                Me.lblTitle.Size = New System.Drawing.Size(122, 27)
                Me.lblTitle.TabIndex = 0
                Me.lblTitle.Text = "mRemoteNG"
                Me.lblTitle.UseCompatibleTextRendering = True
                '
                'lblVersion
                '
                Me.lblVersion.AutoSize = True
                Me.lblVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblVersion.ForeColor = System.Drawing.SystemColors.ControlText
                Me.lblVersion.Location = New System.Drawing.Point(16, 56)
                Me.lblVersion.Name = "lblVersion"
                Me.lblVersion.Size = New System.Drawing.Size(57, 22)
                Me.lblVersion.TabIndex = 1
                Me.lblVersion.Text = "Version"
                Me.lblVersion.UseCompatibleTextRendering = True
                '
                'lblChangeLog
                '
                Me.lblChangeLog.AutoSize = True
                Me.lblChangeLog.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblChangeLog.ForeColor = System.Drawing.SystemColors.ControlText
                Me.lblChangeLog.Location = New System.Drawing.Point(16, 199)
                Me.lblChangeLog.Name = "lblChangeLog"
                Me.lblChangeLog.Size = New System.Drawing.Size(92, 22)
                Me.lblChangeLog.TabIndex = 6
                Me.lblChangeLog.Text = "Change Log:"
                Me.lblChangeLog.UseCompatibleTextRendering = True
                '
                'lblLicense
                '
                Me.lblLicense.AutoSize = True
                Me.lblLicense.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblLicense.ForeColor = System.Drawing.SystemColors.ControlText
                Me.lblLicense.Location = New System.Drawing.Point(16, 96)
                Me.lblLicense.Name = "lblLicense"
                Me.lblLicense.Size = New System.Drawing.Size(58, 22)
                Me.lblLicense.TabIndex = 5
                Me.lblLicense.Text = "License"
                Me.lblLicense.UseCompatibleTextRendering = True
                '
                'lblCopyright
                '
                Me.lblCopyright.AutoSize = True
                Me.lblCopyright.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblCopyright.ForeColor = System.Drawing.SystemColors.ControlText
                Me.lblCopyright.Location = New System.Drawing.Point(16, 76)
                Me.lblCopyright.Name = "lblCopyright"
                Me.lblCopyright.Size = New System.Drawing.Size(70, 22)
                Me.lblCopyright.TabIndex = 2
                Me.lblCopyright.Text = "Copyright"
                Me.lblCopyright.UseCompatibleTextRendering = True
                '
                'About
                '
                Me.BackColor = System.Drawing.SystemColors.Control
                Me.ClientSize = New System.Drawing.Size(784, 564)
                Me.Controls.Add(Me.pnlTop)
                Me.Controls.Add(Me.pnlBottom)
                Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.ForeColor = System.Drawing.SystemColors.ControlText
                Me.Icon = Global.mRemoteNG.My.Resources.Resources.mRemote_Icon
                Me.MaximumSize = New System.Drawing.Size(20000, 10000)
                Me.Name = "About"
                Me.TabText = "About"
                Me.Text = "About"
                Me.pnlTop.ResumeLayout(False)
                Me.pnlTop.PerformLayout()
                CType(Me.pbLogo, System.ComponentModel.ISupportInitialize).EndInit()
                Me.pnlBottom.ResumeLayout(False)
                Me.pnlBottom.PerformLayout()
                Me.ResumeLayout(False)

            End Sub
#End Region

#Region "Public Methods"
            Public Sub New(ByVal Panel As DockContent)
                Me.WindowType = Type.About
                Me.DockPnl = Panel
                Me.InitializeComponent()
                App.Runtime.FontOverride(Me)
            End Sub
#End Region

#Region "Private Methods"
            Private Sub ApplyLanguage()
                lblLicense.Text = My.Resources.strLabelReleasedUnderGPL
                lblChangeLog.Text = My.Resources.strLabelChangeLog
                TabText = My.Resources.strAbout
                Text = My.Resources.strAbout
            End Sub

            Private Sub ApplyEditions()
#If PORTABLE Then
                lblEdition.Text = My.Resources.strLabelPortableEdition
                lblEdition.Visible = True
#End If
            End Sub

            Private Sub FillLinkLabel(ByVal llbl As LinkLabel, ByVal Text As String, ByVal URL As String)
                llbl.Links.Clear()

                Dim Open As Integer = Text.IndexOf("[")
                Dim Close As Integer
                While Open <> -1
                    Text = Text.Remove(Open, 1)
                    Close = Text.IndexOf("]", Open)
                    If Close = -1 Then Exit While
                    Text = Text.Remove(Close, 1)
                    llbl.Links.Add(Open, Close - Open, URL)
                    Open = Text.IndexOf("[", Open)
                End While

                llbl.Text = Text
            End Sub
#End Region

#Region "Form Stuff"
            Private Sub About_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
                ApplyLanguage()
                ApplyEditions()

                Try
                    lblCopyright.Text = My.Application.Info.Copyright

                    Me.lblVersion.Text = "Version " & My.Application.Info.Version.ToString

                    FillLinkLabel(llblFAMFAMFAM, My.Resources.strFAMFAMFAMAttribution, My.Resources.strFAMFAMFAMAttributionURL)
                    FillLinkLabel(llblMagicLibrary, My.Resources.strMagicLibraryAttribution, My.Resources.strMagicLibraryAttributionURL)
                    FillLinkLabel(llblWeifenLuo, My.Resources.strWeifenLuoAttribution, My.Resources.strWeifenLuoAttributionURL)

                    If File.Exists(My.Application.Info.DirectoryPath & "\CHANGELOG.TXT") Then
                        Dim sR As New StreamReader(My.Application.Info.DirectoryPath & "\CHANGELOG.TXT")
                        Me.txtChangeLog.Text = sR.ReadToEnd
                        sR.Close()
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Loading About failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub llblFAMFAMFAM_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llblFAMFAMFAM.LinkClicked
                App.Runtime.GoToURL(My.Resources.strFAMFAMFAMAttributionURL)
            End Sub

            Private Sub llblMagicLibrary_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llblMagicLibrary.LinkClicked
                App.Runtime.GoToURL(My.Resources.strMagicLibraryAttributionURL)
            End Sub

            Private Sub llblWeifenLuo_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llblWeifenLuo.LinkClicked
                App.Runtime.GoToURL(My.Resources.strWeifenLuoAttributionURL)
            End Sub
#End Region
        End Class
    End Namespace
End Namespace