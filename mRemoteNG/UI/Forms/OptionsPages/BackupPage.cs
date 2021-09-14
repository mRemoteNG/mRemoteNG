using mRemoteNG.Config;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Config.Connections;
using mRemoteNG.Security;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public sealed partial class BackupPage
    {
        private readonly FrmMain _frmMain = FrmMain.Default;
        private List<DropdownList> _connectionBackup;

        public BackupPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Properties.Resources.Backup_Icon;
        }

        public override string PageName
        {
            get => Language.strBackup;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            buttonBrowsePath.Text = Language.strBrowse;

            _connectionBackup = new List<DropdownList>
            {
                {
                    new DropdownList((int) ConnectionsBackupFrequencyEnum.Never,
                        Language.ConnectionsBackupFrequencyNever)
                },
                {
                    new DropdownList((int) ConnectionsBackupFrequencyEnum.OnEdit,
                        Language.ConnectionsBackupFrequencyOnEdit)
                },
                {
                    new DropdownList((int) ConnectionsBackupFrequencyEnum.OnExit,
                        Language.ConnectionsBackupFrequencyOnExit)
                },
                {
                    new DropdownList((int) ConnectionsBackupFrequencyEnum.Daily,
                        Language.ConnectionsBackupFrequencyDaily)
                },
                {
                    new DropdownList((int) ConnectionsBackupFrequencyEnum.Weekly,
                        Language.ConnectionsBackupFrequencyWeekly)
                },
                //{ new DropdownList( (int)ConnectionsBackupFrequencyEnum.Custom, Language.ConnectionsBackupFrequencyCustom)}
            };

            cmbConnectionBackupFrequency.DataSource = _connectionBackup;
            cmbConnectionBackupFrequency.DisplayMember = "DisplayString";
            cmbConnectionBackupFrequency.ValueMember = "Index";

            lblConnectionsBackupFrequency.Text = Language.strConnectionBackupFrequency;
            lblConnectionsBackupMaxCount.Text = Language.strConnectionsBackupMaxCount;
            lblConnectionsBackupPath.Text = Language.strConnectionsBackupPath;
        }

        public override void LoadSettings()
        {
            numMaxBackups.Value = Convert.ToDecimal(Settings.Default.BackupFileKeepCount);

            if (Settings.Default.SaveConnectionsFrequency == (int) ConnectionsBackupFrequencyEnum.Unassigned)
            {
                if (Settings.Default.SaveConnectionsAfterEveryEdit)
                {
                    Settings.Default.SaveConnectionsFrequency = (int) ConnectionsBackupFrequencyEnum.OnEdit;
                }
                else if (Settings.Default.SaveConsOnExit)
                {
                    Settings.Default.SaveConnectionsFrequency = (int) ConnectionsBackupFrequencyEnum.OnExit;
                }
                else
                {
                    Settings.Default.SaveConnectionsFrequency = (int) ConnectionsBackupFrequencyEnum.Never;
                }
            }

            cmbConnectionBackupFrequency.SelectedValue = Settings.Default.SaveConnectionsFrequency;
            textBoxConnectionBackupPath.Text = Settings.Default.CustomConsPath;

        }

        public override void SaveSettings()
        {
            Settings.Default.BackupFileKeepCount = (int) numMaxBackups.Value;

            if (Settings.Default.AutoSaveEveryMinutes > 0)
            {
                _frmMain.tmrAutoSave.Interval = Settings.Default.AutoSaveEveryMinutes * 60000;
                _frmMain.tmrAutoSave.Enabled = true;
            }
            else
            {
                _frmMain.tmrAutoSave.Enabled = false;
            }

            Settings.Default.SaveConnectionsFrequency = (int) cmbConnectionBackupFrequency.SelectedValue;

            if (textBoxConnectionBackupPath.Text.Trim().Length <= 0)
            {
                Settings.Default.LoadConsFromCustomLocation = false;
                Settings.Default.CustomConsPath = String.Empty;
            }
            else
            {
                var newFileName = textBoxConnectionBackupPath.Text;

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

            //Obsolete. Set to false
            Settings.Default.SaveConnectionsAfterEveryEdit = false;
            Settings.Default.SaveConsOnExit = false;
        }

        private void ButtonBrowsePath_Click(object sender, EventArgs e)
        {
            var saveDialog = DialogFactory.ConnectionsSaveAsDialog();

            var dialogResult = saveDialog.ShowDialog(this);

            textBoxConnectionBackupPath.Text = dialogResult == DialogResult.OK ? saveDialog.FileName : string.Empty;
        }
    }
}