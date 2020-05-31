using System;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;

namespace mRemoteNG.UI.Menu
{
    public class MainFileMenu : ToolStripMenuItem
    {
        private ToolStripMenuItem _mMenFileNew;
        private ToolStripMenuItem _mMenFileLoad;
        private ToolStripMenuItem _mMenFileSave;
        private ToolStripMenuItem _mMenFileSaveAs;
        private ToolStripSeparator _mMenFileSep1;
        private ToolStripMenuItem _mMenFileExit;
        private ToolStripMenuItem _mMenFileDuplicate;
        private ToolStripSeparator _mMenFileSep2;
        private ToolStripMenuItem _mMenFileNewConnection;
        private ToolStripMenuItem _mMenFileNewFolder;
        private ToolStripSeparator _mMenFileSep3;
        private ToolStripSeparator _mMenFileSep4;
        private ToolStripMenuItem _mMenFileDelete;
        private ToolStripMenuItem _mMenFileRename;
        private ToolStripSeparator _mMenFileSep5;
        private ToolStripMenuItem _mMenFileExport;
        private ToolStripMenuItem _mMenFileImportFromFile;
        private ToolStripMenuItem _mMenFileImportFromActiveDirectory;
        private ToolStripMenuItem _mMenFileImportFromPortScan;
        private ToolStripMenuItem _mMenFileImport;
        private ToolStripMenuItem _mMenReconnectAll;

        public ConnectionTreeWindow TreeWindow { get; set; }
        public IConnectionInitiator ConnectionInitiator { get; set; }

        public MainFileMenu()
        {
            Initialize();
        }

