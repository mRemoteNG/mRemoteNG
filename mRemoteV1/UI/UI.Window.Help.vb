Imports System.ComponentModel
Imports mRemote3G.My.Resources
Imports WeifenLuo.WinFormsUI.Docking

Namespace UI

    Namespace Window
        Public Class Help
            Inherits Base

#Region "Form Init"

            Friend WithEvents tvIndex As TreeView
            Friend WithEvents imgListHelp As ImageList
            Private components As IContainer
            Friend WithEvents pnlSplitter As SplitContainer
            Friend WithEvents lblDocName As Label
            Friend WithEvents wbHelp As WebBrowser

            Private Sub InitializeComponent()
                Me.components = New System.ComponentModel.Container
                Dim TreeNode1 = New TreeNode("Introduction")
                Dim TreeNode2 = New TreeNode("Prerequisites")
                Dim TreeNode3 = New TreeNode("Installation")
                Dim TreeNode4 = New TreeNode("Configuration")
                Dim TreeNode5 = New TreeNode("SQL Configuration")
                Dim TreeNode6 = New TreeNode("Command-Line Switches")
                Dim TreeNode7 = New TreeNode("Getting Started",
                                             New TreeNode() {TreeNode2, TreeNode3, TreeNode4, TreeNode5, TreeNode6})
                Dim TreeNode8 = New TreeNode("Main Menu")
                Dim TreeNode9 = New TreeNode("Connections")
                Dim TreeNode10 = New TreeNode("Config")
                Dim TreeNode11 = New TreeNode("Errors and Infos")
                Dim TreeNode12 = New TreeNode("Save As / Export")
                Dim TreeNode13 = New TreeNode("Sessions")
                Dim TreeNode14 = New TreeNode("Screenshot Manager")
                Dim TreeNode15 = New TreeNode("Connection")
                Dim TreeNode16 = New TreeNode("Options")
                Dim TreeNode17 = New TreeNode("Update")
                Dim TreeNode18 = New TreeNode("SSH File Transfer")
                Dim TreeNode19 = New TreeNode("Quick Connect")
                Dim TreeNode20 = New TreeNode("Import From Active Directory")
                Dim TreeNode21 = New TreeNode("External Applications")
                Dim TreeNode22 = New TreeNode("Port Scan")
                Dim TreeNode23 = New TreeNode("User Interface",
                                              New TreeNode() _
                                                 {TreeNode8, TreeNode9, TreeNode10, TreeNode11, TreeNode12, TreeNode13,
                                                  TreeNode14, TreeNode15, TreeNode16, TreeNode17, TreeNode18, TreeNode19,
                                                  TreeNode20, TreeNode21, TreeNode22})
                Dim TreeNode24 = New TreeNode("Quick Reference")
                Dim TreeNode25 = New TreeNode("Help", New TreeNode() {TreeNode1, TreeNode7, TreeNode23, TreeNode24})
                Me.wbHelp = New WebBrowser
                Me.tvIndex = New TreeView
                Me.imgListHelp = New ImageList(Me.components)
                Me.pnlSplitter = New SplitContainer
                Me.lblDocName = New Label
                Me.pnlSplitter.Panel1.SuspendLayout()
                Me.pnlSplitter.Panel2.SuspendLayout()
                Me.pnlSplitter.SuspendLayout()
                Me.SuspendLayout()
                '
                'wbHelp
                '
                Me.wbHelp.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
                                           Or AnchorStyles.Left) _
                                          Or AnchorStyles.Right),
                                         AnchorStyles)
                Me.wbHelp.Location = New Point(1, 36)
                Me.wbHelp.MinimumSize = New Size(20, 20)
                Me.wbHelp.Name = "wbHelp"
                Me.wbHelp.ScriptErrorsSuppressed = True
                Me.wbHelp.Size = New Size(327, 286)
                Me.wbHelp.TabIndex = 1
                '
                'tvIndex
                '
                Me.tvIndex.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
                                            Or AnchorStyles.Left) _
                                           Or AnchorStyles.Right),
                                          AnchorStyles)
                Me.tvIndex.BorderStyle = BorderStyle.None
                Me.tvIndex.HideSelection = False
                Me.tvIndex.Location = New Point(1, 1)
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
                TreeNode21.Tag = "ExternalTools"
                TreeNode21.Text = "External Tools"
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
                Me.tvIndex.Nodes.AddRange(New TreeNode() {TreeNode25})
                Me.tvIndex.ShowRootLines = False
                Me.tvIndex.Size = New Size(207, 321)
                Me.tvIndex.TabIndex = 0
                '
                'imgListHelp
                '
                Me.imgListHelp.ColorDepth = ColorDepth.Depth32Bit
                Me.imgListHelp.ImageSize = New Size(16, 16)
                Me.imgListHelp.TransparentColor = Color.Transparent
                '
                'pnlSplitter
                '
                Me.pnlSplitter.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
                                                Or AnchorStyles.Left) _
                                               Or AnchorStyles.Right),
                                              AnchorStyles)
                Me.pnlSplitter.FixedPanel = FixedPanel.Panel1
                Me.pnlSplitter.Location = New Point(0, 0)
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
                Me.pnlSplitter.Size = New Size(542, 323)
                Me.pnlSplitter.SplitterDistance = 209
                Me.pnlSplitter.TabIndex = 2
                '
                'lblDocName
                '
                Me.lblDocName.Anchor = CType(((AnchorStyles.Top Or AnchorStyles.Left) _
                                              Or AnchorStyles.Right),
                                             AnchorStyles)
                Me.lblDocName.BackColor = Color.DimGray
                Me.lblDocName.Font = New Font("Microsoft Sans Serif", 12.0!, FontStyle.Bold, GraphicsUnit.Point,
                                              CType(0, Byte))
                Me.lblDocName.ForeColor = Color.White
                Me.lblDocName.Location = New Point(1, 1)
                Me.lblDocName.Name = "lblDocName"
                Me.lblDocName.Size = New Size(327, 35)
                Me.lblDocName.TabIndex = 2
                Me.lblDocName.Text = "Introduction"
                Me.lblDocName.TextAlign = ContentAlignment.MiddleLeft
                '
                'Help
                '
                Me.ClientSize = New Size(542, 323)
                Me.Controls.Add(Me.pnlSplitter)
                Me.Icon = Help_Icon
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

            Public Sub New(Panel As DockContent)
                Me.WindowType = Type.Help
                Me.DockPnl = Panel
                Me.InitializeComponent()

                Me.FillImageList()
                Me.tvIndex.ImageList = Me.imgListHelp
                Me.SetImages(Me.tvIndex.Nodes(0))
            End Sub

