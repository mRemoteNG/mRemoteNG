using System;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Properties;
using mRemoteNG.Security;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Menu
{
    [SupportedOSPlatform("windows")]
    public class FileMenu : ToolStripMenuItem
    {
        private ToolStripMenuItem _mMenToolsOptions = null!;
        private ToolStripMenuItem _mMenNewConnection = null!;
        private ToolStripMenuItem _mMenFileNew = null!;
        private ToolStripMenuItem _mMenFileLoad = null!;
        private ToolStripMenuItem _mMenFileSave = null!;
        private ToolStripMenuItem _mMenRecentConnections = null!;
        private ToolStripMenuItem _mMenFileSaveAs = null!;
        private ToolStripMenuItem _mMenFileExit = null!;
        private ToolStripSeparator _mMenFileSep2 = null!;
        private ToolStripSeparator _mMenFileSep1 = null!;

        public ConnectionTreeWindow? TreeWindow { get; set; }

        public FileMenu()
        {
            Initialize();
        }

        private void Initialize()
        {
            _mMenNewConnection = new ToolStripMenuItem();
            _mMenFileNew = new ToolStripMenuItem();
            _mMenFileLoad = new ToolStripMenuItem();
            _mMenFileSave = new ToolStripMenuItem();
            _mMenFileSaveAs = new ToolStripMenuItem();
            _mMenFileSep2 = new ToolStripSeparator();
            _mMenFileSep1 = new ToolStripSeparator();
            _mMenFileExit = new ToolStripMenuItem();
            _mMenToolsOptions = new ToolStripMenuItem();
            _mMenRecentConnections = new ToolStripMenuItem();

            _mMenRecentConnections.Name = "mMenRecentConnections";
            _mMenRecentConnections.Text = "Recent Connections";

            RecentConnectionsService.Instance.RecentConnectionsChanged += (s, e) =>
            {
                if (FrmMain.IsCreated && FrmMain.Default.InvokeRequired)
                {
                    FrmMain.Default.BeginInvoke(new Action(RebuildRecentConnectionsMenu));
                }
                else
                {
                    RebuildRecentConnectionsMenu();
                }
            };

            // 
            // mMenFile
            // 
            DropDownItems.AddRange(new ToolStripItem[]
            {
                _mMenNewConnection,
                _mMenFileNew,
                _mMenFileLoad,
                _mMenRecentConnections,
                _mMenFileSave,
                _mMenFileSaveAs,
                _mMenFileSep1,
                _mMenToolsOptions,
                _mMenFileSep2,
                _mMenFileExit
            });
            Name = "mMenFile";
            Size = new System.Drawing.Size(37, 20);
            Text = Language._File;
            DropDownOpening += mMenFile_DropDownOpening;
            // 
            // mMenNewConnection
            // 
            _mMenNewConnection.Image = Properties.Resources.AddItem_16x;
            _mMenNewConnection.Name = "mMenNewConnection";
            _mMenNewConnection.Size = new System.Drawing.Size(281, 22);
            _mMenNewConnection.Text = Language.NewConnection;
            _mMenNewConnection.Click += mMenNewConnection_Click;
            // 
            // mMenFileNew
            // 
            _mMenFileNew.Image = Properties.Resources.NewFile_16x;
            _mMenFileNew.Name = "mMenFileNew";
            _mMenFileLoad.ShortcutKeys = Keys.Control | Keys.N;
            _mMenFileNew.Size = new System.Drawing.Size(281, 22);
            _mMenFileNew.Text = Language.NewConnectionFile;
            _mMenFileNew.Click += mMenFileNew_Click;
            // 
            // mMenFileLoad
            // 
            _mMenFileLoad.Image = Properties.Resources.OpenFile_16x;
            _mMenFileLoad.Name = "mMenFileLoad";
            _mMenFileLoad.ShortcutKeys = Keys.Control | Keys.O;
            _mMenFileLoad.Size = new System.Drawing.Size(281, 22);
            _mMenFileLoad.Text = Language.OpenConnectionFile;
            _mMenFileLoad.Click += mMenFileLoad_Click;
            // 
            // mMenFileSave
            // 
            _mMenFileSave.Name = "mMenFileSave";
            _mMenFileSave.ShortcutKeys = Keys.Control | Keys.S;
            _mMenFileSave.Size = new System.Drawing.Size(281, 22);
            _mMenFileSave.Text = Language.SaveConnectionFile;
            _mMenFileSave.Click += mMenFileSave_Click;
            
            RebuildRecentConnectionsMenu();
            // 
            // mMenFileSaveAs
            // 
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
            // mMenFileSep1
            // 
            _mMenFileSep1.Name = "mMenFileSep3";
            _mMenFileSep1.Size = new System.Drawing.Size(278, 6);
            // 
            // mMenToolsOptions
            // 
            _mMenToolsOptions.Image = Properties.Resources.Settings_16x;
            _mMenToolsOptions.Name = "mMenToolsOptions";
            _mMenToolsOptions.Size = new System.Drawing.Size(184, 22);
            _mMenToolsOptions.Text = Language.OptionsMenuItem;
            _mMenToolsOptions.Click += mMenToolsOptions_Click;
            // 
            // mMenFileExit
            // 
            _mMenFileExit.Image = Properties.Resources.CloseSolution_16x;
            _mMenFileExit.Name = "mMenFileExit";
            _mMenFileExit.ShortcutKeys = Keys.Alt | Keys.F4;
            _mMenFileExit.Size = new System.Drawing.Size(281, 22);
            _mMenFileExit.Text = Language.Exit;
            _mMenFileExit.Click += mMenFileExit_Click;
        }

        public void ApplyLanguage()
        {
            Text = Language._File;
            _mMenNewConnection.Text = Language.NewConnection;
            _mMenFileNew.Text = Language.NewConnectionFile;
            _mMenFileLoad.Text = Language.OpenConnectionFile;
            _mMenFileSave.Text = Language.SaveConnectionFile;
            _mMenFileSaveAs.Text = Language.SaveConnectionFileAs;
            _mMenToolsOptions.Text = Language.OptionsMenuItem;
            _mMenFileExit.Text = Language.Exit;
            _mMenRecentConnections.Text = "Recent Connections";
        }

        private void mMenFile_DropDownOpening(object sender, EventArgs e)
        {
            // Hide "Save As" when connections are stored in a database — saving to an
            // XML file while the authoritative source is SQL is misleading and unsafe.
            _mMenFileSaveAs.Visible = !Runtime.ConnectionsService.UsingDatabase;
        }

        private void RebuildRecentConnectionsMenu()
        {
            if (_mMenRecentConnections == null) return;
            
            _mMenRecentConnections.DropDownItems.Clear();
            var recent = RecentConnectionsService.Instance.GetRecentConnections().ToList();

            if (recent.Count == 0)
            {
                _mMenRecentConnections.Enabled = false;
                return;
            }

            _mMenRecentConnections.Enabled = true;
            foreach (var conn in recent)
            {
                var item = new ToolStripMenuItem(conn.Name);
                item.Tag = conn;
                item.Click += (s, e) =>
                {
                    if (s is ToolStripMenuItem menuItem && menuItem.Tag is ConnectionInfo connection)
                    {
                        Runtime.ConnectionInitiator.OpenConnection(connection);
                    }
                };
                
                _mMenRecentConnections.DropDownItems.Add(item);
            }
        }

        #region File

        private void mMenNewConnection_Click(object sender, EventArgs e)
        {
            TreeWindow?.ConnectionTree.AddConnection();
        }

        private void mMenFileNew_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = DialogFactory.ConnectionsSaveAsDialog())
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
                // Load as additional connection file — supports multiple files open simultaneously (#2331)
                using OpenFileDialog loadDialog = DialogFactory.BuildLoadConnectionsDialog();
                if (loadDialog.ShowDialog() != DialogResult.OK) return;
                Runtime.ConnectionsService.LoadAdditionalConnectionFile(loadDialog.FileName);
                return;
            }

            Runtime.LoadConnections(true);
        }

        private void mMenFileSave_Click(object sender, EventArgs e)
        {
            Runtime.ConnectionsService.SaveConnectionsAsync();
        }

        private void mMenFileSaveAs_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = DialogFactory.ConnectionsSaveAsDialog())
            {
                if (saveFileDialog.ShowDialog(FrmMain.Default) != DialogResult.OK)
                    return;

                string newFileName = saveFileDialog.FileName;

                var connectionTreeModel = Runtime.ConnectionsService.ConnectionTreeModel;
                if (connectionTreeModel == null)
                    return;

                Runtime.ConnectionsService.SaveConnections(connectionTreeModel, false, new SaveFilter(), newFileName);

                if (newFileName == ConnectionsService.GetDefaultStartupConnectionFileName())
                {
                    Properties.OptionsBackupPage.Default.LoadConsFromCustomLocation = false;
                }
                else
                {
                    Properties.OptionsBackupPage.Default.LoadConsFromCustomLocation = true;
                    Properties.OptionsBackupPage.Default.BackupLocation = newFileName;
                }
            }
        }

        private void mMenFileExit_Click(object sender, EventArgs e)
        {
            Shutdown.Quit();
        }

        private void mMenToolsOptions_Click(object sender, EventArgs e)
        {
            AppWindows.Show(WindowType.Options);
        }

        #endregion
    }
}