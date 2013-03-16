Imports WeifenLuo.WinFormsUI.Docking

Namespace UI
    Namespace Window
        Public Class Help
            Inherits UI.Window.Base

#Region "Form Init"
            Friend WithEvents tvIndex As System.Windows.Forms.TreeView
            Friend WithEvents imgListHelp As System.Windows.Forms.ImageList
            Private components As System.ComponentModel.IContainer
            Friend WithEvents pnlSplitter As System.Windows.Forms.SplitContainer
            Friend WithEvents lblDocName As System.Windows.Forms.Label
            Friend WithEvents wbHelp As System.Windows.Forms.WebBrowser

            Private Sub InitializeComponent()
                Me.components = New System.ComponentModel.Container
                Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Introduction")
                Dim TreeNode2 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Prerequisites")
                Dim TreeNode3 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Installation")
                Dim TreeNode4 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Configuration")
                Dim TreeNode5 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("SQL Configuration")
                Dim TreeNode6 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Command-Line Switches")
                Dim TreeNode7 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Getting Started", New System.Windows.Forms.TreeNode() {TreeNode2, TreeNode3, TreeNode4, TreeNode5, TreeNode6})
                Dim TreeNode8 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Main Menu")
                Dim TreeNode9 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Connections")
                Dim TreeNode10 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Config")
                Dim TreeNode11 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Errors and Infos")
                Dim TreeNode12 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Save As / Export")
                Dim TreeNode13 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Sessions")
                Dim TreeNode14 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Screenshot Manager")
                Dim TreeNode15 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Connection")
                Dim TreeNode16 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Options")
                Dim TreeNode17 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Update")
                Dim TreeNode18 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("SSH File Transfer")
                Dim TreeNode19 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Quick Connect")
                Dim TreeNode20 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Import From Active Directory")
                Dim TreeNode21 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("External Applications")
                Dim TreeNode22 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Port Scan")
                Dim TreeNode23 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("User Interface", New System.Windows.Forms.TreeNode() {TreeNode8, TreeNode9, TreeNode10, TreeNode11, TreeNode12, TreeNode13, TreeNode14, TreeNode15, TreeNode16, TreeNode17, TreeNode18, TreeNode19, TreeNode20, TreeNode21, TreeNode22})
                Dim TreeNode24 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Quick Reference")
                Dim TreeNode25 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Help", New System.Windows.Forms.TreeNode() {TreeNode1, TreeNode7, TreeNode23, TreeNode24})
                Me.wbHelp = New System.Windows.Forms.WebBrowser
                Me.tvIndex = New System.Windows.Forms.TreeView
                Me.imgListHelp = New System.Windows.Forms.ImageList(Me.components)
                Me.pnlSplitter = New System.Windows.Forms.SplitContainer
                Me.lblDocName = New System.Windows.Forms.Label
                Me.pnlSplitter.Panel1.SuspendLayout()
                Me.pnlSplitter.Panel2.SuspendLayout()
                Me.pnlSplitter.SuspendLayout()
                Me.SuspendLayout()
                '
                'wbHelp
                '
                Me.wbHelp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.wbHelp.Location = New System.Drawing.Point(1, 36)
                Me.wbHelp.MinimumSize = New System.Drawing.Size(20, 20)
                Me.wbHelp.Name = "wbHelp"
                Me.wbHelp.ScriptErrorsSuppressed = True
                Me.wbHelp.Size = New System.Drawing.Size(327, 286)
                Me.wbHelp.TabIndex = 1
                '
                'tvIndex
                '
                Me.tvIndex.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.tvIndex.BorderStyle = System.Windows.Forms.BorderStyle.None
                Me.tvIndex.HideSelection = False
                Me.tvIndex.Location = New System.Drawing.Point(1, 1)
                Me.tvIndex.Name = "tvIndex"
                TreeNode1.Name = "Node0"
                TreeNode1.Tag = "Introduction"
                TreeNode1.Text = "Introduction"
                TreeNode2.Name = "Node0"
                TreeNode2.Tag = "Prerequisites"
                TreeNode2.Text = "Prerequisites"
                TreeNode3.Name = "Node3"
                TreeNode3.Tag = "Installation"
                TreeNode3.Text = "Installation"
                TreeNode4.Name = "Node4"
                TreeNode4.Tag = "Configuration"
                TreeNode4.Text = "Configuration"
                TreeNode5.Name = "Node0"
                TreeNode5.Tag = "ConfigurationSQL"
                TreeNode5.Text = "SQL Configuration"
                TreeNode6.Name = "Node5"
                TreeNode6.Tag = "CMDSwitches"
                TreeNode6.Text = "Command-Line Switches"
                TreeNode7.Name = "Node1"
                TreeNode7.Text = "Getting Started"
                TreeNode8.Name = "Node7"
                TreeNode8.Tag = "MainMenu"
                TreeNode8.Text = "Main Menu"
                TreeNode9.Name = "Node8"
                TreeNode9.Tag = "Connections"
                TreeNode9.Text = "Connections"
                TreeNode10.Name = "Node9"
                TreeNode10.Tag = "Config"
                TreeNode10.Text = "Config"
                TreeNode11.Name = "Node10"
                TreeNode11.Tag = "ErrorsAndInfos"
                TreeNode11.Text = "Errors and Infos"
                TreeNode12.Name = "Node11"
                TreeNode12.Tag = "SaveAsExport"
                TreeNode12.Text = "Save As / Export"
                TreeNode13.Name = "Node12"
                TreeNode13.Tag = "Sessions"
                TreeNode13.Text = "Sessions"
                TreeNode14.Name = "Node13"
                TreeNode14.Tag = "ScreenshotManager"
                TreeNode14.Text = "Screenshot Manager"
                TreeNode15.Name = "Node14"
                TreeNode15.Tag = "Connection"
                TreeNode15.Text = "Connection"
                TreeNode16.Name = "Node15"
                TreeNode16.Tag = "Options"
                TreeNode16.Text = "Options"
                TreeNode17.Name = "Node16"
                TreeNode17.Tag = "Update"
                TreeNode17.Text = "Update"
                TreeNode18.Name = "Node17"
                TreeNode18.Tag = "SSHFileTransfer"
                TreeNode18.Text = "SSH File Transfer"
                TreeNode19.Name = "Node18"
                TreeNode19.Tag = "QuickConnect"
                TreeNode19.Text = "Quick Connect"
                TreeNode20.Name = "Node19"
                TreeNode20.Tag = "ImportFromAD"
                TreeNode20.Text = "Import From Active Directory"
                TreeNode21.Name = "Node1"
                TreeNode21.Tag = "ExternalApps"
                TreeNode21.Text = "External Applications"
                TreeNode22.Name = "Node0"
                TreeNode22.Tag = "PortScan"
                TreeNode22.Text = "Port Scan"
                TreeNode23.Name = "Node6"
                TreeNode23.Text = "User Interface"
                TreeNode24.Name = "Node20"
                TreeNode24.Tag = "QuickReference"
                TreeNode24.Text = "Quick Reference"
                TreeNode25.Name = "Node0"
                TreeNode25.Text = "Help"
                Me.tvIndex.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode25})
                Me.tvIndex.ShowRootLines = False
                Me.tvIndex.Size = New System.Drawing.Size(207, 321)
                Me.tvIndex.TabIndex = 0
                '
                'imgListHelp
                '
                Me.imgListHelp.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
                Me.imgListHelp.ImageSize = New System.Drawing.Size(16, 16)
                Me.imgListHelp.TransparentColor = System.Drawing.Color.Transparent
                '
                'pnlSplitter
                '
                Me.pnlSplitter.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
                Me.pnlSplitter.Location = New System.Drawing.Point(0, 0)
                Me.pnlSplitter.Name = "pnlSplitter"
                '
                'pnlSplitter.Panel1
                '
                Me.pnlSplitter.Panel1.Controls.Add(Me.tvIndex)
                '
                'pnlSplitter.Panel2
                '
                Me.pnlSplitter.Panel2.Controls.Add(Me.lblDocName)
                Me.pnlSplitter.Panel2.Controls.Add(Me.wbHelp)
                Me.pnlSplitter.Size = New System.Drawing.Size(542, 323)
                Me.pnlSplitter.SplitterDistance = 209
                Me.pnlSplitter.TabIndex = 2
                '
                'lblDocName
                '
                Me.lblDocName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.lblDocName.BackColor = System.Drawing.Color.DimGray
                Me.lblDocName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.lblDocName.ForeColor = System.Drawing.Color.White
                Me.lblDocName.Location = New System.Drawing.Point(1, 1)
                Me.lblDocName.Name = "lblDocName"
                Me.lblDocName.Size = New System.Drawing.Size(327, 35)
                Me.lblDocName.TabIndex = 2
                Me.lblDocName.Text = "Introduction"
                Me.lblDocName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
                '
                'Help
                '
                Me.ClientSize = New System.Drawing.Size(542, 323)
                Me.Controls.Add(Me.pnlSplitter)
                Me.Icon = Global.mRemoteNG.My.Resources.Resources.Help_Icon
                Me.Name = "Help"
                Me.TabText = "Help"
                Me.Text = "Help"
                Me.pnlSplitter.Panel1.ResumeLayout(False)
                Me.pnlSplitter.Panel2.ResumeLayout(False)
                Me.pnlSplitter.ResumeLayout(False)
                Me.ResumeLayout(False)

            End Sub
