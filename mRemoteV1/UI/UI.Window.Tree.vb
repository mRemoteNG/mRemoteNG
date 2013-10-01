Imports mRemoteNG
Imports mRemoteNG.My
Imports WeifenLuo.WinFormsUI.Docking
Imports mRemoteNG.App.Runtime

Namespace UI
    Namespace Window
        Public Class Tree
            Inherits UI.Window.Base

#Region "Form Init"
            Private components As System.ComponentModel.IContainer
            Friend WithEvents txtSearch As System.Windows.Forms.TextBox
            Friend WithEvents pnlConnections As System.Windows.Forms.Panel
            Friend WithEvents imgListTree As System.Windows.Forms.ImageList
            Friend WithEvents msMain As System.Windows.Forms.MenuStrip
            Friend WithEvents mMenView As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents mMenViewExpandAllFolders As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents mMenViewCollapseAllFolders As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTree As System.Windows.Forms.ContextMenuStrip
            Friend WithEvents cMenTreeAddConnection As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeAddFolder As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeSep1 As System.Windows.Forms.ToolStripSeparator
            Friend WithEvents cMenTreeConnect As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeConnectWithOptions As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeConnectWithOptionsConnectToConsoleSession As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeConnectWithOptionsNoCredentials As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeConnectWithOptionsConnectInFullscreen As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeDisconnect As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeSep2 As System.Windows.Forms.ToolStripSeparator
            Friend WithEvents cMenTreeToolsTransferFile As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeToolsImportExport As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeToolsImportExportExportmRemoteXML As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeToolsImportExportImportmRemoteXML As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeToolsImportExportSep1 As System.Windows.Forms.ToolStripSeparator
            Friend WithEvents cMenTreeToolsImportExportImportFromAD As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeToolsSort As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeToolsSortAscending As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeToolsSortDescending As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeSep3 As System.Windows.Forms.ToolStripSeparator
            Friend WithEvents cMenTreeRename As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeDelete As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeSep4 As System.Windows.Forms.ToolStripSeparator
            Friend WithEvents cMenTreeMoveUp As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeMoveDown As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
            Friend WithEvents cMenTreeToolsImportExportImportFromRDPFiles As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeToolsExternalApps As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeDuplicate As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeToolsImportExportImportFromPortScan As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeConnectWithOptionsChoosePanelBeforeConnecting As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenTreeConnectWithOptionsDontConnectToConsoleSession As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents mMenSortAscending As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents mMenAddConnection As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents mMenAddFolder As System.Windows.Forms.ToolStripMenuItem
            Public WithEvents tvConnections As System.Windows.Forms.TreeView
            Private Sub InitializeComponent()
                Me.components = New System.ComponentModel.Container()
                Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Connections")
                Me.tvConnections = New System.Windows.Forms.TreeView()
                Me.cMenTree = New System.Windows.Forms.ContextMenuStrip(Me.components)
                Me.cMenTreeAddConnection = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeAddFolder = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeSep1 = New System.Windows.Forms.ToolStripSeparator()
                Me.cMenTreeConnect = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeConnectWithOptions = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeConnectWithOptionsNoCredentials = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeConnectWithOptionsConnectToConsoleSession = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeConnectWithOptionsDontConnectToConsoleSession = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeConnectWithOptionsConnectInFullscreen = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeDisconnect = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeSep2 = New System.Windows.Forms.ToolStripSeparator()
                Me.cMenTreeToolsTransferFile = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeToolsImportExport = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeToolsImportExportExportmRemoteXML = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeToolsImportExportImportmRemoteXML = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeToolsImportExportSep1 = New System.Windows.Forms.ToolStripSeparator()
                Me.cMenTreeToolsImportExportImportFromAD = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeToolsImportExportImportFromRDPFiles = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeToolsImportExportImportFromPortScan = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeToolsSort = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeToolsSortAscending = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeToolsSortDescending = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeToolsExternalApps = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeSep3 = New System.Windows.Forms.ToolStripSeparator()
                Me.cMenTreeDuplicate = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeRename = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeDelete = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeSep4 = New System.Windows.Forms.ToolStripSeparator()
                Me.cMenTreeMoveUp = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTreeMoveDown = New System.Windows.Forms.ToolStripMenuItem()
                Me.imgListTree = New System.Windows.Forms.ImageList(Me.components)
                Me.pnlConnections = New System.Windows.Forms.Panel()
                Me.PictureBox1 = New System.Windows.Forms.PictureBox()
                Me.txtSearch = New System.Windows.Forms.TextBox()
                Me.msMain = New System.Windows.Forms.MenuStrip()
                Me.mMenAddConnection = New System.Windows.Forms.ToolStripMenuItem()
                Me.mMenAddFolder = New System.Windows.Forms.ToolStripMenuItem()
                Me.mMenView = New System.Windows.Forms.ToolStripMenuItem()
                Me.mMenViewExpandAllFolders = New System.Windows.Forms.ToolStripMenuItem()
                Me.mMenViewCollapseAllFolders = New System.Windows.Forms.ToolStripMenuItem()
                Me.mMenSortAscending = New System.Windows.Forms.ToolStripMenuItem()
                Me.cMenTree.SuspendLayout()
                Me.pnlConnections.SuspendLayout()
                CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
                Me.msMain.SuspendLayout()
                Me.SuspendLayout()
                '
                'tvConnections
                '
                Me.tvConnections.AllowDrop = True
                Me.tvConnections.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.tvConnections.BorderStyle = System.Windows.Forms.BorderStyle.None
                Me.tvConnections.ContextMenuStrip = Me.cMenTree
                Me.tvConnections.HideSelection = False
                Me.tvConnections.ImageIndex = 0
                Me.tvConnections.ImageList = Me.imgListTree
                Me.tvConnections.LabelEdit = True
                Me.tvConnections.Location = New System.Drawing.Point(0, 0)
                Me.tvConnections.Name = "tvConnections"
                TreeNode1.Name = "nodeRoot"
                TreeNode1.Text = "Connections"
                Me.tvConnections.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode1})
                Me.tvConnections.SelectedImageIndex = 0
                Me.tvConnections.Size = New System.Drawing.Size(192, 410)
                Me.tvConnections.TabIndex = 20
                '
                'cMenTree
                '
                Me.cMenTree.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.cMenTree.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cMenTreeAddConnection, Me.cMenTreeAddFolder, Me.cMenTreeSep1, Me.cMenTreeConnect, Me.cMenTreeConnectWithOptions, Me.cMenTreeDisconnect, Me.cMenTreeSep2, Me.cMenTreeToolsTransferFile, Me.cMenTreeToolsImportExport, Me.cMenTreeToolsSort, Me.cMenTreeToolsExternalApps, Me.cMenTreeSep3, Me.cMenTreeDuplicate, Me.cMenTreeRename, Me.cMenTreeDelete, Me.cMenTreeSep4, Me.cMenTreeMoveUp, Me.cMenTreeMoveDown})
                Me.cMenTree.Name = "cMenTree"
                Me.cMenTree.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
                Me.cMenTree.Size = New System.Drawing.Size(187, 336)
                '
                'cMenTreeAddConnection
                '
                Me.cMenTreeAddConnection.Image = Global.mRemoteNG.My.Resources.Resources.Connection_Add
                Me.cMenTreeAddConnection.Name = "cMenTreeAddConnection"
                Me.cMenTreeAddConnection.Size = New System.Drawing.Size(186, 22)
                Me.cMenTreeAddConnection.Text = "New Connection"
                '
                'cMenTreeAddFolder
                '
                Me.cMenTreeAddFolder.Image = Global.mRemoteNG.My.Resources.Resources.Folder_Add
                Me.cMenTreeAddFolder.Name = "cMenTreeAddFolder"
                Me.cMenTreeAddFolder.Size = New System.Drawing.Size(186, 22)
                Me.cMenTreeAddFolder.Text = "New Folder"
                '
                'cMenTreeSep1
                '
                Me.cMenTreeSep1.Name = "cMenTreeSep1"
                Me.cMenTreeSep1.Size = New System.Drawing.Size(183, 6)
                '
                'cMenTreeConnect
                '
                Me.cMenTreeConnect.Image = Global.mRemoteNG.My.Resources.Resources.Play
                Me.cMenTreeConnect.Name = "cMenTreeConnect"
                Me.cMenTreeConnect.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
                    Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
                Me.cMenTreeConnect.Size = New System.Drawing.Size(186, 22)
                Me.cMenTreeConnect.Text = "Connect"
                '
                'cMenTreeConnectWithOptions
                '
                Me.cMenTreeConnectWithOptions.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cMenTreeConnectWithOptionsConnectToConsoleSession, Me.cMenTreeConnectWithOptionsDontConnectToConsoleSession, Me.cMenTreeConnectWithOptionsConnectInFullscreen, Me.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting, Me.cMenTreeConnectWithOptionsNoCredentials
                                                                                                              })
                Me.cMenTreeConnectWithOptions.Name = "cMenTreeConnectWithOptions"
                Me.cMenTreeConnectWithOptions.Size = New System.Drawing.Size(186, 22)
                Me.cMenTreeConnectWithOptions.Text = "Connect (with options)"
                '
                'cMenTreeConnectWithOptionsConnectToConsoleSession
                '
                Me.cMenTreeConnectWithOptionsConnectToConsoleSession.Name = "cMenTreeConnectWithOptionsConnectToConsoleSession"
                Me.cMenTreeConnectWithOptionsConnectToConsoleSession.Size = New System.Drawing.Size(231, 22)
                Me.cMenTreeConnectWithOptionsConnectToConsoleSession.Text = "Connect to console session"
                '
                'cMenTreeConnectWithOptionsDontConnectToConsoleSession
                '
                Me.cMenTreeConnectWithOptionsDontConnectToConsoleSession.Name = "cMenTreeConnectWithOptionsDontConnectToConsoleSession"
                Me.cMenTreeConnectWithOptionsDontConnectToConsoleSession.Size = New System.Drawing.Size(231, 22)
                Me.cMenTreeConnectWithOptionsDontConnectToConsoleSession.Text = "Don't connect to console session"
                Me.cMenTreeConnectWithOptionsDontConnectToConsoleSession.Visible = False
                '
                'cMenTreeConnectWithOptionsConnectInFullscreen
                '
                Me.cMenTreeConnectWithOptionsConnectInFullscreen.Image = Global.mRemoteNG.My.Resources.Resources.Fullscreen
                Me.cMenTreeConnectWithOptionsConnectInFullscreen.Name = "cMenTreeConnectWithOptionsConnectInFullscreen"
                Me.cMenTreeConnectWithOptionsConnectInFullscreen.Size = New System.Drawing.Size(231, 22)
                Me.cMenTreeConnectWithOptionsConnectInFullscreen.Text = "Connect in fullscreen"
                '
                'cMenTreeConnectWithOptionsNoCredentials
                '
                Me.cMenTreeConnectWithOptionsConnectInFullscreen.Image = Global.mRemoteNG.My.Resources.Resources.Fullscreen
                Me.cMenTreeConnectWithOptionsConnectInFullscreen.Name = "cMenTreeConnectWithOptionsNoCredentials"
                Me.cMenTreeConnectWithOptionsConnectInFullscreen.Size = New System.Drawing.Size(231, 22)
                Me.cMenTreeConnectWithOptionsConnectInFullscreen.Text = "Connect without credentials"
                '
                'cMenTreeConnectWithOptionsChoosePanelBeforeConnecting
                '
                Me.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Image = Global.mRemoteNG.My.Resources.Resources.Panels
                Me.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Name = "cMenTreeConnectWithOptionsChoosePanelBeforeConnecting"
                Me.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Size = New System.Drawing.Size(231, 22)
                Me.cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Text = "Choose panel before connecting"
                '
                'cMenTreeDisconnect
                '
                Me.cMenTreeDisconnect.Image = Global.mRemoteNG.My.Resources.Resources.Pause
                Me.cMenTreeDisconnect.Name = "cMenTreeDisconnect"
                Me.cMenTreeDisconnect.Size = New System.Drawing.Size(186, 22)
                Me.cMenTreeDisconnect.Text = "Disconnect"
                '
                'cMenTreeSep2
                '
                Me.cMenTreeSep2.Name = "cMenTreeSep2"
                Me.cMenTreeSep2.Size = New System.Drawing.Size(183, 6)
                '
                'cMenTreeToolsTransferFile
                '
                Me.cMenTreeToolsTransferFile.Image = Global.mRemoteNG.My.Resources.Resources.SSHTransfer
                Me.cMenTreeToolsTransferFile.Name = "cMenTreeToolsTransferFile"
                Me.cMenTreeToolsTransferFile.Size = New System.Drawing.Size(186, 22)
                Me.cMenTreeToolsTransferFile.Text = "Transfer File (SSH)"
                '
                'cMenTreeToolsImportExport
                '
                Me.cMenTreeToolsImportExport.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cMenTreeToolsImportExportExportmRemoteXML, Me.cMenTreeToolsImportExportImportmRemoteXML, Me.cMenTreeToolsImportExportSep1, Me.cMenTreeToolsImportExportImportFromAD, Me.cMenTreeToolsImportExportImportFromRDPFiles, Me.cMenTreeToolsImportExportImportFromPortScan})
                Me.cMenTreeToolsImportExport.Name = "cMenTreeToolsImportExport"
                Me.cMenTreeToolsImportExport.Size = New System.Drawing.Size(186, 22)
                Me.cMenTreeToolsImportExport.Text = "Import/Export"
                '
                'cMenTreeToolsImportExportExportmRemoteXML
                '
                Me.cMenTreeToolsImportExportExportmRemoteXML.Image = Global.mRemoteNG.My.Resources.Resources.Connections_SaveAs
                Me.cMenTreeToolsImportExportExportmRemoteXML.Name = "cMenTreeToolsImportExportExportmRemoteXML"
                Me.cMenTreeToolsImportExportExportmRemoteXML.Size = New System.Drawing.Size(204, 22)
                Me.cMenTreeToolsImportExportExportmRemoteXML.Text = "Export mRemote XML"
                '
                'cMenTreeToolsImportExportImportmRemoteXML
                '
                Me.cMenTreeToolsImportExportImportmRemoteXML.Image = Global.mRemoteNG.My.Resources.Resources.Connections_Load
                Me.cMenTreeToolsImportExportImportmRemoteXML.Name = "cMenTreeToolsImportExportImportmRemoteXML"
                Me.cMenTreeToolsImportExportImportmRemoteXML.Size = New System.Drawing.Size(204, 22)
                Me.cMenTreeToolsImportExportImportmRemoteXML.Text = "Import mRemote XML"
                '
                'cMenTreeToolsImportExportSep1
                '
                Me.cMenTreeToolsImportExportSep1.Name = "cMenTreeToolsImportExportSep1"
                Me.cMenTreeToolsImportExportSep1.Size = New System.Drawing.Size(201, 6)
                '
                'cMenTreeToolsImportExportImportFromAD
                '
                Me.cMenTreeToolsImportExportImportFromAD.Image = Global.mRemoteNG.My.Resources.Resources.ActiveDirectory
                Me.cMenTreeToolsImportExportImportFromAD.Name = "cMenTreeToolsImportExportImportFromAD"
                Me.cMenTreeToolsImportExportImportFromAD.Size = New System.Drawing.Size(204, 22)
                Me.cMenTreeToolsImportExportImportFromAD.Text = "Import from Active Directory"
                '
                'cMenTreeToolsImportExportImportFromRDPFiles
                '
                Me.cMenTreeToolsImportExportImportFromRDPFiles.Image = Global.mRemoteNG.My.Resources.Resources.RDP
                Me.cMenTreeToolsImportExportImportFromRDPFiles.Name = "cMenTreeToolsImportExportImportFromRDPFiles"
                Me.cMenTreeToolsImportExportImportFromRDPFiles.Size = New System.Drawing.Size(204, 22)
                Me.cMenTreeToolsImportExportImportFromRDPFiles.Text = "Import from .RDP file(s)"
                '
                'cMenTreeToolsImportExportImportFromPortScan
                '
                Me.cMenTreeToolsImportExportImportFromPortScan.Image = Global.mRemoteNG.My.Resources.Resources.PortScan
                Me.cMenTreeToolsImportExportImportFromPortScan.Name = "cMenTreeToolsImportExportImportFromPortScan"
                Me.cMenTreeToolsImportExportImportFromPortScan.Size = New System.Drawing.Size(204, 22)
                Me.cMenTreeToolsImportExportImportFromPortScan.Text = "Import from Port Scan"
                '
                'cMenTreeToolsSort
                '
                Me.cMenTreeToolsSort.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cMenTreeToolsSortAscending, Me.cMenTreeToolsSortDescending})
                Me.cMenTreeToolsSort.Name = "cMenTreeToolsSort"
                Me.cMenTreeToolsSort.Size = New System.Drawing.Size(186, 22)
                Me.cMenTreeToolsSort.Text = "Sort"
                '
                'cMenTreeToolsSortAscending
                '
                Me.cMenTreeToolsSortAscending.Image = Global.mRemoteNG.My.Resources.Resources.Sort_AZ
                Me.cMenTreeToolsSortAscending.Name = "cMenTreeToolsSortAscending"
                Me.cMenTreeToolsSortAscending.Size = New System.Drawing.Size(157, 22)
                Me.cMenTreeToolsSortAscending.Text = "Ascending (A-Z)"
                '
                'cMenTreeToolsSortDescending
                '
                Me.cMenTreeToolsSortDescending.Image = Global.mRemoteNG.My.Resources.Resources.Sort_ZA
                Me.cMenTreeToolsSortDescending.Name = "cMenTreeToolsSortDescending"
                Me.cMenTreeToolsSortDescending.Size = New System.Drawing.Size(157, 22)
                Me.cMenTreeToolsSortDescending.Text = "Descending (Z-A)"
                '
                'cMenTreeToolsExternalApps
                '
                Me.cMenTreeToolsExternalApps.Image = Global.mRemoteNG.My.Resources.Resources.ExtApp
                Me.cMenTreeToolsExternalApps.Name = "cMenTreeToolsExternalApps"
                Me.cMenTreeToolsExternalApps.Size = New System.Drawing.Size(186, 22)
                Me.cMenTreeToolsExternalApps.Text = "External Applications"
                '
                'cMenTreeSep3
                '
                Me.cMenTreeSep3.Name = "cMenTreeSep3"
                Me.cMenTreeSep3.Size = New System.Drawing.Size(183, 6)
                '
                'cMenTreeDuplicate
                '
                Me.cMenTreeDuplicate.Image = Global.mRemoteNG.My.Resources.Resources.Connection_Duplicate
                Me.cMenTreeDuplicate.Name = "cMenTreeDuplicate"
                Me.cMenTreeDuplicate.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
                Me.cMenTreeDuplicate.Size = New System.Drawing.Size(186, 22)
                Me.cMenTreeDuplicate.Text = "Duplicate"
                '
                'cMenTreeRename
                '
                Me.cMenTreeRename.Image = Global.mRemoteNG.My.Resources.Resources.Rename
                Me.cMenTreeRename.Name = "cMenTreeRename"
                Me.cMenTreeRename.ShortcutKeys = System.Windows.Forms.Keys.F2
                Me.cMenTreeRename.Size = New System.Drawing.Size(186, 22)
                Me.cMenTreeRename.Text = "Rename"
                '
                'cMenTreeDelete
                '
                Me.cMenTreeDelete.Image = Global.mRemoteNG.My.Resources.Resources.Delete
                Me.cMenTreeDelete.Name = "cMenTreeDelete"
                Me.cMenTreeDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete
                Me.cMenTreeDelete.Size = New System.Drawing.Size(186, 22)
                Me.cMenTreeDelete.Text = "Delete"
                '
                'cMenTreeSep4
                '
                Me.cMenTreeSep4.Name = "cMenTreeSep4"
                Me.cMenTreeSep4.Size = New System.Drawing.Size(183, 6)
                '
                'cMenTreeMoveUp
                '
                Me.cMenTreeMoveUp.Image = Global.mRemoteNG.My.Resources.Resources.Arrow_Up
                Me.cMenTreeMoveUp.Name = "cMenTreeMoveUp"
                Me.cMenTreeMoveUp.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Up), System.Windows.Forms.Keys)
                Me.cMenTreeMoveUp.Size = New System.Drawing.Size(186, 22)
                Me.cMenTreeMoveUp.Text = "Move up"
                '
                'cMenTreeMoveDown
                '
                Me.cMenTreeMoveDown.Image = Global.mRemoteNG.My.Resources.Resources.Arrow_Down
                Me.cMenTreeMoveDown.Name = "cMenTreeMoveDown"
                Me.cMenTreeMoveDown.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Down), System.Windows.Forms.Keys)
                Me.cMenTreeMoveDown.Size = New System.Drawing.Size(186, 22)
                Me.cMenTreeMoveDown.Text = "Move down"
                '
                'imgListTree
                '
                Me.imgListTree.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
                Me.imgListTree.ImageSize = New System.Drawing.Size(16, 16)
                Me.imgListTree.TransparentColor = System.Drawing.Color.Transparent
                '
                'pnlConnections
                '
                Me.pnlConnections.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pnlConnections.Controls.Add(Me.PictureBox1)
                Me.pnlConnections.Controls.Add(Me.txtSearch)
                Me.pnlConnections.Controls.Add(Me.tvConnections)
                Me.pnlConnections.Location = New System.Drawing.Point(0, 25)
                Me.pnlConnections.Name = "pnlConnections"
                Me.pnlConnections.Size = New System.Drawing.Size(192, 428)
                Me.pnlConnections.TabIndex = 9
                '
                'PictureBox1
                '
                Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
                Me.PictureBox1.Image = Global.mRemoteNG.My.Resources.Resources.Search
                Me.PictureBox1.Location = New System.Drawing.Point(2, 412)
                Me.PictureBox1.Name = "PictureBox1"
                Me.PictureBox1.Size = New System.Drawing.Size(16, 16)
                Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
                Me.PictureBox1.TabIndex = 1
                Me.PictureBox1.TabStop = False
                '
                'txtSearch
                '
                Me.txtSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None
                Me.txtSearch.ForeColor = System.Drawing.SystemColors.GrayText
                Me.txtSearch.Location = New System.Drawing.Point(19, 413)
                Me.txtSearch.Name = "txtSearch"
                Me.txtSearch.Size = New System.Drawing.Size(171, 13)
                Me.txtSearch.TabIndex = 30
                Me.txtSearch.TabStop = False
                Me.txtSearch.Text = "Search"
                '
                'msMain
                '
                Me.msMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.msMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mMenAddConnection, Me.mMenAddFolder, Me.mMenView, Me.mMenSortAscending})
                Me.msMain.Location = New System.Drawing.Point(0, 0)
                Me.msMain.Name = "msMain"
                Me.msMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
                Me.msMain.ShowItemToolTips = True
                Me.msMain.Size = New System.Drawing.Size(192, 24)
                Me.msMain.TabIndex = 10
                Me.msMain.Text = "MenuStrip1"
                '
                'mMenAddConnection
                '
                Me.mMenAddConnection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                Me.mMenAddConnection.Image = Global.mRemoteNG.My.Resources.Resources.Connection_Add
                Me.mMenAddConnection.Name = "mMenAddConnection"
                Me.mMenAddConnection.Size = New System.Drawing.Size(28, 20)
                '
                'mMenAddFolder
                '
                Me.mMenAddFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                Me.mMenAddFolder.Image = Global.mRemoteNG.My.Resources.Resources.Folder_Add
                Me.mMenAddFolder.Name = "mMenAddFolder"
                Me.mMenAddFolder.Size = New System.Drawing.Size(28, 20)
                '
                'mMenView
                '
                Me.mMenView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                Me.mMenView.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mMenViewExpandAllFolders, Me.mMenViewCollapseAllFolders})
                Me.mMenView.Image = Global.mRemoteNG.My.Resources.Resources.View
                Me.mMenView.Name = "mMenView"
                Me.mMenView.Size = New System.Drawing.Size(28, 20)
                Me.mMenView.Text = "&View"
                '
                'mMenViewExpandAllFolders
                '
                Me.mMenViewExpandAllFolders.Image = Global.mRemoteNG.My.Resources.Resources.Expand
                Me.mMenViewExpandAllFolders.Name = "mMenViewExpandAllFolders"
                Me.mMenViewExpandAllFolders.Size = New System.Drawing.Size(161, 22)
                Me.mMenViewExpandAllFolders.Text = "Expand all folders"
                '
                'mMenViewCollapseAllFolders
                '
                Me.mMenViewCollapseAllFolders.Image = Global.mRemoteNG.My.Resources.Resources.Collapse
                Me.mMenViewCollapseAllFolders.Name = "mMenViewCollapseAllFolders"
                Me.mMenViewCollapseAllFolders.Size = New System.Drawing.Size(161, 22)
                Me.mMenViewCollapseAllFolders.Text = "Collapse all folders"
                '
                'mMenSortAscending
                '
                Me.mMenSortAscending.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                Me.mMenSortAscending.Image = Global.mRemoteNG.My.Resources.Resources.Sort_AZ
                Me.mMenSortAscending.Name = "mMenSortAscending"
                Me.mMenSortAscending.Size = New System.Drawing.Size(28, 20)
                '
                'Tree
                '
                Me.ClientSize = New System.Drawing.Size(192, 453)
                Me.Controls.Add(Me.msMain)
                Me.Controls.Add(Me.pnlConnections)
                Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.HideOnClose = True
                Me.Icon = Global.mRemoteNG.My.Resources.Resources.Root_Icon
                Me.Name = "Tree"
                Me.TabText = "Connections"
                Me.Text = "Connections"
                Me.cMenTree.ResumeLayout(False)
                Me.pnlConnections.ResumeLayout(False)
                Me.pnlConnections.PerformLayout()
                CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
                Me.msMain.ResumeLayout(False)
                Me.msMain.PerformLayout()
                Me.ResumeLayout(False)
                Me.PerformLayout()

            End Sub
