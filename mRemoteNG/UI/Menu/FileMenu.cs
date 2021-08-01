using System;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;
using mRemoteNG.Security;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;

namespace mRemoteNG.UI.Menu
{
    public class FileMenu : ToolStripMenuItem
    {
        private ToolStripMenuItem _mMenFileNew;
        private ToolStripMenuItem _mMenFileLoad;
        private ToolStripMenuItem _mMenFileSave;
        private ToolStripMenuItem _mMenFileSaveAs;
        private ToolStripMenuItem _mMenFileExit;
        private ToolStripSeparator _mMenFileSep2;
        private ToolStripSeparator _mMenFileSep1;
        private ToolStripMenuItem _mMenReconnectAll;

        public ConnectionTreeWindow TreeWindow { get; set; }
        public IConnectionInitiator ConnectionInitiator { get; set; }

        public FileMenu()
        {
            Initialize();
        }

        private void Initialize()
        {
            _mMenFileNew = new ToolStripMenuItem();
            _mMenFileLoad = new ToolStripMenuItem();
            _mMenFileSave = new ToolStripMenuItem();
            _mMenFileSaveAs = new ToolStripMenuItem();
            _mMenFileSep2 = new ToolStripSeparator();
            _mMenReconnectAll = new ToolStripMenuItem();
            _mMenFileSep1 = new ToolStripSeparator();
            _mMenFileExit = new ToolStripMenuItem();

            // 
            // mMenFile
            // 
            DropDownItems.AddRange(new ToolStripItem[]
            {
                _mMenFileNew,
                _mMenFileLoad,
                _mMenFileSave,
                _mMenFileSaveAs,
                _mMenFileSep2,
                _mMenReconnectAll,
                _mMenFileSep1,
                _mMenFileExit
            });
            Name = "mMenFile";
            Size = new System.Drawing.Size(37, 20);
            Text = Language._File;
            // 
            // mMenFileNew
            // 
            _mMenFileNew.Image = Properties.Resources.Connections_New;
            _mMenFileNew.Name = "mMenFileNew";
            _mMenFileNew.Size = new System.Drawing.Size(281, 22);
            _mMenFileNew.Text = Language.NewConnectionFile;
            _mMenFileNew.Click += mMenFileNew_Click;
            // 
            // mMenFileLoad
            // 
            _mMenFileLoad.Image = Properties.Resources.Connections_Load;
            _mMenFileLoad.Name = "mMenFileLoad";
            _mMenFileLoad.ShortcutKeys = Keys.Control | Keys.O;
            _mMenFileLoad.Size = new System.Drawing.Size(281, 22);
            _mMenFileLoad.Text = Language.OpenConnectionFile;
            _mMenFileLoad.Click += mMenFileLoad_Click;
            // 
            // mMenFileSave
            // 
            _mMenFileSave.Image = Properties.Resources.Connections_Save;
            _mMenFileSave.Name = "mMenFileSave";
            _mMenFileSave.ShortcutKeys = Keys.Control | Keys.S;
            _mMenFileSave.Size = new System.Drawing.Size(281, 22);
            _mMenFileSave.Text = Language.SaveConnectionFile;
            _mMenFileSave.Click += mMenFileSave_Click;
            // 
            // mMenFileSaveAs
            // 
            _mMenFileSaveAs.Image = Properties.Resources.Connections_SaveAs;
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
            // mMenReconnectAll
            // 
            _mMenReconnectAll.Image = Properties.Resources.Refresh;
            _mMenReconnectAll.Name = "mMenReconnectAll";
            _mMenReconnectAll.Size = new System.Drawing.Size(281, 22);
            _mMenReconnectAll.Text = Language.ReconnectAllConnections;
            _mMenReconnectAll.Click += mMenReconnectAll_Click;
            // 
            // mMenFileSep1
            // 
            _mMenFileSep1.Name = "mMenFileSep3";
            _mMenFileSep1.Size = new System.Drawing.Size(278, 6);
            // 
            // mMenFileExit
            // 
            _mMenFileExit.Image = Properties.Resources.Quit;
            _mMenFileExit.Name = "mMenFileExit";
            _mMenFileExit.ShortcutKeys = Keys.Alt | Keys.F4;
            _mMenFileExit.Size = new System.Drawing.Size(281, 22);
            _mMenFileExit.Text = Language.Exit;
            _mMenFileExit.Click += mMenFileExit_Click;
        }

        public void ApplyLanguage()
        {
            Text = Language._File;
            _mMenFileNew.Text = Language.NewConnectionFile;
            _mMenFileLoad.Text = Language.OpenConnectionFile;
            _mMenFileSave.Text = Language.SaveConnectionFile;
            _mMenFileSaveAs.Text = Language.SaveConnectionFileAs;
            _mMenFileExit.Text = Language.Exit;
        }

        #region File

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

        private void mMenFileExit_Click(object sender, EventArgs e)
        {
            Shutdown.Quit();
        }

        #endregion
    }
}