#End Region

#Region "Public Methods"
            Public Sub New(ByVal Panel As DockContent)
                Me.WindowType = Type.Help
                Me.DockPnl = Panel
                Me.InitializeComponent()

                Me.FillImageList()
                Me.tvIndex.ImageList = Me.imgListHelp
                Me.SetImages(Me.tvIndex.Nodes(0))
            End Sub
#End Region

#Region "Private Methods"
            Private Sub Help_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
                Me.tvIndex.Nodes(0).Expand()
                Me.tvIndex.SelectedNode = Me.tvIndex.Nodes(0).Nodes(0)
            End Sub

            Private Sub Help_Shown(sender As Object, e As EventArgs) Handles Me.Shown
                ' This can only be set once the WebBrowser control is shown, it will throw a COM exception otherwise.
                wbHelp.AllowWebBrowserDrop = False
            End Sub
            
            Private Sub tvIndex_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvIndex.NodeMouseClick
                Me.tvIndex.SelectedNode = e.Node
            End Sub

            Private Sub tvIndex_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvIndex.AfterSelect
                If e.Node.Tag <> "" Then
                    Me.wbHelp.Navigate(My.Application.Info.DirectoryPath & "\Help\" & e.Node.Tag & ".htm")
                End If
            End Sub

            Private Sub wbHelp_DocumentTitleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wbHelp.DocumentTitleChanged
                Me.lblDocName.Text = Me.wbHelp.DocumentTitle
            End Sub

            Private Sub FillImageList()
                Me.imgListHelp.Images.Add("File", My.Resources.Page)
                Me.imgListHelp.Images.Add("Folder", My.Resources.Folder)
                Me.imgListHelp.Images.Add("Help", My.Resources.Help)
            End Sub

            Private Sub SetImages(ByVal node As TreeNode)
                node.ImageIndex = 2
                node.SelectedImageIndex = 2

                For Each n As TreeNode In node.Nodes
                    If n.Nodes.Count > 0 Then
                        n.ImageIndex = 1
                        n.SelectedImageIndex = 1
                    Else
                        n.ImageIndex = 0
                        n.SelectedImageIndex = 0
                    End If
                Next
            End Sub
#End Region
        End Class
    End Namespace
End Namespace