#End Region

#Region "Form Stuff"
            Private Sub Tree_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
                ApplyLanguage()

                AddHandler Themes.ThemeManager.ThemeChanged, AddressOf ApplyTheme
                ApplyTheme()

                txtSearch.Multiline = True
                txtSearch.MinimumSize = New Size(0, 14)
                txtSearch.Size = New Size(txtSearch.Size.Width, 14)
                txtSearch.Multiline = False
            End Sub

            Private Sub ApplyLanguage()
                cMenTreeAddConnection.Text = My.Language.strAddConnection
                cMenTreeAddFolder.Text = My.Language.strAddFolder
                cMenTreeConnect.Text = My.Language.strConnect
                cMenTreeConnectWithOptions.Text = My.Language.strConnectWithOptions
                cMenTreeConnectWithOptionsConnectToConsoleSession.Text = My.Language.strConnectToConsoleSession
                cMenTreeConnectWithOptionsNoCredentials.Text = My.Language.strConnectNoCredentials
                cMenTreeConnectWithOptionsConnectInFullscreen.Text = My.Language.strConnectInFullscreen
                cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Text = My.Language.strChoosePanelBeforeConnecting
                cMenTreeDisconnect.Text = My.Language.strMenuDisconnect
                cMenTreeToolsTransferFile.Text = My.Language.strMenuTransferFile
                cMenTreeToolsImportExport.Text = My.Language.strImportExport
                cMenTreeToolsImportExportExportmRemoteXML.Text = My.Language.strExportmRemoteXML
                cMenTreeToolsImportExportImportmRemoteXML.Text = My.Language.strImportmRemoteXML
                cMenTreeToolsImportExportImportFromAD.Text = My.Language.strImportAD
                cMenTreeToolsImportExportImportFromRDPFiles.Text = My.Language.strImportRDPFiles
                cMenTreeToolsImportExportImportFromPortScan.Text = My.Language.strImportPortScan
                cMenTreeToolsSort.Text = My.Language.strSort
                cMenTreeToolsSortAscending.Text = My.Language.strSortAsc
                cMenTreeToolsSortDescending.Text = My.Language.strSortDesc
                cMenTreeToolsExternalApps.Text = My.Language.strMenuExternalTools
                cMenTreeDuplicate.Text = My.Language.strDuplicate
                cMenTreeRename.Text = My.Language.strRename
                cMenTreeDelete.Text = My.Language.strMenuDelete
                cMenTreeMoveUp.Text = My.Language.strMoveUp
                cMenTreeMoveDown.Text = My.Language.strMoveDown
                mMenAddConnection.ToolTipText = My.Language.strAddConnection
                mMenAddFolder.ToolTipText = My.Language.strAddFolder
                mMenView.ToolTipText = My.Language.strMenuView.Replace("&", "")
                mMenViewExpandAllFolders.Text = My.Language.strExpandAllFolders
                mMenViewCollapseAllFolders.Text = My.Language.strCollapseAllFolders
                mMenSortAscending.ToolTipText = My.Language.strSortAsc
                txtSearch.Text = Language.strSearchPrompt
                TabText = My.Language.strConnections
                Text = My.Language.strConnections
            End Sub

            Public Sub ApplyTheme()
                With Themes.ThemeManager.ActiveTheme
                    msMain.BackColor = .ToolbarBackgroundColor
                    msMain.ForeColor = .ToolbarTextColor
                    tvConnections.BackColor = .ConnectionsPanelBackgroundColor
                    tvConnections.ForeColor = .ConnectionsPanelTextColor
                    tvConnections.LineColor = .ConnectionsPanelTreeLineColor
                    BackColor = .ToolbarBackgroundColor
                    txtSearch.BackColor = .SearchBoxBackgroundColor
                    txtSearch.ForeColor = .SearchBoxTextPromptColor
                End With
            End Sub
