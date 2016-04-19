Imports System.ComponentModel
Imports System.IO
Imports mRemote3G.App
Imports mRemote3G.Messages
Imports mRemote3G.My.Resources
Imports WeifenLuo.WinFormsUI.Docking

Namespace UI

    Namespace Window
        Public Class About
            Inherits Base

#Region "Form Init"

            Friend WithEvents lblCopyright As Label
            Friend WithEvents lblTitle As Label
            Friend WithEvents lblVersion As Label
            Friend WithEvents lblLicense As Label
            Friend WithEvents txtChangeLog As TextBox
            Friend WithEvents lblChangeLog As Label
            Friend WithEvents pnlBottom As Panel
            Friend WithEvents pbLogo As PictureBox
            Friend WithEvents lblEdition As Label
            Friend WithEvents llblFAMFAMFAM As LinkLabel
            Friend WithEvents llblMagicLibrary As LinkLabel
            Friend WithEvents llblWeifenLuo As LinkLabel
            Friend WithEvents pnlTop As Panel

            Private Sub InitializeComponent()
                Me.pnlTop = New Panel()
                Me.lblEdition = New Label()
                Me.pbLogo = New PictureBox()
                Me.pnlBottom = New Panel()
                Me.llblWeifenLuo = New LinkLabel()
                Me.llblMagicLibrary = New LinkLabel()
                Me.llblFAMFAMFAM = New LinkLabel()
                Me.txtChangeLog = New TextBox()
                Me.lblTitle = New Label()
                Me.lblVersion = New Label()
                Me.lblChangeLog = New Label()
                Me.lblLicense = New Label()
                Me.lblCopyright = New Label()
                Me.pnlTop.SuspendLayout()
                CType(Me.pbLogo, ISupportInitialize).BeginInit()
                Me.pnlBottom.SuspendLayout()
                Me.SuspendLayout()
                '
                'pnlTop
                '
                Me.pnlTop.Anchor = CType(((AnchorStyles.Top Or AnchorStyles.Left) _
                                          Or AnchorStyles.Right),
                                         AnchorStyles)
                Me.pnlTop.BackColor = Color.Black
                Me.pnlTop.Controls.Add(Me.lblEdition)
                Me.pnlTop.Controls.Add(Me.pbLogo)
                Me.pnlTop.ForeColor = Color.White
                Me.pnlTop.Location = New Point(- 1, - 1)
                Me.pnlTop.Name = "pnlTop"
                Me.pnlTop.Size = New Size(788, 145)
                Me.pnlTop.TabIndex = 0
                '
                'lblEdition
                '
                Me.lblEdition.Anchor = CType((AnchorStyles.Top Or AnchorStyles.Right), AnchorStyles)
                Me.lblEdition.BackColor = Color.Black
                Me.lblEdition.Font = New Font("Microsoft Sans Serif", 14.25!, FontStyle.Bold, GraphicsUnit.Point,
                                              CType(0, Byte))
                Me.lblEdition.ForeColor = Color.White
                Me.lblEdition.Location = New Point(509, 107)
                Me.lblEdition.Name = "lblEdition"
                Me.lblEdition.Size = New Size(264, 29)
                Me.lblEdition.TabIndex = 0
                Me.lblEdition.Text = "Edition"
                Me.lblEdition.TextAlign = ContentAlignment.BottomRight
                Me.lblEdition.Visible = False
                '
                'pbLogo
                '
                Me.pbLogo.Image = Logo
                Me.pbLogo.Location = New Point(8, 8)
                Me.pbLogo.Name = "pbLogo"
                Me.pbLogo.Size = New Size(492, 128)
                Me.pbLogo.SizeMode = PictureBoxSizeMode.AutoSize
                Me.pbLogo.TabIndex = 1
                Me.pbLogo.TabStop = False
                '
                'pnlBottom
                '
                Me.pnlBottom.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
                                              Or AnchorStyles.Left) _
                                             Or AnchorStyles.Right),
                                            AnchorStyles)
                Me.pnlBottom.BackColor = SystemColors.Control
                Me.pnlBottom.Controls.Add(Me.llblWeifenLuo)
                Me.pnlBottom.Controls.Add(Me.llblMagicLibrary)
                Me.pnlBottom.Controls.Add(Me.llblFAMFAMFAM)
                Me.pnlBottom.Controls.Add(Me.txtChangeLog)
                Me.pnlBottom.Controls.Add(Me.lblTitle)
                Me.pnlBottom.Controls.Add(Me.lblVersion)
                Me.pnlBottom.Controls.Add(Me.lblChangeLog)
                Me.pnlBottom.Controls.Add(Me.lblLicense)
                Me.pnlBottom.Controls.Add(Me.lblCopyright)
                Me.pnlBottom.ForeColor = SystemColors.ControlText
                Me.pnlBottom.Location = New Point(- 1, 144)
                Me.pnlBottom.Name = "pnlBottom"
                Me.pnlBottom.Size = New Size(788, 418)
                Me.pnlBottom.TabIndex = 1
                '
                'llblWeifenLuo
                '
                Me.llblWeifenLuo.AutoSize = True
                Me.llblWeifenLuo.Font = New Font("Microsoft Sans Serif", 11.0!, FontStyle.Regular, GraphicsUnit.Point,
                                                 CType(0, Byte))
                Me.llblWeifenLuo.ForeColor = SystemColors.ControlText
                Me.llblWeifenLuo.LinkColor = Color.Blue
                Me.llblWeifenLuo.Location = New Point(8, 184)
                Me.llblWeifenLuo.Name = "llblWeifenLuo"
                Me.llblWeifenLuo.Size = New Size(97, 27)
                Me.llblWeifenLuo.TabIndex = 9
                Me.llblWeifenLuo.TabStop = True
                Me.llblWeifenLuo.Text = "WeifenLuo"
                Me.llblWeifenLuo.UseCompatibleTextRendering = True
                '
                'llblMagicLibrary
                '
                Me.llblMagicLibrary.AutoSize = True
                Me.llblMagicLibrary.Font = New Font("Microsoft Sans Serif", 11.0!, FontStyle.Regular, GraphicsUnit.Point,
                                                    CType(0, Byte))
                Me.llblMagicLibrary.ForeColor = SystemColors.ControlText
                Me.llblMagicLibrary.LinkColor = Color.Blue
                Me.llblMagicLibrary.Location = New Point(8, 157)
                Me.llblMagicLibrary.Name = "llblMagicLibrary"
                Me.llblMagicLibrary.Size = New Size(115, 27)
                Me.llblMagicLibrary.TabIndex = 8
                Me.llblMagicLibrary.TabStop = True
                Me.llblMagicLibrary.Text = "MagicLibrary"
                Me.llblMagicLibrary.UseCompatibleTextRendering = True
                '
                'llblFAMFAMFAM
                '
                Me.llblFAMFAMFAM.AutoSize = True
                Me.llblFAMFAMFAM.Font = New Font("Microsoft Sans Serif", 11.0!, FontStyle.Regular, GraphicsUnit.Point,
                                                 CType(0, Byte))
                Me.llblFAMFAMFAM.ForeColor = SystemColors.ControlText
                Me.llblFAMFAMFAM.LinkColor = Color.Blue
                Me.llblFAMFAMFAM.Location = New Point(8, 130)
                Me.llblFAMFAMFAM.Name = "llblFAMFAMFAM"
                Me.llblFAMFAMFAM.Size = New Size(126, 27)
                Me.llblFAMFAMFAM.TabIndex = 4
                Me.llblFAMFAMFAM.TabStop = True
                Me.llblFAMFAMFAM.Text = "FAMFAMFAM"
                Me.llblFAMFAMFAM.UseCompatibleTextRendering = True
                '
                'txtChangeLog
                '
                Me.txtChangeLog.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
                                                 Or AnchorStyles.Left) _
                                                Or AnchorStyles.Right),
                                               AnchorStyles)
                Me.txtChangeLog.BackColor = SystemColors.Control
                Me.txtChangeLog.BorderStyle = BorderStyle.None
                Me.txtChangeLog.Cursor = Cursors.Default
                Me.txtChangeLog.Font = New Font("Microsoft Sans Serif", 9.0!, FontStyle.Regular, GraphicsUnit.Point,
                                                CType(0, Byte))
                Me.txtChangeLog.ForeColor = SystemColors.ControlText
                Me.txtChangeLog.Location = New Point(24, 241)
                Me.txtChangeLog.Multiline = True
                Me.txtChangeLog.Name = "txtChangeLog"
                Me.txtChangeLog.ReadOnly = True
                Me.txtChangeLog.ScrollBars = ScrollBars.Vertical
                Me.txtChangeLog.Size = New Size(760, 175)
                Me.txtChangeLog.TabIndex = 7
                Me.txtChangeLog.TabStop = False
                '
                'lblTitle
                '
                Me.lblTitle.AutoSize = True
                Me.lblTitle.Font = New Font("Microsoft Sans Serif", 14.0!, FontStyle.Bold, GraphicsUnit.Point,
                                            CType(0, Byte))
                Me.lblTitle.ForeColor = SystemColors.ControlText
                Me.lblTitle.Location = New Point(8, 16)
                Me.lblTitle.Name = "lblTitle"
                Me.lblTitle.Size = New Size(149, 33)
                Me.lblTitle.TabIndex = 0
                Me.lblTitle.Text = "mRemote3G"
                Me.lblTitle.UseCompatibleTextRendering = True
                '
                'lblVersion
                '
                Me.lblVersion.AutoSize = True
                Me.lblVersion.Font = New Font("Microsoft Sans Serif", 11.0!, FontStyle.Regular, GraphicsUnit.Point,
                                              CType(0, Byte))
                Me.lblVersion.ForeColor = SystemColors.ControlText
                Me.lblVersion.Location = New Point(8, 49)
                Me.lblVersion.Name = "lblVersion"
                Me.lblVersion.Size = New Size(71, 27)
                Me.lblVersion.TabIndex = 1
                Me.lblVersion.Text = "Version"
                Me.lblVersion.UseCompatibleTextRendering = True
                '
                'lblChangeLog
                '
                Me.lblChangeLog.AutoSize = True
                Me.lblChangeLog.Font = New Font("Microsoft Sans Serif", 11.0!, FontStyle.Regular, GraphicsUnit.Point,
                                                CType(0, Byte))
                Me.lblChangeLog.ForeColor = SystemColors.ControlText
                Me.lblChangeLog.Location = New Point(8, 211)
                Me.lblChangeLog.Name = "lblChangeLog"
                Me.lblChangeLog.Size = New Size(115, 27)
                Me.lblChangeLog.TabIndex = 6
                Me.lblChangeLog.Text = "Change Log:"
                Me.lblChangeLog.UseCompatibleTextRendering = True
                '
                'lblLicense
                '
                Me.lblLicense.AutoSize = True
                Me.lblLicense.Font = New Font("Microsoft Sans Serif", 11.0!, FontStyle.Regular, GraphicsUnit.Point,
                                              CType(0, Byte))
                Me.lblLicense.ForeColor = SystemColors.ControlText
                Me.lblLicense.Location = New Point(8, 103)
                Me.lblLicense.Name = "lblLicense"
                Me.lblLicense.Size = New Size(72, 27)
                Me.lblLicense.TabIndex = 5
                Me.lblLicense.Text = "License"
                Me.lblLicense.UseCompatibleTextRendering = True
                '
                'lblCopyright
                '
                Me.lblCopyright.AutoSize = True
                Me.lblCopyright.Font = New Font("Microsoft Sans Serif", 11.0!, FontStyle.Regular, GraphicsUnit.Point,
                                                CType(0, Byte))
                Me.lblCopyright.ForeColor = SystemColors.ControlText
                Me.lblCopyright.Location = New Point(8, 76)
                Me.lblCopyright.Name = "lblCopyright"
                Me.lblCopyright.Size = New Size(88, 27)
                Me.lblCopyright.TabIndex = 2
                Me.lblCopyright.Text = "Copyright"
                Me.lblCopyright.UseCompatibleTextRendering = True
                '
                'About
                '
                Me.BackColor = SystemColors.Control
                Me.ClientSize = New Size(784, 564)
                Me.Controls.Add(Me.pnlTop)
                Me.Controls.Add(Me.pnlBottom)
                Me.Font = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point, CType(0, Byte))
                Me.ForeColor = SystemColors.ControlText
                Me.Icon = mRemote_Icon
                Me.MaximumSize = New Size(20000, 10000)
                Me.Name = "About"
                Me.TabText = "About"
                Me.Text = "About"
                Me.pnlTop.ResumeLayout(False)
                Me.pnlTop.PerformLayout()
                CType(Me.pbLogo, ISupportInitialize).EndInit()
                Me.pnlBottom.ResumeLayout(False)
                Me.pnlBottom.PerformLayout()
                Me.ResumeLayout(False)
            End Sub

