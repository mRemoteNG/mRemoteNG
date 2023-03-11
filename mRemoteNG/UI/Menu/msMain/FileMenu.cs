using System;
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
        private ToolStripMenuItem _mMenToolsOptions;
        private ToolStripMenuItem _mMenFileNew;
        private ToolStripMenuItem _mMenFileLoad;
        private ToolStripMenuItem _mMenFileSave;
        private ToolStripMenuItem _mMenFileSaveAs;
        private ToolStripMenuItem _mMenFileExit;
        private ToolStripSeparator _mMenFileSep2;
        private ToolStripSeparator _mMenFileSep1;

        public ConnectionTreeWindow TreeWindow { get; set; }

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
            _mMenFileSep1 = new ToolStripSeparator();
            _mMenFileExit = new ToolStripMenuItem();
            _mMenToolsOptions = new ToolStripMenuItem();

            // 
            // mMenFile
            // 
            DropDownItems.AddRange(new ToolStripItem[]
            {
                _mMenFileNew,
                _mMenFileLoad,
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
            _mMenToolsOptions.Text = Language.Options;
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
            _mMenFileNew.Text = Language.NewConnectionFile;
            _mMenFileLoad.Text = Language.OpenConnectionFile;
            _mMenFileSave.Text = Language.SaveConnectionFile;
            _mMenFileSaveAs.Text = Language.SaveConnectionFileAs;
            _mMenToolsOptions.Text = Language.Options;
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

                Runtime.ConnectionsService.SaveConnections(Runtime.ConnectionsService.ConnectionTreeModel, false, new SaveFilter(), newFileName);

                if (newFileName == Runtime.ConnectionsService.GetDefaultStartupConnectionFileName())
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
            Windows.Show(WindowType.Options);
        }

        #endregion
    }
}