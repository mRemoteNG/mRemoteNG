using mRemoteNG.Config;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using mRemoteNG.App;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.ACL;
using mRemoteNG.Security;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class BackupPage
    {
        private readonly FrmMain _frmMain = FrmMain.Default;
        private List<DropdownList> _permissionsListing;
        
        public BackupPage()
        {
            InitializeComponent();
            Check4ACL();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.DocumentsFolder_16x);
        }

        public override string PageName
        {
            get => Language.strBackup;
            set { }
        }

        public void Check4ACL()
        {
            if (Properties.OptionsRbac.Default.ActiveRole == "AdminRole")
            {
                lblACL.Visible = true;
                lblBacupPageShowInOptionsMenu.Visible = true;
                pnlShowForUser.Visible = true;

                plBackupEnable.Visible = true;
                plBackupType.Visible = true;
                plBackupFrequency.Visible = true;
                plBackupNumber.Visible = true;
                plBackupNameFormat.Visible = true;
                plBackupLocation.Visible = true;

                cbBackupEnableACL.Visible = true;
                cbBackupTypeACL.Visible = true;
                cbBackupFrequencyACL.Visible = true;
                cbBackupNumberACL.Visible = true;
                cbBackupNameFormatACL.Visible = true;
                cbBackupLocationACL.Visible = true;

                lblBackupEnable.Visible = true;
                lblBackupEnable.Enabled = true;
                pnlBackupEnable.Visible = true;
                pnlBackupEnable.Enabled = true;
                lblBackupType.Visible = true;
                lblBackupType.Enabled = true;
                pnlBackupType.Visible = true;
                pnlBackupType.Enabled = true;
                plMakeBackup.Visible = true;
                plMakeBackup.Enabled = true;
                numMaxBackups.Visible = true;
                numMaxBackups.Enabled = true;
                txtBackupNameFormat.Visible = true;
                txtBackupNameFormat.Enabled = true;
                txtConnectionsBackupPath.Visible = true;
                txtConnectionsBackupPath.Enabled = true;
                btnBrowsePath.Visible = true;
                btnBrowsePath.Enabled = true;
                lblMakeBackup.Visible = true;
                lblMakeBackup.Enabled = true;
                lblConnectionsBackupMaxCount.Visible = true;
                lblConnectionsBackupMaxCount.Enabled = true;
                lblBackupNameFormat.Visible = true;
                lblBackupNameFormat.Enabled = true;
                lblConnectionsBackupPath.Visible = true;
                lblConnectionsBackupPath.Enabled = true;
            } else
            {
                lblACL.Visible = false;
                lblBacupPageShowInOptionsMenu.Visible = false;
                pnlShowForUser.Visible = false;

                cbBackupEnableACL.Visible = false;
                cbBackupTypeACL.Visible = false;
                cbBackupFrequencyACL.Visible = false;
                cbBackupNumberACL.Visible = false;
                cbBackupNameFormatACL.Visible = false;
                cbBackupLocationACL.Visible = false;

                lblBackupEnable.Visible = Properties.OptionsBackupPage.Default.cbBackupEnableACL == 0 ? false : true;
                lblBackupEnable.Enabled = Properties.OptionsBackupPage.Default.cbBackupEnableACL == 1 ? false : true;
                pnlBackupEnable.Visible = Properties.OptionsBackupPage.Default.cbBackupEnableACL == 0 ? false : true;
                pnlBackupEnable.Enabled = Properties.OptionsBackupPage.Default.cbBackupEnableACL == 1 ? false : true;

                lblBackupType.Visible = Properties.OptionsBackupPage.Default.cbBackupTypeACL == 0 ? false : true;
                lblBackupType.Enabled = Properties.OptionsBackupPage.Default.cbBackupTypeACL == 1 ? false : true;
                pnlBackupType.Visible = Properties.OptionsBackupPage.Default.cbBackupTypeACL == 0 ? false : true;
                pnlBackupType.Enabled = Properties.OptionsBackupPage.Default.cbBackupTypeACL == 1 ? false : true;

                lblMakeBackup.Visible = Properties.OptionsBackupPage.Default.cbBackupFrequencyACL == 0 ? false : true;
                lblMakeBackup.Enabled = Properties.OptionsBackupPage.Default.cbBackupFrequencyACL == 1 ? false : true;
                plMakeBackup.Visible = Properties.OptionsBackupPage.Default.cbBackupFrequencyACL == 0 ? false : true;
                plMakeBackup.Enabled = Properties.OptionsBackupPage.Default.cbBackupFrequencyACL == 1 ? false : true;

                lblConnectionsBackupMaxCount.Visible = Properties.OptionsBackupPage.Default.cbBackupNumberACL == 0 ? false : true;
                lblConnectionsBackupMaxCount.Enabled = Properties.OptionsBackupPage.Default.cbBackupNumberACL == 1 ? false : true;
                numMaxBackups.Visible = Properties.OptionsBackupPage.Default.cbBackupNumberACL == 0 ? false : true;
                numMaxBackups.Enabled = Properties.OptionsBackupPage.Default.cbBackupNumberACL == 1 ? false : true;

                lblBackupNameFormat.Visible = Properties.OptionsBackupPage.Default.cbBackupNameFormatACL == 0 ? false : true;
                lblBackupNameFormat.Enabled = Properties.OptionsBackupPage.Default.cbBackupNameFormatACL == 1 ? false : true;
                txtBackupNameFormat.Visible = Properties.OptionsBackupPage.Default.cbBackupNameFormatACL == 0 ? false : true;
                txtBackupNameFormat.Enabled = Properties.OptionsBackupPage.Default.cbBackupNameFormatACL == 1 ? false : true;

                lblConnectionsBackupPath.Visible = Properties.OptionsBackupPage.Default.cbBackupLocationACL == 0 ? false : true;
                lblConnectionsBackupPath.Enabled = Properties.OptionsBackupPage.Default.cbBackupLocationACL == 1 ? false : true;
                txtConnectionsBackupPath.Visible = Properties.OptionsBackupPage.Default.cbBackupLocationACL == 0 ? false : true;
                txtConnectionsBackupPath.Enabled = Properties.OptionsBackupPage.Default.cbBackupLocationACL == 1 ? false : true;
                btnBrowsePath.Visible = Properties.OptionsBackupPage.Default.cbBackupLocationACL == 0 ? false : true;
                btnBrowsePath.Enabled = Properties.OptionsBackupPage.Default.cbBackupLocationACL == 1 ? false : true;
            }
        }
        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            _permissionsListing = new List<DropdownList>
            {
                {new DropdownList((int) ACLPermissions.Hidden, Language.ACLPermissionsHidden)},
                {new DropdownList((int) ACLPermissions.ReadOnly, Language.ACLPermissionsReadOnly)},
                {new DropdownList((int) ACLPermissions.WriteAllow, Language.ACLPermissionsWriteAllow)},
            };

            btnBrowsePath.Text = Language.strBrowse;

            lblBacupPageShowInOptionsMenu.Text = Language.PageСontrolInOptionsMenu;
            cbBacupPageInOptionMenu.Text = Language.ShowForUser;

            cbBackupEnableACL.BindingContext = new BindingContext();
            cbBackupEnableACL.DataSource = _permissionsListing;
            cbBackupEnableACL.DisplayMember = "DisplayString";
            cbBackupEnableACL.ValueMember = "Index";

            cbBackupTypeACL.BindingContext = new BindingContext();
            cbBackupTypeACL.DataSource = _permissionsListing;
            cbBackupTypeACL.DisplayMember = "DisplayString";
            cbBackupTypeACL.ValueMember = "Index";

            cbBackupFrequencyACL.BindingContext = new BindingContext();
            cbBackupFrequencyACL.DataSource = _permissionsListing;
            cbBackupFrequencyACL.DisplayMember = "DisplayString";
            cbBackupFrequencyACL.ValueMember = "Index";

            cbBackupNumberACL.BindingContext = new BindingContext();
            cbBackupNumberACL.DataSource = _permissionsListing;
            cbBackupNumberACL.DisplayMember = "DisplayString";
            cbBackupNumberACL.ValueMember = "Index";

            cbBackupNameFormatACL.BindingContext = new BindingContext();
            cbBackupNameFormatACL.DataSource = _permissionsListing;
            cbBackupNameFormatACL.DisplayMember = "DisplayString";
            cbBackupNameFormatACL.ValueMember = "Index";

            cbBackupLocationACL.BindingContext = new BindingContext();
            cbBackupLocationACL.DataSource = _permissionsListing;
            cbBackupLocationACL.DisplayMember = "DisplayString";
            cbBackupLocationACL.ValueMember = "Index";

            lblMakeBackup.Text = Language.strConnectionBackupFrequency;
            lblConnectionsBackupMaxCount.Text = Language.strConnectionsBackupMaxCount;
            lblConnectionsBackupPath.Text = Language.strConnectionsBackupPath;
        }

        public override void LoadSettings()
        {
            numMaxBackups.Value = Convert.ToDecimal(Properties.OptionsBackupPage.Default.BackupFileKeepCount);

            cbBacupPageInOptionMenu.Checked = Properties.OptionsBackupPage.Default.cbBacupPageInOptionMenu;
            cbBackupEnableACL.SelectedValue = Properties.OptionsBackupPage.Default.cbBackupEnableACL;
            cbBackupTypeACL.SelectedValue = Properties.OptionsBackupPage.Default.cbBackupTypeACL;
            cbBackupFrequencyACL.SelectedValue = Properties.OptionsBackupPage.Default.cbBackupFrequencyACL;
            cbBackupNumberACL.SelectedValue = Properties.OptionsBackupPage.Default.cbBackupNumberACL;
            cbBackupNameFormatACL.SelectedValue = Properties.OptionsBackupPage.Default.cbBackupNameFormatACL;
            cbBackupLocationACL.SelectedValue = Properties.OptionsBackupPage.Default.cbBackupLocationACL;
            txtBackupNameFormat.Text = Properties.OptionsBackupPage.Default.BackupFileNameFormat;

            cbMakeBackupOnExit.Checked = Properties.OptionsBackupPage.Default.BackupConnectionsOnExit;
            cbMakeBackupOnEdit.Checked = Properties.OptionsBackupPage.Default.BackupConnectionsOnEdit;
            cbMakeBackupOnSave.Checked = Properties.OptionsBackupPage.Default.BackupConnectionsOnSave;

            numMaxBackups.Value = Properties.OptionsBackupPage.Default.BackupFileKeepCount;
            if (numMaxBackups.Value == 0)
                rbBackupEnableDisable.Checked = true;
            txtConnectionsBackupPath.Text = Properties.OptionsBackupPage.Default.BackupLocation;

        }

        public override void SaveSettings()
        {
            Properties.OptionsBackupPage.Default.BackupFileKeepCount = (int) numMaxBackups.Value;
            /*
            if (Properties.OptionsBackupPage.Default.AutoSaveEveryMinutes > 0)
            {
                _frmMain.tmrAutoSave.Interval = Properties.OptionsBackupPage.Default.AutoSaveEveryMinutes * 60000;
                _frmMain.tmrAutoSave.Enabled = true;
            }
            else
            {
                _frmMain.tmrAutoSave.Enabled = false;
            }
            */
            Properties.OptionsBackupPage.Default.cbBackupEnableACL = (int) cbBackupEnableACL.SelectedValue;
            Properties.OptionsBackupPage.Default.cbBackupTypeACL = (int) cbBackupTypeACL.SelectedValue;
            Properties.OptionsBackupPage.Default.cbBackupFrequencyACL = (int) cbBackupFrequencyACL.SelectedValue;
            Properties.OptionsBackupPage.Default.cbBackupNumberACL = (int) cbBackupNumberACL.SelectedValue;
            Properties.OptionsBackupPage.Default.cbBackupNameFormatACL = (int) cbBackupNameFormatACL.SelectedValue;
            Properties.OptionsBackupPage.Default.cbBackupLocationACL = (int) cbBackupLocationACL.SelectedValue;

            Properties.OptionsBackupPage.Default.BackupFileNameFormat = (string) txtBackupNameFormat.Text;

            Properties.OptionsBackupPage.Default.BackupConnectionsOnExit = (bool) cbMakeBackupOnExit.Checked;
            Properties.OptionsBackupPage.Default.BackupConnectionsOnEdit = (bool) cbMakeBackupOnEdit.Checked;
            Properties.OptionsBackupPage.Default.BackupConnectionsOnSave = (bool) cbMakeBackupOnSave.Checked;

            Properties.OptionsBackupPage.Default.BackupFileKeepCount = (int) numMaxBackups.Value;

            Properties.OptionsBackupPage.Default.BackupLocation = (string) txtConnectionsBackupPath.Text;
            
            Properties.OptionsBackupPage.Default.cbBacupPageInOptionMenu = cbBacupPageInOptionMenu.Checked;

            //Save settings to persist changes between application sessions
            Properties.OptionsBackupPage.Default.Save();
        }

        private void ButtonBrowsePath_Click(object sender, EventArgs e)
        {
            var selectFolderDialog = DialogFactory.SelectFolder(Language.lblConnectionsBackupPath);
            txtConnectionsBackupPath.Text = selectFolderDialog.ShowDialog() == CommonFileDialogResult.Ok ? selectFolderDialog.FileName : txtConnectionsBackupPath.Text;
        }

        private void rbBackupEnableDisable_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBackupEnableDisable.Checked)
            {
                lblBackupType.Enabled = false;
                pnlBackupType.Enabled = false;
                lblMakeBackup.Enabled = false;
                plMakeBackup.Enabled = false;
                lblConnectionsBackupMaxCount.Enabled = false;
                numMaxBackups.Enabled = false;
                numMaxBackups.Value = 0;
                lblBackupNameFormat.Enabled = false;
                txtBackupNameFormat.Enabled = false;
                lblConnectionsBackupPath.Enabled = false;
                txtConnectionsBackupPath.Enabled = false;
                btnBrowsePath.Enabled = false;
                cbBackupTypeACL.Enabled = false;
                cbBackupFrequencyACL.Enabled = false;
                cbBackupNumberACL.Enabled = false;
                cbBackupNameFormatACL.Enabled = false;
                cbBackupLocationACL.Enabled = false;
            }
            else
            {
                lblBackupType.Enabled = true;
                pnlBackupType.Enabled = true;
                lblMakeBackup.Enabled = true;
                plMakeBackup.Enabled = true;
                lblConnectionsBackupMaxCount.Enabled = true;
                numMaxBackups.Enabled = true;
                numMaxBackups.Value = 10;
                lblBackupNameFormat.Enabled = true;
                txtBackupNameFormat.Enabled = true;
                lblConnectionsBackupPath.Enabled = true;
                txtConnectionsBackupPath.Enabled = true;
                btnBrowsePath.Enabled = true;
                cbBackupTypeACL.Enabled = true;
                cbBackupFrequencyACL.Enabled = true;
                cbBackupNumberACL.Enabled = true;
                cbBackupNameFormatACL.Enabled = true;
                cbBackupLocationACL.Enabled = true;
            }
        }
    }
}