#End Region

#Region "Public Methods"

            Public Sub New(Panel As DockContent)
                Me.WindowType = Type.About
                Me.DockPnl = Panel
                Me.InitializeComponent()
                Runtime.FontOverride(Me)
            End Sub

#End Region

#Region "Private Methods"

            Private Sub ApplyLanguage()
                lblLicense.Text = Language.Language.strLabelReleasedUnderGPL
                lblChangeLog.Text = Language.Language.strLabelChangeLog
                TabText = Language.Language.strAbout
                Text = Language.Language.strAbout
            End Sub

            Private Sub ApplyEditions()
#If PORTABLE Then
                lblEdition.Text = My.Language.strLabelPortableEdition
                lblEdition.Visible = True
#End If
            End Sub

            Private Sub FillLinkLabel(llbl As LinkLabel, Text As String, URL As String)
                llbl.Links.Clear()

                Dim Open As Integer = Text.IndexOf("[")
                Dim Close As Integer
                While Open <> - 1
                    Text = Text.Remove(Open, 1)
                    Close = Text.IndexOf("]", Open)
                    If Close = - 1 Then Exit While
                    Text = Text.Remove(Close, 1)
                    llbl.Links.Add(Open, Close - Open, URL)
                    Open = Text.IndexOf("[", Open)
                End While

                llbl.Text = Text
            End Sub

