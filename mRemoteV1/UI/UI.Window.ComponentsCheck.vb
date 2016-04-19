Imports System.ComponentModel
Imports System.IO
Imports System.Threading
Imports AxMSTSCLib
Imports mRemote3G.App
Imports mRemote3G.Connection.Protocol
Imports mRemote3G.Messages
Imports mRemote3G.My
Imports mRemote3G.My.Resources
Imports VncSharp
Imports WeifenLuo.WinFormsUI.Docking

Namespace UI

    Namespace Window
        Public Class ComponentsCheck
            Inherits Base

#Region "Form Stuff"

            Friend WithEvents pbCheck1 As PictureBox
            Friend WithEvents lblCheck1 As Label
            Friend WithEvents pnlCheck2 As Panel
            Friend WithEvents lblCheck2 As Label
            Friend WithEvents pbCheck2 As PictureBox
            Friend WithEvents pnlCheck3 As Panel
            Friend WithEvents lblCheck3 As Label
            Friend WithEvents pbCheck3 As PictureBox
            Friend WithEvents pnlCheck4 As Panel
            Friend WithEvents lblCheck4 As Label
            Friend WithEvents pbCheck4 As PictureBox
            Friend WithEvents btnCheckAgain As Button
            Friend WithEvents txtCheck1 As TextBox
            Friend WithEvents txtCheck2 As TextBox
            Friend WithEvents txtCheck3 As TextBox
            Friend WithEvents txtCheck4 As TextBox
            Friend WithEvents chkAlwaysShow As CheckBox
            Friend WithEvents pnlChecks As Panel
            Friend WithEvents pnlCheck1 As Panel

            Private Sub InitializeComponent()
                Me.pnlCheck1 = New Panel()
                Me.txtCheck1 = New TextBox()
                Me.lblCheck1 = New Label()
                Me.pbCheck1 = New PictureBox()
                Me.pnlCheck2 = New Panel()
                Me.txtCheck2 = New TextBox()
                Me.lblCheck2 = New Label()
                Me.pbCheck2 = New PictureBox()
                Me.pnlCheck3 = New Panel()
                Me.txtCheck3 = New TextBox()
                Me.lblCheck3 = New Label()
                Me.pbCheck3 = New PictureBox()
                Me.pnlCheck4 = New Panel()
                Me.txtCheck4 = New TextBox()
                Me.lblCheck4 = New Label()
                Me.pbCheck4 = New PictureBox()
                Me.btnCheckAgain = New Button()
                Me.chkAlwaysShow = New CheckBox()
                Me.pnlChecks = New Panel()
                Me.pnlCheck1.SuspendLayout()
                CType(Me.pbCheck1, ISupportInitialize).BeginInit()
                Me.pnlCheck2.SuspendLayout()
                CType(Me.pbCheck2, ISupportInitialize).BeginInit()
                Me.pnlCheck3.SuspendLayout()
                CType(Me.pbCheck3, ISupportInitialize).BeginInit()
                CType(Me.pbCheck4, ISupportInitialize).BeginInit()
                Me.pnlChecks.SuspendLayout()
                Me.SuspendLayout()
                '
                'pnlCheck1
                '
                Me.pnlCheck1.Anchor = CType(((AnchorStyles.Top Or AnchorStyles.Left) _
                                             Or AnchorStyles.Right),
                                            AnchorStyles)
                Me.pnlCheck1.Controls.Add(Me.txtCheck1)
                Me.pnlCheck1.Controls.Add(Me.lblCheck1)
                Me.pnlCheck1.Controls.Add(Me.pbCheck1)
                Me.pnlCheck1.Location = New Point(3, 3)
                Me.pnlCheck1.Name = "pnlCheck1"
                Me.pnlCheck1.Size = New Size(562, 130)
                Me.pnlCheck1.TabIndex = 10
                Me.pnlCheck1.Visible = False
                '
                'txtCheck1
                '
                Me.txtCheck1.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
                                              Or AnchorStyles.Left) _
                                             Or AnchorStyles.Right),
                                            AnchorStyles)
                Me.txtCheck1.BackColor = SystemColors.Control
                Me.txtCheck1.BorderStyle = BorderStyle.None
                Me.txtCheck1.Location = New Point(129, 29)
                Me.txtCheck1.Multiline = True
                Me.txtCheck1.Name = "txtCheck1"
                Me.txtCheck1.ReadOnly = True
                Me.txtCheck1.Size = New Size(430, 97)
                Me.txtCheck1.TabIndex = 2
                '
                'lblCheck1
                '
                Me.lblCheck1.Anchor = CType(((AnchorStyles.Top Or AnchorStyles.Left) _
                                             Or AnchorStyles.Right),
                                            AnchorStyles)
                Me.lblCheck1.Font = New Font("Microsoft Sans Serif", 12.0!, FontStyle.Bold, GraphicsUnit.Point,
                                             CType(0, Byte))
                Me.lblCheck1.ForeColor = SystemColors.ControlText
                Me.lblCheck1.Location = New Point(108, 3)
                Me.lblCheck1.Name = "lblCheck1"
                Me.lblCheck1.Size = New Size(451, 23)
                Me.lblCheck1.TabIndex = 1
                Me.lblCheck1.Text = "RDP check succeeded!"
                '
                'pbCheck1
                '
                Me.pbCheck1.Anchor = CType(((AnchorStyles.Top Or AnchorStyles.Bottom) _
                                            Or AnchorStyles.Left),
                                           AnchorStyles)
                Me.pbCheck1.Location = New Point(3, 3)
                Me.pbCheck1.Name = "pbCheck1"
                Me.pbCheck1.Size = New Size(72, 123)
                Me.pbCheck1.TabIndex = 0
                Me.pbCheck1.TabStop = False
                '
                'pnlCheck2
                '
                Me.pnlCheck2.Anchor = CType(((AnchorStyles.Top Or AnchorStyles.Left) _
                                             Or AnchorStyles.Right),
                                            AnchorStyles)
                Me.pnlCheck2.Controls.Add(Me.txtCheck2)
                Me.pnlCheck2.Controls.Add(Me.lblCheck2)
                Me.pnlCheck2.Controls.Add(Me.pbCheck2)
                Me.pnlCheck2.Location = New Point(3, 139)
                Me.pnlCheck2.Name = "pnlCheck2"
                Me.pnlCheck2.Size = New Size(562, 130)
                Me.pnlCheck2.TabIndex = 20
                Me.pnlCheck2.Visible = False
                '
                'txtCheck2
                '
                Me.txtCheck2.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
                                              Or AnchorStyles.Left) _
                                             Or AnchorStyles.Right),
                                            AnchorStyles)
                Me.txtCheck2.BackColor = SystemColors.Control
                Me.txtCheck2.BorderStyle = BorderStyle.None
                Me.txtCheck2.Location = New Point(129, 29)
                Me.txtCheck2.Multiline = True
                Me.txtCheck2.Name = "txtCheck2"
                Me.txtCheck2.ReadOnly = True
                Me.txtCheck2.Size = New Size(430, 97)
                Me.txtCheck2.TabIndex = 2
                '
                'lblCheck2
                '
                Me.lblCheck2.Anchor = CType(((AnchorStyles.Top Or AnchorStyles.Left) _
                                             Or AnchorStyles.Right),
                                            AnchorStyles)
                Me.lblCheck2.Font = New Font("Microsoft Sans Serif", 12.0!, FontStyle.Bold, GraphicsUnit.Point,
                                             CType(0, Byte))
                Me.lblCheck2.Location = New Point(112, 3)
                Me.lblCheck2.Name = "lblCheck2"
                Me.lblCheck2.Size = New Size(447, 23)
                Me.lblCheck2.TabIndex = 1
                Me.lblCheck2.Text = "RDP check succeeded!"
                '
                'pbCheck2
                '
                Me.pbCheck2.Anchor = CType(((AnchorStyles.Top Or AnchorStyles.Bottom) _
                                            Or AnchorStyles.Left),
                                           AnchorStyles)
                Me.pbCheck2.Location = New Point(3, 3)
                Me.pbCheck2.Name = "pbCheck2"
                Me.pbCheck2.Size = New Size(72, 123)
                Me.pbCheck2.TabIndex = 0
                Me.pbCheck2.TabStop = False
                '
                'pnlCheck3
                '
                Me.pnlCheck3.Anchor = CType(((AnchorStyles.Top Or AnchorStyles.Left) _
                                             Or AnchorStyles.Right),
                                            AnchorStyles)
                Me.pnlCheck3.Controls.Add(Me.txtCheck3)
                Me.pnlCheck3.Controls.Add(Me.lblCheck3)
                Me.pnlCheck3.Controls.Add(Me.pbCheck3)
                Me.pnlCheck3.Location = New Point(3, 275)
                Me.pnlCheck3.Name = "pnlCheck3"
                Me.pnlCheck3.Size = New Size(562, 130)
                Me.pnlCheck3.TabIndex = 30
                Me.pnlCheck3.Visible = False
                '
                'txtCheck3
                '
                Me.txtCheck3.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
                                              Or AnchorStyles.Left) _
                                             Or AnchorStyles.Right),
                                            AnchorStyles)
                Me.txtCheck3.BackColor = SystemColors.Control
                Me.txtCheck3.BorderStyle = BorderStyle.None
                Me.txtCheck3.Location = New Point(129, 29)
                Me.txtCheck3.Multiline = True
                Me.txtCheck3.Name = "txtCheck3"
                Me.txtCheck3.ReadOnly = True
                Me.txtCheck3.Size = New Size(430, 97)
                Me.txtCheck3.TabIndex = 2
                '
                'lblCheck3
                '
                Me.lblCheck3.Anchor = CType(((AnchorStyles.Top Or AnchorStyles.Left) _
                                             Or AnchorStyles.Right),
                                            AnchorStyles)
                Me.lblCheck3.Font = New Font("Microsoft Sans Serif", 12.0!, FontStyle.Bold, GraphicsUnit.Point,
                                             CType(0, Byte))
                Me.lblCheck3.Location = New Point(112, 3)
                Me.lblCheck3.Name = "lblCheck3"
                Me.lblCheck3.Size = New Size(447, 23)
                Me.lblCheck3.TabIndex = 1
                Me.lblCheck3.Text = "RDP check succeeded!"
                '
                'pbCheck3
                '
                Me.pbCheck3.Anchor = CType(((AnchorStyles.Top Or AnchorStyles.Bottom) _
                                            Or AnchorStyles.Left),
                                           AnchorStyles)
                Me.pbCheck3.Location = New Point(3, 3)
                Me.pbCheck3.Name = "pbCheck3"
                Me.pbCheck3.Size = New Size(72, 123)
                Me.pbCheck3.TabIndex = 0
                Me.pbCheck3.TabStop = False
                '
                'pnlCheck4
                '
                Me.pnlCheck4.Location = New Point(0, 0)
                Me.pnlCheck4.Name = "pnlCheck4"
                Me.pnlCheck4.Size = New Size(200, 100)
                Me.pnlCheck4.TabIndex = 51
                '
                'txtCheck4
                '
                Me.txtCheck4.Location = New Point(0, 0)
                Me.txtCheck4.Name = "txtCheck4"
                Me.txtCheck4.Size = New Size(100, 22)
                Me.txtCheck4.TabIndex = 0
                '
                'lblCheck4
                '
                Me.lblCheck4.Location = New Point(0, 0)
                Me.lblCheck4.Name = "lblCheck4"
                Me.lblCheck4.Size = New Size(100, 23)
                Me.lblCheck4.TabIndex = 0
                '
                'pbCheck4
                '
                Me.pbCheck4.Location = New Point(0, 0)
                Me.pbCheck4.Name = "pbCheck4"
                Me.pbCheck4.Size = New Size(100, 50)
                Me.pbCheck4.TabIndex = 0
                Me.pbCheck4.TabStop = False
                '
                'btnCheckAgain
                '
                Me.btnCheckAgain.Anchor = CType((AnchorStyles.Bottom Or AnchorStyles.Right), AnchorStyles)
                Me.btnCheckAgain.FlatStyle = FlatStyle.Flat
                Me.btnCheckAgain.Location = New Point(476, 596)
                Me.btnCheckAgain.Name = "btnCheckAgain"
                Me.btnCheckAgain.Size = New Size(104, 26)
                Me.btnCheckAgain.TabIndex = 0
                Me.btnCheckAgain.Text = "Check again"
                Me.btnCheckAgain.UseVisualStyleBackColor = True
                '
                'chkAlwaysShow
                '
                Me.chkAlwaysShow.Anchor = CType((AnchorStyles.Bottom Or AnchorStyles.Left), AnchorStyles)
                Me.chkAlwaysShow.AutoSize = True
                Me.chkAlwaysShow.FlatStyle = FlatStyle.Flat
                Me.chkAlwaysShow.Location = New Point(12, 596)
                Me.chkAlwaysShow.Name = "chkAlwaysShow"
                Me.chkAlwaysShow.Size = New Size(288, 24)
                Me.chkAlwaysShow.TabIndex = 51
                Me.chkAlwaysShow.Text = "Always show this screen at startup"
                Me.chkAlwaysShow.UseVisualStyleBackColor = True
                '
                'pnlChecks
                '
                Me.pnlChecks.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
                                              Or AnchorStyles.Left) _
                                             Or AnchorStyles.Right),
                                            AnchorStyles)
                Me.pnlChecks.AutoScroll = True
                Me.pnlChecks.Controls.Add(Me.pnlCheck1)
                Me.pnlChecks.Controls.Add(Me.pnlCheck2)
                Me.pnlChecks.Controls.Add(Me.pnlCheck3)
                Me.pnlChecks.Controls.Add(Me.pnlCheck4)
                Me.pnlChecks.Location = New Point(12, 12)
                Me.pnlChecks.Name = "pnlChecks"
                Me.pnlChecks.Size = New Size(568, 422)
                Me.pnlChecks.TabIndex = 52
                '
                'ComponentsCheck
                '
                Me.ClientSize = New Size(592, 634)
                Me.Controls.Add(Me.pnlChecks)
                Me.Controls.Add(Me.chkAlwaysShow)
                Me.Controls.Add(Me.btnCheckAgain)
                Me.Font = New Font("Microsoft Sans Serif", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType(0, Byte))
                Me.Icon = ComponentsCheck_Icon
                Me.Name = "ComponentsCheck"
                Me.TabText = "Components Check"
                Me.Text = "Components Check"
                Me.pnlCheck1.ResumeLayout(False)
                Me.pnlCheck1.PerformLayout()
                CType(Me.pbCheck1, ISupportInitialize).EndInit()
                Me.pnlCheck2.ResumeLayout(False)
                Me.pnlCheck2.PerformLayout()
                CType(Me.pbCheck2, ISupportInitialize).EndInit()
                Me.pnlCheck3.ResumeLayout(False)
                Me.pnlCheck3.PerformLayout()
                CType(Me.pbCheck3, ISupportInitialize).EndInit()
                CType(Me.pbCheck4, ISupportInitialize).EndInit()
                Me.pnlChecks.ResumeLayout(False)
                Me.ResumeLayout(False)
                Me.PerformLayout()
            End Sub

