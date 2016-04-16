Namespace UI
    Namespace Window
        Partial Public Class PortScan
            Inherits UI.Window.Base
#Region " Windows Form Designer generated code "

            Friend WithEvents lblEndIP As System.Windows.Forms.Label
            Friend WithEvents lblStartIP As System.Windows.Forms.Label
            Friend WithEvents btnScan As System.Windows.Forms.Button
            Friend WithEvents ipEnd As IPTextBox.IPTextBox
            Friend WithEvents lvHosts As System.Windows.Forms.ListView
            Friend WithEvents clmHost As System.Windows.Forms.ColumnHeader
            Friend WithEvents clmSSH As System.Windows.Forms.ColumnHeader
            Friend WithEvents clmTelnet As System.Windows.Forms.ColumnHeader
            Friend WithEvents clmHTTP As System.Windows.Forms.ColumnHeader
            Friend WithEvents clmHTTPS As System.Windows.Forms.ColumnHeader
            Friend WithEvents clmRlogin As System.Windows.Forms.ColumnHeader
            Friend WithEvents clmRDP As System.Windows.Forms.ColumnHeader
            Friend WithEvents clmVNC As System.Windows.Forms.ColumnHeader
            Friend WithEvents clmOpenPorts As System.Windows.Forms.ColumnHeader
            Friend WithEvents clmClosedPorts As System.Windows.Forms.ColumnHeader
            Friend WithEvents prgBar As System.Windows.Forms.ProgressBar
            Friend WithEvents lblOnlyImport As System.Windows.Forms.Label
            Friend WithEvents cbProtocol As System.Windows.Forms.ComboBox
            Friend WithEvents pnlPorts As System.Windows.Forms.Panel
            Friend WithEvents portEnd As System.Windows.Forms.NumericUpDown
            Friend WithEvents portStart As System.Windows.Forms.NumericUpDown
            Friend WithEvents Label2 As System.Windows.Forms.Label
            Friend WithEvents Label1 As System.Windows.Forms.Label
            Friend WithEvents btnImport As System.Windows.Forms.Button
            Friend WithEvents ipStart As IPTextBox.IPTextBox

            Private Sub InitializeComponent()
                Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PortScan))
                Me.ipStart = New IPTextBox.IPTextBox()
                Me.ipEnd = New IPTextBox.IPTextBox()
                Me.lblStartIP = New System.Windows.Forms.Label()
                Me.lblEndIP = New System.Windows.Forms.Label()
                Me.btnScan = New System.Windows.Forms.Button()
                Me.lvHosts = New System.Windows.Forms.ListView()
                Me.btnImport = New System.Windows.Forms.Button()
                Me.cbProtocol = New System.Windows.Forms.ComboBox()
                Me.lblOnlyImport = New System.Windows.Forms.Label()
                Me.clmHost = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
                Me.clmSSH = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
                Me.clmTelnet = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
                Me.clmHTTP = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
                Me.clmHTTPS = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
                Me.clmRlogin = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
                Me.clmRDP = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
                Me.clmVNC = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
                Me.clmOpenPorts = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
                Me.clmClosedPorts = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
                Me.prgBar = New System.Windows.Forms.ProgressBar()
                Me.pnlPorts = New System.Windows.Forms.Panel()
                Me.portEnd = New System.Windows.Forms.NumericUpDown()
                Me.portStart = New System.Windows.Forms.NumericUpDown()
                Me.Label2 = New System.Windows.Forms.Label()
                Me.Label1 = New System.Windows.Forms.Label()
                Me.pnlImport = New System.Windows.Forms.Panel()
                Me.pnlPorts.SuspendLayout()
                CType(Me.portEnd, System.ComponentModel.ISupportInitialize).BeginInit()
                CType(Me.portStart, System.ComponentModel.ISupportInitialize).BeginInit()
                Me.pnlImport.SuspendLayout()
                Me.SuspendLayout()
                '
                'ipStart
                '
                Me.ipStart.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
                Me.ipStart.Location = New System.Drawing.Point(12, 25)
                Me.ipStart.Name = "ipStart"
                Me.ipStart.Size = New System.Drawing.Size(113, 20)
                Me.ipStart.TabIndex = 10
                '
                'ipEnd
                '
                Me.ipEnd.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
                Me.ipEnd.Location = New System.Drawing.Point(131, 25)
                Me.ipEnd.Name = "ipEnd"
                Me.ipEnd.Size = New System.Drawing.Size(113, 20)
                Me.ipEnd.TabIndex = 15
                '
                'lblStartIP
                '
                Me.lblStartIP.AutoSize = True
                Me.lblStartIP.Location = New System.Drawing.Point(9, 9)
                Me.lblStartIP.Name = "lblStartIP"
                Me.lblStartIP.Size = New System.Drawing.Size(45, 13)
                Me.lblStartIP.TabIndex = 0
                Me.lblStartIP.Text = "Start IP:"
                '
                'lblEndIP
                '
                Me.lblEndIP.AutoSize = True
                Me.lblEndIP.Location = New System.Drawing.Point(128, 9)
                Me.lblEndIP.Name = "lblEndIP"
                Me.lblEndIP.Size = New System.Drawing.Size(42, 13)
                Me.lblEndIP.TabIndex = 5
                Me.lblEndIP.Text = "End IP:"
                '
                'btnScan
                '
                Me.btnScan.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.btnScan.Image = Global.mRemote3G.My.Resources.Resources.Search
                Me.btnScan.Location = New System.Drawing.Point(420, 9)
                Me.btnScan.Name = "btnScan"
                Me.btnScan.Size = New System.Drawing.Size(86, 58)
                Me.btnScan.TabIndex = 20
                Me.btnScan.Text = "&Scan"
                Me.btnScan.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
                Me.btnScan.UseVisualStyleBackColor = True
                '
                'lvHosts
                '
                Me.lvHosts.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.lvHosts.FullRowSelect = True
                Me.lvHosts.GridLines = True
                Me.lvHosts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
                Me.lvHosts.HideSelection = False
                Me.lvHosts.Location = New System.Drawing.Point(12, 73)
                Me.lvHosts.Name = "lvHosts"
                Me.lvHosts.Size = New System.Drawing.Size(494, 214)
                Me.lvHosts.TabIndex = 26
                Me.lvHosts.UseCompatibleStateImageBehavior = False
                Me.lvHosts.View = System.Windows.Forms.View.Details
                '
                'btnImport
                '
                Me.btnImport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.btnImport.Location = New System.Drawing.Point(419, 6)
                Me.btnImport.Name = "btnImport"
                Me.btnImport.Size = New System.Drawing.Size(75, 23)
                Me.btnImport.TabIndex = 101
                Me.btnImport.Text = "&Import"
                Me.btnImport.UseVisualStyleBackColor = True
                '
                'cbProtocol
                '
                Me.cbProtocol.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
                Me.cbProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
                Me.cbProtocol.FormattingEnabled = True
                Me.cbProtocol.Items.AddRange(New Object() {"SSH2", "Telnet", "HTTP", "HTTPS", "Rlogin", "RDP", "VNC"})
                Me.cbProtocol.Location = New System.Drawing.Point(98, 8)
                Me.cbProtocol.Name = "cbProtocol"
                Me.cbProtocol.Size = New System.Drawing.Size(122, 21)
                Me.cbProtocol.TabIndex = 28
                '
                'lblOnlyImport
                '
                Me.lblOnlyImport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
                Me.lblOnlyImport.AutoSize = True
                Me.lblOnlyImport.Location = New System.Drawing.Point(0, 11)
                Me.lblOnlyImport.Name = "lblOnlyImport"
                Me.lblOnlyImport.Size = New System.Drawing.Size(92, 13)
                Me.lblOnlyImport.TabIndex = 1
                Me.lblOnlyImport.Text = "Protocol to import:"
                '
                'clmHost
                '
                Me.clmHost.Text = "Hostname/IP"
                Me.clmHost.Width = 130
                '
                'clmSSH
                '
                Me.clmSSH.Text = "SSH"
                Me.clmSSH.Width = 50
                '
                'clmTelnet
                '
                Me.clmTelnet.Text = "Telnet"
                Me.clmTelnet.Width = 50
                '
                'clmHTTP
                '
                Me.clmHTTP.Text = "HTTP"
                Me.clmHTTP.Width = 50
                '
                'clmHTTPS
                '
                Me.clmHTTPS.Text = "HTTPS"
                Me.clmHTTPS.Width = 50
                '
                'clmRlogin
                '
                Me.clmRlogin.Text = "Rlogin"
                Me.clmRlogin.Width = 50
                '
                'clmRDP
                '
                Me.clmRDP.Text = "RDP"
                Me.clmRDP.Width = 50
                '
                'clmVNC
                '
                Me.clmVNC.Text = "VNC"
                Me.clmVNC.Width = 50
                '
                'clmOpenPorts
                '
                Me.clmOpenPorts.Text = "Open Ports"
                Me.clmOpenPorts.Width = 150
                '
                'clmClosedPorts
                '
                Me.clmClosedPorts.Text = "Closed Ports"
                Me.clmClosedPorts.Width = 150
                '
                'prgBar
                '
                Me.prgBar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.prgBar.Location = New System.Drawing.Point(12, 51)
                Me.prgBar.Name = "prgBar"
                Me.prgBar.Size = New System.Drawing.Size(402, 16)
                Me.prgBar.Step = 1
                Me.prgBar.TabIndex = 28
                '
                'pnlPorts
                '
                Me.pnlPorts.Controls.Add(Me.portEnd)
                Me.pnlPorts.Controls.Add(Me.portStart)
                Me.pnlPorts.Controls.Add(Me.Label2)
                Me.pnlPorts.Controls.Add(Me.Label1)
                Me.pnlPorts.Location = New System.Drawing.Point(268, 7)
                Me.pnlPorts.Name = "pnlPorts"
                Me.pnlPorts.Size = New System.Drawing.Size(152, 38)
                Me.pnlPorts.TabIndex = 18
                '
                'portEnd
                '
                Me.portEnd.Location = New System.Drawing.Point(79, 18)
                Me.portEnd.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
                Me.portEnd.Name = "portEnd"
                Me.portEnd.Size = New System.Drawing.Size(67, 20)
                Me.portEnd.TabIndex = 15
                '
                'portStart
                '
                Me.portStart.Location = New System.Drawing.Point(6, 18)
                Me.portStart.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
                Me.portStart.Name = "portStart"
                Me.portStart.Size = New System.Drawing.Size(67, 20)
                Me.portStart.TabIndex = 5
                '
                'Label2
                '
                Me.Label2.AutoSize = True
                Me.Label2.Location = New System.Drawing.Point(76, 2)
                Me.Label2.Name = "Label2"
                Me.Label2.Size = New System.Drawing.Size(51, 13)
                Me.Label2.TabIndex = 10
                Me.Label2.Text = "End Port:"
                '
                'Label1
                '
                Me.Label1.AutoSize = True
                Me.Label1.Location = New System.Drawing.Point(3, 2)
                Me.Label1.Name = "Label1"
                Me.Label1.Size = New System.Drawing.Size(54, 13)
                Me.Label1.TabIndex = 0
                Me.Label1.Text = "Start Port:"
                '
                'pnlImport
                '
                Me.pnlImport.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlImport.Controls.Add(Me.btnImport)
                Me.pnlImport.Controls.Add(Me.lblOnlyImport)
                Me.pnlImport.Controls.Add(Me.cbProtocol)
                Me.pnlImport.Location = New System.Drawing.Point(12, 287)
                Me.pnlImport.Name = "pnlImport"
                Me.pnlImport.Size = New System.Drawing.Size(494, 29)
                Me.pnlImport.TabIndex = 102
                '
                'PortScan
                '
                Me.AcceptButton = Me.btnImport
                Me.ClientSize = New System.Drawing.Size(518, 328)
                Me.Controls.Add(Me.pnlImport)
                Me.Controls.Add(Me.lvHosts)
                Me.Controls.Add(Me.pnlPorts)
                Me.Controls.Add(Me.prgBar)
                Me.Controls.Add(Me.btnScan)
                Me.Controls.Add(Me.lblEndIP)
                Me.Controls.Add(Me.lblStartIP)
                Me.Controls.Add(Me.ipEnd)
                Me.Controls.Add(Me.ipStart)
                Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
                Me.Name = "PortScan"
                Me.TabText = "Port Scan"
                Me.Text = "Port Scan"
                Me.pnlPorts.ResumeLayout(False)
                Me.pnlPorts.PerformLayout()
                CType(Me.portEnd, System.ComponentModel.ISupportInitialize).EndInit()
                CType(Me.portStart, System.ComponentModel.ISupportInitialize).EndInit()
                Me.pnlImport.ResumeLayout(False)
                Me.pnlImport.PerformLayout()
                Me.ResumeLayout(False)
                Me.PerformLayout()

            End Sub
            Friend WithEvents pnlImport As System.Windows.Forms.Panel
#End Region
        End Class
    End Namespace
End Namespace
