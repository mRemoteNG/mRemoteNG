Imports WeifenLuo.WinFormsUI.Docking
Imports mRemoteNG.App.Runtime

Namespace UI
    Namespace Window
        Public Class PortScan
            Inherits UI.Window.Base

#Region "Form Init"
            Friend WithEvents lblEndIP As System.Windows.Forms.Label
            Friend WithEvents lblStartIP As System.Windows.Forms.Label
            Friend WithEvents btnScan As System.Windows.Forms.Button
            Friend WithEvents pnlDivider As System.Windows.Forms.Panel
            Friend WithEvents ipEnd As IPTextBox.IPTextBox
            Friend WithEvents splContainer As System.Windows.Forms.SplitContainer
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
            Friend WithEvents btnCancel As System.Windows.Forms.Button
            Friend WithEvents ipStart As IPTextBox.IPTextBox

            Private Sub InitializeComponent()
                Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PortScan))
                Me.ipStart = New IPTextBox.IPTextBox
                Me.ipEnd = New IPTextBox.IPTextBox
                Me.lblStartIP = New System.Windows.Forms.Label
                Me.lblEndIP = New System.Windows.Forms.Label
                Me.btnScan = New System.Windows.Forms.Button
                Me.pnlDivider = New System.Windows.Forms.Panel
                Me.splContainer = New System.Windows.Forms.SplitContainer
                Me.lvHosts = New System.Windows.Forms.ListView
                Me.btnCancel = New System.Windows.Forms.Button
                Me.btnImport = New System.Windows.Forms.Button
                Me.cbProtocol = New System.Windows.Forms.ComboBox
                Me.lblOnlyImport = New System.Windows.Forms.Label
                Me.clmHost = New System.Windows.Forms.ColumnHeader
                Me.clmSSH = New System.Windows.Forms.ColumnHeader
                Me.clmTelnet = New System.Windows.Forms.ColumnHeader
                Me.clmHTTP = New System.Windows.Forms.ColumnHeader
                Me.clmHTTPS = New System.Windows.Forms.ColumnHeader
                Me.clmRlogin = New System.Windows.Forms.ColumnHeader
                Me.clmRDP = New System.Windows.Forms.ColumnHeader
                Me.clmVNC = New System.Windows.Forms.ColumnHeader
                Me.clmOpenPorts = New System.Windows.Forms.ColumnHeader
                Me.clmClosedPorts = New System.Windows.Forms.ColumnHeader
                Me.prgBar = New System.Windows.Forms.ProgressBar
                Me.pnlPorts = New System.Windows.Forms.Panel
                Me.portEnd = New System.Windows.Forms.NumericUpDown
                Me.portStart = New System.Windows.Forms.NumericUpDown
                Me.Label2 = New System.Windows.Forms.Label
                Me.Label1 = New System.Windows.Forms.Label
                Me.splContainer.Panel1.SuspendLayout()
                Me.splContainer.Panel2.SuspendLayout()
                Me.splContainer.SuspendLayout()
                Me.pnlPorts.SuspendLayout()
                CType(Me.portEnd, System.ComponentModel.ISupportInitialize).BeginInit()
                CType(Me.portStart, System.ComponentModel.ISupportInitialize).BeginInit()
                Me.SuspendLayout()
                '
                'ipStart
                '
                Me.ipStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.ipStart.Location = New System.Drawing.Point(7, 24)
                Me.ipStart.Name = "ipStart"
                Me.ipStart.Size = New System.Drawing.Size(113, 20)
                Me.ipStart.TabIndex = 10
                '
                'ipEnd
                '
                Me.ipEnd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.ipEnd.Location = New System.Drawing.Point(133, 24)
                Me.ipEnd.Name = "ipEnd"
                Me.ipEnd.Size = New System.Drawing.Size(113, 20)
                Me.ipEnd.TabIndex = 15
                '
                'lblStartIP
                '
                Me.lblStartIP.AutoSize = True
                Me.lblStartIP.Location = New System.Drawing.Point(4, 8)
                Me.lblStartIP.Name = "lblStartIP"
                Me.lblStartIP.Size = New System.Drawing.Size(45, 13)
                Me.lblStartIP.TabIndex = 0
                Me.lblStartIP.Text = "Start IP:"
                '
                'lblEndIP
                '
                Me.lblEndIP.AutoSize = True
                Me.lblEndIP.Location = New System.Drawing.Point(130, 8)
                Me.lblEndIP.Name = "lblEndIP"
                Me.lblEndIP.Size = New System.Drawing.Size(42, 13)
                Me.lblEndIP.TabIndex = 5
                Me.lblEndIP.Text = "End IP:"
                '
                'btnScan
                '
                Me.btnScan.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.btnScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnScan.Image = Global.mRemoteNG.My.Resources.Resources.Search
                Me.btnScan.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
                Me.btnScan.Location = New System.Drawing.Point(448, 8)
                Me.btnScan.Name = "btnScan"
                Me.btnScan.Size = New System.Drawing.Size(86, 58)
                Me.btnScan.TabIndex = 20
                Me.btnScan.Text = "&Scan"
                Me.btnScan.TextAlign = System.Drawing.ContentAlignment.MiddleRight
                Me.btnScan.UseVisualStyleBackColor = True
                '
                'pnlDivider
                '
                Me.pnlDivider.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlDivider.BackColor = System.Drawing.Color.DimGray
                Me.pnlDivider.Location = New System.Drawing.Point(0, 0)
                Me.pnlDivider.Name = "pnlDivider"
                Me.pnlDivider.Size = New System.Drawing.Size(542, 4)
                Me.pnlDivider.TabIndex = 20
                '
                'splContainer
                '
                Me.splContainer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.splContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
                Me.splContainer.IsSplitterFixed = True
                Me.splContainer.Location = New System.Drawing.Point(0, 74)
                Me.splContainer.Name = "splContainer"
                Me.splContainer.Orientation = System.Windows.Forms.Orientation.Horizontal
                '
                'splContainer.Panel1
                '
                Me.splContainer.Panel1.Controls.Add(Me.lvHosts)
                Me.splContainer.Panel1.Controls.Add(Me.pnlDivider)
                '
                'splContainer.Panel2
                '
                Me.splContainer.Panel2.Controls.Add(Me.btnCancel)
                Me.splContainer.Panel2.Controls.Add(Me.btnImport)
                Me.splContainer.Panel2.Controls.Add(Me.cbProtocol)
                Me.splContainer.Panel2.Controls.Add(Me.lblOnlyImport)
                Me.splContainer.Size = New System.Drawing.Size(542, 256)
                Me.splContainer.SplitterDistance = 213
                Me.splContainer.TabIndex = 27
                '
                'lvHosts
                '
                Me.lvHosts.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.lvHosts.BorderStyle = System.Windows.Forms.BorderStyle.None
                Me.lvHosts.FullRowSelect = True
                Me.lvHosts.GridLines = True
                Me.lvHosts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
                Me.lvHosts.HideSelection = False
                Me.lvHosts.Location = New System.Drawing.Point(0, 7)
                Me.lvHosts.Name = "lvHosts"
                Me.lvHosts.Size = New System.Drawing.Size(542, 205)
                Me.lvHosts.TabIndex = 26
                Me.lvHosts.UseCompatibleStateImageBehavior = False
                Me.lvHosts.View = System.Windows.Forms.View.Details
                '
                'btnCancel
                '
                Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
                Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnCancel.Location = New System.Drawing.Point(459, 8)
                Me.btnCancel.Name = "btnCancel"
                Me.btnCancel.Size = New System.Drawing.Size(75, 23)
                Me.btnCancel.TabIndex = 111
                Me.btnCancel.Text = "&Cancel"
                Me.btnCancel.UseVisualStyleBackColor = True
                '
                'btnImport
                '
                Me.btnImport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.btnImport.Location = New System.Drawing.Point(378, 8)
                Me.btnImport.Name = "btnImport"
                Me.btnImport.Size = New System.Drawing.Size(75, 23)
                Me.btnImport.TabIndex = 101
                Me.btnImport.Text = "&Import"
                Me.btnImport.UseVisualStyleBackColor = True
                '
                'cbProtocol
                '
                Me.cbProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
                Me.cbProtocol.FlatStyle = System.Windows.Forms.FlatStyle.Flat
                Me.cbProtocol.FormattingEnabled = True
                Me.cbProtocol.Items.AddRange(New Object() {"SSH2", "Telnet", "HTTP", "HTTPS", "Rlogin", "RDP", "VNC"})
                Me.cbProtocol.Location = New System.Drawing.Point(111, 9)
                Me.cbProtocol.Name = "cbProtocol"
                Me.cbProtocol.Size = New System.Drawing.Size(122, 21)
                Me.cbProtocol.TabIndex = 28
                '
                'lblOnlyImport
                '
                Me.lblOnlyImport.AutoSize = True
                Me.lblOnlyImport.Location = New System.Drawing.Point(3, 12)
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
                Me.prgBar.Location = New System.Drawing.Point(7, 50)
                Me.prgBar.Name = "prgBar"
                Me.prgBar.Size = New System.Drawing.Size(432, 16)
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
                Me.portEnd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.portEnd.Location = New System.Drawing.Point(81, 17)
                Me.portEnd.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
                Me.portEnd.Name = "portEnd"
                Me.portEnd.Size = New System.Drawing.Size(67, 20)
                Me.portEnd.TabIndex = 15
                '
                'portStart
                '
                Me.portStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                Me.portStart.Location = New System.Drawing.Point(3, 17)
                Me.portStart.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
                Me.portStart.Name = "portStart"
                Me.portStart.Size = New System.Drawing.Size(67, 20)
                Me.portStart.TabIndex = 5
                '
                'Label2
                '
                Me.Label2.AutoSize = True
                Me.Label2.Location = New System.Drawing.Point(78, 1)
                Me.Label2.Name = "Label2"
                Me.Label2.Size = New System.Drawing.Size(51, 13)
                Me.Label2.TabIndex = 10
                Me.Label2.Text = "End Port:"
                '
                'Label1
                '
                Me.Label1.AutoSize = True
                Me.Label1.Location = New System.Drawing.Point(0, 2)
                Me.Label1.Name = "Label1"
                Me.Label1.Size = New System.Drawing.Size(54, 13)
                Me.Label1.TabIndex = 0
                Me.Label1.Text = "Start Port:"
                '
                'PortScan
                '
                Me.AcceptButton = Me.btnImport
                Me.CancelButton = Me.btnCancel
                Me.ClientSize = New System.Drawing.Size(542, 330)
                Me.Controls.Add(Me.pnlPorts)
                Me.Controls.Add(Me.prgBar)
                Me.Controls.Add(Me.splContainer)
                Me.Controls.Add(Me.btnScan)
                Me.Controls.Add(Me.lblEndIP)
                Me.Controls.Add(Me.lblStartIP)
                Me.Controls.Add(Me.ipEnd)
                Me.Controls.Add(Me.ipStart)
                Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
                Me.Name = "PortScan"
                Me.TabText = "Port Scan"
                Me.Text = "Port Scan"
                Me.splContainer.Panel1.ResumeLayout(False)
                Me.splContainer.Panel2.ResumeLayout(False)
                Me.splContainer.Panel2.PerformLayout()
                Me.splContainer.ResumeLayout(False)
                Me.pnlPorts.ResumeLayout(False)
                Me.pnlPorts.PerformLayout()
                CType(Me.portEnd, System.ComponentModel.ISupportInitialize).EndInit()
                CType(Me.portStart, System.ComponentModel.ISupportInitialize).EndInit()
                Me.ResumeLayout(False)
                Me.PerformLayout()

            End Sub