#End Region

#Region "Form Stuff"

            Private Sub About_Load(sender As Object, e As EventArgs) Handles Me.Load
                ApplyLanguage()
                ApplyEditions()

                Try
                    lblCopyright.Text = My.Application.Info.Copyright

                    Me.lblVersion.Text = "Version " & My.Application.Info.Version.ToString

                    FillLinkLabel(llblFAMFAMFAM, Language.Language.strFAMFAMFAMAttribution,
                                  Language.Language.strFAMFAMFAMAttributionURL)
                    FillLinkLabel(llblMagicLibrary, Language.Language.strMagicLibraryAttribution,
                                  Language.Language.strMagicLibraryAttributionURL)
                    FillLinkLabel(llblWeifenLuo, Language.Language.strWeifenLuoAttribution,
                                  Language.Language.strWeifenLuoAttributionURL)

                    If File.Exists(My.Application.Info.DirectoryPath & "\CHANGELOG.TXT") Then
                        Dim sR As New StreamReader(My.Application.Info.DirectoryPath & "\CHANGELOG.TXT")
                        Me.txtChangeLog.Text = sR.ReadToEnd
                        sR.Close()
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        "Loading About failed" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub llblFAMFAMFAM_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) _
                Handles llblFAMFAMFAM.LinkClicked
                Runtime.GoToURL(Language.Language.strFAMFAMFAMAttributionURL)
            End Sub

            Private Sub llblMagicLibrary_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) _
                Handles llblMagicLibrary.LinkClicked
                Runtime.GoToURL(Language.Language.strMagicLibraryAttributionURL)
            End Sub

            Private Sub llblWeifenLuo_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) _
                Handles llblWeifenLuo.LinkClicked
                Runtime.GoToURL(Language.Language.strWeifenLuoAttributionURL)
            End Sub

#End Region
        End Class
    End Namespace

End Namespace