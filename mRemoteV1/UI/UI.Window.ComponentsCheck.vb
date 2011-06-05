Imports WeifenLuo.WinFormsUI.Docking
Imports System.IO
Imports mRemoteNG.App.Runtime
Imports System.Threading

Namespace UI
    Namespace Window
        Public Class ComponentsCheck
            Inherits UI.Window.Base

#Region "Form Stuff"
            Friend WithEvents pbCheck1 As System.Windows.Forms.PictureBox
            Friend WithEvents lblCheck1 As System.Windows.Forms.Label
            Friend WithEvents pnlCheck2 As System.Windows.Forms.Panel
            Friend WithEvents lblCheck2 As System.Windows.Forms.Label
            Friend WithEvents pbCheck2 As System.Windows.Forms.PictureBox
            Friend WithEvents pnlCheck3 As System.Windows.Forms.Panel
            Friend WithEvents lblCheck3 As System.Windows.Forms.Label
            Friend WithEvents pbCheck3 As System.Windows.Forms.PictureBox
            Friend WithEvents pnlCheck4 As System.Windows.Forms.Panel
            Friend WithEvents lblCheck4 As System.Windows.Forms.Label
            Friend WithEvents pbCheck4 As System.Windows.Forms.PictureBox
            Friend WithEvents pnlCheck5 As System.Windows.Forms.Panel
            Friend WithEvents lblCheck5 As System.Windows.Forms.Label
            Friend WithEvents pbCheck5 As System.Windows.Forms.PictureBox
            Friend WithEvents btnCheckAgain As System.Windows.Forms.Button
            Friend WithEvents txtCheck1 As System.Windows.Forms.TextBox
            Friend WithEvents txtCheck2 As System.Windows.Forms.TextBox
            Friend WithEvents txtCheck3 As System.Windows.Forms.TextBox
            Friend WithEvents txtCheck4 As System.Windows.Forms.TextBox
            Friend WithEvents txtCheck5 As System.Windows.Forms.TextBox
            Friend WithEvents chkAlwaysShow As System.Windows.Forms.CheckBox
            Friend WithEvents pnlChecks As System.Windows.Forms.Panel
            Friend WithEvents pnlCheck6 As System.Windows.Forms.Panel
            Friend WithEvents txtCheck6 As System.Windows.Forms.TextBox
            Friend WithEvents lblCheck6 As System.Windows.Forms.Label
            Friend WithEvents pbCheck6 As System.Windows.Forms.PictureBox
            Friend WithEvents pnlCheck1 As System.Windows.Forms.Panel

            Private Sub InitializeComponent()
                Me.pnlCheck1 = New System.Windows.Forms.Panel
                Me.txtCheck1 = New System.Windows.Forms.TextBox
                Me.lblCheck1 = New System.Windows.Forms.Label
                Me.pbCheck1 = New System.Windows.Forms.PictureBox
                Me.pnlCheck2 = New System.Windows.Forms.Panel
                Me.txtCheck2 = New System.Windows.Forms.TextBox
                Me.lblCheck2 = New System.Windows.Forms.Label
                Me.pbCheck2 = New System.Windows.Forms.PictureBox
                Me.pnlCheck3 = New System.Windows.Forms.Panel
                Me.txtCheck3 = New System.Windows.Forms.TextBox
                Me.lblCheck3 = New System.Windows.Forms.Label
                Me.pbCheck3 = New System.Windows.Forms.PictureBox
                Me.pnlCheck4 = New System.Windows.Forms.Panel
                Me.txtCheck4 = New System.Windows.Forms.TextBox
                Me.lblCheck4 = New System.Windows.Forms.Label
                Me.pbCheck4 = New System.Windows.Forms.PictureBox
                Me.pnlCheck5 = New System.Windows.Forms.Panel
                Me.txtCheck5 = New System.Windows.Forms.TextBox
                Me.lblCheck5 = New System.Windows.Forms.Label
                Me.pbCheck5 = New System.Windows.Forms.PictureBox
                Me.btnCheckAgain = New System.Windows.Forms.Button
                Me.chkAlwaysShow = New System.Windows.Forms.CheckBox
                Me.pnlChecks = New System.Windows.Forms.Panel
                Me.pnlCheck6 = New System.Windows.Forms.Panel
                Me.txtCheck6 = New System.Windows.Forms.TextBox
                Me.lblCheck6 = New System.Windows.Forms.Label
                Me.pbCheck6 = New System.Windows.Forms.PictureBox
                Me.pnlCheck1.SuspendLayout()
                CType(Me.pbCheck1, System.ComponentModel.ISupportInitialize).BeginInit()
                Me.pnlCheck2.SuspendLayout()
                CType(Me.pbCheck2, System.ComponentModel.ISupportInitialize).BeginInit()
                Me.pnlCheck3.SuspendLayout()
                CType(Me.pbCheck3, System.ComponentModel.ISupportInitialize).BeginInit()
                Me.pnlCheck4.SuspendLayout()
                CType(Me.pbCheck4, System.ComponentModel.ISupportInitialize).BeginInit()
                Me.pnlCheck5.SuspendLayout()
                CType(Me.pbCheck5, System.ComponentModel.ISupportInitialize).BeginInit()
                Me.pnlChecks.SuspendLayout()
                Me.pnlCheck6.SuspendLayout()
                CType(Me.pbCheck6, System.ComponentModel.ISupportInitialize).BeginInit()
                Me.SuspendLayout()
                '
                'pnlCheck1
                '
                Me.pnlCheck1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlCheck1.Controls.Add(Me.txtCheck1)
                Me.pnlCheck1.Controls.Add(Me.lblCheck1)
                Me.pnlCheck1.Controls.Add(Me.pbCheck1)
                Me.pnlCheck1.Location = New System.Drawing.Point(3, 3)
                Me.pnlCheck1.Name = "pnlCheck1"
                Me.pnlCheck1.Size = New System.Drawing.Size(562, 130)
                Me.pnlCheck1.TabIndex = 10
                Me.pnlCheck1.Visible = False
                '
                'txtCheck1
                '
                Me.txtCheck1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.txtCheck1.BackColor = System.Drawing.SystemColors.Control
                Me.txtCheck1.BorderStyle = System.Windows.Forms.BorderStyle.None
                Me.txtCheck1.Location = New System.Drawing.Point(129, 29)
                Me.txtCheck1.Multiline = True
                Me.txtCheck1.Name = "txtCheck1"
                Me.txtCheck1.ReadOnly = True
                Me.txtCheck1.Size = New System.Drawing.Size(430, 97)
                Me.txtCheck1.TabIndex = 2
                '
                'lblCheck1
                '
                Me.lblCheck1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.lblCheck1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblCheck1.ForeColor = System.Drawing.SystemColors.ControlText
                Me.lblCheck1.Location = New System.Drawing.Point(108, 3)
                Me.lblCheck1.Name = "lblCheck1"
                Me.lblCheck1.Size = New System.Drawing.Size(451, 23)
                Me.lblCheck1.TabIndex = 1
                Me.lblCheck1.Text = "RDP check succeeded!"
                '
                'pbCheck1
                '
                Me.pbCheck1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
                Me.pbCheck1.Location = New System.Drawing.Point(3, 3)
                Me.pbCheck1.Name = "pbCheck1"
                Me.pbCheck1.Size = New System.Drawing.Size(72, 123)
                Me.pbCheck1.TabIndex = 0
                Me.pbCheck1.TabStop = False
                '
                'pnlCheck2
                '
                Me.pnlCheck2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlCheck2.Controls.Add(Me.txtCheck2)
                Me.pnlCheck2.Controls.Add(Me.lblCheck2)
                Me.pnlCheck2.Controls.Add(Me.pbCheck2)
                Me.pnlCheck2.Location = New System.Drawing.Point(3, 139)
                Me.pnlCheck2.Name = "pnlCheck2"
                Me.pnlCheck2.Size = New System.Drawing.Size(562, 130)
                Me.pnlCheck2.TabIndex = 20
                Me.pnlCheck2.Visible = False
                '
                'txtCheck2
                '
                Me.txtCheck2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.txtCheck2.BackColor = System.Drawing.SystemColors.Control
                Me.txtCheck2.BorderStyle = System.Windows.Forms.BorderStyle.None
                Me.txtCheck2.Location = New System.Drawing.Point(129, 29)
                Me.txtCheck2.Multiline = True
                Me.txtCheck2.Name = "txtCheck2"
                Me.txtCheck2.ReadOnly = True
                Me.txtCheck2.Size = New System.Drawing.Size(430, 97)
                Me.txtCheck2.TabIndex = 2
                '
                'lblCheck2
                '
                Me.lblCheck2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.lblCheck2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblCheck2.Location = New System.Drawing.Point(112, 3)
                Me.lblCheck2.Name = "lblCheck2"
                Me.lblCheck2.Size = New System.Drawing.Size(447, 23)
                Me.lblCheck2.TabIndex = 1
                Me.lblCheck2.Text = "RDP check succeeded!"
                '
                'pbCheck2
                '
                Me.pbCheck2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
                Me.pbCheck2.Location = New System.Drawing.Point(3, 3)
                Me.pbCheck2.Name = "pbCheck2"
                Me.pbCheck2.Size = New System.Drawing.Size(72, 123)
                Me.pbCheck2.TabIndex = 0
                Me.pbCheck2.TabStop = False
                '
                'pnlCheck3
                '
                Me.pnlCheck3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlCheck3.Controls.Add(Me.txtCheck3)
                Me.pnlCheck3.Controls.Add(Me.lblCheck3)
                Me.pnlCheck3.Controls.Add(Me.pbCheck3)
                Me.pnlCheck3.Location = New System.Drawing.Point(3, 275)
                Me.pnlCheck3.Name = "pnlCheck3"
                Me.pnlCheck3.Size = New System.Drawing.Size(562, 130)
                Me.pnlCheck3.TabIndex = 30
                Me.pnlCheck3.Visible = False
                '
                'txtCheck3
                '
                Me.txtCheck3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.txtCheck3.BackColor = System.Drawing.SystemColors.Control
                Me.txtCheck3.BorderStyle = System.Windows.Forms.BorderStyle.None
                Me.txtCheck3.Location = New System.Drawing.Point(129, 29)
                Me.txtCheck3.Multiline = True
                Me.txtCheck3.Name = "txtCheck3"
                Me.txtCheck3.ReadOnly = True
                Me.txtCheck3.Size = New System.Drawing.Size(430, 97)
                Me.txtCheck3.TabIndex = 2
                '
                'lblCheck3
                '
                Me.lblCheck3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.lblCheck3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblCheck3.Location = New System.Drawing.Point(112, 3)
                Me.lblCheck3.Name = "lblCheck3"
                Me.lblCheck3.Size = New System.Drawing.Size(447, 23)
                Me.lblCheck3.TabIndex = 1
                Me.lblCheck3.Text = "RDP check succeeded!"
                '
                'pbCheck3
                '
                Me.pbCheck3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
                Me.pbCheck3.Location = New System.Drawing.Point(3, 3)
                Me.pbCheck3.Name = "pbCheck3"
                Me.pbCheck3.Size = New System.Drawing.Size(72, 123)
                Me.pbCheck3.TabIndex = 0
                Me.pbCheck3.TabStop = False
                '
                'pnlCheck4
                '
                Me.pnlCheck4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlCheck4.Controls.Add(Me.txtCheck4)
                Me.pnlCheck4.Controls.Add(Me.lblCheck4)
                Me.pnlCheck4.Controls.Add(Me.pbCheck4)
                Me.pnlCheck4.Location = New System.Drawing.Point(3, 411)
                Me.pnlCheck4.Name = "pnlCheck4"
                Me.pnlCheck4.Size = New System.Drawing.Size(562, 130)
                Me.pnlCheck4.TabIndex = 40
                Me.pnlCheck4.Visible = False
                '
                'txtCheck4
                '
                Me.txtCheck4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.txtCheck4.BackColor = System.Drawing.SystemColors.Control
                Me.txtCheck4.BorderStyle = System.Windows.Forms.BorderStyle.None
                Me.txtCheck4.Location = New System.Drawing.Point(129, 30)
                Me.txtCheck4.Multiline = True
                Me.txtCheck4.Name = "txtCheck4"
                Me.txtCheck4.ReadOnly = True
                Me.txtCheck4.Size = New System.Drawing.Size(430, 97)
                Me.txtCheck4.TabIndex = 2
                '
                'lblCheck4
                '
                Me.lblCheck4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.lblCheck4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblCheck4.Location = New System.Drawing.Point(112, 3)
                Me.lblCheck4.Name = "lblCheck4"
                Me.lblCheck4.Size = New System.Drawing.Size(447, 23)
                Me.lblCheck4.TabIndex = 1
                Me.lblCheck4.Text = "RDP check succeeded!"
                '
                'pbCheck4
                '
                Me.pbCheck4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
                Me.pbCheck4.Location = New System.Drawing.Point(3, 3)
                Me.pbCheck4.Name = "pbCheck4"
                Me.pbCheck4.Size = New System.Drawing.Size(72, 123)
                Me.pbCheck4.TabIndex = 0
                Me.pbCheck4.TabStop = False
                '
                'pnlCheck5
                '
                Me.pnlCheck5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlCheck5.Controls.Add(Me.txtCheck5)
                Me.pnlCheck5.Controls.Add(Me.lblCheck5)
                Me.pnlCheck5.Controls.Add(Me.pbCheck5)
                Me.pnlCheck5.Location = New System.Drawing.Point(3, 547)
                Me.pnlCheck5.Name = "pnlCheck5"
                Me.pnlCheck5.Size = New System.Drawing.Size(562, 130)
                Me.pnlCheck5.TabIndex = 50
                Me.pnlCheck5.Visible = False
                '
                'txtCheck5
                '
                Me.txtCheck5.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.txtCheck5.BackColor = System.Drawing.SystemColors.Control
                Me.txtCheck5.BorderStyle = System.Windows.Forms.BorderStyle.None
                Me.txtCheck5.Location = New System.Drawing.Point(129, 29)
                Me.txtCheck5.Multiline = True
                Me.txtCheck5.Name = "txtCheck5"
                Me.txtCheck5.ReadOnly = True
                Me.txtCheck5.Size = New System.Drawing.Size(430, 97)
                Me.txtCheck5.TabIndex = 2
                '
                'lblCheck5
                '
                Me.lblCheck5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.lblCheck5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblCheck5.Location = New System.Drawing.Point(112, 3)
                Me.lblCheck5.Name = "lblCheck5"
                Me.lblCheck5.Size = New System.Drawing.Size(447, 23)
                Me.lblCheck5.TabIndex = 1
                Me.lblCheck5.Text = "RDP check succeeded!"
                '
                'pbCheck5
                '
                Me.pbCheck5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
                Me.pbCheck5.Location = New System.Drawing.Point(3, 3)
                Me.pbCheck5.Name = "pbCheck5"
                Me.pbCheck5.Size = New System.Drawing.Size(72, 123)
                Me.pbCheck5.TabIndex = 0
                Me.pbCheck5.TabStop = False
                '
                'btnCheckAgain
                '
                Me.btnCheckAgain.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.btnCheckAgain.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnCheckAgain.Location = New System.Drawing.Point(476, 842)
                Me.btnCheckAgain.Name = "btnCheckAgain"
                Me.btnCheckAgain.Size = New System.Drawing.Size(104, 23)
                Me.btnCheckAgain.TabIndex = 0
                Me.btnCheckAgain.Text = "Check again"
                Me.btnCheckAgain.UseVisualStyleBackColor = True
                '
                'chkAlwaysShow
                '
                Me.chkAlwaysShow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
                Me.chkAlwaysShow.AutoSize = True
                Me.chkAlwaysShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.chkAlwaysShow.Location = New System.Drawing.Point(12, 846)
                Me.chkAlwaysShow.Name = "chkAlwaysShow"
                Me.chkAlwaysShow.Size = New System.Drawing.Size(185, 17)
                Me.chkAlwaysShow.TabIndex = 51
                Me.chkAlwaysShow.Text = "Always show this screen at startup"
                Me.chkAlwaysShow.UseVisualStyleBackColor = True
                '
                'pnlChecks
                '
                Me.pnlChecks.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlChecks.AutoScroll = True
                Me.pnlChecks.Controls.Add(Me.pnlCheck1)
                Me.pnlChecks.Controls.Add(Me.pnlCheck2)
                Me.pnlChecks.Controls.Add(Me.pnlCheck3)
                Me.pnlChecks.Controls.Add(Me.pnlCheck6)
                Me.pnlChecks.Controls.Add(Me.pnlCheck5)
                Me.pnlChecks.Controls.Add(Me.pnlCheck4)
                Me.pnlChecks.Location = New System.Drawing.Point(12, 12)
                Me.pnlChecks.Name = "pnlChecks"
                Me.pnlChecks.Size = New System.Drawing.Size(568, 824)
                Me.pnlChecks.TabIndex = 52
                '
                'pnlCheck6
                '
                Me.pnlCheck6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlCheck6.Controls.Add(Me.txtCheck6)
                Me.pnlCheck6.Controls.Add(Me.lblCheck6)
                Me.pnlCheck6.Controls.Add(Me.pbCheck6)
                Me.pnlCheck6.Location = New System.Drawing.Point(3, 683)
                Me.pnlCheck6.Name = "pnlCheck6"
                Me.pnlCheck6.Size = New System.Drawing.Size(562, 130)
                Me.pnlCheck6.TabIndex = 50
                Me.pnlCheck6.Visible = False
                '
                'txtCheck6
                '
                Me.txtCheck6.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.txtCheck6.BackColor = System.Drawing.SystemColors.Control
                Me.txtCheck6.BorderStyle = System.Windows.Forms.BorderStyle.None
                Me.txtCheck6.Location = New System.Drawing.Point(129, 29)
                Me.txtCheck6.Multiline = True
                Me.txtCheck6.Name = "txtCheck6"
                Me.txtCheck6.ReadOnly = True
                Me.txtCheck6.Size = New System.Drawing.Size(430, 97)
                Me.txtCheck6.TabIndex = 2
                '
                'lblCheck6
                '
                Me.lblCheck6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.lblCheck6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblCheck6.Location = New System.Drawing.Point(112, 3)
                Me.lblCheck6.Name = "lblCheck6"
                Me.lblCheck6.Size = New System.Drawing.Size(447, 23)
                Me.lblCheck6.TabIndex = 1
                Me.lblCheck6.Text = "RDP check succeeded!"
                '
                'pbCheck6
                '
                Me.pbCheck6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
                Me.pbCheck6.Location = New System.Drawing.Point(3, 3)
                Me.pbCheck6.Name = "pbCheck6"
                Me.pbCheck6.Size = New System.Drawing.Size(72, 123)
                Me.pbCheck6.TabIndex = 0
                Me.pbCheck6.TabStop = False
                '
                'ComponentsCheck
                '
                Me.ClientSize = New System.Drawing.Size(592, 877)
                Me.Controls.Add(Me.pnlChecks)
                Me.Controls.Add(Me.chkAlwaysShow)
                Me.Controls.Add(Me.btnCheckAgain)
                Me.Icon = Global.mRemoteNG.My.Resources.Resources.ComponentsCheck_Icon
                Me.Name = "ComponentsCheck"
                Me.TabText = "Components Check"
                Me.Text = "Components Check"
                Me.pnlCheck1.ResumeLayout(False)
                Me.pnlCheck1.PerformLayout()
                CType(Me.pbCheck1, System.ComponentModel.ISupportInitialize).EndInit()
                Me.pnlCheck2.ResumeLayout(False)
                Me.pnlCheck2.PerformLayout()
                CType(Me.pbCheck2, System.ComponentModel.ISupportInitialize).EndInit()
                Me.pnlCheck3.ResumeLayout(False)
                Me.pnlCheck3.PerformLayout()
                CType(Me.pbCheck3, System.ComponentModel.ISupportInitialize).EndInit()
                Me.pnlCheck4.ResumeLayout(False)
                Me.pnlCheck4.PerformLayout()
                CType(Me.pbCheck4, System.ComponentModel.ISupportInitialize).EndInit()
                Me.pnlCheck5.ResumeLayout(False)
                Me.pnlCheck5.PerformLayout()
                CType(Me.pbCheck5, System.ComponentModel.ISupportInitialize).EndInit()
                Me.pnlChecks.ResumeLayout(False)
                Me.pnlCheck6.ResumeLayout(False)
                Me.pnlCheck6.PerformLayout()
                CType(Me.pbCheck6, System.ComponentModel.ISupportInitialize).EndInit()
                Me.ResumeLayout(False)
                Me.PerformLayout()

            End Sub