#End Region

#Region "Public Methods"
            Public Sub New(ByVal Panel As DockContent, ByVal Mode As Tools.PortScan.PortScanMode)
                Me.WindowType = Type.PortScan
                Me.DockPnl = Panel
                Me.InitializeComponent()
                psMode = Mode
            End Sub
#End Region

            Private psMode As Tools.PortScan.PortScanMode
            Private pScanner As Tools.PortScan.Scanner
            Private scanning As Boolean = False

#Region "Form Stuff"
            Private Sub PortScan_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
                ApplyLanguage()

                Try
                    If psMode = Tools.PortScan.PortScanMode.Normal Then
                        Me.lvHosts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.clmHost, Me.clmOpenPorts, Me.clmClosedPorts})
                        pnlPorts.Visible = True
                        splContainer.Panel2Collapsed = True
                    Else
                        Me.lvHosts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.clmHost, Me.clmSSH, Me.clmTelnet, Me.clmHTTP, Me.clmHTTPS, Me.clmRlogin, Me.clmRDP, Me.clmVNC})
                        pnlPorts.Visible = False
                        splContainer.Panel2Collapsed = False
                        cbProtocol.SelectedIndex = 0
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strPortScanCouldNotLoadPanel & vbNewLine & vbNewLine & ex.Message)
                End Try
            End Sub

            Private Sub ApplyLanguage()
                lblStartIP.Text = My.Resources.strStartIP & ":"
                lblEndIP.Text = My.Resources.strEndIP & ":"
                btnScan.Text = My.Resources.strButtonScan
                btnCancel.Text = My.Resources.strButtonCancel
                btnImport.Text = My.Resources.strButtonImport
                lblOnlyImport.Text = My.Resources.strProtocolToImport & ":"
                clmHost.Text = My.Resources.strColumnHostnameIP
                clmOpenPorts.Text = My.Resources.strOpenPorts
                clmClosedPorts.Text = My.Resources.strClosedPorts
                Label2.Text = My.Resources.strEndPort & ":"
                Label1.Text = My.Resources.strStartPort & ":"
                TabText = My.Resources.strMenuPortScan
                Text = My.Resources.strMenuPortScan
            End Sub

            Private Sub btnScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScan.Click
                If scanning = True Then
                    StopScan()
                Else
                    If ipOK() Then
                        StartScan()
                    Else
                        MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, My.Resources.strCannotStartPortScan)
                    End If
                End If
            End Sub

            Private Sub btnImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImport.Click
                Dim prot As mRemoteNG.Connection.Protocol.Protocols = Tools.Misc.StringToEnum(GetType(mRemoteNG.Connection.Protocol.Protocols), cbProtocol.SelectedItem)

                Dim arrHosts As New ArrayList
                For Each lvItem As ListViewItem In lvHosts.SelectedItems
                    arrHosts.Add(lvItem.Tag)
                Next

                App.Runtime.ImportConnectionsFromPortScan(arrHosts, prot)

                'Me.DialogResult = System.Windows.Forms.DialogResult.OK
                'Me.Close()
            End Sub

            Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
                Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
                Me.Close()
            End Sub