#End Region

#Region "Public Methods"
            Public Sub New(ByVal Panel As DockContent)
                Me.WindowType = Type.Tree
                Me.DockPnl = Panel
                Me.InitializeComponent()
                Me.FillImageList()

                _DescriptionTooltip = New ToolTip
                _DescriptionTooltip.InitialDelay = 300
                _DescriptionTooltip.ReshowDelay = 0
            End Sub

            Public Sub InitialRefresh()
                Me.tvConnections_AfterSelect(Me.tvConnections, New TreeViewEventArgs(Me.tvConnections.SelectedNode, TreeViewAction.ByMouse))
            End Sub
#End Region

#Region "Public Properties"
            Private _DescriptionTooltip As ToolTip
            Public Property DescriptionTooltip() As ToolTip
                Get
                    Return _DescriptionTooltip
                End Get
                Set(ByVal value As ToolTip)
                    _DescriptionTooltip = value
                End Set
            End Property
#End Region

#Region "Private Methods"
            Private Sub FillImageList()
                Try
                    imgListTree.Images.Add(Resources.Root)
                    imgListTree.Images.Add(Resources.Folder)
                    imgListTree.Images.Add(Resources.Play)
                    imgListTree.Images.Add(Resources.Pause)
                    imgListTree.Images.Add(Resources.PuttySessions)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "FillImageList (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub tvConnections_BeforeLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles tvConnections.BeforeLabelEdit
                cMenTreeDelete.ShortcutKeys = Keys.None
            End Sub

            Private Sub tvConnections_AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles tvConnections.AfterLabelEdit
                Try
                    cMenTreeDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete

                    mRemoteNG.Tree.Node.FinishRenameSelectedNode(e.Label)
                    Windows.configForm.pGrid_SelectedObjectChanged()
                    Me.ShowHideTreeContextMenuItems(e.Node)
                    SaveConnectionsBG()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_AfterLabelEdit (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub tvConnections_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvConnections.AfterSelect
                Try
                    Select Case mRemoteNG.Tree.Node.GetNodeType(e.Node)
                        Case mRemoteNG.Tree.Node.Type.Connection, mRemoteNG.Tree.Node.Type.PuttySession
                            Windows.configForm.SetPropertyGridObject(e.Node.Tag)
                        Case mRemoteNG.Tree.Node.Type.Container
                            Windows.configForm.SetPropertyGridObject(TryCast(e.Node.Tag, Container.Info).ConnectionInfo)
                        Case mRemoteNG.Tree.Node.Type.Root, mRemoteNG.Tree.Node.Type.PuttyRoot
                            Windows.configForm.SetPropertyGridObject(e.Node.Tag)
                        Case Else
                            Exit Sub
                    End Select

                    Windows.configForm.pGrid_SelectedObjectChanged()
                    ShowHideTreeContextMenuItems(e.Node)
                    Windows.sessionsForm.GetSessions(True)

                    LastSelected = mRemoteNG.Tree.Node.GetConstantID(e.Node)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_AfterSelect (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub tvConnections_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvConnections.NodeMouseClick
                Try
                    Me.ShowHideTreeContextMenuItems(Me.tvConnections.SelectedNode)
                    Me.tvConnections.SelectedNode = e.Node

                    If e.Button = System.Windows.Forms.MouseButtons.Left Then
                        If Settings.SingleClickOnConnectionOpensIt And _
                            (mRemoteNG.Tree.Node.GetNodeType(e.Node) = mRemoteNG.Tree.Node.Type.Connection Or _
                             mRemoteNG.Tree.Node.GetNodeType(e.Node) = mRemoteNG.Tree.Node.Type.PuttySession) Then
                            OpenConnection()
                        End If

                        If My.Settings.SingleClickSwitchesToOpenConnection And mRemoteNG.Tree.Node.GetNodeType(e.Node) = mRemoteNG.Tree.Node.Type.Connection Then
                            App.Runtime.SwitchToOpenConnection(e.Node.Tag)
                        End If
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_NodeMouseClick (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub tvConnections_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvConnections.NodeMouseDoubleClick
                If mRemoteNG.Tree.Node.GetNodeType(mRemoteNG.Tree.Node.SelectedNode) = mRemoteNG.Tree.Node.Type.Connection Or _
                   mRemoteNG.Tree.Node.GetNodeType(mRemoteNG.Tree.Node.SelectedNode) = mRemoteNG.Tree.Node.Type.PuttySession Then
                    OpenConnection()
                End If
            End Sub

            Private Sub tvConnections_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tvConnections.MouseMove
                Try
                    mRemoteNG.Tree.Node.SetNodeToolTip(e, Me.DescriptionTooltip)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_MouseMove (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Function IsConnectionOpen(ByVal ConnectionInfos() As mRemoteNG.Connection.Info) As Boolean
                Try
                    If ConnectionInfos IsNot Nothing Then
                        For Each conI As mRemoteNG.Connection.Info In ConnectionInfos
                            If conI.OpenConnections.Count > 0 Then
                                Return True
                            End If
                        Next
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "IsConnectionOpen (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try

                Return False
            End Function

            Private Sub EnableMenuItemsRecursive(ByVal items As ToolStripItemCollection, Optional ByVal enable As Boolean = True)
                Dim menuItem As ToolStripMenuItem
                For Each item As ToolStripItem In items
                    menuItem = TryCast(item, ToolStripMenuItem)
                    If menuItem Is Nothing Then Continue For
                    menuItem.Enabled = enable
                    If menuItem.HasDropDownItems Then
                        EnableMenuItemsRecursive(menuItem.DropDownItems, enable)
                    End If
                Next
            End Sub

            Private Sub ShowHideTreeContextMenuItems(ByVal selectedNode As TreeNode)
                If selectedNode Is Nothing Then Return

                Try
                    cMenTree.Enabled = True
                    EnableMenuItemsRecursive(cMenTree.Items)

                    Select Case mRemoteNG.Tree.Node.GetNodeType(selectedNode)
                        Case mRemoteNG.Tree.Node.Type.Connection
                            Dim connectionInfo As mRemoteNG.Connection.Info = selectedNode.Tag

                            If connectionInfo.OpenConnections.Count = 0 Then
                                cMenTreeDisconnect.Enabled = False
                            End If

                            If Not (connectionInfo.Protocol = mRemoteNG.Connection.Protocol.Protocols.SSH1 Or _
                                    connectionInfo.Protocol = mRemoteNG.Connection.Protocol.Protocols.SSH2) Then
                                cMenTreeToolsTransferFile.Enabled = False
                            End If

                            If Not (connectionInfo.Protocol = mRemoteNG.Connection.Protocol.Protocols.RDP Or _
                                    connectionInfo.Protocol = mRemoteNG.Connection.Protocol.Protocols.ICA) Then
                                cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = False
                                cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = False
                            End If

                            cMenTreeToolsImportExport.Enabled = False
                        Case mRemoteNG.Tree.Node.Type.PuttySession
                            Dim puttySessionInfo As mRemoteNG.Connection.PuttySession.Info = selectedNode.Tag

                            cMenTreeAddConnection.Enabled = False
                            cMenTreeAddFolder.Enabled = False

                            If puttySessionInfo.OpenConnections.Count = 0 Then
                                cMenTreeDisconnect.Enabled = False
                            End If

                            If Not (puttySessionInfo.Protocol = mRemoteNG.Connection.Protocol.Protocols.SSH1 Or _
                                    puttySessionInfo.Protocol = mRemoteNG.Connection.Protocol.Protocols.SSH2) Then
                                cMenTreeToolsTransferFile.Enabled = False
                            End If

                            cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = False
                            cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = False
                            cMenTreeToolsImportExport.Enabled = False
                            cMenTreeToolsSort.Enabled = False
                            cMenTreeDuplicate.Enabled = False
                            cMenTreeRename.Enabled = False
                            cMenTreeDelete.Enabled = False
                            cMenTreeMoveUp.Enabled = False
                            cMenTreeMoveDown.Enabled = False
                        Case mRemoteNG.Tree.Node.Type.Container
                            cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = False
                            cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = False
                            cMenTreeDisconnect.Enabled = False

                            Dim openConnections As Integer = 0
                            Dim connectionInfo As mRemoteNG.Connection.Info
                            For Each node As TreeNode In selectedNode.Nodes
                                If TypeOf node.Tag Is mRemoteNG.Connection.Info Then
                                    connectionInfo = node.Tag
                                    openConnections = openConnections + connectionInfo.OpenConnections.Count
                                End If
                            Next
                            If openConnections = 0 Then
                                cMenTreeDisconnect.Enabled = False
                            End If

                            cMenTreeToolsTransferFile.Enabled = False
                            cMenTreeToolsExternalApps.Enabled = False
                        Case mRemoteNG.Tree.Node.Type.Root
                            cMenTreeConnect.Enabled = False
                            cMenTreeConnectWithOptions.Enabled = False
                            cMenTreeConnectWithOptionsConnectInFullscreen.Enabled = False
                            cMenTreeConnectWithOptionsConnectToConsoleSession.Enabled = False
                            cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Enabled = False
                            cMenTreeDisconnect.Enabled = False
                            cMenTreeToolsTransferFile.Enabled = False
                            cMenTreeToolsExternalApps.Enabled = False
                            cMenTreeDuplicate.Enabled = False
                            cMenTreeDelete.Enabled = False
                            cMenTreeMoveUp.Enabled = False
                            cMenTreeMoveDown.Enabled = False
                        Case mRemoteNG.Tree.Node.Type.PuttyRoot
                            cMenTreeAddConnection.Enabled = False
                            cMenTreeAddFolder.Enabled = False
                            cMenTreeConnect.Enabled = False
                            cMenTreeConnectWithOptions.Enabled = False
                            cMenTreeDisconnect.Enabled = False
                            cMenTreeToolsTransferFile.Enabled = False
                            cMenTreeConnectWithOptions.Enabled = False
                            cMenTreeToolsImportExport.Enabled = False
                            cMenTreeToolsSort.Enabled = False
                            cMenTreeToolsExternalApps.Enabled = False
                            cMenTreeDuplicate.Enabled = False
                            cMenTreeRename.Enabled = False
                            cMenTreeDelete.Enabled = False
                            cMenTreeMoveUp.Enabled = False
                            cMenTreeMoveDown.Enabled = False
                        Case Else
                            cMenTree.Enabled = False
                    End Select
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "ShowHideTreeContextMenuItems (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Drag and Drop"
            Private Sub tvConnections_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles tvConnections.DragDrop
                Try
                    'Check that there is a TreeNode being dragged
                    If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", True) = False Then Exit Sub

                    'Get the TreeView raising the event (incase multiple on form)
                    Dim selectedTreeview As TreeView = CType(sender, TreeView)

                    'Get the TreeNode being dragged
                    Dim dropNode As TreeNode = CType(e.Data.GetData("System.Windows.Forms.TreeNode"), TreeNode)

                    'The target node should be selected from the DragOver event
                    Dim targetNode As TreeNode = selectedTreeview.SelectedNode

                    If dropNode Is targetNode Then
                        Exit Sub
                    End If

                    If mRemoteNG.Tree.Node.GetNodeType(dropNode) = mRemoteNG.Tree.Node.Type.Root Then
                        Exit Sub
                    End If

                    If dropNode Is targetNode.Parent Then
                        Exit Sub
                    End If

                    'Remove the drop node from its current location
                    dropNode.Remove()

                    'If there is no targetNode add dropNode to the bottom of
                    'the TreeView root nodes, otherwise add it to the end of
                    'the dropNode child nodes

                    If mRemoteNG.Tree.Node.GetNodeType(targetNode) = mRemoteNG.Tree.Node.Type.Root Or mRemoteNG.Tree.Node.GetNodeType(targetNode) = mRemoteNG.Tree.Node.Type.Container Then
                        targetNode.Nodes.Insert(0, dropNode)
                    Else
                        targetNode.Parent.Nodes.Insert(targetNode.Index + 1, dropNode)
                    End If

                    If mRemoteNG.Tree.Node.GetNodeType(dropNode) = mRemoteNG.Tree.Node.Type.Connection Or mRemoteNG.Tree.Node.GetNodeType(dropNode) = mRemoteNG.Tree.Node.Type.Container Then
                        If mRemoteNG.Tree.Node.GetNodeType(dropNode.Parent) = mRemoteNG.Tree.Node.Type.Container Then
                            dropNode.Tag.Parent = dropNode.Parent.Tag
                        ElseIf mRemoteNG.Tree.Node.GetNodeType(dropNode.Parent) = mRemoteNG.Tree.Node.Type.Root Then
                            dropNode.Tag.Parent = Nothing
                            If mRemoteNG.Tree.Node.GetNodeType(dropNode) = mRemoteNG.Tree.Node.Type.Connection Then
                                dropNode.Tag.Inherit.TurnOffInheritanceCompletely()
                            ElseIf mRemoteNG.Tree.Node.GetNodeType(dropNode) = mRemoteNG.Tree.Node.Type.Container Then
                                dropNode.Tag.ConnectionInfo.Inherit.TurnOffInheritanceCompletely()
                            End If
                        End If
                    End If

                    'Ensure the newley created node is visible to
                    'the user and select it
                    dropNode.EnsureVisible()
                    selectedTreeview.SelectedNode = dropNode

                    SaveConnectionsBG()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_DragDrop (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub tvConnections_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles tvConnections.DragEnter
                Try
                    'See if there is a TreeNode being dragged
                    If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", True) Then
                        'TreeNode found allow move effect
                        e.Effect = DragDropEffects.Move
                    Else
                        'No TreeNode found, prevent move
                        e.Effect = DragDropEffects.None
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_DragEnter (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub tvConnections_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles tvConnections.DragOver
                Try
                    'Check that there is a TreeNode being dragged 
                    If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", True) = False Then Exit Sub

                    'Get the TreeView raising the event (incase multiple on form)
                    Dim selectedTreeview As TreeView = CType(sender, TreeView)

                    'As the mouse moves over nodes, provide feedback to 
                    'the user by highlighting the node that is the 
                    'current drop target
                    Dim pt As Point = CType(sender, TreeView).PointToClient(New Point(e.X, e.Y))
                    Dim targetNode As TreeNode = selectedTreeview.GetNodeAt(pt)

                    'Select the node currently under the cursor
                    selectedTreeview.SelectedNode = targetNode

                    'Check that the selected node is not the dropNode and
                    'also that it is not a child of the dropNode and 
                    'therefore an invalid target
                    Dim dropNode As TreeNode = CType(e.Data.GetData("System.Windows.Forms.TreeNode"), TreeNode)

                    Dim puttyRootInfo As Root.PuttySessions.Info
                    Do Until targetNode Is Nothing
                        puttyRootInfo = TryCast(targetNode.Tag, Root.PuttySessions.Info)
                        If puttyRootInfo IsNot Nothing Or targetNode Is dropNode Then
                            e.Effect = DragDropEffects.None
                            Return
                        End If
                        targetNode = targetNode.Parent
                    Loop

                    'Currently selected node is a suitable target
                    e.Effect = DragDropEffects.Move
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_DragOver (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub tvConnections_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles tvConnections.ItemDrag
                Try
                    Dim dragTreeNode As TreeNode = TryCast(e.Item, TreeNode)
                    If dragTreeNode Is Nothing Then Return

                    If dragTreeNode.Tag Is Nothing Then Return
                    If TypeOf dragTreeNode.Tag Is mRemoteNG.Connection.PuttySession.Info Or _
                       Not (TypeOf dragTreeNode.Tag Is mRemoteNG.Connection.Info Or _
                            TypeOf dragTreeNode.Tag Is Container.Info) Then
                        tvConnections.SelectedNode = dragTreeNode
                        Return
                    End If

                    'Set the drag node and initiate the DragDrop 
                    DoDragDrop(e.Item, DragDropEffects.Move)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_ItemDrag (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Tree Context Menu"
            Private Sub cMenTreeAddConnection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenTreeAddConnection.Click, mMenAddConnection.Click
                Me.AddConnection()
                SaveConnectionsBG()
            End Sub

            Private Sub cMenTreeAddFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenTreeAddFolder.Click, mMenAddFolder.Click
                Me.AddFolder()
                SaveConnectionsBG()
            End Sub

            Private Sub cMenTreeConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenTreeConnect.Click
                App.Runtime.OpenConnection(mRemoteNG.Connection.Info.Force.DoNotJump)
            End Sub

            Private Sub cMenTreeConnectWithOptionsConnectToConsoleSession_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenTreeConnectWithOptionsConnectToConsoleSession.Click
                App.Runtime.OpenConnection(mRemoteNG.Connection.Info.Force.UseConsoleSession Or mRemoteNG.Connection.Info.Force.DoNotJump)
            End Sub

            Private Sub cMenTreeConnectWithOptionsNoCredentials_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenTreeConnectWithOptionsNoCredentials.Click
                App.Runtime.OpenConnection(mRemoteNG.Connection.Info.Force.NoCredentials)
            End Sub

            Private Sub cMenTreeConnectWithOptionsDontConnectToConsoleSession_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenTreeConnectWithOptionsDontConnectToConsoleSession.Click
                App.Runtime.OpenConnection(mRemoteNG.Connection.Info.Force.DontUseConsoleSession Or mRemoteNG.Connection.Info.Force.DoNotJump)
            End Sub

            Private Sub cMenTreeConnectWithOptionsConnectInFullscreen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenTreeConnectWithOptionsConnectInFullscreen.Click
                App.Runtime.OpenConnection(mRemoteNG.Connection.Info.Force.Fullscreen Or mRemoteNG.Connection.Info.Force.DoNotJump)
            End Sub

            Private Sub cMenTreeConnectWithOptionsChoosePanelBeforeConnecting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenTreeConnectWithOptionsChoosePanelBeforeConnecting.Click
                App.Runtime.OpenConnection(mRemoteNG.Connection.Info.Force.OverridePanel Or mRemoteNG.Connection.Info.Force.DoNotJump)
            End Sub

            Private Sub cMenTreeDisconnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenTreeDisconnect.Click
                Me.DisconnectConnection()
            End Sub

            Private Sub cMenTreeToolsTransferFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenTreeToolsTransferFile.Click
                Me.SSHTransferFile()
            End Sub

            Private Sub cMenTreeToolsImportExportExportmRemoteXML_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenTreeToolsImportExportExportmRemoteXML.Click
                Me.ExportXML()
            End Sub

            Private Sub cMenTreeToolsImportExportImportmRemoteXML_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenTreeToolsImportExportImportmRemoteXML.Click
                Me.ImportXML()
            End Sub

            Private Sub cMenTreeToolsImportExportImportFromAD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenTreeToolsImportExportImportFromAD.Click
                Me.ImportFromAD()
            End Sub

            Private Sub cMenTreeToolsImportExportImportFromRDPFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenTreeToolsImportExportImportFromRDPFiles.Click
                Me.ImportFromRDPFiles()
            End Sub

            Private Sub cMenTreeToolsImportExportImportFromPortScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenTreeToolsImportExportImportFromPortScan.Click
                Me.ImportFromPortScan()
            End Sub

            Private Sub mMenSortAscending_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles mMenSortAscending.Click
                tvConnections.BeginUpdate()
                mRemoteNG.Tree.Node.Sort(tvConnections.Nodes.Item(0), System.Windows.Forms.SortOrder.Ascending)
                tvConnections.EndUpdate()
                SaveConnectionsBG()
            End Sub

            Private Sub cMenTreeToolsSortAscending_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cMenTreeToolsSortAscending.Click
                tvConnections.BeginUpdate()
                mRemoteNG.Tree.Node.Sort(tvConnections.SelectedNode, System.Windows.Forms.SortOrder.Ascending)
                tvConnections.EndUpdate()
                SaveConnectionsBG()
            End Sub

            Private Sub cMenTreeToolsSortDescending_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cMenTreeToolsSortDescending.Click
                tvConnections.BeginUpdate()
                mRemoteNG.Tree.Node.Sort(tvConnections.SelectedNode, System.Windows.Forms.SortOrder.Descending)
                tvConnections.EndUpdate()
                SaveConnectionsBG()
            End Sub

            Private Sub cMenTree_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles cMenTree.Opening
                AddExternalApps()
            End Sub

            Private Sub cMenTreeToolsExternalAppsEntry_Click(ByVal sender As Object, ByVal e As System.EventArgs)
                StartExternalApp(sender.Tag)
            End Sub

            Private Sub cMenTreeDuplicate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenTreeDuplicate.Click
                mRemoteNG.Tree.Node.CloneNode(tvConnections.SelectedNode)
                SaveConnectionsBG()
            End Sub

            Private Sub cMenTreeRename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenTreeRename.Click
                mRemoteNG.Tree.Node.StartRenameSelectedNode()
                SaveConnectionsBG()
            End Sub

            Private Sub cMenTreeDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenTreeDelete.Click
                mRemoteNG.Tree.Node.DeleteSelectedNode()
                SaveConnectionsBG()
            End Sub

            Private Sub cMenTreeMoveUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenTreeMoveUp.Click
                mRemoteNG.Tree.Node.MoveNodeUp()
                SaveConnectionsBG()
            End Sub

            Private Sub cMenTreeMoveDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenTreeMoveDown.Click
                mRemoteNG.Tree.Node.MoveNodeDown()
                SaveConnectionsBG()
            End Sub
#End Region

#Region "Context Menu Actions"
            Public Sub AddConnection()
                Try
                    If tvConnections.SelectedNode Is Nothing Then tvConnections.SelectedNode = tvConnections.Nodes.Item(0)

                    Dim newTreeNode As TreeNode = mRemoteNG.Tree.Node.AddNode(mRemoteNG.Tree.Node.Type.Connection)
                    If newTreeNode Is Nothing Then
                        MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "UI.Window.Tree.AddConnection() failed." & vbNewLine & "mRemoteNG.Tree.Node.AddNode() returned Nothing.", True)
                        Return
                    End If

                    Dim containerNode As TreeNode = tvConnections.SelectedNode
                    If mRemoteNG.Tree.Node.GetNodeType(containerNode) = mRemoteNG.Tree.Node.Type.Connection Then
                        containerNode = containerNode.Parent
                    End If

                    Dim newConnectionInfo As New mRemoteNG.Connection.Info()
                    If mRemoteNG.Tree.Node.GetNodeType(containerNode) = mRemoteNG.Tree.Node.Type.Root Then
                        newConnectionInfo.Inherit.TurnOffInheritanceCompletely()
                    Else
                        newConnectionInfo.Parent = containerNode.Tag
                    End If

                    newConnectionInfo.TreeNode = newTreeNode
                    newTreeNode.Tag = newConnectionInfo
                    ConnectionList.Add(newConnectionInfo)

                    containerNode.Nodes.Add(newTreeNode)

                    tvConnections.SelectedNode = newTreeNode
                    tvConnections.SelectedNode.BeginEdit()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "UI.Window.Tree.AddConnection() failed." & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Sub AddFolder()
                Try
                    Dim newNode As TreeNode = mRemoteNG.Tree.Node.AddNode(mRemoteNG.Tree.Node.Type.Container)
                    Dim newContainerInfo As New Container.Info()
                    newNode.Tag = newContainerInfo
                    newContainerInfo.TreeNode = newNode

                    Dim selectedNode As TreeNode = mRemoteNG.Tree.Node.SelectedNode
                    Dim parentNode As TreeNode
                    If selectedNode Is Nothing Then
                        parentNode = tvConnections.Nodes(0)
                    Else
                        If mRemoteNG.Tree.Node.GetNodeType(selectedNode) = mRemoteNG.Tree.Node.Type.Connection Then
                            parentNode = selectedNode.Parent
                        Else
                            parentNode = selectedNode
                        End If
                    End If

                    newContainerInfo.ConnectionInfo = New mRemoteNG.Connection.Info(newContainerInfo)

                    ' We can only inherit from a container node, not the root node or connection nodes
                    If mRemoteNG.Tree.Node.GetNodeType(parentNode) = mRemoteNG.Tree.Node.Type.Container Then
                        newContainerInfo.Parent = parentNode.Tag
                    Else
                        newContainerInfo.ConnectionInfo.Inherit.TurnOffInheritanceCompletely()
                    End If

                    App.Runtime.ContainerList.Add(newContainerInfo)
                    parentNode.Nodes.Add(newNode)

                    Me.tvConnections.SelectedNode = newNode
                    Me.tvConnections.SelectedNode.BeginEdit()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, String.Format(My.Language.strErrorAddFolderFailed, ex.Message), True)
                End Try
            End Sub

            Private Sub DisconnectConnection()
                Try
                    If Me.tvConnections.SelectedNode IsNot Nothing Then
                        If TypeOf Me.tvConnections.SelectedNode.Tag Is mRemoteNG.Connection.Info Then
                            Dim conI As mRemoteNG.Connection.Info = Me.tvConnections.SelectedNode.Tag
                            For i As Integer = 0 To conI.OpenConnections.Count - 1
                                conI.OpenConnections(i).Disconnect()
                            Next
                        End If

                        If TypeOf Me.tvConnections.SelectedNode.Tag Is mRemoteNG.Container.Info Then
                            For Each n As TreeNode In Me.tvConnections.SelectedNode.Nodes
                                If TypeOf n.Tag Is mRemoteNG.Connection.Info Then
                                    Dim conI As mRemoteNG.Connection.Info = n.Tag
                                    For i As Integer = 0 To conI.OpenConnections.Count - 1
                                        conI.OpenConnections(i).Disconnect()
                                    Next
                                End If
                            Next
                        End If
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "DisconnectConnection (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SSHTransferFile()
                Try
                    Windows.Show(Type.SSHTransfer)

                    Dim conI As mRemoteNG.Connection.Info = mRemoteNG.Tree.Node.SelectedNode.Tag

                    Windows.sshtransferForm.Hostname = conI.Hostname
                    Windows.sshtransferForm.Username = conI.Username
                    Windows.sshtransferForm.Password = conI.Password
                    Windows.sshtransferForm.Port = conI.Port
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SSHTransferFile (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub ExportXML()
                Try
                    If Me.tvConnections.SelectedNode IsNot Nothing Then
                        Windows.exportForm = New UI.Window.Export(Windows.exportPanel, Me.tvConnections.SelectedNode)
                        Windows.exportPanel = Windows.exportForm

                        Windows.exportForm.Show(frmMain.pnlDock)
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "ExportXml (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub ImportXML()
                Try
                    If Me.tvConnections.SelectedNode IsNot Nothing Then
                        App.Runtime.ImportConnections()
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "ImportXML (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub ImportFromAD()
                Try
                    If Me.tvConnections.SelectedNode IsNot Nothing Then
                        Windows.Show(Type.ADImport)
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "ImportFromAD (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub ImportFromRDPFiles()
                Try
                    App.Runtime.ImportConnectionsRdpFile()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "ImportFromRDPFiles (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub ImportFromPortScan()
                Try
                    If Me.tvConnections.SelectedNode IsNot Nothing Then
                        Windows.Show(Type.PortScan, Tools.PortScan.PortScanMode.Import)
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "ImportFromPortScan (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub AddExternalApps()
                Try
                    'clean up
                    cMenTreeToolsExternalApps.DropDownItems.Clear()

                    'add ext apps
                    For Each extA As Tools.ExternalTool In ExternalTools
                        Dim nItem As New ToolStripMenuItem
                        nItem.Text = extA.DisplayName
                        nItem.Tag = extA

                        nItem.Image = extA.Image

                        AddHandler nItem.Click, AddressOf cMenTreeToolsExternalAppsEntry_Click

                        cMenTreeToolsExternalApps.DropDownItems.Add(nItem)
                    Next
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "cMenTreeTools_DropDownOpening failed (UI.Window.Tree)" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub StartExternalApp(ByVal ExtA As Tools.ExternalTool)
                Try
                    If mRemoteNG.Tree.Node.GetNodeType(mRemoteNG.Tree.Node.SelectedNode) = mRemoteNG.Tree.Node.Type.Connection Or _
                       mRemoteNG.Tree.Node.GetNodeType(mRemoteNG.Tree.Node.SelectedNode) = mRemoteNG.Tree.Node.Type.PuttySession Then
                        ExtA.Start(mRemoteNG.Tree.Node.SelectedNode.Tag)
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "cMenTreeToolsExternalAppsEntry_Click failed (UI.Window.Tree)" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Menu"
            Private Sub mMenViewExpandAllFolders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenViewExpandAllFolders.Click
                mRemoteNG.Tree.Node.ExpandAllNodes()
            End Sub

            Private Sub mMenViewCollapseAllFolders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mMenViewCollapseAllFolders.Click
                If tvConnections.SelectedNode IsNot Nothing Then
                    If tvConnections.SelectedNode.IsEditing Then tvConnections.SelectedNode.EndEdit(False)
                End If
                mRemoteNG.Tree.Node.CollapseAllNodes()
            End Sub
#End Region

#Region "Search"
            Private Sub txtSearch_GotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles txtSearch.GotFocus
                txtSearch.ForeColor = Themes.ThemeManager.ActiveTheme.SearchBoxTextColor
                If txtSearch.Text = Language.strSearchPrompt Then
                    txtSearch.Text = ""
                End If
            End Sub

            Private Sub txtSearch_LostFocus(ByVal sender As Object, ByVal e As EventArgs) Handles txtSearch.LostFocus
                If txtSearch.Text = "" Then
                    txtSearch.ForeColor = Themes.ThemeManager.ActiveTheme.SearchBoxTextPromptColor
                    txtSearch.Text = Language.strSearchPrompt
                End If
            End Sub

            Private Sub txtSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyDown
                Try
                    If e.KeyCode = Keys.Escape Then
                        e.Handled = True
                        Me.tvConnections.Focus()
                    ElseIf e.KeyCode = Keys.Up Then
                        Me.tvConnections.SelectedNode = Me.tvConnections.SelectedNode.PrevVisibleNode
                    ElseIf e.KeyCode = Keys.Down Then
                        Me.tvConnections.SelectedNode = Me.tvConnections.SelectedNode.NextVisibleNode
                    Else
                        Me.tvConnections_KeyDown(sender, e)
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "txtSearch_KeyDown (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub txtSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
                Me.tvConnections.SelectedNode = mRemoteNG.Tree.Node.Find(Me.tvConnections.Nodes(0), Me.txtSearch.Text)
            End Sub

            Private Sub tvConnections_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tvConnections.KeyPress
                Try
                    If Char.IsLetterOrDigit(e.KeyChar) Then
                        Me.txtSearch.Text = e.KeyChar

                        Me.txtSearch.Focus()
                        Me.txtSearch.SelectionStart = Me.txtSearch.TextLength
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_KeyPress (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub tvConnections_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tvConnections.KeyDown
                Try
                    If e.KeyCode = Keys.Enter Then
                        If TypeOf Me.tvConnections.SelectedNode.Tag Is mRemoteNG.Connection.Info Then
                            e.Handled = True
                            App.Runtime.OpenConnection()
                        Else
                            If Me.tvConnections.SelectedNode.IsExpanded Then
                                Me.tvConnections.SelectedNode.Collapse(True)
                            Else
                                Me.tvConnections.SelectedNode.Expand()
                            End If
                        End If
                    ElseIf e.KeyCode = Keys.Escape Xor e.KeyCode = Keys.Control Or e.KeyCode = Keys.F Then
                        Me.txtSearch.Focus()
                        Me.txtSearch.SelectionStart = Me.txtSearch.TextLength
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "tvConnections_KeyDown (UI.Window.Tree) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region
        End Class
    End Namespace
End Namespace