#End Region

#Region "Private Methods"

            Private Sub Help_Load(sender As Object, e As EventArgs) Handles Me.Load
                tvIndex.Nodes(0).ExpandAll()
                tvIndex.SelectedNode = tvIndex.Nodes(0).Nodes(0)
            End Sub

            Private Sub Help_Shown(sender As Object, e As EventArgs) Handles Me.Shown
                ' This can only be set once the WebBrowser control is shown, it will throw a COM exception otherwise.
                wbHelp.AllowWebBrowserDrop = False
            End Sub

            Private Sub tvIndex_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) _
                Handles tvIndex.NodeMouseClick
                Me.tvIndex.SelectedNode = e.Node
            End Sub

            Private Sub tvIndex_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles tvIndex.AfterSelect
                If e.Node.Tag <> "" Then
                    Me.wbHelp.Navigate(My.Application.Info.DirectoryPath & "\Help\" & e.Node.Tag & ".htm")
                End If
            End Sub

            Private Sub wbHelp_DocumentTitleChanged(sender As Object, e As EventArgs) _
                Handles wbHelp.DocumentTitleChanged
                Me.lblDocName.Text = Me.wbHelp.DocumentTitle
            End Sub

            Private Sub FillImageList()
                Me.imgListHelp.Images.Add("File", Page)
                Me.imgListHelp.Images.Add("Folder", Folder)
                Me.imgListHelp.Images.Add("Help", My.Resources.Help)
            End Sub

            Private Sub SetImages(node As TreeNode)
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