#End Region

#Region "Public Methods"
            Public Sub New(ByVal Panel As DockContent)
                Me.WindowType = Type.ComponentsCheck
                Me.DockPnl = Panel
                Me.InitializeComponent()
            End Sub
#End Region

#Region "Form Stuff"
            Private Sub ComponentsCheck_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
                ApplyLanguage()

                chkAlwaysShow.Checked = My.Settings.StartupComponentsCheck
                CheckComponents()
            End Sub

            Private Sub ApplyLanguage()
                TabText = My.Resources.strComponentsCheck
                Text = My.Resources.strComponentsCheck
                chkAlwaysShow.Text = My.Resources.strCcAlwaysShowScreen
                btnCheckAgain.Text = My.Resources.strCcCheckAgain
            End Sub

            Private Sub btnCheckAgain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckAgain.Click
                CheckComponents()
            End Sub

            Private Sub chkAlwaysShow_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAlwaysShow.CheckedChanged
                My.Settings.StartupComponentsCheck = chkAlwaysShow.Checked
                My.Settings.Save()
            End Sub
#End Region

#Region "Private Methods"
            Private Sub CheckComponents()
                Dim errorMsg As String = My.Resources.strCcNotInstalledProperly

                pnlCheck1.Visible = True
                pnlCheck2.Visible = True
                pnlCheck3.Visible = True
                pnlCheck4.Visible = True
                pnlCheck5.Visible = True
                pnlCheck6.Visible = True


                Dim RDP As AxMSTSCLib.AxMsRdpClient6NotSafeForScripting = Nothing

                Try
                    RDP = New AxMSTSCLib.AxMsRdpClient6NotSafeForScripting
                    RDP.CreateControl()

                    Do Until RDP.Created
                        Thread.Sleep(10)
                        System.Windows.Forms.Application.DoEvents()
                    Loop

                    pbCheck1.Image = My.Resources.Good_Symbol
                    lblCheck1.ForeColor = Color.DarkOliveGreen
                    lblCheck1.Text = "RDP (Remote Desktop) " & My.Resources.strCcCheckSucceeded
                    txtCheck1.Text = String.Format(My.Resources.strCcRDPOK, RDP.Version)
                Catch ex As Exception
                    pbCheck1.Image = My.Resources.Bad_Symbol
                    lblCheck1.ForeColor = Color.Firebrick
                    lblCheck1.Text = "RDP (Remote Desktop) " & My.Resources.strCcCheckFailed
                    txtCheck1.Text = My.Resources.strCcRDPFailed

                    MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "RDP " & errorMsg, True)
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, ex.Message, True)
                End Try

                If RDP IsNot Nothing Then RDP.Dispose()


                Dim VNC As VncSharp.RemoteDesktop = Nothing

                Try
                    VNC = New VncSharp.RemoteDesktop
                    VNC.CreateControl()

                    Do Until VNC.Created
                        Thread.Sleep(10)
                        System.Windows.Forms.Application.DoEvents()
                    Loop

                    pbCheck2.Image = My.Resources.Good_Symbol
                    lblCheck2.ForeColor = Color.DarkOliveGreen
                    lblCheck2.Text = "VNC (Virtual Network Computing) " & My.Resources.strCcCheckSucceeded
                    txtCheck2.Text = String.Format(My.Resources.strCcVNCOK, VNC.ProductVersion)
                Catch ex As Exception
                    pbCheck2.Image = My.Resources.Bad_Symbol
                    lblCheck2.ForeColor = Color.Firebrick
                    lblCheck2.Text = "VNC (Virtual Network Computing) " & My.Resources.strCcCheckFailed
                    txtCheck2.Text = My.Resources.strCcVNCFailed

                    MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "VNC " & errorMsg, True)
                    'mC.AddMessage(Messages.MessageClass.ErrorMsg, ex.Message, True)
                End Try

                If VNC IsNot Nothing Then VNC.Dispose()


                Dim pPath As String = ""
                If My.Settings.UseCustomPuttyPath = False Then
                    pPath = My.Application.Info.DirectoryPath & "\putty.exe"
                Else
                    pPath = My.Settings.CustomPuttyPath
                End If

                If File.Exists(pPath) Then
                    pbCheck3.Image = My.Resources.Good_Symbol
                    lblCheck3.ForeColor = Color.DarkOliveGreen
                    lblCheck3.Text = "PuTTY (SSH/Telnet/Rlogin/RAW) " & My.Resources.strCcCheckSucceeded
                    txtCheck3.Text = My.Resources.strCcPuttyOK
                Else
                    pbCheck3.Image = My.Resources.Bad_Symbol
                    lblCheck3.ForeColor = Color.Firebrick
                    lblCheck3.Text = "PuTTY (SSH/Telnet/Rlogin/RAW) " & My.Resources.strCcCheckFailed
                    txtCheck3.Text = My.Resources.strCcPuttyFailed

                    MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "PuTTY " & errorMsg, True)
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "File " & pPath & " does not exist.", True)
                End If


                Dim ICA As AxWFICALib.AxICAClient = Nothing

                Try
                    ICA = New AxWFICALib.AxICAClient
                    ICA.Parent = Me
                    ICA.CreateControl()

                    Do Until ICA.Created
                        Thread.Sleep(10)
                        System.Windows.Forms.Application.DoEvents()
                    Loop

                    pbCheck4.Image = My.Resources.Good_Symbol
                    lblCheck4.ForeColor = Color.DarkOliveGreen
                    lblCheck4.Text = "ICA (Citrix ICA) " & My.Resources.strCcCheckSucceeded
                    txtCheck4.Text = String.Format(My.Resources.strCcICAOK, ICA.Version)
                Catch ex As Exception
                    pbCheck4.Image = My.Resources.Bad_Symbol
                    lblCheck4.ForeColor = Color.Firebrick
                    lblCheck4.Text = "ICA (Citrix ICA) " & My.Resources.strCcCheckFailed
                    txtCheck4.Text = My.Resources.strCcICAFailed

                    MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "ICA " & errorMsg, True)
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, ex.Message, True)
                End Try

                If ICA IsNot Nothing Then ICA.Dispose()


                Dim GeckoBad As Boolean = False

                If My.Settings.XULRunnerPath = "" Then
                    GeckoBad = True
                End If

                If Directory.Exists(My.Settings.XULRunnerPath) Then
                    If File.Exists(Path.Combine(My.Settings.XULRunnerPath, "xpcom.dll")) = False Then
                        GeckoBad = True
                    End If
                Else
                    GeckoBad = True
                End If

                If GeckoBad = False Then
                    pbCheck5.Image = My.Resources.Good_Symbol
                    lblCheck5.ForeColor = Color.DarkOliveGreen
                    lblCheck5.Text = "Gecko (Firefox) Rendering Engine (HTTP/S) " & My.Resources.strCcCheckSucceeded
                    txtCheck5.Text = My.Resources.strCcGeckoOK
                Else
                    pbCheck5.Image = My.Resources.Bad_Symbol
                    lblCheck5.ForeColor = Color.Firebrick
                    lblCheck5.Text = "Gecko (Firefox) Rendering Engine (HTTP/S) " & My.Resources.strCcCheckFailed
                    txtCheck5.Text = My.Resources.strCcGeckoFailed

                    MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "Gecko " & errorMsg, True)
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "XULrunner was not found in " & My.Settings.XULRunnerPath, True)
                End If


                Dim eol As EOLWTSCOM.WTSCOM = Nothing

                Try
                    eol = New EOLWTSCOM.WTSCOM()

                    pbCheck6.Image = My.Resources.Good_Symbol
                    lblCheck6.ForeColor = Color.DarkOliveGreen
                    lblCheck6.Text = "(RDP) Sessions " & My.Resources.strCcCheckSucceeded
                    txtCheck6.Text = My.Resources.strCcEOLOK
                Catch ex As Exception
                    pbCheck6.Image = My.Resources.Bad_Symbol
                    lblCheck6.ForeColor = Color.Firebrick
                    lblCheck6.Text = "(RDP) Sessions " & My.Resources.strCcCheckFailed
                    txtCheck6.Text = My.Resources.strCcEOLFailed

                    MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "EOLWTSCOM " & errorMsg, True)
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, ex.Message, True)
                End Try
            End Sub
#End Region

        End Class
    End Namespace
End Namespace