        private void Initialize()
        {
            _mMenFileNewConnection = new ToolStripMenuItem();
            _mMenFileNewFolder = new ToolStripMenuItem();
            _mMenFileSep1 = new ToolStripSeparator();
            _mMenFileNew = new ToolStripMenuItem();
            _mMenFileLoad = new ToolStripMenuItem();
            _mMenFileSave = new ToolStripMenuItem();
            _mMenFileSaveAs = new ToolStripMenuItem();
            _mMenFileSep2 = new ToolStripSeparator();
            _mMenFileDelete = new ToolStripMenuItem();
            _mMenFileRename = new ToolStripMenuItem();
            _mMenFileDuplicate = new ToolStripMenuItem();
            _mMenFileSep4 = new ToolStripSeparator();
            _mMenReconnectAll = new ToolStripMenuItem();
            _mMenFileSep3 = new ToolStripSeparator();
            _mMenFileImport = new ToolStripMenuItem();
            _mMenFileImportFromFile = new ToolStripMenuItem();
            _mMenFileImportFromActiveDirectory = new ToolStripMenuItem();
            _mMenFileImportFromPortScan = new ToolStripMenuItem();
            _mMenFileExport = new ToolStripMenuItem();
            _mMenFileSep5 = new ToolStripSeparator();
            _mMenFileExit = new ToolStripMenuItem();

            // 
            // mMenFile
            // 
            DropDownItems.AddRange(new ToolStripItem[]
            {
                _mMenFileNewConnection,
                _mMenFileNewFolder,
                _mMenFileSep1,
                _mMenFileNew,
                _mMenFileLoad,
                _mMenFileSave,
                _mMenFileSaveAs,
                _mMenFileSep2,
                _mMenFileDelete,
                _mMenFileRename,
                _mMenFileDuplicate,
                _mMenFileSep4,
                _mMenReconnectAll,
                _mMenFileSep3,
                _mMenFileImport,
                _mMenFileExport,
                _mMenFileSep5,
                _mMenFileExit
            });
            Name = "mMenFile";
            Size = new System.Drawing.Size(37, 20);
            Text = Language._File;
            //DropDownOpening += mMenFile_DropDownOpening;
            DropDownClosed += OnDropDownClosed;
            // 
            // mMenFileNewConnection
            // 
            _mMenFileNewConnection.Image = Resources.Connection_Add;
            _mMenFileNewConnection.Name = "mMenFileNewConnection";
            _mMenFileNewConnection.ShortcutKeys = Keys.Control | Keys.N;
            _mMenFileNewConnection.Size = new System.Drawing.Size(281, 22);
            _mMenFileNewConnection.Text = Language.NewConnection;
            _mMenFileNewConnection.Click += mMenFileNewConnection_Click;
            // 
            // mMenFileNewFolder
            // 
            _mMenFileNewFolder.Image = Resources.Folder_Add;
            _mMenFileNewFolder.Name = "mMenFileNewFolder";
            _mMenFileNewFolder.ShortcutKeys = (Keys.Control | Keys.Shift)
                                            | Keys.N;
            _mMenFileNewFolder.Size = new System.Drawing.Size(281, 22);
            _mMenFileNewFolder.Text = Language.NewFolder;
            _mMenFileNewFolder.Click += mMenFileNewFolder_Click;
            // 
            // mMenFileSep1
            // 
            _mMenFileSep1.Name = "mMenFileSep1";
            _mMenFileSep1.Size = new System.Drawing.Size(278, 6);
            // 
            // mMenFileNew
            // 
            _mMenFileNew.Image = Resources.Connections_New;
            _mMenFileNew.Name = "mMenFileNew";
            _mMenFileNew.Size = new System.Drawing.Size(281, 22);
            _mMenFileNew.Text = Language.NewConnectionFile;
            _mMenFileNew.Click += mMenFileNew_Click;
            // 
            // mMenFileLoad
            // 
            _mMenFileLoad.Image = Resources.Connections_Load;
            _mMenFileLoad.Name = "mMenFileLoad";
            _mMenFileLoad.ShortcutKeys = Keys.Control | Keys.O;
            _mMenFileLoad.Size = new System.Drawing.Size(281, 22);
            _mMenFileLoad.Text = Language.OpenConnectionFile;
            _mMenFileLoad.Click += mMenFileLoad_Click;
            // 
            // mMenFileSave
            // 
            _mMenFileSave.Image = Resources.Connections_Save;
            _mMenFileSave.Name = "mMenFileSave";
            _mMenFileSave.ShortcutKeys = Keys.Control | Keys.S;
            _mMenFileSave.Size = new System.Drawing.Size(281, 22);
            _mMenFileSave.Text = Language.SaveConnectionFile;
            _mMenFileSave.Click += mMenFileSave_Click;
            // 
            // mMenFileSaveAs
            // 
            _mMenFileSaveAs.Image = Resources.Connections_SaveAs;
            _mMenFileSaveAs.Name = "mMenFileSaveAs";
            _mMenFileSaveAs.ShortcutKeys = (Keys.Control | Keys.Shift)
                                         | Keys.S;
            _mMenFileSaveAs.Size = new System.Drawing.Size(281, 22);
            _mMenFileSaveAs.Text = Language.SaveConnectionFileAs;
            _mMenFileSaveAs.Click += mMenFileSaveAs_Click;
            // 
            // mMenFileSep2
            // 
            _mMenFileSep2.Name = "mMenFileSep2";
            _mMenFileSep2.Size = new System.Drawing.Size(278, 6);
            // 
            // mMenFileDelete
            // 
            _mMenFileDelete.Image = Resources.Delete;
            _mMenFileDelete.Name = "mMenFileDelete";
            _mMenFileDelete.Size = new System.Drawing.Size(281, 22);
            _mMenFileDelete.Text = Language.Delete;
            _mMenFileDelete.Click += mMenFileDelete_Click;
            // 
            // mMenFileRename
            // 
            _mMenFileRename.Image = Resources.Rename;
            _mMenFileRename.Name = "mMenFileRename";
            _mMenFileRename.Size = new System.Drawing.Size(281, 22);
            _mMenFileRename.Text = Language.Rename;
            _mMenFileRename.Click += mMenFileRename_Click;
            // 
            // mMenFileDuplicate
            // 
            _mMenFileDuplicate.Image = Resources.page_copy;
            _mMenFileDuplicate.Name = "mMenFileDuplicate";
            _mMenFileDuplicate.Size = new System.Drawing.Size(281, 22);
            _mMenFileDuplicate.Text = Language.Duplicate;
            _mMenFileDuplicate.Click += mMenFileDuplicate_Click;
            // 
            // mMenFileSep4
            // 
            _mMenFileSep4.Name = "mMenFileSep4";
            _mMenFileSep4.Size = new System.Drawing.Size(278, 6);
            // 
            // mMenReconnectAll
            // 
            _mMenReconnectAll.Image = Resources.Refresh;
            _mMenReconnectAll.Name = "mMenReconnectAll";
            _mMenReconnectAll.Size = new System.Drawing.Size(281, 22);
            _mMenReconnectAll.Text = Language.ReconnectAllConnections;
            _mMenReconnectAll.Click += mMenReconnectAll_Click;
            // 
            // mMenFileSep3
            // 
            _mMenFileSep3.Name = "mMenFileSep3";
            _mMenFileSep3.Size = new System.Drawing.Size(278, 6);
            // 
            // mMenFileImport
            // 
            _mMenFileImport.DropDownItems.AddRange(new ToolStripItem[]
            {
                _mMenFileImportFromFile,
                _mMenFileImportFromActiveDirectory,
                _mMenFileImportFromPortScan
            });
            _mMenFileImport.Name = "mMenFileImport";
            _mMenFileImport.Size = new System.Drawing.Size(281, 22);
            _mMenFileImport.Text = Language._Import;
            // 
            // mMenFileImportFromFile
            // 
            _mMenFileImportFromFile.Name = "mMenFileImportFromFile";
            _mMenFileImportFromFile.Size = new System.Drawing.Size(235, 22);
            _mMenFileImportFromFile.Text = Language.ImportFromFile;
            _mMenFileImportFromFile.Click += mMenFileImportFromFile_Click;
            // 
            // mMenFileImportFromActiveDirectory
            // 
            _mMenFileImportFromActiveDirectory.Name = "mMenFileImportFromActiveDirectory";
            _mMenFileImportFromActiveDirectory.Size = new System.Drawing.Size(235, 22);
            _mMenFileImportFromActiveDirectory.Text = Language.ImportAD;
            _mMenFileImportFromActiveDirectory.Click += mMenFileImportFromActiveDirectory_Click;
            // 
            // mMenFileImportFromPortScan
            // 
            _mMenFileImportFromPortScan.Name = "mMenFileImportFromPortScan";
            _mMenFileImportFromPortScan.Size = new System.Drawing.Size(235, 22);
            _mMenFileImportFromPortScan.Text = Language.ImportPortScan;
            _mMenFileImportFromPortScan.Click += mMenFileImportFromPortScan_Click;
            // 
            // mMenFileExport
            // 
            _mMenFileExport.Name = "mMenFileExport";
            _mMenFileExport.Size = new System.Drawing.Size(281, 22);
            _mMenFileExport.Text = Language._ExportToFile;
            _mMenFileExport.Click += mMenFileExport_Click;
            // 
            // mMenFileSep5
            // 
            _mMenFileSep5.Name = "mMenFileSep5";
            _mMenFileSep5.Size = new System.Drawing.Size(278, 6);
            // 
            // mMenFileExit
            // 
            _mMenFileExit.Image = Resources.Quit;
            _mMenFileExit.Name = "mMenFileExit";
            _mMenFileExit.ShortcutKeys = Keys.Alt | Keys.F4;
            _mMenFileExit.Size = new System.Drawing.Size(281, 22);
            _mMenFileExit.Text = Language.Exit;
            _mMenFileExit.Click += mMenFileExit_Click;
        }