#End Region

#Region "Public Methods"

            Public Sub New(Panel As DockContent)
                Me.WindowType = Type.ComponentsCheck
                Me.DockPnl = Panel
                Me.InitializeComponent()
            End Sub

#End Region

#Region "Form Stuff"

            Private Sub ComponentsCheck_Load(sender As Object, e As EventArgs) Handles Me.Load
                ApplyLanguage()

                chkAlwaysShow.Checked = Settings.StartupComponentsCheck
                CheckComponents()
            End Sub

            Private Sub ApplyLanguage()
                TabText = Language.Language.strComponentsCheck
                Text = Language.Language.strComponentsCheck
                chkAlwaysShow.Text = Language.Language.strCcAlwaysShowScreen
                btnCheckAgain.Text = Language.Language.strCcCheckAgain
            End Sub

            Private Sub btnCheckAgain_Click(sender As Object, e As EventArgs) Handles btnCheckAgain.Click
                CheckComponents()
            End Sub

            Private Sub chkAlwaysShow_CheckedChanged(sender As Object, e As EventArgs) _
                Handles chkAlwaysShow.CheckedChanged
                Settings.StartupComponentsCheck = chkAlwaysShow.Checked
                Settings.Save()
            End Sub

#End Region

#Region "Private Methods"

            Private Sub CheckComponents()
                Dim errorMsg As String = Language.Language.strCcNotInstalledProperly

                pnlCheck1.Visible = True
                pnlCheck2.Visible = True
                pnlCheck3.Visible = True
                pnlCheck4.Visible = True

                Dim rdpClient As AxMsRdpClient8NotSafeForScripting = Nothing

                Try
                    rdpClient = New AxMsRdpClient8NotSafeForScripting
                    rdpClient.CreateControl()

                    Do Until rdpClient.Created
                        Thread.Sleep(0)
                        System.Windows.Forms.Application.DoEvents()
                    Loop

                    If Not New Version(rdpClient.Version) >= RDP.Versions.RDC80 Then
                        Throw _
                            New Exception(
                                String.Format("Found RDC Client version {0} but version {1} or higher is required.",
                                              rdpClient.Version, RDP.Versions.RDC60))
                    End If

                    pbCheck1.Image = Good_Symbol
                    lblCheck1.ForeColor = Color.DarkOliveGreen
                    lblCheck1.Text = "RDP (Remote Desktop) " & Language.Language.strCcCheckSucceeded
                    txtCheck1.Text = String.Format(Language.Language.strCcRDPOK, rdpClient.Version)
                Catch ex As Exception
                    pbCheck1.Image = Bad_Symbol
                    lblCheck1.ForeColor = Color.Firebrick
                    lblCheck1.Text = "RDP (Remote Desktop) " & Language.Language.strCcCheckFailed
                    txtCheck1.Text = Language.Language.strCcRDPFailed

                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "RDP " & errorMsg, True)
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, ex.ToString(), True)
                End Try

                If rdpClient IsNot Nothing Then rdpClient.Dispose()


                Dim VNC As RemoteDesktop = Nothing

                Try
                    VNC = New RemoteDesktop
                    VNC.CreateControl()

                    Do Until VNC.Created
                        Thread.Sleep(10)
                        System.Windows.Forms.Application.DoEvents()
                    Loop

                    pbCheck2.Image = Good_Symbol
                    lblCheck2.ForeColor = Color.DarkOliveGreen
                    lblCheck2.Text = "VNC (Virtual Network Computing) " & Language.Language.strCcCheckSucceeded
                    txtCheck2.Text = String.Format(Language.Language.strCcVNCOK, VNC.ProductVersion)
                Catch ex As Exception
                    pbCheck2.Image = Bad_Symbol
                    lblCheck2.ForeColor = Color.Firebrick
                    lblCheck2.Text = "VNC (Virtual Network Computing) " & Language.Language.strCcCheckFailed
                    txtCheck2.Text = Language.Language.strCcVNCFailed

                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "VNC " & errorMsg, True)
                End Try

                If VNC IsNot Nothing Then VNC.Dispose()


                Dim pPath = ""
                If Settings.UseCustomPuttyPath = False Then
                    pPath = Application.Info.DirectoryPath & "\PuTTYNG.exe"
                Else
                    pPath = Settings.CustomPuttyPath
                End If

                If File.Exists(pPath) Then
                    pbCheck3.Image = Good_Symbol
                    lblCheck3.ForeColor = Color.DarkOliveGreen
                    lblCheck3.Text = "PuTTY (SSH/Telnet/Rlogin/RAW) " & Language.Language.strCcCheckSucceeded
                    txtCheck3.Text = Language.Language.strCcPuttyOK
                Else
                    pbCheck3.Image = Bad_Symbol
                    lblCheck3.ForeColor = Color.Firebrick
                    lblCheck3.Text = "PuTTY (SSH/Telnet/Rlogin/RAW) " & Language.Language.strCcCheckFailed
                    txtCheck3.Text = Language.Language.strCcPuttyFailed

                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "PuTTY " & errorMsg, True)
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "File " & pPath & " does not exist.",
                                                        True)
                End If
            End Sub

#End Region
        End Class
    End Namespace

End Namespace