#End Region

#Region "Threading Delegates"
            Private Delegate Sub AddListViewItemCB(ByVal Item As ListViewItem)
            Private Sub AddListViewItem(ByVal Item As ListViewItem)
                If lvHosts.InvokeRequired Then
                    Dim d As New AddListViewItemCB(AddressOf AddListViewItem)
                    Me.Invoke(d, New Object() {Item})
                Else
                    lvHosts.Items.Add(Item)
                    Item.EnsureVisible()
                End If
            End Sub

            Private Delegate Sub SwitchButtonTextCB()
            Private Sub SwitchButtonText()
                If btnScan.InvokeRequired Then
                    Dim d As New SwitchButtonTextCB(AddressOf SwitchButtonText)
                    Me.Invoke(d)
                Else
                    If scanning = True Then
                        btnScan.Text = My.Resources.strButtonStop
                    Else
                        btnScan.Text = My.Resources.strButtonScan
                    End If
                End If
            End Sub

            Private Delegate Sub SetPrgBarCB(ByVal Value As Integer, ByVal Max As Integer)
            Private Sub SetPrgBar(ByVal Value As Integer, ByVal Max As Integer)
                If prgBar.InvokeRequired Then
                    Dim d As New SetPrgBarCB(AddressOf SetPrgBar)
                    Me.Invoke(d, New Object() {Value, Max})
                Else
                    prgBar.Maximum = Max
                    prgBar.Value = Value
                End If
            End Sub