        public void ApplyLanguage()
        {
            Text = Language._File;
            _mMenFileNewConnection.Text = Language.NewConnection;
            _mMenFileNewFolder.Text = Language.NewFolder;
            _mMenFileNew.Text = Language.NewConnectionFile;
            _mMenFileLoad.Text = Language.OpenConnectionFile;
            _mMenFileSave.Text = Language.SaveConnectionFile;
            _mMenFileSaveAs.Text = Language.SaveConnectionFileAs;
            _mMenFileImport.Text = Language._Import;
            _mMenFileImportFromFile.Text = Language.ImportFromFile;
            _mMenFileImportFromActiveDirectory.Text = Language.ImportAD;
            _mMenFileImportFromPortScan.Text = Language.ImportPortScan;
            _mMenFileExport.Text = Language._ExportToFile;
            _mMenFileExit.Text = Language.Exit;
        }

        #region File

        internal void mMenFile_DropDownOpening(object sender, EventArgs e)
        {
            var selectedNodeType = TreeWindow.SelectedNode?.GetTreeNodeType();
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (selectedNodeType)
            {
                case TreeNodeType.Root:
                    _mMenFileNewConnection.Enabled = true;
                    _mMenFileNewFolder.Enabled = true;
                    _mMenFileDelete.Enabled = false;
                    _mMenFileRename.Enabled = true;
                    _mMenFileDuplicate.Enabled = false;
                    _mMenReconnectAll.Enabled = true;
                    _mMenFileDelete.Text = Language.Delete;
                    _mMenFileRename.Text = Language.RenameFolder;
                    _mMenFileDuplicate.Text = Language.Duplicate;
                    _mMenReconnectAll.Text = Language.ReconnectAll;
                    break;
                case TreeNodeType.Container:
                    _mMenFileNewConnection.Enabled = true;
                    _mMenFileNewFolder.Enabled = true;
                    _mMenFileDelete.Enabled = true;
                    _mMenFileRename.Enabled = true;
                    _mMenFileDuplicate.Enabled = true;
                    _mMenReconnectAll.Enabled = true;
                    _mMenFileDelete.Text = Language.DeleteFolder;
                    _mMenFileRename.Text = Language.RenameFolder;
                    _mMenFileDuplicate.Text = Language.DuplicateFolder;
                    _mMenReconnectAll.Text = Language.ReconnectAll;
                    break;
                case TreeNodeType.Connection:
                    _mMenFileNewConnection.Enabled = true;
                    _mMenFileNewFolder.Enabled = true;
                    _mMenFileDelete.Enabled = true;
                    _mMenFileRename.Enabled = true;
                    _mMenFileDuplicate.Enabled = true;
                    _mMenReconnectAll.Enabled = true;
                    _mMenFileDelete.Text = Language.DeleteConnection;
                    _mMenFileRename.Text = Language.RenameConnection;
                    _mMenFileDuplicate.Text = Language.DuplicateConnection;
                    _mMenReconnectAll.Text = Language.ReconnectAll;
                    break;
                case TreeNodeType.PuttyRoot:
                case TreeNodeType.PuttySession:
                    _mMenFileNewConnection.Enabled = false;
                    _mMenFileNewFolder.Enabled = false;
                    _mMenFileDelete.Enabled = false;
                    _mMenFileRename.Enabled = false;
                    _mMenFileDuplicate.Enabled = false;
                    _mMenReconnectAll.Enabled = true;
                    _mMenFileDelete.Text = Language.Delete;
                    _mMenFileRename.Text = Language.Rename;
                    _mMenFileDuplicate.Text = Language.Duplicate;
                    _mMenReconnectAll.Text = Language.ReconnectAll;
                    break;
                default:
                    _mMenFileNewConnection.Enabled = true;
                    _mMenFileNewFolder.Enabled = true;
                    _mMenFileDelete.Enabled = false;
                    _mMenFileRename.Enabled = false;
                    _mMenFileDuplicate.Enabled = false;
                    _mMenReconnectAll.Enabled = true;
                    _mMenFileDelete.Text = Language.Delete;
                    _mMenFileRename.Text = Language.Rename;
                    _mMenFileDuplicate.Text = Language.Duplicate;
                    _mMenReconnectAll.Text = Language.ReconnectAll;
                    break;
            }
        }

