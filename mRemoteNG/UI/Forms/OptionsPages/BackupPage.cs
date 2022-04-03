using mRemoteNG.Config;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using mRemoteNG.App;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.ACL;
using mRemoteNG.Security;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public sealed partial class BackupPage
    {
        private readonly FrmMain _frmMain = FrmMain.Default;
        private List<DropdownList> _permissionsListing;
        private List<DropdownList> _connectionBackup;

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
            if (Properties.rbac.Default.ActiveRole == "AdminRole")
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
                cbConnectionBackupFrequency.Visible = true;
                cbConnectionBackupFrequency.Enabled = true;
                numMaxBackups.Visible = true;
                numMaxBackups.Enabled = true;
                txtBackupNameFormat.Visible = true;
                txtBackupNameFormat.Enabled = true;
                txtConnectionsBackupPath.Visible = true;
                txtConnectionsBackupPath.Enabled = true;
                btnBrowsePath.Visible = true;
                btnBrowsePath.Enabled = true;
                lblConnectionsBackupFrequency.Visible = true;
                lblConnectionsBackupFrequency.Enabled = true;
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

                lblConnectionsBackupFrequency.Visible = Properties.OptionsBackupPage.Default.cbBackupFrequencyACL == 0 ? false : true;
                lblConnectionsBackupFrequency.Enabled = Properties.OptionsBackupPage.Default.cbBackupFrequencyACL == 1 ? false : true;
                cbConnectionBackupFrequency.Visible = Properties.OptionsBackupPage.Default.cbBackupFrequencyACL == 0 ? false : true;
                cbConnectionBackupFrequency.Enabled = Properties.OptionsBackupPage.Default.cbBackupFrequencyACL == 1 ? false : true;

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
                {
                    new DropdownList((int) ACLPermissions.Hidden, Language.ACLPermissionsHidden)
                },
                {
                    new DropdownList((int) ACLPermissions.ReadOnly, Language.ACLPermissionsReadOnly)
                },
                {
                    new DropdownList((int) ACLPermissions.WriteAllow, Language.ACLPermissionsWriteAllow)
                },

            };

            btnBrowsePath.Text = Language.strBrowse;

            _connectionBackup = new List<DropdownList>
            {
                {
                    new DropdownList((int) ConnectionsBackupFrequencyEnum.Never, Language.ConnectionsBackupFrequencyNever)
                },
                {
                    new DropdownList((int) ConnectionsBackupFrequencyEnum.OnEdit, Language.ConnectionsBackupFrequencyOnEdit)
                },
                {
                    new DropdownList((int) ConnectionsBackupFrequencyEnum.OnExit, Language.ConnectionsBackupFrequencyOnExit)
                },
                {
                    new DropdownList((int) ConnectionsBackupFrequencyEnum.Daily, Language.ConnectionsBackupFrequencyDaily)
                },
                {
                    new DropdownList((int) ConnectionsBackupFrequencyEnum.Weekly, Language.ConnectionsBackupFrequencyWeekly)
                },
                //{ new DropdownList( (int)ConnectionsBackupFrequencyEnum.Custom, Language.ConnectionsBackupFrequencyCustom)}
            };

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

            cbConnectionBackupFrequency.BindingContext = new BindingContext();
            cbConnectionBackupFrequency.DataSource = _connectionBackup;
            cbConnectionBackupFrequency.DisplayMember = "DisplayString";
            cbConnectionBackupFrequency.ValueMember = "Index";

            lblConnectionsBackupFrequency.Text = Language.strConnectionBackupFrequency;
            lblConnectionsBackupMaxCount.Text = Language.strConnectionsBackupMaxCount;
            lblConnectionsBackupPath.Text = Language.strConnectionsBackupPath;
        }

        public override void LoadSettings()
        {
            numMaxBackups.Value = Convert.ToDecimal(Properties.OptionsBackupPage.Default.BackupFileKeepCount);

            if (Properties.OptionsBackupPage.Default.SaveConnectionsFrequency == (int) ConnectionsBackupFrequencyEnum.Unassigned)
            {
                if (Properties.OptionsBackupPage.Default.SaveConnectionsAfterEveryEdit)
                {
                    Properties.OptionsBackupPage.Default.SaveConnectionsFrequency = (int) ConnectionsBackupFrequencyEnum.OnEdit;
                }
                else if (Properties.OptionsBackupPage.Default.SaveConsOnExit)
                {
                    Properties.OptionsBackupPage.Default.SaveConnectionsFrequency = (int) ConnectionsBackupFrequencyEnum.OnExit;
                }
                else
                {
                    Properties.OptionsBackupPage.Default.SaveConnectionsFrequency = (int) ConnectionsBackupFrequencyEnum.Never;
                }
            }

            cbBacupPageInOptionMenu.Checked = Properties.OptionsBackupPage.Default.cbBacupPageInOptionMenu;
            cbBackupEnableACL.SelectedValue = Properties.OptionsBackupPage.Default.cbBackupEnableACL;
            cbBackupTypeACL.SelectedValue = Properties.OptionsBackupPage.Default.cbBackupTypeACL;
            cbBackupFrequencyACL.SelectedValue = Properties.OptionsBackupPage.Default.cbBackupFrequencyACL;
            cbBackupNumberACL.SelectedValue = Properties.OptionsBackupPage.Default.cbBackupNumberACL;
            cbBackupNameFormatACL.SelectedValue = Properties.OptionsBackupPage.Default.cbBackupNameFormatACL;
            cbBackupLocationACL.SelectedValue = Properties.OptionsBackupPage.Default.cbBackupLocationACL;
            txtBackupNameFormat.Text = Properties.OptionsBackupPage.Default.BackupFileNameFormat;

            cbConnectionBackupFrequency.SelectedValue = Properties.OptionsBackupPage.Default.SaveConnectionsFrequency;
            txtConnectionsBackupPath.Text = Properties.OptionsBackupPage.Default.BackupLocation;

        }

        public override void SaveSettings()
        {
            Properties.OptionsBackupPage.Default.BackupFileKeepCount = (int) numMaxBackups.Value;

            if (Properties.OptionsBackupPage.Default.AutoSaveEveryMinutes > 0)
            {
                _frmMain.tmrAutoSave.Interval = Properties.OptionsBackupPage.Default.AutoSaveEveryMinutes * 60000;
                _frmMain.tmrAutoSave.Enabled = true;
            }
            else
            {
                _frmMain.tmrAutoSave.Enabled = false;
            }

            Properties.OptionsBackupPage.Default.cbBackupEnableACL = (int) cbBackupEnableACL.SelectedValue;
            Properties.OptionsBackupPage.Default.cbBackupTypeACL = (int) cbBackupTypeACL.SelectedValue;
            Properties.OptionsBackupPage.Default.cbBackupFrequencyACL = (int) cbBackupFrequencyACL.SelectedValue;
            Properties.OptionsBackupPage.Default.cbBackupNumberACL = (int) cbBackupNumberACL.SelectedValue;
            Properties.OptionsBackupPage.Default.cbBackupNameFormatACL = (int) cbBackupNameFormatACL.SelectedValue;
            Properties.OptionsBackupPage.Default.cbBackupLocationACL = (int) cbBackupLocationACL.SelectedValue;

            Properties.OptionsBackupPage.Default.BackupFileNameFormat = (string) txtBackupNameFormat.Text;

            Properties.OptionsBackupPage.Default.SaveConnectionsFrequency = (int) cbConnectionBackupFrequency.SelectedValue;

            if (txtConnectionsBackupPath.Text.Trim().Length <= 0)
            {
                Properties.OptionsBackupPage.Default.LoadConsFromCustomLocation = false;
                Properties.OptionsBackupPage.Default.BackupLocation = String.Empty;
            }
            else
            {
                var newFileName = txtConnectionsBackupPath.Text;

                Runtime.ConnectionsService.SaveConnections(Runtime.ConnectionsService.ConnectionTreeModel, false,
                    new SaveFilter(), newFileName);

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

            Properties.OptionsBackupPage.Default.cbBacupPageInOptionMenu = cbBacupPageInOptionMenu.Checked;

            //Obsolete. Set to false
            Properties.OptionsBackupPage.Default.SaveConnectionsAfterEveryEdit = false;
            Properties.OptionsBackupPage.Default.SaveConsOnExit = false;

            //Save settings to persist changes between application sessions
            Properties.OptionsBackupPage.Default.Save();
        }

        private void ButtonBrowsePath_Click(object sender, EventArgs e)
        {
            var selectFolderDialog = DialogFactory.SelectFolder(Language.lblConnectionsBackupPath);
            txtConnectionsBackupPath.Text = selectFolderDialog.ShowDialog() == CommonFileDialogResult.Ok ? selectFolderDialog.FileName : txtConnectionsBackupPath.Text;
        }
    }
}