#End Region

#Region "Methods"
            Private Sub StartScan()
                Try
                    scanning = True
                    SwitchButtonText()
                    SetPrgBar(0, 100)
                    lvHosts.Items.Clear()

                    If psMode = Tools.PortScan.PortScanMode.Import Then
                        pScanner = New Tools.PortScan.Scanner(ipStart.Text, ipEnd.Text)
                    Else
                        pScanner = New Tools.PortScan.Scanner(ipStart.Text, ipEnd.Text, portStart.Value, portEnd.Value)
                    End If

                    AddHandler pScanner.BeginHostScan, AddressOf Event_BeginHostScan
                    AddHandler pScanner.HostScanned, AddressOf Event_HostScanned
                    AddHandler pScanner.ScanComplete, AddressOf Event_ScanComplete

                    pScanner.StartScan()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "StartScan failed (UI.Window.PortScan)" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub StopScan()
                pScanner.StopScan()
                scanning = False
                SwitchButtonText()
            End Sub

            Private Function ipOK() As Boolean
                If ipStart.Octet1 = "" Then Return False
                If ipStart.Octet2 = "" Then Return False
                If ipStart.Octet3 = "" Then Return False
                If ipStart.Octet4 = "" Then Return False

                If ipEnd.Octet1 = "" Then Return False
                If ipEnd.Octet2 = "" Then Return False
                If ipEnd.Octet3 = "" Then Return False
                If ipEnd.Octet4 = "" Then Return False

                Return True
            End Function
#End Region

#Region "Event Handlers"
            Private Sub Event_BeginHostScan(ByVal Host As String)
                MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, "Scanning " & Host, True)
            End Sub

            Private Sub Event_HostScanned(ByVal Host As Tools.PortScan.ScanHost, ByVal AlreadyScanned As Integer, ByVal ToBeScanned As Integer)
                MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, "Host scanned " & Host.HostIP, True)

                Dim lvI As ListViewItem = Host.ToListViewItem(psMode)
                AddListViewItem(lvI)

                SetPrgBar(AlreadyScanned, ToBeScanned)
            End Sub

            Private Sub Event_ScanComplete(ByVal Hosts As ArrayList)
                scanning = False
                SwitchButtonText()
                MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, "Scan complete!")
            End Sub
#End Region
        End Class
    End Namespace
End Namespace