        private void OnDropDownClosed(object sender, EventArgs eventArgs)
        {
            _mMenFileNewConnection.Enabled = true;
            _mMenFileNewFolder.Enabled = true;
            _mMenFileDelete.Enabled = true;
            _mMenFileRename.Enabled = true;
            _mMenFileDuplicate.Enabled = true;
            _mMenReconnectAll.Enabled = true;
        }

        private void mMenFileNewConnection_Click(object sender, EventArgs e)
        {
            TreeWindow.ConnectionTree.AddConnection();
        }

        private void mMenFileNewFolder_Click(object sender, EventArgs e)
        {
            TreeWindow.ConnectionTree.AddFolder();
        }

        private void mMenFileNew_Click(object sender, EventArgs e)
        {
            using (var saveFileDialog = DialogFactory.ConnectionsSaveAsDialog())
            {
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                Runtime.ConnectionsService.NewConnectionsFile(saveFileDialog.FileName);
            }
        }

        private void mMenFileLoad_Click(object sender, EventArgs e)
        {
            if (Runtime.ConnectionsService.IsConnectionsFileLoaded)
            {
                var msgBoxResult = MessageBox.Show(Language.SaveConnectionsFileBeforeOpeningAnother,
                                                   Language.Save, MessageBoxButtons.YesNoCancel);
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (msgBoxResult)
                {
                    case DialogResult.Yes:
                        Runtime.ConnectionsService.SaveConnections();
                        break;
                    case DialogResult.Cancel:
                        return;
                }
            }

            Runtime.LoadConnections(true);
        }

        private void mMenFileSave_Click(object sender, EventArgs e)
        {
            Runtime.ConnectionsService.SaveConnectionsAsync();
        }

        private void mMenFileSaveAs_Click(object sender, EventArgs e)
        {
            using (var saveFileDialog = DialogFactory.ConnectionsSaveAsDialog())
            {
                if (saveFileDialog.ShowDialog(FrmMain.Default) != DialogResult.OK)
                    return;

                var newFileName = saveFileDialog.FileName;

                Runtime.ConnectionsService.SaveConnections(Runtime.ConnectionsService.ConnectionTreeModel, false,
                                                           new SaveFilter(), newFileName);

                if (newFileName == Runtime.ConnectionsService.GetDefaultStartupConnectionFileName())
                {
                    Settings.Default.LoadConsFromCustomLocation = false;
                }
                else
                {
                    Settings.Default.LoadConsFromCustomLocation = true;
                    Settings.Default.CustomConsPath = newFileName;
                }
            }
        }

        private void mMenFileDelete_Click(object sender, EventArgs e)
        {
            TreeWindow.ConnectionTree.DeleteSelectedNode();
        }

        private void mMenFileRename_Click(object sender, EventArgs e)
        {
            TreeWindow.ConnectionTree.RenameSelectedNode();
        }

        private void mMenFileDuplicate_Click(object sender, EventArgs e)
        {
            TreeWindow.ConnectionTree.DuplicateSelectedNode();
        }

        private void mMenReconnectAll_Click(object sender, EventArgs e)
        {
            if (Runtime.WindowList == null || Runtime.WindowList.Count == 0) return;
            foreach (BaseWindow window in Runtime.WindowList)
            {
                if (!(window is ConnectionWindow connectionWindow))
                    return;

                connectionWindow.reconnectAll(ConnectionInitiator);
            }
        }

        private void mMenFileImportFromFile_Click(object sender, EventArgs e)
        {
            var selectedNode = TreeWindow.SelectedNode;
            ContainerInfo importDestination;
            if (selectedNode == null)
                importDestination = Runtime.ConnectionsService.ConnectionTreeModel.RootNodes.First();
            else
                importDestination = selectedNode as ContainerInfo ?? selectedNode.Parent;
            Import.ImportFromFile(importDestination);
        }

        private void mMenFileImportFromActiveDirectory_Click(object sender, EventArgs e)
        {
            Windows.Show(WindowType.ActiveDirectoryImport);
        }

        private void mMenFileImportFromPortScan_Click(object sender, EventArgs e)
        {
            Windows.Show(WindowType.PortScan);
        }

        private void mMenFileExport_Click(object sender, EventArgs e)
        {
            Export.ExportToFile(Windows.TreeForm.SelectedNode, Runtime.ConnectionsService.ConnectionTreeModel);
        }

        private void mMenFileExit_Click(object sender, EventArgs e)
        {
            Shutdown.Quit();
        }

        #endregion
    }
}